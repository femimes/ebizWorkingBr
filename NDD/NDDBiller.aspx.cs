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

public partial class NDD_NDDBiller : System.Web.UI.Page
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


        string name = e.NewValues["name"].ToString();
        //string productName = e.NewValues["productName"].ToString();
        string phone = e.NewValues["phoneNumber"].ToString();
        string email = e.NewValues["email"].ToString();

        string rcNumber = e.NewValues["rcNumber"].ToString();
        string address = e.NewValues["address"].ToString();
        //string account_number = e.NewValues["account_number"].ToString();
        ASPxTextBox account_number = (ASPxTextBox)e.NewValues["accountNumber"];
        ASPxLabel account_name = (ASPxLabel)e.NewValues["accountName"];

        string CorpStatus = "0";/* e.NewValues["CorpStatus"].ToString();*/
        //ASPxCheckBox chkaplyRate = (ASPxCheckBox)e.NewValues["Status"];
        string ApplyRate = "";

        object RowInsert;
        if (string.IsNullOrEmpty(name) == true)
        {

            cus_val.Text = "Biller name cannot be null or empty";
            return;

        }

        //if (string.IsNullOrEmpty(productName) == true)
        //{

        //    cus_val.Text = "product Name cannot be empty or null";
        //    return;

        //}

        if (string.IsNullOrEmpty(rcNumber) == true)
        {

            cus_val.Text = "rcNumber cannot be empty or null";

            return;

        }


        GridViewDataColumn AccountNameHead = (GridViewDataColumn)ASPxGridView1.Columns["Account Name"];
        GridViewDataColumn AccountNoHead = (GridViewDataColumn)ASPxGridView1.Columns["AccountNo"];
        ASPxLabel lblAcctName = (ASPxLabel)ASPxGridView1.FindEditRowCellTemplateControl(AccountNameHead, "lblAcctName");
        ASPxTextBox txtAccountNo = (ASPxTextBox)ASPxGridView1.FindEditRowCellTemplateControl(AccountNoHead, "txtAccountNo");

        #region Revallidate name

        DataRow row; DataSet dsp = new DataSet();

        string strAcctMaster = string.Format("SpAcctMasterSelectOne '{0}'", txtAccountNo.Text);



        string strCon = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnectSource"].ConnectionString;
        string strBenCon = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString;

        SqlConnection Conns = new SqlConnection(strCon);

        SqlDataAdapter cmdAdapter = new SqlDataAdapter(strAcctMaster, Conns);

        cmdAdapter.Fill(dsp); string straccountname = "";
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
        string strCons = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString;
        string bank_no = System.Configuration.ConfigurationManager.ConnectionStrings["EazyBankHostCode"].ConnectionString;
        SqlConnection Conn = new SqlConnection(strCons);
        DataSet ds = new DataSet();

        SqlCommand cmd = new SqlCommand();
        Conn.Open();
        //SqlParameter result;
        cmd.CommandText = "SpNDDBillerAdd";
        cmd.Connection = Conn;
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add(new SqlParameter("@name", name));
        cmd.Parameters.Add(new SqlParameter("@phoneNumber", phone));
        cmd.Parameters.Add(new SqlParameter("@productName", ""));
        cmd.Parameters.Add(new SqlParameter("@rcNumber", rcNumber));
        cmd.Parameters.Add(new SqlParameter("@email", email));
        cmd.Parameters.Add(new SqlParameter("@address", address));
        cmd.Parameters.Add(new SqlParameter("@bankCode", bank_no));
        cmd.Parameters.Add(new SqlParameter("@accountName", straccountname));
        cmd.Parameters.Add(new SqlParameter("@accountNumber", txtAccountNo.Text));
        cmd.Parameters.Add(new SqlParameter("@mandateStatusNotificationUrl", "1"));
        cmd.Parameters.Add(new SqlParameter("@addedby", guser));


        cmd.ExecuteNonQuery();
        Conn.Close();


        string msg = string.Format("New NDDCode Biller Profile  Modification on BillerName- {0}; productName: {1}; rcNumber {2}:email:{3};accountno:{4};accountname:{5};bankcode:{6};Phone:{7}", name, "", rcNumber, email, txtAccountNo.Text, straccountname, bank_no, phone);
        Classes.RecordOp(String.Format("Sproc_AddAuditLog '044','{0}','{1}',''", msg, guser));



        int index = -1;

        ASPxGridView1.SettingsEditing.Mode = (GridViewEditingMode)index;
        ASPxGridView1.DataBind();



    }
    protected void ASPxGridView1_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
    {
        //luser = User.Identity.Name;
        //luser = User.Identity.Name;// User.Identity.Name;
        //string l_getHeadCode = "";

        //luser = Request.Cookies["LogonUserID"].Value;// HttpContext.Current.User.Identity.Name;//HttpContext.Current.User.Identity.Name; //HttpContext.Current.User.Identity.Name;// HttpContext.Current.User.Identity.Name;
        //StrLocation = luser.Substring(this.luser.LastIndexOf("?") + 1);
        //struser = luser.Substring(0, this.luser.LastIndexOf("?"));
        //guser = struser;


        ////string l_SeqNo = e.Keys[0].ToString();
        //string CorpCode = e.NewValues["keyid"].ToString();
        ////string l_SeqNo = e.Keys[0].ToString();// e.NewValues["SeqNo"].ToString();
        //string CorpDesc = e.NewValues["keyDesc"].ToString();
        //string Corptype = e.NewValues["keytype"].ToString();
        //string remarks = e.NewValues["Remarks"].ToString();
        //string CorpStatus = "0";/* e.NewValues["CorpStatus"].ToString();*/
        ////ASPxCheckBox chkaplyRate = (ASPxCheckBox)e.NewValues["Status"];
        //string ApplyRate = "";

        //object RowInsert;
        //if (string.IsNullOrEmpty(CorpCode) == true)
        //{

        //    cus_val.Text = "Setup Code cannot be null or empty";
        //    return;

        //}

        //if (string.IsNullOrEmpty(CorpDesc) == true)
        //{

        //    cus_val.Text = "Setup description cannot be empty or null";
        //    return;

        //}

        //if (string.IsNullOrEmpty(Corptype) == true)
        //{

        //    cus_val.Text = "Setup type details cannot be empty or null";

        //    return;

        //}





        //string strCons = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString;
        //SqlConnection Conn = new SqlConnection(strCons);
        //DataSet ds = new DataSet();

        //SqlCommand cmd = new SqlCommand();
        //Conn.Open();
        ////SqlParameter result;
        //cmd.CommandText = "SpAddAllKYCinfo";
        //cmd.Connection = Conn;
        //cmd.CommandType = CommandType.StoredProcedure;

        //cmd.Parameters.Add(new SqlParameter("@keyid", CorpCode));
        //cmd.Parameters.Add(new SqlParameter("@keydesc", CorpDesc));
        //cmd.Parameters.Add(new SqlParameter("@Keytype", Corptype));
        //cmd.Parameters.Add(new SqlParameter("@Remarks", remarks));
        ////cmd.Parameters.Add(new SqlParameter("@CorpAcctNo", CorpAcctNo));

        ////cmd.Parameters.Add(new SqlParameter("@CorpStatus", "0"));
        ////cmd.Parameters.Add(new SqlParameter("@AddedBy", guser));


        //cmd.ExecuteNonQuery();
        //Conn.Close();


        //string msg = string.Format("New KYC Code  Modification on Code- {0}; Description: {1}; type {2}:", CorpCode, CorpDesc, Corptype);
        //Classes.RecordOp(String.Format("Sproc_AddAuditLog '044','{0}','{1}',''", msg, guser));


        //ASPxGridView1.SettingsEditing.Mode = (GridViewEditingMode.EditFormAndDisplayRow);// i
        //ASPxGridView1.DataBind();




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
        luser = User.Identity.Name;
        luser = User.Identity.Name;// User.Identity.Name;
        string l_getHeadCode = "";
        //StrLocation = luser.Substring(this.luser.LastIndexOf("?") + 1);
        //struser = luser.Substring(0, this.luser.LastIndexOf("?"));
        guser = User.Identity.Name; ;
        string l_SeqNo = e.Keys[0].ToString();

        string name = e.Values["name"].ToString();
        string contact = e.Values["address"].ToString();
        string phone = e.Values["phoneNumber"].ToString();
        string email = e.Values["email"].ToString();

        string tin = e.Values["rcNumber"].ToString();


        string account_number = e.Values["accountNumber"].ToString();
        //string account_number = e.NewValues["account_number"].ToString();
        //ASPxTextBox account_number = (ASPxTextBox)e.Values["accountNumber"];
        string statusDesc = e.Values["statusDesc"].ToString();
        //try
        //{
        //NETClass.Classes.GetPath(13);
        //}catch (Exception px )
        //    {
        //        ASPxGridView1.Caption = px.Message;
        //    }
        object RowInsert; lblMsg.Text = "";
        if (statusDesc == "Approved")
        {

            lblMsg.Text = "record already Approved";
            return;

        }

        //if (string.IsNullOrEmpty(IntRateDesc) == true)
        //{

        //    cus_val.Text = "Setup description cannot be empty or null";
        //    return;

        //}



        string strCon = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString;
        SqlConnection Conn = new SqlConnection(strCon);
        DataSet ds = new DataSet();

        SqlCommand cmd = new SqlCommand();
        Conn.Open();
        SqlParameter result;
        cmd.CommandText = "SpNDDBillerDelete";
        cmd.Connection = Conn;
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add(new SqlParameter("@SeqNo", l_SeqNo));



        cmd.ExecuteNonQuery();
        Conn.Close();


        string msg = string.Format("Application NDD Create Biller Deleteting on - {0};Merchant name:{1}; contact:{2};account:{3} ", l_SeqNo, name, contact, account_number);
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