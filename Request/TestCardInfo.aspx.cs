using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebRefVanzo;


public partial class SCPLite_TestCardInfo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack && !IsCallback)
        {

        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {


        //ICardProcess Cardpro = new ICardProcess();
        //string id = "54645622"; // max digit
        //Random rnd = new Random();

        //string id = rnd.Next(54645622).ToString();
        ////string id = Guid.NewGuid().ToString("N");
        //Guid.NewGuid().ToString("N");
        //string result = Token.PayCodeToken(id, "ITB", "ATM", txtAmount.Text, "1234", "ITB", txtAccountNo.Text, true,
        //   "3455553", "ITB", txtPhoneNo.Text);
        //lblTOken.Text = Token.PayCodeToken(id, "ITB", "ATM", txtAmount.Text, "1234", "ITB",txtAccountNo.Text,true,
        //    "3455553", "ITB",txtPhoneNo.Text);
        //version = "001";
        //pan = "5198990000000532";
        //pin = "0000";
        //expirydate = "2201";
        //cvv2 = "145";

        
        // lblResp.Text = Cardpro..(id, "ITB", "ATM", txtAmount.Text, "1234", "ITB", txtAccountNo.Text,true,
        //"3455553", "ITB", txtPhoneNo.Text);
        //     JavaScriptSerializer jssp = new JavaScriptSerializer();
        //dynamic jsonp = jssp.DeserializeObject(result);

        WebRefVanzo.TwoFactorService hardToken = new TwoFactorService();

        //var resp = hardToken.verifyToken(txtuserid.Text, txtclientid.Text, txtserial.Text, txtToken.Text);
        var resp2 = hardToken.doResponseAuthentication("Itmb", "ITMB", txtToken.Text);

        if (resp2.status.ToString() == "1")
        {
            //lblResp.Text = ICardProcess.Blockcards(txtSeqno.Text, txtPanno.Text, txtpin.Text,txtExpdate.Text, txtCvv.Text);

        }
        else
        {
            lblResp.Text = "Operation Failed";
        }
        //Response.Write(resp2.description);
        //Response.Write(resp2.status);
        //Response.Write(resp);
        //Operation Successful1WebRefVanzo.response
        //var resuls = hardToken.verifyToken("Itmb", "ITMB", "763332", psword.Text);
        //Response.Write(hardToken.verifyToken(txtuserid.Text, txtclientid.Text, txtserial.Text, txtToken.Text));


    }

    protected void btnUnblock_Click(object sender, EventArgs e)
    {
        //lblResp.Text = ICardProcess.UnBlockcards(txtSeqno.Text, txtPanno.Text, txtpin.Text, txtExpdate.Text, txtCvv.Text);
    }

    protected void btnCardlink_Click(object sender, EventArgs e)
    {
        //lblResp.Text = ICardProcess.Linkcards(txtSeqno.Text, txtPanno.Text, txtpin.Text, txtExpdate.Text, txtCvv.Text);
    }
  protected void btnCardunlink_Click(object sender, EventArgs e)
    {
        //lblResp.Text = ICardProcess.UnLinkcards(txtSeqno.Text, txtPanno.Text, txtpin.Text, txtExpdate.Text, txtCvv.Text);
    }
    protected void btnPinReissue_Click(object sender, EventArgs e)
    {
        //lblResp.Text = ICardProcess.PinReIssuance(txtSeqno.Text, txtPanno.Text, txtpin.Text, txtExpdate.Text, txtCvv.Text);
    }

    protected void btnGetCardInfo_Click(object sender, EventArgs e)
    {
        //lblResp.Text = ICardProcess.ExistCardInfo(txtSeqno.Text, txtPanno.Text, txtpin.Text, txtExpdate.Text, txtCvv.Text);
    }
}