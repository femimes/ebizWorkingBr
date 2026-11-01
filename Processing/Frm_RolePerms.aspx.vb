Imports NETClass
Imports System.Data


Partial Class Processing_Frm_RolePerms
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then
            'If Dbconnect.IAccess(guser, Request.Url.ToString) = False Then
            LoadPermTypes()

            LoadBoxes()
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
        Dim ds As New DataSet()
        Me.role_id.Text = Request.QueryString("p")
        ds = classes.GetDataset("Sproc_GetUnselectedRolePerm '" & role_id.Text & "'," & permtype.SelectedValue & "")
        ListBox1.DataSource = ds.Tables(0)
        ListBox1.DataTextField = "obj_desc"
        ListBox1.DataValueField = "obj_no"
        ListBox1.DataBind()


        ds = classes.GetDataset("Sproc_GetSelectedRolePerm '" & role_id.Text & "'," & permtype.SelectedValue & "")
        ListBox2.DataSource = ds.Tables(0)
        ListBox2.DataTextField = "obj_desc"
        ListBox2.DataValueField = "obj_no"
        ListBox2.DataBind()


        ds.Dispose()
    End Sub


    Private Sub LoadPermTypes()
        Dim ds As New DataSet

        ds = classes.GetDataset("Sproc_GetPermTypes")
        permtype.DataSource = ds.Tables(0)
        permtype.DataTextField = "obj_module_desc"
        permtype.DataValueField = "obj_module"
        permtype.DataBind()

        ds.Dispose()

    End Sub

    Private Sub ImageButton2_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton2.Click
        If ListBox2.SelectedIndex <> -1 Then
            Classes.RecordOp("Sproc_DeleteRolePerm '" & role_id.Text & "','" & ListBox2.SelectedItem.Value & "'")
            LoadBoxes()
        End If


    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        If ListBox1.SelectedIndex <> -1 Then
            Classes.RecordOp("Sproc_AddRolePerm '" & role_id.Text & "','" & ListBox1.SelectedItem.Value & "'")

            LoadBoxes()
        End If


    End Sub

    Private Sub permtype_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles permtype.SelectedIndexChanged
        LoadBoxes()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Response.Redirect("Frm_Roles.aspx?p=" & Request.QueryString("p") & "&y=" & Request.QueryString("y") & "")

    End Sub
End Class
