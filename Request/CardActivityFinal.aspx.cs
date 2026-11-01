using System;
using System.Web.UI;

/// <summary>
/// Card activity (final). The original page had a grid whose data source had no query, so it never showed data;
/// this version keeps that behaviour with a plain GridView (no DevExpress) and says so on the page.
/// </summary>
public partial class Request_CardActivityFinal : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ASPxGridView1.DataSource = new object[0];
            ASPxGridView1.DataBind();
        }
    }
}
