using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;
using NETClass;

public partial class Administration_BillerSetup : System.Web.UI.Page
{
    public string luser;
    public string StrLocation;
    public string struser;
    public string guser;

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    // Start on category 001 (the old default) when it exists
    protected void ASPxComboBox1_DataBound(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        ListItem item = ASPxComboBox1.Items.FindByValue("001");
        if (item != null) ASPxComboBox1.SelectedValue = "001";
    }

    protected void ASPxComboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        ResetForm();
        ShowSuccess("");
        ASPxGridView1.PageIndex = 0;
        ASPxGridView1.DataBind();
    }

    // =====================================================================
    // Edit: load the row into the form
    // =====================================================================
    protected void ASPxGridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "EditBiller") return;

        int index;
        if (!int.TryParse(Convert.ToString(e.CommandArgument), out index) || index < 0 || index >= ASPxGridView1.DataKeys.Count) return;

        DataKey keys = ASPxGridView1.DataKeys[index];
        txtBillerID.Text = Key(keys, "BillerID");
        txtBillerDesc.Text = Key(keys, "BillerDesc");
        txtBillerAbbr.Text = Key(keys, "BillerAbbr");
        txtBillerOpCode.Text = Key(keys, "BillerOpCode");
        txtBillerShortcut.Text = Key(keys, "BillerShortcut");

        hfEditing.Value = txtBillerID.Text;
        txtBillerID.ReadOnly = true;
        litFormTitle.Text = "Edit biller " + HttpUtility.HtmlEncode(txtBillerID.Text);
        btnSave.Text = "Save changes";
        btnCancel.Text = "Cancel";
        ShowSuccess("");
    }

    // =====================================================================
    // Save (add or change: the procedure does both)
    // =====================================================================
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;

        string catCode = ASPxComboBox1.SelectedValue;
        if (catCode == "") { ShowError("Choose a biller category first."); return; }

        bool editing = hfEditing.Value != "";
        string billerId = editing ? hfEditing.Value : txtBillerID.Text.Trim();
        string desc = txtBillerDesc.Text.Trim();
        string abbr = txtBillerAbbr.Text.Trim();
        string opCode = txtBillerOpCode.Text.Trim();
        string shortcut = txtBillerShortcut.Text.Trim();

        try
        {
            LoadCurrentUser();

            using (SqlConnection conn = new SqlConnection(Cfg("EazyConnect")))
            using (SqlCommand cmd = new SqlCommand("SpAddBillerMasterEx", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@BillerID", billerId));
                cmd.Parameters.Add(new SqlParameter("@BillerDesc", desc));
                cmd.Parameters.Add(new SqlParameter("@BillerAbbr", abbr));
                cmd.Parameters.Add(new SqlParameter("@BillerOpCode", opCode));
                cmd.Parameters.Add(new SqlParameter("@BillerCatCode", catCode));
                cmd.Parameters.Add(new SqlParameter("@BillerShortcut", shortcut));
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            Audit("900", string.Format("{0}Application Biller's Setup Modification on - {1} Code:{2};Desc:{3}; Abbr:{4}; SlugCode:{5}; Category:{6}; Shortcut:{7};",
                editing ? "" : "New ", desc, billerId, desc, abbr, opCode, catCode, shortcut));

            ResetForm();
            ASPxGridView1.DataBind();
            ShowSuccess(string.Format("Biller {0} {1}.", desc, editing ? "updated" : "added"));
        }
        catch (Exception ex)
        {
            ShowError(string.Format("Biller {0} was not saved: {1}", desc, ex.Message));
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ResetForm();
        ShowSuccess("");
    }

    private void ResetForm()
    {
        hfEditing.Value = "";
        txtBillerID.ReadOnly = false;
        txtBillerID.Text = "";
        txtBillerDesc.Text = "";
        txtBillerAbbr.Text = "";
        txtBillerOpCode.Text = "";
        txtBillerShortcut.Text = "";
        litFormTitle.Text = "Add biller";
        btnSave.Text = "Add biller";
        btnCancel.Text = "Clear";
    }

    // =====================================================================
    // Delete
    // =====================================================================
    protected void ASPxGridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        e.Cancel = true;
        DataKey keys = ASPxGridView1.DataKeys[e.RowIndex];
        string billerId = Key(keys, "BillerID");
        string desc = Key(keys, "BillerDesc");
        string catCode = ASPxComboBox1.SelectedValue;

        try
        {
            LoadCurrentUser();
            using (SqlConnection conn = new SqlConnection(Cfg("EazyConnect")))
            using (SqlCommand cmd = new SqlCommand("SpDeleteBillerMaster", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@BillerID", billerId));
                cmd.Parameters.Add(new SqlParameter("@BillerCatCode", catCode));
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            Audit("900", string.Format("Application Biller's Setup Deleteting on - {0} ", billerId));
            if (hfEditing.Value == billerId) ResetForm();
            ASPxGridView1.DataBind();
            ShowSuccess(string.Format("Biller {0} deleted.", desc));
        }
        catch (Exception ex)
        {
            ShowError(string.Format("Biller {0} was not deleted: {1}", desc, ex.Message));
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

    private static string Key(DataKey keys, string name)
    {
        object v = keys[name];
        return v == null || v == DBNull.Value ? "" : Convert.ToString(v, CultureInfo.InvariantCulture);
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