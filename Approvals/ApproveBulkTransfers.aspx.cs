using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NETClass;

public partial class Approvals_ApproveBulkTransfers : System.Web.UI.Page
{
    public string gUser;
    public string luser = "";
    public string StrLocation = "";
    public string struser = "";
    public decimal l_totalAmt = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadCurrentUser();
            txtuserID.Text = gUser;
            LoadBatches();
        }
        pnlBatch.Visible = drpBatchRefNo.SelectedValue != "";
    }

    // Batch list shows "reference - title"
    private void LoadBatches()
    {
        string current = drpBatchRefNo.SelectedValue;
        drpBatchRefNo.Items.Clear();
        drpBatchRefNo.Items.Add(new ListItem("Choose a batch", ""));

        DataView view = SqlDataSource1.Select(DataSourceSelectArguments.Empty) as DataView;
        if (view != null)
        {
            foreach (DataRowView row in view)
            {
                string refNo = Convert.ToString(row["CustAcctNo"]);
                string title = Convert.ToString(row["RequestTitle"]);
                if (drpBatchRefNo.Items.FindByValue(refNo) != null) continue;
                drpBatchRefNo.Items.Add(new ListItem(title == "" ? refNo : refNo + " - " + title, refNo));
            }
        }
        if (drpBatchRefNo.Items.FindByValue(current) != null) drpBatchRefNo.SelectedValue = current;
        pnlBatch.Visible = drpBatchRefNo.SelectedValue != "";
    }

    protected void drpBatchRefNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowSuccess("");
        ctrlGrid.PageIndex = 0;
        ctrlGrid.DataBind();
    }

    protected void ctrlGrid_DataBound(object sender, EventArgs e)
    {
        litTotal.Text = ctrlGrid.Rows.Count > 0 ? "&middot; " + ctrlGrid.Rows.Count.ToString("N0") + " on this page" : "";
    }

    // =====================================================================
    // Approve
    // =====================================================================
    protected void BtnApprove_Click(object sender, EventArgs e)
    {
        if (!LoadCurrentUser()) { ShowError("Your session has expired. Sign in again."); return; }
        string batchRef = drpBatchRefNo.SelectedValue;
        if (batchRef == "") { ShowError("Choose a batch first."); return; }

        List<GridViewRow> rows = SelectedRows();
        if (rows.Count == 0) { ShowError("Select at least one payment first."); return; }

        try
        {
            // 1. Is this officer allowed to approve?
            string validity, smsNo;
            CheckOfficer(out validity, out smsNo);
            if (validity == "99")
            {
                ShowError("You cannot approve and confirm this request. Ask another officer to approve it.");
                return;
            }

            // 2. Approve each ticked payment
            int done = 0;
            decimal total = 0m;
            List<string> failed = new List<string>();
            foreach (GridViewRow row in rows)
            {
                string seqNo = Convert.ToString(ctrlGrid.DataKeys[row.RowIndex]["SeqNo"]);
                try
                {
                    using (SqlConnection conn = new SqlConnection(Cfg("EazyConnect")))
                    using (SqlCommand cmd = new SqlCommand("SpNapsApproveTransBatch", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@SeqNo", seqNo));
                        cmd.Parameters.Add(new SqlParameter("@ApprovingOfficer", gUser));
                        cmd.Parameters.Add(new SqlParameter("@status", "0"));
                        SqlParameter status = cmd.Parameters.Add(new SqlParameter("@StatusResponse", SqlDbType.Int));
                        status.Direction = ParameterDirection.Output;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    done++;
                    total += ToDecimal(ctrlGrid.DataKeys[row.RowIndex]["Amount"]);
                }
                catch (Exception ex)
                {
                    failed.Add(seqNo + ": " + ex.Message);
                }
            }

            if (done == 0)
            {
                ShowError("No payments were approved: " + string.Join("; ", failed.ToArray()));
                ctrlGrid.DataBind();
                return;
            }

            // 3. Officers who can approve and confirm get a one-time password and go to the confirm page
            bool canApprove = false, canConfirm = false;
            foreach (DataRow dr in QueryPositional("SpNapsGetUsersRight", gUser).Rows)
            {
                canApprove = Convert.ToString(dr["can_approve_entries"]) == "True";
                canConfirm = Convert.ToString(dr["can_audit_entries"]) == "True";
            }

            if (canApprove && canConfirm)
            {
                string token = Classes.GenerateRandomDigit(DateTime.Today.Date, 6);

                using (SqlConnection conn = new SqlConnection(Cfg("EazyConnect")))
                using (SqlCommand cmd = new SqlCommand("SpAddExtTransNapsTokens", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@SeqID", batchRef));
                    cmd.Parameters.Add(new SqlParameter("@AccountNo", "00"));
                    cmd.Parameters.Add(new SqlParameter("@Amount", total));
                    cmd.Parameters.Add(new SqlParameter("@Token", token));
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                if (validity == "10" && smsNo != "")
                {
                    string message = string.Format("DO NOT DISCLOSE* A pending Bulk Transaction is awaiting your confirmation. Use {0} as your One Time Password. It expires in 5mins.", token);
                    string phone = ToInternational(smsNo);
                    Classes.FnPushSMS(phone, Cfg("SmsUserName"), Cfg("SmsPwds"), Cfg("CompanyBrand"), message, phone);
                }

                Response.Redirect("~/eOperations/FTBulkConfirmByTokens.aspx?a=" + HttpUtility.UrlEncode(batchRef), false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            LoadBatches();
            ctrlGrid.DataBind();
            if (failed.Count == 0)
                ShowSuccess(string.Format("{0} payment{1} approved (NGN {2:N2}).", done, done == 1 ? "" : "s", total));
            else
            {
                litResult.Text = HttpUtility.HtmlEncode(string.Format("{0} approved (NGN {1:N2}).", done, total));
                Literal1.Text = HttpUtility.HtmlEncode(failed.Count + " not approved: " + string.Join("; ", failed.ToArray()));
            }
        }
        catch (Exception ex)
        {
            ShowError("The approval stopped: " + ex.Message);
            ctrlGrid.DataBind();
        }
    }

    // =====================================================================
    // Reject
    // =====================================================================
    protected void BtnDisapprove_Click(object sender, EventArgs e)
    {
        if (!LoadCurrentUser()) { ShowError("Your session has expired. Sign in again."); return; }

        int done = 0;
        List<string> failed = new List<string>();
        foreach (GridViewRow row in SelectedRows())
        {
            string seqNo = Convert.ToString(ctrlGrid.DataKeys[row.RowIndex]["SeqNo"]);
            TextBox reasonBox = row.FindControl("tatReason") as TextBox;
            string reason = reasonBox == null ? "" : reasonBox.Text.Trim();
            if (reason == "") { failed.Add(seqNo + ": reason missing"); continue; }
            try
            {
                using (SqlConnection conn = new SqlConnection(Cfg("EazyConnect")))
                using (SqlCommand cmd = new SqlCommand("SpNapsDisApproveTransBatch", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@SeqNo", seqNo));
                    cmd.Parameters.Add(new SqlParameter("@ApprovingOfficer", gUser));
                    cmd.Parameters.Add(new SqlParameter("@status", "0"));
                    SqlParameter status = cmd.Parameters.Add(new SqlParameter("@StatusResponse", SqlDbType.UniqueIdentifier));
                    status.Direction = ParameterDirection.Output;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                Classes.RecordOp(string.Format("Sproc_AddAuditLog '910','{0}','{1}',''",
                    Sql(string.Format("Title: Bulk Payment Rejection;  Batch:{0}; SeqNo:{1}; Reason: {2}; RejectedBy: {3}", drpBatchRefNo.SelectedValue, seqNo, reason, gUser)),
                    Sql(gUser)));
                done++;
            }
            catch (Exception ex)
            {
                failed.Add(seqNo + ": " + ex.Message);
            }
        }

        LoadBatches();
        ctrlGrid.DataBind();
        if (done == 0 && failed.Count == 0) ShowError("Select at least one payment first.");
        else if (failed.Count == 0) ShowSuccess(string.Format("{0} payment{1} rejected.", done, done == 1 ? "" : "s"));
        else
        {
            litResult.Text = done > 0 ? HttpUtility.HtmlEncode(done + " rejected.") : "";
            Literal1.Text = HttpUtility.HtmlEncode(failed.Count + " not rejected: " + string.Join("; ", failed.ToArray()));
        }
    }

    protected void btncancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ApproveBulkTransfers.aspx", false);
        Context.ApplicationInstance.CompleteRequest();
    }

    // =====================================================================
    // Helpers
    // =====================================================================
    private void CheckOfficer(out string validity, out string smsNo)
    {
        using (SqlConnection conn = new SqlConnection(Cfg("EazyConnect")))
        using (SqlCommand cmd = new SqlCommand("SpNapsCheckOfficerValidity", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@ApprovingOfficer", gUser));
            SqlParameter status = cmd.Parameters.Add(new SqlParameter("@StatusResponse", SqlDbType.VarChar, 50));
            status.Direction = ParameterDirection.Output;
            SqlParameter phone = cmd.Parameters.Add(new SqlParameter("@SMSNo", SqlDbType.VarChar, 50));
            phone.Direction = ParameterDirection.Output;
            conn.Open();
            cmd.ExecuteNonQuery();
            validity = Convert.ToString(status.Value).Trim();
            smsNo = Convert.ToString(phone.Value).Trim();
        }
    }

    // 0803xxxxxxx / 803xxxxxxx -> +234803xxxxxxx
    private static string ToInternational(string phone)
    {
        if (phone.Length == 11) return "+234" + phone.Substring(1, 10);
        if (phone.Length == 10) return "+234" + phone;
        return phone;
    }

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

    private static decimal ToDecimal(object value)
    {
        if (value == null || value == DBNull.Value) return 0m;
        if (value is decimal) return (decimal)value;
        decimal d;
        return decimal.TryParse(Convert.ToString(value, CultureInfo.InvariantCulture), NumberStyles.Any, CultureInfo.InvariantCulture, out d) ? d : 0m;
    }

    private static string Sql(string value)
    {
        return (value ?? "").Replace("'", "''");
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

    private static string Cfg(string name)
    {
        ConnectionStringSettings s = ConfigurationManager.ConnectionStrings[name];
        return s == null ? "" : s.ConnectionString;
    }

    // Runs a procedure whose parameters are passed by position (names not known)
    private static DataTable QueryPositional(string proc, params object[] values)
    {
        DataTable table = new DataTable();
        using (SqlConnection conn = new SqlConnection(Cfg("EazyConnect")))
        using (SqlCommand cmd = new SqlCommand())
        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
        {
            List<string> names = new List<string>();
            for (int i = 0; i < values.Length; i++)
            {
                names.Add("@p" + i);
                cmd.Parameters.Add(new SqlParameter("@p" + i, values[i] ?? DBNull.Value));
            }
            cmd.Connection = conn;
            cmd.CommandText = "EXEC " + proc + (names.Count > 0 ? " " + string.Join(",", names.ToArray()) : "");
            adapter.Fill(table);
        }
        return table;
    }
}