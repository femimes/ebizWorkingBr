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
//using NETClass;
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
//using DevExpress.Web;
//////using System.Collections;
using System.Configuration;
using System.Net.Sockets;
using WebReference;
using System.IO;
using System.Text;


public partial class eOperations_NapsCheckValidation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack && !IsCallback)
        {
        }
    }
    protected void btnpayment_Click(object sender, EventArgs e)
    {
        //SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString);
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
        //string strGetRecs = string.Format("SpNapsExtTransNapsDetSel {0}", Request.QueryString["a"]);
        //SqlDataAdapter cmd = new SqlDataAdapter(strGetRecs, connection);
        //DataTable dts = new DataTable("PaymentRecord");
        //cmd.Fill(dts);
        #endregion
        string SessionID = Guid.NewGuid().ToString().Substring(1, 25);
        string clientId = "";

         string FileName = "0001"; string ScheduleId = SessionID; string DebitSortCode = "050"; string DebitAccountNumber = "0123456789"; string ClientId = clientId;

         WebReference.PitchControlCenterWithXML ks = new WebReference.PitchControlCenterWithXML();
         string strval = string.Format("<?xml version=\"1.0\" encoding=\"UTF-8\" ?><PaymentRequest><Header><FileName>0001</FileName><ScheduleId>{0}</ScheduleId><DebitSortCode>050</DebitSortCode><DebitAccountNumber>0123456789</DebitAccountNumber><ClientId>ERCAS_01</ClientId></Header><PaymentRecord><Beneficiary>{2}</Beneficiary><Amount>{3}</Amount><AccountNumber>{1}</AccountNumber><SortCode>{4}</SortCode><Narration>test</Narration></PaymentRecord><HashValue /></PaymentRequest>", ScheduleId,txtAcctno.Text,txtAccName.Text,txtAmount.Text,txtBank.Text);
         
         lblScheduleID.Text = ScheduleId;
         string getValidations = ks.uploadPaymentSchedule(strval);
         Response.Write(getValidations);
    
    }
    protected void btnStatus_Click(object sender, EventArgs e)
    {
        string SessionID = Guid.NewGuid().ToString().Substring(1, 25);
        string clientId = "";

        //string FileName = "0001"; string ScheduleId = SessionID; string DebitSortCode = "050"; string DebitAccountNumber = "0123456789"; string ClientId = clientId;


        string FileName = "0001"; string ScheduleId = SessionID; string DebitSortCode = "050"; string DebitAccountNumber = "9123456789"; string ClientId = clientId;

       lblScheduleID.Text = "daf1158-57ed-4d14-bad4-09";
        WebReference.PitchControlCenterWithXML ks = new WebReference.PitchControlCenterWithXML();
        string strResustatus = string.Format("<?xml version=\"1.0\" encoding=\"UTF-8\" ?><StatusRequest><Header><ScheduleId>{0}</ScheduleId><ClientId>ERCAS_01</ClientId></Header><HashValue></HashValue></StatusRequest>",lblScheduleID.Text);
        
        string getStatus = ks.getPaymentScheduleStatus(strResustatus);


        Response.Write(getStatus);
    
    }
    protected void Button1_Click(object sender, EventArgs e)
    {

        //SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString);
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
        //string strGetRecs = string.Format("SpNapsExtTransNapsDetSel {0}", Request.QueryString["a"]);
        //SqlDataAdapter cmd = new SqlDataAdapter(strGetRecs, connection);
        //DataTable dts = new DataTable("PaymentRecord");
        //cmd.Fill(dts);
        #endregion
        string SessionID = Guid.NewGuid().ToString().Substring(1, 25);
        string clientId = "";

        string FileName = "0001"; string ScheduleId = SessionID; string DebitSortCode = "050"; string DebitAccountNumber = "0123456789"; string ClientId = clientId;

        WebReference.PitchControlCenterWithXML ks = new WebReference.PitchControlCenterWithXML();
        string strval = string.Format("<?xml version=\"1.0\" encoding=\"UTF-8\" ?><PaymentRequest><Header><FileName>0001</FileName><ScheduleId>{0}</ScheduleId><DebitSortCode>050</DebitSortCode><DebitAccountNumber>0123456789</DebitAccountNumber><ClientId>ERCAS_01</ClientId></Header><PaymentRecord><Beneficiary>{2}</Beneficiary><Amount>{3}</Amount><AccountNumber>{1}</AccountNumber><SortCode>{4}</SortCode><Narration>test</Narration></PaymentRecord><PaymentRecord><Beneficiary>{2}</Beneficiary><Amount>{3}</Amount><AccountNumber>{5}</AccountNumber><SortCode>{6}</SortCode><Narration>testing </Narration></PaymentRecord><HashValue /></PaymentRequest>", ScheduleId,txtAcctno.Text,txtAccName.Text,txtAmount.Text,txtBank.Text,txtAcctNo2.Text ,txtBank2.Text);

        lblScheduleID.Text = ScheduleId;
        string getValidations = ks.uploadPaymentSchedule(strval);
        Response.Write(getValidations);
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        string getStatus = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?><PaymentResponse><Header><ScheduleId>753634000170323190044400001710</ScheduleId><ClientId>ERCAS_01</ClientId><DebitSortCode></DebitSortCode><DebitAccountNumber></DebitAccountNumber><Status>06</Status></Header><HashValue></HashValue></PaymentResponse>";

        StringReader sr = new StringReader(getStatus);
        DataSet LoadDs = new DataSet();
        //LoadDs.ReadXml(strDecryMsg);
        LoadDs.ReadXml(sr);
        //txtOTP.Visible = false;

        foreach (DataRow DrRow in LoadDs.Tables["PaymentResponse"].Rows)
        {
            Response.Write(DrRow["Status"]);//
            if (DrRow["Status"] == "00")//if transaction is successful
            {
                //Update Naps entry table


            }
        }
    }
}