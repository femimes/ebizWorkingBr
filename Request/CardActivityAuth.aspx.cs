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

public partial class Request_CardActivityAuth :  System.Web.UI.Page
{
    public string gUser = HttpContext.Current.User.Identity.Name;//HttpContext.Current.User.Identity.Name;
    public string luser = HttpContext.Current.User.Identity.Name;
    public string StrLocation = "";
    public string struser = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack && !IsCallback)
        //if (IsCallback)
        {
            luser = HttpContext.Current.User.Identity.Name;
            StrLocation = luser.Substring(luser.LastIndexOf("?") + 1);
            struser = luser.Substring(0, luser.LastIndexOf("?"));
            gUser = struser;

            //txtPin.Text = "";
            lblUser.Text = gUser;// Request.Cookies["eLogonUserID"].Value;
            //loadDetails();
            //LoadingPanel.ContainerElementID = false ? "Panel" : "";
            LoadingPanel.ContainerElementID = false ? "Panel" : "";
            Thread.Sleep(7500);
        }

        //Thread.Sleep(7500);
    }

    void loadDetails()
    {
        string RefNo = ""; string TransType = "002";
        string strRespCode = ""; string strRespDesc = "";
        decimal amount = 0;
        decimal Fee = 0;
        string strResults = ""; DataTable dt = new DataTable();
        decimal TotalAmts = 0;
        dt = iPoints.ProcessGetTempTrans(Request.QueryString["RefNo"], lblUser.Text, "2", Request.Cookies["Token"].Value);
        foreach (DataRow item in dt.Rows)
        {
            //AccountNo = item["AccountNo"].ToString();
            //creditAccountNo = item["BenAcctNo"].ToString();
            //mobileNo = "";// item["mobileNo"].ToString();
            //Location = item["Location"].ToString();
            amount = decimal.Parse(item["amount"].ToString());
            Fee = decimal.Parse(item["Fee"].ToString());
            TransType = item["TransType"].ToString();
            //creditBankcode = item["BenSortCode"].ToString();
            TotalAmts += amount + Fee;
        }

        lblwebRefNo.Text = string.Format("Payment Reference: {0}", Request.QueryString["RefNo"]);
        lblAmount.Text = string.Format("NGN {0:0,0.00}", TotalAmts);

        if (TransType == "001")
        {
            lblwebRefNo.Visible = false;
            customSwitch1.Visible = false;
        }


        if (TransType == "002")
        {
            lblwebRefNo.Visible = false;

        }


    }
    protected void btnConfirm_Click(object sender, EventArgs e)
    {

        luser = HttpContext.Current.User.Identity.Name;
        StrLocation = luser.Substring(luser.LastIndexOf("?") + 1);
        struser = luser.Substring(0, luser.LastIndexOf("?"));
        gUser = struser;

        if (this.Page.IsValid == true)
        {
            string RefNo = ""; string TransType = "002";
            string strRespCode = ""; string strRespDesc = "";
            string strResults = ""; DataTable dt = new DataTable();
            #region Initalise and get parameters
            string Token2FAUserId = "";string Token2FAClientId = "";
            string CardISWClientId = ""; string CardISWClientKey = ""; string CardISWBankCode = "";string CardlinkAcctNo = "";
            foreach (DataRow  item in NETClass.Classes.SqlDbGetDataset("Sproc_GetPref").Tables[0].Rows)
            {
                  Token2FAUserId = item["Token2FAUserId"].ToString(); Token2FAClientId = item["Token2FAClientId"].ToString();
                  CardISWClientId = item["CardISWClientId"].ToString(); CardISWClientKey = item["CardISWClientKey"].ToString();
                  CardISWBankCode = item["CardISWBankCode"].ToString();
                CardlinkAcctNo = item["linkedAccountNo"].ToString();
            }
            Token2FAUserId = gUser;
            #endregion

            WebRefVanzo.TwoFactorService hardToken = new TwoFactorService();

          
            var respToken = hardToken.doResponseAuthentication(Token2FAUserId, Token2FAClientId, psword.Text);
            
            if (respToken.status.ToString() == "1")
        
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
                string creditAccountName = "";

                string cardPin = "";
                string cardCVV = "";
                string cardPanNo = "";
                string cardMonth = "";
                string cardAccount = "";
                string cardMaintTyoe = "";
                dt = NETClass.Classes.SqlDbGetDataset(string.Format("SpGetBatRecCardMaintInfo {0}", Request.QueryString["RefNo"])).Tables[0];//iPoints.ProcessGetTempTrans(Request.QueryString["RefNo"], lblUser.Text, "2", Request.Cookies["Token"].Value);
                foreach (DataRow item in dt.Rows)
                {
                    AccountNo = item["accountNo"].ToString();
                    cardPin = item["depCardpin"].ToString();
                    cardCVV = item["depcardcvv"].ToString();
                    cardPanNo =  item["CardInstno"].ToString();
                    cardMonth = item["depcardmonth"].ToString();
                    cardAccount = item["accountNo"].ToString();
                    CardlinkAcctNo = item["LinkedAccountNo"].ToString();
                    cardMaintTyoe = item["maintType"].ToString();
                    TransType = item["maintType"].ToString();
                    string strGetCustInfo = string.Format("SpAXMobileNo '{0}'",AccountNo);
                    foreach (DataRow dr in NETClass.Classes.SqlDbGetDataset(strGetCustInfo).Tables[0].Rows)
                    {
                        mobileNo = dr["SMSNo"].ToString();
                    }

                    if (TransType == "100004")
                    {
                        //strResults = iPoints.ProcessTellerInterFTOTP(psword.Text, AccountNo, creditAccountNo, Request.QueryString["RefNo"], Location, amount, Fee, narration, lblUser.Text, creditBankcode, Request.QueryString["RefNo"], Request.Cookies["Token"].Value);
                        //strResults = ICardProcess.Blockcards(CardISWBankCode, gUser, CardISWClientId, CardISWClientKey,
                        //    "1",cardPanNo,cardPin,cardMonth,cardCVV);

                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        //dynamic json = jss.DeserializeObject(strResults);
                        //string strResp = "";
                        //strRespDesc = "Success";
                        //ICardProcess.AddtoLogFile(strResults, "Block Details");
                        //strResp = json["success"].ToString();

                        //if (bool.Parse(strResp) == false)
                        //{
                        //    btnConfirm.Enabled = true;
                        //    litEmmorMsg.Text = strRespDesc;

                        //}
                        //else
                        {
                            // update approval records
                            string l_refNo = Request.QueryString["RefNo"];
                            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString);
                            SqlCommand commandApd = new SqlCommand();
                            connection.Open();
                            commandApd.CommandText = "Sproc_UpdateCardMaintenance";
                            commandApd.Connection = connection;

                            commandApd.CommandType = CommandType.StoredProcedure;
                            commandApd.Parameters.Add(new SqlParameter("@eBatRefNo", l_refNo));

                            commandApd.Parameters.Add(new SqlParameter("@status", 3));
                            commandApd.ExecuteNonQuery();
                            connection.Close();
                            Response.Redirect(string.Format("../Approvals/ApproveCardMaint.aspx", Request.QueryString["RefNo"], psword.Text));

                        }

                    }
                    if (TransType == "100005")
                    {
                        //strResults = iPoints.ProcessTellerInterFTOTP(psword.Text, AccountNo, creditAccountNo, Request.QueryString["RefNo"], Location, amount, Fee, narration, lblUser.Text, creditBankcode, Request.QueryString["RefNo"], Request.Cookies["Token"].Value);

                        #region Determine Card account Type
                        string accountType = "10"; string strProdtype = "1";
                        string strBenCon = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnectIBank"].ConnectionString;

                        SqlConnection Conn = new SqlConnection(strBenCon);DataTable dtcard = new DataTable();
                        SqlCommand cmd = new SqlCommand();
                        SqlDataAdapter cmdAdapter = new SqlDataAdapter("SpAcctMasterSelectOne", Conn);
                        cmdAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        cmdAdapter.SelectCommand.Parameters.Add(new SqlParameter("@AccountNo", cardAccount));
                        cmdAdapter.Fill(dtcard);
                        foreach (DataRow itemRows in dtcard.Rows)
                        {
                            strProdtype = itemRows["ProdCatCode"].ToString();
                        }
                        if (strProdtype =="1")
                        {
                            accountType = "20";
                        }
                        if (strProdtype == "2")
                        {
                            accountType = "10";
                        }
                        
                        if (strProdtype == "3" || strProdtype == "4")
                        {
                            accountType = "50";
                        }
                        if (strProdtype == "6")
                        {
                            accountType = "30";
                        }
                        #endregion

                        strResults = ICardProcess.UnBlockcards(CardISWBankCode, gUser, CardISWClientId, CardISWClientKey,
                            accountType, cardPanNo, cardPin, cardMonth, cardCVV);


                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        dynamic json = jss.DeserializeObject(strResults);
                        string strResp = "";
                        strRespDesc = "Success";
                        ICardProcess.AddtoLogFile(strResults, "Unblock Details");
                        strResp = json["success"].ToString();

                        if (bool.Parse(strResp) == false)
                        {
                            btnConfirm.Enabled = true;
                            litEmmorMsg.Text = strRespDesc;

                        }
                        else
                        {
                            // update approval records
                            string l_refNo = Request.QueryString["RefNo"];
                            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString);
                            SqlCommand commandApd = new SqlCommand();
                            connection.Open();
                            commandApd.CommandText = "Sproc_UpdateCardMaintenance";
                            commandApd.Connection = connection;

                            commandApd.CommandType = CommandType.StoredProcedure;
                            commandApd.Parameters.Add(new SqlParameter("@eBatRefNo", l_refNo));

                            commandApd.Parameters.Add(new SqlParameter("@status", 3));
                            commandApd.ExecuteNonQuery();
                            connection.Close();
                            Response.Redirect(string.Format("../Approvals/ApproveCardMaint.aspx", Request.QueryString["RefNo"], psword.Text));

                        }

                    }

                    if (TransType == "100006")
                    {
                        //strResults = iPoints.ProcessTellerInterFTOTP(psword.Text, AccountNo, creditAccountNo, Request.QueryString["RefNo"], Location, amount, Fee, narration, lblUser.Text, creditBankcode, Request.QueryString["RefNo"], Request.Cookies["Token"].Value);
                        strResults = ICardProcess.UnLinkcards(CardISWBankCode, gUser, CardISWClientId, CardISWClientKey,
                            "1", cardPanNo, cardPin, cardMonth, cardCVV, AccountNo);


                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        dynamic json = jss.DeserializeObject(strResults);
                        string strResp = "";
                        strRespDesc = "Success";
                        ICardProcess.AddtoLogFile(strResults, "Unlink Details");
                        strResp = json["success"].ToString();

                        if (bool.Parse(strResp) == false)
                        {
                            btnConfirm.Enabled = true;
                            litEmmorMsg.Text = strRespDesc;

                        }
                        else
                        {
                            string l_refNo = Request.QueryString["RefNo"];
                            // update approval records
                            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString);
                            SqlCommand commandApd = new SqlCommand();
                            connection.Open();
                            commandApd.CommandText = "Sproc_UpdateCardMaintenance";
                            commandApd.Connection = connection;

                            commandApd.CommandType = CommandType.StoredProcedure;
                            commandApd.Parameters.Add(new SqlParameter("@eBatRefNo", l_refNo));

                            commandApd.Parameters.Add(new SqlParameter("@status", 3));
                            commandApd.ExecuteNonQuery();
                            connection.Close();
                            Response.Redirect(string.Format("../Approvals/ApproveCardMaint.aspx", Request.QueryString["RefNo"], psword.Text));

                        }

                    }
                    if (TransType == "100007")
                    {


                        #region Determine Card account Type
                        string accountType = "10"; string strProdtype = "1";
                        string strBenCon = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnectIBank"].ConnectionString;

                        SqlConnection Conn = new SqlConnection(strBenCon); DataTable dtcard = new DataTable();
                        SqlCommand cmd = new SqlCommand();
                        SqlDataAdapter cmdAdapter = new SqlDataAdapter("SpAcctMasterSelectOne", Conn);
                        cmdAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        cmdAdapter.SelectCommand.Parameters.Add(new SqlParameter("@AccountNo", cardAccount));
                        cmdAdapter.Fill(dtcard);
                        foreach (DataRow itemRows in dtcard.Rows)
                        {
                            strProdtype = itemRows["ProdCatCode"].ToString();
                        }
                        if (strProdtype == "1")
                        {
                            accountType = "20";
                        }
                        if (strProdtype == "2")
                        {
                            accountType = "10";
                        }

                        if (strProdtype == "3" || strProdtype == "4")
                        {
                            accountType = "50";
                        }
                        if (strProdtype == "6")
                        {
                            accountType = "30";
                        }
                        #endregion

                        //strResults = iPoints.ProcessTellerInterFTOTP(psword.Text, AccountNo, creditAccountNo, Request.QueryString["RefNo"], Location, amount, Fee, narration, lblUser.Text, creditBankcode, Request.QueryString["RefNo"], Request.Cookies["Token"].Value);
                        strResults = ICardProcess.PinReIssuance(CardISWBankCode, gUser, CardISWClientId, CardISWClientKey,
                            accountType, cardPanNo, cardPin, cardMonth, cardCVV,"72",AccountNo,mobileNo,gUser);


                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        dynamic json = jss.DeserializeObject(strResults);
                        string strResp = "";
                        strRespDesc = "Success";
                        ICardProcess.AddtoLogFile(strResults, "Pin ReIssueance Details");
                        strResp = json["success"].ToString();

                        if (bool.Parse(strResp) == true)
                        {
                            //Response.Write("informations" + strResp);
                            string l_refNo = Request.QueryString["RefNo"];
                            litEmmorMsg.Text = strRespDesc;

                            //        // update approval records

                            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString);
                            SqlCommand commandApd = new SqlCommand();
                            connection.Open();
                            commandApd.CommandText = "Sproc_UpdateCardMaintenance";
                            commandApd.Connection = connection;

                            commandApd.CommandType = CommandType.StoredProcedure;
                            commandApd.Parameters.Add(new SqlParameter("@eBatRefNo", l_refNo));

                            commandApd.Parameters.Add(new SqlParameter("@status", 3));
                            commandApd.ExecuteNonQuery();
                            connection.Close();

                            Response.Redirect(string.Format("../Approvals/ApproveCardMaint.aspx", Request.QueryString["RefNo"], psword.Text));

                            //    }
                        }
                        else
                        {
                            strRespDesc = "Failed";
                            litEmmorMsg.Text = strRespDesc;
                        }

                    }
                    if (TransType == "100008")
                    {
                        string l_refNo = Request.QueryString["RefNo"];
                        //check account bal and status
                        #region section check

                        SqlConnection connectionChk = new SqlConnection(ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString);
                        //  string str12 = Guid.NewGuid().ToString().Substring(0, 12).ToUpper().Replace("-", "");
                        //string str14 = NETClass.NETClass.GetRequestDataSet(string.Format("SELECT dbo.FnEncryptString('{0}') AS EnCryptValues", str12)).Tables[0].Rows[0]["EnCryptValues"].ToString();
                        SqlCommand commandCheck = new SqlCommand();
                        connectionChk.Open();
                        commandCheck.CommandText = "SpCheckCardLink";
                        commandCheck.Connection = connectionChk;

                        commandCheck.CommandType = CommandType.StoredProcedure;
                        commandCheck.Parameters.Add(new SqlParameter("@eBatRefNo", l_refNo));
                        commandCheck.Parameters.Add(new SqlParameter("@status", 3));
                        commandCheck.Parameters.Add(new SqlParameter("@MaintLocation", StrLocation));
                        SqlParameter parameterCode = commandCheck.Parameters.Add(new SqlParameter("@ResponseCode", SqlDbType.VarChar, 5000));
                        parameterCode.Direction = ParameterDirection.Output;
                        SqlParameter parameterMsg = commandCheck.Parameters.Add(new SqlParameter("@ResponseDesc", SqlDbType.VarChar, 5000));
                        parameterMsg.Direction = ParameterDirection.Output;
                        commandCheck.ExecuteNonQuery();
                        connectionChk.Close();
                        #endregion
                        if (parameterCode.Value.ToString() == "0")
                        {
                            //strResults = iPoints.ProcessTellerInterFTOTP(psword.Text, AccountNo, creditAccountNo, Request.QueryString["RefNo"], Location, amount, Fee, narration, lblUser.Text, creditBankcode, Request.QueryString["RefNo"], Request.Cookies["Token"].Value);
                            strResults = ICardProcess.Linkcards(CardISWBankCode, gUser, CardISWClientId, CardISWClientKey,
                                "1", cardPanNo, cardPin, cardMonth, cardCVV, CardlinkAcctNo);


                            JavaScriptSerializer jss = new JavaScriptSerializer();
                            dynamic json = jss.DeserializeObject(strResults);
                            string strResp = "";
                            strRespDesc = "Success";
                            ICardProcess.AddtoLogFile(strResults, "Linking Details");
                            strResp = json["success"].ToString();

                            if (bool.Parse(strResp) == true)
                            {
                                Response.Write("informations" + strResp);

                                litEmmorMsg.Text = strRespDesc;

                                //        // update approval records

                                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString);
                                SqlCommand commandApd = new SqlCommand();
                                connection.Open();
                                commandApd.CommandText = "Sproc_UpdateCardMaintenance";
                                commandApd.Connection = connection;

                                commandApd.CommandType = CommandType.StoredProcedure;
                                commandApd.Parameters.Add(new SqlParameter("@eBatRefNo", l_refNo));
                                commandApd.Parameters.Add(new SqlParameter("@status", 3));
                                //commandApd.Parameters.Add(new SqlParameter("@status", int.Parse("3")));
                                commandApd.ExecuteNonQuery();
                                connection.Close();

                                Response.Redirect(string.Format("../Approvals/ApproveCardMaint.aspx", Request.QueryString["RefNo"], psword.Text));

                                //    }
                            }
                            else
                            {
                                strRespDesc = "Failed";
                                litEmmorMsg.Text = strRespDesc;
                                lblwebRefNo.Text = "Failed";
                            }



                        }

                        else
                        {
                            strRespDesc = "Failed";
                            litEmmorMsg.Text = parameterMsg.Value.ToString();
                            lblwebRefNo.Text = "Failed";

                        }
                    
                    }


                }



            }

        }


    }

    

}