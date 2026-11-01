using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net.Mail;
using System.Text;

public partial class eOperations_MultipleImages : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }


}
public static class RegexExtensions
{
    const string _directoryName = "";
    public static string GetPattern(this IEnumerable<string> valuesToSearch)
    {
        return string.Format("({0})", string.Join("|", valuesToSearch));
    }
    //private string[] GetFullNamesOfImages()
    //{
    //    string images = Path.Combine(_directoryName, "Images");
    //    if (!Directory.Exists(images))
//    ----------------
    //    return Directory.GetFiles(images);
    //}

  
    //private string InsertImages(string body)
    //{
    //    var images = GetFullNamesOfImages().Select(Path.GetFileName).ToArray();
    //    return Regex.Replace(body, "(Images/)?" + images.GetPattern(), "cid:$2", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    //}

    //private string[] GetFullNamesOfImages()
    //{
    //    string images = Path.Combine(_directoryName, "Images");
    //    if (!Directory.Exists(images))
    //        return new string[0];
    //    return Directory.GetFiles(images);
    //}

    //private MailMessage CreateMail(SmtpClient smtp, string toAddress, string body)
    //{
    //    var images = GetFullNamesOfImages();

    //    string decodedBody = WebUtility.HtmlDecode(body);
    //    var text = AlternateView.CreateAlternateViewFromString(decodedBody, null, MediaTypeNames.Text.Plain);
    //    var html = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);
    //    foreach (var image in images)
    //    {
    //        html.LinkedResources.Add(new LinkedResource(image, new ContentType("image/png"))
    //        {
    //            ContentId = Path.GetFileName(image)
    //        });
    //    }


    //    var credentials = (NetworkCredential)smtp.Credentials;

    //    var message = new MailMessage(new MailAddress(credentials.UserName), new MailAddress(toAddress))
    //    {
    //        Subject = "Some subj",
    //        Body = decodedBody
    //    };
    //    message.AlternateViews.Add(text);
    //    message.AlternateViews.Add(html);
    //    return message;
    //}
}
