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
Imports System.Security.Cryptography
Imports System.Web.Script.Serialization
Imports System.Xml
Imports Newtonsoft.Json

Namespace iBankPoints
    Public Class iPoints


        Public Shared Function ProcessAphaBetaPayRefReceipts(Token As String, PaymentRef As String) As String

            '  #region Details Alpha Beta 

            'Dim strHostUrl As String = System.Configuration.ConfigurationManager.ConnectionStrings("iMPhaseHostUrl").ToString()
            ' //http://xxsg.ebs-rcm.com/interface/revcodebyAgencyRef"
            Dim strAgentUrl As String = String.Format("{0}", "http://xxsg.ebs-rcm.com/Interface/ReceiptByPaymentRef")

            Dim Clientid As String = "715439421649801"
            Dim Key As String = "PH5UVZG993ND6BMADS65"
            Dim State As String = "XXSG"

            Dim UE As System.Text.UnicodeEncoding = New System.Text.UnicodeEncoding()
            Dim message As Byte() = UE.GetBytes(Key)
            Dim hashString As SHA512Managed = New SHA512Managed()

            Dim hash As String = ""
            '//Key + Pid + State
            Dim strhashSum As String = String.Format("{0}{1}{2}", Key, PaymentRef, State)

            Using sha512Hash As SHA512 = SHA512.Create()
                Dim sourceBytes As Byte() = System.Text.Encoding.UTF8.GetBytes(strhashSum)
                Dim hashBytes As Byte() = sha512Hash.ComputeHash(sourceBytes)
                hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty)

            End Using

            '#Region Add Auth Info
            Dim httpWebRequest As HttpWebRequest = HttpWebRequest.Create(strAgentUrl)

            httpWebRequest.ContentType = "application/json"

            httpWebRequest.Method = "POST"
            Using streamWriter = New StreamWriter(httpWebRequest.GetRequestStream())
                Dim ljson As Object = New With
                    {
                 .PaymentRef = PaymentRef,
                   .State = State,
                   .hash = hash,
                  .Clientid = Clientid
                }
                Dim serializer As New JavaScriptSerializer()
                Dim serializedResult = serializer.Serialize(ljson)
                streamWriter.Write(serializedResult)
                streamWriter.Flush()
                streamWriter.Close()
            End Using

            Dim HttpResponse As HttpWebResponse = CType(httpWebRequest.GetResponse(), HttpWebResponse)

            Dim Result = ""


            Using streamReader = New StreamReader(HttpResponse.GetResponseStream())
                Result = streamReader.ReadToEnd()
            End Using

            If (Result.Contains("PId not found") = False) Then

                Dim x = Newtonsoft.Json.Linq.JObject.Parse(Result)
                Return Result
            Else
                Return ""

            End If

        End Function


        Public Shared Function ProcessAphaBetaValWebGuid(Token As String, webguid As String, Type As String) As String


            '  #region Details Alpha Beta 

            'Dim strHostUrl As String = System.Configuration.ConfigurationManager.ConnectionStrings("iMPhaseHostUrl").ToString()
            ' //http://xxsg.ebs-rcm.com/interface/revcodebyAgencyRef"
            Dim strAgentUrl As String = String.Format("{0}", "http://xxsg.ebs-rcm.com/Interface/Validate")

            Dim Clientid As String = "715439421649801"
            Dim Key As String = "PH5UVZG993ND6BMADS65"
            Dim State As String = "XXSG"

            Dim UE As System.Text.UnicodeEncoding = New System.Text.UnicodeEncoding()
            Dim message As Byte() = UE.GetBytes(Key)
            Dim hashString As SHA512Managed = New SHA512Managed()

            Dim hash As String = ""
            '//Key + Pid + State
            Dim strhashSum As String = String.Format("{0}{2}{1}", Key, State, webguid)

            Using sha512Hash As SHA512 = SHA512.Create()
                Dim sourceBytes As Byte() = System.Text.Encoding.UTF8.GetBytes(strhashSum)
                Dim hashBytes As Byte() = sha512Hash.ComputeHash(sourceBytes)
                hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty)

            End Using

            '#Region Add Auth Info
            Dim httpWebRequest As HttpWebRequest = HttpWebRequest.Create(strAgentUrl)

            httpWebRequest.ContentType = "application/json"

            httpWebRequest.Method = "POST"
            Using streamWriter = New StreamWriter(httpWebRequest.GetRequestStream())
                Dim ljson As Object = New With
                    {
                  .webguid = webguid,
                    .Type = Type,
                   .State = State,
                    .Hash = hash,
                    .Clientid = Clientid
                }
                Dim serializer As New JavaScriptSerializer()
                Dim serializedResult = serializer.Serialize(ljson)
                streamWriter.Write(serializedResult)
                streamWriter.Flush()
                streamWriter.Close()
            End Using

            Dim HttpResponse As HttpWebResponse = CType(httpWebRequest.GetResponse(), HttpWebResponse)

            Dim Result = ""


            Using streamReader = New StreamReader(HttpResponse.GetResponseStream())
                Result = streamReader.ReadToEnd()
            End Using


            ' #endregion

            'Dim jss As JavaScriptSerializer = New JavaScriptSerializer()
            ''Dim Jsonresult As New Dictionary(Of String, String) = jss.DeserializeObject(results)}
            'Dim Jsonresult = jss.DeserializeObject(Result)


            If (Result.Contains("PId not found") = False) Then

                Dim x = Newtonsoft.Json.Linq.JObject.Parse(Result)
                Return Result
            Else
                Return ""

            End If

        End Function

        Public Shared Function ProcessAphaBetaValWebGuidDs(Token As String, webguid As String, type As String) As DataTable

            '  #region Details Alpha Beta 

            'Dim strHostUrl As String = System.Configuration.ConfigurationManager.ConnectionStrings("iMPhaseHostUrl").ToString()
            ' //http://xxsg.ebs-rcm.com/interface/revcodebyAgencyRef"
            Dim strAgentUrl As String = String.Format("{0}", "http://xxsg.ebs-rcm.com/Interface/Validate")

            Dim Clientid As String = "715439421649801"
            Dim Key As String = "PH5UVZG993ND6BMADS65"
            Dim State As String = "XXSG"

            Dim UE As System.Text.UnicodeEncoding = New System.Text.UnicodeEncoding()
            Dim message As Byte() = UE.GetBytes(Key)
            Dim hashString As SHA512Managed = New SHA512Managed()

            Dim hash As String = ""
            '//Key + Pid + State
            Dim strhashSum As String = String.Format("{0}{2}{1}", Key, State, webguid)

            Using sha512Hash As SHA512 = SHA512.Create()
                Dim sourceBytes As Byte() = System.Text.Encoding.UTF8.GetBytes(strhashSum)
                Dim hashBytes As Byte() = sha512Hash.ComputeHash(sourceBytes)
                hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty)

            End Using

            '#Region Add Auth Info
            Dim httpWebRequest As HttpWebRequest = HttpWebRequest.Create(strAgentUrl)

            httpWebRequest.ContentType = "application/json"

            httpWebRequest.Method = "POST"
            Using streamWriter = New StreamWriter(httpWebRequest.GetRequestStream())
                Dim ljson As Object = New With
                    {
                  .webguid = webguid,
                    .Type = type,
                   .State = State,
                    .Hash = hash,
                    .Clientid = Clientid
                }
                Dim serializer As New JavaScriptSerializer()
                Dim serializedResult = serializer.Serialize(ljson)
                streamWriter.Write(serializedResult)
                streamWriter.Flush()
                streamWriter.Close()
            End Using

            Dim HttpResponse As HttpWebResponse = CType(httpWebRequest.GetResponse(), HttpWebResponse)

            Dim Result = ""


            Using streamReader = New StreamReader(HttpResponse.GetResponseStream())
                Result = streamReader.ReadToEnd()
            End Using


            ' #endregion

            If (Result.Contains("FAILED") = False) Then

                Dim x = Newtonsoft.Json.Linq.JObject.Parse(Result)
                Dim xd1 As XmlDocument = JsonConvert.DeserializeObject(Of XmlDocument)(Result) '(XmlDocument)JsonConvert.DeserializeXmlNode(Result, "root")
                Dim xmlReader = New XmlNodeReader(xd1)
                Dim ds As DataSet = New DataSet()
                ds.ReadXml(xmlReader)
                Dim View As DataView = New DataView(ds.Tables(0))
                Dim dtQueryTable As DataTable = View.ToTable(False, New String() {"Pid", "PayerName", "AgencyCode", "RevenueCode", "OraAgencyRev", "Currency", "AmountDue"})
                Return dtQueryTable
                'Return Result
            Else
                Dim dt = New DataTable()
                Return dt

            End If
        End Function


        Public Shared Function ProcessTempTrans(BenAcctNo As String, BenSortCode As String, Location As String, mobileno As String, AccountNo As String, AccountName As String,
             Amount As Decimal, Fee As Decimal, BenAcctDesc As String, Narration As String,
               BenSortInfo As String, UserName As String, PaymentID As String, TransType As String, Token As String) As String

            Dim strHostUrl As String = System.Configuration.ConfigurationManager.ConnectionStrings("iMPhaseHostUrl").ToString()



            Dim strAgentUrl As String = String.Format("{0}/{1}", strHostUrl, "TransDetailsHist.aspx")


            '#Region Add Auth Info
            Dim httpWebRequest As HttpWebRequest = HttpWebRequest.Create(strAgentUrl)

            httpWebRequest.ContentType = "application/json"

            httpWebRequest.Method = "POST"
            httpWebRequest.Headers.Add("Token", Token)


            '  #endregion

            Dim RequestTimeStamp As String = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm:ss+0000")
            Using streamWriter = New StreamWriter(httpWebRequest.GetRequestStream())

                Dim ljson As Object = New With
                    {
                    .BenAcctNo = BenAcctNo,
                    .BenSortCode = BenSortCode,
                    .Location = Location,
                    .mobileno = mobileno,
                    .AccountNo = AccountNo,
                    .AccountName = AccountName,
                    .Amount = Amount,
                    .Fee = Fee,
                    .BenAcctDesc = BenAcctDesc,
                    .Narration = Narration,
                    .BenSortInfo = BenSortInfo,
                    .UserName = UserName,
                    .PaymentID = PaymentID,
                    .TransType = TransType
                }

                Dim serializer As New JavaScriptSerializer()
                Dim serializedResult = serializer.Serialize(ljson)
                streamWriter.Write(serializedResult)
                streamWriter.Flush()
                streamWriter.Close()
            End Using

            Dim HttpResponse As HttpWebResponse = CType(httpWebRequest.GetResponse(), HttpWebResponse)

            Dim Result = ""


            Using streamReader = New StreamReader(HttpResponse.GetResponseStream())
                Result = streamReader.ReadToEnd()
            End Using


            ' #endregion


            Return Result


        End Function

        Public Shared Function ProcessLocalFTTeller(PIN As String, AccountNo As String,
         creditAccountNo As String, mobileNo As String, Location As String, amount As Decimal,
          Fee As Decimal, narration As String, UserName As String, creditBankcode As String, Token As String) As String

            Dim strHostUrl As String = System.Configuration.ConfigurationManager.ConnectionStrings("iMPhaseHostUrl").ToString()



            Dim strAgentUrl As String = String.Format("{0}/{1}", strHostUrl, "FTLocal.aspx")


            '#Region Add Auth Info
            Dim httpWebRequest As HttpWebRequest = HttpWebRequest.Create(strAgentUrl)

            httpWebRequest.ContentType = "application/json"

            httpWebRequest.Method = "POST"
            httpWebRequest.Headers.Add("Token", Token)


            '  #endregion

            Dim RequestTimeStamp As String = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm:ss+0000")
            Using streamWriter = New StreamWriter(httpWebRequest.GetRequestStream())

                Dim ljson As Object = New With
                    {
                    .PIN = PIN,
                     .AccountNo = AccountNo,
                     .creditAccountNo = creditAccountNo,
                     .mobileNo = mobileNo,
                     .Location = Location,
                     .amount = amount,
                     .Fee = Fee,
                     .narration = narration,
                     .UserName = UserName,
                     .creditBankcode = creditBankcode
                }

                Dim serializer As New JavaScriptSerializer()
                Dim serializedResult = serializer.Serialize(ljson)
                streamWriter.Write(serializedResult)
                streamWriter.Flush()
                streamWriter.Close()
            End Using

            Dim HttpResponse As HttpWebResponse = CType(httpWebRequest.GetResponse(), HttpWebResponse)

            Dim Result = ""


            Using streamReader = New StreamReader(HttpResponse.GetResponseStream())
                Result = streamReader.ReadToEnd()
            End Using


            ' #endregion


            Return Result
        End Function


        Public Shared Function ProcessLocalFTValidate(PIN As String, AccountNo As String,
         creditAccountNo As String, mobileNo As String, Location As String, amount As Decimal,
          Fee As Decimal, narration As String, UserName As String, creditBankcode As String, Token As String) As String

            Dim strHostUrl As String = System.Configuration.ConfigurationManager.ConnectionStrings("iMPhaseHostUrl").ToString()



            Dim strAgentUrl As String = String.Format("{0}/{1}", strHostUrl, "FTLocalValidate.aspx")


            '#Region Add Auth Info
            Dim httpWebRequest As HttpWebRequest = HttpWebRequest.Create(strAgentUrl)

            httpWebRequest.ContentType = "application/json"

            httpWebRequest.Method = "POST"
            httpWebRequest.Headers.Add("Token", Token)


            '  #endregion

            Dim RequestTimeStamp As String = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm:ss+0000")
            Using streamWriter = New StreamWriter(httpWebRequest.GetRequestStream())

                Dim ljson As Object = New With
                    {
                    .PIN = PIN,
                     .AccountNo = AccountNo,
                     .creditAccountNo = creditAccountNo,
                     .mobileNo = mobileNo,
                     .Location = Location,
                     .amount = amount,
                     .Fee = Fee,
                     .narration = narration,
                     .UserName = UserName,
                     .creditBankcode = creditBankcode
                }

                Dim serializer As New JavaScriptSerializer()
                Dim serializedResult = serializer.Serialize(ljson)
                streamWriter.Write(serializedResult)
                streamWriter.Flush()
                streamWriter.Close()
            End Using

            Dim HttpResponse As HttpWebResponse = CType(httpWebRequest.GetResponse(), HttpWebResponse)

            Dim Result = ""


            Using streamReader = New StreamReader(HttpResponse.GetResponseStream())
                Result = streamReader.ReadToEnd()
            End Using


            ' #endregion


            Return Result
        End Function

        Public Shared Function ProcessAphaBetaValWebGuidRepDs(Token As String, webguid As String, type As String) As DataTable



            '  #region Details Alpha Beta 

            'Dim strHostUrl As String = System.Configuration.ConfigurationManager.ConnectionStrings("iMPhaseHostUrl").ToString()
            ' //http://xxsg.ebs-rcm.com/interface/revcodebyAgencyRef"
            Dim strAgentUrl As String = String.Format("{0}", "http://xxsg.ebs-rcm.com/Interface/Validate")

            Dim Clientid As String = "715439421649801"
            Dim Key As String = "PH5UVZG993ND6BMADS65"
            Dim State As String = "XXSG"

            Dim UE As System.Text.UnicodeEncoding = New System.Text.UnicodeEncoding()
            Dim message As Byte() = UE.GetBytes(Key)
            Dim hashString As SHA512Managed = New SHA512Managed()

            Dim hash As String = ""
            '//Key + Pid + State
            Dim strhashSum As String = String.Format("{0}{2}{1}", Key, State, webguid)

            Using sha512Hash As SHA512 = SHA512.Create()
                Dim sourceBytes As Byte() = System.Text.Encoding.UTF8.GetBytes(strhashSum)
                Dim hashBytes As Byte() = sha512Hash.ComputeHash(sourceBytes)
                hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty)

            End Using

            '#Region Add Auth Info
            Dim httpWebRequest As HttpWebRequest = HttpWebRequest.Create(strAgentUrl)

            httpWebRequest.ContentType = "application/json"

            httpWebRequest.Method = "POST"
            Using streamWriter = New StreamWriter(httpWebRequest.GetRequestStream())
                Dim ljson As Object = New With
                    {
                  .webguid = webguid,
                    .Type = type,
                   .State = State,
                    .Hash = hash,
                    .Clientid = Clientid
                }
                Dim serializer As New JavaScriptSerializer()
                Dim serializedResult = serializer.Serialize(ljson)
                streamWriter.Write(serializedResult)
                streamWriter.Flush()
                streamWriter.Close()
            End Using

            Dim HttpResponse As HttpWebResponse = CType(httpWebRequest.GetResponse(), HttpWebResponse)

            Dim Result = ""


            Using streamReader = New StreamReader(HttpResponse.GetResponseStream())
                Result = streamReader.ReadToEnd()
            End Using


            ' #endregion

            If (Result.Contains("FAILED") = False) Then

                Dim x = Newtonsoft.Json.Linq.JObject.Parse(Result)
                Dim xd1 As XmlDocument = JsonConvert.DeserializeObject(Of XmlDocument)(Result) '(XmlDocument)JsonConvert.DeserializeXmlNode(Result, "root")
                Dim xmlReader = New XmlNodeReader(xd1)
                Dim ds As DataSet = New DataSet()
                ds.ReadXml(xmlReader)
                Dim View As DataView = New DataView(ds.Tables(0))
                Dim dtQueryTable As DataTable = View.ToTable(False, New String() {"Pid", "PayerName", "AgencyCode", "RevenueCode", "OraAgencyRev", "Currency", "AmountDue"})
                Return dtQueryTable
                'Return Result
            Else
                Dim dt = New DataTable()
                Return dt

            End If

        End Function

        Public Shared Function ProcessAphaBetaPaymentNotify(Token As String, webguid As String,
 tellerID As String, PaymentChannel As String, TellerName As String, BankNote As String, dates As String, amountpaid As Double, creditaccount As String, paymentref As String
            ) As String


            Dim OriginatingIP As String = GetIPAddress().Replace("::ffff:", "")



            '  #region Details Alpha Beta 

            'Dim strHostUrl As String = System.Configuration.ConfigurationManager.ConnectionStrings("iMPhaseHostUrl").ToString()
            ' //http://xxsg.ebs-rcm.com/interface/revcodebyAgencyRef"
            Dim strAgentUrl As String = String.Format("{0}", "http://xxsg.ebs-rcm.com/Interface/Payment")

            Dim Clientid As String = "715439421649801"
            Dim Key As String = "PH5UVZG993ND6BMADS65"
            Dim State As String = "XXSG"

            Dim UE As System.Text.UnicodeEncoding = New System.Text.UnicodeEncoding()
            Dim message As Byte() = UE.GetBytes(Key)
            Dim hashString As SHA512Managed = New SHA512Managed()

            Dim hash As String = ""
            '//Key + Pid + State
            Dim strhashSum As String = String.Format("{0}{1}{2}{3}", Key, webguid, State, amountpaid)

            Using sha512Hash As SHA512 = SHA512.Create()
                Dim sourceBytes As Byte() = System.Text.Encoding.UTF8.GetBytes(strhashSum)
                Dim hashBytes As Byte() = sha512Hash.ComputeHash(sourceBytes)
                hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty)

            End Using

            '#Region Add Auth Info
            Dim httpWebRequest As HttpWebRequest = HttpWebRequest.Create(strAgentUrl)

            httpWebRequest.ContentType = "application/json"

            httpWebRequest.Method = "POST"
            ' //httpWebRequest.Headers.Add("Token", Token)


            '  #endregion


            Using streamWriter = New StreamWriter(httpWebRequest.GetRequestStream())

                Dim ljson As Object = New With
                    {
                  .webguid = webguid,
                    .amountpaid = amountpaid,
                    .paymentref = paymentref,
                    .creditaccount = creditaccount,
                    .Date = DateTime.Today.Date.ToString("ddMMMyyyy"),
                    .PaymentChannel = PaymentChannel,
                    .TellerName = TellerName,
                    .BankNote = BankNote,
                    .tellerID = tellerID,
                    .State = State,
                    .hash = hash,
                    .Clientid = Clientid
                }

                Dim serializer As New JavaScriptSerializer()
                Dim serializedResult = serializer.Serialize(ljson)
                streamWriter.Write(serializedResult)
                streamWriter.Flush()
                streamWriter.Close()
            End Using

            Dim HttpResponse As HttpWebResponse = CType(httpWebRequest.GetResponse(), HttpWebResponse)

            Dim Result = ""


            Using streamReader = New StreamReader(HttpResponse.GetResponseStream())
                Result = streamReader.ReadToEnd()
            End Using


            ' #endregion

            'Dim jss As JavaScriptSerializer = New JavaScriptSerializer()
            ''Dim Jsonresult As New Dictionary(Of String, String) = jss.DeserializeObject(results)}
            'Dim Jsonresult = jss.DeserializeObject(Result)


            If (Result.Contains("PId not found") = False) Then

                Dim x = Newtonsoft.Json.Linq.JObject.Parse(Result)
                Return Result
            End If
            Return ""





        End Function
        Public Shared Function ProcessAphaBetaPayRefNos(Token As String, CustRefNo As String,
                                                          Amount As Double, RevCode As String, AgencyCode As String, AppliedDate As String,
                                                          AssessmentRef As String) As String



            Dim OriginatingIP As String = GetIPAddress().Replace("::ffff:", "")



            '  #region Details Alpha Beta 

            'Dim strHostUrl As String = System.Configuration.ConfigurationManager.ConnectionStrings("iMPhaseHostUrl").ToString()
            ' //http://xxsg.ebs-rcm.com/interface/revcodebyAgencyRef"
            Dim strAgentUrl As String = String.Format("{0}", "http://xxsg.ebs-rcm.com/Interface/WebGuid")

            Dim Clientid As String = "715439421649801"
            Dim Key As String = "PH5UVZG993ND6BMADS65"
            Dim State As String = "XXSG"

            Dim UE As System.Text.UnicodeEncoding = New System.Text.UnicodeEncoding()
            Dim message As Byte() = UE.GetBytes(Key)
            Dim hashString As SHA512Managed = New SHA512Managed()

            Dim hash As String = ""
            '//Key + Pid + State
            Dim strhashSum As String = String.Format("{0}{1}{2}{3}", Key, CustRefNo, State, Amount)

            Using sha512Hash As SHA512 = SHA512.Create()
                Dim sourceBytes As Byte() = System.Text.Encoding.UTF8.GetBytes(strhashSum)
                Dim hashBytes As Byte() = sha512Hash.ComputeHash(sourceBytes)
                hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty)

            End Using

            '#Region Add Auth Info
            Dim httpWebRequest As HttpWebRequest = HttpWebRequest.Create(strAgentUrl)

            httpWebRequest.ContentType = "application/json"

            httpWebRequest.Method = "POST"
            ' //httpWebRequest.Headers.Add("Token", Token)


            '  #endregion


            Using streamWriter = New StreamWriter(httpWebRequest.GetRequestStream())

                Dim ljson As Object = New With
                    {
                     .pid = CustRefNo,
                    .Amount = Amount,
                    .AgencyCode = AgencyCode,
                    .RevCode = RevCode,
                    .AppliedDate = AppliedDate,
                    .AssessmentRef = AssessmentRef,
                    .State = State,
                    .hash = hash,
                    .Clientid = Clientid
                }

                Dim serializer As New JavaScriptSerializer()
                Dim serializedResult = serializer.Serialize(ljson)
                streamWriter.Write(serializedResult)
                streamWriter.Flush()
                streamWriter.Close()
            End Using

            Dim HttpResponse As HttpWebResponse = CType(httpWebRequest.GetResponse(), HttpWebResponse)

            Dim Result = ""


            Using streamReader = New StreamReader(HttpResponse.GetResponseStream())
                Result = streamReader.ReadToEnd()
            End Using


            ' #endregion

            'Dim jss As JavaScriptSerializer = New JavaScriptSerializer()
            ''Dim Jsonresult As New Dictionary(Of String, String) = jss.DeserializeObject(results)}
            'Dim Jsonresult = jss.DeserializeObject(Result)


            If (Result.Contains("PId not found") = False) Then

                Dim x = Newtonsoft.Json.Linq.JObject.Parse(Result)
                Return Result
            End If
            Return ""


        End Function
        Public Shared Function GetIPAddress() As String
            Return ""
        End Function
        Public Shared Function ProcessAphaBetaCustRefNos(Token As String, CustRefNo As String) As String


            Dim OriginatingIP As String = GetIPAddress().Replace("::ffff:", "")



            '  #region Details Alpha Beta 

            'Dim strHostUrl As String = System.Configuration.ConfigurationManager.ConnectionStrings("iMPhaseHostUrl").ToString()
            ' //http://xxsg.ebs-rcm.com/interface/revcodebyAgencyRef"
            Dim strAgentUrl As String = String.Format("{0}", "http://xxsg.ebs-rcm.com/Interface/VerifyPid")

            Dim Clientid As String = "715439421649801"
            Dim Key As String = "PH5UVZG993ND6BMADS65"
            Dim State As String = "XXSG"

            Dim UE As System.Text.UnicodeEncoding = New System.Text.UnicodeEncoding()
            Dim message As Byte() = UE.GetBytes(Key)
            Dim hashString As SHA512Managed = New SHA512Managed()

            Dim hash As String = ""
            '//Key + Pid + State
            Dim strhashSum As String = String.Format("{0}{2}{1}", Key, State, CustRefNo)

            Using sha512Hash As SHA512 = SHA512.Create()
                Dim sourceBytes As Byte() = System.Text.Encoding.UTF8.GetBytes(strhashSum)
                Dim hashBytes As Byte() = sha512Hash.ComputeHash(sourceBytes)
                hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty)

            End Using

            '#Region Add Auth Info
            Dim httpWebRequest As HttpWebRequest = HttpWebRequest.Create(strAgentUrl)

            httpWebRequest.ContentType = "application/json"

            httpWebRequest.Method = "POST"
            ' //httpWebRequest.Headers.Add("Token", Token)


            '  #endregion


            Using streamWriter = New StreamWriter(httpWebRequest.GetRequestStream())

                Dim ljson As Object = New With
                    {
                    .pid = CustRefNo,
                    .State = State,
                     .Hash = hash,
                     .Clientid = Clientid
                }

                Dim serializer As New JavaScriptSerializer()
                Dim serializedResult = serializer.Serialize(ljson)
                streamWriter.Write(serializedResult)
                streamWriter.Flush()
                streamWriter.Close()
            End Using

            Dim HttpResponse As HttpWebResponse = CType(httpWebRequest.GetResponse(), HttpWebResponse)

            Dim Result = ""


            Using streamReader = New StreamReader(HttpResponse.GetResponseStream())
                Result = streamReader.ReadToEnd()
            End Using


            ' #endregion

            'Dim jss As JavaScriptSerializer = New JavaScriptSerializer()
            ''Dim Jsonresult As New Dictionary(Of String, String) = jss.DeserializeObject(results)}
            'Dim Jsonresult = jss.DeserializeObject(Result)


            If (Result.Contains("PId not found") = False) Then

                Dim x = Newtonsoft.Json.Linq.JObject.Parse(Result)
                Return Result
            End If
            Return ""







        End Function

        Public Shared Function ProcessAphaBetaCustRefNo(Token As String, CustRefNo As String) As DataTable
            Dim OriginatingIP As String = GetIPAddress().Replace("::ffff:", "")



            '  #region Details Alpha Beta 

            'Dim strHostUrl As String = System.Configuration.ConfigurationManager.ConnectionStrings("iMPhaseHostUrl").ToString()

            Dim strAgentUrl As String = String.Format("{0}", "http://xxsg.ebs-rcm.com/Interface/VerifyPid")

            Dim Clientid As String = "715439421649801"
            Dim Key As String = "PH5UVZG993ND6BMADS65"
            Dim State As String = "XXSG"

            Dim UE As System.Text.UnicodeEncoding = New System.Text.UnicodeEncoding()
            Dim message As Byte() = UE.GetBytes(Key)
            Dim hashString As SHA512Managed = New SHA512Managed()

            Dim hash As String = ""
            '//Key + Pid + State
            Dim strhashSum As String = String.Format("{0}{2}{1}", Key, State, CustRefNo)

            Using sha512Hash As SHA512 = SHA512.Create()
                Dim sourceBytes As Byte() = System.Text.Encoding.UTF8.GetBytes(strhashSum)
                Dim hashBytes As Byte() = sha512Hash.ComputeHash(sourceBytes)
                hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty)

            End Using

            '#Region Add Auth Info
            Dim httpWebRequest As HttpWebRequest = HttpWebRequest.Create(strAgentUrl)

            httpWebRequest.ContentType = "application/json"

            httpWebRequest.Method = "POST"
            '//httpWebRequest.Headers.Add("Token", Token)


            '  #endregion


            Using streamWriter = New StreamWriter(httpWebRequest.GetRequestStream())

                Dim ljson As Object = New With
                    {
                    .pid = CustRefNo,
                     .State = State,
                     .Hash = hash,
                     .Clientid = Clientid
                }

                ' // json = Convert.ToBase64String(Encoding.Default.GetBytes(json))
                Dim serializer As New JavaScriptSerializer()
                Dim serializedResult = serializer.Serialize(ljson)
                streamWriter.Write(serializedResult)
                streamWriter.Flush()
                streamWriter.Close()
            End Using

            Dim HttpResponse As HttpWebResponse = CType(httpWebRequest.GetResponse(), HttpWebResponse)

            Dim Result = ""


            Using streamReader = New StreamReader(HttpResponse.GetResponseStream())
                Result = streamReader.ReadToEnd()
            End Using


            ' #endregion

            'Dim jss As JavaScriptSerializer = New JavaScriptSerializer()
            ''Dim Jsonresult As New Dictionary(Of String, String) = jss.DeserializeObject(results)}
            'Dim Jsonresult = jss.DeserializeObject(Result)
            'Dim dt As DataTable = JsonConvert.DeserializeObject(Result, (TypeOf (DataTable)))



            If (Result.Contains("PId not found") = False) Then

                Dim dt As DataTable = JsonConvert.DeserializeObject(Of DataTable)(Result)
                Return dt

            Else

                Dim dt As DataTable = New DataTable()
                Return dt
            End If
        End Function

        Public Shared Function ProcessAphaBetaRevList(Token As String, AgencyRef As String) As DataTable
            Dim OriginatingIP As String = GetIPAddress().Replace("::ffff:", "")



            '  #region Details Alpha Beta 

            'Dim strHostUrl As String = System.Configuration.ConfigurationManager.ConnectionStrings("iMPhaseHostUrl").ToString()

            Dim strAgentUrl As String = String.Format("{0}", "http://xxsg.ebs-rcm.com/interface/revcodebyAgencyRef")

            Dim Clientid As String = "715439421649801"
            Dim Key As String = "PH5UVZG993ND6BMADS65"
            Dim State As String = "XXSG"

            Dim UE As System.Text.UnicodeEncoding = New System.Text.UnicodeEncoding()
            Dim message As Byte() = UE.GetBytes(Key)
            Dim hashString As SHA512Managed = New SHA512Managed()

            Dim hash As String = ""
            '//Key + Pid + State
            Dim strhashSum As String = String.Format("{0}{1}{2}", Key, State, AgencyRef)

            Using sha512Hash As SHA512 = SHA512.Create()
                Dim sourceBytes As Byte() = System.Text.Encoding.UTF8.GetBytes(strhashSum)
                Dim hashBytes As Byte() = sha512Hash.ComputeHash(sourceBytes)
                hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty)

            End Using

            '#Region Add Auth Info
            Dim httpWebRequest As HttpWebRequest = HttpWebRequest.Create(strAgentUrl)

            httpWebRequest.ContentType = "application/json"

            httpWebRequest.Method = "POST"
            '//httpWebRequest.Headers.Add("Token", Token)


            '  #endregion


            Using streamWriter = New StreamWriter(httpWebRequest.GetRequestStream())

                Dim ljson As Object = New With
                    {
                    .AgencyHead = AgencyRef,
                     .State = State,
                     .Hash = hash,
                     .Clientid = Clientid
                }

                ' // json = Convert.ToBase64String(Encoding.Default.GetBytes(json))
                Dim serializer As New JavaScriptSerializer()
                Dim serializedResult = serializer.Serialize(ljson)
                streamWriter.Write(serializedResult)
                streamWriter.Flush()
                streamWriter.Close()
            End Using

            Dim HttpResponse As HttpWebResponse = CType(httpWebRequest.GetResponse(), HttpWebResponse)

            Dim Result = ""


            Using streamReader = New StreamReader(HttpResponse.GetResponseStream())
                Result = streamReader.ReadToEnd()
            End Using


            ' #endregion

            'Dim jss As JavaScriptSerializer = New JavaScriptSerializer()
            ''Dim Jsonresult As New Dictionary(Of String, String) = jss.DeserializeObject(results)}
            'Dim Jsonresult = jss.DeserializeObject(Result)
            'Dim dt As DataTable = JsonConvert.DeserializeObject(Result, (TypeOf (DataTable)))



            If (Result.Contains("Authentication Failed") = False) Then

                Dim dt As DataTable = JsonConvert.DeserializeObject(Of DataTable)(Result)
                Return dt

            Else

                Dim dt As DataTable = New DataTable()
                Return dt
            End If

        End Function

  Public Shared Function GenerateUserModules(strUserName As String) As DataTable


            Dim OriginatingIP As String = GetIPAddress().Replace("::ffff:", "")



            '  #region Details Alpha Beta 

            Dim strHostUrl As String = System.Configuration.ConfigurationManager.ConnectionStrings("iMPhaseHostUrl").ToString()

            Dim strAgentUrl As String = String.Format("{0}/{1}", strHostUrl, "UserModulesMenu.aspx")





            '#Region Add Auth Info
            Dim httpWebRequest As HttpWebRequest = HttpWebRequest.Create(strAgentUrl)

            httpWebRequest.ContentType = "application/json"

            httpWebRequest.Method = "POST"
            'httpWebRequest.Headers.Add("Token", Token)


            '  #endregion

            '    ChannelCode = ChannelCode,
            ''            AccountNo = strAccountNo
            Using streamWriter = New StreamWriter(httpWebRequest.GetRequestStream())

                Dim ljson As Object = New With
                    {
                   .UserName = strUserName
                }
                Dim serializer As New JavaScriptSerializer()
                Dim serializedResult = serializer.Serialize(ljson)
                ' // json = Convert.ToBase64String(Encoding.Default.GetBytes(json))
                streamWriter.Write(serializedResult)
                streamWriter.Flush()
                streamWriter.Close()
            End Using

            Dim HttpResponse As HttpWebResponse = CType(httpWebRequest.GetResponse(), HttpWebResponse)

            Dim Result = ""


            Using streamReader = New StreamReader(HttpResponse.GetResponseStream())
                Result = streamReader.ReadToEnd()
            End Using


            ' #endregion

            'Dim jss As JavaScriptSerializer = New JavaScriptSerializer()
            'Dim Jsonresult As New Dictionary(Of String, String) = jss.DeserializeObject(results)}
            'Dim Jsonresult = jss.DeserializeObject(Result)
            'Dim dt As DataTable = JsonConvert.DeserializeObject(Result, (TypeOf (DataTable)))
            'Dim dataSet = JsonConvert.DeserializeObject(Of DataSet)(Result)
            'Dim dataSet = Jsonresult.(Result)("root").ToObject(Of DataTable)()
            'Dim dt As DataTable = dataSet.Tables(0)



            'Dim x = Newtonsoft.Json.Linq.JObject.Parse(Result)
            'Dim xd1 As XmlDocument = JsonConvert.DeserializeObject(Of XmlDocument)(Result) '(XmlDocument)JsonConvert.DeserializeXmlNode(Result, "root")
            'Dim xmlReader = New XmlNodeReader(xd1)
            'Dim ds As DataSet = New DataSet()
            'ds.ReadXml(xmlReader)
            'Dim View As DataView = New DataView(ds.Tables(0))
            'Dim dt As DataTable = View.ToTable()

            Dim dt As DataTable = JsonConvert.DeserializeObject(Of DataTable)(Result)
            Return dt


        End Function

        Public Shared Function GenerateUserMenus(strUserName As String, ObjModule As String) As DataTable


            Dim OriginatingIP As String = GetIPAddress().Replace("::ffff:", "")



            '  #region Details Alpha Beta 

            Dim strHostUrl As String = System.Configuration.ConfigurationManager.ConnectionStrings("iMPhaseHostUrl").ToString()

            Dim strAgentUrl As String = String.Format("{0}/{1}", strHostUrl, "UserModulesMenu.aspx")





            '#Region Add Auth Info
            Dim httpWebRequest As HttpWebRequest = HttpWebRequest.Create(strAgentUrl)

            httpWebRequest.ContentType = "application/json"

            httpWebRequest.Method = "POST"
            'httpWebRequest.Headers.Add("Token", Token)


            '  #endregion

            '    ChannelCode = ChannelCode,
            ''            AccountNo = strAccountNo
            Using streamWriter = New StreamWriter(httpWebRequest.GetRequestStream())

                Dim ljson As Object = New With
                    {
                   .UserName = strUserName,
                   .ObjModule = ObjModule
                }
                Dim serializer As New JavaScriptSerializer()
                Dim serializedResult = serializer.Serialize(ljson)
                ' // json = Convert.ToBase64String(Encoding.Default.GetBytes(json))
                streamWriter.Write(serializedResult)
                streamWriter.Flush()
                streamWriter.Close()
            End Using

            Dim HttpResponse As HttpWebResponse = CType(httpWebRequest.GetResponse(), HttpWebResponse)

            Dim Result = ""


            Using streamReader = New StreamReader(HttpResponse.GetResponseStream())
                Result = streamReader.ReadToEnd()
            End Using


            ' #endregion

            'Dim jss As JavaScriptSerializer = New JavaScriptSerializer()
            'Dim Jsonresult As New Dictionary(Of String, String) = jss.DeserializeObject(results)}
            'Dim Jsonresult = jss.DeserializeObject(Result)
            'Dim dt As DataTable = JsonConvert.DeserializeObject(Result, (TypeOf (DataTable)))
            'Dim dataSet = JsonConvert.DeserializeObject(Of DataSet)(Result)
            'Dim dataSet = Jsonresult.(Result)("root").ToObject(Of DataTable)()
            'Dim dt As DataTable = dataSet.Tables(0)



            'Dim x = Newtonsoft.Json.Linq.JObject.Parse(Result)
            'Dim xd1 As XmlDocument = JsonConvert.DeserializeObject(Of XmlDocument)(Result) '(XmlDocument)JsonConvert.DeserializeXmlNode(Result, "root")
            'Dim xmlReader = New XmlNodeReader(xd1)
            'Dim ds As DataSet = New DataSet()
            'ds.ReadXml(xmlReader)
            'Dim View As DataView = New DataView(ds.Tables(0))
            'Dim dt As DataTable = View.ToTable()

            Dim dt As DataTable = JsonConvert.DeserializeObject(Of DataTable)(Result)
            Return dt


        End Function

        Public Shared Function ProcessAphaBetaBillsType(Token As String) As DataTable

            Dim OriginatingIP As String = GetIPAddress().Replace("::ffff:", "")



            '  #region Details Alpha Beta 

            'Dim strHostUrl As String = System.Configuration.ConfigurationManager.ConnectionStrings("iMPhaseHostUrl").ToString()

            Dim strAgentUrl As String = String.Format("{0}", "http://xxsg.ebs-rcm.com/Interface/billtype")

            Dim Clientid As String = "715439421649801"
            Dim Key As String = "PH5UVZG993ND6BMADS65"
            Dim State As String = "XXSG"

            Dim UE As System.Text.UnicodeEncoding = New System.Text.UnicodeEncoding()
            Dim message As Byte() = UE.GetBytes(Key)
            Dim hashString As SHA512Managed = New SHA512Managed()

            Dim hash As String = ""
            '//Key + Pid + State
            Dim strhashSum As String = String.Format("{0}{1}", Key, State)

            Using sha512Hash As SHA512 = SHA512.Create()
                Dim sourceBytes As Byte() = System.Text.Encoding.UTF8.GetBytes(strhashSum)
                Dim hashBytes As Byte() = sha512Hash.ComputeHash(sourceBytes)
                hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty)

            End Using

            '#Region Add Auth Info
            Dim httpWebRequest As HttpWebRequest = HttpWebRequest.Create(strAgentUrl)

            httpWebRequest.ContentType = "application/json"

            httpWebRequest.Method = "POST"
            '//httpWebRequest.Headers.Add("Token", Token)


            '  #endregion


            Using streamWriter = New StreamWriter(httpWebRequest.GetRequestStream())

                Dim ljson As Object = New With
                    {
                     .State = State,
                     .Hash = hash,
                     .Clientid = Clientid
                }
                Dim serializer As New JavaScriptSerializer()
                Dim serializedResult = serializer.Serialize(ljson)
                'Dim strJson As String = JavaScriptSerializer().Serialize(New With
                '    {
                '     .State = State,
                '     .Hash = hash,
                '     .Clientid = Clientid
                '})
                ' // json = Convert.ToBase64String(Encoding.Default.GetBytes(json))
                streamWriter.Write(serializedResult)
                streamWriter.Flush()
                streamWriter.Close()
            End Using

            Dim HttpResponse As HttpWebResponse = CType(httpWebRequest.GetResponse(), HttpWebResponse)

            Dim Result = ""


            Using streamReader = New StreamReader(HttpResponse.GetResponseStream())
                Result = streamReader.ReadToEnd()
            End Using


            ' #endregion

            'Dim jss As JavaScriptSerializer = New JavaScriptSerializer()
            ''Dim Jsonresult As New Dictionary(Of String, String) = jss.DeserializeObject(results)}
            'Dim Jsonresult = jss.DeserializeObject(Result)
            'Dim dt As DataTable = JsonConvert.DeserializeObject(Result, (TypeOf (DataTable)))

            'Dim dt As DataTable = JsonConvert.DeserializeObject(Of DataTable)(Result)
            'Return dt
            If (Result.Contains("Failed") = False) Then

                Dim dt As DataTable = JsonConvert.DeserializeObject(Of DataTable)(Result)
                Return dt

            Else

                Dim dt As DataTable = New DataTable()
                Return dt
            End If
        End Function
        Public Shared Function ProcessAphaBetaAgencyds(Token As String) As DataTable


            Dim OriginatingIP As String = GetIPAddress().Replace("::ffff:", "")



            '  #region Details Alpha Beta 

            'Dim strHostUrl As String = System.Configuration.ConfigurationManager.ConnectionStrings("iMPhaseHostUrl").ToString()
            ' //http://xxsg.ebs-rcm.com/interface/revcodebyAgencyRef"
            Dim strAgentUrl As String = String.Format("{0}", "http://xxsg.ebs-rcm.com/Interface/AgencyCode")

            Dim Clientid As String = "715439421649801"
            Dim Key As String = "PH5UVZG993ND6BMADS65"
            Dim State As String = "XXSG"

            Dim UE As System.Text.UnicodeEncoding = New System.Text.UnicodeEncoding()
            Dim message As Byte() = UE.GetBytes(Key)
            Dim hashString As SHA512Managed = New SHA512Managed()

            Dim hash As String = ""
            '//Key + Pid + State
            Dim strhashSum As String = String.Format("{0}{1}", Key, State)

            Using sha512Hash As SHA512 = SHA512.Create()
                Dim sourceBytes As Byte() = System.Text.Encoding.UTF8.GetBytes(strhashSum)
                Dim hashBytes As Byte() = sha512Hash.ComputeHash(sourceBytes)
                hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty)

            End Using

            '#Region Add Auth Info
            Dim httpWebRequest As HttpWebRequest = HttpWebRequest.Create(strAgentUrl)

            httpWebRequest.ContentType = "application/json"

            httpWebRequest.Method = "POST"
            '//httpWebRequest.Headers.Add("Token", Token)


            '  #endregion


            Using streamWriter = New StreamWriter(httpWebRequest.GetRequestStream())

                Dim ljson As Object = New With
                    {
                     .State = State,
                     .Hash = hash,
                     .Clientid = Clientid
                }
                Dim serializer As New JavaScriptSerializer()
                Dim serializedResult = serializer.Serialize(ljson)
                ' // json = Convert.ToBase64String(Encoding.Default.GetBytes(json))
                streamWriter.Write(serializedResult)
                streamWriter.Flush()
                streamWriter.Close()
            End Using

            Dim HttpResponse As HttpWebResponse = CType(httpWebRequest.GetResponse(), HttpWebResponse)

            Dim Result = ""


            Using streamReader = New StreamReader(HttpResponse.GetResponseStream())
                Result = streamReader.ReadToEnd()
            End Using


            ' #endregion

            'Dim jss As JavaScriptSerializer = New JavaScriptSerializer()
            ''Dim Jsonresult As New Dictionary(Of String, String) = jss.DeserializeObject(results)}
            'Dim Jsonresult = jss.DeserializeObject(Result)
            'Dim dt As DataTable = JsonConvert.DeserializeObject(Result, (TypeOf (DataTable)))

            Dim dt As DataTable = JsonConvert.DeserializeObject(Of DataTable)(Result)
            Return dt

        End Function

        Public Shared Function ProcessFTChargesDs(Amount As Double, ChannelCode As String, Token As String) As DataTable


            Dim OriginatingIP As String = GetIPAddress().Replace("::ffff:", "")



            '  #region Details Alpha Beta 

            Dim strHostUrl As String = System.Configuration.ConfigurationManager.ConnectionStrings("iMPhaseHostUrl").ToString()
            ' //http://xxsg.ebs-rcm.com/interface/revcodebyAgencyRef"
            Dim strAgentUrl As String = String.Format("{0}/{1}", strHostUrl, "FTCharges.aspx")



            'Dim Clientid As String = "715439421649801"
            'Dim Key As String = "PH5UVZG993ND6BMADS65"
            'Dim State As String = "XXSG"

            'Dim UE As System.Text.UnicodeEncoding = New System.Text.UnicodeEncoding()
            'Dim message As Byte() = UE.GetBytes(Key)
            'Dim hashString As SHA512Managed = New SHA512Managed()

            'Dim hash As String = ""
            ''//Key + Pid + State
            'Dim strhashSum As String = String.Format("{0}{1}", Key, State)

            'Using sha512Hash As SHA512 = SHA512.Create()
            '    Dim sourceBytes As Byte() = System.Text.Encoding.UTF8.GetBytes(strhashSum)
            '    Dim hashBytes As Byte() = sha512Hash.ComputeHash(sourceBytes)
            '    hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty)

            'End Using

            '#Region Add Auth Info
            Dim httpWebRequest As HttpWebRequest = HttpWebRequest.Create(strAgentUrl)

            httpWebRequest.ContentType = "application/json"

            httpWebRequest.Method = "POST"
            '//httpWebRequest.Headers.Add("Token", Token)


            '  #endregion


            Using streamWriter = New StreamWriter(httpWebRequest.GetRequestStream())

                Dim ljson As Object = New With
                    {
                   .Amount = Amount,
                   .ChannelCode = ChannelCode
                }
                Dim serializer As New JavaScriptSerializer()
                Dim serializedResult = serializer.Serialize(ljson)
                ' // json = Convert.ToBase64String(Encoding.Default.GetBytes(json))
                streamWriter.Write(serializedResult)
                streamWriter.Flush()
                streamWriter.Close()
            End Using

            Dim HttpResponse As HttpWebResponse = CType(httpWebRequest.GetResponse(), HttpWebResponse)

            Dim Result = ""


            Using streamReader = New StreamReader(HttpResponse.GetResponseStream())
                Result = streamReader.ReadToEnd()
            End Using


            ' #endregion

            'Dim jss As JavaScriptSerializer = New JavaScriptSerializer()
            ''Dim Jsonresult As New Dictionary(Of String, String) = jss.DeserializeObject(results)}
            'Dim Jsonresult = jss.DeserializeObject(Result)
            'Dim dt As DataTable = JsonConvert.DeserializeObject(Result, (TypeOf (DataTable)))

            Dim dt As DataTable = JsonConvert.DeserializeObject(Of DataTable)(Result)
            Return dt

        End Function


        Public Shared Function ProcessProfileds(UserName As String, Token As String) As DataTable


            Dim OriginatingIP As String = GetIPAddress().Replace("::ffff:", "")



            '  #region Details Alpha Beta 

            Dim strHostUrl As String = System.Configuration.ConfigurationManager.ConnectionStrings("iMPhaseHostUrl").ToString()
            ' //http://xxsg.ebs-rcm.com/interface/revcodebyAgencyRef"
            Dim strAgentUrl As String = String.Format("{0}/{1}", strHostUrl, "UserProfile.aspx")





            '#Region Add Auth Info
            Dim httpWebRequest As HttpWebRequest = HttpWebRequest.Create(strAgentUrl)

            httpWebRequest.ContentType = "application/json"

            httpWebRequest.Method = "POST"
            'httpWebRequest.Headers.Add("Token", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkhwIiwibmJmIjoxNjQ0OTM5NTQ2LCJleHAiOjE2NDQ5NDY3NDYsImlhdCI6MTY0NDkzOTU0NiwiVXNlck5hbWUiOiJIcCJ9.SvRAqtrojJk_Lt1-G9knPvCwPwD2OFIy7iGMnXm8LpQ")
            httpWebRequest.Headers.Add("Token", Token)


            '  #endregion


            Using streamWriter = New StreamWriter(httpWebRequest.GetRequestStream())

                Dim ljson As Object = New With
                    {
                    .UserID = UserName
                    }
                Dim serializer As New JavaScriptSerializer()
                Dim serializedResult = serializer.Serialize(ljson)
                ' // json = Convert.ToBase64String(Encoding.Default.GetBytes(json))
                streamWriter.Write(serializedResult)
                streamWriter.Flush()
                streamWriter.Close()
            End Using

            Dim HttpResponse As HttpWebResponse = CType(httpWebRequest.GetResponse(), HttpWebResponse)

            Dim Result = ""


            Using streamReader = New StreamReader(HttpResponse.GetResponseStream())
                Result = streamReader.ReadToEnd()
            End Using


            ' #endregion

            'Dim jss As JavaScriptSerializer = New JavaScriptSerializer()
            ''Dim Jsonresult As New Dictionary(Of String, String) = jss.DeserializeObject(results)}
            'Dim Jsonresult = jss.DeserializeObject(Result)
            'Dim dt As DataTable = JsonConvert.DeserializeObject(Result, (TypeOf (DataTable)))

            Dim dt As DataTable = JsonConvert.DeserializeObject(Of DataTable)(Result)
            Return dt

        End Function

        Public Shared Function ProcessProfileAuthds(UserName As String, Token As String) As DataTable


            Dim OriginatingIP As String = GetIPAddress().Replace("::ffff:", "")



            '  #region Details Alpha Beta 

            Dim strHostUrl As String = System.Configuration.ConfigurationManager.ConnectionStrings("iMPhaseHostUrl").ToString()
            ' //http://xxsg.ebs-rcm.com/interface/revcodebyAgencyRef"
            Dim strAgentUrl As String = String.Format("{0}/{1}", strHostUrl, "UserProfile.aspx")





            '#Region Add Auth Info
            Dim httpWebRequest As HttpWebRequest = HttpWebRequest.Create(strAgentUrl)

            httpWebRequest.ContentType = "application/json"

            httpWebRequest.Method = "POST"
            'httpWebRequest.Headers.Add("Token", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkhwIiwibmJmIjoxNjQ0OTM5NTQ2LCJleHAiOjE2NDQ5NDY3NDYsImlhdCI6MTY0NDkzOTU0NiwiVXNlck5hbWUiOiJIcCJ9.SvRAqtrojJk_Lt1-G9knPvCwPwD2OFIy7iGMnXm8LpQ")
            httpWebRequest.Headers.Add("AuthToken", Token)


            '  #endregion


            Using streamWriter = New StreamWriter(httpWebRequest.GetRequestStream())

                Dim ljson As Object = New With
                    {
                    .UserID = UserName
                    }
                Dim serializer As New JavaScriptSerializer()
                Dim serializedResult = serializer.Serialize(ljson)
                ' // json = Convert.ToBase64String(Encoding.Default.GetBytes(json))
                streamWriter.Write(serializedResult)
                streamWriter.Flush()
                streamWriter.Close()
            End Using

            Dim HttpResponse As HttpWebResponse = CType(httpWebRequest.GetResponse(), HttpWebResponse)

            Dim Result = ""


            Using streamReader = New StreamReader(HttpResponse.GetResponseStream())
                Result = streamReader.ReadToEnd()
            End Using


            ' #endregion

            'Dim jss As JavaScriptSerializer = New JavaScriptSerializer()
            ''Dim Jsonresult As New Dictionary(Of String, String) = jss.DeserializeObject(results)}
            'Dim Jsonresult = jss.DeserializeObject(Result)
            'Dim dt As DataTable = JsonConvert.DeserializeObject(Result, (TypeOf (DataTable)))

            Dim dt As DataTable = JsonConvert.DeserializeObject(Of DataTable)(Result)
            Return dt

        End Function


        Public Shared Function ProcessUserAcctLimits(ChannelCode As String, strAccountNo As String, Token As String) As DataTable


            Dim OriginatingIP As String = GetIPAddress().Replace("::ffff:", "")



            '  #region Details Alpha Beta 

            Dim strHostUrl As String = System.Configuration.ConfigurationManager.ConnectionStrings("iMPhaseHostUrl").ToString()
            ' //http://xxsg.ebs-rcm.com/interface/revcodebyAgencyRef"
            Dim strAgentUrl As String = String.Format("{0}/{1}", strHostUrl, "AccountLimits.aspx")





            '#Region Add Auth Info
            Dim httpWebRequest As HttpWebRequest = HttpWebRequest.Create(strAgentUrl)

            httpWebRequest.ContentType = "application/json"

            httpWebRequest.Method = "POST"
            httpWebRequest.Headers.Add("Token", Token)


            '  #endregion

            '    ChannelCode = ChannelCode,
            ''            AccountNo = strAccountNo
            Using streamWriter = New StreamWriter(httpWebRequest.GetRequestStream())

                Dim ljson As Object = New With
                    {
                   .ChannelCode = ChannelCode,
                   .AccountNo = strAccountNo
                }
                Dim serializer As New JavaScriptSerializer()
                Dim serializedResult = serializer.Serialize(ljson)
                ' // json = Convert.ToBase64String(Encoding.Default.GetBytes(json))
                streamWriter.Write(serializedResult)
                streamWriter.Flush()
                streamWriter.Close()
            End Using

            Dim HttpResponse As HttpWebResponse = CType(httpWebRequest.GetResponse(), HttpWebResponse)

            Dim Result = ""


            Using streamReader = New StreamReader(HttpResponse.GetResponseStream())
                Result = streamReader.ReadToEnd()
            End Using


            ' #endregion

            'Dim jss As JavaScriptSerializer = New JavaScriptSerializer()
            ''Dim Jsonresult As New Dictionary(Of String, String) = jss.DeserializeObject(results)}
            'Dim Jsonresult = jss.DeserializeObject(Result)
            'Dim dt As DataTable = JsonConvert.DeserializeObject(Result, (TypeOf (DataTable)))

            Dim dt As DataTable = JsonConvert.DeserializeObject(Of DataTable)(Result)
            Return dt

        End Function


        Public Shared Function ProcessUserAcctList(strUserName As String, strAccountNo As String, Token As String) As DataTable


            Dim OriginatingIP As String = GetIPAddress().Replace("::ffff:", "")



            '  #region Details Alpha Beta 

            Dim strHostUrl As String = System.Configuration.ConfigurationManager.ConnectionStrings("iMPhaseHostUrl").ToString()
            ' //http://xxsg.ebs-rcm.com/interface/revcodebyAgencyRef"
            Dim strAgentUrl As String = String.Format("{0}/{1}", strHostUrl, "UserAcctListings.aspx")





            '#Region Add Auth Info
            Dim httpWebRequest As HttpWebRequest = HttpWebRequest.Create(strAgentUrl)

            httpWebRequest.ContentType = "application/json"

            httpWebRequest.Method = "POST"
            httpWebRequest.Headers.Add("Token", Token)


            '  #endregion

            '    ChannelCode = ChannelCode,
            ''            AccountNo = strAccountNo
            Using streamWriter = New StreamWriter(httpWebRequest.GetRequestStream())

                Dim ljson As Object = New With
                    {
                   .UserName = strUserName,
                   .AccountNo = strAccountNo
                }
                Dim serializer As New JavaScriptSerializer()
                Dim serializedResult = serializer.Serialize(ljson)
                ' // json = Convert.ToBase64String(Encoding.Default.GetBytes(json))
                streamWriter.Write(serializedResult)
                streamWriter.Flush()
                streamWriter.Close()
            End Using

            Dim HttpResponse As HttpWebResponse = CType(httpWebRequest.GetResponse(), HttpWebResponse)

            Dim Result = ""


            Using streamReader = New StreamReader(HttpResponse.GetResponseStream())
                Result = streamReader.ReadToEnd()
            End Using


            ' #endregion

            'Dim jss As JavaScriptSerializer = New JavaScriptSerializer()
            'Dim Jsonresult As New Dictionary(Of String, String) = jss.DeserializeObject(results)}
            'Dim Jsonresult = jss.DeserializeObject(Result)
            'Dim dt As DataTable = JsonConvert.DeserializeObject(Result, (TypeOf (DataTable)))
            'Dim dataSet = JsonConvert.DeserializeObject(Of DataSet)(Result)
            'Dim dataSet = Jsonresult.(Result)("root").ToObject(Of DataTable)()
            'Dim dt As DataTable = dataSet.Tables(0)



            'Dim x = Newtonsoft.Json.Linq.JObject.Parse(Result)
            'Dim xd1 As XmlDocument = JsonConvert.DeserializeObject(Of XmlDocument)(Result) '(XmlDocument)JsonConvert.DeserializeXmlNode(Result, "root")
            'Dim xmlReader = New XmlNodeReader(xd1)
            'Dim ds As DataSet = New DataSet()
            'ds.ReadXml(xmlReader)
            'Dim View As DataView = New DataView(ds.Tables(0))
            'Dim dt As DataTable = View.ToTable()

            Dim dt As DataTable = JsonConvert.DeserializeObject(Of DataTable)(Result)
            Return dt


        End Function

        Public Shared Function ProcessBankList(Token As String) As DataTable


            Dim OriginatingIP As String = GetIPAddress().Replace("::ffff:", "")



            '  #region Details Alpha Beta 

            Dim strHostUrl As String = System.Configuration.ConfigurationManager.ConnectionStrings("iMPhaseHostUrl").ToString()
            ' //http://xxsg.ebs-rcm.com/interface/revcodebyAgencyRef"
            Dim strAgentUrl As String = String.Format("{0}/{1}", strHostUrl, "FTBankLIstings.aspx")





            '#Region Add Auth Info
            Dim httpWebRequest As HttpWebRequest = HttpWebRequest.Create(strAgentUrl)

            httpWebRequest.ContentType = "application/json"

            httpWebRequest.Method = "POST"
            httpWebRequest.Headers.Add("Token", Token)


            '  #endregion

            '    ChannelCode = ChannelCode,
            ''            AccountNo = strAccountNo
            Using streamWriter = New StreamWriter(httpWebRequest.GetRequestStream())

                Dim ljson As Object = Nothing
                Dim serializer As New JavaScriptSerializer()
                Dim serializedResult = serializer.Serialize(ljson)
                ' // json = Convert.ToBase64String(Encoding.Default.GetBytes(json))
                streamWriter.Write(serializedResult)
                streamWriter.Flush()
                streamWriter.Close()
            End Using

            Dim HttpResponse As HttpWebResponse = CType(httpWebRequest.GetResponse(), HttpWebResponse)

            Dim Result = ""


            Using streamReader = New StreamReader(HttpResponse.GetResponseStream())
                Result = streamReader.ReadToEnd()
            End Using


            ' #endregion

            'Dim jss As JavaScriptSerializer = New JavaScriptSerializer()
            'Dim Jsonresult As New Dictionary(Of String, String) = jss.DeserializeObject(results)}
            'Dim Jsonresult = jss.DeserializeObject(Result)
            'Dim dt As DataTable = JsonConvert.DeserializeObject(Result, (TypeOf (DataTable)))
            'Dim dataSet = JsonConvert.DeserializeObject(Of DataSet)(Result)
            'Dim dataSet = Jsonresult.(Result)("root").ToObject(Of DataTable)()
            'Dim dt As DataTable = dataSet.Tables(0)



            'Dim x = Newtonsoft.Json.Linq.JObject.Parse(Result)
            'Dim xd1 As XmlDocument = JsonConvert.DeserializeObject(Of XmlDocument)(Result) '(XmlDocument)JsonConvert.DeserializeXmlNode(Result, "root")
            'Dim xmlReader = New XmlNodeReader(xd1)
            'Dim ds As DataSet = New DataSet()
            'ds.ReadXml(xmlReader)
            'Dim View As DataView = New DataView(ds.Tables(0))
            'Dim dt As DataTable = View.ToTable()

            Dim dt As DataTable = JsonConvert.DeserializeObject(Of DataTable)(Result)
            Return dt


        End Function



        Public Shared Function ProcessGetTempTrans(TransRefSeq As String, UserName As String, FormatType As String, Token As String) As DataTable


            Dim OriginatingIP As String = GetIPAddress().Replace("::ffff:", "")



            '  #region Details Alpha Beta 

            Dim strHostUrl As String = System.Configuration.ConfigurationManager.ConnectionStrings("iMPhaseHostUrl").ToString()
            ' //http://xxsg.ebs-rcm.com/interface/revcodebyAgencyRef"
            Dim strAgentUrl As String = String.Format("{0}/{1}", strHostUrl, "TransDetailsHistGetInfo.aspx")





            '#Region Add Auth Info
            Dim httpWebRequest As HttpWebRequest = HttpWebRequest.Create(strAgentUrl)

            httpWebRequest.ContentType = "application/json"

            httpWebRequest.Method = "POST"
            httpWebRequest.Headers.Add("Token", Token)


            '  #endregion


            Using streamWriter = New StreamWriter(httpWebRequest.GetRequestStream())

                Dim ljson As Object = New With
                    {
                    .TransRefSeq = TransRefSeq,
                    .UserName = UserName,
                    .FormatType = FormatType
                }
                Dim serializer As New JavaScriptSerializer()
                Dim serializedResult = serializer.Serialize(ljson)
                ' // json = Convert.ToBase64String(Encoding.Default.GetBytes(json))
                streamWriter.Write(serializedResult)
                streamWriter.Flush()
                streamWriter.Close()
            End Using

            Dim HttpResponse As HttpWebResponse = CType(httpWebRequest.GetResponse(), HttpWebResponse)

            Dim Result = ""


            Using streamReader = New StreamReader(HttpResponse.GetResponseStream())
                Result = streamReader.ReadToEnd()
            End Using


            ' #endregion

            'Dim jss As JavaScriptSerializer = New JavaScriptSerializer()
            ''Dim Jsonresult As New Dictionary(Of String, String) = jss.DeserializeObject(results)}
            'Dim Jsonresult = jss.DeserializeObject(Result)
            'Dim dt As DataTable = JsonConvert.DeserializeObject(Result, (TypeOf (DataTable)))

            Dim dt As DataTable = JsonConvert.DeserializeObject(Of DataTable)(Result)
            Return dt

        End Function





        Public Shared Function ProcessInterBankBenName(creditAccountNo As String, creditBankcode As String, mobileno As String, Location As String, Token As String) As String

            Dim strHostUrl As String = System.Configuration.ConfigurationManager.ConnectionStrings("iMPhaseHostUrl").ToString()



            Dim strAgentUrl As String = String.Format("{0}/{1}", strHostUrl, "CheckBenName.aspx")


            '#Region Add Auth Info
            Dim httpWebRequest As HttpWebRequest = HttpWebRequest.Create(strAgentUrl)

            httpWebRequest.ContentType = "application/json"

            httpWebRequest.Method = "POST"
            httpWebRequest.Headers.Add("Token", Token)


            '  #endregion

            Dim RequestTimeStamp As String = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm:ss+0000")
            Using streamWriter = New StreamWriter(httpWebRequest.GetRequestStream())

                Dim ljson As Object = New With
                    {
                       .creditAccountNo = creditAccountNo,
                       .creditBankcode = creditBankcode
                }

                Dim serializer As New JavaScriptSerializer()
                Dim serializedResult = serializer.Serialize(ljson)
                streamWriter.Write(serializedResult)
                streamWriter.Flush()
                streamWriter.Close()
            End Using

            Dim HttpResponse As HttpWebResponse = CType(httpWebRequest.GetResponse(), HttpWebResponse)

            Dim Result = ""


            Using streamReader = New StreamReader(HttpResponse.GetResponseStream())
                Result = streamReader.ReadToEnd()
            End Using


            ' #endregion


            Return Result


        End Function


        Public Shared Function ProcessTellerAccessNoMsg(UserName As String, PassWord As String) As String

            Dim strHostUrl As String = System.Configuration.ConfigurationManager.ConnectionStrings("iMPhaseHostUrl").ToString()



            Dim strAgentUrl As String = String.Format("{0}/{1}", strHostUrl, "UserTellerAccess.aspx")
            'Region auth info
            Dim HeaderUsername As String = System.Configuration.ConfigurationManager.ConnectionStrings("KeyID").ConnectionString
            Dim Headerpwds As String = System.Configuration.ConfigurationManager.ConnectionStrings("Key").ConnectionString

            Dim strDeptHeadUserName As String = NETClass.Classes.DecryptString(HeaderUsername)
            Dim strDeptHeadpwds As String = NETClass.Classes.DecryptString(Headerpwds)

            Dim strToken = Convert.ToBase64String(Encoding.Default.GetBytes(String.Format("{0}:{1}", strDeptHeadUserName, strDeptHeadpwds)))

            'End auth Info

            '#Region Add Auth Info
            Dim httpWebRequest As HttpWebRequest = HttpWebRequest.Create(strAgentUrl)

            httpWebRequest.ContentType = "application/json"

            httpWebRequest.Method = "POST"
            httpWebRequest.Headers.Add("AuthToken", strToken)


            '  #endregion

            Dim RequestTimeStamp As String = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm:ss+0000")
            Using streamWriter = New StreamWriter(httpWebRequest.GetRequestStream())

                Dim ljson As Object = New With
                    {
                        .UserName = UserName,
                        .PassWord = PassWord
                }

                Dim serializer As New JavaScriptSerializer()
                Dim serializedResult = serializer.Serialize(ljson)
                streamWriter.Write(serializedResult)
                streamWriter.Flush()
                streamWriter.Close()
            End Using

            Dim HttpResponse As HttpWebResponse = CType(httpWebRequest.GetResponse(), HttpWebResponse)

            Dim Result = ""


            Using streamReader = New StreamReader(HttpResponse.GetResponseStream())
                Result = streamReader.ReadToEnd()
            End Using


            ' #endregion


            Return Result


        End Function


        Public Shared Function ProcessUserLogout(UserName As String) As String

            Dim strHostUrl As String = System.Configuration.ConfigurationManager.ConnectionStrings("iMPhaseHostUrl").ToString()



            Dim strAgentUrl As String = String.Format("{0}/{1}", strHostUrl, "UserLogout.aspx")
            'Region auth info
            Dim HeaderUsername As String = System.Configuration.ConfigurationManager.ConnectionStrings("KeyID").ConnectionString
            Dim Headerpwds As String = System.Configuration.ConfigurationManager.ConnectionStrings("Key").ConnectionString

            Dim strDeptHeadUserName As String = NETClass.Classes.DecryptString(HeaderUsername)
            Dim strDeptHeadpwds As String = NETClass.Classes.DecryptString(Headerpwds)

            Dim strToken = Convert.ToBase64String(Encoding.Default.GetBytes(String.Format("{0}:{1}", strDeptHeadUserName, strDeptHeadpwds)))

            'End auth Info

            '#Region Add Auth Info
            Dim httpWebRequest As HttpWebRequest = HttpWebRequest.Create(strAgentUrl)

            httpWebRequest.ContentType = "application/json"

            httpWebRequest.Method = "POST"
            httpWebRequest.Headers.Add("AuthToken", strToken)


            '  #endregion

            Dim RequestTimeStamp As String = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm:ss+0000")
            Using streamWriter = New StreamWriter(httpWebRequest.GetRequestStream())

                Dim ljson As Object = New With
                    {
                        .UserName = UserName
                }

                Dim serializer As New JavaScriptSerializer()
                Dim serializedResult = serializer.Serialize(ljson)
                streamWriter.Write(serializedResult)
                streamWriter.Flush()
                streamWriter.Close()
            End Using

            Dim HttpResponse As HttpWebResponse = CType(httpWebRequest.GetResponse(), HttpWebResponse)

            Dim Result = ""


            Using streamReader = New StreamReader(HttpResponse.GetResponseStream())
                Result = streamReader.ReadToEnd()
            End Using


            ' #endregion


            Return Result


        End Function

        Public Shared Function ProcessTellerInterFTOTPAuth(PIN As String, AccountNo As String,
          creditAccountNo As String, mobileNo As String, Location As String, amount As Decimal,
          Fee As Decimal, narration As String, UserName As String, creditBankcode As String, TransOthersRef As String, Token As String, OTP As String
        ) As String

            Dim strHostUrl As String = System.Configuration.ConfigurationManager.ConnectionStrings("iMPhaseHostUrl").ToString()



            Dim strAgentUrl As String = String.Format("{0}/{1}", strHostUrl, "FTTellerSingleOTPAuth.aspx")
            'Region auth info
            Dim HeaderUsername As String = System.Configuration.ConfigurationManager.ConnectionStrings("KeyID").ConnectionString
            Dim Headerpwds As String = System.Configuration.ConfigurationManager.ConnectionStrings("Key").ConnectionString

            Dim strDeptHeadUserName As String = NETClass.Classes.DecryptString(HeaderUsername)
            Dim strDeptHeadpwds As String = NETClass.Classes.DecryptString(Headerpwds)

            Dim strToken = Convert.ToBase64String(Encoding.Default.GetBytes(String.Format("{0}:{1}", strDeptHeadUserName, strDeptHeadpwds)))

            'End auth Info

            '#Region Add Auth Info
            Dim httpWebRequest As HttpWebRequest = HttpWebRequest.Create(strAgentUrl)

            httpWebRequest.ContentType = "application/json"

            httpWebRequest.Method = "POST"
            httpWebRequest.Headers.Add("Token", Token)

            Dim strTransCompToken As String = Convert.ToBase64String(Encoding.Default.GetBytes(String.Format("{0}:{1}:{2}:{3}:{4}", AccountNo, creditAccountNo, creditBankcode, amount, narration)))


            httpWebRequest.Headers.Add("TransToken", strTransCompToken)

            '  #endregion

            Dim RequestTimeStamp As String = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm:ss+0000")
            Using streamWriter = New StreamWriter(httpWebRequest.GetRequestStream())

                Dim ljson As Object = New With
                    {
                   .PIN = PIN,
                    .AccountNo = AccountNo,
                    .creditAccountNo = creditAccountNo,
                   .mobileNo = mobileNo,
                    .Location = Location,
                    .amount = amount,
                    .Fee = Fee,
                    .narration = narration,
                    .UserName = UserName,
                    .creditBankcode = creditBankcode,
                    .OTP = OTP,
                    .TransOthersRef = TransOthersRef
                }

                Dim serializer As New JavaScriptSerializer()
                Dim serializedResult = serializer.Serialize(ljson)
                streamWriter.Write(serializedResult)
                streamWriter.Flush()
                streamWriter.Close()
            End Using

            Dim HttpResponse As HttpWebResponse = CType(httpWebRequest.GetResponse(), HttpWebResponse)

            Dim Result = ""


            Using streamReader = New StreamReader(HttpResponse.GetResponseStream())
                Result = streamReader.ReadToEnd()
            End Using


            ' #endregion


            Return Result


        End Function

        Public Shared Function ProcessTellerInterFTOTP(PIN As String, AccountNo As String,
          creditAccountNo As String, mobileNo As String, Location As String, amount As Decimal,
          Fee As Decimal, narration As String, UserName As String, creditBankcode As String, TransOthersRef As String, Token As String
        ) As String

            Dim strHostUrl As String = System.Configuration.ConfigurationManager.ConnectionStrings("iMPhaseHostUrl").ToString()



            Dim strAgentUrl As String = String.Format("{0}/{1}", strHostUrl, "FTTellerSingleRef.aspx")
            'Region auth info
            Dim HeaderUsername As String = System.Configuration.ConfigurationManager.ConnectionStrings("KeyID").ConnectionString
            Dim Headerpwds As String = System.Configuration.ConfigurationManager.ConnectionStrings("Key").ConnectionString

            Dim strDeptHeadUserName As String = NETClass.Classes.DecryptString(HeaderUsername)
            Dim strDeptHeadpwds As String = NETClass.Classes.DecryptString(Headerpwds)

            Dim strToken = Convert.ToBase64String(Encoding.Default.GetBytes(String.Format("{0}:{1}", strDeptHeadUserName, strDeptHeadpwds)))

            'End auth Info

            '#Region Add Auth Info
            Dim httpWebRequest As HttpWebRequest = HttpWebRequest.Create(strAgentUrl)

            httpWebRequest.ContentType = "application/json"

            httpWebRequest.Method = "POST"
            httpWebRequest.Headers.Add("Token", Token)

            Dim strTransCompToken As String = Convert.ToBase64String(Encoding.Default.GetBytes(String.Format("{0}:{1}:{2}:{3}:{4}", AccountNo, creditAccountNo, creditBankcode, amount, narration)))


            httpWebRequest.Headers.Add("TransToken", strTransCompToken)

            '  #endregion

            Dim RequestTimeStamp As String = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm:ss+0000")
            Using streamWriter = New StreamWriter(httpWebRequest.GetRequestStream())

                Dim ljson As Object = New With
                    {
                   .PIN = PIN,
                    .AccountNo = AccountNo,
                    .creditAccountNo = creditAccountNo,
                   .mobileNo = mobileNo,
                    .Location = Location,
                    .amount = amount,
                    .Fee = Fee,
                    .narration = narration,
                    .UserName = UserName,
                    .creditBankcode = creditBankcode,
                    .TransOthersRef = TransOthersRef
                }

                Dim serializer As New JavaScriptSerializer()
                Dim serializedResult = serializer.Serialize(ljson)
                streamWriter.Write(serializedResult)
                streamWriter.Flush()
                streamWriter.Close()
            End Using

            Dim HttpResponse As HttpWebResponse = CType(httpWebRequest.GetResponse(), HttpWebResponse)

            Dim Result = ""


            Using streamReader = New StreamReader(HttpResponse.GetResponseStream())
                Result = streamReader.ReadToEnd()
            End Using


            ' #endregion


            Return Result


        End Function

        Public Shared Function AuthUserHardToken(UserName As String, ClientId As String, PIN As String) As String

            Dim strHostUrl As String = System.Configuration.ConfigurationManager.ConnectionStrings("iMPhaseHostUrl").ToString()



            Dim strAgentUrl As String = String.Format("{0}", strHostUrl, "http://client.twofactor.vanso.com/")
            'Region auth info
            Dim HeaderUsername As String = System.Configuration.ConfigurationManager.ConnectionStrings("KeyID").ConnectionString
            Dim Headerpwds As String = System.Configuration.ConfigurationManager.ConnectionStrings("Key").ConnectionString

            Dim strDeptHeadUserName As String = NETClass.Classes.DecryptString(HeaderUsername)
            Dim strDeptHeadpwds As String = NETClass.Classes.DecryptString(Headerpwds)

            Dim strToken = Convert.ToBase64String(Encoding.Default.GetBytes(String.Format("{0}:{1}", strDeptHeadUserName, strDeptHeadpwds)))

            'End auth Info

            '#Region Add Auth Info
            Dim httpWebRequest As HttpWebRequest = HttpWebRequest.Create(strAgentUrl)

            httpWebRequest.ContentType = "application/json"

            httpWebRequest.Method = "POST"
            'httpWebRequest.Headers.Add("AuthToken", strToken)


            '  #endregion

            Dim RequestTimeStamp As String = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm:ss+0000")
            Using streamWriter = New StreamWriter(httpWebRequest.GetRequestStream())

                Dim ljson As Object = New With
                    {
                        .username = UserName,
                        .ClientId = ClientId,
                        .pin = PIN
                }

                Dim serializer As New JavaScriptSerializer()
                Dim serializedResult = serializer.Serialize(ljson)
                streamWriter.Write(serializedResult)
                streamWriter.Flush()
                streamWriter.Close()
            End Using

            Dim HttpResponse As HttpWebResponse = CType(httpWebRequest.GetResponse(), HttpWebResponse)

            Dim Result = ""


            Using streamReader = New StreamReader(HttpResponse.GetResponseStream())
                Result = streamReader.ReadToEnd()
            End Using


            ' #endregion


            Return Result


        End Function


        Public Shared Function ProcessLocalBenName(creditAccountNo As String, creditBankcode As String, mobileno As String, Location As String, Token As String) As String

            Dim strHostUrl As String = System.Configuration.ConfigurationManager.ConnectionStrings("iMPhaseHostUrl").ToString()



            Dim strAgentUrl As String = String.Format("{0}/{1}", strHostUrl, "CheckLocalBenName.aspx")


            '#Region Add Auth Info
            Dim httpWebRequest As HttpWebRequest = HttpWebRequest.Create(strAgentUrl)

            httpWebRequest.ContentType = "application/json"

            httpWebRequest.Method = "POST"
            httpWebRequest.Headers.Add("Token", Token)


            '  #endregion

            Dim RequestTimeStamp As String = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm:ss+0000")
            Using streamWriter = New StreamWriter(httpWebRequest.GetRequestStream())

                Dim ljson As Object = New With
                    {
                       .creditAccountNo = creditAccountNo,
                       .creditBankcode = creditBankcode
                }

                Dim serializer As New JavaScriptSerializer()
                Dim serializedResult = serializer.Serialize(ljson)
                streamWriter.Write(serializedResult)
                streamWriter.Flush()
                streamWriter.Close()
            End Using

            Dim HttpResponse As HttpWebResponse = CType(httpWebRequest.GetResponse(), HttpWebResponse)

            Dim Result = ""


            Using streamReader = New StreamReader(HttpResponse.GetResponseStream())
                Result = streamReader.ReadToEnd()
            End Using


            ' #endregion


            Return Result


        End Function

    End Class
End Namespace

