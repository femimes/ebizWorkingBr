using System;

//using System.DirectoryServices;
//using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Data;
using System.Net;
using System.Web.Script.Serialization;
//using System.Web.HttpCookie; 
using Newtonsoft.Json;
using System.Security.Cryptography;

public partial class AlphaBeta_AgencyList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack && !IsCallback)
        { 
            txtamount.Text = "1001.15";
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
         

        string strHostUrl = System.Configuration.ConfigurationManager.ConnectionStrings["iMPhaseHostUrl"].ToString();
        string strAgentUrl = string.Format("{0}", "http://xxsg.ebs-rcm.com/Interface/billtype");
        //Uri strUrl;
        string Clientid = "715439421649801";
        string Key = "PH5UVZG993ND6BMADS65";
        string State = "XXSG";
      
        System.Text.UnicodeEncoding UE = new System.Text.UnicodeEncoding();
        byte[] message = UE.GetBytes(Key);
        SHA512Managed hashString = new SHA512Managed();
        string hexNumber = "";
        byte[] hashValue = hashString.ComputeHash(message);
        foreach (byte x in hashValue)
        {
            hexNumber += String.Format("{0:x2}", x);
        }


        string hashData = hexNumber;


        string hash = "";
        string source = "Hello World!";
        string strhashSum = string.Format("{0}{1}", Key, State);
        using (SHA512 sha512Hash = SHA512.Create())
        {
            //From String to byte array
            byte[] sourceBytes = System.Text.Encoding.UTF8.GetBytes(strhashSum);
            byte[] hashBytes = sha512Hash.ComputeHash(sourceBytes);
              hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);

            Console.WriteLine("The SHA512 hash of " + source + " is: " + hash);
        }
        #region Add Auth Info

        var httpWebRequest = (HttpWebRequest)WebRequest.Create(strAgentUrl);

        httpWebRequest.ContentType = "application/json";

        httpWebRequest.Method = "POST";
        //httpWebRequest.Headers.Add("Token", Token);


        #endregion

        string RequestTimeStamp = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm:ss+0000");
        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        {
            string ljson = new JavaScriptSerializer().Serialize(new
            {

                //{ tt = *152 * 50.0 * 08099999, ogn = 08055567654, opid = 010, apiCall = 0, tid = 78767777777777766666}

                //State = "XXSG",//State ,
                //   Hash = "419EC01117A5DD8952F7E7251CEA1D05CD3753B76DD890899CA338808E7289A48FD365416E0EBBB71411A076E647E897F2EFD08F7EE884FEDA2F6AA7B266C30A",//hash,
                //Clientid = "715439421649801" //Clientid
                //State = State,
                //Hash = "419EC01117A5DD8952F7E7251CEA1D05CD3753B76DD890899CA338808E7289A48FD365416E0EBBB71411A076E647E897F2EFD08F7EE884FEDA2F6AA7B266C30A",
                //Clientid = Clientid

                State = State,
                Hash = hash,
                Clientid = Clientid

            });
            // json = Convert.ToBase64String(Encoding.Default.GetBytes(json));
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

         
        Response.Write(result + "</br>");
        Response.Write(hash);
    }
    public static class SHA
    {

        public static string GenerateSHA512String(string inputString)
        {
            SHA512 sha512 = SHA512Managed.Create();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha512.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        private static string GetStringFromHash(byte[] hash)
        {
            System.Text.StringBuilder result = new System.Text.StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }

    }

    protected void Button2_Click(object sender, EventArgs e)
    {


        string strHostUrl = System.Configuration.ConfigurationManager.ConnectionStrings["iMPhaseHostUrl"].ToString();
        string strAgentUrl = string.Format("{0}", "http://xxsg.ebs-rcm.com/interface/revcodebyAgencyRef");
        //Uri strUrl;
        string Clientid = "715439421649801";
        string Key = "PH5UVZG993ND6BMADS65";
        string State = "XXSG";

        System.Text.UnicodeEncoding UE = new System.Text.UnicodeEncoding();
        byte[] message = UE.GetBytes(Key);
        SHA512Managed hashString = new SHA512Managed();
        string hexNumber = "";
        byte[] hashValue = hashString.ComputeHash(message);
        foreach (byte x in hashValue)
        {
            hexNumber += String.Format("{0:x2}", x);
        }


        string hashData = hexNumber;


        string hash = "";
        string source = "Hello World!";
        string strhashSum = string.Format("{0}{1}", Key, State);
        using (SHA512 sha512Hash = SHA512.Create())
        {
            //From String to byte array
            byte[] sourceBytes = System.Text.Encoding.UTF8.GetBytes(strhashSum);
            byte[] hashBytes = sha512Hash.ComputeHash(sourceBytes);
            hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);

            Console.WriteLine("The SHA512 hash of " + source + " is: " + hash);
        }
        #region Add Auth Info

        var httpWebRequest = (HttpWebRequest)WebRequest.Create(strAgentUrl);

        httpWebRequest.ContentType = "application/json";

        httpWebRequest.Method = "POST";
        //httpWebRequest.Headers.Add("Token", Token);


        #endregion

        string RequestTimeStamp = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm:ss+0000");
        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        {
            string ljson = new JavaScriptSerializer().Serialize(new
            {

                //{ tt = *152 * 50.0 * 08099999, ogn = 08055567654, opid = 010, apiCall = 0, tid = 78767777777777766666}

                //State = "XXSG",//State ,
                //   Hash = "419EC01117A5DD8952F7E7251CEA1D05CD3753B76DD890899CA338808E7289A48FD365416E0EBBB71411A076E647E897F2EFD08F7EE884FEDA2F6AA7B266C30A",//hash,
                //Clientid = "715439421649801" //Clientid
               AgencyHead ="3100000",
                 
                State = State,
                Hash = "05F5AC7D3B99ABA9C5C8C41CA0173DDFC01E5506CD41ABAFD9091C5F2933333E71928C598A638EAFEAB351ACDAD75035803B4515496B9FE6CCDCD8E7BD17CC40",
                Clientid = Clientid

                //State = State,
                //Hash = hash,
                //Clientid = Clientid

            });
            // json = Convert.ToBase64String(Encoding.Default.GetBytes(json));
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


        Response.Write(result + "</br>");
        Response.Write(hash);
    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        string RequestTimeStamp = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm:ss+0000");
        string TransDateTimeStamp = DateTime.Now.ToString("yyyy'-'MMMM'-'dd'T'HH':'mm:ss+0000");

        string RefNoSub = Guid.NewGuid().ToString().Substring(0, 8).ToUpper().Replace("-", "");

        string strBrand = System.Configuration.ConfigurationManager.ConnectionStrings["CompanyBrand"].ConnectionString;
        string strPayRefNo = string.Format("{0}{1}{2}", strBrand, RefNoSub, RequestTimeStamp);

        string Token; string CustRefNo = "N-191221"; double Amount = Double.Parse(txtamount.Text);  string RevCode= "1002002";
        string AgencyCode = "3100000";  string AppliedDate = TransDateTimeStamp;
        string AssessmentRef = strPayRefNo;
        //var getheaderReq3 = Page.Request.Headers["Token"];// Page.Request.Headers.GetValues("Token");
        //var getheaderReq2 = Page.Request.Headers["Authorization"];

        #region Details Alpha Beta 

        string strHostUrl = System.Configuration.ConfigurationManager.ConnectionStrings["iMPhaseHostUrl"].ToString();
 
        string strAgentUrl = string.Format("{0}", "http://xxsg.ebs-rcm.com/Interface/WebGuid");
        //Uri strUrl;
        string Clientid = "715439421649801";
        string Key = "PH5UVZG993ND6BMADS65";
        string State = "XXSG";

        System.Text.UnicodeEncoding UE = new System.Text.UnicodeEncoding();
        byte[] message = UE.GetBytes(Key);
        SHA512Managed hashString = new SHA512Managed();

        string hash = "";
        //Key + Pid + State
        string strhashSum = string.Format("{0}{1}{2}{3}", Key, CustRefNo, State, Amount);

        using (SHA512 sha512Hash = SHA512.Create())
        {
            //From String to byte array
            byte[] sourceBytes = System.Text.Encoding.UTF8.GetBytes(strhashSum);
            byte[] hashBytes = sha512Hash.ComputeHash(sourceBytes);
            hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);


        }
        #region Add Auth Info

        var httpWebRequest = (HttpWebRequest)WebRequest.Create(strAgentUrl);

        httpWebRequest.ContentType = "application/json";

        httpWebRequest.Method = "POST";
        //httpWebRequest.Headers.Add("Token", Token);


        #endregion


        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        {
            string ljson = new JavaScriptSerializer().Serialize(new
            {


                pid = CustRefNo,
                Amount = Amount,
                AgencyCode = AgencyCode,
                RevCode = RevCode,
                AppliedDate = AppliedDate,
                AssessmentRef = AssessmentRef,
                state = State,
                hash = hash,
                clientid = Clientid

            });
            // json = Convert.ToBase64String(Encoding.Default.GetBytes(json));
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

        #endregion


        if (result.Contains("PId not found") == false)
        {

            var x = Newtonsoft.Json.Linq.JObject.Parse(result);


            #region Sample Test
            //var jsonLinq = Newtonsoft.Json.Linq.JObject.Parse(result);

            //// Find the first array using Linq
            //var srcArray = jsonLinq.Descendants().Where(d => d is Newtonsoft.Json.Linq.JArray).First();
            //var trgArray = new Newtonsoft.Json.Linq.JArray();
            //foreach (Newtonsoft.Json.Linq.JObject row in srcArray.Children<Newtonsoft.Json.Linq.JObject>())
            //{
            //    var cleanRow = new Newtonsoft.Json.Linq.JObject();
            //    foreach (Newtonsoft.Json.Linq.JProperty column in row.Properties())
            //    {
            //        // Only include JValue types
            //        if (column.Value is Newtonsoft.Json.Linq.JValue)
            //        {
            //            cleanRow.Add(column.Name, column.Value);
            //        }
            //    }

            //    trgArray.Add(cleanRow);
            //}

            //return JsonConvert.DeserializeObject<DataTable>(trgArray.ToString());
            #endregion


            //dynamic jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject(result);
            //DataTable dt = JsonConvert.DeserializeObject<DataTable>(Convert.ToString(jsonObject));
         Response.Write(result);

            //DataTable dt = (DataTable)JsonConvert.DeserializeObject(x.ToString(), (typeof(DataTable)));
            //return dt;
        }
        else
        {
            //DataTable dt = new DataTable();
            Response.Write(result);
        }

    }


    protected void Button4_Click(object sender, EventArgs e)
    {
        string Token; string CustRefNo = "N-1912217"; double Amount = 10000.21; string RevCode = "1002002";
        string AgencyCode = "3100000"; string AppliedDate = "2020-October-14T09:13:25+0000";
        string AssessmentRef = "Abbey2020-10-14T09:13:25+0000"; string PaymentRef = "Abbey mfb - 1";

        //var getheaderReq3 = Page.Request.Headers["Token"];// Page.Request.Headers.GetValues("Token");
        //var getheaderReq2 = Page.Request.Headers["Authorization"];

        #region Details Alpha Beta 

        string strHostUrl = System.Configuration.ConfigurationManager.ConnectionStrings["iMPhaseHostUrl"].ToString();

        string strAgentUrl = string.Format("{0}", "http://xxsg.ebs-rcm.com/Interface/ReceiptByPaymentRef");
        //Uri strUrl;
        string Clientid = "715439421649801";
        string Key = "PH5UVZG993ND6BMADS65";
        string State = "XXSG";

        System.Text.UnicodeEncoding UE = new System.Text.UnicodeEncoding();
        byte[] message = UE.GetBytes(Key);
        SHA512Managed hashString = new SHA512Managed();

        string hash = "";
        //Key + Pid + State
        string strhashSum = string.Format("{0}{1}{2}", Key, PaymentRef, State);

        using (SHA512 sha512Hash = SHA512.Create())
        {
            //From String to byte array
            byte[] sourceBytes = System.Text.Encoding.UTF8.GetBytes(strhashSum);
            byte[] hashBytes = sha512Hash.ComputeHash(sourceBytes);
            hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);


        }
        #region Add Auth Info

        var httpWebRequest = (HttpWebRequest)WebRequest.Create(strAgentUrl);

        httpWebRequest.ContentType = "application/json";

        httpWebRequest.Method = "POST";
        //httpWebRequest.Headers.Add("Token", Token);


        #endregion


        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        {
            string ljson = new JavaScriptSerializer().Serialize(new
            {

                PaymentRef = PaymentRef,
                state = State,
                hash = hash,
                clientid = Clientid

            });
            // json = Convert.ToBase64String(Encoding.Default.GetBytes(json));
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

        #endregion


        if (result.Contains("PId not found") == false)
        {

            var x = Newtonsoft.Json.Linq.JObject.Parse(result);


            #region Sample Test
            //var jsonLinq = Newtonsoft.Json.Linq.JObject.Parse(result);

            //// Find the first array using Linq
            //var srcArray = jsonLinq.Descendants().Where(d => d is Newtonsoft.Json.Linq.JArray).First();
            //var trgArray = new Newtonsoft.Json.Linq.JArray();
            //foreach (Newtonsoft.Json.Linq.JObject row in srcArray.Children<Newtonsoft.Json.Linq.JObject>())
            //{
            //    var cleanRow = new Newtonsoft.Json.Linq.JObject();
            //    foreach (Newtonsoft.Json.Linq.JProperty column in row.Properties())
            //    {
            //        // Only include JValue types
            //        if (column.Value is Newtonsoft.Json.Linq.JValue)
            //        {
            //            cleanRow.Add(column.Name, column.Value);
            //        }
            //    }

            //    trgArray.Add(cleanRow);
            //}

            //return JsonConvert.DeserializeObject<DataTable>(trgArray.ToString());
            #endregion


            //dynamic jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject(result);
            //DataTable dt = JsonConvert.DeserializeObject<DataTable>(Convert.ToString(jsonObject));
            Response.Write(result);

            //DataTable dt = (DataTable)JsonConvert.DeserializeObject(x.ToString(), (typeof(DataTable)));
            //return dt;
        }
        else
        {
            //DataTable dt = new DataTable();
            Response.Write(result);
        }

    }



    protected void Button5_Click(object sender, EventArgs e)
    {

        string Token; string CustRefNo = "N-1912217"; double Amount = 10000.21; string RevCode = "1002002";
        string AgencyCode = "3100000"; string AppliedDate = "2020-October-14T09:13:25+0000";
        string AssessmentRef = "Abbey2020-10-14T09:13:25+0000"; string PaymentRef = "Abbey mfb - 1";
        string webguid = "1901889229";

        //var getheaderReq3 = Page.Request.Headers["Token"];// Page.Request.Headers.GetValues("Token");
        //var getheaderReq2 = Page.Request.Headers["Authorization"];

        #region Details Alpha Beta 

        string strHostUrl = System.Configuration.ConfigurationManager.ConnectionStrings["iMPhaseHostUrl"].ToString();

        string strAgentUrl = string.Format("{0}", "http://xxsg.ebs-rcm.com/Interface/Validate");
        //Uri strUrl;
        string Clientid = "715439421649801";
        string Key = "PH5UVZG993ND6BMADS65";
        string State = "XXSG";

        System.Text.UnicodeEncoding UE = new System.Text.UnicodeEncoding();
        byte[] message = UE.GetBytes(Key);
        SHA512Managed hashString = new SHA512Managed();

        string hash = "";
        //Key + Pid + State
        string strhashSum = string.Format("{0}{1}{2}", Key, webguid, State);

        using (SHA512 sha512Hash = SHA512.Create())
        {
            //From String to byte array
            byte[] sourceBytes = System.Text.Encoding.UTF8.GetBytes(strhashSum);
            byte[] hashBytes = sha512Hash.ComputeHash(sourceBytes);
            hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);


        }
        #region Add Auth Info

        var httpWebRequest = (HttpWebRequest)WebRequest.Create(strAgentUrl);

        httpWebRequest.ContentType = "application/json";

        httpWebRequest.Method = "POST";
        //httpWebRequest.Headers.Add("Token", Token);


        #endregion


        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        {
            string ljson = new JavaScriptSerializer().Serialize(new
            {

                webguid = webguid,
                type= "WEBGUID",
                state = State,
                hash = hash,
                clientid = Clientid

            });
            // json = Convert.ToBase64String(Encoding.Default.GetBytes(json));
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

        #endregion


        if (result.Contains("PId not found") == false)
        {

            var x = Newtonsoft.Json.Linq.JObject.Parse(result);


            #region Sample Test
            //var jsonLinq = Newtonsoft.Json.Linq.JObject.Parse(result);

            //// Find the first array using Linq
            //var srcArray = jsonLinq.Descendants().Where(d => d is Newtonsoft.Json.Linq.JArray).First();
            //var trgArray = new Newtonsoft.Json.Linq.JArray();
            //foreach (Newtonsoft.Json.Linq.JObject row in srcArray.Children<Newtonsoft.Json.Linq.JObject>())
            //{
            //    var cleanRow = new Newtonsoft.Json.Linq.JObject();
            //    foreach (Newtonsoft.Json.Linq.JProperty column in row.Properties())
            //    {
            //        // Only include JValue types
            //        if (column.Value is Newtonsoft.Json.Linq.JValue)
            //        {
            //            cleanRow.Add(column.Name, column.Value);
            //        }
            //    }

            //    trgArray.Add(cleanRow);
            //}

            //return JsonConvert.DeserializeObject<DataTable>(trgArray.ToString());
            #endregion


            //dynamic jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject(result);
            //DataTable dt = JsonConvert.DeserializeObject<DataTable>(Convert.ToString(jsonObject));
            Response.Write(result);

            //DataTable dt = (DataTable)JsonConvert.DeserializeObject(x.ToString(), (typeof(DataTable)));
            //return dt;
        }
        else
        {
            //DataTable dt = new DataTable();
            Response.Write(result);
        }

    }
}