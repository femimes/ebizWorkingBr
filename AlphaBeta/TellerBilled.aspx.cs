using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Script.Serialization;
using System.Net.Mail;

using Newtonsoft.Json;
 


using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using SendGrid.Helpers.Mail;
using System.Net;

public partial class AlphaBeta_TellerBilled : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack && !IsCallback)
        {
            hypdashboard.NavigateUrl = "../mainpage/homeboard.aspx";
            //drpAccountNo.DataBind();
            //ASPxComboBox1.DataBind();
            drpTypes.Value = "WEBGUID";
            clearParameters();
            

        }
    }

    protected void drpAccountNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        drpTypes.Value = "WEBGUID";
    }
    void ValidateRefNoOnly()
    {


        clearParameters();
        DataTable dsAcctList = new DataTable();
        //dsAcctList = iBankPoints.iPoints.ProcessAphaBetaCustRefNo(Request.Cookies["Token"].Value, txtBenAccountNo.Text);
        //dsAcctList = iBankPoints.iPoints.ProcessAphaBetaCustRefNo("10", txtBenAccountNo.Text);

        string strResults = iBankPoints.iPoints.ProcessAphaBetaCustRefNos("10", txtBenAccountNo.Text);
        string strRespCode = ""; string strRespDesc = "";
        //var x = Newtonsoft.Json.Linq.JObject.Parse(iBankPoints.iPoints.ProcessAphaBetaCustRefNos("10", txtBenAccountNo.Text));
        //string expression;  // expression = "Date > #1/1/00#";
        //expression = string.Format("Pid  = {0}", txtBenAccountNo.Text);

        //DataRow[] foundRows;
        //foundRows = x.se(expression);
        if (strResults != "")
        {
            try
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                dynamic json = jss.DeserializeObject(strResults);
                strRespCode = json["ResponseCode"];
                strRespDesc = json["ResponseDesc"];

                if (strRespCode == "SUCCESS")
                {
                    LblPayID.Text = string.Format(": {0}", json["Pid"].ToString());
                    LblPayName.Text = string.Format(": {0}", json["Fullname"].ToString());
                }







            }
            catch (Exception ex)
            {

                LblPayID.Text = string.Format(": {0}", "Wrong ID");
                LblPayName.Text = string.Format(": {0}", "Invalid Reference");
            }

        }
    }


    protected void BtnValidate_Click(object sender, EventArgs e)

    {
        try
        {

            clearParameters();
            string strResults = iBankPoints.iPoints.ProcessAphaBetaCustRefNos("10", txtBenAccountNo.Text);
            string strRespCode = ""; string strRespDesc = ""; string WebGuid = txtBenAccountNo.Text;string agencodeText = "";

            #region Generate webGuid Payment Parameters

            string strWebGuidPayVal = iBankPoints.iPoints.ProcessAphaBetaValWebGuid("10", WebGuid, drpTypes.Value.ToString());//, drpAccountNo.Value.ToString(), TransDateTimeStamp, strPayRefNo);
            JavaScriptSerializer jssWebGuidVal = new JavaScriptSerializer();
            dynamic jsonWebGuidVal = jssWebGuidVal.DeserializeObject(strWebGuidPayVal);
            if (strWebGuidPayVal.Contains("FAILED") == true)
            {
                strRespCode = jsonWebGuidVal["status"];
                strRespDesc = jsonWebGuidVal["statusMessage"];
            }
            else
            {

                drpAccountNo.Value = jsonWebGuidVal["AgencyCode"];
                drpRevDetails.Value = jsonWebGuidVal["RevenueCode"];
                //agencodeText = drpAccountNo.Text;
                txtAmount.Text = jsonWebGuidVal["AmountDue"];
                LblPayID.Text = jsonWebGuidVal["Pid"];
                LblPayName.Text = jsonWebGuidVal["PayerName"];
                lblAmount.Text = jsonWebGuidVal["AmountDue"];
                strRespCode = jsonWebGuidVal["Status"];
                strRespDesc = jsonWebGuidVal["StatusMessage"];
                lblAccountNInfo.Attributes["AgencyCode"] = jsonWebGuidVal["AgencyCode"];
                lblAccountNInfo.Text = drpAccountNo.Text;
            }



            #endregion


            errMsg.Text = strRespDesc;


            //drpAccountNo.Text = agencodeText;
        }
        catch (Exception ex)
        {

            errMsg.Text = string.Format("'{0}''{1}'", "Request Cannot be Processed", ex.Message);
        }
        finally
        {

            //errMsg.Text = "Processing...";
        }

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("TellerBilled.aspx");
    }
    void clearParameters()
    {
        errMsg.Text = "";
        LblPayID.Text = "";
        LblPayName.Text = "";
        lblWebGuid.Text = "";
        btnReceipts.Enabled = false;
        lblPaymentRefNo.Text = "";
        lblAmount.Text = string.Format("NGN {0}", "0.00");
        //txtAmount.Enabled = false;
        drpAccountNo.Value = "";
        drpRevDetails.Value = "";
        txtPeriod.Text = "";
        drpAccountNo.ReadOnly = true;
        drpRevDetails.Enabled = false;
        //drpAccountNo.Enabled = false;
        hypReceipt.NavigateUrl = "#";
        lblAccountNInfo.Text = "";

    }

    void LoadReceiptsDetails(string WebGuid, string type)
    {
        string strRespCode = ""; string strRespDesc = ""; string strWebGuid = "";

        string strResults = iBankPoints.iPoints.ProcessAphaBetaValWebGuid("10", WebGuid, drpTypes.Value.ToString());

        JavaScriptSerializer jss = new JavaScriptSerializer();
        dynamic json = jss.DeserializeObject(strResults);
        strRespCode = json["ResponseCode"];
        strRespDesc = json["ResponseDesc"];

        if (strRespCode == "SUCCESS")
        {

        }


    }


    protected void btnPayBills_Click(object sender, EventArgs e)
    {

        try
        {


        

            //clearParameters();
                string strResults = iBankPoints.iPoints.ProcessAphaBetaCustRefNos("10", txtBenAccountNo.Text);
            string strRespCode = ""; string strRespDesc = ""; string WebGuid = txtBenAccountNo.Text; string agencodeText = "";

           

            string strWebGuidPayVal = iBankPoints.iPoints.ProcessAphaBetaValWebGuid("10", WebGuid, drpTypes.Value.ToString());//, drpAccountNo.Value.ToString(), TransDateTimeStamp, strPayRefNo);
            JavaScriptSerializer jssWebGuidVal = new JavaScriptSerializer();
            dynamic jsonWebGuidVal = jssWebGuidVal.DeserializeObject(strWebGuidPayVal);
            if (strWebGuidPayVal.Contains("FAILED") == true)
            {
                strRespCode = jsonWebGuidVal["status"];
                strRespDesc = jsonWebGuidVal["statusMessage"];
            }
            else
            {

                drpAccountNo.Value = jsonWebGuidVal["AgencyCode"];
                drpRevDetails.Value = jsonWebGuidVal["RevenueCode"];
                //agencodeText = drpAccountNo.Text;
                //txtAmount.Text = jsonWebGuidVal["AmountDue"];
                LblPayID.Text = jsonWebGuidVal["Pid"];
                LblPayName.Text = jsonWebGuidVal["PayerName"];
                lblAmount.Text = jsonWebGuidVal["AmountDue"];
                strRespCode = jsonWebGuidVal["Status"];
                strRespDesc = jsonWebGuidVal["StatusMessage"];


                #region Generate webGuid Payment Parameters
                string RequestTimeStamp = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm:ss+0000");
                string TransDateTimeStamp = txtPeriod.Text; //DateTime.Now.ToString("yyyy'-'MMMM'-'dd'T'HH':'mm:ss+0000");

                string RefNoSub = Guid.NewGuid().ToString().Substring(0, 8).ToUpper().Replace("-", "");
                string strBrand = System.Configuration.ConfigurationManager.ConnectionStrings["CompanyBrand"].ConnectionString;
                string strPayRefNo = string.Format("{0}{1}{2}", strBrand, RefNoSub, RequestTimeStamp.ToUpper().Replace("-", "").Replace("+", "").Replace(":", ""));

                if ((drpTypes.Value.ToString().ToUpper() == "WebGuid".ToUpper()) || (drpTypes.Value.ToString().ToUpper() == "CBS".ToUpper()))
                {
                    if (double.Parse(txtAmount.Text) > double.Parse(lblAmount.Text))
                    {
                        errMsg.Text = string.Format("{0} ", "amount to pay cannot be greater than amount due");
                        return;
                    }

                }
                lblPaymentRefNo.Text = strPayRefNo;

                if (strRespCode != "")
                {
                    #region Generate Online Passcode

                    string strSMSNo = "";
                    string l_email = "";

                    string EmailSenderAddress = System.Configuration.ConfigurationManager.ConnectionStrings["EmailSenderAddress"].ToString();
                    if (l_email == "NILL")
                    {
                        l_email = EmailSenderAddress;
                    }
                    if (l_email.Contains("@") == false)
                    {
                        l_email = EmailSenderAddress;
                    }


                    string l_newPassword = Guid.NewGuid().ToString().Substring(0, 8).ToLower();
                    //string strBrand = System.Configuration.ConfigurationManager.ConnectionStrings["CompanyBrand"].ConnectionString;
                    string strSMSUserName = System.Configuration.ConfigurationManager.ConnectionStrings["SmsUserName"].ConnectionString;
                    string strSMSPwds = System.Configuration.ConfigurationManager.ConnectionStrings["SmsPwds"].ConnectionString;


                    //string EmailSvrDomainName = System.Configuration.ConfigurationManager.ConnectionStrings["SMTPDomainName"].ToString();
                    string EmailserverUsr = System.Configuration.ConfigurationManager.ConnectionStrings["EmailUserName"].ToString();
                    string EmailserverPwd = System.Configuration.ConfigurationManager.ConnectionStrings["EmailPwds"].ToString();
                    string EmailServerPort = System.Configuration.ConfigurationManager.ConnectionStrings["EazyEmailServerPort"].ToString();
                    string EmailServer = System.Configuration.ConfigurationManager.ConnectionStrings["EazyEmailServer"].ConnectionString;
                    string StrCompanyBrand = System.Configuration.ConfigurationManager.ConnectionStrings["CompanyBrand"].ToString();


                    System.Net.Mail.MailAddress strMsgfrom = new MailAddress(EmailSenderAddress, StrCompanyBrand);


                    string emailmsg = string.Format("<html><body> <font style='font-family: Century Gothic'>Dear {2},</b> </br>  <p>Thank you for using our Retail Internet Banking service </br>  Your default password is: <strong> {0}</strong> </br></b> </br> If you have any  difficulty logging on please contact {7} immediately.</br>Thank you for using  {3} Retail Internet Banking Platform! </p></font></body></html>", l_newPassword, "UserName ", "LastName:", strBrand, EmailSenderAddress);




                    //string smsmsg = string.Format("Your User Account is Processed. UserName:{0} Password:{1}", l_email, l_passWrd);
                    MailMessage message = new MailMessage(EmailSenderAddress, l_email, string.Format("{0} Retail Internet Banking Passord Recovery Notification ", StrCompanyBrand), emailmsg);
                    message.From = strMsgfrom;
                    message.IsBodyHtml = true;

                    NetworkCredential basicCredential =
                           //  new NetworkCredential(EmailserverUsr, EmailserverPwd, EmailSvrDomainName);
                           new NetworkCredential(EmailserverUsr, EmailserverPwd);
                    SmtpClient client = new SmtpClient(EmailServer, int.Parse(EmailServerPort)) { Credentials = basicCredential };
                    client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    string EmailProvider = System.Configuration.ConfigurationManager.ConnectionStrings["EmailProvider"].ToString();
                    if (EmailProvider != "SendGrid")
                    {
                        client.Send(message);
                    }
                    else
                    {

                        var apiKey = System.Configuration.ConfigurationManager.ConnectionStrings["EmailApiKey"].ToString(); // "Environment.GetEnvironmentVariable("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");
                        var clientsp = new SendGrid.SendGridClient(apiKey);
                        var from = new EmailAddress(EmailSenderAddress, StrCompanyBrand);
                        var subject = string.Format("{0} Retail Internet Banking Passord Recovery Notification ", StrCompanyBrand); // strSubject;
                        var to = new EmailAddress(l_email, l_email);
                        var plainTextContent = emailmsg;
                        var htmlContent = emailmsg;
                        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                        var response = clientsp.SendEmailAsync(msg);
                    }


                    #endregion
                    Response.Redirect(string.Format("ABConfirmation.aspx?RefNo={0}&q={1}&p={2}&r={3}&s={4}&a={5}", WebGuid, txtPeriod.Text, strPayRefNo,drpTypes.Value,txtPeriod.Text,txtAmount.Text));
                }





                #endregion



            }

            errMsg.Text = strRespDesc;







        }
        catch (Exception ex)
        {

            errMsg.Text = string.Format("'{0}''{1}'", "Request Cannot be Processed", ex.Message);
        }
        finally
        {

            //errMsg.Text = "Processing...";
        }
    }

    protected void btnReceipts_Click(object sender, EventArgs e)
    {
        string strRespCode = ""; string strRespDesc = ""; string strWebGuid = "";

        string strResults = iBankPoints.iPoints.ProcessAphaBetaPayRefReceipts("10", lblPaymentRefNo.Text);

        JavaScriptSerializer jss = new JavaScriptSerializer();
        dynamic json = jss.DeserializeObject(strResults);
        strRespCode = json["Status"];
        strRespDesc = json["StatusMessage"];
        string strurl = "";

        if (strRespCode == "SUCCESS")
        {
            strurl = json["Receipt"];
            Response.Write(strurl);
        }

    }

}