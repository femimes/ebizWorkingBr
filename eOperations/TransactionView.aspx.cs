using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.Script.Serialization;
using iBankPoints;

public partial class Transaction_TransactionView : System.Web.UI.Page
{
    public string gUser = "";
    public string luser = "";
    public string StrLocation = "";
    public string struser = "";

    private string RefNo
    {
        get { return Request.QueryString["RefNo"] ?? ""; }
    }

    private string Token
    {
        get
        {
            HttpCookie c = Request.Cookies["Token"];
            return c == null ? "" : c.Value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadCurrentUser();
            lblUser.Text = gUser;

            try
            {
                LoadDetails();
                LoadSummary();
            }
            catch (Exception ex)
            {
                litEmmorMsg.Text = HttpUtility.HtmlEncode("The transaction could not be loaded: " + ex.Message);
                btnConfirm.Enabled = false;
            }
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

    /// <summary>Shows each field of the pending transaction (format "1") as label/value pairs.</summary>
    private void LoadDetails()
    {
        HttpCookie userCookie = Request.Cookies["eLogonUserID"];
        string userName = userCookie != null && !string.IsNullOrEmpty(userCookie.Value) ? userCookie.Value : lblUser.Text;

        DataTable dt = iPoints.ProcessGetTempTrans(RefNo, userName, "1", Token);
        List<KeyValuePair<string, string>> rows = new List<KeyValuePair<string, string>>();

        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataColumn col in dt.Columns)
                rows.Add(new KeyValuePair<string, string>(col.ColumnName, Convert.ToString(dt.Rows[0][col])));
        }

        rptDetails.DataSource = rows;
        rptDetails.DataBind();
    }

    /// <summary>Works out the total to pay (amount + fee) and what to show for this transaction type.</summary>
    private void LoadSummary()
    {
        string transType = "002";
        decimal total = 0;

        DataTable dt = iPoints.ProcessGetTempTrans(RefNo, lblUser.Text, "2", Token);
        foreach (DataRow item in dt.Rows)
        {
            total += ToDecimal(item["amount"]) + ToDecimal(item["Fee"]);
            transType = Convert.ToString(item["TransType"]);
        }

        lblwebRefNo.Text = HttpUtility.HtmlEncode(string.Format("Payment reference: {0}", RefNo));
        lblAmount.Text = string.Format("NGN {0:#,0.00}", total);

        if (transType == "001")
        {
            lblwebRefNo.Visible = false;
            customSwitch1.Visible = false;
        }
        if (transType == "002")
        {
            lblwebRefNo.Visible = false;
        }
    }

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;

        LoadCurrentUser();
        litEmmorMsg.Text = "";

        try
        {
            string accountNo = "", creditAccountNo = "", location = "", narration = "", creditBankcode = "", transType = "002";
            decimal amount = 0, fee = 0;

            DataTable dt = iPoints.ProcessGetTempTrans(RefNo, lblUser.Text, "2", Token);
            foreach (DataRow item in dt.Rows)
            {
                accountNo = Convert.ToString(item["AccountNo"]);
                creditAccountNo = Convert.ToString(item["BenAcctNo"]);
                location = Convert.ToString(item["Location"]);
                amount = ToDecimal(item["amount"]);
                fee = ToDecimal(item["Fee"]);
                narration = Convert.ToString(item["narration"]);
                creditBankcode = Convert.ToString(item["BenSortCode"]);
                transType = Convert.ToString(item["TransType"]);
            }

            if (transType != "009")
            {
                litEmmorMsg.Text = "This transaction type cannot be confirmed on this page.";
                return;
            }

            string result = iPoints.ProcessTellerInterFTOTP(psword.Text, accountNo, creditAccountNo, RefNo, location,
                amount, fee, narration, lblUser.Text, creditBankcode, RefNo, Token);

            JavaScriptSerializer jss = new JavaScriptSerializer();
            Dictionary<string, object> json = jss.Deserialize<Dictionary<string, object>>(result);
            string code = json.ContainsKey("ResponseCode") ? Convert.ToString(json["ResponseCode"]) : "";
            string desc = json.ContainsKey("ResponseDesc") ? Convert.ToString(json["ResponseDesc"]) : "The transaction could not be confirmed.";

            if (code != "00")
            {
                btnConfirm.Enabled = true;
                litEmmorMsg.Text = HttpUtility.HtmlEncode(desc);
                return;
            }

            btnConfirm.Enabled = false;

            // Next step expects the PIN as before (see note: this puts the PIN in the URL)
            Response.Redirect(string.Format("TransactionViewOTP.aspx?RefNo={0}&sk={1}",
                HttpUtility.UrlEncode(RefNo), HttpUtility.UrlEncode(psword.Text)), false);
            Context.ApplicationInstance.CompleteRequest();
        }
        catch (Exception ex)
        {
            litEmmorMsg.Text = HttpUtility.HtmlEncode("The transaction could not be confirmed: " + ex.Message);
        }
    }

    private static decimal ToDecimal(object value)
    {
        decimal d;
        string s = Convert.ToString(value, CultureInfo.InvariantCulture).Replace(",", "").Trim();
        return decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out d) ? d : 0m;
    }
}