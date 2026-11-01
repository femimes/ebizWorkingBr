using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Xml;
using NETClass;
using WebReference;

public partial class eOperations_FTBulkConfirmByTokens : System.Web.UI.Page
{
    public string gUser = "";
    public string luser = "";
    public string StrLocation = "";
    public string struser = "";

    private static readonly object CertLock = new object();
    private static bool _certCallbackAdded;

    private string BatchRef
    {
        get { return Request.QueryString["a"] ?? ""; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            lblBatchrefNo.Text = HttpUtility.HtmlEncode(BatchRef);
    }

    protected void ctrlGrid_DataBound(object sender, EventArgs e)
    {
        // Show count and total of the batch above the list
        decimal total = 0; int count = 0;
        DataView view = SqlDataSource2.Select(DataSourceSelectArguments.Empty) as DataView;
        if (view != null)
        {
            foreach (DataRowView r in view)
            {
                count++;
                decimal d;
                if (decimal.TryParse(Convert.ToString(r["Amount"]), NumberStyles.Any, CultureInfo.InvariantCulture, out d)) total += d;
            }
        }
        litTotals.Text = string.Format("{0:N0} &middot; {1:N2}", count, total);
    }

    // =====================================================================
    // OTP check
    // =====================================================================
    /// <summary>Checks the OTP for this batch. Returns the response code ("01" means valid) and shows its message.</summary>
    public string validateTokens(string tokens, string SeqID)
    {
        using (SqlConnection conn = new SqlConnection(Cfg("EazyConnect")))
        using (SqlCommand cmd = new SqlCommand("SpNapsCheckTokenValidility", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@SeqID", (SeqID ?? "").Replace("'", "")));
            cmd.Parameters.Add(new SqlParameter("@tokens", tokens));
            SqlParameter code = cmd.Parameters.Add(new SqlParameter("@responseCode", SqlDbType.VarChar, 50));
            code.Direction = ParameterDirection.Output;
            SqlParameter msg = cmd.Parameters.Add(new SqlParameter("@responseMsg", SqlDbType.VarChar, 300));
            msg.Direction = ParameterDirection.Output;

            conn.Open();
            cmd.ExecuteNonQuery();

            string responseCode = Convert.ToString(code.Value);
            if (responseCode != "01")
                Literal1.Text = HttpUtility.HtmlEncode(Convert.ToString(msg.Value));
            return responseCode;
        }
    }

    // =====================================================================
    // Submit
    // =====================================================================
    protected void ASPxButton1_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;

        string otp = txtOTP.Text.Trim();
        litResult.Text = "";
        Literal1.Text = "";

        try
        {
            if (validateTokens(otp, BatchRef) != "01")
                return;

            LoadCurrentUser();

            // ---- Batch records ----
            DataTable records = new DataTable("PaymentRecord");
            using (SqlConnection conn = new SqlConnection(Cfg("EazyConnect")))
            using (SqlDataAdapter adapter = new SqlDataAdapter(string.Format("SpNapsExtTransNAPsHistByRefConfirm '{0}'", Sql(BatchRef)), conn))
            {
                adapter.Fill(records);
            }

            // ---- Client settings (read once) ----
            DataRow prefs = Classes.GetDataset("SELECT * FROM ParamPreferences").Tables[0].Rows[0];
            string clientId = Convert.ToString(prefs["ClientNapsId"]);
            string debitAccount = Convert.ToString(prefs["NapsDebitAccount"]);
            string debitSortCode = Convert.ToString(prefs["NapsSortCode"]);

            string scheduleId = NewScheduleId(otp);

            // ---- Payment schedule XML ----
            string scheduleXml = BuildPaymentSchedule("0001", scheduleId, debitSortCode, debitAccount, clientId, records);

            AllowNapsCertificate();
            PitchControlCenterWithXML service = new PitchControlCenterWithXML();
            string uploadResponse = service.uploadPaymentSchedule(scheduleXml);
            AddtoLogFile(uploadResponse, Page.UniqueID);

            // ---- Status ----
            string statusRequest = string.Format(
                "<?xml version=\"1.0\" encoding=\"UTF-8\" ?><StatusRequest><Header><ScheduleId>{0}</ScheduleId><ClientId>{1}</ClientId></Header><HashValue></HashValue></StatusRequest>",
                SecurityElement(scheduleId), SecurityElement(clientId));
            string statusResponse = new PitchControlCenterWithXML().getPaymentScheduleStatus(statusRequest);
            AddtoLogFile(statusResponse, Page.UniqueID);

            DataSet status = new DataSet();
            using (StringReader reader = new StringReader(statusResponse))
            {
                status.ReadXml(reader);
            }

            int ok = 0, failed = 0;
            string batch = BatchRef;

            if (status.Tables.Contains("PaymentRecord") && status.Tables["PaymentRecord"].Rows.Count > 0)
            {
                // Per-item results: 3 = paid, 10 = failed
                foreach (DataRow item in status.Tables["PaymentRecord"].Rows)
                {
                    string itemStatus = Convert.ToString(item["Status"]);
                    bool success = itemStatus == "00";
                    if (success) ok++; else failed++;

                    Classes.RecordOp(string.Format("SpNapsUpdStatByItemExtTransNAPsHist '{0}','{1}','{2}',{3},'{4}','{5}',{6},'{7}'",
                        Sql(itemStatus), Sql(Col(item, "ErrorReason")), Sql(batch), Amount(Col(item, "Amount")),
                        Sql(Col(item, "AccountNumber")), Sql(Col(item, "SortCode")), success ? 3 : 10, Sql(scheduleId)));
                }
                litResult.Text = HttpUtility.HtmlEncode(string.Format("Schedule {0} submitted: {1} paid, {2} not paid.", scheduleId, ok, failed));
            }
            else if (status.Tables.Contains("Header"))
            {
                // Header only: record the schedule status unless it is already complete
                foreach (DataRow header in status.Tables["Header"].Rows)
                {
                    string headerStatus = Convert.ToString(header["Status"]);
                    if (headerStatus != "00")
                    {
                        Classes.RecordOp(string.Format("SpNapsUpdateStatExtTransNAPsHist '{0}','{1}','{2}','{3}'",
                            Sql(headerStatus), "accepted", Sql(batch), Sql(scheduleId)));
                    }
                    litResult.Text = HttpUtility.HtmlEncode(string.Format("Schedule {0} submitted. Status: {1}.", scheduleId, headerStatus));
                }
            }
            else
            {
                Literal1.Text = HttpUtility.HtmlEncode("The schedule was sent, but the status response could not be read. Check the log before resubmitting.");
            }

            // Stop the batch being sent twice
            pnlOtp.Visible = false;
            ASPxButton1.Visible = false;
            ctrlGrid.DataBind();
        }
        catch (Exception ex)
        {
            AddtoLogFile(ex.ToString(), Page.UniqueID);
            Literal1.Text = HttpUtility.HtmlEncode("The batch was not submitted: " + ex.Message);
        }
    }

    protected void ASPxButton2_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Approvals/ApproveBulkTransfers.aspx", false);
        Context.ApplicationInstance.CompleteRequest();
    }

    // =====================================================================
    // Schedule building
    // =====================================================================
    /// <summary>Schedule id from the OTP and the current time, worked out by SQL as before (30 characters).</summary>
    private static string NewScheduleId(string otp)
    {
        string sql = string.Format(
            "SELECT '{0}' + '{1}' + replace(convert(varchar(8),getdate(),2),'.','') + replace(convert(varchar(8),getdate(),8),':','') + right(replace(convert(varchar(8),getdate(),8),':','') * 100000000009,12) as SessionID",
            Sql(otp), "000");

        DataTable dt = new DataTable();
        using (SqlConnection conn = new SqlConnection(Cfg("EazyConnectSource")))
        using (SqlDataAdapter adapter = new SqlDataAdapter(sql, conn))
        {
            adapter.Fill(dt);
        }
        string id = Convert.ToString(dt.Rows[0]["SessionID"]);
        return id.Length > 30 ? id.Substring(0, 30) : id;
    }

    private static string BuildPaymentSchedule(string fileName, string scheduleId, string debitSortCode, string debitAccount, string clientId, DataTable records)
    {
        StringBuilder sb = new StringBuilder();
        XmlWriterSettings settings = new XmlWriterSettings();
        settings.OmitXmlDeclaration = true;
        settings.Indent = false;

        using (XmlWriter writer = XmlWriter.Create(sb, settings))
        {
            writer.WriteStartElement("PaymentRequest");

            writer.WriteStartElement("Header");
            writer.WriteElementString("FileName", fileName);
            writer.WriteElementString("ScheduleId", scheduleId);
            writer.WriteElementString("DebitSortCode", debitSortCode);
            writer.WriteElementString("DebitAccountNumber", debitAccount);
            writer.WriteElementString("ClientId", clientId);
            writer.WriteEndElement();

            if (records.Rows.Count > 0)
            {
                foreach (DataRow dr in records.Rows)
                {
                    writer.WriteStartElement("PaymentRecord");
                    writer.WriteElementString("Beneficiary", Convert.ToString(dr["BeneficiaryName"]));
                    writer.WriteElementString("Amount", Convert.ToString(dr["Amount"], CultureInfo.InvariantCulture));
                    writer.WriteElementString("AccountNumber", Convert.ToString(dr["BeneficiaryAcctNo"]));
                    writer.WriteElementString("SortCode", Convert.ToString(dr["SortCode"]));
                    writer.WriteElementString("Narration", Convert.ToString(dr["Narration"]));
                    writer.WriteEndElement();
                }
            }
            else
            {
                writer.WriteStartElement("PaymentRecord");
                writer.WriteElementString("Beneficiary", "");
                writer.WriteElementString("Amount", "0");
                writer.WriteElementString("AccountNumber", "");
                writer.WriteElementString("SortCode", "");
                writer.WriteElementString("Narration", "");
                writer.WriteEndElement();
            }

            writer.WriteElementString("HashValue", "");
            writer.WriteEndElement();
        }

        return sb.ToString();
    }

    /// <summary>
    /// Accepts the payment service's certificate even if it does not validate, as the old code did.
    /// Added once, not on every click. Remove this when the service has a valid certificate.
    /// </summary>
    public static void BypassCertificateError()
    {
        AllowNapsCertificate();
    }

    private static void AllowNapsCertificate()
    {
        lock (CertLock)
        {
            if (_certCallbackAdded) return;
            ServicePointManager.ServerCertificateValidationCallback +=
                delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors) { return true; };
            _certCallbackAdded = true;
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
        gUser = struser;
    }

    public void AddtoLogFile(string Message, string WebPage)
    {
        try
        {
            string logPath = Server.MapPath("~/log/");
            Directory.CreateDirectory(logPath);
            string filepath = Path.Combine(logPath, "Log_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt");
            using (StreamWriter writer = new StreamWriter(filepath, true))
            {
                writer.WriteLine("-------START-------" + DateTime.Now);
                writer.WriteLine("Source :" + WebPage);
                writer.WriteLine(Message);
                writer.WriteLine("-------END-------" + DateTime.Now);
            }
        }
        catch
        {
            // Logging must never stop the payment flow
        }
    }

    private static string Col(DataRow row, string name)
    {
        return row.Table.Columns.Contains(name) ? Convert.ToString(row[name]) : "";
    }

    /// <summary>Amount as a SQL number literal (0 if it is not a number).</summary>
    private static string Amount(string value)
    {
        decimal d;
        return decimal.TryParse((value ?? "").Replace(",", ""), NumberStyles.Any, CultureInfo.InvariantCulture, out d)
            ? d.ToString(CultureInfo.InvariantCulture) : "0";
    }

    private static string SecurityElement(string value)
    {
        return System.Security.SecurityElement.Escape(value ?? "");
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