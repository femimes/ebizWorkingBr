Imports System
Imports System.Collections.Generic
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.Web
Imports System.Web.UI.WebControls

Partial Class Mandate_ReconcilationBoard
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
    ' Batch list / grid binding
    ' =====================================================================
    Protected Sub chkMerge_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkMerge.CheckedChanged
        ' The batch list depends on the merge option, so rebuild it
        drpBatchRefNo.Items.Clear()
        drpBatchRefNo.Items.Add(New ListItem("Choose a batch", ""))
        drpBatchRefNo.DataBind()
        grid.DataBind()
    End Sub

    Protected Sub drpBatchRefNo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpBatchRefNo.SelectedIndexChanged
        litResult.Text = ""
        grid.DataBind()
    End Sub

    Protected Sub sqlproducts_Selecting(ByVal sender As Object, ByVal e As SqlDataSourceSelectingEventArgs) Handles sqlproducts.Selecting
        ' The procedure also takes the batch's display text
        Dim item As ListItem = drpBatchRefNo.SelectedItem
        e.Command.Parameters("@description").Value = If(item Is Nothing OrElse item.Value = "", CObj(DBNull.Value), item.Text)
    End Sub

    Protected Sub grid_DataBound(ByVal sender As Object, ByVal e As EventArgs)
        litCount.Text = If(grid.Rows.Count > 0, "&middot; " & grid.Rows.Count.ToString("N0"), "")
    End Sub

    ''' <summary>Formats a number for the data-* attributes the totals script reads.</summary>
    Protected Function Num(ByVal value As Object) As String
        Return ToDecimal(value).ToString(CultureInfo.InvariantCulture)
    End Function

    ' =====================================================================
    ' Post reconciliation
    ' =====================================================================
    Protected Sub btnPost_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPost.Click
        LoadCurrentUser()

        If drpBatchRefNo.SelectedValue = "" Then
            ShowError("Choose a batch first.")
            Exit Sub
        End If

        ' Work the figures out again on the server rather than trusting the browser
        Dim pctBal As Decimal = FormDecimal("txtPropSelBal") * 0.01D
        Dim pctFee As Decimal = FormDecimal("txtPropSubcharge") * 0.01D
        Dim recon As Decimal = FormDecimal("txtRecon")

        Dim amount As Decimal = 0, fee As Decimal = 0, feeOnly As Decimal = 0
        Dim selected As New List(Of GridViewRow)

        For Each row As GridViewRow In grid.Rows
            If row.RowType <> DataControlRowType.DataRow Then Continue For
            Dim keys As DataKey = grid.DataKeys(row.RowIndex)

            Dim chk As CheckBox = TryCast(row.FindControl("chkSelect"), CheckBox)
            If chk IsNot Nothing AndAlso chk.Checked Then
                selected.Add(row)
                amount += ToDecimal(keys("authorisedAmt"))
                fee += ToDecimal(keys("surcharge"))
            End If

            Dim chkFee As CheckBox = TryCast(row.FindControl("chkFeeOnly"), CheckBox)
            If chkFee IsNot Nothing AndAlso chkFee.Checked Then
                feeOnly += ToDecimal(keys("surcharge")) * pctFee
            End If
        Next

        If selected.Count = 0 Then
            ShowError("Select at least one transaction first.")
            Exit Sub
        End If

        Dim netDiff As Decimal = (amount * pctBal) + (fee * pctFee) + feeOnly - recon
        If Math.Abs(netDiff) >= 0.005D Then
            ShowError(String.Format("Reconciliation is not balanced. The net difference is {0:N2}; it must be zero.", netDiff))
            Exit Sub
        End If

        Try
            Using conn As New SqlConnection(Cfg("EazyConnect"))
                conn.Open()
                For Each row As GridViewRow In selected
                    Dim keys As DataKey = grid.DataKeys(row.RowIndex)
                    Using cmd As New SqlCommand("Sproc_ApproveTransRecInfoUpd", conn)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add(New SqlParameter("@extRefNo", Convert.ToString(keys("extRefNo"))))
                        cmd.Parameters.Add(New SqlParameter("@BatchRefNo", BatchRefFor(keys)))
                        cmd.Parameters.Add(New SqlParameter("@reconUser", guser))
                        cmd.Parameters.Add(New SqlParameter("@reconStatus", 0))
                        cmd.Parameters.Add(New SqlParameter("@reconCheckStatus", 3))
                        cmd.ExecuteNonQuery()
                    End Using
                Next
            End Using
        Catch ex As Exception
            ShowError("The reconciliation was not posted: " & ex.Message)
            Exit Sub
        End Try

        Dim target As String = String.Format("PostReconciliation.aspx?p={0}&r={1}&q={2}&s={3}&t={4}",
            Url(recon), Url(fee), Url(amount), Url(FormDecimal("txtPropSelBal")), Url(FormDecimal("txtPropSubcharge")))
        Response.Redirect(target, False)
        Context.ApplicationInstance.CompleteRequest()
    End Sub

    ''' <summary>Batch reference for a row: the chosen batch, or the row's own batch when "all types" is chosen.</summary>
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

    Private Sub ShowError(ByVal message As String)
        litResult.Text = ""
        cusVal.IsValid = False
        cusVal.ErrorMessage = message
        cusVal.Text = HttpUtility.HtmlEncode(message)
    End Sub

    ''' <summary>Keeps typed figures after a postback.</summary>
    Protected Function FormValue(ByVal name As String) As String
        Dim v As String = Request.Form(name)
        Return HttpUtility.HtmlAttributeEncode(If(String.IsNullOrEmpty(v), "0", v))
    End Function

    Private Function FormDecimal(ByVal name As String) As Decimal
        Return ToDecimal(Request.Form(name))
    End Function

    Private Shared Function ToDecimal(ByVal value As Object) As Decimal
        If value Is Nothing OrElse value Is DBNull.Value Then Return 0D
        Dim d As Decimal
        Dim s As String = Convert.ToString(value, CultureInfo.InvariantCulture).Replace(",", "").Trim()
        If Decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, d) Then Return d
        Return 0D
    End Function

    Private Shared Function Url(ByVal value As Decimal) As String
        Return HttpUtility.UrlEncode(value.ToString("0.00", CultureInfo.InvariantCulture))
    End Function

    Private Shared Function Cfg(ByVal name As String) As String
        Dim s As ConnectionStringSettings = ConfigurationManager.ConnectionStrings(name)
        Return If(s Is Nothing, "", s.ConnectionString)
    End Function
End Class