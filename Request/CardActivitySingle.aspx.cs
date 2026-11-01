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

public partial class Request_CardActivitySingle : System.Web.UI.Page
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

            //loadMsgs();
            txtUserID.Text = gUser;

        }
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
    private string checkValidateRequest(string Cardno)
    {
        #region Validate Records

        SqlConnection connValidate = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString);


        SqlCommand commandValidate = new SqlCommand();
        connValidate.Open();
        commandValidate.CommandText = "Sproc_GetAllCardNoInfo";
        commandValidate.Connection = connValidate;
        commandValidate.CommandType = CommandType.StoredProcedure;

        commandValidate.Parameters.Add(new SqlParameter("@CardNo", Cardno));
        //commandValidate.Parameters.Add(new SqlParameter("@requestType", requestType));
        //commandValidate.Parameters.Add(new SqlParameter("@userId", userId));
        //commandValidate.Parameters.Add(new SqlParameter("@locationID", LocationCode));

        SqlParameter parameterMsg = commandValidate.Parameters.Add(new SqlParameter("@AccountNo", SqlDbType.VarChar, 5000));
        parameterMsg.Direction = ParameterDirection.Output;
        commandValidate.ExecuteNonQuery();
        connValidate.Close();




        #endregion





        return parameterMsg.Value.ToString();
    }



    protected void Button3_Click(object sender, EventArgs e)
    {
        #region Keep Records
     
       
        string RefNo = string.Format("MNT{0}", Guid.NewGuid().ToString().Substring(0, 12).ToUpper().Replace("-", ""));



        #region Section Maint


        string strlblerror = "";
        try
        {
            luser = HttpContext.Current.User.Identity.Name;
            StrLocation = luser.Substring(luser.LastIndexOf("?") + 1);
            struser = luser.Substring(0, luser.LastIndexOf("?"));
            gUser = struser;

            string strAccountNo = "";string seqNo = "";
            //GridViewDataColumn columnDateExpiry = (GridViewDataColumn)ASPxGridView1.Columns["CardExpiryDate"];
            #region For section
            string strsel = string.Format("Sproc_GetAcctCardNoInfo '{0}'", txtCardNo.Text);
            foreach (DataRow item in NETClass.Classes.SqlDbGetDataset(strsel).Tables[0].Rows)
            {
                strAccountNo = item["depCardAccountNo"].ToString();
                seqNo = item["SeqCardNo"].ToString();
            }


               //string strAccountNo = checkValidateRequest(txtCardNo.Text);

        if (string.IsNullOrEmpty(strAccountNo) == true)
            {
                errMsg.Text = "Invalid Card information or card not in use";
                return;
            }

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

                    commandValidate.Parameters.Add(new SqlParameter("@reason", txtRemarks.Text));
                    commandValidate.Parameters.Add(new SqlParameter("@UserID", gUser));

                    commandValidate.Parameters.Add(new SqlParameter("@SeqNum", seqNo));
                    commandValidate.Parameters.Add(new SqlParameter("@maintType", DrpMaithType.SelectedValue));
                    commandValidate.Parameters.Add(new SqlParameter("@CardInstno", txtCardNo.Text));

                    commandValidate.Parameters.Add(new SqlParameter("@Cardcvv", txtCVV.Text));
                    commandValidate.Parameters.Add(new SqlParameter("@Cardpin", txtPin.Text));
                    commandValidate.Parameters.Add(new SqlParameter("@Cardmonth", txtMonth.Text.Substring(2).Replace("-","")));

                    SqlParameter parameterMsg = commandValidate.Parameters.Add(new SqlParameter("@ResponseDesc", SqlDbType.VarChar, 5000));
                    parameterMsg.Direction = ParameterDirection.Output;

                    commandValidate.ExecuteNonQuery();
                    connValidate.Close();


                    if (parameterMsg.Value.ToString() != "Success")
                    {

                        strlblerror += parameterMsg.Value + "; ";
                    }


            #endregion

            #endregion
  Classes.alertwindow(this.Page, "A New Card Maintenance Batch Request Reference Generated : - " + RefNo, "Halt");
        Response.Write("<script>alert('Request Successful') </script>");
            errMsg.Text = "Successful";
        }
        catch (Exception ex) {
            errMsg.Text = ex.Message;
        }


        //ASPxGridView1.DataBind();
        #endregion

        #endregion
      
        //Response.Redirect("CardRequestInstant.aspx");
    }

}