using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>Test page: reads back text boxes inside grid rows. Plain ASP.NET controls (no DevExpress).</summary>
public partial class Request_TestingGrid : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        List<string> values = new List<string>();
        foreach (GridViewRow row in ctrlGrid.Rows)
        {
            TextBox tb = row.FindControl("TextBox1") as TextBox;
            if (tb != null) values.Add(tb.Text);
        }
        litResult.Text = HttpUtility.HtmlEncode(string.Format("Read {0} row(s): {1}", values.Count, string.Join(", ", values.ToArray())));
    }
}
