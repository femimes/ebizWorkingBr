using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NETClass;

public partial class eOperations_BulkEMailTemp : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack && !IsCallback)
        {

        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected async void Button1_Click(object sender, EventArgs e)
    {
     
          
        string EmailProvider = System.Configuration.ConfigurationManager.ConnectionStrings["EmailProvider"].ToString();
        if (EmailProvider == "SilverSands")
        {
            var apiKey = System.Configuration.ConfigurationManager.ConnectionStrings["EmailApiKey"].ToString(); ;// "Environment.GetEnvironmentVariable("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");
            string StrCompanyBrand = System.Configuration.ConfigurationManager.ConnectionStrings["CompanyBrand"].ToString();
            string EmailserverUsr = System.Configuration.ConfigurationManager.ConnectionStrings["EmailUserName"].ToString();
            string EmailserverPwd = System.Configuration.ConfigurationManager.ConnectionStrings["EmailPwds"].ToString();
            string EmailServerPort = System.Configuration.ConfigurationManager.ConnectionStrings["EazyEmailServerPort"].ToString();
            string EmailServer = System.Configuration.ConfigurationManager.ConnectionStrings["EazyEmailServer"].ConnectionString;

            var plainTextContent = txtMessage.Text;// string.Format("<html><body>Dear Valued Customer, </br>Kindly be informed that we are carrying out maintenance on the Jubilee-Life ATM Card and USSD Code (*723*321#), we apologize for the inconveniences. We will communicate to you as soon as these services are available for your use.In the meantime, kindly make use of our alternative channels; Mobile Application (Jubilation App) and Internet Banking for your transactions or contact our E-banking team via email, EBANKINGSUPPORT@jubileelifeng.com or call 0700 2000 500..</br></br>We thank you for your understanding. /></body></html>");
            var htmlContent = string.Format("<html><body>Dear Valued Customer, </br>Kindly be informed that we are carrying out maintenance on the Jubilee-Life ATM Card and USSD Code (*723*321#), we apologize for the inconveniences. We will communicate to you as soon as these services are available for your use.In the meantime, kindly make use of our alternative channels; Mobile Application (Jubilation App) and Internet Banking for your transactions or contact our E-banking team via email, EBANKINGSUPPORT@jubileelifeng.com or call 0700 2000 500..</br></br>We thank you for your understanding.</body></html>");
            foreach (DataRow item in NETClass.Classes.AXGetDataset("SELECT * FROM EmailBroadcastTemplate WHERE sno > 4041 ").Tables[0].Rows)
            {

          
            string to = item["EmailAddr"].ToString();// new EmailAddress(toAddress, toAddress);
            string straltInfo = htmlContent;// string.Format("{0} Content View don't support html", emailmsg);
            var subject = txtSubject.Text;
            string strRespMsg = string.Format("{0}; To:{1}", subject, to.ToString());
                string file = "";
            string from = txtFrom.Text;// string.Format("{0}", new EmailAddress(strSenderAddress, StrCompanyBrand));
            AddtoLogFile(to, string.Format("Email OpsResponse {0}", "Before Msg generating"));
                try
                {
                    var statusCode =  await   SendEmailExec(from, EmailserverUsr, EmailserverPwd, StrCompanyBrand, straltInfo, to, subject, straltInfo, file);
              
                }
                catch (Exception ex)
                {

                    throw;
                }
                }

            Response.Write("Done !!!");

        }
    }
    public async Task<string> SendEmailExec(string from, string username, string password, string displayname, string text, string to, string subject, string htmlMsg, string file)
    {
        string url = "https://api.silversands.com.ng/email/1/send";
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

        string ApiKey = Convert.ToBase64String(Encoding.Default.GetBytes(string.Format("{0}:{1}", username, password)));
        //string authorizationHeader = "bGl2aW5ndHJ1c3QtZW1haWw6T21vbHVhYmlAMjAyMw==";
        from = string.Format("{0} <{1}>", displayname, from);


        //string lpaath = Server.MapPath("~/Images/");


        var content = new MultipartFormDataContent("------------" + Guid.NewGuid());

        //var byteArrayContent = new ByteArrayContent(byteDataAd);

        var clientsp = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "https://api.silversands.com.ng/email/3/send");
        request.Headers.Add("Authorization", "Basic " + ApiKey);
        var contentReq = new MultipartFormDataContent("------------" + Guid.NewGuid());
        contentReq.Add(new StringContent(from), "from");
        contentReq.Add(new StringContent(to), "to");
        contentReq.Add(new StringContent(subject), "subject");

        contentReq.Add(new StringContent(htmlMsg), "html");
        //content.Add(new StreamContent(File.OpenRead(file)), "inlineImage", file);

        request.Content = contentReq;
        var responsesp = await clientsp.SendAsync(request);
        responsesp.EnsureSuccessStatusCode();
        var msg = await responsesp.Content.ReadAsStringAsync();
        return msg;



    }


    public void AddtoLogFile(string Message, string WebPage)
    {
        string LogPath = Server.MapPath("~/log/");// ConfigurationManager.AppSettings["LogPath"].ToString();
        string filename = "Log_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
        string filepath = LogPath + filename;
        if (File.Exists(filepath))
        {
            using (StreamWriter writer = new StreamWriter(filepath, true))
            {
                writer.WriteLine("——————-START————-" + DateTime.Now);
                writer.WriteLine("EmailLog :" + WebPage);
                writer.WriteLine(Message);
                writer.WriteLine("——————-END————-" + DateTime.Now);
            }
        }
        else
        {
            StreamWriter writer = File.CreateText(filepath);
            writer.WriteLine("——————-START————-" + DateTime.Now);
            writer.WriteLine("EmailLog :" + WebPage);
            writer.WriteLine(Message);
            writer.WriteLine("——————-END————-" + DateTime.Now);
            writer.Close();
        }
    }

}