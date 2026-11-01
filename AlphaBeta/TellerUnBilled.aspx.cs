using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Script.Serialization;

public partial class AlphaBeta_TellerUnBilled : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack && !IsCallback)
        {
            //drpAccountNo.DataBind();
            //ASPxComboBox1.DataBind();
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
            string strPayerID = string.Format("{0}-{1}", drpPayerType.Value, txtBenAccountNo.Text);
            string strResults = iBankPoints.iPoints.ProcessAphaBetaCustRefNos("10", strPayerID);
            string strRespCode = ""; string strRespDesc = ""; string WebGuid = "";


            if (strResults != "")
            {
                try
                {
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    dynamic json = jss.DeserializeObject(strResults);
                    strRespCode = json["Status"];
                    strRespDesc = json["StatusMessage"];

                    if (strRespCode == "SUCCESS")
                    {
                        LblPayID.Text = string.Format(": {0}", json["Pid"].ToString());
                        LblPayName.Text = string.Format(": {0}", json["Fullname"].ToString());

                        #region section make payment

                        //string RequestTimeStamp = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm:ss+0000");
                        //string TransDateTimeStamp = DateTime.Now.ToString("yyyy'-'MMMM'-'dd'T'HH':'mm:ss+0000");

                        //string strBrand = System.Configuration.ConfigurationManager.ConnectionStrings["CompanyBrand"].ConnectionString;
                        //string strPayRefNo = string.Format("{0}{1}", strBrand, RequestTimeStamp);
                        //lblPaymentRefNo.Text = strPayRefNo;
                        //string strWebGuidPay = iBankPoints.iPoints.ProcessAphaBetaPayRefNos("10", txtBenAccountNo.Text, double.Parse("1.00"), drpRevDetails.Value.ToString(), drpAccountNo.Value.ToString(), TransDateTimeStamp, strPayRefNo);
                        //JavaScriptSerializer jssWebGuid = new JavaScriptSerializer();
                        //dynamic jsonWebGuid = jssWebGuid.DeserializeObject(strWebGuidPay);
                        //strRespCode = jsonWebGuid["RespondCode"];
                        //strRespDesc = jsonWebGuid["ResponseDesc"];

                        //if (strRespCode == "SUCCESS")
                        //{
                        //    WebGuid = jsonWebGuid["WebGuid"];

                        //    errMsg.Text = strRespDesc;
                        //    #region Generate webGuid Payment Parameters

                        //    string strWebGuidPayVal = iBankPoints.iPoints.ProcessAphaBetaValWebGuid("10", WebGuid, drpTypes.Value.ToString());//, drpAccountNo.Value.ToString(), TransDateTimeStamp, strPayRefNo);
                        //    JavaScriptSerializer jssWebGuidVal = new JavaScriptSerializer();
                        //    dynamic jsonWebGuidVal = jssWebGuid.DeserializeObject(strWebGuidPayVal);
                        //    if (strWebGuidPayVal.Contains("FAILED") == true)
                        //    {
                        //        strRespCode = jsonWebGuidVal["status"];
                        //        strRespDesc = jsonWebGuidVal["statusMessage"];
                        //    }
                        //    else
                        //    {
                        //        strRespCode = jsonWebGuidVal["Status"];
                        //        strRespDesc = jsonWebGuidVal["StatusMessage"];
                        //    }

                        //    if (strRespCode == "SUCCESS")
                        //    {
                        //        string strCreditAccount = jsonWebGuidVal["CreditAccount"];

                        //        lblAmount.Text = jsonWebGuidVal["AmountDue"];





                        //    }
                        //    else
                        //    {
                        //        errMsg.Text = strRespDesc;

                        //    }


                        //    #endregion



                        //}
                        //else
                        //{

                        //    errMsg.Text = strRespDesc;
                        //}


                        #endregion


                    }
                    else
                    {
                        errMsg.Text = strRespDesc;
                    }

                }
                catch (Exception ex)
                {

                    LblPayID.Text = string.Format(": {0}", "Wrong ID");
                    LblPayName.Text = string.Format(": {0}", "Invalid Reference");
                }

            }


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
        Response.Redirect("TellerUnBilled.aspx");
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
        hypReceipt.NavigateUrl = "#";
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

            clearParameters();
            string strPayerID = string.Format("{0}-{1}", drpPayerType.Value, txtBenAccountNo.Text);
            string strResults = iBankPoints.iPoints.ProcessAphaBetaCustRefNos("10", strPayerID);
            string strRespCode = ""; string strRespDesc = ""; string WebGuid = "";


            if (strResults != "")
            {
                try
                {
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    dynamic json = jss.DeserializeObject(strResults);
                    strRespCode = json["Status"];
                    strRespDesc = json["StatusMessage"];

                    if (strRespCode == "SUCCESS")
                    {
                        LblPayID.Text = string.Format(": {0}", json["Pid"].ToString());
                        LblPayName.Text = string.Format(": {0}", json["Fullname"].ToString());

                        #region section make payment

                        string RequestTimeStamp = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm:ss+0000");
                        string TransDateTimeStamp = txtPeriod.Text; //DateTime.Now.ToString("yyyy'-'MMMM'-'dd'T'HH':'mm:ss+0000");

                        string RefNoSub = Guid.NewGuid().ToString().Substring(0, 8).ToUpper().Replace("-", "");
                        string strBrand = System.Configuration.ConfigurationManager.ConnectionStrings["CompanyBrand"].ConnectionString;
                        //string strPayRefNo = string.Format("{0}{1}{2}", strBrand, RefNoSub, RequestTimeStamp);

                        string strPayRefNo = string.Format("{0}{1}{2}", strBrand, RefNoSub, RequestTimeStamp.ToUpper().Replace("-", "").Replace("+", "").Replace(":", ""));

                        lblPaymentRefNo.Text = strPayRefNo;
                        string strWebGuidPay = iBankPoints.iPoints.ProcessAphaBetaPayRefNos("10", strPayerID, double.Parse(txtAmount.Text), drpRevDetails.Value.ToString(), drpAccountNo.Value.ToString(), TransDateTimeStamp, strPayRefNo);
                        JavaScriptSerializer jssWebGuid = new JavaScriptSerializer();
                        dynamic jsonWebGuid = jssWebGuid.DeserializeObject(strWebGuidPay);
                        strRespCode = jsonWebGuid["RespondCode"];
                        strRespDesc = jsonWebGuid["ResponseDesc"];

                        if (strRespCode == "SUCCESS")
                        {
                            WebGuid = jsonWebGuid["WebGuid"];
                           
                            errMsg.Text = strRespDesc;
                            if (strRespCode != "")
                            {
                                Response.Redirect(string.Format("ABConfirmation.aspx?RefNo={0}&q={1}&p={2}&r={3}&s={4}&a={5}", WebGuid,txtPeriod.Text, strPayRefNo,drpTypes.Value,"cash",txtAmount.Text));
                            }

                            #region Generate webGuid Payment Parameters

                            string strWebGuidPayVal = iBankPoints.iPoints.ProcessAphaBetaValWebGuid("10", WebGuid, drpTypes.Value.ToString());//, drpAccountNo.Value.ToString(), TransDateTimeStamp, strPayRefNo);
                            JavaScriptSerializer jssWebGuidVal = new JavaScriptSerializer();
                            dynamic jsonWebGuidVal = jssWebGuid.DeserializeObject(strWebGuidPayVal);
                            if (strWebGuidPayVal.Contains("FAILED") == true)
                            {
                                strRespCode = jsonWebGuidVal["status"];
                                strRespDesc = jsonWebGuidVal["statusMessage"];
                            }
                            else
                            {

                                 #region Start payment Notification and Invoicing

                            // if (strRespCode == "SUCCESS")
                            //{
                            //    string strCreditAccount = jsonWebGuidVal["CreditAccount"];

                            //    lblAmount.Text = jsonWebGuidVal["AmountDue"];
                            //    errMsg.Text = strRespDesc;

                            //    #region Generate Payment Infomation

                            //    string strPay = iBankPoints.iPoints.ProcessAphaBetaPaymentNotify("10", WebGuid, "3654122", "BackOffice", "Abbey", "Cash", DateTime.Today.Date.ToString("ddMMMyyyy"), double.Parse(txtAmount.Text), strCreditAccount, strPayRefNo);
                            //    JavaScriptSerializer jssPay = new JavaScriptSerializer();
                            //    dynamic jsonpay = jssWebGuid.DeserializeObject(strPay);
                            //    strRespCode = jsonpay["Status"];
                            //    if (jsonpay["Status"] == "SUCCESS")
                            //    {
                            //        lblWebGuid.Text = string.Format("Payment Reference: {0}", strPayRefNo);
                            //        btnReceipts.Enabled = true;
                            //        hypReceipt.NavigateUrl = jsonpay["ReceiptNumber"];
                            //        hypReceipt.Target = "_new";
                            //        //return;
                            //        errMsg.Text = jsonpay["StatusMessage"];
                            //    }
                            //    else
                            //    {

                            //        errMsg.Text = jsonpay["StatusMessage"];
                            //    }

                            //    #endregion


                            //}
                            //else
                            //{
                            //    errMsg.Text = strRespDesc;

                            //}


                            #endregion
                        
                                strRespCode = jsonWebGuidVal["Status"];
                                strRespDesc = jsonWebGuidVal["StatusMessage"];

                            }

                       

                            #endregion



                        }
                        else
                        {

                            errMsg.Text = strRespDesc;
                        }


                        #endregion


                    }
                    else
                    {
                        errMsg.Text = strRespDesc;
                    }

                }
                catch (Exception ex)
                {

                    LblPayID.Text = string.Format(": {0}", "Wrong ID");
                    LblPayName.Text = string.Format(": {0}", "Invalid Reference");
                }

            }

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