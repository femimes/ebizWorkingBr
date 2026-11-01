using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
 
using System.IO;
using System.Collections.Generic;
using System.Text;
 
using iTextSharp.text;
using iTextSharp.text.pdf;

using iTextSharp.text.html.simpleparser;
 

public partial class eOperations_DemoChecking : System.Web.UI.Page
{public readonly byte[] OWNER = ASCIIEncoding.UTF8.GetBytes("World");
     public readonly byte[] USER = ASCIIEncoding.UTF8.GetBytes("Hello");
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void ASPxButton1_Click(object sender, EventArgs e)
    {

        const string filenameSource = "Statement.pdf";
        const string filenameDest = "HelloWorld_tempfile.pdf";
        using (MemoryStream ms = new MemoryStream())
        {
            // step 1
            Document document = new Document();

            PdfWriter writer = PdfWriter.GetInstance(document, ms);
            writer.SetEncryption(
              USER, OWNER,
              PdfWriter.ALLOW_PRINTING,
              PdfWriter.STANDARD_ENCRYPTION_128
            );
            writer.CreateXmpMetadata();
            // step 3
            document.Open();
            // step 4
            document.Add(new Paragraph("Hello World"));
        }
    }

    public byte[] CreatePdf()
    {
        using (MemoryStream ms = new MemoryStream())
        {
            // step 1
            Document document = new Document();
            
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                writer.SetEncryption(
                  USER, OWNER,
                  PdfWriter.ALLOW_PRINTING,
                  PdfWriter.STANDARD_ENCRYPTION_128
                );
                writer.CreateXmpMetadata();
                // step 3
                document.Open();
                // step 4
                document.Add(new Paragraph("Hello World"));
            
            return ms.ToArray();
        }
    }

    
}