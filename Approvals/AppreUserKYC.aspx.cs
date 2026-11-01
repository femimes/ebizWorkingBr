using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using NETClass;

public partial class Approvals_AppreUserKYC : System.Web.UI.Page
{
    public string gUser;
    public string luser;
    public string StrLocation = "";
    public string struser = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadCurrentUser();
            txtUserID.Text = struser;
            txtlocation.Text = StrLocation;
            // The old grid grouped by this column; sorting keeps the groups together
            ctrlGrid.Sort("user_id", SortDirection.Ascending);
        }
    }

    protected void SqlDataSource1_Selected(object sender, SqlDataSourceStatusEventArgs e)
    {
        litTotal.Text = e.Exception == null && e.AffectedRows > 0 ? "&middot; " + e.AffectedRows.ToString("N0") + " waiting" : "";
    }

    // =====================================================================
    // Approve: status 3
    // =====================================================================
    protected void BtnApprove_Click(object sender, EventArgs e)
    {
        if (!LoadCurrentUser()) { ShowError("Your session has expired. Sign in again."); return; }
        int done = 0;
        List<string> failed = new List<string>();

        foreach (GridViewRow row in SelectedRows())
        {
            string label = Val(row, "user_id");
            try
            {
                string userId = Val(row, "user_id");
                ExecPositional("SpUpdAppUserKYCInfo", userId, Val(row, "remseq"), "3");
                Audit("910", string.Format("Title: User KYC Policy Approval;  User ID:{0}; Kyc desc:-{1}; Reason: {2}/{3}",
                    userId, Val(row, "keydesc"), "Approved By- " + gUser, Val(row, "keyvalues")));
                done++;
            }
            catch (Exception ex)
            {
                failed.Add(label + ": " + ex.Message);
            }
        }

        ctrlGrid.DataBind();
        Report(done, failed, "approved");
    }

    // Document links: set appSettings "KycDocBaseUrl" to move the store; defaults to the old address
    protected string DocUrl(object path)
    {
        string baseUrl = ConfigurationManager.AppSettings["KycDocBaseUrl"];
        if (string.IsNullOrEmpty(baseUrl)) baseUrl = "http://192.168.1.100/radiuspoints/";
        string p = Convert.ToString(path).TrimStart('/');
        return HttpUtility.HtmlAttributeEncode(baseUrl.TrimEnd('/') + "/" + p);
    }

    // =====================================================================
    // Disapprove: status 4, then remove the uploaded file
    // =====================================================================
    protected void btnDisApprove_Click(object sender, EventArgs e)
    {
        if (!LoadCurrentUser()) { ShowError("Your session has expired. Sign in again."); return; }
        int done = 0;
        List<string> failed = new List<string>();

        foreach (GridViewRow row in SelectedRows())
        {
            string label = Val(row, "user_id");
            try
            {
                string reason = Reason(row);
                if (reason == "") { failed.Add(label + ": reason missing"); continue; }
                string userId = Val(row, "user_id");
                string filePath = Val(row, "keyvalues");
                ExecPositional("SpUpdAppUserKYCInfo", userId, Val(row, "remseq"), "4");
                Audit("910", string.Format("Title: User KYC Policy Approval;  User ID:{0}; Kyc desc:-{1}; Reason: {2}/{3}/{4}",
                    userId, Val(row, "keydesc"), "Dis Approved By- " + gUser, filePath, reason));

                try
                {
                    if (filePath != "" && File.Exists(filePath)) File.Delete(filePath);
                }
                catch (IOException)
                {
                    // The request is already disapproved; a locked file is left for clean-up
                }
                catch (UnauthorizedAccessException)
                {
                }
                done++;
            }
            catch (Exception ex)
            {
                failed.Add(label + ": " + ex.Message);
            }
        }

        ctrlGrid.DataBind();
        Report(done, failed, "disapproved");
    }

    protected void btncancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Path, false);
        Context.ApplicationInstance.CompleteRequest();
    }

    // =====================================================================
    // Row selection helpers
    // =====================================================================
    private List<GridViewRow> SelectedRows()
    {
        List<GridViewRow> rows = new List<GridViewRow>();
        foreach (GridViewRow row in ctrlGrid.Rows)
        {
            if (row.RowType != DataControlRowType.DataRow) continue;
            CheckBox chk = row.FindControl("chkSelect") as CheckBox;
            if (chk != null && chk.Checked) rows.Add(row);
        }
        return rows;
    }

    private object Raw(GridViewRow row, string field)
    {
        return ctrlGrid.DataKeys[row.RowIndex][field];
    }

    private string Val(GridViewRow row, string field)
    {
        object v = Raw(row, field);
        return v == null || v == DBNull.Value ? "" : Convert.ToString(v).Trim();
    }

    private static string Reason(GridViewRow row)
    {
        TextBox box = row.FindControl("tatReason") as TextBox;
        return box == null ? "" : box.Text.Trim();
    }

    private void Report(int done, List<string> failed, string verb)
    {
        if (done == 0 && failed.Count == 0)
        {
            ShowError("Select at least one row first.");
            return;
        }
        string ok = done > 0 ? string.Format("{0} {1} {2}.", done, done == 1 ? "request" : "requests", verb) : "";
        if (failed.Count == 0)
        {
            ShowSuccess(ok);
        }
        else
        {
            litResult.Text = HttpUtility.HtmlEncode(ok);
            Literal1.Text = HttpUtility.HtmlEncode(string.Format("{0} not {1}: {2}", failed.Count, verb, string.Join("; ", failed.ToArray())));
        }
    }

    // =====================================================================
    // Common helpers
    // =====================================================================
    private bool LoadCurrentUser()
    {
        HttpCookie cookie = Request.Cookies["LogonUserID"];
        luser = cookie != null && !string.IsNullOrEmpty(cookie.Value) ? cookie.Value : (User.Identity.Name ?? "");
        int split = luser.LastIndexOf('?');
        struser = split >= 0 ? luser.Substring(0, split) : luser;
        StrLocation = split >= 0 ? luser.Substring(split + 1) : "";
        gUser = struser;
        return gUser != "";
    }

    private void Audit(string code, string message)
    {
        Classes.RecordOp(string.Format("Sproc_AddAuditLog '{0}','{1}','{2}',''", code, Sql(message), Sql(gUser)));
    }

    // Runs a procedure whose parameters are passed by position (names not known)
    private static void ExecPositional(string proc, params object[] values)
    {
        using (SqlConnection conn = new SqlConnection(Cfg("EazyConnect")))
        using (SqlCommand cmd = new SqlCommand())
        {
            List<string> names = new List<string>();
            for (int i = 0; i < values.Length; i++)
            {
                names.Add("@p" + i);
                cmd.Parameters.Add(new SqlParameter("@p" + i, values[i] ?? DBNull.Value));
            }
            cmd.Connection = conn;
            cmd.CommandText = "EXEC " + proc + " " + string.Join(",", names.ToArray());
            conn.Open();
            cmd.ExecuteNonQuery();
        }
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