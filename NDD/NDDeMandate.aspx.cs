using System;
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
using System.Web.UI;
using System.Web.UI.WebControls;
using NETClass;

/// <summary>
/// NDD customer e-mandates: capture, edit (until approved) and delete.
/// Data: SpNDDeMandateGetAll / SpAddNDDeMandate / SpUpdateNDDeMandate / SpNDDeMandateDelete.
/// Account names: core banking for this bank, NIBSS name enquiry (Easypay/nameEnquiry) for others.
/// Plain ASP.NET controls (no DevExpress).
/// </summary>
public partial class NDD_NDDeMandate : System.Web.UI.Page
{
    private const string DateFormat = "yyyy-MM-dd";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadBillers();
            txtStartDate.Text = DateTime.Today.ToString(DateFormat, CultureInfo.InvariantCulture);
            UpdateEndDate();
        }
    }

    /// <summary>Approved billers, shown as "name \u2014 product (RC number)".</summary>
    private void LoadBillers()
    {
        ddlBiller.Items.Clear();
        ddlBiller.Items.Add(new ListItem("Choose a customer", ""));
        DataView view = sqlBillers.Select(DataSourceSelectArguments.Empty) as DataView;
        if (view == null) return;
        foreach (DataRowView r in view)
        {
            string id = Convert.ToString(r["billerId"]);
            if (id == "" || ddlBiller.Items.FindByValue(id) != null) continue;
            string text = Col(r.Row, "name");
            string product = Col(r.Row, "productName");
            string rc = Col(r.Row, "rcNumber");
            if (product != "") text += " \u2014 " + product;
            if (rc != "") text += " (" + rc + ")";
            ddlBiller.Items.Add(new ListItem(text, id));
        }
    }

    // =====================================================================
    // Live lookups while the officer fills the form
    // =====================================================================
    protected void txtCustomerAccount_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string name = LocalAccountName(txtCustomerAccount.Text);
            litCustomerAccountName.Text = HttpUtility.HtmlEncode(name);
            if (name == "" && txtCustomerAccount.Text.Trim() != "") ShowError("Customer account " + txtCustomerAccount.Text.Trim() + " was not found in core banking.");
            else ShowSuccess("");
        }
        catch (Exception ex)
        {
            ShowError("The customer account could not be checked: " + ex.Message);
        }
    }

    protected void PayerAccount_Changed(object sender, EventArgs e)
    {
        litPayerAccountName.Text = "";
        if (ddlBank.SelectedValue == "" || txtPayerAccount.Text.Trim() == "") return;
        string name = AccountName(txtPayerAccount.Text, ddlBank.SelectedValue);
        litPayerAccountName.Text = HttpUtility.HtmlEncode(name);
        if (name == "") ShowError("The payer account name could not be confirmed. Check the bank and account number.");
        else ShowSuccess("");
    }

    protected void Period_Changed(object sender, EventArgs e)
    {
        UpdateEndDate();
    }

    private void UpdateEndDate()
    {
        DateTime start; int months;
        txtEndDate.Text = TryDate(txtStartDate.Text, out start) && TryMonths(txtFrequency.Text, out months)
            ? EndDate(start, months).ToString(DateFormat, CultureInfo.InvariantCulture)
            : "";
    }

    private static DateTime EndDate(DateTime start, int months)
    {
        return start.AddMonths(months).AddDays(-1);
    }

    // =====================================================================
    // Save (new or edit)
    // =====================================================================
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!IsValid) return;
        string user = CurrentUser();
        bool editing = hidSeqNo.Value != "";

        string billerId = ddlBiller.SelectedValue;
        string customerAccount = txtCustomerAccount.Text.Trim();
        string payerName = txtPayerName.Text.Trim();
        string phone = txtPhone.Text.Trim();
        string email = txtEmail.Text.Trim();
        string address = txtAddress.Text.Trim();
        string bankCode = ddlBank.SelectedValue;
        string payerAccount = txtPayerAccount.Text.Trim();
        string narration = txtNarration.Text.Trim();
        int mandateType = chkFixed.Checked ? 1 : 2;   // 1 = fixed, 2 = variable

        decimal amount; int months; DateTime start;
        if (!decimal.TryParse(txtAmount.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out amount) || amount <= 0) { ShowError("Enter an amount greater than zero."); return; }
        if (!TryMonths(txtFrequency.Text, out months)) { ShowError("Frequency must be a whole number of months from 1 to 120."); return; }
        if (!TryDate(txtStartDate.Text, out start)) { ShowError("Enter a valid start date."); return; }
        DateTime end = EndDate(start, months);

        try
        {
            // Edits are only allowed while the mandate is still pending
            DataRow existing = null;
            if (editing)
            {
                existing = FindRow(hidSeqNo.Value);
                if (existing == null) { ShowError("That mandate no longer exists."); ResetForm(); grid.DataBind(); return; }
                if (IsLocked(Col(existing, "statusDesc"))) { ShowError("Approved or disapproved mandates cannot be edited."); return; }
            }

            // Product of the chosen biller
            string productId = "", productName = "";
            foreach (DataRow p in Query(MainDb, "EXEC SpNDDBillerGetOnly @billerId", P("@billerId", billerId)).Rows)
            {
                productId = Col(p, "productId");
                productName = Col(p, "productName");
            }
            if (productName == "") { ShowError("The chosen customer has no NDD product yet. Create one under NDD billers > Products first."); return; }

            // Re-validate both account names on the server
            string customerAccountName = LocalAccountName(customerAccount);
            if (customerAccountName == "") { ShowError("Customer account " + customerAccount + " was not found in core banking."); return; }
            litCustomerAccountName.Text = HttpUtility.HtmlEncode(customerAccountName);

            string payerAccountName = AccountName(payerAccount, bankCode);
            if (payerAccountName == "") { ShowError("The payer account name could not be confirmed. Check the bank and account number."); return; }
            litPayerAccountName.Text = HttpUtility.HtmlEncode(payerAccountName);

            string subscriberCode = editing ? Col(existing, "subscriberCode") : DateTime.Now.ToString("yyyyMMdd'T'HHmmssffff", CultureInfo.InvariantCulture);

            List<SqlParameter> ps = new List<SqlParameter>();
            if (editing) ps.Add(P("@SeqNo", hidSeqNo.Value));
            ps.Add(P("@billerId", billerId));
            ps.Add(P("@accountNumber", customerAccount));
            ps.Add(P("@accountName", customerAccountName));
            ps.Add(P("@productId", productId));
            ps.Add(P("@payerName", payerName));
            ps.Add(P("@payerPhoneNumber", phone));
            ps.Add(P("@narration", narration));
            ps.Add(P("@frequency", months.ToString(CultureInfo.InvariantCulture)));
            ps.Add(P("@amount", amount.ToString("0.00", CultureInfo.InvariantCulture)));
            ps.Add(P("@payerEmail", email));
            ps.Add(P("@payerAddress", address));
            ps.Add(P("@payerBankCode", bankCode));
            ps.Add(P("@payerAccountName", payerAccountName));
            ps.Add(P("@payerAccountNumber", payerAccount));
            ps.Add(P("@subscriberCode", subscriberCode));
            ps.Add(P("@mandateType", mandateType));
            ps.Add(P("@startDate", start.ToString(DateFormat, CultureInfo.InvariantCulture)));
            ps.Add(P("@endDate", end.ToString(DateFormat, CultureInfo.InvariantCulture)));
            ps.Add(P("@addedby", user));
            Exec(editing ? "SpUpdateNDDeMandate" : "SpAddNDDeMandate", ps.ToArray());

            Audit("044", string.Format("{0} on BillerName- {1}; productName: {2}; rcNumber {3}:email:{4};accountno:{5};accountname:{6};bankcode:{7};Phone:{8}",
                editing ? "New NDDCode eMandate Profile  Modification" : "New NDDCode Biller Profile  Modification",
                payerName, productName, "", email, payerAccount, payerAccountName, editing ? ddlBank.SelectedItem.Text : Cfg("EazyBankHostCode"), phone), user);

            ResetForm();
            ShowSuccess(editing
                ? string.Format("The mandate for {0} was updated.", payerName)
                : string.Format("The mandate for {0} was saved. It now goes for approval.", payerName));
            grid.DataBind();
        }
        catch (Exception ex)
        {
            ShowError("The mandate was not saved: " + ex.Message);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ResetForm();
        ShowSuccess("");
    }

    private void ResetForm()
    {
        hidSeqNo.Value = hidSubscriberCode.Value = "";
        litFormTitle.Text = "New e-mandate";
        btnSave.Text = "Save mandate";
        btnCancel.Text = "Clear";
        ddlBiller.ClearSelection();
        ddlBank.ClearSelection();
        txtCustomerAccount.Text = txtPayerName.Text = txtPhone.Text = txtEmail.Text = txtAddress.Text = "";
        txtPayerAccount.Text = txtNarration.Text = txtAmount.Text = "";
        txtFrequency.Text = "1";
        txtStartDate.Text = DateTime.Today.ToString(DateFormat, CultureInfo.InvariantCulture);
        chkFixed.Checked = false;
        litCustomerAccountName.Text = litPayerAccountName.Text = "";
        UpdateEndDate();
    }

    // =====================================================================
    // Grid: edit and delete
    // =====================================================================
    protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "EditRow" && e.CommandName != "DeleteRow") return;
        string seqNo = Convert.ToString(e.CommandArgument);
        DataRow row = FindRow(seqNo);
        if (row == null) { ShowError("That mandate no longer exists. The list has been refreshed."); grid.DataBind(); return; }

        if (e.CommandName == "EditRow") { LoadForEdit(row); return; }

        if (Col(row, "statusDesc") == "Approved") { ShowError("Approved mandates cannot be deleted."); return; }
        string user = CurrentUser();
        try
        {
            Exec("SpNDDeMandateDelete", P("@SeqNo", seqNo));
            Audit("004", string.Format("Application eMandate Deleteting on - {0};Payer name:{1}; Addres:{2};billerId:{3};narration:{4} ",
                seqNo, Col(row, "payerName"), Col(row, "payerAddress"), Col(row, "billerId"), Col(row, "narration")), user);
            if (hidSeqNo.Value == seqNo) ResetForm();
            ShowSuccess("The mandate for " + Col(row, "payerName") + " was deleted.");
            grid.DataBind();
        }
        catch (Exception ex)
        {
            ShowError("The mandate was not deleted: " + ex.Message);
        }
    }

    private void LoadForEdit(DataRow row)
    {
        if (IsLocked(Col(row, "statusDesc"))) { ShowError("Approved or disapproved mandates cannot be edited."); return; }

        hidSeqNo.Value = Col(row, "SeqNo");
        hidSubscriberCode.Value = Col(row, "subscriberCode");
        litFormTitle.Text = "Edit e-mandate for " + HttpUtility.HtmlEncode(Col(row, "payerName"));
        btnSave.Text = "Save changes";
        btnCancel.Text = "Cancel edit";

        Select(ddlBiller, Col(row, "billerId"));
        txtCustomerAccount.Text = Col(row, "accountNumber");
        litCustomerAccountName.Text = HttpUtility.HtmlEncode(Col(row, "accountName"));
        txtPayerName.Text = Col(row, "payerName");
        txtPhone.Text = Col(row, "payerPhoneNumber");
        txtEmail.Text = Col(row, "payerEmail");
        txtAddress.Text = Col(row, "payerAddress");
        txtNarration.Text = Col(row, "narration");

        ddlBank.ClearSelection();
        string bankCode = Col(row, "payerBankCode");
        if (bankCode != "" && ddlBank.Items.FindByValue(bankCode) != null) ddlBank.SelectedValue = bankCode;
        else
        {
            ListItem byName = ddlBank.Items.FindByText(Col(row, "bank_name"));
            if (byName != null) byName.Selected = true;
        }
        txtPayerAccount.Text = Col(row, "payerAccountNumber");
        litPayerAccountName.Text = HttpUtility.HtmlEncode(Col(row, "payerAccountName"));

        decimal amount;
        txtAmount.Text = decimal.TryParse(Col(row, "amount"), out amount) ? amount.ToString("0.00", CultureInfo.InvariantCulture) : "";
        decimal freq;
        txtFrequency.Text = decimal.TryParse(Col(row, "frequency"), out freq) ? ((int)freq).ToString(CultureInfo.InvariantCulture) : "1";
        DateTime start;
        txtStartDate.Text = row.Table.Columns.Contains("startDate") && row["startDate"] is DateTime
            ? ((DateTime)row["startDate"]).ToString(DateFormat, CultureInfo.InvariantCulture)
            : (DateTime.TryParse(Col(row, "startDate"), out start) ? start.ToString(DateFormat, CultureInfo.InvariantCulture) : "");
        chkFixed.Checked = IsFixed(row.Table.Columns.Contains("mandateTypeCon") ? row["mandateTypeCon"] : null);
        UpdateEndDate();

        ShowSuccess("Editing the mandate for " + Col(row, "payerName") + ". Change the details and click Save changes.");
    }

    protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow) return;
        DataRowView data = e.Row.DataItem as DataRowView;
        if (data == null) return;
        string status = Col(data.Row, "statusDesc");
        Control edit = e.Row.FindControl("lnkEdit"), del = e.Row.FindControl("lnkDelete");
        if (edit != null) edit.Visible = !IsLocked(status);
        if (del != null) del.Visible = status != "Approved";
        if (Col(data.Row, "SeqNo") == hidSeqNo.Value && hidSeqNo.Value != "") e.Row.CssClass = "grid-edit";
    }

    protected void sqlMandates_Selected(object sender, SqlDataSourceStatusEventArgs e)
    {
        litTotal.Text = e.Exception == null ? string.Format("{0:N0} total", e.AffectedRows) : "";
    }

    // =====================================================================
    // Small helpers
    // =====================================================================
    private DataRow FindRow(string seqNo)
    {
        DataView view = sqlMandates.Select(DataSourceSelectArguments.Empty) as DataView;
        if (view == null) return null;
        foreach (DataRowView r in view)
        {
            if (Convert.ToString(r["SeqNo"]) == seqNo) return r.Row;
        }
        return null;
    }

    private static bool IsLocked(string status)
    {
        return status == "Approved" || status == "DisApproved";
    }

    protected static bool IsFixed(object value)
    {
        if (value == null || value == DBNull.Value) return false;
        if (value is bool) return (bool)value;
        string s = Convert.ToString(value).Trim();
        return s == "1" || s.Equals("true", StringComparison.OrdinalIgnoreCase);
    }

    private static void Select(DropDownList list, string value)
    {
        list.ClearSelection();
        ListItem item = list.Items.FindByValue(value);
        if (item != null) item.Selected = true;
    }

    private static bool TryDate(string value, out DateTime date)
    {
        return DateTime.TryParseExact((value ?? "").Trim(), DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
    }

    private static bool TryMonths(string value, out int months)
    {
        return int.TryParse((value ?? "").Trim(), out months) && months >= 1 && months <= 120;
    }

    // =====================================================================
    // Shared NDD helpers (same code in every NDD page)
    // =====================================================================
    private const string MainDb = "EazyConnect";
    private const string CoreDb = "EazyConnectSource";

    /// <summary>Signed-in officer: the LogonUserID cookie is "user?branch".</summary>
    private string CurrentUser()
    {
        HttpCookie cookie = Request.Cookies["LogonUserID"];
        string luser = cookie != null && !string.IsNullOrEmpty(cookie.Value) ? cookie.Value : (User.Identity.Name ?? "");
        int split = luser.LastIndexOf('?');
        string user = split >= 0 ? luser.Substring(0, split) : luser;
        return user == "" ? "NDDService" : user;
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

    private static void Audit(string code, string message, string actor)
    {
        Classes.RecordOp(string.Format("Sproc_AddAuditLog '{0}','{1}','{2}',''", code, Sql(message), Sql(actor)));
    }

    /// <summary>Runs a stored procedure on the NDD database.</summary>
    private static void Exec(string procedure, params SqlParameter[] parameters)
    {
        using (SqlConnection conn = new SqlConnection(Cfg(MainDb)))
        using (SqlCommand cmd = new SqlCommand(procedure, conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(parameters);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }

    /// <summary>Runs a query (parameters, never string concatenation) and returns the first table.</summary>
    private static DataTable Query(string connectionName, string sql, params SqlParameter[] parameters)
    {
        DataTable table = new DataTable();
        using (SqlConnection conn = new SqlConnection(Cfg(connectionName)))
        using (SqlCommand cmd = new SqlCommand(sql, conn))
        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
        {
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
        return row != null && row.Table.Columns.Contains(column) ? Convert.ToString(row[column]) : "";
    }

    /// <summary>Account name from the bank's own core banking system.</summary>
    private static string LocalAccountName(string accountNo)
    {
        accountNo = (accountNo ?? "").Trim();
        if (accountNo == "") return "";
        DataTable t = Query(CoreDb, "EXEC SpAcctMasterSelectOne @accountNo", P("@accountNo", accountNo));
        return t.Rows.Count == 0 ? "" : Col(t.Rows[t.Rows.Count - 1], "AccountDesc");
    }

    /// <summary>Account name for any bank: core banking for this bank, NIBSS name enquiry for others.</summary>
    private static string AccountName(string accountNo, string bankCode)
    {
        accountNo = (accountNo ?? "").Trim();
        if (accountNo == "" || string.IsNullOrEmpty(bankCode)) return "";
        if (bankCode == Cfg("EazyBankHostCode")) return LocalAccountName(accountNo);
        try
        {
            NddResult r = PostNdd("Easypay/nameEnquiry", new { AccountNo = accountNo, BankSchemeCode = bankCode });
            return r.Code == "00" ? Convert.ToString(Dig(r.Json, "data", "accountName")) : "";
        }
        catch (Exception ex)
        {
            NDDMandateRecovery.AddtoLogFile(ex.Message, "name enquiry failed for " + accountNo);
            return "";
        }
    }

    private sealed class NddResult
    {
        public string Code = "";
        public string Text = "";
        public string Raw = "";
        public Dictionary<string, object> Json;
    }

    /// <summary>POST to the NIBSS NDD gateway (NDDCodeHostUrl) with the configured credentials.</summary>
    private static NddResult PostNdd(string action, object body)
    {
        string url = Cfg("NDDCodeHostUrl").TrimEnd('/') + "/" + action;
        string token = Convert.ToBase64String(Encoding.Default.GetBytes(string.Format("{0}:{1}",
            Classes.DecryptString(Cfg("NDDCodeKeyID")), Classes.DecryptString(Cfg("NDDCodeKey")))));

        JavaScriptSerializer js = new JavaScriptSerializer();
        js.MaxJsonLength = int.MaxValue;
        string payload = js.Serialize(body);
        NDDMandateRecovery.AddtoLogFile(payload, string.Format("Url=>{0} payload request", url));

        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        req.Method = "POST";
        req.ContentType = "application/json";
        req.Timeout = 60000;
        req.Headers.Add("AuthToken", token);
        req.Headers.Add("TransToken", token);
        byte[] bytes = Encoding.UTF8.GetBytes(payload);
        req.ContentLength = bytes.Length;
        using (Stream s = req.GetRequestStream()) { s.Write(bytes, 0, bytes.Length); }

        NddResult r = new NddResult();
        try
        {
            using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
            using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
            {
                r.Raw = reader.ReadToEnd();
            }
        }
        catch (WebException ex)
        {
            // NIBSS explains failures in the error body
            if (ex.Response == null) throw;
            using (StreamReader reader = new StreamReader(ex.Response.GetResponseStream()))
            {
                r.Raw = reader.ReadToEnd();
            }
            if (string.IsNullOrEmpty(r.Raw)) throw;
        }
        NDDMandateRecovery.AddtoLogFile(r.Raw, action + " response");

        r.Json = js.DeserializeObject(r.Raw) as Dictionary<string, object>;
        r.Code = Convert.ToString(Dig(r.Json, "responseCode"));
        r.Text = Convert.ToString(Dig(r.Json, "responseText"));
        return r;
    }

    /// <summary>Walks nested JSON objects: Dig(json, "data", "data", "id").</summary>
    private static object Dig(object node, params string[] path)
    {
        foreach (string key in path)
        {
            Dictionary<string, object> dict = node as Dictionary<string, object>;
            if (dict == null) return null;
            object next = null;
            foreach (KeyValuePair<string, object> kv in dict)
            {
                if (string.Equals(kv.Key, key, StringComparison.OrdinalIgnoreCase)) { next = kv.Value; break; }
            }
            node = next;
        }
        return node;
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

    protected static string StatusCss(object status)
    {
        string s = Convert.ToString(status);
        if (s.IndexOf("Disapprov", StringComparison.OrdinalIgnoreCase) >= 0 || s.IndexOf("Reject", StringComparison.OrdinalIgnoreCase) >= 0) return "pill bad";
        if (s.IndexOf("Approved", StringComparison.OrdinalIgnoreCase) >= 0) return "pill on";
        return "pill off";
    }
}
