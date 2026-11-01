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


public partial class Request_CardBlock :  System.Web.UI.Page
{
    public string gUser = HttpContext.Current.User.Identity.Name;//HttpContext.Current.User.Identity.Name;
    public string luser = HttpContext.Current.User.Identity.Name;
    public string StrLocation = "";
    public string struser = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsCallback && !IsPostBack)
        {

            luser = HttpContext.Current.User.Identity.Name;
            StrLocation = luser.Substring(luser.LastIndexOf("?") + 1);
            struser = luser.Substring(0, luser.LastIndexOf("?"));
            gUser = struser;

            loadMsgs();
            txtUserID.Text = gUser;
             

                drpbranches.Value = "00";
               drp_products.Value = "103";

            //GridViewDataColumn columnBranch = (GridViewDataColumn)ctrlGrid.Columns["BranchName"];
            //ctrlGrid.GroupBy(columnBranch);


              ASPxGridView1.PageIndex = 0;
        }
    }


    private void loadMsgs()
    {
        try
        {
            luser = HttpContext.Current.User.Identity.Name;
            StrLocation = luser.Substring(luser.LastIndexOf("?") + 1);
            struser = luser.Substring(0, luser.LastIndexOf("?"));
            gUser = struser;

            if (string.IsNullOrEmpty((txtSpecificAcct.Text)) == false)
                {

                string strgetAcctDetails = String.Format("GetAllAccountCardMaintReq '{0}','{1}'", drp_products.Value, drpbranches.Value);
                SqlacctDetails.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString;
                SqlacctDetails.SelectCommand = strgetAcctDetails;
                SqlacctDetails.FilterExpression = String.Format("accountno in ({0})", (txtSpecificAcct.Text));

                SqlacctDetails.SelectCommandType = SqlDataSourceCommandType.Text;

               }
            else
            {

                string strgetAcctDetails = String.Format("GetAllAccountCardMaintReq '{0}','{1}'", drp_products.Value, drpbranches.Value);
                SqlacctDetails.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString;
                SqlacctDetails.SelectCommand = strgetAcctDetails;
                SqlacctDetails.SelectCommandType = SqlDataSourceCommandType.Text;
            }


        
         



        }
        catch (Exception ex) { }


        //txtUserID.Text = struser;
        //txtlocation.Text = StrLocation;
        //ctrlGrid.DataBind();
    }


    public String GetIPAddress()
    {
        //The X-Forwarded-For (XFF) HTTP header field is a de facto standard for identifying the originating IP address of a 
        //client connecting to a web server through an HTTP proxy or load balancer
        String ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

        if (string.IsNullOrEmpty(ip))
        {
            ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        }

        return ip;
    }
    protected void drp_products_SelectedIndexChanged(object sender, EventArgs e)  
    {

        loadMsgs();
    }
    protected void ASPxGridView1_BeforePerformDataSelect(object sender, EventArgs e)
    {

        loadMsgs();
    }
    protected void drpbranches_SelectedIndexChanged(object sender, EventArgs e)
    {

        loadMsgs();
    }
    protected void txtSpecificAcct_TextChanged(object sender, EventArgs e)
    {

        loadMsgs();
    }


    private string checkValidateReuest(string AccountNo, string requestType, string userId, string LocationCode)
    {
        #region Validate Records

        SqlConnection connValidate = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString);


        SqlCommand commandValidate = new SqlCommand();
        connValidate.Open();
        commandValidate.CommandText = "SpCardValidateRemotes";
        commandValidate.Connection = connValidate;
        commandValidate.CommandType = CommandType.StoredProcedure;

        commandValidate.Parameters.Add(new SqlParameter("@accountNo", AccountNo));
        commandValidate.Parameters.Add(new SqlParameter("@requestType", requestType));
        commandValidate.Parameters.Add(new SqlParameter("@userId", userId));
        commandValidate.Parameters.Add(new SqlParameter("@locationID", LocationCode));

        SqlParameter parameterMsg = commandValidate.Parameters.Add(new SqlParameter("@responseMsg", SqlDbType.VarChar, 5000));
        parameterMsg.Direction = ParameterDirection.Output;
        commandValidate.ExecuteNonQuery();
        connValidate.Close();




        #endregion





        return parameterMsg.Value.ToString();
    }

    public void ASPxCheckBox2_CheckedChanged(object sender, EventArgs e)
    {
         
        ASPxCheckBox chkAll = (ASPxCheckBox)sender;
        GridViewDataColumn AccountNoHead = (GridViewDataColumn)ASPxGridView1.Columns["AccountNo"];
        int VisibleRowsCount = 0;
        //Dim AccountNoHead As GridViewDataColumn = CType(ASPxGridView1.Columns("AccountNo"), GridViewDataColumn)
        //Dim VisibleRowsCount As Integer = 0
        if ((ASPxGridView1.VisibleRowCount / ASPxGridView1.SettingsPager.PageSize) > 1)
        {
            VisibleRowsCount = ASPxGridView1.SettingsPager.PageSize;
        }
        else
        {
            VisibleRowsCount = ASPxGridView1.VisibleRowCount;
        }
        if (chkAll.Checked == true)
        {
            for (int rowIndex = 0; rowIndex < VisibleRowsCount; rowIndex++)
            {


                ASPxCheckBox ChkRequestTitle = (ASPxCheckBox)ASPxGridView1.FindRowCellTemplateControl(rowIndex, AccountNoHead, "ASPxCheckBox1");

                ChkRequestTitle.Checked = true;
            }
        }
        else
        {
            for (int rowIndex = 0; rowIndex < VisibleRowsCount; rowIndex++)
            {


                ASPxCheckBox ChkRequestTitle = (ASPxCheckBox)ASPxGridView1.FindRowCellTemplateControl(rowIndex, AccountNoHead, "ASPxCheckBox1");

                ChkRequestTitle.Checked = false;
            }
        }
        
    }
}