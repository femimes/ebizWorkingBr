using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using NETClass;

public partial class Administration_BillerProducts : System.Web.UI.Page
{
    public string luser;
    public string StrLocation;
    public string struser;
    public string guser;

    protected void Page_PreRender(object sender, EventArgs e)
    {
        bool hasBiller = ASPxComboBox2.SelectedValue != "";
        pnlProducts.Visible = hasBiller;
        pnlChoose.Visible = !hasBiller;
    }

    // Start on category 001 (the old default) when it exists
    protected void ASPxComboBox1_DataBound(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        if (ASPxComboBox1.Items.FindByValue("001") != null) ASPxComboBox1.SelectedValue = "001";
    }

    protected void ASPxComboBox2_DataBound(object sender, EventArgs e)
    {
        ASPxComboBox2.Items.Insert(0, new ListItem("Choose a biller", ""));
    }

    protected void ASPxComboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        ResetForm();
        ShowSuccess("");
        ASPxComboBox2.DataBind();
        ASPxGridView1.PageIndex = 0;
    }

    protected void ASPxComboBox2_SelectedIndexChanged(object sender, EventArgs e)
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
        if (e.CommandName != "EditProduct") return;

        int index;
        if (!int.TryParse(Convert.ToString(e.CommandArgument), out index) || index < 0 || index >= ASPxGridView1.DataKeys.Count) return;

        DataKey keys = ASPxGridView1.DataKeys[index];
        txtItemCode.Text = Key(keys, "BillerItemCode");
        txtItemDesc.Text = Key(keys, "BillerItemDesc");
        txtAcctNo.Text = Key(keys, "BillerItemAcctNo");
        txtDisplay.Text = Key(keys, "BillerItemDisplay");
        txtShortcut.Text = Key(keys, "BillerItemShortcut");
        txtSlug.Text = Key(keys, "BillerItemSlug");
        txtAmount.Text = ToDecimal(Key(keys, "BillerItemAmt")).ToString("0.00", CultureInfo.InvariantCulture);
        txtFeeCode.Text = Key(keys, "BillFeeCode");
        chkPrecondition.Checked = AsBool(keys["IsPrecondition"]);

        hfEditing.Value = txtItemCode.Text;
        txtItemCode.ReadOnly = true;
        litFormTitle.Text = "Edit product " + HttpUtility.HtmlEncode(txtItemCode.Text);
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
        string billerId = ASPxComboBox2.SelectedValue;
        if (billerId == "") { ShowError("Choose a biller first."); return; }

        bool editing = hfEditing.Value != "";
        string itemCode = editing ? hfEditing.Value : txtItemCode.Text.Trim();
        string desc = txtItemDesc.Text.Trim();
        string acctNo = txtAcctNo.Text.Trim();
        string display = txtDisplay.Text.Trim();
        string shortcut = txtShortcut.Text.Trim();
        string slug = txtSlug.Text.Trim();
        decimal amount = ToDecimal(txtAmount.Text);
        string feeCode = txtFeeCode.Text.Trim();
        int precondition = chkPrecondition.Checked ? 1 : 0;

        try
        {
            LoadCurrentUser();
            SaveProduct(itemCode, billerId, desc, acctNo, display, slug, amount, feeCode, shortcut, precondition);

            Audit("900", string.Format("{0}Application Biller's Product Setup Modification on - {1} Code:{2};Desc:{3}; AcctNo:{4}; SlugCode:{5}; Category:{6}; Shortcut:{7};Amt:{8};FeeCode:{9};IsPrecondition:{10};",
                editing ? "" : "New ", desc, itemCode, desc, acctNo, slug, catCode, shortcut, amount, feeCode, precondition == 1));

            ResetForm();
            ASPxGridView1.DataBind();
            ShowSuccess(string.Format("Product {0} {1}.", desc, editing ? "updated" : "added"));
        }
        catch (Exception ex)
        {
            ShowError(string.Format("Product {0} was not saved: {1}", desc, ex.Message));
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
        txtItemCode.ReadOnly = false;
        txtItemCode.Text = "";
        txtItemDesc.Text = "";
        txtAcctNo.Text = "";
        txtDisplay.Text = "";
        txtShortcut.Text = "";
        txtSlug.Text = "";
        txtAmount.Text = "0.00";
        txtFeeCode.Text = "000";
        chkPrecondition.Checked = false;
        litFormTitle.Text = "Add product";
        btnSave.Text = "Add product";
        btnCancel.Text = "Clear";
    }

    // =====================================================================
    // Delete
    // =====================================================================
    protected void ASPxGridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        e.Cancel = true;
        DataKey keys = ASPxGridView1.DataKeys[e.RowIndex];
        string itemCode = Key(keys, "BillerItemCode");
        string desc = Key(keys, "BillerItemDesc");
        string billerId = ASPxComboBox2.SelectedValue;

        try
        {
            LoadCurrentUser();
            using (SqlConnection conn = new SqlConnection(Cfg("EazyConnect")))
            using (SqlCommand cmd = new SqlCommand("SpDeleteBillerDetails", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@BillerItemCode", itemCode));
                cmd.Parameters.Add(new SqlParameter("@BillerID", billerId));
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            Audit("900", string.Format("Application Biller's Product Setup Deleteting on - {0} ", itemCode));
            if (hfEditing.Value == itemCode) ResetForm();
            ASPxGridView1.DataBind();
            ShowSuccess(string.Format("Product {0} deleted.", desc));
        }
        catch (Exception ex)
        {
            ShowError(string.Format("Product {0} was not deleted: {1}", desc, ex.Message));
        }
    }

    // =====================================================================
    // Automatic update from VAS: categories, then billers, then products
    // =====================================================================
    protected void btnUpd_Click(object sender, EventArgs e)
    {
        int categories = 0, billers = 0, products = 0;
        List<string> problems = new List<string>();

        try
        {
            LoadCurrentUser();

            string host = Cfg("iMPhaseHostUrl").TrimEnd('/');
            string token = Convert.ToBase64String(Encoding.Default.GetBytes(string.Format("{0}:{1}",
                Classes.DecryptString(Cfg("KeyID")), Classes.DecryptString(Cfg("Key")))));

            Dictionary<string, object> catResp = PostVas(host + "/BillerCategoryVAS.aspx", token, new { });
            string catCode = Str(catResp, "ResponseCode");
            AddtoLogFile(string.Format("ResponseCode: {0}; ResponseDesc: {1}", catCode, Str(catResp, "ResponseDesc")), "responseCategory");

            if (catCode != "00")
            {
                ShowError(string.Format("VAS did not return the categories: {0} ({1})", Str(catResp, "ResponseDesc"), catCode));
                return;
            }

            // Default account and shortcut come from the existing categories (last row wins, as before)
            string defaultAcctNo = "", defaultShortcut = "";
            foreach (DataRow row in Query("SpmGetBillerCategory").Rows)
            {
                defaultAcctNo = Convert.ToString(row["BillerCatAcctNo"]);
                defaultShortcut = Convert.ToString(row["BillerShortcut"]);
            }

            foreach (Dictionary<string, object> cat in Rows(catResp.ContainsKey("data") ? catResp["data"] : null))
            {
                string catSlug = Str(cat, "slug");
                string catName = Str(cat, "name");
                if (catSlug == "") continue;

                Exec("SpBillerCatAdd",
                    P("@BillerCatCode", catSlug), P("@BillerCatDesc", catName), P("@BillerCatAcctNo", defaultAcctNo),
                    P("@BillerShortcut", defaultShortcut), P("@BillerCatShortcut", catSlug), P("@BillerCatSlug", catSlug));
                Audit("900", string.Format("Application Biller's Group or Category  Modification - BillerCatCode: {0}; BillerCatname: {1}; BillerCatAcctNo: {2}", catSlug, catName, defaultAcctNo));
                categories++;

                Dictionary<string, object> billResp;
                try { billResp = PostVas(host + "/BillerVAS.aspx", token, new { BillerCatSlug = catSlug }); }
                catch (Exception ex) { problems.Add(string.Format("Billers for {0}: {1}", catSlug, ex.Message)); continue; }

                if (Str(billResp, "RespCode") != "00") continue;

                foreach (Dictionary<string, object> biller in ResponseData(billResp))
                {
                    string billerId = Str(biller, "id");
                    string billerName = Str(biller, "name");
                    string billerSlug = Str(biller, "slug");
                    if (billerId == "") continue;

                    Exec("SpAddBillerMasterEx",
                        P("@BillerID", billerId), P("@BillerDesc", billerName), P("@BillerAbbr", billerSlug),
                        P("@BillerOpCode", billerSlug), P("@BillerCatCode", catSlug), P("@BillerShortcut", ""));
                    Audit("900", string.Format("New Application Biller's Setup Modification on - {0} Code:{1};Desc:{2}; Abbr:{3}; SlugCode:{4}; Category:{5}; Shortcut:{6};",
                        billerName, billerId, billerName, billerSlug, billerSlug, catSlug, "none"));
                    billers++;

                    Dictionary<string, object> itemResp;
                    try { itemResp = PostVas(host + "/BillerItemsVAS.aspx", token, new { BillerID = billerId }); }
                    catch (Exception ex) { problems.Add(string.Format("Products for {0}: {1}", billerName, ex.Message)); continue; }

                    if (itemResp == null || Str(itemResp, "ResponseCode") != "00") continue;

                    // Keep the display label and precondition flag already set for this biller
                    string display = "Reference";
                    bool precondition = false;
                    foreach (DataRow row in Query("SpmGetBillerItems", P("@BillerCatCode", catSlug), P("@BillerID", billerId)).Rows)
                    {
                        display = Convert.ToString(row["BillerItemDisplay"]);
                        precondition = AsBool(row["IsPrecondition"]);
                    }

                    foreach (Dictionary<string, object> item in ResponseData(itemResp))
                    {
                        string itemId = Str(item, "id");
                        if (itemId == "") continue;
                        string itemName = Str(item, "name");
                        string itemSlug = Str(item, "slug");
                        decimal amount = ToDecimal(Str(item, "amount"));

                        SaveProduct(itemId, billerId, itemName, defaultAcctNo, display, itemSlug, amount, "000", "", precondition ? 1 : 0);
                        Audit("900", string.Format("Application Biller's Product Setup Modification on - {0} Code:{1};Desc:{2}; AcctNo:{3}; SlugCode:{4}; Category:{5}; Shortcut:{6};Amt:{7};FeeCode:{8};IsPrecondition:{9};",
                            itemName, itemId, itemName, defaultAcctNo, itemSlug, catSlug, "None", amount, "000", precondition ? 1 : 0));
                        products++;
                    }
                }
            }

            // Re-apply precondition settings
            Classes.RecordOp("SpBillerAutoUpdate");

            ASPxComboBox1.DataBind();
            ASPxComboBox2.DataBind();
            ASPxGridView1.DataBind();

            string summary = string.Format("Updated from VAS: {0} categories, {1} billers, {2} products.", categories, billers, products);
            if (problems.Count > 0)
                ShowError(summary + " Some lists could not be fetched: " + string.Join("; ", problems.ToArray()));
            else
                ShowSuccess(summary);
        }
        catch (Exception ex)
        {
            ShowError(string.Format("The update stopped after {0} categories, {1} billers and {2} products: {3}", categories, billers, products, ex.Message));
        }
    }

    private void SaveProduct(string itemCode, string billerId, string desc, string acctNo, string display, string slug,
        decimal amount, string feeCode, string shortcut, int precondition)
    {
        Exec("SpAddBillerDetailsEx",
            P("@BillerItemCode", itemCode), P("@BillerID", billerId), P("@BillerItemDesc", desc),
            P("@BillerItemAcctNo", acctNo), P("@BillerItemDisplay", display), P("@BillerItemSlug", slug),
            P("@BillerItemAmt", amount), P("@BillFeeCode", feeCode), P("@BillerItemShortcut", shortcut),
            P("@IsPrecondition", precondition));
    }

    // =====================================================================
    // VAS and JSON helpers
    // =====================================================================
    private static Dictionary<string, object> PostVas(string url, string token, object body)
    {
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        req.Method = "POST";
        req.ContentType = "application/json";
        req.Timeout = 120000;
        req.Headers.Add("AuthToken", token);

        byte[] bytes = Encoding.UTF8.GetBytes(new JavaScriptSerializer().Serialize(body));
        req.ContentLength = bytes.Length;
        using (Stream s = req.GetRequestStream()) s.Write(bytes, 0, bytes.Length);

        string raw;
        using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
        using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
            raw = reader.ReadToEnd();

        if (string.IsNullOrEmpty(raw)) return null;
        return Parse(raw) as Dictionary<string, object>;
    }

    private static object Parse(string json)
    {
        JavaScriptSerializer js = new JavaScriptSerializer();
        js.MaxJsonLength = int.MaxValue;
        return js.DeserializeObject(json);
    }

    // "data" may be a JSON string or an object; find the responseData list inside it
    private static IEnumerable<Dictionary<string, object>> ResponseData(Dictionary<string, object> resp)
    {
        object data = resp != null && resp.ContainsKey("data") ? resp["data"] : null;
        if (data is string)
        {
            string s = ((string)data).Trim();
            data = s == "" ? null : Parse(s);
        }
        return Rows(Find(data, "responseData"));
    }

    private static object Find(object node, string key)
    {
        Dictionary<string, object> dict = node as Dictionary<string, object>;
        if (dict != null)
        {
            foreach (KeyValuePair<string, object> kv in dict)
                if (string.Equals(kv.Key, key, StringComparison.OrdinalIgnoreCase)) return kv.Value;
            foreach (object child in dict.Values)
            {
                object found = Find(child, key);
                if (found != null) return found;
            }
            return null;
        }
        IEnumerable list = node as IEnumerable;
        if (list != null && !(node is string))
            foreach (object child in list)
            {
                object found = Find(child, key);
                if (found != null) return found;
            }
        return null;
    }

    // Accept a single object or a list of objects
    private static IEnumerable<Dictionary<string, object>> Rows(object node)
    {
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        if (node is string)
        {
            string s = ((string)node).Trim();
            node = s == "" ? null : Parse(s);
        }
        Dictionary<string, object> one = node as Dictionary<string, object>;
        if (one != null) { rows.Add(one); return rows; }
        IEnumerable list = node as IEnumerable;
        if (list != null)
            foreach (object child in list)
            {
                Dictionary<string, object> d = child as Dictionary<string, object>;
                if (d != null) rows.Add(d);
            }
        return rows;
    }

    private static string Str(Dictionary<string, object> dict, string key)
    {
        if (dict == null) return "";
        foreach (KeyValuePair<string, object> kv in dict)
            if (string.Equals(kv.Key, key, StringComparison.OrdinalIgnoreCase))
                return kv.Value == null ? "" : Convert.ToString(kv.Value, CultureInfo.InvariantCulture).Trim();
        return "";
    }

    // =====================================================================
    // Database helpers
    // =====================================================================
    private static SqlParameter P(string name, object value)
    {
        return new SqlParameter(name, value ?? DBNull.Value);
    }

    private static void Exec(string proc, params SqlParameter[] parameters)
    {
        using (SqlConnection conn = new SqlConnection(Cfg("EazyConnect")))
        using (SqlCommand cmd = new SqlCommand(proc, conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(parameters);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }

    private static DataTable Query(string proc, params SqlParameter[] parameters)
    {
        DataTable table = new DataTable();
        using (SqlConnection conn = new SqlConnection(Cfg("EazyConnect")))
        using (SqlCommand cmd = new SqlCommand(proc, conn))
        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(parameters);
            adapter.Fill(table);
        }
        return table;
    }

    // Log responses only (never the auth token)
    public void AddtoLogFile(string Message, string WebPage)
    {
        try
        {
            string dir = Server.MapPath("~/log/");
            Directory.CreateDirectory(dir);
            string path = Path.Combine(dir, "Log_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt");
            File.AppendAllText(path, string.Format("——————-START————-{0}\r\nEmailLog :{1}\r\n{2}\r\n——————-END————-{0}\r\n", DateTime.Now, WebPage, Message));
        }
        catch
        {
            // Logging must never stop the update
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

    protected bool AsBool(object value)
    {
        if (value == null || value == DBNull.Value) return false;
        if (value is bool) return (bool)value;
        string s = Convert.ToString(value).Trim();
        if (s == "1" || s.Equals("true", StringComparison.OrdinalIgnoreCase)) return true;
        long n;
        return long.TryParse(s, out n) && n != 0;
    }

    private static decimal ToDecimal(string value)
    {
        decimal d;
        return decimal.TryParse((value ?? "").Replace(",", "").Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out d) ? d : 0m;
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