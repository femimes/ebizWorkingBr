Imports System
Imports NETClass
Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
'Imports DevExpress.Web
Imports DevExpress.Web.ASPxEditors
Imports System.Net.Mail

'Imports CrystalDecisions.Shared
'Imports CrystalDecisions.CrystalReports.Engine
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


'Imports CrystalDecisions
Imports Microsoft.Reporting.WebForms
Imports SendGrid
Imports SendGrid.Helpers.Mail
Imports System.Threading.Tasks
Imports System.Net.Http
Imports Newtonsoft.Json
Imports Newtonsoft
Imports Newtonsoft.Json.Linq

Partial Class eOperations_FrmBulkEmail
    Inherits System.Web.UI.Page

    Public luser As String = Context.User.Identity.Name
    Public StrLocation As String = "101" 'luser.Substring(luser.LastIndexOf("?") + 1)
    Public struser As String = "Axadmin" ' Mid(luser, 1, luser.LastIndexOf("?"))
    Public guser As String = struser
    Public l_senderlist As String
    Public dt As New DataTable
    Public ks As IEnumerable = {"'", ".", ",", "[", "]", "}", "{"}



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If (IsPostBack = False And IsCallback = False) Then
            txtUserID.Text = guser

            LoadGrid()


            drpbranches.Value = "101"
            drp_products.Value = "103"
            dateIssue.Date = Today.Date
            dateexpiry.Date = Today.Date.AddDays(1.0)

            Me.ASPxGridView1.PageIndex = 0
        End If
    End Sub
    Sub LoadGrid()
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

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try


            Dim item As ListItem, i As Integer = 0

            ' For Each item In ASPxListBox1.Items

            For i = 0 To ASPxListBox1.Items.Count
                If Me.CheckBox1.Checked = True Then
                    Dim dks As New DataSet
                    Dim strAdd As String = "Sproc_GetAccountEmail '" & ASPxListBox1.Items(i).Value & "'"

                    Dim str_ToAddress As String = NETClass.Classes.GetField(String.Format("Sproc_GetAccountEmail '{0}'", ASPxListBox1.Items(i).Value))

                    If str_ToAddress = Nothing Then
                        str_ToAddress = "info@resortng.com"
                    End If

                    If str_ToAddress <> Nothing Then


                        Dim EmailServer As String = System.Configuration.ConfigurationManager.ConnectionStrings("EazyEmailServer").ConnectionString
                        Dim str_reppath As String = Server.MapPath("~/Mandate/AccountEstatement.rdlc")
                        'Dim lpath As String = Server.MapPath("~/mandate/")
                        Dim lpath As String = "C:\EmailStatement\"
                        '"~/mandate/"(EmailServer, str_ToAddress, item.Value, str_reppath, lpath)
                        CreateMessageWithAttachmentByAcctRep(EmailServer, str_ToAddress, ASPxListBox1.Items(i).Value, str_reppath, lpath)


                        i = i + 1

                    End If


                Else
                    Dim str_getActive As String = "SELECT accountno FROM  dbo.TransHist WHere postdate between  '" & ((dateIssue.Date.ToShortDateString())) & "' AND '" & (dateexpiry.Date.ToShortDateString()) & "' " &
                    " AND accountno  = '" & ASPxListBox1.Items(i).Value & "' AND reversed = 0 AND trans_code < '731'"

                    If NETClass.Classes.GetDataset(str_getActive).Tables(0).Rows.Count > 0 Then


                        Dim str_ToAddress As String = NETClass.Classes.GetField("Sproc_GetAccountEmail '" & ASPxListBox1.Items(i).Value & "'")


                        If str_ToAddress = Nothing Then
                            str_ToAddress = "info@resortng.com"
                        End If

                        If str_ToAddress <> Nothing Then


                            Dim EmailServer As String = System.Configuration.ConfigurationManager.ConnectionStrings("EazyEmailServer").ConnectionString
                            Dim str_reppath As String = Server.MapPath("~/Mandate/AccountEstatement.rdlc")
                            'Dim lpath As String = Server.MapPath("~/mandate/")
                            ' Dim lpath As String = Server.MapPath("~/mandate/")
                            Dim lpath As String = "C:\EmailStatement\"
                            'CreateMessageWithAttachmentByAcctRep(EmailServer, str_ToAddress, item.Value, str_reppath, lpath)
                            CreateMessageWithAttachmentByAcctRep(EmailServer, str_ToAddress, ASPxListBox1.Items(i).Value, str_reppath, lpath)



                            i = i + 1

                        End If

                    End If

                End If


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
            Response.Write("<script>alert('statements Sucessfully Sent for " & i & "') </script>")

        Catch ex As Exception
            Response.Write("<script>alert('The Following error has occured " & ex.Message & "') </script>")


        End Try
    End Sub


    Public Sub CreateMessageWithAttachmentByAcctRep(ByVal server As String, ByVal toAddress As String, ByVal AccountNo As String, ByVal strserverpath As String, ByVal lpath As String)
        ' Create the Path to the Attachments
        On Error Resume Next

        Dim EmailServer As String = server 'EazyEmailServer
        Dim ds As New DataSet
        'Dim rep, cr As New ReportDocument
        Dim str_path As String = NETClass.Classes.GetPath(1)
        'Response.Write(str_path)
        'rep.Load(str_path & "\" & "AccountEStatement" & ".rpt")
        Dim warnings As Microsoft.Reporting.WebForms.Warning() = Nothing
        Dim streamids As String() = Nothing
        Dim mimeType As String = Nothing
        Dim encoding As String = Nothing
        Dim extension As String = Nothing
        Dim deviceInfo As String
        Dim bytes As Byte()

        Dim lr As New Microsoft.Reporting.WebForms.LocalReport


        lr.ReportPath = strserverpath

        'rep.Load("C:\EazybancNET New Features\EazybancNET Reports" & "\" & "Account Statement" & ".rpt")
        Dim str_getstr As String = String.Format("SELECT sub_name, account_no, seq_no, posted_by, post_date, trans_date, mini_account_no, curr_code,description, accountname, narrative, debit, credit, fcy_debit, fcy_credit, balance, fcy_balance, base_bf, fcy_bf, voucher_no, ref_no, headsub_code, exch_rate, deal_no,trans_code, approving_offr1, status_desc, client_name, narration, reversed,  cast('{0}' as datetime)  as startdate, cast('{1}' as datetime)  as enddate, address, city, prod_desc, prod_type_code, prod_type_desc, prod_code, trans_desc, report_head_code,report_head_desc, post_headsub_code, cheque_no, print_stmt, account_status, old_account_no, item_class, control_account_no  FROM VwTransHist WHere account_no ='{2}' AND startdate >='{3}' AND enddate <= '{4}'  order by post_date, seq_no", ((dateIssue.Date.ToShortDateString())), (dateexpiry.Date.ToShortDateString()), AccountNo, dateIssue.Date.ToShortDateString(), dateexpiry.Date.ToShortDateString())


        Dim mysqldata As New SqlDataSource
        mysqldata.ConnectionString = ConfigurationManager.ConnectionStrings("EazyConnect").ConnectionString
        mysqldata.SelectCommand = str_getstr
        mysqldata.DataBind()

        Dim myDataSource = New ReportDataSource("DataSet2_VwTransHist", mysqldata)
        lr.DataSources.Add(myDataSource)

        bytes = lr.Render("pdf",
                 Nothing, mimeType,
                   encoding, extension, streamids, warnings)
        Dim fileAcctNo As String = String.Format("{0}.pdf", AccountNo)

        'Dim fs As New FileStream(lpath & "Statement.pdf", FileMode.Create)

        Dim fs As New FileStream(lpath & fileAcctNo, FileMode.Create)

        fs.Write(bytes, 0, bytes.Length)
        fs.Close()

        ' Response.Write(str_getstr)
        ' ds = NETClass.Classes.GetDataset("SELECT * FROM VwTransHist WHere account_no ='" & AccountNo & "'")
        'ds = NETClass.Classes.GetDataset(str_getstr)


        'rep.SetDatabaseLogon(NETClass.Classes.GetPath(7), NETClass.Classes.GetPath(8), NETClass.Classes.GetPath(5), NETClass.Classes.GetPath(6))
        'rep.SetDataSource(ds.Tables(0))
        ''cr = rep
        '  Dim lp As CrystalDecisions.CrystalReports.Engine.ReportOptions
        ' rep.Export(lp.EnableSaveSummariesWithReport.TrueString)


        'rep.ExportToDisk(ExportFormatType.PortableDocFormat, "C:\Statement.pdf")

        'rep.ExportToDisk(ExportFormatType.PortableDocFormat, "Statement.pdf")






        'Dim file As String = lpath & "Statement.pdf" '"Statement.pdf"
        Dim file As String = String.Format("{0}.pdf", AccountNo)

        '// Create a message and set up the recipients.
        'Dim message As MailMessage = New MailMessage("eStatement@GTHomes" & EmailServer & ".com", "temmy54@yahoo.com", "Estatement Of Account.:-" & AccountNo, "Pls, See the Your attached Statements of Accounts.")
        ' Dim message As MailMessage = New MailMessage("eStatement@GTHomes", "kazeem.owolabi@gthomes.com", "Estatement Of Account.:-" & AccountNo, "Pls, See the Your attached Statements of Accounts.")
        Dim message As MailMessage = New MailMessage("eStatement@Resortng.com", toAddress, "Estatement Of Account.:-" & AccountNo, "Please, see your attached statements of accounts.")


        ''// Create  the file attachment for this e-mail message.
        'Dim data As Attachment = New Attachment(file, Net.Mime.MediaTypeNames.Application.Octet)
        'Dim disposition As Net.Mime.ContentDisposition = data.ContentDisposition
        'disposition.CreationDate = System.IO.File.GetCreationTime(file)
        'disposition.ModificationDate = System.IO.File.GetLastWriteTime(file)
        'disposition.ReadDate = System.IO.File.GetLastAccessTime(file)
        'message.Attachments.Add(data)
        'Dim client As SmtpClient = New SmtpClient(server)
        '' // Add credentials if the SMTP server requires them.
        'client.Credentials = Net.CredentialCache.DefaultNetworkCredentials
        '' client.Send(message)
        'l_senderlist = l_senderlist + AccountNo & ":(" & toAddress & "),"

        '' // Display the values in the ContentDisposition for the attachment.
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
                                              "  AccountDesc as description, AccountDesc as accountname, UserNarrative as narrative, debit, credit, BalAcctCurr as balance, TransID as ref_no, BranchAdded as headsub_code, AcctExchRate as exch_rate, transCode as trans_code, UserNarrative as narration, 0 as reversed,  cast('{0}' as datetime)  as startdate, cast('{1}' as datetime)  as enddate, dbo.FnCustomerAddress(AccountNo, 0) as address,dbo.FnCustomerCity(AccountNo, 0) as city, dbo.FnAccountProductName(AccountNo) as prod_desc, ProdCode as prod_code, TransDesc as trans_desc  FROM VwTransHist WHere accountno ='{2}' AND PostingDateAdded >='{3}' AND PostingDateAdded <= '{4}' order by PostingDateAdded, seq_no", ((dateIssue.Date.ToShortDateString())), (dateexpiry.Date.ToShortDateString()), AccountNo, dateIssue.Date.ToShortDateString(), dateexpiry.Date.ToShortDateString())



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
        'If ASPxRadioButtonList1.SelectedItem.Value = 1 Then


        '    message = New MailMessage(strSenderAddress, toAddress.Replace("'", ""), GenerateMsg(String.Format("{0}", txtSubject.Text), AccountNo), String.Format("<html><body><img alt='Bulk Message' src='cid:companylogo' {0}  /></body></html>", txtMessage.Text))
        'Else

        '    message = New MailMessage(strSenderAddress, toAddress.Replace("'", ""), GenerateMsg(String.Format("{0}", txtSubject.Text), AccountNo), String.Format("<html><body><img alt='Bulk Message' src='cid:companylogo' {0}  /></body></html>", ASPxHtmlEditor1.Html))

        'End If  
        Dim StrCompanyBrand As String = System.Configuration.ConfigurationManager.ConnectionStrings("CompanyBrand").ConnectionString
        Dim strMsgfrom As Mail.MailAddress = New MailAddress(strSenderAddress, StrCompanyBrand)
        If ASPxRadioButtonList1.SelectedItem.Value = 1 Then





            message = New MailMessage(strSenderAddress, toAddress.Replace("'", ""), String.Format("{0}", txtSubject.Text), String.Format("<html><body><img alt='Bulk Message' src='cid:companylogo' {0}  /></body></html>", GenerateMsg(AccountNo, txtMessage.Text)))

        Else

            message = New MailMessage(strSenderAddress, toAddress.Replace("'", ""), String.Format("{0}", txtSubject.Text), String.Format("<html><body><img alt='Bulk Message' src='cid:companylogo' {0}  /></body></html>", GenerateMsg(AccountNo, ASPxHtmlEditor1.Html)))

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
        'If ASPxRadioButtonList1.SelectedItem.Value = 1 Then
        '    strmsg = String.Format("<html><body><img alt='Logo Message' src='cid:companylogo' /></br>{0}  </body></html>", txtMessage.Text)
        '    htmlView = AlternateView.CreateAlternateViewFromString(GenerateMsg(String.Format("{0}", strmsg), AccountNo), Nothing, "text/html")

        'Else
        '    strmsg = String.Format("<html><body><img alt='Logo Message' src='cid:companylogo'  /></br>{0}</body></html>", ASPxHtmlEditor1.Html)
        '    htmlView = AlternateView.CreateAlternateViewFromString(GenerateMsg(String.Format("{0}", strmsg), AccountNo), Nothing, "text/html")


        'End If
        Dim strtypeofobj As String = "0"

        If ASPxRadioButtonList1.SelectedItem.Value = 1 Then
            strmsg = String.Format("<html><body><img alt='Logo Message' src='cid:companylogo' /></br>{0}  </body></html>", GenerateMsg(AccountNo, txtMessage.Text))
            htmlView = AlternateView.CreateAlternateViewFromString(String.Format("{0}", strmsg), Nothing, "text/html")

        Else
            If ASPxHtmlEditor1.Html.Contains("<img src=") = True Then
                Dim images As String() = GetFullNamesOfImages()

                strtypeofobj = 1
                'strmsg = String.Format("<html><body><img alt='Embeded Message' src='cid:MsgimageLogo'/> </br>{0}</body></html>", ASPxHtmlEditor1.Html)
                strmsg = String.Format("<html><body>{0}</body></html>", GenerateMsg(AccountNo, ASPxHtmlEditor1.Html))

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
                strmsg = String.Format("<html><body><img alt='Logo Message' src='cid:companylogo'/> </br>{0}</body></html>", ASPxHtmlEditor1.Html)
                htmlView = AlternateView.CreateAlternateViewFromString(String.Format("{0}", GenerateMsg(AccountNo, strmsg)), Nothing, "text/html")

            End If



        End If
        'string lpaath = Server.MapPath("~//Images//");
        Dim lpathmagepath As String = lpath.Replace("Reports", "Images")
        Dim strInsertpath As String = lpath.Replace("\Reports\", "/")

        'Dim logo As LinkedResource = New LinkedResource("~/Images/logosmallorg.JPG")
        Dim logo As LinkedResource = New LinkedResource(lpathmagepath & "companylogo.JPG")
        'Dim jlogo As LinkedResourceCollection
        If ASPxHtmlEditor1.Html.Contains("<img src=") = True Then



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
            If Me.CheckBox1.Checked = True Then
                Dim dks As New DataSet
                Dim strAdd As String = String.Format("Sproc_GetAccountEmail '{0}'", ASPxListBox1.Items(i).Value)

                Dim str_ToAddress As String = NETClass.Classes.GetField(String.Format("Sproc_GetAccountEmail '{0}'", ASPxListBox1.Items(i).Value))

                If str_ToAddress = Nothing Then
                    str_ToAddress = "femi.mes@gmail.com"
                End If

                If str_ToAddress <> Nothing Then


                    Dim EmailServer As String = System.Configuration.ConfigurationManager.ConnectionStrings("EazyEmailServer").ConnectionString
                    Dim str_reppath As String = Server.MapPath("~/Reports/AccountEstatement.rdlc")
                    Dim lpath As String = Server.MapPath("~/Reports/")
                    ' Dim lpath As String = "C:\EmailStatement\"
                    '"~/mandate/"(EmailServer, str_ToAddress, item.Value, str_reppath, lpath)
                    CreateMessageWithAttachmentByAcctRepSend(EmailServer, str_ToAddress, ASPxListBox1.Items(i).Value, str_reppath, lpath)


                    j = j + 1

                End If


            Else
                Dim str_getActive As String = String.Format("SELECT accountno FROM  Jubilee.dbo.TransHist WHere postingdateAdded between  '{0}' AND '{1}'  AND accountno  = '{2}'  AND transcode < '731' ", ((dateIssue.Date.ToShortDateString())), (dateexpiry.Date.ToShortDateString()), ASPxListBox1.Items(i).Value)

                If NETClass.Classes.GetDataset(str_getActive).Tables(0).Rows.Count > 0 Then


                    Dim str_ToAddress As String = NETClass.Classes.GetField(String.Format("Sproc_GetAccountEmail '{0}'", ASPxListBox1.Items(i).Value))


                    If str_ToAddress = Nothing Then
                        str_ToAddress = "femi.mes@gmail.com"
                    End If

                    If str_ToAddress <> Nothing Then


                        Dim EmailServer As String = System.Configuration.ConfigurationManager.ConnectionStrings("EazyEmailServer").ConnectionString
                        Dim str_reppath As String = Server.MapPath("~/Reports/AccountEstatement.rdlc")
                        'Dim lpath As String = Server.MapPath("~/mandate/")
                        Dim lpath As String = Server.MapPath("~/Reports/")
                        ' Dim lpath As String = "C:\EmailStatement\"
                        'CreateMessageWithAttachmentByAcctRep(EmailServer, str_ToAddress, item.Value, str_reppath, lpath)
                        CreateMessageWithAttachmentByAcctRepSend(EmailServer, str_ToAddress, ASPxListBox1.Items(i).Value, str_reppath, lpath)



                        j = j + 1

                    End If

                End If

            End If


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
        Response.Write(String.Format("<script>alert('Broadcast Sucessfully Sent for {0}') </script>", j))

        'Catch ex As Exception
        '    Response.Write("<script>alert('The Following error has occured " & ex.Message & "') </script>")


        'End Try
    End Sub

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

    Protected Sub ASPxRadioButtonList1_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ASPxRadioButtonList1.SelectedIndexChanged
        If ASPxRadioButtonList1.SelectedItem.Value = 1 Then
            txtMessage.Visible = True
            ASPxHtmlEditor1.Visible = False

        Else
            txtMessage.Visible = False
            ASPxHtmlEditor1.Visible = True

        End If
    End Sub

    Protected Sub Button6_Click(sender As Object, e As System.EventArgs) Handles Button6.Click
        'Try

        On Error Resume Next
        Dim item As ListItem, i As Integer = 0, j As Integer = 0

        ' For Each item In ASPxListBox1.Items

        'For i = 0 To (ASPxListBox1.Items.Count - 1)

        For Each key As String In ASPxGridView1.GetSelectedFieldValues("AccountNo")
            If (ASPxGridView1.Selection.IsRowSelectedByKey(key)) = True Then
                Dim l_refNo As String = ASPxGridView1.GetRowValuesByKeyValue(key, "AccountNo").ToString()
                If Me.CheckBox1.Checked = True Then
                    Dim dks As New DataSet
                    Dim strgetemailconn As String = ConfigurationManager.ConnectionStrings("EazyConnectSource").ConnectionString
                    Dim strgetemail As String = String.Format("Sproc_GetAccountEmail '{0}'", l_refNo)
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
                        Dim str_reppath As String = Server.MapPath("~/Reports/AccountEstatement.rdlc")
                        Dim lpath As String = Server.MapPath("~/Reports/")
                        ' Dim lpath As String = "C:\EmailStatement\"
                        '"~/mandate/"(EmailServer, str_ToAddress, item.Value, str_reppath, lpath)
                        CreateMessageWithAttachmentByAcctRepSend(EmailServer, str_ToAddress, l_refNo, str_reppath, lpath)


                        j = j + 1

                    End If


                Else
                    '  Dim str_getActive As String = String.Format("SELECT accountno FROM   TransHist WHere postingdateAdded between  '{0}' AND '{1}'  AND accountno  = '{2}'  AND transcode < '731' ", ((dateIssue.Date.ToShortDateString())), (dateexpiry.Date.ToShortDateString()), ASPxListBox1.Items(i).Value)

                    ' If NETClass.Classes.GetDataset(str_getActive).Tables(0).Rows.Count > 0 Then


                    Dim strgetemailconn As String = ConfigurationManager.ConnectionStrings("EazyConnectSource").ConnectionString
                    Dim strgetemail As String = String.Format("Sproc_GetAccountEmail '{0}'", l_refNo)
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
                        Dim str_reppath As String = Server.MapPath("~/Reports/AccountEstatement.rdlc")
                        'Dim lpath As String = Server.MapPath("~/mandate/")
                        Dim lpath As String = Server.MapPath("~/Reports/")
                        ' Dim lpath As String = "C:\EmailStatement\"
                        'CreateMessageWithAttachmentByAcctRep(EmailServer, str_ToAddress, item.Value, str_reppath, lpath)
                        CreateMessageWithAttachmentByAcctRepSend(EmailServer, str_ToAddress, l_refNo, str_reppath, lpath)



                        j = j + 1

                    End If

                    'End If

                End If

            End If
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
        Response.Write(String.Format("<script>alert('Broadcast Sucessfully Sent for {0}') </script>", j))

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

    Protected Sub Button7_Click(sender As Object, e As System.EventArgs) Handles Button7.Click
        Response.Redirect("FrmBulkEmail.aspx")
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

