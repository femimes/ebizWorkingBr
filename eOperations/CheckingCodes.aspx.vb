Imports System
Imports NETClass
Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports DevExpress.Web
Imports System.Net.Mail

'Imports CrystalDecisions.Shared
'Imports CrystalDecisions.CrystalReports.Engine
Imports System.Drawing.Printing
Imports Microsoft.ReportingServices
Imports System.Xml
Imports System.Text
Imports System.Security.Cryptography
Imports System.Web.UI.HtmlControls
Imports System.Collections
Imports System.Configuration
Imports System.Net
Imports System.Globalization


'Imports CrystalDecisions
Imports Microsoft.Reporting.WebForms

Partial Class eOperations_CheckingCodes
    Inherits System.Web.UI.Page
    Public ks As IEnumerable = {"'", ".", ",", "[", "]", "}", "{"}
    Public ksj As IEnumerable = {"."}

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
        Dim strInfo As String = "<img src='/ExpCardManager/images/MsgimageLogo.jpg' alt='' />&nbsp;"
        Dim byInfo As String = InsertImages(strInfo)

        Response.Write(GetFullNamesOfImages)
    End Sub

    Public Function GetFullNamesOfImages() As String()
        Dim _directoryName As String = Server.MapPath("~/Images") 'Directory.GetDirectories("~/Images").ToString()
        '   Dim images As String = Path.Combine(_directoryName, "Images")
        Dim images As String = Path.Combine(_directoryName, "")
        If (Not Directory.Exists(images)) Then
            Return Nothing
        End If

        Return Directory.GetFiles(images)
    End Function

    Public Function InsertImages(body As String) As String
        '  Dim images As String() = GetFullNamesOfImages()

        Dim ServDirectoryName As String = Request.FilePath 'Server.MapPath("~/Images")
        Dim strFolder As String() = Request.Url.Segments

        For Each var As String In strFolder
            Response.Write(var & "</br>")
        Next

        Dim strremove As String = String.Format("{0}{1}", strFolder.GetValue(2), strFolder.GetValue(3))
        ServDirectoryName = ServDirectoryName.Replace(strremove, "")

        'Dim kss As  string = GetFullNamesOfImages()

        'Dim images As String() = GetFullNamesOfImages.Select(Path.GetFileName).ToArray()
        Dim patternEnum As String = "(Images/)" + GetPattern(ks)
        Dim info As String = Regex.Replace(body, "(Images/)" + ".", "cid:", RegexOptions.IgnoreCase Or RegexOptions.Compiled)


        Return info.Replace(ServDirectoryName, "")

    End Function

    Public Function GetPattern(ByVal valuesToSearch As IEnumerable) As String

        Return String.Format("({0})", String.Join("|", valuesToSearch))
    End Function

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If (IsPostBack = False And IsCallback = False) Then

        End If
    End Sub

    Private Function GetPattern() As String
        Throw New NotImplementedException
    End Function

End Class
