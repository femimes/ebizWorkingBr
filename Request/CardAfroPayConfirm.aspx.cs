using System.Linq;
using System.Web;

using System.IO;
using System.Data;
using System.Text;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Web.Security;

using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Xml;
using System;
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
                writer.WriteLine("\u2014\u2014\u2014\u2014\u2014\u2014-START\u2014\u2014\u2014\u2014-" + DateTime.Now);
                writer.WriteLine("Source :" + WebPage);
                writer.WriteLine(Message);
                writer.WriteLine("\u2014\u2014\u2014\u2014\u2014\u2014-END\u2014\u2014\u2014\u2014-" + DateTime.Now);
            }
        }
        else
        {
            StreamWriter writer = File.CreateText(filepath);
            writer.WriteLine("\u2014\u2014\u2014\u2014\u2014\u2014-START\u2014\u2014\u2014\u2014-" + DateTime.Now);
            writer.WriteLine("Source :" + WebPage);
            writer.WriteLine(Message);
            writer.WriteLine("\u2014\u2014\u2014\u2014\u2014\u2014-END\u2014\u2014\u2014\u2014-" + DateTime.Now);
            writer.Close();
        }
    }
    public static void AddtoCardFile(string Message, string WebPage)
    {
        string LogPath = System.Web.HttpContext.Current.Server.MapPath("~/log/cardFile/");// ConfigurationManager.AppSettings["LogPath"].ToString();

        string filename = "AfriCardFile_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
        string filepath = LogPath + filename;
        if (File.Exists(filepath))
        {
            using (StreamWriter writer = new StreamWriter(filepath, true))
            {
                writer.WriteLine(Message);
            }
        }
        else
        {
            StreamWriter writer = File.CreateText(filepath);
            writer.WriteLine(Message);
            writer.Close();
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        RunAction("Remove selection", delegate { Button3_Work(sender, e); });
    }

    private void Button3_Work(object sender, EventArgs e)
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
        foreach (var key in SelectedKeys())
        {
            if (key != null)
            {
                
                string panNo = ""; string panNoEncrypted = ""; string strAccountNo = ""; string panNoMasked = ""; string plainPanno = "";
                DataTable dtAcctList = new DataTable();
                string BatchRefNo = RowValue(key, "RefNo").ToString();
                string SeqNo = RowValue(key, "SeqNum").ToString();
                panNo = RowValue(key, "TrackID").ToString();
                strAccountNo = RowValue(key, "AccountNo").ToString();
                panNoMasked = RowValue(key, "PANno").ToString();
                string sCardIssueDate = RowValue(key, "CardIssueDate").ToString();
                string sCardExpiryDate = RowValue(key, "CardExpiryDate").ToString();

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

                #endregion

                if (panNo != "")
                {

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
        ctrlGrid.DataBind();
    }

    protected void Button4_Click(object sender, EventArgs e)
    {
         
        Response.Redirect("CardAfroPayConfirm.aspx");
    }

    protected void Button5_Click(object sender, EventArgs e)
    {
        RunAction("Activate cards", delegate { Button5_Work(sender, e); });
    }

    private void Button5_Work(object sender, EventArgs e)
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
        foreach (var key in SelectedKeys())
        {
            if (key != null)
            {
                string panNo = ""; string panNoEncrypted = "";string strAccountNo = ""; string panNoMasked = ""; string plainPanno = "";
                DataTable dtAcctList = new DataTable();
                string BatchRefNo = RowValue(key, "RefNo").ToString();
                string SeqNo = RowValue(key, "SeqNum").ToString();
                  panNo = RowValue(key, "TrackID").ToString();
                strAccountNo = RowValue(key, "AccountNo").ToString();
                panNoMasked = RowValue(key, "PANno").ToString();
                string sCardIssueDate= RowValue(key, "CardIssueDate").ToString();
                string sCardExpiryDate = RowValue(key, "CardExpiryDate").ToString();

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

                #endregion
             
                if (panNo != "")
                {

                    string cardNo = panNo;
                    // Decrypt cardinfo
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
        ctrlGrid.DataBind();
    }


    // =====================================================================
    // Page plumbing for the plain GridView (selection, row values, feedback)
    // =====================================================================

    /// <summary>Keys (SeqNum) of the rows ticked on the current page.</summary>
    private List<string> SelectedKeys()
    {
        List<string> keys = new List<string>();
        foreach (GridViewRow row in ctrlGrid.Rows)
        {
            if (row.RowType != DataControlRowType.DataRow) continue;
            CheckBox chk = row.FindControl("chkSelect") as CheckBox;
            if (chk != null && chk.Checked) keys.Add(Convert.ToString(ctrlGrid.DataKeys[row.RowIndex].Value));
        }
        return keys;
    }

    private DataView _rows;

    /// <summary>A column of the listed row with this key ("" when the row or column is missing).</summary>
    private object RowValue(object key, string column)
    {
        if (_rows == null) _rows = SqlDataSource1.Select(DataSourceSelectArguments.Empty) as DataView;
        if (_rows == null || !_rows.Table.Columns.Contains(column)) return "";
        foreach (DataRowView r in _rows)
        {
            if (Convert.ToString(r["SeqNum"]) == Convert.ToString(key)) return r[column] ?? "";
        }
        return "";
    }

    /// <summary>Runs an action for the ticked rows and reports the outcome.</summary>
    private void RunAction(string what, Action work)
    {
        int count = SelectedKeys().Count;
        if (count == 0) { ShowError("Tick at least one row first."); return; }
        try
        {
            work();
            ShowSuccess(string.Format("{0}: done for {1} {2}{3}. Details are in the log file.", what, count, "card", count == 1 ? "" : "s"));
        }
        catch (System.Threading.ThreadAbortException) { throw; }
        catch (Exception ex)
        {
            ShowError(what + " stopped: " + ex.Message);
        }
        _rows = null;
        ctrlGrid.DataBind();
    }

    private void ShowSuccess(string message)
    {
        litResult.Text = HttpUtility.HtmlEncode(message);
        Literal1.Text = "";
    }

    private void ShowError(string message)
    {
        Literal1.Text = HttpUtility.HtmlEncode(message);
        litResult.Text = "";
    }

    protected void ctrlGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ctrlGrid.PageIndex = e.NewPageIndex;
    }

    protected void SqlDataSource1_Selected(object sender, SqlDataSourceStatusEventArgs e)
    {
        litTotal.Text = e.Exception == null ? string.Format("{0:N0} {1}{2}", e.AffectedRows, "card", e.AffectedRows == 1 ? "" : "s") : "";
    }

    /// <summary>Shows each batch as "reference - request title".</summary>
    protected void drpBatchRefNo_DataBound(object sender, EventArgs e)
    {
        DataView view = SqlDataSource2.Select(DataSourceSelectArguments.Empty) as DataView;
        if (view == null || !view.Table.Columns.Contains("RequestTitle")) return;
        foreach (DataRowView r in view)
        {
            ListItem item = drpBatchRefNo.Items.FindByValue(Convert.ToString(r["RefNo"]));
            string title = Convert.ToString(r["RequestTitle"]);
            if (item != null && title != "") item.Text = item.Value + " - " + title;
        }
    }
}
