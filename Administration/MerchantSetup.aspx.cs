using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;
using NETClass;

public partial class Administration_MerchantSetup : System.Web.UI.Page
{
    public string luser;
    public string StrLocation;
    public string struser;
    public string guser;

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    // =====================================================================
    // Account name lookup
    // =====================================================================
    protected void txtAccountNo_TextChanged(object sender, EventArgs e)
    {
        string name = LookupAccountName(txtAccountNo.Text.Trim());
        lblAcctName.Text = HttpUtility.HtmlEncode(string.IsNullOrEmpty(name) ? "Account not found" : name);
    }

    private static string LookupAccountName(string accountNo)
    {
        if (accountNo == "") return "";

        string name = "";
        using (SqlConnection conn = new SqlConnection(Cfg("EazyConnectSource")))
        using (SqlDataAdapter adapter = new SqlDataAdapter(string.Format("SpAcctMasterSelectOne '{0}'", Sql(accountNo)), conn))
        {
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            if (ds.Tables.Count > 0)
                foreach (DataRow row in ds.Tables[0].Rows)
                    name = Convert.ToString(row["AccountDesc"]);
        }
        return name;
    }

    // =====================================================================
    // Add
    // =====================================================================
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;

        string name = txtName.Text.Trim();
        string contact = txtContact.Text.Trim();
        string phone = txtPhone.Text.Trim();
        string email = txtEmail.Text.Trim();
        string tin = txtTin.Text.Trim();
        string address = txtAddress.Text.Trim();
        string accountNo = txtAccountNo.Text.Trim();

        try
        {
            LoadCurrentUser();

            // Check the account again on the server
            string accountName = LookupAccountName(accountNo);
            if (string.IsNullOrEmpty(accountName))
            {
                ShowError(string.Format("Account {0} was not found. Check the account number.", accountNo));
                return;
            }
            lblAcctName.Text = HttpUtility.HtmlEncode(accountName);

            string bankNo = Cfg("EazyBankHostCode");

            using (SqlConnection conn = new SqlConnection(Cfg("EazyConnect")))
            using (SqlCommand cmd = new SqlCommand("SpQRMerchantAdd", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@name", name));
                cmd.Parameters.Add(new SqlParameter("@phone", phone));
                cmd.Parameters.Add(new SqlParameter("@contact", contact));
                cmd.Parameters.Add(new SqlParameter("@tin", tin));
                cmd.Parameters.Add(new SqlParameter("@email", email));
                cmd.Parameters.Add(new SqlParameter("@address", address));
                cmd.Parameters.Add(new SqlParameter("@bank_no", bankNo));
                cmd.Parameters.Add(new SqlParameter("@account_name", accountName));
                cmd.Parameters.Add(new SqlParameter("@account_number", accountNo));
                cmd.Parameters.Add(new SqlParameter("@m_fee_bearer", "1"));
                cmd.Parameters.Add(new SqlParameter("@addedby", guser ?? ""));
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            Audit("044", string.Format("New QRCode Merchant Profile Modification on MerchantName- {0}; ContactName: {1}; TIN: {2}; email: {3}; accountno: {4}; accountname: {5}; bankcode: {6}; Phone: {7}",
                name, contact, tin, email, accountNo, accountName, bankNo, phone));

            ClearForm();
            ASPxGridView1.DataBind();
            ShowSuccess(string.Format("Merchant {0} added.", name));
        }
        catch (Exception ex)
        {
            ShowError(string.Format("Merchant {0} was not added: {1}", name, ex.Message));
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearForm();
        ShowSuccess("");
    }

    private void ClearForm()
    {
        txtName.Text = "";
        txtContact.Text = "";
        txtPhone.Text = "";
        txtEmail.Text = "";
        txtTin.Text = "";
        txtAddress.Text = "";
        txtAccountNo.Text = "";
        lblAcctName.Text = "";
    }

    // =====================================================================
    // Delete
    // =====================================================================
    protected void ASPxGridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        e.Cancel = true;
        DataKey keys = ASPxGridView1.DataKeys[e.RowIndex];
        string seqNo = Convert.ToString(keys["SeqNo"]);
        string name = Convert.ToString(keys["name"]);

        try
        {
            LoadCurrentUser();

            using (SqlConnection conn = new SqlConnection(Cfg("EazyConnect")))
            using (SqlCommand cmd = new SqlCommand("SpQRMerchantDelete", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@SeqNo", seqNo));
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            Audit("004", string.Format("Application QR Merchant Deleting on - {0}; Merchant name: {1}; contact: {2}; account: {3}",
                seqNo, name, Convert.ToString(keys["contact"]), Convert.ToString(keys["account_number"])));

            ASPxGridView1.DataBind();
            ShowSuccess(string.Format("Merchant {0} deleted.", name));
        }
        catch (Exception ex)
        {
            ShowError(string.Format("Merchant {0} was not deleted: {1}", name, ex.Message));
        }
    }

    // =====================================================================
    // Helpers
    // =====================================================================
    private void Audit(string code, string message)
    {
        Classes.RecordOp(string.Format("Sproc_AddAuditLog '{0}','{1}','{2}',''", code, Sql(message), Sql(guser)));
    }

    private void LoadCurrentUser()
    {
        HttpCookie cookie = Request.Cookies["LogonUserID"];
        luser = cookie != null && !string.IsNullOrEmpty(cookie.Value) ? cookie.Value : (User.Identity.Name ?? "");
        int split = luser.LastIndexOf('?');
        struser = split >= 0 ? luser.Substring(0, split) : luser;
        StrLocation = split >= 0 ? luser.Substring(split + 1) : "";
        guser = struser;
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