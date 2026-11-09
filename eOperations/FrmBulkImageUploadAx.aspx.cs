using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Mandate / signature bulk upload. Images waiting on the server (Sproc_GetAvaliableMandate[ByProd]) are read from
/// their file path and saved with SpAcctMandatesSave; uploaded mandates (Sproc_GetMandates) can be removed with
/// Sproc_DeleteMandates. Converted from VB; plain ASP.NET controls and parameterised SQL.
/// </summary>
public partial class eOperations_FrmBulkImageUploadAx : System.Web.UI.Page
{
    /// <summary>Largest image accepted, in bytes (about 5 MB, as in the original signature check).</summary>
    private const int MaxImageBytes = 5005001;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadProducts();
            BindAvailable();
            BindUploaded();
        }
    }

    // =====================================================================
    // Helpers
    // =====================================================================

    private static string ImagesDb
    {
        get { return ConfigurationManager.ConnectionStrings["EazyConImages"].ConnectionString; }
    }

    /// <summary>Signed-in user from the "user?branch" identity, or the LogonUserID cookie.</summary>
    private string CurrentUser()
    {
        string id = User != null && User.Identity != null ? User.Identity.Name : "";
        if (string.IsNullOrEmpty(id) && Request.Cookies["LogonUserID"] != null) id = Request.Cookies["LogonUserID"].Value;
        id = id ?? "";
        int split = id.LastIndexOf('?');
        return split >= 0 ? id.Substring(0, split) : id;
    }

    private static DataTable Query(string sql, params SqlParameter[] parameters)
    {
        DataTable table = new DataTable();
        using (SqlConnection conn = new SqlConnection(ImagesDb))
        using (SqlCommand cmd = new SqlCommand(sql, conn))
        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
        {
            cmd.CommandTimeout = 120;
            cmd.Parameters.AddRange(parameters);
            adapter.Fill(table);
        }
        return table;
    }

    private static SqlParameter P(string name, object value)
    {
        return new SqlParameter(name, value ?? DBNull.Value);
    }

    private static string Col(DataRow row, string column)
    {
        if (row == null || !row.Table.Columns.Contains(column) || row.IsNull(column)) return "";
        return Convert.ToString(row[column]);
    }

    /// <summary>Splits "left?right" at the last '?'; with no '?' the whole text is the left part.</summary>
    private static void Split(string text, out string left, out string right)
    {
        text = text ?? "";
        int i = text.LastIndexOf('?');
        left = i >= 0 ? text.Substring(0, i) : text;
        right = i >= 0 ? text.Substring(i + 1) : "";
    }

    private void ShowSuccess(string message)
    {
        litResult.Text = HttpUtility.HtmlEncode(message);
    }

    private void ShowError(string message)
    {
        litError.Text = HttpUtility.HtmlEncode(message);
    }

    private void ClearMessages()
    {
        litResult.Text = "";
        litError.Text = "";
    }

    private static List<GridViewRow> TickedRows(GridView grid)
    {
        List<GridViewRow> rows = new List<GridViewRow>();
        foreach (GridViewRow row in grid.Rows)
        {
            if (row.RowType != DataControlRowType.DataRow) continue;
            CheckBox chk = row.FindControl("chkSelect") as CheckBox;
            if (chk != null && chk.Checked) rows.Add(row);
        }
        return rows;
    }

    // =====================================================================
    // Lists
    // =====================================================================

    private void LoadProducts()
    {
        drp_products.Items.Clear();
        drp_products.Items.Add(new ListItem("All products", ""));
        try
        {
            DataSet ds = NETClass.Classes.AXGetDataset("SpProdMasterSelectAll");
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    string code = Col(r, "ProdCode");
                    if (code == "" || drp_products.Items.FindByValue(code) != null) continue;
                    string name = Col(r, "prodName");
                    drp_products.Items.Add(new ListItem(name == "" ? code : name, code));
                }
            }
        }
        catch (Exception ex)
        {
            ShowError("The product list could not be loaded: " + ex.Message);
        }
    }

    /// <summary>
    /// Waiting images. Each row carries ManAcct ("account?serial") and filepaths ("path?mandate"),
    /// which are split into separate columns for the list.
    /// </summary>
    private void BindAvailable()
    {
        DataTable list = new DataTable();
        list.Columns.Add("AccountNo", typeof(string));
        list.Columns.Add("SerialNo", typeof(string));
        list.Columns.Add("FilePath", typeof(string));
        list.Columns.Add("FileName", typeof(string));
        try
        {
            string product = drp_products.SelectedValue;
            DataTable source = product == ""
                ? Query("EXEC Sproc_GetAvaliableMandate")
                : Query("EXEC Sproc_GetAvaliableMandateByProd @prod", P("@prod", product));

            foreach (DataRow r in source.Rows)
            {
                string account, serial, path, mandate;
                Split(Col(r, "ManAcct"), out account, out serial);
                Split(Col(r, "filepaths"), out path, out mandate);
                string fileName;
                try { fileName = Path.GetFileName(path); } catch (ArgumentException) { fileName = path; }
                list.Rows.Add(account, serial, path, fileName);
            }
        }
        catch (Exception ex)
        {
            ShowError("The waiting images could not be loaded: " + ex.Message);
        }
        gvAvailable.DataSource = list;
        gvAvailable.DataBind();
        litAvailCount.Text = string.Format("{0:N0}", list.Rows.Count);
    }

    private void BindUploaded()
    {
        DataTable list;
        try
        {
            list = Query("EXEC Sproc_GetMandates");
            if (!list.Columns.Contains("account_no")) list.Columns.Add("account_no", typeof(string));
        }
        catch (Exception ex)
        {
            list = new DataTable();
            list.Columns.Add("account_no", typeof(string));
            ShowError("The uploaded mandates could not be loaded: " + ex.Message);
        }
        gvUploaded.DataSource = list;
        gvUploaded.DataBind();
        litSelCount.Text = string.Format("{0:N0}", list.Rows.Count);
    }

    protected void drp_products_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClearMessages();
        BindAvailable();
    }

    // =====================================================================
    // Upload / delete
    // =====================================================================

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        ClearMessages();
        List<GridViewRow> rows = TickedRows(gvAvailable);
        if (rows.Count == 0) { ShowError("Tick at least one account to upload."); return; }

        string user = CurrentUser();
        int done = 0;
        List<string> failed = new List<string>();

        foreach (GridViewRow row in rows)
        {
            DataKey key = gvAvailable.DataKeys[row.RowIndex];
            string account = Convert.ToString(key.Values["AccountNo"]);
            string serial = Convert.ToString(key.Values["SerialNo"]);
            string path = Convert.ToString(key.Values["FilePath"]);
            try
            {
                byte[] photo = ReadImage(path);
                SaveMandate(account, serial, photo, user);
                done++;
            }
            catch (Exception ex)
            {
                failed.Add(string.Format("{0}: {1}", account, ex.Message));
            }
        }

        if (done > 0) ShowSuccess(string.Format("Uploaded {0} mandate image{1}.", done, done == 1 ? "" : "s"));
        if (failed.Count > 0)
            ShowError(string.Format("{0} account(s) not uploaded: {1}", failed.Count, string.Join("; ", failed.ToArray())));

        BindAvailable();
        BindUploaded();
    }

    /// <summary>Reads the whole image file from the server path recorded for the mandate.</summary>
    private static byte[] ReadImage(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) throw new InvalidOperationException("no image file is recorded");
        FileInfo file = new FileInfo(path);
        if (!file.Exists) throw new FileNotFoundException("image file not found (" + file.Name + ")");
        if (file.Length == 0) throw new InvalidOperationException("the image file is empty");
        if (file.Length > MaxImageBytes) throw new InvalidOperationException("the image is larger than 5 MB");
        return File.ReadAllBytes(file.FullName);
    }

    /// <summary>
    /// Saves one mandate image through SpAcctMandatesSave. @Signature and @Image are left to the
    /// procedure's defaults, as before; the serial number is stored as the full name, as before.
    /// </summary>
    private static void SaveMandate(string account, string serial, byte[] photo, string user)
    {
        using (SqlConnection conn = new SqlConnection(ImagesDb))
        using (SqlCommand cmd = new SqlCommand("SpAcctMandatesSave", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 120;
            cmd.Parameters.Add(P("@Sequence", 0));
            cmd.Parameters.Add(P("@AccountNo", account));
            cmd.Parameters.Add(P("@FullName", serial));
            cmd.Parameters.Add(P("@Photo", photo));
            cmd.Parameters.Add(P("@Comments", "Bulk Upload"));
            cmd.Parameters.Add(P("@SignClass", "A"));
            cmd.Parameters.Add(P("@LastModifiedBy", string.IsNullOrEmpty(user) ? "Super" : user));
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        ClearMessages();
        List<GridViewRow> rows = TickedRows(gvUploaded);
        if (rows.Count == 0) { ShowError("Tick at least one uploaded account to delete."); return; }

        int done = 0;
        List<string> failed = new List<string>();
        foreach (GridViewRow row in rows)
        {
            string account = Convert.ToString(gvUploaded.DataKeys[row.RowIndex].Values["account_no"]);
            try
            {
                Query("EXEC Sproc_DeleteMandates @account", P("@account", account));
                done++;
            }
            catch (Exception ex)
            {
                failed.Add(account + ": " + ex.Message);
            }
        }

        if (done > 0) ShowSuccess(string.Format("Deleted {0} uploaded mandate{1}.", done, done == 1 ? "" : "s"));
        if (failed.Count > 0)
            ShowError(string.Format("{0} account(s) not deleted: {1}", failed.Count, string.Join("; ", failed.ToArray())));

        BindAvailable();
        BindUploaded();
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Path, false);
        Context.ApplicationInstance.CompleteRequest();
    }
}
