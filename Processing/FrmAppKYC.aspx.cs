using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;
using NETClass;

public partial class Processing_FrmAppKYC : System.Web.UI.Page
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

        string code = txtKeyid.Text.Trim();
        if (SaveKyc(code, txtKeydesc.Text.Trim(), txtKeytype.Text.Trim(), txtRemarks.Text.Trim()))
        {
            ShowSuccess(string.Format("KYC code {0} saved.", code));
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
        txtKeyid.Text = "";
        txtKeydesc.Text = "";
        txtKeytype.Text = "";
        txtRemarks.Text = "";
    }

    // =====================================================================
    // Edit in the grid
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
        // We save it ourselves, so stop the data source from trying to
        e.Cancel = true;

        GridViewRow row = ASPxGridView1.Rows[e.RowIndex];
        string code = Convert.ToString(ASPxGridView1.DataKeys[e.RowIndex]["Keyid"]);

        if (SaveKyc(code, TextOf(row, "txtEditKeydesc"), TextOf(row, "txtEditKeytype"), TextOf(row, "txtEditRemarks")))
        {
            ASPxGridView1.EditIndex = -1;
            ASPxGridView1.DataBind();
            ShowSuccess(string.Format("KYC code {0} updated.", code));
        }
    }

    // =====================================================================
    // Save (add and update use the same procedure, as before)
    // =====================================================================
    private bool SaveKyc(string code, string desc, string type, string remarks)
    {
        if (code == "") { ShowError("Setup code cannot be empty."); return false; }
        if (desc == "") { ShowError("Setup description cannot be empty."); return false; }
        if (type == "") { ShowError("Setup type cannot be empty."); return false; }

        try
        {
            LoadCurrentUser();

            using (SqlConnection conn = new SqlConnection(Cfg("EazyConnect")))
            using (SqlCommand cmd = new SqlCommand("SpAddAllKYCinfo", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@keyid", code));
                cmd.Parameters.Add(new SqlParameter("@keydesc", desc));
                cmd.Parameters.Add(new SqlParameter("@Keytype", type));
                cmd.Parameters.Add(new SqlParameter("@Remarks", remarks ?? ""));
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            string msg = string.Format("New KYC Code Modification on Code- {0}; Description: {1}; type: {2}", code, desc, type);
            Classes.RecordOp(string.Format("Sproc_AddAuditLog '044','{0}','{1}',''", Sql(msg), Sql(guser)));
            return true;
        }
        catch (Exception ex)
        {
            ShowError(string.Format("KYC code {0} was not saved: {1}", code, ex.Message));
            return false;
        }
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