Imports System
Imports EazyBankNet
Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports DevExpress.Web


Partial Class Request_CardLinkDeAllo
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

    Sub txtAccountNoChnaged(sender As Object, e As System.EventArgs)

        Dim AccountNoHead As GridViewDataColumn = CType(ctrlGrid.Columns("Link Account"), GridViewDataColumn)
        Dim txtacctNo As ASPxTextBox = CType(sender, ASPxTextBox)
        Dim lblLinkAcctName As ASPxLabel = CType(ctrlGrid.FindRowCellTemplateControlByKey(txtacctNo.ToolTip(), AccountNoHead, "lblLinkAcctName"), ASPxLabel)

        Dim strGetAccountName As String = String.Format(" SELECT isnull(EazybankAX.dbo.FnAccountName('{0}'),'Invalid Account')", txtacctNo.Text)
        lblLinkAcctName.Text = NETClass.Classes.SqlDbGetDataset(strGetAccountName).Tables(0).Rows(0).Item(0).ToString()

        If NETClass.Classes.SqlDbGetDataset(String.Format("SpCheckAccountCardLink '{0}'", txtacctNo.Text)).Tables(0).Rows.Count > 0 Then

            lblLinkAcctName.Text = "Invalid, account already mapped"
            lblLinkAcctName.ForeColor = Drawing.Color.Red
            lblLinkAcctName.Font.Bold = True


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
        Dim Displinkact As GridViewDataColumn = CType(ctrlGrid.Columns("Link Account"), GridViewDataColumn)
        Dim DispCvv As GridViewDataColumn = CType(ctrlGrid.Columns("Card CVV"), GridViewDataColumn)
        ' Dim DispCvv As GridViewDataColumn = CType(ctrlGrid.Columns(8), GridViewDataColumn)
        Dim DispPin As GridViewDataColumn = CType(ctrlGrid.Columns("Card PIN"), GridViewDataColumn)
        Dim DispExpiry As GridViewDataColumn = CType(ctrlGrid.Columns("Card ExpiryDate"), GridViewDataColumn)
        Dim DispIssueDate As GridViewDataColumn = CType(ctrlGrid.Columns("Card IssueDate"), GridViewDataColumn)
        Dim DispPanNo As GridViewDataColumn = CType(ctrlGrid.Columns("PanNo"), GridViewDataColumn)

        Dim VisibleRowsCount As Integer = 0

        If ((ctrlGrid.VisibleRowCount / ctrlGrid.SettingsPager.PageSize) > 1) Then
            VisibleRowsCount = ctrlGrid.SettingsPager.PageSize
        Else
            VisibleRowsCount = ctrlGrid.VisibleRowCount
        End If
        Dim rowIndex As Integer


        'For rowIndex = 0 To VisibleRowsCount - 1

        For Each key As String In ctrlGrid.GetSelectedFieldValues("accountno")




            'Dim AccountNo As ASPxCheckBox = CType(ctrlGrid.GetRowValuesByKeyValue(key, "ASPxCheckBox1"), ASPxCheckBox)
            If ctrlGrid.Selection.IsRowSelectedByKey(key) = True Then
                Dim AccountNo As String = key
                Dim PANno As String = ctrlGrid.GetRowValuesByKeyValue(key, "PANno").ToString()
                Dim SeqNum As String = ctrlGrid.GetRowValuesByKeyValue(key, "SeqNum").ToString()
                'Dim CardIssueDate As String = ctrlGrid.GetRowValuesByKeyValue(key, "CardIssueDate")
                'Dim CardExpiryDate As String = ctrlGrid.GetRowValuesByKeyValue(key, "CardExpiryDate")

                'If ctrlGrid.GetRowValuesByKeyValue(key, "SeqNum").ToString() <> Nothing Then
                '    SeqNum = ctrlGrid.GetRowValuesByKeyValue(key, "SeqNum").ToString()
                'End If

                'If AccountNo.Checked = True Then
                Dim Cmd As New SqlCommand, sqlConn As New SqlConnection(strcon), result As SqlParameter

                'Dim DisplName As ASPxLabel = CType(ctrlGrid.FindRowCellTemplateControl(rowIndex, ColCardExpiry, "ASPxTextBox2"), ASPxLabel)
                'Dim txtLinkAcctNo As ASPxTextBox = CType(ctrlGrid.FindRowCellTemplateControlByKey(key, Displinkact, "txtAccountNo"), ASPxTextBox)



                'Dim lblLinkAcctName As ASPxLabel = CType(ctrlGrid.FindRowCellTemplateControlByKey(key, Displinkact, "lblLinkAcctName"), ASPxLabel)

                'Dim txtCVV As ASPxTextBox = CType(ctrlGrid.FindRowCellTemplateControlByKey(key, DispCvv, "txtCardVV"), ASPxTextBox)
                'Dim txtCardPIN As ASPxTextBox = CType(ctrlGrid.FindRowCellTemplateControlByKey(key, DispPin, "txtCardPIN"), ASPxTextBox)
                'Dim txtCardNo = CType(ctrlGrid.FindRowCellTemplateControlByKey(key, DispPanNo, "LblPanNo"), ASPxLabel) 'ctrlGrid.GetRowValuesByKeyValue(txtLinkAcctNo.Text, "PANno")
                'Dim txtMonth As ASPxLabel = CType(ctrlGrid.FindRowCellTemplateControlByKey(key, DispExpiry, "ASPxLabel1"), ASPxLabel) ' ctrlGrid.GetRowValues(rowIndex, "Card ExpiryDate")

                'Dim txtStartdate As ASPxLabel = CType(ctrlGrid.FindRowCellTemplateControlByKey(key, DispExpiry, "ASPxLabel1"), ASPxLabel) 'ctrlGrid.GetRowValuesByKeyValue(rowIndex, "Card IssueDate")






                'Dim AddressName As ASPxTextBox = CType(ctrlGrid.FindRowCellTemplateControl(rowIndex, AddressHead, "ASPxTextBox1"), ASPxTextBox)
                'Dim CityName As ASPxTextBox = CType(ctrlGrid.FindRowCellTemplateControl(rowIndex, CityHead, "ASPxTextBox3"), ASPxTextBox)
                'Dim DOB As ASPxDateEdit = CType(ctrlGrid.FindRowCellTemplateControl(rowIndex, DOBHead, "ASPxDateEdit1"), ASPxDateEdit)

                'If String.IsNullOrEmpty(DOB.Date.ToShortDateString()) = True Then
                '    DOB.Date = "1970-01-01"

                'End If


                Dim strChekLinkAcct As String = "Valid"

                'If String.IsNullOrEmpty(txtLinkAcctNo.Text) = True Then
                '    strChekLinkAcct = "Invalid"
                'End If
                'If Trim(lblLinkAcctName.Text) = "Invalid" Then
                '    strChekLinkAcct = "Invalid"
                'End If

                Dim strchk As String = ""
                'If NETClass.Classes.SqlDbGetDataset(String.Format("SpCheckAccountCardLink {0}", txtLinkAcctNo.Text)).Tables(0).Rows.Count > 0 Then

                '    strChekLinkAcct = "Invalid"

                'End If




                If strChekLinkAcct = "Valid" Then


                    Dim CardCounter As Integer = 0

                    Cmd.CommandText = "Sproc_UpdateCardRequeststatus"
                    Cmd.Connection = sqlConn
                    Cmd.CommandType = CommandType.StoredProcedure

                    sqlConn.Open()
                    Cmd.Parameters.Add(New SqlParameter("@seqNum", SeqNum))

                    Cmd.Parameters.Add(New SqlParameter("@status", 5))

                    Cmd.Parameters.Add(New SqlParameter("@userid", guser))
                    Cmd.ExecuteNonQuery()
                    sqlConn.Close()

                    Dim commandAllo As New SqlCommand, connAllo As New SqlConnection(strcon)
                    'SqlCommand commandValidate = New SqlCommand();
                    connAllo.Open()
                    commandAllo.CommandText = "spUpdateCardAllocation"
                    commandAllo.Connection = connAllo
                    commandAllo.CommandType = CommandType.StoredProcedure
                    commandAllo.Parameters.Add(New SqlParameter("@SeqNum", SeqNum))
                    commandAllo.Parameters.Add(New SqlParameter("@status", 9))
                    commandAllo.ExecuteNonQuery()
                    connAllo.Close()

                End If


            End If



        Next



        'Next
        Response.Redirect("CardLinkDeAllo.aspx")
    End Sub

    Protected Sub btncancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncancel.Click
        Response.Redirect("CardLinkDeAllo.aspx")
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
        Response.Redirect("CardLink.aspx")
    End Sub


    Protected Sub BtnAllocate_Click(sender As Object, e As EventArgs)
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
        Dim Displinkact As GridViewDataColumn = CType(ctrlGrid.Columns("Link Account"), GridViewDataColumn)
        Dim DispCvv As GridViewDataColumn = CType(ctrlGrid.Columns("Card CVV"), GridViewDataColumn)
        ' Dim DispCvv As GridViewDataColumn = CType(ctrlGrid.Columns(8), GridViewDataColumn)
        Dim DispPin As GridViewDataColumn = CType(ctrlGrid.Columns("Card PIN"), GridViewDataColumn)
        Dim DispExpiry As GridViewDataColumn = CType(ctrlGrid.Columns("Card ExpiryDate"), GridViewDataColumn)
        Dim DispIssueDate As GridViewDataColumn = CType(ctrlGrid.Columns("Card IssueDate"), GridViewDataColumn)
        Dim DispPanNo As GridViewDataColumn = CType(ctrlGrid.Columns("PanNo"), GridViewDataColumn)
        Dim DispAddedBy As GridViewDataColumn = CType(ctrlGrid.Columns("Branch Officer"), GridViewDataColumn)

        Dim VisibleRowsCount As Integer = 0

        If ((ctrlGrid.VisibleRowCount / ctrlGrid.SettingsPager.PageSize) > 1) Then
            VisibleRowsCount = ctrlGrid.SettingsPager.PageSize
        Else
            VisibleRowsCount = ctrlGrid.VisibleRowCount
        End If
        Dim rowIndex As Integer


        'For rowIndex = 0 To VisibleRowsCount - 1

        For Each key As String In ctrlGrid.GetSelectedFieldValues("accountno")




            'Dim AccountNo As ASPxCheckBox = CType(ctrlGrid.GetRowValuesByKeyValue(key, "ASPxCheckBox1"), ASPxCheckBox)
            If ctrlGrid.Selection.IsRowSelectedByKey(key) = True Then
                Dim AccountNo As String = key
                Dim PANno As String = ctrlGrid.GetRowValuesByKeyValue(key, "PANno").ToString()
                Dim SeqNum As String = ctrlGrid.GetRowValuesByKeyValue(key, "SeqNum").ToString()
                Dim CardIssueDate As String = ctrlGrid.GetRowValuesByKeyValue(key, "CardIssueDate")
                Dim CardExpiryDate As String = ctrlGrid.GetRowValuesByKeyValue(key, "CardExpiryDate")
                Dim accountName As String = ctrlGrid.GetRowValuesByKeyValue(key, "AccountName")
                'If ctrlGrid.GetRowValuesByKeyValue(key, "SeqNum").ToString() <> Nothing Then
                '    SeqNum = ctrlGrid.GetRowValuesByKeyValue(key, "SeqNum").ToString()
                'End If

                'If AccountNo.Checked = True Then
                Dim Cmd As New SqlCommand, sqlConn As New SqlConnection(strcon), result As SqlParameter

                'Dim DisplName As ASPxLabel = CType(ctrlGrid.FindRowCellTemplateControl(rowIndex, ColCardExpiry, "ASPxTextBox2"), ASPxLabel)
                Dim txtLinkAcctNo As ASPxTextBox = CType(ctrlGrid.FindRowCellTemplateControlByKey(key, Displinkact, "txtAccountNo"), ASPxTextBox)



                Dim lblLinkAcctName As ASPxLabel = CType(ctrlGrid.FindRowCellTemplateControlByKey(key, Displinkact, "lblLinkAcctName"), ASPxLabel)

                Dim txtCVV As ASPxTextBox = CType(ctrlGrid.FindRowCellTemplateControlByKey(key, DispCvv, "txtCardVV"), ASPxTextBox)
                Dim txtCardPIN As ASPxTextBox = CType(ctrlGrid.FindRowCellTemplateControlByKey(key, DispPin, "txtCardPIN"), ASPxTextBox)
                Dim txtCardNo = CType(ctrlGrid.FindRowCellTemplateControlByKey(key, DispPanNo, "LblPanNo"), ASPxLabel) 'ctrlGrid.GetRowValuesByKeyValue(txtLinkAcctNo.Text, "PANno")
                Dim txtMonth As ASPxLabel = CType(ctrlGrid.FindRowCellTemplateControlByKey(key, DispExpiry, "ASPxLabel1"), ASPxLabel) ' ctrlGrid.GetRowValues(rowIndex, "Card ExpiryDate")

                Dim txtStartdate As ASPxLabel = CType(ctrlGrid.FindRowCellTemplateControlByKey(key, DispExpiry, "ASPxLabel1"), ASPxLabel) 'ctrlGrid.GetRowValuesByKeyValue(rowIndex, "Card IssueDate")


                Dim drpOfficers As ASPxComboBox = CType(ctrlGrid.FindRowCellTemplateControlByKey(key, DispAddedBy, "drpOfficers"), ASPxComboBox)

                If drpOfficers.Value = "" Then




                Else
                    'Dim AddressName As ASPxTextBox = CType(ctrlGrid.FindRowCellTemplateControl(rowIndex, AddressHead, "ASPxTextBox1"), ASPxTextBox)
                    'Dim CityName As ASPxTextBox = CType(ctrlGrid.FindRowCellTemplateControl(rowIndex, CityHead, "ASPxTextBox3"), ASPxTextBox)
                    'Dim DOB As ASPxDateEdit = CType(ctrlGrid.FindRowCellTemplateControl(rowIndex, DOBHead, "ASPxDateEdit1"), ASPxDateEdit)

                    'If String.IsNullOrEmpty(DOB.Date.ToShortDateString()) = True Then
                    '    DOB.Date = "1970-01-01"

                    'End If


                    Dim strChekLinkAcct As String = "Valid"

                    'If String.IsNullOrEmpty(txtLinkAcctNo.Text) = True Then
                    '    strChekLinkAcct = "Invalid"
                    'End If
                    'If Trim(lblLinkAcctName.Text) = "Invalid" Then
                    '    strChekLinkAcct = "Invalid"
                    'End If

                    'Dim strchk As String = ""
                    'If NETClass.Classes.SqlDbGetDataset(String.Format("SpCheckAccountCardLink {0}", txtLinkAcctNo.Text)).Tables(0).Rows.Count > 0 Then

                    '    strChekLinkAcct = "Invalid"

                    'End If

                    If strChekLinkAcct = "Valid" Then
                        Dim CardCounter As Integer = 0

                        Cmd.CommandText = "Sproc_UpdateCardRequeststatus"
                        Cmd.Connection = sqlConn
                        Cmd.CommandType = CommandType.StoredProcedure

                        sqlConn.Open()
                        Cmd.Parameters.Add(New SqlParameter("@seqNum", SeqNum))

                        Cmd.Parameters.Add(New SqlParameter("@status", 15))

                        Cmd.Parameters.Add(New SqlParameter("@userid", guser))
                        Cmd.ExecuteNonQuery()
                        sqlConn.Close()

                        ' Card Allocation 4 Linking on Core Banking

                        ' Add ISW maintenance Records


                        Dim commandValidate As New SqlCommand, connValidate As New SqlConnection(strcon)
                        'SqlCommand commandValidate = New SqlCommand();
                        connValidate.Open()
                        commandValidate.CommandText = "SprocAddCardAllocation"
                        commandValidate.Connection = connValidate
                        commandValidate.CommandType = CommandType.StoredProcedure
                        commandValidate.Parameters.Add(New SqlParameter("@batchRefNo", drpBatchRefNo.Value))

                        commandValidate.Parameters.Add(New SqlParameter("@accountNo", AccountNo))
                        commandValidate.Parameters.Add(New SqlParameter("@accountName", accountName))
                        commandValidate.Parameters.Add(New SqlParameter("@userId", drpOfficers.SelectedItem.Value))
                        commandValidate.Parameters.Add(New SqlParameter("@addedBy", guser))


                        commandValidate.Parameters.Add(New SqlParameter("@seqNum", SeqNum))

                        'Dim parameterMsg As SqlParameter = commandValidate.Parameters.Add(New SqlParameter("@ResponseDesc", SqlDbType.VarChar, 5000))
                        'parameterMsg.Direction = ParameterDirection.Output

                        commandValidate.ExecuteNonQuery()
                        connValidate.Close()





                    End If

                End If

            End If



        Next



        'Next
        Response.Redirect("CardLink.aspx")
    End Sub

End Class
