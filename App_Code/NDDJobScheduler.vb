Imports System
Imports System.Configuration
Imports System.IO
Imports System.Threading
Imports System.Web.Hosting
Imports System.Web.Script.Serialization

''' <summary>
''' In-app timer that runs NDDMandateRecovery.RunAll for every pending mandate on a fixed interval
''' (default 60 minutes). Switched on and off from NDDQueryservice.aspx; the setting is kept in
''' ~/App_Data/NDDJobSchedule.json so it survives restarts.
'''
''' The timer lives inside the IIS application, so it only ticks while the site is running.
''' Set the app pool to AlwaysRunning (and the site to preload) so an idle site is not shut down.
''' </summary>
Public NotInheritable Class NDDJobScheduler
    Implements IRegisteredObject

    Public NotInheritable Class Settings
        Public Enabled As Boolean
        Public IntervalMinutes As Integer = 60
        Public NextRun As Nullable(Of DateTime)
        Public UpdatedBy As String = ""
        Public UpdatedAt As Nullable(Of DateTime)
    End Class

    Private Const SettingsPath As String = "~/App_Data/NDDJobSchedule.json"
    Private Shared ReadOnly Sync As New Object()
    Private Shared _instance As NDDJobScheduler

    Private _timer As Timer
    Private _ticking As Integer

    Private Sub New()
    End Sub

    ' =====================================================================
    ' Start (call from Global.asax Application_Start; pages call it too, so it starts on first use)
    ' =====================================================================
    Public Shared Sub EnsureStarted()
        If _instance IsNot Nothing Then Return
        SyncLock Sync
            If _instance IsNot Nothing Then Return
            Dim s As New NDDJobScheduler()
            HostingEnvironment.RegisterObject(s)
            s._timer = New Timer(AddressOf s.Tick, Nothing, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1))
            _instance = s
        End SyncLock
    End Sub

    Public Sub [Stop](ByVal immediate As Boolean) Implements IRegisteredObject.Stop
        SyncLock Sync
            If _timer IsNot Nothing Then
                _timer.Dispose()
                _timer = Nothing
            End If
            _instance = Nothing
        End SyncLock
        HostingEnvironment.UnregisterObject(Me)
    End Sub

    ' Checked every minute; runs when the next run time has passed
    Private Sub Tick(ByVal state As Object)
        If Interlocked.CompareExchange(_ticking, 1, 0) <> 0 Then Return
        Try
            Dim s As Settings = Load()
            If Not s.Enabled Then Return

            Dim now As DateTime = DateTime.Now
            If s.NextRun.HasValue AndAlso now < s.NextRun.Value Then Return

            ' Book the next slot first, so a crash mid-run does not cause a tight retry loop
            s.NextRun = now.AddMinutes(Interval(s))
            Save(s)

            Dim max As Integer
            If Not Integer.TryParse(ConfigurationManager.AppSettings("NDDJobMaxPerRun"), max) OrElse max <= 0 Then max = 200
            NDDMandateRecovery.RunAll("NDDService", max)
        Catch ex As Exception
            NDDMandateRecovery.AddtoLogFile(ex.ToString(), "NDD scheduler tick failed")
        Finally
            Interlocked.Exchange(_ticking, 0)
        End Try
    End Sub

    ' =====================================================================
    ' Settings
    ' =====================================================================
    Public Shared Function Load() As Settings
        SyncLock Sync
            Try
                Dim path As String = HostingEnvironment.MapPath(SettingsPath)
                If path IsNot Nothing AndAlso File.Exists(path) Then
                    Dim s As Settings = New JavaScriptSerializer().Deserialize(Of Settings)(File.ReadAllText(path))
                    If s IsNot Nothing Then Return s
                End If
            Catch ex As Exception
                NDDMandateRecovery.AddtoLogFile(ex.Message, "NDD scheduler settings could not be read")
            End Try
            Return New Settings()
        End SyncLock
    End Function

    Public Shared Sub Save(ByVal s As Settings)
        SyncLock Sync
            Dim path As String = HostingEnvironment.MapPath(SettingsPath)
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path))
            Dim temp As String = path & ".tmp"
            File.WriteAllText(temp, New JavaScriptSerializer().Serialize(s))
            If File.Exists(path) Then File.Delete(path)
            File.Move(temp, path)
        End SyncLock
    End Sub

    ''' <summary>Switch the automatic run on or off. Turning it on schedules the first run one interval from now.</summary>
    Public Shared Function Update(ByVal enabled As Boolean, ByVal intervalMinutes As Integer, ByVal user As String) As Settings
        Dim s As Settings = Load()
        Dim wasOn As Boolean = s.Enabled
        Dim oldInterval As Integer = Interval(s)

        s.Enabled = enabled
        s.IntervalMinutes = Math.Max(15, Math.Min(1440, intervalMinutes))
        If Not enabled Then
            s.NextRun = Nothing
        ElseIf Not wasOn OrElse oldInterval <> s.IntervalMinutes OrElse Not s.NextRun.HasValue Then
            s.NextRun = DateTime.Now.AddMinutes(s.IntervalMinutes)
        End If
        s.UpdatedBy = If(user, "")
        s.UpdatedAt = DateTime.Now
        Save(s)
        EnsureStarted()
        Return s
    End Function

    Private Shared Function Interval(ByVal s As Settings) As Integer
        Return If(s.IntervalMinutes < 15, 60, s.IntervalMinutes)
    End Function
End Class