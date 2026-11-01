Option Strict Off
Imports System
Imports NETClass
Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports System.Net.Mail
Imports Microsoft.ReportingServices
Imports System.Drawing.Printing
Imports Microsoft.Reporting.WebForms
Imports iTextSharp.text
Imports iTextSharp.text.html.simpleparser
Imports iTextSharp.text.pdf
Imports System.Net.Mime
Imports ListItem = System.Web.UI.WebControls.ListItem
Imports System.Collections.Generic
Imports System.Configuration
Imports System.Globalization
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

''' <summary>
''' Account statement notifications: e-mails password-protected PDF statements (AccountEstatementNew.rdlc) to the selected accounts.
''' Accounts: GetAllAccountCardRequest (product, branch, optional account list). Plain ASP.NET controls (no DevExpress);
''' the sending code is the original page's.
''' </summary>
Partial Class eOperations_FrmNstatements
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadFilters()
            drpStatementtype.SelectedValue = "N"
            IssueDate = DateTime.Today
            ExpiryDate = DateTime.Today.AddDays(1.0)
            BindGrid()
        End If
    End Sub

    ' =====================================================================
    ' Statement period (date boxes)
    ' =====================================================================
    Private Property IssueDate As DateTime
        Get
            Dim d As DateTime
            Return If(TryDate(dateIssue.Text, d), d, DateTime.Today)
        End Get
        Set(ByVal value As DateTime)
            dateIssue.Text = DateInput(value)
        End Set
    End Property

    Private Property ExpiryDate As DateTime
        Get
            Dim d As DateTime
            Return If(TryDate(dateexpiry.Text, d), d, DateTime.Today)
        End Get
        Set(ByVal value As DateTime)
            dateexpiry.Text = DateInput(value)
        End Set
    End Property

    Protected Sub drpStatementtype_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        changeDates()
    End Sub

    Protected Sub dateIssue_DateChanged(ByVal sender As Object, ByVal e As EventArgs)
        changeDates()
    End Sub

    Private Const DefaultBranch As String = "101"
    Private Const DefaultProduct As String = "103"

    ' =====================================================================
    ' Accounts
    ' =====================================================================
    Private Sub LoadFilters()
        Try
            For Each r As DataRow In QueryProc("Sproc_GetAllProductsCards").Rows
                Dim code As String = Col(r, "Product Code"), name As String = Col(r, "product"), type As String = Col(r, "Type")
                If code = "" OrElse drp_products.Items.FindByValue(code) IsNot Nothing Then Continue For
                drp_products.Items.Add(New ListItem(If(name = "", code, name) & If(type = "", "", " (" & type & ")"), code))
            Next
            For Each r As DataRow In QueryProc("Sproc_GetAllBranchesCards").Rows
                Dim code As String = Col(r, "Headsub_code"), name As String = Col(r, "sub_name")
                If code = "" OrElse drpbranches.Items.FindByValue(code) IsNot Nothing Then Continue For
                drpbranches.Items.Add(New ListItem(If(name = "", code, name & " (" & code & ")"), code))
            Next
        Catch ex As Exception
            ShowError("The products or branches could not be loaded: " & ex.Message)
        End Try
        Pick(drp_products, DefaultProduct)
        Pick(drpbranches, DefaultBranch)
    End Sub

    Private Shared Function QueryProc(ByVal procedure As String) As DataTable
        Dim table As New DataTable()
        Using conn As New SqlConnection(Cfg(MainDb))
            Using cmd As New SqlCommand(procedure, conn)
                cmd.CommandType = CommandType.StoredProcedure
                Using adapter As New SqlDataAdapter(cmd)
                    adapter.Fill(table)
                End Using
            End Using
        End Using
        Return table
    End Function

    Private Shared Sub Pick(ByVal list As DropDownList, ByVal value As String)
        If list.Items.FindByValue(value) Is Nothing Then list.Items.Insert(0, New ListItem(value, value))
        list.ClearSelection()
        list.Items.FindByValue(value).Selected = True
    End Sub

    Private Function LoadAccounts() As DataTable
        Dim t As DataTable = Query(MainDb, "EXEC GetAllAccountCardRequest @product, @branch", P("@product", drp_products.SelectedValue), P("@branch", drpbranches.SelectedValue))
        Dim wanted As List(Of String) = AccountList(txtSpecificAcct.Text)
        If wanted.Count > 0 AndAlso t.Columns.Contains("AccountNo") Then
            Dim keep As New HashSet(Of String)(wanted, StringComparer.OrdinalIgnoreCase)
            Dim filtered As DataTable = t.Clone()
            For Each r As DataRow In t.Rows
                If keep.Contains(Col(r, "AccountNo")) Then filtered.ImportRow(r)
            Next
            t = filtered
        End If
        For Each column As String In New String() {"AccountNo", "AccountName", "Product", "DOB", "address", "city", "Book Bal", "Cleared Bal", "Trans Status", "Account Status"}
            If Not t.Columns.Contains(column) Then t.Columns.Add(column, GetType(String))
        Next
        Return t
    End Function

    Private Sub BindGrid()
        Try
            Dim t As DataTable = LoadAccounts()
            ctrlGrid.DataSource = t
            ctrlGrid.DataBind()
            litTotal.Text = String.Format("{0:N0} account{1}", t.Rows.Count, If(t.Rows.Count = 1, "", "s"))
        Catch ex As Exception
            ctrlGrid.DataSource = Nothing
            ctrlGrid.DataBind()
            litTotal.Text = ""
            ShowError("The accounts could not be loaded: " & ex.Message)
        End Try
    End Sub

    Protected Sub Filter_Changed(ByVal sender As Object, ByVal e As EventArgs)
        ctrlGrid.PageIndex = 0
        litResult.Text = ""
        litError.Text = ""
        BindGrid()
    End Sub

    Protected Sub ctrlGrid_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        ctrlGrid.PageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    ''' <summary>Ticked accounts on this page, or every listed account when "all pages" is ticked.</summary>
    Private Function SelectedAccounts() As List(Of String)
        Dim accounts As New List(Of String)()
        If chkAllRecords.Checked Then
            For Each r As DataRow In LoadAccounts().Rows
                Dim acct As String = Col(r, "AccountNo")
                If acct <> "" AndAlso Not accounts.Contains(acct) Then accounts.Add(acct)
            Next
            Return accounts
        End If
        For Each row As GridViewRow In ctrlGrid.Rows
            Dim chk As CheckBox = TryCast(row.FindControl("chkSelect"), CheckBox)
            If row.RowType = DataControlRowType.DataRow AndAlso chk IsNot Nothing AndAlso chk.Checked Then
                accounts.Add(Convert.ToString(ctrlGrid.DataKeys(row.RowIndex).Value))
            End If
        Next
        Return accounts
    End Function

    Protected Sub Button7_Click(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect(Request.Path, False)
        Context.ApplicationInstance.CompleteRequest()
    End Sub

    ''' <summary>The customer's e-mail address in core banking ("" when none).</summary>
    Private Shared Function AccountEmail(ByVal accountNo As String) As String
        Dim t As DataTable = Query(CoreDb, "EXEC Sproc_GetAccountEmail @acct", P("@acct", accountNo))
        If t.Rows.Count = 0 OrElse t.Rows(0).IsNull(0) Then Return ""
        Return Convert.ToString(t.Rows(0)(0)).Replace("'", "").Trim()
    End Function

    Private Sub Report(ByVal what As String, ByVal done As Integer, ByVal total As Integer, ByVal skipped As Integer, ByVal skippedWhy As String, ByVal failed As List(Of String))
        ShowSuccess(String.Format("{0} for {1} of {2} account{3}.{4}", what, done, total, If(total = 1, "", "s"),
            If(skipped = 0, "", String.Format(" {0} skipped {1}.", skipped, skippedWhy))))
        If failed.Count > 0 Then litError.Text = HttpUtility.HtmlEncode(String.Format("{0} failed: {1}", failed.Count, String.Join("; ", failed.ToArray())))
    End Sub

    ' =====================================================================
    ' Send statements
    ' =====================================================================
    Protected Sub Button6_Click(ByVal sender As Object, ByVal e As EventArgs)
        Page.Validate("send")
        If Not Page.IsValid Then Return
        If ExpiryDate < IssueDate Then ShowError("The end date is before the start date.") : Return
        Dim accounts As List(Of String) = SelectedAccounts()
        If accounts.Count = 0 Then ShowError("Tick at least one account, or tick ""all pages"".") : Return

        Dim emailServer As String = Cfg("EazyEmailServer")
        Dim reportPath As String = Server.MapPath("~/Reports/AccountEstatementNew.rdlc")
        Dim reportFolder As String = Server.MapPath("~/Reports/")
        Dim sent As Integer = 0, noEmail As Integer = 0
        Dim failed As New List(Of String)()
        For Each accountNo As String In accounts
            Try
                Dim toAddress As String = AccountEmail(accountNo)
                If Not ValidateEmail(toAddress) Then noEmail += 1 : Continue For
                CreateMessageWithAttachmentByAcctRepSend(emailServer, toAddress, accountNo, reportPath, reportFolder)
                sent += 1
            Catch ex As Exception
                failed.Add(accountNo & ": " & ex.Message)
            End Try
        Next
        Report("Statements sent", sent, accounts.Count, noEmail, "because they have no valid e-mail address", failed)
    End Sub

    ' ---- BEGIN unchanged sending code (carried over from the original page) ----
    Public l_senderlist As String
    Public dt As New DataTable

    'Public Sub CreateMessageWithAttachmentByAcctRep(ByVal server As String, ByVal toAddress As String, ByVal AccountNo As String, ByVal strserverpath As String, ByVal lpath As String)
    '    ' Create the Path to the Attachments
    '    On Error Resume Next

    '    Dim EmailServer As String = server 'EazyEmailServer
    '    Dim ds As New DataSet
    '    'Dim rep, cr As New ReportDocument
    '    Dim str_path As String = NETClass.Classes.GetPath(1)
    '    'Response.Write(str_path)
    '    'rep.Load(str_path & "\" & "AccountEStatement" & ".rpt")
    '    Dim warnings As Warning() = Nothing
    '    Dim streamids As String() = Nothing
    '    Dim mimeType As String = Nothing
    '    Dim encoding As String = Nothing
    '    Dim extension As String = Nothing
    '    Dim deviceInfo As String
    '    Dim bytes As Byte()

    '    Dim lr As New Microsoft.Reporting.WebForms.LocalReport

    '    lr.ReportPath = strserverpath

    '    'rep.Load("C:\EazybancNET New Features\EazybancNET Reports" & "\" & "Account Statement" & ".rpt")
    '    Dim str_getstr As String = String.Format("SELECT sub_name, account_no, seq_no, posted_by, post_date, trans_date, mini_account_no, curr_code,description, accountname, narrative, debit, credit, fcy_debit, fcy_credit, balance, fcy_balance, base_bf, fcy_bf, voucher_no, ref_no, headsub_code, exch_rate, deal_no,trans_code, approving_offr1, status_desc, client_name, narration, reversed,  cast('{0}' as datetime)  as startdate, cast('{1}' as datetime)  as enddate, address, city, prod_desc, prod_type_code, prod_type_desc, prod_code, trans_desc, report_head_code,report_head_desc, post_headsub_code, cheque_no, print_stmt, account_status, old_account_no, item_class, control_account_no  FROM VwTransHist WHere account_no ='{2}' AND startdate >='{3}' AND enddate <= '{4}'  order by post_date, seq_no", ((IssueDate.ToShortDateString())), (ExpiryDate.ToShortDateString()), AccountNo, IssueDate.ToShortDateString(), ExpiryDate.ToShortDateString())

    '    Dim mysqldata As New SqlDataSource
    '    mysqldata.ConnectionString = ConfigurationManager.ConnectionStrings("EazyConnect").ConnectionString
    '    mysqldata.SelectCommand = str_getstr
    '    mysqldata.DataBind()

    '    Dim myDataSource = New ReportDataSource("DataSet2_VwTransHist", mysqldata)
    '    lr.DataSources.Add(myDataSource)

    '    bytes = lr.Render("pdf", _
    '             Nothing, mimeType, _
    '               encoding, extension, streamids, warnings)
    '    Dim fileAcctNo As String = String.Format("{0}.pdf", AccountNo)

    '    'Dim fs As New FileStream(lpath & "Statement.pdf", FileMode.Create)

    '    Dim fs As New FileStream(lpath & fileAcctNo, FileMode.Create)

    '    fs.Write(bytes, 0, bytes.Length)
    '    fs.Close()

    '    ' Response.Write(str_getstr)
    '    ' ds = NETClass.Classes.GetDataset("SELECT * FROM VwTransHist WHere account_no ='" & AccountNo & "'")
    '    'ds = NETClass.Classes.GetDataset(str_getstr)

    '    'rep.SetDatabaseLogon(NETClass.Classes.GetPath(7), NETClass.Classes.GetPath(8), NETClass.Classes.GetPath(5), NETClass.Classes.GetPath(6))
    '    'rep.SetDataSource(ds.Tables(0))
    '    ''cr = rep
    '    '  Dim lp As CrystalDecisions.CrystalReports.Engine.ReportOptions
    '    ' rep.Export(lp.EnableSaveSummariesWithReport.TrueString)

    '    'rep.ExportToDisk(ExportFormatType.PortableDocFormat, "C:\Statement.pdf")

    '    'rep.ExportToDisk(ExportFormatType.PortableDocFormat, "Statement.pdf")

    '    'Dim file As String = lpath & "Statement.pdf" '"Statement.pdf"
    '    Dim file As String = String.Format("{0}.pdf", AccountNo)

    '    '// Create a message and set up the recipients.
    '    'Dim message As MailMessage = New MailMessage("eStatement@GTHomes" & EmailServer & ".com", "temmy54@yahoo.com", "Estatement Of Account.:-" & AccountNo, "Pls, See the Your attached Statements of Accounts.")
    '    ' Dim message As MailMessage = New MailMessage("eStatement@GTHomes", "kazeem.owolabi@gthomes.com", "Estatement Of Account.:-" & AccountNo, "Pls, See the Your attached Statements of Accounts.")
    '    Dim message As MailMessage = New MailMessage("eStatement@Resortng.com", toAddress, "Estatement Of Account.:-" & AccountNo, "Please, see your attached statements of accounts.")

    '    ''// Create  the file attachment for this e-mail message.
    '    'Dim data As Attachment = New Attachment(file, Net.Mime.MediaTypeNames.Application.Octet)
    '    'Dim disposition As Net.Mime.ContentDisposition = data.ContentDisposition
    '    'disposition.CreationDate = System.IO.File.GetCreationTime(file)
    '    'disposition.ModificationDate = System.IO.File.GetLastWriteTime(file)
    '    'disposition.ReadDate = System.IO.File.GetLastAccessTime(file)
    '    'message.Attachments.Add(data)
    '    'Dim client As SmtpClient = New SmtpClient(server)
    '    '' // Add credentials if the SMTP server requires them.
    '    'client.Credentials = Net.CredentialCache.DefaultNetworkCredentials
    '    '' client.Send(message)
    '    'l_senderlist = l_senderlist + AccountNo & ":(" & toAddress & "),"

    '    '' // Display the values in the ContentDisposition for the attachment.
    '    'Dim cd As Net.Mime.ContentDisposition = data.ContentDisposition
    '    'Console.Write("Content disposition")
    '    'Console.Write(cd.ToString())
    '    'Console.WriteLine("File {0}", cd.FileName)
    '    'Console.WriteLine("Size {0}", cd.Size)
    '    'Console.WriteLine("Creation {0}", cd.CreationDate)
    '    'Console.WriteLine("Modification {0}", cd.ModificationDate)
    '    'Console.WriteLine("Read {0}", cd.ReadDate)
    '    'Console.WriteLine("Inline {0}", cd.Inline)
    '    'Console.WriteLine("Parameters: {0}", cd.Parameters.Count)
    '    'Dim d As DictionaryEntry
    '    'For Each d In cd.Parameters

    '    '    Console.WriteLine("{0} = {1}", d.Key, d.Value)
    '    'Next
    '    'data.Dispose()

    'End Sub

    Private Function GetStreamAsByteArray(ByVal stream As System.IO.Stream) As Byte()

        Dim streamLength As Integer = Convert.ToInt32(stream.Length)

        Dim fileData As Byte() = New Byte(streamLength) {}

        ' Read the file into a byte array
        stream.Read(fileData, 0, streamLength)
        stream.Flush()
        stream.Close()

        Return fileData

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="server"></param>
    ''' <param name="toAddress"></param>
    ''' <param name="AccountNo"></param>
    ''' <param name="strserverpath"></param>
    ''' <param name="lpath"></param>
    Public Sub CreateMessageWithAttachmentByAcctRepSend(ByVal server As String, ByVal toAddress As String, ByVal AccountNo As String, ByVal strserverpath As String, ByVal lpath As String)
        ' Create the Path to the Attachments
        On Error Resume Next
        changeDates()
        Dim EmailServer As String = server 'EazyEmailServer
        Dim ds As New DataSet
        'Dim rep, cr As New ReportDocument
        Dim str_path As String = NETClass.Classes.GetPath(1)
        'Response.Write(str_path)
        'rep.Load(str_path & "\" & "AccountEStatement" & ".rpt")
        Dim warnings As Warning() = Nothing
        Dim streamids As String() = Nothing
        Dim mimeType As String = Nothing
        Dim encoding As String = Nothing
        Dim extension As String = Nothing
        Dim deviceInfo As String = "<DeviceInfo><HumanReadablePDF>True</HumanReadablePDF></DeviceInfo>"
        Dim bytes As Byte()
        Dim fbytes As Byte()
        Dim lr As New Microsoft.Reporting.WebForms.LocalReport

        lr.ReportPath = strserverpath

        Dim Strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("EazyConnect").ToString()
        Dim conn As System.Data.SqlClient.SqlConnection = New SqlConnection(Strcon)
        Dim strComName As String = "", strComContactTitles As String = "", strComAddress As String = ""
        Dim strComCity As String = "", strComComState As String = "", strComPhone As String = ""
        Dim strComEmail As String = "", strComLogojpg As String = ""
        Dim strreportcom As String = "SELECT * FROM AppReportsInfo"

        Dim comks As DataSet = New DataSet()
        Dim cmddscom As SqlDataAdapter = New SqlDataAdapter(strreportcom, conn)
        cmddscom.SelectCommand.CommandType = CommandType.Text

        cmddscom.Fill(comks)

        For Each jkr As DataRow In comks.Tables(0).Rows
            strComName = String.Format("{0}", jkr("ComName"))
            strComContactTitles = String.Format("{0}", jkr("ComContactTitles"))
            strComAddress = String.Format("{0}", jkr("ComAddress"))
            strComCity = String.Format("{0}", jkr("ComCity"))
            strComComState = String.Format("{0}", jkr("ComState"))
            strComEmail = String.Format("{0}", jkr("ComEmail"))
            strComPhone = String.Format("{0}", jkr("ComPhone"))
            strComLogojpg = String.Format("{0}", jkr("ComLogojpg"))
        Next

        'rep.Load("C:\EazybancNET New Features\EazybancNET Reports" & "\" & "Account Statement" & ".rpt")
        Dim str_getstr As String = String.Format("SELECT BranchName as sub_name, AccountNo as account_no, SeqNo as seq_no, AddedBy as posted_by, PostingDateAdded as post_date, ValueDate as trans_date,ParentAccountNo as mini_account_no, currCode as curr_code, isnull((select top 1 BalAcctCurr FROM TRanshist WHERE AccountNo ='{2}' AND  PostingDateAdded < '{3}' order by PostingDateAdded desc ),0) as 'base_bf', " &
                                              "  AccountDesc as description, AccountDesc as accountname, UserNarrative as narrative, debit, credit, BalAcctCurr as balance, TransID as ref_no, BranchAdded as headsub_code, AcctExchRate as exch_rate, transCode as trans_code, UserNarrative as narration, 0 as reversed,  cast('{0}' as datetime)  as startdate, cast('{1}' as datetime)  as enddate, dbo.FnCustomerAddress(AccountNo, 0) as address,dbo.FnCustomerCity(AccountNo, 0) as city, dbo.FnAccountProductName(AccountNo) as prod_desc, ProdCode as prod_code, TransDesc as trans_desc  FROM VwTransHist WHere accountno ='{2}' AND PostingDateAdded >='{3}' AND PostingDateAdded <= '{4}' order by PostingDateAdded, seq_no", ((IssueDate.ToShortDateString())), (ExpiryDate.ToShortDateString()), AccountNo, IssueDate.ToShortDateString(), ExpiryDate.ToShortDateString())

        Dim strreportstr As String = String.Format("SELECT BranchName as sub_name, AccountNo as account_no, SeqNo as seq_no, AddedBy as posted_by, PostingDateAdded as post_date, ValueDate as trans_date,ParentAccountNo as mini_account_no, currCode as curr_code, isnull((select top 1 BalAcctCurr FROM TRanshist WHERE AccountNo ='{2}' AND  PostingDateAdded < '{3}' order by PostingDateAdded desc ),0) as 'base_bf', " +
                                             "  AccountDesc as description, AccountDesc as accountname, UserNarrative as narrative, debit, credit, BalAcctCurr as balance, TransID as ref_no, BranchAdded as headsub_code, AcctExchRate as exch_rate, transCode as trans_code, UserNarrative as narration, 0 as reversed,  cast('{0}' as datetime)  as startdate, cast('{1}' as datetime)  as enddate, dbo.FnCustomerAddress(AccountNo, 0) as address, " +
                                             " dbo.FnCustomerCity(AccountNo, 0) as city, dbo.FnAccountProductName(AccountNo) as prod_desc, ProdCode as prod_code, TransDesc as trans_desc, '{5}' as ComName, '{6}' as ComContactTitles, '{7}' As ComAddress, '{8}' as ComCity,'{9}' as ComState, '{10}' as ComEmail, '{11}' as ComPhone,  '{12}' as ComLogojpg  FROM VwTransHist " +
                                             " WHere accountno ='{2}' AND PostingDateAdded >='{3}' AND PostingDateAdded <= '{4}' order by PostingDateAdded, seq_no", ((IssueDate.ToShortDateString())), (ExpiryDate.ToShortDateString()), AccountNo, IssueDate.ToShortDateString(), ExpiryDate.ToShortDateString(), strComName, strComContactTitles, strComAddress, strComCity, strComComState, strComEmail, strComPhone, strComLogojpg)

        Dim mysqldata As New SqlDataSource
        mysqldata.ConnectionString = ConfigurationManager.ConnectionStrings("EazyConnectSource").ConnectionString
        mysqldata.SelectCommand = strreportstr
        mysqldata.DataBind()

        Dim myDataSource = New ReportDataSource("DataSet2_VwTransHist", mysqldata)
        lr.DataSources.Add(myDataSource)

        bytes = lr.Render("pdf",
                     deviceInfo, mimeType,
                       encoding, extension, streamids, warnings)

        Dim fs As New FileStream(lpath & "Statement.pdf", FileMode.Create)
        fs.Write(bytes, 0, bytes.Length)
        fs.Close()
        'Dim pdfDoc As New Document(PageSize.A2, 10.0F, 10.0F, 10.0F, 0.0F)

        'Dim pdfDoc As New Document(PageSize.A2, 10.0F, 10.0F, 10.0F, 0.0F)
        'Dim htmlworker As HTMLWorker = New HTMLWorker(pdfDoc)
        'PdfWriter.GetInstance(pdfDoc, fs)
        'pdfDoc.Open()

        'pdfDoc.Close()
        'Dim Filebytes As Byte() = bytes

        'Dim htmlparser As New HTMLWorker(pdfDoc)
        'PdfWriter.GetInstance(pdfDoc, )
        '    pdfDoc.Open()
        '    htmlparser.Parse(bytes)
        '    pdfDoc.Close()
        'Dim bytes As Byte() = memoryStream.ToArray()
        'memoryStream.Close()
        'Dim input As New FileStream(lpath & "Statement.pdf", FileMode.Create)
        'Input.Write(bytes, 0, bytes.Length)
        'input.Close()
        'Dim PdfReader As iTextSharp.text.pdf.PdfReader = New iTextSharp.text.pdf.PdfReader(lpath & "Statement.pdf")
        'Dim PdfDoc As New iTextSharp.text.Document()
        'Dim password As String = "pass@123"
        'Dim ks As PdfWriter = PdfWriter.GetInstance(pdfDoc, New FileStream(lpath & "Statement.pdf", FileMode.Open))
        'ks.SetEncryption(PdfWriter.STRENGTH40BITS, password, password, PdfWriter.ALLOW_SCREENREADERS)

        'pdfDoc.Open()

        'pdfDoc.SetPageSize(iTextSharp.text.PageSize.LEDGER.Rotate())

        'Dim DirectContent As iTextSharp.text.pdf.PdfContentByte '= PdfWriter.DirectContent
        'Dim iNumberOfPages As Integer = PdfReader.NumberOfPages
        'Dim iPage As Integer = 0

        'pdfDoc.Close()

        Using tinput As New MemoryStream(bytes)
            Using toutput As New MemoryStream()

                Dim reader As New PdfReader(lpath & "Statement.pdf")
                PdfEncryptor.IsCopyAllowed(0)
                PdfEncryptor.IsPrintingAllowed(0)
                Dim password As String = Right(AccountNo, 6)
                PdfEncryptor.Encrypt(reader, New FileStream(lpath & "AccountStatement.pdf", FileMode.Create), PdfWriter.STRENGTH128BITS, password, password, PdfWriter.ALLOW_SCREENREADERS)
                'fbytes = toutput.ToArray()
                'Response.ContentType = "application/pdf"
                'Response.AddHeader("content-disposition", "attachment;filename=kstatement.pdf")
                'Response.Cache.SetCacheability(HttpCacheability.NoCache)
                ''Response.BinaryWrite(toutput.ToArray())
                'Response.BinaryWrite(bytes)
                'Response.End()
            End Using
        End Using

        'Dim outputs As New FileStream(lpath & "Statement2.pdf", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite)
        'Dim bbytes As Byte()
        'Dim output As New MemoryStream(bbytes, True)
        'Dim bbytes As Byte() = bytes

        '0000159340'  

        'Dim cpassword As String = "pass@123"
        ''Dim reader As  New PdfReader(input)

        '' Dim reader As New PdfReader(lpath & "Statement.pdf")
        'Dim reader As New PdfReader(input)

        ''reader = input.Read(bytes, 0, bytes.Length)
        'outputs.Write(bytes, 0, bytes.Length)
        ' ''PdfEncryptor.Encrypt(reader, outputs, False, password, password, PdfWriter.ALLOW_SCREENREADERS)
        'PdfEncryptor.Encrypt(reader, outputs, True, password, password, PdfWriter.ALLOW_SCREENREADERS)

        'Response.ContentType = "application/pdf"
        'Response.AddHeader("content-disposition", "attachment;filename=statement.pdf")
        'Response.Cache.SetCacheability(HttpCacheability.NoCache)
        'Response.BinaryWrite(GetStreamAsByteArray(outputs))

        'Response.End()
        'outputs.Close()
        ' End Using
        'End Using

        Dim strgetAcctDetails As String = String.Format("SELECT * FROM AcctMaster WHERE AccountNo = '{0}'", AccountNo)

        Dim Strcons As String = System.Configuration.ConfigurationManager.ConnectionStrings("EazyConnectSource").ToString()
        Dim conns As System.Data.SqlClient.SqlConnection = New SqlConnection(Strcons)

        Dim comksp As DataSet = New DataSet()
        Dim cmddscomp As SqlDataAdapter = New SqlDataAdapter(strgetAcctDetails, conns)
        cmddscomp.SelectCommand.CommandType = CommandType.Text

        cmddscomp.Fill(comksp)
        Dim AccountName As String = ""

        For Each jkrk As DataRow In comksp.Tables(0).Rows
            AccountName = String.Format("{0}", jkrk("AccountDesc"))
        Next

        Dim file As String = lpath & "AccountStatement.pdf" '"Statement.pdf"
        Dim strpwdmask As String = String.Format("xxxxxx{0}", Right(AccountNo, 3))
        ' Dim file As String = String.Format("'{0}'.pdf", AccountNo)
        Dim strSenderAddress As String = ConfigurationManager.ConnectionStrings("EmailSenderAddress").ConnectionString
        Dim strTitleSubject As String = "Monthly"
        If drpStatementtype.SelectedValue = "M" Then
            strTitleSubject = String.Format("Monthly Statement Of Account: {2} For {0} {1}", MonthName(IssueDate.Month), IssueDate.Year, strpwdmask)
        ElseIf drpStatementtype.SelectedValue = "N" Then
            strTitleSubject = String.Format(" Statement Of Account: ", MonthName(IssueDate.Month), IssueDate.Year, strpwdmask)
        ElseIf drpStatementtype.SelectedValue = "W" Then
            strTitleSubject = String.Format("Weekly Statement Of Account: {4} From {2} To {3}", MonthName(IssueDate.Month), IssueDate.Year, IssueDate, ExpiryDate, strpwdmask)

        ElseIf drpStatementtype.SelectedValue = "Q" Then
            strTitleSubject = String.Format("Quartely Statement Of Account: {4} From {2} To {3}", MonthName(IssueDate.Month), IssueDate.Year, IssueDate, ExpiryDate, strpwdmask)
        ElseIf drpStatementtype.SelectedValue = "Y" Then
            strTitleSubject = String.Format("Yearly Statement Of Account: {4} From {2} To {3}", MonthName(IssueDate.Month), IssueDate.Year, IssueDate, ExpiryDate, strpwdmask)
        ElseIf drpStatementtype.SelectedValue = "D" Then
            strTitleSubject = String.Format("Daily Statement Of Account: {4} From {2} To {3}", MonthName(IssueDate.Month), IssueDate.Year, IssueDate, ExpiryDate, strpwdmask)

        End If
        '// Create a message and set up the recipients.
        'Dim message As MailMessage = New MailMessage("eStatement@GTHomes" & EmailServer & ".com", "temmy54@yahoo.com", "Estatement Of Account.:-" & AccountNo, "Pls, See the Your attached Statements of Accounts.")

        ' Dim message As MailMessage = New MailMessage("eStatement@GTHomes", "kazeem.owolabi@gthomes.com", "Estatement Of Account.:-" & AccountNo, "Pls, See the Your attached Statements of Accounts.")
        Dim StrCompanyBrand As String = System.Configuration.ConfigurationManager.ConnectionStrings("CompanyBrand").ConnectionString
        Dim strMsgfrom As Net.Mail.MailAddress = New MailAddress(strSenderAddress, StrCompanyBrand)
        Dim message As MailMessage = New MailMessage(strSenderAddress, toAddress.Replace("'", ""), strTitleSubject, String.Format("<html><body> Dear {0},</br> <p>Please, find attached your monthly Abbey e-statements of Accounts.</br> </p> To provide Confidentiality, a password protection has been built in.</br> When prompted for password, please enter the last six digit of your account number.</br>Regards </br></br>This email is for the intended recipient only. {1} makes no representations about the correctness of the information contained in this email. All information are provided 'as-is' without warranty of any kind and are subject to change without notice. The entire risk arising out of their use remains with the recipient. In no event shall {1} be liable for any direct, consequential, incidental special, punitive or other damage. </br><img alt=' ' src='cid:companylogo' /></body></html>", AccountName, "Abbey Mortgagebank Plc"))
        message.From = strMsgfrom

        'Dim message As MailMessage = New MailMessage("Femi", "femi_mes@yahoo.com", "Estatement Of Account.:-" & AccountNo, "<html><body>Please, see your attached statements of Accounts.</br></br> <img alt=' ' src='cid:companylogo' /></body></html>")

        '// Create  the file attachment for this e-mail message.
        Dim data As Attachment = New Attachment(file, Net.Mime.MediaTypeNames.Application.Octet)
        Dim disposition As Net.Mime.ContentDisposition = data.ContentDisposition
        disposition.CreationDate = System.IO.File.GetCreationTime(file)
        disposition.ModificationDate = System.IO.File.GetLastWriteTime(file)
        disposition.ReadDate = System.IO.File.GetLastAccessTime(file)
        message.Attachments.Add(data)

        Dim strmsg As String = String.Format("<html><body> Dear {0},</br> <p>Please, find attached your monthly Abbey e-statements of Accounts.</br> </p> To provide Confidentiality, a password protection has been built in.</br> When prompted for password, please enter the last six digit of your account number.</br>Regards </br></br>This email is for the intended recipient only. {1} makes no representations about the correctness of the information contained in this email. All information are provided 'as-is' without warranty of any kind and are subject to change without notice. The entire risk arising out of their use remains with the recipient. In no event shall {1} be liable for any direct, consequential, incidental special, punitive or other damage. </br><img alt=' ' src='cid:companylogo' /></body></html>", AccountName, "Abbey Mortgagebank Plc")
        Dim htmlView = AlternateView.CreateAlternateViewFromString(String.Format("{0}", strmsg), Nothing, "text/html")
        Dim lpathmagepath As String = lpath.Replace("Reports", "Images")
        'Dim logo As LinkedResource = New LinkedResource("~/Images/logosmallorg.JPG")
        Dim logo As LinkedResource = New LinkedResource(lpathmagepath & "companylogo.JPG")

        logo.ContentId = "companylogo"

        htmlView.LinkedResources.Add(logo)
        message.AlternateViews.Add(htmlView)
        message.IsBodyHtml = True

        Dim client As SmtpClient = New SmtpClient(server)
        ' // Add credentials if the SMTP server requires them.
        Dim StrUserName As String = System.Configuration.ConfigurationManager.ConnectionStrings("EmailUserName").ConnectionString
        Dim StrPwds As String = System.Configuration.ConfigurationManager.ConnectionStrings("EmailPwds").ConnectionString
        Dim iPort As Integer = Integer.Parse(System.Configuration.ConfigurationManager.ConnectionStrings("EazyEmailServerPort").ConnectionString)
        Dim NetworkCredentials As Net.NetworkCredential = New Net.NetworkCredential(StrUserName, StrPwds)

        'client.Credentials = Net.CredentialCache.DefaultNetworkCredentials
        client.Credentials = NetworkCredentials
        client.Port = iPort
        client.EnableSsl = False
        'client.PickupDirectoryLocation = "C:\EmailStatement\"
        client.DeliveryMethod = SmtpDeliveryMethod.Network

        ' client.Send(message)

        ' Sections to push via SendGrid

        Dim strRep = ""

        Dim EmailProvider As String = ConfigurationManager.ConnectionStrings("EmailProvider").ToString()
        If (EmailProvider <> "SendGrid") Then
            client.Send(message)

            strRep = "Ok Default"
        Else
            'Dim StrCompanyBrand As String = System.Configuration.ConfigurationManager.ConnectionStrings("CompanyBrand").ToString()
            Dim EmailSenderAddress As String = strSenderAddress

            Dim apiKey = System.Configuration.ConfigurationManager.ConnectionStrings("EmailApiKey").ToString() ' ;// "Environment.GetEnvironmentVariable("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");
            Dim clientsp As SendGrid.SendGridClient = New SendGrid.SendGridClient(apiKey)
            Dim from = New SendGrid.Helpers.Mail.EmailAddress(EmailSenderAddress, StrCompanyBrand)
            Dim subject = "Online Statement Of Account.-" + AccountNo
            Dim jto = New SendGrid.Helpers.Mail.EmailAddress(toAddress, toAddress)
            Dim plainTextContent = strmsg ' String.Format("<html><body>Dear {0},</br>Please find attached your statement of account – {1} . </br>The file Is in PDF format. You will need ADOBE Or Acrobat Reader to open it.Please note that this Is Not your official statement of account. To obtain your official account statement, contact your branch office.</br></br>Thank you. <img alt=' ' src='cid:companylogo' /></body></html>", AccountName, AccountNo);
            Dim htmlContent = strmsg ' String.Format("<html><body>Dear {0},</br>Please find attached your statement of account – {1} . </br>The file is in PDF format. You will need ADOBE or Acrobat Reader to open it.Please note that this is NOT your official statement of account. To obtain your official account statement, contact your branch office.</br></br>Thank you. <img alt=' ' src='cid:companylogo' /></body></html>", AccountName, AccountNo);

            Dim msg = SendGrid.Helpers.Mail.MailHelper.CreateSingleEmail(from, jto, subject, plainTextContent, htmlContent)
            Dim datank = New System.Net.Mail.Attachment(file, System.Net.Mime.MediaTypeNames.Application.Octet)
            'byte[] byteData = Encoding.ASCII.GetBytes(File.ReadAllText(file));
            Dim byteData As Byte() = My.Computer.FileSystem.ReadAllBytes(file) '// Encoding.ASCII.GetBytes(File.ReadAllBytes(file));
            Dim msgAddAttachments As New SendGrid.Helpers.Mail.Attachment ' = New List < SendGrid.Helpers.Mail.Attachment >
            msgAddAttachments.Content = Convert.ToBase64String(byteData)
            msgAddAttachments.Filename = file
            'Dim kl As String = System.Web.GetMimeMapping(file)

            msgAddAttachments.Type = "application/pdf"
            msgAddAttachments.Disposition = "attachment"
            msg.AddAttachment(msgAddAttachments)

            'SendGrid.Helpers.Mail.Attachment attachments = New SendGrid.Helpers.Mail.Attachment();
            'attachments.ContentId =
            Dim Response = clientsp.SendEmailAsync(msg)
            strRep = Response.ToString()
        End If

        Dim strlogmsg = String.Format("{0} ; Response{1}", strmsg, strRep)

        AddtoLogFile(strlogmsg, "Statement By Email")

        l_senderlist = String.Format("{0}{1}:({2}),", l_senderlist, AccountNo, toAddress)

        ' // Display the values in the ContentDisposition for the attachment.
        Dim cd As Net.Mime.ContentDisposition = data.ContentDisposition
        Console.Write("Content disposition")
        Console.Write(cd.ToString())
        Console.WriteLine("File {0}", cd.FileName)
        Console.WriteLine("Size {0}", cd.Size)
        Console.WriteLine("Creation {0}", cd.CreationDate)
        Console.WriteLine("Modification {0}", cd.ModificationDate)
        Console.WriteLine("Read {0}", cd.ReadDate)
        Console.WriteLine("Inline {0}", cd.Inline)
        Console.WriteLine("Parameters: {0}", cd.Parameters.Count)
        Dim d As DictionaryEntry
        For Each d In cd.Parameters

            Console.WriteLine("{0} = {1}", d.Key, d.Value)
        Next
        data.Dispose()

    End Sub

    Public Function ValidateEmail(ByVal strCheck As String) As Boolean
        Try
            Dim vEmailAddress As New System.Net.Mail.MailAddress(strCheck)
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    Sub changeDates()
        Dim strTitleSubject As String = "Monthly"
        If drpStatementtype.SelectedValue = "M" Then
            ExpiryDate = IssueDate.AddMonths(1).AddDays(-1)
            dateexpiry.ReadOnly = True
            strTitleSubject = String.Format("Monthly Statement Of Account For {0} {1}", MonthName(IssueDate.Month), IssueDate.Year)
        ElseIf drpStatementtype.SelectedValue = "N" Then
            'ExpiryDate = IssueDate.AddMonths(1)
            dateexpiry.ReadOnly = False
            strTitleSubject = String.Format(" Statement Of Account ", MonthName(IssueDate.Month), IssueDate.Year)
        ElseIf drpStatementtype.SelectedValue = "W" Then
            ExpiryDate = IssueDate.AddDays(7)
            dateexpiry.ReadOnly = True
            strTitleSubject = String.Format("Weekly Statement Of Account From {2} To {3}", MonthName(IssueDate.Month), IssueDate.Year, IssueDate, ExpiryDate)

        ElseIf drpStatementtype.SelectedValue = "Q" Then
            ExpiryDate = IssueDate.AddMonths(3).AddDays(-1)
            dateexpiry.ReadOnly = True
            strTitleSubject = String.Format("Quartely Statement Of Account From {2} To {3}", MonthName(IssueDate.Month), IssueDate.Year, IssueDate, ExpiryDate)
        ElseIf drpStatementtype.SelectedValue = "Y" Then
            ExpiryDate = IssueDate.AddYears(1).AddDays(-1)
            dateexpiry.ReadOnly = True
            strTitleSubject = String.Format("Yearly Statement Of Account From {2} To {3}", MonthName(IssueDate.Month), IssueDate.Year, IssueDate, ExpiryDate)
        ElseIf drpStatementtype.SelectedValue = "D" Then
            ExpiryDate = IssueDate.AddDays(1)
            dateexpiry.ReadOnly = True
            strTitleSubject = String.Format("Daily Statement Of Account From {2} To {3}", MonthName(IssueDate.Month), IssueDate.Year, IssueDate, ExpiryDate)

        End If
    End Sub

    Sub AddtoLogFile(Message As String, WebPage As String)

        Dim LogPath As String = Server.MapPath("~/log/") ' ConfigurationManager.AppSettings["LogPath"].ToString()
        Dim filename As String = "Log_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt"
        Dim filepath As String = LogPath + filename
        Dim writer As System.IO.StreamWriter
        If (File.Exists(filepath)) Then
            writer = My.Computer.FileSystem.OpenTextFileWriter(filepath, True)
            writer.WriteLine("Source :" + WebPage)
            writer.WriteLine(Message)
            writer.WriteLine("——————-END————-" + DateTime.Now)

        Else

            writer = File.CreateText(filepath)
            writer.WriteLine("——————-START————-" + DateTime.Now)
            writer.WriteLine("Source :" + WebPage)
            writer.WriteLine(Message)
            writer.WriteLine("——————-END————-" + DateTime.Now)
            writer.Close()

        End If
    End Sub
    ' ---- END unchanged sending code ----

    ' =====================================================================
    ' Shared Request-page helpers (same code in every rewritten Request page)
    ' =====================================================================
    Private Const MainDb As String = "EazyConnect"
    Private Const CoreDb As String = "EazyConnectSource"

    ''' <summary>Signed-in officer: the LogonUserID cookie is "user?branch".</summary>
    Private Function CurrentUser() As String
        Dim cookie As HttpCookie = Request.Cookies("LogonUserID")
        Dim luser As String = If(cookie IsNot Nothing AndAlso Not String.IsNullOrEmpty(cookie.Value), cookie.Value, If(User.Identity.Name, ""))
        Dim split As Integer = luser.LastIndexOf("?"c)
        Dim name As String = If(split >= 0, luser.Substring(0, split), luser)
        Return If(name = "", "system", name)
    End Function

    ''' <summary>Branch of the signed-in officer (the part after "?" in the cookie).</summary>
    Private Function CurrentBranch() As String
        Dim cookie As HttpCookie = Request.Cookies("LogonUserID")
        Dim luser As String = If(cookie IsNot Nothing AndAlso Not String.IsNullOrEmpty(cookie.Value), cookie.Value, If(User.Identity.Name, ""))
        Dim split As Integer = luser.LastIndexOf("?"c)
        Return If(split >= 0, luser.Substring(split + 1), "")
    End Function

    Private Shared Function Cfg(ByVal name As String) As String
        Dim s As ConnectionStringSettings = ConfigurationManager.ConnectionStrings(name)
        Return If(s Is Nothing, "", s.ConnectionString)
    End Function

    Private Shared Function SqlText(ByVal value As String) As String
        Return If(value, "").Replace("'", "''")
    End Function

    Private Shared Sub Audit(ByVal code As String, ByVal message As String, ByVal actor As String)
        Classes.RecordOp(String.Format("Sproc_AddAuditLog '{0}','{1}','{2}',''", code, SqlText(message), SqlText(actor)))
    End Sub

    ''' <summary>Runs a stored procedure on the main database.</summary>
    Private Shared Sub Exec(ByVal procedure As String, ByVal ParamArray parameters As SqlParameter())
        Using conn As New SqlConnection(Cfg(MainDb))
            Using cmd As New SqlCommand(procedure, conn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddRange(parameters)
                conn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    ''' <summary>Runs a query with parameters (never string concatenation) and returns the first table.</summary>
    Private Shared Function Query(ByVal connectionName As String, ByVal sql As String, ByVal ParamArray parameters As SqlParameter()) As DataTable
        Dim table As New DataTable()
        Using conn As New SqlConnection(Cfg(connectionName))
            Using cmd As New SqlCommand(sql, conn)
                cmd.Parameters.AddRange(parameters)
                Using adapter As New SqlDataAdapter(cmd)
                    adapter.Fill(table)
                End Using
            End Using
        End Using
        Return table
    End Function

    Private Shared Function P(ByVal name As String, ByVal value As Object) As SqlParameter
        Return New SqlParameter(name, If(value, DBNull.Value))
    End Function

    Private Shared Function Col(ByVal row As DataRow, ByVal column As String) As String
        If row Is Nothing OrElse Not row.Table.Columns.Contains(column) OrElse row.IsNull(column) Then Return ""
        Return Convert.ToString(row(column))
    End Function

    ''' <summary>Splits "123, 456 789" into a clean list of account numbers.</summary>
    Private Shared Function AccountList(ByVal text As String) As List(Of String)
        Dim result As New List(Of String)()
        For Each part As String In If(text, "").Split(New Char() {","c, ";"c, " "c, Convert.ToChar(13), Convert.ToChar(10), Convert.ToChar(9)}, StringSplitOptions.RemoveEmptyEntries)
            Dim acct As String = part.Trim().Trim("'"c)
            If acct <> "" AndAlso Not result.Contains(acct) Then result.Add(acct)
        Next
        Return result
    End Function

    Private Shared Function TryDate(ByVal value As String, ByRef result As DateTime) As Boolean
        Return DateTime.TryParseExact(If(value, "").Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, result)
    End Function

    ''' <summary>Formats a date for an HTML date input (yyyy-MM-dd).</summary>
    Protected Shared Function DateInput(ByVal value As Object) As String
        If TypeOf value Is DateTime Then Return CType(value, DateTime).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
        Dim d As DateTime
        If DateTime.TryParse(Convert.ToString(value), d) Then Return d.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
        Return ""
    End Function

    Private Sub ShowSuccess(ByVal message As String)
        litResult.Text = HttpUtility.HtmlEncode(message)
        litError.Text = ""
    End Sub

    Private Sub ShowError(ByVal message As String)
        litError.Text = HttpUtility.HtmlEncode(message)
        litResult.Text = ""
    End Sub
End Class

