using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Diagnostics;
using System.Security.Principal;
using System.Web;
using System.Web.Profile;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
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

public partial class eAuthValidation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       if (!base.IsPostBack && !base.IsCallback)
        {
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {

        #region Validate Users
        string strUserName = TextBox1.Text;
        string strPwds = TextBox2.Text;
        string RespCode = "00";

        try
        {
            // System.DirectoryServices.AccountManagement.ActiveDirectoryValidator directoryValidator = new ActiveDirectoryValidator(string.Format("LDAP://{0}", (object)ConfigurationManager.AppSettings["DefaultActiveDirectoryServer"]));
            if (new PrincipalContext(ContextType.Domain, ConfigurationManager.AppSettings["DefaultActiveDirectoryServer"]).ValidateCredentials(strUserName, strPwds))
            {
                //txtUserName.Text = "Axadmin";
                //drplocation.Value = "101";
                //  FormsAuthentication.SetAuthCookie(string.Format("{0}?{1}", txtUserName.Text, drplocation.Value), false);


                //string UserName = string.Format("{0}?{1}", strUserName, drplocation.Value);
                //Response.Cookies["LogonUserID"].Value = UserName;
                //FormsAuthentication.SetAuthCookie(string.Format("{0}?{1}", txtUserName.Text, drplocation.Value), false);
                //FormsAuthentication.RedirectFromLoginPage(UserName, true);
                //this.Response.Redirect("~/WebPages.aspx");
                Response.Write(RespCode);
            }
            else
            {
                RespCode = "02";
                //RespDesc = "Domain Server cannot be Contacted";
                Response.Write(RespCode);
                //Classes.alertwindow(this.Page, "Domain Server cannot be Contacted", "Halt");
                //LoginUser.FailureText = "Domain Server cannot be Contacted";
                //string lp = "test";
            }
        }
        catch (Exception expr)
        {
            RespCode = "03";
            //RespDesc = expr.Message;
            Response.Write(expr.Message);
        }


        #endregion

    }
}