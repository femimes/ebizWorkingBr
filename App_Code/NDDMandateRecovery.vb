Imports System
Imports System.Collections.Generic
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Threading
Imports System.Web.Hosting
Imports System.Web.Script.Serialization
Imports NETClass

''' <summary>
''' NDD e-mandate status check (TSQ) and recovery.
''' Used by NDDQueryservice.aspx (officer-triggered), NDDJobScheduler (automatic run)
''' and NDDQueryJob.ashx (scheduled task).
''' </summary>
Public NotInheritable Class NDDMandateRecovery

    Private Const LogPrefix As String = "NDDAppLog_"

    ' Only one run at a time on this server, whether started by the job or the page
    Private Shared _running As Integer
    Private Shared _lastJobRun As RunSummary
    Private Shared ReadOnly LogLock As New Object()

    Private Sub New()
    End Sub

    Public NotInheritable Class RunSummary
        Public Trigger As String = ""
        Public Started As DateTime
        Public Finished As DateTime
        Public Checked As Integer
        Public Recovered As Integer
        Public NotRecovered As New List(Of String)()
        Public Failed As New List(Of String)()
        Public Skipped As Boolean
        Public SkipReason As String = ""

        Public Function Text() As String
            If Skipped Then Return "Skipped: " & SkipReason
            Return String.Format("{0} checked, {1} sent for recovery, {2} not ready, {3} failed",
                Checked, Recovered, NotRecovered.Count, Failed.Count)
        End Function
    End Class

    ''' <summary>Last unattended run on this server (cleared when the app restarts).</summary>
    Public Shared ReadOnly Property LastJobRun As RunSummary
        Get
            Return _lastJobRun
        End Get
    End Property

    Public Shared ReadOnly Property IsRunning As Boolean
        Get
            Return Thread.VolatileRead(_running) = 1
        End Get
    End Property

    ' =====================================================================
    ' Run for every approved mandate (the job)
    ' =====================================================================
    Public Shared Function RunAll(ByVal actor As String, ByVal maxItems As Integer) As RunSummary
        Dim summary As New RunSummary()
        summary.Trigger = actor
        summary.Started = DateTime.Now

        If Interlocked.CompareExchange(_running, 1, 0) <> 0 Then
            summary.Skipped = True
            summary.SkipReason = "a run is already in progress"
            summary.Finished = DateTime.Now
            Return summary
        End If

        Try
            For Each code As String In PendingMandateCodes()
                If maxItems > 0 AndAlso summary.Checked >= maxItems Then Exit For
                If HostingEnvironment.ShutdownReason <> ApplicationShutdownReason.None Then Exit For ' app is stopping

                summary.Checked += 1
                RunOne(code, actor, summary)
            Next
        Catch ex As Exception
            summary.Failed.Add("run stopped: " & ex.Message)
        Finally
            summary.Finished = DateTime.Now
            Interlocked.Exchange(_running, 0)
        End Try

        _lastJobRun = summary
        AddtoLogFile(summary.Text() & If(summary.Failed.Count > 0, vbCrLf & String.Join(vbCrLf, summary.Failed.ToArray()), ""),
            "NDD job run by " & actor)
        Audit("044", String.Format("NDDMandate recovery job run; {0}", summary.Text()), actor)
        Return summary
    End Function

    ' =====================================================================
    ' Run for chosen mandates (the page)
    ' =====================================================================
    Public Shared Function RunSelected(ByVal mandateCodes As IEnumerable(Of String), ByVal actor As String) As RunSummary
        Dim summary As New RunSummary()
        summary.Trigger = actor
        summary.Started = DateTime.Now

        If Interlocked.CompareExchange(_running, 1, 0) <> 0 Then
            summary.Skipped = True
            summary.SkipReason = "the unattended job is running right now; try again in a few minutes"
            summary.Finished = DateTime.Now
            Return summary
        End If

        Try
            For Each code As String In mandateCodes
                summary.Checked += 1
                RunOne(code, actor, summary)
            Next
        Finally
            summary.Finished = DateTime.Now
            Interlocked.Exchange(_running, 0)
        End Try
        Return summary
    End Function

    Private Shared Sub RunOne(ByVal code As String, ByVal actor As String, ByVal summary As RunSummary)
        Try
            Dim outcome As String = ProcessMandate(code, actor)
            If outcome = "" Then
                summary.Recovered += 1
            Else
                summary.NotRecovered.Add(code & ": " & outcome)
            End If
        Catch ex As WebException
            summary.Failed.Add(code & ": NIBSS could not be reached (" & ex.Message & ")")
        Catch ex As SqlException
            summary.Failed.Add(code & ": database error (" & ex.Message & ")")
        Catch ex As Exception
            summary.Failed.Add(code & ": " & ex.Message)
        End Try
    End Sub

    ' Approved mandates (status 3) waiting for a status check
    Private Shared Function PendingMandateCodes() As List(Of String)
        Dim table As New DataTable()
        Using conn As New SqlConnection(Cfg("EazyConnect"))
            Using cmd As New SqlCommand("SpNDDeMandateGetInfo", conn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add(New SqlParameter("@status", 3))
                Using adapter As New SqlDataAdapter(cmd)
                    adapter.Fill(table)
                End Using
            End Using
        End Using

        Dim codes As New List(Of String)()
        Dim seen As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
        For Each row As DataRow In table.Rows
            Dim code As String = Convert.ToString(row("mandateCode")).Trim()
            If code <> "" AndAlso seen.Add(code) Then codes.Add(code)
        Next
        Return codes
    End Function

    ' =====================================================================
    ' One mandate: returns "" when recovery was started, otherwise why not
    ' =====================================================================
    Public Shared Function ProcessMandate(ByVal mandateCode As String, ByVal actor As String) As String
        ' 1. Ask NIBSS for the mandate status
        Dim tsq As ApiResult = PostNdd("eMandateStatusTSQ", New With {.MandateCode = mandateCode})

        Dim workflowStatus As String = Convert.ToString(Dig(tsq.Json, "data", "data", "workflowStatus"))
        Dim returnedCode As String = Convert.ToString(Dig(tsq.Json, "data", "data", "mandateCode"))
        If returnedCode <> "" Then mandateCode = returnedCode

        If tsq.Code <> "00" OrElse workflowStatus <> "Bank Approved" Then
            ReportStatus(mandateCode, tsq.Code, tsq.Text)
            If tsq.Code <> "00" Then Return Failure(tsq)
            Return "status is " & If(workflowStatus = "", "unknown", workflowStatus)
        End If

        ' 2. Bank approved: start recovery
        AddtoLogFile(mandateCode, "emandatecode Checked for Recovery")
        Dim rec As ApiResult = PostNdd("eMandateApproval", New With {.MandateCode = mandateCode})

        If rec.Code <> "00" Then
            ReportStatus(mandateCode, rec.Code, rec.Text)
            Return "recovery not started: " & Failure(rec)
        End If

        Dim sessionID As String = Convert.ToString(Dig(rec.Json, "data", "sessionID"))
        Dim transactionId As String = Convert.ToString(Dig(rec.Json, "data", "transactionId"))
        Dim paymentReference As String = Convert.ToString(Dig(rec.Json, "data", "paymentReference"))

        Using conn As New SqlConnection(Cfg("EazyConnect"))
            Using cmd As New SqlCommand("SpGetNDDeMandateUpd", conn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add(New SqlParameter("@mandateCode", mandateCode))
                cmd.Parameters.Add(New SqlParameter("@sessionID", sessionID))
                cmd.Parameters.Add(New SqlParameter("@status", 5))
                conn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
        ReportStatus(mandateCode, rec.Code, rec.Text)

        Audit("044", String.Format("NDDMandate FT Recovery on Mandate- {0}; SessionID: {1}; TransactionID {2}:paymentReference :{3};Recovery:{4}",
            mandateCode, sessionID, transactionId, paymentReference, "True"), actor)
        Return ""
    End Function

    Private Shared Sub ReportStatus(ByVal mandateCode As String, ByVal code As String, ByVal desc As String)
        Using conn As New SqlConnection(Cfg("EazyConnect"))
            Using cmd As New SqlCommand("spNDDMandateRptstatus", conn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add(New SqlParameter("@mandateCode", mandateCode))
                cmd.Parameters.Add(New SqlParameter("@responseCode", If(code, "")))
                cmd.Parameters.Add(New SqlParameter("@responseDesc", If(desc, "")))
                conn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    ' =====================================================================
    ' NIBSS NDD API
    ' =====================================================================
    Private NotInheritable Class ApiResult
        Public Code As String = ""
        Public Text As String = ""
        Public Raw As String = ""
        Public Json As Dictionary(Of String, Object)
    End Class

    Private Shared Function PostNdd(ByVal action As String, ByVal body As Object) As ApiResult
        Dim url As String = Cfg("NDDCodeHostUrl").TrimEnd("/"c) & "/" & action
        Dim token As String = Convert.ToBase64String(Encoding.Default.GetBytes(String.Format("{0}:{1}",
            Classes.DecryptString(Cfg("NDDCodeKeyID")), Classes.DecryptString(Cfg("NDDCodeKey")))))

        Dim payload As String = New JavaScriptSerializer().Serialize(body)
        AddtoLogFile(payload, String.Format("Url=>{0} payload request", url))

        Dim req As HttpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)
        req.Method = "POST"
        req.ContentType = "application/json"
        req.Timeout = 60000
        req.Headers.Add("AuthToken", token)
        req.Headers.Add("TransToken", token)

        Dim bytes As Byte() = Encoding.UTF8.GetBytes(payload)
        req.ContentLength = bytes.Length
        Using s As Stream = req.GetRequestStream()
            s.Write(bytes, 0, bytes.Length)
        End Using

        Dim r As New ApiResult()
        Try
            Using resp As HttpWebResponse = CType(req.GetResponse(), HttpWebResponse)
                Using reader As New StreamReader(resp.GetResponseStream())
                    r.Raw = reader.ReadToEnd()
                End Using
            End Using
        Catch ex As WebException
            ' Keep the error body: NIBSS explains failures in it
            If ex.Response Is Nothing Then Throw
            Using reader As New StreamReader(ex.Response.GetResponseStream())
                r.Raw = reader.ReadToEnd()
            End Using
            If String.IsNullOrEmpty(r.Raw) Then Throw
        End Try
        AddtoLogFile(r.Raw, action & " response")

        Dim js As New JavaScriptSerializer()
        js.MaxJsonLength = Integer.MaxValue
        r.Json = TryCast(js.DeserializeObject(r.Raw), Dictionary(Of String, Object))
        r.Code = Convert.ToString(Dig(r.Json, "responseCode"))
        r.Text = Convert.ToString(Dig(r.Json, "responseText"))
        Return r
    End Function

    ' Walk nested JSON objects: Dig(json, "data", "data", "id")
    Private Shared Function Dig(ByVal node As Object, ByVal ParamArray path As String()) As Object
        For Each key As String In path
            Dim dict As Dictionary(Of String, Object) = TryCast(node, Dictionary(Of String, Object))
            If dict Is Nothing Then Return Nothing
            Dim nextNode As Object = Nothing
            For Each kv As KeyValuePair(Of String, Object) In dict
                If String.Equals(kv.Key, key, StringComparison.OrdinalIgnoreCase) Then
                    nextNode = kv.Value
                    Exit For
                End If
            Next
            node = nextNode
        Next
        Return node
    End Function

    Private Shared Function Failure(ByVal r As ApiResult) As String
        Return String.Format("{0} ({1})", If(r.Text = "", "no reason given", r.Text), If(r.Code = "", "no code", r.Code))
    End Function

    ' =====================================================================
    ' Helpers
    ' =====================================================================
    Public Shared Sub AddtoLogFile(ByVal Message As String, ByVal WebPage As String)
        Try
            Dim dir As String = HostingEnvironment.MapPath("~/log/")
            Directory.CreateDirectory(dir)
            Dim path As String = System.IO.Path.Combine(dir, LogPrefix & DateTime.Now.ToString("dd-MM-yyyy") & ".txt")
            SyncLock LogLock
                File.AppendAllText(path, String.Format("——————-START————-{0}{3}## :{1}{3}{2}{3}——————-END————-{0}{3}",
                    DateTime.Now, WebPage, Message, vbCrLf))
            End SyncLock
        Catch
            ' Logging must never stop a recovery run
        End Try
    End Sub

    Private Shared Sub Audit(ByVal code As String, ByVal message As String, ByVal actor As String)
        Try
            Classes.RecordOp(String.Format("Sproc_AddAuditLog '{0}','{1}','{2}',''", code, Sql(message), Sql(actor)))
        Catch ex As Exception
            AddtoLogFile(ex.Message, "audit write failed")
        End Try
    End Sub

    Private Shared Function Cfg(ByVal name As String) As String
        Dim s As ConnectionStringSettings = ConfigurationManager.ConnectionStrings(name)
        Return If(s Is Nothing, "", s.ConnectionString)
    End Function

    Private Shared Function Sql(ByVal value As String) As String
        Return If(value, "").Replace("'", "''")
    End Function
End Class