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
using System.Web.UI.WebControls;
using NETClass;

public partial class Approvals_ApprNDDBiller : System.Web.UI.Page
{
    public string gUser;
    public string luser;
    public string StrLocation = "";
    public string struser = "";

    private const string LogPrefix = "eNDDLog_";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadCurrentUser();
            txtUserID.Text = struser;
            txtlocation.Text = StrLocation;
            // The old grid grouped by this column; sorting keeps the groups together
            ctrlGrid.Sort("addedby", SortDirection.Ascending);
        }
    }

    protected void SqlDataSource1_Selected(object sender, SqlDataSourceStatusEventArgs e)
    {
        litTotal.Text = e.Exception == null && e.AffectedRows > 0 ? "&middot; " + e.AffectedRows.ToString("N0") + " waiting" : "";
    }

    // =====================================================================
    // Approve: create the biller on NIBSS, then save the biller ID
    // =====================================================================
    protected void BtnApprove_Click(object sender, EventArgs e)
    {
        if (!LoadCurrentUser()) { ShowError("Your session has expired. Sign in again."); return; }
        int done = 0;
        List<string> failed = new List<string>();

        foreach (GridViewRow row in SelectedRows())
        {
            string label = Val(row, "name");
            try
            {
                ApiResult r = PostNdd("CreateBiller", new
                {
                    rcNumber = Val(row, "rcNumber"),
                    name = Val(row, "name"),
                    address = Val(row, "address"),
                    email = Val(row, "email"),
                    phoneNumber = Val(row, "phoneNumber"),
                    accountNumber = Val(row, "accountNumber"),
                    accountName = Val(row, "accountName"),
                    bankCode = Val(row, "bankCode"),
                    mandateStatusNotificationUrl = Val(row, "mandateStatusNotificationUrl")
                }, false);

                Audit("910", string.Format("Title: NDDCode Biller Profile Approval;  PostedBy ID:{0}; ResponseText -{1}; ApprovedBy: {2}",
                    Val(row, "addedby"), r.Raw, "Approved By- " + gUser));

                if (r.Code != "00") { failed.Add(label + ": " + Failure(r)); continue; }

                string billerId = Convert.ToString(Dig(r.Json, "data", "data", "id"));
                AddtoLogFile(billerId, "Biller Id");
                ExecPositional("SpUpdAppNDDBiller", gUser, Val(row, "SeqNo"), billerId, "3", r.Text);
                done++;
            }
            catch (Exception ex)
            {
                failed.Add(label + ": " + ex.Message);
            }
        }

        ctrlGrid.DataBind();
        Report(done, failed, "approved");
    }

    // =====================================================================
    // Disapprove: status 4 with the reason
    // =====================================================================
    protected void btnDisApprove_Click(object sender, EventArgs e)
    {
        if (!LoadCurrentUser()) { ShowError("Your session has expired. Sign in again."); return; }
        int done = 0;
        List<string> failed = new List<string>();

        foreach (GridViewRow row in SelectedRows())
        {
            string label = Val(row, "name");
            try
            {
                string reason = Reason(row);
                if (reason == "") { failed.Add(label + ": reason missing"); continue; }
                ExecPositional("SpUpdAppNDDBiller", gUser, Val(row, "SeqNo"), "", "4", reason);
                Audit("910", string.Format("Title: NDD Biller Profile Approval;  PostedBy ID:{0}; ResponseText -{1}; DisApprovedBy: {2}",
                    Val(row, "addedby"), reason, "Dis Approved By- " + gUser));
                done++;
            }
            catch (Exception ex)
            {
                failed.Add(label + ": " + ex.Message);
            }
        }

        ctrlGrid.DataBind();
        Report(done, failed, "disapproved");
    }

    // =====================================================================
    // Row selection helpers
    // =====================================================================
    private List<GridViewRow> SelectedRows()
    {
        List<GridViewRow> rows = new List<GridViewRow>();
        foreach (GridViewRow row in ctrlGrid.Rows)
        {
            if (row.RowType != DataControlRowType.DataRow) continue;
            CheckBox chk = row.FindControl("chkSelect") as CheckBox;
            if (chk != null && chk.Checked) rows.Add(row);
        }
        return rows;
    }

    private object Raw(GridViewRow row, string field)
    {
        return ctrlGrid.DataKeys[row.RowIndex][field];
    }

    private string Val(GridViewRow row, string field)
    {
        object v = Raw(row, field);
        return v == null || v == DBNull.Value ? "" : Convert.ToString(v).Trim();
    }

    private static string Reason(GridViewRow row)
    {
        TextBox box = row.FindControl("tatReason") as TextBox;
        return box == null ? "" : box.Text.Trim();
    }

    private void Report(int done, List<string> failed, string verb)
    {
        if (done == 0 && failed.Count == 0)
        {
            ShowError("Select at least one row first.");
            return;
        }
        string ok = done > 0 ? string.Format("{0} {1} {2}.", done, done == 1 ? "request" : "requests", verb) : "";
        if (failed.Count == 0)
        {
            ShowSuccess(ok);
        }
        else
        {
            litResult.Text = HttpUtility.HtmlEncode(ok);
            Literal1.Text = HttpUtility.HtmlEncode(string.Format("{0} not {1}: {2}", failed.Count, verb, string.Join("; ", failed.ToArray())));
        }
    }

    // =====================================================================
    // Common helpers
    // =====================================================================
    private bool LoadCurrentUser()
    {
        HttpCookie cookie = Request.Cookies["LogonUserID"];
        luser = cookie != null && !string.IsNullOrEmpty(cookie.Value) ? cookie.Value : (User.Identity.Name ?? "");
        int split = luser.LastIndexOf('?');
        struser = split >= 0 ? luser.Substring(0, split) : luser;
        StrLocation = split >= 0 ? luser.Substring(split + 1) : "";
        gUser = struser;
        return gUser != "";
    }

    private void Audit(string code, string message)
    {
        Classes.RecordOp(string.Format("Sproc_AddAuditLog '{0}','{1}','{2}',''", code, Sql(message), Sql(gUser)));
    }

    // Runs a procedure whose parameters are passed by position (names not known)
    private static void ExecPositional(string proc, params object[] values)
    {
        using (SqlConnection conn = new SqlConnection(Cfg("EazyConnect")))
        using (SqlCommand cmd = new SqlCommand())
        {
            List<string> names = new List<string>();
            for (int i = 0; i < values.Length; i++)
            {
                names.Add("@p" + i);
                cmd.Parameters.Add(new SqlParameter("@p" + i, values[i] ?? DBNull.Value));
            }
            cmd.Connection = conn;
            cmd.CommandText = "EXEC " + proc + " " + string.Join(",", names.ToArray());
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }

    private void ShowSuccess(string message)
    {
        litResult.Text = HttpUtility.HtmlEncode(message);
        Literal1.Text = "";
    }

    private void ShowError(string message)
    {
        Literal1.Text = HttpUtility.HtmlEncode(message);
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

    // =====================================================================
    // NIBSS NDD API
    // =====================================================================
    private sealed class ApiResult
    {
        public string Code = "";
        public string Text = "";
        public string Raw = "";
        public Dictionary<string, object> Json;
    }

    private static ApiResult PostNdd(string action, object body, bool sendTransToken)
    {
        string url = Cfg("NDDCodeHostUrl").TrimEnd('/') + "/" + action;
        string token = Convert.ToBase64String(Encoding.Default.GetBytes(string.Format("{0}:{1}",
            Classes.DecryptString(Cfg("NDDCodeKeyID")), Classes.DecryptString(Cfg("NDDCodeKey")))));

        string payload = new JavaScriptSerializer().Serialize(body);
        AddtoLogFile(payload, string.Format("Url=>{0} payload request", url));

        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        req.Method = "POST";
        req.ContentType = "application/json";
        req.Timeout = 60000;
        req.Headers.Add("AuthToken", token);
        if (sendTransToken) req.Headers.Add("TransToken", token);

        byte[] bytes = Encoding.UTF8.GetBytes(payload);
        req.ContentLength = bytes.Length;
        using (Stream s = req.GetRequestStream()) s.Write(bytes, 0, bytes.Length);

        ApiResult r = new ApiResult();
        try
        {
            using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
            using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
                r.Raw = reader.ReadToEnd();
        }
        catch (WebException ex)
        {
            // Keep the error body: NIBSS explains failures in it
            if (ex.Response == null) throw;
            using (StreamReader reader = new StreamReader(ex.Response.GetResponseStream()))
                r.Raw = reader.ReadToEnd();
            if (string.IsNullOrEmpty(r.Raw)) throw;
        }
        AddtoLogFile(r.Raw, action + " response");

        JavaScriptSerializer js = new JavaScriptSerializer();
        js.MaxJsonLength = int.MaxValue;
        r.Json = js.DeserializeObject(r.Raw) as Dictionary<string, object>;
        r.Code = Convert.ToString(Dig(r.Json, "responseCode"));
        r.Text = Convert.ToString(Dig(r.Json, "responseText"));
        return r;
    }

    // Walk nested JSON objects: Dig(json, "data", "data", "id")
    private static object Dig(object node, params string[] path)
    {
        foreach (string key in path)
        {
            Dictionary<string, object> dict = node as Dictionary<string, object>;
            if (dict == null) return null;
            object next = null;
            foreach (KeyValuePair<string, object> kv in dict)
                if (string.Equals(kv.Key, key, StringComparison.OrdinalIgnoreCase)) { next = kv.Value; break; }
            node = next;
        }
        return node;
    }

    private static string Failure(ApiResult r)
    {
        return string.Format("{0} ({1})", r.Text == "" ? "no reason given" : r.Text, r.Code == "" ? "no code" : r.Code);
    }

    public static void AddtoLogFile(string Message, string WebPage)
    {
        try
        {
            string dir = HttpContext.Current.Server.MapPath("~/log/");
            Directory.CreateDirectory(dir);
            string path = Path.Combine(dir, LogPrefix + DateTime.Now.ToString("dd-MM-yyyy") + ".txt");
            File.AppendAllText(path, string.Format("——————-START————-{0}\r\nSource :{1}\r\n{2}\r\n——————-END————-{0}\r\n", DateTime.Now, WebPage, Message));
        }
        catch
        {
            // Logging must never stop an approval
        }
    }
}