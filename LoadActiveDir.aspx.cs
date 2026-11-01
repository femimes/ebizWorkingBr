using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
 using System.Data;
using System.Configuration;
public partial class LoadActiveDir : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack && !IsCallback)
        {
            string strAuthenticUsr = "";

            string strAuthenticpwd = "";
            //checks if LDAP server is unavailable. 
            //ConfigurationManager.AppSettings["DefaultActiveDirectoryServer"] LDAP Server name e.g value="ombplc.com"/
            if (new PrincipalContext(ContextType.Domain, ConfigurationManager.AppSettings["DefaultActiveDirectoryServer"]).ValidateCredentials(strAuthenticUsr, strAuthenticpwd))
            {

                var context = new PrincipalContext(ContextType.Domain, "http://pokkie");
                var searcher = new PrincipalSearcher(new UserPrincipal(context));

                
                
                foreach (var result in searcher.FindAll())
                {
                    DropDownList1.Items.Add(result.Name);
                }

                DropDownList1.DataBind();
            }
            else
            {
                Response.Write("Domain Server cannot be Contacted");

            }
        }
      


    } 
}