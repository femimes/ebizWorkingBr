Imports NETClass
Imports System.Data
Partial Class Processing_Frm_UserReset
    Inherits System.Web.UI.Page
    Public guser As String = UserProfile.IUserAuthName
    Public l_timercounts As Integer
    Private strReferrer As Uri
    Public gUtils As New Classes()
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        If IsPostBack = False Then

            'If Dbconnect.IAccess(guser, Request.Url.ToString) = False Then

            BindGrid()
                'user_id.Text = DataGrid1.Item(0, 0)
                Me.chk_all.Checked = True
                Dim Timer1 As New System.Timers.Timer
                Timer1.Interval = 20000
                Timer1.Enabled = True
                ' Timer1.AutoReset = True
                'AddHandler Timer1.Elapsed, New System.Timers.ElapsedEventHandler(AddressOf Me.Timer1_Elapsed)
                'Timer1.Start()
                l_timercounts = 0

                Timer1.BeginInit()
                DataGrid1.CurrentPageIndex = 0

                If Timer1.Interval.ToString = 200000 Then
                    Response.Redirect("Frm_UserReset.aspx")
                End If

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
            ' Session("RefferPage") = Page.Request.Url.ToString

        End If

    End Sub


    Sub BindGrid()
        Dim ds As New DataSet()
        Dim str_sel As String = Nothing

        If drp_con.SelectedValue = Nothing Then
            str_sel = "Sproc_Aspx_UserstatusAll_sel"
        ElseIf drp_con.SelectedValue = 1 Then
            str_sel = "Select distinct a.*,a.User_names as 'Full names',a.user_id as 'User ID' " & _
                      "from AppUsers a left Outer Join AppUserslogged b on " & _
                      "a.User_id = b.User_id  where grace_count = " & txtcri.Text & "   order by 'Full names'"

        ElseIf drp_con.SelectedValue = 2 Then
            str_sel = "Select distinct a.*,a.User_names as 'Full names',a.user_id as 'User ID' " & _
                               "from AppUsers a left Outer Join AppUserslogged b on " & _
                               "a.User_id = b.User_id  where User_names like  '" & txtcri.Text & "' + '%'  order by 'Full names'"
        ElseIf drp_con.SelectedValue = 3 Then
            str_sel = "Select distinct a.*,a.User_names as 'Full names',a.user_id as 'User ID' " & _
                               "from AppUsers a left Outer Join AppUserslogged b on " & _
                               "a.User_id = b.User_id  where disabled like  '" & txtcri.Text & "' + '%'  order by 'Full names'"

        ElseIf drp_con.SelectedValue = 4 Then
            str_sel = "Select distinct a.*,a.User_names as 'Full names',a.user_id as 'User ID' " & _
                               "from AppUsers a left Outer Join AppUserslogged b on " & _
                               " a.User_id = b.User_id  where User_names like  '" & txtcri.Text & "' + '%'  order by 'Full names'"




        End If
        'ds = gUtils.GetDataset("Sproc_Aspx_UserstatusAll_sel")
        ds = gUtils.GetDataset(str_sel)

        DataGrid1.DataSource = ds
        DataGrid1.DataMember = "table"
        Me.DataGrid1.DataBind()
        Fillstatus()
        FillLockedStatus()
        FillDisableStatus()
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged

        DataGrid1.CurrentPageIndex = e.NewPageIndex
        BindGrid()
    End Sub
    Private Sub Btn_reset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_reset.Click
        Dim item As DataGridItem
        Me.DataGrid1.Columns(2).Visible = True
        For Each item In DataGrid1.Items
            Dim chkopt As CheckBox = CType(item.Cells(0).Controls(1), CheckBox)
            If chkopt.Checked = True Then
                If Me.chk_lock.Checked = True Then
                    gUtils.UnlockUserAccount(item.Cells(2).Text)
                End If

                If Me.chk_disabled.Checked = True Then
                    gUtils.EnableUserAccount(item.Cells(2).Text)
                End If
                If chk_pwds.Checked = True Then
                    gUtils.ResetUserPassword(item.Cells(2).Text)

                End If

                If Me.chk_log.Checked = True Then
                    gUtils.ClearUserAccount(item.Cells(2).Text)
                End If

                If Me.chk_all.Checked = True Then
                    gUtils.UnlockUserAccount(item.Cells(2).Text)
                    gUtils.EnableUserAccount(item.Cells(2).Text)
                    gUtils.ClearUserAccount(item.Cells(2).Text)
                    gUtils.ResetUserPassword(item.Cells(2).Text)

                End If

                ' MsgBox(gUtils.ErrorCode(135), MsgBoxStyle.Information)


            End If




        Next
        Response.Redirect("Frm_UserReset.aspx?u=" & Request.QueryString("u") & "&y=" & Request.QueryString("y") & "")
        'If user_id.Text <> Nothing Then


    End Sub

    Private Sub DataGrid1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGrid1.SelectedIndexChanged
        Response.Redirect("Frm_UserReset.aspx?u=" & Request.QueryString("u") & "&y=" & Request.QueryString("y") & "")
    End Sub
    Sub FillLockedStatus()
        Dim item As DataGridItem
        Me.DataGrid1.Columns(2).Visible = True
        Me.DataGrid1.Columns(8).Visible = True
        For Each item In DataGrid1.Items



            Dim l_locked As System.Web.UI.WebControls.Label = CType(item.Cells(4).Controls(1), System.Web.UI.WebControls.Label)
            'If DataGrid1.SelectedIndex = -1 Then
            Dim l_logonlabel As Label = CType(item.Cells(8).Controls(1), Label)


            If l_logonlabel.Text.Trim = 0 Then
                l_locked.Text = "Off"

            Else
                l_locked.Text = "On"
            End If



        Next
        Me.DataGrid1.Columns(2).Visible = False
        Me.DataGrid1.Columns(8).Visible = False
    End Sub

    Sub FillDisableStatus()
        Dim item As DataGridItem
        Me.DataGrid1.Columns(2).Visible = True
        Me.DataGrid1.Columns(9).Visible = True
        For Each item In DataGrid1.Items



            Dim l_locked As System.Web.UI.WebControls.Label = CType(item.Cells(6).Controls(1), System.Web.UI.WebControls.Label)
            'If DataGrid1.SelectedIndex = -1 Then
            Dim l_logonlabel As Label = CType(item.Cells(9).Controls(1), Label)


            If l_logonlabel.Text.Trim = 0 Then
                l_locked.Text = "Enabled"

            Else
                l_locked.Text = "Disabled"
            End If



        Next
        Me.DataGrid1.Columns(2).Visible = False
        Me.DataGrid1.Columns(9).Visible = False
    End Sub
    Sub Fillstatus()
        Dim item As DataGridItem
        Me.DataGrid1.Columns(2).Visible = True
        Me.DataGrid1.Columns(7).Visible = True
        For Each item In DataGrid1.Items



            Dim l_logonImage As System.Web.UI.WebControls.Image = CType(item.Cells(3).Controls(1), System.Web.UI.WebControls.Image)
            'If DataGrid1.SelectedIndex = -1 Then
            Dim l_logonlabel As Label = CType(item.Cells(7).Controls(1), Label)


            If IsNumeric(l_logonlabel.Text.Trim) = True Then
                l_logonImage.ImageUrl = "../Image/buddy.gif"
                l_logonImage.ToolTip = "User is currently logged on"

            Else

            End If



        Next
        Me.DataGrid1.Columns(2).Visible = False
        Me.DataGrid1.Columns(7).Visible = False

    End Sub
    ''Private Sub CreateTimer()
    ''    Dim Timer1 As New System.Timers.Timer()
    ''    Timer1.Interval = 5000
    ''    Timer1.Enabled = True
    ''    AddHandler Timer1.Elapsed, New System.Timers.ElapsedEventHandler(AddressOf Me.Timer1_Elapsed)
    ''End Sub

    'Private Sub Timer1_Elapsed(ByVal sender As System.Object, ByVal e As System.Timers.ElapsedEventArgs)

    '    System.Windows.Forms.MessageBox.Show("Elapsed!", "Timer Event Raised!")

    '    Response.Redirect("Frm_UserReset.aspx")
    'End Sub




    Private Sub Timer2_Elapsed(ByVal sender As System.Object, ByVal e As System.Timers.ElapsedEventArgs)
        Response.Redirect("Frm_UserReset.aspx?u=" & Request.QueryString("u") & "&y=" & Request.QueryString("y") & "")
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        Dim l_sortexp As String = e.SortExpression.ToString

        Dim ds As New DataSet
        'ds = gUtils.GetDataset("Sproc_Aspx_UserstatusAll_sel")
        Dim str_sel As String = Nothing

        If drp_con.SelectedValue = Nothing Then
            str_sel = "Sproc_Aspx_UserstatusAll_sel"
        ElseIf drp_con.SelectedValue = 1 Then
            str_sel = "Select distinct a.*,a.User_names as 'Full names',a.user_id as 'User ID' " & _
                      "from AppUsers a left Outer Join AppUserslogged b on " & _
                      "a.User_id = b.User_id  where grace_count = " & txtcri.Text & "    order by 'Full names'"

        ElseIf drp_con.SelectedValue = 2 Then
            str_sel = "Select distinct a.*,a.User_names as 'Full names',a.user_id as 'User ID' " & _
                               "from AppUsers a left Outer Join AppUserslogged b on " & _
                               "a.User_id = b.User_id  where User_names like  '" & txtcri.Text & "'   + '%'   order by 'Full names'"
        ElseIf drp_con.SelectedValue = 3 Then
            str_sel = "Select distinct a.*,a.User_names as 'Full names',a.user_id as 'User ID' " & _
                               "from AppUsers a left Outer Join AppUserslogged b on " & _
                               "a.User_id = b.User_id  where disabled like  '" & txtcri.Text & "' + '%'    order by 'Full names'"

        ElseIf drp_con.SelectedValue = 4 Then
            str_sel = "Select distinct a.*,a.User_names as 'Full names',a.user_id as 'User ID' " & _
                               "from AppUsers a left Outer Join AppUserslogged b on " & _
                               "a.User_id = b.User_id  where User_names like  '" & txtcri.Text & "' + '%'   order by 'Full names'"




        End If
        'ds = gUtils.GetDataset("Sproc_Aspx_UserstatusAll_sel")
        ds = gUtils.GetDataset(str_sel)


        ds.Tables(0).DefaultView.Sort = l_sortexp

        DataGrid1.DataSource = ds.Tables(0).DefaultView

        DataGrid1.DataMember = "table"

        Me.DataGrid1.DataBind()
        Fillstatus()
        FillLockedStatus()
        FillDisableStatus()
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            BindGrid()
        Catch ex As Exception
             Classes.alertwindow(Me.Page, ex.Message, "Halt")
     
        End Try

    End Sub
End Class

