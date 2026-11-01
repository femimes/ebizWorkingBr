
using ASP;
using DevExpress.Web;
using NETClass;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Diagnostics;
using System.Security.Principal;
using System.Web;
using System.Web.Profile;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Configuration;
using System.Net;
public partial class eOperations_IncomingMsg : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {

       

        SqlConnection connection3 = new SqlConnection(ConfigurationManager.ConnectionStrings["EazyConImages2"].ConnectionString);
        DataSet ksp = new DataSet();
        string infomsg = string.Format("SELECT * FROM incomings order by id ");

        string strMsgDetails = string.Format("SELECT * from  dbo.in_msgs Where msg_status = 0");
        SqlDataAdapter comgetRecs = new SqlDataAdapter(infomsg, connection3);
        SqlDataAdapter cmdMsgDetails = new SqlDataAdapter(strMsgDetails, connection3);
        cmdMsgDetails.Fill(ksp, "TblMsg");
         comgetRecs.Fill(ksp,"TblPrototypeMsg");
        string phoneNo = "0000";
        string strmsginfo = "";


        #region scetion pull new incoming messages
        foreach (DataRow drp in ksp.Tables["TblMsg"].Rows)
        {

            phoneNo = drp["received_from"].ToString();

         #region Region to check for Message Conditions

        foreach (DataRow dr in ksp.Tables["TblPrototypeMsg"].Rows)
        {
            //char strTemp = char.Parse(dr["prototype"].ToString());
            //char[] delimiters = new char[] { '\r', '\n' };
            char[] delimiters = new char[] {' '};
            string strPrototype = dr["prototype"].ToString();

            string msg = drp["msg_text"].ToString();

            string[] strsplitMsg = msg.Split(delimiters, StringSplitOptions.None);
            string[] strsliptProtype = strPrototype.Split(delimiters, StringSplitOptions.None);

            int Iprotypecount = strsliptProtype.Length;
            int Imsgpecount = strsplitMsg.Length;


            //Response.Write(string.Format("Prototype = '{0}' and Message = '{1}' </br>", Iprotypecount,Imsgpecount));
            //Response.Write(string.Format("'{0}' </br>", Iprotypecount));
            if (Iprotypecount == Imsgpecount)
            {
                //Response.Write(string.Format("'{0}' </br>", strsliptProtype.GetValue(0)));
                //Response.Write(string.Format("'{0}' </br>", strsplitMsg.GetValue(0)));
                int i= 0;
                if (strsplitMsg.GetValue(0).ToString() == strsliptProtype.GetValue(0).ToString())
                {


                    string strSenderValidatn = dr["Sender_Conn_details"].ToString();
                    int sqlbegins = strSenderValidatn.LastIndexOf("sql=");
                    string strsenderCmd = "";
                    if (sqlbegins !=  -1)
                    {
                        strsenderCmd = strSenderValidatn.Substring(sqlbegins + 4);
                    }
                    string sqlsmt = strsenderCmd;

                     
                    if (strsenderCmd.Contains("%1"))
                    {
                        sqlsmt = sqlsmt.Replace("%1", strsplitMsg.GetValue(1).ToString());
                    }
                    if (strsenderCmd.Contains("%2"))
                    {
                        sqlsmt = sqlsmt.Replace("%2", strsplitMsg.GetValue(2).ToString());
                    }
                    if (strsenderCmd.Contains("%3"))
                    {
                        sqlsmt = sqlsmt.Replace("%3", strsplitMsg.GetValue(3).ToString());
                    }
                    if (strsenderCmd.Contains("%4"))
                    {
                        sqlsmt = sqlsmt.Replace("%4", strsplitMsg.GetValue(4).ToString());
                    }
                    
                    sqlsmt = sqlsmt.Replace("%phone", phoneNo);
                    string ChkSenderInList = dr["ChkSenderInList"].ToString();
                    //Response.Write(sqlsmt);
                    if (sqlsmt != "" && ChkSenderInList == "1")
                   // if (sqlsmt != "" ) //    if (sqlsmt != "" && dr["ChkSenderInList"] == "1")// checks senders condition 
                    {
                        SqlDataAdapter cmdsqlsmt = new SqlDataAdapter(sqlsmt, connection3);
                        //Response.Write(dr["prototype"]);
                         //Response.Write(sqlsmt);
                        DataSet jsp = new DataSet();
                        cmdsqlsmt.Fill(jsp, "TblSenderCmdChk");
                        if (jsp.Tables["TblSenderCmdChk"].Rows.Count > 0)// rules exist successful
                        {

                            // script to take action
                            #region section to execute all actions to be performed
                            DataSet dsp = new DataSet();
                            string strAtnQuery = string.Format("SELECT * FROM Actions WHERE incomings_id = '{0}' order by action_id ", dr["id"]);
                            SqlDataAdapter cmdAtnsqlsmt = new SqlDataAdapter(strAtnQuery, connection3);
                            cmdAtnsqlsmt.Fill(dsp, "TblActions");



                            // getavaliable acctions to execute

                            foreach (DataRow drActions in dsp.Tables["TblActions"].Rows)
                            {
                                //// identify string for connection

                                string strActionDdl = drActions["action_ddl"].ToString();

                                //get connection string Name
                                int intActionstrName = strActionDdl.IndexOf(";", 0);
                                string strActionstrName = strActionDdl.Substring(0, intActionstrName);
                                if (intActionstrName == -1)
                                {
                                    break;

                                }
                                else
                                {
                                    string connUserName = ConfigurationManager.ConnectionStrings["SmsUserName"].ConnectionString;
                                    string pwds = ConfigurationManager.ConnectionStrings["SmsPwds"].ConnectionString;
                                    string displayname = ConfigurationManager.ConnectionStrings["CompanyBrand"].ConnectionString;
                                    
                                    string newVariable = "</br>";
                                    string ResulfrmTbl = PerformAction(phoneNo, strActionDdl, strActionstrName, strsplitMsg, drActions["description"].ToString(), drActions["action_dml"].ToString(), drActions["action_type"].ToString());
                                    //Response.Write(strActionstrName  + "</br>");
                                    //Response.Write(drActions["action_id"].ToString() + "</br>"); 
                                    Response.Write(ResulfrmTbl + newVariable);

                                    Pushsms(phoneNo, connUserName, pwds, displayname, ResulfrmTbl, phoneNo);
                                }
                                //Response.Write(String.Format("{0}{1}</br>", part, string.Format(" Part index = {0}", i)));


                            }



                            #endregion
                            foreach (string part in strsplitMsg)
                            {

                                //Response.Write(String.Format("{0}{1}</br>", part, string.Format(" Part index = {0}", i)));


                            }
                            i = i + 1;
                        }
                        else //if there is   sender condition to check  and it fails - rules fails
                        {

                        }
                    }
                    else // checks if there is no senders condition
                    {


                    }


                }
            }


            
        }


        #endregion



            // section to update treated msgs


        DataSet lsp = new DataSet();
        string strMsgDetUpd = string.Format("update in_msgs set msg_status = 1 WHERE msg_id = '{0}'",drp["msg_id"]);
        SqlDataAdapter cmdMsgDetupd = new SqlDataAdapter(strMsgDetUpd, connection3);
        cmdMsgDetupd.Fill(lsp, "TblMsgupd");

        //Response.Write(String.Format("Message ID = {0} </br>",drp["msg_id"]));

        }
        #endregion
            
        }
        catch (Exception ex)
        {
            //throw;
            Response.Write(ex.Message);
        }
    }


    static string PerformAction(string phoneNo, string conString, string ConObj, string[] incomingmsg, string strBrand,string strActionTemplate,string strActionType)
    {
        //try
        {

      
        string strPerformAction = conString;
        int sqlbegins = strPerformAction.LastIndexOf("sql=");
        string strActionCmd = "";
        if (sqlbegins != -1)
        {
            strActionCmd = strPerformAction.Substring(sqlbegins + 4);
        }
        string sqlsmt = strActionCmd;
        if (strActionCmd.Contains("%1"))
        {
            sqlsmt = sqlsmt.Replace("%1", incomingmsg.GetValue(1).ToString());
        }
        if (strActionCmd.Contains("%2"))
        {
            sqlsmt = sqlsmt.Replace("%2", incomingmsg.GetValue(2).ToString());
        }
        if ( strActionCmd.Contains("%3"))
        {
            sqlsmt = sqlsmt.Replace("%3", incomingmsg.GetValue(3).ToString());
        }
        if (strActionCmd.Contains("%4"))
        {
            sqlsmt = sqlsmt.Replace("%4", incomingmsg.GetValue(4).ToString());
        }
        sqlsmt = sqlsmt.Replace("%phone", phoneNo);
        string strrowsRec = conString;
        if (sqlsmt != "" && strActionType != "0")
        {
            SqlConnection strByconn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConObj].ConnectionString);
            DataSet ksp = new DataSet();
            SqlDataAdapter cmdsqlsmt = new SqlDataAdapter(sqlsmt, strByconn);
            //Response.Write(dr["prototype"]);
            //Response.Write(sqlsmt);
            cmdsqlsmt.Fill(ksp, "TblActionChk");
            int tblsize = ksp.Tables.Count;

            if (tblsize > 0)
            {
                if (ksp.Tables["TblActionChk"].Rows.Count > 0 && tblsize > 0)// rules exist successful
                {

                    strrowsRec = ksp.Tables["TblActionChk"].TableName;

                    string strgenerateMsg = "";// ksp.Tables["TblActionChk"].Rows[0][""].ToString();
                    char[] delimiters = new char[] { ' ' };
                    string[] strsplitMsg = strActionTemplate.Split(delimiters, StringSplitOptions.None);
                    foreach (string part in strsplitMsg)
                    {
                        string lpart = part;
                        if (part.Contains("{"))
                        {
                            string lheadename = part.Replace("{","").Replace("}","").Replace(",","").Replace(".","").Replace(";","");
                            string lConvertedVal = ksp.Tables["TblActionChk"].Rows[0][lheadename].ToString();
                            lpart = part.Replace(lheadename, lConvertedVal).Replace("{","").Replace("}","");

                        }
                        strgenerateMsg = String.Format("{0} {1}", strgenerateMsg, lpart);
                        //Response.Write(String.Format("{0}{1}</br>", part, string.Format(" Part index = {0}", i)));


                    }
                    return strgenerateMsg;
                }
            }
            else
            {
                return strActionTemplate;
                // returm msg

            }


        }

        else
        {
//send avaliable msg template
            return "";
           // string msgTosend = strActionTemplate;

        }


        return strrowsRec;
        }
       // catch (Exception)
        {

            //throw;
        }
        return "";
       
    }



    public static string Pushsms(string PhoneNo, string username, string pwds, string displayname, string msg, string msgto)
    {
        WebRequest request = WebRequest.Create(string.Format("http://193.105.74.59/api/sendsms/plain?user={0}&password={1}&sender={2}&SMSText={3}&GSM={4}", new object[] { username, pwds, displayname, msg, msgto }));
        request.Method = "POST";
        string s = "";
        byte[] bytes = Encoding.UTF8.GetBytes(s);
        request.ContentType = "application/x-www-form-urlencoded";
        request.ContentLength = bytes.Length;
        Stream requestStream = request.GetRequestStream();
        requestStream.Write(bytes, 0, bytes.Length);
        requestStream.Close();
        WebResponse response = request.GetResponse();
        Console.WriteLine(((HttpWebResponse)response).StatusDescription);
        requestStream = response.GetResponseStream();
        StreamReader reader = new StreamReader(requestStream);
        string str3 = reader.ReadToEnd();
        Console.WriteLine(str3);
        reader.Close();
        requestStream.Close();
        return str3;
    }



}