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

'Imports CrystalDecisions
Imports Microsoft.Reporting.WebForms

'Imports iTextSharp.text
'Imports iTextSharp.text.html.simpleparser
'Imports iTextSharp.text.pdf

Partial Class eOperations_FrmShedBulkEmail
    Inherits System.Web.UI.Page
    Public luser As String = Context.User.Identity.Name
    Public StrLocation As String = "101" 'luser.Substring(luser.LastIndexOf("?") + 1)
    Public struser As String = "Axadmin" ' Mid(luser, 1, luser.LastIndexOf("?"))
    Public guser As String = struser
    Public l_senderlist As String
    Public dt As New DataTable

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If (IsPostBack = False And IsCallback = False) Then
            txtUserID.Text = guser

            LoadGrid()

            drpStatementtype.Value = "N"
            drpbranches.Value = "101"
            drp_products.Value = "103"
            dateIssue.Date = Today.Date
            dateexpiry.Date = Today.Date.AddDays(1.0)

            Me.ASPxGridView1.PageIndex = 0
        End If
    End Sub

    Sub LoadGrid()
        luser = Context.User.Identity.Name
        ' StrLocation = luser.Substring(luser.LastIndexOf("?") + 1)
        'struser = Mid(luser, 1, luser.LastIndexOf("?"))
        guser = struser
        If String.IsNullOrEmpty(Trim(txtSpecificAcct.Text)) = False Then



            Dim strgetAcctDetails As String = String.Format("GetAllAccountCardRequest '{0}','{1}'", drp_products.Value, drpbranches.Value)
            SqlacctDetails.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("EazyConnect").ConnectionString
            SqlacctDetails.SelectCommand = strgetAcctDetails
            SqlacctDetails.FilterExpression = String.Format("accountno in ({0})", Trim(txtSpecificAcct.Text))

            SqlacctDetails.SelectCommandType = SqlDataSourceCommandType.Text

        Else

            Dim strgetAcctDetails As String = String.Format("GetAllAccountCardRequest '{0}','{1}'", drp_products.Value, drpbranches.Value)
            SqlacctDetails.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("EazyConnect").ConnectionString
            SqlacctDetails.SelectCommand = strgetAcctDetails
            SqlacctDetails.SelectCommandType = SqlDataSourceCommandType.Text

        End If
        'Dim strgetOffr As String = String.Format("Sproc_GetTransApprovingOfficers {0},2,'{1}','{2}','{3}'", CDec(200), gEazy.GetTransCodeInfo("002", 2), StrLocation, guser)
        'sqlAppr.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("EazyConnect").ConnectionString
        'sqlAppr.SelectCommand = strgetOffr
        'sqlAppr.SelectCommandType = SqlDataSourceCommandType.Text

        'Response.Write(strgetAcctDetails)
    End Sub

    Protected Sub ASPxGridView1_BeforePerformDataSelect(ByVal sender As Object, ByVal e As System.EventArgs) Handles ASPxGridView1.BeforePerformDataSelect
        LoadGrid()
    End Sub

    Protected Sub drp_products_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drp_products.SelectedIndexChanged

        LoadGrid()
    End Sub

    Protected Sub drpbranches_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpbranches.SelectedIndexChanged

        LoadGrid()
    End Sub

    Protected Sub txtSpecificAcct_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSpecificAcct.TextChanged

        LoadGrid()
    End Sub


    Protected Sub cmdClear_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles cmdClear.Command
        ASPxListBox1.Dispose()

        ASPxGridView1.DataBind()
    End Sub

    Protected Sub cmdClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClear.Click
        ASPxListBox1.Items.Clear()
    End Sub

    Protected Sub Button6_Click(sender As Object, e As System.EventArgs) Handles Button6.Click
        Dim iferror As String = ValidateEntries()
        If iferror = "" Then



            Dim str_sel As String, strauthUser As String, strgetauthcookies As String
            strgetauthcookies = "AXAdmin?101" 'Request.Cookies("LogonUserID").Value
            If strgetauthcookies <> Nothing Then

                guser = strgetauthcookies.Substring(0, strgetauthcookies.LastIndexOf("?"))
            Else
                luser = Context.User.Identity.Name
                StrLocation = luser.Substring(luser.LastIndexOf("?") + 1)
                struser = luser.Substring(0, luser.LastIndexOf("?")) ' Mid(luser, 1, luser.LastIndexOf("?"))
                guser = struser

            End If
            Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("EazyConnect").ConnectionString
            Dim CmdMst As New SqlCommand, sqlConnMst As New SqlConnection(strcon), resultMst As SqlParameter

            If Me.txtref_no.Text = Nothing Then

                txtref_no.Text = Guid.NewGuid().ToString().Substring(0, 12).ToUpper()
                Classes.alertwindow(Me.Page, "A New Batch Request Reference Generated : - " & Me.txtref_no.Text, "Halt")



                'Section for master records

                CmdMst.CommandText = "Sproc_AddAcctBulkRequestMst"
                CmdMst.Connection = sqlConnMst
                CmdMst.CommandType = CommandType.StoredProcedure

                sqlConnMst.Open()
                CmdMst.Parameters.Add(New SqlParameter("@AccountNo", ""))

                CmdMst.Parameters.Add(New SqlParameter("@Email", ""))
                CmdMst.Parameters.Add(New SqlParameter("@IssueDate", dateIssue.Date))

                CmdMst.Parameters.Add(New SqlParameter("@ExpiryDate", dateexpiry.Date))
                CmdMst.Parameters.Add(New SqlParameter("@RecurrentCounter", Boolean.Parse(chkRecurrent.Checked)))
                CmdMst.Parameters.Add(New SqlParameter("@SeqNum", "0"))
                CmdMst.Parameters.Add(New SqlParameter("@RequestTitle", txtTitle.Text))
                CmdMst.Parameters.Add(New SqlParameter("@Status", 0))

                CmdMst.Parameters.Add(New SqlParameter("@RefNo", txtref_no.Text))

                 CmdMst.Parameters.Add(New SqlParameter("@AddedBy", guser))

                '  Cmd.Parameters.Add(New SqlParameter("@ApprovedBy", drp_appr.Value))
                CmdMst.Parameters.Add(New SqlParameter("@ApprovedBy", "System"))

                CmdMst.Parameters.Add(New SqlParameter("@StatementType", drpStatementtype.Value))
                CmdMst.Parameters.Add(New SqlParameter("@TransTime", drpTime.Text))
                CmdMst.ExecuteNonQuery()


                sqlConnMst.Close()


                'End sections


 


                For i = 0 To (ASPxListBox1.Items.Count - 1)

                    'Dim AccountNo As ASPxCheckBox = CType(ASPxGridView1.FindRowCellTemplateControl(rowIndex, AccountNoHead, "ASPxCheckBox1"), ASPxCheckBox)
                    Dim AccountNo = ASPxListBox1.Items(i).Value
                    'If AccountNo.Checked = True Then
                    Dim Cmd As New SqlCommand, sqlConn As New SqlConnection(strcon), result As SqlParameter


                    Dim strgetToAddress As String = String.Format("SpSMSNoAccountNo '{0}'", ASPxListBox1.Items(i).Value)
                    Dim PhoneNo As String = Nothing
                    If NETClass.Classes.AXGetDataset(strgetToAddress).Tables(0).Rows.Count > 0 Then
                        PhoneNo = NETClass.Classes.AXGetDataset(strgetToAddress).Tables(0).Rows(0)("msg_to").ToString()
                    End If

                    'Dim PhoneNo As ASPxTextBox = CType(ASPxGridView1.FindRowCellTemplateControl(rowIndex, PhoneNoHead, "ASPxTextBox4"), ASPxTextBox)

                    Dim str_ToAddress As String = NETClass.Classes.AxGetField(String.Format("Sproc_GetAccountEmail '{0}'", AccountNo))

                    If str_ToAddress = Nothing Then
                        str_ToAddress = ""
                    End If
                    If PhoneNo = Nothing Then
                        PhoneNo = ""
                    End If
                    Cmd.CommandText = "Sproc_AddAcctBulkRequestDetails"
                    Cmd.Connection = sqlConn
                    Cmd.CommandType = CommandType.StoredProcedure

                    sqlConn.Open()
                    Cmd.Parameters.Add(New SqlParameter("@AccountNo", AccountNo))
                    Cmd.Parameters.Add(New SqlParameter("@Email", str_ToAddress))
                    Cmd.Parameters.Add(New SqlParameter("@PhoneNo", PhoneNo))
                    Cmd.Parameters.Add(New SqlParameter("@RefNo", txtref_no.Text))


                    Cmd.Parameters.Add(New SqlParameter("@AddedBy", guser))

                    

                    Cmd.ExecuteNonQuery()


                    sqlConn.Close()
                    'End If
                Next

                Response.Write("<script>alert('Request Successful') </script>")
                txtref_no.Text = Nothing
            End If



        Else
            Dim stringinfo As String = String.Format("<script>alert('{0}') </script>", iferror)
            Response.Write(stringinfo)
            ASPxGridView1.DataBind()

        End If
    End Sub

    Protected Sub Button7_Click(sender As Object, e As System.EventArgs) Handles Button7.Click
        Response.Redirect("FrmShedBulkEmail.aspx")
    End Sub


    Function ValidateEntries() As String
        Dim ds As New DataSet, item As DataGridItem, str_err As String

        'If drp_appr.Value = "" Then
        '    str_err = str_err + drp_appr.Text + "<br>" + ","

        'End If

        'Public luser As String = Context.User.Identity.Name
        'Public StrLocation As String = luser.Substring(luser.LastIndexOf("?") + 1)
        'Public struser As String = Mid(luser, 1, luser.LastIndexOf("?"))
        'Public guser As String = struser


        Return str_err
    End Function
    Sub changeDates()
        Dim strTitleSubject As String = "Monthly"
        If drpStatementtype.Value = "M" Then
            dateexpiry.Date = dateIssue.Date.AddMonths(1).AddDays(-1)
            dateexpiry.ReadOnly = True
            strTitleSubject = String.Format("Monthly Statement Of Account For {0} {1}", MonthName(dateIssue.Date.Month), dateIssue.Date.Year)
        ElseIf drpStatementtype.Value = "N" Then
            'dateexpiry.Date = dateIssue.Date.AddMonths(1)
            dateexpiry.ReadOnly = False
            strTitleSubject = String.Format(" Statement Of Account ", MonthName(dateIssue.Date.Month), dateIssue.Date.Year)
        ElseIf drpStatementtype.Value = "W" Then
            dateexpiry.Date = dateIssue.Date.AddDays(7)
            dateexpiry.ReadOnly = True
            strTitleSubject = String.Format("Weekly Statement Of Account From {2} To {3}", MonthName(dateIssue.Date.Month), dateIssue.Date.Year, dateIssue.Date, dateexpiry.Date)

        ElseIf drpStatementtype.Value = "Q" Then
            dateexpiry.Date = dateIssue.Date.AddMonths(3).AddDays(-1)
            dateexpiry.ReadOnly = True
            strTitleSubject = String.Format("Quartely Statement Of Account From {2} To {3}", MonthName(dateIssue.Date.Month), dateIssue.Date.Year, dateIssue.Date, dateexpiry.Date)
        ElseIf drpStatementtype.Value = "Y" Then
            dateexpiry.Date = dateIssue.Date.AddYears(1).AddDays(-1)
            dateexpiry.ReadOnly = True
            strTitleSubject = String.Format("Yearly Statement Of Account From {2} To {3}", MonthName(dateIssue.Date.Month), dateIssue.Date.Year, dateIssue.Date, dateexpiry.Date)
        ElseIf drpStatementtype.Value = "D" Then
            dateexpiry.Date = dateIssue.Date.AddDays(1)
            dateexpiry.ReadOnly = True
            strTitleSubject = String.Format("Daily Statement Of Account From {2} To {3}", MonthName(dateIssue.Date.Month), dateIssue.Date.Year, dateIssue.Date, dateexpiry.Date)

        End If
    End Sub

    Protected Sub dateIssue_DateChanged(sender As Object, e As System.EventArgs) Handles dateIssue.DateChanged
        changeDates()
    End Sub

    Protected Sub drpStatementtype_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles drpStatementtype.SelectedIndexChanged
        changeDates()
    End Sub
End Class
