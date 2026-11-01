Imports System
Imports NETClass
Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports DevExpress.Web
Imports System.Net.Mail
Imports Microsoft.ReportingServices
'Imports CrystalDecisions.Shared
'Imports CrystalDecisions.CrystalReports.Engine
Imports System.Drawing.Printing


'Imports CrystalDecisions
Imports Microsoft.Reporting.WebForms

Imports iTextSharp.text
Imports iTextSharp.text.html.simpleparser
Imports iTextSharp.text.pdf
Imports System.Net.Mime




Partial Class eOperations_FrmNstatements
    Inherits System.Web.UI.Page
    Public luser As String = Context.User.Identity.Name
    Public StrLocation As String = "101" 'luser.Substring(luser.LastIndexOf("?") + 1)
    Public struser As String = "Axadmin" ' Mid(luser, 1, luser.LastIndexOf("?"))
    Public guser As String = struser
    Public l_senderlist As String
    Public dt As New DataTable
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If (IsPostBack = False And IsCallback = False) Then
            txtUserID.Text = guser

            LoadGrid()

            drpStatementtype.Value = "N"

            drpbranches.Value = "101"
            drp_products.Value = "103"
            dateIssue.Date = Today.Date
            dateexpiry.Date = Today.Date.AddDays(1.0)

            Me.ASPxGridView1.PageIndex = 0
        End If
    End Sub
    Sub LoadGrid()
        Try
            luser = Context.User.Identity.Name
            ' StrLocation = luser.Substring(luser.LastIndexOf("?") + 1)
            'struser = Mid(luser, 1, luser.LastIndexOf("?"))
            guser = struser



            If String.IsNullOrEmpty(Trim(txtSpecificAcct.Text)) = False Then



                Dim strgetAcctDetails As String = String.Format("GetAllAccountCardRequest '{0}','{1}'", drp_products.Value, drpbranches.Value)
                SqlacctDetails.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("EazyConnect").ConnectionString
                SqlacctDetails.SelectCommand = strgetAcctDetails
                SqlacctDetails.FilterExpression = String.Format("accountno in ({0})", Trim(txtSpecificAcct.Text))

                SqlacctDetails.SelectCommandType = SqlDataSourceCommandType.Text

            Else

                Dim strgetAcctDetails As String = String.Format("GetAllAccountCardRequest '{0}','{1}'", drp_products.Value, drpbranches.Value)
                SqlacctDetails.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("EazyConnect").ConnectionString
                SqlacctDetails.SelectCommand = strgetAcctDetails
                SqlacctDetails.SelectCommandType = SqlDataSourceCommandType.Text

            End If
            'Dim strgetOffr As String = String.Format("Sproc_GetTransApprovingOfficers {0},2,'{1}','{2}','{3}'", CDec(200), gEazy.GetTransCodeInfo("002", 2), StrLocation, guser)
            'sqlAppr.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("EazyConnect").ConnectionString
            'sqlAppr.SelectCommand = strgetOffr
            'sqlAppr.SelectCommandType = SqlDataSourceCommandType.Text

            'Response.Write(strgetAcctDetails)

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub ASPxGridView1_BeforePerformDataSelect(ByVal sender As Object, ByVal e As System.EventArgs) Handles ASPxGridView1.BeforePerformDataSelect
        LoadGrid()
    End Sub

    Protected Sub drp_products_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drp_products.SelectedIndexChanged

        LoadGrid()
    End Sub

    Protected Sub drpbranches_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpbranches.SelectedIndexChanged

        LoadGrid()
    End Sub

    Protected Sub txtSpecificAcct_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSpecificAcct.TextChanged

        LoadGrid()
    End Sub



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
    '    Dim str_getstr As String = String.Format("SELECT sub_name, account_no, seq_no, posted_by, post_date, trans_date, mini_account_no, curr_code,description, accountname, narrative, debit, credit, fcy_debit, fcy_credit, balance, fcy_balance, base_bf, fcy_bf, voucher_no, ref_no, headsub_code, exch_rate, deal_no,trans_code, approving_offr1, status_desc, client_name, narration, reversed,  cast('{0}' as datetime)  as startdate, cast('{1}' as datetime)  as enddate, address, city, prod_desc, prod_type_code, prod_type_desc, prod_code, trans_desc, report_head_code,report_head_desc, post_headsub_code, cheque_no, print_stmt, account_status, old_account_no, item_class, control_account_no  FROM VwTransHist WHere account_no ='{2}' AND startdate >='{3}' AND enddate <= '{4}'  order by post_date, seq_no", ((dateIssue.Date.ToShortDateString())), (dateexpiry.Date.ToShortDateString()), AccountNo, dateIssue.Date.ToShortDateString(), dateexpiry.Date.ToShortDateString())


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
                                              "  AccountDesc as description, AccountDesc as accountname, UserNarrative as narrative, debit, credit, BalAcctCurr as balance, TransID as ref_no, BranchAdded as headsub_code, AcctExchRate as exch_rate, transCode as trans_code, UserNarrative as narration, 0 as reversed,  cast('{0}' as datetime)  as startdate, cast('{1}' as datetime)  as enddate, dbo.FnCustomerAddress(AccountNo, 0) as address,dbo.FnCustomerCity(AccountNo, 0) as city, dbo.FnAccountProductName(AccountNo) as prod_desc, ProdCode as prod_code, TransDesc as trans_desc  FROM VwTransHist WHere accountno ='{2}' AND PostingDateAdded >='{3}' AND PostingDateAdded <= '{4}' order by PostingDateAdded, seq_no", ((dateIssue.Date.ToShortDateString())), (dateexpiry.Date.ToShortDateString()), AccountNo, dateIssue.Date.ToShortDateString(), dateexpiry.Date.ToShortDateString())

        Dim strreportstr As String = String.Format("SELECT BranchName as sub_name, AccountNo as account_no, SeqNo as seq_no, AddedBy as posted_by, PostingDateAdded as post_date, ValueDate as trans_date,ParentAccountNo as mini_account_no, currCode as curr_code, isnull((select top 1 BalAcctCurr FROM TRanshist WHERE AccountNo ='{2}' AND  PostingDateAdded < '{3}' order by PostingDateAdded desc ),0) as 'base_bf', " +
                                             "  AccountDesc as description, AccountDesc as accountname, UserNarrative as narrative, debit, credit, BalAcctCurr as balance, TransID as ref_no, BranchAdded as headsub_code, AcctExchRate as exch_rate, transCode as trans_code, UserNarrative as narration, 0 as reversed,  cast('{0}' as datetime)  as startdate, cast('{1}' as datetime)  as enddate, dbo.FnCustomerAddress(AccountNo, 0) as address, " +
                                             " dbo.FnCustomerCity(AccountNo, 0) as city, dbo.FnAccountProductName(AccountNo) as prod_desc, ProdCode as prod_code, TransDesc as trans_desc, '{5}' as ComName, '{6}' as ComContactTitles, '{7}' As ComAddress, '{8}' as ComCity,'{9}' as ComState, '{10}' as ComEmail, '{11}' as ComPhone,  '{12}' as ComLogojpg  FROM VwTransHist " +
                                             " WHere accountno ='{2}' AND PostingDateAdded >='{3}' AND PostingDateAdded <= '{4}' order by PostingDateAdded, seq_no", ((dateIssue.Date.ToShortDateString())), (dateexpiry.Date.ToShortDateString()), AccountNo, dateIssue.Date.ToShortDateString(), dateexpiry.Date.ToShortDateString(), strComName, strComContactTitles, strComAddress, strComCity, strComComState, strComEmail, strComPhone, strComLogojpg)



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
        If drpStatementtype.Value = "M" Then
            strTitleSubject = String.Format("Monthly Statement Of Account: {2} For {0} {1}", MonthName(dateIssue.Date.Month), dateIssue.Date.Year, strpwdmask)
        ElseIf drpStatementtype.Value = "N" Then
            strTitleSubject = String.Format(" Statement Of Account: ", MonthName(dateIssue.Date.Month), dateIssue.Date.Year, strpwdmask)
        ElseIf drpStatementtype.Value = "W" Then
            strTitleSubject = String.Format("Weekly Statement Of Account: {4} From {2} To {3}", MonthName(dateIssue.Date.Month), dateIssue.Date.Year, dateIssue.Date, dateexpiry.Date, strpwdmask)

        ElseIf drpStatementtype.Value = "Q" Then
            strTitleSubject = String.Format("Quartely Statement Of Account: {4} From {2} To {3}", MonthName(dateIssue.Date.Month), dateIssue.Date.Year, dateIssue.Date, dateexpiry.Date, strpwdmask)
        ElseIf drpStatementtype.Value = "Y" Then
            strTitleSubject = String.Format("Yearly Statement Of Account: {4} From {2} To {3}", MonthName(dateIssue.Date.Month), dateIssue.Date.Year, dateIssue.Date, dateexpiry.Date, strpwdmask)
        ElseIf drpStatementtype.Value = "D" Then
            strTitleSubject = String.Format("Daily Statement Of Account: {4} From {2} To {3}", MonthName(dateIssue.Date.Month), dateIssue.Date.Year, dateIssue.Date, dateexpiry.Date, strpwdmask)

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



    Protected Sub cmdClear_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles cmdClear.Command
        ASPxListBox1.Dispose()

        ASPxGridView1.DataBind()
    End Sub

    Protected Sub cmdClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClear.Click
        ASPxListBox1.Items.Clear()
    End Sub

    Protected Sub Button5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button5.Click
        'Try
        On Error Resume Next

        Dim item As ListItem, i As Integer = 0, j As Integer = 0

        ' For Each item In ASPxListBox1.Items

        For i = 0 To (ASPxListBox1.Items.Count - 1)
            ' If NETClass.Classes.GetDataset(str_getActive).Tables(0).Rows.Count > 0 Then


            'Dim str_ToAddress As String = NETClass.Classes.GetField("Sproc_GetAccountEmail '" & ASPxListBox1.Items(i).Value & "'")
            Dim strgetemailconn As String = ConfigurationManager.ConnectionStrings("EazyConnectSource").ConnectionString
            Dim strgetemail As String = String.Format("Sproc_GetAccountEmail '{0}'", ASPxListBox1.Items(i).Value)
            Dim DsEmail As New DataSet()
            Dim sqlgetemailcon As SqlClient.SqlConnection = New SqlConnection(strgetemailconn)

            Dim sqlDataAdapter As SqlClient.SqlDataAdapter = New SqlDataAdapter(strgetemail, sqlgetemailcon)
            sqlDataAdapter.Fill(DsEmail)
            Dim str_ToAddress As String = ""
            If DsEmail.Tables(0).Rows.Count > 0 Then
                str_ToAddress = String.Format("'{0}'", DsEmail.Tables(0).Rows(0).Item(0))
            End If
            Dim strSenderAddress As String = ConfigurationManager.ConnectionStrings("EmailSenderAddress").ConnectionString
            'If str_ToAddress = Nothing Then
            '    str_ToAddress = strSenderAddress ' "femi.mes@gmail.com"
            'End If
            'If (str_ToAddress.Contains("@") = False) Then

            '    str_ToAddress = strSenderAddress
            'End If

            If ValidateEmail(str_ToAddress) = True Then


                Dim EmailServer As String = System.Configuration.ConfigurationManager.ConnectionStrings("EazyEmailServer").ConnectionString
                Dim str_reppath As String = Server.MapPath("~/Reports/AccountEstatementNew.rdlc")
                'Dim lpath As String = Server.MapPath("~/mandate/")
                Dim lpath As String = Server.MapPath("~/Reports/")
                ' Dim lpath As String = "C:\EmailStatement\"
                'CreateMessageWithAttachmentByAcctRep(EmailServer, str_ToAddress, item.Value, str_reppath, lpath)
                CreateMessageWithAttachmentByAcctRepSend(EmailServer, str_ToAddress, ASPxListBox1.Items(i).Value, str_reppath, lpath)



                j = j + 1

            End If

            'End If




        Next

        'If item.Selected = True Then







        'End If

        'Next
        If dt.Rows.Count > 0 Then
            'Me.GridView1.DataSource = dt
            'Me.GridView1.DataBind()


        Else
            'Me.GridView1.DataSource = Nothing
            'Me.GridView1.Dispose()


        End If
        Response.Write(String.Format("<script>alert('statements Sucessfully Sent for {0}') </script>", j))

        'Catch ex As Exception
        '    Response.Write("<script>alert('The Following error has occured " & ex.Message & "') </script>")


        'End Try
    End Sub

    Public Function ValidateEmail(ByVal strCheck As String) As Boolean
        Try
            Dim vEmailAddress As New System.Net.Mail.MailAddress(strCheck)
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function
    Protected Sub Button6_Click(sender As Object, e As System.EventArgs) Handles Button6.Click
        Button5_Click(sender, e)

    End Sub

    Protected Sub Button7_Click(sender As Object, e As System.EventArgs) Handles Button7.Click
        Response.Redirect("FrmNstatements.aspx")
    End Sub

    Protected Sub Button3_Click(sender As Object, e As System.EventArgs) Handles Button3.Click
        Response.Redirect("FrmNstatements.aspx")
    End Sub

    Protected Sub dateIssue_DateChanged(sender As Object, e As System.EventArgs) Handles dateIssue.DateChanged
        changeDates()
    End Sub

    Protected Sub drpStatementtype_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles drpStatementtype.SelectedIndexChanged
        changeDates()
    End Sub

    Sub changeDates()
        Dim strTitleSubject As String = "Monthly"
        If drpStatementtype.Value = "M" Then
            dateexpiry.Date = dateIssue.Date.AddMonths(1).AddDays(-1)
            dateexpiry.ReadOnly = True
            strTitleSubject = String.Format("Monthly Statement Of Account For {0} {1}", MonthName(dateIssue.Date.Month), dateIssue.Date.Year)
        ElseIf drpStatementtype.Value = "N" Then
            'dateexpiry.Date = dateIssue.Date.AddMonths(1)
            dateexpiry.ReadOnly = False
            strTitleSubject = String.Format(" Statement Of Account ", MonthName(dateIssue.Date.Month), dateIssue.Date.Year)
        ElseIf drpStatementtype.Value = "W" Then
            dateexpiry.Date = dateIssue.Date.AddDays(7)
            dateexpiry.ReadOnly = True
            strTitleSubject = String.Format("Weekly Statement Of Account From {2} To {3}", MonthName(dateIssue.Date.Month), dateIssue.Date.Year, dateIssue.Date, dateexpiry.Date)

        ElseIf drpStatementtype.Value = "Q" Then
            dateexpiry.Date = dateIssue.Date.AddMonths(3).AddDays(-1)
            dateexpiry.ReadOnly = True
            strTitleSubject = String.Format("Quartely Statement Of Account From {2} To {3}", MonthName(dateIssue.Date.Month), dateIssue.Date.Year, dateIssue.Date, dateexpiry.Date)
        ElseIf drpStatementtype.Value = "Y" Then
            dateexpiry.Date = dateIssue.Date.AddYears(1).AddDays(-1)
            dateexpiry.ReadOnly = True
            strTitleSubject = String.Format("Yearly Statement Of Account From {2} To {3}", MonthName(dateIssue.Date.Month), dateIssue.Date.Year, dateIssue.Date, dateexpiry.Date)
        ElseIf drpStatementtype.Value = "D" Then
            dateexpiry.Date = dateIssue.Date.AddDays(1)
            dateexpiry.ReadOnly = True
            strTitleSubject = String.Format("Daily Statement Of Account From {2} To {3}", MonthName(dateIssue.Date.Month), dateIssue.Date.Year, dateIssue.Date, dateexpiry.Date)

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


End Class

