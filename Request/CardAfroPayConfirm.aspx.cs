using System.Linq;
using System.Web;


using System.IO;
using System.Data;
using System.Text;
using System.Net;
using System.Web.UI;
using System.Web.Script.Serialization;
using System.Web.Security;

using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Xml;
using System;
//using System.Net.Http;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Reflection;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1;
using System.Globalization;
using DevExpress.Web;
public partial class Request_CardAfroPayConfirm :  System.Web.UI.Page
{
    public string gUser = HttpContext.Current.User.Identity.Name;//HttpContext.Current.User.Identity.Name;
    public string luser = HttpContext.Current.User.Identity.Name;
    public string StrLocation = "";
    public string struser = "";

    public static void AddtoLogFile(string Message, string WebPage)
    {
        string LogPath = System.Web.HttpContext.Current.Server.MapPath("~/log/");// ConfigurationManager.AppSettings["LogPath"].ToString();
        string filename = "AfriLog_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
        string filepath = LogPath + filename;
        if (File.Exists(filepath))
        {
            using (StreamWriter writer = new StreamWriter(filepath, true))
            {
                writer.WriteLine("——————-START————-" + DateTime.Now);
                writer.WriteLine("Source :" + WebPage);
                writer.WriteLine(Message);
                writer.WriteLine("——————-END————-" + DateTime.Now);
            }
        }
        else
        {
            StreamWriter writer = File.CreateText(filepath);
            writer.WriteLine("——————-START————-" + DateTime.Now);
            writer.WriteLine("Source :" + WebPage);
            writer.WriteLine(Message);
            writer.WriteLine("——————-END————-" + DateTime.Now);
            writer.Close();
        }
    }
    public static void AddtoCardFile(string Message, string WebPage)
    {
        string LogPath = System.Web.HttpContext.Current.Server.MapPath("~/log/cardFile/");// ConfigurationManager.AppSettings["LogPath"].ToString();

        //foreach (DataRow item in NETClass.Classes.GetDataset("Sproc_GetPref").Tables[0].Rows)
        //{
        //    LogPath = item["AfriGopayApiKey"].ToString();

        //}
        string filename = "AfriCardFile_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
        string filepath = LogPath + filename;
        if (File.Exists(filepath))
        {
            using (StreamWriter writer = new StreamWriter(filepath, true))
            {
                //writer.WriteLine("——————-START————-" + DateTime.Now);
                //writer.WriteLine("Source :" + WebPage);
                writer.WriteLine(Message);
                //writer.WriteLine("——————-END————-" + DateTime.Now);
            }
        }
        else
        {
            StreamWriter writer = File.CreateText(filepath);
            //writer.WriteLine("——————-START————-" + DateTime.Now);
            //writer.WriteLine("Source :" + WebPage);
            writer.WriteLine(Message);
            //writer.WriteLine("——————-END————-" + DateTime.Now);
            writer.Close();
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        char[] delimiterChars = { ',' };
        string[] words = null;// txtAcctList.Text.Split(delimiterChars);

        string strConchk = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnectSource"].ConnectionString;
        string strConeBiz = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString;


        SqlConnection Connchk = new SqlConnection(strConchk); DataSet chkds = new DataSet();

        SqlConnection ConnchkVal = new SqlConnection(strConchk); DataSet chkdsval = new DataSet();
        SqlCommand cmdchkval = new SqlCommand();

        SqlConnection Connbiz = new SqlConnection(strConeBiz);
        SqlCommand cmdbiz = new SqlCommand();
        

        DataTable dtAcctListFrows = new DataTable();
        
        GridViewDataColumn columnSeqNum = (GridViewDataColumn)ctrlGrid.Columns["SeqNum"];
        columnSeqNum.Visible = true;
        foreach (var key in ctrlGrid.GetSelectedFieldValues("SeqNum"))
        {
            if (ctrlGrid.Selection.IsRowSelectedByKey(key))
            {
                
                //ctrlGrid.GetRowValuesByKeyValue(key, "payerPhoneNumber").ToString();
                string panNo = ""; string panNoEncrypted = ""; string strAccountNo = ""; string panNoMasked = ""; string plainPanno = "";
                DataTable dtAcctList = new DataTable();
                string BatchRefNo = ctrlGrid.GetRowValuesByKeyValue(key, "RefNo").ToString();
                string SeqNo = ctrlGrid.GetRowValuesByKeyValue(key, "SeqNum").ToString();
                panNo = ctrlGrid.GetRowValuesByKeyValue(key, "TrackID").ToString();
                strAccountNo = ctrlGrid.GetRowValuesByKeyValue(key, "AccountNo").ToString();
                panNoMasked = ctrlGrid.GetRowValuesByKeyValue(key, "PANno").ToString();
                string sCardIssueDate = ctrlGrid.GetRowValuesByKeyValue(key, "CardIssueDate").ToString();
                string sCardExpiryDate = ctrlGrid.GetRowValuesByKeyValue(key, "CardExpiryDate").ToString();

                panNoEncrypted = panNo;

                #region Add Auth Info
                string requestUrl = "";
                string AfroGOPAYHost = System.Configuration.ConfigurationManager.ConnectionStrings["AfroGOPAYHost"].ConnectionString;
                requestUrl = string.Format("{0}/{1}", AfroGOPAYHost, "Cards/BookCard");
                string HeaderUsername = System.Configuration.ConfigurationManager.ConnectionStrings["AfroGOPAYID"].ConnectionString;
                string Headerpwds = System.Configuration.ConfigurationManager.ConnectionStrings["AfroGOPAYKey"].ConnectionString;

                string strDeptHeadUserName = NETClass.Classes.DecryptString(HeaderUsername);
                string strDeptHeadpwds = NETClass.Classes.DecryptString(Headerpwds);

                string strAuthToken = Convert.ToBase64String(Encoding.Default.GetBytes(string.Format("{0}:{1}", strDeptHeadUserName, strDeptHeadpwds)));

                //var httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUrl);

                //httpWebRequest.ContentType = "application/json";

                //httpWebRequest.Method = "POST";
                //httpWebRequest.Headers.Add("AuthToken", strAuthToken);


                #endregion

                if (panNo != "")
                {


                    //    Cmd.CommandText = "Sproc_ApproveCardRequestDisConfirm"
                    //Cmd.Connection = sqlConn
                    //Cmd.CommandType = CommandType.StoredProcedure

                    //sqlConn.Open()

                //    Cmd.Parameters.Add(New SqlParameter("@AccountNo", AccountNo.Text))
                //        Cmd.Parameters.Add(New SqlParameter("@CardIssueDate", ctrlGrid.GetRowValues(rowIndex, "CardIssueDate")))
                //Cmd.Parameters.Add(New SqlParameter("@CardExipryDate", ctrlGrid.GetRowValues(rowIndex, "CardExpiryDate")))

                //Cmd.Parameters.Add(New SqlParameter("@PANno", AccountNo.ToolTip))

                //Cmd.Parameters.Add(New SqlParameter("@CardCounter", CardCounter))
                //Cmd.Parameters.Add(New SqlParameter("@status", 20))


                //Cmd.Parameters.Add(New SqlParameter("@RefNo", drpBatchRefNo.Value))

                //Cmd.Parameters.Add(New SqlParameter("@ComfirmBy", guser))




                    DateTime CardIssueDate; DateTime CardExipryDate;
                    CardIssueDate = DateTime.Parse(sCardIssueDate);

                    CardExipryDate = DateTime.Parse(sCardExpiryDate);
                    SqlDataAdapter cmdAdapterAcctUpd = new SqlDataAdapter("Sproc_ApproveCardRequestDisConfirm", Connbiz);

                    cmdAdapterAcctUpd.SelectCommand.CommandType = CommandType.StoredProcedure;
                 
                    cmdAdapterAcctUpd.SelectCommand.Parameters.Add(new SqlParameter("@accountNo", strAccountNo));
                    cmdAdapterAcctUpd.SelectCommand.Parameters.Add(new SqlParameter("@CardIssueDate", CardIssueDate));
                    cmdAdapterAcctUpd.SelectCommand.Parameters.Add(new SqlParameter("@CardExipryDate", CardExipryDate));
                    cmdAdapterAcctUpd.SelectCommand.Parameters.Add(new SqlParameter("@PANno", panNoMasked));
                    cmdAdapterAcctUpd.SelectCommand.Parameters.Add(new SqlParameter("@CardCounter", 1));
                    cmdAdapterAcctUpd.SelectCommand.Parameters.Add(new SqlParameter("@status", 20));
                    cmdAdapterAcctUpd.SelectCommand.Parameters.Add(new SqlParameter("@RefNo", BatchRefNo));
                    cmdAdapterAcctUpd.SelectCommand.Parameters.Add(new SqlParameter("@ComfirmBy", gUser));

                    cmdAdapterAcctUpd.Fill(dtAcctList);


                }
            }

            // write to file the Cardfile
        }
        columnSeqNum.Visible = false;
        ctrlGrid.DataBind();
    }

    protected void Button4_Click(object sender, EventArgs e)
    {
         
        Response.Redirect("CardAfroPayConfirm.aspx");
    }


    protected void Button5_Click(object sender, EventArgs e)
    {
        char[] delimiterChars = { ',' };
        string[] words = null;// txtAcctList.Text.Split(delimiterChars);

        string strConchk = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnectSource"].ConnectionString;
        string strConeBiz = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString;


        SqlConnection Connchk = new SqlConnection(strConchk); DataSet chkds = new DataSet();

        SqlConnection ConnchkVal = new SqlConnection(strConchk); DataSet chkdsval = new DataSet();
        SqlCommand cmdchkval = new SqlCommand();

        SqlConnection Connbiz = new SqlConnection(strConeBiz);
        SqlCommand cmdbiz = new SqlCommand();
        //SqlDataAdapter cmdAdapterchkval = new SqlDataAdapter("SpAcctMasterSelectOne", Connchk);
        //SqlDataAdapter cmdAdapterchkval = new SqlDataAdapter("spAfrigoPayCardAcctsAll", Connchk);

        //cmdAdapterchkval.SelectCommand.CommandType = CommandType.StoredProcedure;


        //cmdAdapterchkval.Fill(dtAcctList);

        DataTable dtAcctListFrows = new DataTable();
        //DataRow[] findrows = dtAcctListFrows.Select(string.Format("AccountNo in ({0})", ""));
        //foreach (var key in ctrlGrid.GetSelectedFieldValues("RefNo"))
        //foreach (DataRow item in findrows)
        GridViewDataColumn columnSeqNum = (GridViewDataColumn)ctrlGrid.Columns["SeqNum"];
        columnSeqNum.Visible = true;
        foreach (var key in ctrlGrid.GetSelectedFieldValues("SeqNum"))
        {
            if (ctrlGrid.Selection.IsRowSelectedByKey(key))
            {
                //System.Console.WriteLine($"<{word}>");
                //if (ctrlGrid.Selection.IsRowSelectedByKey(key))
                //{
                //}
                //ctrlGrid.GetRowValuesByKeyValue(key, "payerPhoneNumber").ToString();
                string panNo = ""; string panNoEncrypted = "";string strAccountNo = ""; string panNoMasked = ""; string plainPanno = "";
                DataTable dtAcctList = new DataTable();
                string BatchRefNo = ctrlGrid.GetRowValuesByKeyValue(key, "RefNo").ToString();
                string SeqNo = ctrlGrid.GetRowValuesByKeyValue(key, "SeqNum").ToString();
                  panNo = ctrlGrid.GetRowValuesByKeyValue(key, "TrackID").ToString();
                strAccountNo = ctrlGrid.GetRowValuesByKeyValue(key, "AccountNo").ToString();
                panNoMasked = ctrlGrid.GetRowValuesByKeyValue(key, "PANno").ToString();
                string sCardIssueDate= ctrlGrid.GetRowValuesByKeyValue(key, "CardIssueDate").ToString();
                string sCardExpiryDate = ctrlGrid.GetRowValuesByKeyValue(key, "CardExpiryDate").ToString();

                panNoEncrypted = panNo;

                #region Add Auth Info
                string requestUrl = "";
                string AfroGOPAYHost = System.Configuration.ConfigurationManager.ConnectionStrings["AfroGOPAYHost"].ConnectionString;
                requestUrl = string.Format("{0}/{1}", AfroGOPAYHost, "Cards/BookCard");
                string HeaderUsername = System.Configuration.ConfigurationManager.ConnectionStrings["AfroGOPAYID"].ConnectionString;
                string Headerpwds = System.Configuration.ConfigurationManager.ConnectionStrings["AfroGOPAYKey"].ConnectionString;

                string strDeptHeadUserName = NETClass.Classes.DecryptString(HeaderUsername);
                string strDeptHeadpwds = NETClass.Classes.DecryptString(Headerpwds);

                string strAuthToken = Convert.ToBase64String(Encoding.Default.GetBytes(string.Format("{0}:{1}", strDeptHeadUserName, strDeptHeadpwds)));

                //var httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUrl);

                //httpWebRequest.ContentType = "application/json";

                //httpWebRequest.Method = "POST";
                //httpWebRequest.Headers.Add("AuthToken", strAuthToken);


                #endregion
             
                if (panNo != "")
                {


                    string cardNo = panNo;
                    // Decrypt cardinfo
                    //panNo = AesDecrypt(panNoEncrypted, "tnWvYXRLaARMQrtQubsfzbaUcaRQBABV");
                    string ActrequestUrl = string.Format("{0}/{1}", AfroGOPAYHost, "Cards/ActivateEncryptedCard"); ;
                    var httpWebActRequest = (HttpWebRequest)WebRequest.Create(ActrequestUrl);

                    httpWebActRequest.ContentType = "application/json";

                    httpWebActRequest.Method = "POST";
                    httpWebActRequest.Headers.Add("AuthToken", strAuthToken);

                    using (var streamWriter = new StreamWriter(httpWebActRequest.GetRequestStream()))
                    {
                        string ljson = new JavaScriptSerializer().Serialize(new
                        {

                            cardNo



                        });
                        // json = Convert.ToBase64String(Encoding.Default.GetBytes(json));
                        streamWriter.Write(ljson);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                    var serializerSettings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        DefaultValueHandling = DefaultValueHandling.Ignore
                    };
                    var httpActResponse = (HttpWebResponse)httpWebActRequest.GetResponse();
                    var Activateresult = "";
                    using (var streamReader = new StreamReader(httpActResponse.GetResponseStream()))
                    {
                        Activateresult = streamReader.ReadToEnd();
                    }

                    httpActResponse.Close();
                    // section Activate card info
                    string ActivationResp = "";

                    try
                    {
                        JavaScriptSerializer jssp = new JavaScriptSerializer();
                        //dynamic jsonp = jssp.DeserializeObject(result);


                        var jsonSmsInfo = JsonConvert.DeserializeObject(Activateresult, serializerSettings);
                        dynamic jsonp = jssp.DeserializeObject(Activateresult);

                        string respcode = ""; string data = ""; string respdesc = "";
                        respcode = jsonp["responseCode"];
                        respdesc = jsonp["responseText"];


                        if (respcode == "00")
                        {
                            panNoEncrypted = jsonp["statusMessage"]["crd_no_cipher"];
                            ActivationResp = jsonp["data"];
                            plainPanno = jsonp["panNo"];
                        }
                    }
                    catch (Exception)
                    {

                        //throw;
                    }







                    string cardfileRec = string.Format("{0},{1},{2}", panNoMasked, strAccountNo, ActivationResp);


                    AddtoLogFile(cardfileRec, "CardfileInfo Activated");

                     
                    DateTime CardIssueDate; DateTime CardExipryDate;
                    CardIssueDate = DateTime.Parse(sCardIssueDate);

                    CardExipryDate = DateTime.Parse(sCardExpiryDate);
                    SqlDataAdapter cmdAdapterAcctUpd = new SqlDataAdapter("Sproc_ApproveCardRequestConfirm", Connbiz);

                    cmdAdapterAcctUpd.SelectCommand.CommandType = CommandType.StoredProcedure;
                    cmdAdapterAcctUpd.SelectCommand.Parameters.Add(new SqlParameter("@eBatRefNo", BatchRefNo));
                    cmdAdapterAcctUpd.SelectCommand.Parameters.Add(new SqlParameter("@ComfirmBy", gUser));
                    cmdAdapterAcctUpd.SelectCommand.Parameters.Add(new SqlParameter("@panno", plainPanno));
                    
                    cmdAdapterAcctUpd.SelectCommand.Parameters.Add(new SqlParameter("@CardCounter", 1));
                    cmdAdapterAcctUpd.SelectCommand.Parameters.Add(new SqlParameter("@accountNo", strAccountNo));
                    cmdAdapterAcctUpd.SelectCommand.Parameters.Add(new SqlParameter("@CardIssueDate", CardIssueDate));
                    cmdAdapterAcctUpd.SelectCommand.Parameters.Add(new SqlParameter("@CardExipryDate", CardExipryDate));
                    
                    cmdAdapterAcctUpd.Fill(dtAcctList);


                }
            }

            // write to file the Cardfile
        }
        columnSeqNum.Visible = false;
        ctrlGrid.DataBind();
    }



}