Imports NETClass
Imports System.Data
Partial Class Processing_FrmTelSupervisors
    Inherits System.Web.UI.Page

    Public guser As String = Context.User.Identity.Name
    Public l_userId As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then
            'If Dbconnect.IAccess(guser, Request.Url.ToString) = False Then
            LoadBoxes()
            l_userId = Request.QueryString("p")
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
            'Session("RefferPage") = Page.Request.Url.ToString

        End If
    End Sub
    '    Dim msgbox As Messaging.Design.MessageDesigner


    Private Sub LoadBoxes()
        Dim ds As New DataSet
        l_userId = Request.QueryString("p")
        user_id.Text = Request.QueryString("p")
        ds = Classes.GetDataset("Sproc_GetUnselectedUserSups '" & l_userId & "'")

        ListBox1.DataSource = ds.Tables(0)
        ListBox1.DataTextField = "user_names"
        ListBox1.DataValueField = "user_id"
        Me.ListBox1.DataBind()

        ds = Classes.GetDataset("Sproc_GetSelectedUserSups '" & user_id.Text & "'")

        ListBox2.DataSource = ds.Tables(0)
        ListBox2.DataTextField = "user_names"
        ListBox2.DataValueField = "sup_user_id"
        Me.ListBox2.DataBind()

        ds.Dispose()

    End Sub

    Private Sub ImageButton2_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton2.Click
        If ListBox2.SelectedIndex <> -1 Then
            l_userId = Request.QueryString("p")
            Classes.RecordOp("Sproc_DeleteUserFromSup '" & l_userId & "','" & ListBox2.SelectedValue & "'")

            LoadBoxes()
        End If


    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        If ListBox1.SelectedIndex <> -1 Then
            l_userId = Request.QueryString("p")
            '     classes.RecordOp("Sproc_AddUserToSite '" & l_userId & "','" & ListBox1.SelectedItem.Value & "'")
            Classes.RecordOp("Sproc_AddUserToSup '" & l_userId & "','" & ListBox1.SelectedValue & "'")

            LoadBoxes()
        End If


    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Session("l_qtry") = Request.QueryString("p")
        Response.Redirect("FrmTellerInfo.aspx?p=" & Request.QueryString("p") & "&y=" & Request.QueryString("y") & "")

    End Sub
End Class
