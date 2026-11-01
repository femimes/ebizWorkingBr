using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using NETClass;
using System.IO;
using System.Text;
using System.Net;
using System.Security.Cryptography;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using System.Web.Services.Protocols;
using System.Xml.Serialization;
using System.Security;
  
using System.Web.Script.Serialization;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;


public partial class Request_CardRequestInstant :  System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack && !IsCallback)
        {

        }
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        #region Keep Records
        string cardtype = "Savings";
        //if (ChkCardType.Checked == true)
        //    {

        //    cardtype = "Current";

        //    }
        string RefNo = string.Format("DUM{0}", Guid.NewGuid().ToString().Substring(0, 12).ToUpper().Replace("-", ""));

        SqlConnection connectionIB = new SqlConnection(ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString);
        //  string str12 = Guid.NewGuid().ToString().Substring(0, 12).ToUpper().Replace("-", "");
        //string str14 = NETClass.NETClass.GetRequestDataSet(string.Format("SELECT dbo.FnEncryptString('{0}') AS EnCryptValues", str12)).Tables[0].Rows[0]["EnCryptValues"].ToString();
        SqlCommand commandApd = new SqlCommand();
        connectionIB.Open();
        //if (ChkCardType.Checked == false )
        { 
        commandApd.CommandText = "Sproc_AddCardRequestInstantAll";
        commandApd.Connection = connectionIB;

        commandApd.CommandType = CommandType.StoredProcedure;
        commandApd.Parameters.Add(new SqlParameter("@RequestReqNo", RefNo));
        commandApd.Parameters.Add(new SqlParameter("@RequestTitle", txtTitle.Text));
        commandApd.Parameters.Add(new SqlParameter("@CardInstantCount", TxtdummyCount.Text));
        commandApd.Parameters.Add(new SqlParameter("@Remarks", txtRemarks.Text));
        commandApd.Parameters.Add(new SqlParameter("@UserId", "System"));
        commandApd.Parameters.Add(new SqlParameter("@CardBrand", drpCardType.SelectedValue));
        }
        //else
        //{
        //    commandApd.CommandText = "Sproc_AddCardRequestInstantCur";
        //    commandApd.Connection = connectionIB;

        //    commandApd.CommandType = CommandType.StoredProcedure;
        //    commandApd.Parameters.Add(new SqlParameter("@RequestReqNo", RefNo));
        //    commandApd.Parameters.Add(new SqlParameter("@RequestTitle", txtTitle.Text));
        //    commandApd.Parameters.Add(new SqlParameter("@CardInstantCount", TxtdummyCount.Text));
        //    commandApd.Parameters.Add(new SqlParameter("@Remarks", txtRemarks.Text));
        //    commandApd.Parameters.Add(new SqlParameter("@UserId", "System"));


        //}
        commandApd.ExecuteNonQuery();
        connectionIB.Close();



        #endregion
        Classes.alertwindow(this.Page, "A New Dummy Batch Request Reference Generated : - " + RefNo, "Halt");
        Response.Write("<script>alert('Request Successful') </script>");
        //Response.Redirect("CardRequestInstant.aspx");
    }


    protected void Button4_Click(object sender, EventArgs e)
    {
         
        Response.Redirect("CardRequestInstant.aspx");
    }
}