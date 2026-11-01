Option Strict Off
Imports System
Imports NETClass
Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports System.Net.Mail
Imports System.Drawing.Printing
Imports Microsoft.ReportingServices
Imports System.Xml
Imports System.Text
Imports System.Security.Cryptography
Imports System.Web.UI.HtmlControls
Imports System.Collections
Imports System.Configuration
Imports System.Net
Imports System.Globalization
Imports Microsoft.Reporting.WebForms
Imports SendGrid
Imports SendGrid.Helpers.Mail
Imports System.Threading.Tasks
Imports System.Net.Http
Imports Newtonsoft.Json
Imports Newtonsoft
Imports Newtonsoft.Json.Linq
Imports System.Collections.Generic
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

''' <summary>
''' Bulk e-mail broadcast: sends the subject and message (with account placeholders filled in) to the selected accounts.
''' Accounts: GetAllAccountCardRequest (product, branch, optional account list). Plain ASP.NET controls (no DevExpress);
''' the sending code is the original page's.
''' </summary>
Partial Class eOperations_FrmBulkEmail
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadFilters()
            BindGrid()
        End If
    End Sub

    ' Kept for the original sending code, which builds (unused) statement queries for today's date.
    Private ReadOnly Property IssueDate As DateTime
        Get
            Return DateTime.Today
        End Get
    End Property

    Private ReadOnly Property ExpiryDate As DateTime
        Get
            Return DateTime.Today.AddDays(1.0)
        End Get
    End Property

    Protected Sub rblFormat_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim html As Boolean = rblFormat.SelectedValue = "2"
        txtMessage.Visible = Not html
        txtHtml.Visible = html
    End Sub

    Private Function MessageText() As String
        Return If(rblFormat.SelectedValue = "2", txtHtml.Text, txtMessage.Text).Trim()
    End Function

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
    ' Send broadcast
    ' =====================================================================
    Protected Sub Button6_Click(ByVal sender As Object, ByVal e As EventArgs)
        Page.Validate("send")
        If Not Page.IsValid Then Return
        If MessageText() = "" Then ShowError("Write the message first.") : Return
        Dim accounts As List(Of String) = SelectedAccounts()
        If accounts.Count = 0 Then ShowError("Tick at least one account, or tick ""all pages"".") : Return

        Dim emailServer As String = Cfg("EazyEmailServer")
        Dim reportPath As String = Server.MapPath("~/Reports/AccountEstatement.rdlc")
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
        Report("Broadcast sent", sent, accounts.Count, noEmail, "because they have no valid e-mail address", failed)
    End Sub

    ' ---- BEGIN unchanged sending code (carried over from the original page) ----
    Public l_senderlist As String
    Public dt As New DataTable
    Public ks As IEnumerable = {"'", ".", ",", "[", "]", "}", "{"}

    Sub CreateMessageWithAttachmentByAcctRepSend(ByVal server As String, ByVal toAddress As String, ByVal AccountNo As String, ByVal strserverpath As String, ByVal lpath As String)
        ' Create the Path to the Attachments
        'On Error Resume Next

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
        Dim deviceInfo As String
        Dim bytes As Byte()

        Dim lr As New Microsoft.Reporting.WebForms.LocalReport

        lr.ReportPath = strserverpath

        'rep.Load("C:\EazybancNET New Features\EazybancNET Reports" & "\" & "Account Statement" & ".rpt")
        Dim str_getstr As String = String.Format("SELECT BranchName as sub_name, AccountNo as account_no, SeqNo as seq_no, AddedBy as posted_by, PostingDateAdded as post_date, ValueDate as trans_date,ParentAccountNo as mini_account_no, currCode as curr_code," &
                                              "  AccountDesc as description, AccountDesc as accountname, UserNarrative as narrative, debit, credit, BalAcctCurr as balance, TransID as ref_no, BranchAdded as headsub_code, AcctExchRate as exch_rate, transCode as trans_code, UserNarrative as narration, 0 as reversed,  cast('{0}' as datetime)  as startdate, cast('{1}' as datetime)  as enddate, dbo.FnCustomerAddress(AccountNo, 0) as address,dbo.FnCustomerCity(AccountNo, 0) as city, dbo.FnAccountProductName(AccountNo) as prod_desc, ProdCode as prod_code, TransDesc as trans_desc  FROM VwTransHist WHere accountno ='{2}' AND PostingDateAdded >='{3}' AND PostingDateAdded <= '{4}' order by PostingDateAdded, seq_no", ((IssueDate.ToShortDateString())), (ExpiryDate.ToShortDateString()), AccountNo, IssueDate.ToShortDateString(), ExpiryDate.ToShortDateString())

        Dim mysqldata As New SqlDataSource
        mysqldata.ConnectionString = ConfigurationManager.ConnectionStrings("EazyConnectSource").ConnectionString
        mysqldata.SelectCommand = str_getstr
        mysqldata.DataBind()

        '  Dim myDataSource = New ReportDataSource("DataSet2_VwTransHist", mysqldata)
        ' lr.DataSources.Add(myDataSource)

        ' bytes = lr.Render("pdf", _
        '      Nothing, mimeType, _
        '       encoding, extension, streamids, warnings)

        'Dim fs As New FileStream(lpath & "Statement.pdf", FileMode.Create)
        'fs.Write(bytes, 0, bytes.Length)
        '  fs.Close()

        ' Dim file As String = lpath & "Statement.pdf" '"Statement.pdf"
        ' Dim file As String = String.Format("'{0}'.pdf", AccountNo)
        Dim strSenderAddress As String = ConfigurationManager.ConnectionStrings("EmailSenderAddress").ConnectionString
        '// Create a message and set up the recipients.
        'Dim message As MailMessage = New MailMessage("eStatement@GTHomes" & EmailServer & ".com", "temmy54@yahoo.com", "Estatement Of Account.:-" & AccountNo, "Pls, See the Your attached Statements of Accounts.")
        ' Dim message As MailMessage = New MailMessage("eStatement@GTHomes", "kazeem.owolabi@gthomes.com", "Estatement Of Account.:-" & AccountNo, "Pls, See the Your attached Statements of Accounts.")
        Dim message As MailMessage
        'If rblFormat.SelectedValue = 1 Then

        '    message = New MailMessage(strSenderAddress, toAddress.Replace("'", ""), GenerateMsg(String.Format("{0}", txtSubject.Text), AccountNo), String.Format("<html><body><img alt='Bulk Message' src='cid:companylogo' {0}  /></body></html>", txtMessage.Text))
        'Else

        '    message = New MailMessage(strSenderAddress, toAddress.Replace("'", ""), GenerateMsg(String.Format("{0}", txtSubject.Text), AccountNo), String.Format("<html><body><img alt='Bulk Message' src='cid:companylogo' {0}  /></body></html>", txtHtml.Text))

        'End If  
        Dim StrCompanyBrand As String = System.Configuration.ConfigurationManager.ConnectionStrings("CompanyBrand").ConnectionString
        Dim strMsgfrom As System.Net.Mail.MailAddress = New MailAddress(strSenderAddress, StrCompanyBrand)
        If rblFormat.SelectedValue = 1 Then

            message = New MailMessage(strSenderAddress, toAddress.Replace("'", ""), String.Format("{0}", txtSubject.Text), String.Format("<html><body><img alt='Bulk Message' src='cid:companylogo' {0}  /></body></html>", GenerateMsg(AccountNo, txtMessage.Text)))

        Else

            message = New MailMessage(strSenderAddress, toAddress.Replace("'", ""), String.Format("{0}", txtSubject.Text), String.Format("<html><body><img alt='Bulk Message' src='cid:companylogo' {0}  /></body></html>", GenerateMsg(AccountNo, txtHtml.Text)))

        End If
        message.From = strMsgfrom

        '// Create  the file attachment for this e-mail message.
        'Dim data As Attachment = New Attachment(file, Net.Mime.MediaTypeNames.Application.Octet)
        'Dim disposition As Net.Mime.ContentDisposition = data.ContentDisposition
        'disposition.CreationDate = System.IO.File.GetCreationTime(file)
        'disposition.ModificationDate = System.IO.File.GetLastWriteTime(file)
        'disposition.ReadDate = System.IO.File.GetLastAccessTime(file)
        'message.Attachments.Add(data)

        Dim htmlView As AlternateView
        Dim strmsg As String = ""
        Dim i As Integer = 0
        'If rblFormat.SelectedValue = 1 Then
        '    strmsg = String.Format("<html><body><img alt='Logo Message' src='cid:companylogo' /></br>{0}  </body></html>", txtMessage.Text)
        '    htmlView = AlternateView.CreateAlternateViewFromString(GenerateMsg(String.Format("{0}", strmsg), AccountNo), Nothing, "text/html")

        'Else
        '    strmsg = String.Format("<html><body><img alt='Logo Message' src='cid:companylogo'  /></br>{0}</body></html>", txtHtml.Text)
        '    htmlView = AlternateView.CreateAlternateViewFromString(GenerateMsg(String.Format("{0}", strmsg), AccountNo), Nothing, "text/html")

        'End If
        Dim strtypeofobj As String = "0"

        If rblFormat.SelectedValue = 1 Then
            strmsg = String.Format("<html><body><img alt='Logo Message' src='cid:companylogo' /></br>{0}  </body></html>", GenerateMsg(AccountNo, txtMessage.Text))
            htmlView = AlternateView.CreateAlternateViewFromString(String.Format("{0}", strmsg), Nothing, "text/html")

        Else
            If txtHtml.Text.Contains("<img src=") = True Then
                Dim images As String() = GetFullNamesOfImages()

                strtypeofobj = 1
                'strmsg = String.Format("<html><body><img alt='Embeded Message' src='cid:MsgimageLogo'/> </br>{0}</body></html>", txtHtml.Text)
                strmsg = String.Format("<html><body>{0}</body></html>", GenerateMsg(AccountNo, txtHtml.Text))

                Dim decodedBody As String = WebUtility.HtmlDecode(strmsg)
                Dim strinfomsg As String = InsertImages(strmsg)
                htmlView = AlternateView.CreateAlternateViewFromString(String.Format("{0}", strinfomsg), Nothing, "text/html")
                'htmlView = AlternateView.CreateAlternateViewFromString(String.Format("{0}", strmsg), Nothing, MediaTypeNames.Text.Html)

                For Each image As String In images
                    'item.Item(i).ContentId = Path.GetFileName(image)
                    'item.Item(i).CreateLinkedResourceFromString(image)
                    'Dim StrImagepath As String = image
                    'Dim strimagefilename As String = Path.GetFileName(image)
                    ''Dim "strimagefilename" + i As LinkedResource
                    ''strimagefilename = New LinkedResource(image)

                    'htmlView.LinkedResources.Add(New LinkedResource(image)) ', New ContentType("text/html")))
                    'htmlView.ContentId = Path.GetFileName(image)
                    If strinfomsg.Contains(Path.GetFileName(image)) = True Then
                        htmlView.LinkedResources.Add(New LinkedResource(image))
                        htmlView.LinkedResources(i).ContentId = Path.GetFileName(image)

                        i = i + 1

                    End If

                Next

            Else
                strmsg = String.Format("<html><body><img alt='Logo Message' src='cid:companylogo'/> </br>{0}</body></html>", txtHtml.Text)
                htmlView = AlternateView.CreateAlternateViewFromString(String.Format("{0}", GenerateMsg(AccountNo, strmsg)), Nothing, "text/html")

            End If

        End If
        'string lpaath = Server.MapPath("~//Images//");
        Dim lpathmagepath As String = lpath.Replace("Reports", "Images")
        Dim strInsertpath As String = lpath.Replace("\Reports\", "/")

        'Dim logo As LinkedResource = New LinkedResource("~/Images/logosmallorg.JPG")
        Dim logo As LinkedResource = New LinkedResource(lpathmagepath & "companylogo.JPG")
        'Dim jlogo As LinkedResourceCollection
        If txtHtml.Text.Contains("<img src=") = True Then

            'Dim msglog As LinkedResource = New LinkedResource(strInsertpath & "MsgimageLogo.JPG")
            'msglog.ContentId = "MsgimageLogo"
            'htmlView.LinkedResources.Add(msglog)
        Else
            logo.ContentId = "companylogo"

            htmlView.LinkedResources.Add(logo)
        End If
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
        'client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory
        client.DeliveryMethod = SmtpDeliveryMethod.Network
        Dim EmailProvider As String = System.Configuration.ConfigurationManager.ConnectionStrings("EmailProvider").ToString()
        If (EmailProvider = "Exchange") Then

            client.Send(message)
        End If

        If (EmailProvider = "SendGrid") Then

            Dim file = lpathmagepath & "companylogo.JPG"
            Dim strTitleSubject = String.Format("{0}", txtSubject.Text)
            Dim apiKey = System.Configuration.ConfigurationManager.ConnectionStrings("EmailApiKey") '// "Environment.GetEnvironmentVariable("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");
            Dim clientsp As SendGridClient = New SendGridClient(apiKey, Nothing)
            Dim from = New EmailAddress(strSenderAddress.Replace("'", ""), StrCompanyBrand)
            'var subject = "Mobile Statement Of Account.-" + AccountNo;
            Dim strto = New EmailAddress(toAddress.Replace("'", ""), toAddress)
            Dim plainTextContent = strmsg ' String.Format("<html><body>Dear {0},</br>Please find attached your statement of account – {1} . </br>The file Is in PDF format. You will need ADOBE Or Acrobat Reader to open it. To provide Confidentiality, a password protection has been built in.</br> When prompted for password, please enter the last six digit of your account number</br></br>Thank you. <img alt=' ' src='cid:companylogo' /></body></html>", AccountName, AccountNo)
            Dim htmlContent = strmsg 'String.Format("<html><body>Dear {0},</br>Please find attached your statement of account – {1} . </br>The file is in PDF format. You will need ADOBE or Acrobat Reader to open it.To provide Confidentiality, a password protection has been built in.</br> When prompted for password, please enter the last six digit of your account number.</br></br>Thank you. <img alt=' ' src='cid:companylogo' /></body></html>", AccountName, AccountNo)

            Dim msg = MailHelper.CreateSingleEmail(from, strto, strTitleSubject, plainTextContent, htmlContent)
            Dim datank As System.Net.Mail.Attachment = New System.Net.Mail.Attachment(file, System.Net.Mime.MediaTypeNames.Application.Octet)
            '//byte[] byteData = Encoding.ASCII.GetBytes(File.ReadAllText(file));
            Dim byteData As Byte() = System.IO.File.ReadAllBytes(file) '// Encoding.ASCII.GetBytes(File.ReadAllBytes(file));

            Dim spAttach As SendGrid.Helpers.Mail.Attachment
            spAttach.Content = Convert.ToBase64String(byteData)
            'spAttach.ContentId = ""
            spAttach.Type = "application/jpg"
            spAttach.Disposition = "attachment"
            msg.AddAttachment(spAttach)
            Dim Response = clientsp.SendEmailAsync(msg)

        End If

        If (EmailProvider = "SilverSands") Then
            Dim file = lpathmagepath & "companylogo.JPG"
            Dim strTitleSubject = String.Format("{0}", txtSubject.Text)

            Dim from = strSenderAddress.Replace("'", "") 'New EmailAddress(strSenderAddress.Replace("'", ""), StrCompanyBrand)
            'var subject = "Mobile Statement Of Account.-" + AccountNo;
            Dim strto As String = toAddress.Replace("'", "") ' New EmailAddress(toAddress.Replace("'", ""), toAddress)
            Dim EmailserverUsr = System.Configuration.ConfigurationManager.ConnectionStrings("EmailUserName").ToString()
            Dim EmailserverPwd = System.Configuration.ConfigurationManager.ConnectionStrings("EmailPwds").ToString()
            Dim EmailServerPort = System.Configuration.ConfigurationManager.ConnectionStrings("EazyEmailServerPort").ToString()

            Dim plainTextContent = strmsg ' String.Format("<html><body>Dear {0},</br>Please find attached your statement of account – {1} . </br>The file Is in PDF format. You will need ADOBE Or Acrobat Reader to open it. To provide Confidentiality, a password protection has been built in.</br> When prompted for password, please enter the last six digit of your account number</br></br>Thank you. <img alt=' ' src='cid:companylogo' /></body></html>", AccountName, AccountNo)
            Dim htmlContent = strmsg 'String.Format("<html><body>Dear {0},</br>Please find attached your statement of account – {1} . </br>The file is in PDF format. You will need ADOBE or Acrobat Reader to open it.To provide Confidentiality, a password protection has been built in.</br> When prompted for password, please enter the last six digit of your account number.</br></br>Thank you. <img alt=' ' src='cid:companylogo' /></body></html>", AccountName, AccountNo)

            '   AddtoLogFile(l_email, String.Format("LoginAccess Email Info/responseCode {0}", "Before Msg generating"));

            Dim statusCode = SendEmailExec(from, EmailserverUsr, EmailserverPwd, StrCompanyBrand, plainTextContent, strto, strTitleSubject, htmlContent)

        End If

        If (EmailProvider = "Zeptomail") Then
            Dim file = lpathmagepath & "companylogo.JPG"
            Dim strTitleSubject = String.Format("{0}", txtSubject.Text)
            Dim apiKey = System.Configuration.ConfigurationManager.ConnectionStrings("EmailApiKey")
            Dim from = strSenderAddress.Replace("'", "") 'New EmailAddress(strSenderAddress.Replace("'", ""), StrCompanyBrand)
            'var subject = "Mobile Statement Of Account.-" + AccountNo;
            Dim strto As String = toAddress.Replace("'", "") ' New EmailAddress(toAddress.Replace("'", ""), toAddress)
            Dim EmailserverUsr = System.Configuration.ConfigurationManager.ConnectionStrings("EmailUserName").ToString()
            Dim EmailserverPwd = System.Configuration.ConfigurationManager.ConnectionStrings("EmailPwds").ToString()
            Dim EmailServerPort = System.Configuration.ConfigurationManager.ConnectionStrings("EazyEmailServerPort").ToString()

            Dim plainTextContent = strmsg ' String.Format("<html><body>Dear {0},</br>Please find attached your statement of account – {1} . </br>The file Is in PDF format. You will need ADOBE Or Acrobat Reader to open it. To provide Confidentiality, a password protection has been built in.</br> When prompted for password, please enter the last six digit of your account number</br></br>Thank you. <img alt=' ' src='cid:companylogo' /></body></html>", AccountName, AccountNo)
            Dim htmlContent = strmsg 'String.Format("<html><body>Dear {0},</br>Please find attached your statement of account – {1} . </br>The file is in PDF format. You will need ADOBE or Acrobat Reader to open it.To provide Confidentiality, a password protection has been built in.</br> When prompted for password, please enter the last six digit of your account number.</br></br>Thank you. <img alt=' ' src='cid:companylogo' /></body></html>", AccountName, AccountNo)

            '   AddtoLogFile(l_email, String.Format("LoginAccess Email Info/responseCode {0}", "Before Msg generating"));

            Dim statusCode = SendZeptomailExec(from, EmailserverUsr, EmailserverPwd, StrCompanyBrand, plainTextContent, strto, strTitleSubject, htmlContent)

        End If

        l_senderlist = String.Format("{0}{1}:({2}),", l_senderlist, AccountNo, toAddress)

        ' // Display the values in the ContentDisposition for the attachment.
        'Dim cd As Net.Mime.ContentDisposition = data.ContentDisposition
        'Console.Write("Content disposition")
        'Console.Write(cd.ToString())
        'Console.WriteLine("File {0}", cd.FileName)
        'Console.WriteLine("Size {0}", cd.Size)
        'Console.WriteLine("Creation {0}", cd.CreationDate)
        'Console.WriteLine("Modification {0}", cd.ModificationDate)
        'Console.WriteLine("Read {0}", cd.ReadDate)
        'Console.WriteLine("Inline {0}", cd.Inline)
        'Console.WriteLine("Parameters: {0}", cd.Parameters.Count)
        'Dim d As DictionaryEntry
        'For Each d In cd.Parameters

        '    Console.WriteLine("{0} = {1}", d.Key, d.Value)
        'Next
        'data.Dispose()

    End Sub

    Public Function SendEmailExec(from As String, username As String, password As String, displayname As String, text As String, strto As String, subject As String, htmlMsg As String) As String
        Dim url As String = "https://api.silversands.com.ng/email/1/send"
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 Or SecurityProtocolType.Tls Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls12

        Dim ApiKey As String = Convert.ToBase64String(Encoding.Default.GetBytes(String.Format("{0}:{1}", username, password)))

        from = String.Format("{0} <{1}>", displayname, from)

        Dim strboundary As String = String.Format("{0}{1}", "------------", Guid.NewGuid())

        Dim Content = New MultipartFormDataContent(strboundary)

        ' byteArrayContent = New ByteArrayContent(byteDataAd);
        Dim client As New HttpClient()

        Dim clientsp As New HttpClient()
        Dim Request = New HttpRequestMessage(HttpMethod.Post, "https://api.silversands.com.ng/email/3/send")
        Request.Headers.Add("Authorization", "Basic " + ApiKey)
        Dim contentReq = New MultipartFormDataContent(strboundary)
        contentReq.Add(New StringContent(from), "from")
        contentReq.Add(New StringContent(strto), "to")
        contentReq.Add(New StringContent(subject), "subject")

        contentReq.Add(New StringContent(htmlMsg), "html")

        Request.Content = contentReq
        'Await clientsp.SendAsync(Request)
        Dim responsesp As Task(Of HttpResponseMessage)
        responsesp = clientsp.SendAsync(Request)

        Dim msg As String = responsesp.Result.RequestMessage.ToString()

        'Dim msgs As String = Await client.PostAsync(Request, contentReq).Result

        Return msg
    End Function

    Public Function SendZeptomailExec(from As String, username As String, password As String, displayname As String, text As String, strto As String, subject As String, htmlMsg As String) As String
        Dim url As String = "https://api.zeptomail.com/v1.1/email"
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 Or SecurityProtocolType.Tls Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls12

        Dim ApiKey As String = System.Configuration.ConfigurationManager.ConnectionStrings("EmailApiKey").ConnectionString()

        'from = String.Format("{0} <{1}>", displayname, from)

        Dim sttomsg As New strEmailAdressClass With {.address = strto, .name = strto}

        Dim strFrom As New strFromClass With {.address = from, .name = displayname}
        Dim strToaddress As New strToClass With {.email_address = sttomsg}

        Dim strboundary As String = String.Format("{0}{1}", "------------", Guid.NewGuid())

        Dim Content = New MultipartFormDataContent(strboundary)

        Dim IcoObjects As New IcoObject With {.subject = subject, .htmlbody = htmlMsg,
            .[to] = "[" + JsonConvert.SerializeObject(strToaddress) + "]",
            .from = JsonConvert.SerializeObject(strFrom)}

        ' byteArrayContent = New ByteArrayContent(byteDataAd);
        Dim client As New HttpClient()

        Dim jsonsp = JsonConvert.SerializeObject(IcoObjects)
        'Dim client As New HttpClient()

        Dim clientsp As New HttpClient()
        Dim Request As HttpWebRequest = WebRequest.Create(New Uri(url))
        Request.Accept = "application/json"
        Request.ContentType = "application/json"
        Request.Method = "POST"
        Request.PreAuthenticate = True

        Request.Headers.Add("Authorization", ApiKey)
        Dim Json As String = JsonConvert.SerializeObject(IcoObjects)
        Dim parsedContent As JObject = JObject.Parse(jsonsp)

        Console.WriteLine(parsedContent.ToString())
        Dim encodings As ASCIIEncoding = New ASCIIEncoding()
        Dim zepbytes As Byte() = encodings.GetBytes(parsedContent.ToString())

        Dim newStream As Stream = Request.GetRequestStream()
        newStream.Write(zepbytes, 0, zepbytes.Length)
        newStream.Close()

        Dim Response = Request.GetResponse()

        Dim Stream = Response.GetResponseStream()
        Dim SR = New StreamReader(Stream)
        Dim msg = SR.ReadToEnd()

        'Dim msg As String = responsesp.Result.RequestMessage.ToString()

        'Dim msgs As String = Await client.PostAsync(Request, contentReq).Result

        Return msg
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>

    Function GenerateMsg(ByVal AccountNo As String, ByVal rulestr As String) As String
        Dim lstr As String = rulestr

        Dim i As Integer, msg As String
        Dim msgsts As Integer = 1, colval As String = "", colvalname As String, strdef As String
        For i = 0 To lstr.Length - 1

            If lstr.Substring(i, 1) = "{" Then

                msgsts = 0
                colvalname = ""
            ElseIf lstr.Substring(i, 1) = "}" Then
                ' MsgBox(Trim(Replace(colval, "}", "")))
                Dim deitemcode As String = Trim(Replace(colval, "}", ""))
                colvalname = formmsg(deitemcode, AccountNo)
                ' MsgBox(colvalname)
                msgsts = 1
                colval = ""
                strdef = ""
                msg = msg + colvalname
            End If

            If msgsts = 1 Then
                colval = ""
                'strdef = strdef + lstr.Substring(i, 1) + colvalname
                msg = msg + lstr.Substring(i, 1)
            ElseIf msgsts = 0 Then

                If lstr.Substring(i, 1) = "{" Then
                    colval = ""
                Else
                    colval = colval + lstr.Substring(i, 1)

                End If

            End If

        Next
        Return Replace(msg, "}", "")

    End Function
    Function formmsg(ByVal item_code As String, ByVal AccountNo As String) As String
        Dim k As Integer = 0
        ' For k = 0 To DataGrid1.Attributes("NoOfCols") - 1
        If Trim(item_code) = "AccountNo" Then

            Return AccountNo
        ElseIf Trim(item_code) = "AccountDesc" Then

            Dim strgetToInfo As String = String.Format("SELECT [dbo].[FnAccountName]('{0}')", AccountNo)

            Dim strconn As String = ConfigurationManager.ConnectionStrings("EazyConnectSource").ConnectionString
            Dim sqlConn As Data.SqlClient.SqlConnection = New SqlClient.SqlConnection(strconn)
            Dim sqlDataAdapter As Data.SqlClient.SqlDataAdapter = New SqlDataAdapter(strgetToInfo, sqlConn)
            Dim Ds As New DataSet()
            sqlDataAdapter.Fill(Ds)
            Return Ds.Tables(0).Rows(0).Item(0)
        ElseIf Trim(item_code) = "AccountBookBal" Then
            Dim strgetToInfo As String = String.Format("SELECT [dbo].[FnBookBal]('{0}')", AccountNo)

            Dim strconn As String = ConfigurationManager.ConnectionStrings("EazyConnectSource").ConnectionString
            Dim sqlConn As Data.SqlClient.SqlConnection = New SqlClient.SqlConnection(strconn)
            Dim sqlDataAdapter As Data.SqlClient.SqlDataAdapter = New SqlDataAdapter(strgetToInfo, sqlConn)
            Dim Ds As New DataSet()
            sqlDataAdapter.Fill(Ds)
            Return Ds.Tables(0).Rows(0).Item(0)
        ElseIf Trim(item_code) = "AccountAvailableBal" Then
            Dim strgetToInfo As String = String.Format("SELECT [dbo].[FnAvailableBal]('{0}')", AccountNo)

            Dim strconn As String = ConfigurationManager.ConnectionStrings("EazyConnectSource").ConnectionString
            Dim sqlConn As Data.SqlClient.SqlConnection = New SqlClient.SqlConnection(strconn)
            Dim sqlDataAdapter As Data.SqlClient.SqlDataAdapter = New SqlDataAdapter(strgetToInfo, sqlConn)
            Dim Ds As New DataSet()
            sqlDataAdapter.Fill(Ds)
            Return Ds.Tables(0).Rows(0).Item(0)
        ElseIf Trim(item_code) = "AccountBranchName" Then
            Dim strgetToInfo As String = String.Format("SELECT [dbo].[FnAccountBranchName]('{0}')", AccountNo)

            Dim strconn As String = ConfigurationManager.ConnectionStrings("EazyConnectSource").ConnectionString
            Dim sqlConn As Data.SqlClient.SqlConnection = New SqlClient.SqlConnection(strconn)
            Dim sqlDataAdapter As Data.SqlClient.SqlDataAdapter = New SqlDataAdapter(strgetToInfo, sqlConn)
            Dim Ds As New DataSet()
            sqlDataAdapter.Fill(Ds)
            Return Ds.Tables(0).Rows(0).Item(0)

        ElseIf Trim(item_code) = "AccountProductName" Then
            Dim strgetToInfo As String = String.Format("SELECT [dbo].[FnAccountProductName]('{0}')", AccountNo)

            Dim strconn As String = ConfigurationManager.ConnectionStrings("EazyConnectSource").ConnectionString
            Dim sqlConn As Data.SqlClient.SqlConnection = New SqlClient.SqlConnection(strconn)
            Dim sqlDataAdapter As Data.SqlClient.SqlDataAdapter = New SqlDataAdapter(strgetToInfo, sqlConn)
            Dim Ds As New DataSet()
            sqlDataAdapter.Fill(Ds)
            Return Ds.Tables(0).Rows(0).Item(0)
            'Else
            '    Return "something"

        End If

        ' Next

    End Function

    Public Function ValidateEmail(ByVal strCheck As String) As Boolean
        Try
            Dim vEmailAddress As New System.Net.Mail.MailAddress(strCheck)
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    Public Function GetFullNamesOfImages() As String()
        Dim _directoryName As String = Server.MapPath("~/Images") 'Directory.GetDirectories("~/Images").ToString()
        '   Dim images As String = Path.Combine(_directoryName, "Images")
        Dim images As String = Path.Combine(_directoryName, "")
        If (Not Directory.Exists(images)) Then
            Return Nothing
        End If

        Return Directory.GetFiles(images)
    End Function

    Public Function InsertImages(body As String) As String
        '  Dim images As String() = GetFullNamesOfImages()

        Dim ServDirectoryName As String = Request.FilePath 'Server.MapPath("~/Images")
        Dim strFolder As String() = Request.Url.Segments

        'For Each var As String In strFolder
        '    Response.Write(var & "</br>")
        'Next

        Dim strremove As String = String.Format("{0}{1}", strFolder.GetValue(2), strFolder.GetValue(3))
        ServDirectoryName = ServDirectoryName.Replace(strremove, "")

        'Dim kss As  string = GetFullNamesOfImages()

        'Dim images As String() = GetFullNamesOfImages.Select(Path.GetFileName).ToArray()
        Dim patternEnum As String = "(Images/)" + GetPattern(ks)
        Dim info As String = Regex.Replace(body, "(Images/)", "cid:", RegexOptions.IgnoreCase Or RegexOptions.Compiled)

        Return info.Replace(ServDirectoryName, "")

    End Function

    Public Function GetPattern(ByVal valuesToSearch As IEnumerable) As String

        Return String.Format("({0})", String.Join("|", valuesToSearch))
    End Function
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
Public Class IcoObject

    Public Property from As Object
    Public Property [to] As Object
    Public Property subject As Object
    Public Property htmlbody As Object
End Class
Public Class strFromClass

    Public Property address As String
    Public Property name As String
End Class
Public Class strToClass

    Public Property email_address
End Class
Public Class strEmailAdressClass

    Public Property address As String
    Public Property name As String
End Class

