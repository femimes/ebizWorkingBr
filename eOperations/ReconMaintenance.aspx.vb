Imports System
Imports System.Collections.Generic
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.UI.WebControls

Partial Class Mandate_ReconMaintenance
    Inherits System.Web.UI.Page

    Public luser As String
    Public StrLocation As String
    Public struser As String
    Public guser As String

    ' Value the batch list uses for "all transaction types"
    Private Const AllTypesValue As String = "   ;All TransactionTypes"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub

    ' =====================================================================
    ' Binding
    ' =====================================================================
    Protected Sub drpBatchRefNo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpBatchRefNo.SelectedIndexChanged
        litResult.Text = ""
        grid.DataBind()
    End Sub

    Protected Sub sqlproducts_Selecting(ByVal sender As Object, ByVal e As SqlDataSourceSelectingEventArgs) Handles sqlproducts.Selecting
        Dim item As ListItem = drpBatchRefNo.SelectedItem
        e.Command.Parameters("@description").Value = If(item Is Nothing OrElse item.Value = "", CObj(DBNull.Value), item.Text)
    End Sub

    Protected Sub grid_DataBound(ByVal sender As Object, ByVal e As EventArgs)
        litCount.Text = If(grid.Rows.Count > 0, "&middot; " & grid.Rows.Count.ToString("N0"), "")
    End Sub

    ' =====================================================================
    ' Drop one row
    ' =====================================================================
    Protected Sub grid_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
        If e.CommandName <> "DropOne" Then Exit Sub

        LoadCurrentUser()
        Dim index As Integer = Convert.ToInt32(e.CommandArgument)
        Dim keys As DataKey = grid.DataKeys(index)
        Dim refNo As String = Convert.ToString(keys("extRefNo"))

        Try
            Using conn As New SqlConnection(Cfg("EazyConnect"))
                conn.Open()
                DropTransaction(conn, keys)
            End Using
            grid.DataBind()
            ShowSuccess("Transaction " & refNo & " dropped.")
        Catch ex As Exception
            ShowError("Transaction " & refNo & " was not dropped: " & ex.Message)
        End Try
    End Sub

    ' =====================================================================
    ' Drop the ticked rows
    ' =====================================================================
    Protected Sub btnPost_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPost.Click
        LoadCurrentUser()

        Dim done As Integer = 0
        Dim problems As New List(Of String)

        Using conn As New SqlConnection(Cfg("EazyConnect"))
            conn.Open()
            For Each row As GridViewRow In grid.Rows
                If row.RowType <> DataControlRowType.DataRow Then Continue For
                Dim chk As CheckBox = TryCast(row.FindControl("ChkBoxItem"), CheckBox)
                If chk Is Nothing OrElse Not chk.Checked Then Continue For

                Dim keys As DataKey = grid.DataKeys(row.RowIndex)
                Try
                    DropTransaction(conn, keys)
                    done += 1
                Catch ex As Exception
                    problems.Add(Convert.ToString(keys("extRefNo")) & ": " & ex.Message)
                End Try
            Next
        End Using

        If done = 0 AndAlso problems.Count = 0 Then
            ShowError("Select at least one transaction first.")
            Exit Sub
        End If

        grid.DataBind()
        If problems.Count > 0 Then
            ShowError(String.Join(" | ", problems.ToArray()))
        End If
        If done > 0 Then
            litResult.Text = HttpUtility.HtmlEncode(String.Format("Dropped {0} transaction{1}.", done, If(done = 1, "", "s")))
        End If
    End Sub

    Private Sub DropTransaction(ByVal conn As SqlConnection, ByVal keys As DataKey)
        Using cmd As New SqlCommand("Sproc_ApproveTransRecInfoMaintUpd", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add(New SqlParameter("@extRefNo", Convert.ToString(keys("extRefNo"))))
            cmd.Parameters.Add(New SqlParameter("@BatchRefNo", BatchRefFor(keys)))
            cmd.Parameters.Add(New SqlParameter("@reconUser", guser))
            cmd.Parameters.Add(New SqlParameter("@reconStatus", 0))
            cmd.Parameters.Add(New SqlParameter("@reconCheckStatus", 3))
            cmd.Parameters.Add(New SqlParameter("@status", 9))
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Private Function BatchRefFor(ByVal keys As DataKey) As String
        Dim value As String = drpBatchRefNo.SelectedValue
        If value = AllTypesValue Then Return Convert.ToString(keys("BatchRefNo"))
        Dim split As Integer = value.IndexOf(";"c)
        Return If(split >= 0, value.Substring(0, split), value)
    End Function

    ' =====================================================================
    ' Helpers
    ' =====================================================================
    Private Sub LoadCurrentUser()
        luser = If(Context.User.Identity.Name, "")
        Dim split As Integer = luser.LastIndexOf("?"c)
        struser = If(split >= 0, luser.Substring(0, split), luser)
        StrLocation = If(split >= 0, luser.Substring(split + 1), "")
        guser = struser
    End Sub

    Private Sub ShowSuccess(ByVal message As String)
        litResult.Text = HttpUtility.HtmlEncode(message)
    End Sub

    Private Sub ShowError(ByVal message As String)
        litResult.Text = ""
        cusVal.IsValid = False
        cusVal.ErrorMessage = message
        cusVal.Text = HttpUtility.HtmlEncode(message)
    End Sub

    Private Shared Function Cfg(ByVal name As String) As String
        Dim s As ConnectionStringSettings = ConfigurationManager.ConnectionStrings(name)
        Return If(s Is Nothing, "", s.ConnectionString)
    End Function
End Class