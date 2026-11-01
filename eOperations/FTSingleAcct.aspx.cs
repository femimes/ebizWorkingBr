using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using DevExpress.Web;
using NETClass;
 
using System.IO;
using System.Text;
using System.Net;
using System.Security.Cryptography;
using System.Configuration;
using System.Collections;
using WebReference;

public partial class eOperations_FTSingleAcct : System.Web.UI.Page
{
    public string gUser = HttpContext.Current.User.Identity.Name;//HttpContext.Current.User.Identity.Name;
    public string luser = "";
    public string StrLocation = "";
    public string struser = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack && !IsCallback)
        {

        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        luser = "master?101";// HttpContext.Current.User.Identity.Name;//HttpContext.Current.User.Identity.Name; //HttpContext.Current.User.Identity.Name;// HttpContext.Current.User.Identity.Name;
        StrLocation = luser.Substring(luser.LastIndexOf("?") + 1);
        struser = luser.Substring(0, luser.LastIndexOf("?"));
        gUser = struser;

        if (validateInfo() != "1")
        {
            return;

        }
        //determine the transaction fee amount
        string feecharge = Classes.GetDataset(string.Format("SpNapsGetFeeCharge {0}", txtAmounts.Text)).Tables[0].Rows[0][0].ToString();

        #region Save Records

        SqlConnection connection2 = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString);


        SqlCommand commandIns = new SqlCommand();
        connection2.Open();
        commandIns.CommandText = "SpAddNapsExtTransNapsDet";
        commandIns.Connection = connection2;
        commandIns.CommandType = CommandType.StoredProcedure;
        commandIns.Parameters.Add(new SqlParameter("@AccountNo", txtAccountNo.Text));
        commandIns.Parameters.Add(new SqlParameter("@AccountName", lblAccountName.Text));
        commandIns.Parameters.Add(new SqlParameter("@BenAccountNo", txtBenAccountNo.Text));
        commandIns.Parameters.Add(new SqlParameter("@BenAccountName", lblBenAccountNo.Text));
        commandIns.Parameters.Add(new SqlParameter("@BenBankCode", drpBanks.Value));
        commandIns.Parameters.Add(new SqlParameter("@Amount", txtAmounts.Text));
        commandIns.Parameters.Add(new SqlParameter("@FeeAmount", feecharge));
        commandIns.Parameters.Add(new SqlParameter("@Narration", txtNarration.Text ));
        commandIns.Parameters.Add(new SqlParameter("@Postdate", lblPaymentDate.Text));
        commandIns.Parameters.Add(new SqlParameter("@Postedby", gUser));

        SqlParameter parameter = commandIns.Parameters.Add(new SqlParameter("@SeqID", SqlDbType.UniqueIdentifier));
        parameter.Direction = ParameterDirection.Output;

        commandIns.ExecuteNonQuery();
        connection2.Close();

     string    RefNo = parameter.Value.ToString();
        #endregion
   
    string strUrl = string.Format("FTSingleConfirm.aspx?a='{0}'",RefNo);
     
        // section generate tokens
    string tokens = Classes.GenerateRandomDigit(DateTime.Today.Date, 6);
         
 

    #region Save Token Details
    SqlConnection connection3 = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString);


    SqlCommand commandInsToks = new SqlCommand();
    connection3.Open();
    commandInsToks.CommandText = "SpAddExtTransNapsTokens";
    commandInsToks.Connection = connection3;
    commandInsToks.CommandType = CommandType.StoredProcedure;
    commandInsToks.Parameters.Add(new SqlParameter("@SeqID", RefNo));
    commandInsToks.Parameters.Add(new SqlParameter("@AccountNo", txtAccountNo.Text));
    commandInsToks.Parameters.Add(new SqlParameter("@Amount", txtAmounts.Text));
    commandInsToks.Parameters.Add(new SqlParameter("@Token", tokens));
   
    //SqlParameter parametertoks = commandInsToks.Parameters.Add(new SqlParameter("@SeqID", SqlDbType.UniqueIdentifier));
    //parameter.Direction = ParameterDirection.Output;

    commandInsToks.ExecuteNonQuery();
    connection3.Close();
     
         // Mails or sms can be sent 

    #endregion
   
    

    Response.Redirect(strUrl);
    }

    public string validateInfo()
    {
        string transflag = "1";

        // Section checks Posting rights
        luser = "master?101";// HttpContext.Current.User.Identity.Name;//HttpContext.Current.User.Identity.Name; //HttpContext.Current.User.Identity.Name;// HttpContext.Current.User.Identity.Name;
        StrLocation = this.luser.Substring(this.luser.LastIndexOf("?") + 1);
        struser = this.luser.Substring(0, this.luser.LastIndexOf("?"));
        gUser = this.struser;
        string struserPosting = string.Format("SpNapsGetUserPostingLimits '{0}',{1}", gUser, txtAmounts.Text);
        if (Classes.GetDataset(struserPosting).Tables[0].Rows.Count > 0)
        {
            transflag = "1";
        }
        else
        {
            Literal1.Text = "Transaction not winthin User posting limits";
            transflag = "0";
        }
        //End user posting rights

        //checks acct validity
        string rString1 = string.Format("SELECT AccountNo FROM VwAccMaster WHERE AccountNo = '{0}'", txtAccountNo.Text);
        string rString3 = string.Format("SELECT AccountNo FROM VwAccMaster WHERE AccountNo = '{0}' AND AccountStatus = '1' ", txtAccountNo.Text);
        string rString4 = string.Format("SELECT AccountNo FROM VwAccMaster WHERE AccountNo = '{0}' AND ProdCatCode IN ('1','2','Z')", txtAccountNo.Text);
        if (Classes.AxGetField(rString1) == "")
        {
            txtAccountNo.IsValid = false;
            txtAccountNo.ValidationSettings.ErrorText = "Invalid Account";
            transflag = "0";
        }
        if (Classes.AxGetField(rString3) == "")
        {
            txtAccountNo.IsValid = false;
            txtAccountNo.ValidationSettings.ErrorText = "Account is either dormant, closed or unapproved";
            transflag = "0";
        }
        if (Classes.AxGetField(rString4) == "")
        {
            txtAccountNo.IsValid = false;
            txtAccountNo.ValidationSettings.ErrorText = "Only Retail and GL Accounts are allowed in this posting screen ";
            transflag = "0";
        }

        //ends 




        return transflag;
    }
    protected void txtAccountNo_TextChanged(object sender, EventArgs e)
    {
        
        lblAccountName.Text = "";
        SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnectSource"].ConnectionString);
        //connection2.Open();

        
        DataSet ds = new DataSet();
        string strinfo = string.Format("select dbo.FnAccountName('{0}')", txtAccountNo.Text);
        SqlDataAdapter cmd = new SqlDataAdapter(strinfo, connection);
        cmd.Fill(ds, "TblAccountDet");
        if (ds.Tables[0].Rows.Count > 0)
        {
             lblAccountName.Text = string.Format("{0}",ds.Tables[0].Rows[0][0]);

        }

        

    }
    protected void txtBenAccountNo_TextChanged(object sender, EventArgs e)
    {
       SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnectSource"].ConnectionString);
      
        string strdetails = "SELECT * FROM ParamPreferences";
        string clientId = NETClass.Classes.GetDataset(strdetails).Tables[0].Rows[0]["ClientNapsId"].ToString();
        string strSessionID = string.Format("SELECT '{0}' + '{1}' + replace(convert(varchar(8),getdate(),2),'.','') + replace(convert(varchar(8),getdate(),8),':','') + right(replace(convert(varchar(8),getdate(),8),':','') * 100000000009,12) as SessionID", clientId, drpBanks.Value); 
        DataTable dtsessionID = new DataTable();


    //  WebReference.NIPInterface ks = new NIPInterface();
        //"<?xml version="1.0" encoding="UTF-8"?><VendorUploadRequest><Header><ScheduleId>002</ScheduleId><ClientId>NIBSS_01</ClientId>"
       
        //string strgetbalinfos = string.Format("SELECT  '{0}' AS ResponseCode,'{1}' as ResponseText,'{2}' AS AccountName,'{3}' AS BalanceDate,{4} As Amount,'{5}' As Currency", respcode, str3, AccountName, BalanceDate, Amount, Currency);

        //DataTable dt = new DataTable("VendorUploadRequest");

        //SqlDataAdapter cmd = new SqlDataAdapter(strgetbalinfos, connection);
        //cmd.Fill(dt);

        // section generate BatchrequestID..
        lblBenAccountNo.Text = "Name from Text";


        //return dt;
        SqlDataAdapter cmdsessionID = new SqlDataAdapter(strSessionID, connection);
        cmdsessionID.Fill(dtsessionID);
        string SessionID = dtsessionID.Rows[0]["SessionID"].ToString().Substring(0, 30);
        string ScheduleId = SessionID;
        string strAcctVal = string.Format("<?xml version=\"1.0\" encoding=\"UTF-8\" ?><VendorUploadRequest><Header><ScheduleId>{0}</ScheduleId><ClientId>{1}</ClientId></Header>"
                                + "<Vendor><VendorNumber>{2}</VendorNumber><AccountName>{3}</AccountName><AccountNumber>{4}</AccountNumber><SortCode>{5}</SortCode></Vendor>"
                                + "<HashValue></HashValue></VendorUploadRequest>", ScheduleId, clientId, "00", lblBenAccountNo.Text,txtBenAccountNo.Text, drpBanks.Value);

        WebReference.PitchControlCenterWithXML ks = new WebReference.PitchControlCenterWithXML();
        //ks.getVendorUpdateStatus(strAcctVal);
        //string verificationdetails = ks.uploadNewVendors(strAcctval);
    

    }

}