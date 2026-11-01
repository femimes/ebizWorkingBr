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
Imports System.Collections.Generic
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

''' <summary>
''' Bulk SMS broadcast: sends the message (with account placeholders filled in) to the phone number of each selected account.
''' Accounts: GetAllAccountCardRequest (product, branch, optional account list). Plain ASP.NET controls (no DevExpress);
''' the sending code is the original page's.
''' </summary>
Partial Class Processing_FrmBulkSMS
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

        Dim useSsl As Boolean = Cfg("SMSSSL") = "1"
        Dim smsUserName As String = Cfg("SmsUserName")
        Dim pwds As String = Cfg("SmsPwds")
        Dim displayname As String = Cfg("CompanyBrand")
        Dim sent As Integer = 0, skipped As Integer = 0
        Dim failed As New List(Of String)()

        For Each accountNo As String In accounts
            Try
                Dim phones As DataTable = Query(MainDb, "EXEC SpSMSNoAccountNo @acct", P("@acct", accountNo))
                Dim toPhone As String = If(phones.Rows.Count = 0, "", Col(phones.Rows(0), "msg_to"))
                If toPhone = "" Then skipped += 1 : Continue For

                If CheckBox1.Checked Then
                    ' charged messages only go to accounts with SMS alerts switched on
                    Dim master As DataSet = NETClass.Classes.AXGetDataset(String.Format("SpAcctMasterSelectOne '{0}'", SqlText(accountNo)))
                    Dim enabled As String = If(master.Tables.Count = 0 OrElse master.Tables(0).Rows.Count = 0, "", Col(master.Tables(0).Rows(0), "EnableSMS"))
                    If enabled <> "True" Then skipped += 1 : Continue For
                End If

                Dim message As String = GenerateMsg(accountNo, MessageText())
                Dim response As String
                If useSsl Then
                    response = NETClass.Classes.FnPushSMSSSL(toPhone, smsUserName, pwds, displayname, message, toPhone)
                Else
                    response = NETClass.Classes.FnPushSMS(toPhone, smsUserName, pwds, displayname, message, toPhone)
                End If

                If CheckBox1.Checked Then
                    ' record the message so the charge can be applied
                    NETClass.Classes.RecordOp(String.Format("Sproc_MsgBulkPushsms '{0}','{1}','{2}','{3}'", SqlText(message), SqlText(accountNo), SqlText(toPhone), SqlText(response)))
                End If
                sent += 1
            Catch ex As Exception
                failed.Add(accountNo & ": " & ex.Message)
            End Try
        Next
        Report("Broadcast sent", sent, accounts.Count, skipped, If(CheckBox1.Checked, "(no phone number or SMS alerts not enabled)", "(no phone number)"), failed)
    End Sub

    ' ---- BEGIN unchanged sending code (carried over from the original page) ----
 

        Public l_senderlist As String
        Public dt As New DataTable

     

 

     
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

