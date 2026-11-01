using System;
using System.Web;
using System.Web.UI;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        bool signedIn = User != null && User.Identity != null && User.Identity.IsAuthenticated;
        lnkSignIn.Visible = !signedIn;
        if (signedIn)
        {
            // identity is "user?branch"
            string name = User.Identity.Name ?? "";
            int split = name.LastIndexOf('?');
            if (split >= 0) name = name.Substring(0, split);
            if (name != "") litGreeting.Text = "Welcome back, " + HttpUtility.HtmlEncode(name);
        }
    }
}
