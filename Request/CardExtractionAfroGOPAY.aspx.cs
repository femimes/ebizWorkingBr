using System.Linq;
using System.Web;

using System.IO;
using System.Data;
using System.Text;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Web.Security;

using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Xml;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Reflection;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1;
using System.Globalization;

public partial class Request_CardExtractionAfroGOPAY :  System.Web.UI.Page
{
    public string gUser = HttpContext.Current.User.Identity.Name;//HttpContext.Current.User.Identity.Name;
    public string luser = HttpContext.Current.User.Identity.Name;
    public string StrLocation = "";
    public string struser = "";

    public static void AddtoLogFile(string Message, string WebPage)
    {
        string LogPath = System.Web.HttpContext.Current.Server.MapPath("~/log/");// ConfigurationManager.AppSettings["LogPath"].ToString();
        string filename = "AfriLog_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
        string filepath = LogPath + filename;
        if (File.Exists(filepath))
        {
            using (StreamWriter writer = new StreamWriter(filepath, true))
            {
                writer.WriteLine("\u2014\u2014\u2014\u2014\u2014\u2014-START\u2014\u2014\u2014\u2014-" + DateTime.Now);
                writer.WriteLine("Source :" + WebPage);
                writer.WriteLine(Message);
                writer.WriteLine("\u2014\u2014\u2014\u2014\u2014\u2014-END\u2014\u2014\u2014\u2014-" + DateTime.Now);
            }
        }
        else
        {
            StreamWriter writer = File.CreateText(filepath);
            writer.WriteLine("\u2014\u2014\u2014\u2014\u2014\u2014-START\u2014\u2014\u2014\u2014-" + DateTime.Now);
            writer.WriteLine("Source :" + WebPage);
            writer.WriteLine(Message);
            writer.WriteLine("\u2014\u2014\u2014\u2014\u2014\u2014-END\u2014\u2014\u2014\u2014-" + DateTime.Now);
            writer.Close();
        }
    }
    public static void AddtoCardFile(string Message, string WebPage)
    {
        string LogPath = System.Web.HttpContext.Current.Server.MapPath("~/log/cardFile/");// ConfigurationManager.AppSettings["LogPath"].ToString();

            
        string filename = "AfriCardFile_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
        string filepath = LogPath + filename;
        if (File.Exists(filepath))
        {
            using (StreamWriter writer = new StreamWriter(filepath, true))
            {
                writer.WriteLine(Message);
            }
        }
        else
        {
            StreamWriter writer = File.CreateText(filepath);
            writer.WriteLine(Message);
            writer.Close();
        }
    }

    public string AESEncrpt(string data)
    {
        try
        {
            string aesKey = "tnWvYXRLaARMQrtQubsfzbaUcaRQBABV";
            byte[] iv = Encoding.UTF8.GetBytes("0123456789ABCDEF");
            byte[] key = Encoding.UTF8.GetBytes(aesKey);
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                byte[] encrypted;

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(data);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }

                return Convert.ToBase64String(encrypted);
            }
        }
        catch (Exception e)
        {
            throw new Exception("AES encryption failed", e);
        }
    }

    public Dictionary<string, object> ConvertObjectToDictionary(object obj)
    {
        if (obj == null)
        throw new ArgumentNullException(null);
        var dictionary = new Dictionary<string, object>();
        var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            var value = property.GetValue(obj);
            dictionary[property.Name] = value != null ? value.ToString() : string.Empty;
        }

        return dictionary;
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

 
    string GenerateCarinfo(string strAccountNo)
    {
        var key = "nbzjfdiehurgsxct";
        var iv = "sngtmqpfurxdbkwj";
        var CryptoKey = "tnWvYXRLaARMQrtQubsfzbaUcaRQBABV";
        var InitializationVectorKey = "MIIBCQKCAQCy5Oa9VnLvn6ZXos4bJN27909nxz+7M3PEGPpISHaE6lHLfDadnrp51FEnUyYVMfrS9q+3tRrGMmC1t1UxXlr7kpVHwNo1EQViz5VcL4zoiwxCbTIc2oRzFgQ6C9BGH6Xn2up4CaQfVzkRgIIz/YLTC6TxnFBzbmseosXhmWZzB2M/qRtB0m63FtHeKZlt995LOVoXE4RRW+uauDwJY+bojotpxLIHQlpmS2T331X6u+wURzQL/ovrcrWzhXwKq35GPXSAlXZOOhd9k6cmHsLM4YjjKfYGjP5ImhKISd21y6yhsnJiJoDuwOJwYaUJs6pHlqU0BUTv24G/5KqKD8gNAgMBAAE=";
        var EntityCode = "2211592356743287";
        var Bankcode = ConfigurationManager.ConnectionStrings["AfroGOPAYBankHostCode"].ConnectionString;
        var publickey = "MIIBCQKCAQCy5Oa9VnLvn6ZXos4bJN27909nxz+7M3PEGPpISHaE6lHLfDadnrp51FEnUyYVMfrS9q+3tRrGMmC1t1UxXlr7kpVHwNo1EQViz5VcL4zoiwxCbTIc2oRzFgQ6C9BGH6Xn2up4CaQfVzkRgIIz/YLTC6TxnFBzbmseosXhmWZzB2M/qRtB0m63FtHeKZlt995LOVoXE4RRW+uauDwJY+bojotpxLIHQlpmS2T331X6u+wURzQL/ovrcrWzhXwKq35GPXSAlXZOOhd9k6cmHsLM4YjjKfYGjP5ImhKISd21y6yhsnJiJoDuwOJwYaUJs6pHlqU0BUTv24G/5KqKD8gNAgMBAAE=";
        var BIN = "56403701";
        DataTable dt = new DataTable();

        string requestUrl = string.Format("https://192.234.10.197:443/reset");
        string strAuthozization = ""; string clientId = "3f1a5ca1-9c5d-4b1d-a185-ad316d214c93"; string ClientSecreat = "Kjw8Q~iI27upIcCQB4NL_hA~B2VwprrJuB3KmcjR";

        #region Section Validate Headers

        #endregion
        string str3 = "";

        requestUrl = string.Format("http://172.20.11.210:443/bft/cms/BookingOpenCard", new object[] { "profile", "otp", "code", "https://jlmbapps.jubileelifeng.com/BVNVAL/Authorize", "770640d0-9af3-4ea9-8faf-bd19858ce35c" });

        var httpWebRequest = (WebRequest)WebRequest.Create(requestUrl);
        string strapi_key = "MIIBCQKCAQCy5Oa9VnLvn6ZXos4bJN27909nxz+7M3PEGPpISHaE6lHLfDadnrp51FEnUyYVMfrS9q+3tRrGMmC1t1UxXlr7kpVHwNo1EQViz5VcL4zoiwxCbTIc2oRzFgQ6C9BGH6Xn2up4CaQfVzkRgIIz/YLTC6TxnFBzbmseosXhmWZzB2M/qRtB0m63FtHeKZlt995LOVoXE4RRW+uauDwJY+bojotpxLIHQlpmS2T331X6u+wURzQL/ovrcrWzhXwKq35GPXSAlXZOOhd9k6cmHsLM4YjjKfYGjP5ImhKISd21y6yhsnJiJoDuwOJwYaUJs6pHlqU0BUTv24G/5KqKD8gNAgMBAAE=";//Header AESKey
        string strAES_key = "tnWvYXRLaARMQrtQubsfzbaUcaRQBABV";//payload AESKey
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "POST";
        httpWebRequest.Headers.Add("api_key", strapi_key);
        AddtoLogFile(requestUrl, string.Format("{0} : Authrization => {1}:- {2}", "Book Request Url Request", "Request Booking", strAccountNo));
        var result = ""; string strErrCode = "";
        string EazyInstitutionCode = ConfigurationManager.ConnectionStrings["EazyInstitutionCode"].ConnectionString;

        string channel = "1"; string bank_number = "999998"; string account_number = strAccountNo; /*txtAccountNo.Text*/;
        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss"); string order_no = DateTime.Now.ToString("yyyyMMddHHmmssffff"); string code_type = "1"; decimal amount = 500;

        string stringA = ""; string crd_bin = "56403701";
        string supp_flg = "P";//Identifier of primary and secondary card: P-Primary,S - Secondary11Reserved for future use. Default value is \u2018P\u2019 whenleft null
        string opn_fee = "0";// Card open fee, Currency: NGN
        string an_fee = "0";// Annual fee, Currency: NGN
        string br_no = "0000";// "937";// Branch no.
        string apply_opr = "937";// Current Operator
        string crd_get_way = "C";// pickup method: C-Counter, M-Mail
        string iss_cnl_cd = "TLL";// Channel code, fill in any string to indicate request channel in bank's side
        string crd_typ = "00";// 00 Debit card,02 Prepaid card,03 Virtual card
        string id_no = order_no;//Yes ean..32 ID no.
        string id_typ = "01";//ID type(More in Appendix)
        string ci_no = "01";//Customer no.
        string mbl_no = "2348023378362";//Reserved phone number, prefix must be \u201c234\u201d
        string bvn = "02337836211";//en11 BVN
        string address = "Lagos";//  address, allowed special characters: \u201c@\u201d, \u201d,\u201d, \u201d-\u201c and space
        string email = "femi@hotmail.com";//email
        string fst_nm = "Testing";//First name 
        string lst_nm = "Tofa";//Last name 
        string inst_id = EazyInstitutionCode; // Institution ID
        string jrn_no = Guid.NewGuid().ToString("N");// order_no;//JournalId;
        DataTable dtAcctList = new DataTable();
        string strConchk = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnectSource"].ConnectionString;
        string strConeBiz = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString;

        SqlConnection Connchk = new SqlConnection(strConchk); DataSet chkds = new DataSet();

        SqlConnection ConnchkVal = new SqlConnection(strConchk); DataSet chkdsval = new DataSet();
        SqlCommand cmdchkval = new SqlCommand();
        SqlDataAdapter cmdAdapterchkval = new SqlDataAdapter("Sproc_NIPGetAccountAx", Connchk);

        cmdAdapterchkval.SelectCommand.CommandType = CommandType.StoredProcedure;
        cmdAdapterchkval.SelectCommand.Parameters.Add(new SqlParameter("@AccountNo", account_number));
        cmdAdapterchkval.Fill(dtAcctList);

        List<object> strAcctList = new List<object>(); string subaccts = ""; string ac_no = ""; string ac_typ = ""; string ccy = ""; string dft_flg = "";
        foreach (DataRow item in dtAcctList.Rows)
        {

            var pocoObject = (new
            {
                ac_no = AESEncrpt(item["AccountNo"].ToString()),
                ac_typ = "10",//item["accountType"],//account type\uff1a10: Savings;60: Checking 90: Shadow account(Each account Type has at most one account)
                ccy = item["currCode"],//Currency Type\uff1aNGN
                dft_flg = "Y" // item["currCode"],//Identifier of default account\uff1aY:Yes\uff1bN: No

            });

            ci_no = item["CusID"].ToString();
            bvn = item["BankVerificationNumber"].ToString();
            strAcctList.Add(pocoObject);
        }

        SqlConnection ConnchkCusInfo = new SqlConnection(strConchk); DataTable chkCusinfo = new DataTable();

        SqlDataAdapter cmdAdapterCusinfo = new SqlDataAdapter("SpCusMasterSelectOne", ConnchkCusInfo);
        cmdAdapterCusinfo.SelectCommand.CommandType = CommandType.StoredProcedure;
        cmdAdapterCusinfo.SelectCommand.Parameters.Add(new SqlParameter("@CusID", ci_no));

        cmdAdapterCusinfo.Fill(chkCusinfo);

        foreach (DataRow item in chkCusinfo.Rows)
        {

            fst_nm = item["FirstName"].ToString();
            lst_nm = item["LastName"].ToString();

        }

        SqlConnection ConnchkAddress = new SqlConnection(strConchk); DataTable chkaddress = new DataTable();

        SqlDataAdapter cmdAdapterAddress = new SqlDataAdapter("SpEntAddressSelectOneEntity", ConnchkAddress);
        cmdAdapterAddress.SelectCommand.CommandType = CommandType.StoredProcedure;
        cmdAdapterAddress.SelectCommand.Parameters.Add(new SqlParameter("@EntityID", ci_no));
        cmdAdapterAddress.SelectCommand.Parameters.Add(new SqlParameter("@EntityTypeCode", "01"));

        cmdAdapterAddress.Fill(chkaddress);

        foreach (DataRow item in chkaddress.Rows)
        {

            address = item["street"].ToString();
        }

        if (string.IsNullOrEmpty(address) == true)
        {
            address = "TOFA MFB";
        }
        SqlConnection ConnchkEmail = new SqlConnection(strConeBiz); DataTable chkEmail = new DataTable();

        SqlDataAdapter cmdAdapterEmail = new SqlDataAdapter("SpAXMobileEmail", ConnchkEmail);
        cmdAdapterEmail.SelectCommand.CommandType = CommandType.StoredProcedure;
        cmdAdapterEmail.SelectCommand.Parameters.Add(new SqlParameter("@AccountNo", account_number));
        cmdAdapterEmail.Fill(chkEmail);

        foreach (DataRow item in chkEmail.Rows)
        {

            email = item["EmailAddr"].ToString();
        }

        SqlConnection ConnchkSMS = new SqlConnection(strConeBiz); DataTable chkSMSno = new DataTable();

        SqlDataAdapter cmdAdapterSMS = new SqlDataAdapter("SpAXMobileNo", ConnchkSMS);
        cmdAdapterSMS.SelectCommand.CommandType = CommandType.StoredProcedure;
        cmdAdapterSMS.SelectCommand.Parameters.Add(new SqlParameter("@AccountNo", account_number));
        cmdAdapterSMS.Fill(chkSMSno);

        foreach (DataRow item in chkSMSno.Rows)
        {

            mbl_no = item["SMSNo"].ToString();
        }

        object body = new
        {

            crd_bin,
            supp_flg,
            opn_fee,
            an_fee,
            br_no,
            apply_opr,
            crd_get_way,
            iss_cnl_cd,
            crd_typ,
            acc_data_list = JsonConvert.SerializeObject(strAcctList.ToArray()),
            id_no = AESEncrpt(id_no),
            id_typ,
            ci_no = AESEncrpt(ci_no),
            mbl_no = AESEncrpt(mbl_no),
            bvn = AESEncrpt(bvn),
            address = AESEncrpt(address),
            email = AESEncrpt(email),
            fst_nm = AESEncrpt(fst_nm),
            lst_nm = AESEncrpt(lst_nm),
            inst_id,
            jrn_no,
        };

        Dictionary<string, object> sigObject = ConvertObjectToDictionary(body);

        string signature = GenerateSignature(sigObject, strAES_key);

        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        {
            string ljson = JsonConvert.SerializeObject(new
            {

                body,

                sign = signature.ToLower(CultureInfo.InvariantCulture)

            });

            streamWriter.Write(ljson);
            AddtoLogFile(ljson, string.Format("{0}", "Bookcard Request Payload Request"));
            streamWriter.Flush();
            streamWriter.Close();

        }
        try
        {

            using (WebResponse response = httpWebRequest.GetResponse())
            {
                Console.WriteLine("Won't get here");
                HttpWebResponse httpResponses = (HttpWebResponse)response;
                strErrCode = string.Format("Error code: {0}", httpResponses.StatusCode);
                using (Stream data = response.GetResponseStream())
                using (var reader = new StreamReader(data))
                {
                    result = reader.ReadToEnd();

                }

            }
        }
        catch (WebException ex)
        {
            using (WebResponse response = ex.Response)
            {
                HttpWebResponse httpResponses = (HttpWebResponse)response;
                strErrCode = string.Format("Error code: {0}", httpResponses.StatusCode);
                using (Stream data = response.GetResponseStream())
                using (var reader = new StreamReader(data))
                {
                    result = reader.ReadToEnd();

                }
            }
        }

        AddtoLogFile(result, string.Format("{0}", "Bookcard Request Response"));
        string respcode = "00"; string respdesc = "success";
        if (strErrCode == "Error code: OK")
        {
            respcode = "00";
            respdesc = "success";

        }
        else
        {
        }

        var serializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore
        };

        JavaScriptSerializer jssp = new JavaScriptSerializer();

        var jsonSmsInfo = JsonConvert.DeserializeObject(result, serializerSettings);
        dynamic jsonp = jssp.DeserializeObject(result);
        string cardNoEncrypted = ""; string cardNoMasked = "";

        cardNoEncrypted = jsonp["crd_no_cipher"];
        cardNoMasked = jsonp["crd_no_mask"];

        ////Response.Write("Decrypted String: - " + AesDecrypt(txtEncryptCypher.Text, "tnWvYXRLaARMQrtQubsfzbaUcaRQBABV") + "</br>");

        return cardNoEncrypted;
    }
    string GenerateCardAllinfo(string strAccountNo, string curCusID, string CurrCode, string BVN, string Ac_type, string PhoneNo, string Email, string FirstName, string LastName)
    {
        var key = "nbzjfdiehurgsxct";
        var iv = "sngtmqpfurxdbkwj";
        var CryptoKey = "tnWvYXRLaARMQrtQubsfzbaUcaRQBABV";
        var InitializationVectorKey = "MIIBCQKCAQCy5Oa9VnLvn6ZXos4bJN27909nxz+7M3PEGPpISHaE6lHLfDadnrp51FEnUyYVMfrS9q+3tRrGMmC1t1UxXlr7kpVHwNo1EQViz5VcL4zoiwxCbTIc2oRzFgQ6C9BGH6Xn2up4CaQfVzkRgIIz/YLTC6TxnFBzbmseosXhmWZzB2M/qRtB0m63FtHeKZlt995LOVoXE4RRW+uauDwJY+bojotpxLIHQlpmS2T331X6u+wURzQL/ovrcrWzhXwKq35GPXSAlXZOOhd9k6cmHsLM4YjjKfYGjP5ImhKISd21y6yhsnJiJoDuwOJwYaUJs6pHlqU0BUTv24G/5KqKD8gNAgMBAAE=";
        var EntityCode = "2211592356743287";
        var Bankcode = "937";
        var publickey = "MIIBCQKCAQCy5Oa9VnLvn6ZXos4bJN27909nxz+7M3PEGPpISHaE6lHLfDadnrp51FEnUyYVMfrS9q+3tRrGMmC1t1UxXlr7kpVHwNo1EQViz5VcL4zoiwxCbTIc2oRzFgQ6C9BGH6Xn2up4CaQfVzkRgIIz/YLTC6TxnFBzbmseosXhmWZzB2M/qRtB0m63FtHeKZlt995LOVoXE4RRW+uauDwJY+bojotpxLIHQlpmS2T331X6u+wURzQL/ovrcrWzhXwKq35GPXSAlXZOOhd9k6cmHsLM4YjjKfYGjP5ImhKISd21y6yhsnJiJoDuwOJwYaUJs6pHlqU0BUTv24G/5KqKD8gNAgMBAAE=";
        var BIN = "56403701";
        DataTable dt = new DataTable();

        string requestUrl = string.Format("https://192.234.10.197:443/reset");
        string strAuthozization = ""; string clientId = "3f1a5ca1-9c5d-4b1d-a185-ad316d214c93"; string ClientSecreat = "Kjw8Q~iI27upIcCQB4NL_hA~B2VwprrJuB3KmcjR";

        #region Section Validate Headers

        #endregion
        string str3 = "";

        requestUrl = string.Format("http://172.20.11.210:443/bft/cms/BookingOpenCard", new object[] { "profile", "otp", "code", "https://jlmbapps.jubileelifeng.com/BVNVAL/Authorize", "770640d0-9af3-4ea9-8faf-bd19858ce35c" });

        var httpWebRequest = (WebRequest)WebRequest.Create(requestUrl);
        string strapi_key = "MIIBCQKCAQCy5Oa9VnLvn6ZXos4bJN27909nxz+7M3PEGPpISHaE6lHLfDadnrp51FEnUyYVMfrS9q+3tRrGMmC1t1UxXlr7kpVHwNo1EQViz5VcL4zoiwxCbTIc2oRzFgQ6C9BGH6Xn2up4CaQfVzkRgIIz/YLTC6TxnFBzbmseosXhmWZzB2M/qRtB0m63FtHeKZlt995LOVoXE4RRW+uauDwJY+bojotpxLIHQlpmS2T331X6u+wURzQL/ovrcrWzhXwKq35GPXSAlXZOOhd9k6cmHsLM4YjjKfYGjP5ImhKISd21y6yhsnJiJoDuwOJwYaUJs6pHlqU0BUTv24G/5KqKD8gNAgMBAAE=";//Header AESKey
        string strAES_key = "tnWvYXRLaARMQrtQubsfzbaUcaRQBABV";//payload AESKey
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "POST";
        httpWebRequest.Headers.Add("api_key", strapi_key);
        AddtoLogFile(requestUrl, string.Format("{0} : Authrization => {1}:- {2}", "Book Request Url Request", "Request Booking", strAccountNo));
        var result = ""; string strErrCode = "";
        string EazyInstitutionCode = ConfigurationManager.ConnectionStrings["EazyInstitutionCode"].ConnectionString;

        string channel = "1"; string bank_number = "999998"; string account_number = strAccountNo; /*txtAccountNo.Text*/;
        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss"); string order_no = DateTime.Now.ToString("yyyyMMddHHmmssffff"); string code_type = "1"; decimal amount = 500;

        string stringA = ""; string crd_bin = "56403701";
        string supp_flg = "P";//Identifier of primary and secondary card: P-Primary,S - Secondary11Reserved for future use. Default value is \u2018P\u2019 whenleft null
        string opn_fee = "0";// Card open fee, Currency: NGN
        string an_fee = "0";// Annual fee, Currency: NGN
        string br_no = "0000";// "937";// Branch no.
        string apply_opr = "937";// Current Operator
        string crd_get_way = "C";// pickup method: C-Counter, M-Mail
        string iss_cnl_cd = "TLL";// Channel code, fill in any string to indicate request channel in bank's side
        string crd_typ = "00";// 00 Debit card,02 Prepaid card,03 Virtual card
        string id_no = order_no;//Yes ean..32 ID no.
        string id_typ = "01";//ID type(More in Appendix)
        string ci_no = curCusID;//Customer no.
        string mbl_no = PhoneNo;//Reserved phone number, prefix must be \u201c234\u201d
        string bvn = BVN;//en11 BVN
        string address = "Lagos";//  address, allowed special characters: \u201c@\u201d, \u201d,\u201d, \u201d-\u201c and space
        string email = Email;//email
        string fst_nm = FirstName;//First name 
        string lst_nm = LastName;//Last name 
        string inst_id = EazyInstitutionCode; // Institution ID
        string jrn_no = Guid.NewGuid().ToString("N");// order_no;//JournalId;
        DataTable dtAcctList = new DataTable();
        string strConchk = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnectSource"].ConnectionString;
        string strConeBiz = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString;

        ////SqlDataAdapter cmdAdapterchkval = new SqlDataAdapter("SpAcctMasterSelectOne", Connchk);

        List<object> strAcctList = new List<object>(); string subaccts = ""; string ac_no = ""; string ac_typ = ""; string ccy = ""; string dft_flg = "";
        {

            var pocoObject = (new
            {
                ac_no = AESEncrpt(account_number),
                ac_typ = "10",//item["accountType"],//account type\uff1a10: Savings;60: Checking 90: Shadow account(Each account Type has at most one account)
                ccy = CurrCode,//Currency Type\uff1aNGN
                dft_flg = "Y" // item["currCode"],//Identifier of default account\uff1aY:Yes\uff1bN: No

            });

            ci_no = curCusID;
            bvn = BVN;
            strAcctList.Add(pocoObject);
        }

        SqlConnection ConnchkAddress = new SqlConnection(strConchk); DataTable chkaddress = new DataTable();

        SqlDataAdapter cmdAdapterAddress = new SqlDataAdapter("SpEntAddressSelectOneEntity", ConnchkAddress);
        cmdAdapterAddress.SelectCommand.CommandType = CommandType.StoredProcedure;
        cmdAdapterAddress.SelectCommand.Parameters.Add(new SqlParameter("@EntityID", ci_no));
        cmdAdapterAddress.SelectCommand.Parameters.Add(new SqlParameter("@EntityTypeCode", "01"));

        cmdAdapterAddress.Fill(chkaddress);

        foreach (DataRow item in chkaddress.Rows)
        {

            address = item["street"].ToString();
        }

        if (string.IsNullOrEmpty(address) == true)
        {
            address = "TOFA MFB";
        }

        object body = new
        {

            crd_bin,
            supp_flg,
            opn_fee,
            an_fee,
            br_no,
            apply_opr,
            crd_get_way,
            iss_cnl_cd,
            crd_typ,
            acc_data_list = JsonConvert.SerializeObject(strAcctList.ToArray()),
            id_no = AESEncrpt(id_no),
            id_typ,
            ci_no = AESEncrpt(ci_no),
            mbl_no = AESEncrpt(mbl_no),
            bvn = AESEncrpt(bvn),
            address = AESEncrpt(address),
            email = AESEncrpt(email),
            fst_nm = AESEncrpt(fst_nm),
            lst_nm = AESEncrpt(lst_nm),
            inst_id,
            jrn_no,
        };

        Dictionary<string, object> sigObject = ConvertObjectToDictionary(body);

        string signature = GenerateSignature(sigObject, strAES_key);

        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        {
            string ljson = JsonConvert.SerializeObject(new
            {

                body,

                sign = signature.ToLower(CultureInfo.InvariantCulture)

            });

            streamWriter.Write(ljson);
            AddtoLogFile(ljson, string.Format("{0}", "Bookcard Request Payload Request"));
            streamWriter.Flush();
            streamWriter.Close();

        }
        try
        {

            using (WebResponse response = httpWebRequest.GetResponse())
            {
                Console.WriteLine("Won't get here");
                HttpWebResponse httpResponses = (HttpWebResponse)response;
                strErrCode = string.Format("Error code: {0}", httpResponses.StatusCode);
                using (Stream data = response.GetResponseStream())
                using (var reader = new StreamReader(data))
                {
                    result = reader.ReadToEnd();

                }

            }
        }
        catch (WebException ex)
        {
            using (WebResponse response = ex.Response)
            {
                HttpWebResponse httpResponses = (HttpWebResponse)response;
                strErrCode = string.Format("Error code: {0}", httpResponses.StatusCode);
                using (Stream data = response.GetResponseStream())
                using (var reader = new StreamReader(data))
                {
                    result = reader.ReadToEnd();

                }
            }
        }

        AddtoLogFile(result, string.Format("{0}", "Bookcard Request Response"));
        string respcode = "00"; string respdesc = "success";
        if (strErrCode == "Error code: OK")
        {
            respcode = "00";
            respdesc = "success";

        }
        else
        {
        }

        var serializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore
        };

        JavaScriptSerializer jssp = new JavaScriptSerializer();

        var jsonSmsInfo = JsonConvert.DeserializeObject(result, serializerSettings);
        dynamic jsonp = jssp.DeserializeObject(result);
        string cardNoEncrypted = ""; string cardNoMasked = "";

        cardNoEncrypted = jsonp["crd_no_cipher"];
        cardNoMasked = jsonp["crd_no_mask"];

        ////Response.Write("Decrypted String: - " + AesDecrypt(txtEncryptCypher.Text, "tnWvYXRLaARMQrtQubsfzbaUcaRQBABV") + "</br>");

        return cardNoEncrypted;
    }

    string ActivateCardinfo(string cardNo)
    {

        var key = "nbzjfdiehurgsxct";
        var iv = "sngtmqpfurxdbkwj";
        var CryptoKey = "tnWvYXRLaARMQrtQubsfzbaUcaRQBABV";
        var InitializationVectorKey = "MIIBCQKCAQCy5Oa9VnLvn6ZXos4bJN27909nxz+7M3PEGPpISHaE6lHLfDadnrp51FEnUyYVMfrS9q+3tRrGMmC1t1UxXlr7kpVHwNo1EQViz5VcL4zoiwxCbTIc2oRzFgQ6C9BGH6Xn2up4CaQfVzkRgIIz/YLTC6TxnFBzbmseosXhmWZzB2M/qRtB0m63FtHeKZlt995LOVoXE4RRW+uauDwJY+bojotpxLIHQlpmS2T331X6u+wURzQL/ovrcrWzhXwKq35GPXSAlXZOOhd9k6cmHsLM4YjjKfYGjP5ImhKISd21y6yhsnJiJoDuwOJwYaUJs6pHlqU0BUTv24G/5KqKD8gNAgMBAAE=";
        var EntityCode = "2211592356743287";
        var Bankcode = "937";
        var publickey = "MIIBCQKCAQCy5Oa9VnLvn6ZXos4bJN27909nxz+7M3PEGPpISHaE6lHLfDadnrp51FEnUyYVMfrS9q+3tRrGMmC1t1UxXlr7kpVHwNo1EQViz5VcL4zoiwxCbTIc2oRzFgQ6C9BGH6Xn2up4CaQfVzkRgIIz/YLTC6TxnFBzbmseosXhmWZzB2M/qRtB0m63FtHeKZlt995LOVoXE4RRW+uauDwJY+bojotpxLIHQlpmS2T331X6u+wURzQL/ovrcrWzhXwKq35GPXSAlXZOOhd9k6cmHsLM4YjjKfYGjP5ImhKISd21y6yhsnJiJoDuwOJwYaUJs6pHlqU0BUTv24G/5KqKD8gNAgMBAAE=";
        var BIN = "56403701";

        string defaultPwds = "1234";
        DataTable dt = new DataTable();

        string requestUrl = string.Format("https://192.234.10.197:443/reset");
        string strAuthozization = ""; string clientId = "3f1a5ca1-9c5d-4b1d-a185-ad316d214c93"; string ClientSecreat = "Kjw8Q~iI27upIcCQB4NL_hA~B2VwprrJuB3KmcjR";

        #region Section Validate Headers

        #endregion
        string str3 = "";

        requestUrl = string.Format("http://172.20.11.210:443/bft/cms/cardActivation", new object[] { "profile", "otp", "code", "https://jlmbapps.jubileelifeng.com/BVNVAL/Authorize", "770640d0-9af3-4ea9-8faf-bd19858ce35c" });

        var httpWebRequest = (WebRequest)WebRequest.Create(requestUrl);
        string strapi_key = "MIIBCQKCAQCy5Oa9VnLvn6ZXos4bJN27909nxz+7M3PEGPpISHaE6lHLfDadnrp51FEnUyYVMfrS9q+3tRrGMmC1t1UxXlr7kpVHwNo1EQViz5VcL4zoiwxCbTIc2oRzFgQ6C9BGH6Xn2up4CaQfVzkRgIIz/YLTC6TxnFBzbmseosXhmWZzB2M/qRtB0m63FtHeKZlt995LOVoXE4RRW+uauDwJY+bojotpxLIHQlpmS2T331X6u+wURzQL/ovrcrWzhXwKq35GPXSAlXZOOhd9k6cmHsLM4YjjKfYGjP5ImhKISd21y6yhsnJiJoDuwOJwYaUJs6pHlqU0BUTv24G/5KqKD8gNAgMBAAE=";//Header AESKey
        string strAES_key = "tnWvYXRLaARMQrtQubsfzbaUcaRQBABV";//payload AESKey
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "POST";
        httpWebRequest.Headers.Add("api_key", strapi_key);
        AddtoLogFile(requestUrl, string.Format("{0} : Authrization => {1}", "Card Activation Url Request", "Card Activation"));
        var result = ""; string strErrCode = "";
        string EazyInstitutionCode = ConfigurationManager.ConnectionStrings["EazyInstitutionCode"].ConnectionString;

        string channel = "1"; string bank_number = "999998"; string account_number = ""; /*txtAccountNo.Text*/;
        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss"); string order_no = DateTime.Now.ToString("yyyyMMddHHmmssff"); string code_type = "1"; decimal amount = 500;

        string stringA = ""; string crd_bin = "56403701";
        string supp_flg = "P";//Identifier of primary and secondary card: P-Primary,S - Secondary11Reserved for future use. Default value is \u2018P\u2019 whenleft null
        string opn_fee = "0";// Card open fee, Currency: NGN
        string an_fee = "0";// Annual fee, Currency: NGN
        string br_no = "0000";// "937";// Branch no.
        string apply_opr = "937";// Current Operator
        string crd_get_way = "C";// pickup method: C-Counter, M-Mail
        string iss_cnl_cd = "TLL";// Channel code, fill in any string to indicate request channel in bank's side
        string crd_typ = "00";// 00 Debit card,02 Prepaid card,03 Virtual card
        string id_no = "262626626262626262";//Yes ean..32 ID no.
        string id_typ = "01";//ID type(More in Appendix)
        string ci_no = "01";//Customer no.
        string mbl_no = "2348023378362";//Reserved phone number, prefix must be \u201c234\u201d
        string bvn = "02337836211";//en11 BVN
        string address = "Lagos";//  address, allowed special characters: \u201c@\u201d, \u201d,\u201d, \u201d-\u201c and space
        string email = "femi@hotmail.com";//email
        string fst_nm = "Testing";//First name 
        string lst_nm = "Tofa";//Last name 
        string inst_id = EazyInstitutionCode; // Institution ID
        string jrn_no = Guid.NewGuid().ToString("N");// order_no;//JournalId;
        string rmk = "Activate new card";
        string pay_pswd = PinEcryption(defaultPwds, cardNo);
        string crd_no = AESEncrpt(cardNo);
        DataTable dtAcctList = new DataTable();

        object body = new
        {
            crd_no,
            pay_pswd,
            iss_cnl_cd,
            br_no,
            rmk,
            apply_opr,
            inst_id,
            jrn_no

        };

        Dictionary<string, object> sigObject = ConvertObjectToDictionary(body);

        string signature = GenerateSignature(sigObject, strAES_key);

        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        {
            string ljson = JsonConvert.SerializeObject(new
            {

                body,

                sign = signature.ToLower(CultureInfo.InvariantCulture)

            });

            streamWriter.Write(ljson);
            AddtoLogFile(ljson, string.Format("{0}", "Card Activation Payload Request"));
            streamWriter.Flush();
            streamWriter.Close();

        }
        try
        {

            using (WebResponse response = httpWebRequest.GetResponse())
            {
                Console.WriteLine("Won't get here");
                HttpWebResponse httpResponses = (HttpWebResponse)response;
                strErrCode = string.Format("Error code: {0}", httpResponses.StatusCode);
                using (Stream data = response.GetResponseStream())
                using (var reader = new StreamReader(data))
                {
                    result = reader.ReadToEnd();

                }

            }
        }
        catch (WebException ex)
        {
            using (WebResponse response = ex.Response)
            {
                HttpWebResponse httpResponses = (HttpWebResponse)response;
                strErrCode = string.Format("Error code: {0}", httpResponses.StatusCode);
                using (Stream data = response.GetResponseStream())
                using (var reader = new StreamReader(data))
                {
                    result = reader.ReadToEnd();

                }
            }
        }

        AddtoLogFile(result, string.Format("{0}", "Card Activation Response"));
        string respcode = "00"; string respdesc = "success";
        if (strErrCode == "Error code: OK")
        {
            respcode = "00";
            respdesc = "success";

        }
        else
        {
        }

        var serializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore
        };

        JavaScriptSerializer jssp = new JavaScriptSerializer();

        var jsonSmsInfo = JsonConvert.DeserializeObject(result, serializerSettings);
        dynamic jsonp = jssp.DeserializeObject(result);
        string ActivationTime = ""; string cardNoMasked = "";

        ActivationTime = jsonp["activation_time"];
        return ActivationTime;
    }

    private string Sha256(string input)
    {
        using (var sha256 = SHA256.Create())
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha256.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }

    public string GenerateSignature(Dictionary<string, object> parameters, string aesKey)
    {
        var signBuilder = new StringBuilder();
        var keyList = new List<string>(parameters.Keys);
        keyList.Sort();
        foreach (var key in keyList)
        {
            var value = parameters[key];
            if (value == null)
            {
                continue;
            }
            if (value is string && string.IsNullOrWhiteSpace((string)value))
            {
                continue;
            }
            if (!(value is string) && !(value is int) && !(value is long) && !(value is double) && !(value is decimal))
            {
                value = JsonConvert.SerializeObject(value);
            }
            signBuilder.Append(key).Append("=").Append(value);
            signBuilder.Append("&");
        }
        signBuilder.Append("salt").Append("=").Append(aesKey);

        return Sha256(signBuilder.ToString());
    }

    public string AesDecrypt(string encryptedData, string aesKey)
    {
        try
        {
            var AesIV = "0123456789ABCDEF";
            byte[] iv = Encoding.UTF8.GetBytes(AesIV);
            byte[] key = Encoding.UTF8.GetBytes(aesKey);
            byte[] cipherText = Convert.FromBase64String(encryptedData);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            throw new Exception("AES decryption failed", e);
        }
    }

    // Pin encryption started

    public string PinEcryption(string PIN, string PAN)
    {
        string response = string.Empty;

        byte[] pinBlockData = Format0(PIN, PAN);
        string pubkey = "MIIBCQKCAQCp35WWuuYrHeKpvtLysAPYOIafRqaGQvDV+aU6BS/U0JTcrPGlxTYHaMNwkvR7bdfhcqxuFL8z5J6lkqg+9wZ4EtsC/4ZEgNbaoejWFO6y958SOmJdVgSV3o49Xpezhiy3jtn8XHGU+rYjpIpNvUaprV0HA0pb/AGLoW60YTq76KytSDtyoWPHYZHJsOjRVsjSsA4wNbSDQyzofj4NbMER35Ra99jDcoiDrhkt2D6TFIQ9LjgRIYrP1sxadVprNpOfWk09B4axUE+/CslSbbxtVg/gQuevN/FAk6ZsMj6h+FFgtTTI9VrCdO6/bv0Tv3vF/vOMKAZANDgV9TQqqQmZAgMBAAE=";// _config.GetValue<string>("AfriGo:PinKey");
        byte[] encryptedData = EncryptWithPublicKey(pinBlockData, pubkey);

        // Convert encrypted data to Base64 string for readability
        string encode = Convert.ToBase64String(encryptedData);

        return encode;
    }

    static byte[] EncryptWithPublicKey(byte[] dataToEncrypt, string publicKey)
    {
        byte[] publicKeyBytes = Convert.FromBase64String(publicKey);

        //// Parse the public key using BouncyCastle

        return null;
    }

    static byte[] Format0(string PIN, string PAN)
    {
        string PINField = "0" + PIN.Length.ToString("X") + PIN.PadRight(14, 'F');
        string PANWithoutCheckDigit = PAN.Substring(0, PAN.Length - 1);
        string PANField = "0000" + (PANWithoutCheckDigit.Length > 12 ? PANWithoutCheckDigit.Substring(PANWithoutCheckDigit.Length - 12) : PANWithoutCheckDigit.PadLeft(12, '0'));
        byte[] PINFieldByteArray = DecodeHex(PINField);
        byte[] PANFieldByteArray = DecodeHex(PANField);
        byte[] PINBlockByteArray = new byte[8];
        for (int i = 0; i < 8; i++)
        {
            PINBlockByteArray[i] = (byte)(PINFieldByteArray[i] ^ PANFieldByteArray[i]);
        }
        return PINBlockByteArray;
    }

    static byte[] DecodeHex(string hexStr)
    {
        return string.IsNullOrEmpty(hexStr) ? null : DecodeHex2(hexStr.ToCharArray());
    }

    static byte[] DecodeHex2(char[] hexData)
    {
        int len = hexData.Length;
        if ((len & 1) != 0)
        {
            throw new Exception("Odd number of characters.");
        }

        byte[] outBytes = new byte[len >> 1];
        int i = 0;

        for (int j = 0; j < len; ++i)
        {
            int f = ToDigit(hexData[j], j) << 4;
            ++j;
            f |= ToDigit(hexData[j], j);
            ++j;
            outBytes[i] = (byte)(f & 0xFF);
        }

        return outBytes;
    }
    static int ToDigit(char ch, int index)
    {
        int digit = Convert.ToInt32(ch.ToString(), 16);
        if (digit == -1)
        {
            throw new Exception("Illegal hexadecimal character " + ch + " at index " + index);
        }
        return digit;
    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        RunAction("Generate cards", delegate { Button3_Work(sender, e); });
    }

    private void Button3_Work(object sender, EventArgs e)
    {
        char[] delimiterChars = { ',' };
        string[] words = null;// txtAcctList.Text.Split(delimiterChars);
      
        string strConchk = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnectSource"].ConnectionString;
        string strConeBiz = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString;

        SqlConnection Connchk = new SqlConnection(strConchk); DataSet chkds = new DataSet();

        SqlConnection ConnchkVal = new SqlConnection(strConchk); DataSet chkdsval = new DataSet();
        SqlCommand cmdchkval = new SqlCommand();

        SqlConnection Connbiz = new SqlConnection(strConeBiz); 
        SqlCommand cmdbiz = new SqlCommand();

        DataTable dtAcctListFrows = new DataTable();
            foreach (var key in SelectedKeys())
        {
            if (key != null)
            {
                DataTable dtAcctList = new DataTable();
                string BatchrefNo = RowValue(key, "RefNo").ToString();

                SqlDataAdapter cmdAdapterchkval = new SqlDataAdapter(string.Format("spAfrigoPayCardAcctsRefNo '{0}'", BatchrefNo), strConchk);

                cmdAdapterchkval.Fill(dtAcctList);

                foreach (DataRow item in dtAcctList.Rows)
                {

               

                string panNo = "";
                string panNoEncrypted = "";string panNoMasked = "";
                string strAccountNo = ""; string smsNo = "2348030000000";
                strAccountNo = item["AccountNo"].ToString();
                if (string.IsNullOrEmpty(item["SMSNo"].ToString()) == false)
                {
                    smsNo = item["SMSNo"].ToString();
                }

                    #region Add Auth Info
                    string requestUrl = "";
                    string AfroGOPAYHost = System.Configuration.ConfigurationManager.ConnectionStrings["AfroGOPAYHost"].ConnectionString;
                    requestUrl = string.Format("{0}/{1}", AfroGOPAYHost, "Cards/BookCard");
                    string HeaderUsername = System.Configuration.ConfigurationManager.ConnectionStrings["AfroGOPAYID"].ConnectionString;
                    string Headerpwds = System.Configuration.ConfigurationManager.ConnectionStrings["AfroGOPAYKey"].ConnectionString;

                    string strDeptHeadUserName = NETClass.Classes.DecryptString(HeaderUsername);
                    string strDeptHeadpwds = NETClass.Classes.DecryptString(Headerpwds);

                    string strAuthToken = Convert.ToBase64String(Encoding.Default.GetBytes(string.Format("{0}:{1}", strDeptHeadUserName, strDeptHeadpwds)));

                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUrl);

                    httpWebRequest.ContentType = "application/json";

                    httpWebRequest.Method = "POST";
                    httpWebRequest.Headers.Add("AuthToken", strAuthToken);

                    #endregion

                    #region Section generateCard info
                    string RequestTimeStamp = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm:ss+0000");
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string ljson = new JavaScriptSerializer().Serialize(new
                        {

                            AccountNo = strAccountNo,
                            CusID = item["CusID"].ToString(),
                            CurrCode = item["CurrCode"].ToString(),
                            BankVerificationNumber = item["BankVerificationNumber"].ToString(),
                            CardType ="10",
                            smsNo,
                            EmailAddr = item["EmailAddr"].ToString(),
                            FirstName = item["FirstName"].ToString(),
                            LastName = item["LastName"].ToString()
                         

                        });
                        streamWriter.Write(ljson);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    var result = "";
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        result = streamReader.ReadToEnd();
                    }
                    httpResponse.Close();

                    var serializerSettings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        DefaultValueHandling = DefaultValueHandling.Ignore
                    };
                    try
                    {
                        JavaScriptSerializer jssp = new JavaScriptSerializer();

                        var jsonSmsInfo = JsonConvert.DeserializeObject(result, serializerSettings);
                        dynamic jsonp = jssp.DeserializeObject(result);

                        string respcode = ""; string data = ""; string respdesc = "";
                        AddtoLogFile(result, "Card CMS Response");
                        respcode = jsonp["responseCode"];
                        respdesc = jsonp["responseText"];

                        #endregion

                    
                        if (respcode == "00")
                        {
                             panNoEncrypted = jsonp["statusMessage"]["crd_no_cipher"];
                            panNoMasked = jsonp["statusMessage"]["crd_no_mask"];
                            panNo = jsonp["data"];
                        }
                    }
                    catch (Exception)
                    {

                    }

 

                    if (panNoEncrypted != "")
                {

                    // Decrypt cardinfo

                    // section Activate card info

                    string cardfileRec = string.Format("{0},{1}", panNo, strAccountNo);

                    AddtoCardFile(cardfileRec, "CardfileInfo");
 
                        SqlDataAdapter cmdAdapterAcctUpd = new SqlDataAdapter("SpUpdateCardReqRefItem", Connbiz);

                        cmdAdapterAcctUpd.SelectCommand.CommandType = CommandType.StoredProcedure;
                        cmdAdapterAcctUpd.SelectCommand.Parameters.Add(new SqlParameter("@eBatRefNo", BatchrefNo));
                        cmdAdapterAcctUpd.SelectCommand.Parameters.Add(new SqlParameter("@accountNo", strAccountNo));
                        cmdAdapterAcctUpd.SelectCommand.Parameters.Add(new SqlParameter("@panno", panNoMasked));
                        cmdAdapterAcctUpd.SelectCommand.Parameters.Add(new SqlParameter("@trackId", panNoEncrypted));
                        cmdAdapterAcctUpd.SelectCommand.Parameters.Add(new SqlParameter("@status", 5));
                        cmdAdapterAcctUpd.Fill(dtAcctList);

                    }

                }

            }

            // write to file the Cardfile
        }
        ctrlGrid.DataBind();
    }

    protected void Button4_Click(object sender, EventArgs e)
    {
        RunAction("Drop selection", delegate { Button4_Work(sender, e); });
    }

    private void Button4_Work(object sender, EventArgs e)
    {

        string strlblerror = "";
        try
        {
            luser = HttpContext.Current.User.Identity.Name;
            StrLocation = luser.Substring(luser.LastIndexOf("?") + 1);
            struser = luser.Substring(0, luser.LastIndexOf("?"));
            gUser = struser;
        }
        catch (Exception ex)
        {
            Literal1.Text = "User section expired";
            return;
        }

        char[] delimiterChars = { ',' };
        string[] words = null;// txtAcctList.Text.Split(delimiterChars);

        string strConchk = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnectSource"].ConnectionString;
        string strConeBiz = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString;

        SqlConnection Connchk = new SqlConnection(strConchk); DataSet chkds = new DataSet();

        SqlConnection ConnchkVal = new SqlConnection(strConchk); DataSet chkdsval = new DataSet();
        SqlCommand cmdchkval = new SqlCommand();

        SqlConnection Connbiz = new SqlConnection(strConeBiz);
        SqlCommand cmdbiz = new SqlCommand();

        DataTable dtAcctListFrows = new DataTable();
        foreach (var key in SelectedKeys())
        {
            if (key != null)
            {
              
                DataTable dtAcctList = new DataTable();
                string BatchrefNo = RowValue(key, "RefNo").ToString();

                ////cmdAdapterchkval.SelectCommand.CommandType = CommandType.StoredProcedure;

                cmdbiz.CommandText = "Sproc_ApproveCardRequestUpd";
                cmdbiz.Connection = Connbiz;
                cmdbiz.CommandType = CommandType.StoredProcedure;

                Connbiz.Open();
                cmdbiz.Parameters.Add((new SqlParameter("@eBatRefNo", BatchrefNo)));
                cmdbiz.Parameters.Add((new SqlParameter("@ApprovingOfficer", gUser)));
                cmdbiz.Parameters.Add(new SqlParameter("@status", 99));

                cmdbiz.ExecuteNonQuery();

                Connbiz.Close();
               

            }

            // write to file the Cardfile
        }
    }

    protected void Button5_Click(object sender, EventArgs e)
    {
        RunAction("Generate and activate cards", delegate { Button5_Work(sender, e); });
    }

    private void Button5_Work(object sender, EventArgs e)
    {
        char[] delimiterChars = { ',' };
        string[] words = null;// txtAcctList.Text.Split(delimiterChars);

        string strConchk = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnectSource"].ConnectionString;
        string strConeBiz = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString;

        SqlConnection Connchk = new SqlConnection(strConchk); DataSet chkds = new DataSet();

        SqlConnection ConnchkVal = new SqlConnection(strConchk); DataSet chkdsval = new DataSet();
        SqlCommand cmdchkval = new SqlCommand();

        SqlConnection Connbiz = new SqlConnection(strConeBiz);
        SqlCommand cmdbiz = new SqlCommand();

        DataTable dtAcctListFrows = new DataTable();
        foreach (var key in SelectedKeys())
        {
            if (key != null)
            {
                DataTable dtAcctList = new DataTable();
                string BatchrefNo = RowValue(key, "RefNo").ToString();

                SqlDataAdapter cmdAdapterchkval = new SqlDataAdapter(string.Format("spAfrigoPayCardAcctsRefNo '{0}'", BatchrefNo), strConchk);

                cmdAdapterchkval.Fill(dtAcctList);

                foreach (DataRow item in dtAcctList.Rows)
                {

                    string panNo = "";
                    string panNoEncrypted = "";string panNoMasked = "";
                    string strAccountNo = ""; string smsNo = "2348030000000";
                    strAccountNo = item["AccountNo"].ToString();
                    if (string.IsNullOrEmpty(item["SMSNo"].ToString()) == false)
                    {
                        smsNo = item["SMSNo"].ToString();
                    }

                    #region Add Auth Info
                    string requestUrl = "";
                    string AfroGOPAYHost = System.Configuration.ConfigurationManager.ConnectionStrings["AfroGOPAYHost"].ConnectionString;
                    requestUrl = string.Format("{0}/{1}", AfroGOPAYHost, "Cards/BookCard");
                    string HeaderUsername = System.Configuration.ConfigurationManager.ConnectionStrings["AfroGOPAYID"].ConnectionString;
                    string Headerpwds = System.Configuration.ConfigurationManager.ConnectionStrings["AfroGOPAYKey"].ConnectionString;

                    string strDeptHeadUserName = NETClass.Classes.DecryptString(HeaderUsername);
                    string strDeptHeadpwds = NETClass.Classes.DecryptString(Headerpwds);

                    string strAuthToken = Convert.ToBase64String(Encoding.Default.GetBytes(string.Format("{0}:{1}", strDeptHeadUserName, strDeptHeadpwds)));

                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUrl);

                    httpWebRequest.ContentType = "application/json";

                    httpWebRequest.Method = "POST";
                    httpWebRequest.Headers.Add("AuthToken", strAuthToken);

                    #endregion

                    #region Section generateCard info
                    string RequestTimeStamp = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm:ss+0000");
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string ljson = new JavaScriptSerializer().Serialize(new
                        {

                            AccountNo = strAccountNo,
                            CusID = item["CusID"].ToString(),
                            CurrCode = item["CurrCode"].ToString(),
                            BankVerificationNumber = item["BankVerificationNumber"].ToString(),
                            CardType = "10",
                            smsNo,
                            EmailAddr = item["EmailAddr"].ToString(),
                            FirstName = item["FirstName"].ToString(),
                            LastName = item["LastName"].ToString()

                        });
                        streamWriter.Write(ljson);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    var result = "";
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        result = streamReader.ReadToEnd();
                    }
                    httpResponse.Close();

                    var serializerSettings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        DefaultValueHandling = DefaultValueHandling.Ignore
                    };
                    try
                    {
                        JavaScriptSerializer jssp = new JavaScriptSerializer();

                        var jsonSmsInfo = JsonConvert.DeserializeObject(result, serializerSettings);
                        dynamic jsonp = jssp.DeserializeObject(result);

                        string respcode = ""; string data = ""; string respdesc = "";
                        AddtoLogFile(result, "Card CMS Response");
                        respcode = jsonp["responseCode"];
                        respdesc = jsonp["responseText"];

                        #endregion

                        if (respcode == "00")
                        {
                            panNoEncrypted = jsonp["statusMessage"]["crd_no_cipher"];
                            panNoMasked = jsonp["statusMessage"]["crd_no_mask"];
                            panNo = jsonp["data"];
                        }
                    }
                    catch (Exception)
                    {

                    }

                    if (panNoEncrypted != "")
                    {

                        string cardfileRec = string.Format("{0},{1}", panNo, strAccountNo);

                        AddtoCardFile(cardfileRec, "CardfileInfo");

                        string cardNo = panNo;
                        // Decrypt cardinfo
                        string ActrequestUrl = string.Format("{0}/{1}", AfroGOPAYHost, "Cards/ActivateCard"); ;
                        var httpWebActRequest = (HttpWebRequest)WebRequest.Create(ActrequestUrl);

                        httpWebActRequest.ContentType = "application/json";

                        httpWebActRequest.Method = "POST";
                        httpWebActRequest.Headers.Add("AuthToken", strAuthToken);

                        using (var streamWriter = new StreamWriter(httpWebActRequest.GetRequestStream()))
                        {
                            string ljson = new JavaScriptSerializer().Serialize(new
                            {

                                cardNo  

                            });
                            streamWriter.Write(ljson);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        var httpActResponse = (HttpWebResponse)httpWebActRequest.GetResponse();
                        var Activateresult = "";
                        using (var streamReader = new StreamReader(httpActResponse.GetResponseStream()))
                        {
                            Activateresult = streamReader.ReadToEnd();
                        }

                        httpActResponse.Close();
                        // section Activate card info
                        string ActivationResp = "";

                        try
                        {
                            JavaScriptSerializer jssp = new JavaScriptSerializer();

                            var jsonSmsInfo = JsonConvert.DeserializeObject(Activateresult, serializerSettings);
                            dynamic jsonp = jssp.DeserializeObject(Activateresult);

                            string respcode = ""; string data = ""; string respdesc = ""; 
                            respcode = jsonp["responseCode"];
                            respdesc = jsonp["responseText"];
 

                            if (respcode == "00")
                            {
                                panNoEncrypted = jsonp["statusMessage"]["crd_no_cipher"];
                                ActivationResp = jsonp["data"];
                            }
                        }
                        catch (Exception)
                        {

                        }

                       

                        string cardfileReclog = string.Format("{0},{1},{2}", panNoMasked, strAccountNo, ActivationResp);

                        AddtoLogFile(cardfileReclog, "CardfileInfo Activated");

 

  

                        SqlDataAdapter cmdAdapterAcctUpd = new SqlDataAdapter("SpUpdateCardReqRefItem", Connbiz);

                        cmdAdapterAcctUpd.SelectCommand.CommandType = CommandType.StoredProcedure;
                        cmdAdapterAcctUpd.SelectCommand.Parameters.Add(new SqlParameter("@eBatRefNo", BatchrefNo));
                        cmdAdapterAcctUpd.SelectCommand.Parameters.Add(new SqlParameter("@accountNo", strAccountNo));
                        cmdAdapterAcctUpd.SelectCommand.Parameters.Add(new SqlParameter("@panno", panNoMasked));
                        cmdAdapterAcctUpd.SelectCommand.Parameters.Add(new SqlParameter("@trackId", panNoEncrypted));
                        cmdAdapterAcctUpd.SelectCommand.Parameters.Add(new SqlParameter("@status", 7));
                        cmdAdapterAcctUpd.Fill(dtAcctList);

                    }

                }

            }

            // write to file the Cardfile
        }
        ctrlGrid.DataBind();
    }


    // =====================================================================
    // Page plumbing for the plain GridView (selection, row values, feedback)
    // =====================================================================

    /// <summary>Keys (RefNo) of the rows ticked on the current page.</summary>
    private List<string> SelectedKeys()
    {
        List<string> keys = new List<string>();
        foreach (GridViewRow row in ctrlGrid.Rows)
        {
            if (row.RowType != DataControlRowType.DataRow) continue;
            CheckBox chk = row.FindControl("chkSelect") as CheckBox;
            if (chk != null && chk.Checked) keys.Add(Convert.ToString(ctrlGrid.DataKeys[row.RowIndex].Value));
        }
        return keys;
    }

    private DataView _rows;

    /// <summary>A column of the listed row with this key ("" when the row or column is missing).</summary>
    private object RowValue(object key, string column)
    {
        if (_rows == null) _rows = SqlDataSource1.Select(DataSourceSelectArguments.Empty) as DataView;
        if (_rows == null || !_rows.Table.Columns.Contains(column)) return "";
        foreach (DataRowView r in _rows)
        {
            if (Convert.ToString(r["RefNo"]) == Convert.ToString(key)) return r[column] ?? "";
        }
        return "";
    }

    /// <summary>Runs an action for the ticked rows and reports the outcome.</summary>
    private void RunAction(string what, Action work)
    {
        int count = SelectedKeys().Count;
        if (count == 0) { ShowError("Tick at least one row first."); return; }
        try
        {
            work();
            ShowSuccess(string.Format("{0}: done for {1} {2}{3}. Details are in the log file.", what, count, "batch", count == 1 ? "" : "s"));
        }
        catch (System.Threading.ThreadAbortException) { throw; }
        catch (Exception ex)
        {
            ShowError(what + " stopped: " + ex.Message);
        }
        _rows = null;
        ctrlGrid.DataBind();
    }

    private void ShowSuccess(string message)
    {
        litResult.Text = HttpUtility.HtmlEncode(message);
        Literal1.Text = "";
    }

    private void ShowError(string message)
    {
        Literal1.Text = HttpUtility.HtmlEncode(message);
        litResult.Text = "";
    }

    protected void ctrlGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ctrlGrid.PageIndex = e.NewPageIndex;
    }

    protected void SqlDataSource1_Selected(object sender, SqlDataSourceStatusEventArgs e)
    {
        litTotal.Text = e.Exception == null ? string.Format("{0:N0} {1}{2}", e.AffectedRows, "batch", e.AffectedRows == 1 ? "" : "s") : "";
    }
}
