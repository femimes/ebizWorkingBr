using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Script.Serialization;
public partial class AlphaBeta_ABCGenReceipt : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack && !IsCallback)
        {
            InitParameters();

        }
    }

    void InitParameters()
    {
        string strRespCode = ""; string strRespDesc = ""; string strWebGuid = "";
        //Transaction Payment Reference:

        lBlPayRefNo.Text = string.Format("Payment Reference: {0}", Request.QueryString["RefNo"]);

        //string strResults = iBankPoints.iPoints.ProcessAphaBetaPayRefReceipts("10", Request.QueryString["RefNo"]);

        //JavaScriptSerializer jss = new JavaScriptSerializer();
        //dynamic json = jss.DeserializeObject(strResults);
        //strRespCode = json["Status"];
        //strRespDesc = json["StatusMessage"];
        string strurl = "";

        //if (strRespCode == "SUCCESS")
        //{
        strurl = Request.Cookies["ReceiptNumber"].Value;
        hypReceipt.NavigateUrl = strurl;
        hypReceipt.Target = "_new";
        //    //Response.Write(strurl);
        //}
    }
    
}