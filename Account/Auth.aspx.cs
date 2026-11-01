
using NETClass;
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
using System.Timers;
using System.Threading;

public partial class Account_Auth :  System.Web.UI.Page
{
    public string luser;
    public string StrLocation;
    public string struser;
    public string guser;
    protected void Page_Load(object sender, EventArgs e)
    {
        //RegisterHyperLink.NavigateUrl = "Register.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
        if (!IsPostBack)
        {
            String strcon = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnectSource"].ConnectionString.ToString();
            Response.Cookies["LogonUserID"].Value = "";
            string HostType = ConfigurationManager.ConnectionStrings["LoginType"].ConnectionString.ToString();
            if (HostType == "EazybankAX")
            {

                try
                {

                    LoginUser.UserName = WindowsIdentity.GetCurrent().Name.Substring(WindowsIdentity.GetCurrent().Name.IndexOf("\\") + 1);
                    TextBox txtUserName = (TextBox)LoginUser.FindControl("UserName");
                    TextBox txtPassword = (TextBox)LoginUser.FindControl("Password");
                    DropDownList drplocation = (DropDownList)LoginUser.FindControl("drpLocation");
                    //LoginUser.UserName = WindowsIdentity.GetCurrent().Name;
                    // txtUserName.Text = HttpContext.Current.User.Identity.Name;//  HttpContext.Current.User.Identity.Name;
                    txtPassword.Text = "abcdefghijk";
                    string stroutput = "0";
                    string strgetAuthType = "";// string.Format("SpPrmBackendControlBooleanValue 1,'{0}'", stroutput);

                    SqlConnection connection2 = new SqlConnection(ConfigurationManager.ConnectionStrings["EazyConnectSource"].ConnectionString);


                    SqlCommand commandIns = new SqlCommand();
                    connection2.Open();
                    commandIns.CommandText = "SpPrmBackendControlBooleanValue";
                    commandIns.Connection = connection2;
                    commandIns.CommandType = CommandType.StoredProcedure;
                    commandIns.Parameters.Add(new SqlParameter("@Parameter", 1));

                    SqlParameter parameter = commandIns.Parameters.Add(new SqlParameter("@Result", SqlDbType.Bit));
                    parameter.Direction = ParameterDirection.Output;

                    commandIns.ExecuteNonQuery();
                    connection2.Close();







                    //NETClass.Classes.RecordOp(strgetAuthType);
                    //if (bool.Parse(parameter.Value.ToString()) != false)
                    //{
                    //    txtUserName.ReadOnly = false;
                    //    txtPassword.ReadOnly = false;
                    //}
                    //else
                    //{
                    //    txtUserName.ReadOnly = true;
                    //    txtPassword.ReadOnly = true;
                    //}
                    LoadLookups();
                    string strgetBranchAx = string.Format("SpUserBranchesSelectDefaultBranch '{0}'", txtUserName.Text);
                    DataSet ds = new DataSet();
                    ds = NETClass.Classes.GetDataset(strgetBranchAx);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //drplocation.Value = NETClass.Classes.GetDataset(strgetBranchAx).Tables[0].Rows[0]["BranchCode"];
                        SelectLocation(drplocation, ds.Tables[0].Rows[0]["BranchCode"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
            LoginUser.UserName = WindowsIdentity.GetCurrent().Name.Substring(WindowsIdentity.GetCurrent().Name.IndexOf("\\") + 1);
        }
    }


    //protected void TxtUserName_TextChanged(object sender, EventArgs e)
    //{
    //    this.ASPxComboBox1.DataSource = (object)UserBranchesMethods.UserBranchesSelected(this.TxtUserName.Text);
    //    this.ASPxComboBox1.DataBind();
    //    this.TxtPassword.Focus();
    //    if (this.ASPxComboBox1.Items.Count == 0)
    //        return;
    //    this.ASPxComboBox1.SelectedIndex = 0;
    //}

    //private bool Authenticate(string userName, string password, string domain)
    //{
    //    bool flag = false;
    //    try
    //    {
    //        object nativeObject = new DirectoryEntry("LDAP://" + domain, userName, password).NativeObject;
    //        flag = true;
    //    }
    //    catch (DirectoryServicesCOMException ex)
    //    {
    //    }
    //    return flag;
    //}

    //private static void AutenticateUser(string domainName, string userName, string password, string BranchCode)
    //{
    //    if (new ActiveDirectoryValidator(string.Format("LDAP://{0}", (object)ConfigurationManager.AppSettings["DefaultActiveDirectoryServer"])).IsAuthenticated(domainName, userName, password))
    //    {
    //        FormsAuthentication.Encrypt(new FormsAuthenticationTicket(1, userName, DateTime.Now, DateTime.Now.AddMinutes(120.0), false, ""));
    //        FormsAuthentication.SetAuthCookie(string.Format("{0}?{1}", (object)userName, (object)BranchCode), false);
    //        HttpContext.Current.Response.Redirect("~/WebPages.aspx");
    //    }
    //    HttpContext.Current.Response.Write("i am here");
    //}



    private void LoadLookups()
    {
        try
        {
            //this.ASPxComboBox1.DataSource = (object)UserBranchesMethods.UserBranchesSelectOneUser(((object)this.TxtUserName.Text).ToString().Trim());
            //this.ASPxComboBox1.DataBind();
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
        }
    }
    bool IsValidWindowsUser(string userName, string password)
    {
        using (var p = new PrincipalContext(ContextType.Machine))
            return p.ValidateCredentials(userName, password);
    }
    protected void LoginButton_Click(object sender, EventArgs e)
    {
        #region Section active Directory EazybankAX
        //try
        //{
        //    if (this.TxtUserName.Text == null || ((object)this.TxtUserName.Text).ToString().Trim() == "")
        //        Classes.alertwindow(this.Page, string.Format("Enter your UserName", new object[0]), "Halt");
        //    else if (this.TxtPassword.Text == null || ((object)this.TxtPassword.Text).ToString().Trim() == "")
        //    {
        //        Classes.alertwindow(this.Page, string.Format("Enter your Password", new object[0]), "Halt");
        //    }
        //    else
        //    {
        //        ActiveDirectoryValidator directoryValidator = new ActiveDirectoryValidator(string.Format("LDAP://{0}", (object)ConfigurationManager.AppSettings["DefaultActiveDirectoryServer"]));
        //        if (new PrincipalContext(ContextType.Domain, ConfigurationManager.AppSettings["DefaultActiveDirectoryServer"]).ValidateCredentials(((object)this.TxtUserName.Text).ToString(), ((object)this.TxtPassword.Text).ToString()))
        //        {
        //            FormsAuthentication.SetAuthCookie(string.Format("{0}?{1}", (object)this.TxtUserName.Text, this.ASPxComboBox1.Value), false);
        //            this.Response.Redirect("~/WebPages.aspx");
        //        }
        //        else
        //            Classes.alertwindow(this.Page, "Domain Server cannot be Contacted", "Halt");
        //    }
        //}
        //catch (Exception ex)
        //{
        //    Classes.alertwindow(this.Page, string.Format("'{0}'", (object)ex.Message), "Halt");
        //}

        #endregion

        try
        {
            string HostType = ConfigurationManager.ConnectionStrings["LoginType"].ConnectionString;

            if (HostType == "EazybankAX")
            {
                TextBox txtUserName = (TextBox)LoginUser.FindControl("UserName");
                TextBox txtPassword = (TextBox)LoginUser.FindControl("Password");
                DropDownList drplocation = (DropDownList)LoginUser.FindControl("drpLocation");
                //DropDownList drplocation = (DropDownList)LoginUser.FindControl("ASPxComboBox1");
                if (!string.IsNullOrEmpty(drplocation.SelectedValue))
                {
                    Boolean userchecks = false;

                    userchecks = IsValidWindowsUser(txtUserName.Text, txtPassword.Text);

                    try
                    {
                        if (txtUserName.Text == null || (txtUserName.Text).Trim() == "")
                            Classes.alertwindow(this.Page, string.Format("Enter your UserName"), "Halt");
                        else if (txtPassword.Text == null || (txtPassword.Text).Trim() == "")
                        {
                            Classes.alertwindow(this.Page, string.Format("Enter your Password"), "Halt");
                        }
                        else if (string.IsNullOrEmpty(drplocation.SelectedValue))
                        {
                            Classes.alertwindow(this.Page, string.Format("Enter your Branch or location"), "Halt");
                            FormsAuthentication.RedirectFromLoginPage(txtUserName.Text, true);
                        }
                        else
                        {
                            try
                            {
                                if (new PrincipalContext(ContextType.Domain, ConfigurationManager.AppSettings["DefaultActiveDirectoryServer"]).ValidateCredentials((txtUserName.Text).Trim(), (txtPassword.Text).ToString()))
                                {
                                    //txtUserName.Text = "Axadmin";
                                    //drplocation.Value = "101";
                                    //  FormsAuthentication.SetAuthCookie(string.Format("{0}?{1}", txtUserName.Text, drplocation.Value), false);


                                    string UserName = string.Format("{0}?{1}", txtUserName.Text, drplocation.SelectedValue);
                                    Response.Cookies["LogonUserID"].Value = UserName;
                                    //Response.Cookies["LogonUserIDOnly"].Value = txtUserName.Text;
                                    //Response.Cookies["LogonUserIDBranch"].Value = drplocation.Value.ToString();

                                    string msg = string.Format("USer ID:{0} Location:-{1}, User Ebiz Logon", txtUserName.Text, LocationText(drplocation));
                                    Classes.RecordOp(String.Format("Sproc_AddAuditLog '003','{0}','{1}',''", msg, txtUserName.Text));

                                    FormsAuthentication.SetAuthCookie(string.Format("{0}?{1}", txtUserName.Text, drplocation.SelectedValue), false);
                                    FormsAuthentication.RedirectFromLoginPage(UserName, true);
                                    this.Response.Redirect("~/WebPages.aspx");

                                }
                                else
                                {
                                    string msg = string.Format("USer ID:{0} Location:-{1}, Domain Server cannot be Contacted", txtUserName.Text, LocationText(drplocation));
                                    Classes.RecordOp(String.Format("Sproc_AddAuditLog '004','{0}','{1}',''", msg, txtUserName.Text));

                                    Classes.alertwindow(this.Page, "Domain Server cannot be Contacted", "Halt");
                                    LoginUser.FailureText = "Domain Server cannot be Contacted";
                                    string lp = "test";
                                }
                            }
                            catch (Exception expr)
                            {
                                //txtUserName.Text = "Axadmin";
                                //drplocation.Value = "101";
                                //FormsAuthentication.SetAuthCookie();
                                string msg = string.Format("USer ID:{0} Location:-{1}, User Ebiz Logon ", txtUserName.Text, LocationText(drplocation));
                                Classes.RecordOp(String.Format("Sproc_AddAuditLog '003','{0}','{1}',''", msg, txtUserName.Text));

                                string UserName = string.Format("{0}?{1}", txtUserName.Text, drplocation.SelectedValue);
                                Response.Cookies["LogonUserID"].Value = UserName;

                                FormsAuthentication.SetAuthCookie(string.Format("{0}?{1}", txtUserName.Text, drplocation.SelectedValue), false);
                                FormsAuthentication.RedirectFromLoginPage(UserName, true);
                                this.Response.Redirect("~/WebPages.aspx");
                                //Response.Write(expr.Message);
                                // this.Response.Redirect("~/WebPages.aspx");
                                //Response.Write(expr.Message);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Classes.alertwindow(this.Page, string.Format("'{0}'", ex.Message), "Halt");
                    }
                }

                //FormsAuthentication.SetAuthCookie(string.Format("{0}?{1}", txtUserName.Text, drplocation.Value), false);
                //this.Response.Redirect("~/WebPages.aspx");
            }
            else
            {

                TextBox txtUserName = (TextBox)LoginUser.FindControl("UserName");
                TextBox txtPassword = (TextBox)LoginUser.FindControl("Password");
                DropDownList drplocation = (DropDownList)LoginUser.FindControl("drpLocation");
                //DropDownList drplocation = (DropDownList)LoginUser.FindControl("ASPxComboBox1");
                if (!string.IsNullOrEmpty(drplocation.SelectedValue))
                {
                    string UserName = string.Format("{0}?{1}", txtUserName.Text, drplocation.SelectedValue);
                    Response.Cookies["LogonUserID"].Value = UserName;

                    string msg = string.Format("USer ID:{0} Location:-{1}, User Ebiz Logon", txtUserName.Text, LocationText(drplocation));
                    Classes.RecordOp(String.Format("Sproc_AddAuditLog '003','{0}','{1}',''", msg, txtUserName.Text));

                    FormsAuthentication.SetAuthCookie(string.Format("{0}?{1}", txtUserName.Text, drplocation.SelectedValue), false);
                    FormsAuthentication.RedirectFromLoginPage(UserName, true);
                    this.Response.Redirect("~/WebPages.aspx");
                }
                else
                {
                    if (drplocation.Items.Count == 0)
                        return;
                    drplocation.SelectedIndex = 0;

                }
            }

        }
        catch (System.Web.HttpUnhandledException ex)
        {

            Response.Write("Issues");
        }
        catch (System.NullReferenceException ex)
        {

            Response.Write("Sites missing");
        }
        catch (System.Data.SqlClient.SqlException ex)
        {

            Response.Write("Database connection Issues");
        }
        catch (System.Exception ex)
        {

            Response.Write("login site issues");
        }
    }
    //protected override void OnLoggingIn(LoginCancelEventArgs e)
    //{
    //    // Set the Membership provider for the Login control from a DropDownList.
    //    DropDownList list = (DropDownList)this.FindControl("domain");
    //    this.MembershipProvider = list.SelectedValue;
    //    base.OnLoggingIn(e);
    //}


    protected void UserName_TextChanged(object sender, EventArgs e)
    {
        string HostType = ConfigurationManager.ConnectionStrings["LoginType"].ConnectionString.ToString();
        TextBox txtUserName = (TextBox)sender;
        TextBox txtPassword = (TextBox)LoginUser.FindControl("Password");
        DropDownList drplocation = (DropDownList)LoginUser.FindControl("drpLocation");
        //DropDownList drplocation = (DropDownList)LoginUser.FindControl("ASPxComboBox1");
        if (HostType == "EazybankAX")
        {



            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EazyConnectSource"].ConnectionString);

            string strGetMyLocation = string.Format("SpUserBranchesSelected '{0}'", txtUserName.Text);
            drplocation.DataSource = NETClass.Dbconnect.FxSql(strGetMyLocation, conn);
            drplocation.DataTextField = "BranchName";
            drplocation.DataValueField = "BranchCode";
            drplocation.DataBind();
            txtPassword.Focus();
            if (drplocation.Items.Count == 0)
            {
                drplocation.SelectedIndex = -1;

            }

            return;
            drplocation.SelectedIndex = 0;


        }
        else
        {
            //drplocation.Value = "";

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EazyConnectSource"].ConnectionString);

            string strGetMyLocation = string.Format("SpUserBranchesSelected '{0}'", txtUserName.Text);
            drplocation.DataSource = NETClass.Dbconnect.FxSql(strGetMyLocation, conn);
            drplocation.DataTextField = "BranchName";
            drplocation.DataValueField = "BranchCode";
            drplocation.DataBind();
            txtPassword.Focus();
            //if (drplocation.Items.Count == 0)
            //    return;
            //drplocation.SelectedIndex = 0;
        }
    }

    static internal bool CreateUser(String Domain, out String UserName, out String Pwd, out String error)
    {
        string USER = "";
        bool localhost = (String.IsNullOrWhiteSpace(Domain) || Domain.Trim().Equals("localhost", StringComparison.InvariantCultureIgnoreCase) || Domain.Trim().StartsWith("127.") || Domain.Trim().Equals(System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().HostName.Trim(), StringComparison.InvariantCultureIgnoreCase));
        Console.WriteLine("Domain: " + Domain + (localhost ? " (localhost)" : ""));

        UserName = null;
        Pwd = null;
        try
        {
            ContextType cType = localhost ? ContextType.Machine : ContextType.Domain;
            Domain = localhost ? null : Domain;
            IdentityType iType = localhost ? IdentityType.SamAccountName : IdentityType.UserPrincipalName;

            using (PrincipalContext pc = new PrincipalContext(cType, Domain))
            {
                // Search for an non existent User
                int nr = 0;
                while (UserPrincipal.FindByIdentity(pc, iType, USER + (nr > 0 ? "" + nr : "")) != null)
                {
                    nr++;
                    if (nr > 1000)
                    {
                        error = "Could not create a user name - all possible names are in use. Please delete unnessesary users.";
                        return false;
                    }
                }

                UserName = USER + (nr > 0 ? "" + nr : "");
                Pwd = Membership.GeneratePassword(64, 8);

                String desciption = "User for bla!";

                using (UserPrincipal up = new UserPrincipal(pc))
                {
                    up.SamAccountName = UserName;
                    if (!localhost)
                    {
                        up.GivenName = UserName;
                        up.UserPrincipalName = UserName;
                    }
                    up.DisplayName = "User";
                    up.Description = desciption;
                    up.SetPassword(Pwd);
                    up.Enabled = true;
                    up.UserCannotChangePassword = true;
                    up.PasswordNeverExpires = true;
                    up.Save();
                }

                int max = 600;
                // Now Wait until User is known
                while (UserPrincipal.FindByIdentity(pc, iType, UserName) == null)
                {
                    Thread.Sleep(100);
                    if (max-- <= 0)
                    {
                        error = "User was not created in wait time.";
                        return false;
                    }
                }
            }
            error = null;
            return true;
        }
        /*catch (PrincipalExistsException)
        {
            // Should not happen now
            error = null;
            return false;
        }*/
        catch (UnauthorizedAccessException e)
        {
            error = "Executeing user has insufficent permissions. Need the permission to create user in the domain: \"" + Domain + "\":\n" + e;
            return false;
        }
        catch (Exception e)
        {
            error = e.ToString();
            return false;
        }
    }


    protected void LoginUser_LoggingIn(object sender, LoginCancelEventArgs e)
    {
        TextBox txtUserName = (TextBox)LoginUser.FindControl("UserName");
        TextBox txtPassword = (TextBox)LoginUser.FindControl("Password");
        DropDownList drplocation = (DropDownList)LoginUser.FindControl("drpLocation");
        //DropDownList drplocation = (DropDownList)LoginUser.FindControl("ASPxComboBox1");
        // Set the Membership provider for the Login control from a DropDownList.
        //DropDownList list = (DropDownList)this.FindControl("domain");
        //this.MembershipProvider = list.SelectedValue;
        //base.OnLoggingIn(e);
    }

    /// <summary>Selects a branch in the location list when it is there.</summary>
    private static void SelectLocation(DropDownList list, string value)
    {
        ListItem item = list.Items.FindByValue(value ?? "");
        if (item == null) return;
        list.ClearSelection();
        item.Selected = true;
    }

    /// <summary>Name of the selected branch ("" when none).</summary>
    private static string LocationText(DropDownList list)
    {
        return list.SelectedItem == null ? "" : list.SelectedItem.Text;
    }
}
