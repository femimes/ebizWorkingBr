Imports System
Imports NETClass
Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports DevExpress.Web
Imports DevExpress
Partial Class Request_CardTokens
    Inherits System.Web.UI.Page


    Public luser As String
    Public StrLocation As String
    Public struser As String
    Public guser As String

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
    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim iferror As String = ValidateEntries()
        If iferror = "" Then



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

            If Me.txtref_no.Text = Nothing Then

                txtref_no.Text = Guid.NewGuid().ToString().Substring(0, 12).ToUpper()
                Classes.alertwindow(Me.Page, "A New Batch Request Reference Generated : - " & Me.txtref_no.Text, "Halt")

                If chkAllRecords.Checked = False Then


                    Dim AccountNoHead As GridViewDataColumn = CType(ASPxGridView1.Columns("AccountNo"), GridViewDataColumn)
                    Dim VisibleRowsCount As Integer = 0

                    If ((ASPxGridView1.VisibleRowCount / ASPxGridView1.SettingsPager.PageSize) > 1) Then
                        VisibleRowsCount = ASPxGridView1.SettingsPager.PageSize
                    Else
                        VisibleRowsCount = ASPxGridView1.VisibleRowCount
                    End If
                    Dim rowIndex As Integer


                    For rowIndex = 0 To VisibleRowsCount - 1

                        Dim AccountNo As ASPxCheckBox = CType(ASPxGridView1.FindRowCellTemplateControl(rowIndex, AccountNoHead, "ASPxCheckBox1"), ASPxCheckBox)

                        If AccountNo.Checked = True Then
                            Dim Cmd As New SqlCommand, sqlConn As New SqlConnection(strcon), result As SqlParameter
                            Dim DisplNameHead As GridViewDataColumn = CType(ASPxGridView1.Columns("AccountName"), GridViewDataColumn)
                            Dim AddressHead As GridViewDataColumn = CType(ASPxGridView1.Columns("Address"), GridViewDataColumn)
                            Dim CityHead As GridViewDataColumn = CType(ASPxGridView1.Columns("city"), GridViewDataColumn)
                            Dim PANnoHead As GridViewDataColumn = CType(ASPxGridView1.Columns("PANno"), GridViewDataColumn)

                            Dim SeqHead As GridViewDataColumn = CType(ASPxGridView1.Columns("Address"), GridViewDataColumn)
                            Dim CounterHead As GridViewDataColumn = CType(ASPxGridView1.Columns("city"), GridViewDataColumn)
                            Dim DOBHead As GridViewDataColumn = CType(ASPxGridView1.Columns("DOB"), GridViewDataColumn)
                            Dim PhoneNoHead As GridViewDataColumn = CType(ASPxGridView1.Columns("PhoneNo"), GridViewDataColumn)
                            Dim DisplName As ASPxLabel = CType(ASPxGridView1.FindRowCellTemplateControl(rowIndex, DisplNameHead, "ASPxTextBox2"), ASPxLabel)
                            'Dim AddressName As ASPxTextBox = CType(ASPxGridView1.FindRowCellTemplateControl(rowIndex, AddressHead, "ASPxTextBox1"), ASPxTextBox)
                            Dim PANno As ASPxLabel = CType(ASPxGridView1.FindRowCellTemplateControl(rowIndex, PANnoHead, "ASPxTextBox2"), ASPxLabel)

                            Dim AddressName As Label = CType(ASPxGridView1.FindRowCellTemplateControl(rowIndex, AddressHead, "txtaddress"), Label)

                            Dim CityName As ASPxLabel = CType(ASPxGridView1.FindRowCellTemplateControl(rowIndex, CityHead, "ASPxTextBox3"), ASPxLabel)
                            Dim DOB As ASPxDateEdit = CType(ASPxGridView1.FindRowCellTemplateControl(rowIndex, DOBHead, "ASPxDateEdit1"), ASPxDateEdit)

                            If String.IsNullOrEmpty(DOB.Date.ToShortDateString()) = True Then
                                DOB.Date = "1970-01-01"

                            End If

                            Dim PhoneNo As ASPxTextBox = CType(ASPxGridView1.FindRowCellTemplateControl(rowIndex, PhoneNoHead, "ASPxTextBox4"), ASPxTextBox)



                            Cmd.CommandText = "Sproc_AddCardRequestMob"
                            Cmd.Connection = sqlConn
                            Cmd.CommandType = CommandType.StoredProcedure

                            sqlConn.Open()
                            Cmd.Parameters.Add(New SqlParameter("@RefNo", txtref_no.Text))
                            Cmd.Parameters.Add(New SqlParameter("@AccountNo", AccountNo.Text))
                            Cmd.Parameters.Add(New SqlParameter("@Description", DisplName.Text))
                            Cmd.Parameters.Add(New SqlParameter("@PanNo", PANno.CssClass))
                            Cmd.Parameters.Add(New SqlParameter("@Address", AddressName.Text))
                            Cmd.Parameters.Add(New SqlParameter("@City", CityName.Text))
                            Cmd.Parameters.Add(New SqlParameter("@CountryCode", "NGA"))
                            Cmd.Parameters.Add(New SqlParameter("@CardBrand", drpbrand.SelectedItem.Value))
                            Cmd.Parameters.Add(New SqlParameter("@CardIssueDate", dateIssue.Date))

                            Cmd.Parameters.Add(New SqlParameter("@CardExpiryDate", dateexpiry.Date))
                            'Cmd.Parameters.Add(New SqlParameter("@CardCounter", ASPxGridView1.GetRowValuesByKeyValue(AccountNo.Text, "CardIssueCounter")))
                            Cmd.Parameters.Add(New SqlParameter("@CardCounter", 0))

                            'Cmd.Parameters.Add(New SqlParameter("@Seq", ASPxGridView1.GetRowValuesByKeyValue(AccountNo.Text, "seq")))
                            Cmd.Parameters.Add(New SqlParameter("@Seq", 2))

                            Cmd.Parameters.Add(New SqlParameter("@RequestTitle", txtTitle.Text))


                            '   Cmd.Parameters.Add(New SqlParameter("@DOB", DOB.Date))
                            Cmd.Parameters.Add(New SqlParameter("@DOB", "1970-01-01"))
                            Cmd.Parameters.Add(New SqlParameter("@AddedBy", guser))

                            '  Cmd.Parameters.Add(New SqlParameter("@ApprovedBy", drp_appr.Value))
                            Cmd.Parameters.Add(New SqlParameter("@ApprovedBy", "System"))

                            Cmd.Parameters.Add(New SqlParameter("@CardPhoneNo", PhoneNo.Text))

                            Cmd.ExecuteNonQuery()


                            sqlConn.Close()
                        End If
                    Next
                Else
                    'Select all records
                    Dim OldPageSize As Integer = ASPxGridView1.SettingsPager.PageSize
                    ASPxGridView1.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords
                    'For pageindex = 0 To ASPxGridView1.PageCount
                    Dim VisibleRowsCount As Integer = 0
                    Dim AccountNoHead As GridViewDataColumn = CType(ASPxGridView1.Columns("AccountNo"), GridViewDataColumn)

                    If ((ASPxGridView1.VisibleRowCount / ASPxGridView1.SettingsPager.PageSize) > 1) Then
                        VisibleRowsCount = ASPxGridView1.SettingsPager.PageSize
                    Else
                        VisibleRowsCount = ASPxGridView1.VisibleRowCount
                    End If


                    Dim rowIndex As Integer
                    Dim lp As Integer = 0
                    ASPxGridView1.DataBind()
                    For rowIndex = 0 To ASPxGridView1.VisibleRowCount - 1

                        Dim AccountNo As ASPxCheckBox = CType(ASPxGridView1.FindRowCellTemplateControl(rowIndex, AccountNoHead, "ASPxCheckBox1"), ASPxCheckBox)


                        Dim Cmd As New SqlCommand, sqlConn As New SqlConnection(strcon), result As SqlParameter
                        Dim DisplNameHead As GridViewDataColumn = CType(ASPxGridView1.Columns("AccountName"), GridViewDataColumn)
                        Dim AddressHead As GridViewDataColumn = CType(ASPxGridView1.Columns("Address"), GridViewDataColumn)
                        Dim CityHead As GridViewDataColumn = CType(ASPxGridView1.Columns("city"), GridViewDataColumn)
                        Dim PANnoHead As GridViewDataColumn = CType(ASPxGridView1.Columns("PANno"), GridViewDataColumn)
                        Dim SeqHead As GridViewDataColumn = CType(ASPxGridView1.Columns("Address"), GridViewDataColumn)
                        Dim CounterHead As GridViewDataColumn = CType(ASPxGridView1.Columns("city"), GridViewDataColumn)
                        Dim DOBHead As GridViewDataColumn = CType(ASPxGridView1.Columns("DOB"), GridViewDataColumn)
                        Dim PhoneNoHead As GridViewDataColumn = CType(ASPxGridView1.Columns("PhoneNo"), GridViewDataColumn)
                        Dim DisplName As ASPxLabel = CType(ASPxGridView1.FindRowCellTemplateControl(rowIndex, DisplNameHead, "ASPxTextBox2"), ASPxLabel)
                        'Dim AddressName As ASPxTextBox = CType(ASPxGridView1.FindRowCellTemplateControl(rowIndex, AddressHead, "ASPxTextBox1"), ASPxTextBox)
                        Dim PANno As ASPxLabel = CType(ASPxGridView1.FindRowCellTemplateControl(rowIndex, PANnoHead, "ASPxTextBox2"), ASPxLabel)

                        Dim AddressName As Label = CType(ASPxGridView1.FindRowCellTemplateControl(rowIndex, AddressHead, "txtaddress"), Label)

                        Dim CityName As ASPxLabel = CType(ASPxGridView1.FindRowCellTemplateControl(rowIndex, CityHead, "ASPxTextBox3"), ASPxLabel)
                        Dim DOB As ASPxDateEdit = CType(ASPxGridView1.FindRowCellTemplateControl(rowIndex, DOBHead, "ASPxDateEdit1"), ASPxDateEdit)

                        If String.IsNullOrEmpty(DOB.Date.ToShortDateString()) = True Then
                            DOB.Date = "1970-01-01"

                        End If

                        Dim PhoneNo As ASPxTextBox = CType(ASPxGridView1.FindRowCellTemplateControl(rowIndex, PhoneNoHead, "ASPxTextBox4"), ASPxTextBox)





                        Cmd.CommandText = "Sproc_AddCardRequestMob"
                        Cmd.Connection = sqlConn
                        Cmd.CommandType = CommandType.StoredProcedure

                        sqlConn.Open()
                        Cmd.Parameters.Add(New SqlParameter("@RefNo", txtref_no.Text))
                        Cmd.Parameters.Add(New SqlParameter("@AccountNo", AccountNo.Text))
                        Cmd.Parameters.Add(New SqlParameter("@Description", DisplName.Text))
                        Cmd.Parameters.Add(New SqlParameter("@PanNo", PANno.CssClass))
                        Cmd.Parameters.Add(New SqlParameter("@Address", AddressName.Text))
                        Cmd.Parameters.Add(New SqlParameter("@City", CityName.Text))
                        Cmd.Parameters.Add(New SqlParameter("@CountryCode", "NGA"))
                        Cmd.Parameters.Add(New SqlParameter("@CardBrand", drpbrand.SelectedItem.Value))
                        Cmd.Parameters.Add(New SqlParameter("@CardIssueDate", dateIssue.Date))

                        Cmd.Parameters.Add(New SqlParameter("@CardExpiryDate", dateexpiry.Date))
                        'Cmd.Parameters.Add(New SqlParameter("@CardCounter", ASPxGridView1.GetRowValuesByKeyValue(AccountNo.Text, "CardIssueCounter")))
                        Cmd.Parameters.Add(New SqlParameter("@CardCounter", 0))

                        'Cmd.Parameters.Add(New SqlParameter("@Seq", ASPxGridView1.GetRowValuesByKeyValue(AccountNo.Text, "seq")))
                        Cmd.Parameters.Add(New SqlParameter("@Seq", 2))

                        Cmd.Parameters.Add(New SqlParameter("@RequestTitle", txtTitle.Text))


                        Cmd.Parameters.Add(New SqlParameter("@DOB", DOB.Date))

                        Cmd.Parameters.Add(New SqlParameter("@AddedBy", guser))
                        Cmd.Parameters.Add(New SqlParameter("@ApprovedBy", drp_appr.Value))
                        Cmd.Parameters.Add(New SqlParameter("@CardPhoneNo", PhoneNo.Text))


                        Cmd.ExecuteNonQuery()


                        sqlConn.Close()


                    Next

                    ASPxGridView1.SettingsPager.Mode = GridViewPagerMode.ShowPager
                    ASPxGridView1.SettingsPager.PageSize = OldPageSize

                End If
                Response.Write("<script>alert('Request Successful') </script>")
                txtref_no.Text = Nothing
            End If



        Else
            Dim stringinfo As String = String.Format("<script>alert('{0}') </script>", iferror)
            Response.Write(stringinfo)
            ASPxGridView1.DataBind()

        End If
    End Sub


    Sub LoadBoxes()
        'Dim ds As New DataSet
        'sqlproducts.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("EazyConnect").ConnectionString
        'sqlproducts.SelectCommand = "Sproc_GetAllProducts"
        'sqlproducts.SelectCommandType = SqlDataSourceCommandType.Text


        'sqlBranches.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("EazyConnect").ConnectionString
        'sqlBranches.SelectCommand = "Sproc_GetAllBranches"
        'sqlBranches.SelectCommandType = SqlDataSourceCommandType.Text

        '
        'Dim strgetOffr As String = String.Format("Sproc_GetTransApprovingOfficers {0},2,'{1}','{2}','{3}'", CDec(200), gEazy.GetTransCodeInfo("002", 2), StrLocation, guser)
        'sqlAppr.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("EazyConnect").ConnectionString
        'sqlAppr.SelectCommand = strgetOffr
        'sqlAppr.SelectCommandType = SqlDataSourceCommandType.Text



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


        Dim strgetAcctDetails As String = String.Format("Sproc_GetTransApprovingOfficers {0},2,'{1}','{2}','{3}'", CDec(200), "002", StrLocation, guser)
        sqlAppr.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("EazyConnect").ConnectionString
        sqlAppr.SelectCommand = strgetAcctDetails
        sqlAppr.SelectCommandType = SqlDataSourceCommandType.Text



    End Sub
    Sub LoadGrid()
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
        If String.IsNullOrEmpty(Trim(txtSpecificAcct.Text)) = False Then



            Dim strgetAcctDetails As String = String.Format("GetAllAccountCardRequestToken '{0}','{1}'", drp_products.Value, drpbranches.Value)
            SqlacctDetails.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("EazyConnect").ConnectionString
            SqlacctDetails.SelectCommand = strgetAcctDetails
            SqlacctDetails.FilterExpression = String.Format("accountno in ({0})", Trim(txtSpecificAcct.Text))

            SqlacctDetails.SelectCommandType = SqlDataSourceCommandType.Text

        Else

            Dim strgetAcctDetails As String = String.Format("GetAllAccountCardRequestToken '{0}','{1}'", drp_products.Value, drpbranches.Value)
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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If (IsPostBack = False) Then
        '    txtUserID.Text = guser
        '    LoadBoxes()
        '    LoadGrid()
        'End If

        If (IsPostBack = False And IsCallback = False) Then
            txtUserID.Text = guser

            LoadBoxes()
            LoadGrid()


            drpbranches.Value = "00"
            drp_products.Value = "103"


            Me.ASPxGridView1.PageIndex = 0
        End If
    End Sub



    Protected Sub ASPxGridView1_BeforePerformDataSelect(ByVal sender As Object, ByVal e As System.EventArgs) Handles ASPxGridView1.BeforePerformDataSelect
        LoadGrid()
    End Sub

    Protected Sub drp_products_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drp_products.SelectedIndexChanged
        LoadBoxes()
        LoadGrid()
    End Sub

    Protected Sub drpbranches_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpbranches.SelectedIndexChanged
        LoadBoxes()
        LoadGrid()
    End Sub

    Protected Sub Button3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button3.Click
        Response.Redirect("CardTokens.aspx")
    End Sub

    Protected Sub Button4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button4.Click
        LoadGrid()
    End Sub


    Protected Sub ASPxCheckBox2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim chkAll As ASPxCheckBox = CType(sender, ASPxCheckBox)
        'Response.Write("i got here")
        Dim AccountNoHead As GridViewDataColumn = CType(ASPxGridView1.Columns("AccountNo"), GridViewDataColumn)
        Dim VisibleRowsCount As Integer = 0

        If ((ASPxGridView1.VisibleRowCount / ASPxGridView1.SettingsPager.PageSize) > 1) Then
            VisibleRowsCount = ASPxGridView1.SettingsPager.PageSize
        Else
            VisibleRowsCount = ASPxGridView1.VisibleRowCount
        End If
        If (chkAll.Checked = True) Then
            Dim rowIndex As Integer


            For rowIndex = 0 To VisibleRowsCount - 1



                'CheckBox ChkFirstName = (CheckBox)ctrlGrid.FindRowCellTemplateControl(rowIndex, columnFirstName, "CheckBox1")
                Dim AccountNo As ASPxCheckBox = CType(ASPxGridView1.FindRowCellTemplateControl(rowIndex, AccountNoHead, "ASPxCheckBox1"), ASPxCheckBox)

                AccountNo.Checked = True
            Next
        Else
            Dim rowIndex As Integer
            For rowIndex = 0 To VisibleRowsCount - 1



                'CheckBox ChkFirstName = (CheckBox)ctrlGrid.FindRowCellTemplateControl(rowIndex, columnFirstName, "CheckBox1")
                Dim AccountNo As ASPxCheckBox = CType(ASPxGridView1.FindRowCellTemplateControl(rowIndex, AccountNoHead, "ASPxCheckBox1"), ASPxCheckBox)

                AccountNo.Checked = False
            Next
        End If

    End Sub


    Protected Sub txtSpecificAcct_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSpecificAcct.TextChanged
        LoadBoxes()
        LoadGrid()
    End Sub

    Protected Sub ASPxButton1_Click(sender As Object, e As System.EventArgs) Handles ASPxButton1.Click
        Button1_Click(sender, e)
    End Sub

    Protected Sub ASPxButton2_Click(sender As Object, e As System.EventArgs) Handles ASPxButton2.Click
        Response.Redirect("CardTokens.aspx")
    End Sub
End Class
