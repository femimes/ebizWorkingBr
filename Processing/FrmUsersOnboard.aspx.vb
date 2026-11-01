Imports System
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports NETClass

Partial Class Processing_FrmUsersOnboard
    Inherits System.Web.UI.Page

    Public luser As String
    Public StrLocation As String
    Public struser As String
    Public guser As String

    ' =====================================================================
    ' Page load
    ' =====================================================================
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            users.Text = Request.QueryString("p")
            If Not String.IsNullOrEmpty(users.Text) AndAlso users.Text.Trim() <> "" Then
                GetRecord()
            Else
                ClearAll()
            End If

            HyperLink1.NavigateUrl = "javascript:labelUserInfo('document.aspnetForm." & users.ClientID & "','document.aspnetForm." & names.ClientID & "');"
        End If
    End Sub

    ' =====================================================================
    ' Load / clear
    ' =====================================================================
    Private Sub ClearAll()
        names.Text = ""
        txtAccountNo.Text = ""
        disabled.Checked = False
        CheckAllEntries.Checked = False
        CheckAudit.Checked = False
        approval.Checked = False
    End Sub

    Private Sub GetRecord()
        Dim mobile As String = users.Text.Trim()
        If mobile = "" Then Exit Sub

        Try
            Dim table As DataTable = Classes.GetDataset(String.Format("Sproc_GetUser '{0}'", Sql(mobile))).Tables(0)

            If table.Rows.Count > 0 Then
                Dim row As DataRow = table.Rows(0)
                names.Text = Convert.ToString(row("user_names"))
                CheckAllEntries.Checked = AsBool(row("can_check_entries"))
                CheckAudit.Checked = AsBool(row("can_audit_entries"))
                approval.Checked = AsBool(row("can_approve_entries"))
                disabled.Checked = AsBool(row("disabled"))

                txtAccountNo.Text = ""
                For Each item As DataRow In Classes.GetDataset(String.Format("Sproc_AppUserAcctMapsSel '{0}'", Sql(mobile))).Tables(0).Rows
                    txtAccountNo.Text = Convert.ToString(item("accountNo"))
                Next

                ShowResult("Existing profile loaded for " & mobile & ".", Nothing)
            Else
                ClearAll()
                ShowResult(Nothing, Nothing)
            End If
        Catch ex As Exception
            ShowResult(Nothing, "The profile could not be loaded: " & ex.Message)
        End Try
    End Sub

    Private Sub users_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles users.TextChanged
        GetRecord()
    End Sub

    Private Sub cmd_go_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_go.Click
        GetRecord()
    End Sub

    Private Sub Button3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button3.Click
        users.Text = ""
        ClearAll()
        ShowResult(Nothing, Nothing)
    End Sub

    ' =====================================================================
    ' Save: create the USSD profile
    ' =====================================================================
    Private Sub Btn_Capture_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_Capture.Click
        If Not Page.IsValid Then Exit Sub

        LoadCurrentUser()

        Dim mobile As String = users.Text.Trim()
        Dim fullNames As String = names.Text.Trim()
        Dim accountNo As String = txtAccountNo.Text.Trim()
        Dim msg As String

        If mobile = "" OrElse fullNames = "" OrElse accountNo = "" Then
            ShowResult(Nothing, "Enter the mobile number, full names and account number.")
            Exit Sub
        End If

        Try
            ' The account must have a policy record before the profile can be created
            Dim policy As New DataSet()
            Using conn As New SqlConnection(Cfg("EazyConnect"))
                Using adapter As New SqlDataAdapter("SpGetAcctPolicy", conn)
                    adapter.SelectCommand.CommandType = CommandType.StoredProcedure
                    adapter.SelectCommand.Parameters.Add(New SqlParameter("@AccountNo", accountNo))
                    adapter.Fill(policy)
                End Using
            End Using

            If policy.Tables.Count = 0 OrElse policy.Tables(0).Rows.Count = 0 Then
                msg = mobile & " USSD User Profile cannot be onboarded, Check account Information"
                Audit("001", msg)
                ShowResult(Nothing, "This profile cannot be onboarded. Check the account number.")
                Exit Sub
            End If

            ' Do not overwrite an existing profile
            Dim existing As DataTable = Classes.GetDataset(String.Format("Sproc_GetUser '{0}'", Sql(mobile))).Tables(0)
            If existing.Rows.Count > 0 Then
                msg = mobile & "  User Profile cannot be onboarded, Profile details already exist"
                Audit("001", msg)
                ShowResult(Nothing, "A profile already exists for " & mobile & ".")
                Exit Sub
            End If

            Classes.RecordOp(String.Format("Sproc_AddUser '{0}','{1}',{2},'{3}',{4},{5},{6}",
                Sql(mobile), Sql(fullNames), Bit(disabled.Checked), Sql(guser),
                Bit(CheckAllEntries.Checked), Bit(CheckAudit.Checked), Bit(approval.Checked)))

            ' Placeholder password, as before; the USSD onboarding procedure sets up access
            Classes.RecordOp(String.Format("Update AppUsers SET Password ='xxxxxxx' WHERE user_id = '{0}'", Sql(mobile)))
            Classes.RecordOp(String.Format("SpCreateUSSDOnbaording '{0}'", Sql(mobile)))

            Using conn As New SqlConnection(Cfg("EazyConnect"))
                Using cmd As New SqlCommand("SpAddAppUserAcctMaps", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add(New SqlParameter("@user_id", mobile))
                    cmd.Parameters.Add(New SqlParameter("@accountNo", accountNo))
                    cmd.Parameters.Add(New SqlParameter("@phoneNo", mobile))
                    cmd.Parameters.Add(New SqlParameter("@cusId", Convert.ToString(policy.Tables(0).Rows(0)("CusID"))))
                    cmd.Parameters.Add(New SqlParameter("@email", ""))
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using

            msg = mobile & " USSD Profile Created or Modified"
            Audit("001", msg)
            ShowResult("USSD profile created for " & mobile & ".", Nothing)

        Catch ex As Exception
            ShowResult(Nothing, "The profile was not saved: " & ex.Message)
        End Try
    End Sub

    ' =====================================================================
    ' Delete
    ' =====================================================================
    Private Sub btn_delete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_delete.Click
        LoadCurrentUser()

        Dim mobile As String = users.Text.Trim()
        If mobile = "" Then
            ShowResult(Nothing, "Enter the mobile number of the profile to delete.")
            Exit Sub
        End If

        Try
            GetRecord()
            Dim accountNo As String = txtAccountNo.Text.Trim()

            Classes.RecordOp(String.Format("Sproc_DeleteUser '{0}'", Sql(mobile)))
            Classes.RecordOp(String.Format("Sproc_DeleteAppUserAcctMaps '{0}','{1}','{2}'", Sql(mobile), Sql(accountNo), Sql(mobile)))

            Audit("002", mobile & " USSD Profile Deletion Requested")

            users.Text = ""
            ClearAll()
            ShowResult("Deletion requested for the USSD profile " & mobile & ".", Nothing)
        Catch ex As Exception
            ShowResult(Nothing, "The profile was not deleted: " & ex.Message)
        End Try
    End Sub

    ' =====================================================================
    ' Membership links (unchanged)
    ' =====================================================================
    Private Sub DataGrid1_CancelCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.CancelCommand
        RedirectWithUser("Frm_UsersRoles.aspx", e.Item.Cells(4).Text)
    End Sub

    Private Sub DataGrid1_DeleteCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.DeleteCommand
        RedirectWithUser("Frm_UsersWorking_sites.aspx", e.Item.Cells(4).Text)
    End Sub

    Private Sub DataGrid1_UpdateCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.UpdateCommand
        RedirectWithUser("Frm_UsersSupervisors.aspx", e.Item.Cells(4).Text)
    End Sub

    Protected Sub Linkbutton131_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        RedirectWithUser("FrmUsersCompanies.aspx", users.Text)
    End Sub

    Protected Sub Linkbutton13_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        RedirectWithUser("FrmUsersMaintTypes.aspx", users.Text)
    End Sub

    Protected Sub Linkbutton30_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        RedirectWithUser("FrmUsersAppMaintTypes.aspx", users.Text)
    End Sub

    Private Sub RedirectWithUser(ByVal page As String, ByVal userId As String)
        Response.Redirect(String.Format("{0}?p={1}&y={2}", page,
            HttpUtility.UrlEncode(userId), HttpUtility.UrlEncode(Request.QueryString("y"))), False)
        Context.ApplicationInstance.CompleteRequest()
    End Sub

    ' =====================================================================
    ' Helpers
    ' =====================================================================
    Private Sub LoadCurrentUser()
        Dim cookie As HttpCookie = Request.Cookies("LogonUserID")
        If cookie IsNot Nothing AndAlso Not String.IsNullOrEmpty(cookie.Value) Then
            luser = cookie.Value
        Else
            luser = If(Context.User.Identity.Name, "")
        End If

        Dim split As Integer = luser.LastIndexOf("?"c)
        If split >= 0 Then
            struser = luser.Substring(0, split)
            StrLocation = luser.Substring(split + 1)
        Else
            struser = luser
            StrLocation = ""
        End If
        guser = struser
    End Sub

    Private Sub Audit(ByVal code As String, ByVal message As String)
        Classes.RecordOp(String.Format("Sproc_AddAuditLog '{0}','{1}','{2}',''", code, Sql(message), Sql(guser)))
    End Sub

    Private Sub ShowResult(ByVal success As String, ByVal errorText As String)
        litResult.Text = If(String.IsNullOrEmpty(success), "", HttpUtility.HtmlEncode(success))
        litError.Text = If(String.IsNullOrEmpty(errorText), "", HttpUtility.HtmlEncode(errorText))
    End Sub

    Private Shared Function AsBool(ByVal value As Object) As Boolean
        If value Is Nothing OrElse value Is DBNull.Value Then Return False
        If TypeOf value Is Boolean Then Return CBool(value)
        Dim s As String = Convert.ToString(value).Trim()
        If s = "1" OrElse s.Equals("true", StringComparison.OrdinalIgnoreCase) Then Return True
        Dim n As Long
        Return Long.TryParse(s, n) AndAlso n <> 0
    End Function

    Private Shared Function Bit(ByVal value As Boolean) As Integer
        Return If(value, 1, 0)
    End Function

    Private Shared Function Cfg(ByVal name As String) As String
        Dim s As ConnectionStringSettings = ConfigurationManager.ConnectionStrings(name)
        Return If(s Is Nothing, "", s.ConnectionString)
    End Function

    Private Shared Function Sql(ByVal value As String) As String
        Return If(value, "").Replace("'", "''")
    End Function
End Class