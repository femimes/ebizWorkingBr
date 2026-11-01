using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using NETClass;

public partial class Processing_FrmAppPolicy : System.Web.UI.Page
{
    public string luser;
    public string StrLocation;
    public string struser;
    public string guser;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            loadAppParameter();

            if (Request.QueryString["saved"] == "1")
                litResult.Text = "Application parameters saved.";
        }
    }

    private void loadAppParameter()
    {
        try
        {
            foreach (DataRow item in Classes.SqlDbGetDataset("Sproc_GetAppPolicies").Tables[0].Rows)
            {
                txtPassPeriod.Text = Convert.ToString(item["password_duration"]);
                password_len.Text = Convert.ToString(item["password_len"]);
                log_on_retries.Text = Convert.ToString(item["log_on_retries"]);
                grace_log_on.Text = Convert.ToString(item["grace_log_on"]);
                SignupRetryEmailCount.Text = Convert.ToString(item["SignupRetryEmailCount"]);
                SignupRetryPhoneCount.Text = Convert.ToString(item["SignupRetryPhoneCount"]);
                OTPTimeoutSec.Text = Convert.ToString(item["OTPTimeoutSec"]);
                PwdsCheckHistCount.Text = Convert.ToString(item["PwdsCheckHistCount"]);
                ValidateOnEazybankAX.Checked = AsBool(item["ValidateOnEazybankAX"]);
                AccountLimitCheck.Checked = AsBool(item["AccountLimitCheck"]);
            }
        }
        catch (Exception ex)
        {
            litError.Text = HttpUtility.HtmlEncode("The current settings could not be loaded: " + ex.Message);
        }
    }

    protected void UpdateInfo_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;

        string accountLimit = AccountLimitCheck.Checked ? "1" : "0";
        string validateOnAx = ValidateOnEazybankAX.Checked ? "1" : "0";

        try
        {
            LoadCurrentUser();

            using (SqlConnection conn = new SqlConnection(Cfg("EazyConnect")))
            using (SqlCommand cmd = new SqlCommand("Sproc_UpdateAppPolicies", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@password_duration", Num(txtPassPeriod.Text)));
                cmd.Parameters.Add(new SqlParameter("@password_len", Num(password_len.Text)));
                cmd.Parameters.Add(new SqlParameter("@log_on_retries", Num(log_on_retries.Text)));
                cmd.Parameters.Add(new SqlParameter("@grace_log_on", Num(grace_log_on.Text)));
                cmd.Parameters.Add(new SqlParameter("@AccountLimitCheck", accountLimit));
                cmd.Parameters.Add(new SqlParameter("@ValidateOnEazybankAX", validateOnAx));
                cmd.Parameters.Add(new SqlParameter("@SignupRetryEmailCount", Num(SignupRetryEmailCount.Text)));
                cmd.Parameters.Add(new SqlParameter("@SignupRetryPhoneCount", Num(SignupRetryPhoneCount.Text)));
                cmd.Parameters.Add(new SqlParameter("@OTPTimeoutSec", Num(OTPTimeoutSec.Text)));
                cmd.Parameters.Add(new SqlParameter("@PwdsCheckHistCount", Num(PwdsCheckHistCount.Text)));
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            string msg = string.Format(
                "New Application Parameter Modification on - {0}, Pass Period:{1} Pass Length:{2} Logon Retries:{3} LimitCheck:{4} ValidateAX:{5} EmailSignupRetries:{6} PhoneSignupRetries:{7} OTP Timeout:{8} Pass HistCheck:{9} Pass GraceLogon:{10}",
                DateTime.Now.ToString("dddd MMMM dd yyyy, hh:mm tt"), txtPassPeriod.Text, password_len.Text, log_on_retries.Text,
                accountLimit, validateOnAx, SignupRetryEmailCount.Text, SignupRetryPhoneCount.Text, OTPTimeoutSec.Text,
                PwdsCheckHistCount.Text, grace_log_on.Text);
            Classes.RecordOp(string.Format("Sproc_AddAuditLog '036','{0}','{1}',''", Sql(msg), Sql(guser)));

            Response.Redirect("FrmAppPolicy.aspx?saved=1", false);
            Context.ApplicationInstance.CompleteRequest();
        }
        catch (Exception ex)
        {
            litResult.Text = "";
            litError.Text = HttpUtility.HtmlEncode("The parameters were not saved: " + ex.Message);
        }
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        litResult.Text = "";
        litError.Text = "";
        loadAppParameter();
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
        guser = struser;
    }

    private static long Num(string value)
    {
        long n;
        return long.TryParse((value ?? "").Trim(), out n) ? n : 0;
    }

    private static bool AsBool(object value)
    {
        if (value == null || value == DBNull.Value) return false;
        if (value is bool) return (bool)value;

        string s = Convert.ToString(value).Trim();
        if (s == "1" || s.Equals("true", StringComparison.OrdinalIgnoreCase)) return true;

        long n;
        return long.TryParse(s, out n) && n != 0;
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