Imports NETClass
Imports System.Data
Partial Class Processing_Frm_UsersRoles
    Inherits System.Web.UI.Page
    Public luser As String
    Public StrLocation As String
    Public struser As String


    Public guser As String = Context.User.Identity.Name

    Public l_userId As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            LoadBoxes()
            l_userId = Request.QueryString("p")
            Me.Label1.Text = Request.QueryString("p")
        End If
    End Sub


    Private Sub LoadBoxes()
        Dim ds As New DataSet
        l_userId = Request.QueryString("p")

        ds = classes.GetDataset("Sproc_GetUnselectedUserRoles '" & l_userId & "'")
        ListBox1.DataSource = ds.Tables(0)
        ListBox1.DataTextField = "role_desc"
        ListBox1.DataValueField = "role_id"
        Me.ListBox1.DataBind()

        ds = classes.GetDataset("Sproc_GetSelectedUserRoles '" & l_userId & "'")
        ListBox2.DataSource = ds.Tables(0)
        ListBox2.DataTextField = "role_desc"
        ListBox2.DataValueField = "role_id"
        Me.ListBox2.DataBind()

        ds.Dispose()

    End Sub

    Private Sub ImageButton2_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton2.Click
        l_userId = Request.QueryString("p")
        Dim strgetauthcookies As String

        strgetauthcookies = Request.Cookies("LogonUserID").Value
        If strgetauthcookies <> Nothing Then

            guser = strgetauthcookies.Substring(0, strgetauthcookies.LastIndexOf("?"))
        Else
            luser = Context.User.Identity.Name
            StrLocation = luser.Substring(luser.LastIndexOf("?") + 1)
            struser = luser.Substring(0, luser.LastIndexOf("?")) ' Mid(luser, 1, luser.LastIndexOf("?"))
            guser = struser


        End If

        If ListBox2.SelectedIndex <> -1 Then

            Dim msg As String
            msg = l_userId & "Ebiz Profile Role Permission Deleted  or Removed " & "Branch:" & ListBox2.SelectedItem.Text

            Classes.RecordOp("Sproc_DeleteUserFromRole '" & l_userId & "','" & ListBox2.SelectedItem.Value & "'")
            Classes.RecordOp(String.Format("Sproc_DeleteUserFromSite '{0}','{1}'", l_userId, ListBox2.SelectedItem.Value))


            Classes.RecordOp("Sproc_AddAuditLog '002','" & msg & "','" & guser & "',''")
            LoadBoxes()


        End If


    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Dim strgetauthcookies As String

        strgetauthcookies = Request.Cookies("LogonUserID").Value
        If strgetauthcookies <> Nothing Then

            guser = strgetauthcookies.Substring(0, strgetauthcookies.LastIndexOf("?"))
        Else
            luser = Context.User.Identity.Name
            StrLocation = luser.Substring(luser.LastIndexOf("?") + 1)
            struser = luser.Substring(0, luser.LastIndexOf("?")) ' Mid(luser, 1, luser.LastIndexOf("?"))
            guser = struser


        End If

        If ListBox1.SelectedIndex <> -1 Then
            l_userId = Request.QueryString("p")


            Classes.RecordOp("Sproc_AddUserToRole '" & l_userId & "','" & ListBox1.SelectedItem.Value & "'")
            Dim msg As String
            msg = l_userId & "Ebiz Profile Role permission  or Modified " & "Branch:" & ListBox1.SelectedItem.Text

            Classes.RecordOp("Sproc_AddAuditLog '001','" & msg & "','" & guser & "',''")
            LoadBoxes()

        End If


    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Session("l_qtry") = Request.QueryString("p")
        Response.Redirect("Frm_Users.aspx?p=" & Request.QueryString("p") & "&y=" & Request.QueryString("y") & "")
    End Sub
End Class
