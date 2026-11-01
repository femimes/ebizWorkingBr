Imports System
Imports System.Collections.Generic
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports NETClass

''' <summary>
''' Standard card request for selected accounts, with editable card details.
''' Lists accounts with GetAllAccountCardRequest (product, branch) and saves each chosen account with Sproc_AddCardRequest
''' under one new batch reference. Plain ASP.NET controls (no DevExpress).
''' </summary>
Partial Class Request_CardRequest
    Inherits System.Web.UI.Page

    Private Const ListProcedure As String = "GetAllAccountCardRequest"
    Private Const AddProcedure As String = "Sproc_AddCardRequest"
    Private Const SendPhoneNumber As Boolean = False
    Private Const FixedDateOfBirth As String = "1970-01-01"

    ''' <summary>One card to request.</summary>
    Private NotInheritable Class CardLine
        Public AccountNo As String = ""
        Public DisplayName As String = ""
        Public Address As String = ""
        Public City As String = ""
        Public Phone As String = ""
        Public Pan As String = ""
        Public DateOfBirth As String = FixedDateOfBirth
    End Class

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadLists()
            txtIssueDate.Text = DateTime.Today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
            BindGrid()
        End If
    End Sub

    ' =====================================================================
    ' Drop-down lists
    ' =====================================================================
    Private Sub LoadLists()
        ddlProduct.Items.Clear()
        For Each r As DataRow In Query(MainDb, "EXEC Sproc_GetAllProductsCards").Rows
            Dim text As String = Col(r, "Product")
            If Col(r, "Type") <> "" Then text &= " (" & Col(r, "Type") & ")"
            ddlProduct.Items.Add(New ListItem(text, Col(r, "Product Code")))
        Next
        ddlBranch.Items.Clear()
        For Each r As DataRow In Query(MainDb, "EXEC Sproc_GetAllBranchesCards").Rows
            ddlBranch.Items.Add(New ListItem(Col(r, "sub_name") & " (" & Col(r, "Headsub_code") & ")", Col(r, "Headsub_code")))
        Next
        SelectValue(ddlProduct, "103")
        SelectValue(ddlBranch, "00")
    End Sub

    Private Shared Sub SelectValue(ByVal list As DropDownList, ByVal value As String)
        Dim item As ListItem = list.Items.FindByValue(value)
        If item IsNot Nothing Then
            list.ClearSelection()
            item.Selected = True
        End If
    End Sub

    ' =====================================================================
    ' Account list
    ' =====================================================================
    Protected Sub Filter_Changed(ByVal sender As Object, ByVal e As EventArgs)
        grid.PageIndex = 0
        BindGrid()
        ShowSuccess("")
    End Sub

    Protected Sub grid_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        grid.PageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Private Function LoadAccounts() As DataView
        Dim table As DataTable = Query(MainDb, "EXEC " & ListProcedure & " @product, @branch",
            P("@product", ddlProduct.SelectedValue), P("@branch", ddlBranch.SelectedValue))
        Dim view As New DataView(table)
        Dim accounts As List(Of String) = AccountList(txtSpecificAccounts.Text)
        If accounts.Count > 0 AndAlso table.Columns.Contains("AccountNo") Then
            Dim quoted As New List(Of String)()
            For Each a As String In accounts
                quoted.Add("'" & a.Replace("'", "''") & "'")
            Next
            view.RowFilter = String.Format("Convert(AccountNo, 'System.String') IN ({0})", String.Join(",", quoted.ToArray()))
        End If
        Return view
    End Function

    Private Sub BindGrid()
        Try
            Dim view As DataView = LoadAccounts()
            grid.DataSource = view
            grid.DataBind()
            litTotal.Text = String.Format("{0:N0} account{1}", view.Count, If(view.Count = 1, "", "s"))
        Catch ex As Exception
            grid.DataSource = Nothing
            grid.DataBind()
            litTotal.Text = ""
            ShowError("The accounts could not be loaded: " & ex.Message)
        End Try
    End Sub

    ' =====================================================================
    ' Submit
    ' =====================================================================
    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As EventArgs)
        If Not IsValid Then Return
        Dim user As String = CurrentUser()

        Dim issueDate, expiryDate As DateTime
        If Not TryDate(txtIssueDate.Text, issueDate) Then ShowError("Enter a valid card issue date.") : Return
        If Not TryDate(txtExpiryDate.Text, expiryDate) Then ShowError("Enter a valid card expiry date.") : Return
        If expiryDate <= issueDate Then ShowError("The expiry date must be after the issue date.") : Return
        If ddlBrand.SelectedValue = "" Then ShowError("Choose the card brand.") : Return

        ' Values typed on the current page win over the stored values
        Dim onPage As New Dictionary(Of String, CardLine)()
        Dim ticked As New List(Of CardLine)()
        For Each row As GridViewRow In grid.Rows
            If row.RowType <> DataControlRowType.DataRow Then Continue For
            Dim line As CardLine = LineFromRow(row)
            onPage(line.AccountNo) = line
            Dim chk As CheckBox = TryCast(row.FindControl("chkSelect"), CheckBox)
            If chk IsNot Nothing AndAlso chk.Checked Then ticked.Add(line)
        Next

        Dim lines As List(Of CardLine)
        If chkAllPages.Checked Then
            lines = New List(Of CardLine)()
            For Each r As DataRowView In LoadAccounts()
                Dim acct As String = Col(r.Row, "AccountNo")
                lines.Add(If(onPage.ContainsKey(acct), onPage(acct), LineFromData(r.Row)))
            Next
        Else
            lines = ticked
        End If
        If lines.Count = 0 Then ShowError("Tick at least one account, or tick ""Include every page"".") : Return

        Dim refNo As String = Guid.NewGuid().ToString("N").Substring(0, 12).ToUpperInvariant()
        Dim saved As Integer = 0
        Dim failed As New List(Of String)()
        For Each line As CardLine In lines
            Try
                Dim ps As New List(Of SqlParameter)()
                ps.Add(P("@RefNo", refNo))
                ps.Add(P("@AccountNo", line.AccountNo))
                ps.Add(P("@Description", line.DisplayName))
                ps.Add(P("@PanNo", line.Pan))
                ps.Add(P("@Address", line.Address))
                ps.Add(P("@City", line.City))
                ps.Add(P("@CountryCode", "NGA"))
                ps.Add(P("@CardBrand", ddlBrand.SelectedValue))
                ps.Add(P("@CardIssueDate", issueDate))
                ps.Add(P("@CardExpiryDate", expiryDate))
                ps.Add(P("@CardCounter", 0))
                ps.Add(P("@Seq", 2))
                ps.Add(P("@RequestTitle", txtTitle.Text.Trim()))
                ps.Add(P("@DOB", line.DateOfBirth))
                ps.Add(P("@AddedBy", user))
                ps.Add(P("@ApprovedBy", "System"))
                If SendPhoneNumber Then ps.Add(P("@CardPhoneNo", line.Phone))
                Exec(AddProcedure, ps.ToArray())
                saved += 1
            Catch ex As Exception
                failed.Add(line.AccountNo & " (" & ex.Message & ")")
            End Try
        Next

        If saved > 0 Then
            Audit("044", String.Format("{0} card request batch {1} ({2}); {3} account(s) requested; issue {4:yyyy-MM-dd}; expiry {5:yyyy-MM-dd}; brand {6}",
                "Standard", refNo, txtTitle.Text.Trim(), saved, issueDate, expiryDate, ddlBrand.SelectedValue), user)
        End If
        If failed.Count = 0 Then
            ShowSuccess(String.Format("Batch {0} submitted: {1} card request{2}.", refNo, saved, If(saved = 1, "", "s")))
            chkAllPages.Checked = False
            BindGrid()
        Else
            ShowSuccess(If(saved > 0, String.Format("Batch {0}: {1} card request(s) saved.", refNo, saved), ""))
            litError.Text = HttpUtility.HtmlEncode(String.Format("{0} account(s) were not saved: {1}", failed.Count, String.Join("; ", failed.ToArray())))
        End If
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect(Request.Path, False)
        Context.ApplicationInstance.CompleteRequest()
    End Sub

    ' =====================================================================
    ' Reading a row (edited values) or a data row (stored values)
    ' =====================================================================
    Private Function LineFromRow(ByVal row As GridViewRow) As CardLine
        Dim line As New CardLine()
        line.AccountNo = Convert.ToString(grid.DataKeys(row.RowIndex)("AccountNo"))
        line.DisplayName = ValueOf(row, "txtName", "hidName")
        line.Address = ValueOf(row, "txtAddress", "hidAddress")
        line.City = ValueOf(row, "txtCity", "hidCity")
        line.Phone = ValueOf(row, "txtPhone", "hidPhone")
        line.Pan = ValueOf(row, "", "hidPan")
        Dim dob As String = ValueOf(row, "txtDob", "")
        Dim d As DateTime
        If TryDate(dob, d) Then line.DateOfBirth = d.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
        Return line
    End Function

    Private Shared Function LineFromData(ByVal r As DataRow) As CardLine
        Dim line As New CardLine()
        line.AccountNo = Col(r, "AccountNo")
        line.DisplayName = Col(r, "AccountName")
        line.Address = Col(r, "Address")
        line.City = Col(r, "city")
        line.Phone = Col(r, "PhoneNo")
        line.Pan = Col(r, "PANno")
        Return line
    End Function

    Private Shared Function ValueOf(ByVal row As GridViewRow, ByVal textBoxId As String, ByVal hiddenId As String) As String
        If textBoxId <> "" Then
            Dim tb As TextBox = TryCast(row.FindControl(textBoxId), TextBox)
            If tb IsNot Nothing Then Return tb.Text.Trim()
        End If
        If hiddenId <> "" Then
            Dim hf As HiddenField = TryCast(row.FindControl(hiddenId), HiddenField)
            If hf IsNot Nothing Then Return hf.Value.Trim()
        End If
        Return ""
    End Function

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
