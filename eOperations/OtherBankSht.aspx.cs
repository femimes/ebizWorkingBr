using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using iBankPoints;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Web.Script.Serialization;
using DevExpress.Web;
using System.Web.Configuration;
using System.Configuration;
using iBankPoints;


public partial class Transaction_OtherBankSht : System.Web.UI.Page
{
    public DataTable ks = new DataTable();
    public string gUser = HttpContext.Current.User.Identity.Name;//HttpContext.Current.User.Identity.Name;
    public string luser = HttpContext.Current.User.Identity.Name;
    public string StrLocation = "";
    public string struser = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
             if (!IsPostBack && !IsCallback)
                {
                    //hypdashboard.NavigateUrl = "../mainpage/UIHome.aspx";
                    luser = HttpContext.Current.User.Identity.Name;
                    StrLocation = luser.Substring(luser.LastIndexOf("?") + 1);
                    struser = luser.Substring(0, luser.LastIndexOf("?"));
                    gUser = struser;
            
                    #region Section generate token Auth details

                    string strRespCode = ""; string strRespDesc = ""; string strToken = "";
                    string strResults = "";


                    strResults = iBankPoints.iPoints.ProcessTellerAccessNoMsg(gUser.Trim(), "");
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    dynamic json = jss.DeserializeObject(strResults);
                    strRespCode = json["ResponseCode"];
                    strRespDesc = json["ResponseDesc"];
                    strToken = json["Token"];
                    if (strRespCode == "00")
        
                       {
                        //iBankPoints.iPoints.ProcessUserLogout(gUser.Trim());
                        Response.Cookies["Token"].Value = strToken;
                        Response.Cookies["Token"].Secure = true;
                        Response.Cookies["Token"].HttpOnly = true;


                        Response.Cookies["LogonUserID"].Value = gUser.Trim();
                        Response.Cookies["LogonUserID"].Secure = true;
                        Response.Cookies["LogonUserID"].HttpOnly = true;

                        //   HttpCookie cookie = new HttpCookie("TokenAuth", strToken);
                        //   //cookie.Domain.
                        //   //cookie.Values.Add("Token1", strToken);
                        //   Response.Cookies.Add(cookie);

                        //   Response.Cookies["TokenAuth"].Value = strToken;
                        //   Response.Cookies["TokenAuth"].Secure = true;
                        //   Response.Cookies["TokenAuth"].HttpOnly = true;
                        //   //cookie.Values.Add("address", address);
                        //  // FormsAuthentication.SetAuthCookie(string.Format("{0}", txtUserName.Text, strToken), false);

                        //   // FormsAuthentication.SetAuthCookie.
                        ////   FormsAuthentication.RedirectFromLoginPage(txtUserName.Text, true);
                        //   if (Request.Cookies["TokenAuth"] == null)
                        //   {
                        //       // txtretrieve.Text = "No cookie found";
                        //   }
                        //Session["Reset"] = true;
                        //Response.Redirect("~/mainpage/AbbeyDesk.aspx");
                        txtUserName.Text = gUser.Trim();
                   
                            ks = iBankPoints.iPoints.ProcessProfileds(gUser.Trim(), Request.Cookies["Token"].Value);
                      
                      
                        foreach (DataRow item in ks.Rows)
                        {
                            lblAccountNo.Text = item["AccountNo"].ToString();
                        }

                        customSwitch1.Checked = false;

                    #region Show Limits

                            string expression;  // expression = "Date > #1/1/00#";
                            expression = string.Format("AccountNo  = {0}", lblAccountNo.Text);
                            DataTable dsAcctLimits = new DataTable();
                            dsAcctLimits = iBankPoints.iPoints.ProcessUserAcctLimits("002", lblAccountNo.Text, Request.Cookies["Token"].Value);
                            DataRow[] foundLimitRows;
                            foundLimitRows = dsAcctLimits.Select(expression);
                            foreach (DataRow item in foundLimitRows)
                            {
                                LblPayName.Text = item["AccountDesc"].ToString();
                                lblAmount.Text = string.Format("{0}", (decimal.Parse(item["IPortalInterTrf"].ToString()) - decimal.Parse(item["IportalNipInterPerDay"].ToString())).ToString("#,###,##0.00"));
                                //lblAmount.Text = string.Format("{0}", (decimal.Parse(item["IPortalLocalTrf"].ToString()) - decimal.Parse("0")).ToString("#,###,##0.00"));
                                lblTransAmountLimit.Text = string.Format("{0}", (decimal.Parse(item["IPortalInterTrfOnly"].ToString())).ToString("#,###,##0.00"));


                            }

                    #endregion

                    }
                    else
                    {
                        errMsg.Text = strRespDesc;
                        Response.Write(strRespDesc);
                    }
                #endregion




                //Session["Reset"] = true;
                //Configuration config = WebConfigurationManager.OpenWebConfiguration("~/Web.Config");
                //SessionStateSection section = (SessionStateSection)config.GetSection("system.web/sessionState");
                //int timeout = (int)section.Timeout.TotalMinutes * 1000 * 60;
                //ClientScript.RegisterStartupScript(this.GetType(), "SessionAlert", "SessionExpireAlert(" + timeout + ");", true);


            }
        }
        catch (Exception ex)
        {

            Response.Write(ex.Message);
        }
        
    }

    string LoadAcctDetails(string strAccountNo)
    {

        luser = HttpContext.Current.User.Identity.Name;
        StrLocation = luser.Substring(luser.LastIndexOf("?") + 1);
        struser = luser.Substring(0, luser.LastIndexOf("?"));
        gUser = struser;
        DataTable dsAcctList = new DataTable();
        dsAcctList =  iBankPoints.iPoints.ProcessUserAcctList(gUser, lblAccountNo.Text, Request.Cookies["Token"].Value);


        string expression;  // expression = "Date > #1/1/00#";
        expression = string.Format("AccountNo  = {0}", strAccountNo);
        lblAvail.Attributes["AvailBal"] = "0";
        DataRow[] foundRows;
        foundRows = dsAcctList.Select(expression);
        foreach (DataRow item in foundRows)
        {
            TxtCurr.Text = item["CurrCode"].ToString();
            lblAvail.Text = string.Format("{0}", decimal.Parse(item["AvailBal"].ToString()).ToString("#,##0.00"));
            //lblAvail.Attributes["AvailBal"]
        }

        #region Show Limits
        DataTable dsAcctLimits = new DataTable();
        dsAcctLimits = iBankPoints.iPoints.ProcessUserAcctLimits("002", lblAccountNo.Text, Request.Cookies["Token"].Value);
        DataRow[] foundLimitRows;
        foundLimitRows = dsAcctLimits.Select(expression);
        foreach (DataRow item in foundLimitRows)
        {
            LblPayName.Text = item["AccountDesc"].ToString();
            lblAmount.Text = string.Format("{0}", (decimal.Parse(item["IPortalInterTrf"].ToString()) - decimal.Parse(item["IportalNipInterPerDay"].ToString())).ToString("#,###,##0.00"));
            lblTransAmountLimit.Text = string.Format("{0}", (decimal.Parse(item["IPortalInterTrfOnly"].ToString())).ToString("#,###,##0.00"));

            //lblAmount.Text = string.Format("{0}", (decimal.Parse(item["IPortalLocalTrf"].ToString()) - decimal.Parse("0")).ToString("#,###,##0.00"));

        }
        #endregion

        return "";
    }

    protected void ASPxComboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        //LoadAcctDetails(ASPxComboBox1.Value.ToString());
    }

    string LoadCharges(double Amount, string ChannelCode)
    {
        string strResults = ""; decimal FeeCharge = 0; string strval = ""; DataTable dt = new DataTable();
        //strResults = iPoints.ProcessFTCharges(Amount, ChannelCode);
        //JavaScriptSerializer jss = new JavaScriptSerializer();
        //dynamic json = jss.DeserializeObject(strResults);
        ////FeeCharge = Int64.Parse(json["FeeCharge"]);
        //strval = string.Format("{0}", json["FeeCharge"]);
        //strRespDesc = json["ResponseDesc"];
        foreach (DataRow item in iPoints.ProcessFTChargesDs(Amount, ChannelCode, Request.Cookies["Token"].Value).Rows)
        {
            strval = string.Format("{0}", decimal.Parse(item["FeeCharge"].ToString()).ToString("#,##0.00"));
        }


        return strval;
    }


    protected void drpAccountNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadAcctDetails(drpAccountNo.Value.ToString());
        LoadBeneficiaryList();
    }



    protected void drpBenList_SelectedIndexChanged(object sender, EventArgs e)
    {
        drpBenList.Columns[2].Visible = true;
        string strItemSel = drpBenList.Text;
        int iLastIndex = strItemSel.LastIndexOf(";");
        string strBankCode = strItemSel.Substring(iLastIndex + 1);
        LoadSavedBenDetails(drpBenList.Value.ToString(), strBankCode);
        //LoadBeneficiaryList();
        drpBenList.Columns[2].Visible = false;
    }

    protected void txtAmount_TextChanged(object sender, EventArgs e)
    {
        //LoadBeneficiaryList();
        //LoadBenDetails();
        LoadBeneficiaryList();
        LoadAcctDetails(drpAccountNo.Value.ToString());
        txtFee.Text = LoadCharges(double.Parse(txtAmount.Text), "002");
        txtAmount.Attributes["ErrorInfo"] = "";


        if (double.Parse(txtAmount.Text) < 50)
        {
            txtAmount.Attributes["ErrorInfo"] = "Amount below minimum transaction value";

            txtAmount.IsValid = false;
            txtAmount.ErrorText = "Amount below minimum transaction value";

        }
        if (double.Parse(txtAmount.Text) > double.Parse(lblAvail.Text) )
        {
            txtAmount.Attributes["ErrorInfo"] = "Account available amount exceeded";

            txtAmount.IsValid = false;
            txtAmount.ErrorText = "Account available amount exceeded";

        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strSavedBenAcctNo"></param>
    /// <param name="BankCode"></param>
    public void LoadSavedBenDetails(string strSavedBenAcctNo, string BankCode)
    {

        txtBenAccountNo.Text = strSavedBenAcctNo;
        ASPxComboBox1.DataBind();
        string strbankname = "";
        string ivalue = "";
        //DevExpress.Web.ASPxComboBox listItemks;
        foreach (ListEditItem item in ASPxComboBox1.Items)
        {
            ivalue = item.Value.ToString().Trim();
            if (ivalue == BankCode.Trim())
            {
                strbankname = item.Text;
            }
        }

        txtBenAccountNo.ReadOnly = true;
        lblBankName.Text = strbankname;// ASPxComboBox1.SelectedItem.GetValue(BankCode).ToString();




    }

    public void LoadBenDetails()
    {
        string strRespCode = ""; string strRespDesc = "";
        string strResults = "";
        lblBenName.Text = "";
        strResults = iBankPoints.iPoints.ProcessInterBankBenName(txtBenAccountNo.Text, ASPxComboBox1.Value.ToString(), "001", "00", Request.Cookies["Token"].Value);
        JavaScriptSerializer jss = new JavaScriptSerializer();
        dynamic json = jss.DeserializeObject(strResults);
        strRespCode = json["ResponseCode"];
        strRespDesc = json["ResponseDesc"];
        if (strRespCode == "00")
        {
            lblBenName.Text = strRespDesc;
            errMsg.Text = string.Empty;
        }
        else
        {

            errMsg.Text = strRespDesc;
            //ValSumAlert.
        }



    }
    public void LoadBeneficiaryList()
    {
        txtBenAccountNo.ReadOnly = false;
        lblBenOrBankList.Text = "Beneficiary Bank Listings:";
        drpBenList.Visible = false;
        lblBankName.Visible = false;
        ASPxComboBox1.Visible = true;
        ASPxComboBox1.DataBind();
        if (customSwitch1.Checked == true)
        {
            //Response.Write("i got here!!!");
            lblBenOrBankList.Text = "Beneficiary List:";
            drpBenList.Visible = true;
            //  drpBenList.dat
            lblBankName.Visible = true;
            txtBenAccountNo.ReadOnly = true;
            ASPxComboBox1.Visible = false;

        }
    }
    protected void txtBenAccountNo_TextChanged(object sender, EventArgs e)
    {

        LoadBenDetails();
        LoadBeneficiaryList();
        //lblBenName.Text =  iPoints.Proc
    }

    public string ValidateTransfer()
    {
        string strResult = "success";
        if (errMsg.Text != "")
        {
            strResult = "wrong transaction parameters";
            errMsg.Text = strResult;
        }
        if (lblBenName.Text == "")
        {

            strResult = "wrong beneficiary name/account";
            errMsg.Text = strResult;
        }

        //string strBancode = System.Configuration.ConfigurationManager.ConnectionStrings["CompanyBrandCode"].ToString();
        //if ((strBancode == ASPxComboBox1.Value.ToString()) && (txtBenAccountNo.Text == drpAccountNo.SelectedItem.GetValue("AccountNo").ToString()))
        //{
        //    strResult = "Beneficiary account no and Sender account no cannot be same";
        //    errMsg.Text = strResult;
        //}
        return strResult;
    }

    protected void btnTransfer_Click(object sender, EventArgs e)
    {
        luser = HttpContext.Current.User.Identity.Name;
        StrLocation = luser.Substring(luser.LastIndexOf("?") + 1);
        struser = luser.Substring(0, luser.LastIndexOf("?"));
        gUser = struser;

        if (ValidateTransfer() == "success")
        {
            if (customSwitch1.Checked == false)
            {
                string RefNo = ""; string TransType = "009";
                string strRespCode = ""; string strRespDesc = "";
                string strResults = "";
                //strResults = iBankPoints.iPoints.ProcessLocalBenName(txtBenAccountNo.Text, "00", "001", "00");
                string strAccountName = "";
                // drpAccountNo.Columns[2].Visible = true;
                strAccountName = drpAccountNo.SelectedItem.GetValue("AccountDesc").ToString();
                string PaymentID = Guid.NewGuid().ToString().Substring(0, 12).ToUpper().Replace("-", "");
                RefNo = iPoints.ProcessTempTrans(txtBenAccountNo.Text, ASPxComboBox1.Value.ToString(), iPoints.GetIPAddress(),
                    StrLocation, drpAccountNo.Value.ToString(), strAccountName, decimal.Parse(txtAmount.Text),
                    decimal.Parse(txtFee.Text), lblBenName.Text, txtNarration.Text, ASPxComboBox1.Text.Trim(), gUser, PaymentID, TransType, Request.Cookies["Token"].Value);

                JavaScriptSerializer jss = new JavaScriptSerializer();
                dynamic json = jss.DeserializeObject(RefNo);
                strRespCode = json["ResponseCode"];
                strRespDesc = json["ResponseDesc"];
                if (strRespCode != "")
                {
                    Response.Redirect(string.Format("TransactionView.aspx?RefNo={0}", strRespCode));
                }
            }
            else
            {
                string RefNo = ""; string TransType = "002";
                string strRespCode = ""; string strRespDesc = "";
                string strResults = "";
                //strResults = iBankPoints.iPoints.ProcessLocalBenName(txtBenAccountNo.Text, "00", "001", "00");
                if (drpBenList == null)
                {
                    errMsg.Text = "Select saved beneficiary details";
                    return;
                }
                string PaymentID = Guid.NewGuid().ToString().Substring(0, 12).ToUpper().Replace("-", "");
                RefNo = iPoints.ProcessTempTrans(txtBenAccountNo.Text, ASPxComboBox1.Value.ToString(), iPoints.GetIPAddress(),
                    "", drpAccountNo.Value.ToString(), drpAccountNo.SelectedItem.Text, decimal.Parse(txtAmount.Text),
                    decimal.Parse(txtFee.Text), lblBenName.Text, txtNarration.Text, ASPxComboBox1.Text.Trim(), Request.Cookies["LogonUserID"].Value, PaymentID, TransType, Request.Cookies["Token"].Value);

                JavaScriptSerializer jss = new JavaScriptSerializer();
                dynamic json = jss.DeserializeObject(RefNo);
                strRespCode = json["ResponseCode"];
                strRespDesc = json["ResponseDesc"];
                if (strRespCode != "")
                {
                    Response.Redirect(string.Format("TransactionView.aspx?RefNo={0}", strRespCode));
                }


            }

        }
        else
        {
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("OtherBankSht.aspx");
    }
}