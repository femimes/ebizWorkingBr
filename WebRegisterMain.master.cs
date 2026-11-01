using System;
using System.Web;
using System.Web.UI.WebControls;

public partial class WebRegisterMain : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;

        // LoginName only exists in the signed-in template; nothing to do for visitors.
        LoginName headLoginName = HeadLoginView.FindControl("HeadLoginName") as LoginName;
        if (headLoginName == null) return;

        HttpCookie cookie = Request.Cookies["LogonUserID"];
        string raw = (cookie != null && !string.IsNullOrEmpty(cookie.Value))
            ? cookie.Value
            : HttpContext.Current.User.Identity.Name;

        // The cookie holds "user?branch"; the header shows just the user.
        int i = raw.LastIndexOf('?');
        string display = i > 0 ? raw.Substring(0, i) : raw;

        // FormatString goes through string.Format, so escape braces.
        headLoginName.FormatString = HttpUtility.HtmlEncode(display).Replace("{", "{{").Replace("}", "}}");
    }
}
