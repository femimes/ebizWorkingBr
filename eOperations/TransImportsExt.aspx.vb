Imports NETClass
Imports System
Imports System.Data
Imports System.IO
Imports System.Drawing
Imports System.Data.SqlClient
Imports System.Windows.Forms

Partial Class Mandate_TransImportsExt
    Inherits System.Web.UI.Page
    Public luser As String = Context.User.Identity.Name
    Public StrLocation As String = luser.Substring(luser.LastIndexOf("?") + 1)
    Public struser As String = Mid(luser, 1, luser.LastIndexOf("?"))
    Public guser As String = struser
    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            Dim m_strFileName As String = UploadMultipleFiles_Clicked()
            If m_strFileName <> Nothing Then
                If drptranstype.SelectedValue = 2 Then

                    InsertAllRecords(m_strFileName)
                ElseIf drptranstype.SelectedValue = 3 Then

                    InsertAllRecordsPromo(m_strFileName)
                ElseIf drptranstype.SelectedValue = 4 Then
                    InsertAllRecordsPromoAccts(m_strFileName)
                ElseIf drptranstype.SelectedValue = 8 Then
                    InsertAllRecordsATMRecon(m_strFileName)
                ElseIf drptranstype.SelectedValue = 9 Then
                    InsertAllRecordsATMReconAll(m_strFileName)
                ElseIf drptranstype.SelectedValue = 10 Then
                    InsertAllRecordsATMReChargeRecon(m_strFileName)
                ElseIf drptranstype.SelectedValue = 11 Then
                    InsertAllRecordsATMRePOSRecon(m_strFileName)
                ElseIf drptranstype.SelectedValue = 12 Then
                    InsertAllRecordsATMReMerPOSRecon(m_strFileName)
                Else

                    InsertAllRecordHr(m_strFileName)
                End If


            End If
        Catch ex As Exception
            Response.Write("<script>alert('Invalid file format " & ex.Message & "') </script>")
        End Try
    End Sub
    Private Sub LoadApprovingOfficers()
        On Error Resume Next

        Dim ds As New DataSet
        luser = Context.User.Identity.Name
        StrLocation = luser.Substring(luser.LastIndexOf("?") + 1)
        struser = Mid(luser, 1, luser.LastIndexOf("?"))
        gUser = struser
        ds = NETClass.Classes.GetDataset("Sproc_GetTransApprovingOfficers " & CDec(200) & ",2,'" & EazyClass.GetTransCodeInfo("002", 2) & "','" & StrLocation & "','" & struser & "'")

        offr2.DataSource = ds.Tables(0)
        offr2.DataTextField = "user_id"
        offr2.DataValueField = "user_id"
        offr2.DataBind()

    End Sub

    Private Sub LoadApprovingOfficers2(ByVal amount As Decimal)
        On Error Resume Next

        Dim ds As New DataSet

        ds = NETClass.Classes.GetDataset("Sproc_GetTransApprovingOfficers " & CDec(amount) & ",2,'" & EazyClass.GetTransCodeInfo("002", 2) & "','" & StrLocation & "','" & struser & "'")

        offr2.DataSource = ds.Tables(0)
        offr2.DataTextField = "user_id"
        offr2.DataValueField = "user_id"
        offr2.DataBind()

    End Sub

    Sub InsertAllRecords(ByVal m_strFileName As String)

        luser = Context.User.Identity.Name
        StrLocation = luser.Substring(luser.LastIndexOf("?") + 1)
        struser = Mid(luser, 1, luser.LastIndexOf("?"))
        guser = struser

        ' Assign a new object to the variable.
        Dim l_strval31 As String = m_strFileName  ' backupname.Value.Trim
        'backupname.Value = "C:\TempMultipleFiles\" & m_strFileName
        'Dim sr As StreamReader = New StreamReader(backupname.Value.Trim)
        Dim sr As StreamReader = New StreamReader("C:\TempMultipleFiles\" & m_strFileName)

        Dim line As String
        Dim dsp As New DataSet, dr As DataRow, dt As New DataTable, dc As DataColumn
        dt.Columns.Add(New DataColumn(" Post Date   ", GetType(String)))
        dt.Columns.Add(New DataColumn("TransDate  ", GetType(String)))

        dt.Columns.Add(New DataColumn("Account No", GetType(String)))
        dt.Columns.Add(New DataColumn("Transaction Code", GetType(String)))
        dt.Columns.Add(New DataColumn("Narrative", GetType(String)))
        dt.Columns.Add(New DataColumn("Cheque No ", GetType(String)))
        dt.Columns.Add(New DataColumn("Debit", GetType(String)))

        dt.Columns.Add(New DataColumn("Credit", GetType(String)))
        dt.Columns.Add(New DataColumn("Contra", GetType(String)))
        dt.Columns.Add(New DataColumn("Instr No", GetType(String)))
        dt.Columns.Add(New DataColumn("Reference Code", GetType(String)))
        dt.Columns.Add(New DataColumn("Remarks", GetType(String)))
        Do
            line = sr.ReadLine()
            Dim l_val As Integer = Len(line)

            Dim str_map As String = line


            Dim l_pos As Integer = InStr(line, ",") 'line.IndexOf(",")
            Dim l_strval As String = Mid(line, 1, l_pos)
            Dim l_strval2 As String = Mid(line, l_pos + 1)
            Dim l_pos2 As Integer = InStr(l_strval2, ",")
            l_strval2 = Mid(l_strval2, 1, l_pos2)
            'Dim l_strval3 As String = Mid(line, l_pos + l_pos2 + 1)
            Dim l_strval3 As String = Mid(line, l_pos + l_pos2 + 1)

            Dim l_pos3 As Integer = InStr(l_strval3, ",")

            l_strval3 = Mid(l_strval3, 1, l_pos3)

            Dim l_strval4 As String = Mid(line, l_pos + l_pos2 + l_pos3 + 1)
            Dim l_pos4 As Integer = InStr(l_strval4, ",")
            l_strval4 = Mid(l_strval4, 1, l_pos4)

            Dim l_strval5 As String = Mid(line, l_pos + l_pos2 + l_pos3 + l_pos4 + 1)
            Dim l_pos5 As Integer = InStr(l_strval5, ",")
            l_strval5 = Mid(l_strval5, 1, l_pos5)



            Dim l_strval6 As String = Mid(line, l_pos + l_pos2 + l_pos3 + l_pos4 + l_pos5 + 1)
            Dim l_pos6 As Integer = InStr(l_strval6, ",")
            l_strval6 = Mid(l_strval6, 1, l_pos6)

            Dim l_strval7 As String = Mid(line, l_pos + l_pos2 + l_pos3 + l_pos4 + l_pos5 + l_pos6 + 1)
            Dim l_pos7 As Integer = InStr(l_strval7, ",")
            l_strval7 = Mid(l_strval7, 1, l_pos7)


            Dim l_strval8 As String = Mid(line, l_pos + l_pos2 + l_pos3 + l_pos4 + l_pos5 + l_pos6 + l_pos7 + 1)
            Dim l_pos8 As Integer = InStr(l_strval8, ",")
            'Dim l_pine As String = l_strval8
            l_strval8 = Mid(l_strval8, 1, l_pos8)

            Dim l_strval9 As String = Mid(line, l_pos + l_pos2 + l_pos3 + l_pos4 + l_pos5 + l_pos6 + l_pos7 + l_pos8 + 1)
            Dim l_pos9 As Integer = InStr(l_strval9, ",")
            'Dim l_pine As String = l_strval8
            l_strval9 = Mid(l_strval9, 1, l_pos9)

            Dim l_strval10 As String = Mid(line, l_pos + l_pos2 + l_pos3 + l_pos4 + l_pos5 + l_pos6 + l_pos7 + l_pos8 + l_pos9 + 1)
            Dim l_pos10 As Integer = InStr(l_strval10, ",")
            'Dim l_pine As String = l_strval8
            l_strval10 = Mid(l_strval10, 1, l_pos10)

            Dim l_strval11 As String = Mid(line, l_pos + l_pos2 + l_pos3 + l_pos4 + l_pos5 + l_pos6 + l_pos7 + l_pos8 + l_pos9 + l_pos10 + 1)
            'Dim l_pos11 As Integer = InStr(l_strval11, ",")
            ''Dim l_pine As String = l_strval8
            'l_strval11 = Mid(l_strval11, 1, l_pos11)


            'Dim l_strval12 As String = Mid(line, l_pos + l_pos2 + l_pos3 + l_pos4 + l_pos5 + l_pos6 + l_pos7 + l_pos8 + l_pos9 + l_pos10 + l_pos11 + 1)
            'Dim l_pos12 As Integer = InStr(l_strval12, ",")
            ''Dim l_pine As String = l_strval8
            'l_strval12 = Mid(l_strval12, 1, l_pos12)

            'Dim l_strval13 As String = Mid(line, l_pos + l_pos2 + l_pos3 + l_pos4 + l_pos5 + l_pos6 + l_pos7 + l_pos8 + l_pos9 + l_pos10 + l_pos11 + l_pos12 + 1)

            'Dim l_pos13 As Integer = InStr(l_strval13, ",")
            ''Dim l_pine As String = l_strval8
            'l_strval13 = Mid(l_strval13, 1, l_pos13)
            'Dim l_strval14 As String = Mid(line, l_pos + l_pos2 + l_pos3 + l_pos4 + l_pos5 + l_pos6 + l_pos7 + l_pos8 + l_pos9 + l_pos10 + l_pos11 + l_pos12 + +l_pos13 + 1)



            Dim l_PostDate As String = l_strval2
            Dim l_transDate As String = l_strval3
            Dim l_accountno As String = l_strval
            Dim l_transcode As String = l_strval10
            Dim l_narrative As String = l_strval11
            Dim l_description As String = l_strval4
            Dim l_debit As String = l_strval5
            Dim l_credit As String = l_strval6
            Dim l_bankcode As String = l_strval7
            Dim l_chequeno As String = l_strval8
            Dim l_TransRef As String = l_strval9

            'Dim l_CustomerName As String = l_strval10
            'Dim l_Outletype As String = l_strval11
            'Dim l_TownshipCode As String = l_strval12
            'Dim l_Township As String = l_strval13
            'Dim l_TransType As String = l_strval14


            'Dim l_suppliercode As String = l_strval11

            'Dim l_seqno As String = l_strval12
            If Trim(l_PostDate) <> Nothing Then

                dr = dt.NewRow()
                'If (l_accountno.Length <> 15 And l_accountno.Length <> 0) Then
                '    Dim l_Mypos As Integer = l_accountno.Length
                '    l_accountno = l_accountno.Insert(4, "/")
                '    l_accountno = l_accountno.Insert(l_Mypos - 3, "/")



                'End If
                dr(0) = Replace(Replace(l_PostDate, ",", ""), ",", "")
                dr(1) = l_transDate.Replace(",", "").Replace(",", "")
                Dim accountrep As String
                If EazyClass.AccountExist(Trim(l_accountno.Replace(",", ""))) = True Then
                    accountrep = Trim(l_accountno.Replace(",", ""))
                Else
                    Dim myoldacctno As String = "t" & Trim(l_accountno.Replace(",", ""))

                    If EazyClass.AccountExist(EazyClass.FullAccount(StrLocation, myoldacctno)) = True Then
                        accountrep = EazyClass.FullAccount(StrLocation, myoldacctno)
                    Else
                        accountrep = Trim(l_accountno.Replace(",", ""))
                    End If
                End If

                dr(2) = accountrep 'Trim(l_accountno.Replace(",", ""))
                dr(3) = Trim(l_transcode.Replace(",", ""))
                dr(4) = Replace(Replace(l_narrative, ",", ""), ",", "")
                dr(5) = l_description.Replace(",", "").Replace(",", "")
                dr(6) = Trim(l_debit.Replace(",", ""))
                dr(7) = Trim(l_credit.Replace(",", ""))
                dr(8) = Trim(l_bankcode.Replace(",", ""))

                dr(9) = Trim(l_chequeno.Replace(",", ""))
                dr(10) = Trim(l_TransRef.Replace(",", ""))
                dr(11) = "Pass"

                dt.Rows.Add(dr)
            End If
        Loop Until line Is Nothing
        sr.Close()

        DataGrid1.DataSource = dt

        DataGrid1.DataBind()

        Dim str_error As String = ValidateEntries()
        If str_error = Nothing Then

        Else
            'Me.cust_val.IsValid = False
            'Me.cust_val.ToolTip = "Record Already Exist !!" & str_error
            Response.Write("<script>alert(' The following entries are with errors !!" & str_error & "') </script>")
            'DataGrid2.Visible = True
        End If


    End Sub

    Sub InsertAllRecordsPromo(ByVal m_strFileName As String)

        luser = Context.User.Identity.Name
        StrLocation = luser.Substring(luser.LastIndexOf("?") + 1)
        struser = Mid(luser, 1, luser.LastIndexOf("?"))
        guser = struser

        ' Assign a new object to the variable.
        Dim l_strval31 As String = m_strFileName  ' backupname.Value.Trim
        'backupname.Value = "C:\TempMultipleFiles\" & m_strFileName
        'Dim sr As StreamReader = New StreamReader(backupname.Value.Trim)
        Dim sr As StreamReader = New StreamReader("C:\TempMultipleFiles\" & m_strFileName)

        Dim line As String
        Dim dsp As New DataSet, dr As DataRow, dt As New DataTable, dc As DataColumn
        dt.Columns.Add(New DataColumn("Account RefNo", GetType(String)))
        dt.Columns.Add(New DataColumn("Post Date", GetType(String)))
        dt.Columns.Add(New DataColumn("Trans Date  ", GetType(String)))
        dt.Columns.Add(New DataColumn("Account RefName", GetType(String)))
        dt.Columns.Add(New DataColumn("Customer ContactAddress", GetType(String)))
        dt.Columns.Add(New DataColumn("Telephone", GetType(String)))
        dt.Columns.Add(New DataColumn("Deposit SlipNo", GetType(String)))
        dt.Columns.Add(New DataColumn("Amount", GetType(String)))
        dt.Columns.Add(New DataColumn("Promo Remarks", GetType(String)))
        dt.Columns.Add(New DataColumn("Product Description", GetType(String)))
        dt.Columns.Add(New DataColumn("SystemRemarks", GetType(String)))

        Do
            line = sr.ReadLine()
            Dim l_val As Integer = Len(line)

            Dim str_map As String = line


            Dim l_pos As Integer = InStr(line, ",") 'line.IndexOf(",")
            Dim l_strval As String = Mid(line, 1, l_pos)
            Dim l_strval2 As String = Mid(line, l_pos + 1)
            Dim l_pos2 As Integer = InStr(l_strval2, ",")
            l_strval2 = Mid(l_strval2, 1, l_pos2)
            'Dim l_strval3 As String = Mid(line, l_pos + l_pos2 + 1)
            Dim l_strval3 As String = Mid(line, l_pos + l_pos2 + 1)

            Dim l_pos3 As Integer = InStr(l_strval3, ",")

            l_strval3 = Mid(l_strval3, 1, l_pos3)

            Dim l_strval4 As String = Mid(line, l_pos + l_pos2 + l_pos3 + 1)
            Dim l_pos4 As Integer = InStr(l_strval4, ",")
            l_strval4 = Mid(l_strval4, 1, l_pos4)

            Dim l_strval5 As String = Mid(line, l_pos + l_pos2 + l_pos3 + l_pos4 + 1)
            Dim l_pos5 As Integer = InStr(l_strval5, ",")
            l_strval5 = Mid(l_strval5, 1, l_pos5)



            Dim l_strval6 As String = Mid(line, l_pos + l_pos2 + l_pos3 + l_pos4 + l_pos5 + 1)
            Dim l_pos6 As Integer = InStr(l_strval6, ",")
            l_strval6 = Mid(l_strval6, 1, l_pos6)

            Dim l_strval7 As String = Mid(line, l_pos + l_pos2 + l_pos3 + l_pos4 + l_pos5 + l_pos6 + 1)
            Dim l_pos7 As Integer = InStr(l_strval7, ",")
            l_strval7 = Mid(l_strval7, 1, l_pos7)


            Dim l_strval8 As String = Mid(line, l_pos + l_pos2 + l_pos3 + l_pos4 + l_pos5 + l_pos6 + l_pos7 + 1)
            Dim l_pos8 As Integer = InStr(l_strval8, ",")
            'Dim l_pine As String = l_strval8
            l_strval8 = Mid(l_strval8, 1, l_pos8)

            Dim l_strval9 As String = Mid(line, l_pos + l_pos2 + l_pos3 + l_pos4 + l_pos5 + l_pos6 + l_pos7 + l_pos8 + 1)
            Dim l_pos9 As Integer = InStr(l_strval9, ",")
            'Dim l_pine As String = l_strval8
            l_strval9 = Mid(l_strval9, 1, l_pos9)

            Dim l_strval10 As String = Mid(line, l_pos + l_pos2 + l_pos3 + l_pos4 + l_pos5 + l_pos6 + l_pos7 + l_pos8 + l_pos9 + 1)
            Dim l_pos10 As Integer = InStr(l_strval10, ",")
            'Dim l_pine As String = l_strval8
            l_strval10 = Mid(l_strval10, 1, l_pos10)

            Dim l_strval11 As String = Mid(line, l_pos + l_pos2 + l_pos3 + l_pos4 + l_pos5 + l_pos6 + l_pos7 + l_pos8 + l_pos9 + l_pos10 + 1)
            'Dim l_pos11 As Integer = InStr(l_strval11, ",")
            ''Dim l_pine As String = l_strval8
            'l_strval11 = Mid(l_strval11, 1, l_pos11)


            'Dim l_strval12 As String = Mid(line, l_pos + l_pos2 + l_pos3 + l_pos4 + l_pos5 + l_pos6 + l_pos7 + l_pos8 + l_pos9 + l_pos10 + l_pos11 + 1)
            'Dim l_pos12 As Integer = InStr(l_strval12, ",")
            ''Dim l_pine As String = l_strval8
            'l_strval12 = Mid(l_strval12, 1, l_pos12)

            'Dim l_strval13 As String = Mid(line, l_pos + l_pos2 + l_pos3 + l_pos4 + l_pos5 + l_pos6 + l_pos7 + l_pos8 + l_pos9 + l_pos10 + l_pos11 + l_pos12 + 1)

            'Dim l_pos13 As Integer = InStr(l_strval13, ",")
            ''Dim l_pine As String = l_strval8
            'l_strval13 = Mid(l_strval13, 1, l_pos13)
            'Dim l_strval14 As String = Mid(line, l_pos + l_pos2 + l_pos3 + l_pos4 + l_pos5 + l_pos6 + l_pos7 + l_pos8 + l_pos9 + l_pos10 + l_pos11 + l_pos12 + +l_pos13 + 1)



            Dim l_Accountno As String = l_strval
            Dim l_PostDate As String = l_strval2
            Dim l_TransDate As String = l_strval3
            Dim l_Description As String = l_strval4
            Dim l_ContactAddress As String = l_strval5
            Dim l_Telephone As String = l_strval6
            Dim l_DepositSlipNo As String = l_strval7
            Dim l_Amount As String = l_strval8
            Dim l_PromoRemarks As String = l_strval9
            Dim l_Product As String = l_strval10
            Dim l_Remarks As String = l_strval11





            'Dim l_CustomerName As String = l_strval10
            'Dim l_Outletype As String = l_strval11
            'Dim l_TownshipCode As String = l_strval12
            'Dim l_Township As String = l_strval13
            'Dim l_TransType As String = l_strval14


            'Dim l_suppliercode As String = l_strval11

            'Dim l_seqno As String = l_strval12
            If Trim(l_PostDate) <> Nothing Then

                dr = dt.NewRow()
                'If (l_accountno.Length <> 15 And l_accountno.Length <> 0) Then
                '    Dim l_Mypos As Integer = l_accountno.Length
                '    l_accountno = l_accountno.Insert(4, "/")
                '    l_accountno = l_accountno.Insert(l_Mypos - 3, "/")



                'End If
                Dim accountrep As String
                If EazyClass.AccountExist(Trim(l_Accountno.Replace(",", ""))) = True Then
                    accountrep = Trim(l_Accountno.Replace(",", ""))
                Else
                    Dim myoldacctno As String = "t" & Trim(l_Accountno.Replace(",", ""))

                    If EazyClass.AccountExist(EazyClass.FullAccount(StrLocation, myoldacctno)) = True Then
                        accountrep = EazyClass.FullAccount(StrLocation, myoldacctno)
                    Else
                        accountrep = Trim(l_Accountno.Replace(",", ""))

                    End If
                End If

                dr(0) = accountrep 'Trim(l_accountno.Replace(",", ""))
                dr(1) = Replace(Replace(l_PostDate, ",", ""), ",", "")
                dr(2) = l_TransDate.Replace(",", "").Replace(",", "")

                dr(3) = Trim(l_Description.Replace(",", ""))
                dr(4) = Replace(Replace(l_ContactAddress, ",", ""), ",", "")
                dr(5) = l_Telephone.Replace(",", "").Replace(",", "")
                dr(6) = Trim(l_DepositSlipNo.Replace(",", ""))
                dr(7) = Trim(l_Amount.Replace(",", ""))
                dr(8) = Trim(l_PromoRemarks.Replace(",", ""))
                dr(9) = Trim(l_Product.Replace(",", ""))
                dr(10) = "Pass"

                dt.Rows.Add(dr)
            End If
        Loop Until line Is Nothing
        sr.Close()

        DataGrid1.DataSource = dt

        DataGrid1.DataBind()

        Dim str_error As String = ValidateEntriesPromo()
        If str_error = Nothing Then

        Else
            'Me.cust_val.IsValid = False
            'Me.cust_val.ToolTip = "Record Already Exist !!" & str_error
            Response.Write("<script>alert(' The following entries are with errors !!" & str_error & "') </script>")
            'DataGrid2.Visible = True
        End If


    End Sub

    Sub InsertAllRecordsPromoAccts(ByVal m_strFileName As String)

        luser = Context.User.Identity.Name
        StrLocation = luser.Substring(luser.LastIndexOf("?") + 1)
        struser = Mid(luser, 1, luser.LastIndexOf("?"))
        guser = struser

        ' Assign a new object to the variable.
        Dim l_strval31 As String = m_strFileName  ' backupname.Value.Trim
        'backupname.Value = "C:\TempMultipleFiles\" & m_strFileName
        'Dim sr As StreamReader = New StreamReader(backupname.Value.Trim)
        'Dim sr As StreamReader = New StreamReader("C:\TempMultipleFiles\" & m_strFileName)

        Dim line As String
        Dim dsp As New DataSet, dr As DataRow, dt As New DataTable, dc As DataColumn
        dt.Columns.Add(New DataColumn("Account RefNo ", GetType(String)))
        dt.Columns.Add(New DataColumn("Post Date", GetType(String)))
        dt.Columns.Add(New DataColumn("Trans Date", GetType(String)))
        dt.Columns.Add(New DataColumn("CustomerNo", GetType(String)))
        dt.Columns.Add(New DataColumn("Title ", GetType(String)))
        dt.Columns.Add(New DataColumn("Customer ProfileName  ", GetType(String)))
        dt.Columns.Add(New DataColumn("Sex", GetType(String)))
        dt.Columns.Add(New DataColumn("DateOfBirth", GetType(String)))
        dt.Columns.Add(New DataColumn("Old NETAcctNo  ", GetType(String)))
        dt.Columns.Add(New DataColumn("Old NETAcctRefName", GetType(String)))
        dt.Columns.Add(New DataColumn("Customer ContactAddress", GetType(String)))
        dt.Columns.Add(New DataColumn("Telephone", GetType(String)))
        dt.Columns.Add(New DataColumn("Product Description", GetType(String)))
        dt.Columns.Add(New DataColumn("BranchCode", GetType(String)))
        dt.Columns.Add(New DataColumn("SystemRemarks", GetType(String)))

        ' Dim filename As String = Server.MapPath(docpath)
        Dim thisDataSet As New DataSet
        ' Create a FileStream object with the file path and name.
        Dim myFileStream As New System.IO.FileStream _
           ("C:\TempMultipleFiles\" & m_strFileName, FileMode.Open)
        ' Create a new XmlTextReader object with the FileStream.
        Dim myXmlTextReader As New System.Xml.XmlTextReader(myFileStream)
        ' Read the schema into the DataSet and close the reader.
        thisDataSet.ReadXml(myXmlTextReader)
        myXmlTextReader.Close()

        Dim row As DataRow
        For Each row In thisDataSet.Tables(0).Rows
            dr = dt.NewRow()
            dr(0) = row("old_account_no")
            dr(1) = Format(EazyClass.GetPostingDate, "dd-MMM-yyyy") '  row("post_date")
            dr(2) = Format(CDate(row("post_date")), "dd-MMM-yyyy")
            dr(3) = row("cust_no")

            dr(4) = row("title_code")
            dr(5) = row("names")
            dr(6) = row("sex")

            dr(7) = row("birthdate").ToString()
            dr(8) = row("account_no")
            dr(9) = row("account_desc")
            dr(10) = row("address")
            dr(11) = row("telephone")
            dr(12) = row("prod_code")
            dr(13) = row("headsub_code")
            dr(14) = "Pass"

            dt.Rows.Add(dr)





        Next




        DataGrid1.DataSource = dt
        DataGrid1.DataBind()
        ' format columns
        DataGrid1.Columns(2).HeaderStyle.Wrap = False

        DataGrid1.Columns(2).ItemStyle.Wrap = False
        DataGrid1.Columns(1).HeaderStyle.Wrap = False
        DataGrid1.Columns(1).ItemStyle.Wrap = False



        'Dim str_error As String = ValidateEntriesPromo()
        'If str_error = Nothing Then

        'Else
        '    'Me.cust_val.IsValid = False
        '    'Me.cust_val.ToolTip = "Record Already  Exist !!" & str_error
        '    Response.Write("<script>alert(' The following entries are with errors !!" & str_error & "') </script>")
        '    'DataGrid2.Visible = True
        'End If


    End Sub


    Sub InsertAllRecordsATMRecon(ByVal m_strFileName As String)

        luser = Context.User.Identity.Name
        StrLocation = luser.Substring(luser.LastIndexOf("?") + 1)
        struser = Mid(luser, 1, luser.LastIndexOf("?"))
        guser = struser

        ' Assign a new object to the variable.
        Dim l_strval31 As String = m_strFileName  ' backupname.Value.Trim
        'backupname.Value = "C:\TempMultipleFiles\" & m_strFileName
        'Dim sr As StreamReader = New StreamReader(backupname.Value.Trim)
        Dim sr As StreamReader = New StreamReader("C:\TempMultipleFiles\" & m_strFileName)

        Dim line As String
        Dim dsp As New DataSet, dr As DataRow, dt As New DataTable, dc As DataColumn
        dt.Columns.Add(New DataColumn("Trans DateTime", GetType(String)))
        dt.Columns.Add(New DataColumn("PAN", GetType(String)))

        dt.Columns.Add(New DataColumn("AccountNo", GetType(String)))
        dt.Columns.Add(New DataColumn("Requested Amount", GetType(String)))
        dt.Columns.Add(New DataColumn("Authorised Amount", GetType(String)))
        dt.Columns.Add(New DataColumn("Fee Amount", GetType(String)))
        dt.Columns.Add(New DataColumn("Narration", GetType(String)))
        dt.Columns.Add(New DataColumn("Transaction Reference", GetType(String)))
        dt.Columns.Add(New DataColumn("Transaction Status", GetType(String)))
        dt.Columns.Add(New DataColumn("SystemRemarks", GetType(String)))
        'sr.ReadLine()
        Do
            line = sr.ReadLine()
            Dim l_val As Integer = Len(line)

            Dim str_map As String = line



            Dim kdod As String = line
            If Trim(line).Length = 0 Then
                Exit Do
            End If

            Dim kps1 As Integer = kdod.IndexOf(",", 1)
            Dim kps2 As Integer = kdod.IndexOf(",", kps1 + 1)
            Dim kps3 As Integer = kdod.IndexOf(",", kps2 + 1)
            Dim kps4 As Integer = kdod.IndexOf(",", kps3 + 1)
            Dim kps5 As Integer = kdod.IndexOf(",", kps4 + 1)
            Dim kps6 As Integer = kdod.IndexOf(",", kps5 + 1)
            Dim kps7 As Integer = kdod.IndexOf(",", kps6 + 1)
            Dim kps8 As Integer = kdod.IndexOf(",", kps7 + 1)
            Dim kps9 As Integer = kdod.IndexOf(",", kps8 + 1)
            Dim kps10 As Integer = kdod.IndexOf(",", kps9 + 1)
            Dim kps11 As Integer = kdod.IndexOf(",", kps10 + 1)
            Dim kps12 As Integer = kdod.IndexOf(",", kps11 + 1)
            Dim kps13 As Integer = kdod.IndexOf(",", kps12 + 1)
            Dim kps14 As Integer = kdod.IndexOf(",", kps13 + 1)
            Dim kps15 As Integer = kdod.IndexOf(",", kps14 + 1)
            Dim kps16 As Integer = kdod.IndexOf(",", kps15 + 1)
            Dim kps17 As Integer = kdod.IndexOf(",", kps16 + 1)
            'Dim kps18 As Integer = kdod.IndexOf(",", kps17 + 1)
            'Dim kps19 As Integer = kdod.IndexOf(",", kps18 + 1)
            'Dim kps20 As Integer = kdod.IndexOf(",", kps19 + 1)

            'Report Start Date,ReportEnd Date,PAN,TERMINAL_ID,CARD ACCEPTOR_ID,& _
            'CARD ACCEPTOR_NAME,SOURCE NODE,SINK NODE,TRAN TYPE,RSP CODE,MESSAGE TYPE,
            'DATE TIME,REQUESTED AMOUNT,FINAL AMOUNT,SURCHARGE,STAN,RERIEVAL REF,SETTLE AMOUNT IMPACT,TRAN TYPE,RESPONSE


            Dim l_Transnarr As String = Mid(line, 1, kps1).Replace(Chr(34), "")
            Dim l_ReportPostdate As String = line.Substring(kps1 + 1, kps2 - (kps1 + 1)).Replace(Chr(34), "") ' Mid(line, kps2)
            Dim l_CompName As String = line.Substring(kps2 + 1, kps3 - (kps2 + 1)).Replace(Chr(34), "")
            Dim l_empty1 As String = line.Substring(kps3 + 1, kps4 - (kps3 + 1)).Replace(Chr(34), "")
            Dim l_empty2 As String = line.Substring(kps4 + 1, kps5 - (kps4 + 1)).Replace(Chr(34), "") ' Mid(line, kps2)
            Dim l_transdatetime As String = line.Substring(kps5 + 1, kps6 - (kps5 + 1)).Replace(Chr(34), "")
            Dim l_stans As String = line.Substring(kps6 + 1, kps7 - (kps6 + 1)).Replace(Chr(34), "")
            Dim l_seqno As String = line.Substring(kps7 + 1, kps8 - (kps7 + 1)).Replace(Chr(34), "")
            Dim l_pan As String = line.Substring(kps8 + 1, kps9 - (kps8 + 1)).Replace(Chr(34), "")
            Dim l_narr As String = line.Substring(kps9 + 1, kps10 - (kps9 + 1)).Replace(Chr(34), "")
            Dim l_acctno As String = line.Substring(kps10 + 1, kps11 - (kps10 + 1)).Replace(Chr(34), "")
            Dim l_msgstatus As String = line.Substring(kps11 + 1, kps12 - (kps11 + 1)).Replace(Chr(34), "")
            Dim l_reqamount As String = line.Substring(kps12 + 1, kps13 - (kps12 + 1)).Replace(Chr(34), "")
            Dim l_finalamount As String = line.Substring(kps13 + 1, kps14 - (kps13 + 1)).Replace(Chr(34), "")
            Dim l_feeamount As String = line.Substring(kps14 + 1, kps15 - (kps14 + 1)).Replace(Chr(34), "")
            Dim l_transamt As String = line.Substring(kps15 + 1, kps16 - (kps15 + 1)).Replace(Chr(34), "")
            Dim l_refno As String = line.Substring(kps16 + 1).Replace(Chr(34), "")
            'Dim l_transamt As String = line.Substring(kps17 + 1, kps18 - (kps17 + 1))
            ''Dim l_msgtranstype As String = line.Substring(kps18 + 1, kps19 - (kps18 + 1))
            'Dim l_msgstatus As String = line.Substring(kps19 + 1)

            l_msgstatus = String.Format("{0}", l_msgstatus.Replace(Chr(34), ""))
            '' Replace invalid characters with empty strings.
            'Return Regex.Replace(strIn, "[^\w\.@-]", "")
            'Response.Write(Chr(35))
            'If Trim(l_PostDate) <> Nothing Then
            If Trim(l_msgstatus) = "Approved" And Trim(l_msgstatus) <> Nothing Then
                dr = dt.NewRow()


                dr(0) = l_transdatetime
                dr(1) = l_pan
                dr(2) = l_acctno
                dr(3) = l_reqamount
                dr(4) = l_finalamount
                dr(5) = l_feeamount
                dr(6) = l_narr
                dr(7) = l_refno
                dr(8) = l_msgstatus
                dr(9) = "Pass"

                dt.Rows.Add(dr)
            End If
        Loop Until line Is Nothing
        'sr.Close()

        DataGrid1.DataSource = dt

        DataGrid1.DataBind()

        Dim str_error As String = "true" 'ValidateEntriesPromo()
        If str_error = Nothing Then

        Else
            'Me.cust_val.IsValid = False
            'Me.cust_val.ToolTip = "Record Already Exist !!" & str_error
            Response.Write("<script>alert(' The following entries are with errors !!" & str_error & "') </script>")
            'DataGrid2.Visible = True
        End If


    End Sub



    Sub InsertAllRecordsATMReChargeRecon(ByVal m_strFileName As String)

        luser = Context.User.Identity.Name
        StrLocation = luser.Substring(luser.LastIndexOf("?") + 1)
        struser = Mid(luser, 1, luser.LastIndexOf("?"))
        guser = struser

        ' Assign a new object to the variable.
        Dim l_strval31 As String = m_strFileName  ' backupname.Value.Trim
        'backupname.Value = "C:\TempMultipleFiles\" & m_strFileName
        'Dim sr As StreamReader = New StreamReader(backupname.Value.Trim)
        Dim sr As StreamReader = New StreamReader("C:\TempMultipleFiles\" & m_strFileName)

        Dim line As String
        Dim dsp As New DataSet, dr As DataRow, dt As New DataTable, dc As DataColumn
        dt.Columns.Add(New DataColumn("Trans DateTime", GetType(String)))
        dt.Columns.Add(New DataColumn("PAN", GetType(String)))

        dt.Columns.Add(New DataColumn("AccountNo", GetType(String)))
        dt.Columns.Add(New DataColumn("Requested Amount", GetType(String)))
        dt.Columns.Add(New DataColumn("Authorised Amount", GetType(String)))
        dt.Columns.Add(New DataColumn("Fee Amount", GetType(String)))
        dt.Columns.Add(New DataColumn("Narration", GetType(String)))
        dt.Columns.Add(New DataColumn("Transaction Reference", GetType(String)))
        dt.Columns.Add(New DataColumn("Transaction Status", GetType(String)))
        dt.Columns.Add(New DataColumn("SystemRemarks", GetType(String)))
        'sr.ReadLine()
        Do
            line = sr.ReadLine()
            Dim l_val As Integer = Len(line)

            Dim str_map As String = line



            Dim kdod As String = line
            If Trim(line).Length = 0 Then
                Exit Do
            End If

            Dim kps1 As Integer = kdod.IndexOf(",", 1)
            Dim kps2 As Integer = kdod.IndexOf(",", kps1 + 1)
            Dim kps3 As Integer = kdod.IndexOf(",", kps2 + 1)
            Dim kps4 As Integer = kdod.IndexOf(",", kps3 + 1)
            Dim kps5 As Integer = kdod.IndexOf(",", kps4 + 1)
            Dim kps6 As Integer = kdod.IndexOf(",", kps5 + 1)
            Dim kps7 As Integer = kdod.IndexOf(",", kps6 + 1)
            Dim kps8 As Integer = kdod.IndexOf(",", kps7 + 1)
            Dim kps9 As Integer = kdod.IndexOf(",", kps8 + 1)
            Dim kps10 As Integer = kdod.IndexOf(",", kps9 + 1)
            Dim kps11 As Integer = kdod.IndexOf(",", kps10 + 1)
            Dim kps12 As Integer = kdod.IndexOf(",", kps11 + 1)
            Dim kps13 As Integer = kdod.IndexOf(",", kps12 + 1)
            Dim kps14 As Integer = kdod.IndexOf(",", kps13 + 1)
            Dim kps15 As Integer = kdod.IndexOf(",", kps14 + 1)
            Dim kps16 As Integer = kdod.IndexOf(",", kps15 + 1)
            Dim kps17 As Integer = kdod.IndexOf(",", kps16 + 1)
            Dim kps18 As Integer = kdod.IndexOf(",", kps17 + 1)
            Dim kps19 As Integer = kdod.IndexOf(",", kps18 + 1)
            Dim kps20 As Integer = kdod.IndexOf(",", kps19 + 1)
            Dim kps21 As Integer = kdod.IndexOf(",", kps20 + 1)

            'Report Start Date,ReportEnd Date,PAN,TERMINAL_ID,CARD ACCEPTOR_ID,& _
            'CARD ACCEPTOR_NAME,SOURCE NODE,SINK NODE,TRAN TYPE,RSP CODE,MESSAGE TYPE,
            'DATE TIME,REQUESTED AMOUNT,FINAL AMOUNT,SURCHARGE,STAN,RERIEVAL REF,SETTLE AMOUNT IMPACT,TRAN TYPE,RESPONSE


            Dim l_Transnarr As String = Mid(line, 1, kps1).Replace(Chr(34), "")
            Dim l_ReportPostdate As String = line.Substring(kps1 + 1, kps2 - (kps1 + 1)).Replace(Chr(34), "") ' Mid(line, kps2)
            Dim l_CompName As String = line.Substring(kps2 + 1, kps3 - (kps2 + 1)).Replace(Chr(34), "")
            Dim l_empty1 As String = line.Substring(kps3 + 1, kps4 - (kps3 + 1)).Replace(Chr(34), "")
            Dim l_empty2 As String = line.Substring(kps4 + 1, kps5 - (kps4 + 1)).Replace(Chr(34), "") ' Mid(line, kps2)


            Dim l_transdatetime As String = line.Substring(kps9 + 1, kps10 - (kps9 + 1)).Replace(Chr(34), "")
            Dim l_stans As String = line.Substring(kps10 + 1, kps11 - (kps10 + 1)).Replace(Chr(34), "")
            Dim l_seqno As String = line.Substring(kps11 + 1, kps12 - (kps11 + 1)).Replace(Chr(34), "")
            Dim l_pan As String = line.Substring(kps12 + 1, kps13 - (kps12 + 1)).Replace(Chr(34), "")
            Dim l_narr As String = line.Substring(kps13 + 1, kps14 - (kps13 + 1)).Replace(Chr(34), "")


            Dim l_acctno As String = line.Substring(kps14 + 1, kps15 - (kps14 + 1)).Replace(Chr(34), "")
            Dim l_msgstatus As String = line.Substring(kps15 + 1, kps16 - (kps15 + 1)).Replace(Chr(34), "")
            Dim l_reqamount As String = line.Substring(kps16 + 1, kps17 - (kps16 + 1)).Replace(Chr(34), "")
            Dim l_finalamount As String = line.Substring(kps17 + 1, kps18 - (kps17 + 1)).Replace(Chr(34), "")
            Dim l_feeamount As String = line.Substring(kps18 + 1, kps19 - (kps18 + 1)).Replace(Chr(34), "")
            Dim l_transamt As String = line.Substring(kps19 + 1, kps20 - (kps19 + 1)).Replace(Chr(34), "")
            Dim l_refno As String = line.Substring(kps20 + 1).Replace(Chr(34), "")
            ''Dim l_transamt As String = line.Substring(kps17 + 1, kps18 - (kps17 + 1))
            ''Dim l_msgtranstype As String = line.Substring(kps18 + 1, kps19 - (kps18 + 1))
            'Dim l_msgstatus As String = line.Substring(kps19 + 1)

            l_msgstatus = String.Format("{0}", l_msgstatus.Replace(Chr(34), ""))
            '' Replace invalid characters with empty strings.
            'Return Regex.Replace(strIn, "[^\w\.@-]", "")
            'Response.Write(Chr(35))
            'If Trim(l_PostDate) <> Nothing Then
            If Trim(l_msgstatus) = "Approved" And Trim(l_msgstatus) <> Nothing Then
                dr = dt.NewRow()


                dr(0) = l_transdatetime
                dr(1) = l_pan
                dr(2) = l_acctno
                dr(3) = l_reqamount
                dr(4) = l_finalamount
                dr(5) = l_feeamount
                dr(6) = l_narr
                dr(7) = l_refno
                dr(8) = l_msgstatus
                dr(9) = "Pass"

                dt.Rows.Add(dr)
            End If
        Loop Until line Is Nothing
        'sr.Close()

        DataGrid1.DataSource = dt

        DataGrid1.DataBind()

        Dim str_error As String = "true" 'ValidateEntriesPromo()
        If str_error = Nothing Then

        Else
            'Me.cust_val.IsValid = False
            'Me.cust_val.ToolTip = "Record Already Exist !!" & str_error
            Response.Write("<script>alert(' The following entries are with errors !!" & str_error & "') </script>")
            'DataGrid2.Visible = True
        End If


    End Sub

    Sub InsertAllRecordsATMRePOSRecon(ByVal m_strFileName As String)

        luser = Context.User.Identity.Name
        StrLocation = luser.Substring(luser.LastIndexOf("?") + 1)
        struser = Mid(luser, 1, luser.LastIndexOf("?"))
        guser = struser

        ' Assign a new object to the variable.
        Dim l_strval31 As String = m_strFileName  ' backupname.Value.Trim
        'backupname.Value = "C:\TempMultipleFiles\" & m_strFileName
        'Dim sr As StreamReader = New StreamReader(backupname.Value.Trim)
        Dim sr As StreamReader = New StreamReader("C:\TempMultipleFiles\" & m_strFileName)

        Dim line As String
        Dim dsp As New DataSet, dr As DataRow, dt As New DataTable, dc As DataColumn
        dt.Columns.Add(New DataColumn("Trans DateTime", GetType(String)))
        dt.Columns.Add(New DataColumn("PAN", GetType(String)))

        dt.Columns.Add(New DataColumn("AccountNo", GetType(String)))
        dt.Columns.Add(New DataColumn("Requested Amount", GetType(String)))
        dt.Columns.Add(New DataColumn("Authorised Amount", GetType(String)))
        dt.Columns.Add(New DataColumn("Fee Amount", GetType(String)))
        dt.Columns.Add(New DataColumn("Narration", GetType(String)))
        dt.Columns.Add(New DataColumn("Transaction Reference", GetType(String)))
        dt.Columns.Add(New DataColumn("Transaction Status", GetType(String)))
        dt.Columns.Add(New DataColumn("SystemRemarks", GetType(String)))
        'sr.ReadLine()
        Do
            line = sr.ReadLine()
            Dim l_val As Integer = Len(line)

            Dim str_map As String = line



            Dim kdod As String = line
            If Trim(line).Length = 0 Then
                Exit Do
            End If
            Dim kps0 As Integer = kdod.IndexOf(",", 1)
            Dim kps1 As Integer = kdod.IndexOf(",", kps0 + 1)
            Dim kps2 As Integer = kdod.IndexOf(",", kps1 + 1)
            Dim kps3 As Integer = kdod.IndexOf(",", kps2 + 1)
            Dim kps4 As Integer = kdod.IndexOf(",", kps3 + 1)
            Dim kps5 As Integer = kdod.IndexOf(",", kps4 + 1)
            Dim kps6 As Integer = kdod.IndexOf(",", kps5 + 1)
            Dim kps7 As Integer = kdod.IndexOf(",", kps6 + 1)
            Dim kps8 As Integer = kdod.IndexOf(",", kps7 + 1)
            Dim kps9 As Integer = kdod.IndexOf(",", kps8 + 1)
            Dim kps10 As Integer = kdod.IndexOf(",", kps9 + 1)
            Dim kps11 As Integer = kdod.IndexOf(",", kps10 + 1)
            Dim kps12 As Integer = kdod.IndexOf(",", kps11 + 1)
            Dim kps13 As Integer = kdod.IndexOf(",", kps12 + 1)
            Dim kps14 As Integer = kdod.IndexOf(",", kps13 + 1)
            Dim kps15 As Integer = kdod.IndexOf(",", kps14 + 1)
            Dim kps16 As Integer = kdod.IndexOf(",", kps15 + 1)
            Dim kps17 As Integer = kdod.IndexOf(",", kps16 + 1)
            Dim kps18 As Integer = kdod.IndexOf(",", kps17 + 1)
            Dim kps19 As Integer = kdod.IndexOf(",", kps18 + 1)
            Dim kps20 As Integer = kdod.IndexOf(",", kps19 + 1)
            Dim kps21 As Integer = kdod.IndexOf(",", kps20 + 1)

            'Report Start Date,ReportEnd Date,PAN,TERMINAL_ID,CARD ACCEPTOR_ID,& _
            'CARD ACCEPTOR_NAME,SOURCE NODE,SINK NODE,TRAN TYPE,RSP CODE,MESSAGE TYPE,
            'DATE TIME,REQUESTED AMOUNT,FINAL AMOUNT,SURCHARGE,STAN,RERIEVAL REF,SETTLE AMOUNT IMPACT,TRAN TYPE,RESPONSE

            Dim l_narrs As String = Mid(line, 1, kps0).Replace(Chr(34), "")
            Dim l_Transnarr As String = line.Substring(kps0 + 1, kps1 - (kps0 + 1)).Replace(Chr(34), "")
            Dim l_ReportPostdate As String = line.Substring(kps1 + 1, kps2 - (kps1 + 1)).Replace(Chr(34), "") ' Mid(line, kps2)
            Dim l_CompName As String = line.Substring(kps2 + 1, kps3 - (kps2 + 1)).Replace(Chr(34), "")
            Dim l_empty1 As String = line.Substring(kps3 + 1, kps4 - (kps3 + 1)).Replace(Chr(34), "")
            Dim l_empty2 As String = line.Substring(kps4 + 1, kps5 - (kps4 + 1)).Replace(Chr(34), "") ' Mid(line, kps2)
            Dim l_transdatetime As String = line.Substring(kps5 + 1, kps6 - (kps5 + 1)).Replace(Chr(34), "") ' Mid(line, kps2)
            Dim l_stans As String = line.Substring(kps6 + 1, kps7 - (kps6 + 1)).Replace(Chr(34), "")
            Dim l_seqno As String = line.Substring(kps7 + 1, kps8 - (kps7 + 1)).Replace(Chr(34), "")
            Dim l_pan As String = line.Substring(kps8 + 1, kps9 - (kps8 + 1)).Replace(Chr(34), "")
            Dim l_acctno As String = line.Substring(kps9 + 1, kps10 - (kps9 + 1)).Replace(Chr(34), "")
            Dim l_narr As String = line.Substring(kps10 + 1, kps11 - (kps10 + 1)).Replace(Chr(34), "")
            Dim l_transnarrType As String = line.Substring(kps11 + 1, kps12 - (kps11 + 1)).Replace(Chr(34), "")

            Dim l_msgstatus As String = line.Substring(kps12 + 1, kps13 - (kps12 + 1)).Replace(Chr(34), "")



            Dim l_reqamount As String = line.Substring(kps13 + 1, kps14 - (kps13 + 1)).Replace(Chr(34), "")
            Dim l_finalamount As String = line.Substring(kps14 + 1, kps15 - (kps14 + 1)).Replace(Chr(34), "")
            Dim l_feeamount As String = line.Substring(kps15 + 1, kps16 - (kps15 + 1)).Replace(Chr(34), "")
            Dim l_transamt As String = line.Substring(kps16 + 1, kps17 - (kps16 + 1)).Replace(Chr(34), "")
            Dim l_refno As String = line.Substring(kps17 + 1, kps18 - (kps17 + 1)).Replace(Chr(34), "")
            Dim l_refnoRems As String = line.Substring(kps18 + 1).Replace(Chr(34), "")
            ''Dim l_transamt As String = line.Substring(kps17 + 1, kps18 - (kps17 + 1))
            ''Dim l_msgtranstype As String = line.Substring(kps18 + 1, kps19 - (kps18 + 1))
            'Dim l_msgstatus As String = line.Substring(kps19 + 1)

            l_msgstatus = String.Format("{0}", l_msgstatus.Replace(Chr(34), ""))
            '' Replace invalid characters with empty strings.
            'Return Regex.Replace(strIn, "[^\w\.@-]", "")
            'Response.Write(Chr(35))
            'If Trim(l_PostDate) <> Nothing Then
            If Trim(l_msgstatus) = "Approved" And Trim(l_msgstatus) <> Nothing Then
                dr = dt.NewRow()


                dr(0) = l_transdatetime
                dr(1) = l_pan
                dr(2) = l_acctno
                dr(3) = l_reqamount
                dr(4) = l_finalamount
                dr(5) = l_feeamount
                dr(6) = l_empty1
                dr(7) = l_refno
                dr(8) = l_msgstatus
                dr(9) = "Pass"

                dt.Rows.Add(dr)
            End If
        Loop Until line Is Nothing
        'sr.Close()

        DataGrid1.DataSource = dt

        DataGrid1.DataBind()

        Dim str_error As String = "true" 'ValidateEntriesPromo()
        If str_error = Nothing Then

        Else
            'Me.cust_val.IsValid = False
            'Me.cust_val.ToolTip = "Record Already Exist !!" & str_error
            Response.Write("<script>alert(' The following entries are with errors !!" & str_error & "') </script>")
            'DataGrid2.Visible = True
        End If


    End Sub

    Sub InsertAllRecordsATMReMerPOSRecon(ByVal m_strFileName As String)

        luser = Context.User.Identity.Name
        StrLocation = luser.Substring(luser.LastIndexOf("?") + 1)
        struser = Mid(luser, 1, luser.LastIndexOf("?"))
        guser = struser

        ' Assign a new object to the variable.
        Dim l_strval31 As String = m_strFileName  ' backupname.Value.Trim
        'backupname.Value = "C:\TempMultipleFiles\" & m_strFileName
        'Dim sr As StreamReader = New StreamReader(backupname.Value.Trim)
        Dim sr As StreamReader = New StreamReader("C:\TempMultipleFiles\" & m_strFileName)

        Dim line As String
        Dim dsp As New DataSet, dr As DataRow, dt As New DataTable, dc As DataColumn
        'dt.Columns.Add(New DataColumn("BlankA", GetType(String)))
        'dt.Columns.Add(New DataColumn("Host Business Date", GetType(String)))

        'dt.Columns.Add(New DataColumn("Date", GetType(String)))
        'dt.Columns.Add(New DataColumn("BlankD", GetType(String)))
        'dt.Columns.Add(New DataColumn("TerminalOwner", GetType(String)))
        'dt.Columns.Add(New DataColumn("Reference Code", GetType(String)))
        'dt.Columns.Add(New DataColumn("TransactionDateTime", GetType(String)))
        'dt.Columns.Add(New DataColumn("TerminalID", GetType(String)))
        'dt.Columns.Add(New DataColumn("Stan", GetType(String)))
        'dt.Columns.Add(New DataColumn("TerminalOwner", GetType(String)))


        'dt.Columns.Add(New DataColumn("PAN", GetType(String)))
        'dt.Columns.Add(New DataColumn("Transaction Type", GetType(String)))
        'dt.Columns.Add(New DataColumn("AccountType", GetType(String)))
        'dt.Columns.Add(New DataColumn("Rcode", GetType(String)))
        'dt.Columns.Add(New DataColumn("ExchangeRate", GetType(String)))

        'dt.Columns.Add(New DataColumn("FinalAmount", GetType(String)))
        'dt.Columns.Add(New DataColumn("NET FeeAmt", GetType(String)))
        'dt.Columns.Add(New DataColumn("GrossAmount", GetType(String)))
        'dt.Columns.Add(New DataColumn("TransRefNo", GetType(String)))
        'dt.Columns.Add(New DataColumn("AccountNo", GetType(String)))

        'dt.Columns.Add(New DataColumn("NET Amount", GetType(String)))



        dt.Columns.Add(New DataColumn("Trans DateTime", GetType(String)))
        dt.Columns.Add(New DataColumn("PAN", GetType(String)))

        dt.Columns.Add(New DataColumn("AccountNo", GetType(String)))
        dt.Columns.Add(New DataColumn("Requested Amount", GetType(String)))
        dt.Columns.Add(New DataColumn("Authorised Amount", GetType(String)))
        dt.Columns.Add(New DataColumn("Fee Amount", GetType(String)))
        dt.Columns.Add(New DataColumn("Narration", GetType(String)))
        dt.Columns.Add(New DataColumn("Transaction Reference", GetType(String)))
        dt.Columns.Add(New DataColumn("Transaction Status", GetType(String)))
        dt.Columns.Add(New DataColumn("SystemRemarks", GetType(String)))


        'sr.ReadLine()
        Do
            line = sr.ReadLine()
            Dim l_val As Integer = Len(line)

            Dim str_map As String = line



            Dim kdod As String = line
            If Trim(line).Length = 0 Then
                Exit Do
            End If
            Dim kps0 As Integer = kdod.IndexOf(",", 1)
            Dim kps1 As Integer = kdod.IndexOf(",", kps0 + 1)
            Dim kps2 As Integer = kdod.IndexOf(",", kps1 + 1)
            Dim kps3 As Integer = kdod.IndexOf(",", kps2 + 1)
            Dim kps4 As Integer = kdod.IndexOf(",", kps3 + 1)
            Dim kps5 As Integer = kdod.IndexOf(",", kps4 + 1)
            Dim kps6 As Integer = kdod.IndexOf(",", kps5 + 1)
            Dim kps7 As Integer = kdod.IndexOf(",", kps6 + 1)
            Dim kps8 As Integer = kdod.IndexOf(",", kps7 + 1)
            Dim kps9 As Integer = kdod.IndexOf(",", kps8 + 1)
            Dim kps10 As Integer = kdod.IndexOf(",", kps9 + 1)
            Dim kps11 As Integer = kdod.IndexOf(",", kps10 + 1)
            Dim kps12 As Integer = kdod.IndexOf(",", kps11 + 1)
            Dim kps13 As Integer = kdod.IndexOf(",", kps12 + 1)
            Dim kps14 As Integer = kdod.IndexOf(",", kps13 + 1)
            Dim kps15 As Integer = kdod.IndexOf(",", kps14 + 1)
            Dim kps16 As Integer = kdod.IndexOf(",", kps15 + 1)
            Dim kps17 As Integer = kdod.IndexOf(",", kps16 + 1)
            Dim kps18 As Integer = kdod.IndexOf(",", kps17 + 1)
            Dim kps19 As Integer = kdod.IndexOf(",", kps18 + 1)
            Dim kps20 As Integer = kdod.IndexOf(",", kps19 + 1)
            Dim kps21 As Integer = kdod.IndexOf(",", kps20 + 1)

            'Report Start Date,ReportEnd Date,PAN,TERMINAL_ID,CARD ACCEPTOR_ID,& _
            'CARD ACCEPTOR_NAME,SOURCE NODE,SINK NODE,TRAN TYPE,RSP CODE,MESSAGE TYPE,
            'DATE TIME,REQUESTED AMOUNT,FINAL AMOUNT,SURCHARGE,STAN,RERIEVAL REF,SETTLE AMOUNT IMPACT,TRAN TYPE,RESPONSE

            Dim l_narrs As String = Mid(line, 1, kps0).Replace(Chr(34), "")
            Dim l_Transnarr As String = line.Substring(kps0 + 1, kps1 - (kps0 + 1)).Replace(Chr(34), "")
            Dim l_ReportPostdate As String = line.Substring(kps1 + 1, kps2 - (kps1 + 1)).Replace(Chr(34), "") ' Mid(line, kps2)
            Dim l_CompName As String = line.Substring(kps2 + 1, kps3 - (kps2 + 1)).Replace(Chr(34), "")
            Dim l_empty1 As String = line.Substring(kps3 + 1, kps4 - (kps3 + 1)).Replace(Chr(34), "")
            Dim l_empty2 As String = line.Substring(kps4 + 1, kps5 - (kps4 + 1)).Replace(Chr(34), "") ' Mid(line, kps2)
            Dim l_transdatetime As String = line.Substring(kps5 + 1, kps6 - (kps5 + 1)).Replace(Chr(34), "") ' Mid(line, kps2)
            Dim l_terminalID As String = line.Substring(kps6 + 1, kps7 - (kps6 + 1)).Replace(Chr(34), "") ' Mid(line, kps2)

            Dim l_stans As String = line.Substring(kps7 + 1, kps8 - (kps7 + 1)).Replace(Chr(34), "")
            Dim l_TerminalOwner As String = line.Substring(kps8 + 1, kps9 - (kps8 + 1)).Replace(Chr(34), "")
            Dim l_pan As String = line.Substring(kps9 + 1, kps10 - (kps9 + 1)).Replace(Chr(34), "")
            Dim l_transnarrType As String = line.Substring(kps10 + 1, kps11 - (kps10 + 1)).Replace(Chr(34), "")
            Dim l_acctnotype As String = line.Substring(kps11 + 1, kps12 - (kps11 + 1)).Replace(Chr(34), "")
            Dim l_Rcode As String = line.Substring(kps12 + 1, kps13 - (kps12 + 1)).Replace(Chr(34), "")
            Dim l_exchangerate As String = line.Substring(kps13 + 1, kps14 - (kps13 + 1)).Replace(Chr(34), "")
            Dim l_reqamount As String = line.Substring(kps14 + 1, kps15 - (kps14 + 1)).Replace(Chr(34), "")
            Dim l_NETFeeAmt As String = line.Substring(kps15 + 1, kps16 - (kps15 + 1)).Replace(Chr(34), "")
            Dim l_finalamount As String = line.Substring(kps16 + 1, kps17 - (kps16 + 1)).Replace(Chr(34), "")
            Dim l_seqno As String = line.Substring(kps17 + 1, kps18 - (kps17 + 1)).Replace(Chr(34), "")
            Dim l_acctno As String = line.Substring(kps18 + 1, kps19 - (kps18 + 1)).Replace(Chr(34), "")
            Dim l_transamt As String = line.Substring(kps19 + 1, kps20 - (kps19 + 1)).Replace(Chr(34), "")





            'Dim l_feeamount As String = line.Substring(kps15 + 1, kps16 - (kps15 + 1)).Replace(Chr(34), "")
            'Dim l_transamt As String = line.Substring(kps16 + 1, kps17 - (kps16 + 1)).Replace(Chr(34), "")
            ''Dim l_refno As String = line.Substring(kps17 + 1, kps18 - (kps17 + 1)).Replace(Chr(34), "")
            ''Dim l_refnoRems As String = line.Substring(kps18 + 1).Replace(Chr(34), "")
            '' ''Dim l_transamt As String = line.Substring(kps17 + 1, kps18 - (kps17 + 1))
            ''Dim l_msgtranstype As String = line.Substring(kps18 + 1, kps19 - (kps18 + 1))
            'Dim l_msgstatus As String = line.Substring(kps19 + 1)

            Dim l_msgstatus = String.Format("{0}", l_Rcode.Replace(Chr(34), ""))
            '' Replace invalid characters with empty strings.
            'Return Regex.Replace(strIn, "[^\w\.@-]", "")
            'Response.Write(Chr(35))
            'If Trim(l_PostDate) <> Nothing Then
            Dim l_ResAcctNo As String = ""
            If Trim(l_msgstatus) = "Approved" And Trim(l_msgstatus) <> Nothing Then
                dr = dt.NewRow()
                Dim strsel As String = String.Format("Sproc_GetTerminalAcct '{0}','{1}'", l_terminalID, "POS")
                If (Classes.GetDataset(strsel).Tables(0).Rows.Count > 0) Then


                    l_ResAcctNo = String.Format("{0}", Classes.GetDataset(strsel).Tables(0).Rows(0)("AccountNo").ToString())
                End If

                dr(0) = l_transdatetime
                dr(1) = l_pan
                dr(2) = l_ResAcctNo
                dr(3) = l_reqamount
                dr(4) = l_transamt
                dr(5) = l_NETFeeAmt
                dr(6) = l_CompName
                dr(7) = l_seqno
                dr(8) = l_msgstatus
                dr(9) = "Pass"

                dt.Rows.Add(dr)
            End If
        Loop Until line Is Nothing
        'sr.Close()

        DataGrid1.DataSource = dt

        DataGrid1.DataBind()

        Dim str_error As String = ValidateEntriesPOS() '"true" 'ValidateEntriesPromo()
        If str_error = Nothing Then

        Else
            'Me.cust_val.IsValid = False
            'Me.cust_val.ToolTip = "Record Already Exist !!" & str_error
            Response.Write("<script>alert(' The following entries are with errors !!" & str_error & "') </script>")
            'DataGrid2.Visible = True
        End If


    End Sub




    Sub InsertAllRecordsATMReconAll(ByVal m_strFileName As String)

        luser = Context.User.Identity.Name
        StrLocation = luser.Substring(luser.LastIndexOf("?") + 1)
        struser = Mid(luser, 1, luser.LastIndexOf("?"))
        guser = struser

        ' Assign a new object to the variable.
        Dim l_strval31 As String = m_strFileName  ' backupname.Value.Trim
        'backupname.Value = "C:\TempMultipleFiles\" & m_strFileName
        'Dim sr As StreamReader = New StreamReader(backupname.Value.Trim)
        Dim sr As StreamReader = New StreamReader("C:\TempMultipleFiles\" & m_strFileName)

        Dim line As String
        Dim dsp As New DataSet, dr As DataRow, dt As New DataTable, dc As DataColumn
        dt.Columns.Add(New DataColumn("Report StartDate", GetType(String)))
        dt.Columns.Add(New DataColumn("Report EndDate", GetType(String)))
        dt.Columns.Add(New DataColumn("PAN", GetType(String)))
        dt.Columns.Add(New DataColumn("TERMINAL ID", GetType(String)))
        dt.Columns.Add(New DataColumn("CARD ACCEPTORID", GetType(String)))
        dt.Columns.Add(New DataColumn("CARD ACCEPTORNAME", GetType(String)))
        dt.Columns.Add(New DataColumn("TransTime", GetType(String)))
        dt.Columns.Add(New DataColumn("Requested Amount", GetType(String)))
        dt.Columns.Add(New DataColumn("Authorised Amount", GetType(String)))
        dt.Columns.Add(New DataColumn("Fee Amount", GetType(String)))
        dt.Columns.Add(New DataColumn("Narration", GetType(String)))
        dt.Columns.Add(New DataColumn("Transaction Reference", GetType(String)))
        dt.Columns.Add(New DataColumn("Transaction Status", GetType(String)))
        dt.Columns.Add(New DataColumn("SystemRemarks", GetType(String)))
        sr.ReadLine()
        Do
            line = sr.ReadLine()
            Dim l_val As Integer = Len(line)

            Dim str_map As String = line



            Dim kdod As String = line
            If Trim(line).Length = 0 Then
                Exit Do
            End If

            Dim kps1 As Integer = kdod.IndexOf(",", 1)
            Dim kps2 As Integer = kdod.IndexOf(",", kps1 + 1)
            Dim kps3 As Integer = kdod.IndexOf(",", kps2 + 1)
            Dim kps4 As Integer = kdod.IndexOf(",", kps3 + 1)
            Dim kps5 As Integer = kdod.IndexOf(",", kps4 + 1)
            Dim kps6 As Integer = kdod.IndexOf(",", kps5 + 1)
            Dim kps7 As Integer = kdod.IndexOf(",", kps6 + 1)
            Dim kps8 As Integer = kdod.IndexOf(",", kps7 + 1)
            Dim kps9 As Integer = kdod.IndexOf(",", kps8 + 1)
            Dim kps10 As Integer = kdod.IndexOf(",", kps9 + 1)
            Dim kps11 As Integer = kdod.IndexOf(",", kps10 + 1)
            Dim kps12 As Integer = kdod.IndexOf(",", kps11 + 1)
            Dim kps13 As Integer = kdod.IndexOf(",", kps12 + 1)
            Dim kps14 As Integer = kdod.IndexOf(",", kps13 + 1)
            Dim kps15 As Integer = kdod.IndexOf(",", kps14 + 1)
            Dim kps16 As Integer = kdod.IndexOf(",", kps15 + 1)
            Dim kps17 As Integer = kdod.IndexOf(",", kps16 + 1)
            Dim kps18 As Integer = kdod.IndexOf(",", kps17 + 1)
            Dim kps19 As Integer = kdod.IndexOf(",", kps18 + 1)
            Dim kps20 As Integer = kdod.IndexOf(",", kps19 + 1)

            'Report Start Date,ReportEnd Date,PAN,TERMINAL_ID,CARD ACCEPTOR_ID,& _
            'CARD ACCEPTOR_NAME,SOURCE NODE,SINK NODE,TRAN TYPE,RSP CODE,MESSAGE TYPE,
            'DATE TIME,REQUESTED AMOUNT,FINAL AMOUNT,SURCHARGE,STAN,RERIEVAL REF,SETTLE AMOUNT IMPACT,TRAN TYPE,RESPONSE


            Dim l_reportstartdate As String = Mid(line, 1, kps1)
            Dim l_reportenddate As String = line.Substring(kps1 + 1, kps2 - (kps1 + 1)) ' Mid(line, kps2)
            Dim l_pan As String = line.Substring(kps2 + 1, kps3 - (kps2 + 1))
            Dim l_terminalID As String = line.Substring(kps3 + 1, kps4 - (kps3 + 1))
            Dim l_CardAcceptID As String = line.Substring(kps4 + 1, kps5 - (kps4 + 1)) ' Mid(line, kps2)
            Dim l_CardAcceptName As String = line.Substring(kps5 + 1, kps6 - (kps5 + 1))
            Dim l_sourcenode As String = line.Substring(kps6 + 1, kps7 - (kps6 + 1))
            Dim l_sinknode As String = line.Substring(kps7 + 1, kps8 - (kps7 + 1))
            Dim l_trantype As String = line.Substring(kps8 + 1, kps9 - (kps8 + 1))
            Dim l_rspcode As String = line.Substring(kps9 + 1, kps10 - (kps9 + 1))
            Dim l_msgtype As String = line.Substring(kps10 + 1, kps11 - (kps10 + 1))
            Dim l_time As String = line.Substring(kps11 + 1, kps12 - (kps11 + 1))
            Dim l_reqamount As String = line.Substring(kps12 + 1, kps13 - (kps12 + 1))
            Dim l_finalamount As String = line.Substring(kps13 + 1, kps14 - (kps13 + 1))
            Dim l_feeamount As String = line.Substring(kps14 + 1, kps15 - (kps14 + 1))
            Dim l_stan As String = line.Substring(kps15 + 1, kps16 - (kps15 + 1))
            Dim l_refno As String = line.Substring(kps16 + 1, kps17 - (kps16 + 1))
            Dim l_transamt As String = line.Substring(kps17 + 1, kps18 - (kps17 + 1))
            Dim l_msgtranstype As String = line.Substring(kps18 + 1, kps19 - (kps18 + 1))
            Dim l_msgstatus As String = line.Substring(kps19 + 1)


            'If Trim(l_PostDate) <> Nothing Then
            If Trim(l_msgstatus) = "Approved" And Trim(l_msgstatus) <> Nothing Then
                dr = dt.NewRow()


                dr(0) = l_reportstartdate
                dr(1) = l_reportenddate
                dr(2) = l_pan
                dr(3) = l_terminalID
                dr(4) = l_CardAcceptID
                dr(5) = l_CardAcceptName
                dr(6) = l_time
                dr(7) = l_reqamount
                dr(8) = l_finalamount
                dr(9) = l_feeamount
                dr(10) = l_msgtranstype
                dr(11) = l_refno
                dr(12) = l_msgstatus
                dr(13) = "Pass"

                dt.Rows.Add(dr)
            End If
        Loop Until line Is Nothing
        'sr.Close()

        DataGrid1.DataSource = dt

        DataGrid1.DataBind()

        Dim str_error As String = "true" 'ValidateEntriesPromo()
        If str_error = Nothing Then

        Else
            'Me.cust_val.IsValid = False
            'Me.cust_val.ToolTip = "Record Already Exist !!" & str_error
            Response.Write("<script>alert(' The following entries are with errors !!" & str_error & "') </script>")
            'DataGrid2.Visible = True
        End If


    End Sub

    Sub InsertAllRecordHr(ByVal m_strFileName As String)
        ' Assign a new object to the variable.
        Dim l_strval31 As String = m_strFileName  ' backupname.Value.Trim
        'backupname.Value = "C:\TempMultipleFiles\" & m_strFileName
        'Dim sr As StreamReader = New StreamReader(backupname.Value.Trim)
        Dim sr As StreamReader = New StreamReader("C:\TempMultipleFiles\" & m_strFileName)

        Dim line As String
        Dim dsp As New DataSet, dr As DataRow, dt As New DataTable, dc As DataColumn
        dt.Columns.Add(New DataColumn(" Post Date   ", GetType(String)))
        dt.Columns.Add(New DataColumn("TransDate  ", GetType(String)))

        dt.Columns.Add(New DataColumn("Account No", GetType(String)))
        dt.Columns.Add(New DataColumn("Transaction Code", GetType(String)))
        dt.Columns.Add(New DataColumn("Narrative", GetType(String)))
        dt.Columns.Add(New DataColumn("Description ", GetType(String)))
        dt.Columns.Add(New DataColumn("Debit", GetType(String)))

        dt.Columns.Add(New DataColumn("Credit", GetType(String)))
        dt.Columns.Add(New DataColumn("Bank Code", GetType(String)))
        dt.Columns.Add(New DataColumn("Cheque No", GetType(String)))
        dt.Columns.Add(New DataColumn("Reference Code", GetType(String)))
        dt.Columns.Add(New DataColumn("Remarks", GetType(String)))
        Do
            line = sr.ReadLine()
            Dim l_val As Integer = Len(line)

            Dim str_map As String = line


            Dim l_pos As Integer = InStr(line, ",") 'line.IndexOf(",")
            Dim l_strval As String = Mid(line, 1, l_pos)
            Dim l_strval2 As String = Mid(line, l_pos + 1)
            Dim l_pos2 As Integer = InStr(l_strval2, ",")
            l_strval2 = Mid(l_strval2, 1, l_pos2)
            'Dim l_strval3 As String = Mid(line, l_pos + l_pos2 + 1)
            Dim l_strval3 As String = Mid(line, l_pos + l_pos2 + 1)

            Dim l_pos3 As Integer = InStr(l_strval3, ",")

            l_strval3 = Mid(l_strval3, 1, l_pos3)

            Dim l_strval4 As String = Mid(line, l_pos + l_pos2 + l_pos3 + 1)
            Dim l_pos4 As Integer = InStr(l_strval4, ",")
            l_strval4 = Mid(l_strval4, 1, l_pos4)

            Dim l_strval5 As String = Mid(line, l_pos + l_pos2 + l_pos3 + l_pos4 + 1)
            Dim l_pos5 As Integer = InStr(l_strval5, ",")
            l_strval5 = Mid(l_strval5, 1, l_pos5)



            Dim l_strval6 As String = Mid(line, l_pos + l_pos2 + l_pos3 + l_pos4 + l_pos5 + 1)
            Dim l_pos6 As Integer = InStr(l_strval6, ",")
            l_strval6 = Mid(l_strval6, 1, l_pos6)

            Dim l_strval7 As String = Mid(line, l_pos + l_pos2 + l_pos3 + l_pos4 + l_pos5 + l_pos6 + 1)
            Dim l_pos7 As Integer = InStr(l_strval7, ",")
            l_strval7 = Mid(l_strval7, 1, l_pos7)


            Dim l_strval8 As String = Mid(line, l_pos + l_pos2 + l_pos3 + l_pos4 + l_pos5 + l_pos6 + l_pos7 + 1)
            Dim l_pos8 As Integer = InStr(l_strval8, ",")
            'Dim l_pine As String = l_strval8
            l_strval8 = Mid(l_strval8, 1, l_pos8)

            Dim l_strval9 As String = Mid(line, l_pos + l_pos2 + l_pos3 + l_pos4 + l_pos5 + l_pos6 + l_pos7 + l_pos8 + 1)
            Dim l_pos9 As Integer = InStr(l_strval9, ",")
            'Dim l_pine As String = l_strval8
            l_strval9 = Mid(l_strval9, 1, l_pos9)

            Dim l_strval10 As String = Mid(line, l_pos + l_pos2 + l_pos3 + l_pos4 + l_pos5 + l_pos6 + l_pos7 + l_pos8 + l_pos9 + 1)
            Dim l_pos10 As Integer = InStr(l_strval10, ",")
            'Dim l_pine As String = l_strval8
            l_strval10 = Mid(l_strval10, 1, l_pos10)

            Dim l_strval11 As String = Mid(line, l_pos + l_pos2 + l_pos3 + l_pos4 + l_pos5 + l_pos6 + l_pos7 + l_pos8 + l_pos9 + l_pos10 + 1)
            'Dim l_pos11 As Integer = InStr(l_strval11, ",")
            ''Dim l_pine As String = l_strval8
            'l_strval11 = Mid(l_strval11, 1, l_pos11)


            'Dim l_strval12 As String = Mid(line, l_pos + l_pos2 + l_pos3 + l_pos4 + l_pos5 + l_pos6 + l_pos7 + l_pos8 + l_pos9 + l_pos10 + l_pos11 + 1)
            'Dim l_pos12 As Integer = InStr(l_strval12, ",")
            ''Dim l_pine As String = l_strval8
            'l_strval12 = Mid(l_strval12, 1, l_pos12)

            'Dim l_strval13 As String = Mid(line, l_pos + l_pos2 + l_pos3 + l_pos4 + l_pos5 + l_pos6 + l_pos7 + l_pos8 + l_pos9 + l_pos10 + l_pos11 + l_pos12 + 1)

            'Dim l_pos13 As Integer = InStr(l_strval13, ",")
            ''Dim l_pine As String = l_strval8
            'l_strval13 = Mid(l_strval13, 1, l_pos13)
            'Dim l_strval14 As String = Mid(line, l_pos + l_pos2 + l_pos3 + l_pos4 + l_pos5 + l_pos6 + l_pos7 + l_pos8 + l_pos9 + l_pos10 + l_pos11 + l_pos12 + +l_pos13 + 1)



            Dim l_PostDate As String = Format(EazyClass.GetPostingDate(), "dd-MMM-yyy") 'l_strval
            Dim l_transDate As String = l_strval
            Dim l_accountno As String = l_strval2
            Dim l_transcode As String = "016"
            Dim l_narrative As String = l_strval3
            Dim l_description As String = l_strval4
            Dim l_debit As String = l_strval5
            Dim l_credit As String = l_strval6
            Dim l_bankcode As String = l_strval7
            Dim l_chequeno As String = l_strval8
            Dim l_TransRef As String = l_strval9
            Dim l_glchecks As String = l_strval10
            'Response.Write(l_glchecks)
            'Dim l_CustomerName As String = l_strval10
            'Dim l_Outletype As String = l_strval11
            'Dim l_TownshipCode As String = l_strval12
            'Dim l_Township As String = l_strval13
            'Dim l_TransType As String = l_strval14


            'Dim l_suppliercode As String = l_strval11

            'Dim l_seqno As String = l_strval12
            If Trim(l_transDate) <> Nothing Then

                dr = dt.NewRow()
                'If (l_accountno.Length <> 15 And l_accountno.Length <> 0) Then
                '    Dim l_Mypos As Integer = l_accountno.Length
                '    l_accountno = l_accountno.Insert(4, "/")
                '    l_accountno = l_accountno.Insert(l_Mypos - 3, "/")



                'End If
                dr(0) = Replace(Replace(l_PostDate, ",", ""), ",", "")
                dr(1) = l_transDate.Replace(",", "").Replace(",", "")
                dr(2) = Trim(l_accountno.Replace(",", ""))
                dr(3) = Trim(l_transcode.Replace(",", ""))
                dr(4) = Replace(Replace(l_narrative, ",", ""), ",", "")
                dr(5) = l_description.Replace(",", "").Replace(",", "")
                dr(6) = Trim(l_debit.Replace(",", ""))
                dr(7) = Trim(l_credit.Replace(",", ""))
                dr(8) = Trim(l_bankcode.Replace(",", ""))

                dr(9) = Trim(l_chequeno.Replace(",", ""))
                dr(10) = Trim(l_TransRef.Replace(",", ""))
                dr(11) = "Pass"

                dt.Rows.Add(dr)
            End If
        Loop Until line Is Nothing
        sr.Close()

        DataGrid1.DataSource = dt

        DataGrid1.DataBind()

        Dim str_error As String = ValidateEntriesHr()
        If str_error = Nothing Then

        Else
            'Me.cust_val.IsValid = False
            'Me.cust_val.ToolTip = "Record Already Exist !!" & str_error
            Response.Write("<script>alert(' The following entries are with errors !!" & str_error & "') </script>")
            'DataGrid2.Visible = True
        End If


    End Sub

    Function ValidateEntries() As String
        Dim ds As New DataSet, item As DataGridItem, str_err As String
        Dim Cmin As Decimal = 0, Cmax As Decimal = 0

        For Each item In DataGrid1.Items

            Dim l_PostDate As String = Trim(item.Cells(0).Text)
            Dim l_transDate As String = Trim(item.Cells(1).Text)
            Dim l_accountno As String = Trim(item.Cells(2).Text)
            Dim l_transcode As String = Trim(item.Cells(3).Text)
            Dim l_narrative As String = Trim(item.Cells(4).Text)
            Dim l_description As String = Trim(item.Cells(5).Text)
            Dim l_debit As String = Trim(item.Cells(6).Text)
            Dim l_credit As String = Trim(item.Cells(7).Text)
            Dim l_bankcode As String = Trim(item.Cells(8).Text)
            Dim l_chequeno As String = Trim(item.Cells(10).Text)
            Dim l_TransRef As String = Trim(item.Cells(10).Text)
            Dim l_status As String = Trim(item.Cells(11).Text)

            'Dim l_LoadingDate As String = l_strval
            'Dim l_MerketerCode As String = l_strval2
            'Dim l_meterticket As String = l_strval3
            'Dim l_Invoiceno As String = l_strval4
            'Dim l_ProductCode As String = l_strval5
            'Dim l_QuantityLoaded As String = l_strval6
            'Dim l_ReceivingDepot As String = l_strval7
            'Dim l_truckno As String = l_strval8
            'Dim l_Transporter As String = l_strval9

            'Dim drclass As OleDb.OleDbDataReader
            'Dim strClassCode As String = Me.txtCode.Text.Trim
            'drclass = CheckAssetClassCode(l_ClassCode.Trim)
            'Dim str_chk As String = "Sproc_GetDefDetCode '" & 8 & "','" & l_TownshipCode.Trim & "'"
            ' Dbconnect.FxSqlBoolean(str_chk, Dbconnect.Myconnect) = False Then
            'Dim l_validDeptCode As String = "Sproc_SectorValidDepts_sel '" & l_location & "','" & l_sectorCode & "'"
            'Dim str_sub As String = "Sproc_FixedAssetBrands_sel '" & l_ClassCode & "','" & l_subclass & "'"
            item.Cells(11).CssClass = 1
            If IsDate(CDate(Trim(l_PostDate))) = False Then
                str_err = str_err + item.Cells(0).Text + "<br>" + ","
                item.Cells(0).BackColor = Color.Red
                item.Cells(0).ToolTip = "invalid Posting Date"
                item.Cells(11).Text = "(Failed)- invalid Posting Date"
                item.Cells(11).CssClass = 0
            End If
            If IsDate(Trim(l_transDate)) = False Then
                str_err = str_err + item.Cells(1).Text + "<br>" + ","
                item.Cells(1).BackColor = Color.Red
                item.Cells(1).ToolTip = "invalid Transaction Date"
                item.Cells(11).Text = "(Failed)- invalid Transaction Date"
                item.Cells(11).CssClass = 0
            End If
            If Classes.GetBitField("Sproc_GetExtRefNumExist '" & Trim(l_TransRef) & "'") = True Then

                str_err = str_err + item.Cells(10).Text + "<br>" + ","
                item.Cells(10).BackColor = Color.Red
                item.Cells(10).ToolTip = "Transaction Sequence Code alreday Exist"
                item.Cells(11).Text = "(Failed)- Transaction Sequence Code alreday Exist"
                item.Cells(11).CssClass = 0
            End If
            If Classes.GetParamRule(47) = True Then

                Dim Str_checkminiAcct2 As String = "SELECT mini_account_no FROM ChartCodes WHERE mini_account_no ='" & l_bankcode & "'"
                'Response.Write(Str_checkminiAcct)
                If EazyClass.AccountExist(l_bankcode) = False Then
                    If Classes.GetField(Str_checkminiAcct2) <> l_bankcode Then
                        str_err = str_err + item.Cells(8).Text + "<br>" + ","
                        item.Cells(8).BackColor = Color.Red
                        item.Cells(8).ToolTip = "Invalid Account No"
                        item.Cells(11).Text = "(Failed)- Invalid Account No"
                        item.Cells(11).CssClass = 0
                    End If
                End If

            End If
            Dim Strchkmap As String = "Sproc_GetExtTransCodesExist '" & Trim(l_transcode) & "'"
            Dim result As String

            result = NETClass.Classes.GetField(Strchkmap)

            If Len(result) > 0 Then

            Else
                str_err = str_err + item.Cells(3).Text + "<br>" + ","
                item.Cells(3).BackColor = Color.Red
                item.Cells(3).ToolTip = "Explanation Code description is not yet mapped"
                item.Cells(11).Text = "(Failed)- Explanation Code description is not yet mapped"
                item.Cells(11).CssClass = 0
            End If

            If EazyClass.IsPostingAccount(l_accountno) = False Then
                ' CDate(l_PostDate) >NETClass.Classes.GetPostingDate Then
                str_err = str_err + item.Cells(2).Text + "<br>" + ","
                item.Cells(0).BackColor = Color.Red
                item.Cells(0).ToolTip = "Invalid Account for Posting"
                item.Cells(11).Text = "( Failed ) Invalid Account for Postings Pls check account status"

            End If
            If IsNumeric(Trim(l_credit)) = False Then
                str_err = str_err + item.Cells(7).Text + "<br>" + ","
                item.Cells(7).BackColor = Color.Red
                item.Cells(7).ToolTip = "(Failed)- Invalid credit amount"
                item.Cells(11).Text = "(Failed)- Invalid credit amount"
                item.Cells(11).CssClass = 0
            End If
            If IsNumeric(Trim(l_debit)) = False Then
                str_err = str_err + item.Cells(7).Text + "<br>" + ","
                item.Cells(6).BackColor = Color.Red
                item.Cells(6).ToolTip = "(Failed)- Invalid debit amount"
                item.Cells(11).Text = "(Failed)- Invalid debit amount"
                item.Cells(11).CssClass = 0
            End If
            If Trim(l_accountno) = "" Then
                str_err = str_err + item.Cells(2).Text + "<br>" + ","
                item.Cells(2).BackColor = Color.Red
                item.Cells(2).ToolTip = "Entry already posted before upload"
                item.Cells(11).Text = "(Failed)- Entry already posted before upload"
                item.Cells(11).CssClass = 0
            End If

            If CDate(l_PostDate) > NETClass.Classes.GetPostingDate Then
                str_err = str_err + item.Cells(2).Text + "<br>" + ","
                item.Cells(0).BackColor = Color.Red
                item.Cells(0).ToolTip = "Entry Postdate is greater than EazybancNET posting Date"
                item.Cells(11).Text = "( Passed )on Later date"

            End If
            Dim Str_checkminiAcct As String = "SELECT mini_account_no FROM ChartCodes WHERE mini_account_no ='" & l_accountno & "'"
            'Response.Write(Str_checkminiAcct)
            If EazyClass.AccountExist(l_accountno) = False Then
                If Classes.GetField(Str_checkminiAcct) <> l_accountno Then
                    str_err = str_err + item.Cells(2).Text + "<br>" + ","
                    item.Cells(2).BackColor = Color.Red
                    item.Cells(2).ToolTip = "Invalid Account No"
                    item.Cells(11).Text = "(Failed)- Invalid Account No"
                    item.Cells(11).CssClass = 0
                End If
            End If


            Cmin = l_credit
            If Cmax < Cmin Then
                Cmax = Cmin

            End If


        Next
        'DataGrid2.Visible = False

        Return str_err
        LoadApprovingOfficers2(Cmax)
    End Function

    Function ValidateEntriesHr() As String
        Dim ds As New DataSet, item As DataGridItem, str_err As String
        Dim Cmin As Decimal = 0, Cmax As Decimal = 0

        For Each item In DataGrid1.Items

            Dim l_PostDate As String = Trim(item.Cells(0).Text)
            Dim l_transDate As String = Trim(item.Cells(1).Text)
            Dim l_accountno As String = Trim(item.Cells(2).Text)
            Dim l_transcode As String = Trim(item.Cells(3).Text)
            Dim l_narrative As String = Trim(item.Cells(4).Text)
            Dim l_description As String = Trim(item.Cells(5).Text)
            Dim l_debit As String = Trim(item.Cells(6).Text)
            Dim l_credit As String = Trim(item.Cells(7).Text)
            Dim l_bankcode As String = Trim(item.Cells(8).Text)
            Dim l_chequeno As String = Trim(item.Cells(10).Text)
            Dim l_TransRef As String = Trim(item.Cells(10).Text)
            Dim l_status As String = Trim(item.Cells(11).Text)

            'Dim l_LoadingDate As String = l_strval
            'Dim l_MerketerCode As String = l_strval2
            'Dim l_meterticket As String = l_strval3
            'Dim l_Invoiceno As String = l_strval4
            'Dim l_ProductCode As String = l_strval5
            'Dim l_QuantityLoaded As String = l_strval6
            'Dim l_ReceivingDepot As String = l_strval7
            'Dim l_truckno As String = l_strval8
            'Dim l_Transporter As String = l_strval9

            'Dim drclass As OleDb.OleDbDataReader
            'Dim strClassCode As String = Me.txtCode.Text.Trim
            'drclass = CheckAssetClassCode(l_ClassCode.Trim)
            'Dim str_chk As String = "Sproc_GetDefDetCode '" & 8 & "','" & l_TownshipCode.Trim & "'"
            ' Dbconnect.FxSqlBoolean(str_chk, Dbconnect.Myconnect) = False Then
            'Dim l_validDeptCode As String = "Sproc_SectorValidDepts_sel '" & l_location & "','" & l_sectorCode & "'"
            'Dim str_sub As String = "Sproc_FixedAssetBrands_sel '" & l_ClassCode & "','" & l_subclass & "'"
            item.Cells(11).CssClass = 1
            If IsDate(CDate(Trim(l_PostDate))) = False Then
                str_err = str_err + item.Cells(0).Text + "<br>" + ","
                item.Cells(0).BackColor = Color.Red
                item.Cells(0).ToolTip = "invalid Posting Date"
                item.Cells(11).Text = "(Failed)- invalid Posting Date"
                item.Cells(11).CssClass = 0
            End If
            If IsDate(Trim(l_transDate)) = False Then
                str_err = str_err + item.Cells(1).Text + "<br>" + ","
                item.Cells(1).BackColor = Color.Red
                item.Cells(1).ToolTip = "invalid Transaction Date"
                item.Cells(11).Text = "(Failed)- invalid Transaction Date"
                item.Cells(11).CssClass = 0
            End If
            If Classes.GetBitField("Sproc_GetExtRefNumExist '" & l_TransRef & "'") = True Then

                str_err = str_err + item.Cells(10).Text + "<br>" + ","
                item.Cells(10).BackColor = Color.Red
                item.Cells(10).ToolTip = "Transaction Sequence Code alreday Exist"
                item.Cells(11).Text = "(Failed)- Transaction Sequence Code alreday Exist"
                item.Cells(11).CssClass = 0
            End If
            'If Classes.GetParamRule(47) = True Then

            '    Dim Str_checkminiAcct2 As String = "SELECT mini_account_no FROM ChartCodes WHERE mini_account_no ='" & l_bankcode & "'"
            '    'Response.Write(Str_checkminiAcct)
            '    If EazyClass.AccountExist(l_bankcode) = False Then
            '        If Classes.GetField(Str_checkminiAcct2) <> l_bankcode Then
            '            str_err = str_err + item.Cells(8).Text + "<br>" + ","
            '            item.Cells(8).BackColor = Color.Red
            '            item.Cells(8).ToolTip = "Invalid Account No"
            '            item.Cells(11).Text = "(Failed)- Invalid Account No"
            '            item.Cells(11).CssClass = 0
            '        End If
            '    End If

            'End If
            If Classes.GetBitField("Sproc_GetExtTransCodesExist '" & l_transcode & "'") = False Then

                str_err = str_err + item.Cells(3).Text + "<br>" + ","
                item.Cells(3).BackColor = Color.Red
                item.Cells(3).ToolTip = "Explanation Code description is not yet mapped"
                item.Cells(11).Text = "(Failed)- Explanation Code description is not yet mapped"
                item.Cells(11).CssClass = 0
            End If
            If IsNumeric(Trim(l_credit)) = False Then
                str_err = str_err + item.Cells(7).Text + "<br>" + ","
                item.Cells(7).BackColor = Color.Red
                item.Cells(7).ToolTip = "(Failed)- Invalid credit amount"
                item.Cells(11).Text = "(Failed)- Invalid credit amount"
                item.Cells(11).CssClass = 0
            End If
            If IsNumeric(Trim(l_debit)) = False Then
                str_err = str_err + item.Cells(7).Text + "<br>" + ","
                item.Cells(6).BackColor = Color.Red
                item.Cells(6).ToolTip = "(Failed)- Invalid debit amount"
                item.Cells(11).Text = "(Failed)- Invalid debit amount"
                item.Cells(11).CssClass = 0
            End If
            If Trim(l_accountno) = "" Then
                str_err = str_err + item.Cells(2).Text + "<br>" + ","
                item.Cells(2).BackColor = Color.Red
                item.Cells(2).ToolTip = "Entry already posted before upload"
                item.Cells(11).Text = "(Failed)- Entry already posted before upload"
                item.Cells(11).CssClass = 0
            End If

            If CDate(l_PostDate) > NETClass.Classes.GetPostingDate Then
                str_err = str_err + item.Cells(2).Text + "<br>" + ","
                item.Cells(0).BackColor = Color.Red
                item.Cells(0).ToolTip = "Entry Postdate is greater than EazybancNET posting Date"
                item.Cells(11).Text = "( Passed )on Later date"

            End If
            Dim Str_checkminiAcct As String = "SELECT mini_account_no FROM ChartCodes WHERE mini_account_no ='" & l_accountno & "'"
            'Response.Write(Str_checkminiAcct)
            If EazyClass.AccountExist(l_accountno) = False Then
                If Classes.GetField(Str_checkminiAcct) <> l_accountno Then
                    str_err = str_err + item.Cells(2).Text + "<br>" + ","
                    item.Cells(2).BackColor = Color.Red
                    item.Cells(2).ToolTip = "Invalid Account No"
                    item.Cells(11).Text = "(Failed)- Invalid Account No"
                    item.Cells(11).CssClass = 0
                End If
            End If


            Cmin = l_credit
            If Cmax < Cmin Then
                Cmax = Cmin

            End If


        Next
        'DataGrid2.Visible = False

        Return str_err
        LoadApprovingOfficers2(Cmax)
    End Function


    Function ValidateEntriesPromo() As String
        Dim ds As New DataSet, item As DataGridItem, str_err As String
        Dim Cmin As Decimal = 0, Cmax As Decimal = 0

        For Each item In DataGrid1.Items
            DataGrid1.Attributes("GenErrors") = "N"



            Dim l_accountno As String = Trim(item.Cells(0).Text)
            Dim l_PostDate As String = Trim(item.Cells(1).Text)
            Dim l_TransDate As String = Trim(item.Cells(2).Text)
            Dim l_Description As String = Trim(item.Cells(3).Text)
            Dim l_ContactAddress As String = Trim(item.Cells(4).Text)
            Dim l_Telephone As String = Trim(item.Cells(5).Text)
            Dim l_DepositSlipNo As String = Trim(item.Cells(6).Text)
            Dim l_Amount As String = Trim(item.Cells(7).Text)
            Dim l_PromoRemarks As String = Trim(item.Cells(8).Text)
            Dim l_Product As String = Trim(item.Cells(9).Text)
            Dim l_status As String = Trim(item.Cells(10).Text)
            'Dim l_TransRef As String = Trim(item.Cells(10).Text)
            'Dim l_status As String = Trim(item.Cells(11).Text)


            'Dim drclass As OleDb.OleDbDataReader
            'Dim strClassCode As String = Me.txtCode.Text.Trim
            'drclass = CheckAssetClassCode(l_ClassCode.Trim)
            'Dim str_chk As String = "Sproc_GetDefDetCode '" & 8 & "','" & l_TownshipCode.Trim & "'"
            ' Dbconnect.FxSqlBoolean(str_chk, Dbconnect.Myconnect) = False Then
            'Dim l_validDeptCode As String = "Sproc_SectorValidDepts_sel '" & l_location & "','" & l_sectorCode & "'"
            'Dim str_sub As String = "Sproc_FixedAssetBrands_sel '" & l_ClassCode & "','" & l_subclass & "'"
            item.Cells(10).CssClass = 1
            If IsDate(CDate(Trim(l_PostDate))) = False Then
                str_err = str_err + item.Cells(1).Text + "<br>" + ","
                item.Cells(1).BackColor = Color.Red
                item.Cells(1).ToolTip = "invalid Posting Date"
                item.Cells(10).Text = "(Failed)- invalid Posting Date"
                item.Cells(10).CssClass = 0
                DataGrid1.Attributes("GenErrors") = "Y"
            End If
            If IsDate(Trim(l_TransDate)) = False Then
                str_err = str_err + item.Cells(2).Text + "<br>" + ","
                item.Cells(2).BackColor = Color.Red
                item.Cells(2).ToolTip = "invalid Transaction Date"
                item.Cells(10).Text = "(Failed)- invalid Transaction Date"
                item.Cells(10).CssClass = 0
                DataGrid1.Attributes("GenErrors") = "Y"
            End If

            If (Trim(l_Description)) = "" Then
                str_err = str_err + item.Cells(3).Text + "<br>" + ","
                item.Cells(3).BackColor = Color.Red
                item.Cells(3).ToolTip = "invalid Account  Name or Reference Name "
                item.Cells(10).Text = "(Failed)- invalid Account  Name or Reference Name "
                item.Cells(10).CssClass = 0
                DataGrid1.Attributes("GenErrors") = "Y"
            End If

            If EazyClass.IsPostingAccount(l_accountno) = False Then
                ' CDate(l_PostDate) >NETClass.Classes.GetPostingDate Then
                str_err = str_err + item.Cells(0).Text + "<br>" + ","
                item.Cells(0).BackColor = Color.Red
                item.Cells(0).ToolTip = "Invalid Account for Posting"
                item.Cells(10).Text = "( Failed ) Invalid Account for Postings Pls check account status"
                DataGrid1.Attributes("GenErrors") = "Y"

            End If
            If IsNumeric(Trim(l_Amount)) = False Then
                str_err = str_err + item.Cells(7).Text + "<br>" + ","
                item.Cells(7).BackColor = Color.Red
                item.Cells(7).ToolTip = "(Failed)- Invalid amount for Upload"
                item.Cells(10).Text = "(Failed)- Invalid  amount for Upload"
                item.Cells(10).CssClass = 0
                DataGrid1.Attributes("GenErrors") = "Y"
            End If

            If Trim(l_accountno) = "" Then
                str_err = str_err + item.Cells(0).Text + "<br>" + ","
                item.Cells(0).BackColor = Color.Red
                item.Cells(0).ToolTip = "Entry already posted before upload"
                item.Cells(10).Text = "(Failed)- Entry already posted before upload"
                item.Cells(10).CssClass = 0
                DataGrid1.Attributes("GenErrors") = "Y"
            End If

            If CDate(l_PostDate) > NETClass.Classes.GetPostingDate Then
                str_err = str_err + item.Cells(1).Text + "<br>" + ","
                item.Cells(1).BackColor = Color.Red
                item.Cells(1).ToolTip = "Entry Postdate is greater than EazybancNET posting Date"
                item.Cells(10).Text = "( Passed )on Later date"

            End If
            If Trim(l_Telephone) = "" Then

                str_err = str_err + item.Cells(5).Text + "<br>" + ","
                item.Cells(5).BackColor = Color.Blue
                item.Cells(5).ToolTip = "Customer PhoneNo missing, he might not receive alerts"
                item.Cells(10).Text = "( Passed ) In valid ContactPhone"

            End If
            If Trim(l_DepositSlipNo) = "" Then

                str_err = str_err + item.Cells(6).Text + "<br>" + ","
                item.Cells(6).BackColor = Color.Red
                item.Cells(6).ToolTip = "Deposit SlipNo missing"
                item.Cells(10).Text = "( Failed ) Deposit SlipNo missing"
                item.Cells(10).CssClass = 0
                DataGrid1.Attributes("GenErrors") = "Y"
            End If
            Dim Str_checkminiAcct As String = "SELECT mini_account_no FROM ChartCodes WHERE mini_account_no ='" & l_accountno & "'"
            'Response.Write(Str_checkminiAcct)
            Dim strcheckProduct As String = "SELECT Prod_code From Products WHERE prod_code  ='" & l_Product & "'"



            If Classes.GetField(strcheckProduct) <> l_Product Then

                str_err = str_err + item.Cells(9).Text + "<br>" + ","
                item.Cells(9).BackColor = Color.Red
                item.Cells(9).ToolTip = "Invalid Product code"
                item.Cells(10).Text = "(failed)- Invalid Product code"
                item.Cells(10).CssClass = 0
                DataGrid1.Attributes("GenErrors") = "Y"
            End If
            If EazyClass.AccountExist(l_accountno) = False Then

                If EazyClass.AccountExist(l_PromoRemarks) = False Then
                    str_err = str_err + item.Cells(2).Text + "<br>" + ","
                    item.Cells(0).BackColor = Color.Blue
                    item.Cells(0).ToolTip = "New AccountNo to be Opened"
                    item.Cells(10).Text = "(Passed)- Invalid Account No"
                    ' item.Cells(11).CssClass = 0

                Else
                    l_accountno = l_PromoRemarks
                End If

            End If


            '  Cmin = l_credit
            If Cmax < Cmin Then
                Cmax = Cmin

            End If


        Next
        'DataGrid2.Visible = False

        Return str_err
        LoadApprovingOfficers2(Cmax)
    End Function


    Function ValidateEntriesPOS() As String
        Dim ds As New DataSet, item As DataGridItem, str_err As String
        Dim Cmin As Decimal = 0, Cmax As Decimal = 0

        For Each item In DataGrid1.Items

            Dim l_transdatetime As String = Trim(item.Cells(0).Text)
            Dim l_pan As String = Trim(item.Cells(1).Text)
            Dim l_acctno As String = Trim(item.Cells(2).Text)
            Dim l_reqamount As String = Trim(item.Cells(3).Text)
            Dim l_finalamount As String = Trim(item.Cells(4).Text)
            Dim l_feeamount As String = Trim(item.Cells(5).Text)
            Dim l_narr As String = Trim(item.Cells(6).Text)
            Dim l_refno As String = Trim(item.Cells(7).Text)
            Dim l_msgstatus As String = Trim(item.Cells(8).Text)

            'Dim l_LoadingDate As String = l_strval
            'Dim l_MerketerCode As String = l_strval2
            'Dim l_meterticket As String = l_strval3
            'Dim l_Invoiceno As String = l_strval4
            'Dim l_ProductCode As String = l_strval5
            'Dim l_QuantityLoaded As String = l_strval6
            'Dim l_ReceivingDepot As String = l_strval7
            'Dim l_truckno As String = l_strval8
            'Dim l_Transporter As String = l_strval9

            'Dim drclass As OleDb.OleDbDataReader
            'Dim strClassCode As String = Me.txtCode.Text.Trim
            'drclass = CheckAssetClassCode(l_ClassCode.Trim)
            'Dim str_chk As String = "Sproc_GetDefDetCode '" & 8 & "','" & l_TownshipCode.Trim & "'"
            ' Dbconnect.FxSqlBoolean(str_chk, Dbconnect.Myconnect) = False Then
            'Dim l_validDeptCode As String = "Sproc_SectorValidDepts_sel '" & l_location & "','" & l_sectorCode & "'"
            'Dim str_sub As String = "Sproc_FixedAssetBrands_sel '" & l_ClassCode & "','" & l_subclass & "'"
            item.Cells(9).CssClass = 1


            'If Classes.GetParamRule(47) = True Then

            '    Dim Str_checkminiAcct2 As String = "SELECT mini_account_no FROM ChartCodes WHERE mini_account_no ='" & l_bankcode & "'"
            '    'Response.Write(Str_checkminiAcct)
            '    If EazyClass.AccountExist(l_bankcode) = False Then
            '        If Classes.GetField(Str_checkminiAcct2) <> l_bankcode Then
            '            str_err = str_err + item.Cells(8).Text + "<br>" + ","
            '            item.Cells(8).BackColor = Color.Red
            '            item.Cells(8).ToolTip = "Invalid Account No"
            '            item.Cells(11).Text = "(Failed)- Invalid Account No"
            '            item.Cells(11).CssClass = 0
            '        End If
            '    End If

            'End If

            If IsNumeric(Trim(l_finalamount)) = False Then
                str_err = str_err + item.Cells(7).Text + "<br>" + ","
                item.Cells(4).BackColor = Color.Red
                item.Cells(4).ToolTip = "(Failed)- Invalid amount for Posting"
                item.Cells(9).Text = "(Failed)- Invalid amount for Posting"
                item.Cells(9).CssClass = 0
            End If

            If Trim(l_acctno) = "" Then
                str_err = str_err + item.Cells(2).Text + "<br>" + ","
                item.Cells(2).BackColor = Color.Red
                item.Cells(2).ToolTip = "Invalid Account No Mapped to the Terminal"
                item.Cells(9).Text = "(Failed)- Invalid Account No Mapped to the Terminal"
                item.Cells(9).CssClass = 0
            End If


            Dim Str_checkminiAcct As String = "SELECT mini_account_no FROM ChartCodes WHERE mini_account_no ='" & l_acctno & "'"
            'Response.Write(Str_checkminiAcct)
            If EazyClass.AccountExist(l_acctno) = False Then
                If Classes.GetField(Str_checkminiAcct) <> l_acctno Then
                    str_err = str_err + item.Cells(2).Text + "<br>" + ","
                    item.Cells(2).BackColor = Color.Red
                    item.Cells(2).ToolTip = "Invalid Account No Mapped to the Terminal"
                    item.Cells(9).Text = "(Failed)- Invalid Account No Mapped to the Terminal"
                    item.Cells(9).CssClass = 0
                End If
            End If





        Next
        'DataGrid2.Visible = False

        Return str_err
        LoadApprovingOfficers2(Cmax)
    End Function





    Function UploadMultipleFiles_Clicked() As String  'ByVal Sender As Object, ByVal e As EventArgs)

        'Variable to hold the result
        Dim m_strResultMessage As String
        'Variable to hold the FileName
        Dim m_strFileName As String
        'Variable FolderName where the files  will be saved
        Dim m_strFolderName As String = "C:\TempMultipleFiles\"
        'Dim m_strFolderName As String = "\\pokkie\Returns\Budget Office\"
        'Variable to hold the File
        Dim m_objFile As HttpPostedFile




        'Variable used in the Loop
        Dim i As Integer

        Try
            'Loop Through the Files
            For i = 0 To Request.Files.Count - 1

                'Get the HttpPostedFile
                m_objFile = Request.Files(i)

                'Check that the File exists has a name and is not empty
                If Not (m_objFile Is Nothing Or m_objFile.FileName = "" Or m_objFile.ContentLength < 1) Then

                    'Get the name of the file
                    m_strFileName = m_objFile.FileName


                    m_strFileName = Path.GetFileName(m_strFileName)

                    'Creates the folder if it does not exists
                    If (Not Directory.Exists(m_strFolderName)) Then
                        Directory.CreateDirectory(m_strFolderName)
                    End If

                    'Save each uploaded file
                    m_objFile.SaveAs(m_strFolderName & m_strFileName)

                    'Assign the File Name and File Type to Result
                    m_strResultMessage = m_strResultMessage & "Uploaded File: " & m_objFile.FileName & " of type " & m_objFile.ContentType & " <br> "

                    'Hide the Multiple Form Upload Panel
                    'MultipleFileUploadForm.Visible = False
                End If
            Next

            'If no files where selected provide a user friendly message
            If m_strResultMessage = "" Then
                m_strResultMessage = "Select the file to upload or import."
                m_strFileName = Nothing
            End If
            Response.Write("<script>alert('" & m_strResultMessage & "') </script>")

            Return m_strFileName
        Catch errorVariable As Exception
            'Trap the exception
            m_strResultMessage = errorVariable.ToString()
        End Try

        'Unhide the Result Label 
        'ResultMsg.Visible = True
        'Assign the Result to ResultMsg Label Text
        'ResultMsg.Text = m_strResultMessage

    End Function

    Protected Sub Button3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button3.Click
        '  Dim str_error As String = ValidateEntries()
        luser = Context.User.Identity.Name
        StrLocation = luser.Substring(luser.LastIndexOf("?") + 1)
        struser = Mid(luser, 1, luser.LastIndexOf("?"))
        guser = struser
        If Trim(struser) <> "" Then


            'If str_error = Nothing Then
            If drptranstype.SelectedValue = 2 Then
                Console.Write("I am here")
                If Me.txtref_no.Text = Nothing Then

                    txtref_no.Text = EazyClass.GetExtJournalNum(StrLocation)
                    Classes.alertwindow(Me.Page, "A New Batch Reference Generated : - " & Me.txtref_no.Text, "Halt")
                    Classes.RecordOp("Sproc_AddJournal '" & txtref_no.Text & "','" & "Promo Upload Transaction" & "'," & CDec(0) & ",'" & "N" & "','" & struser & "','" & StrLocation & "',0")

                    Dim item As DataGridItem
                    For Each item In DataGrid1.Items
                        Dim l_PostDate As String = Trim(item.Cells(0).Text)
                        Dim l_transDate As String = Trim(item.Cells(1).Text)
                        Dim l_accountno As String = Trim(item.Cells(2).Text)
                        Dim l_transcode As String = Trim(item.Cells(3).Text)
                        Dim l_narrative As String = Trim(item.Cells(4).Text)
                        Dim l_description As String = Trim(item.Cells(5).Text)
                        Dim l_debit As String = Trim(item.Cells(6).Text)
                        Dim l_credit As String = Trim(item.Cells(7).Text)
                        Dim l_bankcode As String = Trim(item.Cells(8).Text)
                        Dim l_chequeno As String = Trim(item.Cells(10).Text)
                        Dim l_TransRef As String = Trim(item.Cells(11).Text)
                        Dim l_status As String = 1 'Trim(item.Cells(1).CssClass)

                        If l_bankcode = "" Then
                            l_bankcode = l_chequeno
                        End If
                        'Dim strAcccTest As String = "SELECT account_no FROM Accounts WHERE old_account_no = '" & l_account & "'"
                        If EazyClass.AccountExist(l_accountno) = False Then


                            l_accountno = EazyClass.FullAccount(StrLocation, l_accountno)


                        End If


                        'Dim str_ins As String = "Sproc_AddExtTransHistJor '" & Format(CDate(l_PostDate), "MM-dd-yyyy") & "','" & Format(CDate(l_transDate), "MM-dd-yyyy") & "','" & l_accountno & "'," & _
                        '"'" & l_transcode & "','" & l_narrative & "','" & l_description & "'," & l_debit & " ," & l_credit & "," & _
                        '"'" & l_bankcode & "','" & l_chequeno & "','" & l_TransRef & "','" & txtref_no.Text & "','" & struser & "'," & CInt(l_status) & ",'" & Me.offr2.SelectedValue & "','" & StrLocation & "'"
                        'Classes.RecordOp(str_ins)
                        ''Response.Write(str_ins)


                        Dim str_ins As String = "Sproc_AddExtTransHistJornal '" & Format(CDate(l_PostDate), "MM-dd-yyyy") & "','" & Format(CDate(l_transDate), "MM-dd-yyyy") & "','" & l_accountno & "'," & _
                        "'" & l_transcode & "','" & l_narrative & "','" & l_description & "'," & l_debit & " ," & l_credit & "," & _
                        "'" & l_bankcode & "','" & l_chequeno & "','" & l_TransRef & "','" & txtref_no.Text & "','" & struser & "'," & CInt(l_status) & ",'" & Me.offr2.SelectedValue & "','" & StrLocation & "'"
                        Classes.RecordOp(str_ins)



                        '  Response.Write(str_ins)
                    Next


                End If
            ElseIf drptranstype.SelectedValue = 3 Then

                If Me.txtref_no.Text = Nothing Then

                    txtref_no.Text = EazyClass.GetExtJournalNum(StrLocation)
                    Classes.alertwindow(Me.Page, "A New Batch Reference Generated : - " & Me.txtref_no.Text, "Halt")
                    If DataGrid1.Attributes("GenErrors") <> "Y" Then


                        Dim item As DataGridItem
                        For Each item In DataGrid1.Items
                            Dim l_Accountno As String = Trim(item.Cells(0).Text)
                            Dim l_PostDate As String = Trim(item.Cells(1).Text)
                            Dim l_TransDate As String = Trim(item.Cells(2).Text)
                            Dim l_Description As String = Trim(item.Cells(3).Text)
                            Dim l_ContactAddress As String = Trim(item.Cells(4).Text)
                            Dim l_Telephone As String = Trim(item.Cells(5).Text)
                            Dim l_DepositSlipNo As String = Trim(item.Cells(6).Text)
                            Dim l_Amount As String = Trim(item.Cells(7).Text)
                            Dim l_PromoRemarks As String = Trim(item.Cells(8).Text)
                            Dim l_product As String = Trim(item.Cells(9).Text)
                            Dim l_status As String = Trim(item.Cells(10).Text)

                            Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("EazyConnect").ConnectionString
                            Dim Cmd As New SqlCommand, sqlConn As New SqlConnection(strcon), result As SqlParameter
                            '@ExtRefNo Varchar(50),
                            '@ExtBatchNo Varchar(50),
                            '@PostDate datetime,
                            '@TransDate datetime,
                            '@TransCode varchar(10),
                            '@RefName Varchar(50),
                            '@RefDepositNo varchar(50),
                            '@Amount decimal(28,2),
                            '@RefAddress nvarchar(500),
                            '@RefRemarks varchar(500),
                            '@RefAddedBy varchar(50),
                            '@RefApprovedBy varchar(50),
                            '@Validatecheck bit,
                            '@Telephone varchar(20)
                            Cmd.CommandText = "Sproc_AddExtTransPromo"
                            Cmd.Connection = sqlConn
                            Cmd.CommandType = CommandType.StoredProcedure


                            Cmd.Parameters.Add(New SqlParameter("@ExtRefNo", l_Accountno))
                            Cmd.Parameters.Add(New SqlParameter("@ExtBatchNo", txtref_no.Text))
                            Cmd.Parameters.Add(New SqlParameter("@PostDate", Format(CDate(l_PostDate), "MM-dd-yyyy")))
                            Cmd.Parameters.Add(New SqlParameter("@TransDate", Format(CDate(l_TransDate), "MM-dd-yyyy")))
                            Cmd.Parameters.Add(New SqlParameter("@TransCode", "Promo"))
                            Cmd.Parameters.Add(New SqlParameter("@RefName", l_Description))
                            Cmd.Parameters.Add(New SqlParameter("@RefDepositNo", l_DepositSlipNo))
                            Cmd.Parameters.Add(New SqlParameter("@Amount", l_Amount))
                            Cmd.Parameters.Add(New SqlParameter("@RefAddress", l_ContactAddress))

                            Cmd.Parameters.Add(New SqlParameter("@RefRemarks", l_PromoRemarks))
                            Cmd.Parameters.Add(New SqlParameter("@RefAddedBy", guser))
                            Cmd.Parameters.Add(New SqlParameter("@RefApprovedBy", offr2.SelectedValue))

                            Cmd.Parameters.Add(New SqlParameter("@Validatecheck", item.Cells(10).CssClass))
                            Cmd.Parameters.Add(New SqlParameter("@Telephone", l_Telephone))
                            Cmd.Parameters.Add(New SqlParameter("@ProdCode", l_product))


                            Cmd.ExecuteNonQuery()


                            sqlConn.Close()
                        Next
                    Else

                        Response.Write("<script>alert('Upload Batch entries has some errors!!!, Pls correct Batch') </script>")
                        Exit Sub

                    End If

                End If
            ElseIf drptranstype.SelectedValue = 8 Then 'Reconciliation Upload

                If Me.txtref_no.Text = Nothing Then

                    txtref_no.Text = EazyClass.GetExtJournalNum(StrLocation)
                    Classes.alertwindow(Me.Page, "A New Batch Reference Generated : - " & Me.txtref_no.Text, "Halt")
                    If DataGrid1.Attributes("GenErrors") <> "Y" Then


                        Dim item As DataGridItem
                        For Each item In DataGrid1.Items
                            Dim l_transdatetime As String = Trim(item.Cells(0).Text)
                            Dim l_pan As String = Trim(item.Cells(1).Text)
                            Dim l_acctno As String = Trim(item.Cells(2).Text)
                            Dim l_reqamount As String = Trim(item.Cells(3).Text)
                            Dim l_finalamount As String = Trim(item.Cells(4).Text)
                            Dim l_feeamount As String = Trim(item.Cells(5).Text)
                            Dim l_narr As String = Trim(item.Cells(6).Text)
                            Dim l_refno As String = Trim(item.Cells(7).Text)
                            Dim l_msgstatus As String = Trim(item.Cells(8).Text)

                            Dim l_contrans As String = String.Format("{0}/{1}/{2} {3}", l_transdatetime.Substring(6, 4), l_transdatetime.Substring(3, 2), l_transdatetime.Substring(0, 2), l_transdatetime.Substring(11))
                            Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("EazyConnect").ConnectionString
                            Dim Cmd As New SqlCommand, sqlConn As New SqlConnection(strcon), result As SqlParameter

                            '@extRefNo varchar(50), @poststartdate datetime,
                            '@postenddate datetime, @pan varchar(50), 
                            '@terminalID varchar(50),@cardAccpID varchar(50),
                            '@cardAccpName varchar(50), @transSystemDateTime datetime,
                            '@requestAmt decimal(18,2),@authorisedAmt decimal(18,2),
                            '@surcharge decimal(18,2),@narrative varchar(250),
                            '@BatchRefNo varchar(50),@reconStatus bit,
                            '@entered_by varchar(50) 

                            Cmd.CommandText = "Sproc_AddExtTransInfo"
                            Cmd.Connection = sqlConn
                            Cmd.CommandType = CommandType.StoredProcedure
                            sqlConn.Open()

                            Cmd.Parameters.Add(New SqlParameter("@poststartdate", CDate(l_contrans)))
                            Cmd.Parameters.Add(New SqlParameter("@postenddate", CDate(l_contrans)))
                            'Cmd.Parameters.Add(New SqlParameter("@transSystemDateTime", Format((l_time), "MM-dd-yyyy")))
                            Cmd.Parameters.Add(New SqlParameter("@transSystemDateTime", CDate(l_contrans)))

                            Cmd.Parameters.Add(New SqlParameter("@extRefNo", l_refno))
                            Cmd.Parameters.Add(New SqlParameter("@pan", l_pan))
                            Cmd.Parameters.Add(New SqlParameter("@cardAccpID", l_acctno))
                            Cmd.Parameters.Add(New SqlParameter("@cardAccpName", l_acctno))
                            Cmd.Parameters.Add(New SqlParameter("@terminalID", l_acctno))
                            Cmd.Parameters.Add(New SqlParameter("@requestAmt", l_reqamount))
                            Cmd.Parameters.Add(New SqlParameter("@authorisedAmt", l_finalamount))
                            Cmd.Parameters.Add(New SqlParameter("@surcharge", l_feeamount))
                            'Cmd.Parameters.Add(New SqlParameter("@ExtBatchNo", txtref_no.Text))
                            Cmd.Parameters.Add(New SqlParameter("@BatchRefNo", txtref_no.Text))
                            Cmd.Parameters.Add(New SqlParameter("@narrative", l_narr))
                            Cmd.Parameters.Add(New SqlParameter("@reconStatus", 0))
                            Cmd.Parameters.Add(New SqlParameter("@entered_by", guser))



                            Cmd.ExecuteNonQuery()


                            sqlConn.Close()
                        Next
                    Else

                        Response.Write("<script>alert('Upload Batch entries has some errors!!!, Pls correct Batch') </script>")
                        Exit Sub

                    End If

                End If

            ElseIf drptranstype.SelectedValue = 10 Then 'Reconciliation Recharge Upload

                If Me.txtref_no.Text = Nothing Then

                    txtref_no.Text = EazyClass.GetExtJournalNum(StrLocation)
                    Classes.alertwindow(Me.Page, "A New Batch Reference Generated : - " & Me.txtref_no.Text, "Halt")
                    If DataGrid1.Attributes("GenErrors") <> "Y" Then


                        Dim item As DataGridItem
                        For Each item In DataGrid1.Items
                            Dim l_transdatetime As String = Trim(item.Cells(0).Text)
                            Dim l_pan As String = Trim(item.Cells(1).Text)
                            Dim l_acctno As String = Trim(item.Cells(2).Text)
                            Dim l_reqamount As String = Trim(item.Cells(3).Text)
                            Dim l_finalamount As String = Trim(item.Cells(4).Text)
                            Dim l_feeamount As String = Trim(item.Cells(5).Text)
                            Dim l_narr As String = Trim(item.Cells(6).Text)
                            Dim l_refno As String = Trim(item.Cells(7).Text)
                            Dim l_msgstatus As String = Trim(item.Cells(8).Text)

                            Dim l_contrans As String = String.Format("{0}/{1}/{2} {3}", l_transdatetime.Substring(6, 4), l_transdatetime.Substring(3, 2), l_transdatetime.Substring(0, 2), l_transdatetime.Substring(11))
                            Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("EazyConnect").ConnectionString
                            Dim Cmd As New SqlCommand, sqlConn As New SqlConnection(strcon), result As SqlParameter

                            '@extRefNo varchar(50), @poststartdate datetime,
                            '@postenddate datetime, @pan varchar(50), 
                            '@terminalID varchar(50),@cardAccpID varchar(50),
                            '@cardAccpName varchar(50), @transSystemDateTime datetime,
                            '@requestAmt decimal(18,2),@authorisedAmt decimal(18,2),
                            '@surcharge decimal(18,2),@narrative varchar(250),
                            '@BatchRefNo varchar(50),@reconStatus bit,
                            '@entered_by varchar(50) 

                            Cmd.CommandText = "Sproc_AddExtTransInfo"
                            Cmd.Connection = sqlConn
                            Cmd.CommandType = CommandType.StoredProcedure
                            sqlConn.Open()

                            Cmd.Parameters.Add(New SqlParameter("@poststartdate", CDate(l_contrans)))
                            Cmd.Parameters.Add(New SqlParameter("@postenddate", CDate(l_contrans)))
                            'Cmd.Parameters.Add(New SqlParameter("@transSystemDateTime", Format((l_time), "MM-dd-yyyy")))
                            Cmd.Parameters.Add(New SqlParameter("@transSystemDateTime", CDate(l_contrans)))

                            Cmd.Parameters.Add(New SqlParameter("@extRefNo", l_refno))
                            Cmd.Parameters.Add(New SqlParameter("@pan", l_pan))
                            Cmd.Parameters.Add(New SqlParameter("@cardAccpID", l_acctno))
                            Cmd.Parameters.Add(New SqlParameter("@cardAccpName", l_acctno))
                            Cmd.Parameters.Add(New SqlParameter("@terminalID", l_acctno))
                            Cmd.Parameters.Add(New SqlParameter("@requestAmt", l_reqamount))
                            Cmd.Parameters.Add(New SqlParameter("@authorisedAmt", l_finalamount))
                            Cmd.Parameters.Add(New SqlParameter("@surcharge", l_feeamount))
                            'Cmd.Parameters.Add(New SqlParameter("@ExtBatchNo", txtref_no.Text))
                            Cmd.Parameters.Add(New SqlParameter("@BatchRefNo", txtref_no.Text))
                            Cmd.Parameters.Add(New SqlParameter("@narrative", l_narr))
                            Cmd.Parameters.Add(New SqlParameter("@reconStatus", 0))
                            Cmd.Parameters.Add(New SqlParameter("@entered_by", guser))



                            Cmd.ExecuteNonQuery()


                            sqlConn.Close()
                        Next
                    Else

                        Response.Write("<script>alert('Upload Batch entries has some errors!!!, Pls correct Batch') </script>")
                        Exit Sub

                    End If

                End If


            ElseIf drptranstype.SelectedValue = 11 Then 'Reconciliation POS Upload

                If Me.txtref_no.Text = Nothing Then

                    txtref_no.Text = EazyClass.GetExtJournalNum(StrLocation)
                    Classes.alertwindow(Me.Page, "A New Batch Reference Generated : - " & Me.txtref_no.Text, "Halt")
                    If DataGrid1.Attributes("GenErrors") <> "Y" Then


                        Dim item As DataGridItem
                        For Each item In DataGrid1.Items
                            Dim l_transdatetime As String = Trim(item.Cells(0).Text)
                            Dim l_pan As String = Trim(item.Cells(1).Text)
                            Dim l_acctno As String = Trim(item.Cells(2).Text)
                            Dim l_reqamount As String = Trim(item.Cells(3).Text)
                            Dim l_finalamount As String = Trim(item.Cells(4).Text)
                            Dim l_feeamount As String = Trim(item.Cells(5).Text)
                            Dim l_narr As String = Trim(item.Cells(6).Text)
                            Dim l_refno As String = Trim(item.Cells(7).Text)
                            Dim l_msgstatus As String = Trim(item.Cells(8).Text)

                            Dim l_contrans As String = String.Format("{0}/{1}/{2} {3}", l_transdatetime.Substring(6, 4), l_transdatetime.Substring(3, 2), l_transdatetime.Substring(0, 2), l_transdatetime.Substring(11))
                            Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("EazyConnect").ConnectionString
                            Dim Cmd As New SqlCommand, sqlConn As New SqlConnection(strcon), result As SqlParameter

                            '@extRefNo varchar(50), @poststartdate datetime,
                            '@postenddate datetime, @pan varchar(50), 
                            '@terminalID varchar(50),@cardAccpID varchar(50),
                            '@cardAccpName varchar(50), @transSystemDateTime datetime,
                            '@requestAmt decimal(18,2),@authorisedAmt decimal(18,2),
                            '@surcharge decimal(18,2),@narrative varchar(250),
                            '@BatchRefNo varchar(50),@reconStatus bit,
                            '@entered_by varchar(50) 

                            Cmd.CommandText = "Sproc_AddExtTransInfo"
                            Cmd.Connection = sqlConn
                            Cmd.CommandType = CommandType.StoredProcedure
                            sqlConn.Open()

                            Cmd.Parameters.Add(New SqlParameter("@poststartdate", CDate(l_transdatetime)))
                            Cmd.Parameters.Add(New SqlParameter("@postenddate", CDate(l_transdatetime)))
                            'Cmd.Parameters.Add(New SqlParameter("@transSystemDateTime", Format((l_time), "MM-dd-yyyy")))
                            Cmd.Parameters.Add(New SqlParameter("@transSystemDateTime", CDate(l_transdatetime)))

                            Cmd.Parameters.Add(New SqlParameter("@extRefNo", l_refno))
                            Cmd.Parameters.Add(New SqlParameter("@pan", l_pan))
                            Cmd.Parameters.Add(New SqlParameter("@cardAccpID", l_acctno))
                            Cmd.Parameters.Add(New SqlParameter("@cardAccpName", l_acctno))
                            Cmd.Parameters.Add(New SqlParameter("@terminalID", l_acctno))
                            Cmd.Parameters.Add(New SqlParameter("@requestAmt", l_reqamount))
                            Cmd.Parameters.Add(New SqlParameter("@authorisedAmt", l_finalamount))
                            Cmd.Parameters.Add(New SqlParameter("@surcharge", l_feeamount))
                            'Cmd.Parameters.Add(New SqlParameter("@ExtBatchNo", txtref_no.Text))
                            Cmd.Parameters.Add(New SqlParameter("@BatchRefNo", txtref_no.Text))
                            Cmd.Parameters.Add(New SqlParameter("@narrative", l_narr))
                            Cmd.Parameters.Add(New SqlParameter("@reconStatus", 0))
                            Cmd.Parameters.Add(New SqlParameter("@entered_by", guser))



                            Cmd.ExecuteNonQuery()


                            sqlConn.Close()
                        Next
                    Else

                        Response.Write("<script>alert('Upload Batch entries has some errors!!!, Pls correct Batch') </script>")
                        Exit Sub

                    End If

                End If
                'Merchant Pos
            ElseIf drptranstype.SelectedValue = 12 Then 'Reconciliation  Merchart POS Upload

                If Me.txtref_no.Text = Nothing Then

                    txtref_no.Text = EazyClass.GetExtJournalNum(StrLocation)
                    Classes.alertwindow(Me.Page, "A New Batch Reference Generated : - " & Me.txtref_no.Text, "Halt")
                    Classes.RecordOp("Sproc_AddJournal '" & txtref_no.Text & "','" & "Merchant POS Upload" & "'," & CDec(0) & ",'" & "N" & "','" & struser & "','" & StrLocation & "',0")

                    If DataGrid1.Attributes("GenErrors") <> "Y" Then


                        Dim item As DataGridItem
                        For Each item In DataGrid1.Items
                            Dim l_transdatetime As String = Trim(item.Cells(0).Text)
                            Dim l_pan As String = Trim(item.Cells(1).Text)
                            Dim l_acctno As String = Trim(item.Cells(2).Text)
                            Dim l_reqamount As String = Trim(item.Cells(3).Text)
                            Dim l_finalamount As String = Trim(item.Cells(4).Text)
                            Dim l_feeamount As String = Trim(item.Cells(5).Text)
                            Dim l_narr As String = Trim(item.Cells(6).Text)
                            Dim l_refno As String = Trim(item.Cells(7).Text)
                            Dim l_msgstatus As String = Trim(item.Cells(8).Text)

                            Dim l_contrans As String = String.Format("{0}/{1}/{2} {3}", l_transdatetime.Substring(6, 4), l_transdatetime.Substring(3, 2), l_transdatetime.Substring(0, 2), l_transdatetime.Substring(11))
                            Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("EazyConnect").ConnectionString
                            Dim Cmd As New SqlCommand, sqlConn As New SqlConnection(strcon), result As SqlParameter

                            '@extRefNo varchar(50), @poststartdate datetime,
                            '@postenddate datetime, @pan varchar(50), 
                            '@terminalID varchar(50),@cardAccpID varchar(50),
                            '@cardAccpName varchar(50), @transSystemDateTime datetime,
                            '@requestAmt decimal(18,2),@authorisedAmt decimal(18,2),
                            '@surcharge decimal(18,2),@narrative varchar(250),
                            '@BatchRefNo varchar(50),@reconStatus bit,
                            '@entered_by varchar(50) 

                            Cmd.CommandText = "Sproc_AddExtTransInfo"
                            Cmd.Connection = sqlConn
                            Cmd.CommandType = CommandType.StoredProcedure
                            sqlConn.Open()

                            'Cmd.Parameters.Add(New SqlParameter("@poststartdate", CDate(l_transdatetime)))
                            'Cmd.Parameters.Add(New SqlParameter("@postenddate", CDate(l_transdatetime)))
                            ''Cmd.Parameters.Add(New SqlParameter("@transSystemDateTime", Format((l_time), "MM-dd-yyyy")))
                            'Cmd.Parameters.Add(New SqlParameter("@transSystemDateTime", CDate(l_transdatetime)))

                            'Cmd.Parameters.Add(New SqlParameter("@extRefNo", l_refno))
                            'Cmd.Parameters.Add(New SqlParameter("@pan", l_pan))
                            'Cmd.Parameters.Add(New SqlParameter("@cardAccpID", l_acctno))
                            'Cmd.Parameters.Add(New SqlParameter("@cardAccpName", l_acctno))
                            'Cmd.Parameters.Add(New SqlParameter("@terminalID", l_acctno))
                            'Cmd.Parameters.Add(New SqlParameter("@requestAmt", l_reqamount))
                            'Cmd.Parameters.Add(New SqlParameter("@authorisedAmt", l_finalamount))
                            'Cmd.Parameters.Add(New SqlParameter("@surcharge", l_feeamount))
                            ''Cmd.Parameters.Add(New SqlParameter("@ExtBatchNo", txtref_no.Text))
                            'Cmd.Parameters.Add(New SqlParameter("@BatchRefNo", txtref_no.Text))
                            'Cmd.Parameters.Add(New SqlParameter("@narrative", l_narr))
                            'Cmd.Parameters.Add(New SqlParameter("@reconStatus", 0))
                            'Cmd.Parameters.Add(New SqlParameter("@entered_by", guser))



                            'Cmd.ExecuteNonQuery()


                            sqlConn.Close()

                            Dim l_postdate As DateTime = EazyClass.GetPostingDate
                            Dim l_transdate As DateTime = l_postdate
                            Dim l_transcode As String = "020"
                            Dim l_narrative As String = String.Format("Bng/Stlmnt/{0}/{1}", l_pan, l_transdatetime)
                            Dim l_credit As Decimal = l_finalamount
                            Dim l_debit As Decimal = 0
                            Dim l_bankcode As String = "Fidelity Bank"
                            Dim l_chequeno As String = ""
                            Dim str_ins As String = String.Format("Sproc_AddExtTransHistJornalPos '{0}','{1}','{2}','{3}','{4}','{4}',{5} ,{6},'{7}','{8}','{9}','{10}','{11}',{12},'{13}','{14}'", Format(CDate(l_postdate), "MM-dd-yyyy"), Format(CDate(l_transdate), "MM-dd-yyyy"), l_acctno, l_transcode, l_narrative, l_debit, l_credit, l_bankcode, l_chequeno, l_refno, txtref_no.Text, struser, CInt(1), Me.offr2.SelectedValue, StrLocation)
                            Classes.RecordOp(str_ins)

                        Next
                    Else

                        Response.Write("<script>alert('Upload Batch entries has some errors!!!, Pls correct Batch') </script>")
                        Exit Sub

                    End If

                End If

            ElseIf drptranstype.SelectedValue = 9 Then 'Reconciliation Upload

                If Me.txtref_no.Text = Nothing Then

                    txtref_no.Text = EazyClass.GetExtJournalNum(StrLocation)
                    Classes.alertwindow(Me.Page, "A New Batch Reference Generated : - " & Me.txtref_no.Text, "Halt")
                    If DataGrid1.Attributes("GenErrors") <> "Y" Then


                        Dim item As DataGridItem
                        For Each item In DataGrid1.Items
                            Dim l_reportstartdate As String = Trim(item.Cells(0).Text)
                            Dim l_reportenddate As String = Trim(item.Cells(1).Text)
                            Dim l_pan As String = Trim(item.Cells(2).Text)
                            Dim l_terminalID As String = Trim(item.Cells(3).Text)
                            Dim l_CardAcceptID As String = Trim(item.Cells(4).Text)
                            Dim l_CardAcceptName As String = Trim(item.Cells(5).Text)
                            Dim l_time As String = Trim(item.Cells(6).Text)
                            Dim l_reqamount As String = Trim(item.Cells(7).Text)
                            Dim l_finalamount As String = Trim(item.Cells(8).Text)
                            Dim l_feeamount As String = Trim(item.Cells(9).Text)
                            Dim l_msgtranstype As String = Trim(item.Cells(10).Text)
                            Dim l_refno As String = Trim(item.Cells(11).Text)
                            Dim l_msgstatus As String = Trim(item.Cells(12).Text)


                            Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("EazyConnect").ConnectionString
                            Dim Cmd As New SqlCommand, sqlConn As New SqlConnection(strcon), result As SqlParameter

                            '@extRefNo varchar(50), @poststartdate datetime,
                            '@postenddate datetime, @pan varchar(50), 
                            '@terminalID varchar(50),@cardAccpID varchar(50),
                            '@cardAccpName varchar(50), @transSystemDateTime datetime,
                            '@requestAmt decimal(18,2),@authorisedAmt decimal(18,2),
                            '@surcharge decimal(18,2),@narrative varchar(250),
                            '@BatchRefNo varchar(50),@reconStatus bit,
                            '@entered_by varchar(50) 

                            Cmd.CommandText = "Sproc_AddExtTransInfo"
                            Cmd.Connection = sqlConn
                            Cmd.CommandType = CommandType.StoredProcedure
                            sqlConn.Open()

                            Cmd.Parameters.Add(New SqlParameter("@poststartdate", l_reportstartdate))
                            Cmd.Parameters.Add(New SqlParameter("@postenddate", l_reportenddate))
                            'Cmd.Parameters.Add(New SqlParameter("@transSystemDateTime", Format((l_time), "MM-dd-yyyy")))
                            Cmd.Parameters.Add(New SqlParameter("@transSystemDateTime", l_time))

                            Cmd.Parameters.Add(New SqlParameter("@extRefNo", l_refno))
                            Cmd.Parameters.Add(New SqlParameter("@pan", l_pan))
                            Cmd.Parameters.Add(New SqlParameter("@cardAccpID", l_CardAcceptID))
                            Cmd.Parameters.Add(New SqlParameter("@cardAccpName", l_CardAcceptName))
                            Cmd.Parameters.Add(New SqlParameter("@terminalID", l_terminalID))
                            Cmd.Parameters.Add(New SqlParameter("@requestAmt", l_reqamount))
                            Cmd.Parameters.Add(New SqlParameter("@authorisedAmt", l_finalamount))
                            Cmd.Parameters.Add(New SqlParameter("@surcharge", l_feeamount))
                            'Cmd.Parameters.Add(New SqlParameter("@ExtBatchNo", txtref_no.Text))
                            Cmd.Parameters.Add(New SqlParameter("@BatchRefNo", txtref_no.Text))
                            Cmd.Parameters.Add(New SqlParameter("@narrative", l_msgtranstype))
                            Cmd.Parameters.Add(New SqlParameter("@reconStatus", 0))
                            Cmd.Parameters.Add(New SqlParameter("@entered_by", guser))



                            Cmd.ExecuteNonQuery()


                            sqlConn.Close()


                        Next
                    Else

                        Response.Write("<script>alert('Upload Batch entries has some errors!!!, Pls correct Batch') </script>")
                        Exit Sub

                    End If

                End If

            ElseIf drptranstype.SelectedValue = "4" Then

                ' Begin Section with xml Inputs
                If Me.txtref_no.Text = Nothing Then

                    txtref_no.Text = EazyClass.GetExtJournalNum(StrLocation)
                    Classes.alertwindow(Me.Page, "A New Batch Reference Generated : - " & Me.txtref_no.Text, "Halt")
                    If DataGrid1.Attributes("GenErrors") <> "Y" Then


                        Dim item As DataGridItem
                        For Each item In DataGrid1.Items

                            Dim l_Accountno As String = Trim(item.Cells(0).Text)
                            Dim l_PostDate As String = Trim(item.Cells(1).Text)
                            Dim l_TransDate As String = Trim(item.Cells(2).Text)
                            Dim l_CustNo As String = Trim(item.Cells(3).Text)
                            Dim l_Title As String = Trim(item.Cells(4).Text)
                            Dim l_CustName As String = Trim(item.Cells(5).Text)
                            Dim l_Sex As String = Trim(item.Cells(6).Text)
                            Dim l_DateOfBirth As String = Trim(item.Cells(7).Text)
                            Dim l_NETAcctNo As String = Trim(item.Cells(8).Text)
                            Dim l_NETAcctName As String = Trim(item.Cells(9).Text)
                            Dim l_Address As String = Trim(item.Cells(10).Text)
                            Dim l_Telephone As String = Trim(item.Cells(11).Text)
                            Dim l_ProdCode As String = Trim(item.Cells(12).Text)
                            Dim l_headsubcode As String = Trim(item.Cells(13).Text)
                            Dim l_status As String = Trim(item.Cells(14).Text)
                            If (l_DateOfBirth) = "&nbsp;" Then
                                l_DateOfBirth = "1900-01-01"
                            End If

                            Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("EazyConnect").ConnectionString
                            Dim Cmd As New SqlCommand, sqlConn As New SqlConnection(strcon), result As SqlParameter

                            Cmd.CommandText = "Sproc_AddExtTransPromoAccts"
                            Cmd.Connection = sqlConn
                            Cmd.CommandType = CommandType.StoredProcedure
                            Cmd.Connection.Open()





                            Cmd.Parameters.Add(New SqlParameter("@ExtRefNo", l_Accountno))
                            Cmd.Parameters.Add(New SqlParameter("@ExtBatchNo", txtref_no.Text))
                            Cmd.Parameters.Add(New SqlParameter("@PostDate", Format(CDate(l_PostDate), "MM-dd-yyyy")))
                            Cmd.Parameters.Add(New SqlParameter("@TransDate", Format(CDate(l_TransDate), "MM-dd-yyyy")))
                            Cmd.Parameters.Add(New SqlParameter("@TransCode", "Promo"))
                            Cmd.Parameters.Add(New SqlParameter("@RefCust_No", l_CustNo))
                            Cmd.Parameters.Add(New SqlParameter("@RefCustnames", l_CustName))
                            Cmd.Parameters.Add(New SqlParameter("@RefTitle_Code", l_Title))
                            Cmd.Parameters.Add(New SqlParameter("@RefCust_Type", ""))
                            Cmd.Parameters.Add(New SqlParameter("@RefSex", l_Sex))
                            Cmd.Parameters.Add(New SqlParameter("@RefBirtDate", Format(CDate(l_DateOfBirth), "MM-dd-yyyy")))
                            Cmd.Parameters.Add(New SqlParameter("@RefOldAccountNo", l_NETAcctNo))
                            Cmd.Parameters.Add(New SqlParameter("@RefOldAccountName", l_NETAcctName))
                            Cmd.Parameters.Add(New SqlParameter("@RefAddress", l_Address))

                            Cmd.Parameters.Add(New SqlParameter("@RefRemarks", l_status))
                            Cmd.Parameters.Add(New SqlParameter("@RefAddedBy", guser))
                            Cmd.Parameters.Add(New SqlParameter("@RefApprovedBy", offr2.SelectedValue))

                            Cmd.Parameters.Add(New SqlParameter("@Validatecheck", item.Cells(10).CssClass))
                            Cmd.Parameters.Add(New SqlParameter("@Telephone", l_Telephone))
                            Cmd.Parameters.Add(New SqlParameter("@ProdCode", l_ProdCode))
                            Cmd.Parameters.Add(New SqlParameter("@headsub_code", l_headsubcode))

                            Cmd.ExecuteNonQuery()


                            sqlConn.Close()
                        Next
                    Else

                        Response.Write("<script>alert('Upload Batch entries has some errors!!!, Pls correct Batch') </script>")
                        Exit Sub

                    End If

                End If

                'End Section xml
            Else
                txtref_no.Text = EazyClass.GetExtJournalNum(StrLocation)
                Classes.alertwindow(Me.Page, "A New Batch Reference Generated : - " & Me.txtref_no.Text, "Halt")
                Classes.RecordOp("Sproc_AddJournal '" & txtref_no.Text & "','" & "Monthly Salaries" & "'," & CDec(0) & ",'" & "N" & "','" & struser & "','" & StrLocation & "',0")

                Dim item As DataGridItem
                For Each item In DataGrid1.Items
                    Dim l_PostDate As String = Trim(item.Cells(0).Text)
                    Dim l_transDate As String = Trim(item.Cells(1).Text)
                    Dim l_accountno As String = Trim(item.Cells(2).Text)
                    Dim l_transcode As String = Trim(item.Cells(3).Text)
                    Dim l_narrative As String = Trim(item.Cells(4).Text)
                    Dim l_description As String = Trim(item.Cells(5).Text)
                    Dim l_debit As String = Trim(item.Cells(6).Text)
                    Dim l_credit As String = Trim(item.Cells(7).Text)
                    Dim l_bankcode As String = Trim(item.Cells(8).Text)
                    Dim l_chequeno As String = Trim(item.Cells(10).Text)
                    Dim l_TransRef As String = Trim(item.Cells(10).Text)
                    Dim l_status As String = 1 'Trim(item.Cells(11).CssClass)

                    If l_bankcode = "" Then
                        l_bankcode = l_chequeno
                    End If


                    Dim str_ins As String = "Sproc_AddExtTransHistJornal '" & Format(CDate(l_PostDate), "MM-dd-yyyy") & "','" & Format(CDate(l_transDate), "MM-dd-yyyy") & "','" & l_accountno & "'," & _
                    "'" & l_transcode & "','" & l_narrative & "','" & l_description & "'," & l_debit & " ," & l_credit & "," & _
                    "'" & l_bankcode & "','" & l_chequeno & "','" & l_TransRef & "','" & txtref_no.Text & "','" & struser & "'," & CInt(l_status) & ",'" & Me.offr2.SelectedValue & "','" & StrLocation & "'"
                    Classes.RecordOp(str_ins)



                    Response.Write(str_ins)

                Next
            End If
            'Dim str_error As String = ValidateEntries()
            'If str_error = Nothing  Then
            Response.Write("<script>alert('Upload Completed') </script>")
            txtref_no.Text = Nothing

            DataGrid1.DataSource = Nothing
            DataGrid1.DataBind()


        Else

            Response.Write("<script>alert(' Your user section has expired, please logout and try the upload again ') </script>")

        End If



    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then

            LoadApprovingOfficers()
        End If

    End Sub

    Protected Sub Button4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button4.Click
        Response.Redirect("TransImportsExt.aspx")

    End Sub

   
End Class
