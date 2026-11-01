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
using System.Configuration;

public partial class Request_CardActivity :  System.Web.UI.Page
{
    public string gUser = HttpContext.Current.User.Identity.Name;//HttpContext.Current.User.Identity.Name;
    public string luser = HttpContext.Current.User.Identity.Name;
    public string StrLocation = "";
    public string struser = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if ( !IsCallback && !IsPostBack)
        {

            luser = HttpContext.Current.User.Identity.Name;
            StrLocation = luser.Substring(luser.LastIndexOf("?") + 1);
            struser = luser.Substring(0, luser.LastIndexOf("?"));
            gUser = struser;

            //loadMsgs();
            txtUserID.Text = gUser;


            drpbranches.Value = "00";
            drp_products.Value = "103";

            //GridViewDataColumn columnBranch = (GridViewDataColumn)ctrlGrid.Columns["BranchName"];
            //ctrlGrid.GroupBy(columnBranch);

            SqlacctDetails.DataBind();
            ASPxGridView1.PageIndex = 0;
        }
    }


    private void loadMsgs()
    {
        //try
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
        //catch (Exception ex) { }

        SqlacctDetails.DataBind();
        txtUserID.Text = struser;
        //txtlocation.Text = StrLocation;
        ASPxGridView1.DataBind();
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
    //protected void ASPxGridView1_BeforePerformDataSelect(object sender, EventArgs e)
    //{

    //    loadMsgs();
    //}
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

        //SqlConnection connValidate = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString);


        //SqlCommand commandValidate = new SqlCommand();
        //connValidate.Open();
        //commandValidate.CommandText = "SpCardValidateRemotes";
        //commandValidate.Connection = connValidate;
        //commandValidate.CommandType = CommandType.StoredProcedure;

        //commandValidate.Parameters.Add(new SqlParameter("@accountNo", AccountNo));
        //commandValidate.Parameters.Add(new SqlParameter("@requestType", requestType));
        //commandValidate.Parameters.Add(new SqlParameter("@userId", userId));
        //commandValidate.Parameters.Add(new SqlParameter("@locationID", LocationCode));

        //SqlParameter parameterMsg = commandValidate.Parameters.Add(new SqlParameter("@responseMsg", SqlDbType.VarChar, 5000));
        //parameterMsg.Direction = ParameterDirection.Output;
        //commandValidate.ExecuteNonQuery();
        //connValidate.Close();




        #endregion





        return "Success";
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

    protected void Button3_Click(object sender, EventArgs e)
    {
        #region Keep Records
        //string cardtype = "Savings";
        //if (ChkCardType.Checked == true)
        //{

        //    cardtype = "Current";

        //}
        string RefNo = string.Format("MNT{0}", Guid.NewGuid().ToString().Substring(0, 12).ToUpper().Replace("-", ""));



        #region Section Maint


        string strlblerror = "";
        try
        {
            luser = HttpContext.Current.User.Identity.Name;
            StrLocation = luser.Substring(luser.LastIndexOf("?") + 1);
            struser = luser.Substring(0, luser.LastIndexOf("?"));
            gUser = struser;
        }
        catch (Exception ex) { }

        String OriginatingIP = GetIPAddress().Replace("::ffff:", "");
        String strHostName = HttpContext.Current.Request.UserHostAddress.ToString();

        OriginatingIP = System.Net.Dns.GetHostAddresses(strHostName).GetValue(0).ToString();
        GridViewDataColumn columnReason = (GridViewDataColumn)ASPxGridView1.Columns["City"];
            GridViewDataColumn columndepCardpanno = (GridViewDataColumn)ASPxGridView1.Columns["depCardpanno"];
        GridViewDataColumn columnAccountName = (GridViewDataColumn)ASPxGridView1.Columns["AccountName"];
        GridViewDataColumn columnAccountNo = (GridViewDataColumn)ASPxGridView1.Columns["AccountNo"];
        GridViewDataColumn columnPin = (GridViewDataColumn)ASPxGridView1.Columns["Pin"];
        GridViewDataColumn columncvv = (GridViewDataColumn)ASPxGridView1.Columns["Cvv"];
        GridViewDataColumn columnExpDate = (GridViewDataColumn)ASPxGridView1.Columns["CardExpDate"];

        columndepCardpanno.Visible = true;
        int VisibleRowsCount = 0;
        if ((ASPxGridView1.VisibleRowCount / ASPxGridView1.SettingsPager.PageSize) > 1)
        {
            VisibleRowsCount = ASPxGridView1.SettingsPager.PageSize;
        }
        else
        {
            VisibleRowsCount = ASPxGridView1.VisibleRowCount;
        }
        //GridViewDataColumn columnDateExpiry = (GridViewDataColumn)ASPxGridView1.Columns["CardExpiryDate"];
        #region For section
        for (int rowIndex = 0; rowIndex < VisibleRowsCount; rowIndex++)
        {

            bool isRowselected = false;
            isRowselected = ASPxGridView1.Selection.IsRowSelected(rowIndex);
            if (isRowselected == true)
            {

                ASPxTextBox tatReason = (ASPxTextBox)ASPxGridView1.FindRowCellTemplateControl(rowIndex, columnReason, "tatReason");

          
            ASPxLabel ChkAccountName = (ASPxLabel)ASPxGridView1.FindRowCellTemplateControl(rowIndex, columnAccountName, "lblAccountName");
            ASPxLabel CardAccountNo = (ASPxLabel)ASPxGridView1.FindRowCellTemplateControl(rowIndex, columnAccountNo, "lblAccountNo");
            ASPxLabel CardPanno   = (ASPxLabel)ASPxGridView1.FindRowCellTemplateControl(rowIndex, columndepCardpanno, "lbldepCardpanno");
            ASPxTextBox CardPin = (ASPxTextBox)ASPxGridView1.FindRowCellTemplateControl(rowIndex, columnPin, "txtPin");
            ASPxTextBox CardCvv = (ASPxTextBox)ASPxGridView1.FindRowCellTemplateControl(rowIndex, columncvv, "txtCvv");
                TextBox CardCvv2 = (TextBox)ASPxGridView1.FindRowCellTemplateControl(rowIndex, columncvv, "txtcvv2");

                TextBox CardExpdate = (TextBox)ASPxGridView1.FindRowCellTemplateControl(rowIndex, columnExpDate, "txtMonth");
           
                string strCardPanno = CardPanno.Text;// ASPxGridView1.GetRowValuesByKeyValue(ASPxListBox1.Items[rowIndex].Value.ToString(), "depCardpanno").ToString();

            string strAccountNo = CardAccountNo.Text;// ASPxGridView1.GetRowValuesByKeyValue(ASPxListBox1.Items[rowIndex].Value.ToString(), "Account No").ToString();
                                     // ctrlGrid.FindRowCellTemplateControlByKey
                                     //string strCardTypes = ctrlGrid.GetRowValuesByKeyValue(ASPxListBox1.Items[rowIndex].Value.ToString(), "CardBrand").ToString();

            

            string ststatus = checkValidateReuest(strAccountNo, strCardPanno, gUser, StrLocation);


            if (tatReason.Text != string.Empty)
            {
                #region Validate Records

                SqlConnection connValidate = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString);



                SqlCommand commandValidate = new SqlCommand();
                connValidate.Open();
                commandValidate.CommandText = "Sproc_AddCardMaint";
                commandValidate.Connection = connValidate;
                commandValidate.CommandType = CommandType.StoredProcedure;
                commandValidate.Parameters.Add(new SqlParameter("@eBatRefNo", RefNo));
                commandValidate.Parameters.Add(new SqlParameter("@requestTitle", txtTitle.Text));
                commandValidate.Parameters.Add(new SqlParameter("@accountNo", strAccountNo));
                commandValidate.Parameters.Add(new SqlParameter("@eIssueDate", DateTime.Today.Date));
                commandValidate.Parameters.Add(new SqlParameter("@eExipryDate", DateTime.Today.Date));

                commandValidate.Parameters.Add(new SqlParameter("@reason", tatReason.Text));
                commandValidate.Parameters.Add(new SqlParameter("@UserID", gUser));

                commandValidate.Parameters.Add(new SqlParameter("@SeqNum",  CardPanno.ToolTip.ToString()));
                commandValidate.Parameters.Add(new SqlParameter("@maintType", DrpMaithType.SelectedValue));
                commandValidate.Parameters.Add(new SqlParameter("@CardInstno", strCardPanno));

                commandValidate.Parameters.Add(new SqlParameter("@Cardcvv", CardCvv.Text));
                commandValidate.Parameters.Add(new SqlParameter("@Cardpin", CardPin.Text));
                commandValidate.Parameters.Add(new SqlParameter("@Cardmonth", CardExpdate.Text));
      
                SqlParameter parameterMsg = commandValidate.Parameters.Add(new SqlParameter("@ResponseDesc", SqlDbType.VarChar, 5000));
                parameterMsg.Direction = ParameterDirection.Output;

                commandValidate.ExecuteNonQuery();
                connValidate.Close();


                if (parameterMsg.Value.ToString() != "Success")
                {

                    strlblerror += parameterMsg.Value + "; ";
                }


                #endregion
            }

            else
            {
                ststatus = "Reason missing";
                strlblerror += ststatus + "; ";
            }

        }

            //    }

            //  }




        }

        #endregion
        #region For by Grid
        DataSet  ds = new DataSet();
        //foreach (DevExpress.Web.Gr item in ASPxGridView1.Columns)
        //{
        //    ds = item.Grid.DetailRows;


        //}

        //String strval = ""; 
        //for (int rowIndex = 0; rowIndex < VisibleRowsCount; rowIndex++)
        //{
        //    bool isRowselected = false;
        //      isRowselected = ASPxGridView1.Selection.IsRowSelected(rowIndex);
        //      ASPxLabel ChkAccountNo = (ASPxLabel)ASPxGridView1.FindRowCellTemplateControl(rowIndex, columnAccountNo, "ASPxTextBox2");
        //    strval = ChkAccountNo.Text;
        //}
        //    DataRow dr;
        //Console.Write("First Details: -" + strval);
        columndepCardpanno.Visible = false;

        #endregion
        //Literal1.Text = strlblerror;


        //ASPxGridView1.DataBind();
        #endregion

        #endregion
        Classes.alertwindow(this.Page, "A New Card Maintenance Batch Request Reference Generated : - " + RefNo, "Halt");
        Response.Write("<script>alert('Request Successful') </script>");
        //Response.Redirect("CardRequestInstant.aspx");
    }

}