Imports System
Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Imports System.Configuration.ConfigurationManager
Imports System.Net.Mail
Imports System.Windows.Forms
Imports System.Net
Imports System.Security
Imports Microsoft.VisualBasic
Imports Microsoft
Imports System.Text
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Collections
Imports System.Security.Cryptography.X509Certificates
Imports System.Net.Security
Imports Newtonsoft.Json
Imports System.Web.Script.Serialization

Namespace NETClass

    Public Class Classes

        Public Shared Function GenerateRandomDigit(SystemDateTime As Date, lengthMax As Integer) As String

            '      using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            '{
            '    // Buffer storage.
            '    byte[] data = new byte[4];

            '    // Ten iterations.
            '    for (int i = 0; i < 10; i++)
            '    {
            '        // Fill buffer.
            '        rng.GetBytes(data);

            '        // Convert to int 32.
            '        int value = BitConverter.ToInt32(data, 0);
            '        Console.WriteLine(value);
            '    }
            '}
            Dim result As String
            result = Guid.NewGuid().ToString().Substring(0, lengthMax).ToUpper()
            Try
                If Classes.GetDataset("SELECT TokenType From ParamPreferences").Tables(0).Rows(0).Item("TokenType").ToString() = "1" Then

                    Dim Rng As New System.Security.Cryptography.RNGCryptoServiceProvider()
                    Dim bite As Integer
                    bite = lengthMax
                    Dim data() As Byte = New Byte(lengthMax - 1) {}
                    Dim value As String
                    Rng.GetBytes(data)

                    '   For index = 1 To lengthMax
                    value = BitConverter.ToInt32(data, 0)
                    ' Next


                    value = Replace(value, "-", "")


                    Return value.Substring(0, lengthMax)

                End If


            Catch ex As Exception

            End Try

            Return result
        End Function


        Public Shared Function FnPushSMSSSL(PhoneNo As String, username As String, pwds As String, displayname As String, msg As String, msgto As String) As String

            Dim request As WebRequest = WebRequest.Create(String.Format("https://api.silversands.com.ng/sms/1/text/single?user={0}&password={1}&sender={2}&SMSText={3}&GSM={4}&type=longSMS", username, pwds, displayname, msg, msgto))


            Dim HttpWebRequest As HttpWebRequest = HttpWebRequest.Create(String.Format("https://api.silversands.com.ng/sms/1/text/single?user={0}&password={1}&sender={2}&SMSText={3}&GSM={4}&type=longSMS", username, pwds, displayname, msg, msgto))



            Dim strHeaderToken As String = Convert.ToBase64String(Encoding.Default.GetBytes(String.Format("{0}:{1}", username, pwds)))
            HttpWebRequest.ContentType = "application/json"

            HttpWebRequest.Headers.Add("Authorization", "Basic " + strHeaderToken)
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

            HttpWebRequest.Method = "POST"
            Using streamWriter = New StreamWriter(HttpWebRequest.GetRequestStream())
                Dim ljson As Object = New With
                    {
                    .from = displayname,
                    .to = msgto,
                    .text = msg
                }
                Dim serializer As New JavaScriptSerializer()
                Dim serializedResult = serializer.Serialize(ljson)
                streamWriter.Write(serializedResult)
                streamWriter.Flush()
                streamWriter.Close()
            End Using

            Dim HttpResponse As HttpWebResponse = CType(HttpWebRequest.GetResponse(), HttpWebResponse)

            Dim Result = ""


            Using streamReader = New StreamReader(HttpResponse.GetResponseStream())
                Result = streamReader.ReadToEnd()
            End Using



            Return Result


        End Function

        Public Shared Function FnPushSMS(PhoneNo As String, username As String, pwds As String, displayname As String, msg As String, msgto As String) As String

            'Dim request As WebRequest = WebRequest.Create(String.Format("http://193.105.74.159/api/sendsms/plain?user={0}&password={1}&sender={2}&SMSText={3}&GSM={4}&type=longSMS", username, pwds, displayname, msg, msgto))

            'Dim request As WebRequest = WebRequest.Create(String.Format("https://api2.silversandslimited.com/api/sendsms/plain?user={0}&password={1}&sender={2}&SMSText={3}&GSM={4}&type=longSMS", username, pwds, displayname, msg, msgto))

            Dim request As WebRequest = WebRequest.Create(String.Format("http://api.silversands.com.ng/sendsms/plain?user={0}&password={1}&sender={2}&SMSText={3}&GSM={4}&type=longSMS", username, pwds, displayname, msg, msgto))

            request.Method = "POST"
            Dim s As String = ""
            Dim bytes As Byte() = Encoding.UTF8.GetBytes(s)
            request.ContentType = "application/x-www-form-urlencoded"
            request.ContentLength = bytes.Length
            ServicePointManager.ServerCertificateValidationCallback =
            New System.Net.Security.RemoteCertificateValidationCallback(AddressOf customCertValidation)
            Dim requestStream As Stream = request.GetRequestStream()
            requestStream.Write(bytes, 0, bytes.Length)
            requestStream.Close()
            Dim response As WebResponse = request.GetResponse()
            'Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            requestStream = response.GetResponseStream()
            Dim reader As StreamReader = New StreamReader(requestStream)
            Dim str3 As String = reader.ReadToEnd()
            Console.WriteLine(str3)
            reader.Close()
            requestStream.Close()
            Return str3



        End Function

        Public Shared Function customCertValidation(ByVal sender As Object,
                                                ByVal cert As X509Certificate,
                                                ByVal chain As X509Chain,
                                                ByVal errors As SslPolicyErrors) As Boolean

            Return True

        End Function
        Public Shared Function FixPDFMergeErrorOFReportViewer2010(bytPDF As Byte()) As Byte()


            If (IsNothing(bytPDF) = False) Then

                For i As Integer = 0 To bytPDF.Length

                    If (bytPDF(i) = Convert.ToByte("s")) And (bytPDF(i + 1) = Convert.ToByte("t")) And (bytPDF(i + 2) = Convert.ToByte("r")) And (bytPDF(i + 3) = Convert.ToByte("e")) And (bytPDF(i + 4) = Convert.ToByte("a")) And (bytPDF(i + 5) = Convert.ToByte("m")) Then

                        If (bytPDF(i + 6) = 32) Then
                            Dim j As Integer
                            For j = i + 6 To j > i Step --j

                                bytPDF(j) = bytPDF(j - 1)
                            Next


                        End If


                    End If


                Next



            End If


            Return bytPDF

        End Function

        Public Shared Function FnGetGridDatasource(ByVal nocolumn As String, ByVal norow As String) As DataView

            Dim length As Int64 = Int64.Parse(nocolumn)
            Dim rowlength As Int64 = Int64.Parse(norow)
            'Int64 rowlength = 5;

            '#region Section for virtual cols
            'DataRow dr;
            'DataGridItem iCells;
            'DataTable dt = new DataTable();
            Dim ds As New DataSet

            Dim dt As New DataTable
            Dim dr As DataRow
            Dim icells As DataGridItem

            For i = 0 To length - 1
                Dim mycolnme As String = String.Format("chek{0}", i)

                dt.Columns.Add(New DataColumn(mycolnme, System.Type.GetType("System.String")))
            Next

            For j = 0 To rowlength - 1
                dr = dt.NewRow()



                dt.Rows.Add(dr)
            Next
            Dim dv As New DataView(dt)
            Return dv

        End Function



        Public Shared Function FnGetGridProdDatasource(ByVal proditemcode As String, ByVal prodcode As String) As DataView
            Dim strget As String = String.Format("Sproc_GetTransCodeSubItems '{0}','{1}'", proditemcode, prodcode)
            Dim strgetrowcount As String = String.Format("SELECT max(sub_itemRowCount) as 'rowlength'  From TransCodeSubItems WHERE item_code ='{0}' AND trans_code ='{1}'", proditemcode, prodcode)

            'Dim length As Int64 = Int64.Parse(nocolumn)
            Dim rowlength As Int64 = 0
            If (Classes.GetDataset(strgetrowcount).Tables(0).Rows.Count > 0) Then


                rowlength = Int64.Parse(Classes.GetDataset(strgetrowcount).Tables(0).Rows(0)("rowlength").ToString())
            End If
            'Int64 rowlength = 5;

            '#region Section for virtual cols
            'DataRow dr;
            'DataGridItem iCells;
            'DataTable dt = new DataTable();
            Dim ds As New DataSet

            Dim dt As New DataTable
            Dim dr As DataRow
            Dim icells As DataGridItem
            Dim strIndex As Integer = 1
            For Each ks As DataRow In Classes.SqlDbGetDataset(strget).Tables(0).Rows

                Dim mycolnme As String = String.Format("{0}. {1}", strIndex, ks("sub_item_desc").ToString())

                dt.Columns.Add(New DataColumn(mycolnme, System.Type.GetType("System.String")))
                strIndex += 1
            Next


            For j = 0 To rowlength - 1
                dr = dt.NewRow()



                dt.Rows.Add(dr)
            Next
            Dim dv As New DataView(dt)
            Return dv

        End Function



        Function CreateDataSourceDetObj() As ICollection
            Dim ds As New DataSet

            Dim dt As New DataTable
            Dim dr As DataRow
            Dim icells As DataGridItem
            dt.Columns.Add(New DataColumn("SerialNo", GetType(String)))

            Dim ilenght As Integer = 100, i As Integer
            For i = 1 To ilenght
                dr = dt.NewRow()
                dr(0) = i


                dt.Rows.Add(dr)
            Next




            Dim dv As New DataView(dt)
            Return dv


        End Function


        Public Shared Function SendNETMails(ByVal ToAddress As String, ByVal FromAddress As String, ByVal Subject As String, ByVal Msg As String) As Boolean


            Dim NETMailServer As String, NETMailServerPort As String
            NETMailServer = System.Configuration.ConfigurationManager.ConnectionStrings("EazyEmailServer").ConnectionString()
            NETMailServerPort = System.Configuration.ConfigurationManager.ConnectionStrings("EazyEmailServerPort").ConnectionString()
            Dim strParamRulePos As String = String.Format("SELECT rule_value FROM ParamRules WHERE rule_id = {0}", 7)
            If Classes.GetBitField(strParamRulePos) = True Then

                Using message As _
                                New MailMessage(FromAddress, ToAddress, Subject, Msg)

                    ' SmtpClient is used to send the e-mail
                    Dim mailClient As New SmtpClient(NETMailServer, CInt(NETMailServerPort))
                    ' UseDefaultCredentials tells the mail client to use the 
                    ' Windows credentials of the account (i.e. user account) 
                    ' being used to run the application
                    mailClient.UseDefaultCredentials = True

                    ' Send delivers the message to the mail server
                    mailClient.Send(message)

                End Using

            End If
            'CreateMessageWithAttachment("pokkie")
            'Dim MyMsg As New System.Net.Mail.MailMessage
            'MyMsg.Attachments.Item(0).
        End Function

        Public Shared Function alertwindow(ByVal aspxpage As System.Web.UI.Page, ByVal Msgger As String, ByVal desc As String) As String

            Msgger = """" & Msgger & """"
            'Msgger = "" & Msgger & ""


            Dim javastring As System.Text.StringBuilder
            javastring = New System.Text.StringBuilder
            javastring.Append("<script language='Javascript'>")
            javastring.Append("alert(" & Msgger & ");")
            javastring.Append("windows.location='")
            javastring.Append("Urlstring:")
            javastring.Append("';")
            javastring.Append("</script>")


            If Not aspxpage.ClientScript.IsClientScriptBlockRegistered(desc) Then
                aspxpage.ClientScript.RegisterClientScriptBlock(aspxpage.GetType(), desc, javastring.ToString)
            End If
            Return True
        End Function

        Public Shared Function Closewindow(ByVal aspxpage As System.Web.UI.Page) As String


            Dim javastring As System.Text.StringBuilder
            javastring = New System.Text.StringBuilder
            javastring.Append("<script language='Javascript'>")
            javastring.Append("self.close")
            javastring.Append("</script>")



            Dim script As String = "<script>"

            script += "self.close()"
            script += "</" + "script>"
            ' RegisterClientScriptBlock("test", script)



            If Not aspxpage.ClientScript.IsClientScriptBlockRegistered("Halt") Then
                aspxpage.ClientScript.RegisterClientScriptBlock(aspxpage.GetType(), "Halt", script.ToString)
            End If
            Return True
        End Function

        Public Function Confirmwindow(ByVal aspxpage As System.Web.UI.Page, ByVal desc As String) As String
            Dim javastring As System.Text.StringBuilder
            javastring = New System.Text.StringBuilder
            javastring.Append("<script language='javascript'>")
            javastring.Append("var res=confirm("" Are you sure want to Refer Transaction?"");")
            javastring.Append("if (res==1){}")
            javastring.Append("else{history.go(-1);}")
            javastring.Append("</script>")

            If Not aspxpage.ClientScript.IsClientScriptBlockRegistered(desc) Then
                aspxpage.ClientScript.RegisterClientScriptBlock(aspxpage.GetType(), desc, javastring.ToString)
            End If
            Return True
        End Function

        Public Function ConfirmwindowInfo(ByVal aspxpage As System.Web.UI.Page, ByVal desc As String) As String
            Dim javastring As System.Text.StringBuilder
            javastring = New System.Text.StringBuilder
            javastring.Append("<script language='javascript'>")
            javastring.Append("var res=confirm(""Are you Sure? " & desc & ");")
            javastring.Append("if (res==1){}")
            javastring.Append("else{history.go(-1);}")
            javastring.Append("</script>")

            If Not aspxpage.ClientScript.IsClientScriptBlockRegistered(desc) Then
                aspxpage.ClientScript.RegisterClientScriptBlock(aspxpage.GetType(), desc, javastring.ToString)
            End If
            Return True
        End Function
        Public Shared Function SqlDbGetDataset(ByVal rString As String) As DataSet
            '  This functions returns o Dataset based on a Strored Procedure or
            '  Select statement passed as the rString parameter

            Dim Cmd As New SqlCommand, OleDbConn As New SqlConnection
            Dim Adapter As New SqlDataAdapter, Ds As New DataSet

            OleDbConn.ConnectionString = DbSqlConnectionString()

            Cmd.CommandText = rString
            Cmd.Connection = OleDbConn
            Cmd.CommandType = CommandType.Text

            Adapter.SelectCommand = Cmd
            Adapter.Fill(Ds, "Table")

            Try
                OleDbConn.Open()
                Cmd.ExecuteNonQuery()
                OleDbConn.Close()

                Return Ds
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleDbConn.Close()
            End Try

        End Function

        Public Shared Function GenerateRefNum(ByVal type As String) As String

            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = DbSqlConnectionString()

            Cmd.CommandText = "Sproc_GenerateNumber"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@OrgCode", "0101"))
                Cmd.Parameters.Add(New SqlParameter("@TransDate", Format(Today.Date, "MM-dd-yyyy")))
                Cmd.Parameters.Add(New SqlParameter("@ItemType", type))

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


        Public Shared Function GetPostingDate() As DateTime
            Dim result As DateTime
            result = DateTime.Parse(GetSqlDataset2("SELECT post_date FROM ParamPreferences WHERE rec_id = '1'", DbSqlConnectionString()).Tables(0).Rows(0).Item("Post_date"))

            Return result
        End Function



        Public Shared Function DbSqlConnectionString() As String
            'Dim StreamFile As StreamReader

            'Try
            '    StreamFile = New StreamReader("Startup.ini")
            '    OleConnectionString = "Provider=SQLOLEDB.1;" & StreamFile.ReadToEnd
            '    StreamFile.Close()
            'Catch Exc As Exception
            '    Console.WriteLine(Exc.Message)
            'End Try

            DbSqlConnectionString = ConfigurationManager.ConnectionStrings("EazyConnect").ConnectionString


        End Function


        Public Function GetsRefererUrlString(ByVal referrerUrl As Uri) As String


            Dim l_localurl As String = referrerUrl.LocalPath

            Dim l_pos As Integer = l_localurl.IndexOf("/")
            Dim l_path As String = Mid(l_localurl, l_pos + 1)



            Return l_path
        End Function




        Public Shared Function GetConnectionString() As String
            GetConnectionString = System.Configuration.ConfigurationSettings.AppSettings("MyConnect")
        End Function
        Public Shared Function OleConnectionString() As String
            OleConnectionString = ConfigurationSettings.AppSettings("OleConnect")
        End Function

        Public Shared Function GetParamRule(ByVal ruleid As Integer) As Boolean
            Return GetBitField("SELECT rule_value FROM ParamRules WHERE rule_id = " & ruleid & "")
        End Function
        Public Shared Function AxGetParamRule(ByVal ruleid As Integer) As Boolean
            Return AxGetBitField("SELECT rule_value FROM ParamRules WHERE rule_id = " & ruleid & "")
        End Function
        Public Shared Function GetAllUsers() As DataSet
            Return GetDataset("SELECT user_id,user_names FROM AppUsers")
        End Function

        Public Shared Function FullAmount(ByVal sAmnt As String) As Double
            Dim aLen As Integer, i As Integer

            sAmnt = Trim(sAmnt)
            aLen = Len(sAmnt)

            For i = 1 To aLen - 1
                If CheckDigit(CStr(Mid$(sAmnt, i, 1))) = False Then
                    MsgBox("Improper Number Input")
                    Exit Function
                End If
            Next i

            If Right$(sAmnt.ToUpper, 1) = "H" Then
                FullAmount = CDbl(Left$(sAmnt, aLen - 1)) * 100
            ElseIf Right$(sAmnt.ToUpper, 1) = "T" Then
                FullAmount = CDbl(Left$(sAmnt, aLen - 1)) * 1000
            ElseIf Right$(sAmnt.ToUpper, 1) = "M" Then
                FullAmount = CDbl(Left$(sAmnt, aLen - 1)) * 1000000
            ElseIf Right$(sAmnt.ToUpper, 1) = "B" Then
                FullAmount = CDbl(Left$(sAmnt, aLen - 1)) * 1000000000
            Else
                FullAmount = sAmnt
            End If

            FullAmount = Format(CDec(FullAmount), "#,##0.00")
        End Function

        Public Shared Function MenuItemExist(ByVal userid As String, ByVal menuitem As String, ByVal dev As String) As Boolean
            Dim result As String

            If dev = "Y" Then
                Return True
            Else
                result = GetField("SELECT menu_item FROM VwAppUsersPerm WHERE user_id = '" & userid & "' AND menu_item = '" & menuitem & "'")

                If Len(result) > 0 Then
                    Return True
                Else
                    Return False
                End If
            End If


        End Function

        Public Shared Function CheckDigit(ByVal dg As String) As Boolean
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

        Public Shared Function FormatString(ByVal cString As String, ByVal cLength As Integer) As String
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

        Public Shared Function InTMB(ByVal lChar As String) As Boolean
            If lChar = "T" Or lChar = "M" Or lChar = "B" Then
                InTMB = True
            ElseIf lChar = "t" Or lChar = "m" Or lChar = "b" Then
                InTMB = True
            Else
                InTMB = False
            End If
        End Function

        Public Shared Function InPeriod(ByVal aString As String) As Boolean
            If aString <> "D" And aString <> "W" And aString <> "M" And aString <> "Q" And aString <> "H" And aString <> "Y" And aString <> "N" Then
                InPeriod = False
            Else
                InPeriod = True
            End If
        End Function

        Public Shared Function Inyn(ByVal aString As String) As Boolean
            If aString <> "Y" And aString <> "N" Then
                Inyn = False
            Else
                Inyn = True
            End If
        End Function

        Public Shared Function RecordOp(ByVal rString As String) As String
            Dim Cmd As New SqlCommand, OleDbConn As New SqlConnection

            OleDbConn.ConnectionString = DbSqlConnectionString()

            Cmd.CommandText = rString
            Cmd.Connection = OleDbConn
            Cmd.CommandType = CommandType.Text

            Try
                OleDbConn.Open()
                Cmd.ExecuteNonQuery()
                OleDbConn.Close()

                RecordOp = "Request Successfully Executed"
            Catch Exc As Exception
                RecordOp = Exc.Message
            Finally
                OleDbConn.Close()
            End Try

        End Function


        Public Shared Function AXRecordOp(ByVal rString As String) As String
            Dim Cmd As New SqlCommand, OleDbConn As New SqlConnection

            OleDbConn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("EazyConnectSource").ConnectionString()

            Cmd.CommandText = rString
            Cmd.Connection = OleDbConn
            Cmd.CommandType = CommandType.Text

            Try
                OleDbConn.Open()
                Cmd.ExecuteNonQuery()
                OleDbConn.Close()

                AXRecordOp = "Request Successfully Executed"
            Catch Exc As Exception
                AXRecordOp = Exc.Message
            Finally
                OleDbConn.Close()
            End Try

        End Function



        Public Shared Function RecordOp2(ByVal rString As String, ByVal ConnString As String) As String
            Dim Cmd As New SqlCommand, OleDbConn As New SqlConnection

            OleDbConn.ConnectionString = DbSqlConnectionString()

            Cmd.CommandText = rString
            Cmd.Connection = OleDbConn
            Cmd.CommandType = CommandType.Text

            Try
                OleDbConn.Open()
                Cmd.ExecuteNonQuery()
                OleDbConn.Close()

                RecordOp2 = "Request Successfully Executed"
            Catch Exc As Exception
                RecordOp2 = Exc.Message
            Finally
                OleDbConn.Close()
            End Try

        End Function

        Public Shared Function GetDataset(ByVal rString As String) As DataSet
            '  This functions returns o Dataset based on a Strored Procedure or
            '  Select statement passed as the rString parameter

            Dim Cmd As New SqlCommand, OleDbConn As New SqlConnection
            Dim Adapter As New SqlDataAdapter, Ds As New DataSet

            OleDbConn.ConnectionString = DbSqlConnectionString()

            Cmd.CommandText = rString
            Cmd.Connection = OleDbConn
            Cmd.CommandType = CommandType.Text

            Adapter.SelectCommand = Cmd
            Adapter.Fill(Ds, "Table")

            Try
                OleDbConn.Open()
                Cmd.ExecuteNonQuery()
                OleDbConn.Close()

                Return Ds
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleDbConn.Close()
            End Try

        End Function



        Public Shared Function AXGetDataset(ByVal rString As String) As DataSet
            '  This functions returns o Dataset based on a Strored Procedure or
            '  Select statement passed as the rString parameter

            Dim Cmd As New SqlCommand, OleDbConn As New SqlConnection
            Dim Adapter As New SqlDataAdapter, Ds As New DataSet

            OleDbConn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("EazyConnectSource").ConnectionString()

            Cmd.CommandText = rString
            Cmd.Connection = OleDbConn
            Cmd.CommandType = CommandType.Text

            Adapter.SelectCommand = Cmd
            Adapter.Fill(Ds, "Table")

            Try
                OleDbConn.Open()
                Cmd.ExecuteNonQuery()
                OleDbConn.Close()

                Return Ds
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleDbConn.Close()
            End Try

        End Function

        Public Shared Function GetDataset2(ByVal rString As String, ByVal ConnString As String) As DataSet
            '  This functions returns o Dataset based on a Strored Procedure or
            '  Select statement passed as the rString parameter

            Dim Cmd As New SqlCommand, OleDbConn As New SqlConnection
            Dim Adapter As New SqlDataAdapter, Ds As New DataSet

            OleDbConn.ConnectionString = ConnString

            Cmd.CommandText = rString
            Cmd.Connection = OleDbConn
            Cmd.CommandType = CommandType.Text

            Adapter.SelectCommand = Cmd
            Adapter.Fill(Ds, "Table")

            Try
                OleDbConn.Open()
                Cmd.ExecuteNonQuery()
                OleDbConn.Close()

                Return Ds
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleDbConn.Close()
            End Try

        End Function

        Public Shared Function GetSqlDataset2(ByVal rString As String, ByVal ConnString As String) As DataSet
            '  This functions returns o Dataset based on a Strored Procedure or
            '  Select statement passed as the rString parameter

            Dim Cmd As New SqlCommand, OleDbConn As New SqlConnection
            Dim Adapter As New SqlDataAdapter, Ds As New DataSet

            OleDbConn.ConnectionString = ConnString

            Cmd.CommandText = rString
            Cmd.Connection = OleDbConn
            Cmd.CommandType = CommandType.Text

            Adapter.SelectCommand = Cmd
            Adapter.Fill(Ds, "Table")

            Try
                OleDbConn.Open()
                Cmd.ExecuteNonQuery()
                OleDbConn.Close()

                Return Ds
            Catch Exc As Exception
                Console.WriteLine(Exc.Message)
            Finally
                OleDbConn.Close()
            End Try

        End Function

        Public Shared Function FormatAmount(ByVal amount) As String
            Dim cLen As Integer

            If amount = "" Then
                FormatAmount = 0
                Exit Function
            End If

            cLen = Len(amount)

            If InTMB(Mid(Trim(amount), cLen, 1)) Then
                amount = Format(FullAmount(amount), "#,##0.00")
            End If

            FormatAmount = Format(CDbl(amount), "#,##0.00;#,##0.00")
        End Function
        Public Shared Function GetField(ByVal rString As String) As String
            Dim OleConn As New SqlConnection, ds As New DataSet

            Try
                OleConn.ConnectionString = DbSqlConnectionString()
                Dim Cmd As New SqlDataAdapter(rString, OleConn.ConnectionString)
                Cmd.Fill(ds)
                Return ds.Tables(0).Rows(0).Item(0)
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            Finally
                OleConn.Close()
            End Try

        End Function

        Public Shared Function AxGetField(ByVal rString As String) As String
            Dim OleConn As New SqlConnection, ds As New DataSet

            Try
                OleConn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("EazyConnectSource").ConnectionString()
                Dim Cmd As New SqlDataAdapter(rString, OleConn.ConnectionString)
                Cmd.Fill(ds)
                Return ds.Tables(0).Rows(0).Item(0)
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            Finally
                OleConn.Close()
            End Try

        End Function

        Public Shared Function GetBitField(ByVal rString As String) As Boolean
            Dim OleConn As New SqlConnection, ds As New DataSet

            Try
                OleConn.ConnectionString = DbSqlConnectionString()
                Dim Cmd As New SqlDataAdapter(rString, OleConn.ConnectionString)
                Cmd.Fill(ds)
                Return ds.Tables(0).Rows(0).Item(0)
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            Finally
                OleConn.Close()
            End Try

        End Function
        Public Shared Function AxGetBitField(ByVal rString As String) As Boolean
            Dim OleConn As New SqlConnection, ds As New DataSet

            Try
                OleConn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("EazyConnectSource").ConnectionString()
                Dim Cmd As New SqlDataAdapter(rString, OleConn.ConnectionString)
                Cmd.Fill(ds)
                Return ds.Tables(0).Rows(0).Item(0)
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            Finally
                OleConn.Close()
            End Try

        End Function
        Public Shared Function GetBitField2(ByVal rString As String, ByVal ConnString As String) As Boolean
            Dim OleConn As New SqlConnection, ds As New DataSet

            Try
                OleConn.ConnectionString = ConnString
                Dim Cmd As New SqlDataAdapter(rString, OleConn.ConnectionString)
                Cmd.Fill(ds)
                Return ds.Tables(0).Rows(0).Item(0)
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            Finally
                OleConn.Close()
            End Try

        End Function

        Public Shared Function GetField2(ByVal rString As String, ByVal ConnString As String) As String
            Dim OleConn As New SqlConnection, ds As New DataSet

            Try
                OleConn.ConnectionString = ConnString
                Dim Cmd As New SqlDataAdapter(rString, OleConn.ConnectionString)
                Cmd.Fill(ds)
                Return ds.Tables(0).Rows(0).Item(0)
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            Finally
                OleConn.Close()
            End Try

        End Function

        Public Shared Function GetFieldDate(ByVal rString As String) As Date

            Dim OleConn As New SqlConnection, ds As New DataSet

            Try
                OleConn.ConnectionString = DbSqlConnectionString()
                Dim Cmd As New SqlDataAdapter(rString, OleConn.ConnectionString)
                Cmd.Fill(ds)
                Return ds.Tables(0).Rows(0).Item(0)
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            Finally
                OleConn.Close()
            End Try

        End Function

        Public Shared Function EncryptString(ByVal rString As String) As String
            Dim sLen As Integer, i As Integer

            If rString = "xxx" Then
                EncryptString = rString
                Exit Function
            End If

            If Len(Trim(rString)) > 0 Then
                sLen = Len(Trim(rString))

                For i = 1 To sLen
                    EncryptString = EncryptString & Chr(Asc(Mid(rString, i, 1)) + 5)
                Next
            End If
        End Function

        Public Shared Function DecryptString(ByVal rString As String) As String
            Dim sLen As Integer, i As Integer

            If Len(Trim(rString)) > 0 Then
                sLen = Len(Trim(rString))

                For i = 1 To sLen
                    DecryptString = DecryptString & Chr(Asc(Mid(rString, i, 1)) - 5)
                Next
            End If
        End Function

        Public Shared Function UserValidate(ByVal username As String, ByVal passwd As String) As String
            Dim result As String

            If passwd <> "xxx" Then
                passwd = EncryptString(passwd)
            End If

            result = GetField("SELECT user_id FROM AppUsers WHERE user_id = '" & username & "' AND password = '" & passwd & "'")

            Return result
        End Function

        Public Shared Function IsPasswordExpired(ByVal username As String, ByVal passdur As Integer) As Boolean
            Dim result As Date

            result = GetFieldDate("SELECT last_password_change FROM AppUsers WHERE user_id = '" & username & "'")

            If DateAdd(DateInterval.Day, passdur, result) <= Today.Date Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function DaysPasswordToExpire(ByVal username As String, ByVal passdur As Integer) As Integer
            Dim result As Date, expirydate As Date

            result = GetFieldDate("SELECT last_password_change FROM AppUsers WHERE user_id = '" & username & "'")

            expirydate = DateAdd(DateInterval.Day, passdur, result)

            Return DateDiff(DateInterval.Day, Today.Date, expirydate)
        End Function

        Public Shared Function IsUserLocked(ByVal username As String) As Boolean
            Dim result As String

            result = GetField("SELECT user_id FROM AppUsers WHERE user_id = '" & username & "' AND (locked <> 0 Or disabled <> 0)")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function GetGraceCount(ByVal username As String) As Integer
            Dim result As Integer

            result = GetField("SELECT grace_count FROM AppUsers WHERE user_id = '" & username & "'")
            Return result
        End Function

        Public Shared Function GetReport(ByVal objno As Integer) As String
            Return GetField("SELECT obj_name FROM AppObjects WHERE obj_no = " & objno & "")
        End Function

        Public Shared Function GetReportDetails(ByVal objno As Integer) As DataSet
            Return GetDataset("SELECT * FROM AppObjects WHERE obj_no = " & objno & "")
        End Function

        Public Shared Function IncreaseGraceCount(ByVal username As String)
            RecordOp("UPDATE AppUsers SET grace_count = grace_count + 1 WHERE user_id = '" & username & "'")
        End Function

        Public Shared Function LockUserAccount(ByVal username As String)
            RecordOp("UPDATE AppUsers SET locked = -1 WHERE user_id = '" & username & "'")
        End Function

        Public Shared Function DisableUserAccount(ByVal username As String)
            RecordOp("UPDATE AppUsers SET disabled = -1 WHERE user_id = '" & username & "'")
        End Function

        Public Shared Function UnlockUserAccount(ByVal username As String)
            RecordOp("UPDATE AppUsers SET locked = 0,grace_count = 0 WHERE user_id = '" & username & "'")
        End Function

        Public Shared Function EnableUserAccount(ByVal username As String)
            RecordOp("UPDATE AppUsers SET disabled = 0 WHERE user_id = '" & username & "'")
        End Function

        Public Shared Function ClearUserAccount(ByVal username As String)
            RecordOp("DELETE FROM AppUsersLogged WHERE user_id = '" & username & "'")
        End Function

        Public Shared Function ResetUserPassword(ByVal username As String)
            RecordOp("UPDATE AppUsers SET password = 'xxx',locked = 0,disabled = 0,grace_count = 0 WHERE user_id = '" & username & "'")
        End Function
        Public Shared Function AddLoggedUser(ByVal username As String)
            If UCase(username) <> "MASTER" Then
                RecordOp("INSERT INTO AppUsersLogged (user_id) VALUES ('" & username & "')")
            End If
        End Function

        Public Shared Function IsCorrectPassword(ByVal username As String, ByVal passwd As String) As Boolean
            If passwd = "xxx" Then
                IsCorrectPassword = True
                Exit Function
            End If

            Dim result As String
            result = GetField("SELECT password FROM AppUsers WHERE user_id = '" & username & "'")

            If DecryptString(result) = passwd Then
                IsCorrectPassword = True
            Else
                IsCorrectPassword = False
            End If
        End Function

        Public Shared Function IsPasswordUsed(ByVal username As String, ByVal passwd As String) As Boolean
            Dim result As String

            passwd = EncryptString(passwd)
            result = GetField("SELECT password FROM AppPwdLog WHERE user_id = '" & username & "' AND  password = '" & passwd & "'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function IsUserLogged(ByVal username As String) As Boolean
            Dim result As String

            result = GetField("SELECT user_id FROM AppUsersLogged WHERE user_id = '" & username & "'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function ChangePassword(ByVal username As String, ByVal passwd As String) As String
            passwd = EncryptString(passwd)
            'RecordOp("Sproc_AddAppPwdLog '" & username & "'")
            'RecordOp("Sproc_ChangePassword '" & username & "', '" & passwd & "'")

            Dim conn As SqlConnection = New SqlConnection(DbSqlConnectionString())

            Dim cmd As SqlCommand = New SqlCommand
            conn.Open()


            cmd.CommandText = "Sproc_AddAppPwdLog"
            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add(New SqlParameter("@UserID", (username)))
            cmd.ExecuteNonQuery()
            conn.Close()

            Dim cmdChangePassword As SqlCommand = New SqlCommand
            conn.Open()
            cmdChangePassword.CommandText = "Sproc_ChangePassword"
            cmdChangePassword.Connection = conn
            cmdChangePassword.CommandType = CommandType.StoredProcedure
            cmdChangePassword.Parameters.Add(New SqlParameter("@UserID", (username)))
            cmdChangePassword.Parameters.Add(New SqlParameter("@NewPassword", (passwd)))
            cmdChangePassword.ExecuteNonQuery()
            conn.Close()
            ChangePassword = "Password Changed"
        End Function

        Public Shared Function ChangePasswordWithDetails(ByVal username As String, ByVal passwd As String, MobileNo As String, urlInfo As String, location As String) As String
            ' passwd = EncryptString(passwd)
            'RecordOp("Sproc_AddAppPwdLog '" & username & "'")
            'RecordOp("Sproc_ChangePassword '" & username & "', '" & passwd & "'")

            Dim conn As SqlConnection = New SqlConnection(DbSqlConnectionString())

            Dim cmd As SqlCommand = New SqlCommand
            conn.Open()


            cmd.CommandText = "Sproc_AddAppPwdLogWithDetails"
            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add(New SqlParameter("@UserID", (username)))
            cmd.Parameters.Add(New SqlParameter("@mobileno", (MobileNo)))
            cmd.Parameters.Add(New SqlParameter("@urlinfo", (urlInfo)))
            cmd.Parameters.Add(New SqlParameter("@location", (location)))

            cmd.ExecuteNonQuery()
            conn.Close()

            Dim cmdChangePassword As SqlCommand = New SqlCommand
            conn.Open()
            cmdChangePassword.CommandText = "Sproc_ChangePassword"
            cmdChangePassword.Connection = conn
            cmdChangePassword.CommandType = CommandType.StoredProcedure
            cmdChangePassword.Parameters.Add(New SqlParameter("@UserID", (username)))
            cmdChangePassword.Parameters.Add(New SqlParameter("@NewPassword", (passwd)))
            cmdChangePassword.ExecuteNonQuery()
            conn.Close()
            ChangePasswordWithDetails = "Password Changed"
        End Function



        Public Shared Function IsPermGranted(ByVal userid As String, ByVal objno As Integer) As Boolean
            Dim result As String

            result = GetField("SELECT user_id FROM VwAppUsersAndRoles WHERE user_id = '" & userid & "' AND obj_no = " & CInt(objno) & " ")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        'Public Shared Function ErrorCode(ByVal code As Integer) As String
        '    Dim result As String

        '    result = GetField("SELECT error_desc FROM ErrorCodes WHERE error_code = " & code & "")

        '    If ErrorCodeType(code) = "1" Then
        '        Return "Error# " & CStr(code) & Chr(13) & result
        '    ElseIf ErrorCodeType(code) = "2" Then
        '        Return result
        '    Else
        '        Return "Message# " & CStr(code) & Chr(13) & result
        '    End If
        'End Function

        Public Shared Function ErrorCodeWeb(ByVal code As Integer) As String



            Select Case code
                Case Is = 1
                    Return code & " - " & "A Budget Session is Currently  On"

                Case Is = 2
                    Return code & " - " & "A Budget Session with the following Number is  currently  opened"
                Case Is = 3
                    Return code & " - " & "Error# : Account Invalid for this Branch"
                Case Is = 4
                    Return code & " - " & "Error# : Account is Invalid for this Sub-Org"
                Case Is = 5
                    Return code & " - " & "Error# : Account is not an Active and Valid Posting Account"
                Case Is = 6
                    Return code & " - " & "Error# : Account is not an Active Valid Cash Account"

                Case Is = 7
                    Return code & " - " & "Error# : Account is not Valid for this Posting"
                Case Is = 8
                    Return code & " - " & "Error# : Account not Valid for the Currently Logged Site"
                Case Is = 9
                    Return code & " - " & "Error# : Main or Contra Account not Valid for this Operation"
                Case Is = 10
                    Return code & " - " & "Message#: Add this Account Budget Record?"
                Case Is = 11
                    Return code & " - " & "Error# : Advance Account is not allowed in this context"
                Case Is = 12
                    Return code & " - " & "Error# : Advance Account not Valid for Logon User"
                Case Is = 13
                    Return code & " - " & "Message#: Advance Reference Number is: "
                Case Is = 14
                    Return code & " - " & "Error# : Amount Distribution Error"
                Case Is = 15
                    Return code & " - " & "Message#: Approve and Post this Budget Session?"
                Case Is = 17
                    Return code & " - " & "Message#: Approve this Referenced Transaction?"
                Case Is = 18
                    Return code & " - " & "Message#: Approve this Opened Account?"
                Case Is = 19
                    Return code & " - " & "Message#: Approve this Record?"
                Case Is = 20
                    Return code & " - " & "Error# : Cash Account is not allowed in this context"
                Case Is = 21
                    Return code & " - " & "Error# : Can't Approve and Post this Budget because Budget not Balanced or not for Current Year"
                Case Is = 22
                    Return code & " - " & "Error# : Can't Save because no Reference No."
                Case Is = 23
                    Return code & " - " & "Error# : Can't Save, Posting Limit Exceeded"
                Case Is = 24
                    Return code & " - " & "Error# : Can't Save, User Posting Limit is Exceeded"
                Case Is = 25
                    Return code & " - " & "Error# : Cash Account is not allowed in this context"
                Case Is = 26
                    Return code & " - " & "Message#: Delete Role?"
                Case Is = 27
                    Return code & " - " & "Message#: Delete this Account?"
                Case Is = 28
                    Return code & " - " & "Message#: Delete this Advance?"
                Case Is = 29
                    Return code & " - " & "Message#: Delete this Chart Code Item?"
                Case Is = 30
                    Return code & " - " & "Message#: Delete this Drawdown?"
                Case Is = 31
                    Return code & " - " & "Message#: Delete this Entry?"
                Case Is = 32
                    Return code & " - " & "Message#: Delete this Facility?"
                Case Is = 33
                    Return code & " - " & "Message#: Delete this Head Code?"
                Case Is = 34
                    Return code & " - " & "Message#: Delete this Item?"
                Case Is = 35
                    Return code & " - " & "Message#: Delete this Journal?"
                Case Is = 36
                    Return code & " - " & "Message#: Delete this Payment?"
                Case Is = 37
                    Return code & " - " & "Message#: Delete this Receipt?"
                Case Is = 38
                    Return code & " - " & "Message#: Delete this Record and Distribution?"
                Case Is = 39
                    Return code & " - " & "Message#: Delete this Record?"
                Case Is = 40
                    Return code & " - " & "Message#: Delete this Settlement"
                Case Is = 41
                    Return code & " - " & "Message#: Allow Forced/Unconditional Dropping of PDOs"
                Case Is = 42
                    Return code & " - " & "Message#: Delete User's Profile?"
                Case Is = 43
                    Return code & " - " & "Drawdown Reference Number is: "
                Case Is = 44
                    Return code & " - " & "Entries Successfully Posted"
                Case Is = 45
                    Return code & " - " & "Entry has been Approved or Posted"
                Case Is = 46
                    Return code & " - " & "Error# : Entry is not Valid in this Context"
                Case Is = 47
                    Return code & " - " & "Message#: Execute Budget Data Integrity Checks?"
                Case Is = 53
                    Return code & " - " & "Message#: Generate/Consolidate Financials?"
                Case Is = 54
                    Return code & " - " & "Message#: Generate New Facility Number?"
                Case Is = 55
                    Return code & " - " & "Message#: Generate New Journal Number for the Selected Journal Type?"
                Case Is = 56
                    Return code & " - " & "Error# : Incompatible Account Currencies"
                Case Is = 57
                    Return code & " - " & "Error# : Inconsistent Amounts"
                Case Is = 58
                    Return code & " - " & "Error# : Inconsistent Confirmed Password"
                Case Is = 59
                    Return code & " - " & "Error# : Invalid Amount"
                Case Is = 60
                    Return code & " - " & "Error# : Invalid Budget Amount"
                Case Is = 61
                    Return code & " - " & "Error# : Invalid Budget Amount, can't Add"
                Case Is = 62
                    Return code & " - " & "Error# : Invalid Budget Code"
                Case Is = 63
                    Return code & " - " & "Error# : Invalid Exchange Rate"
                Case Is = 64
                    Return code & " - " & "Error# : Invalid Old Password"
                Case Is = 65
                    Return code & " - " & "Error# : Invalid Trans Type. It must be D or C"
                Case Is = 66
                    Return code & " - " & "Error# : Invalid Transaction Type"
                Case Is = 67
                    Return code & " - " & "Error# : Invalid User"
                Case Is = 68
                    Return code & " - " & "Error# : Invalid Year of Budget"
                Case Is = 69
                    Return code & " - " & "Error# : Invalid Tenor"
                Case Is = 70
                    Return code & " - " & "Error# : Journal already Sent for Approval. You may recall it"
                Case Is = 71
                    Return code & " - " & "Error# : Journal does not Exist or already Approved"
                Case Is = 72
                    Return code & " - " & "Error# : Journal not earlier Sent for Approval"
                Case Is = 73
                    Return code & " - " & "Error# : Journal not for Currently Logged Site"
                Case Is = 74
                    Return code & " - " & "Error# : Entries Out of Balance"
                Case Is = 75
                    Return code & " - " & "Error# : No Budget Session is Currently Opened"
                Case Is = 76
                    Return code & " - " & "Error# : No Such Approved Facility"
                Case Is = 77
                    Return code & " - " & "Message#: Open New Budget Session?"
                Case Is = 78
                    Return code & " - " & "Error# : Password previously used"
                Case Is = 79
                    Return code & " - " & "Error# : Payment Date cannot be more Posting Date"
                Case Is = 80
                    Return code & " - " & "Error# : Please Enter Description"
                Case Is = 81
                    Return code & " - " & "Message#: Please ensure that Starting No. is not be greater than Ending No."
                Case Is = 82
                    Return code & " - " & "Message#: Please Enter a Journal No."
                Case Is = 83
                    Return code & " - " & "Message#: Please Enter a Valid Interest Rate"
                Case Is = 84
                    Return code & " - " & "Message#: Please Enter a Valid Principal Amount"
                Case Is = 85
                    Return code & " - " & "Message#: Please Enter Main Account/Contra No."
                Case Is = 86
                    Return code & " - " & "Message#: Please Enter Valid Amount"
                Case Is = 87
                    Return code & " - " & "Message#: Please Enter Beneficiary's Name"
                Case Is = 88
                    Return code & " - " & "Message#: Please Enter Budget Code"
                Case Is = 89
                    Return code & " - " & "Message#: Please Enter Cash or Bank Account"
                Case Is = 90
                    Return code & " - " & "Message#: Please Enter Code"
                Case Is = 91
                    Return code & " - " & "Message#: Please Enter Control Account"
                Case Is = 92
                    Return code & " - " & "Message#: Please Enter Description"
                Case Is = 93
                    Return code & " - " & "Message#: Please Enter Ending No."
                Case Is = 94
                    Return code & " - " & "Message#: Please Enter Exchange Rate"
                Case Is = 95
                    Return code & " - " & "Message#: Please Enter Facility No."
                Case Is = 96
                    Return code & " - " & "Message#: Please Enter Journal No."
                Case Is = 97
                    Return code & " - " & "Message#: Please Enter Name"
                Case Is = 98
                    Return code & " - " & "Message#: Please Enter Payer's Name"
                Case Is = 99
                    Return code & " - " & "Message#: Please Enter Payment Amount"
                Case Is = 100
                    Return code & " - " & "Message#: Please Enter Project No."
                Case Is = 101
                    Return code & " - " & "Message#: Please Enter Receipt Amount"
                Case Is = 102
                    Return code & " - " & "Message#: Please Enter Role Description"
                Case Is = 103
                    Return code & " - " & "Message#: Please Enter Role ID No."
                Case Is = 104
                    Return code & " - " & "Message#: Please Enter Starting No."
                Case Is = 105
                    Return code & " - " & "Message#: Please Enter Subhead Code"
                Case Is = 106
                    Return code & " - " & "Message#: Please Enter the Code"
                Case Is = 107
                    Return code & " - " & "Message#: Please Enter the Transaction Description"
                Case Is = 108
                    Return code & " - " & "Message#: Please Enter Transaction Description."
                Case Is = 109
                    Return code & " - " & "Message#: Please Enter User ID"
                Case Is = 110
                    Return code & " - " & "Message#: Please Enter User Names"
                Case Is = 111
                    Return code & " - " & "Message#: Please Enter Valid Exchange Rate"
                Case Is = 112
                    Return code & " - " & "Message#: Please Enter Value"
                Case Is = 113
                    Return code & " - " & "Message#: Please Enter Voucher No."
                Case Is = 114
                    Return code & " - " & "Message#: Please Enter Year of Budget"
                Case Is = 115
                    Return code & " - " & "Message#: Please Select a Purpose"
                Case Is = 116
                    Return code & " - " & "Message#: Please Select a Record"
                Case Is = 117
                    Return code & " - " & "Message#: Please Select a Valid Interest Rate Type"
                Case Is = 118
                    Return code & " - " & "Message#: Please Select 'Bal As At'"
                Case Is = 119
                    Return code & " - " & "Message#: Please Select Collateral Type"
                Case Is = 120
                    Return code & " - " & "Message#: Please Select Distribution Type"
                Case Is = 121
                    Return code & " - " & "Message#: Please Select Facility Type"
                Case Is = 122
                    Return code & " - " & "Message#: Please select Head/Sub-org to log onto"
                Case Is = 123
                    Return code & " - " & "Message#: Please Select Item Class"
                Case Is = 124
                    Return code & " - " & "Message#: Please Select Journal"
                Case Is = 125
                    Return code & " - " & "Message#: Please Select Level 2 Code"
                Case Is = 126
                    Return code & " - " & "Message#: Please Select Parent Head Code"
                Case Is = 127
                    Return code & " - " & "Message#: Please Select Status"
                Case Is = 128
                    Return code & " - " & "Message#: Please Select Type"
                Case Is = 129
                    Return code & " - " & "Message#: Please Select Type Code"
                Case Is = 130
                    Return code & " - " & "Message#: Please Set Code"
                Case Is = 131
                    Return code & " - " & "Error# : Post Date cannot be more than Open Period's Date"
                Case Is = 132

                    Return code & " - " & "Error# : Process Halted, Approval Limit Violation"
                Case Is = 133
                    Return code & " - " & "Error# : Process Halted,User Posting Limit Violation"
                Case Is = 134
                    Return code & " - " & "Error# : Receipt Date cannot be more Posting Date"
                Case Is = 135
                    Return code & " - " & "Request Granted"
                Case Is = 136
                    Return code & " - " & "Error# : Request Rejected because Minimum Length of Password is"
                Case Is = 137
                    Return code & " - " & "Message#: Save the new Status?"
                Case Is = 138
                    Return code & " - " & "Message#: Save this Budget Control Total?"
                Case Is = 139
                    Return code & " - " & "Message#: Save this Drawdown?"
                Case Is = 140
                    Return code & " - " & "Message#: Save this Item?"
                Case Is = 141
                    Return code & " - " & "Message#: Save this Payment?"
                Case Is = 142
                    Return code & " - " & "Message#: Save this Receipt?"
                Case Is = 143
                    Return code & " - " & "Message#: Save this Settlement?"
                Case Is = 144
                    Return code & " - " & "Message#: Save/Update Facility?"
                Case Is = 145
                    Return code & " - " & "Message#: Save/Update Flexi Account?"
                Case Is = 146
                    Return code & " - " & "Message#: Save/Update Record?"
                Case Is = 147
                    Return code & " - " & "Message#: Save/Update Role?"
                Case Is = 148
                    Return code & " - " & "Message#: Save/Update this Chart Code Item?"
                Case Is = 149
                    Return code & " - " & "Message#: Save/Update this Entry?"
                Case Is = 150
                    Return code & " - " & "Message#: Save/Update this Head Code?"
                Case Is = 151
                    Return code & " - " & "Message#: Save/Update this Journal?"
                Case Is = 152
                    Return code & " - " & "Message#: Save/Update this Record?"
                Case Is = 153
                    Return code & " - " & "Message#: Save/Update this Sub-Org?"
                Case Is = 154
                    Return code & " - " & "Message#: Save/Update User's Profile?"
                Case Is = 155
                    Return code & " - " & "Settlement Reference Number is: "
                Case Is = 156
                    Return code & " - " & "Error# : Sorry, you are currently Logged on another workstation"
                Case Is = 157
                    Return code & " - " & "Error# : There is no Journal to Approve"
                Case Is = 158
                    Return code & " - " & "Message#: This action will delete entire budget session and all entries made. Do you really want to continue?"
                Case Is = 159
                    Return code & " - " & "Message#: This action will Post all Approved Entries. Do you really want to continue?"
                Case Is = 160
                    Return code & " - " & "This Operation is only allowed for Flexi Accounts"
                Case Is = 161
                    Return code & " - " & "Error# : Transaction Date cannot be later than Posting Date"
                Case Is = 162
                    Return code & " - " & "Error# : Transaction Date cannot be more Posting Date"
                Case Is = 163
                    Return code & " - " & "Message#: Update Enterprise Policies?"
                Case Is = 164
                    Return code & " - " & "Error# : Permission not Granted"
                Case Is = 165
                    Return code & " - " & "You must enter a Transaction Date that is not later than current Posting Date"
                Case Is = 166
                    Return code & " - " & "Message#: You sure want to Activate this Site with the Selected Items?"
                Case Is = 167
                    Return code & " - " & "Message#: You sure want to Exit?"
                Case Is = 168
                    Return code & " - " & "Message#: You sure want to Update Accounts Chart with New Items?"
                Case Is = 169
                    Return code & " - " & "Your account is Locked or Disabled. Contact your System Administrator"
                Case Is = 170
                    Return code & " - " & "Your password has Expired and your Account Locked. Contact your System Administrator"
                Case Is = 171
                    Return code & " - " & "Your password has Expired needs changing.  You have remaining Grace Logon of "
                Case Is = 172
                    Return code & " - " & "Message#: Reject opening of this Account?"
                Case Is = 173
                    Return code & " - " & "Message#: Reject this Referral?"
                Case Is = 174
                    Return code & " - " & "Message#: Reject this Payment/Receipt?"
                Case Is = 175
                    Return code & " - " & "Message#: Reject this Facility?"
                Case Is = 176
                    Return code & " - " & "Please select a Head for Report"
                Case Is = 177
                    Return code & " - " & "Please select a Sub-Org for Report"
                Case Is = 178
                    Return code & " - " & "Please select an Item Class for Report"
                Case Is = 179
                    Return code & " - " & "Error# : Invalid Date to Open"
                Case Is = 180
                    Return code & " - " & "Error# : Inconsistent Dates entered"
                Case Is = 181
                    Return code & " - " & "Message#: Some entries are not yet approved. Push Forward? Click Yes to Push, No to Delete, Cancel to stop Date Opening"
                Case Is = 182
                    Return code & " - " & "Message#: Are you sure you want to Close this Period?"
                Case Is = 183
                    Return code & " - " & "Budget Limit Exceeded, can't save"
                Case Is = 184
                    Return code & " - " & "Message#: Please Enter a Mobile Phone Number to send SMS Text"
                Case Is = 185
                    Return code & " - " & "Message#: Please Enter the Message you wish to send"
                Case Is = 186
                    Return code & " - " & "Message#: Overwrite the selected month's budget amount?"
                Case Is = 187
                    Return code & " - " & "Message#: Clear Distribution Section only?"
                Case Is = 188
                    Return code & " - " & "Project does not Exist, Unapproved or Completed"
                Case Is = 189
                    Return code & " - " & "Error# : Invalid OD Charge Type"
                Case Is = 190
                    Return code & " - " & "Error# : Invalid Project Description"
                Case Is = 191
                    Return code & " - " & "Error# : Invalid Project Type"
                Case Is = 192
                    Return code & " - " & "Error# : Invalid OD Charge Rate"
                Case Is = 193
                    Return code & " - " & "Error# : Invalid Project Fcy Amount"
                Case Is = 194
                    Return code & " - " & "Error# : Invalid Project Currency"
                Case Is = 195
                    Return code & " - " & "Project start date must be earlier than project end date"
                Case Is = 196
                    Return code & " - " & "Can't delete at this stage.  Payments already made on project"
                Case Is = 197
                    Return code & " - " & "Message#: Reject this Project?"
                Case Is = 198
                    Return code & " - " & "Message#: Generate New Flexi Account Number?"
                Case Is = 199
                    Return code & " - " & "No such approved Facility in current site"
                Case Is = 200
                    Return code & " - " & "No such Advance in current site"
                Case Is = 201
                    Return code & " - " & "Message#: Please select a Currency"
                Case Is = 202
                    Return code & " - " & "You cannot initiate Cash Advance here.  Use the Payments module instead"
                Case Is = 203
                    Return code & " - " & "You must enter a Cash Advance number to settle"
                Case Is = 204
                    Return code & " - " & "Only Cash Accounts allowed in Cash Journals"
                Case Is = 205
                    Return code & " - " & "Message#: Is the selected and displayed Journal Type Correct?"
                Case Is = 206
                    Return code & " - " & "Message#: Refer Transaction for Approval?"
                Case Is = 207
                    Return code & " - " & "Message#: Recall the Referred Transaction and Suspend Approval?"
                Case Is = 208
                    Return code & " - " & "Error# : Account is not a valid Invoice Account"
                Case Is = 209
                    Return code & " - " & "Error# : Invoice Account not allowed in this context"
                Case Is = 210
                    Return code & " - " & "Message#: Auto-generate Invoice Number?"
                Case Is = 211
                    Return code & " - " & "Error# : Invalid quantity entered"
                Case Is = 212
                    Return code & " - " & "Error# : Invalid Unit Price/Cost"
                Case Is = 213
                    Return code & " - " & "Account must be an Asset or Liability account"
                Case Is = 214
                    Return code & " - " & "Project Closing Date cannot be earlier than Start Date"
                Case Is = 215
                    Return code & " - " & "Project Clsoing Date cannot be earlier than last Payment Date"
                Case Is = 216
                    Return code & " - " & "Project already completed or closed"
                Case Is = 217
                    Return code & " - " & "Message#: Are you sure you want to Close this Period?"
                Case Is = 218
                    Return code & " - " & "Please Enter a Valid Tenor"
                Case Is = 219
                    Return code & " - " & "Invalid Date"
                Case Is = 220
                    Return code & " - " & "No such approved Invoice in current site"
                Case Is = 221
                    Return code & " - " & "Message#: Refresh Trial Balance?"
                Case Is = 222
                    Return code & " - " & "Tax Accounts must be in System Base Currency"
                Case Is = 223
                    Return code & " - " & "Message#: Please Enter Officer No."
                Case Is = 224
                    Return code & " - " & "Message#: Please Enter Officer Names"
                Case Is = 225
                    Return code & " - " & "Message#: Please Select Officer's Marketing Unit"
                Case Is = 226
                    Return code & " - " & "Message#: Please Enter Customer Names, Address and City"
                Case Is = 227
                    Return code & " - " & "Message#: Please Enter Customer's Sex"
                Case Is = 228
                    Return code & " - " & "No Customer Number found"
                Case Is = 229
                    Return code & " - " & "Message#: Please Enter a Tele/Fax Number"
                Case Is = 230
                    Return code & " - " & "Message#: Please, Enter Setup Code"
                Case Is = 231
                    Return code & " - " & "Message#: Please, Enter Description 0r Narrative"
                Case Is = 232
                    Return code & " - " & "Message#: Please, Enter Control Mini Account"
                Case Is = 233
                    Return code & " - " & "Message#: Please, Enter Interest Application Frequency of M or Q"
                Case Is = 234
                    Return code & " - " & "Message#: Please Enter a Band"
                Case Is = 235
                    Return code & " - " & "Message#: Please, Enter Customer Number"
                Case Is = 236
                    Return code & " - " & "Error# : No Such Customer Number Exists"
                Case Is = 237
                    Return code & " - " & "Message#: Please, Enter Account Names"
                Case Is = 238
                    Return code & " - " & "Message#: Please, Enter Main Narrative"
                Case Is = 239
                    Return code & " - " & "Message#: Please, Enter Contra Narrative"
                Case Is = 240
                    Return code & " - " & "Transaction type of charge requires a Charge Code"
                Case Is = 241
                    Return code & " - " & "Looped transactions require the Parent Trans Code"
                Case Is = 242
                    Return code & " - " & "Loop Level cannot exceed 1 Level"
                Case Is = 243
                    Return code & " - " & "User Transaction Codes cannot exceed 700"
                Case Is = 244
                    Return code & " - " & "Error# : Trans field must be C for Credit or D for Debit"
                Case Is = 245
                    Return code & " - " & "No such Parent Trans Code Exist"
                Case Is = 246
                    Return code & " - " & "Only charge transactions type can have charge code not '000' and also Apply Charge on Debit"
                Case Is = 247
                    Return code & " - " & "Ceiling Amounts cannot be less than User Posting Limit Amounts"
                Case Is = 248

                    Return code & " - " & "Teller Account must be a Cash Account Type"
                Case Is = 249
                    Return code & " - " & "Error# : Transaction Code/Account Type Incompatibility"
                Case Is = 250
                    Return code & " - " & "Posting has been stopped by Administrator"
                Case Is = 251
                    Return code & " - " & "Debit Entries not allowed on this Account"
                Case Is = 252
                    Return code & " - " & "Credit Entries not allowed on this Account"
                Case Is = 253
                    Return code & " - " & "Error# : You are not permitted to post into other branches GL Accounts"
                Case Is = 254
                    Return code & " - " & "Error# : Account Limit cannot be Exceeded"
                Case Is = 256
                    Return code & " - " & "Error# : Transaction Code not entered/selected"
                Case Is = 257
                    Return code & " - " & "Error# : Future Value Dates not allowed"
                Case Is = 258
                    Return code & " - " & "Error# : Instrument No. is required"
                Case Is = 259
                    Return code & " - " & "Error# : Instrument Number has previously been used"
                Case Is = 260
                    Return code & " - " & "Error# : Instrument Number range does not exists"
                Case Is = 261
                    Return code & " - " & "Error# : Insufficient Till Cash"
                Case Is = 262
                    Return code & " - " & "Error# : Approved Posting Ceiling Limit cannot be exceeded"
                Case Is = 263
                    Return code & " - " & "Error# : Looping not allowed on Cash Transactions"
                Case Is = 264
                    Return code & " - " & "Error# : Journal Accounts Limit Violation.  View Journal Check Log"
                Case Is = 265
                    Return code & " - " & "Instrument Start and End numbers required"
                Case Is = 266
                    Return code & " - " & "Error# : Instrument End number cannot be less than Start number"
                Case Is = 267
                    Return code & " - " & "Error# : Instrument number range already exists or Approved"
                Case Is = 268
                    Return code & " - " & "Error# : Account and Product Type mismatch"
                Case Is = 269
                    Return code & " - " & "Error# : Instrument number range does not exist or has previously been revoked for the Account"
                Case Is = 270
                    Return code & " - " & "Message#: Commit the Request?"
                Case Is = 271
                    Return code & " - " & "Error# : Account type must be Teller Cash or Bank"
                Case Is = 272
                    Return code & " - " & "Error# : Account type cannot be Teller Cash or Bank"
                Case Is = 273
                    Return code & " - " & "Error# : Overdraft maintenance can only be done at the Head Office "
                Case Is = 274
                    Return code & " - " & "Please select a valid Approving Body"
                Case Is = 275
                    Return code & " - " & "Message#: Disapprove Entry?"
                Case Is = 276
                    Return code & " - " & "Error# : Other Bank Code must be Local Bank Code"
                Case Is = 277
                    Return code & " - " & "Error# : Clearing Instruments must have future Value Dates"
                Case Is = 278
                    Return code & " - " & "Please, select a Clearing Type"
                Case Is = 279
                    Return code & " - " & "Error# : Invalid Bank Code selected"
                Case Is = 280
                    Return code & " - " & "Error# : Invalid Clearing Days for clearing instruments"
                Case Is = 281
                    Return code & " - " & "Error# : No Such Cheque in Clearing. Cheque not entered, not approved or has already cleared"
                Case Is = 282
                    Return code & " - " & "Error# : Debit and Credit on same account not allowed for this transaction type"
                Case Is = 283
                    Return code & " - " & "Error# : Please select bank for instruments transactions"
                Case Is = 284
                    Return code & " - " & "Error# : Cheque has previously been lodged"
                Case Is = 285
                    Return code & " - " & "Error# : Please, select an approving officer"
                Case Is = 286
                    Return code & " - " & "Error# : Only Posted Transactions can be reversed"
                Case Is = 287
                    Return code & " - " & "Error# : SO Type and Sub-type required"
                Case Is = 288
                    Return code & " - " & "Error# : SO Type and Sub-type mismatch"
                Case Is = 289
                    Return code & " - " & "Error# : Main and Contra accounts are required"
                Case Is = 290
                    Return code & " - " & "Error# : Starting Date must be in the future"
                Case Is = 291
                    Return code & " - " & "Error# : Please, select a valid Frequency"
                Case Is = 292
                    Return code & " - " & "Error# : Please, enter SO Amount"
                Case Is = 293
                    Return code & " - " & "Error# : Please, enter Sweep Threshhold"
                Case Is = 294
                    Return code & " - " & "Error# : Please, specify the the Total amount for SO"
                Case Is = 295
                    Return code & " - " & "Error# : The Ending Date cannot be earlier than Starting Date"
                Case Is = 296
                    Return code & " - " & "Error# : Please, specify the number of times to execute SO"
                Case Is = 297
                    Return code & " - " & "Error# : You cannot have a Charge Amount and a Charge Code simultaneously"
                Case Is = 298
                    Return code & " - " & "Error# : You must enter a Charge Account for the entered Charge"
                Case Is = 299
                    Return code & " - " & "Error# : Main and Contra accounts cannot be the same"
                Case Is = 300
                    Return code & " - " & "Error# : Value Date cannot be earlier than Account Open Date"
                Case Is = 301
                    Return code & " - " & "Error# : Instrument Number is Stopped"
                Case Is = 302
                    Return code & " - " & "Error# : Invalid Stopping Reason selected"
                Case Is = 303
                    Return code & " - " & "Error# : Starting number cannot be greater than ending number"
                Case Is = 304
                    Return code & " - " & "Error# : Date Reported cannot be in the future"
                Case Is = 305
                    Return code & " - " & "Message#: Unstop the Selected Intrument"
                Case Is = 306
                    Return code & " - " & "Transaction Already Approved"
                Case Is = 307
                    Return code & " - " & "Error# : Some Instrument Leaflet already used in the Range"
                Case Is = 308
                    Return code & " - " & "Error# : Transaction/Value Date must fall withing Deal/Loan Duration"
                Case Is = 309
                    Return code & " - " & "Error# : Deal/Loan Account is an already an Active Account"
                Case Is = 310
                    Return code & " - " & "Error# : Only fresh booking can be done on zero balance accounts"
                Case Is = 311
                    Return code & " - " & "Error# : Deal/Loan Product is not Correctly Set up"
                Case Is = 312
                    Return code & " - " & "Error# : Transaction Date must be after last transaction or repayment date on Deal/Loan"
                Case Is = 313
                    Return code & " - " & "Error# : Invalid Interest Rate"
                Case Is = 314
                    Return code & " - " & "Error# : Invalid Tenor"
                Case Is = 315
                    Return code & " - " & "Error# : Invalid Year Base"
                Case Is = 316
                    Return code & " - " & "Error# : Invalid Penalty Rate"
                Case Is = 317
                    Return code & " - " & "Error# : Invalid Withholding Tax Rate"
                Case Is = 318
                    Return code & " - " & "Error# : Please select Marketing Officer"
                Case Is = 319
                    Return code & " - " & "Error# : Please select appropriate  Deal/Loan"
                Case Is = 320
                    Return code & " - " & "Error# : Deal/Loan Transaction and Account Mismatch"
                Case Is = 321
                    Return code & " - " & "Error# : Rate or Rollover Rate outside Approved Rate Band"
                Case Is = 322
                    Return code & " - " & "Error# : Deal/Loan already matured"
                Case Is = 323
                    Return code & " - " & "Error# : Account has a pending approval. It must be approved before another"
                Case Is = 324
                    Return code & " - " & "Error# : This operation is not allowed for Scheduled Loans"
                Case Is = 325
                    Return code & " - " & "Error# : Loan Operative Account is required"
                Case Is = 326
                    Return code & " - " & "Error# : Invalid Rollover Tenor or Rate"
                Case Is = 327
                    Return code & " - " & "Error# : Invalid Length of Head, Sub-org, Group Control or Line Item"
                Case Is = 328
                    Return code & " - " & "Error# : Accounts with balances cannot be converted to Controls"
                Case Is = 329
                    Return code & " - " & "Error# : Control Accounts with balances/transactions cannot be reconverted"
                Case Is = 330
                    Return code & " - " & "Error# : You can only use Properly Active Scheduled Loan Accounts"
                Case Is = 331
                    Return code & " - " & "Please note that Loan has an approved Repayment Schedule"
                Case Is = 332
                    Return code & " - " & "Error# : Invalid Annuity Tenor"
                Case Is = 333
                    Return code & " - " & "Error# : Invalid Number of Repayments"
                Case Is = 334
                    Return code & " - " & "Error# : Schedule Type and Frequency must be selected"
                Case Is = 335
                    Return code & " - " & "Error# : No Loan Schedule for this Loan.  Posting officer must generate it before Approval"
                Case Is = 336
                    Return code & " - " & "Error# : Repayment Date must be between 1st and 28th of the month"
                Case Is = 337
                    Return code & " - " & "Error# : Invalid Loan Balance"
                Case Is = 338
                    Return code & " - " & "Error# : Proxy Locations must have an associated GL Account"
                Case Is = 339
                    Return code & " - " & "Error# : Only Proxy Locations can be Activated here"
                Case Is = 340
                    Return code & " - " & "Error# : Only Non-Proxy Locations can be Activated here"
                Case Is = 341
                    Return code & " - " & "Error# : Please select a Maintenance Type"
                Case Is = 342
                    Return code & " - " & "Error# : You are not permitted to view or print Protected Accounts"
                Case Is = 343
                    Return code & " - " & "Error# : Customer  not Valid for the Currently Logged Site"
                Case Is = 344
                    Return code & " - " & "Error# : Liquidation Account is required for Auto-Rollover of Upfront Deals"
                Case Is = 345
                    Return code & " - " & "Error# : You must run EOD first"
                Case Is = 346
                    Return code & " - " & "Error# : SOD can only run for a date after last EOD"
                Case Is = 347
                    Return code & " - " & "Error# : SOD can only be run for date within the Current Month"
                Case Is = 348
                    Return code & " - " & "Error# : You cannot run SOD for a Public Holiday"
                Case Is = 349
                    Return code & " - " & "Error# : Saturday Banking not allowed"
                Case Is = 350
                    Return code & " - " & "Error# : Sunday Banking not allowed"
                Case Is = 351
                    Return code & " - " & "Message#: Commence Processing?"
                Case Is = 352
                    Return code & " - " & "Error# : End of Month must be run on the Last Day of Month"
                Case Is = 353
                    Return code & " - " & "Error# : Can't run SOM now, Run EOM first"
                Case Is = 354
                    Return code & " - " & "Error# : Can't run SOD, run SOM first"
                Case Is = 355
                    Return code & " - " & "Error# : Uncompunded Interest Products must have Interest Receival and Payable mini accounts"
                Case Is = 356
                    Return code & " - " & "Error# : Product Type and Attribute Mismatch"
                Case Is = 357
                    Return code & " - " & "Error# : Call Deposits and Placements must have Tenor of 1"
                Case Is = 358
                    Return code & " - " & "Error# : Invalid Product Code"
                Case Is = 359
                    Return code & " - " & "Error# : Invalid Balance Type"
                Case Is = 360
                    Return code & " - " & "Error# : Another account has this Old Account Number"
                Case Is = 361
                    Return code & " - " & "Message#: Process Request?"
                Case Is = 362
                    Return code & " - " & "Error# : Image not Found or Corrupted"
                Case Is = 363
                    Return code & " - " & "Error# : Account not Found"
                Case Is = 364
                    Return code & " - " & "Error# : You did not post this Transaction Sequence"
                Case Is = 365
                    Return code & " - " & "Error# : You are not permitted to print account statements"
                Case Is = 366
                    Return code & " - " & "Error# : Repayments cannot be earlier than Execution Start Date"
                Case Is = 367
                    Return code & " - " & "Error# : You must enter a Backup Name"
                Case Is = 368
                    Return code & " - " & "Error# : Invalid Charge Rate"
                Case Is = 369
                    Return code & " - " & "Error# : Previous Journal Number is required"
                Case Is = 370
                    Return code & " - " & "Error# : Account already closed"
                Case Is = 371
                    Return code & " - " & "Error# : Account cannot be closed because it has uncleared cheques"
                Case Is = 372
                    Return code & " - " & "Error# : Account cannot be closed because it has Standing Order"
                Case Is = 373
                    Return code & " - " & "Error# : Account cannot be closed because it has Loan Repayment Schedule"
                Case Is = 374
                    Return code & " - " & "Error# : Account cannot be closed because it has uncommitted transactions"
                Case Is = 375
                    Return code & " - " & "Error# : Insufficient Vault Cash"
                Case Is = 376
                    Return code & " - " & "Error# : Max Cheque Leaflets cannot be exceeded"
                Case Is = 377
                    Return code & " - " & "Error# : COT Rate margin must be expressed as fraction preceeded by sign e.g -0.002, 0.003"
                Case Is = 378
                    Return code & " - " & "Error# : Scheduled Loans exist without Schedules"
                Case Is = 379
                    Return code & " - " & "Error# : Standing Order not allowed for Scheduled Loans"
                Case Is = 380
                    Return code & " - " & "Error# : Tenor Type must be D - Days, W - Weeks, M - months, Q - Quarters, Y - Years"
                Case Is = 381
                    Return code & " - " & "Error# : Account Officer is required"
                Case Is = 382
                    Return code & " - " & "Error# : Account Marketer is required"
                Case Is = 383
                    Return code & " - " & "Error# : Only Head Office Bank Accounts are allowed"
                Case Is = 384
                    Return code & " - " & "Error# : Bank Accounts not allowed as contra"
                Case Is = 385
                    Return code & " - " & "Error# : Select the correct Draft Type"
                Case Is = 386
                    Return code & " - " & "Error# : Third Party rate cannot be more than full interest rate"
                Case Is = 387

                    Return code & " - " & "Error# : Deal or Loan account must have 0 balance before closing"
                Case Is = 388
                    Return code & " - " & "Message#: Journal Accounts Limit Violation.  Do you want to View Journal Check Log?"
                Case Is = 389
                    Return code & " - " & "Error# : Manual Generation of Account numbers not allowed"
                Case Is = 390
                    Return code & " - " & "Error# : Can't commit this Journal because of Insufficient account balances. View Journal log"
                Case Is = 391
                    Return code & " - " & "Error# : Loan Batch or Group does not exist"
                Case Is = 392
                    Return code & " - " & "Error# : Entered Value will throw the amount/rate to negative"
                Case Is = 393

                    Return code & " - " & "Error# : Account has no Existing Overdraft for Maintenance"
                Case Is = 394
                    Return code & " - " & "Error# : Account already has an Overdraft, Use the Overdraft Maintenance screen"
                Case Is = 395
                    Return code & " - " & "Error# : Please, Enter Supplier Names"
                Case Is = 396
                    Return code & " - " & "Error# : Asset Voucher No is required"
                Case Is = 397
                    Return code & " - " & "Error# : Purchase Date cannot be later than current Posting Date"
                Case Is = 398
                    Return code & " - " & "Error# : Depreciation Start Date cannot be earlier than Purchase Date"
                Case Is = 399
                    Return code & " - " & "Error# : Asset Description required"
                Case Is = 400
                    Return code & " - " & "Error# : Invalid Quantity or  Unit Cost or Useful Life or Nominal Value "
                Case Is = 401
                    Return code & " - " & "Error# : Voucher No. exist under an existing Fixed Asset"
                Case Is = 402
                    Return code & " - " & "Error# : Invalid Fixed Asset ID No."
                Case Is = 403
                    Return code & " - " & "Error# : New Password must be Alphanumeric"
                Case Is = 404
                    Return code & " - " & "Error# : Invalid Economic Sector"
                Case Is = 405
                    Return code & " - " & "Error# : Request failed due to Insufficient Balance"
                Case Is = 406
                    Return code & " - " & "Error# : You cannot reverse transactions with Closed Account"
                Case Is = 407
                    Return code & " - " & "Error# : Share investment date cannot be later than first transaction or posting date"
                Case Is = 408
                    Return code & " - " & "Error# : Shareholder has historical transaction"
                Case Is = 409
                    Return code & " - " & "Error# : Shareholder has pending transaction"
                Case Is = 410
                    Return code & " - " & "Error# : Shareholder does not exist"
                Case Is = 411
                    Return code & " - " & "Error# : Invalid Number of Shares"
                Case Is = 412
                    Return code & " - " & "Error# : Entered Date cannot be earlier than original shareholder's Investment Date"
                Case Is = 413
                    Return code & " - " & "Error# : Cut-Off Date cannot be later than Current Posting Date"
                Case Is = 414
                    Return code & " - " & "Error# : Invalid Earning per Share"
                Case Is = 415

                    Return code & " - " & "Error# : Invalid WHT Rate"
                Case Is = 416
                    Return code & " - " & "Error# : Fixed Repayment Amount cannot be greater than Loan Amount"
                Case Is = 417
                    Return code & " - " & "Error# : Repayment Date must fall between Effective andf Maturity Dates"
                Case Is = 418
                    Return code & " - " & "Error# : Property Address/Asset Description is required in this context"
                Case Is = 419
                    Return code & " - " & "Error# : You cannot maintain your User Profile"
                Case Is = 420
                    Return code & " - " & "Error# : Bank Accounts not allowed in Journals"
                Case Is = 421
                    Return code & " - " & "Please change your password. Number of days left before its expiry is"
                Case Is = 422
                    Return code & " - " & "Error# : Some repayments will exceed maturity date. Reduce number of repayments."
                Case Is = 423
                    Return code & " - " & "Error# : Recurring Fee repayment cannot be earlier than Loan Effective Date"
                Case Is = 424
                    Return code & " - " & "Error# : Recurring Fee Description is required"
                Case Is = 425
                    Return code & " - " & "Error# : Cost Centre/Unit Required"
                Case Is = 426
                    Return code & " - " & "Error# : Fees Repayment must not be earlier than Schedule Start Date"
                Case Is = 427
                    Return code & " - " & "Error# : Can't Rollback Status because transaction is in a Valid State"
                Case Is = 428
                    Return code & " - " & "Error# : Reduction amount cannot be more than asset Net Book Value"
                Case Is = 429
                    Return code & " - " & "Error# : Invalid COT or Interest Rate"
                Case Is = 430
                    Return code & " - " & "Error# : Amount Drawn cannot be more than Amount Approved"
                Case Is = 431

                    Return code & " - " & "Error# : Equity Contribution cannot be more than Amount Approved"
                Case Is = 432
                    Return code & " - " & "Error# : Generate/Regenerate Loan Repayment Schedule"
                Case Is = 433
                    Return code & " - " & "Error# : Loan and Schedule Amount Mismatch. Regenerate Schedule"
                Case Is = 434
                    Return code & " - " & "Error# :  Schedule Repayment Date must start after Loan Effective Date"
                Case Is = 435
                    Return code & " - " & "Error# :  Amount Approved and Loan Fees Amount Mismatch. Delete and recreate fees"
                Case Is = 436

                    Return code & " - " & "Error# :  Amount must be greater or equal Accrued Interest"
                Case Is = 437
                    Return code & " - " & "Error# : Trans/Value Date cannot be earlier than Last Entry Date"
                Case Is = 438

                    Return code & " - " & "Error# : Maturity/Expiry Date must be in the future"
                Case Is = 439
                    Return code & " - " & "Error# : Effective Date of Change must be between Last Entry and Current"
                Case Is = 440
                    Return code & " - " & "Error# : Overdrafts only allowed on Current Accounts"
                Case Is = 441
                    Return code & " - " & "Error# : Loan maturity cannot be earlier than last repayment"
                Case Is = 442
                    Return code & " - " & "Error# : Interest Grace date must fall between Value and Maturity Dates"
                Case Is = 443
                    Return code & " - " & "Error# : Repayments can only start after Interest Grace Period"
                Case Is = 444
                    Return code & " - " & "Error# : Operation not allowed for Accounts with PDOs"
                Case Is = 445
                    Return code & " - " & "Error# : Account is used as Operative Account on Loan/Deal"
                Case Is = 446
                    Return code & " - " & "Error# : Invalid Fee Type"
                Case Is = 447
                    Return code & " - " & "Error# : Selected Rate Maintenance invalid for this Account"
                Case Is = 448
                    Return code & " - " & "Error# : Effective Date cannot be in the future"
                Case Is = 449
                    Return code & " - " & "Error# : Rate change cannot be on terminated Deals/Loans"
                Case Is = 450
                    Return code & " - " & "Error# : Effective Date cannot be after Maturity"
                Case Is = 451
                    Return code & " - " & "Error# : Effective Date cannot be earlier than Earliest Possible Date"
                Case Is = 1000
                    Return code & " - " & "Select the User or Role to send message to"
                Case Is = 1001
                    Return code & " - " & "Please, enter a Subject for the message"
                Case Is = 1002
                    Return code & " - " & "Please, enter the details of the message"
                Case Is = 1004
                    Return code & " - " & "Please, enter a reason"
                Case Is = 1005
                    Return code & " - " & "Enter bearer's name"
                Case Is = 5001
                    Return code & " - " & "Transaction on a Dormant or Classified Account"
                Case Is = 5002
                    Return code & " - " & "Withdrawal on an Account with Existing Overdraft Line"
                Case Is = 5003
                    Return code & " - " & "Transaction of this nature requires approval always"
                Case Is = 5004
                    Return code & " - " & "User's Posting Limit Exceeded"
                Case Is = 5005
                    Return code & " - " & "Request for TOD or Forced Debit"
                Case Is = 9000
                    Return code & " - " & "Message#: Have you taken a Backup?"
                Case Is = 9999
                    Return code & " - " & "Error# : Permission not Granted"











            End Select





        End Function





        Public Shared Function ErrorCode(ByVal code As Integer) As String
            Dim result As String

            result = GetField("SELECT error_desc FROM ErrorCodes WHERE error_code = " & code & "")

            If ErrorCodeType(code) = "1" Then
                Return "Error# " & CStr(code) & Chr(13) & result
            ElseIf ErrorCodeType(code) = "2" Then
                Return result
            Else
                Return "Message# " & CStr(code) & " " & Chr(13) & result
            End If



        End Function






        Public Shared Function ErrorCodeType(ByVal code As Integer) As String
            Dim result As String

            result = GetField("SELECT type FROM ErrorCodes WHERE error_code = " & code & "")
            Return result
        End Function

        Public Shared Function IsNewMessage(ByVal username As String) As Boolean
            Dim result As String

            result = GetField("SELECT subject FROM Messages WHERE user_to = '" & username & "' AND opened = 'No'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function RejectionExist(ByVal refno As String) As Boolean
            Dim result As String

            result = GetField("SELECT ref_no FROM AuditLog WHERE ref_no = '" & refno & "'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function GetMobileNo() As String
            Return GetField("SELECT mobile_no FROM ParamPreferences WHERE rec_id = '1'")
        End Function

        Public Shared Function GetParamAccount(ByVal headsubcode As String, ByVal id As String) As String
            Dim Separator As String, head As String, suborg As String, acct As String

            Separator = GetField("SELECT separator FROM ParamPreferences WHERE rec_id = '1'")
            head = GetField("SELECT head_code FROM HeadsAndSubs WHERE headsub_code = '" & headsubcode & "'")
            suborg = GetField("SELECT sub_org FROM HeadsAndSubs WHERE headsub_code = '" & headsubcode & "'")
            acct = GetField("SELECT mini_account_no FROM ParamAccounts WHERE id = '" & id & "'")

            Return head & Separator & suborg & Separator & acct
        End Function

        Public Shared Function ConvertDate(ByVal reportdate As Date) As String
            Dim varyear As String, varmonth As String, varday As String
            varyear = CStr(reportdate.Year)
            varmonth = CStr(reportdate.Month)
            varday = CStr(reportdate.Day)

            ConvertDate = "Date('" & varyear & "," & varmonth & "," & varday & "')"
        End Function

        Public Shared Function ConvertAmount(ByVal fcyamnt As Decimal, ByVal rate As Decimal, ByVal curr As String) As Decimal
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = DbSqlConnectionString()

            Cmd.CommandText = "Sproc_ConvertAmount"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@Amount", fcyamnt))
                Cmd.Parameters.Add(New SqlParameter("@CurrCode", curr))
                Cmd.Parameters.Add(New SqlParameter("@ExchRate", rate))

                result = Cmd.Parameters.Add(New SqlParameter("@ConvertedAmount", SqlDbType.VarChar, 30))
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
        Public Shared Function ConvertBackAmount(ByVal lcyamnt As Decimal, ByVal rate As Decimal, ByVal curr As String) As Decimal
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = DbSqlConnectionString()

            Cmd.CommandText = "Sproc_ConvertBackAmount"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@Amount", lcyamnt))
                Cmd.Parameters.Add(New SqlParameter("@CurrCode", curr))
                Cmd.Parameters.Add(New SqlParameter("@ExchRate", rate))

                result = Cmd.Parameters.Add(New SqlParameter("@ConvertedAmount", SqlDbType.VarChar, 30))
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
        Public Shared Function GetPreviousRejections(ByVal refno As String) As String
            Dim Cmd As New SqlCommand, OleConn As New SqlConnection, result As SqlParameter

            OleConn.ConnectionString = DbSqlConnectionString()

            Cmd.CommandText = "Sproc_GetPreviousRejections"
            Cmd.Connection = OleConn
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                OleConn.Open()
                Cmd.Parameters.Add(New SqlParameter("@RefNo", refno))
                result = Cmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.VarChar, 500))
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

        Public Shared Function SumDatasetColumn(ByVal ds As DataSet, ByVal idx As Integer) As Decimal
            On Error Resume Next

            Dim Ctr As Integer, itr As Integer, totalsum As Decimal, row As DataRow

            If ds.Tables(0).Rows.Count <= 0 Or ds.Tables(0).Columns.Count < idx Then
                Return 0
                Exit Function
            End If

            Ctr = ds.Tables(0).Rows.Count

            For itr = 0 To (Ctr - 1)
                row = ds.Tables(0).Rows(itr)
                totalsum = totalsum + row(idx)
            Next

            Return totalsum
        End Function

        Public Shared Function IsLeapYear(ByVal cYear As Integer) As Boolean
            If DateDiff(DateInterval.Day, DateSerial(cYear, 1, 1), DateSerial(cYear + 1, 1, 1)) = 366 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function GetDaysInYear(ByVal cYear As Integer) As Integer
            Return DateDiff(DateInterval.Day, DateSerial(cYear, 1, 1), DateSerial(cYear + 1, 1, 1))
        End Function

        Public Shared Function IsJournalPosted(ByVal refno As String) As Boolean
            Dim result As String

            result = GetField("SELECT ref_no FROM Journals WHERE ref_no = '" & refno & "' AND status = 'P'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function OrgLevelExist(ByVal orgcode As String) As Boolean
            Dim result As String
            result = GetField("SELECT org_desc FROM Organs WHERE org_code = '" & orgcode & "'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function OrgExist(ByVal segment As String, ByVal orgcode As String) As Boolean
            Dim result As String
            result = GetField("SELECT org_desc FROM Organs WHERE segment_code = '" & segment & "' AND org_code = '" & orgcode & "'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function RecordsWithParentCodeExist(ByVal orgcode As String) As Boolean
            Dim result As String
            result = GetField("SELECT org_desc FROM Organs WHERE parent_org_code = '" & orgcode & "'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function FsRecordsWithParentCodeExist(ByVal itemcode As String) As Boolean
            Dim result As String
            result = GetField("SELECT segment_code FROM FsItems WHERE fs_parent_code = '" & itemcode & "'")

            If Len(result) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function GetOrgType(ByVal orgcode As String) As Boolean
            Return GetField("SELECT org_type FROM Organs WHERE org_code = '" & orgcode & "'")
        End Function

        Public Shared Function GetPath(ByVal pathid As Integer) As String
            Return GetField("SELECT path_string FROM Paths WHERE path_item = " & pathid & "")
        End Function

        Public Shared Function GetUserPassword(ByVal userid As String) As String
            Return GetField("SELECT password FROM AppUsers WHERE user_id = '" & userid & "'")
        End Function

        Public Shared Function GetOrgDesc(ByVal orgcode As String) As String
            Return GetField("SELECT org_desc FROM Organs WHERE org_code = '" & orgcode & "'")
        End Function

        Public Shared Function GetOrgLevel(ByVal orgcode As String) As Integer
            Return GetField("SELECT org_level_code FROM Organs WHERE org_code = '" & orgcode & "'")
        End Function

        Public Shared Function GetMonthVal(ByVal monthchar As String) As Integer
            If UCase(monthchar) = "JANURY" Then
                GetMonthVal = 1
            ElseIf UCase(monthchar) = "FEBRUARY" Then
                GetMonthVal = 2
            ElseIf UCase(monthchar) = "MARCH" Then
                GetMonthVal = 3
            ElseIf UCase(monthchar) = "APRIL" Then
                GetMonthVal = 4
            ElseIf UCase(monthchar) = "MAY" Then
                GetMonthVal = 5
            ElseIf UCase(monthchar) = "JUNE" Then
                GetMonthVal = 6
            ElseIf UCase(monthchar) = "JULY" Then
                GetMonthVal = 7
            ElseIf UCase(monthchar) = "AUGUST" Then
                GetMonthVal = 8
            ElseIf UCase(monthchar) = "SEPTEMBER" Then
                GetMonthVal = 9
            ElseIf UCase(monthchar) = "OCTOBER" Then
                GetMonthVal = 10
            ElseIf UCase(monthchar) = "NOVEMBER" Then
                GetMonthVal = 11
            ElseIf UCase(monthchar) = "DECEMBER" Then
                GetMonthVal = 12
            End If
        End Function

        Public Shared Function IsDetailOrg(ByVal orgcode As String) As Boolean
            Return GetField("SELECT org_type FROM Organs WHERE org_code = '" & orgcode & "'")
        End Function

        Public Shared Function IsAlphanumeric(ByVal password As String) As Boolean
            On Error Resume Next

            Dim digitfound As Boolean, charfound As Boolean, StrLen As Integer, Ctr As Integer
            Dim currchar As String

            digitfound = False
            charfound = False

            StrLen = Len(password)

            For Ctr = 1 To StrLen
                currchar = Mid$(password, Ctr, 1)

                If currchar = "0" Or currchar = "1" Or currchar = "2" Or currchar = "3" Or currchar = "4" Or currchar = "5" _
                Or currchar = "6" Or currchar = "7" Or currchar = "8" Or currchar = "9" Then
                    digitfound = True
                Else
                    charfound = True
                End If
            Next Ctr

            If digitfound = True And charfound = True Then
                Return True
            Else
                Return False
            End If
        End Function


        Public Shared Function MyDateAdd(ByVal NoInterVals As Integer, ByVal DateToAdd As DateTime, ByVal Freq As String) As DateTime


            If Freq = "Year" Then
                Return DateAdd(DateInterval.Year, NoInterVals, DateToAdd)

            End If
            If Freq = "Month" Then
                Return DateAdd(DateInterval.Month, NoInterVals, DateToAdd)
            End If
            If Freq = "Day" Then
                Return DateAdd(DateInterval.Day, NoInterVals, DateToAdd)
            End If
            If Freq = "Weekday" Then
                Return DateAdd(DateInterval.Weekday, NoInterVals, DateToAdd)
            End If
            If Freq = "Hour" Then
                Return DateAdd(DateInterval.Hour, NoInterVals, DateToAdd)
            End If
            If Freq = "Quarter" Then
                Return DateAdd(DateInterval.Quarter, NoInterVals, DateToAdd)
            End If

        End Function

        Public Shared Function MyFormat(ByVal MyDateFormat As String) As String

            Return (Format(CDate(MyDateFormat), "dd-MMM-yyyy"))
        End Function
    End Class

End Namespace
