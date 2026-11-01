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
 


public partial class AlphaBeta_ABConfirmation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack && !IsCallback)
        {
            //txtPin.Text = "";
            lblwebRefNo.Text = string.Format("{0} Reference: {1}", Request.QueryString["r"], Request.QueryString["RefNo"]);
            lblAmount.Text = string.Format("NGN {0:0,0.00}", Request.QueryString["a"]);
        }
    }

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        if (this.Page.IsValid == true)
        {
            string RefNo = ""; string TransType = "101";
            string strRespCode = ""; string strRespDesc = "";
            string strResults = ""; DataTable dt = new DataTable();
            {
                ////                "PIN":"3333","AccountNo":"0000081182","creditAccountNo":"0000081292","mobileNo":null,
                //"Location":null,"amount":100,"Fee":10.75,"narration":"TestLocal","UserName":"femmes","creditBankcode":"090003"}
                string AccountNo = "";
                string creditAccountNo = "";
                string mobileNo = "";
                string Location = "";
                decimal amount = 0;
                decimal Fee = 0;
                string narration = "";
                string UserName = "";
                string creditBankcode = "";
                string Pid = "";
                string PidName = "";
                string AgencyCode = "";
                string strPayRefNo = "";
                

                string strWebGldVal = iBankPoints.iPoints.ProcessAphaBetaValWebGuid(Request.Cookies["Token"].Value, Request.QueryString["RefNo"], Request.QueryString["r"]);//, drpAccountNo.Value.ToString(), TransDateTimeStamp, strPayRefNo);
                JavaScriptSerializer jssWebGldVal = new JavaScriptSerializer();
                dynamic jsonWebGldVal = jssWebGldVal.DeserializeObject(strWebGldVal);
              
                if (strWebGldVal.Contains("FAILED") == true)
                {
                    strRespCode = jsonWebGldVal["status"];
                    strRespDesc = jsonWebGldVal["statusMessage"];
                }
                else
                {
                    strRespCode = jsonWebGldVal["Status"];
                    strRespDesc = jsonWebGldVal["StatusMessage"];
                }

                if (strWebGldVal.Contains("FAILED") == false)
                {
                    creditAccountNo = jsonWebGldVal["CreditAccount"];
                    lblwebRefNo.Attributes["creditAccountNo"] = creditAccountNo;
                    AgencyCode = jsonWebGldVal["AgencyCode"].ToString();
                    Pid = jsonWebGldVal["Pid"].ToString();
                    //jsonWebGldVal["AmountDue"].ToString()
                    amount = decimal.Parse(Request.QueryString["a"]);

                    PidName = jsonWebGldVal["PayerName"].ToString();
                    lblwebRefNo.Attributes["Pid"] = Pid;
                    lblwebRefNo.Attributes["PidName"] = PidName;
                    strPayRefNo = Request.QueryString["p"];//  jsonWebGldVal["PayerName"].ToString();



                    #region Section PreValidate Bank Payment details information
                 //   dt = iBankPoints.iPoints.ProcessProfileds(Request.Cookies["eLogonUserID"].Value, Request.Cookies["Token"].Value);
                 // get Teller Till Accounts for the debittransaction
                    string strAccountName = "";
                    //AccountNo = string.Format("{0}", jsonUser["AccountNo"].ToString());
                    foreach (DataRow item in dt.Rows)
                    {
                        AccountNo = item["AccountNo"].ToString();
                        strAccountName = item["user_names"].ToString();
                    }

                    string strResultsPre = "";

                    // Check or validate account for posting Teller
                    //strResultsPre = iPoints.ProcessLocalFTValidate(psword.Text, AccountNo, creditAccountNo, mobileNo, Location, amount, Fee, narration, Request.Cookies["eLogonUserID"].Value, creditBankcode, Request.Cookies["Token"].Value);
                    JavaScriptSerializer jssPre = new JavaScriptSerializer();
                    dynamic jsonPre = jssPre.DeserializeObject(strResultsPre);
                    strRespCode = jsonPre["ResponseCode"];
                    strRespDesc = jsonPre["ResponseDesc"];

                    if (strRespCode == "00")
                    { 
                      #region Generate webGuid Payment Parameters

                    string strWebGuidPayVal = iBankPoints.iPoints.ProcessAphaBetaValWebGuid("10", Request.QueryString["RefNo"], Request.QueryString["r"]);//, drpAccountNo.Value.ToString(), TransDateTimeStamp, strPayRefNo);
                    JavaScriptSerializer jssWebGuidVal = new JavaScriptSerializer();
                    //dynamic jsonWebGuidVal = jssWebGuid.DeserializeObject(strWebGuidPayVal);

                    #region Start payment Notification and Invoicing
                    string strBrand = System.Configuration.ConfigurationManager.ConnectionStrings["CompanyBrand"].ConnectionString;

                    string RequestTimeStamp = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm:ss+0000");
                    //string TransDateTimeStamp = txtPeriod.Text; //DateTime.Now.ToString("yyyy'-'MMMM'-'dd'T'HH':'mm:ss+0000");
                    //string RefNoSub = Guid.NewGuid().ToString().Substring(0, 8).ToUpper().Replace("-", "");
                    //string strBrand = System.Configuration.ConfigurationManager.ConnectionStrings["CompanyBrand"].ConnectionString;
                    // = string.Format("{0}{1}{2}", strBrand, RefNoSub, RequestTimeStamp);
             

                  
                             #region Generate Payment Infomation

                    string strPay = iBankPoints.iPoints.ProcessAphaBetaPaymentNotify("10", Request.QueryString["RefNo"], "3654122", "BackOffice", strBrand, Request.QueryString["s"], Request.QueryString["q"], Convert.ToDouble(amount), creditAccountNo, strPayRefNo);
                    JavaScriptSerializer jssPay = new JavaScriptSerializer();
                    dynamic jsonpay = jssPay.DeserializeObject(strPay);
                    // strRespCode = jsonpay["status"];

                    if (strPay.Contains("FAILED") == true)
                    {
                        strRespCode = jsonpay["status"];
                        strRespDesc = jsonpay["statusMessage"];
                    }
                    else
                    {
                        strRespCode = jsonpay["Status"];
                        strRespDesc = jsonpay["StatusMessage"];
                    }
                    if (strRespCode == "SUCCESS")
                    {

                        narration = string.Format("{0};{1};{2}", Request.QueryString["RefNo"], Pid, AgencyCode);
                        Response.Cookies["ReceiptNumber"].Value = jsonpay["ReceiptNumber"];
                        #region get login user parameters

                    //    dt = iBankPoints.iPoints.ProcessProfileds(Request.Cookies["eLogonUserID"].Value, Request.Cookies["Token"].Value);
                        JavaScriptSerializer jssUser = new JavaScriptSerializer();
                        dynamic jsonUser = jssUser.DeserializeObject(strResults);
                        //string strAccountName = ""; 
                        //AccountNo = string.Format("{0}", jsonUser["AccountNo"].ToString());
                        foreach (DataRow item in dt.Rows)
                        {
                            AccountNo = item["AccountNo"].ToString();
                            strAccountName = item["user_names"].ToString();
                        }
                            //strAccountNo = jsonUser["AccountNo"];
                        //AccountNo = Convert.ToString(jsonUser["AccountNo"]);
                        #endregion

                        string strRenueAcct = creditAccountNo;
                        string strSortCode = Pid;string TransRefNoVal = "";

                        RefNo = iPoints.ProcessTempTrans(strRenueAcct, strSortCode, iPoints.GetIPAddress(),
                    "", AccountNo, strAccountName, amount, Fee, strBrand, narration, PidName, Request.Cookies["eLogonUserID"].Value, strPayRefNo, TransType, Request.Cookies["Token"].Value);
                        JavaScriptSerializer jssSeqRef = new JavaScriptSerializer();
                        dynamic jsonSeqRef = jssSeqRef.DeserializeObject(RefNo);
                        TransRefNoVal = jsonSeqRef["ResponseCode"];



                        #region Pass EazybankAX entries



                        strResults = iPoints.ProcessLocalFTTeller(psword.Text, AccountNo, strRenueAcct, mobileNo, Location, amount, Fee, narration, Request.Cookies["eLogonUserID"].Value, creditBankcode, Request.Cookies["Token"].Value);
                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        dynamic json = jss.DeserializeObject(strResults);
                        strRespCode = json["ResponseCode"];
                        strRespDesc = json["ResponseDesc"];
                        if (strRespCode == "00")
                        {
                            
                         Response.Redirect(string.Format("ABCGenReceipt.aspx?RefNo={0}&q={1}", strPayRefNo, TransRefNoVal));

                        }
                        else
                        {
                         litEmmorMsg.Text = strRespDesc;
                        }
                        
                        #endregion


                    }
                    else
                    {

                        litEmmorMsg.Text = strRespDesc;
                    }

                    #endregion


                    #endregion

                    #endregion
                    }
                    else
                    {
                        litEmmorMsg.Text = strRespDesc;
                    }



                    #endregion


                }
                else
                {

                    if (strWebGldVal.Contains("has been fully paid") == true)
                    {
                        string strBrand = System.Configuration.ConfigurationManager.ConnectionStrings["CompanyBrand"].ConnectionString;

                        #region Generate Payment Infomation
                        creditAccountNo = lblwebRefNo.Attributes["creditAccountNo"];
                        Pid = lblwebRefNo.Attributes["Pid"];
                        PidName = lblwebRefNo.Attributes["PidName"];
                        amount = decimal.Parse(Request.QueryString["a"]);

                        //PidName = jsonWebGldVal["PayerName"].ToString();
                        strPayRefNo = Request.QueryString["p"];//


                        #region Section PreValidate Bank EazybankAX Payment details information
                      //  dt = iBankPoints.iPoints.ProcessProfileds(Request.Cookies["eLogonUserID"].Value, Request.Cookies["Token"].Value);
                          
                        string strAccountName = "";
                        //AccountNo = string.Format("{0}", jsonUser["AccountNo"].ToString());
                        foreach (DataRow item in dt.Rows)
                        {
                            AccountNo = item["AccountNo"].ToString();
                            strAccountName = item["user_names"].ToString();
                        }
                        string strResultsPre = "";
                        strResultsPre = iPoints.ProcessLocalFTValidate(psword.Text, AccountNo, creditAccountNo, mobileNo, Location, amount, Fee, narration, Request.Cookies["eLogonUserID"].Value, creditBankcode, Request.Cookies["Token"].Value);
                        JavaScriptSerializer jssPre = new JavaScriptSerializer();
                        dynamic jsonPre = jssPre.DeserializeObject(strResultsPre);
                        strRespCode = jsonPre["ResponseCode"];
                        strRespDesc = jsonPre["ResponseDesc"];

                        if (strRespCode == "00")
                        {

                            string strPay = iBankPoints.iPoints.ProcessAphaBetaPaymentNotify("10", Request.QueryString["RefNo"], "3654122", "BackOffice", strBrand, Request.QueryString["s"], Request.QueryString["q"], Convert.ToDouble(amount), creditAccountNo, strPayRefNo);
                            JavaScriptSerializer jssPay = new JavaScriptSerializer();
                            dynamic jsonpay = jssPay.DeserializeObject(strPay);
                            // strRespCode = jsonpay["status"];

                            if (strPay.Contains("FAILED") == true)
                            {
                                strRespCode = jsonpay["status"];
                                strRespDesc = jsonpay["statusMessage"];
                            }
                            else
                            {
                                strRespCode = jsonpay["Status"];
                                strRespDesc = jsonpay["StatusMessage"];
                            }
                            if (strRespCode == "SUCCESS")
                            {

                                narration = string.Format("{0};{1};{2}", Request.QueryString["RefNo"], Pid, AgencyCode);
                                Response.Cookies["ReceiptNumber"].Value = jsonpay["ReceiptNumber"];
                                #region get login user parameters

                               // dt = iBankPoints.iPoints.ProcessProfileds(Request.Cookies["eLogonUserID"].Value, Request.Cookies["Token"].Value);
                                JavaScriptSerializer jssUser = new JavaScriptSerializer();
                                dynamic jsonUser = jssUser.DeserializeObject(strResults);
                                //string strAccountName = "";
                                //AccountNo = string.Format("{0}", jsonUser["AccountNo"].ToString());
                                foreach (DataRow item in dt.Rows)
                                {
                                    AccountNo = item["AccountNo"].ToString();
                                    strAccountName = item["user_names"].ToString();
                                }
                                //strAccountNo = jsonUser["AccountNo"];
                                //AccountNo = Convert.ToString(jsonUser["AccountNo"]);
                                #endregion
                                DataTable dtGL = new DataTable(); string strRenueAcct = "NA";
                                //dtGL = iBankPoints.iPoints.ProcessGetParamGL("922", Request.Cookies["eLogonUserID"].Value, Request.Cookies["Token"].Value);
                                //foreach (DataRow item in dtGL.Rows)
                                //{
                                //    strRenueAcct = item["Value"].ToString();
                                //}
                                creditAccountNo = lblwebRefNo.Attributes["creditAccountNo"];
                                strRenueAcct = creditAccountNo;
                                string strSortCode = Pid; string TransRefNoVal = "";

                                RefNo = iPoints.ProcessTempTrans(strRenueAcct, strSortCode, iPoints.GetIPAddress(),
                            "", AccountNo, strAccountName, amount, Fee, strBrand, narration, PidName, Request.Cookies["eLogonUserID"].Value, strPayRefNo, TransType, Request.Cookies["Token"].Value);
                                JavaScriptSerializer jssSeqRef = new JavaScriptSerializer();
                                dynamic jsonSeqRef = jssSeqRef.DeserializeObject(RefNo);
                                TransRefNoVal = jsonSeqRef["ResponseCode"];



                                #region Pass EazybankAX entries



                                strResults = iPoints.ProcessLocalFTTeller(psword.Text, AccountNo, strRenueAcct, mobileNo, Location, amount, Fee, narration, Request.Cookies["eLogonUserID"].Value, creditBankcode, Request.Cookies["Token"].Value);
                                JavaScriptSerializer jss = new JavaScriptSerializer();
                                dynamic json = jss.DeserializeObject(strResults);
                                strRespCode = json["ResponseCode"];
                                strRespDesc = json["ResponseDesc"];
                                if (strRespCode == "00")
                                {

                                    Response.Redirect(string.Format("ABCGenReceipt.aspx?RefNo={0}&q={1}", strPayRefNo, TransRefNoVal));

                                }
                                else
                                {
                                    litEmmorMsg.Text = strRespDesc;
                                }

                                #endregion


                            }
                            else
                            {

                                litEmmorMsg.Text = strRespDesc;
                            }

                        }
                        else
                        {
                            litEmmorMsg.Text = strRespDesc;
                        }
                        #endregion
                        #endregion



                    }
                    else
                    {
                            litEmmorMsg.Text = strRespDesc;
                    }


                            

                }


              
            }
        }
    }


   
}