using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.XtraExport;
//using TextSharp.text.html.simpleparser;
//using iTextSharp.text.pdf;
using System.IO;
using System.Net.Mail;
using System.Text;
using PdfSharp;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Security;
 
public partial class eOperations_DemoDevXpress : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void ASPxButton1_Click(object sender, EventArgs e)
    {
        //PdfSharp.Pdf.PdfDocument dgd = 
        // Get a fresh copy of the sample PDF file
        const string filenameSource = "Statement.pdf";
        const string filenameDest = "HelloWorld_tempfile.pdf";
        //File.Copy(Path.Combine("../../../../../PDFs/", filenameSource),
         //Path.Combine(Directory.GetCurrentDirectory(), filenameDest), true);
            File.Copy(Path.Combine("C://Web Development//ExpCardManager//Reports//", filenameSource),
          Path.Combine("C://Web Development//ExpCardManager//Approvals//", filenameDest), true);
         
        // Open an existing document. Providing an unrequired password is ignored.
        //PdfDocument document = PdfReader.Open(filenameDest, "some text");
        PdfDocument document = PdfReader.Open("C://Web Development//ExpCardManager//Approvals//" + filenameDest, "some text");
        PdfSecuritySettings securitySettings = document.SecuritySettings;
       
        // Setting one of the passwords automatically sets the security level to 
        // PdfDocumentSecurityLevel.Encrypted128Bit.
        securitySettings.UserPassword = "user";
        securitySettings.OwnerPassword = "owner";
     
        // Don't use 40 bit encryption unless needed for compatibility
        //securitySettings.DocumentSecurityLevel = PdfDocumentSecurityLevel.Encrypted40Bit;
      
        // Restrict some rights.
        securitySettings.PermitAccessibilityExtractContent = false;
        securitySettings.PermitAnnotations = false;
        securitySettings.PermitAssembleDocument = false;
        securitySettings.PermitExtractContent = false;
        securitySettings.PermitFormsFill = true;
        securitySettings.PermitFullQualityPrint = false;
        securitySettings.PermitModifyDocument = true;
        securitySettings.PermitPrint = false;
   
        // Save the document...
        document.Save("C://Web Development//ExpCardManager//Approvals//" + filenameDest);
        // ...and start a viewer.
       // Process.Start(filenameDest);

    }


}