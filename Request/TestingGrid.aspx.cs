using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;

public partial class Request_TestingGrid :  System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsCallback && !IsPostBack)
        {

        }
      }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string strcity = "";
        GridViewDataColumn columnCity = (GridViewDataColumn)ctrlGrid.Columns["CardTypeDesc"];
        for (int rowIndex = 0; rowIndex < ctrlGrid.VisibleRowCount; rowIndex++)
        {
            TextBox aspTxtCity = (TextBox)ctrlGrid.FindRowCellTemplateControl(rowIndex, columnCity, "TextBox1");
            strcity = aspTxtCity.Text;
        }
    }
}