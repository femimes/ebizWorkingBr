using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SiteMaster : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        litYear.Text = DateTime.Now.Year.ToString();
        MarkCurrentMenuItem();
        ShowSignedInUser();
    }

    /// <summary>Highlights the menu item of the page being shown.</summary>
    private void MarkCurrentMenuItem()
    {
        string current = Request.AppRelativeCurrentExecutionFilePath ?? "";
        foreach (MenuItem item in NavigationMenu.Items)
        {
            if (string.Equals(item.NavigateUrl, current, StringComparison.OrdinalIgnoreCase)) item.Selected = true;
        }
    }

    /// <summary>Shows the signed-in officer's name (the identity is "user?branch").</summary>
    private void ShowSignedInUser()
    {
        if (Page.User == null || Page.User.Identity == null || !Page.User.Identity.IsAuthenticated) return;
        string name = Page.User.Identity.Name ?? "";
        int split = name.LastIndexOf('?');
        if (split >= 0) name = name.Substring(0, split);

        Literal user = HeadLoginView.FindControl("litUser") as Literal;
        Literal initial = HeadLoginView.FindControl("litInitial") as Literal;
        if (user != null) user.Text = HttpUtility.HtmlEncode(name);
        if (initial != null) initial.Text = name == "" ? "?" : HttpUtility.HtmlEncode(name.Substring(0, 1));
    }
}
