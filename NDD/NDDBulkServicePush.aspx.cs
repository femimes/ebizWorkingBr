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
/// Pushes the whole bulk NDD queue: every waiting e-mandate (SpNDDeMandateGetInfo status 3)
/// gets a NIBSS status check (eMandateStatusTSQ); "Bank Approved" mandates are sent for recovery
/// (eMandateApproval) and updated with SpGetNDDeMandateUpd / spNDDMandateRptstatus.
/// The work is done by App_Code/NDDMandateRecovery.vb, the same code the NDD mandate status page
/// and the automatic run use, so a page push and the scheduler never process a mandate twice at once.
/// </summary>
public partial class NDD_NDDBulkServicePush : System.Web.UI.Page
{
    private NDDMandateRecovery.RunSummary _thisRun;

    protected void btnPush_Click(object sender, EventArgs e)
    {
        string user = CurrentUser();
        Server.ScriptTimeout = 3600;
        try
        {
            _thisRun = NDDMandateRecovery.RunAll(user, 0);   // 0 = no limit: the whole queue, as before
        }
        catch (Exception ex)
        {
            ShowError("The queue was not pushed: " + ex.Message);
            return;
        }

        if (_thisRun.Skipped) { ShowError("Not run: " + _thisRun.SkipReason + "."); return; }

        string text = string.Format("Done. {0} checked, {1} sent for recovery.", _thisRun.Checked, _thisRun.Recovered);
        if (_thisRun.NotRecovered.Count > 0)
            text += string.Format(" {0} not recovered: {1}.", _thisRun.NotRecovered.Count, string.Join("; ", _thisRun.NotRecovered.ToArray()));
        litResult.Text = HttpUtility.HtmlEncode(text);
        litError.Text = _thisRun.Failed.Count == 0 ? "" :
            HttpUtility.HtmlEncode(_thisRun.Failed.Count + " could not be checked: " + string.Join("; ", _thisRun.Failed.ToArray()));
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        bool running = NDDMandateRecovery.IsRunning;
        litState.Text = running ? "Running" : "Idle";
        spanState.Attributes["class"] = running ? "pill on" : "pill off";
        btnPush.Enabled = !running;

        try
        {
            txtWaiting.Text = Query(MainDb, "EXEC SpNDDeMandateGetInfo @status", P("@status", 3)).Rows.Count.ToString("N0");
        }
        catch (Exception ex)
        {
            txtWaiting.Text = "?";
            ShowError("The waiting mandates could not be counted: " + ex.Message);
        }

        NDDMandateRecovery.RunSummary last = _thisRun ?? NDDMandateRecovery.LastJobRun;
        if (last == null || last.Skipped)
        {
            txtChecked.Text = txtRecovered.Text = txtFailed.Text = "–";
            litLast.Text = running ? "A run is in progress now." : "No run recorded since the site last started.";
            return;
        }
        txtChecked.Text = last.Checked.ToString("N0");
        txtRecovered.Text = last.Recovered.ToString("N0");
        txtFailed.Text = last.Failed.Count.ToString("N0");
        litLast.Text = HttpUtility.HtmlEncode(string.Format("Last run {0:dd MMM yyyy HH:mm} by {1}, took {2:N0}s.",
            last.Started, last.Trigger, (last.Finished - last.Started).TotalSeconds));
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
