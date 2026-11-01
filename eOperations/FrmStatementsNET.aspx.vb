Imports System
Imports NETClass
Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports DevExpress.Web
Imports System.Net.Mail

'Imports CrystalDecisions.Shared
'Imports CrystalDecisions.CrystalReports.Engine
Imports System.Drawing.Printing
Imports Microsoft.ReportingServices

'Imports CrystalDecisions
Imports Microsoft.Reporting.WebForms

Partial Class eOperations_FrmStatementsNET
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
                        Dim strSenderAddress As String = ConfigurationManager.ConnectionStrings("EmailSenderAddress").ConnectionString
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
                        Dim str_getActive As String = "SELECT accountno FROM  dbo.TransHist WHere postdate between  '" & ((dateIssue.Date.ToShortDateString())) & "' AND '" & (dateexpiry.Date.ToShortDateString()) & "' " & _
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
            Dim str_getstr As String = String.Format("SELECT sub_name, account_no, seq_no, posted_by, post_date, trans_date, mini_account_no, curr_code,description, accountname, narrative, debit, credit, fcy_debit, fcy_credit, balance, fcy_balance, base_bf, fcy_bf, voucher_no, ref_no, headsub_code, exch_rate, deal_no,trans_code, approving_offr1, status_desc, client_name, narration, reversed,  cast('{0}' as datetime)  as startdate, cast('{1}' as datetime)  as enddate, address, city, prod_desc, prod_type_code, prod_type_desc, prod_code, trans_desc, report_head_code,report_head_desc, post_headsub_code, cheque_no, print_stmt, account_status, old_account_no, item_class, control_account_no  FROM VwTransHist WHere account_no ='{2}' AND startdate >='{3}' AND enddate <= '{4}'  order by post_date, seq_no", ((dateIssue.Date.ToShortDateString())), (dateexpiry.Date.ToShortDateString()), AccountNo, dateIssue.Date.ToShortDateString(), dateexpiry.Date.ToShortDateString())


            Dim mysqldata As New SqlDataSource
            mysqldata.ConnectionString = ConfigurationManager.ConnectionStrings("EazyConnect").ConnectionString
            mysqldata.SelectCommand = str_getstr
            mysqldata.DataBind()

            Dim myDataSource = New ReportDataSource("DataSet2_VwTransHist", mysqldata)
            lr.DataSources.Add(myDataSource)

            bytes = lr.Render("pdf", _
                     Nothing, mimeType, _
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



        Public Sub CreateMessageWithAttachmentByAcctRepSend(ByVal server As String, ByVal toAddress As String, ByVal AccountNo As String, ByVal strserverpath As String, ByVal lpath As String)
            ' Create the Path to the Attachments
            On Error Resume Next

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
            Dim str_getstr As String = String.Format("SELECT BranchName as sub_name, AccountNo as account_no, SeqNo as seq_no, AddedBy as posted_by, PostingDateAdded as post_date, ValueDate as trans_date,ParentAccountNo as mini_account_no, currCode as curr_code, isnull((select top 1 BalAcctCurr FROM TRanshist WHERE AccountNo ='{2}' AND  PostingDateAdded < '{3}' order by PostingDateAdded desc ),0) as 'base_bf', " & _
                                                  "  AccountDesc as description, AccountDesc as accountname, UserNarrative as narrative, debit, credit, BalAcctCurr as balance, TransID as ref_no, BranchAdded as headsub_code, AcctExchRate as exch_rate, transCode as trans_code, UserNarrative as narration, 0 as reversed,  cast('{0}' as datetime)  as startdate, cast('{1}' as datetime)  as enddate, dbo.FnCustomerAddress(AccountNo, 0) as address,dbo.FnCustomerCity(AccountNo, 0) as city, dbo.FnAccountProductName(AccountNo) as prod_desc, ProdCode as prod_code, TransDesc as trans_desc  FROM VwTransHist WHere accountno ='{2}' AND PostingDateAdded >='{3}' AND PostingDateAdded <= '{4}' order by PostingDateAdded, seq_no", ((dateIssue.Date.ToShortDateString())), (dateexpiry.Date.ToShortDateString()), AccountNo, dateIssue.Date.ToShortDateString(), dateexpiry.Date.ToShortDateString())



            Dim mysqldata As New SqlDataSource
            mysqldata.ConnectionString = ConfigurationManager.ConnectionStrings("EazyConnectSource").ConnectionString
            mysqldata.SelectCommand = str_getstr
            mysqldata.DataBind()

            Dim myDataSource = New ReportDataSource("DataSet2_VwTransHist", mysqldata)
            lr.DataSources.Add(myDataSource)

            bytes = lr.Render("pdf", _
                     Nothing, mimeType, _
                       encoding, extension, streamids, warnings)

            Dim fs As New FileStream(lpath & "Statement.pdf", FileMode.Create)
            fs.Write(bytes, 0, bytes.Length)
            fs.Close()






            Dim file As String = lpath & "Statement.pdf" '"Statement.pdf"
            ' Dim file As String = String.Format("'{0}'.pdf", AccountNo)
            Dim strSenderAddress As String = ConfigurationManager.ConnectionStrings("EmailSenderAddress").ConnectionString
            '// Create a message and set up the recipients.
            'Dim message As MailMessage = New MailMessage("eStatement@GTHomes" & EmailServer & ".com", "temmy54@yahoo.com", "Estatement Of Account.:-" & AccountNo, "Pls, See the Your attached Statements of Accounts.")
            ' Dim message As MailMessage = New MailMessage("eStatement@GTHomes", "kazeem.owolabi@gthomes.com", "Estatement Of Account.:-" & AccountNo, "Pls, See the Your attached Statements of Accounts.")
            Dim message As MailMessage = New MailMessage(strSenderAddress, toAddress.Replace("'", ""), "Estatement Of Account.:-" & AccountNo, "<html><body>Please, see your attached statements of Accounts.</br></br> <img alt=' ' src='cid:companylogo' /></body></html>")


            '// Create  the file attachment for this e-mail message.
            Dim data As Attachment = New Attachment(file, Net.Mime.MediaTypeNames.Application.Octet)
            Dim disposition As Net.Mime.ContentDisposition = data.ContentDisposition
            disposition.CreationDate = System.IO.File.GetCreationTime(file)
            disposition.ModificationDate = System.IO.File.GetLastWriteTime(file)
            disposition.ReadDate = System.IO.File.GetLastAccessTime(file)
            message.Attachments.Add(data)

            Dim htmlView As AlternateView = AlternateView.CreateAlternateViewFromString("<img src=cid:companylogo>", Nothing, "text/html")
            Dim lpathmagepath As String = lpath.Replace("Reports", "Images")
            'Dim logo As LinkedResource = New LinkedResource("~/Images/logosmallorg.JPG")
            Dim logo As LinkedResource = New LinkedResource(lpathmagepath & "Logo.JPG")


            logo.ContentId = "companylogo"

            htmlView.LinkedResources.Add(logo)
            'message.AlternateViews.Add(htmlView)
            message.IsBodyHtml = True



            Dim client As SmtpClient = New SmtpClient(server)
            ' // Add credentials if the SMTP server requires them.
            Dim StrUserName As String = System.Configuration.ConfigurationManager.ConnectionStrings("EmailUserName").ConnectionString
            Dim StrPwds As String = System.Configuration.ConfigurationManager.ConnectionStrings("EmailPwds").ConnectionString
            Dim NetworkCredentials As Net.NetworkCredential = New Net.NetworkCredential(StrUserName, StrPwds)

            'client.Credentials = Net.CredentialCache.DefaultNetworkCredentials
            client.Credentials = NetworkCredentials
            'client.Port = "587"
            client.EnableSsl = False
            'client.PickupDirectoryLocation = "C:\EmailStatement\"
            client.DeliveryMethod = SmtpDeliveryMethod.Network



            client.Send(message)




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
            Try
                'On Error Resume Next

                Dim item As ListItem, i As Integer = 0

                ' For Each item In ASPxListBox1.Items

                For i = 0 To (ASPxListBox1.Items.Count - 1)
                    If Me.CheckBox1.Checked = True Then
                        Dim dks As New DataSet
                        Dim strAdd As String = "Sproc_GetAccountEmail '" & ASPxListBox1.Items(i).Value & "'"

                        ' Dim str_ToAddress As String = NETClass.Classes.GetField("Sproc_GetAccountEmail '" & ASPxListBox1.Items(i).Value & "'")

                        Dim strgetemailconn As String = ConfigurationManager.ConnectionStrings("EazyConnectSource").ConnectionString
                        Dim strgetemail As String = String.Format("Sproc_GetAccountEmail '{0}'", ASPxListBox1.Items(i).Value)
                        Dim DsEmail As New DataSet()
                        Dim sqlgetemailcon As SqlClient.SqlConnection = New SqlConnection(strgetemailconn)

                        Dim sqlDataAdapter As SqlClient.SqlDataAdapter = New SqlDataAdapter(strgetemail, sqlgetemailcon)
                        sqlDataAdapter.Fill(DsEmail)
                        Dim str_ToAddress As String = String.Format("'{0}'", DsEmail.Tables(0).Rows(0).Item(0))
                        Dim strSenderAddress As String = ConfigurationManager.ConnectionStrings("EmailSenderAddress").ConnectionString
                        If str_ToAddress = Nothing Then
                            str_ToAddress = strSenderAddress '"femi.mes@gmail.com"
                        End If

                        If str_ToAddress <> Nothing Then


                            Dim EmailServer As String = System.Configuration.ConfigurationManager.ConnectionStrings("EazyEmailServer").ConnectionString
                            Dim str_reppath As String = Server.MapPath("~/Reports/AccountEstatement.rdlc")
                            Dim lpath As String = Server.MapPath("~/Reports/")
                            ' Dim lpath As String = "C:\EmailStatement\"
                            '"~/mandate/"(EmailServer, str_ToAddress, item.Value, str_reppath, lpath)
                            CreateMessageWithAttachmentByAcctRepSend(EmailServer, str_ToAddress, ASPxListBox1.Items(i).Value, str_reppath, lpath)


                            i = i + 1

                        End If


                    Else
                        ' Dim str_getActive As String = String.Format("SELECT accountno FROM  Jubilee.dbo.TransHist WHere postingdateAdded between  '{0}' AND '{1}'  AND accountno  = '{2}'  AND transcode < '731' ", ((dateIssue.Date.ToShortDateString())), (dateexpiry.Date.ToShortDateString()), ASPxListBox1.Items(i).Value)

                        ' If NETClass.Classes.GetDataset(str_getActive).Tables(0).Rows.Count > 0 Then


                        'Dim str_ToAddress As String = NETClass.Classes.GetField("Sproc_GetAccountEmail '" & ASPxListBox1.Items(i).Value & "'")
                        Dim strgetemailconn As String = ConfigurationManager.ConnectionStrings("EazyConnectSource").ConnectionString
                        Dim strgetemail As String = String.Format("Sproc_GetAccountEmail '{0}'", ASPxListBox1.Items(i).Value)
                        Dim DsEmail As New DataSet()
                        Dim sqlgetemailcon As SqlClient.SqlConnection = New SqlConnection(strgetemailconn)

                        Dim sqlDataAdapter As SqlClient.SqlDataAdapter = New SqlDataAdapter(strgetemail, sqlgetemailcon)
                        sqlDataAdapter.Fill(DsEmail)
                        Dim str_ToAddress As String = String.Format("'{0}'", DsEmail.Tables(0).Rows(0).Item(0))
                        Dim strSenderAddress As String = ConfigurationManager.ConnectionStrings("EmailSenderAddress").ConnectionString
                        If str_ToAddress = Nothing Then
                            str_ToAddress = strSenderAddress ' "femi.mes@gmail.com"
                        End If

                        If str_ToAddress <> Nothing Then


                            Dim EmailServer As String = System.Configuration.ConfigurationManager.ConnectionStrings("EazyEmailServer").ConnectionString
                            Dim str_reppath As String = Server.MapPath("~/Reports/AccountEstatement.rdlc")
                            'Dim lpath As String = Server.MapPath("~/mandate/")
                            Dim lpath As String = Server.MapPath("~/Reports/")
                            ' Dim lpath As String = "C:\EmailStatement\"
                            'CreateMessageWithAttachmentByAcctRep(EmailServer, str_ToAddress, item.Value, str_reppath, lpath)
                            CreateMessageWithAttachmentByAcctRepSend(EmailServer, str_ToAddress, ASPxListBox1.Items(i).Value, str_reppath, lpath)



                            i = i + 1

                        End If

                        'End If

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


        Protected Sub Button6_Click(sender As Object, e As System.EventArgs) Handles Button6.Click
            Button5_Click(sender, e)

        End Sub

        Protected Sub Button7_Click(sender As Object, e As System.EventArgs) Handles Button7.Click
            Response.Redirect("FrmStatements.aspx")
        End Sub
    End Class
