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
/// Popup opened from NDDQueryservice.aspx: NDDUpdateMandStatus.aspx?q=2|3&amp;p=mandateCode.
/// q=2 suspends and q=3 cancels the mandate on NIBSS (eMandateStatus), then sets the local
/// status with SpNDDeMandateStatusUpd (98 = suspended, 99 = cancelled).
/// </summary>
public partial class NDD_NDDUpdateMandStatus : System.Web.UI.Page
{
    private string Action { get { return Request.QueryString["q"] ?? ""; } }
    private string MandateCode { get { return Request.QueryString["p"] ?? ""; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        bool suspend = Action == "2", cancel = Action == "3";
        btnMandstatus.Visible = suspend || cancel;
        btnMandstatus.Text = suspend ? "Suspend mandate" : "Cancel mandate";
        btnMandstatus.OnClientClick = string.Format("return confirm('{0} mandate {1} on NIBSS? The payer will no longer be debited{2}.');",
            suspend ? "Suspend" : "Cancel", HttpUtility.JavaScriptStringEncode(MandateCode), suspend ? " until it is reactivated" : "");
        litTitle.Text = suspend ? "Suspend NDD mandate" : cancel ? "Cancel NDD mandate" : "NDD mandate";
        litIntro.Text = suspend
            ? "Check the details, then suspend the mandate. NIBSS stops debiting the payer until it is reactivated."
            : cancel
                ? "Check the details, then cancel the mandate. NIBSS stops debiting the payer permanently."
                : "Mandate details.";
    }

    protected void fvMandate_DataBound(object sender, EventArgs e)
    {
        DataRowView row = fvMandate.DataItem as DataRowView;
        if (row == null) { btnMandstatus.Visible = false; lblStatus.Visible = false; return; }
        string status = Col(row.Row, "statusDesc");
        if (status == "") status = Col(row.Row, "status");
        lblStatus.Text = HttpUtility.HtmlEncode(status);
        lblStatus.CssClass = StatusCss(status);
        lblStatus.Visible = status != "";
    }

    protected void btnMandstatus_Click(object sender, EventArgs e)
    {
        if (Action != "2" && Action != "3") return;
        string user = CurrentUser();

        DataView view = sqlMandate.Select(DataSourceSelectArguments.Empty) as DataView;
        if (view == null || view.Count == 0) { ShowError("Mandate " + MandateCode + " was not found."); return; }
        DataRow mandate = view[0].Row;

        // Same request shape as before; the mandate code is now sent (it used to be blank)
        string mandateUpdateRequests = new JavaScriptSerializer().Serialize(new
        {
            MandateCode = MandateCode,
            BillerID = 1,
            AccountNumber = Col(mandate, "payerAccountNumber"),
            MandateStatus = Action
        });

        NddResult r;
        try
        {
            r = PostNdd("eMandateStatus", new { mandateUpdateRequests = mandateUpdateRequests });
        }
        catch (Exception ex)
        {
            ShowError("NIBSS could not be reached: " + ex.Message);
            return;
        }

        string verb = Action == "2" ? "suspended" : "cancelled";
        if (r.Code != "00")
        {
            ShowError(string.Format("NIBSS did not update the mandate: {0} ({1}).", r.Text == "" ? "no reason given" : r.Text, r.Code == "" ? "no code" : r.Code));
            Audit("044", string.Format("NDDMandate status change ({0}) refused on Mandate- {1}; response {2} {3}", verb, MandateCode, r.Code, r.Text), user);
            return;
        }

        try
        {
            Exec("SpNDDeMandateStatusUpd",
                P("@SeqNo", Col(mandate, "SeqNo")),
                P("@status", Action == "2" ? 98 : 99));
            Audit("044", string.Format("NDDMandate {0} on Mandate- {1}; SeqNo {2}", verb, MandateCode, Col(mandate, "SeqNo")), user);
            ShowSuccess(string.Format("Mandate {0} was {1}.", MandateCode, verb));
            btnMandstatus.Visible = false;
            fvMandate.DataBind();
        }
        catch (Exception ex)
        {
            ShowError(string.Format("NIBSS {0} the mandate, but the local status was not updated: {1}", verb, ex.Message));
        }
    }

    // ----- display helpers for the details list -----
    protected static string H(object value)
    {
        string s = Convert.ToString(value);
        return s == "" ? "&ndash;" : HttpUtility.HtmlEncode(s);
    }

    protected static string Money(object value)
    {
        decimal d;
        return decimal.TryParse(Convert.ToString(value), out d) ? "NGN " + d.ToString("N2") : H(value);
    }

    protected static string Day(object value)
    {
        return value is DateTime ? ((DateTime)value).ToString("dd MMM yyyy") : H(value);
    }

    protected static string Stamp(object value)
    {
        return value is DateTime ? ((DateTime)value).ToString("dd MMM yyyy HH:mm") : H(value);
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
