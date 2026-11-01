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
using System.Threading;
using WebRefVanzo;
using System.Configuration;
using System.Data.SqlClient;

public partial class Approvals_Default :  System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
//        eMaintSeqnum eBatRefNo   accountNo eIssueDate  eExipryDate systemDateTime  status reason  userID approveID   approveDateTime SeqNum  maintType CardInstno  Token requestTitle    pin cvv2    cardmonth LinkedAccountNo MaintLocation Maintenance Desc depCardpin  depcardcvv depcardmonth
//A9FA9DF3 - 8923 - 441E-B42A - CFA12935DCB2    MNT9BDC6FE8073  0000084736  2024 - 01 - 05 05:58:17.000 2024 - 01 - 05 05:58:17.000 2022 - 06 - 03 07:51:44.780 0   Precard Linking hp NULL    NULL        100008  50612402028800000005714 NULL Card Linking    6666    666 7956    0000084736  NULL Link Card   1111    111 2401

        string strResults = ICardProcess.Linkcards("0082", "femi", "Iteieiwiewiewew", "ADADDWWEWE",
                              "1", "50612402028800000005714", "1111", "7956", "551", "0000084736");
        Response.Write(strResults);
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
         //string obj = "{\"responseCode\":\"89\",\"responseText\":\"emandate creation response details with issues\",\"data\":{\"errors\":{\"startDate\":[\"Start date cannot be in the past\"]},\"type\":\"https://tools.ietf.org/html/rfc7231#section-6.5.1\",\"title\":\"One or more validation errors occurred.\",\"status\":400,\"traceId\":\"00-6a91bca13da19967190ec93680156146-973285931c2f1cad-00\"}}";

        string obj = "{\"responseCode\":\"00\",\"responseText\":\"Success\",\"data\":{\"responseMessage\":\"Success\",\"responseCode\":\"00\",\"data\":{\"mandateCode\":\"5767726/35/4762918761\",\"subscriberCode\":\"20250408T1141578392\",\"phoneNumber\":\"08034473783\",\"description\":\"Welcome to NIBSS e-mandate authentication service, a seamless and convenient authentication experience. Kindly proceed with a token payment of N50.00 into account number 9880218357 with Paystack-Titan Bank. This payment will trigger the  authentication of your mandate. Thank You\"}}}";


        //string obj = "{\"responseCode\":\"89\",\"responseText\":\"emandate creation response details with issues\",\"data\":{\"errors\":{\"accountName\":[\"Invalid parameter entered for AccountName\"]},\"type\":\"https://tools.ietf.org/html/rfc7231#section-6.5.1\",\"title\":\"One or more validation errors occurred.\",\"status\":400,\"traceId\":\"00-90a4023e6f0b335cb333abdcccbc98e2-9d850942984298d9-00\"}}";


        var serializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore
        };
        JavaScriptSerializer jssp = new JavaScriptSerializer();
        var jsonSmsInfo = JsonConvert.DeserializeObject(obj, serializerSettings);
        dynamic jsonp = jssp.DeserializeObject(obj);

        string respcode = ""; string data = ""; string respdesc = "";
        respcode = jsonp["responseCode"];
        respdesc = jsonp["responseText"];
        //AddtoLogFile(result, "ResultResponse");
       

        if (respcode == "00")
        {
            //string billerId = jsonp["data"]["id"]; string productId = "";


            #region bind merchant collection acction

            string mandateCode = "";
            if (respcode == "00")


            {

                //AddtoLogFile(jsonp["data"], "emandate Justmain Response data ");
                //AddtoLogFile(jsonp["data"]["mandateCode"], "emandate main Response data ");
                //AddtoLogFile(jsonp["data"]["data"]["mandateCode"], "emandate Response data mandateCode ");

                //mandateCode = jsonp["data"]["data"]["mandateCode"];
                //SqlConnection connValidate = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString);
                string valresult = jsonp["data"]["mandateCode"].ToString();
                Response.Write(valresult);

                //string strins = string.Format("SpUpdAppNDDeMandate '{0}','{1}','{2}','{3}','{4}'", gUser, key, mandateCode, "3", respdesc);
                //NETClass.Classes.GetDataset(strins);

            }





            #endregion





        }
        else
        {
            string  jsonstringErrs = JsonConvert.SerializeObject(jsonp["data"]["errors"], serializerSettings);


            Response.Write(jsonstringErrs);
            JavaScriptSerializer jsspk = new JavaScriptSerializer();
            dynamic jsonp3 = jsspk.Serialize(jsonp["data"]["errors"]);

            //var result = jsonp["data"]["errors"]["startDate"];
            string valresult = string.Format("{0}", jsonp3);
            //string valresult = string.Format("{0}", jsonp["data"]["errors"].ToString());
            //string valresult = jsonp["data"]["errors"]["startDate"][0].ToString();
            //ststatus += respdesc;

            Response.Write(valresult);
        }

    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        string respcode = ""; string data = ""; string respdesc = ""; string mandatestatus = ""; string sessionID = ""; string transactionId = ""; string paymentReference = "";
        var serializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore
        };

        string obj = "{\"responseCode\":\"00\",\"responseText\":\"Success\", \"data\":{\"responseCode\":\"00\",\"sessionID\":\"999999250526000650198505409646\",\"transactionId\":\"100024250526000816000003401306\",\"channelCode\":1,\"nameEnquiryRef\":\"999999250526000650198505409646\",\"destinationInstitutionCode\":\"100024\",\"beneficiaryAccountName\":\"OLAOSEBIKAN ADEYEMI\",\"beneficiaryAccountNumber\":\"0000093440\",\"beneficiaryKYCLevel\":\"1\",\"beneficiaryBankVerificationNumber\":\"\",\"originatorAccountName\":null,\"originatorAccountNumber\":\"2875353019\",\"originatorBankVerificationNumber\":\"22000000090\",\"originatorKYCLevel\":\"3\",\"transactionLocation\":\"1.38716,3.05117\",\"narration\":\"LOAN REPAYMENT IFO ADEYEMI OLAOSEBIKAN 0000080404\",\"paymentReference\":\"NIPMINI/100024250526000816000003401306/0000080404\",\"amount\":228468.97}}";

        JavaScriptSerializer jsspRec = new JavaScriptSerializer();
        var jsonSmsRecInfo = JsonConvert.DeserializeObject(obj, serializerSettings);
        dynamic jsonpRec = jsspRec.DeserializeObject(obj);
        //AddtoLogFile(Recresult, "emandate Recovery on Easypay");


        //string data = ""; string respdesc = ""; string mandatestatus = "";
        respcode = jsonpRec["responseCode"];
        respdesc = jsonpRec["responseText"];
        //workflowStatus = jsonp["data"]["data"]["workflowStatus"];

                if (respcode == "00")
                {

                    sessionID = jsonpRec["data"]["sessionID"];

                    transactionId = jsonpRec["data"]["transactionId"];
                    paymentReference = jsonpRec["data"]["paymentReference"];
                }

        Response.Write(sessionID + "</br>");
        Response.Write(transactionId);
    }
}