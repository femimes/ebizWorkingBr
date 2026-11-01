Imports System.Web.Security
Imports System.Web.Security.FormsIdentity
Imports System.Security.Principal
Imports System.IO
Imports System.Data.SqlClient
Imports System.Configuration
Imports Microsoft.VisualBasic
Imports NETClass
Imports System.Data

Partial Class Processing_FrmUsersOnly
    Inherits System.Web.UI.Page
    Public gUtils As New NETClass.Classes()
    Public gEazy As New NETClass.EazyClass()


    Public gUser As String
    Public luser As String
    Public StrLocation As String
    Public struser As String
    Private strReferrer As Uri
    Public Trans As String
    Public cBankCode As String

    Sub LoadUserDetails()
        luser = Context.User.Identity.Name
        StrLocation = luser.Substring(luser.LastIndexOf("?") + 1)
        struser = Mid(luser, 1, luser.LastIndexOf("?"))
        gUser = struser
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        If IsPostBack = False Then
            'luser = Context.User.Identity.Name
            'StrLocation = luser.Substring(luser.LastIndexOf("?") + 1)
            'struser = Mid(luser, 1, luser.LastIndexOf("?"))
            'gUser = struser
            'If Dbconnect.IAccess(gUser, Request.Url.ToString) = False Then


            Me.users.Text = Request.QueryString("p")
                If users.Text.Trim <> Nothing Then
                    GetRecord()
                Else
                    ClearAll()
                End If
                Me.hypusers.NavigateUrl = "javascript:labelUserInfo('document.aspnetForm." + users.ClientID.ToString() + "','document.aspnetForm." + names.ClientID.ToString() + "');"
                Me.hypTeller.NavigateUrl = "javascript:lblUserAccount('document.aspnetForm." + tellerAcct.ClientID.ToString() + "');"

            'Else

            '    Try
            '        If Not Page.Request.UrlReferrer Is Nothing Then

            '            Response.Redirect("../AccessInfo.aspx?p=" & Page.Request.UrlReferrer.ToString() & "")
            '        Else
            '            strReferrer = Page.Request.UrlReferrer
            '            Dim str As String = Convert.ToString(strReferrer)
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
        End If
    End Sub
    Private Sub ClearAll()
        ' users.Text = Nothing
        names.Text = Nothing

        minrec.Text = Format(0, "#,##0.00")
        maxrec.Text = Format(0, "#,##0.00")
        minpay.Text = Format(0, "#,##0.00")
        maxpay.Text = Format(0, "#,##0.00")
        minnoncash.Text = Format(0, "#,##0.00")
        maxnoncash.Text = Format(0, "#,##0.00")
        ceilrec.Text = Format(0, "#,##0.00")
        ceilpay.Text = Format(0, "#,##0.00")
        ceilnoncash.Text = Format(0, "#,##0.00")

        tellerAcct.Text = Nothing

        minrec1.Text = Format(0, "#,##0.00")
        maxrec1.Text = Format(0, "#,##0.00")
        minpay1.Text = Format(0, "#,##0.00")
        maxpay1.Text = Format(0, "#,##0.00")
        minnoncash1.Text = Format(0, "#,##0.00")
        maxnoncash1.Text = Format(0, "#,##0.00")
        minod.Text = Format(0, "#,##0.00")
        maxod.Text = Format(0, "#,##0.00")

        disabled.Checked = False
        post_ib_gl.Checked = False
        viewprotected.Checked = False

        printstmt.Checked = False

        disabled.Checked = False

        'Button4.Enabled = False
        'Button6.Enabled = False

    End Sub

    Private Function Validation() As Boolean
        If users.Text = Nothing Then
            'MsgBox(gUtils.ErrorCode(109), MsgBoxStyle.Information)

            Return False
        ElseIf names.Text = Nothing Then
            'MsgBox(gUtils.ErrorCode(110), MsgBoxStyle.Information)

            Return False
        ElseIf CDec(ceilrec.Text) < CDec(maxrec.Text) Or CDec(ceilpay.Text) < CDec(maxpay.Text) Or CDec(ceilnoncash.Text) < CDec(maxnoncash.Text) Then
            ' MsgBox(gUtils.ErrorCode(247), MsgBoxStyle.Information)
            Me.cus_val.IsValid = False
            Me.cus_val.ErrorMessage = gUtils.ErrorCode(247)

            Return False
        Else
            Return True
        End If
    End Function
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        '    Dim msgbox As Messaging.Design.MessageDesigner


    End Sub

    Private Sub Btn_Capture_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_Capture.Click
        Dim msg As String

        luser = Context.User.Identity.Name
        StrLocation = luser.Substring(luser.LastIndexOf("?") + 1)
        struser = Mid(luser, 1, luser.LastIndexOf("?"))
        gUser = struser
        If Validation() = False Then
            Exit Sub
        End If
        'gUtils.RecordOp("Sproc_AddUser '" & users.Text & "','" & names.Text & "'," & CDec(minrec.Text) & "," & CDec(maxrec.Text) & "," & _
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

        Dim str_val As String = "Sproc_AddUser '" & users.Text & "','" & names.Text & "','" & CDec(minrec.Text) & "','" & CDec(maxrec.Text) & "',"
        str_val = str_val + " '" & CDec(minpay.Text) & "','" & CDec(maxpay.Text) & "','" & CDec(minnoncash.Text) & "','" & CDec(maxnoncash.Text) & "',"
        str_val = str_val + "'" & CDec(ceilrec.Text) & "','" & CDec(ceilpay.Text) & "','" & CDec(ceilnoncash.Text) & "','" & tellerAcct.Text & "',"
        str_val = str_val + "'" & CDec(minrec1.Text) & "','" & CDec(maxrec1.Text) & "','" & CDec(minpay1.Text) & "','" & CDec(maxpay1.Text) & "',"
        str_val = str_val + "'" & CDec(minnoncash1.Text) & "','" & CDec(maxnoncash1.Text) & "','" & CDec(minod.Text) & "','" & CDec(maxod.Text) & "',"
        str_val = str_val + "" & CInt(disabled.Checked) & ",'" & gUser & "'," & CInt(post_ib_gl.Checked) & "," & CInt(viewprotected.Checked) & "," & CInt(printstmt.Checked) & ""




        Classes.alertwindow(Me.Page, "Request Sucessfully Executed For User ID " & gUser, "Halt")



        ' Me.tellerAcct.Text = str_val
        ' Dbconnect.FxSql(str_val, Dbconnect.Myconnect)
        gUtils.RecordOp(str_val)
        msg = users.Text & " Profile Created or Modified"
        gUtils.RecordOp("Sproc_AddAuditLog '001','" & msg & "','" & gUser & "',''")
        'Populate the grid from database
        Dim row As DataRow, table As DataTable, ds As New DataSet()

        ds = gUtils.GetDataset("Sproc_GetUser '" & users.Text.Trim & "'")
        table = ds.Tables("table")
        Me.DataGrid1.DataSource = table
        Me.DataGrid1.DataBind()

        'ClearAll()


    End Sub

    Private Sub ceilrec_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ceilrec.TextChanged
        Try
            If ceilrec.Text = Nothing Then
                ceilrec.Text = Format(0, "#,##0.00")
            Else
                ceilrec.Text = Format(gUtils.FullAmount(ceilrec.Text), "#,##0.00")
            End If
        Catch ex As Exception

            'Finally

            '    ceilrec.Text = Format(0, "#,##0.00")
        End Try
    End Sub

    Private Sub minrec_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles minrec.TextChanged
        Try
            If minrec.Text = Nothing Then
                minrec.Text = Format(0, "#,##0.00")
            Else
                minrec.Text = Format(gUtils.FullAmount(minrec.Text), "#,##0.00")
            End If
        Catch ex As Exception

            'Finally

            '    minrec.Text = Format(0, "#,##0.00")
        End Try
    End Sub

    Private Sub maxrec_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles maxrec.TextChanged
        Try
            If maxrec.Text = Nothing Then
                maxrec.Text = Format(0, "#,##0.00")
            Else
                maxrec.Text = Format(gUtils.FullAmount(maxrec.Text), "#,##0.00")
            End If
        Catch ex As Exception

            'Finally

            '    maxrec.Text = Format(0, "#,##0.00")
        End Try
    End Sub

    Private Sub minpay_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles minpay.TextChanged
        Try
            If minpay.Text = Nothing Then
                minpay.Text = Format(0, "#,##0.00")
            Else
                minpay.Text = Format(gUtils.FullAmount(minpay.Text), "#,##0.00")
            End If
        Catch ex As Exception

            'Finally

            '    minpay.Text = Format(0, "#,##0.00")
        End Try
    End Sub

    Private Sub maxpay_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles maxpay.TextChanged
        Try
            If maxpay.Text = Nothing Then
                maxpay.Text = Format(0, "#,##0.00")
            Else
                maxpay.Text = Format(gUtils.FullAmount(maxpay.Text), "#,##0.00")
            End If
        Catch ex As Exception

            'Finally

            '    maxpay.Text = Format(0, "#,##0.00")
        End Try
    End Sub

    Private Sub ceilpay_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ceilnoncash.TextChanged, ceilpay.TextChanged
        Try
            If ceilpay.Text = Nothing Then
                ceilpay.Text = Format(0, "#,##0.00")
            Else
                ceilpay.Text = Format(gUtils.FullAmount(ceilpay.Text), "#,##0.00")
            End If
        Catch ex As Exception

            'Finally

            '    ceilpay.Text = Format(0, "#,##0.00")
        End Try
    End Sub

    Private Sub minnoncash_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles minnoncash.TextChanged
        Try
            If minnoncash.Text = Nothing Then
                minnoncash.Text = Format(0, "#,##0.00")
            Else
                minnoncash.Text = Format(gUtils.FullAmount(minnoncash.Text), "#,##0.00")
            End If
        Catch ex As Exception

            'Finally

            '    minnoncash.Text = Format(0, "#,##0.00")
        End Try
    End Sub

    Private Sub maxnoncash_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles maxnoncash.TextChanged
        Try
            If maxnoncash.Text = Nothing Then
                maxnoncash.Text = Format(0, "#,##0.00")
            Else
                maxnoncash.Text = Format(gUtils.FullAmount(maxnoncash.Text), "#,##0.00")
            End If
        Catch ex As Exception

            'Finally

            '    maxnoncash.Text = Format(0, "#,##0.00")
        End Try
    End Sub

    Private Sub ceilnoncash_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ceilnoncash.TextChanged
        Try
            If ceilnoncash.Text = Nothing Then
                ceilnoncash.Text = Format(0, "#,##0.00")
            Else
                ceilnoncash.Text = Format(gUtils.FullAmount(ceilnoncash.Text), "#,##0.00")
            End If
        Catch ex As Exception

            'Finally

            'ceilnoncash.Text = Format(0, "#,##0.00")
        End Try
    End Sub

    Private Sub minrec1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles minrec1.TextChanged
        Try
            If minrec1.Text = Nothing Then
                minrec1.Text = Format(0, "#,##0.00")
            Else
                minrec1.Text = Format(gUtils.FullAmount(minrec1.Text), "#,##0.00")
            End If
        Catch ex As Exception

            'Finally

            '    minrec1.Text = Format(0, "#,##0.00")
        End Try
    End Sub

    Private Sub minpay1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles minpay1.TextChanged
        Try
            If minpay1.Text = Nothing Then
                minpay1.Text = Format(0, "#,##0.00")
            Else
                minpay1.Text = Format(gUtils.FullAmount(minpay1.Text), "#,##0.00")
            End If
        Catch ex As Exception

            'Finally

            '    minpay1.Text = Format(0, "#,##0.00")
        End Try
    End Sub

    Private Sub minnoncash1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles minnoncash1.TextChanged
        Try
            If minnoncash1.Text = Nothing Then
                minnoncash1.Text = Format(0, "#,##0.00")
            Else
                minnoncash1.Text = Format(gUtils.FullAmount(minnoncash1.Text), "#,##0.00")
            End If
        Catch ex As Exception

            'Finally

            '    Me.minnoncash1.Text = Format(0, "#,##0.00")
        End Try
    End Sub

    Private Sub minod_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles minod.TextChanged
        Try
            If minod.Text = Nothing Then
                minod.Text = Format(0, "#,##0.00")
            Else
                minod.Text = Format(gUtils.FullAmount(minod.Text), "#,##0.00")
            End If
        Catch ex As Exception

            'Finally

            ''    minod.Text = Format(0, "#,##0.00")
        End Try
    End Sub

    Private Sub maxrec1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles maxrec1.TextChanged
        Try
            If maxrec1.Text = Nothing Then
                maxrec1.Text = Format(0, "#,##0.00")
            Else
                maxrec1.Text = Format(gUtils.FullAmount(maxrec1.Text), "#,##0.00")
            End If
        Catch ex As Exception

            'Finally

            '    maxrec1.Text = Format(0, "#,##0.00")
        End Try
    End Sub

    Private Sub maxpay1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles maxpay1.TextChanged

        Try
            If maxpay1.Text = Nothing Then
                maxpay1.Text = Format(0, "#,##0.00")
            Else
                maxpay1.Text = Format(gUtils.FullAmount(maxpay1.Text), "#,##0.00")
            End If
        Catch ex As Exception

            'Finally

            '    maxpay1.Text = Format(0, "#,##0.00")
        End Try
    End Sub

    Private Sub maxnoncash1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles maxnoncash1.TextChanged
        Try
            If maxnoncash1.Text = Nothing Then
                maxnoncash1.Text = Format(0, "#,##0.00")
            Else
                maxnoncash1.Text = Format(gUtils.FullAmount(maxnoncash1.Text), "#,##0.00")
            End If
        Catch ex As Exception

            'Finally

            '    maxnoncash1.Text = Format(0, "#,##0.00")
        End Try

    End Sub

    Private Sub maxod_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles maxod.TextChanged
        Try
            If maxod.Text = Nothing Then
                maxod.Text = Format(0, "#,##0.00")
            Else
                maxod.Text = Format(gUtils.FullAmount(maxod.Text), "#,##0.00")
            End If
        Catch ex As Exception

            'Finally

            '    maxod.Text = Format(0, "#,##0.00")
        End Try
    End Sub

    Private Sub GetRecord()
        On Error Resume Next

        Dim row As DataRow, table As DataTable, ds As New DataSet

        If users.Text <> Nothing Then
            ds = gUtils.GetDataset("Sproc_GetUser '" & users.Text & "'")
            table = ds.Tables("table")

            If table.Rows.Count > 0 Then
                row = table.Rows(0)

                names.Text = row("user_names")
                minrec.Text = Format(row("min_cash_rec"), "#,##0.00")
                maxrec.Text = Format(row("max_cash_rec"), "#,##0.00")
                minpay.Text = Format(row("min_cash_pay"), "#,##0.00")
                maxpay.Text = Format(row("max_cash_pay"), "#,##0.00")
                minnoncash.Text = Format(row("min_non_cash"), "#,##0.00")
                maxnoncash.Text = Format(row("max_non_cash"), "#,##0.00")


                ceilrec.Text = Format(row("ceiling_cash_rec"), "#,##0.00")
                ceilpay.Text = Format(row("ceiling_cash_pay"), "#,##0.00")
                ceilnoncash.Text = Format(row("ceiling_non_cash"), "#,##0.00")

                tellerAcct.Text = row("teller_account_no")

                minrec1.Text = Format(row("min_cash_rec_appr"), "#,##0.00")
                maxrec1.Text = Format(row("max_cash_rec_appr"), "#,##0.00")
                minpay1.Text = Format(row("min_cash_pay_appr"), "#,##0.00")
                maxpay1.Text = Format(row("max_cash_pay_appr"), "#,##0.00")
                minnoncash1.Text = Format(row("min_non_cash_appr"), "#,##0.00")
                maxnoncash1.Text = Format(row("max_non_cash_appr"), "#,##0.00")
                minod.Text = Format(row("min_od_appr"), "#,##0.00")
                maxod.Text = Format(row("max_od_appr"), "#,##0.00")
                post_ib_gl.Checked = CBool(row("post_ib_gl"))
                viewprotected.Checked = CBool(row("view_protected"))
                printstmt.Checked = CBool(row("print_stmt"))

                disabled.Checked = CBool(row("disabled"))

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


    Private Sub tellerAcct_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tellerAcct.TextChanged
        If tellerAcct.Text <> Nothing Then
            If gEazy.IsCashAccount(tellerAcct.Text) = False Then
                tellerAcct.Text = Nothing
                Me.cus_val.IsValid = False
                Me.cus_val.ErrorMessage = gUtils.ErrorCode(248)

            End If
        End If
    End Sub

    Private Sub users_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles users.TextChanged

        GetRecord()
    End Sub

    Private Sub DataGrid1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub btn_delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_delete.Click
        Dim msg As String

        If Me.users.Text = Nothing Then
            Me.cus_val.IsValid = False
            Me.cus_val.ErrorMessage = gUtils.ErrorCode(109)
        Else

            gUtils.RecordOp("Sproc_DeleteUser '" & users.Text & "'")

            msg = users.Text & " Profile Deletion Requested"
            gUtils.RecordOp("Sproc_AddAuditLog '002','" & msg & "','" & gUser & "',''")
            Response.Redirect("FrmUsersOnly.aspx")

        End If
    End Sub

    Private Sub DataGrid1_CancelCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.CancelCommand

        Me.DataGrid1.Columns(3).Visible = True
        Dim str_id As String = e.Item.Cells(3).Text
        Response.Redirect("Frm_UsersRoles.aspx?p=" & str_id & "")
        'me.DataGrid1.Columns(3).Visible = True 
    End Sub


    Private Sub DataGrid1_DeleteCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.DeleteCommand
        Me.DataGrid1.Columns(3).Visible = True
        Dim str_id As String = e.Item.Cells(3).Text
        Response.Redirect("Frm_UsersWorking_sites.aspx?p=" & str_id & "")

    End Sub


    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Response.Redirect("FrmUsersOnly.aspx")

    End Sub

    Private Sub cmd_go_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_go.Click
        users_TextChanged(sender, e)

    End Sub
End Class

