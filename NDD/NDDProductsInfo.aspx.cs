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


public partial class NDD_NDDProductsInfo :  System.Web.UI.Page
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
            //StrLocation = this.luser.Substring(this.luser.LastIndexOf("?") + 1);
            //struser = this.luser.Substring(0, this.luser.LastIndexOf("?"));
            //guser = this.struser;
            // txtUserid.Text = HttpContext.Current.User.Identity.Name;
            if (Request.QueryString["q"].ToString() != "3")
            {
                Response.Redirect("NDDBiller.aspx");
            }
            txtmch_no.Text = Request.QueryString["p"].ToString();
            txtsub_no.Text = Request.QueryString["r"].ToString();
          
            ASPxGridView1.DataBind();
        }
    }

    protected void ASPxGridView1_CustomErrorText(object sender, ASPxGridViewCustomErrorTextEventArgs e)
    {
        e.ErrorText = string.Empty;
        if (e.Exception is NullReferenceException)
            e.ErrorText = " Fill all the setup fields/ expired session";
        else if (e.Exception is InvalidOperationException)
            e.ErrorText = "InvalidOperationException Text";
        //else if (e.Exception is )
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


        string l_SeqNo = e.NewValues[0].ToString();


        
        string productName = e.NewValues["productName"].ToString();
        string billerId = txtmch_no.Text.ToString();
        string name = txtsub_no.Text.ToString();
 
        string CorpStatus = "0";/* e.NewValues["CorpStatus"].ToString();*/
        
        string ApplyRate = "";

        object RowInsert;
        if (string.IsNullOrEmpty(productName) == true)
        {

            cus_val.Text = "product Title cannot be null or empty";
            return;

        }



 
        #region Revallidate name

        DataRow row; DataSet dsp = new DataSet();

        
        #endregion
 



        #region Create Qrcode Merchant on nibss
        string str3 = ""; string requestUrl = "";
        string NDDCodeHostUrl = System.Configuration.ConfigurationManager.ConnectionStrings["NDDCodeHostUrl"].ConnectionString;
        requestUrl = string.Format("{0}/{1}", NDDCodeHostUrl, "CreateProduct");






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
        httpWebRequest.Headers.Add("TransToken", strAuthToken);

        #endregion

        string RequestTimeStamp = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm:ss+0000");

        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        {
            string ljson = new JavaScriptSerializer().Serialize(new
            {


                billerId,

                productName 


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

        
        var serializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore
        };

        JavaScriptSerializer jssp = new JavaScriptSerializer();
        //dynamic jsonp = jssp.DeserializeObject(result);


        var jsonSmsInfo = JsonConvert.DeserializeObject(result, serializerSettings);
        dynamic jsonp = jssp.DeserializeObject(result);

        string respcode = ""; string data = ""; string respdesc = "";
        respcode = jsonp["responseCode"];
        respdesc = jsonp["responseText"];

        #endregion





        string strCons = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString;
        string OrderSn = "";
        //string bank_no = System.Configuration.ConfigurationManager.ConnectionStrings["EazyBankHostCode"].ConnectionString;
        if (respcode == "00")
        {
            
            string productId = jsonp["data"]["data"]["id"];
 
            SqlConnection Conn = new SqlConnection(strCons);
            DataSet ds = new DataSet();

            SqlCommand cmd = new SqlCommand();
            Conn.Open();
            //SqlParameter result;
            cmd.CommandText = "SpNDDProductHistadd";
            cmd.Connection = Conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@user_id", guser));
            cmd.Parameters.Add(new SqlParameter("@billerId", billerId));
            cmd.Parameters.Add(new SqlParameter("@name", name));

            cmd.Parameters.Add(new SqlParameter("@productId", productId));

            cmd.Parameters.Add(new SqlParameter("@productName", productName));
            cmd.Parameters.Add(new SqlParameter("@status", "3"));

            

            cmd.ExecuteNonQuery();
            Conn.Close();


        }

        string strinfo = string.Format("Biller/BillerId/Product Name:- {0}/{1}", name,billerId, productName);


        string msg = string.Format("New NDDBiller Product Profile  Modification on NDDBiller Name- {0};  Message:{1}; AddedBy:{2}", strinfo, result, guser);
        Classes.RecordOp(String.Format("Sproc_AddAuditLog '044','{0}','{1}',''", msg, guser));



        int index = -1;

        ASPxGridView1.SettingsEditing.Mode = (GridViewEditingMode)index;
        ASPxGridView1.DataBind();



    }
    protected void ASPxGridView1_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
    {
         
 

    }
    protected void ASPxGridView1_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
    {
        
    }
    protected void ASPxGridView1_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
    {
        luser = User.Identity.Name;
        luser = User.Identity.Name;// User.Identity.Name;
        string l_getHeadCode = "";
        //StrLocation = luser.Substring(this.luser.LastIndexOf("?") + 1);
        //struser = luser.Substring(0, this.luser.LastIndexOf("?"));
        guser = User.Identity.Name; ;
        string l_SeqNo = e.Keys[0].ToString();

        
        object RowInsert;

 
        //ASPxTextBox account_number = (ASPxTextBox)e.Values["accountNumber"];
        string statusDesc = e.Values["statusDesc"].ToString();

        string strCon = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString;
        SqlConnection Conn = new SqlConnection(strCon);
        DataSet ds = new DataSet();

        SqlCommand cmd = new SqlCommand();
        Conn.Open();
        SqlParameter result;
        cmd.CommandText = "SpNDDProductDelete";
        cmd.Connection = Conn;
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add(new SqlParameter("@SeqNo", l_SeqNo));



        cmd.ExecuteNonQuery();
        Conn.Close();


        string msg = string.Format("Application NDD Product Deleting Deleteting on - {0}; ", l_SeqNo);
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


    protected void drpMchno_SelectedIndexChanged(object sender, EventArgs e)
    {
        ASPxGridView1.DataBind();
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
    }

    protected void ASPxGridView1_Init(object sender, EventArgs e)
    {
        ASPxGridView gridView = sender as ASPxGridView;
        gridView.JSProperties["cpShowDeleteConfirmBox"] = false;
    }
}