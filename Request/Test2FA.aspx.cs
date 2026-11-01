using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
 
using System.Data;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Web.Script.Serialization;
 
using System.Threading;
using WebRefVanzo;

public partial class Request_Test2FA : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack && !IsCallback)
        //if (IsCallback)
        {
        }
    }

    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {


    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        WebRefVanzo.TwoFactorService hardToken = new TwoFactorService();

        var resp = hardToken.verifyToken(txtuserid.Text, txtclientid.Text, txtserial.Text, txtToken.Text);
        var resp2 = hardToken.doResponseAuthentication(txtuserid.Text, txtclientid.Text, txtToken.Text);
        Response.Write(resp2.description);
        Response.Write(resp2.status);
        Response.Write(resp);
        //Operation Successful1WebRefVanzo.response
        //var resuls = hardToken.verifyToken("Itmb", "ITMB", "763332", psword.Text);
        Response.Write(hardToken.verifyToken(txtuserid.Text, txtclientid.Text, txtserial.Text, txtToken.Text));
    }
    protected void Button2_Click(object sender, EventArgs e)
    {

        //Operation Successful1WebRefVanzo.response
        //var resuls = hardToken.verifyToken("Itmb", "ITMB", "763332", psword.Text);

        Response.Write(txtMonth.Text.Substring(2));
    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        var resp2 = @"{""message"":""Could not link card"",""success"":false,""data"":{""message"":""connect ETIMEDOUT 172.25.15.11:80"",""name"":""Error"",""stack"":""Error: connect ETIMEDOUT 172.25.15.11:80\n    at TCPConnectWrap.afterConnect [as oncomplete] (net.js:1146:16)"",""config"":{""url"":""http://172.25.15.11/api/v1/scplite/cards/accounts"",""method"":""post"",""data"":""{\""authData\"":\""CeisSulZQ727duoYojfAcd08o6fwhoJ/Ief0pFlMcqWA2leYttEnkSdU3Qx6CeC1dQ8rF6URcNPWDKqXHHAUrxIp8A57j9kbGohVFEW9DkHqjMrBrEdbcqo0SROurWWf25dRkcr2XDXTTOOkvx9VZl5YIQ2tfvJri+TslUDK8X4B6L2kaxeUyMipOH3fOtop9jIw/5fnrg/7cQoaWFMisrkWjr/CIjRZjIW6OTpTpOsE5FtR556zAP2EsVShvdZSydpXv0alkRdgvpTy+ihhujxDdUb8MsIS/DUcaEu4dDpPwJ8pnlvkKXhyxLwD0j5705Hfa+zQUYfNUAYLoxf0Bw==\"",\""accountNo\"":\""0000187111\"",\""accountType\"":\""20\"",\""accountTypeQualifier\"":\""1\"",\""accountCurrency\"":\""566\"",\""sequenceNo\"":\""001\""}"",""headers"":{""Accept"":""application/json, text/plain, */*"",""Content-Type"":""application/json; charset=UTF-8"",""Nonce"":""e896e5e2b6a4acbf0cc7362f8c03ca97"",""Authorization"":""InterswitchAuth SUtJQTEyOUVENTUxRENFMTlCM0ZEMTJBOTQzRTQ0ODc1OTJCRTk0NTQ0M0E="",""Timestamp"":1651921721,""Signature"":""1KVgou+5eOSlSdQi3pJxATtavmg="",""BankCode"":""72123"",""authData"":""CeisSulZQ727duoYojfAcd08o6fwhoJ/Ief0pFlMcqWA2leYttEnkSdU3Qx6CeC1dQ8rF6URcNPWDKqXHHAUrxIp8A57j9kbGohVFEW9DkHqjMrBrEdbcqo0SROurWWf25dRkcr2XDXTTOOkvx9VZl5YIQ2tfvJri+TslUDK8X4B6L2kaxeUyMipOH3fOtop9jIw/5fnrg/7cQoaWFMisrkWjr/CIjRZjIW6OTpTpOsE5FtR556zAP2EsVShvdZSydpXv0alkRdgvpTy+ihhujxDdUb8MsIS/DUcaEu4dDpPwJ8pnlvkKXhyxLwD0j5705Hfa+zQUYfNUAYLoxf0Bw=="",""IssuerNr"":""72"",""UserID"":""olatoye.omilabu"",""SignatureMethod"":""SHA1"",""User-Agent"":""axios/0.21.4"",""Content-Length"":473},""transformRequest"":[null],""transformResponse"":[null],""timeout"":0,""xsrfCookieName"":""XSRF-TOKEN"",""xsrfHeaderName"":""X-XSRF-TOKEN"",""maxContentLength"":-1,""maxBodyLength"":-1,""transitional"":{""silentJSONParsing"":true,""forcedJSONParsing"":true,""clarifyTimeoutError"":false}},""code"":""ETIMEDOUT""}}";

        JavaScriptSerializer jss = new JavaScriptSerializer();
        dynamic json = jss.DeserializeObject(resp2);
        string strResp = "";
        strResp = json["success"].ToString();
        if (bool.Parse(strResp) == true)
        {
            Response.Write("informations" + strResp);
        }
        else
        {
            Response.Write("informations" + strResp);

        }


    }



}