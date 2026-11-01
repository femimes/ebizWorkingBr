using NETClass;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices.AccountManagement;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.IO;

public partial class Account_Login : System.Web.UI.Page
{
    // ---------- Controls inside the asp:Login template ----------
    private TextBox UserNameBox  { get { return (TextBox)LoginUser.FindControl("UserName"); } }
    private TextBox PasswordBox  { get { return (TextBox)LoginUser.FindControl("Password"); } }
    private CheckBox RememberBox { get { return (CheckBox)LoginUser.FindControl("RememberMe"); } }
    private DropDownList BranchList { get { return (DropDownList)LoginUser.FindControl("ASPxComboBox1"); } }

    private static string HostType
    {
        get { return ConfigurationManager.ConnectionStrings["LoginType"].ConnectionString; }
    }

    private static string ConnectionString
    {
        get { return ConfigurationManager.ConnectionStrings["EazyConnectSource"].ConnectionString; }
    }

    // =====================================================================
    // Page load
    // =====================================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
            return;

        Response.Cookies["LogonUserID"].Value = "";
        Response.Cookies["LogonUserIDName"].Value = "";
        // Prefill the username from the Windows identity (as before)
        string identity = WindowsIdentity.GetCurrent().Name;
        LoginUser.UserName = identity.Substring(identity.IndexOf("\\") + 1);

        try
        {
            // Load this user's branches straight away so the list is not empty
            BindBranches(LoginUser.UserName);

            if (HostType == "EazybankAX")
            {
                // Pre-select the user's default branch, if it is in their list
                DataSet ds = NETClass.Classes.GetDataset(string.Format(
                    "SpUserBranchesSelectDefaultBranch '{0}'", Sql(LoginUser.UserName)));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    SelectBranch(ds.Tables[0].Rows[0]["BranchCode"].ToString());
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Trace.WriteLine(ex.Message);
        }
    }

    // =====================================================================
    // Username changed -> reload branches, keep the current pick if still valid
    // =====================================================================
    protected void UserName_TextChanged(object sender, EventArgs e)
    {
        string previous = BranchList.SelectedValue;

        BindBranches(UserNameBox.Text);
        SelectBranch(previous);

        PasswordBox.Focus();
    }

    // =====================================================================
    // Sign in
    // =====================================================================
    protected void LoginButton_Click(object sender, EventArgs e)
    {
        string userName = (UserNameBox.Text ?? "").Trim();
        string password = PasswordBox.Text ?? "";

        if (userName == "") { Fail("Enter your username."); return; }

        // Read the branch the person picked
        ListItem branch = GetSelectedBranch(userName);
        if (branch == null) { Fail("Select your branch."); return; }

        try
        {
            if (HostType == "EazybankAX")
            {
                if (password.Trim() == "") { Fail("Enter your password."); return; }

                bool valid;
                try
                {
                    using (PrincipalContext pc = new PrincipalContext(ContextType.Domain,
                               ConfigurationManager.AppSettings["DefaultActiveDirectoryServer"]))
                    {
                        valid = pc.ValidateCredentials(userName, password);
                        //AddtoLogFile(string.Format("UserID-{0} Validation:-{1}", userName,valid), "UserAD Validation");
                        
                    }
                }
                catch (Exception ex)
                {
                    // Never sign someone in when the check itself failed
                    //AddtoLogFile(string.Format("UserID-{0} Validation:-{1} stacktrace: {2};/{3}", userName, ex.Message,ex.StackTrace,ex.InnerException), "UserAD ValidationIssues");
                    Audit("004", string.Format("USer ID:{0} Location:-{1}, Domain check error: {2}",
                        userName, branch.Text, ex.Message), userName);
                    Fail("We couldn't reach the domain server. Try again in a moment.");
                    return;
                }

                if (!valid)
                {
                    Audit("004", string.Format("USer ID:{0} Location:-{1}, Invalid username or password",
                        userName, branch.Text), userName);
                    Fail("Your username or password is incorrect.");
                    return;
                }
            }

            SignIn(userName, branch);
        }
        catch (SqlException)
        {
            Fail("Database connection issues.");
        }
        catch (Exception ex)
        {
            Fail("Login site issues: " + ex.Message);
        }
    }

    // =====================================================================
    // Branch helpers
    // =====================================================================

    /// <summary>Fills the branch list for a user, with a "Select your branch" placeholder.</summary>
    private void BindBranches(string userName)
    {
        DropDownList ddl = BranchList;
        ddl.Items.Clear();

        if (!string.IsNullOrWhiteSpace(userName))
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                ddl.DataSource = NETClass.Dbconnect.FxSql(
                    string.Format("SpUserBranchesSelected '{0}'", Sql(userName.Trim())), conn);
                ddl.DataTextField = "BranchName";
                ddl.DataValueField = "BranchCode";
                ddl.DataBind();
            }
        }

        ddl.Items.Insert(0, new ListItem("Select your branch", ""));

        // Only one real branch? Pick it for them.
        ddl.SelectedIndex = (ddl.Items.Count == 2) ? 1 : 0;
    }

    /// <summary>Selects a branch by code if it exists in the list.</summary>
    private void SelectBranch(string branchCode)
    {
        if (string.IsNullOrEmpty(branchCode)) return;

        ListItem item = BranchList.Items.FindByValue(branchCode);
        if (item != null)
        {
            BranchList.ClearSelection();
            item.Selected = true;
        }
    }
    public static void AddtoLogFile(dynamic Message, dynamic WebPage)
    {
        string LogPath = System.Web.HttpContext.Current.Server.MapPath("~/log/");// ConfigurationManager.AppSettings["LogPath"].ToString();
        string filename = "Log_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
        string filepath = LogPath + filename;
        if (File.Exists(filepath))
        {
            using (StreamWriter writer = new StreamWriter(filepath, true))
            {
                writer.WriteLine("——————-START————-" + DateTime.Now);
                writer.WriteLine("## :" + WebPage);
                writer.WriteLine(Message);
                writer.WriteLine("——————-END————-" + DateTime.Now);
            }
        }
        else
        {
            StreamWriter writer = File.CreateText(filepath);
            writer.WriteLine("——————-START————-" + DateTime.Now);
            writer.WriteLine("## :" + WebPage);
            writer.WriteLine(Message);
            writer.WriteLine("——————-END————-" + DateTime.Now);
            writer.Close();
        }
    }
    /// <summary>
    /// Returns the branch the person picked, or null if none.
    /// If the list came back empty on postback, it is rebuilt and the posted value re-applied.
    /// </summary>
    private ListItem GetSelectedBranch(string userName)
    {
        DropDownList ddl = BranchList;

        if (ddl.Items.Count <= 1)
        {
            string posted = Request.Form[ddl.UniqueID];
            BindBranches(userName);
            SelectBranch(posted);
        }

        ListItem item = ddl.SelectedItem;
        if (item == null || string.IsNullOrEmpty(item.Value))
            return null;

        return item;
    }

    // =====================================================================
    // Sign-in / messages / audit
    // =====================================================================
    private void SignIn(string userName, ListItem branch)
    {
        string logonId = string.Format("{0}?{1}", userName, branch.Value);
        string logonIdName = string.Format("{0}?{1}", userName, branch.Text);
        Response.Cookies["LogonUserID"].Value = logonId;
        Response.Cookies["LogonUserIDName"].Value = logonIdName;
        Audit("003", string.Format("User ID:{0} Location:-{1}, User Ebiz Logon", userName, branch.Text), userName);

        FormsAuthentication.SetAuthCookie(logonId, RememberBox != null && RememberBox.Checked);

        // Redirect without throwing ThreadAbortException (which was landing in the catch blocks)
        Response.Redirect("~/WebPages.aspx", false);
        Context.ApplicationInstance.CompleteRequest();
    }

    private void Fail(string message)
    {
        LoginUser.FailureText = message;
        Literal failure = (Literal)LoginUser.FindControl("FailureText");
        if (failure != null) failure.Text = HttpUtility.HtmlEncode(message);
    }

    private static void Audit(string code, string message, string userName)
    {
        try
        {
            Classes.RecordOp(string.Format("Sproc_AddAuditLog '{0}','{1}','{2}',''",
                Sql(code), Sql(message), Sql(userName)));
        }
        catch (Exception ex)
        {
            System.Diagnostics.Trace.WriteLine(ex.Message);
        }
    }

    /// <summary>Escapes single quotes for the string-built SQL calls.</summary>
    private static string Sql(string value)
    {
        return (value ?? "").Replace("'", "''");
    }

    // =====================================================================
    // Unchanged from the original
    // =====================================================================
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

                using (UserPrincipal up = new UserPrincipal(pc))
                {
                    up.SamAccountName = UserName;
                    if (!localhost)
                    {
                        up.GivenName = UserName;
                        up.UserPrincipalName = UserName;
                    }
                    up.DisplayName = "User";
                    up.Description = "User for bla!";
                    up.SetPassword(Pwd);
                    up.Enabled = true;
                    up.UserCannotChangePassword = true;
                    up.PasswordNeverExpires = true;
                    up.Save();
                }

                int max = 600;
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
}
