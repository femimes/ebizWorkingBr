Imports System.Data
Imports System
Imports System.Data.OleDb
Imports System.Security.Principal
Imports System.Data.SqlClient
Imports System.Web.Security
Imports System.Web.SessionState
Imports System.Math
Imports Microsoft.VisualBasic
Imports Microsoft
Imports System.Configuration.ConfigurationManager
Imports System.Web
Imports System.Configuration

Namespace NETClass


    Public Class Dbconnect
        Public str_con As String
        Public con As New SqlConnection
        'Shared Myconnect As String
        Shared Property Myconnect() As SqlConnection

            Set(ByVal Value As SqlConnection)
                'Helps to trap the database Connection String

            End Set

            Get
                Dim Mystr_con As String
                Dim mycon As SqlConnection
                ' mycon.
                'Mystr_con = ConfigurationSettings.AppSettings("OleConnect")

                Mystr_con = ConnectionStrings("EazyConnect").ConnectionString

                If mycon Is Nothing Then

                    mycon = New SqlClient.SqlConnection(Mystr_con)

                End If

                If mycon.State = ConnectionState.Closed Then

                    mycon.Open()

                End If
                'Retrieve the database Connection String from web.config
                Return mycon 'de_ConnectionString

            End Get
        End Property

        Public Shared Function shortlen(ByVal l_desc As String) As String
            If l_desc.Length <= 30 Then

                Return l_desc
            Else
                Return l_desc.Substring(0, 30) & " ..."

            End If
        End Function

        Public Shared Function dateformat(ByVal nData As String)
            Dim sCent As String, sYr As String, sMth As String, sDay As String

            dateformat = "01012000"
            If Len(nData) = 7 Then
                sCent = Mid(nData, 1, 1)
                sYr = Format(Mid(nData, 2, 2), "00")
                sMth = Format(Mid(nData, 4, 2), "00")
                sDay = Format(Mid(nData, 6, 2), "00")

                If sCent = "9" Then
                    dateformat = "Open"
                ElseIf sCent = "1" Then
                    dateformat = CStr(sDay) + "-" + CStr(sMth) + "-20" + CStr(sYr)
                Else
                    dateformat = CStr(sDay) + "-" + CStr(sMth) + "-19" + CStr(sYr)
                End If
            ElseIf Len(nData) = 6 Then
                sYr = Format(Mid(nData, 1, 2), "00")
                sMth = Format(Mid(nData, 3, 2), "00")
                sDay = Format(Mid(nData, 5, 2), "00")
                dateformat = CStr(sDay) + "-" + CStr(sMth) + "-19" + CStr(sYr)
            End If
        End Function
        Public Shared Function formatdate(ByVal nData As Date)
            Dim sCent As String, sYr As String, sMth As String, sDay As String
            Dim dDate As Date

            dDate = nData ' CDate(nData)
            sCent = "1"
            If Year(dDate) < 2000 Then sCent = "0"

            sMth = Format(dDate.Month, "00")
            sYr = Right(dDate.Year, 2)
            sDay = Format(dDate.Day, "00")
            formatdate = sCent + sYr + sMth + sDay
        End Function

        Public Shared Function LeapYear(ByVal nYear As Integer) As Integer
            If ((nYear Mod 4) = 0) And (((nYear Mod 100) <> 100) Or ((nYear Mod 400) = 0)) Then
                LeapYear = 1
            Else
                LeapYear = 0
            End If
        End Function

        Public Function datebuild(ByVal dDate As Date) As Long
            Dim sCent As String, nYr As String, nMth As String, nDay As String

            sCent = "0"
            If Year(dDate) > 1999 Then sCent = "1"
            nYr = Right(CStr(Year(dDate)), 2)
            nMth = Right(Format(CStr(Month(dDate)), "00"), 2)
            nDay = Right(Format(CStr(Day(dDate)), "00"), 2)

            datebuild = CLng(sCent + nYr + nMth + nDay)

        End Function

        Sub MessageLine(ByVal Msg As String)
            Beep()
            Beep()
            ' MsgBox(Msg, 16, "Information")
        End Sub

        Public Shared Function Access(ByVal l_user As String, ByVal l_Access As String, ByVal l_module As String) As Boolean
            Dim ds As New DataSet
            'Dim flag As Boolean = False

            Dim l_sqlref As String = "sp_UserAccess_sel '" & l_user & "','" & l_Access & "','" & l_module & "'"


            'Dim cmd_chk As New sqlDataAdapter(l_sqlref, Dbconnect.Myconnect)
            'cmd_chk.Fill(ds, "tbl_chk")
            'If ds.Tables("tbl_chk").Rows.Count = 0 Then

            'Else
            '    flag = True

            'End If
            'Return flag
            Dim objCmd As New SqlCommand(l_sqlref, Dbconnect.Myconnect)
            Dim flag As Boolean = True
            Dim objDR As SqlDataReader
            Try
                Dim cmd_chk As New SqlDataAdapter(l_sqlref, Dbconnect.Myconnect)
                objDR = objCmd.ExecuteReader(CommandBehavior.CloseConnection)
                If objDR.Read Then
                    flag = False
                    Return flag

                Else
                    Return flag
                End If

            Catch ex As Exception
                Return flag
            End Try


        End Function
        Public Shared Sub logUserAccess(ByVal l_user As String, ByVal l_screenurl As String, ByVal l_bolaccess As String)
            Dim ds As New DataSet
            Dim str_log As String = "sp_UserActivity_log '" & l_user & "','" & l_screenurl & "','" & Today.Date & "','" & Today.Now.ToLongTimeString & "','" & l_bolaccess & "'"
            'Dbconnect.FxSql(str_log, Dbconnect.Myconnect)
            Dim cmd As New SqlDataAdapter(str_log, Dbconnect.Myconnect)
            cmd.Fill(ds)

        End Sub
        Public Shared Function IAccess(ByVal l_User As String, ByVal l_ScreenUrl As String) As Boolean
            Dim ds As New DataSet
            'Dim flag As Boolean = False
            ' session("org_code")

            Dim l_pos As Integer = 0, l_papos As Integer
            Dim str_val As String = l_ScreenUrl
            l_pos = str_val.LastIndexOf("/")
            l_ScreenUrl = l_ScreenUrl.Substring(l_pos + 1)
            l_papos = l_ScreenUrl.LastIndexOf("?")


            If l_papos > 0 Then

                l_ScreenUrl = l_ScreenUrl.Remove(l_papos, (l_ScreenUrl.Length - l_papos))
            Else

            End If
            Dim str_user As String = l_User
            l_pos = str_user.LastIndexOf("\")

            l_User = l_User.Substring(l_pos + 1)

            Dim l_sqlref As String = "Sproc_GetMenuWebItems '" & l_User & "','" & l_ScreenUrl & "'"


            'Dim cmd_chk As New sqlDataAdapter(l_sqlref, Dbconnect.Myconnect)
            'cmd_chk.Fill(ds, "tbl_chk")
            'If ds.Tables("tbl_chk").Rows.Count = 0 Then

            'Else
            '    flag = True

            'End If
            'Return flag
            Dim objCmd As New SqlClient.SqlCommand(l_sqlref, Dbconnect.Myconnect)
            Dim flag As Boolean = True
            Dim objDR As SqlDataReader
            Try
                Dim cmd_chk As New SqlDataAdapter(l_sqlref, Dbconnect.Myconnect)
                objDR = objCmd.ExecuteReader(CommandBehavior.CloseConnection)
                If objDR.Read Then
                    flag = False
                    'logUserAccess(l_User, l_ScreenUrl, flag)
                    Return flag

                Else
                    ' logUserAccess(l_User, l_ScreenUrl, flag)
                    Return flag
                End If
                ' logUserAccess(l_User, l_ScreenUrl, flag)


            Catch ex As Exception
                Return flag
            End Try


        End Function
        Public Shared Function IAccessMystr(ByVal l_User As String, ByVal l_ScreenUrl As String) As String
            Dim ds As New DataSet
            'Dim flag As Boolean = False

            Dim l_pos As Integer = 0, l_papos As Integer
            Dim str_val As String = l_ScreenUrl
            l_pos = str_val.LastIndexOf("/")
            l_ScreenUrl = l_ScreenUrl.Substring(l_pos + 1)
            l_papos = l_ScreenUrl.LastIndexOf("?")


            If l_papos > 0 Then

                l_ScreenUrl = l_ScreenUrl.Remove(l_papos, (l_ScreenUrl.Length - l_papos))
            Else

            End If
            Dim str_user As String = l_User
            l_pos = str_user.LastIndexOf("\")

            l_User = l_User.Substring(l_pos + 1)

            Dim l_sqlref As String = "Sproc_GetMenuWebItems '" & l_User & "','" & l_ScreenUrl & "'"


            'Dim cmd_chk As New sqlDataAdapter(l_sqlref, Dbconnect.Myconnect)
            'cmd_chk.Fill(ds, "tbl_chk")
            'If ds.Tables("tbl_chk").Rows.Count = 0 Then

            'Else
            '    flag = True

            'End If
            'Return flag
            Dim objCmd As New SqlCommand(l_sqlref, Dbconnect.Myconnect)
            Dim flag As Boolean = True
            Dim objDR As SqlDataReader
            Try
                Dim cmd_chk As New SqlDataAdapter(l_sqlref, Dbconnect.Myconnect)
                objDR = objCmd.ExecuteReader(CommandBehavior.CloseConnection)
                If objDR.Read Then
                    flag = False
                    'logUserAccess(l_User, l_ScreenUrl, flag)
                    Return l_sqlref

                Else
                    ' logUserAccess(l_User, l_ScreenUrl, flag)
                    Return l_sqlref
                End If
                ' logUserAccess(l_User, l_ScreenUrl, flag)


            Catch ex As Exception
                Return flag
            End Try


        End Function

        Public Shared Function IAccessApproval(ByVal l_User As String, ByVal l_ScreenUrl As String) As Boolean
            Dim ds As New DataSet
            'Dim flag As Boolean = False

            Dim l_pos As Integer = 0, l_papos As Integer
            Dim str_val As String = l_ScreenUrl
            l_pos = str_val.LastIndexOf("/")
            l_ScreenUrl = l_ScreenUrl.Substring(l_pos + 1)
            l_papos = l_ScreenUrl.LastIndexOf("?")


            If l_papos > 0 Then

                l_ScreenUrl = l_ScreenUrl.Remove(l_papos, (l_ScreenUrl.Length - l_papos))
            Else

            End If
            Dim str_user As String = l_User
            l_pos = str_user.LastIndexOf("\")

            l_User = l_User.Substring(l_pos + 1)

            Dim l_sqlref As String = "Sproc_Param_UserAppStatusSecReq_sel '" & l_User & "','" & l_ScreenUrl & "'"


            'Dim cmd_chk As New sqlDataAdapter(l_sqlref, Dbconnect.Myconnect)
            'cmd_chk.Fill(ds, "tbl_chk")
            'If ds.Tables("tbl_chk").Rows.Count = 0 Then

            'Else
            '    flag = True

            'End If
            'Return flag
            Dim objCmd As New SqlCommand(l_sqlref, Dbconnect.Myconnect)
            Dim flag As Boolean = True
            Dim objDR As SqlDataReader
            Try
                Dim cmd_chk As New SqlDataAdapter(l_sqlref, Dbconnect.Myconnect)
                objDR = objCmd.ExecuteReader(CommandBehavior.CloseConnection)
                If objDR.Read Then
                    flag = False
                    logUserAccess(l_User, l_ScreenUrl, flag)
                    Return flag

                Else
                    logUserAccess(l_User, l_ScreenUrl, flag)
                    Return flag
                End If
                ' logUserAccess(l_User, l_ScreenUrl, flag)


            Catch ex As Exception
                Return flag
            End Try


        End Function

        Public Shared Function IAccessCommit(ByVal l_User As String, ByVal l_ScreenUrl As String) As Boolean
            Dim ds As New DataSet
            'Dim flag As Boolean = False
            Dim l_pos As Integer = 0
            Dim str_val As String = l_ScreenUrl

            l_pos = str_val.LastIndexOf("/")
            l_ScreenUrl = l_ScreenUrl.Substring(l_pos + 1)
            Dim str_user As String = l_User
            l_pos = str_user.LastIndexOf("\")

            l_User = l_User.Substring(l_pos + 1)

            Dim l_sqlref As String = "Sproc_Param_UserSecReqCommit_sel '" & l_User & "','" & l_ScreenUrl & "'"


            'Dim cmd_chk As New sqlDataAdapter(l_sqlref, Dbconnect.Myconnect)
            'cmd_chk.Fill(ds, "tbl_chk")
            'If ds.Tables("tbl_chk").Rows.Count = 0 Then

            'Else
            '    flag = True

            'End If
            'Return flag
            Dim objCmd As New SqlCommand(l_sqlref, Dbconnect.Myconnect)
            Dim flag As Boolean = True
            Dim objDR As SqlDataReader
            Try
                Dim cmd_chk As New SqlDataAdapter(l_sqlref, Dbconnect.Myconnect)
                objDR = objCmd.ExecuteReader(CommandBehavior.CloseConnection)
                If objDR.Read Then
                    flag = False
                    logUserAccess(l_User, l_ScreenUrl, flag)
                    Return flag

                Else
                    logUserAccess(l_User, l_ScreenUrl, flag)
                    Return flag
                End If

            Catch ex As Exception
                Return flag
            End Try


        End Function

        Public Shared Function IAccess2(ByVal l_User As String, ByVal l_ScreenUrl As String) As String
            Dim ds As New DataSet
            'Dim flag As Boolean = False
            Dim l_pos As Integer = 0, l_papos As Integer
            Dim str_val As String = l_ScreenUrl
            l_pos = str_val.LastIndexOf("/")
            l_ScreenUrl = l_ScreenUrl.Substring(l_pos + 1)
            l_papos = l_ScreenUrl.LastIndexOfAny("?")


            If l_papos > 0 Then

                l_ScreenUrl.Remove(l_papos, (l_ScreenUrl.Length - l_papos))
            Else
            End If
            Dim l_sqlref As String = "Sproc_Param_UserSecReq_sel '" & l_User & "','" & l_ScreenUrl & "'"


            'Dim cmd_chk As New sqlDataAdapter(l_sqlref, Dbconnect.Myconnect)
            'cmd_chk.Fill(ds, "tbl_chk")
            'If ds.Tables("tbl_chk").Rows.Count = 0 Then

            'Else
            '    flag = True

            'End If
            'Return flag
            Dim objCmd As New SqlCommand(l_sqlref, Dbconnect.Myconnect)
            Dim flag As Boolean = True
            Dim objDR As SqlDataReader
            Try
                Dim cmd_chk As New SqlDataAdapter(l_sqlref, Dbconnect.Myconnect)
                objDR = objCmd.ExecuteReader(CommandBehavior.CloseConnection)
                If objDR.Read Then
                    flag = False
                    Return l_sqlref

                Else
                    Return l_sqlref
                End If

            Catch ex As Exception
                Return flag
            End Try


        End Function

        Public Shared Function Role(ByVal l_user As String, ByVal l_Access As String, ByVal l_module As String, ByVal l_ApprovalRight As String) As Boolean
            Dim ds As New DataSet
            Dim flag As Boolean = False

            Dim l_sqlref As String = "sp_UserAccessRole_sel '" & l_user & "','" & l_Access & "','" & l_module & "','" & l_ApprovalRight & "'"


            Dim cmd_chk As New SqlDataAdapter(l_sqlref, Dbconnect.Myconnect)
            cmd_chk.Fill(ds, "tbl_chk")
            If ds.Tables("tbl_chk").Rows.Count = 0 Then

            Else
                flag = True

            End If
            Return flag

        End Function



        Public Shared Function FxSql(ByVal spquery As String, ByVal spcon As SqlConnection) As DataSet
            Dim ds As New DataSet
            Try
                Dim cmd_chk As New SqlDataAdapter(spquery, spcon)
                cmd_chk.Fill(ds, "tbl_chk")
                'If ds.Tables("tbl_chk").Rows.Count = 0 Then
                '    ds = Nothing

                'End If
                Return ds
                spcon.Close()
            Catch ex As Exception


            End Try


        End Function

        Public Shared Function MsgError(ByVal ErrCode As String, ByVal MsgIndex As Integer) As String


            Try
                Dim str_sel As String = "sproc_FsError_sel '" & ErrCode & "'"
                Dbconnect.FxSql(str_sel, Dbconnect.Myconnect)

                Return Dbconnect.FxSql(str_sel, Dbconnect.Myconnect).Tables(0).Rows(0).Item(MsgIndex).ToString

            Catch ex As Exception
                Return "No Error Message"

            End Try
        End Function
        Public Shared Function FxSqlReader(ByVal spquery As String, ByVal spcon As SqlConnection) As SqlDataReader
            Dim objCmd As New SqlCommand(spquery, spcon)
            Dim objDR As SqlDataReader
            Try
                Dim cmd_chk As New SqlDataAdapter(spquery, spcon)
                objDR = objCmd.ExecuteReader(CommandBehavior.CloseConnection)
                Return objDR

            Catch ex As Exception
                Return Nothing
            End Try


        End Function
        Public Shared Function FxSqlBoolean(ByVal spquery As String, ByVal spcon As SqlConnection) As Boolean
            Dim objCmd As New SqlCommand(spquery, spcon)
            Dim flag As Boolean = True
            Dim objDR As SqlDataReader
            Try
                Dim cmd_chk As New SqlDataAdapter(spquery, spcon)
                objDR = objCmd.ExecuteReader(CommandBehavior.CloseConnection)
                If objDR.Read Then
                    flag = False
                    Return flag

                Else
                    Return flag
                End If

            Catch ex As Exception
                Return flag
            End Try


        End Function
        Public Shared Function MetaLogon(ByVal l_DomainId As String) As SqlDataReader

            Dim str_logon As String = "sproc_MetaData_employeeInfo '" & l_DomainId & "'"

            Dim objCmd As New SqlCommand(str_logon, Dbconnect.Myconnect)
            Dim objDR As SqlDataReader

            Try
                Dim cmd_chk As New SqlDataAdapter(str_logon, Dbconnect.Myconnect)
                objDR = objCmd.ExecuteReader(CommandBehavior.CloseConnection)
                Return objDR

            Catch ex As Exception
                Return Nothing
            End Try


        End Function


        Shared Property mydisconnect() As SqlConnection
            Get
                Dim Mystr_con As String
                Dim mycon As SqlConnection
                Mystr_con = ConfigurationSettings.AppSettings("FixedAsset")
                mycon.Close()
                Return mycon 'de_ConnectionString

            End Get
            Set(ByVal Value As SqlConnection)

            End Set
        End Property



    End Class

    Public Class UserProfile

        Shared Property Location() As String
            Get
                Dim Mystr_con As String
                'Dim str_sel As String = "sp_metalogon '" & sga_marshallweb.UserProfile.IUserAuthName  & "'"
                Dim l_pos As Integer, l_user As String = UserProfile.IUserAuthName
                Dim str_user As String = UserProfile.IUserAuthName
                l_pos = str_user.LastIndexOf("\")

                l_user = l_user.Substring(l_pos + 1)

                Dim str_sel As String = "sp_metalogon '" & l_user & "'"
                If Dbconnect.FxSqlBoolean(str_sel, Dbconnect.Myconnect) = True Then
                    Return Nothing

                Else
                    Dim l_return As String = Dbconnect.FxSql(str_sel, Dbconnect.Myconnect).Tables(0).Rows(0).Item("Branch")

                    Return l_return 'de_ConnectionString
                End If
            End Get
            Set(ByVal Value As String)

            End Set
        End Property

        Shared Property IUserAuthName() As String
            Get
                Dim Mystr_con As String, AuthUserName As String
                Dim Str_type As String = System.Configuration.ConfigurationSettings.AppSettings("UserAuthType")

                If Str_type = "Windows" Then
                    AuthUserName = WindowsIdentity.GetCurrent.Name
                Else

                    AuthUserName = HttpContext.Current.User.Identity.Name 'User.Identity.Name

                End If
                Return AuthUserName



            End Get
            Set(ByVal Value As String)

            End Set
        End Property

        Shared Property LocationCode() As String
            Get
                Dim l_pos As Integer, l_user As String = UserProfile.IUserAuthName
                Dim Mystr_con As String
                Dim str_user As String = UserProfile.IUserAuthName
                l_pos = str_user.LastIndexOf("\")

                l_user = l_user.Substring(l_pos + 1)

                Dim str_sel As String = "sp_metalogon '" & l_user & "'"
                If Dbconnect.FxSqlBoolean(str_sel, Dbconnect.Myconnect) = True Then
                    Return Nothing

                Else
                    Dim l_return As String = Dbconnect.FxSql(str_sel, Dbconnect.Myconnect).Tables(0).Rows(0).Item("Location_code")

                    Return l_return 'de_ConnectionString

                End If

            End Get
            Set(ByVal Value As String)

            End Set
        End Property
        Shared Property Department() As String
            Get
                Dim Mystr_con As String

                Dim str_sel As String = "sp_metalogon '" & UserProfile.IUserAuthName & "'"
                If Dbconnect.FxSqlBoolean(str_sel, Dbconnect.Myconnect) = True Then
                    Return Nothing

                Else
                    Dim l_return As String = Dbconnect.FxSql(str_sel, Dbconnect.Myconnect).Tables(0).Rows(0).Item("Department")

                    Return l_return 'de_ConnectionString
                End If
            End Get
            Set(ByVal Value As String)

            End Set
        End Property

        Shared Property DepartmentCode() As String
            Get
                Dim Mystr_con As String

                Dim str_sel As String = "sp_metalogon '" & UserProfile.IUserAuthName & "'"
                If Dbconnect.FxSqlBoolean(str_sel, Dbconnect.Myconnect) = True Then
                    Return Nothing

                Else
                    Dim l_return As String = Dbconnect.FxSql(str_sel, Dbconnect.Myconnect).Tables(0).Rows(0).Item("Department_ID")

                    Return l_return 'de_ConnectionString
                End If
            End Get
            Set(ByVal Value As String)

            End Set
        End Property



        Public Shared Function StaffIDEmail(ByVal StaffID As String) As String

            Dim str_logon As String = "sp_metalogon '" & StaffID & "'"
            Dim LogonName As String = Dbconnect.FxSql(str_logon, Dbconnect.Myconnect).Tables(0).Rows(0).Item("logonname")
            '  LogonName = Replace(LogonName, "\", "/")
            Dim l_pos As Integer = InStr(LogonName, "\")
            Dim l_userID As String = LogonName.Substring(l_pos)
            Dim EmailAddress As String = l_userID & "@BONDBANKNG.COM"



            Try
                Return EmailAddress

            Catch ex As Exception
                Return Nothing
            End Try


        End Function

        Public Shared Function UserEmail(ByVal LogonName As String) As String

            'Dim str_logon As String = "sp_metalogon '" & LogonName & "'"
            LogonName = Replace(LogonName, "\", "/")
            Dim l_pos As Integer = InStr(LogonName, "\")
            Dim l_userID As String = LogonName.Substring(l_pos)
            Dim l_strmail As String = "Sproc_Rep_FixedEmailUserID_sel '" & l_userID & "'"
            Try
                Dim EmailAddress As String = Dbconnect.FxSql(l_strmail, Dbconnect.Myconnect).Tables(0).Rows(0).Item("Emailaddress")





                Return EmailAddress

            Catch ex As Exception
                Return Nothing
            End Try


        End Function


    End Class



    'Asset Class Info
    '

    Public Class Assets
        'Public SqlConn As New SqlConnection("data source=development;initial catalog=AstutePS;user id=sa;password=")
        Public Function AddFixedAsset(ByVal refno As String, ByVal description As String, ByVal assetclass As String, ByVal assetsubclass As String, _
            ByVal dateacquired As Date, ByVal brandcode As String, ByVal supplierno As String, ByVal lpono As String, ByVal invoiceno As String, _
            ByVal voucherno As String, ByVal guarantee As String, ByVal secondhand As Integer, ByVal locationcode As String, ByVal usefullife As Integer, _
            ByVal deprrate As Decimal, ByVal deprmethod As String, ByVal deprstart As Date, ByVal deprend As Date, ByVal freqcode As String, ByVal fcycost As Decimal, _
            ByVal currcode As String, ByVal exchrate As Decimal, ByVal invoicecost As Decimal, ByVal incidentalcost As Decimal, ByVal qty As Integer, _
            ByVal contra As String, ByVal requestedby As String, ByVal approvedby As String, ByVal remarks As String, ByVal payable As Integer, _
            ByVal chargetogl As Integer, ByVal chargearrears As Integer, ByVal leased As Integer, ByVal leaserid As String, ByVal leaseperiod As Integer, ByVal leaseprincipal As Decimal, _
            ByVal leaseinterest As Decimal, ByVal leaseno As String, ByVal cUser As String)

            Dim Cmd As SqlCommand, id As SqlParameter

            Cmd = New SqlCommand("Sproc_AddFixedAsset", Dbconnect.Myconnect)

            Try
                Dbconnect.Myconnect.Open()
                Cmd.CommandType = CommandType.StoredProcedure
                Cmd.Parameters.Add(New SqlParameter("@RefNo", refno.ToUpper))
                Cmd.Parameters.Add(New SqlParameter("@Description", description.ToUpper))
                Cmd.Parameters.Add(New SqlParameter("@AssetClassCode", assetclass))
                Cmd.Parameters.Add(New SqlParameter("@AssetSubClassCode", assetsubclass))
                Cmd.Parameters.Add(New SqlParameter("@BrandCode", brandcode))
                Cmd.Parameters.Add(New SqlParameter("@DateAcquired", dateacquired))
                Cmd.Parameters.Add(New SqlParameter("@SupplierNo", supplierno))
                Cmd.Parameters.Add(New SqlParameter("@LpoNo", lpono))
                Cmd.Parameters.Add(New SqlParameter("@InvoiceNo", invoiceno))
                Cmd.Parameters.Add(New SqlParameter("@VoucherNo", voucherno))
                Cmd.Parameters.Add(New SqlParameter("@Guarantee", guarantee))
                Cmd.Parameters.Add(New SqlParameter("@SecondHand", CInt(secondhand)))
                Cmd.Parameters.Add(New SqlParameter("@LocationCode", locationcode))
                Cmd.Parameters.Add(New SqlParameter("@UsefulLife", CInt(usefullife)))
                Cmd.Parameters.Add(New SqlParameter("@DeprRate", CDec(deprrate)))
                Cmd.Parameters.Add(New SqlParameter("@DeprMethodCode", deprmethod))
                Cmd.Parameters.Add(New SqlParameter("@DeprStartDate", CDate(deprstart)))
                Cmd.Parameters.Add(New SqlParameter("@DeprEndDate", CDate(deprend)))
                Cmd.Parameters.Add(New SqlParameter("@FreqCode", freqcode))
                Cmd.Parameters.Add(New SqlParameter("@FcyCost", CDec(fcycost)))
                Cmd.Parameters.Add(New SqlParameter("@CurrCode", currcode))
                Cmd.Parameters.Add(New SqlParameter("@ExchRate", exchrate))
                Cmd.Parameters.Add(New SqlParameter("@InvoiceCost", CDec(invoicecost)))
                Cmd.Parameters.Add(New SqlParameter("@IncidentalCost", CDec(incidentalcost)))
                Cmd.Parameters.Add(New SqlParameter("@Qty", CInt(qty)))
                Cmd.Parameters.Add(New SqlParameter("@ContraAcct", contra))
                Cmd.Parameters.Add(New SqlParameter("@RequestedBy", requestedby))
                Cmd.Parameters.Add(New SqlParameter("@ApprovedBy", approvedby))
                Cmd.Parameters.Add(New SqlParameter("@Remarks", remarks))
                Cmd.Parameters.Add(New SqlParameter("@Payable", CInt(payable)))
                Cmd.Parameters.Add(New SqlParameter("@ChargeToGl", CInt(chargetogl)))
                Cmd.Parameters.Add(New SqlParameter("@ChargeArrears", CInt(chargearrears)))
                Cmd.Parameters.Add(New SqlParameter("@Leased", CInt(leased)))
                Cmd.Parameters.Add(New SqlParameter("@LeaserID", UCase(leaserid)))
                Cmd.Parameters.Add(New SqlParameter("@LeasePeriod", CInt(leaseperiod)))
                Cmd.Parameters.Add(New SqlParameter("@LeasePrincipal", CDec(leaseprincipal)))
                Cmd.Parameters.Add(New SqlParameter("@LeaseInterest", CDec(leaseinterest)))
                Cmd.Parameters.Add(New SqlParameter("@LeaseNo", UCase(leaseno)))
                Cmd.Parameters.Add(New SqlParameter("@AddedBy", cUser))
                Cmd.ExecuteNonQuery()

            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
                'Finally
                '    Dbconnect.mydisconnect.Close()
            End Try
        End Function
        Public Function WebAddFixedAsset(ByVal description As String, ByVal assetclass As String, ByVal assetsubclass As String, _
          ByVal dateacquired As Date, ByVal brandcode As String, ByVal supplierno As String, ByVal lpono As String, ByVal invoiceno As String, _
          ByVal voucherno As String, ByVal guarantee As String, ByVal secondhand As Integer, ByVal locationcode As String, ByVal usefullife As Integer, _
          ByVal deprrate As Decimal, ByVal deprmethod As String, ByVal deprstart As Date, ByVal deprend As Date, ByVal freqcode As String, ByVal fcycost As Decimal, _
          ByVal currcode As String, ByVal exchrate As Decimal, ByVal invoicecost As Decimal, ByVal incidentalcost As Decimal, ByVal qty As Integer, _
          ByVal contra As String, ByVal requestedby As String, ByVal approvedby As String, ByVal remarks As String, ByVal payable As Integer, _
          ByVal chargetogl As Integer, ByVal chargearrears As Integer, ByVal leased As Integer, ByVal leaserid As String, ByVal leaseperiod As Integer, ByVal leaseprincipal As Decimal, _
          ByVal leaseinterest As Decimal, ByVal leaseno As String, ByVal cUser As String)

            Dim Cmd As SqlCommand, id As SqlParameter

            Cmd = New SqlCommand("Sproc_webAddFixedAsset_ins", Dbconnect.mydisconnect)

            Try
                Dbconnect.Myconnect.Open()
                Cmd.CommandType = CommandType.StoredProcedure
                'Cmd.Parameters.Add(New Sqlparameter("@RefNo", refno.ToUpper))
                Cmd.Parameters.Add(New SqlParameter("@Description", description.ToUpper))
                Cmd.Parameters.Add(New SqlParameter("@AssetClassCode", assetclass))
                Cmd.Parameters.Add(New SqlParameter("@AssetSubClassCode", assetsubclass))
                Cmd.Parameters.Add(New SqlParameter("@BrandCode", brandcode))
                Cmd.Parameters.Add(New SqlParameter("@DateAcquired", dateacquired))
                Cmd.Parameters.Add(New SqlParameter("@SupplierNo", supplierno))
                Cmd.Parameters.Add(New SqlParameter("@LpoNo", lpono))
                Cmd.Parameters.Add(New SqlParameter("@InvoiceNo", invoiceno))
                Cmd.Parameters.Add(New SqlParameter("@VoucherNo", voucherno))
                Cmd.Parameters.Add(New SqlParameter("@Guarantee", guarantee))
                Cmd.Parameters.Add(New SqlParameter("@SecondHand", CInt(secondhand)))
                Cmd.Parameters.Add(New SqlParameter("@LocationCode", locationcode))
                Cmd.Parameters.Add(New SqlParameter("@UsefulLife", CInt(usefullife)))
                Cmd.Parameters.Add(New SqlParameter("@DeprRate", CDec(deprrate)))
                Cmd.Parameters.Add(New SqlParameter("@DeprMethodCode", deprmethod))
                Cmd.Parameters.Add(New SqlParameter("@DeprStartDate", CDate(deprstart)))
                Cmd.Parameters.Add(New SqlParameter("@DeprEndDate", CDate(deprend)))
                Cmd.Parameters.Add(New SqlParameter("@FreqCode", freqcode))
                Cmd.Parameters.Add(New SqlParameter("@FcyCost", CDec(fcycost)))
                Cmd.Parameters.Add(New SqlParameter("@CurrCode", currcode))
                Cmd.Parameters.Add(New SqlParameter("@ExchRate", exchrate))
                Cmd.Parameters.Add(New SqlParameter("@InvoiceCost", CDec(invoicecost)))
                Cmd.Parameters.Add(New SqlParameter("@IncidentalCost", CDec(incidentalcost)))
                Cmd.Parameters.Add(New SqlParameter("@Qty", CInt(qty)))
                Cmd.Parameters.Add(New SqlParameter("@ContraAcct", contra))
                Cmd.Parameters.Add(New SqlParameter("@RequestedBy", requestedby))
                Cmd.Parameters.Add(New SqlParameter("@ApprovedBy", approvedby))
                Cmd.Parameters.Add(New SqlParameter("@Remarks", remarks))
                Cmd.Parameters.Add(New SqlParameter("@Payable", CInt(payable)))
                Cmd.Parameters.Add(New SqlParameter("@ChargeToGl", CInt(chargetogl)))
                Cmd.Parameters.Add(New SqlParameter("@ChargeArrears", CInt(chargearrears)))
                Cmd.Parameters.Add(New SqlParameter("@Leased", CInt(leased)))
                Cmd.Parameters.Add(New SqlParameter("@LeaserID", UCase(leaserid)))
                Cmd.Parameters.Add(New SqlParameter("@LeasePeriod", CInt(leaseperiod)))
                Cmd.Parameters.Add(New SqlParameter("@LeasePrincipal", CDec(leaseprincipal)))
                Cmd.Parameters.Add(New SqlParameter("@LeaseInterest", CDec(leaseinterest)))
                Cmd.Parameters.Add(New SqlParameter("@LeaseNo", UCase(leaseno)))
                Cmd.Parameters.Add(New SqlParameter("@AddedBy", cUser))
                Cmd.ExecuteNonQuery()

            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                Dbconnect.mydisconnect.Close()
            End Try
        End Function

        Public Function AssetCombinationOk(ByVal assetclass As String, ByVal assetsubclass As String, ByVal brand As String) As Boolean
            Dim Cmd As SqlCommand, id As SqlParameter

            Cmd = New SqlCommand("Sproc_AssetCombinationOk", Dbconnect.Myconnect)

            Try
                Dbconnect.Myconnect.Open()

                Cmd.CommandType = CommandType.StoredProcedure
                Cmd.Parameters.Add(New SqlParameter("@AssetClassCode", assetclass))
                Cmd.Parameters.Add(New SqlParameter("@AssetSubClassCode", assetsubclass))
                Cmd.Parameters.Add(New SqlParameter("@BrandCode", brand))
                id = Cmd.Parameters.Add(New SqlParameter("@Response", OleDbType.Integer, 1))
                id.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()

                If id.Value = 0 Then
                    AssetCombinationOk = False
                Else
                    AssetCombinationOk = True
                End If
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                Dbconnect.mydisconnect.Close()
            End Try
        End Function
        Public Function AssetPurchaseNumber() As String
            Dim Cmd As SqlCommand, id As SqlParameter

            Cmd = New SqlCommand("Sproc_AssetPurchaseNumber", Dbconnect.Myconnect)

            Try
                Dbconnect.Myconnect.Open()
                Cmd.CommandType = CommandType.StoredProcedure
                id = Cmd.Parameters.Add(New SqlParameter("@AssetNo", OleDbType.Variant, 20))
                id.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()

                AssetPurchaseNumber = id.Value
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                Dbconnect.mydisconnect.Close()
            End Try
        End Function

        Public Function DeleteFixedAssetPurchased(ByVal refno As String)
            Dim Cmmd As New SqlCommand("Sproc_DeleteFixedAssetPurchased", Dbconnect.Myconnect)

            Try
                Dbconnect.Myconnect.Open()
                Cmmd.CommandType = CommandType.StoredProcedure
                Cmmd.Parameters.Add(New SqlParameter("@RefNo", refno))
                Cmmd.ExecuteNonQuery()

                Dbconnect.mydisconnect.Close()
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                Dbconnect.mydisconnect.Close()
            End Try
        End Function
        Public Function GenerateFixedAssetItem(ByVal refno As String)
            Dim Cmmd As New SqlCommand("Sproc_GenerateFixedAssetItem", Dbconnect.Myconnect)

            Try
                Dbconnect.Myconnect.Open()
                Cmmd.CommandType = CommandType.StoredProcedure
                Cmmd.Parameters.Add(New SqlParameter("@RefNo", refno))
                Cmmd.ExecuteNonQuery()

                Dbconnect.mydisconnect.Close()
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                Dbconnect.mydisconnect.Close()
            End Try
        End Function
        Public Function Getsga_marshallwebPurchase(ByVal refno As String) As DataSet
            Dim Ds As New DataSet
            Dim Adapter As New SqlDataAdapter
            Dim Cmmd As New SqlCommand("SELECT * FROM sga_marshallwebPurchases WHERE ref_no = '" & refno & "'", Dbconnect.Myconnect)

            Try
                Dbconnect.Myconnect.Open()
                Adapter.SelectCommand = Cmmd
                Cmmd.CommandType = CommandType.Text
                Cmmd.ExecuteNonQuery()
                Adapter.Fill(Ds, "Purchase")

                Getsga_marshallwebPurchase = Ds
                Dbconnect.mydisconnect.Close()
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                Dbconnect.mydisconnect.Close()
            End Try
        End Function
        Public Function AssetItemExist(ByVal SeqNo As String) As Boolean
            Dim Cmd As New SqlCommand("Sproc_AssetItemExist", Dbconnect.Myconnect)
            Dim id As SqlParameter

            Try
                Dbconnect.Myconnect.Open()
                Cmd.CommandType = CommandType.StoredProcedure
                Cmd.Parameters.Add(New SqlParameter("@SeqNo", SeqNo))
                id = Cmd.Parameters.Add(New SqlParameter("@Response", SqlDbType.Int, 1))

                id.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()

                AssetItemExist = id.Value
                Dbconnect.mydisconnect.Close()
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                Dbconnect.mydisconnect.Close()
            End Try
        End Function
        Public Function AssetAcctInfoChange(ByVal seqno As String, ByVal changetype As String, ByVal value As Decimal)
            Dim Cmmd As New SqlCommand("Sproc_AssetAcctInfoChange", Dbconnect.Myconnect)

            Try
                Dbconnect.Myconnect.Open()
                Cmmd.CommandType = CommandType.StoredProcedure
                Cmmd.Parameters.Add(New SqlParameter("@SeqNo", seqno))
                Cmmd.Parameters.Add(New SqlParameter("@ChangeType", changetype))
                Cmmd.Parameters.Add(New SqlParameter("@Value", value))
                Cmmd.ExecuteNonQuery()
                Dbconnect.Myconnect.Close()
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                Dbconnect.mydisconnect.Close()
            End Try
        End Function

        Public Function AssetMemoInfoChange(ByVal seqno As String, ByVal assetid As String, _
                ByVal studentid As String, ByVal locationcode As String)
            Dim Cmmd As New SqlCommand("Sproc_AssetMemoInfoChange", Dbconnect.Myconnect)

            Try
                Dbconnect.Myconnect.Open()
                Cmmd.CommandType = CommandType.StoredProcedure
                Cmmd.Parameters.Add(New SqlParameter("@SeqNo", seqno))
                Cmmd.Parameters.Add(New SqlParameter("@AssetId", assetid))
                Cmmd.Parameters.Add(New SqlParameter("@StudentId", studentid))
                Cmmd.Parameters.Add(New SqlParameter("@LocationCode", locationcode))

                Cmmd.ExecuteNonQuery()

                Dbconnect.Myconnect.Close()
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                Dbconnect.mydisconnect.Close()
            End Try
        End Function

        Public Shared Function AssetClassInfo(ByVal assetclass As String, ByVal cItem As Integer) As String
            Dim Cmd As SqlCommand

            Cmd = New SqlCommand("Sproc_AssetClassInfo", Dbconnect.Myconnect)
            Dim id As SqlParameter

            Try
                'Dbconnect.Myconnect.Open()
                'Cmd.CommandType = CommandType.StoredProcedure
                'Cmd.Parameters.Add(New Sqlparameter("@AssetClassCode", assetclass))
                'Cmd.Parameters.Add(New Sqlparameter("@item", cItem))
                'id = Cmd.Parameters.Add(New Sqlparameter("@result", OleDbType.VarChar, 100))
                'id.Direction = ParameterDirection.Output
                'Cmd.ExecuteScalar()
                'Cmd.Dispose()


                AssetClassInfo = Dbconnect.FxSql("Sproc_dbAssetClassInfo '" & assetclass & "','" & cItem & "'", Dbconnect.Myconnect).Tables(0).Rows(0).Item("result")
                Return AssetClassInfo
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
                'Finally
                '    Dbconnect.mydisconnect.Close()
            End Try
        End Function
        Public Shared Function AssetClassAcctNo(ByVal assetclass As String, ByVal clocation As String, ByVal cBrandCode As String) As String
            Dim Cmd As SqlCommand

            Cmd = New SqlCommand("Sproc_AssetClassInfo", Dbconnect.Myconnect)
            Dim id As SqlParameter

            Try
                'Dbconnect.Myconnect.Open()
                'Cmd.CommandType = CommandType.StoredProcedure
                'Cmd.Parameters.Add(New Sqlparameter("@AssetClassCode", assetclass))
                'Cmd.Parameters.Add(New Sqlparameter("@item", cItem))
                'id = Cmd.Parameters.Add(New Sqlparameter("@result", OleDbType.VarChar, 100))
                'id.Direction = ParameterDirection.Output
                'Cmd.ExecuteScalar()
                'Cmd.Dispose()


                AssetClassAcctNo = Dbconnect.FxSql("Sproc_AssetAcctNoClassInfo '" & assetclass & "','" & clocation & "','" & cBrandCode & "'", Dbconnect.Myconnect).Tables(0).Rows(0).Item("ContaNo")
                Return AssetClassAcctNo
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
                'Finally
                '    Dbconnect.mydisconnect.Close()
            End Try
        End Function

    End Class






    Public Class UtilitiesWs

        'Public SqlConn As New SqlConnection("data source=development;initial catalog=AstutePS;user id=sa;password=")

        Public Function FullAmount(ByVal sAmnt As String) As Double
            Dim aLen As Integer, i As Integer

            sAmnt = Trim(sAmnt)
            aLen = Len(sAmnt)

            For i = 1 To aLen - 1
                If CheckDigit(CStr(Mid$(sAmnt, i, 1))) = False Then
                    MsgBox("Improper Number Input")
                    Exit Function
                End If
            Next i

            If Right$(sAmnt.ToUpper, 1) = "T" Then
                FullAmount = CDbl(Left$(sAmnt, aLen - 1)) * 1000
            ElseIf Right$(sAmnt.ToUpper, 1) = "H" Then
                FullAmount = CDbl(Left$(sAmnt, aLen - 1)) * 100

            ElseIf Right$(sAmnt.ToUpper, 1) = "M" Then
                FullAmount = CDbl(Left$(sAmnt, aLen - 1)) * 1000000
            ElseIf Right$(sAmnt.ToUpper, 1) = "B" Then
                FullAmount = CDbl(Left$(sAmnt, aLen - 1)) * 1000000000
            Else
                FullAmount = sAmnt
            End If

            FullAmount = Format(FullAmount, "#,##0.00")
        End Function

        Public Function CheckDigit(ByVal dg As String) As Boolean
            Dim j As Integer

            CheckDigit = False

            If dg = "." Or dg = "," Or dg = "-" Then
                CheckDigit = True
                Exit Function
            End If

            For j = 0 To 9
                If CStr(j) = dg Then
                    CheckDigit = True
                    Exit For
                End If
            Next j

        End Function
        Public Function FormatString(ByVal cString As String, ByVal cLength As Integer) As String
            ' Method for converting string by stuffing them with leading zeros as specified by the cLength argument
            If cLength <= Len(Trim(cString)) Then
                FormatString = cString
                Exit Function
            End If

            Dim i As Integer, newString As String

            newString = ""

            For i = 1 To cLength - Len(Trim(cString))
                newString = newString & "0"
            Next

            FormatString = newString & Trim(cString)
        End Function
        Public Function SystemInfo(ByVal cItem As Integer) As String
            Dim Cmd As SqlCommand

            Cmd = New SqlCommand("Sproc_SystemInfo", Dbconnect.Myconnect)
            Dim id As SqlParameter

            Try
                Dbconnect.Myconnect.Open()
                Cmd.CommandType = CommandType.StoredProcedure
                Cmd.Parameters.Add(New SqlParameter("@item", cItem))
                id = Cmd.Parameters.Add(New SqlParameter("@result", OleDbType.VarChar, 100))
                id.Direction = ParameterDirection.Output
                Cmd.ExecuteScalar()
                Cmd.Dispose()

                SystemInfo = id.Value
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                Dbconnect.mydisconnect.Close()
            End Try
        End Function

        Public Function CurrExchRate(ByVal currcode As String) As Decimal
            Dim Cmd As New SqlCommand("Sproc_CurrExchRate", Dbconnect.Myconnect)
            Dim id As SqlParameter

            Try
                Dim str_sel As String = "Sproc_AnyCurrExchRate '" & currcode & "'"
                If Dbconnect.FxSqlBoolean(str_sel, Dbconnect.Myconnect) = False Then
                    CurrExchRate = Dbconnect.FxSql(str_sel, Dbconnect.Myconnect).Tables(0).Rows(0).Item("exch_rate")


                Else
                    CurrExchRate = 0
                End If
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                ' Dbconnect.mydisconnect.Close()

            End Try
        End Function




    End Class

    Public Module GenParameter

        Dim dsKapity As New DataSet

    End Module

End Namespace

