using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Timers;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using NETClass;
//using NETConnect;
//using NETConnect;
using System.Xml;
using System.IO;
using System.Text;
using System.Net;
using System.Security.Cryptography;
using System.Configuration;
using System.Net.Sockets;
using WebReference;
using System.Globalization;
using System.Net.Mail;
using DevExpress.Web;
using System.Collections;
using System.Configuration;
using System.Net.Sockets;
using WebReference;
using System.IO;
using System.Text;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public partial class eOperations_NapsQueryServices : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!base.IsPostBack && !base.IsCallback)
        {
            try
            {
                this.grid.DataBind();
                this.loadMsgs();
            }
            catch (SqlException)
            {
                this.Literal1.Text = "Datasource Connection Fail ";
                this.CustomValidator1.IsValid = false;
                this.CustomValidator1.Text = "Datasource Connection Fail";
                this.CustomValidator1.ErrorMessage = "Datasource Connection Fail. Please check the datasource connection parameters";
            }
        }
    }

    protected string GetIconImageUrl(GridViewDataItemTemplateContainer container)
    {
        return "";
    }
    protected bool GetIconImageVisibility(GridViewDataItemTemplateContainer container)
    {
        return true;
    }
    protected string GetPercentageText(GridViewDataItemTemplateContainer container)
    {
        return "";
    }

    public static void BypassCertificateError()
    {
        ServicePointManager.ServerCertificateValidationCallback +=

            delegate(
                Object sender1,
                X509Certificate certificate,
                X509Chain chain,
                SslPolicyErrors sslPolicyErrors)
            {
                return true;
            };
    }



    private void loadMsgs()
    {
        try
        {
            SqlConnection connVendor = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnectSource"].ConnectionString);

            string strdetails = "SELECT * FROM ParamPreferences";
            string ClientId = NETClass.Classes.GetDataset(strdetails).Tables[0].Rows[0]["ClientNapsId"].ToString();
            //Response.Write(ClientId);
            long num = 0L;
            GridViewDataColumn column = (GridViewDataColumn)this.grid.Columns["msg_status"];
            GridViewDataColumn column2 = (GridViewDataColumn)this.grid.Columns["msg_text"];
            GridViewDataColumn column3 = (GridViewDataColumn)this.grid.Columns["msg_to"];
            GridViewDataColumn column4 = (GridViewDataColumn)this.grid.Columns["msg_id"];
            GridViewDataColumn column5 = (GridViewDataColumn)this.grid.Columns["Msg_acctno"];
            GridViewDataColumn column6 = (GridViewDataColumn)this.grid.Columns["SortCode"];
            GridViewDataColumn column7 = (GridViewDataColumn)this.grid.Columns["Amount"];
            GridViewDataColumn column8 = (GridViewDataColumn)this.grid.Columns["FeeAmt"];
            GridViewDataColumn column9 = (GridViewDataColumn)this.grid.Columns["MobileNo"];
            GridViewDataColumn column10 = (GridViewDataColumn)this.grid.Columns["BeneficiaryAcctNo"];
            GridViewDataColumn column11 = (GridViewDataColumn)this.grid.Columns["TransRefNo"];
            column.Visible = true;
            column5.Visible = true;
            column6.Visible = true;
            column7.Visible = true;
            column8.Visible = true;
            column9.Visible = true;
            column10.Visible = true;
            column11.Visible = true;
            if ((this.grid.VisibleRowCount / this.grid.SettingsPager.PageSize) > 1)
            {
                num = this.grid.SettingsPager.PageSize;
            }
            else
            {
                num = this.grid.VisibleRowCount;
            }
            for (int i = 0; i < num; i++)
            {
                ASPxLabel label = (ASPxLabel)this.grid.FindRowCellTemplateControl(i, column, "ASPxLabel1");
                ASPxLabel label2 = (ASPxLabel)this.grid.FindRowCellTemplateControl(i, column3, "lblmsg_to");
                ASPxLabel label3 = (ASPxLabel)this.grid.FindRowCellTemplateControl(i, column2, "lblmsg_text");
                ASPxLabel label1 = (ASPxLabel)this.grid.FindRowCellTemplateControl(i, column4, "lblmsg_id");
                ASPxLabel labelAcctNo = (ASPxLabel)this.grid.FindRowCellTemplateControl(i, column5, "lblMsg_acctno");
                ASPxLabel lblSortCode = (ASPxLabel)this.grid.FindRowCellTemplateControl(i, column6, "lblSortCode");
                ASPxLabel lblAmount = (ASPxLabel)this.grid.FindRowCellTemplateControl(i, column7, "lblAmount");
                ASPxLabel lblFeeAmt = (ASPxLabel)this.grid.FindRowCellTemplateControl(i, column8, "lblFeeAmt");
                ASPxLabel lblMobileNo = (ASPxLabel)this.grid.FindRowCellTemplateControl(i, column9, "lblMobileNo");
                ASPxLabel lblBeneficiaryAcctNo = (ASPxLabel)this.grid.FindRowCellTemplateControl(i, column10, "lblBeneficiaryAcctNo");
                ASPxLabel lblTransRefNo = (ASPxLabel)this.grid.FindRowCellTemplateControl(i, column11, "lblTransRefNo");
                if (label.Text == "10")
                {
                    WebReference.PitchControlCenterWithXML ksStatus = new WebReference.PitchControlCenterWithXML();
                    //string strResultstatus = string.Format("<?xml version=\"1.0\" encoding=\"UTF-8\" ?><StatusRequest><Header><ScheduleId>{1}</ScheduleId><ClientId>{0}</ClientId></Header><HashValue></HashValue></StatusRequest>", ClientId,ScheduleId);
                    string strResultstatus = string.Format("<?xml version=\"1.0\" encoding=\"UTF-8\" ?><StatusRequest><Header><ScheduleId>{0}</ScheduleId><ClientId>{1}</ClientId></Header><HashValue></HashValue></StatusRequest>", lblTransRefNo.Text, ClientId);
                    BypassCertificateError();
                    string getStatus = ksStatus.getPaymentScheduleStatus(strResultstatus);
                    AddtoLogFile(getStatus, this.Page.UniqueID);
                    StringReader sr = new StringReader(getStatus);
                    DataSet LoadDs = new DataSet();
                    //LoadDs.ReadXml(strDecryMsg);
                    LoadDs.ReadXml(sr);
                    string ScheduleID = ""; 
                    if (LoadDs.Tables["Header"].Rows.Count > 1)
                    {
                        ScheduleID =  LoadDs.Tables["Header"].Rows[0]["ScheduleId"].ToString();
                        foreach (DataRow DrRow in LoadDs.Tables["PaymentRecord"].Rows)
                        {


                           // Response.Write(DrRow["Status"]);//
                            if (DrRow["Status"].ToString() == "00" && DrRow["AccountNumber"].ToString() == lblBeneficiaryAcctNo.Text && DrRow["SortCode"].ToString() == lblSortCode.Text && DrRow["Amount"] == lblAmount.Text)//if transaction is successful
                            {
                                //Update Naps entry table Section debit on EazybankAx

                                #region Section for account Transaction Format
                                SqlConnection connection3 = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnectSource"].ConnectionString);


                                SqlCommand commandIns = new SqlCommand();
                                connection3.Open();
                                commandIns.CommandText = "SpNapsAxExtTransFTDr";
                                commandIns.Connection = connection3;
                                commandIns.CommandType = CommandType.StoredProcedure;
                                //commandIns.Parameters.Add(new SqlParameter("@Trans_date", DateTime.Today.ToString()));
                                commandIns.Parameters.Add(new SqlParameter("@Account_no", labelAcctNo.Text));
                                commandIns.Parameters.Add(new SqlParameter("@FundAccount_no", lblBeneficiaryAcctNo.Text));
                                commandIns.Parameters.Add(new SqlParameter("@Ext_transcode", "869"));
                                commandIns.Parameters.Add(new SqlParameter("@Narrative",label3.Text));
                                commandIns.Parameters.Add(new SqlParameter("@Description", label3.Text));
                                commandIns.Parameters.Add(new SqlParameter("@DebitAmt", lblAmount.Text));
                                commandIns.Parameters.Add(new SqlParameter("@FeeAmt", lblFeeAmt.Text));
                                commandIns.Parameters.Add(new SqlParameter("@ExtRefNo", label2.Text));
                                commandIns.Parameters.Add(new SqlParameter("@EnteredBy", User.Identity.Name));

                                commandIns.Parameters.Add(new SqlParameter("@FundSortCode", lblSortCode.Text));
                                commandIns.Parameters.Add(new SqlParameter("@AtmTranType", "869"));
                                commandIns.Parameters.Add(new SqlParameter("@ExtTerminalID", label1.Text));
                                 
                                SqlParameter parameter = commandIns.Parameters.Add(new SqlParameter("@ReceiptNo", SqlDbType.VarChar, 100));
                                parameter.Direction = ParameterDirection.Output;
                                SqlParameter parameterRes = commandIns.Parameters.Add(new SqlParameter("@ResponseCode", SqlDbType.VarChar, 100));
                                parameterRes.Direction = ParameterDirection.Output;
                                SqlParameter parameterMsg = commandIns.Parameters.Add(new SqlParameter("@ResponseMsg", SqlDbType.VarChar, 500));
                                parameterMsg.Direction = ParameterDirection.Output;
                                commandIns.ExecuteNonQuery();
                                connection3.Close();

                                if (parameterRes.Value.ToString() != "00")
                                {
                                    //str6 = "0";//
                                    //str3 = "Routine Error";// "Unknown Error";
                                }
                                else
                                {
                                   // str6 = "1";//  return "00";
                                  //str3 = "Approved or completed sucessfully";
                                }

                                #endregion




                                //

                                string strUpdStatus = string.Format("SpNapsUpdStatByItemExtTransNAPsHist '{0}','{1}','{2}',{3},'{4}','{5}',3,'{6}'", DrRow["Status"], DrRow["ErrorReason"], label2.Text, DrRow["Amount"], DrRow["AccountNumber"], DrRow["SortCode"], lblTransRefNo.Text);
                                Classes.RecordOp(strUpdStatus);
                            }
                            else
                            {

                                string strUpdStatus = string.Format("SpNapsUpdStatByItemExtTransNAPsHist '{0}','{1}','{2}',{3},'{4}','{5}',10,'{6}'", DrRow["Status"], DrRow["ErrorReason"], label2.Text, DrRow["Amount"], DrRow["AccountNumber"], DrRow["SortCode"], lblTransRefNo.Text );
                                Classes.RecordOp(strUpdStatus);
                            }
                        }
                    }
                    else
                    {
                        //Response.Write(LoadDs.Tables["Header"].Rows.Count);
                        foreach (DataRow DkrRow in LoadDs.Tables["Header"].Rows)
                        {
                            //Response.Write(DkrRow["Status"]);//
                            string result = DkrRow["Status"].ToString();
                            if (result == "00")//if transaction is successful
                            {
                                //Update Naps entry table

                                #region Section for account Transaction Format
                                SqlConnection connection3 = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnectSource"].ConnectionString);

                                //Response.Write("i am here");
                                //Response.Write(connection3);
                                SqlCommand commandIns = new SqlCommand();
                                connection3.Open();
                                commandIns.CommandText = "SpNapsAxExtTransFTDr";
                                commandIns.Connection = connection3;
                                commandIns.CommandType = CommandType.StoredProcedure;
                                //commandIns.Parameters.Add(new SqlParameter("@Trans_date", DateTime.Today.ToString()));
                                commandIns.Parameters.Add(new SqlParameter("@Account_no", labelAcctNo.Text));
                                commandIns.Parameters.Add(new SqlParameter("@FundAccount_no", lblBeneficiaryAcctNo.Text));
                                commandIns.Parameters.Add(new SqlParameter("@Ext_transcode", "869"));
                                commandIns.Parameters.Add(new SqlParameter("@Narrative", label3.Text));
                                commandIns.Parameters.Add(new SqlParameter("@Description", label3.Text));
                                commandIns.Parameters.Add(new SqlParameter("@DebitAmt", lblAmount.Text));
                                commandIns.Parameters.Add(new SqlParameter("@FeeAmt", lblFeeAmt.Text));
                                commandIns.Parameters.Add(new SqlParameter("@ExtRefNo", label2.Text));
                                commandIns.Parameters.Add(new SqlParameter("@EnteredBy", User.Identity.Name));

                                commandIns.Parameters.Add(new SqlParameter("@FundSortCode", lblSortCode.Text));
                                commandIns.Parameters.Add(new SqlParameter("@AtmTranType", "869"));
                                commandIns.Parameters.Add(new SqlParameter("@ExtTerminalID", label1.Text));

                                SqlParameter parameter = commandIns.Parameters.Add(new SqlParameter("@ReceiptNo", SqlDbType.VarChar, 100));
                                parameter.Direction = ParameterDirection.Output;
                                SqlParameter parameterRes = commandIns.Parameters.Add(new SqlParameter("@ResponseCode", SqlDbType.VarChar, 100));
                                parameterRes.Direction = ParameterDirection.Output;
                                SqlParameter parameterMsg = commandIns.Parameters.Add(new SqlParameter("@ResponseMsg", SqlDbType.VarChar, 500));
                                parameterMsg.Direction = ParameterDirection.Output;
                                commandIns.ExecuteNonQuery();
                                connection3.Close();
                                
                                if (parameterRes.Value.ToString() != "00")
                                {
                                    //str6 = "0";//
                                    //str3 = "Routine Error";// "Unknown Error";
                                }
                                else
                                {
                                    // str6 = "1";//  return "00";
                                    //str3 = "Approved or completed sucessfully";
                                }

                                #endregion

                                string strUpdStatus = string.Format("SpNapsUpdateStatExtTransNAPsHist '{0}','{1}','{2}','{3}'", DkrRow["Status"], "accepted", label2.Text, lblTransRefNo.Text);
                                //Response.Write(strUpdStatus);
                                Classes.RecordOp(strUpdStatus);
                                AddtoLogFile(commandIns.CommandText, this.Page.UniqueID);
                                //Response.Write("I got here");

                            }
                            else
                            {

                                string strUpdStatus = string.Format("SpNapsUpdateStatExtTransNAPsHist '{0}','{1}','{2}','{3}'", DkrRow["Status"], "accepted", label2.Text, lblTransRefNo.Text);
                                //Response.Write(strUpdStatus);
                                Classes.RecordOp(strUpdStatus);
                            }
                        }
                    }



                }
            }
            //column.Visible = false;
            //column5.Visible = false;
            //column6.Visible = false;
            //column7.Visible = false;
            //column8.Visible = false;
            //column9.Visible = false;
            //column10.Visible = false;
            //column11.Visible = false;
        }
        catch (Exception ex) { Response.Write(ex.Message); }
    }

    protected void grid_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
    {
        this.loadMsgs();
    }

    public void AddtoLogFile(string Message, string WebPage)
    {
        string LogPath = Server.MapPath("~/log/");// ConfigurationManager.AppSettings["LogPath"].ToString();
        string filename = "Log_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
        string filepath = LogPath + filename;
        if (File.Exists(filepath))
        {
            using (StreamWriter writer = new StreamWriter(filepath, true))
            {
                writer.WriteLine("——————-START————-" + DateTime.Now);
                writer.WriteLine("Source :" + ErrorPage);
                writer.WriteLine(Message);
                writer.WriteLine("——————-END————-" + DateTime.Now);
            }
        }
        else
        {
            StreamWriter writer = File.CreateText(filepath);
            writer.WriteLine("——————-START————-" + DateTime.Now);
            writer.WriteLine("Source :" + ErrorPage);
            writer.WriteLine(Message);
            writer.WriteLine("——————-END————-" + DateTime.Now);
            writer.Close();
        }
    }

}