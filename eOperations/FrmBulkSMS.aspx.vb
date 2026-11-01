Imports System
Imports NETClass
Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
'Imports DevExpress.Web
'Imports DevExpress.Web.ASPxEditors
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


Partial Class Processing_FrmBulkSMS
    Inherits System.Web.UI.Page

 

    Public luser As String = Context.User.Identity.Name
        Public StrLocation As String = "101" 'luser.Substring(luser.LastIndexOf("?") + 1)
        Public struser As String = "Axadmin" ' Mid(luser, 1, luser.LastIndexOf("?"))
        Public guser As String = struser
        Public l_senderlist As String
        Public dt As New DataTable




        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If (IsPostBack = False And IsCallback = False) Then
            txtUserID.Text = "Axadmin" 'guser

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


 

        Protected Sub cmdClear_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles cmdClear.Command
            ASPxListBox1.Dispose()

            ASPxGridView1.DataBind()
        End Sub

        Protected Sub cmdClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClear.Click
            ASPxListBox1.Items.Clear()
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


        Dim item As ListItem, i As Integer = 0

        ' For Each item In ASPxListBox1.Items

        'For i = 0 To (ASPxListBox1.Items.Count)
        Dim SSLTypeInfo As String = ConfigurationManager.ConnectionStrings("SMSSSL").ConnectionString
        For Each key As String In ASPxGridView1.GetSelectedFieldValues("AccountNo")


            If (ASPxGridView1.Selection.IsRowSelectedByKey(key)) = True Then
                Dim l_refNo As String = ASPxGridView1.GetRowValuesByKeyValue(key, "AccountNo").ToString()
                If Me.CheckBox1.Checked = True Then
                    Dim dks As New DataSet
                    Dim strgetToAddress As String = String.Format("SpSMSNoAccountNo '{0}'", l_refNo)
                    Dim strSMSEnabled As String = "0"
                    Dim strconn As String = ConfigurationManager.ConnectionStrings("EazyConnect").ConnectionString
                    Dim sqlConn As Data.SqlClient.SqlConnection = New SqlClient.SqlConnection(strconn)
                    Dim sqlDataAdapter As Data.SqlClient.SqlDataAdapter = New SqlDataAdapter(strgetToAddress, sqlConn)
                    Dim Ds As New DataSet()
                    sqlDataAdapter.Fill(Ds)
                    Dim smsUserName As String = ConfigurationManager.ConnectionStrings("SmsUserName").ConnectionString
                    Dim pwds As String = ConfigurationManager.ConnectionStrings("SmsPwds").ConnectionString

                    Dim displayname As String = ConfigurationManager.ConnectionStrings("CompanyBrand").ConnectionString
                    Dim strQrySMSstatus As String = String.Format("SpAcctMasterSelectOne '{0}'", l_refNo)
                    Dim str_ToPhoneNo As String = Nothing
                    If Ds.Tables(0).Rows.Count > 0 Then
                        str_ToPhoneNo = Ds.Tables(0).Rows(0)("msg_to").ToString()
                        strSMSEnabled = Classes.AXGetDataset(strQrySMSstatus).Tables(0).Rows(0)("EnableSMS").ToString()
                        If strSMSEnabled = "True" Then
                            Dim strmsg As String = ""
                            If ASPxRadioButtonList1.SelectedItem.Value = 1 Then

                                'strmsg = txtMessage.Text 'GenerateMsg(String.Format("{0}", txtMessage.Text), ASPxListBox1.Items(i).Value)
                                strmsg = GenerateMsg(l_refNo, String.Format("{0}", txtMessage.Text))
                            Else

                                'strmsg = ASPxHtmlEditor1.Html 'GenerateMsg(String.Format("{0}", ASPxHtmlEditor1.Html), ASPxListBox1.Items(i).Value)
                                strmsg = GenerateMsg(l_refNo, String.Format("{0}", ASPxHtmlEditor1.Html))
                            End If
                            Dim strresponse As String = ""

                            If SSLTypeInfo = "1" Then

                                strresponse = NETClass.Classes.FnPushSMSSSL(str_ToPhoneNo, smsUserName, pwds, displayname, strmsg, str_ToPhoneNo)


                            Else
                                strresponse = NETClass.Classes.FnPushSMS(str_ToPhoneNo, smsUserName, pwds, displayname, strmsg, str_ToPhoneNo)

                            End If

                            'insert msgs for charges

                            Dim strpushmsg As String = String.Format("Sproc_MsgBulkPushsms '{0}','{1}','{2}','{3}'", strmsg, l_refNo, str_ToPhoneNo, strresponse)
                            'NETClass.Classes.GetDataset(strpushmsg)
                            NETClass.Classes.RecordOp(strpushmsg)
                        End If

                        i = i + 1
                    End If

                Else
                    '  Dim str_getActive As String = String.Format("SELECT accountno FROM   TransHist WHere postingdateAdded between  '{0}' AND '{1}'  AND accountno  = '{2}'  AND transcode < '731' ", ((dateIssue.Date.ToShortDateString())), (dateexpiry.Date.ToShortDateString()), ASPxListBox1.Items(i).Value)

                    ' If NETClass.Classes.GetDataset(str_getActive).Tables(0).Rows.Count > 0 Then


                    Dim strgetToAddress As String = String.Format("SpSMSNoAccountNo '{0}'", l_refNo)

                    Dim strconn As String = ConfigurationManager.ConnectionStrings("EazyConnect").ConnectionString
                    Dim sqlConn As Data.SqlClient.SqlConnection = New SqlClient.SqlConnection(strconn)
                    Dim sqlDataAdapter As Data.SqlClient.SqlDataAdapter = New SqlDataAdapter(strgetToAddress, sqlConn)
                    Dim Ds As New DataSet()
                    sqlDataAdapter.Fill(Ds)
                    Dim smsUserName As String = ConfigurationManager.ConnectionStrings("SmsUserName").ConnectionString
                    Dim pwds As String = ConfigurationManager.ConnectionStrings("SmsPwds").ConnectionString
                    Dim displayname As String = ConfigurationManager.ConnectionStrings("CompanyBrand").ConnectionString
                    Dim str_ToPhoneNo As String = Nothing
                    If Ds.Tables(0).Rows.Count > 0 Then
                        str_ToPhoneNo = Ds.Tables(0).Rows(0)("msg_to").ToString()
                        Dim strmsg As String = ""
                        If ASPxRadioButtonList1.SelectedItem.Value = 1 Then

                            ' strmsg = txtMessage.Text ' GenerateMsg(String.Format("{0}", txtMessage.Text), ASPxListBox1.Items(i).Value)
                            strmsg = GenerateMsg(l_refNo, String.Format("{0}", txtMessage.Text))


                        Else

                            'strmsg = ASPxHtmlEditor1.Html ' GenerateMsg(String.Format("{0}", ASPxHtmlEditor1.Html), ASPxListBox1.Items(i).Value)
                            strmsg = GenerateMsg(l_refNo, String.Format("{0}", ASPxHtmlEditor1.Html))
                        End If

                        Dim strresponse As String = ""

                        If SSLTypeInfo = "1" Then

                            strresponse = NETClass.Classes.FnPushSMSSSL(str_ToPhoneNo, smsUserName, pwds, displayname, strmsg, str_ToPhoneNo)


                        Else
                            strresponse = NETClass.Classes.FnPushSMS(str_ToPhoneNo, smsUserName, pwds, displayname, strmsg, str_ToPhoneNo)

                        End If
                        ' NETClass.Classes.FnPushSMS(str_ToPhoneNo, smsUserName, pwds, displayname, strmsg, str_ToPhoneNo)
                        i = i + 1
                    End If








                End If



            End If



        Next



        'Next

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
        Response.Write("<script>alert('Broadcast Sucessfully Sent for " & i & "') </script>")

        'Catch ex As Exception
        '    Response.Write("<script>alert('The Following error has occured " & ex.Message & "') </script>")


        'End Try
    End Sub

End Class

