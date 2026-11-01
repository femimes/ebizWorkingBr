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

public partial class Request_CardMaintenance : System.Web.UI.Page
{
    public string gUser = HttpContext.Current.User.Identity.Name;//HttpContext.Current.User.Identity.Name;
    public string luser = HttpContext.Current.User.Identity.Name;
    public string StrLocation = "";
    public string struser = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsCallback && !IsPostBack)
        {


            loadMsgs();

            //GridViewDataColumn columnBranch = (GridViewDataColumn)ctrlGrid.Columns["BranchName"];
            //ctrlGrid.GroupBy(columnBranch);
            DatCardIssue.Date = DateTime.Today.Date;
            DatCardExpiry.Date = DateTime.Today.Date;

            this.ctrlGrid.PageIndex = 0;
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
        }
        catch (Exception ex) { }


        txtUserID.Text = struser;
        txtlocation.Text = StrLocation;
        ctrlGrid.DataBind();
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

    protected void BtnApprove_Click(object sender, EventArgs e)
    {

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
        GridViewDataColumn columnReason = (GridViewDataColumn)ctrlGrid.Columns["Reason"];
        GridViewDataColumn columnDateIssue = (GridViewDataColumn)ctrlGrid.Columns["CardIssueDate"];
        GridViewDataColumn columnDateExpiry = (GridViewDataColumn)ctrlGrid.Columns["CardExpiryDate"];

        for (int rowIndex = 0; rowIndex < ASPxListBox1.Items.Count; rowIndex++)
        {
            string strAccountNo = ctrlGrid.GetRowValuesByKeyValue(ASPxListBox1.Items[rowIndex].Value.ToString(), "AccountNo").ToString();
            // ctrlGrid.FindRowCellTemplateControlByKey
            //string strCardTypes = ctrlGrid.GetRowValuesByKeyValue(ASPxListBox1.Items[rowIndex].Value.ToString(), "CardBrand").ToString();
            string strCardTypesDesc = ctrlGrid.GetRowValuesByKeyValue(ASPxListBox1.Items[rowIndex].Value.ToString(), "CardTypeDesc").ToString();

            DateTime dateRequested = DateTime.Parse(ctrlGrid.GetRowValuesByKeyValue(ASPxListBox1.Items[rowIndex].Value.ToString(), "SystemDateTime").ToString());


            ASPxMemo TatReason = (ASPxMemo)ctrlGrid.FindRowCellTemplateControlByKey(ASPxListBox1.Items[rowIndex].Value.ToString(), columnReason, "tatReason");
           // ASPxDateEdit idatCardExpiry = (ASPxDateEdit)ctrlGrid.FindRowCellTemplateControlByKey(ASPxListBox1.Items[rowIndex].Value.ToString(), columnDateIssue, "DatCardExpiry");
            //ASPxDateEdit datCardIssue = (ASPxDateEdit)ctrlGrid.FindRowCellTemplateControlByKey(ASPxListBox1.Items[rowIndex].Value.ToString(), columnDateExpiry, "DatCardIssue");



            string ststatus = checkValidateReuest(strAccountNo, strCardTypesDesc, gUser, StrLocation);
            //if (ststatus == "Success")
            // {
            //string strins = string.Format("SpAddCardRemoteRequest '{0}','{1}','{2}','{3}','{4}'", txtAccountNo.Text, gUser, drpTypes.SelectedItem.Value, StrLocation, OriginatingIP);
            //NETClass.Classes.GetDataset(strins);
            //for (int vrowIndex = 0; vrowIndex < ctrlGrid.VisibleRowCount; vrowIndex++)

            //{
            //    //ASPxTextBox TatCity = (ASPxTextBox)ctrlGrid.FindRowCellTemplateControl(rowIndex, columnCity, "TxtCity");
                //ASPxMemo TatAccountDesc = (ASPxMemo)ctrlGrid.FindRowCellTemplateControl(rowIndex, columnAccountDesc, "tatAccountDesc");
                //ASPxDateEdit datCardExpiry = (ASPxDateEdit)ctrlGrid.FindRowCellTemplateControl(vrowIndex, columnDateIssue, "DatCardExpiry");
                //ASPxDateEdit datCardIssue = (ASPxDateEdit)ctrlGrid.FindRowCellTemplateControl(vrowIndex, columnDateExpiry, "DatCardIssue");

               // if (TatReason.ToolTip == ASPxListBox1.Items[rowIndex].Value.ToString())
               // {
                if (TatReason.Text != string.Empty)
                { 
                #region Validate Records

                    SqlConnection connValidate = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString);
           


                SqlCommand commandValidate = new SqlCommand();
                connValidate.Open();
                commandValidate.CommandText = "Sproc_AddDeleteCardMaint";
                commandValidate.Connection = connValidate;
                commandValidate.CommandType = CommandType.StoredProcedure;
                commandValidate.Parameters.Add(new SqlParameter("@eBatRefNo", drpBatchRefNo.Value));
                commandValidate.Parameters.Add(new SqlParameter("@accountNo", strAccountNo));
                commandValidate.Parameters.Add(new SqlParameter("@eIssueDate", DatCardIssue.Date));
                commandValidate.Parameters.Add(new SqlParameter("@eExipryDate", DatCardExpiry.Date));
        
                commandValidate.Parameters.Add(new SqlParameter("@reason", TatReason.Text));
                commandValidate.Parameters.Add(new SqlParameter("@UserID", gUser));
         
                commandValidate.Parameters.Add(new SqlParameter("@SeqNum", ASPxListBox1.Items[rowIndex].Value.ToString()));

                //SqlParameter parameterMsgCode = commandValidate.Parameters.Add(new SqlParameter("@ResponseCode", SqlDbType.VarChar, 50));
                //parameterMsgCode.Direction = ParameterDirection.Output;

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


            //    }

          //  }




}

        Literal1.Text = strlblerror;


        ctrlGrid.DataBind();

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


    //protected void btnDisApprove_Click(object sender, EventArgs e)
    //{

    //    string strlblerror = "";
    //    try
    //    {
    //        luser = HttpContext.Current.User.Identity.Name;
    //        StrLocation = luser.Substring(luser.LastIndexOf("?") + 1);
    //        struser = luser.Substring(0, luser.LastIndexOf("?"));
    //        gUser = struser;
    //    }
    //    catch (Exception ex) { }



    //    String OriginatingIP = GetIPAddress().Replace("::ffff:", "");
    //    String strHostName = HttpContext.Current.Request.UserHostAddress.ToString();

    //    OriginatingIP = System.Net.Dns.GetHostAddresses(strHostName).GetValue(0).ToString();
    //    //GridViewDataColumn columnReason = (GridViewDataColumn)ctrlGrid.Columns["remarks"];
    //    //for (int rowIndex = 0; rowIndex < ASPxListBox1.Items.Count; rowIndex++)
    //    //{
    //    //    string strAccountNo = ctrlGrid.GetRowValuesByKeyValue(ASPxListBox1.Items[rowIndex].Value.ToString(), "accountNo").ToString();
    //    //    // ctrlGrid.FindRowCellTemplateControlByKey
    //    //    //string strCardTypes = ctrlGrid.GetRowValuesByKeyValue(ASPxListBox1.Items[rowIndex].Value.ToString(), "requestType").ToString();
    //    //    //string strCardTypesDesc = ctrlGrid.GetRowValuesByKeyValue(ASPxListBox1.Items[rowIndex].Value.ToString(), "CardTypeDesc").ToString();

    //    //    //DateTime dateRequested = DateTime.Parse(ctrlGrid.GetRowValuesByKeyValue(ASPxListBox1.Items[rowIndex].Value.ToString(), "systemDateTime").ToString());

    //    //    //TextBox tatBox = (TextBox)ctrlGrid.FindRowCellTemplateControlByKey(ASPxListBox1.Items[rowIndex].Value.ToString(), columnReason, "chkTat");//(TextBox)ctrlGrid.FindControl("chkTat");
    //    //    ASPxMemo TatReason = (ASPxMemo)ctrlGrid.FindRowCellTemplateControlByKey(ASPxListBox1.Items[rowIndex].Value.ToString(), columnReason, "tatReason");

    //    //    //if (ChkVoucherID.Checked == true)
    //    //    {
    //    //        //  string l_passWrd = GetUniqueKey();
    //    //        string strapprove;



    //    //        #region Section Approve Accounts



    //    //        #endregion



    //    //    }
    //    //    string ststatus = "Success";
    //    //    // string ststatus = checkValidateReuest(strAccountNo, strCardTypes, gUser, StrLocation);
    //    //    if (ststatus == "Success")
    //    //    {
    //    //        //string strins = string.Format("SpAddCardRemoteRequest '{0}','{1}','{2}','{3}','{4}'", txtAccountNo.Text, gUser, drpTypes.SelectedItem.Value, StrLocation, OriginatingIP);
    //    //        //NETClass.Classes.GetDataset(strins);

    //    //        //if (TatReason.Text == "" || TatReason.Text == string.Empty)
    //    //        //{
    //    //        //    //string strins = string.Format("SpAddCardRemoteRequest '{0}','{1}','{2}','{3}','{4}'", txtAccountNo.Text, gUser, drpTypes.SelectedItem.Value, StrLocation, OriginatingIP);
    //    //        //    //NETClass.Classes.GetDataset(strins);
    //    //        //    ststatus = string.Format("missing reason on UserName '{0}'", strAccountNo);
    //    //        //    strlblerror += ststatus + "; ";

    //    //        //}
    //    //        if (gUser == "" || gUser == string.Empty)
    //    //        {

    //    //            ststatus = string.Format("User details missing");
    //    //            strlblerror += ststatus + "; ";
    //    //        }
    //    //        else
    //    //        {

    //    //            #region Validate Records

    //    //            SqlConnection connValidate = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString);


    //    //            SqlCommand commandValidate = new SqlCommand();
    //    //            connValidate.Open();
    //    //            commandValidate.CommandText = "Sproc_ATMDeAppreRequest";
    //    //            commandValidate.Connection = connValidate;
    //    //            commandValidate.CommandType = CommandType.StoredProcedure;

    //    //            //commandValidate.Parameters.Add(new SqlParameter("@Trans_date", dateRequested));
    //    //            commandValidate.Parameters.Add(new SqlParameter("@Account_no", strAccountNo));
    //    //            commandValidate.Parameters.Add(new SqlParameter("@UserID", gUser));
    //    //            commandValidate.Parameters.Add(new SqlParameter("@Status", 4));
    //    //            commandValidate.Parameters.Add(new SqlParameter("@Trans_date", DateTime.Today.Date));
    //    //            commandValidate.Parameters.Add(new SqlParameter("@Narrative", "Dis approved By- " + gUser));
    //    //            commandValidate.Parameters.Add(new SqlParameter("@ExtRefNo", ASPxListBox1.Items[rowIndex].Value.ToString()));

    //    //            //SqlParameter parameterMsgCode = commandValidate.Parameters.Add(new SqlParameter("@ResponseCode", SqlDbType.VarChar, 50));
    //    //            //parameterMsgCode.Direction = ParameterDirection.Output;

    //    //            //SqlParameter parameterMsg = commandValidate.Parameters.Add(new SqlParameter("@ResponseDesc", SqlDbType.VarChar, 5000));
    //    //            //parameterMsg.Direction = ParameterDirection.Output;

    //    //            commandValidate.ExecuteNonQuery();
    //    //            connValidate.Close();


    //    //            //if (parameterMsg.Value.ToString() != "Success")
    //    //            //{

    //    //            //    strlblerror += parameterMsg.Value + "; ";
    //    //            //}


    //    //            #endregion

    //    //        }


    //    //    }

    //    //    else
    //    //    {
    //    //        strlblerror += ststatus + "; ";
    //    //    }



    //    //}

    //    Literal1.Text = strlblerror;


    //    ctrlGrid.DataBind();

    //}
}