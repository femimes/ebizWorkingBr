using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;
using NETClass;

public partial class Processing_FrmCorpOrgs : System.Web.UI.Page
{
    public string luser;
    public string StrLocation;
    public string struser;
    public string guser;

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    // =====================================================================
    // Add
    // =====================================================================
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;

        string code = txtCorpCode.Text.Trim();
        if (SaveOrganisation(code, txtCorpDesc.Text.Trim(), txtCorpAcctNo.Text.Trim(), true))
        {
            ShowSuccess(string.Format("Organisation {0} saved.", code));
            ClearForm();
            ASPxGridView1.DataBind();
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearForm();
        ShowSuccess("");
    }

    private void ClearForm()
    {
        txtCorpCode.Text = "";
        txtCorpDesc.Text = "";
        txtCorpAcctNo.Text = "";
    }

    // =====================================================================
    // Edit and delete in the grid
    // =====================================================================
    protected void ASPxGridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        ShowSuccess("");
        ASPxGridView1.EditIndex = e.NewEditIndex;
    }

    protected void ASPxGridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        ASPxGridView1.EditIndex = -1;
    }

    protected void ASPxGridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        e.Cancel = true;

        GridViewRow row = ASPxGridView1.Rows[e.RowIndex];
        string code = Convert.ToString(ASPxGridView1.DataKeys[e.RowIndex]["CorpCode"]);

        if (SaveOrganisation(code, TextOf(row, "txtEditCorpDesc"), TextOf(row, "txtEditCorpAcctNo"), false))
        {
            ASPxGridView1.EditIndex = -1;
            ASPxGridView1.DataBind();
            ShowSuccess(string.Format("Organisation {0} updated.", code));
        }
    }

    protected void ASPxGridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        e.Cancel = true;
        string code = Convert.ToString(ASPxGridView1.DataKeys[e.RowIndex]["CorpCode"]);

        try
        {
            LoadCurrentUser();

            using (SqlConnection conn = new SqlConnection(Cfg("EazyConnect")))
            using (SqlCommand cmd = new SqlCommand("Sproc_DeleteAppCorpProfile", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CorpCode", code));
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            string msg = string.Format("Application Corporate Company Deleting on - {0}", code);
            Classes.RecordOp(string.Format("Sproc_AddAuditLog '004','{0}','{1}',''", Sql(msg), Sql(guser)));

            ASPxGridView1.EditIndex = -1;
            ASPxGridView1.DataBind();
            ShowSuccess(string.Format("Organisation {0} deleted.", code));
        }
        catch (Exception ex)
        {
            ShowError(string.Format("Organisation {0} was not deleted: {1}", code, ex.Message));
        }
    }

    // =====================================================================
    // Save (add and update use the same procedure, as before)
    // =====================================================================
    private bool SaveOrganisation(string code, string name, string accountNo, bool isNew)
    {
        if (code == "") { ShowError("Corporate code cannot be empty."); return false; }
        if (name == "") { ShowError("Organisation name cannot be empty."); return false; }
        if (accountNo == "") { ShowError("Account number cannot be empty."); return false; }

        try
        {
            LoadCurrentUser();

            // The account must exist before it can be linked
            string accountName = LookupAccountName(accountNo);
            if (string.IsNullOrEmpty(accountName))
            {
                ShowError(string.Format("Account {0} was not found. Check the account number.", accountNo));
                return false;
            }

            using (SqlConnection conn = new SqlConnection(Cfg("EazyConnect")))
            using (SqlCommand cmd = new SqlCommand("Sproc_AddAppCorpProfile", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CorpCode", code));
                cmd.Parameters.Add(new SqlParameter("@CorpDesc", name));
                cmd.Parameters.Add(new SqlParameter("@CorpAcctNo", accountNo));
                cmd.Parameters.Add(new SqlParameter("@CorpStatus", "0"));
                cmd.Parameters.Add(new SqlParameter("@AddedBy", guser));
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            string msg = string.Format(isNew
                    ? "New Corporate Company Modification on Code- {0}; Name: {1}; Account no: {2}"
                    : "Application Modification on Corporate Company on Code- {0}; Name: {1}; Account no: {2}",
                code, name, accountNo);
            Classes.RecordOp(string.Format("Sproc_AddAuditLog '044','{0}','{1}',''", Sql(msg), Sql(guser)));
            return true;
        }
        catch (Exception ex)
        {
            ShowError(string.Format("Organisation {0} was not saved: {1}", code, ex.Message));
            return false;
        }
    }

    private static string LookupAccountName(string accountNo)
    {
        string name = "";
        using (SqlConnection conn = new SqlConnection(Cfg("EazyConnect")))
        using (SqlDataAdapter adapter = new SqlDataAdapter(string.Format("SpGetAxAccountName '{0}'", Sql(accountNo)), conn))
        {
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                    name = Convert.ToString(item["AccountDesc"]);
            }
        }
        return name;
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
        guser = struser;
    }

    private static string TextOf(GridViewRow row, string id)
    {
        TextBox box = row.FindControl(id) as TextBox;
        return box == null ? "" : (box.Text ?? "").Trim();
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