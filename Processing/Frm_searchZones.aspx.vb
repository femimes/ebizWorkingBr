Imports NETClass
Imports System.Data
Partial Class Processing_Frm_searchZones
    Inherits System.Web.UI.Page



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

    Sub bindgrid()
        Dim str_grid As String = "Sproc_GetStateLGAZone '" & Request.QueryString("r") & "','" & Me.sString.Text.Trim.Replace("'", "") & "'"

        Me.DataGrid1.DataSource = Classes.GetDataset(str_grid)
        Me.DataGrid1.DataBind()
        Me.lbl_norec.Text = Classes.GetDataset(str_grid).Tables(0).Rows.Count & " Records Found"


    End Sub




    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        bindgrid()

    End Sub


    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged

        DataGrid1.CurrentPageIndex = e.NewPageIndex
        bindgrid()
    End Sub
    Private Sub DataGrid1_DeleteCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.DeleteCommand

        Dim l_btnstr As LinkButton
        Dim l_name As String
        If Request.QueryString("r") = 1 Then
            l_btnstr = CType(e.Item.Cells(0).Controls(1), LinkButton)
            l_name = l_btnstr.Text ' e.Item.Cells(0).Text.ToString
        ElseIf Request.QueryString("r") = 2 Then
            l_btnstr = CType(e.Item.Cells(1).Controls(1), LinkButton)
            l_name = l_btnstr.Text  'e.Item.Cells(1).Text.ToString

        Else
            l_btnstr = CType(e.Item.Cells(2).Controls(1), LinkButton)
            l_name = l_btnstr.Text ' e.Item.Cells(2).Text.ToString

        End If

        'Dim l_name As String = e.Item.Cells(1).Text.ToString
        Dim script As String = "<script>window.opener.document.forms[0]." + control.Value + ".value = '"
        script += l_btnstr.CssClass.Trim.ToString
        script += "';"
        script += "window.opener.document.forms[0]." + control1.Value + ".value = '"
        script += l_name
        script += "';self.close()"
        script += "</" + "script>"
        RegisterClientScriptBlock("test", script)
        Response.Write(script)

        'Dim l_btnstr As LinkButton = CType(e.Item.Cells(0).Controls(1), LinkButton)
        'Dim l_name As String = e.Item.Cells(1).Text.ToString
        'Dim script As String = "<script>window.opener.document.forms[0]." + control.Value + ".value = '"
        'script += l_btnstr.Text.Trim.ToString

        'script += "';"
        'script += "window.opener.document.forms[0]." + control1.Value + ".value = '"
        'script += l_name
        'script += "';self.close()"
        'script += "</" + "script>"
        'RegisterClientScriptBlock("test", script)
        'Dim script2 As String = "<script>window.opener.document.forms[0]." + control1.Value + ".value = '"

        'script2 += l_name

        'script2 += "';self.close()"
        'script2 += "</" + "script>"
        'RegisterClientScriptBlock("test2'", script)


    End Sub



    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        Dim l_sortexp As String = e.SortExpression.ToString
        'Response.Write(l_sortexp)
        Dim ds As New DataSet

        ds = Classes.GetDataset("Sproc_GetStateLGAZone '" & Request.QueryString("r") & "','" & Me.sString.Text.Trim.Replace("'", "") & "'")
        'Dim str_grid As String = "Sproc_SearchOrganInfo_Sel '" & Me.sString.Text.Trim.Replace("'", "") & "'"



        ds.Tables(0).DefaultView.Sort = l_sortexp

        DataGrid1.DataSource = ds.Tables(0).DefaultView

        DataGrid1.DataMember = "table"

        Me.DataGrid1.DataBind()
    End Sub
End Class

