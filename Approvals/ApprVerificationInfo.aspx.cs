using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;
using NETClass;

public partial class Approvals_ApprVerificationInfo : System.Web.UI.Page
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
            ctrlGrid.Sort("maintTypeDesc", SortDirection.Ascending);
        }
    }

    protected void SqlDataSource1_Selected(object sender, SqlDataSourceStatusEventArgs e)
    {
        litTotal.Text = e.Exception == null && e.AffectedRows > 0 ? "&middot; " + e.AffectedRows.ToString("N0") + " waiting" : "";
    }



    // =====================================================================
    // Disapprove: status 4 (approval happens on the Authorise page)
    // =====================================================================
    protected void btnDisApprove_Click(object sender, EventArgs e)
    {
        if (!LoadCurrentUser()) { ShowError("Your session has expired. Sign in again."); return; }
        int done = 0;
        List<string> failed = new List<string>();

        foreach (GridViewRow row in SelectedRows())
        {
            string label = Val(row, "eBatRefNo");
            try
            {
                string reason = Reason(row);
                if (reason == "") { failed.Add(label + ": reason missing"); continue; }
                using (SqlConnection conn = new SqlConnection(Cfg("EazyConnect")))
                using (SqlCommand cmd = new SqlCommand("Sproc_UpdateCardMaintenance", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@eBatRefNo", Val(row, "eBatRefNo")));
                    cmd.Parameters.Add(new SqlParameter("@status", 4));
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                Audit("910", string.Format("Title: Card Maintenance Approval;  BatchRef:{0}; Reason: {1}; DisApprovedBy: {2}",
                    Val(row, "eBatRefNo"), reason, "Dis Approved By- " + gUser));
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