Imports NETClass
Imports System.Data

Partial Class Processing_Frm_Roles
    Inherits System.Web.UI.Page
    Public guser As String = Context.User.Identity.Name



    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        If IsPostBack = False Then
            'l_userId = Request.QueryString("p")
            'If Dbconnect.IAccess(guser, Request.Url.ToString) = False Then

            ClearAll()
            role_id.Text = Request.QueryString("p")
            If role_id.Text <> Nothing Then
                GetRecord()

            End If

            Me.HyperLink1.NavigateUrl = "javascript:labelRolesInfo('document.aspnetForm." + role_id.ClientID.ToString() + "','document.aspnetForm." + role_desc.ClientID.ToString() + "');"

            'Else

            '    Try
            '        If Not Page.Request.UrlReferrer Is Nothing Then

            '            Response.Redirect("../AccessInfo.aspx?p=" & Page.Request.UrlReferrer.ToString() & "")
            '        Else
            '            'strReferrer = Page.Request.UrlReferrer
            '            Dim str As String = Convert.ToString(Page.Request.UrlReferrer)
            '            If Session("RefferPage") Is Nothing Then
            '                Response.Redirect("../Message.aspx")
            '            Else
            '                Response.Redirect("../AccessInfo.aspx?p=" & Page.Request.Url.ToString)

            '            End If


            '        End If
            '    Catch ex As Exception

            '        Response.Write(ex.Message)

            '        '  Response.Write(CType(ViewState("GoBackTo"), String))
            '    End Try

            'End If
            ' Session("RefferPage") = Page.Request.Url.ToString

        End If
    End Sub
    '    Dim msgbox As Messaging.Design.MessageDesigner
    Private Function Validation() As Boolean
        If role_id.Text = Nothing Then
            '    MsgBox(classes.ErrorCode(103), MsgBoxStyle.Information)
            Me.cus_val.IsValid = False
            Me.cus_val.ErrorMessage = classes.ErrorCode(103)

            Return False
        ElseIf role_desc.Text = Nothing Then
            '   MsgBox(classes.ErrorCode(102), MsgBoxStyle.Information)
            Me.cus_val.IsValid = False
            Me.cus_val.ErrorMessage = classes.ErrorCode(102)
            Return False
        Else
            Return True
        End If
    End Function
    Private Sub ClearAll()
        'role_id.Text = Nothing
        role_desc.Text = Nothing
        btn_perm.Enabled = False
    End Sub


    Private Sub GetRecord()
        Dim row As DataRow, table As DataTable, ds As New DataSet()

        If role_id.Text <> Nothing Then
            ds = classes.GetDataset("Sproc_GetRole '" & role_id.Text & "'")
            table = ds.Tables("table")

            If table.Rows.Count > 0 Then
                row = table.Rows(0)

                role_desc.Text = row("role_desc")
                btn_perm.Enabled = True
            Else
                ClearAll()
            End If

            ds.Dispose()
        Else
            btn_perm.Enabled = False
        End If
    End Sub


    Private Sub role_id_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles role_id.TextChanged
        If role_id.Text <> Nothing Then
            role_id.Text = Classes.FormatString(UCase(role_id.Text), 5)
            role_desc.Text = Nothing

            GetRecord()
        End If
    End Sub

    Private Sub btn_delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_delete.Click
        Dim msg As String

        If Me.role_id.Text <> Nothing Then
            classes.RecordOp("Sproc_DeleteRole '" & role_id.Text & "'")

            msg = role_id.Text & " Role Request for Deletion"
            classes.RecordOp("Sproc_AddAuditLog '010','" & msg & "','" & gUser & "',''")

            Response.Redirect("frm_roles.aspx?u=" & Request.QueryString("u") & "&y=" & Request.QueryString("y") & "")

        End If
    End Sub

    Private Sub Btn_Capture_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_Capture.Click
        Dim msg As String

        If Validation() = False Then
            Exit Sub
        End If

        ' If MsgBox(classes.ErrorCode(147), MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
        Classes.RecordOp("Sproc_AddRole '" & role_id.Text & "','" & role_desc.Text & "'")

        msg = role_id.Text & " Role Created or Modified"
        Classes.RecordOp("Sproc_AddAuditLog '009','" & msg & "','" & gUser & "',''")

        Response.Redirect("frm_roles.aspx")
        ' End If
    End Sub

    Private Sub btn_perm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_perm.Click
        Response.Redirect("Frm_RolePerms.aspx?p=" & Me.role_id.Text.Trim & "&y=" & Request.QueryString("y") & "")

    End Sub

    Private Sub cmd_go_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_go.Click
        If role_id.Text <> Nothing Then
            role_id.Text = Classes.FormatString(UCase(role_id.Text), 5)
            GetRecord()
        End If
    End Sub
End Class
