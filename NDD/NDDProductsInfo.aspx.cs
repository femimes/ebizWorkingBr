using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using NETClass;

/// <summary>
/// Products of one approved NDD biller. Opened from NDDBiller.aspx?p=billerId&amp;q=status&amp;r=name.
/// Adding a product calls NIBSS CreateProduct, then saves it with SpNDDProductHistadd.
/// Plain ASP.NET controls (no DevExpress).
/// </summary>
public partial class NDD_NDDProductsInfo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Only approved billers (status 3) can have products
            if (Request.QueryString["q"] != "3" || string.IsNullOrEmpty(Request.QueryString["p"]))
            {
                Response.Redirect("NDDBiller.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }
            hidBillerId.Value = Request.QueryString["p"];
            hidBillerName.Value = Request.QueryString["r"] ?? "";
        }
        litBillerId.Text = HttpUtility.HtmlEncode(hidBillerId.Value);
        litBillerName.Text = HttpUtility.HtmlEncode(hidBillerName.Value == "" ? "biller" : hidBillerName.Value);
    }

    // =====================================================================
    // Add: create on NIBSS, then save locally
    // =====================================================================
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (!IsValid) return;
        string user = CurrentUser();
        string billerId = hidBillerId.Value;
        string name = hidBillerName.Value;
        string productName = txtProductName.Text.Trim();

        NddResult r;
        try
        {
            r = PostNdd("CreateProduct", new { billerId = billerId, productName = productName });
        }
        catch (Exception ex)
        {
            ShowError("NIBSS could not be reached: " + ex.Message);
            return;
        }

        string info = string.Format("Biller/BillerId/Product Name:- {0}/{1}/{2}", name, billerId, productName);
        Audit("044", string.Format("New NDDBiller Product Profile  Modification on NDDBiller Name- {0};  Message:{1}; AddedBy:{2}", info, r.Raw, user), user);

        if (r.Code != "00")
        {
            ShowError(string.Format("NIBSS did not create the product: {0} ({1}).", r.Text == "" ? "no reason given" : r.Text, r.Code == "" ? "no code" : r.Code));
            return;
        }

        string productId = Convert.ToString(Dig(r.Json, "data", "data", "id"));
        try
        {
            Exec("SpNDDProductHistadd",
                P("@user_id", user),
                P("@billerId", billerId),
                P("@name", name),
                P("@productId", productId),
                P("@productName", productName),
                P("@status", "3"));
            txtProductName.Text = "";
            ShowSuccess(string.Format("{0} was created on NIBSS (product ID {1}).", productName, productId));
            grid.DataBind();
        }
        catch (Exception ex)
        {
            ShowError(string.Format("NIBSS created the product (ID {0}) but it was not saved here: {1}", productId, ex.Message));
        }
    }

    // =====================================================================
    // Grid: delete
    // =====================================================================
    protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "DeleteRow") return;
        string user = CurrentUser();
        string seqNo = Convert.ToString(e.CommandArgument);
        try
        {
            Exec("SpNDDProductDelete", P("@SeqNo", seqNo));
            Audit("004", string.Format("Application NDD Product Deleting Deleteting on - {0}; ", seqNo), user);
            ShowSuccess("The product was deleted.");
            grid.DataBind();
        }
        catch (Exception ex)
        {
            ShowError("The product was not deleted: " + ex.Message);
        }
    }

    protected void sqlProducts_Selected(object sender, SqlDataSourceStatusEventArgs e)
    {
        litTotal.Text = e.Exception == null ? string.Format("{0:N0} total", e.AffectedRows) : "";
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
