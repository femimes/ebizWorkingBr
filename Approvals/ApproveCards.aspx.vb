Imports System
Imports System.Collections.Generic
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.UI.WebControls

Partial Class Approvals_ApproveCards
    Inherits System.Web.UI.Page

    Public luser As String
    Public StrLocation As String
    Public struser As String
    Public guser As String

    Private Const StatusApproved As Integer = 3
    Private Const StatusDisapproved As Integer = 4

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadCurrentUser()
            ' The list excludes requests this officer generated
            txtuserID.Text = guser
            ctrlGrid.PageIndex = 0
        End If
    End Sub

    Protected Sub SqlDataSource1_Selected(ByVal sender As Object, ByVal e As SqlDataSourceStatusEventArgs) Handles SqlDataSource1.Selected
        litTotal.Text = If(e.Exception Is Nothing AndAlso e.AffectedRows > 0, "&middot; " & e.AffectedRows.ToString("N0"), "")
    End Sub

    ' =====================================================================
    ' Bulk approve / disapprove
    ' =====================================================================
    Protected Sub BtnApprove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnApprove.Click
        ProcessSelection(StatusApproved, "approved")
    End Sub

    Protected Sub btnDisApprove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDisApprove.Click
        ProcessSelection(StatusDisapproved, "disapproved")
    End Sub

    Private Sub ProcessSelection(ByVal status As Integer, ByVal verb As String)
        LoadCurrentUser()

        Dim done As Integer = 0
        Dim failed As New List(Of String)()

        For Each row As GridViewRow In ctrlGrid.Rows
            If row.RowType <> DataControlRowType.DataRow Then Continue For
            Dim chk As CheckBox = TryCast(row.FindControl("CheckBox1"), CheckBox)
            If chk Is Nothing OrElse Not chk.Checked Then Continue For

            Dim refNo As String = Convert.ToString(ctrlGrid.DataKeys(row.RowIndex)("RefNo"))
            Try
                SetStatus(refNo, status)
                done += 1
            Catch ex As Exception
                failed.Add(refNo & " (" & ex.Message & ")")
            End Try
        Next

        ctrlGrid.DataBind()

        If done = 0 AndAlso failed.Count = 0 Then
            ShowError("Select at least one card request first.")
        ElseIf failed.Count = 0 Then
            ShowSuccess(String.Format("{0} card request{1} {2}.", done, If(done = 1, "", "s"), verb))
        Else
            ShowError(String.Format("{0} {1}; {2} failed: {3}", done, verb, failed.Count, String.Join("; ", failed.ToArray())))
        End If
    End Sub

    ' =====================================================================
    ' Single-row approve / disapprove
    ' =====================================================================
    Protected Sub ctrlGrid_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles ctrlGrid.RowCommand
        Dim status As Integer
        Dim verb As String
        Select Case e.CommandName
            Case "ApproveOne" : status = StatusApproved : verb = "approved"
            Case "DisapproveOne" : status = StatusDisapproved : verb = "disapproved"
            Case Else : Exit Sub
        End Select

        Dim index As Integer
        If Not Integer.TryParse(Convert.ToString(e.CommandArgument), index) OrElse index < 0 OrElse index >= ctrlGrid.DataKeys.Count Then Exit Sub

        Dim refNo As String = Convert.ToString(ctrlGrid.DataKeys(index)("RefNo"))
        Dim title As String = Convert.ToString(ctrlGrid.DataKeys(index)("RequestTitle"))

        Try
            LoadCurrentUser()
            SetStatus(refNo, status)
            ctrlGrid.DataBind()
            ShowSuccess(String.Format("Card request {0} ({1}) {2}.", title, refNo, verb))
        Catch ex As Exception
            ShowError(String.Format("Card request {0} was not {1}: {2}", refNo, verb, ex.Message))
        End Try
    End Sub

    Protected Sub btncancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncancel.Click
        Response.Redirect("ApproveCards.aspx", False)
        Context.ApplicationInstance.CompleteRequest()
    End Sub

    ' =====================================================================
    ' Helpers
    ' =====================================================================
    Private Sub SetStatus(ByVal refNo As String, ByVal status As Integer)
        Using conn As New SqlConnection(Cfg("EazyConnect"))
            Using cmd As New SqlCommand("Sproc_ApproveCardRequest", conn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add(New SqlParameter("@RefNo", refNo))
                cmd.Parameters.Add(New SqlParameter("@ApprovingOfficer", If(guser, "")))
                cmd.Parameters.Add(New SqlParameter("@status", status))
                conn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Private Sub LoadCurrentUser()
        Dim cookie As HttpCookie = Request.Cookies("LogonUserID")
        luser = If(cookie IsNot Nothing AndAlso Not String.IsNullOrEmpty(cookie.Value), cookie.Value, If(User.Identity.Name, ""))
        Dim split As Integer = luser.LastIndexOf("?"c)
        struser = If(split >= 0, luser.Substring(0, split), luser)
        StrLocation = If(split >= 0, luser.Substring(split + 1), "")
        guser = struser
    End Sub

    Private Sub ShowSuccess(ByVal message As String)
        litResult.Text = HttpUtility.HtmlEncode(message)
        litError.Text = ""
    End Sub

    Private Sub ShowError(ByVal message As String)
        litError.Text = HttpUtility.HtmlEncode(message)
        litResult.Text = ""
    End Sub

    Private Shared Function Cfg(ByVal name As String) As String
        Dim s As ConnectionStringSettings = ConfigurationManager.ConnectionStrings(name)
        Return If(s Is Nothing, "", s.ConnectionString)
    End Function
End Class