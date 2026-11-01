Imports NETClass
Imports System
Imports System.Data
Partial Class Reports_Rs_CardRequest
    Inherits System.Web.UI.Page
    Public gUser As String
    Public luser As String
    Public StrLocation As String
    Public struser As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            loadreport()

        End If
    End Sub
    Sub loadreport()

        Dim reportname As String, reportproc As String, reportdbSource As String

        Dim RDbsViewer As New Microsoft.Reporting.WebForms.ReportDataSource

        RDbsViewer.Name = reportdbSource


        Dim ds As DataSet, row As DataRow, rowIndex As Integer
        Dim repproc As String, repobject As String
        ds = Classes.GetDataset("Sproc_GetAppObject " & CInt(Request.QueryString("a")) & "")

        For rowIndex = 0 To ds.Tables(0).Rows.Count - 1
            row = ds.Tables(0).Rows(rowIndex)

            repobject = row("obj_name")
            repproc = row("obj_proc")
            'repid.Text = row("obj_no")
            'repclass.Text = row("obj_class")
        Next
        ReportViewer1.LocalReport.ReportPath = "Reports\" & "CardRequest" & ".rdlc"
        'If Request.QueryString("r") = 0 Then
        '    ReportViewer1.LocalReport.ReportPath = "Report\" & repobject & ".rdlc" 'AccountEstatementAll.rdlc"

        'Else
        '    ReportViewer1.LocalReport.ReportPath = "Mandate\AccountEstatementAll.rdlc"
        '    'Response.Write("i am here ")
        'End If



        'Dim startdate As String = Request.QueryString("p") '"01-jan-2009"

        'Dim enddate As String = Request.QueryString("q") ' "01-sep-2009"
        'Dim chkall As Integer = Request.QueryString("r") '0
        'Dim account As String = Request.QueryString("s")
        ''Dim reptype As String = Request.QueryString("t") ' "Both"
        'luser = Context.User.Identity.Name
        'StrLocation = luser.Substring(luser.LastIndexOf("?") + 1)
        'struser = Mid(luser, 1, luser.LastIndexOf("?"))
        'gUser = struser
        'Dim lprodesc As String
        'Dim str_sel As String = "Rep_Statement '" & account & "','" & Format(CDate(startdate), "MM-dd-yyyy") & "','" & Format(CDate(enddate), "MM-dd-yyyy") & "'," & chkall & ""



        Me.SqlDataSource1.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("EazyConnect").ConnectionString

        'Response.Write(GetDatasetRecords())
        Me.SqlDataSource1.SelectCommand = String.Format("SELECT * FROM cardrequest WHERE RefNo = '{0}'", Request.QueryString("p"))


        Me.SqlDataSource1.SelectCommandType = SqlDataSourceCommandType.Text
        '
        Me.ReportViewer1.DataBind()

    End Sub
End Class
