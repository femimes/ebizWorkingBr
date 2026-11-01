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



public partial class Request_eChannelsRequest : System.Web.UI.Page
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

            GridViewDataColumn columnBranch = (GridViewDataColumn)ctrlGrid.Columns["BranchName"];
            ctrlGrid.GroupBy(columnBranch);
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
        SqlDataSource1.DataBind();
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

    protected void BtnApprove_Click3(object sender, EventArgs e)
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
        GridViewDataColumn columnAccountDesc = (GridViewDataColumn)ctrlGrid.Columns["AccountDesc"];
        GridViewDataColumn columnAddress = (GridViewDataColumn)ctrlGrid.Columns["Address"];
        GridViewDataColumn columnCity = (GridViewDataColumn)ctrlGrid.Columns["city"];
        GridViewDataColumn columnDOB = (GridViewDataColumn)ctrlGrid.Columns["DOB"];
        GridViewDataColumn columnPhoneNo = (GridViewDataColumn)ctrlGrid.Columns["PhoneNo"];

        if (txtref_no.Text == null || txtref_no.Text == string.Empty)
        {
            txtref_no.Text = Guid.NewGuid().ToString().Substring(0, 12).ToUpper();
            NETClass.Classes.alertwindow(this.Page, string.Format("A new Batch Request Reference Generated : - {0}", this.txtref_no.Text), "Halt");





            for (int rowIndex = 0; rowIndex < ctrlGrid.VisibleRowCount; rowIndex++)
            {

                ASPxTextBox TatCity = (ASPxTextBox)ctrlGrid.FindRowCellTemplateControl(rowIndex, columnCity, "TxtCity");
                ASPxMemo TatAccountDesc = (ASPxMemo)ctrlGrid.FindRowCellTemplateControl(rowIndex, columnAccountDesc, "tatAccountDesc");
                //string strTrackId = TatAccountDesc./*ToolTip*/;
                ASPxTextBox txtCity = (ASPxTextBox)ctrlGrid.FindRowCellTemplateControl(rowIndex, columnCity, "TxtCity");
                ASPxMemo TatAddress = (ASPxMemo)ctrlGrid.FindRowCellTemplateControl(rowIndex, columnAddress, "tatAddress");

                ASPxDateEdit DOB = (ASPxDateEdit)ctrlGrid.FindRowCellTemplateControl(rowIndex, columnDOB, "ASPxDateEdit1");

                ASPxTextBox txtPhoneNo = (ASPxTextBox)ctrlGrid.FindRowCellTemplateControl(rowIndex, columnPhoneNo, "ASPxTextBox4");


                for (int irowIndex = 0; irowIndex < ASPxListBox1.Items.Count; irowIndex++)

                {

                    if (TatAccountDesc != null)
                    {
                        //Response.Write("I am here");
                        if (TatAccountDesc.ToolTip == ASPxListBox1.Items[irowIndex].Value.ToString())
                        {
                            string strAccountNo = ctrlGrid.GetRowValuesByKeyValue(ASPxListBox1.Items[irowIndex].Value.ToString(), "accountNo").ToString();
                            // ctrlGrid.FindRowCellTemplateControlByKey
                            string strCardTypes = ctrlGrid.GetRowValuesByKeyValue(ASPxListBox1.Items[irowIndex].Value.ToString(), "requestType").ToString();
                            string strCardTypesDesc = ctrlGrid.GetRowValuesByKeyValue(ASPxListBox1.Items[irowIndex].Value.ToString(), "CardTypeDesc").ToString();
                            //string strAccountDesc = ctrlGrid.GetRowValuesByKeyValue(ASPxListBox1.Items[rowIndex].Value.ToString(), "AccountDesc").ToString();


                            string ststatus = checkValidateReuest(strAccountNo, strCardTypes, gUser, StrLocation);
                            //  if (ststatus == "Success")
                            {

                                #region Save Records

                                SqlConnection connection2 = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString);


                                SqlCommand commandIns = new SqlCommand();
                                connection2.Open();
                                commandIns.CommandText = "Sproc_AddCardRequestMob";
                                commandIns.Connection = connection2;
                                commandIns.CommandType = CommandType.StoredProcedure;
                                commandIns.Parameters.Add(new SqlParameter("@RefNo", txtref_no.Text));
                                commandIns.Parameters.Add(new SqlParameter("@AccountNo", strAccountNo));
                                commandIns.Parameters.Add(new SqlParameter("@Description", TatAccountDesc.Text));
                                commandIns.Parameters.Add(new SqlParameter("@PanNo", ""));
                                commandIns.Parameters.Add(new SqlParameter("@Address", TatAddress.Text));
                                commandIns.Parameters.Add(new SqlParameter("@City", TatCity.Text));
                                commandIns.Parameters.Add(new SqlParameter("@CountryCode", "NGA"));
                                commandIns.Parameters.Add(new SqlParameter("@CardBrand", strCardTypes));
                                commandIns.Parameters.Add(new SqlParameter("@CardIssueDate", dateIssue.Date));

                                commandIns.Parameters.Add(new SqlParameter("@CardExpiryDate", dateexpiry.Date));
                                commandIns.Parameters.Add(new SqlParameter("@CardCounter", Int64.Parse("0")));
                                commandIns.Parameters.Add(new SqlParameter("@Seq", Int64.Parse("2")));

                                commandIns.Parameters.Add(new SqlParameter("@RequestTitle", txtTitle.Text));

                                commandIns.Parameters.Add(new SqlParameter("@DOB", DOB.Date));
                                commandIns.Parameters.Add(new SqlParameter("@AddedBy", gUser));


                                commandIns.Parameters.Add(new SqlParameter("@ApprovedBy", "System"));

                                commandIns.Parameters.Add(new SqlParameter("@CardPhoneNo", txtPhoneNo.Text));


                                commandIns.ExecuteNonQuery();
                                connection2.Close();

                                //string RefNo = parameter.Value.ToString();

                                //#endregion

                                #region Section Update status on card remote records

                                string strUpd = string.Format("SpCardRemoteReqStatusInfoUpd '{0}',{1},'{2}'", ASPxListBox1.Items[irowIndex].Value.ToString(), 9, gUser);
                                NETClass.Classes.RecordOp(strUpd);
                                #endregion




                                #endregion


                                //        }

                                //    }
                            }



                        }



                    }

                    //}       
                    //catch (Exception)
                    //{

                    //    Response.Write("");
                    //}

                    Literal1.Text = strlblerror;


                    ctrlGrid.DataBind();
                }

            }

        }

        //Response.Redirect("eChannelsRequest.aspx");

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
        GridViewDataColumn columnAccountDesc = (GridViewDataColumn)ctrlGrid.Columns["AccountDesc"];
        GridViewDataColumn columnAddress = (GridViewDataColumn)ctrlGrid.Columns["Address"];
        GridViewDataColumn columnCity = (GridViewDataColumn)ctrlGrid.Columns["city"];
        GridViewDataColumn columnDOB = (GridViewDataColumn)ctrlGrid.Columns["DOB"];
        GridViewDataColumn columnPhoneNo = (GridViewDataColumn)ctrlGrid.Columns["PhoneNo"];

        if (txtref_no.Text == null || txtref_no.Text == string.Empty)
        {
            txtref_no.Text = Guid.NewGuid().ToString().Substring(0, 12).ToUpper();
            NETClass.Classes.alertwindow(this.Page, string.Format("A new Batch Request Reference Generated : - {0}", this.txtref_no.Text), "Halt");

            foreach (var key in ctrlGrid.GetSelectedFieldValues("remSeqNo"))
            {

                if (ctrlGrid.Selection.IsRowSelectedByKey(key))
                {

                    string l_refNo = ctrlGrid.GetRowValuesByKeyValue(key, "remSeqNo").ToString();
                    string strAccountNo = ctrlGrid.GetRowValuesByKeyValue(key, "accountNo").ToString();
                    // ctrlGrid.FindRowCellTemplateControlByKey
                    string strCardTypes = drpBatchRefNo.Value.ToString();//  ctrlGrid.GetRowValuesByKeyValue(key, "requestType").ToString();
                    string strCardTypesDesc = drpBatchRefNo.Text.ToString();// ctrlGrid.GetRowValuesByKeyValue(key, "CardTypeDesc").ToString();

                    //string strCardTypes = ctrlGrid.GetRowValuesByKeyValue(ASPxListBox1.Items[irowIndex].Value.ToString(), "requestType").ToString();
                    //string strCardTypesDesc = ctrlGrid.GetRowValuesByKeyValue(ASPxListBox1.Items[irowIndex].Value.ToString(), "CardTypeDesc").ToString();
                    //string strAccountDesc = ctrlGrid.GetRowValuesByKeyValue(ASPxListBox1.Items[rowIndex].Value.ToString(), "AccountDesc").ToString();


                    //DateTime dateRequested = DateTime.Parse(ctrlGrid.GetRowValuesByKeyValue(key, "systemDateTime").ToString());

                    //for (int rowIndex = 0; rowIndex < ctrlGrid.VisibleRowCount; rowIndex++)


                    {

                        ASPxTextBox TatCity = (ASPxTextBox)ctrlGrid.FindRowCellTemplateControlByKey(key, columnCity, "TxtCity");
                        ASPxMemo TatAccountDesc = (ASPxMemo)ctrlGrid.FindRowCellTemplateControlByKey(key, columnAccountDesc, "tatAccountDesc");
                        //string strTrackId = TatAccountDesc./*ToolTip*/;
                        ASPxTextBox txtCity = (ASPxTextBox)ctrlGrid.FindRowCellTemplateControlByKey(key, columnCity, "TxtCity");
                        ASPxMemo TatAddress = (ASPxMemo)ctrlGrid.FindRowCellTemplateControlByKey(key, columnAddress, "tatAddress");

                        ASPxDateEdit DOB = (ASPxDateEdit)ctrlGrid.FindRowCellTemplateControlByKey(key, columnDOB, "ASPxDateEdit1");

                        ASPxTextBox txtPhoneNo = (ASPxTextBox)ctrlGrid.FindRowCellTemplateControlByKey(key, columnPhoneNo, "ASPxTextBox4");

                        if (TatAccountDesc != null)
                        {
                            //Response.Write("I am here");
                            //if (TatAccountDesc.ToolTip == ASPxListBox1.Items[irowIndex].Value.ToString())


                            //string ststatus = checkValidateReuest(strAccountNo, strCardTypes, gUser, StrLocation);

                            //  if (ststatus == "Success")
                            {

                                #region Save Records

                                SqlConnection connection2 = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString);


                                SqlCommand commandIns = new SqlCommand();
                                connection2.Open();
                                commandIns.CommandText = "Sproc_AddCardRequestMobTrack";
                                commandIns.Connection = connection2;
                                commandIns.CommandType = CommandType.StoredProcedure;
                                commandIns.Parameters.Add(new SqlParameter("@RefNo", txtref_no.Text));
                                commandIns.Parameters.Add(new SqlParameter("@AccountNo", strAccountNo));
                                commandIns.Parameters.Add(new SqlParameter("@Description", TatAccountDesc.Text));
                                commandIns.Parameters.Add(new SqlParameter("@PanNo", ""));
                                commandIns.Parameters.Add(new SqlParameter("@Address", TatAddress.Text));
                                commandIns.Parameters.Add(new SqlParameter("@City", TatCity.Text));
                                commandIns.Parameters.Add(new SqlParameter("@CountryCode", "NGA"));
                                commandIns.Parameters.Add(new SqlParameter("@CardBrand", strCardTypes));
                                commandIns.Parameters.Add(new SqlParameter("@CardIssueDate", dateIssue.Date));

                                commandIns.Parameters.Add(new SqlParameter("@CardExpiryDate", dateexpiry.Date));
                                commandIns.Parameters.Add(new SqlParameter("@CardCounter", Int64.Parse("0")));
                                commandIns.Parameters.Add(new SqlParameter("@Seq", Int64.Parse("2")));

                                commandIns.Parameters.Add(new SqlParameter("@RequestTitle", txtTitle.Text));

                                commandIns.Parameters.Add(new SqlParameter("@DOB", DOB.Date));
                                commandIns.Parameters.Add(new SqlParameter("@AddedBy", gUser));


                                commandIns.Parameters.Add(new SqlParameter("@ApprovedBy", "System"));

                                commandIns.Parameters.Add(new SqlParameter("@CardPhoneNo", txtPhoneNo.Text));
                                commandIns.Parameters.Add(new SqlParameter("@TrackID", l_refNo));

                                commandIns.ExecuteNonQuery();
                                connection2.Close();

                                //string RefNo = parameter.Value.ToString();

                                //#endregion

                                #region Section Update status on card remote records

                                string strUpd = string.Format("SpCardRemoteReqStatusInfoUpd '{0}',{1},'{2}'", l_refNo, 9, gUser);
                                NETClass.Classes.RecordOp(strUpd);
                                #endregion




                                #endregion


                                //        }

                                //    }
                            }






                        }

                        //}       
                        //catch (Exception)
                        //{

                        //    Response.Write("");
                        //}

                    


                    }

                }
              
            }
            Literal1.Text = strlblerror;


                ctrlGrid.DataBind();
        }
        //Response.Redirect("eChannelsRequest.aspx");

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




    protected void btncancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("eChannelsRequest.aspx");
    }


    protected void btnDisApprove_Click(object sender, EventArgs e)
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
        GridViewDataColumn columnAccountDesc = (GridViewDataColumn)ctrlGrid.Columns["AccountDesc"];
        GridViewDataColumn columnAddress = (GridViewDataColumn)ctrlGrid.Columns["Address"];
        GridViewDataColumn columnCity = (GridViewDataColumn)ctrlGrid.Columns["city"];
        GridViewDataColumn columnDOB = (GridViewDataColumn)ctrlGrid.Columns["DOB"];
        GridViewDataColumn columnPhoneNo = (GridViewDataColumn)ctrlGrid.Columns["PhoneNo"];

        foreach (var key in ctrlGrid.GetSelectedFieldValues("remSeqNo"))
        {
            string l_refNo = ctrlGrid.GetRowValuesByKeyValue(key, "remSeqNo").ToString();
            string strAccountNo = ctrlGrid.GetRowValuesByKeyValue(key, "accountNo").ToString();
            // ctrlGrid.FindRowCellTemplateControlByKey
            string strCardTypes = drpBatchRefNo.Value.ToString();//  ctrlGrid.GetRowValuesByKeyValue(key, "requestType").ToString();
            string strCardTypesDesc = drpBatchRefNo.Text.ToString();// ctrlGrid.GetRowValuesByKeyValue(key, "CardTypeDesc").ToString();

            string ststatus = checkValidateReuest(strAccountNo, strCardTypes, gUser, StrLocation);
            if (ctrlGrid.Selection.IsRowSelectedByKey(key))
            {

                #region Save Records

                //SqlConnection connection2 = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString);


                //SqlCommand commandIns = new SqlCommand();
                //connection2.Open();
                //commandIns.CommandText = "Sproc_AddCardRequestMob";
                //commandIns.Connection = connection2;
                //commandIns.CommandType = CommandType.StoredProcedure;
                //commandIns.Parameters.Add(new SqlParameter("@RefNo", txtref_no.Text));
                //commandIns.Parameters.Add(new SqlParameter("@AccountNo", strAccountNo));
                //commandIns.Parameters.Add(new SqlParameter("@Description", TatAccountDesc.Text));
                //commandIns.Parameters.Add(new SqlParameter("@PanNo", ""));
                //commandIns.Parameters.Add(new SqlParameter("@Address", TatAddress.Text));
                //commandIns.Parameters.Add(new SqlParameter("@City", TatCity.Text));
                //commandIns.Parameters.Add(new SqlParameter("@CountryCode", "NGA"));
                //commandIns.Parameters.Add(new SqlParameter("@CardBrand", strCardTypes));
                //commandIns.Parameters.Add(new SqlParameter("@CardIssueDate", dateIssue.Date));

                //commandIns.Parameters.Add(new SqlParameter("@CardExpiryDate", dateexpiry.Date));
                //commandIns.Parameters.Add(new SqlParameter("@CardCounter", 0));
                //commandIns.Parameters.Add(new SqlParameter("@Seq", 2));

                //commandIns.Parameters.Add(new SqlParameter("@RequestTitle", txtTitle.Text));

                //commandIns.Parameters.Add(new SqlParameter("@DOB", DOB.Date));
                //commandIns.Parameters.Add(new SqlParameter("@AddedBy", gUser));


                //commandIns.Parameters.Add(new SqlParameter("@ApprovedBy", "System"));

                //commandIns.Parameters.Add(new SqlParameter("@CardPhoneNo", txtPhoneNo.Text));


                //commandIns.ExecuteNonQuery();
                //connection2.Close();
                // Section Update Records in Card Remote status



                //string RefNo = parameter.Value.ToString();

                //#endregion


                string strUpd = string.Format("SpCardRemoteReqStatusInfoUpd '{0}',{1},'{2}'", l_refNo, 8, gUser);
                NETClass.Classes.RecordOp(strUpd);

                #endregion


            }

            // else
            {
                //     strlblerror += ststatus + "; ";
            }


        }
            ctrlGrid.DataBind();


    }


}


