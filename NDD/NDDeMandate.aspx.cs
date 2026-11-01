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


public partial class NDD_NDDeMandate : System.Web.UI.Page
{
    public string luser;
    public string StrLocation;
    public string struser;
    public string guser;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //luser = User.Identity.Name;// User.Identity.Name;
            //StrLocation = this.luser.Substring(this.luser.LastIndexOf("?") + 1);
            //struser = this.luser.Substring(0, this.luser.LastIndexOf("?"));
            //guser = this.struser;
            // txtUserid.Text = HttpContext.Current.User.Identity.Name;
            ASPxGridView1.DataBind();
        }
    }

    protected void ASPxGridView1_CustomErrorText(object sender, ASPxGridViewCustomErrorTextEventArgs e)
    {
        e.ErrorText = string.Empty;
        if (e.Exception is NullReferenceException)
            e.ErrorText = " Fill all the setup fields";
        else if (e.Exception is InvalidOperationException)
            e.ErrorText = "InvalidOperationException Text";
        //else if (e.Exception is )
        //cus_val.Text = "InvalidOperationException Text";
    }

    protected void ASPxGridView1_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
    {
        //luser = User.Identity.Name;
        luser = User.Identity.Name;// User.Identity.Name;
        string l_getHeadCode = "";

        luser = Request.Cookies["LogonUserID"].Value;// HttpContext.Current.User.Identity.Name;//HttpContext.Current.User.Identity.Name; //HttpContext.Current.User.Identity.Name;// HttpContext.Current.User.Identity.Name;
        StrLocation = luser.Substring(this.luser.LastIndexOf("?") + 1);
        struser = luser.Substring(0, this.luser.LastIndexOf("?"));
        guser = struser;


        //string l_SeqNo = e.NewValues[0].ToString();


        string name = e.NewValues["payerName"].ToString();
        //
       
        string phone = e.NewValues["payerPhoneNumber"].ToString();
        string email = e.NewValues["payerEmail"].ToString();
        string narration = e.NewValues["narration"].ToString();

        
        string address = e.NewValues["payerAddress"].ToString();
        //string account_number = e.NewValues["account_number"].ToString();
        //ASPxTextBox account_number = (ASPxTextBox)e.NewValues["accountNumber"];
        //ASPxLabel account_name = (ASPxLabel)e.NewValues["accountName"];


        //ASPxDateEdit startDate = (ASPxDateEdit)e.NewValues["startDate"];
        //ASPxDateEdit endDate = (ASPxDateEdit)e.NewValues["endDate"];
        //string startDate = e.NewValues["startDate"].ToString();
        //string endDate = e.NewValues["endDate"].ToString();
        string fixedMandate = ""; int mandateType = 0;
        if ( e.NewValues["mandateTypeCon"] == null)
            {

            mandateType = 2;
            }

         

        string CorpStatus = "0";/* e.NewValues["CorpStatus"].ToString();*/
        //ASPxCheckBox chkaplyRate = (ASPxCheckBox)e.NewValues["Status"];
        string ApplyRate = "";

        object RowInsert;
        if (string.IsNullOrEmpty(name) == true)
        {

            cus_val.Text = "payer name cannot be null or empty";
            return;

        }
        if (string.IsNullOrEmpty(fixedMandate) == true)
        {
            mandateType = 2;
        }
        else if(fixedMandate == "1")
        {
            mandateType = 1;
        }
        else
        {
            mandateType = 2;
        }





        GridViewDataColumn AccountNameHead = (GridViewDataColumn)ASPxGridView1.Columns["Account Name"];
        GridViewDataColumn AccountNoHead = (GridViewDataColumn)ASPxGridView1.Columns["AccountNo"];

        GridViewDataTextColumn FrequencyHead = (GridViewDataTextColumn)ASPxGridView1.Columns["Frequency"];
        GridViewDataTextColumn AmountHead = (GridViewDataTextColumn)ASPxGridView1.Columns["Amount"];

        ASPxLabel lblAcctName = (ASPxLabel)ASPxGridView1.FindEditRowCellTemplateControl(AccountNameHead, "lblAcctName");
        ASPxTextBox txtAccountNo = (ASPxTextBox)ASPxGridView1.FindEditRowCellTemplateControl(AccountNoHead, "txtAccountNo");
        


        ASPxSpinEdit frequency = (ASPxSpinEdit)ASPxGridView1.FindEditRowCellTemplateControl(FrequencyHead, "txtFrequency");
        ASPxSpinEdit amount = (ASPxSpinEdit)ASPxGridView1.FindEditRowCellTemplateControl(AmountHead, "txtamount");

        GridViewDataComboBoxColumn BankCodeHead = (GridViewDataComboBoxColumn)ASPxGridView1.Columns["Bank"];
        ASPxComboBox drpBanks = (ASPxComboBox)ASPxGridView1.FindEditRowCellTemplateControl(BankCodeHead, "drpBanks");


        GridViewDataDateColumn startdateHead = (GridViewDataDateColumn)ASPxGridView1.Columns["Start Date"];
        GridViewDataDateColumn enddateHead = (GridViewDataDateColumn)ASPxGridView1.Columns["End Date"];
        ASPxDateEdit startDate = (ASPxDateEdit)ASPxGridView1.FindEditRowCellTemplateControl(startdateHead, "startDate");
        ASPxDateEdit endDate = (ASPxDateEdit)ASPxGridView1.FindEditRowCellTemplateControl(enddateHead, "enddate");

        GridViewDataComboBoxColumn BillerHead = (GridViewDataComboBoxColumn)ASPxGridView1.Columns["Customer"];
        ASPxComboBox drpbillerId = (ASPxComboBox)ASPxGridView1.FindEditRowCellTemplateControl(BillerHead, "drpbillerId");

        //string billerId = drpbillerId.SelectedItem.GetFieldValue("billerId").ToString();
        string billerId =  drpbillerId.SelectedItem.Value.ToString();
        if (string.IsNullOrEmpty(frequency.Text) == true)
        {

            cus_val.Text = "Frequency cannot be empty or null";

            return;

        }
        if (string.IsNullOrEmpty(amount.Text) == true || amount.Text == "0")
        {

            cus_val.Text = "amount cannot be empty or null";

            return;
         
        }
        string productId = ""; string RequestTimeStamp = DateTime.Now.ToString("yyyyMMddTHHmmssffff");
        string productName ="";
        string strGetProduct = string.Format("SpNDDBillerGetOnly {0}", billerId);
          foreach (DataRow item in NETClass.Classes.GetDataset(strGetProduct).Tables[0].Rows)
            {
            productId = item["productId"].ToString();
            productName  = item["productName"].ToString();
            }

        
        //ASPxComboBox drpBanks = (ASPxComboBox)ASPxGridView1.FindEditRowCellTemplateControl(BankCodeHead, "drpBanks");
        if (string.IsNullOrEmpty(productName) == true)
        {

            cus_val.Text = "product Name cannot be empty or null";
            return;

        }
        DataRow row; DataSet ds = new DataSet();

        #region Revallidate name








        string drpbankcode = drpBanks.SelectedItem.Value.ToString();
        string EazyBankHostCode = System.Configuration.ConfigurationManager.ConnectionStrings["EazyBankHostCode"].ConnectionString;
        string strAcctMaster = string.Format("SpAcctMasterSelectOne '{0}'", txtAccountNo.Text);
        //SqlConnection Conns = new SqlConnection(strCon);

        //SqlDataAdapter cmdAdapter = new SqlDataAdapter(strAcctMaster, Conns);


        if (drpbankcode == EazyBankHostCode)
        {
            string strCon = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnectSource"].ConnectionString;
            string strBenCon = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString;

            SqlConnection Conns = new SqlConnection(strCon);
            SqlCommand cmds = new SqlCommand();
            SqlDataAdapter cmdAdapter = new SqlDataAdapter(strAcctMaster, Conns);

            cmdAdapter.Fill(ds);
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                lblAcctName.Text = item["AccountDesc"].ToString();
            }


        }
        else
        {// interbankcode name enquiry

            #region section fetch account name from other banks

            string str3 = ""; string requestUrl = "";
            string NDDCodeHostUrl = System.Configuration.ConfigurationManager.ConnectionStrings["NDDCodeHostUrl"].ConnectionString;
            requestUrl = string.Format("{0}/{1}", NDDCodeHostUrl, "Easypay/nameEnquiry");
            string AccountNo = txtAccountNo.Text;
            string BankSchemeCode = drpbankcode;




            #region Add Auth Info

            string HeaderUsername = System.Configuration.ConfigurationManager.ConnectionStrings["NDDCodeKeyID"].ConnectionString;
            string Headerpwds = System.Configuration.ConfigurationManager.ConnectionStrings["NDDCodeKey"].ConnectionString;

            string strDeptHeadUserName = NETClass.Classes.DecryptString(HeaderUsername);
            string strDeptHeadpwds = NETClass.Classes.DecryptString(Headerpwds);

            string strAuthToken = Convert.ToBase64String(Encoding.Default.GetBytes(string.Format("{0}:{1}", strDeptHeadUserName, strDeptHeadpwds)));

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUrl);

            httpWebRequest.ContentType = "application/json";

            httpWebRequest.Method = "POST";
            httpWebRequest.Headers.Add("AuthToken", strAuthToken);


            #endregion


           

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string ljson = new JavaScriptSerializer().Serialize(new
                {


                    AccountNo,
                    BankSchemeCode

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
            try
            {
                JavaScriptSerializer jssp = new JavaScriptSerializer();
                //dynamic jsonp = jssp.DeserializeObject(result);


                var jsonSmsInfo = JsonConvert.DeserializeObject(result, serializerSettings);
                dynamic jsonp = jssp.DeserializeObject(result);

                string respcode = ""; string data = ""; string respdesc = "";
                respcode = jsonp["responseCode"];
                respdesc = jsonp["responseText"];

                #endregion
                if (respcode == "00")
                {
                    lblAcctName.Text = jsonp["data"]["accountName"];
                }
            }
            catch (Exception)
            {

                //throw;
            }

        }




        #endregion



        GridViewDataColumn AccountNameHeads = (GridViewDataColumn)ASPxGridView1.Columns["Customer AccountNames"];
        GridViewDataColumn AccountNoHeads = (GridViewDataColumn)ASPxGridView1.Columns["Customer AccountNo"];
        ASPxLabel lblcusAcctName = (ASPxLabel)ASPxGridView1.FindEditRowCellTemplateControl(AccountNameHeads, "lblAcctName");
        ASPxTextBox txtcusAccountNo = (ASPxTextBox)ASPxGridView1.FindEditRowCellTemplateControl(AccountNoHeads, "txtcusAccountNo");

        #region Revallidate name

        DataRow rows; DataSet dsp = new DataSet();

        string strAcctMasters = string.Format("SpAcctMasterSelectOne '{0}'", txtcusAccountNo.Text);



        string strcusCon = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnectSource"].ConnectionString;
        string strcusBenCon = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString;

        SqlConnection cusConns = new SqlConnection(strcusCon);

        SqlDataAdapter cmdcusAdapter = new SqlDataAdapter(strAcctMasters, cusConns);

        cmdcusAdapter.Fill(dsp); string straccountname = "";
        foreach (DataRow item in dsp.Tables[0].Rows)
        {
            straccountname = item["AccountDesc"].ToString();
        }

        #endregion

        if (string.IsNullOrEmpty(straccountname) == true)
        {

            cus_val.Text = "account name cannot be empty or null";

            return;

        }




        if (string.IsNullOrEmpty(lblAcctName.Text) == true)
        {

            cus_val.Text = "account name cannot be empty or null";

            return;

        }
        //DateTime dbStartDate = DateTime.Parse(startDate);
        //DateTime dbEndDate = DateTime.Parse(endDate);
        string strCons = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString;
        string bank_no = System.Configuration.ConfigurationManager.ConnectionStrings["EazyBankHostCode"].ConnectionString;
        SqlConnection Conn = new SqlConnection(strCons);
    

        SqlCommand cmd = new SqlCommand();
        Conn.Open();
        //SqlParameter result;
        cmd.CommandText = "SpAddNDDeMandate";
        cmd.Connection = Conn;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add(new SqlParameter("@billerId", billerId));

        cmd.Parameters.Add(new SqlParameter("@accountNumber", txtcusAccountNo.Text));
        cmd.Parameters.Add(new SqlParameter("@accountName", straccountname));
 

        cmd.Parameters.Add(new SqlParameter("@productId", productId));
        cmd.Parameters.Add(new SqlParameter("@payerName", name));
 
        cmd.Parameters.Add(new SqlParameter("@payerPhoneNumber", phone));
        cmd.Parameters.Add(new SqlParameter("@narration", narration));
        cmd.Parameters.Add(new SqlParameter("@frequency", frequency.Text));
        cmd.Parameters.Add(new SqlParameter("@amount", amount.Text));
        cmd.Parameters.Add(new SqlParameter("@payerEmail", email));
        cmd.Parameters.Add(new SqlParameter("@payerAddress", address));
        cmd.Parameters.Add(new SqlParameter("@payerBankCode", drpBanks.SelectedItem.Value));
        cmd.Parameters.Add(new SqlParameter("@payerAccountName", lblAcctName.Text));
        cmd.Parameters.Add(new SqlParameter("@payerAccountNumber", txtAccountNo.Text));
        //cmd.Parameters.Add(new SqlParameter("@subscriberCode", txtAccountNo.Text));
        cmd.Parameters.Add(new SqlParameter("@subscriberCode", RequestTimeStamp));

        cmd.Parameters.Add(new SqlParameter("@mandateType", mandateType));
        cmd.Parameters.Add(new SqlParameter("@startDate", startDate.Date.ToString("yyyy-MM-dd")));
        cmd.Parameters.Add(new SqlParameter("@endDate", endDate.Date.ToString("yyyy-MM-dd")));
        cmd.Parameters.Add(new SqlParameter("@addedby", guser));
       

        cmd.ExecuteNonQuery();
        Conn.Close();


        string msg = string.Format("New NDDCode Biller Profile  Modification on BillerName- {0}; productName: {1}; rcNumber {2}:email:{3};accountno:{4};accountname:{5};bankcode:{6};Phone:{7}", name, productName, "", email, txtAccountNo.Text, lblAcctName.Text, bank_no, phone);
        Classes.RecordOp(String.Format("Sproc_AddAuditLog '044','{0}','{1}',''", msg, guser));



        int index = -1;

        ASPxGridView1.SettingsEditing.Mode = (GridViewEditingMode)index;
        ASPxGridView1.DataBind();



    }
    protected void ASPxGridView1_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
    {
        luser = User.Identity.Name;// User.Identity.Name;
        string l_getHeadCode = "";

        luser = Request.Cookies["LogonUserID"].Value;// HttpContext.Current.User.Identity.Name;//HttpContext.Current.User.Identity.Name; //HttpContext.Current.User.Identity.Name;// HttpContext.Current.User.Identity.Name;
        StrLocation = luser.Substring(this.luser.LastIndexOf("?") + 1);
        struser = luser.Substring(0, this.luser.LastIndexOf("?"));
        guser = struser;


        string l_SeqNo = e.NewValues[0].ToString();


        string name = e.NewValues["payerName"].ToString();
        //

        string phone = e.NewValues["payerPhoneNumber"].ToString();
        string email = e.NewValues["payerEmail"].ToString();
        string narration = e.NewValues["narration"].ToString();
         
        string address = e.NewValues["payerAddress"].ToString();


        //string account_number = e.NewValues["account_number"].ToString();
        //ASPxTextBox account_number = (ASPxTextBox)e.NewValues["accountNumber"];
        //ASPxLabel account_name = (ASPxLabel)e.NewValues["accountName"];


        //ASPxDateEdit startDate = (ASPxDateEdit)e.NewValues["startDate"];
        //ASPxDateEdit endDate = (ASPxDateEdit)e.NewValues["endDate"];
        //string startDate = e.NewValues["startDate"].ToString();
        //string endDate = e.NewValues["endDate"].ToString();
        string statusDesc = ASPxGridView1.GetRowValuesByKeyValue(e.Keys["SeqNo"], "statusDesc").ToString(); //e.NewValues["statusDesc"].ToString();

        string fixedMandate = ""; int mandateType = 0;
        if (e.NewValues["mandateTypeCon"] == null)
        {

            mandateType = 2;
        }


        string CorpStatus = "0";/* e.NewValues["CorpStatus"].ToString();*/
        //ASPxCheckBox chkaplyRate = (ASPxCheckBox)e.NewValues["Status"];
        string ApplyRate = "";

        object RowInsert;

        if (statusDesc == "Approved")
        {
            cus_val.Text = "Record cannot be editted";
            return;
        }

        if (statusDesc == "DisApproved")
        {
            cus_val.Text = "Record cannot be editted";
            return;
        }
        if (string.IsNullOrEmpty(name) == true)
        {

            cus_val.Text = "payer name cannot be null or empty";
            return;

        }

        if (string.IsNullOrEmpty(fixedMandate) == true)
        {
            mandateType = 2;
        }
        else if (fixedMandate == "1")
        {
            mandateType = 1;
        }
        else
        {
            mandateType = 2;
        }





        GridViewDataColumn AccountNameHead = (GridViewDataColumn)ASPxGridView1.Columns["Account Name"];
        GridViewDataColumn AccountNoHead = (GridViewDataColumn)ASPxGridView1.Columns["AccountNo"];

       
        GridViewDataTextColumn AmountHead = (GridViewDataTextColumn)ASPxGridView1.Columns["Amount"];

        ASPxLabel lblAcctName = (ASPxLabel)ASPxGridView1.FindEditRowCellTemplateControl(AccountNameHead, "lblAcctName");
        ASPxTextBox txtAccountNo = (ASPxTextBox)ASPxGridView1.FindEditRowCellTemplateControl(AccountNoHead, "txtAccountNo");
        GridViewDataComboBoxColumn BankCodeHead = (GridViewDataComboBoxColumn)ASPxGridView1.Columns["Bank"];

        GridViewDataTextColumn FrequencyHead = (GridViewDataTextColumn)ASPxGridView1.Columns["Frequency"];

        ASPxSpinEdit frequency = (ASPxSpinEdit)ASPxGridView1.FindEditRowCellTemplateControl(FrequencyHead, "txtFrequency");
        ASPxSpinEdit amount = (ASPxSpinEdit)ASPxGridView1.FindEditRowCellTemplateControl(AmountHead, "txtamount");

        ASPxComboBox drpBanks = (ASPxComboBox)ASPxGridView1.FindEditRowCellTemplateControl(BankCodeHead, "drpBanks");
        GridViewDataDateColumn startdateHead = (GridViewDataDateColumn)ASPxGridView1.Columns["Start Date"];
        GridViewDataDateColumn enddateHead = (GridViewDataDateColumn)ASPxGridView1.Columns["End Date"];
        ASPxDateEdit startDate = (ASPxDateEdit)ASPxGridView1.FindEditRowCellTemplateControl(startdateHead, "startDate");
        ASPxDateEdit endDate = (ASPxDateEdit)ASPxGridView1.FindEditRowCellTemplateControl(enddateHead, "enddate");
        GridViewDataComboBoxColumn BillerHead = (GridViewDataComboBoxColumn)ASPxGridView1.Columns["Customer"];
        ASPxComboBox drpbillerId = (ASPxComboBox)ASPxGridView1.FindEditRowCellTemplateControl(BillerHead, "drpbillerId");


        string billerId = drpbillerId.SelectedItem.Value.ToString();

        if (string.IsNullOrEmpty(frequency.Text) == true)
        {

            cus_val.Text = "Frequency cannot be empty or null";

            return;

        }
        if (string.IsNullOrEmpty(amount.Text) == true || amount.Text == "0")
        {

            cus_val.Text = "amount cannot be empty or null";

            return;

        }
        string productId = "";
        string productName = "";
        string strGetProduct = string.Format("SpNDDBillerGetOnly {0}", billerId);
        foreach (DataRow item in NETClass.Classes.GetDataset(strGetProduct).Tables[0].Rows)
        {
            productId = item["productId"].ToString();
            productName = item["productName"].ToString();
        }


        //ASPxComboBox drpBanks = (ASPxComboBox)ASPxGridView1.FindEditRowCellTemplateControl(BankCodeHead, "drpBanks");
        if (string.IsNullOrEmpty(productName) == true)
        {

            cus_val.Text = "product Name cannot be empty or null";
            return;

        }
        DataRow row; DataSet ds = new DataSet();

        #region Revallidate name








        string drpbankcode = drpBanks.SelectedItem.Value.ToString();
        string EazyBankHostCode = System.Configuration.ConfigurationManager.ConnectionStrings["EazyBankHostCode"].ConnectionString;
        string strAcctMaster = string.Format("SpAcctMasterSelectOne '{0}'", txtAccountNo.Text.Trim());
        //SqlConnection Conns = new SqlConnection(strCon);

        //SqlDataAdapter cmdAdapter = new SqlDataAdapter(strAcctMaster, Conns);


        if (drpbankcode == EazyBankHostCode)
        {
            string strCon = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnectSource"].ConnectionString;
            string strBenCon = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString;

            SqlConnection Conns = new SqlConnection(strCon);
            SqlCommand cmds = new SqlCommand();
            SqlDataAdapter cmdAdapter = new SqlDataAdapter(strAcctMaster, Conns);

            cmdAdapter.Fill(ds);
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                lblAcctName.Text = item["AccountDesc"].ToString();
            }


        }
        else
        {// interbankcode name enquiry

            #region section fetch account name from other banks

            string str3 = ""; string requestUrl = "";
            string NDDCodeHostUrl = System.Configuration.ConfigurationManager.ConnectionStrings["NDDCodeHostUrl"].ConnectionString;
            requestUrl = string.Format("{0}/{1}", NDDCodeHostUrl, "Easypay/nameEnquiry");
            string AccountNo = txtAccountNo.Text;
            string BankSchemeCode = drpbankcode;




            #region Add Auth Info

            string HeaderUsername = System.Configuration.ConfigurationManager.ConnectionStrings["NDDCodeKeyID"].ConnectionString;
            string Headerpwds = System.Configuration.ConfigurationManager.ConnectionStrings["NDDCodeKey"].ConnectionString;

            string strDeptHeadUserName = NETClass.Classes.DecryptString(HeaderUsername);
            string strDeptHeadpwds = NETClass.Classes.DecryptString(Headerpwds);

            string strAuthToken = Convert.ToBase64String(Encoding.Default.GetBytes(string.Format("{0}:{1}", strDeptHeadUserName, strDeptHeadpwds)));

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUrl);

            httpWebRequest.ContentType = "application/json";

            httpWebRequest.Method = "POST";
            httpWebRequest.Headers.Add("AuthToken", strAuthToken);


            #endregion


            string RequestTimeStamp = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm:ss+0000");

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string ljson = new JavaScriptSerializer().Serialize(new
                {


                    AccountNo,
                    BankSchemeCode

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
            try
            {
                JavaScriptSerializer jssp = new JavaScriptSerializer();
                //dynamic jsonp = jssp.DeserializeObject(result);


                var jsonSmsInfo = JsonConvert.DeserializeObject(result, serializerSettings);
                dynamic jsonp = jssp.DeserializeObject(result);

                string respcode = ""; string data = ""; string respdesc = "";
                respcode = jsonp["responseCode"];
                respdesc = jsonp["responseText"];

                #endregion
                if (respcode == "00")
                {
                    lblAcctName.Text = jsonp["data"]["accountName"];
                }
            }
            catch (Exception)
            {

                //throw;
            }

        }




        #endregion

        GridViewDataColumn AccountNameHeads = (GridViewDataColumn)ASPxGridView1.Columns["Customer AccountNames"];
        GridViewDataColumn AccountNoHeads = (GridViewDataColumn)ASPxGridView1.Columns["Customer AccountNo"];
        ASPxLabel lblcusAcctName = (ASPxLabel)ASPxGridView1.FindEditRowCellTemplateControl(AccountNameHeads, "lblAcctName");
        ASPxTextBox txtcusAccountNo = (ASPxTextBox)ASPxGridView1.FindEditRowCellTemplateControl(AccountNoHeads, "txtcusAccountNo");

        #region Revallidate name

        DataRow rows; DataSet dsp = new DataSet();

        string strAcctMasters = string.Format("SpAcctMasterSelectOne '{0}'", txtcusAccountNo.Text);



        string strcusCon = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnectSource"].ConnectionString;
        string strcusBenCon = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString;

        SqlConnection cusConns = new SqlConnection(strcusCon);

        SqlDataAdapter cmdcusAdapter = new SqlDataAdapter(strAcctMasters, cusConns);

        cmdcusAdapter.Fill(dsp); string straccountname = "";
        foreach (DataRow item in dsp.Tables[0].Rows)
        {
            straccountname = item["AccountDesc"].ToString();
        }

        #endregion

        if (string.IsNullOrEmpty(straccountname) == true)
        {

            cus_val.Text = "account name cannot be empty or null";

            return;

        }

        if (string.IsNullOrEmpty(straccountname) == true)
        {

            cus_val.Text = "account name cannot be empty or null";

            return;

        }
        if (string.IsNullOrEmpty(lblAcctName.Text) == true)
        {

            cus_val.Text = "account name cannot be empty or null";

            return;

        }
        //DateTime dbStartDate = DateTime.Parse(startDate);
        //DateTime dbEndDate = DateTime.Parse(endDate);

        string subscriberCode = ASPxGridView1.GetRowValuesByKeyValue(e.Keys["SeqNo"], "subscriberCode").ToString();
        //string subscriberCode = ASPxGridView1.FindEditFormLayoutItemTemplateControl("subscriberCode").ToString();  
        string strCons = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString;
        string bank_no = System.Configuration.ConfigurationManager.ConnectionStrings["EazyBankHostCode"].ConnectionString;
        SqlConnection Conn = new SqlConnection(strCons);


        SqlCommand cmd = new SqlCommand();
        Conn.Open();
        //SqlParameter result;
        cmd.CommandText = "SpUpdateNDDeMandate";
        cmd.Connection = Conn;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add(new SqlParameter("@SeqNo", e.Keys["SeqNo"]));
        cmd.Parameters.Add(new SqlParameter("@billerId", billerId));
        cmd.Parameters.Add(new SqlParameter("@accountNumber", txtcusAccountNo.Text));
        cmd.Parameters.Add(new SqlParameter("@accountName", straccountname));

        cmd.Parameters.Add(new SqlParameter("@productId", productId));
        cmd.Parameters.Add(new SqlParameter("@payerName", name));

        cmd.Parameters.Add(new SqlParameter("@payerPhoneNumber", phone));
        cmd.Parameters.Add(new SqlParameter("@narration", narration.Trim()));
        cmd.Parameters.Add(new SqlParameter("@frequency", frequency.Text));
        cmd.Parameters.Add(new SqlParameter("@amount", amount.Text));
        cmd.Parameters.Add(new SqlParameter("@payerEmail", email));
        cmd.Parameters.Add(new SqlParameter("@payerAddress", address));
        cmd.Parameters.Add(new SqlParameter("@payerBankCode", drpBanks.SelectedItem.Value));
        cmd.Parameters.Add(new SqlParameter("@payerAccountName", lblAcctName.Text));
        cmd.Parameters.Add(new SqlParameter("@payerAccountNumber", txtAccountNo.Text));
        cmd.Parameters.Add(new SqlParameter("@subscriberCode", subscriberCode));
        cmd.Parameters.Add(new SqlParameter("@mandateType", mandateType));
        cmd.Parameters.Add(new SqlParameter("@startDate", startDate.Date.ToString("yyyy-MM-dd")));
        cmd.Parameters.Add(new SqlParameter("@endDate", endDate.Date.ToString("yyyy-MM-dd")));
        cmd.Parameters.Add(new SqlParameter("@addedby", guser));


        cmd.ExecuteNonQuery();
        Conn.Close();


        string msg = string.Format("New NDDCode eMandate Profile  Modification on BillerName- {0}; productName: {1}; rcNumber {2}:email:{3};accountno:{4};accountname:{5};bankcode:{6};Phone:{7}", name, productName, "", email, txtAccountNo.Text, lblAcctName.Text, drpBanks.SelectedItem.Text, phone);
        Classes.RecordOp(String.Format("Sproc_AddAuditLog '044','{0}','{1}',''", msg, guser));


        ASPxGridView1.SettingsEditing.Mode = (GridViewEditingMode.EditFormAndDisplayRow);// i
        ASPxGridView1.DataBind();




    }
    protected void ASPxGridView1_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
    {
        //if ((e.Column.FieldName == "statusDesc") && (e.Editor.Value.ToString() == "Approved"))
        //{
        //    //e.Editor.Visible = false;
        //    //e.Column.Caption = "";
        //    e.Editor.ReadOnly = true;
        //    // e.Column.ReadOnly = false;


        //}

        //ASPxGridView grid = sender as ASPxGridView;
        //if (e.Column.FieldName == "CategoryID")
        //    e.Editor.ReadOnly = !grid.IsNewRowEditing;
    }
    protected void ASPxGridView1_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
    {
        //luser = User.Identity.Name;
        luser = User.Identity.Name;// User.Identity.Name;
        string l_getHeadCode = "";

        luser = Request.Cookies["LogonUserID"].Value;// HttpContext.Current.User.Identity.Name;//HttpContext.Current.User.Identity.Name; //HttpContext.Current.User.Identity.Name;// HttpContext.Current.User.Identity.Name;
        StrLocation = luser.Substring(this.luser.LastIndexOf("?") + 1);
        struser = luser.Substring(0, this.luser.LastIndexOf("?"));
        guser = struser;

        //string l_getHeadCode = "";
        string phone = e.Values["payerPhoneNumber"].ToString();
        string email = e.Values["payerEmail"].ToString();
        string narration = e.Values["narration"].ToString();

        string billerId = e.Values["billerId"].ToString();
        string address = e.Values["payerAddress"].ToString();
        //string account_number = e.Values["account_number"].ToString();
        //ASPxTextBox account_number = (ASPxTextBox)e.Values["accountNumber"];
        //ASPxLabel account_name = (ASPxLabel)e.Values["accountName"];


        //ASPxDateEdit startDate = (ASPxDateEdit)e.Values["startDate"];
        //ASPxDateEdit endDate = (ASPxDateEdit)e.Values["endDate"];
        string startDate = e.Values["startDate"].ToString();
        string endDate = e.Values["endDate"].ToString();
        string fixedMandate = e.Values["mandateTypeCon"].ToString();
        guser = User.Identity.Name; ;
        string l_SeqNo = e.Keys[0].ToString();

        string name = e.Values["payerName"].ToString();

        string statusDesc = e.Values["statusDesc"].ToString();
    
        if (statusDesc == "Approved")
        {

            cus_val.Text = "record already Approved";
            return;

        }



        string strCon = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString;
        SqlConnection Conn = new SqlConnection(strCon);
        DataSet ds = new DataSet();

        SqlCommand cmd = new SqlCommand();
        Conn.Open();
        SqlParameter result;
        cmd.CommandText = "SpNDDeMandateDelete";
        cmd.Connection = Conn;
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add(new SqlParameter("@SeqNo", l_SeqNo));



        cmd.ExecuteNonQuery();
        Conn.Close();


        string msg = string.Format("Application eMandate Deleteting on - {0};Payer name:{1}; Addres:{2};billerId:{3};narration:{4} ", l_SeqNo, name, address, billerId, narration);
        Classes.RecordOp(String.Format("Sproc_AddAuditLog '004','{0}','{1}',''", msg, guser));

        int index = -1;

        ASPxGridView1.SettingsEditing.Mode = (GridViewEditingMode.EditFormAndDisplayRow);// index;
        ASPxGridView1.DataBind();
        DataBind();

        //Response.Redirect("TerminalRegForm.aspx");



    }


    protected void txtAccountNo_TextChanged(object sender, EventArgs e)
    {
        ASPxTextBox account_number = (ASPxTextBox)sender;
        GridViewDataColumn AccountNameHead = (GridViewDataColumn)ASPxGridView1.Columns["Account Name"];
        GridViewDataComboBoxColumn BankCodeHead = (GridViewDataComboBoxColumn)ASPxGridView1.Columns["Bank"];
        ASPxLabel lblAcctName = (ASPxLabel)ASPxGridView1.FindEditRowCellTemplateControl(AccountNameHead, "lblAcctName");
        ASPxComboBox drpBanks = (ASPxComboBox)ASPxGridView1.FindEditRowCellTemplateControl(BankCodeHead, "drpBanks");
        DataRow row; DataSet ds = new DataSet();
        

        string drpbankcode = "";
        if (drpBanks.SelectedIndex != -1)
        {
            drpbankcode = drpBanks.SelectedItem.Value.ToString();
        }
        string EazyBankHostCode = System.Configuration.ConfigurationManager.ConnectionStrings["EazyBankHostCode"].ConnectionString;

        string strAcctMaster = string.Format("SpAcctMasterSelectOne '{0}'", account_number.Text);

        if (drpbankcode == EazyBankHostCode)
        {
            string strCon = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnectSource"].ConnectionString;
            string strBenCon = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString;

            SqlConnection Conn = new SqlConnection(strCon);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter cmdAdapter = new SqlDataAdapter(strAcctMaster, Conn);

            cmdAdapter.Fill(ds);
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                lblAcctName.Text = item["AccountDesc"].ToString();
            }


        }
        else
        {// interbankcode name enquiry
            lblAcctName.Text = "";
            #region section fetch account name from other banks

            string str3 = ""; string requestUrl = "";
            string NDDCodeHostUrl = System.Configuration.ConfigurationManager.ConnectionStrings["NDDCodeHostUrl"].ConnectionString;
            requestUrl = string.Format("{0}/{1}", NDDCodeHostUrl, "Easypay/nameEnquiry");
            string AccountNo = account_number.Text;
            string BankSchemeCode = drpbankcode;




            #region Add Auth Info

            string HeaderUsername = System.Configuration.ConfigurationManager.ConnectionStrings["NDDCodeKeyID"].ConnectionString;
            string Headerpwds = System.Configuration.ConfigurationManager.ConnectionStrings["NDDCodeKey"].ConnectionString;

            string strDeptHeadUserName = NETClass.Classes.DecryptString(HeaderUsername);
            string strDeptHeadpwds = NETClass.Classes.DecryptString(Headerpwds);

            string strAuthToken = Convert.ToBase64String(Encoding.Default.GetBytes(string.Format("{0}:{1}", strDeptHeadUserName, strDeptHeadpwds)));

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUrl);

            httpWebRequest.ContentType = "application/json";

            httpWebRequest.Method = "POST";
            httpWebRequest.Headers.Add("AuthToken", strAuthToken);


            #endregion


            string RequestTimeStamp = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm:ss+0000");

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string ljson = new JavaScriptSerializer().Serialize(new
                {


                    AccountNo,
                    BankSchemeCode

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
            try
            {
                JavaScriptSerializer jssp = new JavaScriptSerializer();
                //dynamic jsonp = jssp.DeserializeObject(result);


                var jsonSmsInfo = JsonConvert.DeserializeObject(result, serializerSettings);
                dynamic jsonp = jssp.DeserializeObject(result);

                string respcode = ""; string data = ""; string respdesc = "";
                respcode = jsonp["responseCode"];
                respdesc = jsonp["responseText"];

                #endregion
                if (respcode == "00")
                {
                    lblAcctName.Text = jsonp["data"]["accountName"];
                }
            }
            catch (Exception)
            {

                //throw;
            }


        }




    }
    protected void txtcusAccountNo_TextChanged(object sender, EventArgs e)
    {
        ASPxTextBox account_number = (ASPxTextBox)sender;
        GridViewDataColumn AccountNameHead = (GridViewDataColumn)ASPxGridView1.Columns["Customer AccountNames"];
        ASPxLabel lblAcctName = (ASPxLabel)ASPxGridView1.FindEditRowCellTemplateControl(AccountNameHead, "lblAcctName");
        DataRow row; DataSet ds = new DataSet();

        string strAcctMaster = string.Format("SpAcctMasterSelectOne '{0}'", account_number.Text);



        string strCon = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnectSource"].ConnectionString;
        string strBenCon = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString;

        SqlConnection Conn = new SqlConnection(strCon);
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter cmdAdapter = new SqlDataAdapter(strAcctMaster, Conn);

        cmdAdapter.Fill(ds);
        foreach (DataRow item in ds.Tables[0].Rows)
        {
            lblAcctName.Text = item["AccountDesc"].ToString();
        }


    }


    protected void ASPxGridView1_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
    {
        ASPxGridView gridView = sender as ASPxGridView;
        if (e.ButtonID == "btDelete")
        {

            //Server-side actions performed before showing popup here

            gridView.JSProperties["cpRowIndex"] = e.VisibleIndex;
            gridView.JSProperties["cpShowDeleteConfirmBox"] = true;
        }


        if (e.ButtonID == "btRemove")
        {
            //string keyValue = ASPxGridView1.GetRowValues(e.VisibleIndex, "SeqNo");
            string keyValue =  ASPxGridView1.GetRowValues(e.VisibleIndex, "SeqNo").ToString();
            //remove(keyValue);
        }
    }

    protected void ASPxGridView1_Init(object sender, EventArgs e)
    {
        ASPxGridView gridView = sender as ASPxGridView;
        gridView.JSProperties["cpShowDeleteConfirmBox"] = false;
    }


    protected void drpBanks_TextChanged(object sender, EventArgs e)
    {
        ASPxComboBox drpBanks = (ASPxComboBox)sender;
        GridViewDataColumn AccountNameHead = (GridViewDataColumn)ASPxGridView1.Columns["Account Name"];
        GridViewDataColumn AccountNoHead = (GridViewDataColumn)ASPxGridView1.Columns["AccountNo"];
        ASPxLabel lblAcctName = (ASPxLabel)ASPxGridView1.FindEditRowCellTemplateControl(AccountNameHead, "lblAcctName");
        ASPxTextBox account_number = (ASPxTextBox)ASPxGridView1.FindEditRowCellTemplateControl(AccountNoHead, "txtAccountNo");
        //ASPxComboBox drpBanks = (ASPxComboBox)ASPxGridView1.FindEditRowCellTemplateControl(BankCodeHead, "drpBanks");
        DataRow row; DataSet ds = new DataSet();

        string drpbankcode = "";
        if (drpBanks.SelectedIndex != -1)
        {
            drpbankcode = drpBanks.SelectedItem.Value.ToString();
        }
        string EazyBankHostCode = System.Configuration.ConfigurationManager.ConnectionStrings["EazyBankHostCode"].ConnectionString;

        string strAcctMaster = string.Format("SpAcctMasterSelectOne '{0}'", account_number.Text);

        if (drpbankcode == EazyBankHostCode)
        {
            string strCon = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnectSource"].ConnectionString;
            string strBenCon = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString;

            SqlConnection Conn = new SqlConnection(strCon);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter cmdAdapter = new SqlDataAdapter(strAcctMaster, Conn);

            cmdAdapter.Fill(ds);
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                lblAcctName.Text = item["AccountDesc"].ToString();
            }


        }
        else
        {// interbankcode name enquiry
            lblAcctName.Text = "";
            #region section fetch account name from other banks

            string str3 = ""; string requestUrl = "";
            string NDDCodeHostUrl = System.Configuration.ConfigurationManager.ConnectionStrings["NDDCodeHostUrl"].ConnectionString;
            requestUrl = string.Format("{0}/{1}", NDDCodeHostUrl, "Easypay/nameEnquiry");
            string AccountNo = account_number.Text;
            string BankSchemeCode = drpbankcode;




            #region Add Auth Info

            string HeaderUsername = System.Configuration.ConfigurationManager.ConnectionStrings["NDDCodeKeyID"].ConnectionString;
            string Headerpwds = System.Configuration.ConfigurationManager.ConnectionStrings["NDDCodeKey"].ConnectionString;

            string strDeptHeadUserName = NETClass.Classes.DecryptString(HeaderUsername);
            string strDeptHeadpwds = NETClass.Classes.DecryptString(Headerpwds);

            string strAuthToken = Convert.ToBase64String(Encoding.Default.GetBytes(string.Format("{0}:{1}", strDeptHeadUserName, strDeptHeadpwds)));

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUrl);

            httpWebRequest.ContentType = "application/json";

            httpWebRequest.Method = "POST";
            httpWebRequest.Headers.Add("AuthToken", strAuthToken);


            #endregion


            string RequestTimeStamp = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm:ss+0000");

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string ljson = new JavaScriptSerializer().Serialize(new
                {


                    AccountNo,
                    BankSchemeCode

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
            try
            {
                JavaScriptSerializer jssp = new JavaScriptSerializer();
                //dynamic jsonp = jssp.DeserializeObject(result);


                var jsonSmsInfo = JsonConvert.DeserializeObject(result, serializerSettings);
                dynamic jsonp = jssp.DeserializeObject(result);

                string respcode = ""; string data = ""; string respdesc = "";
                respcode = jsonp["responseCode"];
                respdesc = jsonp["responseText"];

                #endregion
                if (respcode == "00")
                {
                    lblAcctName.Text = jsonp["data"]["accountName"];
                }
            }
            catch (Exception)
            {

                //throw;
            }
            
        }




    }

    protected void txtFrequency_NumberChanged(object sender, EventArgs e)
    {
        ASPxSpinEdit frequency = (ASPxSpinEdit)sender;
        GridViewDataDateColumn startdateHead = (GridViewDataDateColumn)ASPxGridView1.Columns["Start Date"];
     
        ASPxDateEdit startdate = (ASPxDateEdit)ASPxGridView1.FindEditRowCellTemplateControl(startdateHead, "startDate");

        GridViewDataDateColumn enddateHead = (GridViewDataDateColumn)ASPxGridView1.Columns["End Date"];
        ASPxDateEdit enddate = (ASPxDateEdit)ASPxGridView1.FindEditRowCellTemplateControl(enddateHead, "enddate");
     
        int ifrequency = Int32.Parse(frequency.Text);
        enddate.Date = startdate.Date.AddMonths(ifrequency).AddDays(-1);

    }

    protected void dateIssue_DateChanged(object sender, EventArgs e)
    {
        ASPxDateEdit startdate = (ASPxDateEdit)sender;

        GridViewDataTextColumn FrequencyHead = (GridViewDataTextColumn)ASPxGridView1.Columns["Frequency"];

        ASPxSpinEdit frequency = (ASPxSpinEdit)ASPxGridView1.FindEditRowCellTemplateControl(FrequencyHead, "txtFrequency");
        GridViewDataDateColumn enddateHead = (GridViewDataDateColumn)ASPxGridView1.Columns["End Date"];
        ASPxDateEdit enddate = (ASPxDateEdit)ASPxGridView1.FindEditRowCellTemplateControl(enddateHead, "enddate");

        int ifrequency = Int32.Parse(frequency.Text);
        enddate.Date = startdate.Date.AddMonths(ifrequency).AddDays(-1);


    }



    protected void txtAccountNo_TextChanged1(object sender, EventArgs e)
    {
        ASPxTextBox account_number = (ASPxTextBox)sender;
        GridViewDataColumn AccountNameHead = (GridViewDataColumn)ASPxGridView1.Columns["Customer AccountNames"];
        ASPxLabel lblAcctName = (ASPxLabel)ASPxGridView1.FindEditRowCellTemplateControl(AccountNameHead, "lblAcctName");
        DataRow row; DataSet ds = new DataSet();

        string strAcctMaster = string.Format("SpAcctMasterSelectOne '{0}'", account_number.Text);



        string strCon = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnectSource"].ConnectionString;
        string strBenCon = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString;

        SqlConnection Conn = new SqlConnection(strCon);
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter cmdAdapter = new SqlDataAdapter(strAcctMaster, Conn);

        cmdAdapter.Fill(ds);
        foreach (DataRow item in ds.Tables[0].Rows)
        {
            lblAcctName.Text = item["AccountDesc"].ToString();
        }

    }
}