using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using NETClass;
using System.Net;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Configuration;


public partial class NDD_NDDBulkServicePush :  System.Web.UI.Page
{
    public string luser;
    public string StrLocation;
    public string struser;
    public string guser;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack && !IsCallback)
        {
            //luser = User.Identity.Name;// User.Identity.Name;
            //StrLocation =  luser.Substring(this.luser.LastIndexOf("?") + 1);
            //struser =  luser.Substring(0, this.luser.LastIndexOf("?"));
            //guser = struser;
            // txtUserid.Text = HttpContext.Current.User.Identity.Name;

        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string strConsource = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString;


        DataTable dtRequest = new DataTable(); DataSet ds = new DataSet();
        SqlConnection ConnNDDSource = new SqlConnection(strConsource);
        string strselInfo = string.Format("SpNDDeMandateGetInfo '3'");
        SqlDataAdapter cmdAdapCallReq = new SqlDataAdapter(strselInfo, ConnNDDSource);
        cmdAdapCallReq.Fill(dtRequest);


        foreach (DataRow drRs in dtRequest.Rows)
        {
            string strNDDnarration = ""; string strNDDmandatecode= ""; string strNDDstatus = "";
            //ASPxLabel label = (ASPxLabel)this.grid.FindRowCellTemplateControl(i, column, "ASPxLabel1");
            //ASPxLabel label2 = (ASPxLabel)this.grid.FindRowCellTemplateControl(i, column3, "lblmsg_to");
            //ASPxLabel label3 = (ASPxLabel)this.grid.FindRowCellTemplateControl(i, column2, "lblmsg_text");
            strNDDnarration = drRs["narration"].ToString();
            strNDDmandatecode = drRs["mandateCode"].ToString();
            strNDDstatus = drRs["status"].ToString();
            if (strNDDstatus == "3")
            {
                //if (ConfigurationManager.ConnectionStrings["SmsScheduler"].ConnectionString == "1")
                //{
                //    SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["PushConnect"].ConnectionString);
                //    SqlCommand command = new SqlCommand();
                //    connection.Open();
                //    command.CommandText = "Sproc_SMSScheduler";
                //    command.Connection = connection;
                //    command.CommandType = CommandType.StoredProcedure;
                //    command.ExecuteNonQuery();
                //    connection.Close();
                //}
                //string connectionString = ConfigurationManager.ConnectionStrings["SmsUserName"].ConnectionString;
                //string pwds = ConfigurationManager.ConnectionStrings["SmsPwds"].ConnectionString;
                //string displayname = ConfigurationManager.ConnectionStrings["CompanyBrand"].ConnectionString;

                string str3 = ""; string requestUrl = ""; string MandateCode = strNDDmandatecode;
                //connection2.Close();
                string strSel = string.Format("SpNDDeMandateGetInfo {0}", strNDDstatus);
                SqlConnection connPending = new SqlConnection(ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString);
                SqlDataAdapter commAdapter = new SqlDataAdapter(strSel, connPending);
                commAdapter.Fill(ds);
                string strsel = string.Format("MandateCode ='{0}'", strNDDmandatecode);
                DataRow[] dsfindRows = ds.Tables[0].Select(strsel);
                foreach (DataRow dr in dsfindRows)
                {
                    MandateCode = dr["MandateCode"].ToString();
                }


                #region emandate NDD TSQ on nibss





                string NDDCodeHostUrl = System.Configuration.ConfigurationManager.ConnectionStrings["NDDCodeHostUrl"].ConnectionString;
                requestUrl = string.Format("{0}/{1}", NDDCodeHostUrl, "eMandateStatusTSQ");






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

                string RequestTimeStamp = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm:ss+0000");

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string ljson = new JavaScriptSerializer().Serialize(new
                    {

                        MandateCode

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

                var serializerSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Ignore
                };

                JavaScriptSerializer jssp = new JavaScriptSerializer();
                //dynamic jsonp = jssp.DeserializeObject(result);


                var jsonSmsInfo = JsonConvert.DeserializeObject(result, serializerSettings); string workflowStatus = "";
                dynamic jsonp = jssp.DeserializeObject(result);
                AddtoLogFile(result, "emandate status Check");
                string respcode = ""; string data = ""; string respdesc = ""; string mandatestatus = ""; string sessionID = ""; string transactionId = ""; string paymentReference = "";
                respcode = jsonp["responseCode"];
                respdesc = jsonp["responseText"];
                workflowStatus = jsonp["data"]["data"]["workflowStatus"];
                MandateCode = jsonp["data"]["data"]["mandateCode"];

                if (respcode == "00" && workflowStatus == "Bank Approved")
                {

                    AddtoLogFile(result, "emandatecode Checked for Recovery");
                    #region emandate NDD TSQ on nibss




                    string str30 = ""; string EasypayrequestUrl = "";
                    string NDDEasypayHostUrl = System.Configuration.ConfigurationManager.ConnectionStrings["NDDCodeHostUrl"].ConnectionString;
                    EasypayrequestUrl = string.Format("{0}/{1}", NDDEasypayHostUrl, "eMandateApproval");




                    AddtoLogFile(EasypayrequestUrl, "Easypay Url");

                    #region Initiate NDD Recovery Info -- FT  emandate Easypy recovery status



                    var httpRecWebRequest = (HttpWebRequest)WebRequest.Create(EasypayrequestUrl);

                    httpRecWebRequest.ContentType = "application/json";

                    httpRecWebRequest.Method = "POST";
                    httpRecWebRequest.Headers.Add("AuthToken", strAuthToken);
                    httpRecWebRequest.Headers.Add("TransToken", strAuthToken);

                    #endregion

                    using (var streamRecWriter = new StreamWriter(httpRecWebRequest.GetRequestStream()))
                    {
                        string ljson = new JavaScriptSerializer().Serialize(new
                        {

                            MandateCode

                        });
                        // json = Convert.ToBase64String(Encoding.Default.GetBytes(json));
                        streamRecWriter.Write(ljson);
                        streamRecWriter.Flush();
                        streamRecWriter.Close();
                    }

                    var httpRecResponse = (HttpWebResponse)httpRecWebRequest.GetResponse();
                    var Recresult = "";
                    using (var streamRecReader = new StreamReader(httpRecResponse.GetResponseStream()))
                    {
                        Recresult = streamRecReader.ReadToEnd();
                    }
                    httpRecResponse.Close();
                    JavaScriptSerializer jsspRec = new JavaScriptSerializer();
                    var jsonSmsRecInfo = JsonConvert.DeserializeObject(Recresult, serializerSettings);
                    dynamic jsonpRec = jsspRec.DeserializeObject(Recresult);
                    AddtoLogFile(Recresult, "emandate Recovery on Easypay");


                    //string data = ""; string respdesc = ""; string mandatestatus = "";
                    respcode = jsonpRec["responseCode"];
                    respdesc = jsonpRec["responseText"];
                    //workflowStatus = jsonp["data"]["data"]["workflowStatus"];

                    if (respcode == "00")
                    {

                        sessionID = jsonpRec["data"]["sessionID"];

                        transactionId = jsonpRec["data"]["transactionId"];
                        paymentReference = jsonpRec["data"]["paymentReference"];

                        #region Execute Mandate Update 

                        //
                        string strCons = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString;

                        SqlConnection Conn = new SqlConnection(strCons);


                        SqlCommand cmd = new SqlCommand();
                        Conn.Open();
                        //SqlParameter result;
                        cmd.CommandText = "SpGetNDDeMandateUpd";
                        cmd.Connection = Conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@mandateCode", MandateCode));

                        cmd.Parameters.Add(new SqlParameter("@sessionID", sessionID));
                        cmd.Parameters.Add(new SqlParameter("@status", 5));



                        cmd.ExecuteNonQuery();
                        Conn.Close();


                        SqlConnection ConnReset = new SqlConnection(strCons);
                        SqlCommand cmdReset = new SqlCommand();
                        ConnReset.Open();
                        //SqlParameter result;
                        cmdReset.CommandText = "spNDDMandateRptstatus";
                        cmdReset.Connection = ConnReset;
                        cmdReset.CommandType = CommandType.StoredProcedure;
                        cmdReset.Parameters.Add(new SqlParameter("@mandateCode", MandateCode));

                        cmdReset.Parameters.Add(new SqlParameter("@responseCode", respcode));
                        cmdReset.Parameters.Add(new SqlParameter("@responseDesc", respdesc));



                        cmdReset.ExecuteNonQuery();
                        ConnReset.Close();

                        string msg = string.Format("NDDMandate FT Recovery on Mandate- {0}; SessionID: {1}; TransactionID {2}:paymentReference :{3};Recovery:{4}", MandateCode, sessionID, transactionId, paymentReference, "True");
                        Classes.RecordOp(String.Format("Sproc_AddAuditLog '044','{0}','{1}',''", msg, "NDDService"));

                        #endregion

                    }
                    else
                    {
                        string strCons = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString;

                        SqlConnection ConnReset = new SqlConnection(strCons);


                        SqlCommand cmdReset = new SqlCommand();
                        ConnReset.Open();
                        //SqlParameter result;
                        cmdReset.CommandText = "spNDDMandateRptstatus";
                        cmdReset.Connection = ConnReset;
                        cmdReset.CommandType = CommandType.StoredProcedure;
                        cmdReset.Parameters.Add(new SqlParameter("@mandateCode", MandateCode));

                        cmdReset.Parameters.Add(new SqlParameter("@responseCode", respcode));
                        cmdReset.Parameters.Add(new SqlParameter("@responseDesc", respdesc));



                        cmdReset.ExecuteNonQuery();
                        ConnReset.Close();


                        AddtoLogFile(Recresult, string.Format("emandate Recovery on Easypay Response-{0}", MandateCode));
                    }

                    #endregion

                }
                else
                {
                    string strCons = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString;

                    SqlConnection ConnReset = new SqlConnection(strCons);


                    SqlCommand cmdReset = new SqlCommand();
                    ConnReset.Open();
                    //SqlParameter result;
                    cmdReset.CommandText = "spNDDMandateRptstatus";
                    cmdReset.Connection = ConnReset;
                    cmdReset.CommandType = CommandType.StoredProcedure;
                    cmdReset.Parameters.Add(new SqlParameter("@mandateCode", MandateCode));

                    cmdReset.Parameters.Add(new SqlParameter("@responseCode", respcode));
                    cmdReset.Parameters.Add(new SqlParameter("@responseDesc", respdesc));



                    cmdReset.ExecuteNonQuery();
                    ConnReset.Close();
                    AddtoLogFile(mandatestatus, string.Format("Mandate Status-{0}", MandateCode));
                }

                #endregion
            }
        }

        Response.Write("<script>alert('Request Done !!!') </script>");
    }




    public static void AddtoLogFile(string Message, string WebPage)
    {
        string LogPath = System.Web.HttpContext.Current.Server.MapPath("~/log/");// ConfigurationManager.AppSettings["LogPath"].ToString();
        string filename = "NDDAppBulkLog_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
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