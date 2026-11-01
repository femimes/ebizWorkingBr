Imports NETClass
Partial Class Processing_Frm_searchRoles
    Inherits System.Web.UI.Page
    Public gUtils As New Classes

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here

        If IsPostBack = False Then
            control.Value = Request.QueryString("textbox")
            control1.Value = Request.QueryString("P")
            'Response.Write(control1.Value)
            DataGrid1.CurrentPageIndex = 0 '
            'Me.TextBox1.Text = Request.QueryString("textbox")
            If Me.sString.Text.Trim.Length = 0 Then
            Else
                bindgrid()
            End If


        End If
    End Sub

    'Private Sub thedate_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    '    Dim script As String = "<script>window.opener.document.forms[0]." + control.Value + ".value = '"
    '    script += thedate.SelectedDate.ToString("dd-MM-yyyy")
    '    script += "';self.close()"
    '    script += "</" + "script>"
    '    RegisterClientScriptBlock("test", script)

    'End Sub

    Sub bindgrid()
        Dim str_grid As String = "Sproc_GetRoleDetails_Sel '" & Me.sString.Text.Trim & "'"

        Me.DataGrid1.DataSource = gUtils.GetDataset(str_grid)
        Me.DataGrid1.DataBind()
        Me.lbl_norec.Text = gUtils.GetDataset(str_grid).Tables(0).Rows.Count & " Records Found"


    End Sub




    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        bindgrid()

    End Sub


    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged

        DataGrid1.CurrentPageIndex = e.NewPageIndex
        bindgrid()
    End Sub
    Private Sub DataGrid1_DeleteCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.DeleteCommand

        Dim l_btnstr As LinkButton = CType(e.Item.Cells(0).Controls(1), LinkButton)
        Dim l_name As String = e.Item.Cells(1).Text.ToString
        Dim script As String = "<script>window.opener.document.forms[0]." + control.Value + ".value = '"
        script += l_btnstr.Text.Trim.ToString

        '   script += "';"
        'script += "window.opener.document.forms[0]." + control1.Value + ".value = '"
        'script += l_name
        script += "';self.close()"
        script += "</" + "script>"
        RegisterClientScriptBlock("test", script)
        'Dim script2 As String = "<script>window.opener.document.forms[0]." + control1.Value + ".value = '"

        'script2 += l_name

        'script2 += "';self.close()"
        'script2 += "</" + "script>"
        'RegisterClientScriptBlock("test2'", script)


    End Sub

    Private Sub DataGrid1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGrid1.SelectedIndexChanged

    End Sub
End Class

