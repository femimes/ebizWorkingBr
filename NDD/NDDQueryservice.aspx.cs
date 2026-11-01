using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using NETClass;

public partial class NDD_NDDQueryservice : System.Web.UI.Page
{
    public string luser;
    public string StrLocation;
    public string struser;
    public string guser;

    protected void Page_Load(object sender, EventArgs e)
    {
        NDDJobScheduler.EnsureStarted();
        if (!IsPostBack)
        {
            NDDJobScheduler.Settings sched = NDDJobScheduler.Load();
            chkSchedule.Checked = sched.Enabled;
            string interval = sched.IntervalMinutes.ToString(CultureInfo.InvariantCulture);
            if (ddlInterval.Items.FindByValue(interval) != null) ddlInterval.SelectedValue = interval;
        }
    }

    // =====================================================================
    // Automatic run: switch on/off and choose how often
    // =====================================================================
    protected void btnSaveSchedule_Click(object sender, EventArgs e)
    {
        LoadCurrentUser();
        int minutes;
        if (!int.TryParse(ddlInterval.SelectedValue, out minutes)) minutes = 60;

        try
        {
            NDDJobScheduler.Settings before = NDDJobScheduler.Load();
            NDDJobScheduler.Settings after = NDDJobScheduler.Update(chkSchedule.Checked, minutes, guser);

            if (before.Enabled != after.Enabled || before.IntervalMinutes != after.IntervalMinutes)
                Audit("044", string.Format("NDDMandate automatic recovery run {0}; interval {1} minutes",
                    after.Enabled ? "switched on" : "switched off", after.IntervalMinutes));

            ShowSuccess(after.Enabled
                ? string.Format("Automatic run is on: every {0}. First run at {1:HH:mm}.", Every(after.IntervalMinutes), after.NextRun)
                : "Automatic run is off.");
        }
        catch (Exception ex)
        {
            ShowError("The schedule was not saved: " + ex.Message);
        }
    }

    private static string Every(int minutes)
    {
        return minutes % 60 == 0 && minutes >= 120 ? (minutes / 60) + " hours" : minutes + " minutes";
    }

    protected void SqlDataSource1_Filtering(object sender, SqlDataSourceFilteringEventArgs e)
    {
        // The applied filter is kept between postbacks
        SqlDataSource1.FilterExpression = Convert.ToString(ViewState["filter"] ?? "");
    }

    // =====================================================================
    // Filter
    // =====================================================================
    protected void btngo_Click(object sender, EventArgs e)
    {
        List<string> parts = new List<string>();
        AddLike(parts, "accountNumber", txtAccount.Text);
        AddLike(parts, "mandateCode", txtMandate.Text);
        AddLike(parts, "accountName", txtName.Text);
        AddLike(parts, "narration", txtNarration.Text);

        DateTime from, to;
        bool hasFrom = TryDate(txtFrom.Text, out from);
        bool hasTo = TryDate(txtTo.Text, out to);
        if (hasFrom && hasTo && to < from)
        {
            ShowError("Date to must be on or after date from.");
            return;
        }
        if (hasFrom) parts.Add(string.Format(CultureInfo.InvariantCulture, "systemdatetime >= #{0:MM/dd/yyyy}#", from));
        if (hasTo) parts.Add(string.Format(CultureInfo.InvariantCulture, "systemdatetime < #{0:MM/dd/yyyy}#", to.AddDays(1)));

        ViewState["filter"] = string.Join(" AND ", parts.ToArray());
        ASPxLabel1.Text = parts.Count == 0 ? "" : parts.Count + (parts.Count == 1 ? " filter applied" : " filters applied");
        ShowSuccess("");
        grid.PageIndex = 0;
        grid.DataBind();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtAccount.Text = txtMandate.Text = txtName.Text = txtNarration.Text = txtFrom.Text = txtTo.Text = "";
        ViewState["filter"] = "";
        ASPxLabel1.Text = "";
        ShowSuccess("");
        grid.PageIndex = 0;
        grid.DataBind();
    }

    private static void AddLike(List<string> parts, string column, string value)
    {
        value = (value ?? "").Trim();
        if (value == "") return;
        // Escape for a DataView filter: quotes doubled, wildcards bracketed
        StringBuilder sb = new StringBuilder();
        foreach (char c in value)
        {
            if (c == '\'') sb.Append("''");
            else if (c == '*' || c == '%' || c == '[' || c == ']') sb.Append('[').Append(c).Append(']');
            else sb.Append(c);
        }
        parts.Add(string.Format("Convert({0}, 'System.String') LIKE '%{1}%'", column, sb));
    }

    private static bool TryDate(string value, out DateTime date)
    {
        return DateTime.TryParseExact((value ?? "").Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
    }

    protected string PopupJs(int action, object mandateCode)
    {
        string url = string.Format("NDDUpdateMandStatus.aspx?q={0}&p={1}", action, HttpUtility.UrlEncode(Convert.ToString(mandateCode)));
        return HttpUtility.HtmlAttributeEncode("return openMandate('" + HttpUtility.JavaScriptStringEncode(url) + "');");
    }

    // =====================================================================
    // Status check (TSQ) and recovery: ticked mandates, or all of them
    // =====================================================================
    protected void btnCheck_Click(object sender, EventArgs e)
    {
        LoadCurrentUser();

        List<string> codes = new List<string>();
        foreach (GridViewRow row in grid.Rows)
        {
            if (row.RowType != DataControlRowType.DataRow) continue;
            CheckBox chk = row.FindControl("chkSelect") as CheckBox;
            if (chk != null && chk.Checked) codes.Add(Convert.ToString(grid.DataKeys[row.RowIndex]["mandateCode"]));
        }
        if (codes.Count == 0) { ShowError("Select at least one mandate first."); return; }

        ShowSummary(NDDMandateRecovery.RunSelected(codes, guser));
        grid.DataBind();
    }

    protected void btnRunAll_Click(object sender, EventArgs e)
    {
        LoadCurrentUser();
        Server.ScriptTimeout = 3600;
        ShowSummary(NDDMandateRecovery.RunAll(guser, JobMaxPerRun()));
        grid.DataBind();
    }

    private void ShowSummary(NDDMandateRecovery.RunSummary s)
    {
        if (s.Skipped) { ShowError("Not run: " + s.SkipReason + "."); return; }

        string text = string.Format("{0} checked, {1} sent for recovery.", s.Checked, s.Recovered);
        if (s.NotRecovered.Count > 0)
            text += string.Format(" {0} not recovered: {1}.", s.NotRecovered.Count, string.Join("; ", s.NotRecovered.ToArray()));
        litResult.Text = HttpUtility.HtmlEncode(text);
        Literal1.Text = s.Failed.Count == 0 ? "" : HttpUtility.HtmlEncode(s.Failed.Count + " could not be checked: " + string.Join("; ", s.Failed.ToArray()));
    }

    // =====================================================================
    // Unattended job status
    // =====================================================================
    protected void Page_PreRender(object sender, EventArgs e)
    {
        NDDJobScheduler.Settings sched = NDDJobScheduler.Load();
        litJobState.Text = sched.Enabled ? "On" : "Off";
        spanJobState.Attributes["class"] = sched.Enabled ? "pill on" : "pill off";
        litNextRun.Text = sched.Enabled
            ? HttpUtility.HtmlEncode(string.Format("Every {0}. Next run about {1:dd MMM yyyy HH:mm}. Switched on by {2}.",
                Every(sched.IntervalMinutes), sched.NextRun, sched.UpdatedBy == "" ? "an officer" : sched.UpdatedBy))
            : sched.UpdatedAt.HasValue
                ? HttpUtility.HtmlEncode(string.Format("Off since {0:dd MMM yyyy HH:mm} ({1}).", sched.UpdatedAt, sched.UpdatedBy))
                : "Off. Tick the box and Save to start.";

        NDDMandateRecovery.RunSummary last = NDDMandateRecovery.LastJobRun;
        litJobLast.Text = NDDMandateRecovery.IsRunning
            ? "A run is in progress now."
            : last == null
                ? "No run recorded since the site last started. Runs are also written to the NDDAppLog file."
                : HttpUtility.HtmlEncode(string.Format("Last run {0:dd MMM yyyy HH:mm} by {1} ({2:N0}s): {3}.",
                    last.Started, last.Trigger, (last.Finished - last.Started).TotalSeconds, last.Text()));
    }

    private static int JobMaxPerRun()
    {
        int max;
        return int.TryParse(ConfigurationManager.AppSettings["NDDJobMaxPerRun"], out max) && max > 0 ? max : 200;
    }

    // =====================================================================
    // Helpers
    // =====================================================================
    private void LoadCurrentUser()
    {
        HttpCookie cookie = Request.Cookies["LogonUserID"];
        luser = cookie != null && !string.IsNullOrEmpty(cookie.Value) ? cookie.Value : (User.Identity.Name ?? "");
        int split = luser.LastIndexOf('?');
        struser = split >= 0 ? luser.Substring(0, split) : luser;
        StrLocation = split >= 0 ? luser.Substring(split + 1) : "";
        guser = struser == "" ? "NDDService" : struser;
    }

    private void Audit(string code, string message)
    {
        Classes.RecordOp(string.Format("Sproc_AddAuditLog '{0}','{1}','{2}',''", code, Sql(message), Sql(guser)));
    }

    private void ShowSuccess(string message)
    {
        litResult.Text = HttpUtility.HtmlEncode(message);
        Literal1.Text = "";
    }

    private void ShowError(string message)
    {
        Literal1.Text = HttpUtility.HtmlEncode(message);
        litResult.Text = "";
    }

    private static string Cfg(string name)
    {
        ConnectionStringSettings s = ConfigurationManager.ConnectionStrings[name];
        return s == null ? "" : s.ConnectionString;
    }

    private static string Sql(string value)
    {
        return (value ?? "").Replace("'", "''");
    }
}