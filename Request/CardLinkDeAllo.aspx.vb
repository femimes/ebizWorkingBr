Imports System
Imports System.Collections.Generic
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.IO
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports NETClass

''' <summary>
''' Card deallocation: take allocated cards back from officers.
''' Batches: Sproc_GetCardForExtactInfoInst. Cards in a batch: Sproc_GetAllocatedCardAccountsByRefNo.
''' Plain ASP.NET controls (no DevExpress).
''' </summary>
Partial Class Request_CardLinkDeAllo
    Inherits System.Web.UI.Page

    Private Const BatchProcedure As String = "Sproc_GetCardForExtactInfoInst"
    Private Const BatchStatus As Integer = 15
    Private Const BatchByUser As Boolean = False
    Private Const RowsProcedure As String = "Sproc_GetAllocatedCardAccountsByRefNo"
    Private Const RowsByUser As Boolean = False
    Private Const RowsStatus As Integer = 15

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadBatches()
            BindGrid()
        End If
    End Sub

    ' =====================================================================
    ' Batches and cards
    ' =====================================================================
    Private Sub LoadBatches()
        ddlBatch.Items.Clear()
        ddlBatch.Items.Add(New ListItem("Choose a batch", ""))
        Try
            Dim t As DataTable
            ' named parameters, as the procedures were called before (by name, not by position)
            If BatchByUser Then
                t = Query(MainDb, "EXEC " & BatchProcedure & " @status = @status, @userId = @userId", P("@status", BatchStatus), P("@userId", CurrentUser()))
            Else
                t = Query(MainDb, "EXEC " & BatchProcedure & " @status = @status", P("@status", BatchStatus))
            End If
            For Each r As DataRow In t.Rows
                Dim refNo As String = Col(r, "RefNo")
                If refNo = "" Then refNo = Col(r, "batchRefNo")
                If refNo = "" OrElse ddlBatch.Items.FindByValue(refNo) IsNot Nothing Then Continue For
                Dim title As String = Col(r, "RequestTitle")
                ddlBatch.Items.Add(New ListItem(If(title = "", refNo, refNo & " - " & title), refNo))
            Next
        Catch ex As Exception
            ShowError("The batches could not be loaded: " & ex.Message)
        End Try
        litBatches.Text = String.Format("{0:N0} batch{1} waiting", ddlBatch.Items.Count - 1, If(ddlBatch.Items.Count - 1 = 1, "", "es"))
    End Sub

    Private Function LoadRows() As DataTable
        If ddlBatch.SelectedValue = "" Then Return New DataTable()
        Dim sql As String = "EXEC " & RowsProcedure & " @RefNo = @RefNo"
        Dim args As New List(Of SqlParameter)()
        args.Add(P("@RefNo", ddlBatch.SelectedValue))
        If RowsByUser Then
            sql &= ", @userId = @userId"
            args.Add(P("@userId", CurrentUser()))
        End If
        If RowsStatus >= 0 Then
            sql &= ", @status = @rowStatus"
            args.Add(P("@rowStatus", RowsStatus))
        End If
        Return Query(MainDb, sql, args.ToArray())
    End Function

    Private Sub BindGrid()
        Try
            Dim t As DataTable = LoadRows()
            grid.DataSource = t
            grid.DataBind()
            litTotal.Text = If(ddlBatch.SelectedValue = "", "", String.Format("{0:N0} card{1}", t.Rows.Count, If(t.Rows.Count = 1, "", "s")))
            pnlActions.Visible = t.Rows.Count > 0
        Catch ex As Exception
            grid.DataSource = Nothing
            grid.DataBind()
            pnlActions.Visible = False
            ShowError("The cards could not be loaded: " & ex.Message)
        End Try
    End Sub

    Protected Sub ddlBatch_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        grid.PageIndex = 0
        ShowSuccess("")
        BindGrid()
    End Sub

    Protected Sub grid_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        grid.PageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect(Request.Path, False)
        Context.ApplicationInstance.CompleteRequest()
    End Sub

    ' =====================================================================
    ' Ticked rows, with the stored values of each card
    ' =====================================================================
    Private NotInheritable Class Picked
        Public Row As GridViewRow
        Public Data As DataRow
        Public AccountNo As String = ""
    End Class

    Private Function TickedRows() As List(Of Picked)
        Dim result As New List(Of Picked)()
        Dim byAccount As New Dictionary(Of String, DataRow)(StringComparer.OrdinalIgnoreCase)
        For Each r As DataRow In LoadRows().Rows
            Dim acct As String = Col(r, "accountno")
            If acct <> "" AndAlso Not byAccount.ContainsKey(acct) Then byAccount(acct) = r
        Next
        For Each row As GridViewRow In grid.Rows
            If row.RowType <> DataControlRowType.DataRow Then Continue For
            Dim chk As CheckBox = TryCast(row.FindControl("chkSelect"), CheckBox)
            Dim hid As HiddenField = TryCast(row.FindControl("hidAccount"), HiddenField)
            If chk Is Nothing OrElse hid Is Nothing OrElse Not chk.Checked Then Continue For
            Dim pick As New Picked()
            pick.Row = row
            pick.AccountNo = hid.Value
            If byAccount.ContainsKey(hid.Value) Then pick.Data = byAccount(hid.Value)
            result.Add(pick)
        Next
        Return result
    End Function

    Private Shared Function Counter(ByVal r As DataRow) As Integer
        Dim n As Integer
        Integer.TryParse(Col(r, "CardIssueCounter"), n)
        Return n
    End Function

    Private Shared Function Value(ByVal r As DataRow, ByVal column As String) As Object
        If r Is Nothing OrElse Not r.Table.Columns.Contains(column) Then Return DBNull.Value
        Return r(column)
    End Function

    Private Shared Function RowText(ByVal row As GridViewRow, ByVal id As String) As String
        Dim tb As TextBox = TryCast(row.FindControl(id), TextBox)
        Return If(tb Is Nothing, "", tb.Text.Trim())
    End Function

    ''' <summary>Shows the result of an action on several cards, then refreshes the list.</summary>
    Private Sub Report(ByVal action As String, ByVal done As Integer, ByVal failed As List(Of String))
        If done > 0 Then ShowSuccess(String.Format("{0}: {1} card{2}.", action, done, If(done = 1, "", "s"))) Else ShowSuccess("")
        If failed.Count > 0 Then litError.Text = HttpUtility.HtmlEncode(String.Format("{0} card(s) not processed: {1}", failed.Count, String.Join("; ", failed.ToArray())))
        LoadBatchesKeepSelection()
        BindGrid()
    End Sub

    Private Sub LoadBatchesKeepSelection()
        Dim current As String = ddlBatch.SelectedValue
        LoadBatches()
        Dim item As ListItem = ddlBatch.Items.FindByValue(current)
        If item IsNot Nothing Then
            ddlBatch.ClearSelection()
            item.Selected = True
        End If
    End Sub

    ''' <summary>Name of an account in core banking, or "" if it does not exist.</summary>
    Private Shared Function LinkAccountName(ByVal accountNo As String) As String
        accountNo = If(accountNo, "").Trim()
        If accountNo = "" Then Return ""
        Dim t As DataTable = Query(MainDb, "SELECT ISNULL(EazybankAX.dbo.FnAccountName(@acct), 'Invalid Account')", P("@acct", accountNo))
        Dim name As String = If(t.Rows.Count = 0, "", Convert.ToString(t.Rows(0)(0)))
        Return If(name.StartsWith("Invalid", StringComparison.OrdinalIgnoreCase), "", name)
    End Function

    Private Shared Function AlreadyLinked(ByVal accountNo As String) As Boolean
        Return Query(MainDb, "EXEC SpCheckAccountCardLink @acct", P("@acct", accountNo)).Rows.Count > 0
    End Function

    ''' <summary>Checks the account typed in the "Link account" box of one row.</summary>
    Protected Sub txtLink_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim tb As TextBox = TryCast(sender, TextBox)
        If tb Is Nothing Then Return
        Dim row As GridViewRow = TryCast(tb.NamingContainer, GridViewRow)
        Dim lbl As Label = If(row Is Nothing, Nothing, TryCast(row.FindControl("lblLinkName"), Label))
        If lbl Is Nothing Then Return
        Try
            Dim name As String = LinkAccountName(tb.Text)
            If tb.Text.Trim() = "" Then
                lbl.Text = ""
            ElseIf name = "" Then
                lbl.Text = "Account not found"
                lbl.CssClass = "field-error"
            ElseIf False AndAlso AlreadyLinked(tb.Text.Trim()) Then
                lbl.Text = name & " - already linked to a card"
                lbl.CssClass = "field-error"
            Else
                lbl.Text = name
                lbl.CssClass = "lookup"
            End If
        Catch ex As Exception
            lbl.Text = "Could not check: " & ex.Message
            lbl.CssClass = "field-error"
        End Try
    End Sub

    ' =====================================================================
    ' Actions
    ' =====================================================================
    ''' <summary>Takes the ticked cards back from the officers they were allocated to.</summary>
    Protected Sub btnConfirm_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim user As String = CurrentUser()
        Dim picks As List(Of Picked) = TickedRows()
        If picks.Count = 0 Then ShowError("Tick at least one card first.") : Return
        Dim done As Integer = 0
        Dim failed As New List(Of String)()
        For Each pick As Picked In picks
            If pick.Data Is Nothing Then failed.Add(pick.AccountNo & " (no longer in this batch)") : Continue For
            Try
                Exec("Sproc_UpdateCardRequeststatus", P("@seqNum", Col(pick.Data, "SeqNum")), P("@status", 5), P("@userid", user))
                Exec("spUpdateCardAllocation", P("@SeqNum", Col(pick.Data, "SeqNum")), P("@status", 9))
                done += 1
            Catch ex As Exception
                failed.Add(pick.AccountNo & " (" & ex.Message & ")")
            End Try
        Next
        If done > 0 Then Audit("044", String.Format("Card batch {0}: {1} card(s) deallocated", ddlBatch.SelectedValue, done), user)
        Report("Deallocated", done, failed)
    End Sub

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
