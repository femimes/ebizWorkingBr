using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using NETClass;

public partial class Administration_ActivateRequest : System.Web.UI.Page
{
    // Signed-in staff user. The auth name is stored as "username?branch".
    public string gUser = "";
    public string luser = "";
    public string StrLocation = "";
    public string struser = "";

    // =====================================================================
    // Page load, search and binding helpers
    // =====================================================================
    protected void Page_Load(object sender, EventArgs e)
    {
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
            "Convert(user_id, 'System.String') LIKE '*{0}*' OR Convert(AccountDesc, 'System.String') LIKE '*{0}*' OR Convert(AccountNumber, 'System.String') LIKE '*{0}*'",
            like);
    }

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
            ? string.Format("&middot; {0:N0} waiting", e.AffectedRows)
            : "&middot; matching &ldquo;" + HttpUtility.HtmlEncode(term) + "&rdquo;";
    }

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

    /// <summary>Activate: set a new password and channels, then email the login details.</summary>
    protected void BtnApprove_Click(object sender, EventArgs e)
    {
        RunForSelection("Activated", delegate (SelectedRow row)
        {
            if (row.Mobile == null || row.Portal == null)
                throw new InvalidOperationException("this row could not be read; refresh the page and try again");

            string newPassword = NewPassword(8);

            Classes.RecordOp(string.Format("SpActivatePassword '{0}','{1}','{2}',{3},{4}",
                Sql(row.UserName), Sql(newPassword), Sql(row.AccountNumber),
                Bit(row.Mobile.Checked), Bit(row.Portal.Checked)));

            return SendActivationEmail(row.UserName, row.AccountNumber, newPassword);
        });
    }

    /// <summary>Deactivate: needs a reason on each selected row.</summary>
    protected void btnDisApprove_Click(object sender, EventArgs e)
    {
        RunForSelection("Deactivated", delegate (SelectedRow row)
        {
            string reason = row.Reason == null ? "" : (row.Reason.Text ?? "").Trim();
            if (reason == "")
                throw new InvalidOperationException("enter a reason in the Reason column");

            string rejectInfo = string.Format("Rejected By: '{0}'; Time: '{1}'; Reason: '{2}'",
                gUser, DateTime.Today.ToShortDateString(), reason);

            using (SqlConnection conn = new SqlConnection(Cfg("EazyConnect")))
            using (SqlCommand cmd = new SqlCommand("SpDeActivate", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@UserName", row.UserName));
                cmd.Parameters.Add(new SqlParameter("@Reason", rejectInfo));
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return null;
        });
    }

    protected void btncancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ActivateRequest.aspx", false);
        Context.ApplicationInstance.CompleteRequest();
    }

    // =====================================================================
    // Selection handling
    // =====================================================================

    private sealed class SelectedRow
    {
        public string UserName;
        public string AccountNumber;
        public CheckBox Mobile;
        public CheckBox Portal;
        public TextBox Reason;
    }

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
            row.Mobile = gridRow.FindControl("chkMobile") as CheckBox;
            row.Portal = gridRow.FindControl("chkPortal") as CheckBox;
            row.Reason = gridRow.FindControl("tatReason") as TextBox;
            rows.Add(row);
        }

        return rows;
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
    // Email (SMTP, as before)
    // =====================================================================

    private string SendActivationEmail(string userName, string accountNumber, string newPassword)
    {
        string sender = Cfg("EmailSenderAddress");
        string brand = Cfg("CompanyBrand");
        string warning = null;

        string to = "";
        foreach (DataRow item in Classes.GetDataset(string.Format("SpAXMobileEmail '{0}'", Sql(accountNumber))).Tables[0].Rows)
            to = Convert.ToString(item["EmailAddr"]).Trim();

        if (string.IsNullOrEmpty(to) || to == "NILL" || !to.Contains("@"))
        {
            to = sender;
            warning = string.Format("no valid email on file, so the email went to {0} instead", sender);
        }

        string body =
            "<html><body style=\"font-family: 'Century Gothic', Arial, sans-serif; font-size: 14px; color: #14212e;\">" +
            "<p>Thank you for using our mobile service. Here are your login details:</p>" +
            "<p>Username: <strong>" + HttpUtility.HtmlEncode(userName) + "</strong><br />" +
            "Password: <strong>" + HttpUtility.HtmlEncode(newPassword) + "</strong></p>" +
            "<p>If you have any questions or trouble logging on, please contact any of our branches or our call centre.</p>" +
            "<p>Thank you!<br /><strong>" + HttpUtility.HtmlEncode(brand) + "</strong></p>" +
            "</body></html>";

        try
        {
            using (MailMessage message = new MailMessage(sender, to, "Your Mobile Banking Access Notification", body))
            using (SmtpClient smtp = new SmtpClient(Cfg("EazyEmailServer"), int.Parse(Cfg("EazyEmailServerPort"))))
            {
                message.From = new MailAddress(sender, brand);
                message.IsBodyHtml = true;
                smtp.Credentials = new NetworkCredential(Cfg("EmailUserName"), Cfg("EmailPwds"));
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
        }
        catch (Exception ex)
        {
            return "activated, but the email was not sent: " + ex.Message;
        }

        return warning;
    }

    // =====================================================================
    // Helpers
    // =====================================================================

    private static string Cfg(string name)
    {
        ConnectionStringSettings s = ConfigurationManager.ConnectionStrings[name];
        return s == null ? "" : s.ConnectionString;
    }

    private static string Sql(string value)
    {
        return (value ?? "").Replace("'", "''");
    }

    private static int Bit(bool value)
    {
        return value ? 1 : 0;
    }

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