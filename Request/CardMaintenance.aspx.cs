using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Maintain eRequest: batches from Sproc_GetCardForMaint, requests in a batch from SpCardRequestMaint;
/// ticked requests get new issue/expiry dates through Sproc_AddDeleteCardMaint. Plain ASP.NET controls (no DevExpress).
/// </summary>
public partial class Request_CardMaintenance : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DatCardIssue.Text = DateInput(DateTime.Today);
            DatCardExpiry.Text = DateInput(DateTime.Today);
            LoadBatches();
            BindGrid();
        }
    }

    private void LoadBatches()
    {
        drpBatchRefNo.Items.Clear();
        drpBatchRefNo.Items.Add(new ListItem("Choose a batch", ""));
        try
        {
            foreach (DataRow r in QueryOn(MainDb, "Sproc_GetCardForMaint", CommandType.StoredProcedure).Rows)
            {
                string refNo = Col(r, "RefNo");
                if (refNo == "" || drpBatchRefNo.Items.FindByValue(refNo) != null) continue;
                string title = Col(r, "RequestTitle");
                drpBatchRefNo.Items.Add(new ListItem(title == "" ? refNo : refNo + " - " + title, refNo));
            }
        }
        catch (Exception ex)
        {
            ShowError("The batches could not be loaded: " + ex.Message);
        }
    }

    private DataTable LoadRequests()
    {
        if (drpBatchRefNo.SelectedValue == "") return new DataTable();
        string branch = CurrentBranch(), user = CurrentUser();
        DataTable t = Query("EXEC SpCardRequestMaint @userId = @userId, @locationID = @locationID, @RefNo = @RefNo",
            P("@userId", user == "" ? "00" : user), P("@locationID", branch == "" ? "00" : branch), P("@RefNo", drpBatchRefNo.SelectedValue));
        foreach (string column in new[] { "SeqNum", "AccountNo", "AccountDesc", "CardTypeDesc", "CardIssueDate", "CardExpiryDate", "SystemDateTime" })
            if (!t.Columns.Contains(column)) t.Columns.Add(column, typeof(string));
        return t;
    }

    private void BindGrid()
    {
        try
        {
            DataTable t = LoadRequests();
            ctrlGrid.DataSource = t;
            ctrlGrid.DataBind();
            litTotal.Text = drpBatchRefNo.SelectedValue == "" ? "" : string.Format("{0:N0} request{1}", t.Rows.Count, t.Rows.Count == 1 ? "" : "s");
        }
        catch (Exception ex)
        {
            ctrlGrid.DataSource = null;
            ctrlGrid.DataBind();
            litTotal.Text = "";
            ShowError("The requests could not be loaded: " + ex.Message);
        }
    }

    protected void drpBatchRefNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ctrlGrid.PageIndex = 0;
        litResult.Text = "";
        litError.Text = "";
        BindGrid();
    }

    protected void ctrlGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ctrlGrid.PageIndex = e.NewPageIndex;
        BindGrid();
    }

    protected void btncancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Path, false);
        Context.ApplicationInstance.CompleteRequest();
    }

    protected void BtnApprove_Click(object sender, EventArgs e)
    {
        Page.Validate("submit");
        if (!Page.IsValid) return;
        if (drpBatchRefNo.SelectedValue == "") { ShowError("Choose a batch first."); return; }

        DateTime issue, expiry;
        if (!TryDate(DatCardIssue.Text, out issue) || !TryDate(DatCardExpiry.Text, out expiry)) { ShowError("Enter valid issue and expiry dates."); return; }

        List<GridViewRow> rows = new List<GridViewRow>();
        foreach (GridViewRow row in ctrlGrid.Rows)
        {
            CheckBox chk = row.FindControl("chkSelect") as CheckBox;
            if (row.RowType == DataControlRowType.DataRow && chk != null && chk.Checked) rows.Add(row);
        }
        if (rows.Count == 0) { ShowError("Tick at least one request first."); return; }

        string user = CurrentUser();
        int done = 0;
        List<string> failed = new List<string>();
        foreach (GridViewRow row in rows)
        {
            string seq = Convert.ToString(ctrlGrid.DataKeys[row.RowIndex].Values["SeqNum"]);
            string accountNo = Convert.ToString(ctrlGrid.DataKeys[row.RowIndex].Values["AccountNo"]);
            TextBox tb = row.FindControl("tatReason") as TextBox;
            string reason = tb == null ? "" : tb.Text.Trim();
            if (reason == "") { failed.Add(accountNo + ": reason missing"); continue; }
            try
            {
                string response = Exec("Sproc_AddDeleteCardMaint",
                    P("@eBatRefNo", drpBatchRefNo.SelectedValue),
                    P("@accountNo", accountNo),
                    P("@eIssueDate", issue),
                    P("@eExipryDate", expiry),
                    P("@reason", reason),
                    P("@UserID", user),
                    P("@SeqNum", seq),
                    Out("@ResponseDesc"));
                if (response == "Success") done++;
                else failed.Add(accountNo + ": " + (response == "" ? "not saved" : response));
            }
            catch (Exception ex)
            {
                failed.Add(accountNo + ": " + ex.Message);
            }
        }

        ShowSuccess(done == 0 ? "" : string.Format("Submitted {0} request{1} for maintenance.", done, done == 1 ? "" : "s"));
        if (failed.Count > 0)
            litError.Text = HttpUtility.HtmlEncode(string.Format("{0} request(s) not submitted: {1}", failed.Count, string.Join("; ", failed.ToArray())));
        BindGrid();
    }

    // =====================================================================
    // Shared Request-page helpers (same code in every rewritten C# Request page)
    // =====================================================================

    /// <summary>Signed-in identity "user?branch" (forms identity, else the LogonUserID cookie).</summary>
    private string Identity()
    {
        string id = User != null && User.Identity != null ? User.Identity.Name : "";
        if (string.IsNullOrEmpty(id) && Request.Cookies["LogonUserID"] != null) id = Request.Cookies["LogonUserID"].Value;
        return id ?? "";
    }

    private string CurrentUser()
    {
        string id = Identity();
        int split = id.LastIndexOf('?');
        return split >= 0 ? id.Substring(0, split) : id;
    }

    private string CurrentBranch()
    {
        string id = Identity();
        int split = id.LastIndexOf('?');
        return split >= 0 ? id.Substring(split + 1) : "";
    }

    private static string ConnectionString(string name)
    {
        ConnectionStringSettings s = ConfigurationManager.ConnectionStrings[name];
        return s == null ? "" : s.ConnectionString;
    }

    private static string MainDb { get { return ConnectionString("EazyConnect"); } }

    /// <summary>Runs a query with parameters (never string concatenation) and returns the first table.</summary>
    private static DataTable Query(string sql, params SqlParameter[] parameters)
    {
        return QueryOn(MainDb, sql, CommandType.Text, parameters);
    }

    private static DataTable QueryOn(string connection, string sql, CommandType type, params SqlParameter[] parameters)
    {
        DataTable table = new DataTable();
        using (SqlConnection conn = new SqlConnection(connection))
        using (SqlCommand cmd = new SqlCommand(sql, conn))
        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
        {
            cmd.CommandType = type;
            cmd.Parameters.AddRange(parameters);
            adapter.Fill(table);
        }
        return table;
    }

    /// <summary>Runs a stored procedure on the main database; returns the value of its first output parameter (or "").</summary>
    private static string Exec(string procedure, params SqlParameter[] parameters)
    {
        using (SqlConnection conn = new SqlConnection(MainDb))
        using (SqlCommand cmd = new SqlCommand(procedure, conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(parameters);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
        foreach (SqlParameter p in parameters)
        {
            if (p.Direction == ParameterDirection.Output || p.Direction == ParameterDirection.InputOutput)
                return p.Value == null || p.Value == DBNull.Value ? "" : Convert.ToString(p.Value);
        }
        return "";
    }

    private static SqlParameter P(string name, object value)
    {
        return new SqlParameter(name, value ?? DBNull.Value);
    }

    private static SqlParameter Out(string name)
    {
        SqlParameter p = new SqlParameter(name, SqlDbType.VarChar, 5000);
        p.Direction = ParameterDirection.Output;
        return p;
    }

    private static string Col(DataRow row, string column)
    {
        if (row == null || !row.Table.Columns.Contains(column) || row.IsNull(column)) return "";
        return Convert.ToString(row[column]);
    }

    /// <summary>yyyy-MM-dd for an HTML date box.</summary>
    protected static string DateInput(object value)
    {
        DateTime d;
        if (value is DateTime) return ((DateTime)value).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        if (value != null && value != DBNull.Value && DateTime.TryParse(Convert.ToString(value), out d) && d.Year > 1900)
            return d.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        return "";
    }

    private static bool TryDate(string text, out DateTime value)
    {
        return DateTime.TryParseExact((text ?? "").Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out value);
    }

    /// <summary>Splits "123, 456 789" into a clean list of account numbers.</summary>
    private static List<string> AccountList(string text)
    {
        List<string> result = new List<string>();
        foreach (string part in (text ?? "").Split(new[] { ',', ';', ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries))
        {
            string acct = part.Trim().Trim('\'');
            if (acct != "" && !result.Contains(acct)) result.Add(acct);
        }
        return result;
    }

    private void ShowSuccess(string message)
    {
        litResult.Text = HttpUtility.HtmlEncode(message);
        litError.Text = "";
    }

    private void ShowError(string message)
    {
        litError.Text = HttpUtility.HtmlEncode(message);
        litResult.Text = "";
    }
}
