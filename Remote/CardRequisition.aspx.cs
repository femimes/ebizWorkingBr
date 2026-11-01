using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;
using NETClass;

public partial class Remote_CardRequisition : System.Web.UI.Page
{
    public string gUser;
    public string luser;
    public string StrLocation = "";
    public string struser = "";

    private const int StatusDeleted = 2;
    private const int StatusSubmitted = 3;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadCurrentUser();
            txtUserID.Text = struser;
            txtlocation.Text = StrLocation;
        }
    }

    protected void sqlRequestDetails_Selected(object sender, SqlDataSourceStatusEventArgs e)
    {
        litTotal.Text = e.Exception == null && e.AffectedRows > 0 ? "&middot; " + e.AffectedRows.ToString("N0") : "";
    }

    // =====================================================================
    // Account name lookup
    // =====================================================================
    protected void txtAccountNo_TextChanged(object sender, EventArgs e)
    {
        string name = LookupAccountName(txtAccountNo.Text.Trim());
        lblAccountDesc.Text = HttpUtility.HtmlEncode(name == "" ? "Account not found" : name);
    }

    private static string LookupAccountName(string accountNo)
    {
        if (accountNo == "") return "";
        string name = "";
        DataSet ds = Classes.AXGetDataset(string.Format("SpAcctMasterSelectOne '{0}'", Sql(accountNo)));
        if (ds != null && ds.Tables.Count > 0)
            foreach (DataRow item in ds.Tables[0].Rows)
                name = Convert.ToString(item["AccountDesc"]);
        return name;
    }

    // =====================================================================
    // Add: validate, then save the request
    // =====================================================================
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;
        if (!LoadCurrentUser()) { ShowError("Your session has expired. Sign in again."); return; }

        string accountNo = txtAccountNo.Text.Trim();
        string requestType = drpTypes.SelectedValue;
        string typeName = drpTypes.SelectedItem == null ? requestType : drpTypes.SelectedItem.Text;

        try
        {
            string accountName = LookupAccountName(accountNo);
            if (accountName == "")
            {
                lblAccountDesc.Text = "Account not found";
                ShowError(string.Format("Account {0} was not found. Check the account number.", accountNo));
                return;
            }
            lblAccountDesc.Text = HttpUtility.HtmlEncode(accountName);

            string check = ValidateRequest(accountNo, requestType);
            if (check != "Success")
            {
                ShowError(check);
                return;
            }

            ExecPositional("SpAddCardRemoteRequest", accountNo, gUser, requestType, StrLocation, ClientIp());

            txtAccountNo.Text = "";
            lblAccountDesc.Text = "";
            drpTypes.SelectedIndex = 0;
            ctrlGrid.DataBind();
            ShowSuccess(string.Format("{0} request added for {1} ({2}). Submit it when you are ready.", typeName, accountName, accountNo));
        }
        catch (Exception ex)
        {
            ShowError("The request was not added: " + ex.Message);
        }
    }

    private string ValidateRequest(string accountNo, string requestType)
    {
        using (SqlConnection conn = new SqlConnection(Cfg("EazyConnect")))
        using (SqlCommand cmd = new SqlCommand("SpCardValidateRemotes", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@accountNo", accountNo));
            cmd.Parameters.Add(new SqlParameter("@requestType", requestType));
            cmd.Parameters.Add(new SqlParameter("@userId", gUser));
            cmd.Parameters.Add(new SqlParameter("@locationID", StrLocation));
            SqlParameter msg = cmd.Parameters.Add(new SqlParameter("@responseMsg", SqlDbType.VarChar, 5000));
            msg.Direction = ParameterDirection.Output;
            conn.Open();
            cmd.ExecuteNonQuery();
            return Convert.ToString(msg.Value);
        }
    }

    // =====================================================================
    // Submit / delete selected requests
    // =====================================================================
    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        UpdateSelected(StatusSubmitted, "submitted for approval");
    }

    protected void BtnDelete_Click(object sender, EventArgs e)
    {
        UpdateSelected(StatusDeleted, "deleted");
    }

    private void UpdateSelected(int status, string verb)
    {
        int done = 0;
        List<string> failed = new List<string>();

        foreach (GridViewRow row in ctrlGrid.Rows)
        {
            if (row.RowType != DataControlRowType.DataRow) continue;
            CheckBox chk = row.FindControl("CheckBox1") as CheckBox;
            if (chk == null || !chk.Checked) continue;

            DataKey keys = ctrlGrid.DataKeys[row.RowIndex];
            string accountNo = Convert.ToString(keys["accountNo"]);
            try
            {
                ExecPositional("SpCardRemoteReqStatusUpd", Convert.ToString(keys["remSeqNo"]), status.ToString());
                done++;
            }
            catch (Exception ex)
            {
                failed.Add(accountNo + ": " + ex.Message);
            }
        }

        ctrlGrid.DataBind();

        if (done == 0 && failed.Count == 0) { ShowError("Select at least one request first."); return; }
        string ok = done > 0 ? string.Format("{0} request{1} {2}.", done, done == 1 ? "" : "s", verb) : "";
        if (failed.Count == 0) ShowSuccess(ok);
        else
        {
            litResult.Text = HttpUtility.HtmlEncode(ok);
            Literal1.Text = HttpUtility.HtmlEncode(string.Format("{0} not {1}: {2}", failed.Count, verb, string.Join("; ", failed.ToArray())));
        }
    }

    // =====================================================================
    // Helpers
    // =====================================================================
    private string ClientIp()
    {
        return (Request.UserHostAddress ?? "").Replace("::ffff:", "");
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