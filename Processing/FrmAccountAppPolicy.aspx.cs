using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;
using NETClass;

public partial class Processing_FrmAccountAppPolicy : System.Web.UI.Page
{
    // Signed-in staff user. The auth name is stored as "username?branch".
    public string gUser = "";
    public string luser = "";
    public string StrLocation = "";
    public string struser = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadCurrentUser();
            txtUserID.Text = struser;
            txtlocation.Text = StrLocation;
        }
    }

    private void LoadCurrentUser()
    {
        luser = HttpContext.Current.User.Identity.Name ?? "";
        int split = luser.LastIndexOf('?');
        struser = split >= 0 ? luser.Substring(0, split) : luser;
        StrLocation = split >= 0 ? luser.Substring(split + 1) : "";
        gUser = struser;
    }

    public String GetIPAddress()
    {
        string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        if (string.IsNullOrEmpty(ip))
            ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        return (ip ?? "").Replace("::ffff:", "");
    }

    // =====================================================================
    // Account lookup
    // =====================================================================
    protected void txtAccountNo_TextChanged(object sender, EventArgs e)
    {
        lblAccountDesc.Text = HttpUtility.HtmlEncode(LookupAccountName(txtAccountNo.Text.Trim()) ?? "Account not found");
    }

    private static string LookupAccountName(string accountNo)
    {
        if (accountNo == "") return null;

        string name = null;
        foreach (DataRow item in Classes.AXGetDataset(string.Format("SpAcctMasterSelectOne '{0}'", Sql(accountNo))).Tables[0].Rows)
            name = Convert.ToString(item["AccountDesc"]);
        return name;
    }

    // =====================================================================
    // Add a request
    // =====================================================================
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;

        LoadCurrentUser();
        string accountNo = txtAccountNo.Text.Trim();
        string policyId = drpTypes.SelectedValue;

        try
        {
            string status = CheckValidateRequest(accountNo, policyId, gUser, StrLocation);
            if (status != "Success")
            {
                ShowResult(null, status);
                return;
            }

            Classes.GetDataset(string.Format("SpAddAcctPolicyRequest '{0}','{1}','{2}','{3}','{4}','{5}'",
                Sql(accountNo), Sql(gUser), Sql(policyId), Sql(StrLocation), Sql(GetIPAddress()), Sql(txtPolicyValue.Text.Trim())));

            ctrlGrid.DataBind();
            ShowResult(string.Format("Request added for account {0}.", accountNo), null);
            txtPolicyValue.Text = "";
        }
        catch (Exception ex)
        {
            ShowResult(null, "The request was not added: " + ex.Message);
        }
    }

    private string CheckValidateRequest(string accountNo, string requestType, string userId, string locationCode)
    {
        using (SqlConnection conn = new SqlConnection(Cfg("EazyConnect")))
        using (SqlCommand cmd = new SqlCommand("SpValidateAccountPolicies", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@accountNo", accountNo));
            cmd.Parameters.Add(new SqlParameter("@requestType", requestType));
            cmd.Parameters.Add(new SqlParameter("@userId", userId));
            cmd.Parameters.Add(new SqlParameter("@locationID", locationCode));
            SqlParameter output = cmd.Parameters.Add(new SqlParameter("@responseMsg", SqlDbType.VarChar, 5000));
            output.Direction = ParameterDirection.Output;

            conn.Open();
            cmd.ExecuteNonQuery();
            return Convert.ToString(output.Value);
        }
    }

    // =====================================================================
    // Submit / delete the ticked requests
    // =====================================================================
    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        LoadCurrentUser();
        UpdateSelected("3", "Submitted", true);
    }

    protected void BtnDelete_Click(object sender, EventArgs e)
    {
        LoadCurrentUser();
        UpdateSelected("2", "Deleted", false);
    }

    private void UpdateSelected(string newStatus, string doneLabel, bool audit)
    {
        List<string> problems = new List<string>();
        int done = 0, ticked = 0;

        foreach (GridViewRow row in ctrlGrid.Rows)
        {
            if (row.RowType != DataControlRowType.DataRow) continue;
            CheckBox chk = row.FindControl("CheckBox1") as CheckBox;
            if (chk == null || !chk.Checked) continue;

            ticked++;
            DataKey keys = ctrlGrid.DataKeys[row.RowIndex];
            string seqNo = Convert.ToString(keys["remSeqNo"]);
            string accountNo = Convert.ToString(keys["accountNo"]);

            try
            {
                Classes.GetDataset(string.Format("SpAcctPolicyReqStatusUpd '{0}','{1}'", Sql(seqNo), newStatus));

                if (audit)
                {
                    string msg = string.Format("Title: Account Policy Request; User ID:{0}; LocationCode:-{1}; Reason: {2}; AccountNo: {3}",
                        gUser, StrLocation, "Submitted By- " + gUser, accountNo);
                    Classes.RecordOp(string.Format("Sproc_AddAuditLog '910','{0}','{1}',''", Sql(msg), Sql(gUser)));
                }
                done++;
            }
            catch (Exception ex)
            {
                problems.Add(string.Format("{0}: {1}", accountNo, ex.Message));
            }
        }

        if (ticked == 0)
        {
            ShowResult(null, "Select at least one request first.");
            return;
        }

        ctrlGrid.DataBind();
        ShowResult(done > 0 ? string.Format("{0}: {1} request{2}.", doneLabel, done, done == 1 ? "" : "s") : null,
            problems.Count > 0 ? string.Join("\n", problems.ToArray()) : null);
    }

    // =====================================================================
    // Helpers
    // =====================================================================
    private void ShowResult(string success, string error)
    {
        litResult.Text = string.IsNullOrEmpty(success) ? "" : HttpUtility.HtmlEncode(success);
        Literal1.Text = string.IsNullOrEmpty(error) ? "" : HttpUtility.HtmlEncode(error).Replace("\n", "<br />");
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