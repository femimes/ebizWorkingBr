Imports System.Web.Security
Imports System.Security.Principal
Imports System.Threading
Imports System.Data.OleDb
Imports System
Imports System.Globalization
Imports System.Web.UI.HtmlControls
''Imports System.Web.Services
Imports System.Drawing.Color
Imports DevExpress.Web
Imports NETClass

Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports System.Drawing


Partial Class Mandate_PostReconciliation

    Inherits System.Web.UI.Page
    Public luser As String = Context.User.Identity.Name
    Public StrLocation As String = luser.Substring(luser.LastIndexOf("?") + 1)
    Public struser As String = Mid(luser, 1, luser.LastIndexOf("?"))
    Public guser As String = struser
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If (IsPostBack = False And IsCallback = False) Then
            lblTotalAmt.Text = FormatNumber(Request.QueryString("q") * Request.QueryString("s") * 0.01, 2)
            lblFeeAmt.Text = FormatNumber(Request.QueryString("r") * Request.QueryString("t") * 0.01, 2)
            lblBal.Text = FormatNumber(Request.QueryString("p"), 2)
            bindgrid()
            LoadApprovingOfficers()
        End If
    End Sub
    Protected Sub ASPxButton2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ASPxButton2.Click

    End Sub

    Sub bindgrid()

        Me.grids.DataSource = CreateDataSourceDet()

        Me.grids.DataBind()



    End Sub

    Function CreateDataSourceDet() As ICollection
        Dim ds As New DataSet
        'set the datagrid datasource to the dataset passed in

        Dim dt As New DataTable
        Dim dr As DataRow
        Dim icells As DataGridItem
        dt.Columns.Add(New DataColumn("SerialNo", GetType(String)))

        Dim ilenght As Integer = txtAcNos.Text.Trim, i As Integer
        For i = 1 To ilenght
            dr = dt.NewRow()
            dr(0) = i


            dt.Rows.Add(dr)
        Next


        Dim dv As New DataView(dt)
        Return dv


    End Function

    Protected Sub txtAcNos_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAcNos.TextChanged
        bindgrid()
    End Sub

    Protected Sub ASPxButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ASPxButton1.Click



        Dim ds As New DataSet
        luser = Context.User.Identity.Name
        StrLocation = luser.Substring(luser.LastIndexOf("?") + 1)
        struser = Mid(luser, 1, luser.LastIndexOf("?"))
        guser = struser
        Dim l_refNo As String = ""

        l_refNo = EazyClass.GetExtJournalNum(StrLocation)
        Classes.alertwindow(Me.Page, "A New Batch Reference Generated : - " & l_refNo, "Halt")
        Classes.RecordOp(String.Format("Sproc_AddJournal '{0}','Reconciliation Settlement',{1},'N','{2}','{3}',0", l_refNo, CDec(0), struser, StrLocation))
        'Dim rowIndex As Integer
        Dim chkerror As Integer = 0
        Dim item As DataGridItem
        For rowIndex As Integer = 0 To grids.VisibleRowCount - 1
            Dim AcctNameHead As GridViewDataColumn = CType(grids.Columns("AccountName"), GridViewDataColumn)
            Dim StatusHead As GridViewDataColumn = CType(grids.Columns("Status"), GridViewDataColumn)
            Dim AcctNoHead As GridViewDataColumn = CType(grids.Columns("AccountNo"), GridViewDataColumn)
            Dim AmtHead As GridViewDataColumn = CType(grids.Columns("Amount"), GridViewDataColumn)
            Dim TransTypeHead As GridViewDataColumn = CType(grids.Columns("TransactionType"), GridViewDataColumn)

            Dim lblAcctName As ASPxLabel = CType(grids.FindRowCellTemplateControl(rowIndex, AcctNameHead, "lblAccountName"), ASPxLabel)
            Dim lblStatusName As ASPxLabel = CType(grids.FindRowCellTemplateControl(rowIndex, StatusHead, "lblStatus"), ASPxLabel)
            Dim txtAcctNo As ASPxTextBox = CType(grids.FindRowCellTemplateControl(rowIndex, AcctNoHead, "txtAccountNo"), ASPxTextBox)
            Dim lblAmount As ASPxLabel = CType(grids.FindRowCellTemplateControl(rowIndex, AmtHead, "lblAmount"), ASPxLabel)
            Dim DrpTransType As ASPxComboBox = CType(grids.FindRowCellTemplateControl(rowIndex, TransTypeHead, "ASPxComboBox1"), ASPxComboBox)
            Dim l_debit As String = 0
            Dim l_credit As String = 0
            If lblStatusName.Text = "Valid AccountNo" And lblAmount.Text <> 0 Then


                If DrpTransType.Value = "Dr" Then
                    l_debit = lblAmount.Text
                Else
                    l_credit = lblAmount.Text
                End If
                Dim l_PostDate As String = NETClass.EazyClass.GetPostingDate
                Dim l_transDate As String = drpValueDate.Date 'Trim(item.Cells(1).Text)
                Dim l_accountno As String = txtAcctNo.Text 'Trim(item.Cells(2).Text)
                Dim l_transcode As String = "910" ' Trim(item.Cells(3).Text)
                Dim l_transcodeCr As String = "911" ' Trim(item.Cells(3).Text)
                Dim l_narrative As String = String.Format("Reconciliation Settlement @{0}", Format(drpValueDate.Date, "dd-MMM-yyyy"))  'pva Trim(item.Cells(4).Text)
                Dim l_description As String = String.Format("Reconciliation Settlement @{0}", Format(drpValueDate.Date, "dd-MMM-yyyy"))  ' Trim(item.Cells(5).Text)

                Dim l_bankcode As String = "" 'Trim(item.Cells(8).Text)
                Dim l_chequeno As String = l_accountno 'Trim(item.Cells(10).Text)
                Dim l_TransRef As String = l_refNo 'Trim(item.Cells(10).Text)
                Dim l_status As String = 1 'Trim(item.Cells(11).CssClass)

                If l_bankcode = "" Then
                    l_bankcode = l_chequeno
                End If


                Dim str_ins As String = String.Format("Sproc_AddExtTransCodeHistJornal '{0}','{1}','{2}','{3}','{4}','{5}','{6}' ,{7},{8},'{9}','{10}','{11}','{12}',{13},'{14}','{15}','{16}'", Format(CDate(l_PostDate), "MM-dd-yyyy"), Format(CDate(l_transDate), "MM-dd-yyyy"), l_accountno, l_transcode, l_transcodeCr, l_narrative, l_description, l_debit, l_credit, l_bankcode, l_chequeno, l_TransRef, l_refNo, struser, CInt(l_status), Me.offr2.Value, StrLocation)
                Classes.RecordOp(str_ins)

                'Response.Write(str_ins)

                ' Response.Write(str_ins)
            Else ' determines if their is error
                chkerror = 1
                lblStatusName.ForeColor = Color.Red

            End If
        Next
        If chkerror = 0 Then
            Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("EazyConnect").ConnectionString

            Dim Cmd As New SqlCommand, sqlConn As New SqlConnection(strcon), result As SqlParameter

            Cmd.CommandText = "Sproc_ApproveTransInfoAll"
            Cmd.Connection = sqlConn
            Cmd.CommandType = CommandType.StoredProcedure

            sqlConn.Open()


            Cmd.Parameters.Add(New SqlParameter("@reconUser", guser))
            Cmd.Parameters.Add(New SqlParameter("@reconStatus", 3))
            Cmd.Parameters.Add(New SqlParameter("@reconCheckStatus", 1))


            Cmd.ExecuteNonQuery()


            sqlConn.Close()
            Response.Write("<script>alert('Posting Successful') </script>")
        Else
            Response.Write("<script>alert('Posting has errors') </script>")
        End If

    End Sub

    Private Sub LoadApprovingOfficers()
        On Error Resume Next

        Dim ds As New DataSet
        luser = Context.User.Identity.Name
        StrLocation = luser.Substring(luser.LastIndexOf("?") + 1)
        struser = Mid(luser, 1, luser.LastIndexOf("?"))
        guser = struser
        ds = Classes.GetDataset(String.Format("Sproc_GetTransApprovingOfficers {0},2,'{1}','{2}','{3}'", CDec(200), NETClass.EazyClass.GetTransCodeInfo("002", 2), StrLocation, struser))

        offr2.DataSource = ds.Tables(0)
        offr2.TextField = "user_id"
        offr2.ValueField = "user_id"
        offr2.DataBind()

    End Sub

    Protected Sub txtAccountNo_TextChanged1(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtAccountNo As ASPxTextBox = CType(sender, ASPxTextBox)
        'Dim lblAcctName As ASPxLabel = CType(grids.GetRowValues(1, "AccountName"), ASPxLabel)
        Dim AcctNameHead As GridViewDataColumn = CType(grids.Columns("AccountName"), GridViewDataColumn)
        Dim StatusHead As GridViewDataColumn = CType(grids.Columns("Status"), GridViewDataColumn)

        Dim lblAcctName As ASPxLabel = CType(grids.FindRowCellTemplateControl(txtAccountNo.CssClass - 1, AcctNameHead, "lblAccountName"), ASPxLabel)
        Dim lblStatusName As ASPxLabel = CType(grids.FindRowCellTemplateControl(txtAccountNo.CssClass - 1, StatusHead, "lblStatus"), ASPxLabel)

        Dim l_AcctName As String = EazyClass.GetAccountName(txtAccountNo.Text)
        If String.IsNullOrEmpty(l_AcctName) = False Then
            lblAcctName.Text = l_AcctName
            lblStatusName.Text = "Valid AccountNo"
        Else
            lblStatusName.Text = "Invalid AccountNo"
        End If

        'grids.GetSelectedFieldValues(1, "AccountName"). = 23

        'Response.Write(txtAccountNo.CssClass)
    End Sub


    Protected Sub txtProp_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtProp As ASPxTextBox = CType(sender, ASPxTextBox)
        Dim AmountHead As GridViewDataColumn = CType(grids.Columns("Amount"), GridViewDataColumn)
        Dim AmountTypeHead As GridViewDataColumn = CType(grids.Columns("AmountType"), GridViewDataColumn)


        Dim DrpAmtTypeName As ASPxComboBox = CType(grids.FindRowCellTemplateControl(txtProp.CssClass - 1, AmountTypeHead, "drpAmtType"), ASPxComboBox)
        Dim lblAmount As ASPxLabel = CType(grids.FindRowCellTemplateControl(txtProp.CssClass - 1, AmountHead, "lblAmount"), ASPxLabel)

        If DrpAmtTypeName.Value = 1 Then
            lblAmount.Text = (txtProp.Text * 0.01) * lblTotalAmt.Text
        ElseIf DrpAmtTypeName.Value = 2 Then
            lblAmount.Text = (txtProp.Text * 0.01) * lblFeeAmt.Text
        ElseIf DrpAmtTypeName.Value = 3 Then
            lblAmount.Text = (txtProp.Text * 0.01) * lblBal.Text
        ElseIf DrpAmtTypeName.Value = 4 Then
            lblAmount.Text = ((txtProp.Text * 0.01) * lblFeeAmt.Text) + lblTotalAmt.Text
        ElseIf DrpAmtTypeName.Value = 5 Then
            lblAmount.Text = ((txtProp.Text * 0.01) * lblTotalAmt.Text) + lblFeeAmt.Text
        End If


    End Sub

    Protected Sub drpAmtType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim DrpAmtTypeName As ASPxComboBox = CType(sender, ASPxComboBox)

        Dim AmountHead As GridViewDataColumn = CType(grids.Columns("Amount"), GridViewDataColumn)
        Dim AmountTypeHead As GridViewDataColumn = CType(grids.Columns("AmountType"), GridViewDataColumn)
        Dim PropHead As GridViewDataColumn = CType(grids.Columns("Proportion"), GridViewDataColumn)

        Dim txtProp As ASPxTextBox = CType(grids.FindRowCellTemplateControl(DrpAmtTypeName.CssClass - 1, PropHead, "txtProp"), ASPxTextBox) 'CType(sender, ASPxTextBox)
        Dim lblAmount As ASPxLabel = CType(grids.FindRowCellTemplateControl(DrpAmtTypeName.CssClass - 1, AmountHead, "lblAmount"), ASPxLabel)

        If DrpAmtTypeName.Value = 1 Then
            lblAmount.Text = (txtProp.Text * 0.01) * lblTotalAmt.Text
        ElseIf DrpAmtTypeName.Value = 2 Then
            lblAmount.Text = (txtProp.Text * 0.01) * lblFeeAmt.Text
        ElseIf DrpAmtTypeName.Value = 3 Then
            lblAmount.Text = (txtProp.Text * 0.01) * lblBal.Text
        ElseIf DrpAmtTypeName.Value = 4 Then
            lblAmount.Text = ((txtProp.Text * 0.01) * lblFeeAmt.Text) + lblTotalAmt.Text
        ElseIf DrpAmtTypeName.Value = 5 Then
            lblAmount.Text = ((txtProp.Text * 0.01) * lblTotalAmt.Text) + lblFeeAmt.Text
        End If
    End Sub


End Class
