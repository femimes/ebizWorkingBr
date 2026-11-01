using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using NETClass;
using SendGrid.Helpers.Mail;

public partial class Administration_eUserProfileMgt : System.Web.UI.Page
{
    // Signed-in staff user. The auth name is stored as "username?branch".
    public string gUser = "";
    public string luser = "";
    public string StrLocation = "";
    public string struser = "";

    private static readonly HttpClient Http = new HttpClient();

    // =====================================================================
    // Page load, search and binding helpers
    // =====================================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        // The filter is not remembered between requests, so apply it every time from the search box
        ApplySearchFilter();

        if (!IsPostBack)
        {
            LoadCurrentUser();
            txtUserID.Text = struser;
            txtlocation.Text = StrLocation;
            ctrlGrid.PageIndex = 0;
        }
    }

    private void LoadCurrentUser()
    {
        luser = HttpContext.Current.User.Identity.Name ?? "";
        int split = luser.LastIndexOf('?');
        struser = split >= 0 ? luser.Substring(0, split) : luser;
        StrLocation = split >= 0 ? luser.Substring(split + 1) : "";
        gUser = struser;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ctrlGrid.PageIndex = 0;
        ctrlGrid.DataBind();
    }

    private void ApplySearchFilter()
    {
        string term = (txtSearch.Text ?? "").Trim();
        if (term == "")
        {
            SqlDataSource1.FilterExpression = "";
            return;
        }

        string like = LikeValue(term);
        SqlDataSource1.FilterExpression = string.Format(
            "Convert(user_id, 'System.String') LIKE '*{0}*' OR Convert(AccountDesc, 'System.String') LIKE '*{0}*' OR Convert(AccountNos, 'System.String') LIKE '*{0}*'",
            like);
    }

    /// <summary>Escapes text for a DataView LIKE filter.</summary>
    private static string LikeValue(string value)
    {
        StringBuilder sb = new StringBuilder();
        foreach (char ch in value)
        {
            if (ch == '*' || ch == '%' || ch == '[' || ch == ']') sb.Append('[').Append(ch).Append(']');
            else if (ch == '\'') sb.Append("''");
            else sb.Append(ch);
        }
        return sb.ToString();
    }

    protected void SqlDataSource1_Selected(object sender, SqlDataSourceStatusEventArgs e)
    {
        if (e.Exception != null) return;

        string term = (txtSearch.Text ?? "").Trim();
        litTotal.Text = term == ""
            ? string.Format("&middot; {0:N0} total", e.AffectedRows)
            : "&middot; matching &ldquo;" + HttpUtility.HtmlEncode(term) + "&rdquo;";
    }

    /// <summary>Turns bit/int/string/null values into a bool for the tick boxes.</summary>
    protected bool AsBool(object value)
    {
        if (value == null || value == DBNull.Value) return false;
        if (value is bool) return (bool)value;

        string s = Convert.ToString(value).Trim();
        if (s == "1" || s.Equals("true", StringComparison.OrdinalIgnoreCase) || s.Equals("yes", StringComparison.OrdinalIgnoreCase)) return true;

        long n;
        return long.TryParse(s, out n) && n != 0;
    }

    // =====================================================================
    // Button handlers
    // =====================================================================

    /// <summary>Activate: reset the password, set channels, and email the new login details.</summary>
    protected void BtnApprove_Click(object sender, EventArgs e)
    {
        RunForSelection("Activated", delegate (SelectedRow row)
        {
            RequireControls(row.Mobile, row.Portal);

            if (row.AccountNumber == "******")
                throw new InvalidOperationException("not activated because no account number is available");

            string newPassword = NewPassword(8);

            Classes.RecordOp(string.Format("SpResetActivatePassword '{0}','{1}','{2}',{3},{4},'{5}'",
                Sql(row.UserName), Sql(newPassword), Sql(row.AccountNos),
                Bit(row.Mobile.Checked), Bit(row.Portal.Checked), Sql(gUser)));

            Audit(string.Format("Profile maintenance on - Customer:{0}; Activation: yes; Desc: Password Reset Mail sent; AccountNo: {1}",
                row.UserName, row.AccountNumber), row.UserName);

            return SendActivationEmail(row, newPassword);
        });
    }

    /// <summary>Deactivate: needs a reason on each selected row.</summary>
    protected void btnDisApprove_Click(object sender, EventArgs e)
    {
        RunForSelection("Deactivated", delegate (SelectedRow row)
        {
            string reason = RequireReason(row);
            string rejectInfo = string.Format("Rejected By: {0}; Time: {1}; Reason: {2}",
                gUser, DateTime.Today.ToShortDateString(), reason);

            ExecProc("SpDeResetActivatePassword",
                new SqlParameter("@UserName", row.UserName),
                new SqlParameter("@gUser", gUser),
                new SqlParameter("@Reason", reason));

            Audit(string.Format("Profile maintenance on - Customer:{0}; Deactivation: yes; Desc: Channel Deactivation; AccountNo: {1}; Reason: {2}",
                row.UserName, row.AccountNumber, rejectInfo), row.UserName);

            return null;
        });
    }

    /// <summary>Clear security questions: needs a reason on each selected row.</summary>
    protected void btnSecurityQuestClear_Click(object sender, EventArgs e)
    {
        RunForSelection("Security questions cleared", delegate (SelectedRow row)
        {
            string reason = RequireReason(row);
            string info = string.Format("Security Questions Cleared By: {0}; Time: {1}; Reason: {2}",
                gUser, DateTime.Today.ToShortDateString(), reason);

            ExecProc("SpClearSecQuestion",
                new SqlParameter("@UserName", row.UserName),
                new SqlParameter("@gUser", gUser),
                new SqlParameter("@Reason", info));

            Audit(string.Format("Profile maintenance on - Customer:{0}; Security Question Reset: yes; Desc: Channel Security Questions Cleared; AccountNo: {1}; Reason: {2}",
                row.UserName, row.AccountNos, info), row.UserName);

            return SendSecurityQuestionsEmail(row);
        });
    }

    /// <summary>Clear device information: needs a reason on each selected row.</summary>
    protected void btnDeviceClear_Click(object sender, EventArgs e)
    {
        RunForSelection("Device information cleared", delegate (SelectedRow row)
        {
            string reason = RequireReason(row);
            string info = string.Format("Device Deletion By: {0}; Time: {1}; Reason: {2}",
                gUser, DateTime.Today.ToShortDateString(), reason);

            ExecProc("SPGetUserPassportDeviceDelAll",
                new SqlParameter("@UserId", row.UserName));

            Audit(string.Format("Device Profile maintenance on - Customer:{0}; DeviceDeletion: yes; Desc: Channel Deactivation; AccountNo: {1}; Reason: {2}",
                row.UserName, row.AccountNumber, info), row.UserName);

            return SendDeviceEmail(row);
        });
    }

    /// <summary>Saves the eToken tick box for each selected row (on or off).</summary>
    protected void BtneToken_Click(object sender, EventArgs e)
    {
        RunForSelection("eToken updated", delegate (SelectedRow row)
        {
            RequireControls(row.EToken);

            Classes.RecordOp(string.Format("Sproc_UpdateUsereToken '{0}',{1}",
                Sql(row.UserName), Bit(row.EToken.Checked)));

            Audit(string.Format("Profile maintenance on - Customer:{0}; eToken Activation status: {1}",
                row.UserName, row.EToken.Checked), row.UserName);

            return null;
        });
    }

    protected void btncancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("eUserProfileMgt.aspx", false);
        Context.ApplicationInstance.CompleteRequest();
    }

    // =====================================================================
    // Selection handling
    // =====================================================================

    private sealed class SelectedRow
    {
        public string UserName;
        public string AccountNumber;
        public string AccountNos;
        public string LastName;
        public CheckBox Mobile;
        public CheckBox Portal;
        public CheckBox EToken;
        public TextBox Reason;
    }

    /// <summary>
    /// Runs an action for every ticked row. The action returns a warning (or null) and throws to report a failure.
    /// One bad row does not stop the others.
    /// </summary>
    private void RunForSelection(string doneLabel, Func<SelectedRow, string> action)
    {
        LoadCurrentUser();

        List<SelectedRow> rows = GetSelectedRows();
        List<string> problems = new List<string>();
        int done = 0;

        if (rows.Count == 0)
        {
            ShowResult(null, new List<string> { "Select at least one customer first." });
            return;
        }

        foreach (SelectedRow row in rows)
        {
            try
            {
                string warning = action(row);
                done++;
                if (!string.IsNullOrEmpty(warning))
                    problems.Add(string.Format("{0}: {1}", row.UserName, warning));
            }
            catch (Exception ex)
            {
                problems.Add(string.Format("{0}: {1}", row.UserName, ex.Message));
            }
        }

        // Rebinding refreshes the list and clears the ticks
        ctrlGrid.DataBind();

        string success = done > 0
            ? string.Format("{0}: {1} customer{2}.", doneLabel, done, done == 1 ? "" : "s")
            : null;
        ShowResult(success, problems);
    }

    private List<SelectedRow> GetSelectedRows()
    {
        List<SelectedRow> rows = new List<SelectedRow>();

        foreach (GridViewRow gridRow in ctrlGrid.Rows)
        {
            if (gridRow.RowType != DataControlRowType.DataRow) continue;

            CheckBox chkSelect = gridRow.FindControl("chkSelect") as CheckBox;
            if (chkSelect == null || !chkSelect.Checked) continue;

            DataKey keys = ctrlGrid.DataKeys[gridRow.RowIndex];

            SelectedRow row = new SelectedRow();
            row.UserName = Convert.ToString(keys["user_id"]);
            row.AccountNumber = Convert.ToString(keys["AccountNumber"]);
            row.AccountNos = Convert.ToString(keys["AccountNos"]);
            row.LastName = Convert.ToString(keys["LastName"]);
            row.Mobile = gridRow.FindControl("chkMobile") as CheckBox;
            row.Portal = gridRow.FindControl("chkPortal") as CheckBox;
            row.EToken = gridRow.FindControl("chkeToken") as CheckBox;
            row.Reason = gridRow.FindControl("tatReason") as TextBox;

            rows.Add(row);
        }

        return rows;
    }

    private static void RequireControls(params object[] controls)
    {
        foreach (object c in controls)
        {
            if (c == null)
                throw new InvalidOperationException("this row could not be read; refresh the page and try again");
        }
    }

    private static string RequireReason(SelectedRow row)
    {
        RequireControls(row.Reason);
        string reason = (row.Reason.Text ?? "").Trim();
        if (reason == "")
            throw new InvalidOperationException("enter a reason in the Reason column");
        return reason;
    }

    private void ShowResult(string success, List<string> problems)
    {
        litResult.Text = string.IsNullOrEmpty(success) ? "" : HttpUtility.HtmlEncode(success);

        if (problems == null || problems.Count == 0)
        {
            Literal1.Text = "";
            return;
        }

        StringBuilder sb = new StringBuilder();
        foreach (string p in problems)
        {
            if (sb.Length > 0) sb.Append("<br />");
            sb.Append(HttpUtility.HtmlEncode(p));
        }
        Literal1.Text = sb.ToString();
    }

    // =====================================================================
    // Emails
    // =====================================================================

    private string SendActivationEmail(SelectedRow row, string newPassword)
    {
        Preferences prefs = GetPreferences();
        if (!prefs.AutoActivateUser) return null;

        CustomerContact contact = GetCustomerContact(row.AccountNos, row.LastName);
        string brand = Cfg("CompanyBrand");
        string subject = string.Format("{0} Retail Internet Banking User Profile Activation and Reset", brand);
        string body = EmailLayout(contact.Name, brand, prefs.OrgType,
            "<p>Your profile has been successfully activated. Here are your new login details:</p>" +
            "<p>Username: <strong>" + HttpUtility.HtmlEncode(row.UserName) + "</strong><br />" +
            "Password: <strong>" + HttpUtility.HtmlEncode(newPassword) + "</strong></p>" +
            "<p>You will be asked to change your password the next time you log on. If you have difficulty logging on, please contact " +
            HttpUtility.HtmlEncode(prefs.CompanyContacts) + " or our call centre.</p>");

        return SendEmail(contact, subject, body, "Profile activation");
    }

    private string SendSecurityQuestionsEmail(SelectedRow row)
    {
        Preferences prefs = GetPreferences();
        if (!prefs.AutoActivateUser) return null;

        CustomerContact contact = GetCustomerContact(row.AccountNos, row.LastName);
        string brand = Cfg("CompanyBrand");
        string subject = string.Format("{0} Online User Profile Security Questions Reset", brand);
        string body = EmailLayout(contact.Name, brand, prefs.OrgType,
            "<p>Your security questions have been reset.</p>" +
            "<p>You can set up new security questions the next time you log on with your usual login details. " +
            "If you have difficulty logging on, please contact " + HttpUtility.HtmlEncode(prefs.CompanyContacts) + " or our call centre.</p>");

        return SendEmail(contact, subject, body, "Security question maintenance");
    }

    private string SendDeviceEmail(SelectedRow row)
    {
        Preferences prefs = GetPreferences();
        if (!prefs.AutoActivateUser) return null;

        // Only customers who use iPortal or mobile get the device email
        bool usesPortal = row.Portal != null && row.Portal.Checked;
        bool usesMobile = row.Mobile != null && row.Mobile.Checked;
        if (!usesPortal && !usesMobile) return null;

        CustomerContact contact = GetCustomerContact(row.AccountNos, row.LastName);
        string brand = Cfg("CompanyBrand");
        string subject = string.Format("{0} Retail Banking User Device Profile Maintenance", brand);
        string body = EmailLayout(contact.Name, brand, prefs.OrgType,
            "<p>Your device profile has been reset.</p>" +
            "<p>You will be asked to register your new device the next time you log on. If you have difficulty logging on, please contact " +
            HttpUtility.HtmlEncode(prefs.CompanyContacts) + " or our call centre.</p>");

        return SendEmail(contact, subject, body, "Device maintenance");
    }

    private static string EmailLayout(string name, string brand, string orgType, string content)
    {
        string b = HttpUtility.HtmlEncode(brand);
        string org = HttpUtility.HtmlEncode(string.IsNullOrEmpty(orgType) ? "Mortgage" : orgType);
        return "<html><body style=\"font-family: 'Century Gothic', Arial, sans-serif; font-size: 14px; color: #14212e;\">" +
               "<p>Dear " + HttpUtility.HtmlEncode(name) + ",</p>" +
               content +
               "<p style=\"font-size: 12px; color: #5d6b7a;\">Note: " + b + " " + org + " Bank will never send you a link to an external website " +
               "or ask for your personal banking details by email, telephone, social media (Twitter, Facebook, LinkedIn, WhatsApp, Instagram, etc.) " +
               "or in person. Always keep your login details safe and never share them with anyone.</p>" +
               "<p>Thank you for banking with us.<br /><strong>" + b + "</strong></p>" +
               "</body></html>";
    }

    /// <summary>
    /// Sends through the provider named in the EmailProvider setting (SendGrid, SilverSands, otherwise SMTP/Exchange).
    /// Returns a warning, or null when sent.
    /// </summary>
    private string SendEmail(CustomerContact contact, string subject, string htmlBody, string logSource)
    {
        string sender = Cfg("EmailSenderAddress");
        string brand = Cfg("CompanyBrand");
        string provider = Cfg("EmailProvider");
        string warning = null;

        string to = contact.Email;
        if (string.IsNullOrEmpty(to) || to == "NILL" || !to.Contains("@"))
        {
            // Same fallback as before, but now reported so staff can follow up
            to = sender;
            warning = string.Format("no valid email on file, so the email went to {0} instead", sender);
        }

        try
        {
            if (provider == "SendGrid")
            {
                var client = new SendGrid.SendGridClient(Cfg("EmailApiKey"));
                var msg = MailHelper.CreateSingleEmail(
                    new EmailAddress(sender, brand), new EmailAddress(to, to), subject, htmlBody, htmlBody);

                // Wait for the result so failures are reported (Task.Run avoids an ASP.NET deadlock)
                var response = Task.Run(() => client.SendEmailAsync(msg)).GetAwaiter().GetResult();
                if ((int)response.StatusCode >= 300)
                    return string.Format("email not sent (SendGrid returned {0})", (int)response.StatusCode);
            }
            else if (provider == "SilverSands")
            {
                string apiSubject = string.Format("{0} {1}", brand.ToUpper(), subject.ToUpper());
                string result = Task.Run(() => SendEmailExec(sender, Cfg("EmailUserName"), Cfg("EmailPwds"), brand, htmlBody, to, apiSubject, htmlBody))
                    .GetAwaiter().GetResult();
                AddtoLogFile(to, string.Format("{0}: responseCode {1}", logSource, result));
            }
            else
            {
                using (MailMessage message = new MailMessage(sender, to, subject, htmlBody))
                using (SmtpClient smtp = new SmtpClient(Cfg("EazyEmailServer"), int.Parse(Cfg("EazyEmailServerPort"))))
                {
                    message.From = new MailAddress(sender, brand);
                    message.IsBodyHtml = true;
                    smtp.Credentials = new NetworkCredential(Cfg("EmailUserName"), Cfg("EmailPwds"));
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Send(message);
                }
            }
        }
        catch (Exception ex)
        {
            Exception inner = ex.InnerException ?? ex;
            return "email not sent: " + inner.Message;
        }

        return warning;
    }

    /// <summary>Sends through the SilverSands email API. Throws if the API returns an error.</summary>
    public async Task<string> SendEmailExec(string from, string username, string password, string displayname, string text, string to, string subject, string htmlMsg)
    {
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

        string apiKey = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", username, password)));

        using (var request = new HttpRequestMessage(HttpMethod.Post, "https://api.silversands.com.ng/email/3/send"))
        using (var content = new MultipartFormDataContent("------------" + Guid.NewGuid()))
        {
            request.Headers.Add("Authorization", "Basic " + apiKey);
            content.Add(new StringContent(string.Format("{0} <{1}>", displayname, from)), "from");
            content.Add(new StringContent(to), "to");
            content.Add(new StringContent(subject), "subject");
            content.Add(new StringContent(htmlMsg), "html");
            request.Content = content;

            using (HttpResponseMessage response = await Http.SendAsync(request).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }
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
            // Logging must never stop the action itself
        }
    }

    // =====================================================================
    // Lookups
    // =====================================================================

    private sealed class CustomerContact
    {
        public string Name = "";
        public string Email = "";
        public string SmsNo = "";
    }

    private sealed class Preferences
    {
        public bool AutoActivateUser;
        public string CompanyContacts = "";
        public string OrgType = "";
        public long DefaultSmsVendor;
    }

    private static CustomerContact GetCustomerContact(string accountNos, string fallbackName)
    {
        CustomerContact c = new CustomerContact();
        c.Name = fallbackName ?? "";

        foreach (DataRow item in Classes.GetDataset(string.Format("SpAXMobileNo '{0}'", Sql(accountNos))).Tables[0].Rows)
        {
            c.SmsNo = Convert.ToString(item["SMSNo"]);
            if (item.Table.Columns.Contains("AccountDesc"))
                c.Name = Convert.ToString(item["AccountDesc"]);
        }
        foreach (DataRow item in Classes.GetDataset(string.Format("SpAXMobileEmail '{0}'", Sql(accountNos))).Tables[0].Rows)
        {
            c.Email = Convert.ToString(item["EmailAddr"]).Trim();
        }
        return c;
    }

    private static Preferences GetPreferences()
    {
        Preferences p = new Preferences();
        foreach (DataRow item in Classes.GetDataset("Sproc_GetPref").Tables[0].Rows)
        {
            p.AutoActivateUser = Convert.ToInt64(item["AutoActivateUser"]) == 1;
            if (item.Table.Columns.Contains("CompanyContacts"))
                p.CompanyContacts = Convert.ToString(item["CompanyContacts"]);
            if (item.Table.Columns.Contains("Orgtype"))
                p.OrgType = Convert.ToString(item["Orgtype"]);
            if (item.Table.Columns.Contains("DefaultSMSVendor"))
                p.DefaultSmsVendor = Convert.ToInt64(item["DefaultSMSVendor"]);
        }
        return p;
    }

    // =====================================================================
    // SMS (not called anywhere yet, kept for when SMS alerts are switched on)
    // =====================================================================
    public string Pushsms(string phoneNo, string username, string pwds, string displayname, string msg)
    {
        Preferences prefs = GetPreferences();
        string url;

        if (prefs.DefaultSmsVendor == 1)
        {
            url = string.Format("https://api.silversands.com.ng/sendsms/plain?user={0}&password={1}&sender={2}&SMSText={3}&GSM={4}&type=longSMS",
                Url(username), Url(pwds), Url(displayname), Url(msg), Url(phoneNo));
        }
        else
        {
            url = string.Format("http://sms.hensoftsolutions.com/api.aspx?cmd=sendtextsms&accountname={0}&accountpassword={1}&sender={2}&reciever={3}&Msg={4}",
                Url(username), Url(pwds), Url(displayname), Url(phoneNo), Url(msg));
        }

        WebRequest request = WebRequest.Create(url);
        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        request.ContentLength = 0;

        using (WebResponse response = request.GetResponse())
        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
        {
            return reader.ReadToEnd();
        }
    }

    // =====================================================================
    // Helpers
    // =====================================================================

    private void Audit(string message, string customer)
    {
        Classes.RecordOp(string.Format("Sproc_AddAuditLog '900','{0}','{1}','{2}'",
            Sql(message), Sql(gUser), Sql(customer)));
    }

    private static void ExecProc(string name, params SqlParameter[] parameters)
    {
        using (SqlConnection conn = new SqlConnection(Cfg("EazyConnect")))
        using (SqlCommand cmd = new SqlCommand(name, conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(parameters);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }

    private static string Cfg(string name)
    {
        ConnectionStringSettings s = ConfigurationManager.ConnectionStrings[name];
        return s == null ? "" : s.ConnectionString;
    }

    /// <summary>Escapes single quotes for the string-built SQL calls.</summary>
    private static string Sql(string value)
    {
        return (value ?? "").Replace("'", "''");
    }

    private static string Url(string value)
    {
        return HttpUtility.UrlEncode(value ?? "");
    }

    private static int Bit(bool value)
    {
        return value ? 1 : 0;
    }

    /// <summary>Random password from a cryptographic generator (no look-alike characters such as 0/o, 1/l).</summary>
    private static string NewPassword(int length)
    {
        const string chars = "abcdefghjkmnpqrstuvwxyz23456789";
        byte[] bytes = new byte[length];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(bytes);
        }
        StringBuilder sb = new StringBuilder(length);
        foreach (byte b in bytes)
            sb.Append(chars[b % chars.Length]);
        return sb.ToString();
    }

    public String GetIPAddress()
    {
        string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        if (string.IsNullOrEmpty(ip))
            ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        return ip;
    }
}