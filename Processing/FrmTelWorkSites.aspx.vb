Imports NETClass
Imports System
Imports System.Data

Partial Class Processing_FrmTelWorkSites
    Inherits System.Web.UI.Page
    Public luser As String
    Public StrLocation As String
    Public struser As String


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
        Dim ds As New DataSet()
        l_userId = Request.QueryString("p")
        user_id.Text = Request.QueryString("p")
        ds = Classes.GetDataset(String.Format("Sproc_GetUnselectedUserSites '{0}'", l_userId))
        ListBox1.DataSource = ds.Tables(0)
        ListBox1.DataTextField = "org_desc"
        ListBox1.DataValueField = "Org_code"
        Me.ListBox1.DataBind()

        ds = Classes.GetDataset(String.Format("Sproc_GetSelectedUserSites '{0}'", l_userId))
        ListBox2.DataSource = ds.Tables(0)
        ListBox2.DataTextField = "org_desc"
        ListBox2.DataValueField = "Org_code"
        Me.ListBox2.DataBind()

        ds.Dispose()

    End Sub

    Private Sub ImageButton2_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton2.Click
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
            l_userId = Request.QueryString("p")
            Classes.RecordOp(String.Format("Sproc_DeleteUserFromSite '{0}','{1}'", l_userId, ListBox2.SelectedItem.Value))
            Dim msg As String
            msg = l_userId & " Ebiz Profile Branch permission Deleted  or Removed; " & "Branch:" & ListBox2.SelectedItem.Text

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




            Classes.RecordOp("Sproc_AddUserToSite '" & l_userId & "','" & ListBox1.SelectedItem.Value & "'")
            Dim msg As String
            msg = l_userId & " Ebiz Profile Branch permission  or Modified; " & "Branch:" & ListBox1.SelectedItem.Text

            Classes.RecordOp("Sproc_AddAuditLog '001','" & msg & "','" & guser & "',''")

            LoadBoxes()
        End If


    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Session("l_qtry") = Request.QueryString("p")
        Response.Redirect(String.Format("FrmTellerInfo.aspx?p={0}&y={1}", Request.QueryString("p"), Request.QueryString("y")))

    End Sub
End Class


