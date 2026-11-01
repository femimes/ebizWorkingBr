Imports System
Imports NETClass
Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports DevExpress.Web
Imports System.Web.UI
Imports System.Linq
'Imports Microsoft.Office.Interop.Excel

'Imports System.Windows.Forms
Partial Class Mandate_CardExtraction
    Inherits System.Web.UI.Page
    Public luser As String
    Public StrLocation As String
    Public struser As String
    Public guser As String
    Const NUMBERTYPE = 1
    Const STRTYPE = 2
    Const DATETYPE = 3
    ' Constants Used for String Alignment.

    Const LALIGN = 1
    Const RALIGN = 2

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If (IsPostBack = False And IsCallback = False) Then
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
            txtuserID.Text = guser
            'LoadBoxes()
            'LoadGrid()
            txtFilePath.Text = NETClass.Classes.GetDataset("Sproc_GetPref").Tables(0).Rows(0).Item("ExtractionPath").ToString()
            Me.ctrlGrid.PageIndex = 0
        End If

    End Sub


    Protected Sub CheckBox2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chkAll As System.Web.UI.WebControls.CheckBox = CType(sender, System.Web.UI.WebControls.CheckBox)
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



                Dim AccountNo As CheckBox = CType(ctrlGrid.FindRowCellTemplateControl(rowIndex, AccountNoHead, "CheckBox1"), CheckBox)

                AccountNo.Checked = True
            Next
        Else
            Dim rowIndex As Integer
            For rowIndex = 0 To VisibleRowsCount - 1



                'CheckBox ChkFirstName = (CheckBox)ctrlGrid.FindRowCellTemplateControl(rowIndex, columnFirstName, "CheckBox1")
                Dim AccountNo As CheckBox = CType(ctrlGrid.FindRowCellTemplateControl(rowIndex, AccountNoHead, "CheckBox1"), CheckBox)

                AccountNo.Checked = False
            Next
        End If
    End Sub

    Protected Sub ApprOnly_Click(ByVal sender As Object, ByVal e As System.EventArgs)
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
        'Dim AccountNoHead As GridViewDataColumn = CType(sender, GridViewDataColumn)
        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("EazyConnect").ConnectionString

        Dim btnAppr As ASPxButton = CType(sender, ASPxButton)
        Dim Cmd As New SqlCommand, sqlConn As New SqlConnection(strcon), result As SqlParameter
        generateCardFiles(btnAppr.CommandArgument, "", txtFilePath.Text)

        Cmd.CommandText = "Sproc_ApproveCardRequestUpd"
        Cmd.Connection = sqlConn
        Cmd.CommandType = CommandType.StoredProcedure

        sqlConn.Open()
        Cmd.Parameters.Add(New SqlParameter("@RefNo", btnAppr.CommandArgument))
        Cmd.Parameters.Add(New SqlParameter("@ApprovingOfficer", guser))
        Cmd.Parameters.Add(New SqlParameter("@status", 5))


        Cmd.ExecuteNonQuery()


        sqlConn.Close()
        ' Dim AccountNo As CheckBox = CType(ctrlGrid.FindRowCellTemplateControl(rowIndex, AccountNoHead, "CheckBox1"), CheckBox)
        ' Response.Write("CardExtraction.aspx")
    End Sub




    Protected Sub BtnApprove_Click(ByVal sender As Object, ByVal e As System.EventArgs)
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
        Dim AccountNoHead As GridViewDataColumn = CType(ctrlGrid.Columns("RequestTitle"), GridViewDataColumn)
        'Dim AccountNoHead As GridViewDataColumn = CType(sender, GridViewDataColumn)
        '  Dim FolderPathName As String = Me.FileUpload1.PostedFile.FileName 'Path.GetDirectoryName(Me.FileUpload1.PostedFile.FileName)
        'Response.Write(FolderPathName)






        Dim VisibleRowsCount As Integer = 0

        If ((ctrlGrid.VisibleRowCount / ctrlGrid.SettingsPager.PageSize) > 1) Then
            VisibleRowsCount = ctrlGrid.SettingsPager.PageSize
        Else
            VisibleRowsCount = ctrlGrid.VisibleRowCount
        End If
        Dim rowIndex As Integer

        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("EazyConnect").ConnectionString

        For rowIndex = 0 To VisibleRowsCount - 1

            Dim AccountNo As System.Web.UI.WebControls.CheckBox = CType(ctrlGrid.FindRowCellTemplateControl(rowIndex, AccountNoHead, "CheckBox1"), System.Web.UI.WebControls.CheckBox)

            If AccountNo.Checked = True Then
                Dim Cmd As New SqlCommand, sqlConn As New SqlConnection(strcon), result As SqlParameter
                generateCardFiles(ctrlGrid.GetRowValues(rowIndex, "RefNo"), "", txtFilePath.Text)

                Cmd.CommandText = "Sproc_ApproveCardRequestUpd"
                Cmd.Connection = sqlConn
                Cmd.CommandType = CommandType.StoredProcedure

                sqlConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@RefNo", ctrlGrid.GetRowValues(rowIndex, "RefNo")))
                Cmd.Parameters.Add(New SqlParameter("@ApprovingOfficer", guser))
                Cmd.Parameters.Add(New SqlParameter("@status", 5))


                Cmd.ExecuteNonQuery()


                sqlConn.Close()

            End If
        Next
        'Dim strdipl As String = String.Format("<script>alert('{0}') </script>", FolderPathName)
        '  Response.Write("CardExtraction.aspx")
        ' Response.Write("<script>alert('Extraction Completed Successfully') </script>")
        ctrlGrid.DataBind()
    End Sub


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
    '   Main Function in the Macro.


    Protected Sub btnDrop_Click(ByVal sender As Object, ByVal e As System.EventArgs)
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
        Dim AccountNoHead As GridViewDataColumn = CType(ctrlGrid.Columns("RequestTitle"), GridViewDataColumn)
        'Dim AccountNoHead As GridViewDataColumn = CType(sender, GridViewDataColumn)
        '  Dim FolderPathName As String = Me.FileUpload1.PostedFile.FileName 'Path.GetDirectoryName(Me.FileUpload1.PostedFile.FileName)
        'Response.Write(FolderPathName)






        Dim VisibleRowsCount As Integer = 0

        If ((ctrlGrid.VisibleRowCount / ctrlGrid.SettingsPager.PageSize) > 1) Then
            VisibleRowsCount = ctrlGrid.SettingsPager.PageSize
        Else
            VisibleRowsCount = ctrlGrid.VisibleRowCount
        End If
        Dim rowIndex As Integer

        Dim strcon As String = System.Configuration.ConfigurationManager.ConnectionStrings("EazyConnect").ConnectionString

        For rowIndex = 0 To VisibleRowsCount - 1

            Dim AccountNo As CheckBox = CType(ctrlGrid.FindRowCellTemplateControl(rowIndex, AccountNoHead, "CheckBox1"), CheckBox)

            If AccountNo.Checked = True Then
                Dim Cmd As New SqlCommand, sqlConn As New SqlConnection(strcon), result As SqlParameter
                'generateCardFiles(ctrlGrid.GetRowValues(rowIndex, "RefNo"), "", txtFilePath.Text)

                Cmd.CommandText = "Sproc_ApproveCardRequestUpd"
                Cmd.Connection = sqlConn
                Cmd.CommandType = CommandType.StoredProcedure

                sqlConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@RefNo", ctrlGrid.GetRowValues(rowIndex, "RefNo")))
                Cmd.Parameters.Add(New SqlParameter("@ApprovingOfficer", guser))
                Cmd.Parameters.Add(New SqlParameter("@status", 99))


                Cmd.ExecuteNonQuery()


                sqlConn.Close()

            End If
        Next
        'Dim strdipl As String = String.Format("<script>alert('{0}') </script>", FolderPathName)
        Response.Write("CardExtraction.aspx")
        ' Response.Write("<script>alert('Extraction Completed Successfully') </script>")
        ctrlGrid.DataBind()
    End Sub
End Class
