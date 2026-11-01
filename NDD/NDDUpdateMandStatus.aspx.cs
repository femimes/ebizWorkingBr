using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
 
using System.Data;
using System.Data.SqlClient;
using System.Net;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Text;

public partial class NDD_NDDUpdateMandStatus :  System.Web.UI.Page
{
    public string luser;
    public string StrLocation;
    public string struser;
    public string guser;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            if (Request.QueryString["q"]== "2")
            {
                btnMandstatus.Text = "Suspend MandateCode";
            }
            if (Request.QueryString["q"] == "3")
            {
                btnMandstatus.Text = "Cancel MandateCode";
            }
            //subMerchload(Request.QueryString["p"]);

        }

    }

    protected void btnMandstatus_Click(object sender, EventArgs e)
    {
        luser = User.Identity.Name;
        luser = User.Identity.Name;// User.Identity.Name;
        string l_getHeadCode = "";
        //StrLocation = luser.Substring(this.luser.LastIndexOf("?") + 1);
        //struser = luser.Substring(0, this.luser.LastIndexOf("?"));
        guser = User.Identity.Name; ;
        string l_SeqNo = Request.QueryString["p"];


        string str3 = ""; string requestUrl = ""; string MandateCode = "";
        string NDDCodeHostUrl = System.Configuration.ConfigurationManager.ConnectionStrings["NDDCodeHostUrl"].ConnectionString;
        requestUrl = string.Format("{0}/{1}", NDDCodeHostUrl, "eMandateStatus");


        #region Add Auth Info -- Get emandate status

        string HeaderUsername = System.Configuration.ConfigurationManager.ConnectionStrings["NDDCodeKeyID"].ConnectionString;
        string Headerpwds = System.Configuration.ConfigurationManager.ConnectionStrings["NDDCodeKey"].ConnectionString;

        string strDeptHeadUserName = NETClass.Classes.DecryptString(HeaderUsername);
        string strDeptHeadpwds = NETClass.Classes.DecryptString(Headerpwds);

        string strAuthToken = Convert.ToBase64String(Encoding.Default.GetBytes(string.Format("{0}:{1}", strDeptHeadUserName, strDeptHeadpwds)));

        var httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUrl);

        httpWebRequest.ContentType = "application/json";

        httpWebRequest.Method = "POST";
        httpWebRequest.Headers.Add("AuthToken", strAuthToken);
        httpWebRequest.Headers.Add("TransToken", strAuthToken);

        #endregion
        Label accountNameLabel = (Label)(DataList1.Items[0].FindControl("accountNameLabel"));
        Label payerAccountNumberLabel = (Label)(DataList1.Items[0].FindControl("payerAccountNumberLabel"));

        string RequestTimeStamp = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm:ss+0000");
        //  "MandateCode": "9532885/1/9266768735",
        //"BillerID": 1,
        //"ProductID": 1,
        //"AccountNumber": "2222222222",
        //"MandateStatus": "1"
        var IcoObject = new
        {
            MandateCode = MandateCode,
            BillerID = 1,
            AccountNumber= payerAccountNumberLabel.Text,
            MandateStatus = Request.QueryString["q"]
        };
        string mandateUpdateRequests = JsonConvert.SerializeObject(IcoObject);
        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        {
            string ljson = new JavaScriptSerializer().Serialize(new
            {

                mandateUpdateRequests

            });
            // json = Convert.ToBase64String(Encoding.Default.GetBytes(json));
            streamWriter.Write(ljson);
            streamWriter.Flush();
            streamWriter.Close();
        }

        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        var result = "";
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            result = streamReader.ReadToEnd();
        }
        httpResponse.Close();

        var serializerSettings = new Newtonsoft.Json.JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore
        };

        JavaScriptSerializer jssp = new JavaScriptSerializer();
        //dynamic jsonp = jssp.DeserializeObject(result);


        var jsonSmsInfo = JsonConvert.DeserializeObject(result, serializerSettings); string workflowStatus = ""; string mandateCodeUpd = "";
        dynamic jsonp = jssp.DeserializeObject(result);
        AddtoLogFile(result, "emandate status Check");
        string respcode = ""; string data = ""; string respdesc = ""; string mandatestatus = ""; string sessionID = ""; string transactionId = ""; string paymentReference = "";
        respcode = jsonp["responseCode"];
        respdesc = jsonp["responseText"];
        mandateCodeUpd = jsonp["data"]["data"]["mandateCode"];
         
        if (respcode =="00")
        {
            Int64 status = 3;
            if (Request.QueryString["q"].ToString() == "2")
            {
                status = 98;
            }
            if (Request.QueryString["q"].ToString() == "3")
            {
                status = 99;
            }

            string strCon = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString;
            SqlConnection Conn = new SqlConnection(strCon);
            DataSet ds = new DataSet();

            SqlCommand cmd = new SqlCommand();
            Conn.Open();
            //SqlParameter result;
            cmd.CommandText = "SpNDDeMandateStatusUpd";
            cmd.Connection = Conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@SeqNo", l_SeqNo));

            cmd.Parameters.Add(new SqlParameter("@status", status));



            cmd.ExecuteNonQuery();
            Conn.Close();

        }




        //string msg = string.Format("Application NDD Create Biller Deleteting on - {0};Merchant name:{1}; contact:{2};account:{3} ", l_SeqNo, name, contact, account_number);
        //Classes.RecordOp(String.Format("Sproc_AddAuditLog '004','{0}','{1}',''", msg, guser));

        //int index = -1;

        //grid.SettingsEditing.Mode = (GridViewEditingMode.EditFormAndDisplayRow);// index;
        //grid.DataBind();
        //DataBind();

        //Response.Redirect("TerminalRegForm.aspx");
    }

    public static void AddtoLogFile(string Message, string WebPage)
    {
        string LogPath = System.Web.HttpContext.Current.Server.MapPath("~/log/");// ConfigurationManager.AppSettings["LogPath"].ToString();
        string filename = "NDDAppLog_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
        string filepath = LogPath + filename;
        if (File.Exists(filepath))
        {
            using (StreamWriter writer = new StreamWriter(filepath, true))
            {
                writer.WriteLine("——————-START————-" + DateTime.Now);
                writer.WriteLine("## :" + WebPage);
                writer.WriteLine(Message);
                writer.WriteLine("——————-END————-" + DateTime.Now);
            }
        }
        else
        {
            StreamWriter writer = File.CreateText(filepath);
            writer.WriteLine("——————-START————-" + DateTime.Now);
            writer.WriteLine("## :" + WebPage);
            writer.WriteLine(Message);
            writer.WriteLine("——————-END————-" + DateTime.Now);
            writer.Close();
        }
    }

}