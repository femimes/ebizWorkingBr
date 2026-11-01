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
    Public Class IPayCode



    End Class
    Public Class ICardProcess
        Public Shared Function GenAuthdata(strclientIds As String, strclientSecretKey As String, version As String, pan As String, pin As String, expirydate As String, cvv2 As String) As String
            Dim clientIds As String = strclientIds '"IKIA129ED551DCE19B3FD12A943E4487592BE945443A"
            Dim clientSecretKey As String = strclientSecretKey '"yLZAmbgeU+WshKtIwuBSESLriYgerO80O0y09mY0Zco="

            Dim publicexponet = "010001"
            Dim publicmodulous = "009c7b3ba621a26c4b02f48cfc07ef6ee0aed8e12b4bd11c5cc0abf80d5206be69e1891e60fc88e2d565e2fabe4d0cf630e318a6c721c3ded718d0c530cdf050387ad0a30a336899bbda877d0ec7c7c3ffe693988bfae0ffbab71b25468c7814924f022cb5fda36e0d2c30a7161fa1c6fb5fbd7d05adbef7e68d48f8b6c5f511827c4b1c5ed15b6f20555affc4d0857ef7ab2b5c18ba22bea5d3a79bd1834badb5878d8c7a4b19da20c1f62340b1f7fbf01d2f2e97c9714a9df376ac0ea58072b2b77aeb7872b54a89667519de44d0fc73540beeaec4cb778a45eebfbefe2d817a8a8319b2bc6d9fa714f5289ec7c0dbc43496d71cf2a642cb679b0fc4072fd2cf"
            version = "1"
            Dim authDataCipher As String = version + "Z" + pan + "Z" + pin + "Z" + expirydate + "Z" + cvv2
            Dim encdata As Byte() = Encoding.ASCII.GetBytes(authDataCipher)
            Dim encdatavals As String = Interswitch.SecurityUtils.GetAuthData(pan, pin, expirydate, cvv2, publicmodulous, publicexponet)

            Dim fd As Interswitch.Interswitch = New Interswitch.Interswitch(clientIds, clientSecretKey)
            Dim results As String = fd.GetAuthData(pan, expirydate, cvv2, pin, publicmodulous, publicexponet)

            Return results
        End Function

        Public Shared Function Blockcards(bankCode As String, Userid As String, strclientIds As String, strclientSecretKey As String, version As String, pan As String, pin As String, expirydate As String, cvv2 As String) As String
            Dim clientIds As String = strclientIds '"IKIA129ED551DCE19B3FD12A943E4487592BE945443A"
            Dim clientSecretKey As String = strclientSecretKey '"yLZAmbgeU+WshKtIwuBSESLriYgerO80O0y09mY0Zco="

            Dim struserId As String = "ITMB"
            Dim strbankCode As String = bankCode ' "72123"
            Dim strauthData As String = "DBqAUUSjARvlx2bZ9USHjxsoUVHxJjx7ttAJAZka3NyYCLmJHYMXRY0PBTxazqVHO6eh3GI0MTv3BSVw+lZFMwgvtyCJj6tCq9fyo2HTnx+nbvj4F7ZKYWeq0xoYSVESEA2BPzQf9L1tHjm+WxrqB/SIzIyC0sAZxKPSBieib+ANmuyIYlKLukIAS9IwOsbYGpb4wTcaMebKCeRHmSbEdGpOztcnIjDFRgWpMZo1wUDyfonuBt1XLfVnPq3F8oX0K9oOwVJuv4cus+kGWPBvndMuBNiqOgvKp78uuUVOrdMgVdnJ0Ysi+Uer9q8102iyfScRYY79H6e/7JVx1quCzw=="

            Dim strissuerNr As String = "72"

            Dim strenableActiveActive As String = "yes"


            Dim strAgentUrl As String = String.Format("{0}{1}", ConfigurationManager.ConnectionStrings("PacodeAgentUrl").ConnectionString(), "block-card")
            Dim strurl As String = String.Format("{0}{1}", ConfigurationManager.ConnectionStrings("PacodeHost").ConnectionString(), "cards/block")


            Dim strhost As String = ConfigurationManager.ConnectionStrings("PacodeHost").ConnectionString()

            Dim pocoObject = New With
            {.sequenceNo = "001",
                .holdResponseCode = "05",
                .authData = strauthData
            }

            Dim json As String = JsonConvert.SerializeObject(pocoObject)
            '#Region Add Auth Info
            Dim HeaderUsername As String = System.Configuration.ConfigurationManager.ConnectionStrings("PacodeKeyID").ConnectionString()
            Dim Headerpwds As String = System.Configuration.ConfigurationManager.ConnectionStrings("PacodeKey").ConnectionString()

            Dim strDeptHeadUserName As String = NETClass.Classes.DecryptString(HeaderUsername)
            Dim strDeptHeadpwds As String = NETClass.Classes.DecryptString(Headerpwds)

            Dim strAuthToken = Convert.ToBase64String(Encoding.Default.GetBytes(String.Format("{0}:{1}", strDeptHeadUserName, strDeptHeadpwds)))


            Dim httpWebRequest As HttpWebRequest = HttpWebRequest.Create(strAgentUrl)


            httpWebRequest.ContentType = "application/json"

            httpWebRequest.Method = "POST"

            httpWebRequest.Headers.Add("Authorisation", strAuthToken)
            Using streamWriter = New StreamWriter(httpWebRequest.GetRequestStream())
                Dim ljson As Object = New With
                    {
                    .userId = struserId,
                    .bankCode = strbankCode,
                    .authData = strauthData,
                    .issuerNr = strissuerNr,
                    .expiryDate = expirydate,
                    .pan = pan,
                    .seqNr = "001",
                    .holdRspCode = "05",
                    .url = strurl,
                    .clientId = clientIds,
                    .clientSecret = clientSecretKey,
                    .host = strhost,
                    .enableActiveActive = strenableActiveActive
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




            '#Region end
            Return Result
        End Function

        Public Shared Function UnBlockcards(bankCode As String, Userid As String, strclientIds As String, strclientSecretKey As String, version As String, pan As String, pin As String, expirydate As String, cvv2 As String) As String
            Dim clientIds As String = strclientIds ' "IKIA129ED551DCE19B3FD12A943E4487592BE945443A"
            Dim clientSecretKey As String = strclientSecretKey '"yLZAmbgeU+WshKtIwuBSESLriYgerO80O0y09mY0Zco="

            Dim struserId As String = "ITMB"
            Dim strbankCode As String = bankCode ' "72123"
            Dim strauthData As String = "DBqAUUSjARvlx2bZ9USHjxsoUVHxJjx7ttAJAZka3NyYCLmJHYMXRY0PBTxazqVHO6eh3GI0MTv3BSVw+lZFMwgvtyCJj6tCq9fyo2HTnx+nbvj4F7ZKYWeq0xoYSVESEA2BPzQf9L1tHjm+WxrqB/SIzIyC0sAZxKPSBieib+ANmuyIYlKLukIAS9IwOsbYGpb4wTcaMebKCeRHmSbEdGpOztcnIjDFRgWpMZo1wUDyfonuBt1XLfVnPq3F8oX0K9oOwVJuv4cus+kGWPBvndMuBNiqOgvKp78uuUVOrdMgVdnJ0Ysi+Uer9q8102iyfScRYY79H6e/7JVx1quCzw=="

            Dim strissuerNr As String = "72"

            Dim strenableActiveActive As String = "yes"


            Dim strAgentUrl As String = String.Format("{0}{1}", ConfigurationManager.ConnectionStrings("PacodeAgentUrl").ConnectionString(), "unblock-card")
            Dim strurl As String = String.Format("{0}{1}", ConfigurationManager.ConnectionStrings("PacodeHost").ConnectionString(), "cards/unblock")


            Dim strhost As String = ConfigurationManager.ConnectionStrings("PacodeHost").ConnectionString()


            Dim pocoObject = New With
            {.sequenceNo = "001",
                .holdResponseCode = "45",
                .authData = strauthData
            }

            Dim json As String = JsonConvert.SerializeObject(pocoObject)
            '#Region Add Auth Info
            Dim httpWebRequest As HttpWebRequest = HttpWebRequest.Create(strAgentUrl)
            Dim HeaderUsername As String = System.Configuration.ConfigurationManager.ConnectionStrings("PacodeKeyID").ConnectionString()
            Dim Headerpwds As String = System.Configuration.ConfigurationManager.ConnectionStrings("PacodeKey").ConnectionString()

            Dim strDeptHeadUserName As String = NETClass.Classes.DecryptString(HeaderUsername)
            Dim strDeptHeadpwds As String = NETClass.Classes.DecryptString(Headerpwds)

            Dim strAuthToken = Convert.ToBase64String(Encoding.Default.GetBytes(String.Format("{0}:{1}", strDeptHeadUserName, strDeptHeadpwds)))


            strauthData = GenAuthdata(clientIds, clientSecretKey, version, pan, pin, expirydate, cvv2)
            httpWebRequest.ContentType = "application/json"

            httpWebRequest.Method = "POST"
            httpWebRequest.Headers.Add("Authorisation", strAuthToken)
            Using streamWriter = New StreamWriter(httpWebRequest.GetRequestStream())
                Dim ljson As Object = New With
                    {
                    .data = json,
                    .userId = struserId,
                    .bankCode = strbankCode,
                    .authData = strauthData,
                    .issuerNr = strissuerNr,
                    .expiryDate = expirydate,
                    .pan = pan,
                    .seqNr = "001",
                    .holdRspCode = "90",
                    .accountType = version,
                    .url = strurl,
                    .clientId = clientIds,
                    .clientSecret = clientSecretKey,
                    .host = strhost,
                    .enableActiveActive = strenableActiveActive
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



            '#Region end
            Return Result
        End Function


        Public Shared Function Linkcards(bankCode As String, Userid As String, strclientIds As String, strclientSecretKey As String, version As String, pan As String, pin As String, expirydate As String, cvv2 As String, AccountNoLinked As String) As String
            Dim clientIds As String = strclientIds ' "IKIA129ED551DCE19B3FD12A943E4487592BE945443A"
            Dim clientSecretKey As String = strclientSecretKey '"yLZAmbgeU+WshKtIwuBSESLriYgerO80O0y09mY0Zco="

            Dim struserId As String = "ITMB"
            Dim strbankCode As String = bankCode ' "72123"
            Dim strauthData As String = "DBqAUUSjARvlx2bZ9USHjxsoUVHxJjx7ttAJAZka3NyYCLmJHYMXRY0PBTxazqVHO6eh3GI0MTv3BSVw+lZFMwgvtyCJj6tCq9fyo2HTnx+nbvj4F7ZKYWeq0xoYSVESEA2BPzQf9L1tHjm+WxrqB/SIzIyC0sAZxKPSBieib+ANmuyIYlKLukIAS9IwOsbYGpb4wTcaMebKCeRHmSbEdGpOztcnIjDFRgWpMZo1wUDyfonuBt1XLfVnPq3F8oX0K9oOwVJuv4cus+kGWPBvndMuBNiqOgvKp78uuUVOrdMgVdnJ0Ysi+Uer9q8102iyfScRYY79H6e/7JVx1quCzw=="

            Dim strissuerNr As String = "72"
            Dim accountType As String = "20"
            Dim strenableActiveActive As String = "yes"
            Dim AcctCatCode As String = "1"

            Dim strAcctType As String = String.Format("SpAcctMasterSelectOne '{0}'", AccountNoLinked)

            For Each item As DataRow In NETClass.Classes.AXGetDataset(strAcctType).Tables(0).Rows

                AcctCatCode = item("ProdCatCode")

            Next

            If AcctCatCode = "2" Then

                accountType = "10"

            ElseIf AcctCatCode = "3" Then

                accountType = "50"
            ElseIf AcctCatCode = "4" Then

                accountType = "50"
            ElseIf AcctCatCode = "6" Then

                accountType = "30"
            End If


            Dim strAgentUrl As String = String.Format("{0}{1}", ConfigurationManager.ConnectionStrings("PacodeAgentUrl").ConnectionString(), "linking-card-account")
            Dim strurl As String = String.Format("{0}{1}", ConfigurationManager.ConnectionStrings("PacodeHost").ConnectionString(), "cards/accounts")

            'strauthData = GenAuthdata(clientIds, clientSecretKey, version, pan, pin, expirydate, cvv2)

            Dim strhost As String = String.Format("{0}", ConfigurationManager.ConnectionStrings("PacodeHost").ConnectionString())

            'AddtoLogFile(strAgentUrl, "Url for linking")

            Dim pocoObject = New With
            {
                .issuerNr = strissuerNr,
                .expiryDate = expirydate,
                .pan = pan,
                .seqNr = "001",
                .accountNo = AccountNoLinked,
                .accountTypeQualifier = "1",
                .accountCurrency = "566",
                .sequenceNo = "001"
            }

            Dim json As String = JsonConvert.SerializeObject(pocoObject)

            'AddtoLogFile(json, "jsonRecord")
            '#Region Add Auth Info
            Dim HeaderUsername As String = System.Configuration.ConfigurationManager.ConnectionStrings("PacodeKeyID").ConnectionString()
            Dim Headerpwds As String = System.Configuration.ConfigurationManager.ConnectionStrings("PacodeKey").ConnectionString()
            'AddtoLogFile(HeaderUsername, String.Format("Raw Keys '{0}'", Headerpwds))
            Dim strDeptHeadUserName As String = NETClass.Classes.DecryptString(HeaderUsername)

            'AddtoLogFile(strDeptHeadUserName, String.Format("Decrypted Keys '{0}'", Headerpwds))
            Dim strDeptHeadpwds As String = NETClass.Classes.DecryptString(Headerpwds)

            Dim strAuthToken = Convert.ToBase64String(Encoding.Default.GetBytes(String.Format("{0}:{1}", strDeptHeadUserName, strDeptHeadpwds)))
            'AddtoLogFile(strAuthToken, "AuthToken")

            Dim httpWebRequest As HttpWebRequest = HttpWebRequest.Create(strAgentUrl)


            httpWebRequest.ContentType = "application/json"

            httpWebRequest.Method = "POST"

            httpWebRequest.Headers.Add("Authorisation", strAuthToken)
            Using streamWriter = New StreamWriter(httpWebRequest.GetRequestStream())
                Dim ljson As Object = New With
                    {
                    .userId = struserId,
                    .bankCode = strbankCode,
                    .authData = strauthData,
                    .issuerNr = strissuerNr,
                     .accountId = AccountNoLinked,
                     .accountType = accountType,
                     .expiryDate = expirydate,
                     .pan = pan,
                     .seqNr = "001",
                    .url = strurl,
                    .clientId = clientIds,
                    .clientSecret = clientSecretKey,
                    .host = strhost,
                    .enableActiveActive = strenableActiveActive
                }
                Dim serializer As New JavaScriptSerializer()
                Dim serializedResult = serializer.Serialize(ljson)

                streamWriter.Write(serializedResult)

                'AddtoLogFile(ljson, "card linking payload request")
                streamWriter.Flush()
                streamWriter.Close()
            End Using

            Dim HttpResponse As HttpWebResponse = CType(httpWebRequest.GetResponse(), HttpWebResponse)

            Dim Result = ""


            Using streamReader = New StreamReader(HttpResponse.GetResponseStream())
                Result = streamReader.ReadToEnd()
            End Using

            AddtoLogFile(Result, "card linking payload response")
            ' #endregion




            '#Region end
            Return Result
        End Function


        Public Shared Function UnLinkcards(bankCode As String, Userid As String, strclientIds As String, strclientSecretKey As String, version As String, pan As String, pin As String, expirydate As String, cvv2 As String, AccountNo As String) As String
            Dim clientIds As String = strclientIds '"IKIA129ED551DCE19B3FD12A943E4487592BE945443A"
            Dim clientSecretKey As String = strclientSecretKey '"yLZAmbgeU+WshKtIwuBSESLriYgerO80O0y09mY0Zco="

            Dim struserId As String = "ITMB"
            Dim strbankCode As String = bankCode ' "72123"
            Dim strauthData As String = "DBqAUUSjARvlx2bZ9USHjxsoUVHxJjx7ttAJAZka3NyYCLmJHYMXRY0PBTxazqVHO6eh3GI0MTv3BSVw+lZFMwgvtyCJj6tCq9fyo2HTnx+nbvj4F7ZKYWeq0xoYSVESEA2BPzQf9L1tHjm+WxrqB/SIzIyC0sAZxKPSBieib+ANmuyIYlKLukIAS9IwOsbYGpb4wTcaMebKCeRHmSbEdGpOztcnIjDFRgWpMZo1wUDyfonuBt1XLfVnPq3F8oX0K9oOwVJuv4cus+kGWPBvndMuBNiqOgvKp78uuUVOrdMgVdnJ0Ysi+Uer9q8102iyfScRYY79H6e/7JVx1quCzw=="

            Dim strissuerNr As String = "72"

            Dim strenableActiveActive As String = "yes"

            Dim accountType As String = "20"

            Dim AcctCatCode As String = "1"

            Dim strAcctType As String = String.Format("SpAcctMasterSelectOne '{0}'", AccountNo)

            For Each item As DataRow In NETClass.Classes.AXGetDataset(strAcctType).Tables(0).Rows

                AcctCatCode = item("ProdCatCode")

            Next

            If AcctCatCode = "2" Then

                accountType = "10"
            End If

            Dim strAgentUrl As String = String.Format("{0}{1}", ConfigurationManager.ConnectionStrings("PacodeAgentUrl").ConnectionString(), "unlink-card-account")
            Dim strurl As String = String.Format("{0}{1}", ConfigurationManager.ConnectionStrings("PacodeHost").ConnectionString(), "cards/accounts")


            Dim strhost As String = ConfigurationManager.ConnectionStrings("PacodeHost").ConnectionString()


            Dim pocoObject = New With
            {.authData = strauthData,
                .accountNo = AccountNo,
                .accountType = accountType,
                .accountTypeQualifier = "2",
                .accountCurrency = "566",
                .sequenceNo = "001"
            }

            Dim json As String = JsonConvert.SerializeObject(pocoObject)
            '#Region Add Auth Info
            Dim HeaderUsername As String = System.Configuration.ConfigurationManager.ConnectionStrings("PacodeKeyID").ConnectionString()
            Dim Headerpwds As String = System.Configuration.ConfigurationManager.ConnectionStrings("PacodeKey").ConnectionString()

            Dim strDeptHeadUserName As String = NETClass.Classes.DecryptString(HeaderUsername)
            Dim strDeptHeadpwds As String = NETClass.Classes.DecryptString(Headerpwds)

            Dim strAuthToken = Convert.ToBase64String(Encoding.Default.GetBytes(String.Format("{0}:{1}", strDeptHeadUserName, strDeptHeadpwds)))


            Dim httpWebRequest As HttpWebRequest = HttpWebRequest.Create(strAgentUrl)


            httpWebRequest.ContentType = "application/json"

            httpWebRequest.Method = "POST"

            httpWebRequest.Headers.Add("Authorisation", strAuthToken)
            Using streamWriter = New StreamWriter(httpWebRequest.GetRequestStream())
                Dim ljson As Object = New With
                    {
                    .userId = struserId,
                    .bankCode = strbankCode,
                    .accountNo = AccountNo,
                    .accountType = accountType,
                    .accountTypeQualifier = "2",
                    .accountCurrency = "566",
                    .sequenceNo = "001",
                    .issuerNr = strissuerNr,
                    .url = strurl,
                    .clientId = clientIds,
                    .clientSecret = clientSecretKey,
                    .host = strhost,
                    .enableActiveActive = strenableActiveActive
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



            '#Region end
            Return Result
        End Function


        Public Shared Function PinReIssuance(bankCode As String, Userid As String, strclientIds As String, strclientSecretKey As String, version As String, pan As String, pin As String, expirydate As String, cvv2 As String, strseqNr As String, strcustomerId As String, strmobileNr As String, struserIds As String) As String
            Dim clientIds As String = strclientIds ' "IKIA129ED551DCE19B3FD12A943E4487592BE945443A"
            Dim clientSecretKey As String = strclientSecretKey '"yLZAmbgeU+WshKtIwuBSESLriYgerO80O0y09mY0Zco="

            Dim struserId As String = Userid
            Dim strbankCode As String = bankCode
            Dim strauthData As String = "DBqAUUSjARvlx2bZ9USHjxsoUVHxJjx7ttAJAZka3NyYCLmJHYMXRY0PBTxazqVHO6eh3GI0MTv3BSVw+lZFMwgvtyCJj6tCq9fyo2HTnx+nbvj4F7ZKYWeq0xoYSVESEA2BPzQf9L1tHjm+WxrqB/SIzIyC0sAZxKPSBieib+ANmuyIYlKLukIAS9IwOsbYGpb4wTcaMebKCeRHmSbEdGpOztcnIjDFRgWpMZo1wUDyfonuBt1XLfVnPq3F8oX0K9oOwVJuv4cus+kGWPBvndMuBNiqOgvKp78uuUVOrdMgVdnJ0Ysi+Uer9q8102iyfScRYY79H6e/7JVx1quCzw=="

            Dim strissuerNr As String = "2"

            Dim strenableActiveActive As String = "yes"



            Dim strAgentUrl As String = String.Format("{0}{1}", ConfigurationManager.ConnectionStrings("PacodeAgentUrl").ConnectionString(), "pin-reissuance")
            Dim strurl As String = String.Format("{0}{1}", ConfigurationManager.ConnectionStrings("PacodeHost").ConnectionString(), "pins")


            Dim strhost As String = ConfigurationManager.ConnectionStrings("PacodeHost").ConnectionString()
            'strauthData = GenAuthdata(clientIds, clientSecretKey, version, pan, pin, expirydate, cvv2)

            Dim pocoObject = New With
            {.authData = strauthData,
                .issuerNr = strissuerNr,
                .seqNr = strseqNr,
                .customerId = strcustomerId,
                .mobileNr = strmobileNr,
                .userId = struserId
            }

            Dim json As String = JsonConvert.SerializeObject(pocoObject)
            '#Region Add Auth Info
            Dim HeaderUsername As String = System.Configuration.ConfigurationManager.ConnectionStrings("PacodeKeyID").ConnectionString()
            Dim Headerpwds As String = System.Configuration.ConfigurationManager.ConnectionStrings("PacodeKey").ConnectionString()

            Dim strDeptHeadUserName As String = NETClass.Classes.DecryptString(HeaderUsername)
            Dim strDeptHeadpwds As String = NETClass.Classes.DecryptString(Headerpwds)

            Dim strAuthToken = Convert.ToBase64String(Encoding.Default.GetBytes(String.Format("{0}:{1}", strDeptHeadUserName, strDeptHeadpwds)))


            Dim httpWebRequest As HttpWebRequest = HttpWebRequest.Create(strAgentUrl)


            httpWebRequest.ContentType = "application/json"

            httpWebRequest.Method = "POST"

            httpWebRequest.Headers.Add("Authorisation", strAuthToken)
            Using streamWriter = New StreamWriter(httpWebRequest.GetRequestStream())
                Dim ljson As Object = New With
                    {
                    .userId = struserId,
                    .bankCode = strbankCode,
                    .authData = strauthData,
                    .issuerNr = strissuerNr,
                    .accountType = version,
                    .expiryDate = expirydate,
                    .pan = pan,
                    .seqNr = "001",
                    .pin = pin,
                    .cvv2 = cvv2,
                    .url = strurl,
                    .clientId = clientIds,
                    .clientSecret = clientSecretKey,
                    .host = strhost,
                    .enableActiveActive = strenableActiveActive
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



            '#Region end
            Return Result
        End Function


        Public Shared Sub AddtoLogFile(Message As String, WebPage As String)
            Dim LogPath = System.Web.HttpContext.Current.Server.MapPath("~/log/")  ' ConfigurationManager.AppSettings["LogPath"].ToString();
            Dim filename = "Log_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt"
            Dim filepath = LogPath + filename

            If (File.Exists(filepath)) Then

                Using writer As StreamWriter = New StreamWriter(filepath, True)

                    writer.WriteLine("——————-START————-" + DateTime.Now)
                    writer.WriteLine("Source :" + WebPage)
                    writer.WriteLine(Message)
                    writer.WriteLine("——————-END————-" + DateTime.Now)
                End Using
            Else
                Dim writer As StreamWriter = File.CreateText(filepath)
                writer.WriteLine("——————-START————-" + DateTime.Now)
                writer.WriteLine("Source :" + WebPage)
                writer.WriteLine(Message)
                writer.WriteLine("——————-END————-" + DateTime.Now)
                writer.Close()
            End If

        End Sub



    End Class
End Namespace