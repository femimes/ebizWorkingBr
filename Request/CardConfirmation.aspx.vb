Imports System
Imports EazyBankNet
Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports DevExpress.Web
Partial Class Mandate_CardConfirmation
    Inherits System.Web.UI.Page
    Public luser As String
    Public StrLocation As String
    Public struser As String
    Public guser As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If (IsPostBack = False And IsCallback = False) Then
            txtuserID.Text = guser
            'LoadBoxes()
            'LoadGrid()
            ctrlGrid.DataBind()
            Me.ctrlGrid.PageIndex = 0
        End If

    End Sub



    Protected Sub ASPxCheckBox2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chkAll As ASPxCheckBox = CType(sender, ASPxCheckBox)
        Dim AccountNoHead As GridViewDataColumn = CType(ctrlGrid.Columns("RequestTitle"), GridViewDataColumn)

        Dim VisibleRowsCount As Integer = 0

        If ((ctrlGrid.VisibleRowCount / ctrlGrid.SettingsPager.PageSize) > 1) Then
            VisibleRowsCount = ctrlGrid.SettingsPager.PageSize
        Else
            VisibleRowsCount = ctrlGrid.VisibleRowCount
        End If



        If (chkAll.Checked = True) Then
            Dim rowIndex As Integer


            For rowIndex = 0 To VisibleRowsCount - 1



                Dim AccountNo As ASPxCheckBox = CType(ctrlGrid.FindRowCellTemplateControl(rowIndex, AccountNoHead, "ASPxCheckBox1"), ASPxCheckBox)

                AccountNo.Checked = True
            Next
        Else
            Dim rowIndex As Integer
            For rowIndex = 0 To VisibleRowsCount - 1



                'CheckBox ChkFirstName = (CheckBox)ctrlGrid.FindRowCellTemplateControl(rowIndex, columnFirstName, "CheckBox1")
                Dim AccountNo As ASPxCheckBox = CType(ctrlGrid.FindRowCellTemplateControl(rowIndex, AccountNoHead, "ASPxCheckBox1"), ASPxCheckBox)

                AccountNo.Checked = False
            Next
        End If
    End Sub

    Protected Sub BtnApprove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnApprove.Click

        Dim str_sel As String, strauthUser As String, strgetauthcookies As String
        strgetauthcookies = Request.Cookies("LogonUserID").Value
        If strgetauthcookies <> Nothing Then

            guser = strgetauthcookies.Substring(0, strgetauthcookies.LastIndexOf("?"))
        Else
            luser = Context.User.Identity.Name
            StrLocation = luser.Substring(luser.LastIndexOf("?") + 1)
            struser = luser.Substring(0, luser.LastIndexOf("?")) ' Mid(luser, 1, luser.LastIndexOf("?"))
            guser = struser

        End If

        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("EazyConnect").ConnectionString

        Dim AccountNoHead As GridViewDataColumn = CType(ctrlGrid.Columns("AccountNo"), GridViewDataColumn)
        Dim VisibleRowsCount As Integer = 0

        If ((ctrlGrid.VisibleRowCount / ctrlGrid.SettingsPager.PageSize) > 1) Then
            VisibleRowsCount = ctrlGrid.SettingsPager.PageSize
        Else
            VisibleRowsCount = ctrlGrid.VisibleRowCount
        End If
        Dim rowIndex As Integer


        For rowIndex = 0 To VisibleRowsCount - 1

            Dim AccountNo As ASPxCheckBox = CType(ctrlGrid.FindRowCellTemplateControl(rowIndex, AccountNoHead, "ASPxCheckBox1"), ASPxCheckBox)

            If AccountNo.Checked = True Then
                Dim Cmd As New SqlCommand, sqlConn As New SqlConnection(strcon), result As SqlParameter
                'Dim DisplNameHead As GridViewDataColumn = CType(ctrlGrid.Columns("AccountName"), GridViewDataColumn)
                'Dim AddressHead As GridViewDataColumn = CType(ctrlGrid.Columns("Address"), GridViewDataColumn)
                'Dim CityHead As GridViewDataColumn = CType(ctrlGrid.Columns("city"), GridViewDataColumn)

                'Dim SeqHead As GridViewDataColumn = CType(ctrlGrid.Columns("Address"), GridViewDataColumn)
                'Dim ColCardExpiry As GridViewDataColumn = CType(ctrlGrid.Columns("CardExipryDate"), GridViewDataColumn)

                'Dim CounterHead As GridViewDataColumn = CType(ctrlGrid.Columns("city"), GridViewDataColumn)
                'Dim DOBHead As GridViewDataColumn = CType(ctrlGrid.Columns("DOB"), GridViewDataColumn)
                'Dim DOBHead As GridViewDataColumn = CType(ctrlGrid.Columns("DOB"), GridViewDataColumn)
                'Dim DisplName As ASPxLabel = CType(ctrlGrid.FindRowCellTemplateControl(rowIndex, ColCardExpiry, "ASPxTextBox2"), ASPxLabel)

                'Dim DisplName As ASPxTextBox = CType(ctrlGrid.FindRowCellTemplateControl(rowIndex, DisplNameHead, "ASPxTextBox2"), ASPxTextBox)
                'Dim AddressName As ASPxTextBox = CType(ctrlGrid.FindRowCellTemplateControl(rowIndex, AddressHead, "ASPxTextBox1"), ASPxTextBox)
                'Dim CityName As ASPxTextBox = CType(ctrlGrid.FindRowCellTemplateControl(rowIndex, CityHead, "ASPxTextBox3"), ASPxTextBox)
                'Dim DOB As ASPxDateEdit = CType(ctrlGrid.FindRowCellTemplateControl(rowIndex, DOBHead, "ASPxDateEdit1"), ASPxDateEdit)

                'If String.IsNullOrEmpty(DOB.Date.ToShortDateString()) = True Then
                '    DOB.Date = "1970-01-01"

                'End If

                '@RefNo Varchar(50),
                '@ComfirmBy Varchar(50),
                '@PANno Varchar(50),
                '@CardCounter int,
                '@AccountNo Varchar(50),
                '@CardExipryDate DateTime,
                '@CardIssueDate DateTime,
                '@status int

                Dim CardCounter As Integer = 0
                CardCounter = ctrlGrid.GetRowValues(rowIndex, "CardIssueCounter") + 1
                Cmd.CommandText = "Sproc_ApproveCardRequestConfirm"
                Cmd.Connection = sqlConn
                Cmd.CommandType = CommandType.StoredProcedure

                sqlConn.Open()
                'Cmd.Parameters.Add(New SqlParameter("@RefNo", txtref_no.Text))
                Cmd.Parameters.Add(New SqlParameter("@AccountNo", AccountNo.Text))
                'ctrlGrid.GetRowValues(17, "")
                'Cmd.Parameters.Add(New SqlParameter("@CardExipryDate", ctrlGrid.GetRowValuesByKeyValue(AccountNo.Text, "CardExipryDate")))
                'Cmd.Parameters.Add(New SqlParameter("@CardIssueDate", ctrlGrid.GetRowValuesByKeyValue(AccountNo.Text, "CardIssueDate")))
                'Cmd.Parameters.Add(New SqlParameter("@CardExipryDate", ctrlGrid.GetRowValues(rowIndex, ColCardExpiry)))
                Cmd.Parameters.Add(New SqlParameter("@CardIssueDate", ctrlGrid.GetRowValues(rowIndex, "CardIssueDate")))
                Cmd.Parameters.Add(New SqlParameter("@CardExipryDate", ctrlGrid.GetRowValues(rowIndex, "CardExpiryDate")))

                Cmd.Parameters.Add(New SqlParameter("@PANno", AccountNo.ToolTip))

                Cmd.Parameters.Add(New SqlParameter("@CardCounter", CardCounter))
                Cmd.Parameters.Add(New SqlParameter("@status", 7))


                Cmd.Parameters.Add(New SqlParameter("@RefNo", drpBatchRefNo.Value))

                Cmd.Parameters.Add(New SqlParameter("@ComfirmBy", guser))




                Cmd.ExecuteNonQuery()


                sqlConn.Close()

            End If
        Next
        Response.Redirect("CardConfirmation.aspx")
    End Sub

    Protected Sub btncancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncancel.Click
        Response.Redirect("CardConfirmation.aspx")
    End Sub

    Protected Sub BtnDisapprove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnDisapprove.Click

        Dim str_sel As String, strauthUser As String, strgetauthcookies As String
        strgetauthcookies = Request.Cookies("LogonUserID").Value
        If strgetauthcookies <> Nothing Then

            guser = strgetauthcookies.Substring(0, strgetauthcookies.LastIndexOf("?"))
        Else
            luser = Context.User.Identity.Name
            StrLocation = luser.Substring(luser.LastIndexOf("?") + 1)
            struser = luser.Substring(0, luser.LastIndexOf("?")) ' Mid(luser, 1, luser.LastIndexOf("?"))
            guser = struser

        End If

        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("EazyConnect").ConnectionString

        Dim AccountNoHead As GridViewDataColumn = CType(ctrlGrid.Columns("AccountNo"), GridViewDataColumn)
        Dim VisibleRowsCount As Integer = 0

        If ((ctrlGrid.VisibleRowCount / ctrlGrid.SettingsPager.PageSize) > 1) Then
            VisibleRowsCount = ctrlGrid.SettingsPager.PageSize
        Else
            VisibleRowsCount = ctrlGrid.VisibleRowCount
        End If
        Dim rowIndex As Integer


        For rowIndex = 0 To VisibleRowsCount - 1

            Dim AccountNo As ASPxCheckBox = CType(ctrlGrid.FindRowCellTemplateControl(rowIndex, AccountNoHead, "ASPxCheckBox1"), ASPxCheckBox)

            If AccountNo.Checked = True Then
                Dim Cmd As New SqlCommand, sqlConn As New SqlConnection(strcon), result As SqlParameter
                'Dim DisplNameHead As GridViewDataColumn = CType(ctrlGrid.Columns("AccountName"), GridViewDataColumn)
                'Dim AddressHead As GridViewDataColumn = CType(ctrlGrid.Columns("Address"), GridViewDataColumn)
                'Dim CityHead As GridViewDataColumn = CType(ctrlGrid.Columns("city"), GridViewDataColumn)

                'Dim SeqHead As GridViewDataColumn = CType(ctrlGrid.Columns("Address"), GridViewDataColumn)
                'Dim ColCardExpiry As GridViewDataColumn = CType(ctrlGrid.Columns("CardExipryDate"), GridViewDataColumn)

                'Dim CounterHead As GridViewDataColumn = CType(ctrlGrid.Columns("city"), GridViewDataColumn)
                'Dim DOBHead As GridViewDataColumn = CType(ctrlGrid.Columns("DOB"), GridViewDataColumn)
                'Dim DOBHead As GridViewDataColumn = CType(ctrlGrid.Columns("DOB"), GridViewDataColumn)
                'Dim DisplName As ASPxLabel = CType(ctrlGrid.FindRowCellTemplateControl(rowIndex, ColCardExpiry, "ASPxTextBox2"), ASPxLabel)

                'Dim DisplName As ASPxTextBox = CType(ctrlGrid.FindRowCellTemplateControl(rowIndex, DisplNameHead, "ASPxTextBox2"), ASPxTextBox)
                'Dim AddressName As ASPxTextBox = CType(ctrlGrid.FindRowCellTemplateControl(rowIndex, AddressHead, "ASPxTextBox1"), ASPxTextBox)
                'Dim CityName As ASPxTextBox = CType(ctrlGrid.FindRowCellTemplateControl(rowIndex, CityHead, "ASPxTextBox3"), ASPxTextBox)
                'Dim DOB As ASPxDateEdit = CType(ctrlGrid.FindRowCellTemplateControl(rowIndex, DOBHead, "ASPxDateEdit1"), ASPxDateEdit)

                'If String.IsNullOrEmpty(DOB.Date.ToShortDateString()) = True Then
                '    DOB.Date = "1970-01-01"

                'End If

                '@RefNo Varchar(50),
                '@ComfirmBy Varchar(50),
                '@PANno Varchar(50),
                '@CardCounter int,
                '@AccountNo Varchar(50),
                '@CardExipryDate DateTime,
                '@CardIssueDate DateTime,
                '@status int

                Dim CardCounter As Integer = 0
                CardCounter = ctrlGrid.GetRowValues(rowIndex, "CardIssueCounter") + 1
                Cmd.CommandText = "Sproc_ApproveCardRequestDisConfirm"
                Cmd.Connection = sqlConn
                Cmd.CommandType = CommandType.StoredProcedure

                sqlConn.Open()
                'Cmd.Parameters.Add(New SqlParameter("@RefNo", txtref_no.Text))
                Cmd.Parameters.Add(New SqlParameter("@AccountNo", AccountNo.Text))
                'ctrlGrid.GetRowValues(17, "")
                'Cmd.Parameters.Add(New SqlParameter("@CardExipryDate", ctrlGrid.GetRowValuesByKeyValue(AccountNo.Text, "CardExipryDate")))
                'Cmd.Parameters.Add(New SqlParameter("@CardIssueDate", ctrlGrid.GetRowValuesByKeyValue(AccountNo.Text, "CardIssueDate")))
                'Cmd.Parameters.Add(New SqlParameter("@CardExipryDate", ctrlGrid.GetRowValues(rowIndex, ColCardExpiry)))
                Cmd.Parameters.Add(New SqlParameter("@CardIssueDate", ctrlGrid.GetRowValues(rowIndex, "CardIssueDate")))
                Cmd.Parameters.Add(New SqlParameter("@CardExipryDate", ctrlGrid.GetRowValues(rowIndex, "CardExpiryDate")))

                Cmd.Parameters.Add(New SqlParameter("@PANno", AccountNo.ToolTip))

                Cmd.Parameters.Add(New SqlParameter("@CardCounter", CardCounter))
                Cmd.Parameters.Add(New SqlParameter("@status", 20))


                Cmd.Parameters.Add(New SqlParameter("@RefNo", drpBatchRefNo.Value))

                Cmd.Parameters.Add(New SqlParameter("@ComfirmBy", guser))




                Cmd.ExecuteNonQuery()


                sqlConn.Close()

            End If
        Next
        Response.Redirect("CardConfirmation.aspx")
    End Sub
End Class
