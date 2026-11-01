using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using NETClass;
using System.IO;
using System.Text;
using System.Net;
using System.Security.Cryptography;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using System.Web.Services.Protocols;
using System.Xml.Serialization;
using System.Security;
using System.Web.Script.Serialization;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using Microsoft.VisualBasic;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using ListItem = System.Web.UI.WebControls.ListItem;
using System.Globalization;
using System.Web.UI;

/// <summary>
/// Account statements by e-mail: e-mails password-protected PDF statements (AccountEstatementNew.rdlc) to the selected accounts.
/// Accounts: GetAllAccountCardRequest (product, branch, optional account list). Plain ASP.NET controls (no DevExpress);
/// the statement rendering and e-mail sending code is the original page's.
/// </summary>
public partial class eOperations_FrmBulkEmails : System.Web.UI.Page
{
    private const string DefaultBranch = "101";
    private const string DefaultProduct = "103";
    private const string ReportFile = "~/Reports/AccountEstatementNew.rdlc";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadFilters();
            drpStatementtype.SelectedValue = "N";
            IssueDate = DateTime.Today;
            ExpiryDate = DateTime.Today.AddDays(1.0);
            BindGrid();
        }
    }

    // =====================================================================
    // Statement period (date boxes)
    // =====================================================================

    private DateTime IssueDate
    {
        get { DateTime d; return TryDate(dateIssue.Text, out d) ? d : DateTime.Today; }
        set { dateIssue.Text = DateInput(value); }
    }

    private DateTime ExpiryDate
    {
        get { DateTime d; return TryDate(dateexpiry.Text, out d) ? d : DateTime.Today; }
        set { dateexpiry.Text = DateInput(value); }
    }

    protected void drpStatementtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        dateexpiry.ReadOnly = false;
        changeDates();
    }

    protected void dateIssue_DateChanged(object sender, EventArgs e)
    {
        changeDates();
    }

    // =====================================================================
    // Accounts
    // =====================================================================

    private void LoadFilters()
    {
        try
        {
            foreach (DataRow r in QueryOn(MainDb, "Sproc_GetAllProductsCards", CommandType.StoredProcedure).Rows)
            {
                string code = Col(r, "Product Code"), name = Col(r, "product"), type = Col(r, "Type");
                if (code == "" || drp_products.Items.FindByValue(code) != null) continue;
                drp_products.Items.Add(new ListItem((name == "" ? code : name) + (type == "" ? "" : " (" + type + ")"), code));
            }
            foreach (DataRow r in QueryOn(MainDb, "Sproc_GetAllBranchesCards", CommandType.StoredProcedure).Rows)
            {
                string code = Col(r, "Headsub_code"), name = Col(r, "sub_name");
                if (code == "" || drpbranches.Items.FindByValue(code) != null) continue;
                drpbranches.Items.Add(new ListItem(name == "" ? code : name + " (" + code + ")", code));
            }
        }
        catch (Exception ex)
        {
            ShowError("The products or branches could not be loaded: " + ex.Message);
        }
        Pick(drp_products, DefaultProduct);
        Pick(drpbranches, DefaultBranch);
    }

    private static void Pick(DropDownList list, string value)
    {
        if (list.Items.FindByValue(value) == null) list.Items.Insert(0, new ListItem(value, value));
        list.ClearSelection();
        list.Items.FindByValue(value).Selected = true;
    }

    private DataTable LoadAccounts()
    {
        DataTable t = Query("EXEC GetAllAccountCardRequest @product, @branch", P("@product", drp_products.SelectedValue), P("@branch", drpbranches.SelectedValue));
        List<string> wanted = AccountList(txtSpecificAcct.Text);
        if (wanted.Count > 0 && t.Columns.Contains("AccountNo"))
        {
            HashSet<string> set = new HashSet<string>(wanted, StringComparer.OrdinalIgnoreCase);
            DataTable filtered = t.Clone();
            foreach (DataRow r in t.Rows) if (set.Contains(Col(r, "AccountNo"))) filtered.ImportRow(r);
            t = filtered;
        }
        foreach (string column in new[] { "AccountNo", "AccountName", "Product", "DOB", "address", "city", "Book Bal", "Cleared Bal", "Trans Status", "Account Status" })
            if (!t.Columns.Contains(column)) t.Columns.Add(column, typeof(string));
        return t;
    }

    private void BindGrid()
    {
        try
        {
            DataTable t = LoadAccounts();
            ctrlGrid.DataSource = t;
            ctrlGrid.DataBind();
            litTotal.Text = string.Format("{0:N0} account{1}", t.Rows.Count, t.Rows.Count == 1 ? "" : "s");
        }
        catch (Exception ex)
        {
            ctrlGrid.DataSource = null;
            ctrlGrid.DataBind();
            litTotal.Text = "";
            ShowError("The accounts could not be loaded: " + ex.Message);
        }
    }

    protected void Filter_Changed(object sender, EventArgs e)
    {
        ctrlGrid.PageIndex = 0;
        litResult.Text = "";
        litError.Text = "";
        BindGrid();
    }

    protected void ctrlGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ctrlGrid.PageIndex = e.NewPageIndex;
        BindGrid();
    }

    /// <summary>Ticked accounts on this page, or every listed account when "all pages" is ticked.</summary>
    private List<string> SelectedAccounts()
    {
        List<string> accounts = new List<string>();
        if (chkAllRecords.Checked)
        {
            foreach (DataRow r in LoadAccounts().Rows)
            {
                string acct = Col(r, "AccountNo");
                if (acct != "" && !accounts.Contains(acct)) accounts.Add(acct);
            }
            return accounts;
        }
        foreach (GridViewRow row in ctrlGrid.Rows)
        {
            CheckBox chk = row.FindControl("chkSelect") as CheckBox;
            if (row.RowType == DataControlRowType.DataRow && chk != null && chk.Checked)
                accounts.Add(Convert.ToString(ctrlGrid.DataKeys[row.RowIndex].Value));
        }
        return accounts;
    }

    protected void Button7_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Path, false);
        Context.ApplicationInstance.CompleteRequest();
    }

    // =====================================================================
    // Send statements
    // =====================================================================

    /// <summary>The customer's e-mail address in core banking ("" when none).</summary>
    private static string AccountEmail(string accountNo)
    {
        DataTable t = QueryOn(ConnectionString("EazyConnectSource"), "EXEC Sproc_GetAccountEmail @acct", CommandType.Text, P("@acct", accountNo));
        return t.Rows.Count == 0 || t.Rows[0].IsNull(0) ? "" : Convert.ToString(t.Rows[0][0]).Replace("'", "").Trim();
    }

    protected void Button6_Click(object sender, EventArgs e)
    {
        Page.Validate("send");
        if (!Page.IsValid) return;
        if (ExpiryDate < IssueDate) { ShowError("The end date is before the start date."); return; }

        List<string> accounts = SelectedAccounts();
        if (accounts.Count == 0) { ShowError("Tick at least one account, or tick \"all pages\"."); return; }

        string emailServer = ConnectionString("EazyEmailServer");
        string reportPath = Server.MapPath(ReportFile);
        string reportFolder = Server.MapPath("~/Reports/");
        int sent = 0;
        List<string> noEmail = new List<string>();
        List<string> failed = new List<string>();

        foreach (string accountNo in accounts)
        {
            try
            {
                string to = AccountEmail(accountNo);
                if (!ValidateEmail(to)) { noEmail.Add(accountNo); continue; }
                CreateMessageWithAttachmentByAcctRepSend(emailServer, to, accountNo, reportPath, reportFolder);
                sent++;
            }
            catch (Exception ex)
            {
                failed.Add(accountNo + ": " + ex.Message);
            }
        }

        ShowSuccess(string.Format("Statements sent for {0} of {1} account{2}.{3}", sent, accounts.Count, accounts.Count == 1 ? "" : "s",
            noEmail.Count == 0 ? "" : string.Format(" {0} skipped because they have no valid e-mail address.", noEmail.Count)));
        if (failed.Count > 0)
            litError.Text = HttpUtility.HtmlEncode(string.Format("{0} statement(s) failed: {1}", failed.Count, string.Join("; ", failed.ToArray())));
    }

    // ---- BEGIN unchanged sending code (carried over from the original page) ----
    public Boolean ValidateEmail(string strCheck)
    {
        try
        {
            var vEmailAddress = new System.Net.Mail.MailAddress(strCheck);

        }
        catch (Exception ex)
        {
            return false;
        }

        return true;
    }

    void changeDates()
    {
        string strTitleSubject = "Monthly";
        if (drpStatementtype.SelectedValue == "M")
        {

            ExpiryDate = IssueDate.AddMonths(1).AddDays(-1);
            dateexpiry.ReadOnly = true;
            strTitleSubject = String.Format("Monthly Statement Of Account For {0} {1}", MonthName(IssueDate.Month), ExpiryDate.Year);

        }
        if (drpStatementtype.SelectedValue == "N")
        {

            ExpiryDate = IssueDate.AddMonths(1);
            dateexpiry.ReadOnly = true;
            strTitleSubject = String.Format(" Statement Of Account ", MonthName(IssueDate.Month), ExpiryDate.Year);

        }
        if (drpStatementtype.SelectedValue == "W")
        {

            ExpiryDate = IssueDate.AddDays(7);
            dateexpiry.ReadOnly = true;
            strTitleSubject = String.Format("Weekly Statement Of Account From {2} To {3}", MonthName(IssueDate.Month), ExpiryDate.Year, IssueDate, ExpiryDate);

        }
        if (drpStatementtype.SelectedValue == "Q")
        {

            ExpiryDate = IssueDate.AddMonths(3).AddDays(-1);
            dateexpiry.ReadOnly = true;
            strTitleSubject = String.Format("Quartely Statement Of Account From {2} To {3}", MonthName(IssueDate.Month), ExpiryDate.Year, IssueDate, ExpiryDate);

        }
        if (drpStatementtype.SelectedValue == "Y")
        {

            ExpiryDate = IssueDate.AddYears(1).AddDays(-1);
            dateexpiry.ReadOnly = true;
            strTitleSubject = String.Format("Yearly Statement Of Account From {2} To {3}", MonthName(IssueDate.Month), ExpiryDate.Year, IssueDate, ExpiryDate);

        }

        if (drpStatementtype.SelectedValue == "D")
        {

            ExpiryDate = IssueDate.AddDays(1);
            dateexpiry.ReadOnly = true;
            String.Format("Daily Statement Of Account From {2} To {3}", MonthName(IssueDate.Month), IssueDate.Year, ExpiryDate, ExpiryDate);

        }

    }

    static string MonthName(int month)
    {
        DateTime date = new DateTime(2020, month, 1);

        return date.ToString("MMMM");
    }

    async void CreateMessageWithAttachmentByAcctRepSend(
     string server, string toAddress, string AccountNo, string strserverpath, string lpath
     )
    {

        DataSet ds = new DataSet();

        //String str_path = NETClass.Classes.GetPath(1);

        Warning[] warnings = null;
        string[] streamids = null;
        string mimeType = null;
        string encoding = null;
        string extension = null;
        string deviceInfo = null;
        byte[] bytes;

        Microsoft.Reporting.WebForms.LocalReport lr = new Microsoft.Reporting.WebForms.LocalReport();
        lr.ReportPath = strserverpath;

        string str_getstr = string.Format("SELECT BranchName as sub_name, AccountNo as account_no, SeqNo as seq_no, AddedBy as posted_by, PostingDateAdded as post_date, ValueDate as trans_date,ParentAccountNo as mini_account_no, currCode as curr_code, isnull((select top 1 BalAcctCurr FROM TRanshist WHERE AccountNo ='{2}' AND  PostingDateAdded < '{3}' order by PostingDateAdded desc ),0) as 'base_bf', AccountDesc as description, AccountDesc as accountname, UserNarrative as narrative, debit, credit, BalAcctCurr as balance, TransID as ref_no, BranchAdded as headsub_code, AcctExchRate as exch_rate, transCode as trans_code, UserNarrative as narration, 0 as reversed,  cast('{0}' as datetime)  as startdate, cast('{1}' as datetime)  as enddate, dbo.FnCustomerAddress(AccountNo, 0) as address,dbo.FnCustomerCity(AccountNo, 0) as city, dbo.FnAccountProductName(AccountNo) as prod_desc, ProdCode as prod_code, TransDesc as trans_desc  FROM VwTransHist WHere accountno ='{2}' AND PostingDateAdded >='{3}' AND PostingDateAdded <= '{4}' order by PostingDateAdded, seq_no", ((IssueDate.ToString("yyyy-MM-dd"))), (ExpiryDate.ToString("yyyy-MM-dd")), AccountNo, IssueDate.ToString("yyyy-MM-dd"), ExpiryDate.ToString("yyyy-MM-dd"));

        //string str_getstr = string.Format("SELECT BranchName as sub_name, AccountNo as account_no, SeqNo as seq_no, AddedBy as posted_by, PostingDateAdded as post_date, ValueDate as trans_date,ParentAccountNo as mini_account_no, currCode as curr_code, isnull((select top 1 BalAcctCurr FROM TRanshist WHERE AccountNo ='{2}' AND  PostingDateAdded < '{3}' order by PostingDateAdded desc ),0) as 'base_bf', AccountDesc as description, AccountDesc as accountname, UserNarrative as narrative, debit, credit, BalAcctCurr as balance, TransID as ref_no, BranchAdded as headsub_code, AcctExchRate as exch_rate, transCode as trans_code, UserNarrative as narration, 0 as reversed,  cast('{0}' as datetime)  as startdate, cast('{1}' as datetime)  as enddate, dbo.FnCustomerAddress(AccountNo, 0) as address,dbo.FnCustomerCity(AccountNo, 0) as city, dbo.FnAccountProductName(AccountNo) as prod_desc, ProdCode as prod_code, TransDesc as trans_desc  FROM VwTransHist WHere accountno ='{2}' AND PostingDateAdded >='{3}' AND PostingDateAdded <= '{4}' order by PostingDateAdded, seq_no", startdate, enddate, AccountNo, startdate, enddate);
        string strAcctMaster = string.Format("Sproc_eStatementAx '{0}','{1}','{2}',0", AccountNo, IssueDate.ToString("yyyy-MM-dd"), ExpiryDate.ToString("yyyy-MM-dd"));

        Int64 ivalue = 0;
        SqlDataSource mysqldata = new SqlDataSource();
        mysqldata.ConnectionString = ConfigurationManager.ConnectionStrings["EazyConnectSource"].ConnectionString;
        mysqldata.SelectCommand = str_getstr;
        mysqldata.SelectCommandType = SqlDataSourceCommandType.Text;
        //mysqldata.SelectParameters.Add(new Parameter("@AccountNo", DbType.String, AccountNo));
        //mysqldata.SelectParameters.Add(new Parameter("@Startdate", DbType.Date, string.Format("{0}",startdate)));
        //mysqldata.SelectParameters.Add(new Parameter("@Enddate", DbType.Date, string.Format("{0}", enddate)));
        //mysqldata.SelectParameters.Add(new Parameter("@OrderInfo", DbType.Int16, string.Format("{0}", ivalue)));

        mysqldata.DataBind();

        string fileNameByAccount = string.Format("{0}{1}Statement.pdf", AccountNo, DateTime.Now.ToString("ddMMMyyHHmmss"));

        #region Delete file if exists
        if (File.Exists(fileNameByAccount))
        {
            // no error
            File.Delete(fileNameByAccount);
            //Response.Write("checking file exist");
            // using (FileStream fs = File.Create(lpath + fileNameByAccount))
            //{
            //    fs.Write(bytes, 0, bytes.Length);
            //    fs.Close();
            //    fs.Dispose();
            //}
        }
        else
        {
            //Response.Write("checking file does not exist");
        }

        #endregion

        Microsoft.Reporting.WebForms.ReportDataSource myDataSource = new Microsoft.Reporting.WebForms.ReportDataSource("DataSet2_VwTransHist", mysqldata);
        lr.DataSources.Add(myDataSource);
        //bytes = lr.Render("pdf");//, null, out mimeType,out encoding, extension, streamids, warnings);

        //byte[] Bytes = lr.Render(format: "PDF", deviceInfo: "");
        //bytes = lr.Render("pdf", null, out mimeType, out encoding, out extension, out streamids, out warnings);

        bytes = lr.Render(
       "PDF",
       @"<DeviceInfo><OutputFormat>PDF</OutputFormat><HumanReadablePDF>False</HumanReadablePDF></DeviceInfo>",
       out mimeType,
       out encoding,
       out extension,
       out streamids,
       out warnings);

        using (FileStream fs = new FileStream(lpath + fileNameByAccount, FileMode.Create))
        //using (FileStream fs = File.Create(lpath + fileNameByAccount))
        {
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
            fs.Dispose();
        }

        //FileStream fs = new FileStream(AttacthedPath + "Statement.pdf", FileMode.Create);
        //FileStream fs1 = new FileStream(lpath + fileNameByAccount, FileMode.Create);
        //FileStream fs = new FileStream(lpath + fileNameByAccount, FileMode.Create);
        // Set stream position to end-of-file
        //fs.Seek(0, SeekOrigin.End);
        //  fs1.Write(bytes, 0, bytes.Length);
        //fs1.Close();

        //string password = AccountNo.Length <= 6 ? AccountNo : AccountNo.Substring(AccountNo.Length - 6);//  AccountNo., 6);
        //                                                                                                //PdfReader reader = new PdfReader(lpath + "Statement.pdf");
        //PdfReader reader = new PdfReader(lpath + fileNameByAccount);
        //PdfEncryptor.IsCopyAllowed(0);
        //PdfEncryptor.IsPrintingAllowed(0);
        ////PdfEncryptor.IsModifyContentsAllowed(1);

        ////PdfEncryptor.Encrypt(reader, new FileStream(lpath + "NAccountStatement.pdf", FileMode.OpenOrCreate,FileAccess.Write), PdfWriter.STRENGTH128BITS,password, password, PdfWriter.ALLOW_SCREENREADERS);
        //PdfEncryptor.Encrypt(reader, new FileStream(lpath + fileNameByAccount, FileMode.Create, FileAccess.Write,FileShare.ReadWrite), PdfWriter.STRENGTH128BITS, password, password, PdfWriter.ALLOW_SCREENREADERS);

        using (MemoryStream tinput = new MemoryStream(bytes))
        {
            using (MemoryStream toutput = new MemoryStream())
            {
                //original.Substring(original.Length - numberCharacters);
                string password = AccountNo.Length <= 6 ? AccountNo : AccountNo.Substring(AccountNo.Length - 6);//  AccountNo., 6);
                //PdfReader reader = new PdfReader(lpath + "Statement.pdf");
                PdfReader reader = new PdfReader(lpath + fileNameByAccount);
                PdfEncryptor.IsCopyAllowed(0);
                PdfEncryptor.IsPrintingAllowed(0);
                PdfEncryptor.IsModifyContentsAllowed(1);

                PdfEncryptor.Encrypt(reader, new FileStream(lpath + "AccountStatement.pdf", FileMode.OpenOrCreate), PdfWriter.STRENGTH128BITS, password, password, PdfWriter.ALLOW_SCREENREADERS);

                //PdfEncryptor.Encrypt(reader, new FileStream(lpath + fileNameByAccount, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite), PdfWriter.STRENGTH128BITS, password, password, PdfWriter.ALLOW_SCREENREADERS);

            }
        }

        string strgetAcctDetails = string.Format("SELECT * FROM AcctMaster WHERE AccountNo = '{0}'", AccountNo);

        string Strcons = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnectSource"].ToString();
        System.Data.SqlClient.SqlConnection conns = new SqlConnection(Strcons);

        DataSet comksp = new DataSet();
        SqlDataAdapter cmddscomp = new SqlDataAdapter(strgetAcctDetails, conns);
        cmddscomp.SelectCommand.CommandType = CommandType.Text;

        cmddscomp.Fill(comksp);
        string AccountName = "";
        foreach (DataRow jkrk in comksp.Tables[0].Rows)
        {
            AccountName = string.Format("{0}", jkrk["AccountDesc"]);
        }

        //string file = lpath + "AccountStatement.pdf";
        string file = lpath + fileNameByAccount;

        string strpwdmask = string.Format("xxxxxx{0}", AccountNo.Substring(AccountNo.Length - 3));

        string strSenderAddress = ConfigurationManager.ConnectionStrings["EmailSenderAddress"].ConnectionString;
        string strTitleSubject = "Monthly";
        if (drpStatementtype.SelectedValue == "M")
        {

            strTitleSubject = string.Format("Monthly Statement Of Account: {2} For {0} {1}", MonthName(IssueDate.Month), ExpiryDate.Year, strpwdmask);

        }
        if (drpStatementtype.SelectedValue == "N")
        {

            strTitleSubject = string.Format(" Statement Of Account: ", MonthName(IssueDate.Month), ExpiryDate.Year, strpwdmask);

        }
        if (drpStatementtype.SelectedValue == "W")
        {

            strTitleSubject = string.Format("Weekly Statement Of Account: {4} From {2} To {3}", MonthName(IssueDate.Month), ExpiryDate.Year, IssueDate, ExpiryDate, strpwdmask);

        }
        if (drpStatementtype.SelectedValue == "Q")
        {

            strTitleSubject = string.Format("Quartely Statement Of Account: {4} From {2} To {3}", MonthName(IssueDate.Month), ExpiryDate.Year, IssueDate, ExpiryDate, strpwdmask);

        }
        if (drpStatementtype.SelectedValue == "Y")
        {

            strTitleSubject = string.Format("Yearly Statement Of Account: {4} From {2} To {3}", MonthName(IssueDate.Month), ExpiryDate.Year, IssueDate, ExpiryDate, strpwdmask);

        }

        if (drpStatementtype.SelectedValue == "D")
        {
            strTitleSubject = string.Format("Daily Statement Of Account: {4} From {2} To {3}", MonthName(IssueDate.Month), IssueDate.Year, IssueDate, ExpiryDate, strpwdmask);

        }

        MailMessage message = new MailMessage(strSenderAddress, toAddress.Replace("'", ""), strTitleSubject, string.Format("<html><body>Dear {0},</br>Please find attached your statement of account – {1} . </br>The file is in PDF format. You will need ADOBE or Acrobat Reader to open it.To provide Confidentiality, a password protection has been built in.</br> When prompted for password, please enter the last six digit of your account number.</br></br>Thank you. <img alt=' ' src='cid:companylogo' /></body></html>", AccountName, AccountNo));

        // Create  the file attachment for this e-mail message.
        System.Net.Mail.Attachment data = new System.Net.Mail.Attachment(file, System.Net.Mime.MediaTypeNames.Application.Octet);
        System.Net.Mime.ContentDisposition disposition = data.ContentDisposition;
        disposition.CreationDate = System.IO.File.GetCreationTime(file);
        disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
        disposition.ReadDate = System.IO.File.GetLastAccessTime(file);
        message.Attachments.Add(data);

        AlternateView htmlView = AlternateView.CreateAlternateViewFromString("<img src=cid:companylogo>", null, "text/html");
        string lpathmagepath = lpath.Replace("Reports", "Images");

        LinkedResource logo = new LinkedResource(lpathmagepath + "companylogo.jpg");

        logo.ContentId = "companylogo";

        htmlView.LinkedResources.Add(logo);

        message.IsBodyHtml = true;

        //AddtoLogFile(strTitleSubject, "Email Title gdneral");
        SmtpClient client = new SmtpClient(server);
        // Add credentials if the SMTP server requires them.
        string StrUserName = System.Configuration.ConfigurationManager.ConnectionStrings["EmailUserName"].ConnectionString;
        string StrPwds = System.Configuration.ConfigurationManager.ConnectionStrings["EmailPwds"].ConnectionString;
        System.Net.NetworkCredential NetworkCredentials = new System.Net.NetworkCredential(StrUserName, StrPwds);

        //'client.Credentials = Net.CredentialCache.DefaultNetworkCredentials
        client.Credentials = NetworkCredentials;
        client.Port = int.Parse(System.Configuration.ConfigurationManager.ConnectionStrings["EazyEmailServerPort"].ConnectionString);
        client.EnableSsl = false;
        // 'client.PickupDirectoryLocation = "C:\EmailStatement\"
        client.DeliveryMethod = SmtpDeliveryMethod.Network;

        //client.Send(message);

        string EmailProvider = System.Configuration.ConfigurationManager.ConnectionStrings["EmailProvider"].ToString();
        if (EmailProvider == "Exchange")
        {
            client.Send(message);
        }
        if (EmailProvider == "SendGrid")
        {
            string StrCompanyBrand = System.Configuration.ConfigurationManager.ConnectionStrings["CompanyBrand"].ToString();

            var apiKey = System.Configuration.ConfigurationManager.ConnectionStrings["EmailApiKey"].ToString(); ;// "Environment.GetEnvironmentVariable("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");
            SendGridClient clientsp = new SendGridClient(apiKey);
            var from = new EmailAddress(strSenderAddress, StrCompanyBrand);

            var subject = strTitleSubject;// "Online Statement Of Account.:-" + AccountNo;
            var to = new EmailAddress(toAddress, toAddress);
            string strRespMsg = string.Format("{0}; To:{1}", strTitleSubject, toAddress.ToString());
            //AddtoLogFile(strRespMsg, "Email Title");
            var plainTextContent = string.Format("<html><body>Dear {0},</br>Please find attached your statement of account – {1} . </br>The file is in PDF format. You will need ADOBE or Acrobat Reader to open it.To provide Confidentiality, a password protection has been built in.</br> When prompted for password, please enter the last six digit of your account number.</br></br>Thank you. <img alt=' ' src='cid:companylogo' /></body></html>", AccountName, AccountNo);
            var htmlContent = string.Format("<html><body>Dear {0},</br>Please find attached your statement of account – {1} . </br>The file is in PDF format. You will need ADOBE or Acrobat Reader to open it.To provide Confidentiality, a password protection has been built in.</br> When prompted for password, please enter the last six digit of your account number.</br></br>Thank you. <img alt=' ' src='cid:companylogo' /></body></html>", AccountName, AccountNo);

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            System.Net.Mail.Attachment datank = new System.Net.Mail.Attachment(file, System.Net.Mime.MediaTypeNames.Application.Octet);
            //byte[] byteData = Encoding.ASCII.GetBytes(File.ReadAllText(file));
            byte[] byteData = File.ReadAllBytes(file);// Encoding.ASCII.GetBytes(File.ReadAllBytes(file));

            msg.Attachments = new List<SendGrid.Helpers.Mail.Attachment>
            {
                new SendGrid.Helpers.Mail.Attachment
                {
                     Content = Convert.ToBase64String(byteData),
                    //Filename = file,
                    Filename = "accountStatement", 
                    //Type =  "application/pdf",
                    //Type = System.Net.Mime.MediaTypeNames.Application.Octet,//  MimeMapping.GetMimeMapping(file),
                    Type = "application/pdf",//  MimeMapping.GetMimeMapping(file),
                    Disposition = "attachment"

                }
            };
            SendGrid.Helpers.Mail.Attachment attachments = new SendGrid.Helpers.Mail.Attachment();
            //attachments.ContentId =
            var response = clientsp.SendEmailAsync(msg);
            AddtoLogFile(string.Format("{0}", response), "Email OpsResponse");
        }
        if (EmailProvider == "SilverSands")
        {
            var apiKey = System.Configuration.ConfigurationManager.ConnectionStrings["EmailApiKey"].ToString(); ;// "Environment.GetEnvironmentVariable("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");
            string StrCompanyBrand = System.Configuration.ConfigurationManager.ConnectionStrings["CompanyBrand"].ToString();
            string EmailserverUsr = System.Configuration.ConfigurationManager.ConnectionStrings["EmailUserName"].ToString();
            string EmailserverPwd = System.Configuration.ConfigurationManager.ConnectionStrings["EmailPwds"].ToString();
            string EmailServerPort = System.Configuration.ConfigurationManager.ConnectionStrings["EazyEmailServerPort"].ToString();
            string EmailServer = System.Configuration.ConfigurationManager.ConnectionStrings["EazyEmailServer"].ConnectionString;

            var plainTextContent = string.Format("<html><body>Dear {0},</br>Please find attached your statement of account – {1} . </br>The file is in PDF format. You will need ADOBE or Acrobat Reader to open it.To provide Confidentiality, a password protection has been built in.</br> When prompted for password, please enter the last six digit of your account number.</br></br>Thank you. <img alt=' ' src='cid:companylogo' /></body></html>", AccountName, AccountNo);
            var htmlContent = string.Format("<html><body>Dear {0},</br>Please find attached your statement of account – {1} . </br>The file is in PDF format. You will need ADOBE or Acrobat Reader to open it.To provide Confidentiality, a password protection has been built in.</br> When prompted for password, please enter the last six digit of your account number.</br></br>Thank you. <img alt=' ' src='cid:companylogo' /></body></html>", AccountName, AccountNo);

            string to = toAddress;// new EmailAddress(toAddress, toAddress);
            string straltInfo = htmlContent;// string.Format("{0} Content View don't support html", emailmsg);
            var subject = strTitleSubject;
            string strRespMsg = string.Format("{0}; To:{1}", strTitleSubject, toAddress.ToString());

            string from = strSenderAddress;// string.Format("{0}", new EmailAddress(strSenderAddress, StrCompanyBrand));
            AddtoLogFile(to, string.Format("Email OpsResponse {0}", "Before Msg generating"));

            var statusCode = await SendEmailExec(from, EmailserverUsr, EmailserverPwd, StrCompanyBrand, straltInfo, to, subject, straltInfo, file);

        }
        if (EmailProvider == "Zeptomail")
        {
            string logo_b64 = Convert.ToBase64String(File.ReadAllBytes(lpathmagepath + "companylogo.jpg"));
            var apiKey = System.Configuration.ConfigurationManager.ConnectionStrings["EmailApiKey"].ToString(); ;// "Environment.GetEnvironmentVariable("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");
            string StrCompanyBrand = System.Configuration.ConfigurationManager.ConnectionStrings["CompanyBrand"].ToString();
            string EmailserverUsr = System.Configuration.ConfigurationManager.ConnectionStrings["EmailUserName"].ToString();
            string EmailserverPwd = System.Configuration.ConfigurationManager.ConnectionStrings["EmailPwds"].ToString();
            string EmailServerPort = System.Configuration.ConfigurationManager.ConnectionStrings["EazyEmailServerPort"].ToString();
            string EmailServer = System.Configuration.ConfigurationManager.ConnectionStrings["EazyEmailServer"].ConnectionString;

            var plainTextContent = string.Format("<html><body>Dear {0},</br>Please find attached your statement of account – {1} . </br>The file is in PDF format. You will need ADOBE or Acrobat Reader to open it.To provide Confidentiality, a password protection has been built in.</br> When prompted for password, please enter the last six digit of your account number.</br></br>Thank you. <img alt=' ' src='cid:companylogo' /></body></html>", AccountName, strpwdmask);
            //var htmlContent = string.Format("<html><body>Dear {0},</br>Please find attached your statement of account – {1} . </br>The file is in PDF format. You will need ADOBE or Acrobat Reader to open it.To provide Confidentiality, a password protection has been built in.</br> When prompted for password, please enter the last six digit of your account number.</br></br>Thank you. <img alt=' ' src='cid:companylogo' /></body></html>", AccountName, strpwdmask);
            var htmlContent = string.Format("<html><body><center><div style='border-radius:5px;text-align:left;background-color:aliceblue;width:75%;font-family:Trebuchet MS;'><div style='margin-top:2%;'><div><p> &nbsp;</p><h4> Dear <strong>{0},</strong></h4></div><div><p>Please find attached your statement of account – {1} . </p></div> <div><p>The file is in PDF format. You will need ADOBE or Acrobat Reader to open it.To provide Confidentiality, a password protection has been built in.</br> When prompted for password, please enter the last six digit of your account number.</p></div><div><p>Thank you.</p></div> <img alt='{2}' src='cid:companylogo' /></div></div></center></body></html>", AccountName, strpwdmask, StrCompanyBrand, logo_b64);
            //LinkedResource logo = new LinkedResource(lpathmagepath + "companylogo.jpg");

            //logo.ContentId = "companylogo";

            //htmlContent.LinkedResources.Add(logo);
            string to = toAddress;// new EmailAddress(toAddress, toAddress);
            string straltInfo = htmlContent;// string.Format("{0} Content View don't support html", emailmsg);
            var subject = strTitleSubject;
            string strRespMsg = string.Format("{0}; To:{1}", strTitleSubject, toAddress.ToString());

            string from = strSenderAddress;// string.Format("{0}", new EmailAddress(strSenderAddress, StrCompanyBrand));
            AddtoLogFile(to, string.Format("Zeptomail  OpsResponse {0}", strRespMsg));
            byte[] byteData = File.ReadAllBytes(file);

            //var statusCode = await SendEmailExec(from, EmailserverUsr, EmailserverPwd, StrCompanyBrand, straltInfo, to, subject, straltInfo, file);

            var baseAddress = "https://api.zeptomail.com/v1.1/email";

            var http = (HttpWebRequest)WebRequest.Create(new Uri(baseAddress));
            http.Accept = "application/json";
            http.ContentType = "application/json";
            http.Method = "POST";
            http.PreAuthenticate = true;
            http.Headers.Add("Authorization", apiKey);

            var strFrom = new
            {
                address = from,
                name = StrCompanyBrand

            };
            var strEmailAdress = new
            {
                address = to,
                name = to

            };
            var strto = new
            {

                email_address = strEmailAdress

            };

            var inline_logo = new
            {

                content = logo_b64,
                mime_type = "image/jpg",
                cid = "companylogo"

            };
            var inline_banner = new
            {

                content = logo_b64,
                mime_type = "image/jpg",
                cid = "companybanner"

            };

            var attachments = new
            {

                content = Convert.ToBase64String(byteData),//byteData,
                mime_type = "application/pdf",
                name = "accountStatement.pdf"

            };

            //dynamic strmessage = string.Format("{'from': { 'address': 'e-info@jubileelifeng.com'},'to':  [{'email_address': {'address': 'femi_mes@hotmail.com','name': 'itsupport@jubileelifeng.com'}}],'name': '{1}'}}],'subject':'{2}','htmlbody':'{3}'}", "e-info@jubileelifeng.com", eelTo, subject, htmlContent);
            var IcoObject = new
            {
                from = JsonConvert.SerializeObject(strFrom),// "{" + string.Format("'address': '{0}'", "e-info@jubileelifeng.com") + "}",
                to = "[" + JsonConvert.SerializeObject(strto) + "]",
                //"[{" + string.Format("'email_address': '{0}','name': '{1}'", "femi_mes@hotmail.com", "femi_mes@hotmail.com") + "}]",
                //to = "[{" +  string.Format("'email_address': {0}", "femi_mes@hotmail.com", "femi_mes@hotmail.com") + "}]",
                inline_images = "[" + JsonConvert.SerializeObject(inline_logo) + "]",

                attachments = "[" + JsonConvert.SerializeObject(attachments) + "]",
                subject = subject,
                htmlbody = htmlContent

            };
            string jsonsp = JsonConvert.SerializeObject(IcoObject);
            JObject parsedContent = JObject.Parse(jsonsp);

            Console.WriteLine(parsedContent.ToString());
            ASCIIEncoding encodings = new ASCIIEncoding();
            Byte[] zepbytes = encodings.GetBytes(parsedContent.ToString());

            Stream newStream = http.GetRequestStream();
            newStream.Write(zepbytes, 0, zepbytes.Length);
            newStream.Close();

            var response = http.GetResponse();

            var stream = response.GetResponseStream();
            var sr = new StreamReader(stream);
            var content = sr.ReadToEnd();

        }

        data.Dispose();

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
        content.Add(new StreamContent(File.OpenRead(file)), "inlineImage", file);

        request.Content = contentReq;
        var responsesp = await clientsp.SendAsync(request);
        responsesp.EnsureSuccessStatusCode();
        var msg = await responsesp.Content.ReadAsStringAsync();
        return msg;

    }
    // ---- END unchanged sending code ----

    // =====================================================================
    // Shared Request-page helpers (same code in every rewritten C# Request page)
    // =====================================================================

    /// <summary>Signed-in identity "user?branch" (forms identity, else the LogonUserID cookie).</summary>
    private string Identity()
    {
        string id = User != null && User.Identity != null ? User.Identity.Name : "";
        if (string.IsNullOrEmpty(id) && Request.Cookies["LogonUserID"] != null) id = Request.Cookies["LogonUserID"].Value;
        return id ?? "";
    }

    private string CurrentUser()
    {
        string id = Identity();
        int split = id.LastIndexOf('?');
        return split >= 0 ? id.Substring(0, split) : id;
    }

    private string CurrentBranch()
    {
        string id = Identity();
        int split = id.LastIndexOf('?');
        return split >= 0 ? id.Substring(split + 1) : "";
    }

    private static string ConnectionString(string name)
    {
        ConnectionStringSettings s = ConfigurationManager.ConnectionStrings[name];
        return s == null ? "" : s.ConnectionString;
    }

    private static string MainDb { get { return ConnectionString("EazyConnect"); } }

    /// <summary>Runs a query with parameters (never string concatenation) and returns the first table.</summary>
    private static DataTable Query(string sql, params SqlParameter[] parameters)
    {
        return QueryOn(MainDb, sql, CommandType.Text, parameters);
    }

    private static DataTable QueryOn(string connection, string sql, CommandType type, params SqlParameter[] parameters)
    {
        DataTable table = new DataTable();
        using (SqlConnection conn = new SqlConnection(connection))
        using (SqlCommand cmd = new SqlCommand(sql, conn))
        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
        {
            cmd.CommandType = type;
            cmd.Parameters.AddRange(parameters);
            adapter.Fill(table);
        }
        return table;
    }

    /// <summary>Runs a stored procedure on the main database; returns the value of its first output parameter (or "").</summary>
    private static string Exec(string procedure, params SqlParameter[] parameters)
    {
        using (SqlConnection conn = new SqlConnection(MainDb))
        using (SqlCommand cmd = new SqlCommand(procedure, conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(parameters);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
        foreach (SqlParameter p in parameters)
        {
            if (p.Direction == ParameterDirection.Output || p.Direction == ParameterDirection.InputOutput)
                return p.Value == null || p.Value == DBNull.Value ? "" : Convert.ToString(p.Value);
        }
        return "";
    }

    private static SqlParameter P(string name, object value)
    {
        return new SqlParameter(name, value ?? DBNull.Value);
    }

    private static SqlParameter Out(string name)
    {
        SqlParameter p = new SqlParameter(name, SqlDbType.VarChar, 5000);
        p.Direction = ParameterDirection.Output;
        return p;
    }

    private static string Col(DataRow row, string column)
    {
        if (row == null || !row.Table.Columns.Contains(column) || row.IsNull(column)) return "";
        return Convert.ToString(row[column]);
    }

    /// <summary>yyyy-MM-dd for an HTML date box.</summary>
    protected static string DateInput(object value)
    {
        DateTime d;
        if (value is DateTime) return ((DateTime)value).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        if (value != null && value != DBNull.Value && DateTime.TryParse(Convert.ToString(value), out d) && d.Year > 1900)
            return d.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        return "";
    }

    private static bool TryDate(string text, out DateTime value)
    {
        return DateTime.TryParseExact((text ?? "").Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out value);
    }

    /// <summary>Splits "123, 456 789" into a clean list of account numbers.</summary>
    private static List<string> AccountList(string text)
    {
        List<string> result = new List<string>();
        foreach (string part in (text ?? "").Split(new[] { ',', ';', ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries))
        {
            string acct = part.Trim().Trim('\'');
            if (acct != "" && !result.Contains(acct)) result.Add(acct);
        }
        return result;
    }

    private void ShowSuccess(string message)
    {
        litResult.Text = HttpUtility.HtmlEncode(message);
        litError.Text = "";
    }

    private void ShowError(string message)
    {
        litError.Text = HttpUtility.HtmlEncode(message);
        litResult.Text = "";
    }
}
