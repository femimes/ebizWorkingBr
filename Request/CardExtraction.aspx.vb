Option Strict Off
Imports System
Imports System.Collections.Generic
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.IO
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports NETClass

''' <summary>
''' Card extraction: writes the card production files of approved batches (Sproc_GetCardForExtactInfo, status 3)
''' and moves each batch to status 5 (Sproc_ApproveCardRequestUpd); "Drop" moves it to status 99.
''' Plain ASP.NET controls (no DevExpress). The file-generation code (generateCardFiles and the ISO8583
''' formatting functions) is kept exactly as it was.
''' </summary>
Partial Class Mandate_CardExtraction
    Inherits System.Web.UI.Page

    Const NUMBERTYPE = 1
    Const STRTYPE = 2
    Const DATETYPE = 3
    ' Constants used for string alignment by the file formatting functions.
    Const LALIGN = 1
    Const RALIGN = 2

    Private Const StatusExtracted As Integer = 5
    Private Const StatusDropped As Integer = 99

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Try
                txtFilePath.Text = Convert.ToString(Classes.GetDataset("Sproc_GetPref").Tables(0).Rows(0).Item("ExtractionPath")).Trim().TrimEnd("\"c)
            Catch ex As Exception
                ShowError("The extraction folder could not be read from the preferences (Sproc_GetPref): " & ex.Message)
            End Try
        End If
        litPath.Text = If(txtFilePath.Text = "", "<strong>(not set)</strong>", "<strong>" & HttpUtility.HtmlEncode(txtFilePath.Text) & "</strong>")
    End Sub

    Protected Sub SqlDataSource1_Selected(ByVal sender As Object, ByVal e As SqlDataSourceStatusEventArgs) Handles SqlDataSource1.Selected
        litTotal.Text = If(e.Exception Is Nothing, String.Format("{0:N0} batch{1}", e.AffectedRows, If(e.AffectedRows = 1, "", "es")), "")
    End Sub

    ' =====================================================================
    ' Selection
    ' =====================================================================
    Private Function SelectedRefs() As List(Of String)
        Dim refs As New List(Of String)()
        For Each row As GridViewRow In ctrlGrid.Rows
            If row.RowType <> DataControlRowType.DataRow Then Continue For
            Dim chk As CheckBox = TryCast(row.FindControl("chkSelect"), CheckBox)
            If chk IsNot Nothing AndAlso chk.Checked Then refs.Add(Convert.ToString(ctrlGrid.DataKeys(row.RowIndex).Value))
        Next
        Return refs
    End Function

    ' =====================================================================
    ' Actions
    ' =====================================================================
    ''' <summary>Extract one batch (the "Extract" button on its row).</summary>
    Protected Sub ctrlGrid_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles ctrlGrid.RowCommand
        If e.CommandName <> "Extract" Then Return
        Extract(New List(Of String)(New String() {Convert.ToString(e.CommandArgument)}))
    End Sub

    Protected Sub BtnApprove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnApprove.Click
        Extract(SelectedRefs())
    End Sub

    Protected Sub btnDrop_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDrop.Click
        Dim refs As List(Of String) = SelectedRefs()
        If refs.Count = 0 Then ShowError("Tick at least one batch first.") : Return
        Dim done As Integer = 0
        Dim failed As New List(Of String)()
        For Each refNo As String In refs
            Try
                SetStatus(refNo, StatusDropped)
                done += 1
            Catch ex As Exception
                failed.Add(refNo & ": " & ex.Message)
            End Try
        Next
        Report("Dropped", done, failed, "")
    End Sub

    Private Sub Extract(ByVal refs As List(Of String))
        If refs.Count = 0 Then ShowError("Tick at least one batch first.") : Return
        If txtFilePath.Text.Trim() = "" Then ShowError("No extraction folder is set in the preferences, so nothing was extracted.") : Return
        Dim done As Integer = 0
        Dim failed As New List(Of String)()
        For Each refNo As String In refs
            If refNo = "" Then Continue For
            Try
                generateCardFiles(refNo, "", txtFilePath.Text)
                SetStatus(refNo, StatusExtracted)
                done += 1
            Catch ex As Exception
                failed.Add(refNo & ": " & ex.Message)
            End Try
        Next
        Report("Extracted", done, failed, " Files are in " & txtFilePath.Text & "\<batch ref>.")
    End Sub

    Private Sub SetStatus(ByVal refNo As String, ByVal status As Integer)
        Exec("Sproc_ApproveCardRequestUpd", P("@RefNo", refNo), P("@ApprovingOfficer", CurrentUser()), P("@status", status))
    End Sub

    Private Sub Report(ByVal action As String, ByVal done As Integer, ByVal failed As List(Of String), ByVal extra As String)
        If done > 0 Then ShowSuccess(String.Format("{0} {1} batch{2}.{3}", action, done, If(done = 1, "", "es"), extra)) Else ShowSuccess("")
        If failed.Count > 0 Then litError.Text = HttpUtility.HtmlEncode(String.Format("{0} batch(es) not processed: {1}", failed.Count, String.Join("; ", failed.ToArray())))
        ctrlGrid.DataBind()
    End Sub

    ' =====================================================================
    ' Card file generation (unchanged from the original page)
    ' =====================================================================
    ' ---- BEGIN unchanged file generation ----
    Sub generateCardFiles(ByVal BatchRef As String, ByVal FileLocation As String, ByVal FolderLocation As String)
        Dim strselAccount As String = String.Format("Sproc_ATMGetAccountsByRequest '{0}'", BatchRef)
        Dim ds As New DataSet, row As DataRow
        Dim strconn As String = System.Configuration.ConfigurationManager.ConnectionStrings("EazyConnect").ConnectionString
        Dim Sqlconn As New SqlClient.SqlConnection(strconn)
        Dim Sqlcmd As New SqlClient.SqlDataAdapter(strselAccount, Sqlconn)
        Sqlcmd.Fill(ds, "TableCardInfo")

        'Dim FileFolderPath As String = "C:\Resort\" & Replace(FormatDateTime(Today.Date.Now, Microsoft.VisualBasic.DateFormat.LongTime), ":", "") & Replace(User.Identity.Name, "?", "_") & "\"
        'Dim lp As String = Server.("~/Uploaded/")



        Dim FileFolderPath As String = String.Format("{0}\{1}\", txtFilePath.Text, BatchRef) '"C:\Resort2\Batch1\" ' & Replace(FormatDateTime(Today.Date.Now, Microsoft.VisualBasic.DateFormat.LongTime), ":", "") & Replace(User.Identity.Name, "?", "_") & "\"
        'Creates the folder if it does not exists
        If (Not Directory.Exists(FileFolderPath)) Then
            Directory.CreateDirectory(FileFolderPath)
        End If


        Dim strselReqCardType As String = String.Format("SELECT * FROM Cardrequest a  WHERE    RefNo = '{0}'", BatchRef)
        Dim dss As New DataSet
        Dim Sqlconns As New SqlClient.SqlConnection(strconn)
        Dim Sqlcmds As New SqlClient.SqlDataAdapter(strselReqCardType, Sqlconn)
        Sqlcmds.Fill(dss, "TableCardInfo")
        Dim strProdType As String = ""
        Dim strProdTypeSel As String = ""
        If dss.Tables(0).Rows(0).Item("CardBrand") = "0" Then

            Dim csvFileMobile As String = FileFolderPath & "CardMobile.txt"
            Dim outFileMobile As IO.StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(csvFileMobile, False)



            For Each row In ds.Tables("TableCardInfo").Rows

                ' Section Mobile.txt
                outFileMobile.Write(String.Format("{0},", row("PhoneNo")))  ' Phone Number
                outFileMobile.Write(String.Format("{0},", row("PANno")))  ' Pan No
                outFileMobile.Write(String.Format("{0},", "1234"))  ' Pan No
                outFileMobile.Write(String.Format("{0},", row("account_no")))  ' accountNo No
                outFileMobile.Write(String.Format("{0},", row("ATMProductType")))  ' Account or Product Type No
                outFileMobile.Write(Format_StringAs(Format(CDate(row("CardExpiryDate")), "yyMM") & ",", 4, LALIGN) & "") ' Card expirydate YYMM

                outFileMobile.WriteLine()
            Next
            outFileMobile.Close()

            Exit Sub
        End If
        ' End section if card is mobile


        If dss.Tables(0).Rows(0).Item("CardBrand") = "4" Then

            Dim csvFileOTP As String = FileFolderPath & "OTP.txt"
            Dim outFileOTP As IO.StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(csvFileOTP, False)



            For Each row In dss.Tables("TableCardInfo").Rows
                strProdTypeSel = String.Format("SELECT * from VwaccMaster WHERE accountno = '{0}'", row("accountno"))
                strProdType = NETClass.Classes.AXGetDataset(strProdTypeSel).Tables(0).Rows(0).Item("ProdCatDesc")
                outFileOTP.Write(String.Format("{0},", (Mid(row("address"), 1, 30).ToString.Replace(",", " ").ToString.ToUpper.Replace("'", "").Replace("&", "").Replace(".", "").Replace("/", "").Replace("~", "").Replace("@", "").Replace("!", "").Replace(";", "").Replace(":", "").Replace("+", "").Replace("-", "").Replace("\", "").Replace("*", "").Replace("^", "").Replace(",", "").Replace("_", "").Replace("(", "").Replace(")", "")).Replace(vbTab, "")))  ' Address No
                outFileOTP.Write(String.Format("{0},", (Mid(row("address"), 1, 30).ToString.Replace(",", " ").ToString.ToUpper.Replace("'", "").Replace("&", "").Replace(".", "").Replace("/", "").Replace("~", "").Replace("@", "").Replace("!", "").Replace(";", "").Replace(":", "").Replace("+", "").Replace("-", "").Replace("\", "").Replace("*", "").Replace("^", "").Replace(",", "").Replace("_", "").Replace("(", "").Replace(")", "")).Replace(vbTab, "")))  ' Address 2client_name
                outFileOTP.Write(String.Format("{0},", ""))  ' empty1
                outFileOTP.Write(String.Format("{0},", ""))  ' empty 2
                outFileOTP.Write(String.Format("{0},", (Mid(row("city"), 1, 30).ToString.Replace(",", " ").ToString.ToUpper.Replace("'", "").Replace("&", "").Replace(".", "").Replace("/", "").Replace("~", "").Replace("@", "").Replace("!", "").Replace(";", "").Replace(":", "").Replace("+", "").Replace("-", "").Replace("\", "").Replace("*", "").Replace("^", "").Replace(",", "").Replace("_", "").Replace("(", "").Replace(")", "")).Replace(vbTab, "")))   ' empty3
                outFileOTP.Write(String.Format("{0},", "Nigeria"))  ' empty 4
                outFileOTP.Write(String.Format("{0},", row("PANno")))  'Full Panno      
                outFileOTP.Write(String.Format("{0},", (Mid(row("description").ToString.ToUpper.Replace("'", "").Replace("&", "").Replace(".", "").Replace("/", "").Replace("~", "").Replace("@", "").Replace("!", "").Replace(";", "").Replace(":", "").Replace("+", "").Replace("-", "").Replace("\", "").Replace("*", "").Replace("^", "").Replace(",", "").Replace("_", "").Replace("(", "").Replace(")", "").Replace(vbTab, ""), 1, 25))))  ' Name on Card
                outFileOTP.Write(String.Format("{0},", row("accountno")))  'accountno
                outFileOTP.Write(String.Format("{0},", row("CardPhoneNo")))  'primary phoneno
                outFileOTP.Write(String.Format("{0},", row("CardPhoneNo")))  'secondary phoneno
                outFileOTP.Write(String.Format("{0},", ""))  ' empty1
                outFileOTP.Write(String.Format("{0},", ""))  ' empty 2
                outFileOTP.Write(String.Format("{0},", row("PANno")))  'OTP userid
                outFileOTP.Write(String.Format("{0}", "2"))  ' channels


                outFileOTP.WriteLine()
            Next
            outFileOTP.Close()

            Exit Sub
        End If
        ' End section if card is Tokens

        If dss.Tables(0).Rows(0).Item("CardBrand") = "8" Then

            Dim csvFileUSSD As String = FileFolderPath & "Ussd.csv"
            Dim outFileUSSD As IO.StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(csvFileUSSD, False)

            'Dim WkBookUssd As New Microsoft.Office.Interop.Excel.Workbook

            ''WkBookUssd.Sheets
            'Dim SheetUssd As Worksheet
            'SheetUssd = WkBookUssd.ActiveSheet(0)
            'SheetUssd.Name = "Ussd"

            'For j As Integer = 1 To dss.Tables("TableCardInfo").Columns.Count

            '    For i As Integer = 1 To dss.Tables("TableCardInfo").Rows.Count
            '        'For Each row In dss.Tables("TableCardInfo").Rows


            '        SheetUssd.Cells(i, j) = dss.Tables("TableCardInfo").Rows(i).Item(j)


            '    Next

            'Next
            'WkBookUssd.SaveAs(FileFolderPath & "CardUSSDXLS")
            'WkBookUssd.Close()


            For Each row In ds.Tables("TableCardInfo").Rows
                strProdTypeSel = String.Format("SELECT * from VwaccMaster WHERE accountno = '{0}'", row("account_no"))
                'strProdType = NETClass.Classes.AXGetDataset(strProdTypeSel).Tables(0).Rows(0).Item("ProdCatDesc")
                ' Section OTP.txt
                'outFileUSSD.Write(String.Format("{0},", "NA"))  ' Title
                'outFileUSSD.Write(String.Format("{0},", (Mid(row("description").ToString.ToUpper.Replace("'", "").Replace("&", "").Replace(".", "").Replace("/", "").Replace("~", "").Replace("@", "").Replace("!", "").Replace(";", "").Replace(":", "").Replace("+", "").Replace("-", "").Replace("\", "").Replace("*", "").Replace("^", "").Replace(",", "").Replace("_", "").Replace("(", "").Replace(")", "").Replace(vbTab, ""), 1, 25))))  ' Fullname No
                'outFileUSSD.Write(String.Format("{0},", "."))  ' Other Names No
                'outFileUSSD.Write(String.Format("{0},", (Mid(row("address"), 1, 30).ToString.Replace(",", " ").ToString.ToUpper.Replace("'", "").Replace("&", "").Replace(".", "").Replace("/", "").Replace("~", "").Replace("@", "").Replace("!", "").Replace(";", "").Replace(":", "").Replace("+", "").Replace("-", "").Replace("\", "").Replace("*", "").Replace("^", "").Replace(",", "").Replace("_", "").Replace("(", "").Replace(")", "")).Replace(vbTab, "")))  ' Address No
                'outFileUSSD.Write(String.Format("{0},", (Mid(row("address"), 1, 30).ToString.Replace(",", " ").ToString.ToUpper.Replace("'", "").Replace("&", "").Replace(".", "").Replace("/", "").Replace("~", "").Replace("@", "").Replace("!", "").Replace(";", "").Replace(":", "").Replace("+", "").Replace("-", "").Replace("\", "").Replace("*", "").Replace("^", "").Replace(",", "").Replace("_", "").Replace("(", "").Replace(")", "")).Replace(vbTab, "")))  ' Address 2client_name
                'outFileUSSD.Write(String.Format("{0},", ""))  ' empty1
                'outFileUSSD.Write(String.Format("{0},", ""))  ' empty 2
                'outFileUSSD.Write(String.Format("{0},", (Mid(row("city"), 1, 30).ToString.Replace(",", " ").ToString.ToUpper.Replace("'", "").Replace("&", "").Replace(".", "").Replace("/", "").Replace("~", "").Replace("@", "").Replace("!", "").Replace(";", "").Replace(":", "").Replace("+", "").Replace("-", "").Replace("\", "").Replace("*", "").Replace("^", "").Replace(",", "").Replace("_", "").Replace("(", "").Replace(")", "")).Replace(vbTab, "")))   ' empty3
                'outFileUSSD.Write(String.Format("{0},", "Nigeria"))  ' empty 4
                'outFileUSSD.Write(String.Format("{0},", row("PANno")))  'Full Panno      
                'outFileUSSD.Write(String.Format("{0},", (Mid(row("description").ToString.ToUpper.Replace("'", "").Replace("&", "").Replace(".", "").Replace("/", "").Replace("~", "").Replace("@", "").Replace("!", "").Replace(";", "").Replace(":", "").Replace("+", "").Replace("-", "").Replace("\", "").Replace("*", "").Replace("^", "").Replace(",", "").Replace("_", "").Replace("(", "").Replace(")", "").Replace(vbTab, ""), 1, 25))))  ' Name on Card
                'outFileUSSD.Write(String.Format("{0},", row("Seq")))  'Serial No
                outFileUSSD.Write(String.Format("{0},", row("Description").ToString().Replace(",", "")) + vbTab)  'Customer Name No
                outFileUSSD.Write(String.Format("{0},", row("account_no").ToString()) + vbTab)  'accountno
                outFileUSSD.Write(String.Format("{0},", row("PhoneNo").ToString()) + vbTab)  'primary phoneno
                ' outFileUSSD.Write(String.Format("{0},", row("accountno")))  'accountno
                'outFileUSSD.Write(String.Format("{0}", strProdType))  'Product Type phoneno
                outFileUSSD.Write(String.Format("{0},", row("ProdCatCodeDesc").ToString()) + vbTab)  'Product Type phoneno
                ' outFileUSSD.Write(String.Format("{0},", ""))  ' empty1


                outFileUSSD.WriteLine()
            Next
            outFileUSSD.Close()
            outFileUSSD.Dispose()

            Exit Sub
        End If
        ' End section if card is Ussd



        Dim csvFile As String = FileFolderPath & "cards.txt"
        Dim outFile As IO.StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(csvFile, False)

        Dim csvFileAcct As String = FileFolderPath & "accounts.txt"
        Dim outFileAcct As IO.StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(csvFileAcct, False)

        Dim csvFileCardAcct As String = FileFolderPath & "cardaccounts.txt"
        Dim outFileCardAcct As IO.StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(csvFileCardAcct, False)


        Dim csvFileAcctBal As String = FileFolderPath & "accountbalances.txt"
        Dim outFileAcctBal As IO.StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(csvFileAcctBal, False)
        Dim csvFileCardIssuance As String = FileFolderPath & "cardissuance.txt"


        Dim TxtToCsvFileCardIssuance As String = FileFolderPath & "cardissuance.csv"
        '' Dim csvFileCardIssuance As String = FileFolderPath & "cardissuance.txt"
        Dim outFileCardIssuance As IO.StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(csvFileCardIssuance, False)
        ' Section To generate Card.txt




        For Each row In ds.Tables("TableCardInfo").Rows

            Dim dsAcctMore As New DataSet()
            ' Section Get Additional details from accounts


            'End Section Acct more 

            outFile.Write(String.Format("U,{0},", row("PANno").ToString())) ' PAN Number No
            ' outFile.Write("U," & Format_StringAs(row("account_no"), 19, LALIGN) & ",")   ' PAN Number No
            ' Response.Write(row("PANno").ToString())
            ' outFile.Write(Format_StrNumber(row("CardIssueCounter"), 3, RALIGN) & ",") ' Sequence in which card is issued
            outFile.Write(Format_StrNumber("001", 3, RALIGN) & ",") ' Sequence in which card is issued
            outFile.Write(row("CardBrandName") & ",") ' Card Program Type 'outFile.Write(Format_StringAs("PSHDEBITCARD", 20, RALIGN) & ",") ' Card Program Type
            outFile.Write(row("ATMProductType") & ",") 'outFile.Write(Format_StringAs("1", 3, LALIGN) & ",") ' new Card Postilion Account Type (Card status 1 for active -lenght =8 )
            outFile.Write("6" & ",") 'outFile.Write(Format_String("1", 1, LALIGN) & ",") ' Card status  whether active or not
            outFile.Write(",") 'outFile.Write(Format_String("1", 1, LALIGN) & ",") ' Card status for custom values
            'outFile.Write(Format_StringAs("1508", 4, LALIGN) & ",") ' Card expirydate YYMM
            outFile.Write(Format_StringAs(Format(CDate(row("CardExpiryDate")), "yyMM") & ",", 4, LALIGN) & ",") ' Card expirydate YYMM
            outFile.Write(",") 'outFile.Write(Format_String("02", 2, LALIGN) & ",") ' Hold response code not compulsory
            outFile.Write(",") 'outFile.Write(Format_String("1508", 4, LALIGN) & ",") ' Hold track value
            outFile.Write(",") ' Track 2 offset
            outFile.Write("4,") ' Secure IBM PIN lenght
            outFile.Write(",") ' Secure PIN lenght offset Secure 
            outFile.Write(",") ' inSecure PIN lenght Insecure PIN offset or PVV
            outFile.Write(",") ' Validation data question
            outFile.Write(",") ' Validation data  
            outFile.Write(",") ' Card Holder Response information
            outFile.Write(",") ' Cardholder title  
            outFile.Write(",") ' Cardholder first Name
            outFile.Write(",") ' Cardholder middle Name
            outFile.Write(",") ' Cardholder last Name
            outFile.Write(Mid(row("description").ToString.ToUpper.Replace("'", "").Replace("&", "").Replace(".", "").Replace("/", "").Replace("~", "").Replace("@", "").Replace("!", "").Replace(";", "").Replace(":", "").Replace("+", "").Replace("-", "").Replace("\", "").Replace("*", "").Replace("^", "").Replace(",", "").Replace("_", "").Replace("(", "").Replace(")", "").Replace(vbTab, ""), 1, 25) & ",") 'Format_StringAs(row("description").ToString.Substring(0, 25), 26, LALIGN) & ",") ' Cardholder name on the Card

            outFile.Write((Mid(row("address"), 1, 30).ToString.Replace(",", " ").ToString.ToUpper.Replace("'", "").Replace("&", "").Replace(".", "").Replace("/", "").Replace("~", "").Replace("@", "").Replace("!", "").Replace(";", "").Replace(":", "").Replace("+", "").Replace("-", "").Replace("\", "").Replace("*", "").Replace("^", "").Replace(",", "").Replace("_", "").Replace("(", "").Replace(")", "") & ",").Replace(vbTab, "")) 'Format_StringAs(row("address").ToString.Substring(0, 25), 30, LALIGN) & ",") ' Cardholder Address
            outFile.Write(",") ' Cardholder address 2
            outFile.Write((Mid(row("city").ToString.ToUpper.Replace("'", "").Replace("&", "").Replace(".", "").Replace("/", "").Replace("~", "").Replace("@", "").Replace("!", "").Replace(";", "").Replace(":", "").Replace("+", "").Replace("-", "").Replace("\", "").Replace("*", "").Replace("^", "").Replace(",", "").Replace("_", "").Replace("(", "").Replace(")", ""), 1, 20) & ",").Replace(vbTab, "")) '(Format_StringAs(row("city").ToString.Substring(0, 19), 20, LALIGN) & ",") ' Cardholder City
            outFile.Write((Mid(row("city").ToString.ToUpper.Replace("'", "").Replace("&", "").Replace(".", "").Replace("/", "").Replace("~", "").Replace("@", "").Replace("!", "").Replace(";", "").Replace(":", "").Replace("+", "").Replace("-", "").Replace("\", "").Replace("*", "").Replace("^", "").Replace(",", "").Replace("_", "").Replace("(", "").Replace(")", ""), 1, 20) & ",").Replace(vbTab, "")) '(Format_StringAs(row("city").ToString.Substring(0, 19), 20, LALIGN) & ",") ' Cardholder City' Cardholder Region or state 2
            outFile.Write("234" & ",") ' Cardholder postal Code
            outFile.Write("NGA,") ' Cardholder Country Code

            outFile.Write(Format(CDate(row("DateofBith").ToString()), "yyyyMMdd") & ",") ' Cardholder Date of birth
            outFile.Write("0,") ' Cardholder mailer destination
            outFile.Write((Mid(row("client_name"), 1, 26) & ",")) '(Format_StringAs(row("client_name").ToString.Substring(0, 25), 26, LALIGN) & ",") ' Cardholder company name
            outFile.Write("0,") ' Cardholder VIP status and indicates velocity limit waived
            outFile.Write(",") ' Cardholder VIP status lapse date and indicates velocity expiry date
            outFile.Write("SSPPPPCCC,") ' Cardholder Discretionary data
            outFile.Write(",") ' Cardholder Discretionary data date issued
            outFile.Write(",") ' Cardholder Discretionary data date activated
            outFile.Write(",") ' Cardholder Issuer reference
            outFile.Write(row("headsub_code") & ",") ' Cardholder Branch Code
            outFile.Write((Mid(row("cust_no"), 1, 26))) '(Format_StringAs(row("cust_no"), 26, LALIGN) & ",") ' Cardholder Customer ID


            ' Section Account.txt
            outFileAcct.Write(String.Format("U,{0},", row("account_no")))  ' Acct Number No
            outFileAcct.Write(row("ATMProductType") & ",") 'Account type, Savings Account 10,Current -20, and loans 30,deposits 50, universal 40
            outFileAcct.Write("566" & ",") 'Currency Code
            outFileAcct.Write((Mid(row("cust_no"), 1, 26))) '(Format_StringAs(row("cust_no"), 26, LALIGN) & ",") ' Cardholder Customer ID



            ' Section card accounts
            outFileCardAcct.Write(String.Format("U,{0},", row("PANno")))  ' PAN Number No
            ' outFileCardAcct.Write(row("CardIssueCounter") & ",") 'sequence No type
            outFileCardAcct.Write("001" & ",") 'sequence No type
            outFileCardAcct.Write(Mid(row("account_no"), 1, 19) & ",") 'Account ID
            outFileCardAcct.Write(row("ATMProductType") & ",") 'Account type nominated ' 
            outFileCardAcct.Write("1" & ",") 'Account qualifier
            outFileCardAcct.Write(row("ATMProductType")) 'Account type


            ' Section  accounts Balance
            outFileAcctBal.Write(String.Format("U,{0},", row("account_no")))  '  AcctNumber No
            outFileAcctBal.Write(Format_Amount(row("book_bal"), 12) & ",") 'Ledger Balance No type
            outFileAcctBal.Write(Format_Amount(row("clear_bal"), 12) & ",") 'Available Balance No type
            outFileAcctBal.Write(row("ATMProductType")) 'Account type


            'Section CardIssuance

            outFileCardIssuance.Write(row("Title").ToString() + vbTab) ' CardHolder Name Title 
            outFileCardIssuance.Write(row("FirstName").ToString().Replace(",", "") + vbTab) 'CardHolder First Name  
            outFileCardIssuance.Write(row("MiddleName").ToString().Replace(",", "") + vbTab) 'CardHolder Middle Name
            outFileCardIssuance.Write(row("LastName").ToString().Replace(",", "") + vbTab) 'CardHolder Last Name
            outFileCardIssuance.Write(row("description").ToString().Replace(",", "") + vbTab) 'CardHolder Display Name
            outFileCardIssuance.Write((Format(CDate(row("DateofBith").ToString()), "yyyy/MM/dd")) + vbTab) ' Cardholder Date of birth
            outFileCardIssuance.Write((Mid(row("address").ToString().Replace(",", ""), 1, 30).ToString.Replace(",", " ").ToString.ToUpper.Replace("'", "").Replace("&", "").Replace(".", "").Replace("/", "").Replace("~", "").Replace("@", "").Replace("!", "").Replace(";", "").Replace(":", "").Replace("+", "").Replace("-", "").Replace("\", "").Replace("*", "").Replace("^", "").Replace(",", "").Replace("_", "").Replace("(", "").Replace(")", "") & ",").Replace(vbTab, "") + vbTab) 'Format_StringAs(row("address").ToString.Substring(0, 25), 30, LALIGN) & ",") ' Cardholder Address
            outFileCardIssuance.Write(vbTab) ' Cardholder address 2
            outFileCardIssuance.Write((Mid(row("city").ToString.ToUpper.Replace("'", "").Replace("&", "").Replace(".", "").Replace("/", "").Replace("~", "").Replace("@", "").Replace("!", "").Replace(";", "").Replace(":", "").Replace("+", "").Replace("-", "").Replace("\", "").Replace("*", "").Replace("^", "").Replace(",", "").Replace("_", "").Replace("(", "").Replace(")", ""), 1, 20) & ",").Replace(vbTab, "") + vbTab) '(Format_StringAs(row("city").ToString.Substring(0, 19), 20, LALIGN) & ",") ' Cardholder City
            outFileCardIssuance.Write((Mid(row("StateName").ToString.ToUpper.Replace("'", "").Replace("&", "").Replace(".", "").Replace("/", "").Replace("~", "").Replace("@", "").Replace("!", "").Replace(";", "").Replace(":", "").Replace("+", "").Replace("-", "").Replace("\", "").Replace("*", "").Replace("^", "").Replace(",", "").Replace("_", "").Replace("(", "").Replace(")", ""), 1, 20) & ",").Replace(vbTab, "") + vbTab) '(Format_StringAs(row("city").ToString.Substring(0, 19), 20, LALIGN) & ",") ' Cardholder City' Cardholder Region or state 2

            outFileCardIssuance.Write("NGN" + vbTab) ' Cardholder Country Code 
            outFileCardIssuance.Write("Nigeria" + vbTab) ' Cardholder postal Code
            outFileCardIssuance.Write(row("PhoneNo") + vbTab) 'CardHolder PhoneNo
            outFileCardIssuance.Write(vbTab) ' Cardholder Email 2
            outFileCardIssuance.Write(CStr(row("headsub_code")) + vbTab) ' Cardholder Branch Code
            outFileCardIssuance.Write(String.Format("{0}", CStr(row("CusID"))) + vbTab)   ' Cardholder CustomerID
            outFileCardIssuance.Write(String.Format("{0}", CStr(row("PANno").ToString())) + vbTab) ' Cardholder PANno
            outFileCardIssuance.Write(String.Format("{0}", CStr(row("account_no")).ToString()) + vbTab) ' Cardholder PANno
            outFileCardIssuance.Write(String.Format("{0}", row("ProdCatCodeDesc")) + vbTab) ' Cardholder PANno

            'outFileCardIssuance.Write(row("headsub_code") & ",") ' Cardholder Branch Code
            'outFileCardIssuance.Write((Mid(row("CusID"), 1, 19) & ",")) ' Cardholder CustomerID
            'outFileCardIssuance.Write((Mid(row("PANno").ToString(), 1, 19) & ",")) ' Cardholder PANno
            'outFileCardIssuance.Write((Mid(row("account_no"), 1, 19) & ",")) ' Cardholder PANno
            'outFileCardIssuance.Write(String.Format("{0},", row("ProdCatCodeDesc"))) ' Cardholder PANno


            outFile.WriteLine()
            outFileAcct.WriteLine()
            outFileCardAcct.WriteLine()
            outFileAcctBal.WriteLine()
            outFileCardIssuance.WriteLine()



        Next

        outFile.Close()
        outFileAcct.Close()
        outFileCardAcct.Close()
        outFileAcctBal.Close()
        outFileCardIssuance.Flush()
        outFileCardIssuance.Close()

        ' Section Card.txt

        'Section convert cardIssuance to csv

        'var strDecryMsg = strDECResult.Substring(3);
        '//XmlTextReader xmlReadr = New XmlTextReader(strDecryMsg);
        'StringReader SR = New StringReader(strDecryMsg);
        'DataSet LoadDs = New DataSet();
        '//LoadDs.ReadXml(strDecryMsg);
        'LoadDs.ReadXml(SR);
        'foreach(DataRow DrRow In LoadDs.Tables[0].Rows)
        '{
        '    Response.Write(DrRow["SessionID"]);
        '    Response.Write("</br>");
        '}
        'Dim sr As StreamReader = New StreamReader(csvFileCardIssuance)
        'DataView View = New DataView(ds.Tables[0]);
        '        DataTable dtQueryTable = View.ToTable(False, New String[] { "Status", "Pid", "PayerName", "AgencyCode", "RevenueCode", "OraAgencyRev", "Currency" });


        Dim VwLoadDs = New DataView(ds.Tables("TableCardInfo"))
        Dim dtQueryTable As DataTable = VwLoadDs.ToTable(False, New String() {"Title", "FirstName", "MiddleName",
                                                         "LastName", "description", "DateofBithISWIssuance", "address", "Address2", "city",
                                                         "StateName", "Country", "PostalCode", "PhoneNo", "Email", "headsub_code", "CusID", "PANno", "account_no", "ProdCatCodeDesc"})
        Dim csv As String = String.Empty
        'Adding the Rows
        For Each rowRec As DataRow In dtQueryTable.Rows
            For Each cell As DataColumn In dtQueryTable.Columns
                'Add the Data rows.
                csv += (rowRec(cell.ColumnName).ToString).ToString().Replace(",", ";") & ","c
            Next

            'Add new line.
            csv += vbCr & vbLf
        Next

        'LoadDs.ReadXml(sr)

        'Dim sb As StringBuilder = New StringBuilder
        'Dim dt As DataTable = dtQueryTable
        'For Each dr As DataRow In dt.Rows
        '    For Each dc As DataColumn In dt.Columns
        '        sb.Append((WriteCSV(dr(dc.ColumnName).ToString) + ","))
        '        'sb.Append((ToCsv(dr, True)))
        '    Next
        '    sb.Remove((sb.Length - 1), 1)
        '    sb.AppendLine()
        'Next

        File.WriteAllText(TxtToCsvFileCardIssuance, csv.ToString())
        'File.WriteAllText(TxtToCsvFileCardIssuance, sb.ToString())
        ' Section determine if card is mobile 




        If dss.Tables(0).Rows(0).Item("CardBrand") = "3" Then

            Dim csvFileMobile As String = FileFolderPath & "CardMobile.txt"
            Dim outFileMobile As IO.StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(csvFileMobile, False)



            For Each row In ds.Tables("TableCardInfo").Rows

                ' Section Mobile.txt
                outFileMobile.Write(String.Format("{0},", row("PhoneNo")))  ' Phone Number
                outFileMobile.Write(String.Format("{0},", row("PANno")))  ' Pan No
                outFileMobile.Write(String.Format("{0},", "1234"))  ' Pan No
                outFileMobile.Write(String.Format("{0},", row("account_no")))  ' accountNo No
                outFileMobile.Write(String.Format("{0},", row("ATMProductType")))  ' Account or Product Type No
                outFileMobile.Write(Format_StringAs(Format(CDate(row("CardExpiryDate")), "yyMM") & ",", 4, LALIGN) & "") ' Card expirydate YYMM

                outFileMobile.WriteLine()
            Next
            outFileMobile.Close()


        End If

    End Sub

    ' Section for ISO Formats

    '   Generic Function to Format Amount Fields as expected by ISO8583 File.
    Function Format_Amount(ByVal amt As String, ByVal length As Integer)
        Dim PosOfDot As Integer
        Dim TempAmount As String
        Dim FormatString As String
        Dim i As Integer

        FormatString = ""
        For i = 1 To length
            FormatString = FormatString & "0"
        Next i
        TempAmount = LTrim(RTrim(amt))
        PosOfDot = InStr(1, TempAmount, ".")
        If PosOfDot <> 0 Then
            TempAmount = Left(TempAmount, PosOfDot - 1) & Right(TempAmount, Len(TempAmount) - PosOfDot)
        Else
            TempAmount = TempAmount & "00"
        End If
        Format_Amount = Format(CDbl(TempAmount), FormatString)

    End Function
    '   Generic Function to format String as expected by ISO8583 File.
    Public Function Format_String(ByVal STR As String, ByVal length As Integer, ByVal align As Integer)
        Dim i As Integer
        Dim NoOfSpaces As Integer
        Dim temp As String

        ' Inputs:   STR         -----   String to be Formatted.
        '           length      -----   Length of String After Format.
        '           align       -----   Alignment Required. Decided Left Padding / Right Padding.

        If STR = "" Then    ' No String To Format. Return as many spaces as requested.
            Format_String = Space(length)
            Exit Function
        End If

        temp = ""
        If length <= Len(STR) Then
            Format_String = Left(STR, length)
        Else
            If align = RALIGN Then
                NoOfSpaces = length - Len(STR)
                For i = 1 To NoOfSpaces
                    temp = temp & " "
                Next i
                Format_String = STR & temp
            ElseIf align = LALIGN Then
                For i = 1 To length
                    temp = temp & "0"
                Next i
                Format_String = Format(CDbl(STR), temp)
            Else
                Format_String = STR
            End If
        End If
    End Function
    Public Function ToCsv(source As DataRow,
                          quoteStrings As Boolean,
                          Optional delimiter As String = ",") As String
        Dim fieldValues = source.ItemArray

        If quoteStrings Then
            'Wrap any String values in double-quotes and also escape any double-quotes in the String with another double-quote.
            fieldValues = fieldValues.Select(Function(o) If(TypeOf o Is String,
                                                            """" & CStr(o).Replace("""", """""") & """",
                                                            o)).
                                      ToArray()
        End If

        Return String.Join(delimiter, fieldValues)
    End Function

    Public Shared Function WriteCSV(ByVal input As String) As String
        Try
            If (input Is Nothing) Then
                Return String.Empty
            End If

            Dim containsQuote As Boolean = False
            Dim containsComma As Boolean = False
            Dim containsCommaUnknown As Boolean = False
            Dim len As Integer = input.Length
            Dim i As Integer = 0
            Do While ((i < len) _
                        AndAlso ((containsComma = False) _
                        OrElse (containsQuote = False)))
                Dim ch As Char = input(i)


                If (ch = Microsoft.VisualBasic.ChrW(34)) Then
                    containsQuote = True
                ElseIf (ch = Microsoft.VisualBasic.ChrW(44)) Then
                    containsComma = True
                End If

                i = (i + 1)
            Loop

            If (containsQuote AndAlso containsComma) Then
                input = input.Replace("""", """""") ' And If(containsCommaUnknown, """"" + input + ", "")
            Else
                Return input
            End If

        Catch ex As System.Exception
            Throw
        End Try

    End Function
    '   Generic Function to format String  By string as expected by ISO8583 File.
    Public Function Format_StringAs(ByVal STR As String, ByVal length As Integer, ByVal align As Integer)
        Dim i As Integer
        Dim NoOfSpaces As Integer
        Dim temp As String

        ' Inputs:   STR         -----   String to be Formatted.
        '           length      -----   Length of String After Format.
        '           align       -----   Alignment Required. Decided Left Padding / Right Padding.

        If STR = "" Then    ' No String To Format. Return as many spaces as requested.
            Format_StringAs = Space(length)
            Exit Function
        End If

        temp = ""
        If length <= Len(STR) Then
            Format_StringAs = Left(STR, length)
        Else
            If align = RALIGN Then
                NoOfSpaces = length - Len(STR)
                For i = 1 To NoOfSpaces
                    temp = temp & " "
                Next i
                Format_StringAs = STR & temp
            ElseIf align = LALIGN Then
                For i = 1 To length
                    temp = temp & "0"
                Next i
                Format_StringAs = Format((STR), temp)
            Else
                Format_StringAs = STR
            End If
        End If
    End Function

    '   Generic Function to format String as expected by ISO8583 File.
    Public Function Format_StrNumber(ByVal STR As String, ByVal length As Integer, ByVal align As Integer)
        Dim i As Integer
        Dim NoOfSpaces As Integer
        Dim temp As String

        ' Inputs:   STR         -----   String to be Formatted.
        '           length      -----   Length of String After Format.
        '           align       -----   Alignment Required. Decided Left Padding / Right Padding.

        If STR = "" Then    ' No String To Format. Return as many spaces as requested.
            For i = 1 To length
                Format_StrNumber = Format_StrNumber.ToString + "0".ToString()
            Next
            Exit Function

        End If

        temp = ""
        If length <= Len(STR) Then
            Format_StrNumber = Left(STR, length)
        Else
            If align = RALIGN Then
                NoOfSpaces = length - Len(STR)
                For i = 1 To NoOfSpaces
                    temp = temp & "0"
                Next i
                Format_StrNumber = STR & temp
            ElseIf align = LALIGN Then
                For i = 1 To length
                    temp = temp & "0"
                Next i
                Format_StrNumber = Format(CDbl(STR), temp)
            Else
                Format_StrNumber = STR
            End If
        End If
    End Function

    '   Generic Function to Validate the Data Type is as Expected by the ISO8583 File.
    Function CheckDataEntry(ByVal DataType As Integer, ByVal Data As String) As Boolean
        If DataType = NUMBERTYPE Then
            If Not IsNumeric(Data) Then
                CheckDataEntry = False
                Exit Function
            End If
        ElseIf DataType = DATETYPE Then
            If Not IsDate(Data) Then
                CheckDataEntry = False
                Exit Function
            End If
            If (InStr(1, Data, "-") = 0 And InStr(1, Data, "/") = 0) Then
                CheckDataEntry = False
                Exit Function
            End If
            If Len(Data) <> 10 Then
                CheckDataEntry = False
                Exit Function
            End If
        End If
        CheckDataEntry = True
    End Function
    '   Generic Function to Format Dates as expected by the ISO8583 File.
    Function DateFormat(ByVal InDate As String) As String
        DateFormat = Right(InDate, 4) & Mid(InDate, 4, 2) & Left(InDate, 2)
    End Function
    '   Function to check for Presence of the XML Special Characters in the Excel Contents.
    '   Returns "" if No XML Chars found, else returns the Character itself
    Function CheckXMLChars(ByVal InString As String) As String
        If (InStr(1, InString, "<") <> 0) Then
            CheckXMLChars = "<"
            Exit Function
        ElseIf (InStr(1, InString, ">") <> 0) Then
            CheckXMLChars = ">"
            Exit Function
        ElseIf (InStr(1, InString, "&") <> 0) Then
            CheckXMLChars = "&"
            Exit Function
        ElseIf (InStr(1, InString, "'") <> 0) Then
            CheckXMLChars = "'"
            Exit Function
        ElseIf (InStr(1, InString, """") <> 0) Then
            CheckXMLChars = """"
            Exit Function
        End If
        CheckXMLChars = ""
        Exit Function
    End Function
    ' ---- END unchanged file generation ----

    ' =====================================================================
    ' Shared Request-page helpers (same code in every rewritten Request page)
    ' =====================================================================
    Private Const MainDb As String = "EazyConnect"
    Private Const CoreDb As String = "EazyConnectSource"

    ''' <summary>Signed-in officer: the LogonUserID cookie is "user?branch".</summary>
    Private Function CurrentUser() As String
        Dim cookie As HttpCookie = Request.Cookies("LogonUserID")
        Dim luser As String = If(cookie IsNot Nothing AndAlso Not String.IsNullOrEmpty(cookie.Value), cookie.Value, If(User.Identity.Name, ""))
        Dim split As Integer = luser.LastIndexOf("?"c)
        Dim name As String = If(split >= 0, luser.Substring(0, split), luser)
        Return If(name = "", "system", name)
    End Function

    ''' <summary>Branch of the signed-in officer (the part after "?" in the cookie).</summary>
    Private Function CurrentBranch() As String
        Dim cookie As HttpCookie = Request.Cookies("LogonUserID")
        Dim luser As String = If(cookie IsNot Nothing AndAlso Not String.IsNullOrEmpty(cookie.Value), cookie.Value, If(User.Identity.Name, ""))
        Dim split As Integer = luser.LastIndexOf("?"c)
        Return If(split >= 0, luser.Substring(split + 1), "")
    End Function

    Private Shared Function Cfg(ByVal name As String) As String
        Dim s As ConnectionStringSettings = ConfigurationManager.ConnectionStrings(name)
        Return If(s Is Nothing, "", s.ConnectionString)
    End Function

    Private Shared Function SqlText(ByVal value As String) As String
        Return If(value, "").Replace("'", "''")
    End Function

    Private Shared Sub Audit(ByVal code As String, ByVal message As String, ByVal actor As String)
        Classes.RecordOp(String.Format("Sproc_AddAuditLog '{0}','{1}','{2}',''", code, SqlText(message), SqlText(actor)))
    End Sub

    ''' <summary>Runs a stored procedure on the main database.</summary>
    Private Shared Sub Exec(ByVal procedure As String, ByVal ParamArray parameters As SqlParameter())
        Using conn As New SqlConnection(Cfg(MainDb))
            Using cmd As New SqlCommand(procedure, conn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddRange(parameters)
                conn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    ''' <summary>Runs a query with parameters (never string concatenation) and returns the first table.</summary>
    Private Shared Function Query(ByVal connectionName As String, ByVal sql As String, ByVal ParamArray parameters As SqlParameter()) As DataTable
        Dim table As New DataTable()
        Using conn As New SqlConnection(Cfg(connectionName))
            Using cmd As New SqlCommand(sql, conn)
                cmd.Parameters.AddRange(parameters)
                Using adapter As New SqlDataAdapter(cmd)
                    adapter.Fill(table)
                End Using
            End Using
        End Using
        Return table
    End Function

    Private Shared Function P(ByVal name As String, ByVal value As Object) As SqlParameter
        Return New SqlParameter(name, If(value, DBNull.Value))
    End Function

    Private Shared Function Col(ByVal row As DataRow, ByVal column As String) As String
        If row Is Nothing OrElse Not row.Table.Columns.Contains(column) OrElse row.IsNull(column) Then Return ""
        Return Convert.ToString(row(column))
    End Function

    ''' <summary>Splits "123, 456 789" into a clean list of account numbers.</summary>
    Private Shared Function AccountList(ByVal text As String) As List(Of String)
        Dim result As New List(Of String)()
        For Each part As String In If(text, "").Split(New Char() {","c, ";"c, " "c, Convert.ToChar(13), Convert.ToChar(10), Convert.ToChar(9)}, StringSplitOptions.RemoveEmptyEntries)
            Dim acct As String = part.Trim().Trim("'"c)
            If acct <> "" AndAlso Not result.Contains(acct) Then result.Add(acct)
        Next
        Return result
    End Function

    Private Shared Function TryDate(ByVal value As String, ByRef result As DateTime) As Boolean
        Return DateTime.TryParseExact(If(value, "").Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, result)
    End Function

    ''' <summary>Formats a date for an HTML date input (yyyy-MM-dd).</summary>
    Protected Shared Function DateInput(ByVal value As Object) As String
        If TypeOf value Is DateTime Then Return CType(value, DateTime).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
        Dim d As DateTime
        If DateTime.TryParse(Convert.ToString(value), d) Then Return d.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
        Return ""
    End Function

    Private Sub ShowSuccess(ByVal message As String)
        litResult.Text = HttpUtility.HtmlEncode(message)
        litError.Text = ""
    End Sub

    Private Sub ShowError(ByVal message As String)
        litError.Text = HttpUtility.HtmlEncode(message)
        litResult.Text = ""
    End Sub
End Class
