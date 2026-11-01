Imports System
Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Imports System.Configuration.ConfigurationManager
Imports System.Text
Imports System.Math
Imports Microsoft.VisualBasic
Imports Microsoft


Namespace NETClass
    'Imports NETClass  .Classes
    Public Class EazyClass
        Public Shared gUtils As New Classes()

        Public Shared Function MiniExist(ByVal mini As String, ByVal iclass As String) As String
            Return Classes.GetField("SELECT mini_account_no FROM ChartCodes WHERE mini_account_no = '" & mini & "' AND item_class = '" & iclass & "'")



        End Function

        Public Shared Function GetInstrumentSize(ByVal InstrChargeCode As String) As Integer
            Return gUtils.GetField("SELECT max_leaflets FROM InstrumentsCharges WHERE instr_charge_code = '" & InstrChargeCode & "'")
        End Function

        Public Shared Function GetDefaultTitleCode() As String
            Return gUtils.GetField("SELECT path_string FROM Paths WHERE path_item = 18")
        End Function

        Public Shared Function GetOfficersReportFile(ByVal rtype As Integer) As String
            Return gUtils.GetField("SELECT report_file FROM OfficersReportTypes WHERE report_code = " & rtype & "")
        End Function

        Public Shared Function GetRefLoanAcct(ByVal refno As String) As String
            Return gUtils.GetField("SELECT account_no FROM TransTemp WHERE ref_no = '" & refno & "' AND account_no IN (SELECT account_no FROM VwAccounts WHERE prod_type_code = '005')")
        End Function

        Public Shared Function GetSupplierName(ByVal supplierid As String) As String
            Return gUtils.GetField("SELECT supplier_name FROM FASuppliers WHERE supplier_id = '" & supplierid & "'")
        End Function

        Public Shared Function GetTransReason(ByVal refno As String) As String
            Return gUtils.GetField("SELECT reason FROM TransReasons WHERE ref_no = '" & refno & "'")
        End Function

        Public Shared Function GetCustomerName(ByVal custno As String) As String
            Return gUtils.GetField("SELECT names FROM Customers WHERE cust_no = '" & custno & "'")
        End Function

        Public Shared Function GetAssetsClasses() As DataSet
            Return gUtils.GetDataset("SELECT * FROM FAClasses")
        End Function

        Public Shared Function GetAllOfficers() As DataSet
            Return gUtils.GetDataset("SELECT offr_code,offr_names,mkt_unit_desc FROM VwOfficers")
        End Function

        Public Shared Function GetFrTypeItemDetails(ByVal frtypecode As String, ByVal mini As String, ByVal baltype As String) As DataSet
            Return gUtils.GetDataset("SELECT * FROM VwFrMapper WHERE frtype_code = '" & frtypecode & "' AND mini_account_no = '" & mini & "' AND bal_type = '" & baltype & "'")
        End Function

        Public Shared Function GetGLAccounts(ByVal headsubcode As String) As DataSet
            Return gUtils.GetDataset("SELECT account_no AS [Account No],description AS [Description] FROM Accounts WHERE prod_code = 'ZZZZZ' AND flexi = 0 ORDER BY account_no")
        End Function

        Public Shared Function GetSuppliers() As DataSet
            Return gUtils.GetDataset("SELECT * FROM FASuppliers ORDER BY supplier_name")
        End Function

        Public Shared Function GetLoanGroups() As DataSet
            Return gUtils.GetDataset("SELECT * FROM LoanGroups ORDER BY loan_group_name")
        End Function

        Public Shared Function GetShareBatchEntry(ByVal seqno As Integer) As DataSet
            Return gUtils.GetDataset("SELECT * FROM ShareTransTemp WHERE seq_no = " & seqno & "")
        End Function

        Public Shared Function DeleteShareBatchEntry(ByVal seqno As Integer)
            Return gUtils.GetDataset("DELETE FROM ShareTransTemp WHERE seq_no = " & seqno & "")
        End Function

        Public Shared Function DeleteShareBatch(ByVal refno As String)
            Return gUtils.GetDataset("DELETE FROM ShareTransRefs WHERE ref_no = '" & refno & "'")
        End Function

        Public Shared Function GetSharePayMethods() As DataSet
            Return gUtils.GetDataset("SELECT * FROM SharePayMethods")
        End Function

        Public Shared Function GetShareBatches(ByVal userid As String) As DataSet
            Return gUtils.GetDataset("SELECT * FROM ShareTransRefs WHERE status = 'E' AND added_by = '" & userid & "'")
        End Function

        Public Shared Function GetShareTransCodes() As DataSet
            Return gUtils.GetDataset("SELECT * FROM ShareTransCodes WHERE form_display = 1")
        End Function

        Public Shared Function GetLoanBookCode() As DataSet
            Return gUtils.GetDataset("SELECT * FROM TransCodes WHERE trans_code = '721'")
        End Function

        Public Shared Function GetAsset(ByVal assetid As String) As DataSet
            Return gUtils.GetDataset("SELECT * FROM FAMaster WHERE asset_id = '" & assetid & "'")
        End Function


        Public Shared Function GetSharesTransRefs() As DataSet
            Return gUtils.GetDataset("SELECT * FROM ShareTransRefs")
        End Function


        Public Shared Function GetDividendRefs() As DataSet
            Return gUtils.GetDataset("SELECT * FROM DividendRefs")
        End Function

        Public Shared Function GetCustomer(ByVal custno As String) As DataSet
            Return gUtils.GetDataset("SELECT * FROM Customers WHERE cust_no = '" & custno & "'")
        End Function

        Public Shared Function GetAssetsLocations() As DataSet
            Return gUtils.GetDataset("SELECT * FROM FALocations")
        End Function

        Public Shared Function GetBizUnits() As DataSet
            Return gUtils.GetDataset("SELECT * FROM BizUnits")
        End Function

        Public Shared Function GetAssetClass(ByVal assetclasscode As String) As DataSet
            Return gUtils.GetDataset("SELECT * FROM FAClasses WHERE asset_class_code = '" & assetclasscode & "'")
        End Function

        Public Shared Function GetLoanClasses() As DataSet
            Return gUtils.GetDataset("SELECT * FROM LoanClasses")
        End Function

        Public Shared Function GetLoanRepayFreqs() As DataSet
            Return gUtils.GetDataset("SELECT * FROM Frequencies WHERE freq_code IN ('W','M','Q','Y')")
        End Function

        Public Shared Function CanViewProtected(ByVal userid As String) As Boolean
            Return gUtils.GetBitField("SELECT view_protected FROM AppUsers WHERE user_id = '" & userid & "'")
        End Function

        Public Shared Function GetMaxLeaflets(ByVal instrcode As String) As Integer
            Return gUtils.GetField("SELECT max_leaflets FROM InstrumentsCharges WHERE instr_charge_code = '" & instrcode & "'")
        End Function

        Public Shared Function LoansWithoutSchedulesCount() As Integer
            Return gUtils.GetField("SELECT COUNT(*) FROM Accounts WHERE type = 'C' AND scheduled = 1 AND book_bal <> 0 AND account_no NOT IN (SELECT account_no FROM LoanSchedules)")
        End Function

        Public Shared Function AccountPDOCount(ByVal account As String, ByVal postdate As Date) As Integer
            Return gUtils.GetField("SELECT COUNT(*) FROM LoanSchedules WHERE account_no = '" & account & "' AND post_date < '" & postdate.Date & "'")
        End Function

        Public Shared Function GetTellerUnapprovedPayments(ByVal postedby As String) As Decimal
            Return gUtils.GetField("SELECT ISNULL(SUM(amount),0) FROM VwTransCash WHERE trans = 'D' AND cash = 1 AND status = 'E' AND reversed = 0 AND posted_by = '" & postedby & "' AND post_date = current_post_date")
        End Function

        Public Shared Function GetScheduleTotal(ByVal account As String, ByVal dealno As String) As Decimal
            Return gUtils.GetField("SELECT ISNULL(SUM(amount),0) FROM LoanSchedules WHERE account_no = '" & account & "' AND deal_no = '" & dealno & "'")
        End Function

        Public Shared Function GetLoanChargeAmountApproved(ByVal dealno As String) As Decimal
            Return gUtils.GetField("SELECT ISNULL(amount_approved,0) FROM ODLogs WHERE od_ref_no = '" & dealno & "'")
        End Function

        Public Shared Function GetScheduleStartDate(ByVal account As String, ByVal dealno As String) As Date
            Return gUtils.GetField("SELECT MIN(post_date) FROM LoanSchedules WHERE account_no = '" & account & "' AND deal_no = '" & dealno & "'")
        End Function

        Public Shared Function GetNormalClearingTypes() As DataSet
            Return gUtils.GetDataset("SELECT * FROM ClearingTypes WHERE fixed = 1")
        End Function

        Public Shared Function GetAllHeads() As DataSet
            Return gUtils.GetDataset("SELECT head_code,head_name FROM Heads ORDER BY head_name")
        End Function

        Public Shared Function GetLoanApprovingBody(ByVal accountno As String, ByVal dealno As String) As String
            Return gUtils.GetField("SELECT body_code FROM DealsTrack WHERE account_no = '" & accountno & "' AND deal_no = '" & dealno & "' AND trans_code = '721'")
        End Function

        Public Shared Function GetAllCentres() As DataSet
            Return gUtils.GetDataset("SELECT cons_centre_key,cons_centre_name FROM ConsolCentres ORDER BY cons_centre_name")
        End Function

        Public Shared Function GetReportListByCentres(ByVal consumer As Integer, ByVal centre As Integer) As DataSet
            Return gUtils.GetDataset("SELECT obj_no,obj_desc FROM AppObjects WHERE obj_type = 'R' AND consumer = " & CInt(consumer) & " AND centre_type = " & CInt(centre) & " ORDER BY obj_desc")
        End Function

        Public Shared Function GetContIntAccounts() As DataSet
            Return gUtils.GetDataset("SELECT * FROM ChartCodes WHERE item_type = 'C' AND account_type = 'N' AND cust_int_cont = 1 ORDER BY mini_account_no")
        End Function


        Public Shared Function GetRetailProducts() As DataSet
            Return gUtils.GetDataset("SELECT * FROM Products WHERE attr_code = 'R'")
        End Function

        Public Shared Function GetHeadSubs() As DataSet
            Return gUtils.GetDataset("SELECT * FROM HeadsAndSubs")
        End Function

        Public Shared Function GetNonDefaultHeadSubs() As DataSet
            Return gUtils.GetDataset("SELECT * FROM HeadsAndSubs WHERE headsub_code <> '0000'")
        End Function

        Public Shared Function GetFrozenAmount(ByVal seqno As Integer) As String
            Return gUtils.GetField("SELECT new_value FROM AccountsMaintLog WHERE seq_no = " & seqno & "")
        End Function

        Public Shared Function GetControlName(ByVal budgetcode As String) As String
            Return gUtils.GetField("SELECT budget_desc FROM BudgetControls WHERE budget_code = '" & budgetcode & "'")
        End Function

        Public Shared Function AccountProductDesc(ByVal account As String) As String
            Return gUtils.GetField("SELECT prod_desc FROM VwAccounts WHERE account_no = '" & account & "'")
        End Function

        Public Shared Function GetTransRefTransDesc(ByVal refno As String) As String
            Return gUtils.GetField("SELECT DISTINCT(trans_desc) FROM VwTransTemp WHERE ref_no = '" & refno & "'")
        End Function

        Public Shared Function TransRequiresApproval(ByVal transcode As String) As Boolean
            Return gUtils.GetBitField("SELECT requires_approval FROM TransCodes WHERE trans_code = '" & transcode & "'")
        End Function

        Public Shared Function DraftTrans(ByVal transcode As String) As Boolean
            Return gUtils.GetBitField("SELECT draft_trans FROM TransCodes WHERE trans_code = '" & transcode & "'")
        End Function

        Public Shared Function GetDraftAccount(ByVal transcode As String, ByVal headsubcode As String) As String
            GetDraftAccount = gUtils.GetField("SELECT default_main FROM TransCodes WHERE trans_code = '" & transcode & "'")
            GetDraftAccount = gUtils.GetField("SELECT account_no FROM Accounts WHERE mini_account_no = '" & GetDraftAccount & "' And headsub_code = '" & headsubcode & "'")
        End Function

        Public Shared Function AllowSatBanking() As Boolean
            Return gUtils.GetBitField("SELECT sat_banking FROM ParamPreferences WHERE rec_id = '1'")
        End Function

        Public Shared Function AllowSunBanking() As Boolean
            Return gUtils.GetBitField("SELECT sun_banking FROM ParamPreferences WHERE rec_id = '1'")
        End Function

        Public Shared Function GetAccountNumber(ByVal headsub As String, ByVal prod As String, ByVal custno As String) As String
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("EazyConnect").ConnectionString

            Cmd.CommandText = "Sproc_GenerateAccountNumbers"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try

                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@HeadSubCode", headsub))
                Cmd.Parameters.Add(New SqlParameter("@ProdCode", prod))
                Cmd.Parameters.Add(New SqlParameter("@CustNo", custno))
                result = Cmd.Parameters.Add(New SqlParameter("@NewNumber", SqlDbType.VarChar, 20))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return result.Value
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function AddTransReason(ByVal refno As String, ByVal reason As String) As String

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter
            Dim str As String = System.Configuration.ConfigurationManager.ConnectionStrings("EazyConnect").ConnectionString

            OleConn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("EazyConnect").ConnectionString

            Cmd.CommandText = "Sproc_AddTransReason"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@RefNo", refno))
                Cmd.Parameters.Add(New SqlParameter("@Reason", reason))

                Cmd.ExecuteNonQuery()
                OleConn.Close()
            Catch Exc As Exception
                Return Exc.Message
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function ControlHasAccountsWithBalance(ByVal account As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT account_no FROM Accounts WHERE control_account_no = '" & account & "' AND book_bal <> 0")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function IsUserCashAccount(ByVal userid As String, ByVal account As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT teller_account_no FROM AppUsers WHERE user_id = '" & userid & "' AND teller_account_no = '" & account & "'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function UserCanPrintStmt(ByVal userid As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT user_id FROM AppUsers WHERE user_id = '" & userid & "' AND print_stmt = 1")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function OldAccountExist(ByVal account As String, ByVal oldaccount As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT account_no FROM Accounts WHERE account_no <> '" & account & "' AND old_account_no = '" & oldaccount & "'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function AccountOperativeExist(ByVal account As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT account_no FROM Accounts WHERE settle_acct = '" & account & "' AND book_bal <> 0")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function ScheduleGenerated(ByVal account As String, ByVal dealno As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT account_no FROM LoanSchedules WHERE account_no = '" & account & "' AND deal_no = '" & dealno & "'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function ShareTransTempExist(ByVal custno As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT cust_no FROM ShareTransTemp WHERE cust_no = '" & custno & "'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function ShareTransHistExist(ByVal custno As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT cust_no FROM ShareTransHist WHERE cust_no = '" & custno & "'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function UserTransEntryExist(ByVal seqno As Integer, ByVal userid As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT account_no FROM TransHist WHERE seq_no = " & CInt(seqno) & " AND posted_by = '" & userid & "'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function ProdTypeAttrMatch(ByVal prodtype As String, ByVal attr As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT prod_type_code FROM ProductsTypesAttributes WHERE prod_type_code = '" & prodtype & "' AND attr_code = '" & attr & "'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function IsPublicHoliday(ByVal pdate As Date) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT holiday_desc FROM Holidays WHERE holiday_date = '" & pdate.Date & "'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function EODDateOk() As Boolean
            Dim postdate As Date, lastpostdate As Date

            postdate = gUtils.GetField("SELECT post_date FROM ParamPreferences WHERE rec_id = '1'")
            lastpostdate = gUtils.GetField("SELECT last_post_date FROM ParamPreferences WHERE rec_id = '1'")

            If postdate > lastpostdate Then
                Return True
            Else
                Return False
            End If
        End Function
        Public Shared Function EOMDateOk() As Boolean
            Dim postdate As Date, lastpostdate As Date

            postdate = gUtils.GetField("SELECT post_date FROM ParamPreferences WHERE rec_id = '1'")
            lastpostdate = gUtils.GetField("SELECT period_date FROM ParamPreferences WHERE rec_id = '1'")

            If postdate = lastpostdate Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function EOMOn() As Boolean
            Return gUtils.GetBitField("SELECT eom_run FROM ParamPreferences WHERE rec_id = '1'")
        End Function

        Public Shared Function SODDateOk() As Boolean
            Dim postdate As Date, lastpostdate As Date

            postdate = Classes.GetField("SELECT post_date FROM ParamPreferences WHERE rec_id = '1'")
            lastpostdate = gUtils.GetField("SELECT last_post_date FROM ParamPreferences WHERE rec_id = '1'")

            If postdate = lastpostdate Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function GetMaintFlag(ByVal mainttype As String) As String
            Return gUtils.GetField("SELECT account_maint_flag FROM AccountsMaintTypes WHERE account_maint_type = '" & mainttype & "'")
        End Function

        Public Shared Function IsJournalPosted(ByVal refno As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT ref_no FROM Journals WHERE ref_no = '" & refno & "' AND status = 'P'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function TransHistExist(ByVal refno As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT ref_no FROM TransHist WHERE ref_no = '" & refno & "'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function InvalidTransStatus(ByVal refno As String) As Boolean
            If TransHistExist(refno) = False Then
                If GetTransRefStatus(refno) <> "E" Then
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        End Function

        Public Shared Function IsProxyHead(ByVal headcode As String) As Boolean
            Return gUtils.GetBitField("SELECT proxy FROM Heads WHERE head_code = '" & headcode & "'")
        End Function

        Public Shared Function IsCustomerAccountProtected(ByVal custno As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT account_no FROM Accounts WHERE cust_no = '" & custno & "' AND protected = 1")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function IsProxyHeadSub(ByVal headsubcode As String) As Boolean
            Return gUtils.GetBitField("SELECT proxy FROM VwHeadsAndSubs WHERE headsub_code = '" & headsubcode & "'")
        End Function

        Public Shared Function GetProxyAccount(ByVal headsubcode As String) As String
            Return gUtils.GetField("SELECT proxy_account_no FROM VwHeadsAndSubs WHERE headsub_code = '" & headsubcode & "'")
        End Function

        Public Shared Function IsLoanScheduleApproved(ByVal account As String, ByVal loanno As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT account_no FROM LoanSchedules WHERE account_no = '" & account & "' AND deal_no = '" & loanno & "' AND status = 'A'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function
        Public Shared Function ControlHasBalance(ByVal account As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT account_no FROM Accounts WHERE account_no = '" & account & "' AND book_bal <> 0")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function UnpostedDealExist(ByVal account As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT account_no FROM DealsTrack WHERE account_no = '" & account & "' AND status <> 'P' AND status <> 'R' And reversed = 0")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function GetDealType(ByVal transcode As String) As String
            Return gUtils.GetField("SELECT type FROM DealsTypes WHERE trans_code = '" & transcode & "'")
        End Function

        Public Shared Function GetDealSubType(ByVal transcode As String) As String
            Return gUtils.GetField("SELECT subtype FROM DealsTypes WHERE trans_code = '" & transcode & "'")
        End Function

        Public Shared Function GetTransRefStatus(ByVal transref As String) As String
            Return gUtils.GetField("SELECT status FROM TransRefs WHERE ref_no = '" & transref & "'")
        End Function

        Public Shared Function GetClientCode() As String
            Return gUtils.GetField("SELECT client_code FROM ClientInfo WHERE rec_id = '1'")
        End Function

        Public Shared Function GetClearTypeDays(ByVal clearcode As String) As Integer
            Return gUtils.GetField("SELECT days FROM ClearingTypes WHERE clearing_code = '" & clearcode & "'")
        End Function

        Public Shared Function GetClearType(ByVal clearcode As String) As Boolean
            Return gUtils.GetBitField("SELECT fixed FROM ClearingTypes WHERE clearing_code = '" & clearcode & "'")
        End Function

        Public Shared Function GetSiteAccount(ByVal mini As String, ByVal headsubcode As String) As String
            Return gUtils.GetField("SELECT account_no FROM Accounts WHERE mini_account_no = '" & mini & "' AND headsub_code = '" & headsubcode & "'")
        End Function

        Public Shared Function BudgetCodeExist(ByVal budgetcode As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT budget_code FROM BudgetControls WHERE budget_code = '" & budgetcode & "'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function TransNotApproved(ByVal refno As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT ref_no FROM TransRefs WHERE ref_no = '" & refno & "' AND (status = 'E' Or status = 'R') ")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function SetBudgetAmount(ByVal seqno As Integer, ByVal amount As Decimal)
            Dim result As String
            result = gUtils.RecordOp("UPDATE BudgetHist SET budget_amount = " & amount & " WHERE seq_no = " & seqno & "")
        End Function

        Public Shared Function SetLoanRepaymentAmount(ByVal seqno As Integer, ByVal amount As Decimal, ByVal simulate As Integer)
            Dim result As String

            If simulate = 0 And amount >= 0 Then
                result = gUtils.RecordOp("UPDATE LoanSchedules SET amount = " & amount & " WHERE seq_no = " & seqno & "")
            End If

            If simulate = 1 And amount >= 0 Then
                result = gUtils.RecordOp("UPDATE LoanSchedulesSim SET amount = " & amount & " WHERE seq_no = " & seqno & "")
            End If
        End Function

        Public Shared Function LoanRepaymentMaintUpdate(ByVal account As String, ByVal dealno As String, ByVal postedby As String, ByVal simulate As Integer)
            Dim result As String

            If simulate = 0 Then
                result = gUtils.RecordOp("UPDATE LoanSchedules SET status = 'E',posted_by = '" & postedby & "' WHERE account_no = '" & account & "' AND deal_no = '" & dealno & "'")
            End If

            If simulate = 1 Then
                result = gUtils.RecordOp("UPDATE LoanSchedulesSim SET posted_by = '" & postedby & "' WHERE ref_no = '" & account & "'")
            End If

        End Function

        Public Shared Function RestoreBudgetAmount(ByVal account As String, ByVal type As String, ByVal startdate As Date, ByVal enddate As Date)
            Dim result As String

            If type = "D" Then
                result = gUtils.RecordOp("UPDATE BudgetHist SET budget_amount = budget_prev_day WHERE account_no = '" & account & "' AND post_date BETWEEN '" & startdate & "' AND '" & enddate & "'")
            ElseIf type = "M" Then
                result = gUtils.RecordOp("UPDATE BudgetHist SET budget_amount = budget_prev_month WHERE account_no = '" & account & "' AND post_date BETWEEN '" & startdate & "' AND '" & enddate & "'")
            ElseIf type = "Z" Then
                result = gUtils.RecordOp("UPDATE BudgetHist SET budget_amount = 0 WHERE account_no = '" & account & "' AND post_date BETWEEN '" & startdate & "' AND '" & enddate & "'")
            End If
        End Function

        Public Shared Function GetBudgetTotal(ByVal account As String, ByVal startdate As Date, ByVal enddate As Date) As Decimal
            Return gUtils.GetField("SELECT ISNULL(SUM(budget_amount),0) FROM BudgetHist WHERE account_no = '" & account & "' AND post_date BETWEEN '" & startdate & "' AND '" & enddate & "'")
        End Function

        Public Shared Function InstrumentStopped(ByVal acct As String, ByVal bank As String, ByVal instrno As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT account_no FROM InstrumentsStopped WHERE account_no = '" & acct & "' AND bank_code = '" & bank & "' AND ('" & instrno & "' BETWEEN start_no AND end_no) AND unstopped = 0")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function InstrumentStopped2(ByVal bank As String, ByVal instrno As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT account_no FROM InstrumentsStopped WHERE bank_code = '" & bank & "' AND ('" & instrno & "' BETWEEN start_no AND end_no) AND unstopped = 0")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function SOTypeMatch(ByVal type As String, ByVal subtype As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT so_type FROM SOSubtypes WHERE so_type = '" & type & "' AND so_subtype = '" & subtype & "'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function ChequeExist(ByVal account As String, ByVal bank As String, ByVal instrno As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT cheque_no FROM ClearingCheques WHERE account_no = '" & account & "' AND bank_code = '" & bank & "' AND instr_no = '" & instrno & "'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function ApprovedChequeExist(ByVal account As String, ByVal bank As String, ByVal instrno As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT cheque_no FROM ClearingCheques WHERE account_no = '" & account & "' AND bank_code = '" & bank & "' AND instr_no = '" & instrno & "' AND approved = 1")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function StopPosting() As Boolean
            Return gUtils.GetBitField("SELECT stop_posting FROM ParamPreferences WHERE rec_id = '1'")
        End Function
        Public Shared Function IsCashTransaction(ByVal refno As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT ref_no FROM TransCash WHERE ref_no = '" & refno & "'")

            If Len(result) <> 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function IsVaultTransaction(ByVal refno As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT ref_no FROM TransVault WHERE ref_no = '" & refno & "'")

            If Len(result) <> 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function TransCodeExist(ByVal transcode As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT trans_code FROM TransCodes WHERE trans_code = '" & transcode & "'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function
        Public Shared Function TransAndProdExist(ByVal transcode As String, ByVal prodcode As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT trans_code FROM TransAndProducts WHERE trans_code = '" & transcode & "' And prod_code = '" & prodcode & "'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function
        Public Shared Function TransCodeLevel(ByVal transcode As String) As Integer
            Dim result As Integer

            result = gUtils.GetField("SELECT loop_level FROM TransCodes WHERE trans_code = '" & transcode & "'")

            Return result
        End Function

        Public Shared Function CustExist(ByVal custno As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT cust_no FROM Customers WHERE cust_no = '" & custno & "' ")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function ShareholderExist(ByVal custno As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT cust_no FROM Shareholders WHERE cust_no = '" & custno & "' ")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function GetBaseCurr() As String
            Return gUtils.GetField("SELECT curr_code FROM ParamPreferences WHERE rec_id = '1'")
        End Function

        Public Shared Function GetAdvanceAccount(ByVal advanceno As String) As String
            Return gUtils.GetField("SELECT account_no FROM Advances WHERE advance_no = '" & advanceno & "'")
        End Function

        Public Shared Function GetJournalTotal(ByVal journal As String, ByVal type As String) As Decimal
            If type = "1" Then
                Return gUtils.GetField("SELECT SUM(debit) FROM TransTemp WHERE ref_no = '" & journal & "'")
            ElseIf type = "2" Then
                Return gUtils.GetField("SELECT SUM(credit) FROM TransTemp WHERE ref_no = '" & journal & "'")
            ElseIf type = "3" Then
                Return gUtils.GetField("SELECT check_total FROM Journals WHERE ref_no = '" & journal & "'")
            ElseIf type = "4" Then
                Return gUtils.GetField("SELECT COUNT(ref_no) FROM TransTemp WHERE ref_no = '" & journal & "'")
            End If
        End Function

        Public Shared Function GetJournalStatus(ByVal journal As String) As String
            Return gUtils.GetField("SELECT status FROM Journals WHERE ref_no = '" & journal & "'")
        End Function

        Public Shared Function GetJournalCompletedStatus(ByVal journal As String) As Integer
            Return gUtils.GetField("SELECT completed FROM Journals WHERE ref_no = '" & journal & "'")
        End Function

        Public Shared Function GetJournalSite(ByVal journal As String) As String
            Return gUtils.GetField("SELECT headsub_code FROM Journals WHERE ref_no = '" & journal & "'")
        End Function

        Public Shared Function GetHeadSubName(ByVal head As String, ByVal suborg As String) As String
            Return gUtils.GetField("SELECT sub_name FROM HeadsAndSubs WHERE head_code = '" & head & "' AND sub_org = '" & suborg & "'")
        End Function

        Public Shared Function GetHeadSubReportsPath(ByVal headsub As String) As String
            Return gUtils.GetField("SELECT reports_path FROM HeadsAndSubs WHERE headsub_code = '" & headsub & "'")
        End Function

        Public Shared Function GetHeadSubImagePath(ByVal headsub As String) As String
            Return gUtils.GetField("SELECT image_path FROM HeadsAndSubs WHERE headsub_code = '" & headsub & "'")
        End Function

        Public Shared Function GetHeadAndSubName(ByVal headsubcode As String) As String
            Return gUtils.GetField("SELECT sub_name FROM HeadsAndSubs WHERE headsub_code = '" & headsubcode & "'")
        End Function

        Public Shared Function GetHeadCode(ByVal headsubcode As String) As String
            Return gUtils.GetField("SELECT head_code FROM HeadsAndSubs WHERE headsub_code = '" & headsubcode & "'")
        End Function

        Public Shared Function GetSubOrgCode(ByVal headsubcode As String) As String
            Return gUtils.GetField("SELECT sub_org FROM HeadsAndSubs WHERE headsub_code = '" & headsubcode & "'")
        End Function

        Public Shared Function GetClientName() As String
            Return gUtils.GetField("SELECT client_name FROM ClientInfo WHERE rec_id = '1'")
        End Function

        Public Shared Function GetFirstSharePostDate(ByVal custno As String) As Date
            Return gUtils.GetFieldDate("SELECT MIN(post_date) FROM ShareTransHist WHERE cust_no = '" & custno & "'")
        End Function

        Public Shared Function GetFirstShareTransDate(ByVal custno As String) As Date
            Return gUtils.GetFieldDate("SELECT MIN(trans_date) FROM ShareTransHist WHERE cust_no = '" & custno & "'")
        End Function

        Public Shared Function GetShareholderInvDate(ByVal custno As String) As Date
            Return gUtils.GetFieldDate("SELECT inv_date FROM Shareholders WHERE cust_no = '" & custno & "'")
        End Function

        Public Shared Function GetPostPeriod() As Date
            Return gUtils.GetFieldDate("SELECT period_date FROM ParamPreferences WHERE rec_id = '1'")
        End Function

        Public Shared Function SearchAccounts(ByVal searchitem As String, ByVal searchfield As String, ByVal searchorder As String, ByVal headsub As String, ByVal dormant As Boolean) As DataSet
            If dormant = True Then
                Return gUtils.GetDataset("Sproc_SearchAllAccountWithDormant '" & searchitem & "','" & searchfield & "','" & searchorder & "','" & headsub & "'")
            Else
                Return gUtils.GetDataset("Sproc_SearchAccount '" & searchitem & "','" & searchfield & "','" & searchorder & "','" & headsub & "'")
            End If
        End Function

        Public Shared Function SearchAllAccounts(ByVal searchitem As String, ByVal searchfield As String, ByVal searchorder As String, ByVal headsub As String) As DataSet
            Return gUtils.GetDataset("Sproc_SearchAllAccount '" & searchitem & "','" & searchfield & "','" & searchorder & "','" & headsub & "'")
        End Function

        Public Shared Function SearchCashAccounts(ByVal headcode As String, ByVal suborg As String, ByVal searchitem As String, ByVal searchfield As String, ByVal searchorder As String) As DataSet
            Return gUtils.GetDataset("Sproc_SearchCashAccount '" & headcode & "','" & suborg & "','" & searchitem & "','" & searchfield & "','" & searchorder & "'")
        End Function

        Public Shared Function SearchCustomer(ByVal searchitem As String, ByVal searchfield As String, ByVal searchorder As String, ByVal headsub As String) As DataSet
            Return gUtils.GetDataset("Sproc_SearchCustomer '" & searchitem & "','" & searchfield & "','" & searchorder & "','" & headsub & "'")
        End Function

        Public Shared Function SearchShareholder(ByVal searchitem As String, ByVal searchfield As String, ByVal searchorder As String) As DataSet
            Return gUtils.GetDataset("Sproc_SearchShareholder '" & searchitem & "','" & searchfield & "','" & searchorder & "'")
        End Function

        Public Shared Function SearchSupplier(ByVal searchitem As String, ByVal searchfield As String, ByVal searchorder As String) As DataSet
            Return gUtils.GetDataset("Sproc_SearchSupplier '" & searchitem & "','" & searchfield & "','" & searchorder & "'")
        End Function

        Public Shared Function SearchLoanBatch(ByVal searchitem As String, ByVal searchfield As String, ByVal searchorder As String) As DataSet
            Return gUtils.GetDataset("Sproc_SearchLoanBatch '" & searchitem & "','" & searchfield & "','" & searchorder & "'")
        End Function

        Public Shared Function GetProductsReportHeads() As DataSet
            Return gUtils.GetDataset("SELECT * FROM ProductsReportHeads")
        End Function

        Public Shared Function SearchAdvance(ByVal headsubcode As String, ByVal searchitem As String, ByVal searchfield As String, ByVal searchorder As String) As DataSet
            Return gUtils.GetDataset("Sproc_SearchAdvance '" & headsubcode & "','" & searchitem & "','" & searchfield & "','" & searchorder & "'")
        End Function

        Public Shared Function IsFlexiAccount(ByVal AccountNo As String) As Boolean
            Return gUtils.GetBitField("SELECT flexi FROM Accounts WHERE account_no = '" & AccountNo & "' AND item_type = 'D' AND account_status = '1'")
        End Function

        Public Shared Function IsScheduledLoan(ByVal AccountNo As String) As Boolean
            Return gUtils.GetBitField("SELECT scheduled FROM Accounts WHERE account_no = '" & AccountNo & "' AND book_bal <> 0 AND scheduled = 1")
        End Function

        Public Shared Function GetAccountOpenDate(ByVal accountno As String) As Date
            Return gUtils.GetField("SELECT post_date FROM Accounts WHERE account_no = '" & accountno & "'")
        End Function

        Public Shared Function GetEffectiveDate(ByVal accountno As String) As Date
            Return gUtils.GetField("SELECT effective FROM Accounts WHERE account_no = '" & accountno & "'")
        End Function

        Public Shared Function GetMaturityDate(ByVal accountno As String) As Date
            Return gUtils.GetField("SELECT maturity FROM Accounts WHERE account_no = '" & accountno & "'")
        End Function

        Public Shared Function GetAccountName(ByVal accountno As String) As String
            Return gUtils.GetField("SELECT description FROM Accounts WHERE account_no = '" & accountno & "'")
        End Function

        Public Shared Function GetLoanBatchName(ByVal loanbatchno As String) As String
            Return gUtils.GetField("SELECT loan_group_name FROM LoanGroups WHERE loan_group_code = '" & loanbatchno & "'")
        End Function

        Public Shared Function GetAccountCurr(ByVal accountno As String) As String
            Return gUtils.GetField("SELECT curr_code FROM Accounts WHERE account_no = '" & accountno & "'")
        End Function

        Public Shared Function GetAccountMandate(ByVal accountno As String) As String
            Return gUtils.GetField("SELECT mandate FROM Accounts WHERE account_no = '" & accountno & "'")
        End Function

        Public Shared Function GetAccountBookBal(ByVal accountno As String) As Decimal
            Return gUtils.GetField("SELECT book_bal FROM Accounts WHERE account_no = '" & accountno & "'")
        End Function

        Public Shared Function GetAccountClearBal(ByVal accountno As String) As Decimal
            Return gUtils.GetField("SELECT clear_bal FROM Accounts WHERE account_no = '" & accountno & "'")
        End Function

        Public Shared Function GetAccountType(ByVal accountno As String) As String
            Return gUtils.GetField("SELECT account_type FROM Accounts WHERE account_no = '" & accountno & "'")
        End Function

        Public Shared Function GetAccountItemType(ByVal accountno As String) As String
            Return gUtils.GetField("SELECT item_type FROM Accounts WHERE account_no = '" & accountno & "'")
        End Function

        Public Shared Function GetAccountProdName(ByVal accountno As String) As String
            Return gUtils.GetField("SELECT prod_desc FROM VwAccounts WHERE account_no = '" & accountno & "'")
        End Function
        Public Shared Function GetAccountTypeDesc(ByVal type As String) As String
            Return gUtils.GetField("SELECT account_type_desc FROM AccountTypes WHERE account_type = '" & type & "'")
        End Function

        Public Shared Function GetCurrRate(ByVal currcode As String) As Decimal
            Return gUtils.GetField("SELECT exch_rate FROM ParamCurrencies WHERE curr_code = '" & currcode & "'")
        End Function

        Public Shared Function GetUserCashAcct(ByVal userid As String) As String
            Return gUtils.GetField("SELECT teller_account_no FROM AppUsers WHERE user_id = '" & userid & "'")
        End Function

        Public Shared Function IsCashAccount(ByVal accountno As String) As Boolean
            Dim result As String
            result = gUtils.GetField("SELECT account_no FROM Accounts WHERE account_no = '" & accountno & "' AND item_type = 'D' AND account_type = 'C' AND account_status = '1'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function IsAdvanceAccount(ByVal accountno As String) As Boolean
            Dim result As String
            result = gUtils.GetField("SELECT account_no FROM Accounts WHERE account_no = '" & accountno & "' AND item_type = 'D' AND account_type = 'A' AND account_status = '1' AND transstatus_code <> '3'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function
        Public Shared Function UserPostIbGl(ByVal userid As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT user_id FROM AppUsers WHERE user_id  = '" & userid & "' AND post_ib_gl <> 0")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function
        Public Shared Function IsPostingAccount(ByVal accountno As String) As Boolean
            Dim result As String, ibmini As String

            ibmini = gUtils.GetField("SELECT mini_account_no FROM ParamAccounts WHERE id = '02'")
            result = gUtils.GetField("SELECT account_no FROM Accounts WHERE account_no = '" & accountno & "' AND item_type = 'D' AND account_status = '1' AND transstatus_code <> '3' AND mini_account_no <> '" & ibmini & "'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function
        Public Shared Function IsDebitAllowed(ByVal accountno As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT account_no FROM Accounts WHERE account_no = '" & accountno & "' AND transstatus_code <> '1'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function IsCreditAllowed(ByVal accountno As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT account_no FROM Accounts WHERE account_no = '" & accountno & "' AND transstatus_code <> '2'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function IsAccountHeadSubOk(ByVal accountno As String, ByVal headsub As String) As Boolean
            Dim result As String
            result = gUtils.GetField("SELECT account_no FROM Accounts WHERE account_no = '" & accountno & "' AND headsub_code = '" & headsub & "'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function
        Public Shared Function InstrumentUsed(ByVal instrno As String, ByVal account As String) As Boolean
            Dim result As String
            result = gUtils.GetField("SELECT instr_no FROM InstrumentsUsed WHERE instr_no = '" & instrno & "' AND account_no = '" & account & "'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function InstrumentRangeUsed(ByVal account As String, ByVal startno As String, ByVal endno As String) As Boolean
            Dim result As String
            result = gUtils.GetField("SELECT instr_no FROM InstrumentsUsed WHERE instr_no BETWEEN '" & startno & "' AND '" & endno & "' AND account_no = '" & account & "'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function InstrumentRangeAccountExist(ByVal account As String, ByVal startno As String, ByVal endno As String) As Boolean
            Dim result As String
            result = gUtils.GetField("SELECT start_no FROM Instruments WHERE account_no = '" & account & "' AND start_no = '" & startno & "' AND end_no = '" & endno & "' AND revoked = 0")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function GetAccountItemClass(ByVal accountno As String) As String
            Return gUtils.GetField("SELECT item_class FROM Accounts WHERE account_no = '" & accountno & "'")
        End Function

        Public Shared Function GetAccountItemClassDesc(ByVal itemclass As String) As String
            Return gUtils.GetField("SELECT item_class_desc FROM ChartTypes WHERE item_class = '" & itemclass & "'")
        End Function
        Public Shared Function GetAccountBudgetCode(ByVal accountno As String) As String
            Return gUtils.GetField("SELECT budget_code FROM Accounts WHERE account_no = '" & accountno & "'")
        End Function

        Public Shared Function GetAccountBudgetDesc(ByVal budgetcode As String) As String
            Return gUtils.GetField("SELECT budget_desc FROM BudgetControls WHERE budget_code = '" & budgetcode & "'")
        End Function

        Public Shared Function GetPayRevRefNum(ByVal account As String, ByVal type As String) As String
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString

            Cmd.CommandText = "Sproc_GenerateTransNum"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@AccountNo", account))
                Cmd.Parameters.Add(New SqlParameter("@Type", type))
                result = Cmd.Parameters.Add(New SqlParameter("@PaymentNum", SqlDbType.VarChar, 30))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return result.Value
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function GetNewNumber(ByVal headsub As String, ByVal prod As String, ByVal type As String) As String
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_GenerateNumbers"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@HeadSubCode", headsub))
                Cmd.Parameters.Add(New SqlParameter("@ProdCode", prod))
                Cmd.Parameters.Add(New SqlParameter("@Type", type))
                result = Cmd.Parameters.Add(New SqlParameter("@NewNumber", SqlDbType.VarChar, 20))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return result.Value
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function GetJournalNum(ByVal headsubcode As String, ByVal journaltype As String) As String
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_GenerateJournalNum"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@HeadSubCode", headsubcode))
                Cmd.Parameters.Add(New SqlParameter("@JournalType", journaltype))
                result = Cmd.Parameters.Add(New SqlParameter("@PaymentNum", SqlDbType.VarChar, 30))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return result.Value
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function GetExtJournalNum(ByVal headsubcode As String) As String
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_GenerateExtJournalNum"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@HeadSubCode", headsubcode))
                result = Cmd.Parameters.Add(New SqlParameter("@PaymentNum", SqlDbType.VarChar, 30))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return result.Value
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function GetTransNum(ByVal num As String, ByVal journaltype As String) As String
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_GenerateTransNum"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@AccountNo", num))
                Cmd.Parameters.Add(New SqlParameter("@Type", journaltype))
                result = Cmd.Parameters.Add(New SqlParameter("@PaymentNum", SqlDbType.VarChar, 50))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return result.Value
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function GetLoanFutureRate(ByVal account As String) As String
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_GetLoanFutureRate"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@AccountNo", account))
                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.VarChar, 100))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return result.Value
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function GetJournalInfo(ByVal journal As String, ByVal item As Integer) As String
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_JournalInfo"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@Journal", journal))
                Cmd.Parameters.Add(New SqlParameter("@InfoItem", item))
                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.VarChar, 100))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return result.Value
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function
        Public Shared Function GetAdvanceStatus(ByVal advanceno As String) As String
            Return gUtils.GetField("SELECT status FROM Advances WHERE advance_no = '" & advanceno & "'")
        End Function

        Public Shared Function GetEntryType(ByVal type As String) As String
            Return gUtils.GetField("SELECT entry_type FROM ApprovalTypes WHERE type = '" & type & "'")
        End Function

        Public Shared Function NegateItem(ByVal itemclass As String) As Integer
            Return gUtils.GetField("SELECT debit_bal FROM ChartTypes WHERE item_class = '" & itemclass & "'")
        End Function

        Public Shared Function AccountBudgetOk(ByVal account As String, ByVal amount As Decimal, ByVal budgetcheck As Integer) As Boolean
            Dim result As Decimal

            If budgetcheck = 0 Then
                Return True
            End If

            If GetAccountItemClass(account) <> "51" Then
                Return True
            End If

            If GetAvailableBal(account) < amount Then
                Return False
            Else
                Return True
            End If
        End Function

        Public Shared Function IsUserRecLimitOk(ByVal userid As String, ByVal amount As Decimal) As Boolean
            Dim result As String

            If amount < 0 Then
                amount = -1 * amount
            End If

            result = gUtils.GetField("SELECT user_id FROM AppUsers WHERE user_id = '" & userid & "' AND min_cash_rec <= " & amount & " AND max_cash_rec >= " & amount & "")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function IsUserPayLimitOk(ByVal userid As String, ByVal amount As Decimal) As Boolean
            Dim result As String

            If amount < 0 Then
                amount = -1 * amount
            End If

            result = gUtils.GetField("SELECT user_id FROM AppUsers WHERE user_id = '" & userid & "' AND min_cash_pay <= " & amount & " AND max_cash_pay >= " & amount & "")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function IsUserNonCashLimitOk(ByVal userid As String, ByVal amount As Decimal) As Boolean
            Dim result As String

            If amount < 0 Then
                amount = -1 * amount
            End If

            result = gUtils.GetField("SELECT user_id FROM AppUsers WHERE user_id = '" & userid & "' AND min_non_cash <= " & amount & " AND max_non_cash >= " & amount & "")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function IsUserRecApprLimitOk(ByVal userid As String, ByVal amount As Decimal) As Boolean
            Dim result As String

            If amount < 0 Then
                amount = -1 * amount
            End If

            result = gUtils.GetField("SELECT user_id FROM AppUsers WHERE user_id = '" & userid & "' AND min_cash_rec_appr <= " & amount & " AND max_cash_rec_appr >= " & amount & "")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function IsUserPayApprLimitOk(ByVal userid As String, ByVal amount As Decimal) As Boolean
            Dim result As String

            If amount < 0 Then
                amount = -1 * amount
            End If

            result = gUtils.GetField("SELECT user_id FROM AppUsers WHERE user_id = '" & userid & "' AND min_cash_pay_appr <= " & amount & " AND max_cash_pay_appr >= " & amount & "")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function IsUserNonCashApprLimitOk(ByVal userid As String, ByVal amount As Decimal) As Boolean
            Dim result As String

            If amount < 0 Then
                amount = -1 * amount
            End If

            result = gUtils.GetField("SELECT user_id FROM AppUsers WHERE user_id = '" & userid & "' AND min_non_cash_appr <= " & amount & " AND max_non_cash_appr >= " & amount & "")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function OpenBudgetSessionExist(ByVal headsubcode As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT budget_session_code FROM BudgetSessions WHERE headsub_code = '" & headsubcode & "' AND closed = 0")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function GetAccountSite(ByVal account As String) As String
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_GetAccountSite"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@AccountNo", account))
                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.VarChar, 100))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return result.Value
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function GetCustomerSite(ByVal custno As String) As String
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_GetCustomerSite"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@CustNo", custno))
                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.VarChar, 100))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return result.Value
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function GetCustomerHeadSub(ByVal custno As String) As String
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_GetCustomerHeadSub"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@CustNo", custno))
                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.VarChar, 100))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return result.Value
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function GetBudgetSessionCode(ByVal budgetyear As Integer, ByVal headsubcode As String) As String
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_GenerateBudgetNum"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@Year", budgetyear))
                Cmd.Parameters.Add(New SqlParameter("@HeadSubCode", headsubcode))
                result = Cmd.Parameters.Add(New SqlParameter("@BudgetNum", SqlDbType.VarChar, 30))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return result.Value
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function FullAccount(ByVal headsubcode As String, ByVal sString As String) As String
            Dim sType As Integer

            If Left(Trim(sString), 1) <> "%" And Left(Trim(sString), 2) <> "%%" And UCase(Left(Trim(sString), 1)) <> "T" Then
                Return sString
            ElseIf Left(Trim(sString), 2) = "%%" Then
                sString = Mid(sString, 3, 18)
                sType = 2
            ElseIf Left(Trim(sString), 2) <> "%%" And Left(Trim(sString), 1) = "%" Then
                sString = Mid(sString, 2, 19)
                sType = 1
            ElseIf UCase(Left(Trim(sString), 1)) = "T" Then
                sString = Trim(Mid(sString, 2, 48))
                sType = 3
            End If

            Dim result As String, result1 As String, headcode As String, subcode As String

            headcode = gUtils.GetField("SELECT head_code FROM HeadsAndSubs WHERE headsub_code = '" & headsubcode & "'")
            subcode = gUtils.GetField("SELECT sub_org FROM HeadsAndSubs WHERE headsub_code = '" & headsubcode & "'")
            result = gUtils.GetField("SELECT separator FROM ParamPreferences WHERE rec_id = '1'")

            If sType = 1 Then
                If gUtils.GetParamRule(31) = True Then
                    Return headcode & subcode & result & sString
                Else
                    Return headcode & result & subcode & result & sString
                End If
            ElseIf sType = 3 Then
                result1 = gUtils.GetField("SELECT account_no FROM Accounts WHERE old_account_no = '" & sString & "'")
                Return result1
            Else
                result1 = gUtils.GetField("SELECT flex_prefix FROM ParamPreferences WHERE rec_id = '1'")
                Return result1 & result & headcode & subcode & result & gUtils.FormatString(sString, 7)
            End If
        End Function



        Public Shared Function AXFullAccount(ByVal headsubcode As String, ByVal sString As String) As String
            Dim sType As Integer

            If Left(Trim(sString), 1) <> "%" And Left(Trim(sString), 2) <> "%%" And UCase(Left(Trim(sString), 1)) <> "T" Then
                Return sString
            ElseIf Left(Trim(sString), 2) = "%%" Then
                sString = Mid(sString, 3, 18)
                sType = 2
            ElseIf Left(Trim(sString), 2) <> "%%" And Left(Trim(sString), 1) = "%" Then
                sString = Mid(sString, 2, 19)
                sType = 1
            ElseIf UCase(Left(Trim(sString), 1)) = "T" Then
                sString = Trim(Mid(sString, 2, 48))
                sType = 3
            End If

            Dim result As String, result1 As String, headcode As String, subcode As String

            headcode = gUtils.AxGetField("SELECT head_code FROM HeadsAndSubs WHERE headsub_code = '" & headsubcode & "'")
            subcode = gUtils.AxGetField("SELECT sub_org FROM HeadsAndSubs WHERE headsub_code = '" & headsubcode & "'")
            result = gUtils.AxGetField("SELECT separator FROM ParamPreferences WHERE rec_id = '1'")

            If sType = 1 Then
                If gUtils.AxGetParamRule(31) = True Then
                    Return headcode & subcode & result & sString
                Else
                    Return headcode & result & subcode & result & sString
                End If
            ElseIf sType = 3 Then
                result1 = gUtils.AxGetField("SELECT account_no FROM Accounts WHERE old_account_no = '" & sString & "'")
                Return result1
            Else
                result1 = gUtils.GetField("SELECT flex_prefix FROM ParamPreferences WHERE rec_id = '1'")
                Return result1 & result & headcode & subcode & result & gUtils.FormatString(sString, 7)
            End If
        End Function

        Public Shared Function GetBudgetSession(ByVal headsubcode As String) As String
            Return gUtils.GetField("SELECT budget_session_desc FROM BudgetSessions WHERE headsub_code = '" & headsubcode & "' AND closed = 0")
        End Function

        Public Shared Function GetBudgetCodeName(ByVal budgetcode As String) As String
            Return gUtils.GetField("SELECT budget_desc FROM BudgetControls WHERE budget_code = '" & budgetcode & "'")
        End Function

        Public Shared Function GetBudgetCodeAmount(ByVal budgetcode As String, ByVal budgetsession As String) As Decimal
            Return gUtils.GetField("SELECT budget_total FROM BudgetControlsTemp WHERE budget_code = '" & budgetcode & "' AND budget_session_code = '" & budgetsession & "'")
        End Function

        Public Shared Function GetYearStart() As Integer
            Return gUtils.GetField("SELECT YEAR(year_start) FROM ParamPreferences WHERE rec_id = '1'")
        End Function

        Public Shared Function GetYearEnd() As Integer
            Return gUtils.GetField("SELECT YEAR(year_end) FROM ParamPreferences WHERE rec_id = '1'")
        End Function

        Public Shared Function ApprovalRequired(ByVal headsubcode As String) As Boolean
            Dim ds As New DataSet

            ds = gUtils.GetDataset("SELECT ref_no FROM PaymentsAndRevenues WHERE status = 'E' AND amount <> 0 AND headsub_code = '" & headsubcode & "'")

            If ds.Tables(0).Rows.Count > 0 Then
                Return True
            Else
                Return False
            End If

            ds.Dispose()
        End Function

        Public Shared Function GetBudgetAmount(ByVal account As String, ByVal budgetsession As String) As Decimal
            Return gUtils.GetField("SELECT budget_amount FROM BudgetAccounts WHERE account_no = '" & account & "' AND budget_session_code = '" & budgetsession & "'")
        End Function

        Public Shared Function GetMonthBudgetAmount(ByVal account As String, ByVal mth As Integer) As Decimal
            Return gUtils.GetField("SELECT budget_month_amount FROM BudgetAccountsSub WHERE account_no = '" & account & "' AND budget_month = " & mth & "")
        End Function

        Public Shared Function IsBudgetOk(ByVal headsub As String, ByVal budgetsession As String, ByVal budgetyear As Integer) As Boolean
            Dim result As Date, result1 As String

            result = gUtils.GetField("SELECT year_start FROM ParamPreferences WHERE rec_id = '1'")

            If CInt(Year(result)) <> CInt(budgetyear) Then
                Return False
            End If

            result1 = gUtils.GetField("SELECT type FROM BudgetVerifications WHERE headsub_code = '" & headsub & "' AND budget_session_code = '" & budgetsession & "'")

            If Len(result1) > 0 Then
                Return False
            Else
                Return True
            End If
        End Function

        Public Shared Function IsOLTP() As Boolean
            Return gUtils.GetField("SELECT oltp FROM ParamPreferences WHERE rec_id = '1'")
        End Function

        Public Shared Function GetPostingDate() As Date
            Dim result As Date
            result = gUtils.GetFieldDate("SELECT post_date FROM ParamPreferences WHERE rec_id = '1'")
            Return result
        End Function

        Public Shared Function GetMaxJournalEntry(ByVal journalno As String) As Decimal
            Dim maxvalue As Decimal, creditvalue As Decimal

            maxvalue = gUtils.GetField("SELECT Max(debit) FROM TransTemp WHERE ref_no = '" & journalno & "'")
            creditvalue = gUtils.GetField("SELECT Max(credit) FROM TransTemp WHERE ref_no = '" & journalno & "'")

            If creditvalue > maxvalue Then
                Return creditvalue
            Else
                Return maxvalue
            End If
        End Function

        Public Shared Function JournalForApproval(ByVal user As String, ByVal headsubcode As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT ref_no FROM Journals WHERE approving_offr2 = '" & user & "' AND headsub_code = '" & headsubcode & "' AND completed <> 0 AND status = 'E'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function AccountForApproval(ByVal headsubcode As String, ByVal userid As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT account_no FROM Accounts WHERE headsub_code = '" & headsubcode & "' AND account_status = '2' AND added_by <> '" & userid & "'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function AccountChangesForApproval(ByVal headsubcode As String, ByVal userid As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT account_no FROM AccountsMaintLog WHERE headsub_code = '" & headsubcode & "' AND status = 'E' AND posted_by <> '" & userid & "'" & _
            " AND account_maint_type IN (SELECT account_maint_type FROM AppUsersMaintTypes WHERE user_id = '" & userid & "' AND type = '1')")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function AccountProdTypeMatch(ByVal account As String, ByVal prodtype As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT account_no FROM VwAccounts WHERE account_no = '" & account & "' AND prod_type_code = '" & prodtype & "'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function GetInstrumentProdType(ByVal instrcode As String) As String
            Dim result As String

            result = gUtils.GetField("SELECT prod_type_code FROM InstrumentsCharges WHERE instr_charge_code = '" & instrcode & "'")
            Return result
        End Function

        Public Shared Function TransForApproval(ByVal user As String, ByVal headsubcode As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT ref_no FROM TransRefs WHERE approving_offr2 = '" & user & "' AND headsub_code = '" & headsubcode & "' AND status = 'E' AND completed = 1")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function OverdraftForApproval(ByVal user As String, ByVal headsubcode As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT od_ref_no FROM VwODLogs WHERE approving_offr2 = '" & user & "' AND headsub_code = '" & headsubcode & "' AND status = 'E' AND completed = 1 AND revoked = 0")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function ScheduleForApproval(ByVal user As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT account_no FROM LoanOps WHERE approving_offr = '" & user & "'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function ReversalForApproval(ByVal headsubcode As String, ByVal userid As String) As Boolean
            Dim result As String, postingdate As Date

            postingdate = GetPostingDate()

            result = gUtils.GetField("SELECT ref_no FROM TransRefs WHERE headsub_code = '" & headsubcode & "' AND posted_by <> '" & userid & "' AND reversed_set = 1 AND reversed = 0 AND post_date = '" & postingdate.Date & "'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function CheckUnapprovedEntries() As Boolean
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_CheckUnapprovedEntries"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.Int, 1))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return CBool(result.Value)
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function AdvanceExist(ByVal advanceno As String, ByVal headsub As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT advance_no FROM Advances WHERE advance_no = '" & advanceno & "' AND status = 'A' AND headsub_code = '" & headsub & "'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function AccountExist(ByVal account As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT account_no FROM Accounts WHERE account_no = '" & account & "'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function
        Public Shared Function AXAccountExist(ByVal account As String) As Boolean
            Dim result As String

            result = gUtils.AxGetField("SELECT account_no FROM Accounts WHERE account_no = '" & account & "'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function
        Public Shared Function GetAccountAttribute(ByVal acct As String) As String
            Dim result As String

            result = gUtils.GetField("SELECT attr_code FROM VwAccounts WHERE account_no = '" & acct & "'")
            Return result
        End Function
        Public Shared Function AllowCashInJournals() As Boolean
            Dim result As Integer

            result = gUtils.GetField("SELECT cash_in_journals FROM ParamPreferences WHERE rec_id  = '1'")

            If result <> 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function GetAdvanceInfo(ByVal advanceno As String, ByVal infoitem As Integer) As String
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_AdvanceInfo"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@AdvanceNo", advanceno))
                Cmd.Parameters.Add(New SqlParameter("@InfoItem", infoitem))
                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.VarChar, 100))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return result.Value
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function AccountHasTrans(ByVal account As String) As Boolean
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_AccountHasTrans"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@AccountNo", account))
                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.Int, 1))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return CBool(result.Value)
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function AccountHasOverdrafts(ByVal account As String) As Boolean
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_AccountHasOverdrafts"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@AccountNo", account))
                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.Int, 1))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return CBool(result.Value)
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function


        Public Shared Function GetTransCodeInfo(ByVal transcode As String, ByVal infonum As Integer) As String
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_GetTransCodeInfo"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@TransCode", transcode))
                Cmd.Parameters.Add(New SqlParameter("@InfoNum", infonum))
                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.VarChar, 100))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return (result.Value)
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function GetProdCodeInfo(ByVal prodcode As String, ByVal infonum As Integer) As String
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_GetProdCodeInfo"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@ProdCode", prodcode))
                Cmd.Parameters.Add(New SqlParameter("@InfoNum", infonum))
                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.VarChar, 100))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return (result.Value)
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function GetAccountInfo(ByVal accountno As String, ByVal infonum As Integer) As String
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_GetAccountInfo"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@AccountNo", accountno))
                Cmd.Parameters.Add(New SqlParameter("@InfoNum", infonum))
                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.VarChar, 100))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return (result.Value)
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function GetTransRefTotal(ByVal refno As String, ByVal atype As Integer) As Decimal
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_GetTransRefTotal"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@RefNo", refno))
                Cmd.Parameters.Add(New SqlParameter("@Type", atype))
                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.VarChar, 30))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return CDec(result.Value)
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function GetTransHistRefTotal(ByVal refno As String, ByVal atype As Integer) As Decimal
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_GetTransHistRefTotal"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@RefNo", refno))
                Cmd.Parameters.Add(New SqlParameter("@Type", atype))
                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.VarChar, 30))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return CDec(result.Value)
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function GetHistRefTotal(ByVal refno As String, ByVal atype As Integer) As Decimal
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_GetHistRefTotal"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@RefNo", refno))
                Cmd.Parameters.Add(New SqlParameter("@Type", atype))
                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.VarChar, 30))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return CDec(result.Value)
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function PostingLimitOk(ByVal userid As String, ByVal amount As Decimal, ByVal atype As Integer, ByVal trans As String) As Boolean
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_PostingLimitOk"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@UserID", userid))
                Cmd.Parameters.Add(New SqlParameter("@Amount", amount))
                Cmd.Parameters.Add(New SqlParameter("@Type", atype))
                Cmd.Parameters.Add(New SqlParameter("@Trans", trans))
                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.VarChar, 1))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return CBool(result.Value)
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function CeilingLimitOk(ByVal userid As String, ByVal amount As Decimal, ByVal atype As Integer, ByVal trans As String) As Boolean
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_CeilingLimitOk"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@UserID", userid))
                Cmd.Parameters.Add(New SqlParameter("@Amount", amount))
                Cmd.Parameters.Add(New SqlParameter("@Type", atype))
                Cmd.Parameters.Add(New SqlParameter("@Trans", trans))
                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.VarChar, 1))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return CBool(result.Value)
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function GetAvailableBal(ByVal account As String) As Decimal
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_GetAvailableBal"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@AccountNo", account))
                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.VarChar, 20))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return CDec(result.Value)
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function GetLoanAccruedInt(ByVal account As String) As Decimal
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_GetLoanAccruedInt"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@AccountNo", account))
                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.VarChar, 20))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return CDec(result.Value)
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function GetAvailableBalNoCot(ByVal account As String) As Decimal
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_GetAvailableBalNoCot"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@AccountNo", account))
                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.VarChar, 20))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return CDec(result.Value)
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function GetAvailableBalPDOs(ByVal account As String) As Decimal
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_GetAvailableBalPDOs"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@AccountNo", account))
                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.VarChar, 20))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return CDec(result.Value)
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function AccountLimitOk(ByVal account As String, ByVal amount As Decimal) As Boolean
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_AccountLimitOk"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@AccountNo", account))
                Cmd.Parameters.Add(New SqlParameter("@Amount", amount))
                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.VarChar, 1))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return CBool(result.Value)
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function
        Public Shared Function InstrumentRangeExist(ByVal startno As String, ByVal endno As String, ByVal accountno As String) As Boolean
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_InstrumentRangeExist"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@StartNo", startno))
                Cmd.Parameters.Add(New SqlParameter("@EndNo", endno))
                Cmd.Parameters.Add(New SqlParameter("@AccountNo", accountno))
                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.Int, 1))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return CBool(result.Value)
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function
        Public Shared Function InstrumentExist(ByVal account As String, ByVal instr As String) As Boolean
            Dim result As Integer

            result = gUtils.GetField("SELECT seq_no FROM VwInstruments WHERE account_no = '" & account & "' AND ('" & instr & "' BETWEEN start_no AND end_no) AND approved_by IS NOT NULL AND revoked = 0")

            If result <> 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function LoanGroupExist(ByVal loangroupcode As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT loan_group_code FROM LoanGroups WHERE loan_group_code = '" & loangroupcode & "'")

            If Len(result) <> 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function AssetVoucherExist(ByVal voucher As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT voucher_no FROM FAMaster WHERE voucher_no = '" & voucher & "'")

            If Len(result) <> 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function JournalCheckOk(ByVal refno As String) As Boolean
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_JournalCheckOk"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@RefNo", refno))
                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.VarChar, 1))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return CBool(result.Value)
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function GetSystemAccount(ByVal id As String, ByVal headsubcode As String) As String
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_GetSystemAccount"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure


            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@Id", id))
                Cmd.Parameters.Add(New SqlParameter("@HeadSubCode", headsubcode))
                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.VarChar, 20))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return result.Value
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function GetClearDays(ByVal tenor As Integer) As Date
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_GetClearingDate"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@Tenor", tenor))
                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.DateTime))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return result.Value
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function GetLastLoanRepay(ByVal account As String) As Date
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_GetLastLoanRepay"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@AccountNo", account))
                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.DateTime))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return result.Value
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function GetOverdraft(ByVal account As String) As Decimal
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_GetOverdrafts"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@AccountNo", account))
                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.VarChar, 20))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return CDec(result.Value)
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function DealProdOk(ByVal prodcode As String) As Boolean
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_DealProdOk"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@ProdCode", prodcode))
                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.VarChar, 1))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return CBool(result.Value)
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function DealLastValueDate(ByVal account As String, ByVal dealno As String) As Date

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_DealLastValueDate"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@AccountNo", account))
                Cmd.Parameters.Add(New SqlParameter("@DealNo", dealno))

                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.DateTime))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return result.Value
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function NextLoanExecDate(ByVal account As String) As Date

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_NextLoanExecDate"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@AccountNo", account))

                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.DateTime))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return result.Value
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function GetLastSOM(ByVal account As String) As Date

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_GetLastSOM"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@AccountNo", account))

                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.DateTime))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return result.Value
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function DealTransAccountOk(ByVal account As String, ByVal transcode As String) As Boolean

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_DealTransAccountOk"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@AccountNo", account))
                Cmd.Parameters.Add(New SqlParameter("@TransCode", transcode))
                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.VarChar, 1))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return CBool(result.Value)
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function DealRateOk(ByVal account As String, ByVal rate As Decimal) As Boolean

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_DealRateOk"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@AccountNo", account))
                Cmd.Parameters.Add(New SqlParameter("@Rate", rate))
                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.VarChar, 1))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return CBool(result.Value)
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function LoanScheduleExists(ByVal refno As String) As Boolean

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_LoanScheduleExists"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@RefNo", refno))
                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.VarChar, 1))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return CBool(result.Value)
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function GetAccountMaintValues(ByVal account As String, ByVal mainttype As String) As String

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_GetAccountMaintValues"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@AccountNo", account))
                Cmd.Parameters.Add(New SqlParameter("@AccountMaintType", mainttype))

                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.VarChar, 100))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return result.Value
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Function UserViewAccount(ByVal userid As String, ByVal account As String) As Boolean
            If GetAccountInfo(account, 20) = 0 Or CanViewProtected(userid) = True Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function GetAccountRateForMaint(ByVal account As String, ByVal mainttype As String) As Decimal

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()
            Cmd.Parameters.Add(New SqlParameter("@AccountNo", account))
            Cmd.Parameters.Add(New SqlParameter("@MaintType", mainttype))

            Cmd.CommandText = "Sproc_GetAccountRateForMaint"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()

                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.VarChar, 100))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return CDec(result.Value)
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function CountUnfinishedTrans() As Integer

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_CountUnfinishedTrans"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.VarChar, 100))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return CInt(result.Value)
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function CountUnapprovedTrans() As Integer

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_CountUnapprovedTrans"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.VarChar, 100))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return CInt(result.Value)
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function CountUnapprovedLoanSchedules() As Integer

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_CountUnapprovedLoanSchedules"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.VarChar, 100))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return CInt(result.Value)
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function CountUncommittedProxyTrans() As Integer

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_CountUncommittedProxyTrans"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.VarChar, 100))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return CInt(result.Value)
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function CountLoanScheduleRecs(ByVal account As String, ByVal dealno As String, ByVal simulate As Integer) As Integer

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_CountLoanScheduleRecs"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@AccountNo", account))
                Cmd.Parameters.Add(New SqlParameter("@DealNo", dealno))
                Cmd.Parameters.Add(New SqlParameter("@Simulate", simulate))

                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.VarChar, 100))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return CInt(result.Value)
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function GetSOIncomeAcct(ByVal account As String, ByVal headsubcode As String) As String

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_GetSOIncomeAcct"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@AccountNo", account))
                Cmd.Parameters.Add(New SqlParameter("@HeadSubCode", headsubcode))

                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.VarChar, 20))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return result.Value
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function GenerateTransNum(ByVal headsubcode As String, ByVal type As String) As String

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_GenerateTransNum"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@AccountNo", headsubcode))
                Cmd.Parameters.Add(New SqlParameter("@Type", type))

                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.VarChar, 50))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return result.Value
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function GenerateDealNo(ByVal AccountNo As String) As String

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_GenerateDealNo"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@AccountNo", AccountNo))

                result = Cmd.Parameters.Add(New SqlParameter("@DealNo", SqlDbType.VarChar, 50))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return result.Value
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function ExecuteScheduleDue(ByVal seqno As Integer, ByVal refno As String, ByVal userid As String, ByVal headsub As String) As Boolean

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_ExecuteScheduleDue"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@SeqNo", seqno))
                Cmd.Parameters.Add(New SqlParameter("@RefNo", refno))
                Cmd.Parameters.Add(New SqlParameter("@UserId", userid))
                Cmd.Parameters.Add(New SqlParameter("@HeadSubCode", headsub))

                result = Cmd.Parameters.Add(New SqlParameter("@Res", SqlDbType.Bit, 5))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return result.Value
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function UnclearedChequeExist(ByVal account As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT account_no FROM ClearingCheques WHERE account_no = '" & account & "' OR contra = '" & account & "'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function SOExist(ByVal account As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT account_no FROM SOrders WHERE account_no = '" & account & "' OR contra = '" & account & "' OR charge_contra = '" & account & "'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function TransRefHasClosedAccount(ByVal refno As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT account_no FROM VwTransHist WHERE ref_no = '" & refno & "' AND account_status = '3'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function ScheduleExist(ByVal account As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT account_no FROM LoanSchedules WHERE account_no = '" & account & "' OR int_inc_acct = '" & account & "'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function TransExist(ByVal account As String) As Boolean
            Dim result As String

            result = gUtils.GetField("SELECT account_no FROM TransTemp WHERE account_no = '" & account & "'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function AddCashTrans(ByVal transdate As Date, ByVal accountno As String, ByVal desc As String, ByVal amount As Decimal, ByVal intrno As String, ByVal cashacct As String, ByVal transcode As String, ByVal tod As Integer, ByVal postedby As String, ByVal appoffr As String, ByVal headsub As String, ByVal source As String, ByVal unitcode As String) As String

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_AddCashTrans"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@TransDate", transdate))
                Cmd.Parameters.Add(New SqlParameter("@AccountNo", accountno))
                Cmd.Parameters.Add(New SqlParameter("@Description", desc))
                Cmd.Parameters.Add(New SqlParameter("@Amount", amount))
                Cmd.Parameters.Add(New SqlParameter("@InstrNo", intrno))
                Cmd.Parameters.Add(New SqlParameter("@CashAcct", cashacct))
                Cmd.Parameters.Add(New SqlParameter("@TransCode", transcode))
                Cmd.Parameters.Add(New SqlParameter("@TOD", tod))
                Cmd.Parameters.Add(New SqlParameter("@PostedBy", postedby))
                Cmd.Parameters.Add(New SqlParameter("@ApprovingOffr2", appoffr))
                Cmd.Parameters.Add(New SqlParameter("@HeadSubCode", headsub))
                Cmd.Parameters.Add(New SqlParameter("@Source", source))
                Cmd.Parameters.Add(New SqlParameter("@UnitCode", unitcode))

                result = Cmd.Parameters.Add(New SqlParameter("@RefNo", SqlDbType.VarChar, 50, ParameterDirection.Output))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return result.Value

            Catch Exc As Exception
                Return Exc.Message
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function AddNonCashTrans(ByVal transdate As Date, ByVal accountno As String, ByVal desc As String, ByVal desc2 As String, ByVal amount As Decimal, ByVal transcode As String, ByVal contra As String, ByVal appoffr As String, ByVal bankcode As String, ByVal intrno As String, ByVal clearcode As String, ByVal cleardays As Integer, ByVal postedby As String, ByVal headsub As String, ByVal source As String, ByVal tod As Integer, ByVal unitcode As String) As String

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_AddNonCashTrans"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try

                If bankcode = Nothing Then
                    bankcode = ""
                End If

                If clearcode = Nothing Then
                    clearcode = ""
                End If

                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@TransCode", transcode))
                Cmd.Parameters.Add(New SqlParameter("@AccountNo", accountno))
                Cmd.Parameters.Add(New SqlParameter("@Description", desc))
                Cmd.Parameters.Add(New SqlParameter("@Description2", desc2))
                Cmd.Parameters.Add(New SqlParameter("@Amount", amount))
                Cmd.Parameters.Add(New SqlParameter("@ValueDate", transdate))
                Cmd.Parameters.Add(New SqlParameter("@Contra", contra))
                Cmd.Parameters.Add(New SqlParameter("@ApprovingOffr2", appoffr))
                Cmd.Parameters.Add(New SqlParameter("@BankCode", bankcode))
                Cmd.Parameters.Add(New SqlParameter("@InstrNo", intrno))
                Cmd.Parameters.Add(New SqlParameter("@ClearCode", clearcode))
                Cmd.Parameters.Add(New SqlParameter("@ClearDays", cleardays))
                Cmd.Parameters.Add(New SqlParameter("@PostedBy", postedby))
                Cmd.Parameters.Add(New SqlParameter("@HeadSubCode", headsub))
                Cmd.Parameters.Add(New SqlParameter("@Source", source))
                Cmd.Parameters.Add(New SqlParameter("@TOD", tod))
                Cmd.Parameters.Add(New SqlParameter("@UnitCode", unitcode))

                result = Cmd.Parameters.Add(New SqlParameter("@RefNo", SqlDbType.VarChar, 50))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return result.Value

            Catch Exc As Exception
                Return Exc.Message
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function AddCustomer(ByVal CustNo As String, ByVal Names As String, ByVal CustType As String, ByVal Address As String, ByVal City As String, ByVal Email As String, ByVal Rcno As String, ByVal Rcyear As String, ByVal SectorCode As String, ByVal ClassCode As String, ByVal OwnCode As String, ByVal Contact As String, ByVal Turnover As Decimal, ByVal EmpSize As Integer, ByVal AssetBase As Decimal, ByVal BoardSize As Integer, _
        ByVal TitleCode As String, ByVal Sex As String, ByVal BirthDate As DateTime, ByVal ProfCode As String, ByVal IncomeCode As String, ByVal StyleCode As String, ByVal NextKin As String, ByVal NextKinAddr As String, ByVal NextKinCity As String, ByVal Resident As Boolean, ByVal CountryCode As String, ByVal AddedBy As String, ByVal Remarks As String, ByVal HeadSubCode As String, ByVal EffectChanges As Boolean) As String

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_AddCustomer"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@CustNo", CustNo))
                Cmd.Parameters.Add(New SqlParameter("@Names", Names))
                Cmd.Parameters.Add(New SqlParameter("@CustType", CustType))
                Cmd.Parameters.Add(New SqlParameter("@Address", Address))
                Cmd.Parameters.Add(New SqlParameter("@City", City))
                Cmd.Parameters.Add(New SqlParameter("@Email", Email))
                Cmd.Parameters.Add(New SqlParameter("@Rcno", Rcno))
                Cmd.Parameters.Add(New SqlParameter("@Rcyear", Rcyear))
                Cmd.Parameters.Add(New SqlParameter("@SectorCode", SectorCode))
                Cmd.Parameters.Add(New SqlParameter("@ClassCode", ClassCode))
                Cmd.Parameters.Add(New SqlParameter("@OwnCode", OwnCode))
                Cmd.Parameters.Add(New SqlParameter("@Contact", Contact))
                Cmd.Parameters.Add(New SqlParameter("@Turnover", Turnover))
                Cmd.Parameters.Add(New SqlParameter("@EmpSize", EmpSize))
                Cmd.Parameters.Add(New SqlParameter("@AssetBase", AssetBase))
                Cmd.Parameters.Add(New SqlParameter("@BoardSize", BoardSize))

                Cmd.Parameters.Add(New SqlParameter("@TitleCode", TitleCode))
                Cmd.Parameters.Add(New SqlParameter("@Sex", Sex))
                Cmd.Parameters.Add(New SqlParameter("@BirthDate", BirthDate))
                Cmd.Parameters.Add(New SqlParameter("@ProfCode", ProfCode))
                Cmd.Parameters.Add(New SqlParameter("@IncomeCode", IncomeCode))


                Cmd.Parameters.Add(New SqlParameter("@StyleCode", StyleCode))
                Cmd.Parameters.Add(New SqlParameter("@NextKin", NextKin))
                Cmd.Parameters.Add(New SqlParameter("@NextKinAddr", NextKinAddr))
                Cmd.Parameters.Add(New SqlParameter("@NextKinCity", NextKinCity))
                Cmd.Parameters.Add(New SqlParameter("@Resident", Resident))

                Cmd.Parameters.Add(New SqlParameter("@CountryCode", CountryCode))
                Cmd.Parameters.Add(New SqlParameter("@AddedBy", AddedBy))
                Cmd.Parameters.Add(New SqlParameter("@Remarks", Remarks))
                Cmd.Parameters.Add(New SqlParameter("@HeadSubCode", HeadSubCode))
                Cmd.Parameters.Add(New SqlParameter("@EffectChanges", EffectChanges))


                Cmd.ExecuteNonQuery()
                OleConn.Close()

                Return "Request Executed for " & CustNo
            Catch Exc As Exception
                Return Exc.Message
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function AddVaultTrans(ByVal transdate As Date, ByVal accountno As String, ByVal desc As String, ByVal amount As Decimal, ByVal cashacct As String, ByVal transcode As String, ByVal postedby As String, ByVal appoffr As String, ByVal headsub As String, ByVal source As String) As String

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_AddVaultTrans"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try

                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@TransDate", transdate))
                Cmd.Parameters.Add(New SqlParameter("@AccountNo", accountno))
                Cmd.Parameters.Add(New SqlParameter("@Description", desc))
                Cmd.Parameters.Add(New SqlParameter("@Amount", amount))
                Cmd.Parameters.Add(New SqlParameter("@CashAcct", cashacct))
                Cmd.Parameters.Add(New SqlParameter("@TransCode", transcode))
                Cmd.Parameters.Add(New SqlParameter("@PostedBy", postedby))
                Cmd.Parameters.Add(New SqlParameter("@ApprovingOffr2", appoffr))
                Cmd.Parameters.Add(New SqlParameter("@HeadSubCode", headsub))
                Cmd.Parameters.Add(New SqlParameter("@Source", source))

                result = Cmd.Parameters.Add(New SqlParameter("@RefNo", SqlDbType.VarChar, 50))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return "Executed as Transaction Reference " & result.Value
            Catch Exc As Exception
                Return Exc.Message
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function AddDealTrans(ByVal AccountNo As String, ByVal Narrative As String, ByVal Amount As Decimal, ByVal Effective As Date, ByVal TenorType As String, ByVal Tenor As Integer, ByVal Maturity As Date, ByVal Rate As Decimal, ByVal Base As Integer, ByVal Contra As String, _
        ByVal SettleAcct As String, ByVal AccrualAcct As String, ByVal OffrCode As String, ByVal DealClassCode As String, ByVal PurposeType As String, ByVal Upfront As Boolean, ByVal Scheduled As Boolean, ByVal Type As String, ByVal Subtype As String, ByVal TaxRate As Decimal, _
        ByVal PenaltyRate As Decimal, ByVal FlatPenalty As Boolean, ByVal PostedBy As String, ByVal ApprovingOffr2 As String, ByVal HeadSubCode As String, ByVal Source As String, ByVal Rollover As Boolean, ByVal RolloverRate As Decimal, ByVal RolloverTenor As Integer, _
        ByVal LoanGroupCode As String, ByVal SectorCode As String, ByVal RepayAmount As Decimal, ByVal RepayStartDate As Date, ByVal AssetDesc As String, ByVal Compound As Integer, ByVal FreqCode As String) As String

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_AddDealTrans"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                If LoanGroupCode.Trim = Nothing Then
                    LoanGroupCode = "ZZZZZ"
                End If

                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@AccountNo", AccountNo))
                Cmd.Parameters.Add(New SqlParameter("@Narrative", Narrative))
                Cmd.Parameters.Add(New SqlParameter("@Amount", Amount))
                Cmd.Parameters.Add(New SqlParameter("@Effective", Effective))
                Cmd.Parameters.Add(New SqlParameter("@TenorType", TenorType))
                Cmd.Parameters.Add(New SqlParameter("@Tenor", Tenor))
                Cmd.Parameters.Add(New SqlParameter("@Maturity", Maturity))
                Cmd.Parameters.Add(New SqlParameter("@Rate", Rate))
                Cmd.Parameters.Add(New SqlParameter("@Base", Base))
                Cmd.Parameters.Add(New SqlParameter("@Contra", Contra))
                Cmd.Parameters.Add(New SqlParameter("@SettleAcct", SettleAcct))
                Cmd.Parameters.Add(New SqlParameter("@AccrualAcct", AccrualAcct))
                Cmd.Parameters.Add(New SqlParameter("@OffrCode", OffrCode))
                Cmd.Parameters.Add(New SqlParameter("@DealClassCode", DealClassCode))
                Cmd.Parameters.Add(New SqlParameter("@PurposeType", PurposeType))
                Cmd.Parameters.Add(New SqlParameter("@Upfront", Upfront))
                Cmd.Parameters.Add(New SqlParameter("@Scheduled", Scheduled))
                Cmd.Parameters.Add(New SqlParameter("@Type", Type))
                Cmd.Parameters.Add(New SqlParameter("@Subtype", Subtype))
                Cmd.Parameters.Add(New SqlParameter("@TaxRate", TaxRate))
                Cmd.Parameters.Add(New SqlParameter("@PenaltyRate", PenaltyRate))
                Cmd.Parameters.Add(New SqlParameter("@FlatPenalty", FlatPenalty))
                Cmd.Parameters.Add(New SqlParameter("@PostedBy", PostedBy))
                Cmd.Parameters.Add(New SqlParameter("@ApprovingOffr2", ApprovingOffr2))
                Cmd.Parameters.Add(New SqlParameter("@HeadSubCode", HeadSubCode))
                Cmd.Parameters.Add(New SqlParameter("@Source", Source))
                Cmd.Parameters.Add(New SqlParameter("@Rollover", Rollover))
                Cmd.Parameters.Add(New SqlParameter("@RolloverRate", RolloverRate))
                Cmd.Parameters.Add(New SqlParameter("@RolloverTenor", RolloverTenor))
                Cmd.Parameters.Add(New SqlParameter("@LoanGroupCode", LoanGroupCode))
                Cmd.Parameters.Add(New SqlParameter("@SectorCode", SectorCode))
                Cmd.Parameters.Add(New SqlParameter("@RepayAmount", RepayAmount))
                Cmd.Parameters.Add(New SqlParameter("@RepayStartDate", RepayStartDate))
                Cmd.Parameters.Add(New SqlParameter("@AssetDesc", AssetDesc))
                Cmd.Parameters.Add(New SqlParameter("@Compound", Compound))
                Cmd.Parameters.Add(New SqlParameter("@FreqCode", FreqCode))

                result = Cmd.Parameters.Add(New SqlParameter("@RefNo", SqlDbType.VarChar, 50))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return "Executed as Transaction Reference " & result.Value
            Catch Exc As Exception
                Return Exc.Message
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function AddLoanTrans(ByVal AccountNo As String, ByVal Narrative As String, ByVal FacilityLimit As Decimal, ByVal EquityContr As Decimal, ByVal Amount As Decimal, _
        ByVal Effective As Date, ByVal TenorType As String, ByVal Tenor As Integer, ByVal Maturity As Date, ByVal Rate As Decimal, ByVal Base As Integer, ByVal Contra As String, _
        ByVal SettleAcct As String, ByVal OffrCode As String, ByVal PurposeType As String, ByVal Scheduled As Integer, _
        ByVal PostedBy As String, ByVal ApprovingOffr2 As String, ByVal HeadSubCode As String, ByVal Source As String, _
        ByVal LoanGroupCode As String, ByVal SectorCode As String, ByVal RepayAmount As Decimal, ByVal RepayStartDate As Date, _
        ByVal AssetDesc As String, ByVal Compound As Integer, ByVal FreqCode As String, ByVal DealNo As String, ByVal TransCode As String, _
        ByVal AccruedInt As Decimal, ByVal ApplyIntGrace As Integer, ByVal IntGrace As Date, ByVal ExecFunded As Integer, ByVal serviceint As Integer) As String

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_AddLoanTrans"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                If LoanGroupCode.Trim = Nothing Then
                    LoanGroupCode = "ZZZZZ"
                End If

                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@AccountNo", AccountNo))
                Cmd.Parameters.Add(New SqlParameter("@Narrative", Narrative))
                Cmd.Parameters.Add(New SqlParameter("@FacilityLimit", FacilityLimit))
                Cmd.Parameters.Add(New SqlParameter("@EquityContr", EquityContr))
                Cmd.Parameters.Add(New SqlParameter("@Amount", Amount))
                Cmd.Parameters.Add(New SqlParameter("@Effective", Effective))
                Cmd.Parameters.Add(New SqlParameter("@TenorType", TenorType))
                Cmd.Parameters.Add(New SqlParameter("@Tenor", Tenor))
                Cmd.Parameters.Add(New SqlParameter("@Maturity", Maturity))
                Cmd.Parameters.Add(New SqlParameter("@Rate", Rate))
                Cmd.Parameters.Add(New SqlParameter("@Base", Base))
                Cmd.Parameters.Add(New SqlParameter("@Contra", Contra))
                Cmd.Parameters.Add(New SqlParameter("@SettleAcct", SettleAcct))
                Cmd.Parameters.Add(New SqlParameter("@OffrCode", OffrCode))
                Cmd.Parameters.Add(New SqlParameter("@PurposeType", PurposeType))
                Cmd.Parameters.Add(New SqlParameter("@Scheduled", Scheduled))
                Cmd.Parameters.Add(New SqlParameter("@PostedBy", PostedBy))
                Cmd.Parameters.Add(New SqlParameter("@ApprovingOffr2", ApprovingOffr2))
                Cmd.Parameters.Add(New SqlParameter("@HeadSubCode", HeadSubCode))
                Cmd.Parameters.Add(New SqlParameter("@Source", Source))
                Cmd.Parameters.Add(New SqlParameter("@LoanGroupCode", LoanGroupCode))
                Cmd.Parameters.Add(New SqlParameter("@SectorCode", SectorCode))
                Cmd.Parameters.Add(New SqlParameter("@RepayAmount", RepayAmount))
                Cmd.Parameters.Add(New SqlParameter("@RepayStartDate", RepayStartDate))
                Cmd.Parameters.Add(New SqlParameter("@AssetDesc", AssetDesc))
                Cmd.Parameters.Add(New SqlParameter("@Compound", Compound))
                Cmd.Parameters.Add(New SqlParameter("@FreqCode", FreqCode))
                Cmd.Parameters.Add(New SqlParameter("@DealNo", DealNo))
                Cmd.Parameters.Add(New SqlParameter("@TransCode", TransCode))
                Cmd.Parameters.Add(New SqlParameter("@AccruedInt", AccruedInt))
                Cmd.Parameters.Add(New SqlParameter("@ApplyIntGrace", ApplyIntGrace))
                Cmd.Parameters.Add(New SqlParameter("@IntGrace", IntGrace))
                Cmd.Parameters.Add(New SqlParameter("@ExecFunded", ExecFunded))
                Cmd.Parameters.Add(New SqlParameter("@ServiceInt", serviceint))

                result = Cmd.Parameters.Add(New SqlParameter("@RefNo", SqlDbType.VarChar, 50))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return "Executed as Transaction Reference " & result.Value
            Catch Exc As Exception
                Return Exc.Message
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function AddCustAccount2(ByVal AccountNo As String, ByVal ProdCode As String, ByVal CustNo As String, ByVal Description As String, ByVal Address As String, ByVal City As String, ByVal Email As String, ByVal Marketer As String, ByVal Officer As String, ByVal TransStatus As String, _
        ByVal HeadSubCode As String, ByVal AddedBy As String, ByVal OldAccount As String, ByVal DrRateMargin As Decimal, ByVal CrRateMargin As Decimal, ByVal Mandate As String, ByVal Online As Boolean, ByVal ATm As Boolean, ByVal Estatement As Boolean)
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_AddCustAccount2"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try

                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@AccountNo", AccountNo))
                Cmd.Parameters.Add(New SqlParameter("@ProdCode", ProdCode))
                Cmd.Parameters.Add(New SqlParameter("@CustNo", CustNo))
                Cmd.Parameters.Add(New SqlParameter("@Description", Description))
                Cmd.Parameters.Add(New SqlParameter("@Address", Address))
                Cmd.Parameters.Add(New SqlParameter("@City", City))
                Cmd.Parameters.Add(New SqlParameter("@Email", Email))
                Cmd.Parameters.Add(New SqlParameter("@Marketer", Marketer))
                Cmd.Parameters.Add(New SqlParameter("@Officer", Officer))
                Cmd.Parameters.Add(New SqlParameter("@TransStatus", TransStatus))
                Cmd.Parameters.Add(New SqlParameter("@HeadSubCode", HeadSubCode))
                Cmd.Parameters.Add(New SqlParameter("@AddedBy", AddedBy))
                Cmd.Parameters.Add(New SqlParameter("@OldAccount", OldAccount))
                Cmd.Parameters.Add(New SqlParameter("@DrRateMargin", DrRateMargin))
                Cmd.Parameters.Add(New SqlParameter("@CrRateMargin", CrRateMargin))
                Cmd.Parameters.Add(New SqlParameter("@Mandate", Mandate))
                Cmd.Parameters.Add(New SqlParameter("@Online", Online))
                Cmd.Parameters.Add(New SqlParameter("@Atm", ATm))
                Cmd.Parameters.Add(New SqlParameter("@Estatement", Estatement))
                Cmd.ExecuteNonQuery()
                OleConn.Close()

                Return "Request Executed for " & AccountNo
            Catch Exc As Exception
                Return Exc.Message
            Finally
                OleConn.Close()
            End Try
        End Function
        Public Shared Function AddCustAccount(ByVal AccountNo As String, ByVal ProdCode As String, ByVal CustNo As String, ByVal Description As String, ByVal Address As String, ByVal City As String, ByVal Email As String, ByVal Marketer As String, ByVal Officer As String, ByVal TransStatus As String, _
           ByVal HeadSubCode As String, ByVal AddedBy As String, ByVal OldAccount As String, ByVal DrRateMargin As Decimal, ByVal CrRateMargin As Decimal, ByVal Mandate As String, ByVal Online As Boolean, ByVal ATm As Boolean, ByVal Estatement As Boolean)
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_AddCustAccount"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try

                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@AccountNo", AccountNo))
                Cmd.Parameters.Add(New SqlParameter("@ProdCode", ProdCode))
                Cmd.Parameters.Add(New SqlParameter("@CustNo", CustNo))
                Cmd.Parameters.Add(New SqlParameter("@Description", Description))
                Cmd.Parameters.Add(New SqlParameter("@Address", Address))
                Cmd.Parameters.Add(New SqlParameter("@City", City))
                Cmd.Parameters.Add(New SqlParameter("@Email", Email))
                Cmd.Parameters.Add(New SqlParameter("@Marketer", Marketer))
                Cmd.Parameters.Add(New SqlParameter("@Officer", Officer))
                Cmd.Parameters.Add(New SqlParameter("@TransStatus", TransStatus))
                Cmd.Parameters.Add(New SqlParameter("@HeadSubCode", HeadSubCode))
                Cmd.Parameters.Add(New SqlParameter("@AddedBy", AddedBy))
                Cmd.Parameters.Add(New SqlParameter("@OldAccount", OldAccount))
                Cmd.Parameters.Add(New SqlParameter("@DrRateMargin", DrRateMargin))
                Cmd.Parameters.Add(New SqlParameter("@CrRateMargin", CrRateMargin))
                Cmd.Parameters.Add(New SqlParameter("@Mandate", Mandate))
                Cmd.Parameters.Add(New SqlParameter("@Online", Online))
                Cmd.Parameters.Add(New SqlParameter("@Atm", ATm))
                Cmd.Parameters.Add(New SqlParameter("@Estatement", Estatement))
                Cmd.ExecuteNonQuery()
                OleConn.Close()

                Return "Request Executed for " & AccountNo
            Catch Exc As Exception
                Return Exc.Message
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function AddChartCode(ByVal BudgetCode As String, ByVal SubheadCode As String, ByVal ItemClass As String, ByVal Description As String, ByVal AccountType As String, ByVal CurrCode As String, ByVal ItemType As String, ByVal headoffice As Integer)
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_AddChartCode"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try

                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@BudgetCode", BudgetCode))
                Cmd.Parameters.Add(New SqlParameter("@SubheadCode", SubheadCode))
                Cmd.Parameters.Add(New SqlParameter("@ItemClass", ItemClass))
                Cmd.Parameters.Add(New SqlParameter("@Description", Description))
                Cmd.Parameters.Add(New SqlParameter("@AccountType", AccountType))
                Cmd.Parameters.Add(New SqlParameter("@CurrCode", CurrCode))
                Cmd.Parameters.Add(New SqlParameter("@ItemType", ItemType))
                Cmd.Parameters.Add(New SqlParameter("@HeadOffice", headoffice))

                Cmd.ExecuteNonQuery()
                OleConn.Close()

                Return "Request Successfully Executed"
            Catch Exc As Exception
                Return Exc.Message
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function AddFlexiAccount(ByVal AccountNo As String, ByVal Description As String, ByVal Address As String, ByVal City As String, ByVal Tel As String, ByVal Fax As String, ByVal Email As String, _
        ByVal ControlAccount As String, ByVal AccountType As String, ByVal AddedBy As String)
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_AddFlexiAccount"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try

                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@AccountNo", AccountNo))
                Cmd.Parameters.Add(New SqlParameter("@Description", Description))
                Cmd.Parameters.Add(New SqlParameter("@Address", Address))
                Cmd.Parameters.Add(New SqlParameter("@City", City))
                Cmd.Parameters.Add(New SqlParameter("@Tel", Tel))
                Cmd.Parameters.Add(New SqlParameter("@Fax", Fax))
                Cmd.Parameters.Add(New SqlParameter("@Email", Email))
                Cmd.Parameters.Add(New SqlParameter("@ControlAccount", ControlAccount))
                Cmd.Parameters.Add(New SqlParameter("@AccountType", AccountType))
                Cmd.Parameters.Add(New SqlParameter("@AddedBy", AddedBy))

                Cmd.ExecuteNonQuery()
                OleConn.Close()

                Return "Request Successfully Executed"
            Catch Exc As Exception
                Return Exc.Message
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function AddTransCode(ByVal TransCode As String, ByVal TransDesc As String, ByVal Trans As String, ByVal DefaultMain As String, ByVal MainFixed As Boolean, ByVal TransType As String, ByVal Narrative As String, ByVal Cash As Boolean, ByVal ChargeCot As Boolean, ByVal Batch As Boolean, ByVal DefaultContra As String, _
        ByVal DefaultFixed As Boolean, ByVal ContraNarrative As String, ByVal CountMain As Boolean, ByVal CountContra As Boolean, ByVal DraftTrans As Boolean, ByVal ScreenCode As String, ByVal ChargeCode As String, ByVal ChargeOnDebit As Boolean, ByVal LoopTrans As Boolean, ByVal LoopParentCode As String, _
        ByVal LoopLevel As Integer, ByVal ApplyToGl As Boolean, ByVal AddedBy As Boolean, ByVal RequiresApproval As Boolean) As String

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_AddTransCode"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@TransCode", TransCode))
                Cmd.Parameters.Add(New SqlParameter("@TransDesc", TransDesc))
                Cmd.Parameters.Add(New SqlParameter("@Trans", Trans))
                Cmd.Parameters.Add(New SqlParameter("@DefaultMain", DefaultMain))
                Cmd.Parameters.Add(New SqlParameter("@MainFixed", MainFixed))
                Cmd.Parameters.Add(New SqlParameter("@TransType", TransType))
                Cmd.Parameters.Add(New SqlParameter("@Narrative", Narrative))
                Cmd.Parameters.Add(New SqlParameter("@Cash", Cash))
                Cmd.Parameters.Add(New SqlParameter("@ChargeCot", ChargeCot))
                Cmd.Parameters.Add(New SqlParameter("@Batch", Batch))
                Cmd.Parameters.Add(New SqlParameter("@DefaultContra", DefaultContra))
                Cmd.Parameters.Add(New SqlParameter("@DefaultFixed", DefaultFixed))
                Cmd.Parameters.Add(New SqlParameter("@ContraNarrative", ContraNarrative))
                Cmd.Parameters.Add(New SqlParameter("@CountMain", CountMain))
                Cmd.Parameters.Add(New SqlParameter("@CountContra", CountContra))
                Cmd.Parameters.Add(New SqlParameter("@DraftTrans", DraftTrans))
                Cmd.Parameters.Add(New SqlParameter("@ScreenCode", ScreenCode))
                Cmd.Parameters.Add(New SqlParameter("@ChargeCode", ChargeCode))
                Cmd.Parameters.Add(New SqlParameter("@ChargeOnDebit", ChargeOnDebit))
                Cmd.Parameters.Add(New SqlParameter("@LoopTrans", LoopTrans))
                Cmd.Parameters.Add(New SqlParameter("@LoopParentCode", LoopParentCode))
                Cmd.Parameters.Add(New SqlParameter("@LoopLevel", LoopLevel))
                Cmd.Parameters.Add(New SqlParameter("@ApplyToGl", ApplyToGl))
                Cmd.Parameters.Add(New SqlParameter("@AddedBy", AddedBy))
                Cmd.Parameters.Add(New SqlParameter("@RequiresApproval", RequiresApproval))

                Cmd.ExecuteNonQuery()
                OleConn.Close()

                Return "Request Successfully Executed"
            Catch Exc As Exception
                Return Exc.Message
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function AddSupplier(ByVal supplier_id As String, ByVal supplier_name As String, ByVal supplier_addr As String, ByVal supplier_city As String, ByVal supplier_email As String, ByVal supplier_phones As String, ByVal last_modified_by As String) As String

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_AddSupplier"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try

                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@supplier_id", supplier_id))
                Cmd.Parameters.Add(New SqlParameter("@supplier_name", supplier_name))
                Cmd.Parameters.Add(New SqlParameter("@supplier_addr", supplier_addr))
                Cmd.Parameters.Add(New SqlParameter("@supplier_city", supplier_city))
                Cmd.Parameters.Add(New SqlParameter("@supplier_email", supplier_email))
                Cmd.Parameters.Add(New SqlParameter("@supplier_phones", supplier_phones))
                Cmd.Parameters.Add(New SqlParameter("@last_modified_by", last_modified_by))

                Cmd.ExecuteNonQuery()
                OleConn.Close()

                Return "Record Saved As " & supplier_id
            Catch Exc As Exception
                Return Exc.Message
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function AddFixedAssets(ByVal voucher_no As String, ByVal description As String, ByVal asset_class_code As String, ByVal supplier_id As String, ByVal purchase_date As Date, ByVal depr_start_date As Date, _
        ByVal depr_rate As Decimal, ByVal useful_life As Integer, ByVal nominal_value As Decimal, ByVal asset_cost As Decimal, ByVal qty As Integer, ByVal added_by As String, ByVal headsubcode As String) As String

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_AddFixedAssets"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try

                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@voucher_no", voucher_no))
                Cmd.Parameters.Add(New SqlParameter("@description", description))
                Cmd.Parameters.Add(New SqlParameter("@asset_class_code", asset_class_code))
                Cmd.Parameters.Add(New SqlParameter("@supplier_id", supplier_id))
                Cmd.Parameters.Add(New SqlParameter("@purchase_date", purchase_date))
                Cmd.Parameters.Add(New SqlParameter("@depr_start_date", depr_start_date))
                Cmd.Parameters.Add(New SqlParameter("@depr_rate", depr_rate))
                Cmd.Parameters.Add(New SqlParameter("@useful_life", useful_life))

                Cmd.Parameters.Add(New SqlParameter("@nominal_value", nominal_value))
                Cmd.Parameters.Add(New SqlParameter("@asset_cost", asset_cost))
                Cmd.Parameters.Add(New SqlParameter("@qty", qty))
                Cmd.Parameters.Add(New SqlParameter("@added_by", added_by))
                Cmd.Parameters.Add(New SqlParameter("@headsub_code", headsubcode))

                Cmd.ExecuteNonQuery()
                OleConn.Close()

                'Return "Successfully Generated, click Next to Maintain"
            Catch Exc As Exception
                Return Exc.Message
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function UpdateAsset(ByVal asset_id As String, ByVal headsub_code As String, ByVal location_id As String, ByVal serial_no As String, ByVal chasis_no As String, ByVal engine_no As String, _
        ByVal reg_no As String, ByVal supplier_id As String, ByVal description As String, ByVal last_modified_by As String) As String

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_UpdateAsset"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try

                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@asset_id", asset_id))
                Cmd.Parameters.Add(New SqlParameter("@headsub_code", headsub_code))
                Cmd.Parameters.Add(New SqlParameter("@location_id", location_id))
                Cmd.Parameters.Add(New SqlParameter("@serial_no", serial_no))
                Cmd.Parameters.Add(New SqlParameter("@chasis_no", chasis_no))
                Cmd.Parameters.Add(New SqlParameter("@engine_no", engine_no))
                Cmd.Parameters.Add(New SqlParameter("@reg_no", reg_no))
                Cmd.Parameters.Add(New SqlParameter("@supplier_id", supplier_id))
                Cmd.Parameters.Add(New SqlParameter("@description", description))
                Cmd.Parameters.Add(New SqlParameter("@last_modified_by", last_modified_by))

                Cmd.ExecuteNonQuery()
                OleConn.Close()

                Return "Successfully Saved"
            Catch Exc As Exception
                Return Exc.Message
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function GenerateateSchedule(ByVal AccountNo As String, ByVal SchedType As String, ByVal StartDate As Date, ByVal FreqCode As String, ByVal Repays As Integer, ByVal Tenor As Integer, _
        ByVal Memo As Boolean, ByVal Merge As Boolean, ByVal ServiceInt As Boolean, ByVal PostedBy As String, ByVal ApprovedBy As String, ByVal DealNo As String, ByVal ObserveLimit As Boolean, ByVal Equitycont As Decimal, _
        ByVal ODChargeCode As String, ByVal Recurrate As Decimal, ByVal RecurStartdate As Date, ByVal RecurFreqCode As String, _
        ByVal LastTransDate As Date, ByVal Balance As Decimal, ByVal Rate As Decimal, ByVal DealTenor As Integer, ByVal Effective As Date, ByVal YearBase As Integer) As String

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_GenerateSchedule"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try

                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@AccountNo", AccountNo))
                Cmd.Parameters.Add(New SqlParameter("@SchedType", SchedType))
                Cmd.Parameters.Add(New SqlParameter("@StartDate", StartDate))
                Cmd.Parameters.Add(New SqlParameter("@FreqCode", FreqCode))
                Cmd.Parameters.Add(New SqlParameter("@Repays", Repays))
                Cmd.Parameters.Add(New SqlParameter("@Tenor", Tenor))
                Cmd.Parameters.Add(New SqlParameter("@Memo", Memo))
                Cmd.Parameters.Add(New SqlParameter("@Merge", Merge))
                Cmd.Parameters.Add(New SqlParameter("@ServiceInt", ServiceInt))
                Cmd.Parameters.Add(New SqlParameter("@PostedBy", PostedBy))
                Cmd.Parameters.Add(New SqlParameter("@ApprovedBy", ApprovedBy))
                Cmd.Parameters.Add(New SqlParameter("@DealNo", DealNo))
                Cmd.Parameters.Add(New SqlParameter("@ObserveLimit", ObserveLimit))
                Cmd.Parameters.Add(New SqlParameter("@EquityCont", Equitycont))
                Cmd.Parameters.Add(New SqlParameter("@ODChargeCode", ODChargeCode))
                Cmd.Parameters.Add(New SqlParameter("@RecurRate", Recurrate))
                Cmd.Parameters.Add(New SqlParameter("@RecurStartDate", RecurStartdate))
                Cmd.Parameters.Add(New SqlParameter("@RecurFreqCode", RecurFreqCode))
                Cmd.Parameters.Add(New SqlParameter("@LastTransDate", LastTransDate))
                Cmd.Parameters.Add(New SqlParameter("@Balance", Balance))
                Cmd.Parameters.Add(New SqlParameter("@Rate", Rate))
                Cmd.Parameters.Add(New SqlParameter("@DealTenor", DealTenor))
                Cmd.Parameters.Add(New SqlParameter("@Effective", Effective))
                Cmd.Parameters.Add(New SqlParameter("@Base", YearBase))

                Cmd.ExecuteNonQuery()
                OleConn.Close()

                Return "Executed Successfully"
            Catch Exc As Exception
                Return Exc.Message
            Finally
                OleConn.Close()
            End Try
        End Function


        Public Shared Function SimulateSchedule(ByVal RefNo As String, ByVal SchedType As String, ByVal Balance As Decimal, ByVal Effective As Date, ByVal Tenor As Integer, ByVal DealTenor As Integer, _
        ByVal Rate As Decimal, ByVal Repays As Integer, ByVal ServiceInt As Boolean, ByVal Base As Integer, ByVal StartDate As Date, ByVal FreqCode As String, ByVal PostedBy As String, ByVal Remarks As String, _
        ByVal tenortype As String, ByVal maturity As Date, ByVal equitycont As Decimal, ByVal chargecode As String, ByVal recurrate As Decimal, ByVal recurstartdate As DateTime, ByVal recurfreqcode As String) As String

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_SimulateSchedule"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try

                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@RefNo", RefNo))
                Cmd.Parameters.Add(New SqlParameter("@SchedType", SchedType))
                Cmd.Parameters.Add(New SqlParameter("@Balance", Balance))
                Cmd.Parameters.Add(New SqlParameter("@Effective", Effective))
                Cmd.Parameters.Add(New SqlParameter("@Tenor", Tenor))
                Cmd.Parameters.Add(New SqlParameter("@DealTenor", DealTenor))
                Cmd.Parameters.Add(New SqlParameter("@Rate", Rate))
                Cmd.Parameters.Add(New SqlParameter("@Repays", Repays))
                Cmd.Parameters.Add(New SqlParameter("@ServiceInt", ServiceInt))
                Cmd.Parameters.Add(New SqlParameter("@StartDate", StartDate))
                Cmd.Parameters.Add(New SqlParameter("@Base", Base))
                Cmd.Parameters.Add(New SqlParameter("@FreqCode", FreqCode))
                Cmd.Parameters.Add(New SqlParameter("@PostedBy", PostedBy))
                Cmd.Parameters.Add(New SqlParameter("@Remarks", Remarks))
                Cmd.Parameters.Add(New SqlParameter("@TenorType", tenortype))
                Cmd.Parameters.Add(New SqlParameter("@Maturity", maturity))
                Cmd.Parameters.Add(New SqlParameter("@EquityCont", equitycont))
                Cmd.Parameters.Add(New SqlParameter("@ODChargeCode", chargecode))
                Cmd.Parameters.Add(New SqlParameter("@RecurRate", recurrate))
                Cmd.Parameters.Add(New SqlParameter("@RecurStartDate", recurstartdate))
                Cmd.Parameters.Add(New SqlParameter("@RecurFreqCode", recurfreqcode))

                Cmd.ExecuteNonQuery()
                OleConn.Close()

                Return "Executed Successfully"
            Catch Exc As Exception
                Return Exc.Message
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function AddShareholder(ByVal cust_no As String, ByVal inv_date As DateTime, ByVal pay_code As String, ByVal account_no As String, ByVal added_by As String) As String

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_AddShareholder"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@cust_no", cust_no))
                Cmd.Parameters.Add(New SqlParameter("@inv_date", inv_date))
                Cmd.Parameters.Add(New SqlParameter("@pay_code", pay_code))
                Cmd.Parameters.Add(New SqlParameter("@account_no", account_no))
                Cmd.Parameters.Add(New SqlParameter("@added_by", added_by))

                Cmd.ExecuteNonQuery()
                OleConn.Close()

                Return "Request Successfully Executed"
            Catch Exc As Exception
                Return Exc.Message
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function AddShareBatch(ByVal BatchDesc As String, ByVal UserId As String) As String

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_AddShareBatch"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@BatchDesc", BatchDesc))
                Cmd.Parameters.Add(New SqlParameter("@UserId", UserId))

                result = Cmd.Parameters.Add(New SqlParameter("@RefNo", SqlDbType.VarChar, 50))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return "Share Batch Reference is " & result.Value
            Catch Exc As Exception
                Return Exc.Message
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function AddShareBatchEntry(ByVal seq_no As Integer, ByVal trans_date As Date, ByVal cust_no As String, ByVal unit_price As Decimal, ByVal shares As Integer, ByVal total_price As Decimal, ByVal narrative As String, ByVal entered_by As String, ByVal ref_no As String, ByVal trans_code As String) As String
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_AddShareBatchEntry"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@seq_no", seq_no))
                Cmd.Parameters.Add(New SqlParameter("@trans_date", trans_date))
                Cmd.Parameters.Add(New SqlParameter("@cust_no", cust_no))
                Cmd.Parameters.Add(New SqlParameter("@unit_price", unit_price))
                Cmd.Parameters.Add(New SqlParameter("@shares", shares))
                Cmd.Parameters.Add(New SqlParameter("@total_price", total_price))
                Cmd.Parameters.Add(New SqlParameter("@narrative", narrative))
                Cmd.Parameters.Add(New SqlParameter("@entered_by", entered_by))
                Cmd.Parameters.Add(New SqlParameter("@ref_no", ref_no))
                Cmd.Parameters.Add(New SqlParameter("@trans_code", trans_code))

                Cmd.ExecuteNonQuery()

                OleConn.Close()

                Return "Entry Saved Successfully"
            Catch Exc As Exception
                Return Exc.Message
            Finally
                OleConn.Close()
            End Try
        End Function


        Public Shared Function UpdateShares(ByVal ref_no As String) As String
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_UpdateShares"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@ref_no", ref_no))

                Cmd.ExecuteNonQuery()

                OleConn.Close()

                Return "Updated Successfully"
            Catch Exc As Exception
                Return Exc.Message
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function GenerateShareBonusBatch(ByVal Num As Integer, ByVal Denom As Integer, ByVal CutOffDate As Date, ByVal Narrative As String, ByVal UserId As String) As String

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_GenerateShareBonusBatch"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@Num", Num))
                Cmd.Parameters.Add(New SqlParameter("@Denom", Denom))
                Cmd.Parameters.Add(New SqlParameter("@CutOffDate", CutOffDate))
                Cmd.Parameters.Add(New SqlParameter("@Narrative", Narrative))
                Cmd.Parameters.Add(New SqlParameter("@UserId", UserId))

                result = Cmd.Parameters.Add(New SqlParameter("@RefNo", SqlDbType.VarChar, 50))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return "Bonus Batch Reference is " & result.Value
            Catch Exc As Exception
                Return Exc.Message
            Finally
                OleConn.Close()
            End Try
        End Function


        Public Shared Function GenerateDividend(ByVal eps As Decimal, ByVal wht As Decimal, ByVal CutOffDate As Date, ByVal Narrative As String, ByVal UserId As String) As String

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_GenerateDividend"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@EarnPerShare", eps))
                Cmd.Parameters.Add(New SqlParameter("@Wht", wht))
                Cmd.Parameters.Add(New SqlParameter("@CutOffDate", CutOffDate))
                Cmd.Parameters.Add(New SqlParameter("@Narrative", Narrative))
                Cmd.Parameters.Add(New SqlParameter("@UserId", UserId))

                Cmd.ExecuteNonQuery()
                OleConn.Close()

                Return "Dividend Successfully Generated"
            Catch Exc As Exception
                Return Exc.Message
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function MapChartCode(ByVal FrTypeCode As String, ByVal MiniAccountNo As String, ByVal BalType As String, ByVal FrItemCode As String) As String

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_MapChartCode"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@FrTypeCode", FrTypeCode))
                Cmd.Parameters.Add(New SqlParameter("@MiniAccountNo", MiniAccountNo))
                Cmd.Parameters.Add(New SqlParameter("@BalType", BalType))
                Cmd.Parameters.Add(New SqlParameter("@FrItemCode", FrItemCode))

                Cmd.ExecuteNonQuery()
                OleConn.Close()
            Catch Exc As Exception
                Return Exc.Message
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function AddAccountMaintLog(ByVal AccountNo As String, ByVal AccountMaintType As String, ByVal NewValue As String, ByVal DispValue As String, _
        ByVal Reason As String, ByVal PostedBy As String, ByVal HeadSubCode As String, ByVal effective As Date, ByVal OldValue As String) As String

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_AddAccountMaintLog"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@AccountNo", AccountNo))
                Cmd.Parameters.Add(New SqlParameter("@AccountMaintType", AccountMaintType))
                Cmd.Parameters.Add(New SqlParameter("@NewValue", NewValue))
                Cmd.Parameters.Add(New SqlParameter("@DispValue", DispValue))
                Cmd.Parameters.Add(New SqlParameter("@Reason", Reason))
                Cmd.Parameters.Add(New SqlParameter("@PostedBy", PostedBy))
                Cmd.Parameters.Add(New SqlParameter("@HeadSubCode", HeadSubCode))
                Cmd.Parameters.Add(New SqlParameter("@Effective", effective))
                Cmd.Parameters.Add(New SqlParameter("@OldValue", OldValue))

                Cmd.ExecuteNonQuery()
                OleConn.Close()
            Catch Exc As Exception
                Return Exc.Message
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function ComputeAnnuity(ByVal Amount As Decimal, ByVal Rate As Decimal, ByVal Repays As Integer) As Decimal

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_ComputeAnnuity"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@Amount", Amount))
                Cmd.Parameters.Add(New SqlParameter("@Rate", Rate))
                Cmd.Parameters.Add(New SqlParameter("@Repays", Repays))

                result = Cmd.Parameters.Add(New SqlParameter("@RepayAmnt", SqlDbType.VarChar, 20))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return CDec(result.Value)
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function GetAccountLastInstrNo(ByVal account As String) As Integer

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_GetAccountLastInstrNo"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@AccountNo", account))

                result = Cmd.Parameters.Add(New SqlParameter("@LastNo", SqlDbType.VarChar, 20))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return CDec(result.Value)
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function RegenerateSchedule(ByVal accountno As String, ByVal dealno As String, ByVal seqno As String, ByVal simulate As Integer) As String

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_RegenerateSchedule"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@AccountNo", accountno))
                Cmd.Parameters.Add(New SqlParameter("@DealNo", dealno))
                Cmd.Parameters.Add(New SqlParameter("@Seq", seqno))
                Cmd.Parameters.Add(New SqlParameter("@Simulate", simulate))

                Cmd.ExecuteNonQuery()
                OleConn.Close()
            Catch Exc As Exception
                Return Exc.Message
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function ChangeLoanScheduleAmount(ByVal accountno As String, ByVal dealno As String, ByVal seqno As Integer, ByVal amount As Decimal, ByVal simulate As Integer) As String

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_ChangeLoanScheduleAmount"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@AccountNo", accountno))
                Cmd.Parameters.Add(New SqlParameter("@DealNo", dealno))
                Cmd.Parameters.Add(New SqlParameter("@SeqNo", seqno))
                Cmd.Parameters.Add(New SqlParameter("@Amount", amount))
                Cmd.Parameters.Add(New SqlParameter("@Simulate", simulate))

                Cmd.ExecuteNonQuery()
                OleConn.Close()
            Catch Exc As Exception
                Return Exc.Message
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function UndoSchedule(ByVal accountno As String, ByVal dealno As String, ByVal simulate As Integer) As String

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_UndoSchedule"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@AccountNo", accountno))
                Cmd.Parameters.Add(New SqlParameter("@DealNo", dealno))
                Cmd.Parameters.Add(New SqlParameter("@Simulate", simulate))

                Cmd.ExecuteNonQuery()
                OleConn.Close()
            Catch Exc As Exception
                Return Exc.Message
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function AddPostDatedCheques(ByVal value_date As Date, ByVal bank_code As String, ByVal cheque_no As String, ByVal amount As Decimal, ByVal narrative As String, ByVal account_no As String, ByVal bearer As String, ByVal posted_by As String, ByVal headsub_code As String) As String

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_AddPostDatedCheques"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@value_date", value_date))
                Cmd.Parameters.Add(New SqlParameter("@bank_code", bank_code))
                Cmd.Parameters.Add(New SqlParameter("@cheque_no", cheque_no))
                Cmd.Parameters.Add(New SqlParameter("@amount", amount))
                Cmd.Parameters.Add(New SqlParameter("@narrative", narrative))
                Cmd.Parameters.Add(New SqlParameter("@account_no", account_no))
                Cmd.Parameters.Add(New SqlParameter("@bearer", bearer))
                Cmd.Parameters.Add(New SqlParameter("@posted_by", posted_by))
                Cmd.Parameters.Add(New SqlParameter("@headsub_code", headsub_code))

                Cmd.ExecuteNonQuery()
                OleConn.Close()

                Return "Record Successfully Saved"
            Catch Exc As Exception
                Return Exc.Message
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function AddFAAdjustMain(ByVal trans_code As String, ByVal asset_class_code As String, ByVal start_no As Decimal, ByVal end_no As Decimal, ByVal narrative As String, ByVal amount As Decimal, ByVal posted_by As String, ByVal headsub_code As String) As String

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = gUtils.DbSqlConnectionString()

            Cmd.CommandText = "Sproc_AddFAAdjustMain"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@trans_code", trans_code))
                Cmd.Parameters.Add(New SqlParameter("@asset_class_code", asset_class_code))
                Cmd.Parameters.Add(New SqlParameter("@start_no", start_no))
                Cmd.Parameters.Add(New SqlParameter("@end_no", end_no))
                Cmd.Parameters.Add(New SqlParameter("@narrative", narrative))
                Cmd.Parameters.Add(New SqlParameter("@amount", amount))
                Cmd.Parameters.Add(New SqlParameter("@posted_by", posted_by))
                Cmd.Parameters.Add(New SqlParameter("@headsub_code", headsub_code))

                result = Cmd.Parameters.Add(New SqlParameter("@RefNo", SqlDbType.VarChar, 50))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return result.Value

            Catch Exc As Exception
                Return Exc.Message
            Finally
                OleConn.Close()
            End Try
        End Function
    End Class

    Public Class eFASS
        Public Shared Function MapItem(ByVal itemcode As String, ByVal statementcode As String, ByVal startno As String, ByVal endno As String, ByVal addition As Integer, ByVal baltype As String, ByVal seq As Integer) As String

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = System.Configuration.ConfigurationSettings.AppSettings("eFassConnect")

            Cmd.CommandText = "eFASS_MapItem"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@ItemCode", itemcode))
                Cmd.Parameters.Add(New SqlParameter("@StatementCode", statementcode))
                Cmd.Parameters.Add(New SqlParameter("@StartChartNo", startno))
                Cmd.Parameters.Add(New SqlParameter("@EndChartNo", endno))
                Cmd.Parameters.Add(New SqlParameter("@Addition", addition))
                Cmd.Parameters.Add(New SqlParameter("@BalType", baltype))
                Cmd.Parameters.Add(New SqlParameter("@SysSeq", seq))

                Cmd.ExecuteNonQuery()
                OleConn.Close()

                Return "Saved Successful"

            Catch Exc As Exception
                Return Exc.Message
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function eFASS_SaveInstituteRecord(ByVal code As String, ByVal names As String, ByVal auth As String, ByVal position As String, _
         ByVal contact As String, ByVal telno As String, ByVal eFassConn As String) As String

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = eFassConn

            Cmd.CommandText = "eFASS_SaveInstituteRecord"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@Institute_Code", code))
                Cmd.Parameters.Add(New SqlParameter("@Institute_Name", names))
                Cmd.Parameters.Add(New SqlParameter("@Auth_Name", auth))
                Cmd.Parameters.Add(New SqlParameter("@Auth_Position", position))
                Cmd.Parameters.Add(New SqlParameter("@Contact", contact))
                Cmd.Parameters.Add(New SqlParameter("@Tel_No", telno))

                Cmd.ExecuteNonQuery()
                OleConn.Close()

                Return "Record Saved"
            Catch Exc As Exception
                Return Exc.Message
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function eFASS_GetItemTypeID(ByVal itemcode As String, ByVal eFassConn As String) As String

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = eFassConn

            Cmd.CommandText = "eFASS_GetItemTypeID"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@ItemCode", itemcode))

                result = Cmd.Parameters.Add(New SqlParameter("@ItemTypeID", SqlDbType.VarChar, 20))
                result.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                OleConn.Close()

                Return result.Value
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function eFASS_PostAdjustment(ByVal debitacct As String, ByVal creditacct As String, ByVal reason As String, ByVal amount As Decimal, ByVal eFassConn As String) As String

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = eFassConn

            Cmd.CommandText = "eFASS_PostAdjustment"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@DebitAcct", debitacct))
                Cmd.Parameters.Add(New SqlParameter("@CreditAcct", creditacct))
                Cmd.Parameters.Add(New SqlParameter("@Reason", reason))
                Cmd.Parameters.Add(New SqlParameter("@Amount", amount))

                Cmd.ExecuteNonQuery()
                OleConn.Close()

                Return "Adjustment Saved"
            Catch Exc As Exception
                Return Exc.Message
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function eFASS_AddTerminatedStaff(ByVal staffcode As String, ByVal surname As String, ByVal firstname As String, ByVal middlename As String, ByVal designation As String, ByVal address As String, _
        ByVal birthdate As Date, ByVal state As String, ByVal branch As String, ByVal dept As String, ByVal termdate As Date, _
        ByVal reason As String, ByVal fraudtype As String, ByVal eFassConn As String) As String

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = eFassConn

            Cmd.CommandText = "eFASS_AddTerminatedStaff"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@EXSTAFF_CODE", staffcode))
                Cmd.Parameters.Add(New SqlParameter("@SURNAME", surname))
                Cmd.Parameters.Add(New SqlParameter("@FIRST_NAME", firstname))
                Cmd.Parameters.Add(New SqlParameter("@MIDDLE_NAME", middlename))
                Cmd.Parameters.Add(New SqlParameter("@DESIGNATION", designation))
                Cmd.Parameters.Add(New SqlParameter("@PERM_HOME_ADDRESS", address))
                Cmd.Parameters.Add(New SqlParameter("@DATE_OF_BIRTH", birthdate))
                Cmd.Parameters.Add(New SqlParameter("@STATE_CODE", state))
                Cmd.Parameters.Add(New SqlParameter("@BRANCH_CODE", branch))
                Cmd.Parameters.Add(New SqlParameter("@DEPT", dept))
                Cmd.Parameters.Add(New SqlParameter("@DATE_TERMINATED", termdate))
                Cmd.Parameters.Add(New SqlParameter("@REASON_FOR_TERMINATED", reason))
                Cmd.Parameters.Add(New SqlParameter("@FRAUD_TYPE_CODE", fraudtype))

                Cmd.ExecuteNonQuery()
                OleConn.Close()

                Return "Record Saved"
            Catch Exc As Exception
                Return Exc.Message
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function eFASS_AddPrevWorkPlace(ByVal Sysseq As Integer, ByVal staffcode As String, ByVal orgname As String, ByVal address As String, _
        ByVal telephone As String, ByVal periodfrom As Date, ByVal periodto As Date, ByVal eFassConn As String) As String

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = eFassConn

            Cmd.CommandText = "eFASS_AddPrevWorkPlace"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@SYSSEQ", Sysseq))
                Cmd.Parameters.Add(New SqlParameter("@EXSTAFF_CODE", staffcode))
                Cmd.Parameters.Add(New SqlParameter("@NAME_OF_ORG", orgname))
                Cmd.Parameters.Add(New SqlParameter("@ADDRESS", address))
                Cmd.Parameters.Add(New SqlParameter("@TELEPHONE", telephone))
                Cmd.Parameters.Add(New SqlParameter("@PERIOD_FROM", periodfrom))
                Cmd.Parameters.Add(New SqlParameter("@PERIOD_TO", periodto))

                Cmd.ExecuteNonQuery()
                OleConn.Close()

                Return "Record Saved"
            Catch Exc As Exception
                Return Exc.Message
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function eFASS_AddNextOfKin(ByVal Sysseq As Integer, ByVal staffcode As String, ByVal surname As String, ByVal firstname As String, ByVal middlename As String, ByVal address As String, ByVal eFassConn As String)

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = eFassConn

            Cmd.CommandText = "eFASS_AddNextOfKin"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@SYSSEQ", Sysseq))
                Cmd.Parameters.Add(New SqlParameter("@EXSTAFF_CODE", staffcode))
                Cmd.Parameters.Add(New SqlParameter("@SURNAME", surname))
                Cmd.Parameters.Add(New SqlParameter("@FIRST_NAME", firstname))
                Cmd.Parameters.Add(New SqlParameter("@MIDDLE_NAME", middlename))
                Cmd.Parameters.Add(New SqlParameter("@ADDRESS", address))

                Cmd.ExecuteNonQuery()
                OleConn.Close()

                Return "Record Saved"
            Catch Exc As Exception
                Return Exc.Message
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function eFASS_AddFraud(ByVal Sysseq As Integer, ByVal branch As String, ByVal fraudtypecode As String, ByVal datecommitted As Date, ByVal amount As Decimal, _
        ByVal loss As Decimal, ByVal description As String, ByVal action As String, ByVal actiondate As Date, ByVal eFassConn As String)

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = eFassConn

            Cmd.CommandText = "eFASS_AddFraud"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@SYSTEM_ID", Sysseq))
                Cmd.Parameters.Add(New SqlParameter("@BRANCH_CODE", branch))
                Cmd.Parameters.Add(New SqlParameter("@FRAUD_TYPE_CODE", fraudtypecode))
                Cmd.Parameters.Add(New SqlParameter("@DATE_FRAUD_WAS_COMMITED", datecommitted))
                Cmd.Parameters.Add(New SqlParameter("@AMOUNT_INVOLVED_NAIRA", amount))
                Cmd.Parameters.Add(New SqlParameter("@IF_SUCESSFUL_PARTIALLY_SUCCESSFUL_ACTUAL_LOSS_NAIRA", loss))
                Cmd.Parameters.Add(New SqlParameter("@BRIEF_DESC", description))
                Cmd.Parameters.Add(New SqlParameter("@ACTION_TAKEN", action))
                Cmd.Parameters.Add(New SqlParameter("@ACTION_DATE", actiondate))

                Cmd.ExecuteNonQuery()
                OleConn.Close()

                Return "Record Saved"
            Catch Exc As Exception
                Return Exc.Message
            Finally
                OleConn.Close()
            End Try
        End Function

        Public Shared Function eFASS_AddStaffInvolved(ByVal Sysseq As Integer, ByVal firstname As String, ByVal middlename As String, ByVal lastname As String, ByVal staffcode As String, _
        ByVal category As String, ByVal nationality As String, ByVal passport As String, ByVal address As String, ByVal sex As String, ByVal birthdate As Date, _
        ByVal statecode As String, ByVal idno As String, ByVal systemid As Integer, ByVal eFassConn As String)

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = eFassConn

            Cmd.CommandText = "eFASS_AddStaffInvolved"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@SysSeq", Sysseq))
                Cmd.Parameters.Add(New SqlParameter("@FIRST_NAME", firstname))
                Cmd.Parameters.Add(New SqlParameter("@MIDDLE_NAME", middlename))
                Cmd.Parameters.Add(New SqlParameter("@LAST_NAME", lastname))
                Cmd.Parameters.Add(New SqlParameter("@STAFF_CODE", staffcode))
                Cmd.Parameters.Add(New SqlParameter("@CATEGORY", category))
                Cmd.Parameters.Add(New SqlParameter("@NATIONALITY", nationality))
                Cmd.Parameters.Add(New SqlParameter("@PASSPORT_NO", passport))
                Cmd.Parameters.Add(New SqlParameter("@PERM_HOME_ADDRESS", address))
                Cmd.Parameters.Add(New SqlParameter("@SEX", sex))
                Cmd.Parameters.Add(New SqlParameter("@DATE_OF_BIRTH", birthdate))
                Cmd.Parameters.Add(New SqlParameter("@STATE_OF_ORIGIN_CODE", statecode))
                Cmd.Parameters.Add(New SqlParameter("@NATIONAL_ID_NO", idno))
                Cmd.Parameters.Add(New SqlParameter("@SYSTEM_ID", systemid))

                Cmd.ExecuteNonQuery()
                OleConn.Close()

                Return "Record Saved"
            Catch Exc As Exception
                Return Exc.Message
            Finally
                OleConn.Close()
            End Try
        End Function

    End Class
End Namespace
