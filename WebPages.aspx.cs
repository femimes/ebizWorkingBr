using System;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using NETClass;

public partial class WebPages : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack || IsCallback) return;

        try
        {
            LoadSubMenus();

            string strselPwds = string.Format("SELECT * FROM AppUSers WHERE password ='xxx' AND user_id = '{0}'", "AXadmin");

            if (NETClass.Classes.GetDataset(strselPwds).Tables[0].Rows.Count > 0)
            {
                Classes.alertwindow(this.Page, "Please Change Password!", "Halt");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Trace.WriteLine(ex.ToString());
            litMenu.Text = "<p class=\"nav-error\">The menu could not be loaded. Please try again or contact support.</p>";
        }
    }

    // Reads the signed-in user from the LogonUserID cookie ("user?branch"),
    // falling back to the forms-authentication identity.
    private string GetLogonUser()
    {
        HttpCookie cookie = Request.Cookies["LogonUserID"];
        string raw = (cookie != null && !string.IsNullOrEmpty(cookie.Value))
            ? cookie.Value
            : HttpContext.Current.User.Identity.Name;

        if (string.IsNullOrEmpty(raw)) return null;

        int i = raw.LastIndexOf('?');
        return i > 0 ? raw.Substring(0, i) : raw;
    }

    // Builds the sidebar from the same two stored procedures as before.
    private void LoadSubMenus()
    {
        string user = GetLogonUser();
        if (string.IsNullOrEmpty(user)) return;

        // The cookie is controlled by the browser, so escape quotes before it goes into SQL.
        string safeUser = user.Replace("'", "''");

        DataTable modules = Classes.SqlDbGetDataset(
            string.Format("Sproc_GetMenuModuleItems '{0}'", safeUser)).Tables[0];

        StringBuilder sb = new StringBuilder();

        foreach (DataRow module in modules.Rows)
        {
            string moduleId = module["obj_module"].ToString().Replace("'", "''");

            DataTable items = Classes.SqlDbGetDataset(
                string.Format("Sproc_GetMenuItems '{0}','{1}'", safeUser, moduleId)).Tables[0];

            if (items.Rows.Count == 0) continue;   // no empty groups in the sidebar

            sb.Append("<details class=\"nav-group\"><summary>")
              .Append(HttpUtility.HtmlEncode(module["obj_module_desc"].ToString()))
              .Append("</summary>");

            foreach (DataRow item in items.Rows)
            {
                string url = item["obj_folder"].ToString() + item["obj_aspx_path"].ToString();
                if (url.StartsWith("~")) url = ResolveClientUrl(url);

                sb.Append("<a class=\"nav-link\" target=\"contentUrlPane\" data-id=\"")
                  .Append(HttpUtility.HtmlAttributeEncode(item["obj_no"].ToString()))
                  .Append("\" href=\"")
                  .Append(HttpUtility.HtmlAttributeEncode(url))
                  .Append("\">")
                  .Append(HttpUtility.HtmlEncode(item["obj_desc"].ToString()))
                  .Append("</a>");
            }

            sb.Append("</details>");
        }

        litMenu.Text = sb.ToString();
    }
}
