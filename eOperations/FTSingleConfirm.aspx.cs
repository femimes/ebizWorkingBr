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

public partial class eOperations_FTSingleConfirm : System.Web.UI.Page
{
    public string gUser = HttpContext.Current.User.Identity.Name;//HttpContext.Current.User.Identity.Name;
    public string luser = "";
    public string StrLocation = "";
    public string struser = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack && !IsCallback)
        {
           // Response.Write(Request.QueryString["a"]);
            loadRecs(Request.QueryString["a"]);

        }
    }


    public void loadRecs(string RefDetails)
    {
        luser = "master?101";// HttpContext.Current.User.Identity.Name;//HttpContext.Current.User.Identity.Name; //HttpContext.Current.User.Identity.Name;// HttpContext.Current.User.Identity.Name;
        StrLocation = luser.Substring(luser.LastIndexOf("?") + 1);
        struser = luser.Substring(0, luser.LastIndexOf("?"));
        gUser = struser;
        string strGetRecs = string.Format("SpNapsExtTransNapsDetSel {0}", RefDetails);
        foreach (DataRow dr in Classes.GetDataset(strGetRecs).Tables[0].Rows)
        {
            lblAccountNo.Text = dr["AccountNo"].ToString();
            lblAcctName.Text = dr["AccountName"].ToString();
            lblAmount.Text = dr["Amount"].ToString();
            lblBenAcctNo.Text = dr["BenAccountNo"].ToString();
            lblBenAcctName.Text = dr["BenAccountName"].ToString();
            lblBenBank.Text = dr["BenBankCode"].ToString();
            lblBenBank.CssClass = dr["BenBankCode"].ToString();
            lblPayDetails.Text = dr["Narration"].ToString();
            lblFeeAmount.Text = dr["FeeAmount"].ToString();
        }

    }
    public string validateTokens(string tokens,string SeqID)
    {
        #region check tokens
        




        #region check to validity Records

        SqlConnection connection2 = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString);


        SqlCommand commandIns = new SqlCommand();
        connection2.Open();
        commandIns.CommandText = "SpNapsCheckTokenValidility";
        commandIns.Connection = connection2;
        commandIns.CommandType = CommandType.StoredProcedure;
        commandIns.Parameters.Add(new SqlParameter("@SeqID", SeqID.Replace("'","")));
        commandIns.Parameters.Add(new SqlParameter("@tokens", txtOTP.Text));


        SqlParameter parameter = commandIns.Parameters.Add(new SqlParameter("@responseCode", SqlDbType.VarChar,50));
        parameter.Direction = ParameterDirection.Output;
        SqlParameter parametermsg = commandIns.Parameters.Add(new SqlParameter("@responseMsg", SqlDbType.VarChar,300));
        parametermsg.Direction = ParameterDirection.Output;

        commandIns.ExecuteNonQuery();
        connection2.Close();

        string responsecode = parameter.Value.ToString();
        string responsemsg = parametermsg.Value.ToString();
        #endregion

        Literal1.Text = responsemsg;


       #endregion
        return responsecode;
    }
   
    
    protected void ASPxButton1_Click(object sender, EventArgs e)
    {
        if (validateTokens(txtOTP.Text, Request.QueryString["a"]) != "01")
        {
            return;
        }
        luser = "master?101";// HttpContext.Current.User.Identity.Name;//HttpContext.Current.User.Identity.Name; //HttpContext.Current.User.Identity.Name;// HttpContext.Current.User.Identity.Name;
        StrLocation = luser.Substring(luser.LastIndexOf("?") + 1);
        struser = luser.Substring(0, luser.LastIndexOf("?"));
        gUser = struser;
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString);
        #region Section generate payment schedule

        NumberStyles style;
        DateTimeStyles Datestyles;
        CultureInfo provider;
        // Gets a NumberFormatInfo associated with the en-US culture.
        NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

        style = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands;
        provider = new CultureInfo("en-GB");

        DateTimeFormatInfo myDTFI = new CultureInfo("en-US", false).DateTimeFormat;
        DataSet dsp = new DataSet();
        string strGetRecs = string.Format("SpNapsExtTransNapsDetSel {0}", Request.QueryString["a"]);
        SqlDataAdapter cmd = new SqlDataAdapter(strGetRecs, connection);
        DataTable dts = new DataTable("PaymentRecord");
        cmd.Fill(dts);
        #endregion
        #region Define source parameters


        #region Get clients parameters Details
        SqlConnection connVendor = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnectSource"].ConnectionString);

        string strdetails = "SELECT * FROM ParamPreferences";
        string clientId = NETClass.Classes.GetDataset(strdetails).Tables[0].Rows[0]["ClientNapsId"].ToString();
        string TokenTimeOut = NETClass.Classes.GetDataset(strdetails).Tables[0].Rows[0]["TokenTimeOut"].ToString();
        string strSessionID = string.Format("SELECT '{0}' + '{1}' + replace(convert(varchar(8),getdate(),2),'.','') + replace(convert(varchar(8),getdate(),8),':','') + right(replace(convert(varchar(8),getdate(),8),':','') * 100000000009,12) as SessionID", txtOTP.Text, lblBenBank.CssClass);
        DataTable dtsessionID = new DataTable();

        SqlDataAdapter cmdsessionID = new SqlDataAdapter(strSessionID, connVendor);
        cmdsessionID.Fill(dtsessionID);
        string SessionID = dtsessionID.Rows[0]["SessionID"].ToString().Substring(0, 30);

        #endregion

        string FileName = "0001"; string ScheduleId = SessionID; string DebitSortCode = "050"; string DebitAccountNumber = "0123456789"; string ClientId = clientId;





        #endregion

        #region Insert Records into NAPs Entry table

        #region Save Records

        SqlConnection connNapsHist = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString);


        SqlCommand commandNapsHist = new SqlCommand();
        connNapsHist.Open();
        commandNapsHist.CommandText = "SpAddNapsExtTransNapsHist";
        commandNapsHist.Connection = connNapsHist;
        commandNapsHist.CommandType = CommandType.StoredProcedure;
        commandNapsHist.Parameters.Add(new SqlParameter("@MobileNo", lblAccountNo.Text));
        commandNapsHist.Parameters.Add(new SqlParameter("@AccountNo", lblAccountNo.Text));
        commandNapsHist.Parameters.Add(new SqlParameter("@Postdate", DateTime.Today));
        commandNapsHist.Parameters.Add(new SqlParameter("@BeneficiaryAcctNo", lblBenAcctNo.Text));
        commandNapsHist.Parameters.Add(new SqlParameter("@BeneficiaryName", lblBenAcctName.Text));
        commandNapsHist.Parameters.Add(new SqlParameter("@Narration", lblPayDetails.Text));
        commandNapsHist.Parameters.Add(new SqlParameter("@RequestTitle", FileName));
        commandNapsHist.Parameters.Add(new SqlParameter("@CustAcctNo", lblAccountNo.Text));

        commandNapsHist.Parameters.Add(new SqlParameter("@Amount", lblAmount.Text));
        commandNapsHist.Parameters.Add(new SqlParameter("@FeeAmt", lblFeeAmount.Text));
        commandNapsHist.Parameters.Add(new SqlParameter("@TransCode", 863));

        commandNapsHist.Parameters.Add(new SqlParameter("@AgentID", lblAccountNo.Text));
        commandNapsHist.Parameters.Add(new SqlParameter("@TransRefNo", SessionID));
        commandNapsHist.Parameters.Add(new SqlParameter("@SortCode", lblBenBank.Text));

        commandNapsHist.Parameters.Add(new SqlParameter("@Postedby", gUser));

        commandNapsHist.Parameters.Add(new SqlParameter("@ApprovedBy", gUser));
        commandNapsHist.Parameters.Add(new SqlParameter("@ConfirmBy", gUser));
        commandNapsHist.Parameters.Add(new SqlParameter("@TransRemarks", "single entry"));
        commandNapsHist.Parameters.Add(new SqlParameter("@ProfileID",Request.QueryString["a"].Replace("'","")));

      
        commandNapsHist.ExecuteNonQuery();
        connNapsHist.Close();

     

      
        #endregion
   
        #endregion
      
        #region Start Writing payment Schedule Xml
        DataTable dt = new DataTable("PaymentRequest");
          StringWriter sw = new StringWriter();
        XmlWriterSettings settings = new XmlWriterSettings();
        settings.Encoding = UTF8Encoding.UTF8;
        settings.Indent = true;
        settings.OmitXmlDeclaration = true;
        settings.NewLineOnAttributes = true;
        settings.ConformanceLevel = ConformanceLevel.Auto;
        using (XmlWriter writer = XmlWriter.Create(sw, settings))
        {

            writer.WriteStartDocument();


            writer.WriteStartElement("PaymentRequest", null);
            writer.WriteStartElement("Header",null);
                    writer.WriteElementString("FileName", FileName);
                    writer.WriteElementString("ScheduleId", ScheduleId);
                    writer.WriteElementString("DebitSortCode", DebitSortCode);
                    writer.WriteElementString("DebitAccountNumber", DebitAccountNumber);
                    writer.WriteElementString("ClientId", ClientId);
            writer.WriteEndElement();

            // section generate paydetails

            if (dts.Rows.Count > 0)
            {
                
                foreach (DataRow dr in dts.Rows)
                {
                    writer.WriteStartElement("PaymentRecord", null);
                    
                        writer.WriteElementString("Beneficiary", dr["AccountName"].ToString());
                        writer.WriteElementString("Amount", dr["Amount"].ToString());
                        writer.WriteElementString("AccountNumber", dr["AccountNo"].ToString());
                        writer.WriteElementString("SortCode", dr["BenBankCode"].ToString());
                        writer.WriteElementString("Narration", dr["Narration"].ToString());
                    writer.WriteEndElement();// payment Record  lines end 
                    
                }
                
            }
            else
            {
                writer.WriteStartElement("PaymentRecord");
                writer.WriteElementString("Beneficiary",null);
                writer.WriteElementString("Amount", "0");
                writer.WriteElementString("AccountNumber", null);
                writer.WriteElementString("SortCode", null);
                writer.WriteElementString("Narration", null);
                writer.WriteEndElement();// payment Record  lines 
                
            }//end loop
            writer.WriteElementString("HashValue", null);
            writer.Close();
        }

        dt.WriteXml(sw);
        #endregion
        // remove spaces
        string results = sw.ToString().Replace("<DocumentElement />","").Replace("</DocumentElement>", "").Replace("<DocumentElement>", "").Replace("utf-16", "UTF-8").Replace("\r\n", "").Replace("\t", "").Replace("\n", "").Replace(">  ", ">");//.Replace(">  <","");



        WebReference.PitchControlCenterWithXML ks = new WebReference.PitchControlCenterWithXML();
        string sendandresponse = ks.uploadPaymentSchedule(results);
        sendandresponse = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?><PaymentResponse><Header><ScheduleId>ERCAS_01082  17022813331880000</ScheduleId><ClientId>ERCAS_01</ClientId><DebitSortCode>050</DebitSortCode><DebitAccountNumber>0123456789</DebitAccountNumber><Status>01</Status></Header><PaymentRecord><Beneficiary>OLAKUNRI, ENIOLA</Beneficiary><Amount>300.00</Amount><AccountNumber>0000080893</AccountNumber><SortCode>082</SortCode><Narration>test</Narration><Status></Status><ErrorReason></ErrorReason></PaymentRecord><HashValue></HashValue></PaymentResponse>";

        StringReader sr = new StringReader(sendandresponse);
        DataSet LoadDs = new DataSet();
        //LoadDs.ReadXml(strDecryMsg);
        LoadDs.ReadXml(sr);
        foreach (DataRow DrRow in LoadDs.Tables[0].Rows)
        {
            Response.Write(DrRow["Status"]);//
            if (DrRow["Status"] == "00")//if transaction is successful
            {
                //Update Naps entry table
                txtOTP.Visible = false;
               

            }
        }


    }
}