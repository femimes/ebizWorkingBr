using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>Test page: raw request rows of a card batch (SpCardRequestMaint). Plain ASP.NET controls (no DevExpress).</summary>
public partial class Request_demo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string id = User != null && User.Identity != null ? (User.Identity.Name ?? "") : "";
            int split = id.LastIndexOf('?');
            txtUserID.Text = split >= 0 ? id.Substring(0, split) : id;
            txtlocation.Text = split >= 0 ? id.Substring(split + 1) : "";
        }
    }

    /// <summary>Nothing to list until a batch is chosen.</summary>
    protected void SqlDbReqDetails_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        if (drpBatchRefNo.SelectedValue == "") e.Cancel = true;
    }
}
