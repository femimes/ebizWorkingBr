Imports NETClass
Imports System.Data
Imports System

Partial Class Processing_Frm_Users
    Inherits System.Web.UI.Page
    Public luser As String
    Public StrLocation As String
    Public struser As String

    Public guser As string  = context.User.Identity.Name 
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            ' If Dbconnect.IAccess(guser, Request.Url.ToString) = False Then
            Me.users.Text = Request.QueryString("p")
            If users.Text.Trim <> Nothing Then
                GetRecord()
            Else
                ClearAll()
            End If

            Me.HyperLink1.NavigateUrl = "javascript:labelUserInfo('document.aspnetForm." + users.ClientID.ToString() + "','document.aspnetForm." + names.ClientID.ToString() + "');"



            'Else

            '    Try
            '        If Not Page.Request.UrlReferrer Is Nothing Then

            '            Response.Redirect("../AccessInfo.aspx?p=" & Page.Request.UrlReferrer.ToString() & "")
            '        Else

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
            '' Session("RefferPage") = Page.Request.Url.ToString

        End If
    End Sub
    Private Sub ClearAll()
        ' users.Text = Nothing
        names.Text = Nothing

        'minrec.Text = Format(0, "#,##0.00")
        'maxrec.Text = Format(0, "#,##0.00")
        'minpay.Text = Format(0, "#,##0.00")
        'maxpay.Text = Format(0, "#,##0.00")
        'minnoncash.Text = Format(0, "#,##0.00")
        'maxnoncash.Text = Format(0, "#,##0.00")
        'ceilrec.Text = Format(0, "#,##0.00")
        'ceilpay.Text = Format(0, "#,##0.00")
        'ceilnoncash.Text = Format(0, "#,##0.00")

        'tellerAcct.Text = Nothing

        'minrec1.Text = Format(0, "#,##0.00")
        'maxrec1.Text = Format(0, "#,##0.00")
        'minpay1.Text = Format(0, "#,##0.00")
        'maxpay1.Text = Format(0, "#,##0.00")
        'minnoncash1.Text = Format(0, "#,##0.00")
        'maxnoncash1.Text = Format(0, "#,##0.00")
        ''minod.Text = Format(0, "#,##0.00")
        'maxod.Text = Format(0, "#,##0.00")

        disabled.Checked = False
        Me.CheckAllEntries.Checked = False
        Me.CheckAudit.Checked = False
        Me.approval.Checked = False

        'Button4.Enabled = False
        'Button6.Enabled = False

    End Sub
       Private Function Validation() As Boolean
        If users.Text = Nothing Then
            'MsgBox(classes.ErrorCode(109), MsgBoxStyle.Information)

            Return False
        ElseIf names.Text = Nothing Then
            'MsgBox(classes.ErrorCode(110), MsgBoxStyle.Information)

            Return False
        Else
            Return True
        End If
    End Function

    Private Sub Btn_Capture_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_Capture.Click
        Dim msg As String
        Dim strauthUser As String, strgetauthcookies As String

        strgetauthcookies = Request.Cookies("LogonUserID").Value
        If strgetauthcookies <> Nothing Then

            guser = strgetauthcookies.Substring(0, strgetauthcookies.LastIndexOf("?"))
        Else
            luser = Context.User.Identity.Name
            StrLocation = luser.Substring(luser.LastIndexOf("?") + 1)
            struser = luser.Substring(0, luser.LastIndexOf("?")) ' Mid(luser, 1, luser.LastIndexOf("?"))
            guser = struser


        End If

        If Validation() = False Then
            Exit Sub
        End If
        'classes.RecordOp("Sproc_AddUser '" & users.Text & "','" & names.Text & "'," & CDec(minrec.Text) & "," & CDec(maxrec.Text) & "," & _
        '                            "" & CDec(minpay.Text) & "," & CDec(maxpay.Text) & "," & CDec(minnoncash.Text) & "," & CDec(maxnoncash.Text) & "," & _
        '                            "" & CDec(ceilrec.Text) & "," & CDec(ceilpay.Text) & "," & CDec(ceilnoncash.Text) & ",'" & tellerAcct.Text & "'," & _
        '                           "" & CDec(minrec1.Text) & "," & CDec(maxrec1.Text) & "," & CDec(minpay1.Text) & "," & CDec(maxpay1.Text) & "," & _
        '                           "" & CDec(minnoncash1.Text) & "," & CDec(maxnoncash1.Text) & "," & CDec(minod.Text) & "," & CDec(maxod.Text) & "," & _
        '                         "" & CInt(disabled.Checked) & ",'" & gUser.Name & "'")

        Dim l_disability As Integer
        If disabled.Checked = True Then
            l_disability = 1
        Else
            l_disability = 0
        End If
        Dim Str_sel As String = "Sproc_AddUser '" & users.Text.Trim & "','" & names.Text.Trim & "'," & CInt(disabled.Checked) & ",'" & gUser & "'," & _
                                                  " " & CInt(CheckAllEntries.Checked) & "," & CInt(Me.CheckAudit.Checked) & "," & CInt(approval.Checked) & ""
        'Response.Write(Str_sel)
        classes.RecordOp("Sproc_AddUser '" & users.Text.Trim & "','" & names.Text.Trim & "'," & CInt(disabled.Checked) & ",'" & gUser & "'," & _
                                         " " & CInt(CheckAllEntries.Checked) & "," & CInt(Me.CheckAudit.Checked) & "," & CInt(approval.Checked) & "")

        'classes.RecordOp("Sproc_AddUser '" & users.Text.Trim & "','" & names.Text.Trim & "'," & CInt(disabled.Checked) & ",'" & gUser & "'," & _
        '                                        " " & CInt(CheckAllEntries.Checked) & "," & CInt(CheckAudit.Checked) & "," & CInt(approval.Checked) & "")



        'classes.RecordOp("Sproc_AddUser '" & users.Text & "','" & names.Text & "'," & CDec(minrec.Text) & "," & CDec(maxrec.Text) & "," & _
        '                            "" & CDec(minpay.Text) & "," & CDec(maxpay.Text) & "," & _
        '                           "" & CDec(minnoncash.Text) & "," & CDec(maxnoncash.Text) & "," & CDec(minrec1.Text) & "," & CDec(maxrec1.Text) & "," & _
        '                            "" & CDec(minpay1.Text) & "," & CDec(maxpay1.Text) & "," & _
        '                           "" & CDec(minnoncash1.Text) & "," & CDec(maxnoncash1.Text) & ", " & CInt(disabled.Checked) & ",'" & gUser & "'," & CInt(Me.CheckAllEntries.Checked) & "," & CInt(Me.CheckAudit.Checked) & "," & CInt(Me.approval.Checked) & "")

        ' Section manupulate passwords
        Dim strchangepwds As String = String.Format("Update AppUsers SET Password ='xxxx' WHERE user_id = '{0}'", users.Text.Trim)
        Classes.RecordOp(strchangepwds)


        'End manupulate
        msg = users.Text & " Profile Created or Modified"
        Classes.alertwindow(Me.Page, msg, "Halt")
        classes.RecordOp("Sproc_AddAuditLog '001','" & msg & "','" & gUser & "',''")
        'Populate the grid from database
        Dim row As DataRow, table As DataTable, ds As New DataSet

        ds = classes.GetDataset("Sproc_GetUser '" & users.Text & "'")
        table = ds.Tables("table")
        Me.DataGrid1.DataSource = table
        Me.DataGrid1.DataBind()

        'ClearAll()


    End Sub







    Private Sub GetRecord()
        On Error Resume Next

        Dim row As DataRow, table As DataTable, ds As New DataSet

        If users.Text <> Nothing Then
            ds = Classes.GetDataset("Sproc_GetUserBizOnly '" & users.Text & "'")
            table = ds.Tables("table")

            If table.Rows.Count > 0 Then
                row = table.Rows(0)

                names.Text = row("user_names")
                'ceilrec.Text = Format(row("ceiling_cash_rec"), "#,##0.00")
                'ceilpay.Text = Format(row("ceiling_cash_pay"), "#,##0.00")
                'ceilnoncash.Text = Format(row("ceiling_non_cash"), "#,##0.00")

                '   tellerAcct.Text = row("teller_account_no")

                Me.CheckAllEntries.Checked = CBool(row("can_check_entries"))
                Me.CheckAudit.Checked = CBool(row("can_audit_entries"))
                Me.approval.Checked = CBool(row("can_approve_entries"))

                'minod.Text = Format(row("min_od_appr"), "#,##0.00")
                'maxod.Text = Format(row("max_od_appr"), "#,##0.00")
                disabled.Checked = row("disabled")
                Me.DataGrid1.DataSource = table
                Me.DataGrid1.DataBind()


            Else
                ClearAll()
                Me.DataGrid1.Dispose()
                Me.DataGrid1.DataBind()


            End If

            ds.Dispose()
        Else

        End If
    End Sub




    Private Sub users_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles users.TextChanged

        GetRecord()
    End Sub



    Private Sub btn_delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_delete.Click
        Dim msg As String

        Dim str_sel As String, strauthUser As String, strgetauthcookies As String

        strgetauthcookies = Request.Cookies("LogonUserID").Value
        If strgetauthcookies <> Nothing Then

            guser = strgetauthcookies.Substring(0, strgetauthcookies.LastIndexOf("?"))
        Else
            luser = Context.User.Identity.Name
            StrLocation = luser.Substring(luser.LastIndexOf("?") + 1)
            struser = luser.Substring(0, luser.LastIndexOf("?")) ' Mid(luser, 1, luser.LastIndexOf("?"))
            guser = struser


        End If
        If Me.users.Text = Nothing Then
            Me.cus_val.IsValid = False
            Me.cus_val.ErrorMessage = classes.ErrorCode(109)
            cus_val.Text = Classes.ErrorCode(109)

            Classes.alertwindow(Me.Page, Classes.ErrorCode(109), "Halt")
        Else

            Classes.RecordOp(String.Format("Sproc_DeleteUser '{0}'", users.Text))

            msg = users.Text & " Profile Deletion Requested"
            Classes.alertwindow(Me.Page, msg, "Halt")

            Classes.RecordOp(String.Format("Sproc_AddAuditLog '002','{0}','{1}',''", msg, guser))
            Response.Redirect(String.Format("frm_users.aspx?u={0}&y={1}", Request.QueryString("u"), Request.QueryString("y")))

        End If
    End Sub

    Private Sub DataGrid1_CancelCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.CancelCommand

        Me.DataGrid1.Columns(4).Visible = True
        Dim str_id As String = e.Item.Cells(4).Text
        Response.Redirect(String.Format("Frm_UsersRoles.aspx?p={0}&y={1}", str_id, Request.QueryString("y")))
        'me.DataGrid1.Columns(3).Visible = True 
    End Sub


    Private Sub DataGrid1_DeleteCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.DeleteCommand
        Me.DataGrid1.Columns(4).Visible = True
        Dim str_id As String = e.Item.Cells(4).Text
        Response.Redirect(String.Format("Frm_UsersWorking_sites.aspx?p={0}&y={1}", str_id, Request.QueryString("y")))

    End Sub


    Private Sub cmd_go_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_go.Click
        GetRecord()
    End Sub

    Private Sub DataGrid1_UpdateCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.UpdateCommand
        Me.DataGrid1.Columns(4).Visible = True
        Dim str_id As String = e.Item.Cells(4).Text
        Response.Redirect(String.Format("Frm_UsersSupervisors.aspx?p={0}&y={1}", str_id, Request.QueryString("y")))
    End Sub

    Protected Sub Linkbutton131_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.DataGrid1.Columns(4).Visible = True

        Dim str_id As String = users.Text
        Response.Redirect(String.Format("FrmUsersCompanies.aspx?p={0}&y={1}", str_id, Request.QueryString("y")))
    End Sub

    Protected Sub Linkbutton13_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim str_id As String = users.Text
        Response.Redirect(String.Format("FrmUsersMaintTypes.aspx?p={0}&y={1}", str_id, Request.QueryString("y")))
    End Sub

    Protected Sub Linkbutton30_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim str_id As String = users.Text
        Response.Redirect(String.Format("FrmUsersAppMaintTypes.aspx?p={0}&y={1}", str_id, Request.QueryString("y")))
    End Sub
End Class

