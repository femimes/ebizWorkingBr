using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
//using System.Text.Json;
public partial class Administration_Default2 :  System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected async Task Button1_ClickAsync(object sender, EventArgs e)
    {
        //var resp = await ExecuteBalanceEnquiryAsync("000000");
        dynamic lp = @"{ AccountNumber = 0000038097,Amount = 15000, TransactionReference = 310954158551, location = , date = 22-08-2024, terminalType = , currencyCode = 566, Feecharges = 0 }";
        foreach (var item in lp)
        {
            Response.Write(item["Amount"]);
        }

        
    }

    public async Task<dynamic> ExecuteBalanceEnquiryAsync(string accountNumber)
    {
        decimal availableBalance = 0; decimal LedgerBalance = 0; decimal Fee = 0;//string RefNo = $"31{DateTime.Now.ToString("yyyyMMddHHmmss")}";


        DataSet ds = new DataSet();
        //AddtoLogTransFile($"Balance enquiry Connstr: {NETClass.NETClass.StrConnEazyConnectIB}", "Connection String");
        SqlConnection ConneChannels = new SqlConnection("Data Source=POKKIEHP;Initial Catalog=EazybankAx;User ID=sms;password=abc@123;Integrated Security=false;");

        SqlDataAdapter cmdAdpter = new SqlDataAdapter("Sproc_ATMExtBalEnquiry", ConneChannels);
        cmdAdpter.SelectCommand.CommandType = CommandType.StoredProcedure;
        cmdAdpter.SelectCommand.Parameters.Add(new SqlParameter("@AccountNo", accountNumber));
        cmdAdpter.Fill(ds);
        foreach (DataRow item in ds.Tables[0].Rows)
        {
            availableBalance = decimal.Parse(item["AcctAvaliableBal"].ToString());
            LedgerBalance = decimal.Parse(item["BookBal"].ToString());
        }

        var pocoObject = new
        {

            AccountNo = accountNumber,
            AvailableBalance = availableBalance,
            LedgerBalance = LedgerBalance,
            Issucceful = "True",

            Remarks = "BalanceEnquiry"

        };
        //AddtoLogTransFile($"Balance enquiry {pocoObject}", "Balance Enquiry Result");
        //Response.Write(pocoObject);
        //string result = pocoObject.;
        return  pocoObject;
    }

    public static string[] ToStringArray(object arg)
    {
        var collection = arg as System.Collections.IEnumerable;
        if (collection != null)
        {
            return collection
              .Cast<object>()
              .Select(x => x.ToString())
              .ToArray();
        }

        if (arg == null)
        {
            return new string[] { };
        }

        return new string[] { arg.ToString() };
    }

    protected async void Button1_Click(object sender, EventArgs e)
    {
        var resp = await ExecuteBalanceEnquiryAsync("0000135009");

        if (resp.Issucceful == "False")
        {
            Response.Write("Bad issues");

        }
        else
        {
            Response.Write(resp.AccountNo);
            Response.Write("I am fine");
            UpdateElementFortyEightForBalanceEnquiry(null, "2000", "300");
        }

        string strvak = "01";
    }
 
    private void UpdateElementFortyEightForBalanceEnquiry(string[] dataElement, string availableBal, string ledgerBal)
    {
        var defaultFormat = "+0000000000000000";
        var availableBalance = (int)(Convert.ToDecimal(availableBal) * 100); //to kobo             
        var ledgerBalance = (int)(Convert.ToDecimal(ledgerBal) * 100);
        var floatBalance = ledgerBalance - availableBalance;

        var availableBalanceResponse = HelperService.GetIndicator(availableBalance) + availableBalance.ToString("D16");
        var ledgerBalanceResponse = HelperService.GetIndicator(ledgerBalance) + ledgerBalance.ToString("D16");
        var floatBalanceResponse = HelperService.GetIndicator(floatBalance) + floatBalance.ToString("D16");
        //AddtoLogTransFile($"Construct Resp Message Avail:{availableBalanceResponse},ledgerBal:{ledgerBalanceResponse},FloatingBal:{floatBalanceResponse}", "BE_Format Response");
        //AddtoLogTransFile($"{ledgerBalanceResponse + availableBalanceResponse + floatBalanceResponse + defaultFormat + defaultFormat + "566" + DateTime.Now.ToString("yyyyMMddHHmmss")}", "BE_ Field[48]");
        string dataElements = ledgerBalanceResponse + availableBalanceResponse + floatBalanceResponse + defaultFormat + defaultFormat + "566" + DateTime.Now.ToString("yyyyMMddHHmmss");
    }
    public class HelperService
    {

        public static string GetIndicator(Int64 Amount)
        {
            string FormatAmount = "";
            if (Amount < 0)
            {
                //logger.LogWarning($"Creating endpoint for ip {hosts} and port {IntPort} endpoint...");
                FormatAmount = string.Format("-",Amount);
            }
            else
            {
                FormatAmount = string.Format("+", Amount);
            }


            return FormatAmount;
        }
    }
    class ExpandoObject
    {
 
        public string AccountNumber { get; set; }
        public string Amount { get; set; }
        public string TransactionReference { get; set; }
        public string location { get; set; }
        public DateTimeOffset date { get; set; }
        public string terminalType { get; set; }
        public string currencyCode { get; set; }
        public string Feecharges { get; set; }
    }
 
    protected void Button2_Click(object sender, EventArgs e)
    {
        dynamic lp = @"{ AccountNumber = 0000038097,Amount = 15000, TransactionReference = 310954158551, location = , date = 22-08-2024, terminalType = , currencyCode = 566, Feecharges = 0 }";

        //JObject.Parse(lp);
        //var json = JsonSerializer.Create(lp.Replace("=",":"));// Serialize(lp);
        var json = new System.Web.Script.Serialization.JavaScriptSerializer();
        string jsonString = json.Serialize(lp);


        //string AccountNumbers = w.AccountNumber;
        //dynamic data = JsonSerializer.Serialize<dynamic>();

        //var josn = JsonConvert.DeserializeObject(data);
        //var datak = JObject.Parse(data);
        //ExpandoObject response;
        //response = lp;

        //Console.WriteLine(JsonSerializer.Serialize(response, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, WriteIndented = true }));


        string AccountNumber = "0000081182";
        string Amount="15000" ;
        string TransactionReference= "333954058057";
        string location="";
        string date= "22-08-2024";
        string terminalType="";
        string currencyCode="566";
        string Feecharges="0";
        dynamic withdrawalRequest;
        string RefNo = string.Format("01{0}",DateTime.Now.ToString("yyyyMMddHHmmss"));

        {

//            //AddtoLogTransFile($"Cash Withdrawal {TransactionReference}  AmountProcessed: {Amount}", "CashWithdrawal Processing");

////            @Trans_date datetime,
////@Account_no Varchar(20),
////@Ext_transcode Varchar(5),
////@Narrative Varchar(100),
////@Description Varchar(100),
////@DebitAmt decimal(18, 2),
////@FeeAmt decimal(18, 2),
////@ExtRefNo Varchar(50),
////@CardNo Varchar(50),
////@AtmTranType Varchar(10),
////@ExtTerminalID Varchar(50),
////@RRN Varchar(50),
////@Stan Varchar(50),
//            SqlConnection connection = new SqlConnection(string.Format("Data Source=192.168.1.109;Initial Catalog=EazybankAx;User ID=sms;password=abc@123;Integrated Security=false;"));
//            SqlCommand command = new SqlCommand();
//            connection.Open();
//            command.CommandText = "Sproc_ATMAddExtTransWithDrawal";
//            command.Connection = connection;
//            command.CommandType = CommandType.StoredProcedure;

//            command.Parameters.Add(new SqlParameter("@Trans_date", DateTime.Today.Date.ToString("yyyy-MM-dd")));
//            command.Parameters.Add(new SqlParameter("@Account_no", AccountNumber));
//            command.Parameters.Add(new SqlParameter("@Ext_transcode", "01"));

//            command.Parameters.Add(new SqlParameter("@Narrative", "CASH WDL"));
//            command.Parameters.Add(new SqlParameter("@Description", "CASH WDL"));
//            command.Parameters.Add(new SqlParameter("@DebitAmt", Amount));
//            command.Parameters.Add(new SqlParameter("@FeeAmt", Feecharges));

//            command.Parameters.Add(new SqlParameter("@ExtRefNo", TransactionReference));
//            command.Parameters.Add(new SqlParameter("@CardNo", date));
//            command.Parameters.Add(new SqlParameter("@AtmTranType", "0001"));
//            command.Parameters.Add(new SqlParameter("@ExtTerminalID", RefNo));
//            command.Parameters.Add(new SqlParameter("@RRN", TransactionReference));
//            command.Parameters.Add(new SqlParameter("@Stan", "110"));

//            SqlParameter ResponseCode = command.Parameters.Add(new SqlParameter("@ResponseCode", SqlDbType.VarChar, 100));
//            ResponseCode.Direction = ParameterDirection.Output;
//            //SqlParameter parameter = command.Parameters.Add(new SqlParameter("@Result", SqlDbType.Decimal));
//            //parameter.Direction = ParameterDirection.Output;

//            //command.ExecuteScalar();
//            command.ExecuteNonQuery();
//            connection.Close();

//            //return parameter.Value.ToString();
//            if (ResponseCode.Value.ToString() == "0")
//            {
//                //ks = new withdrawalResult("True", "00", "Success");
//                Response.Write("I got here");

//            }
//            else
//            {
//                //ks = new withdrawalResult("False", ResponseCode.Value.ToString(), "FAILED");
//            }
            UpdateElementFortyEightForDeduction("demostrat", decimal.Parse(Amount), "200", "500");
        }
    }

    void UpdateElementFortyEightForDeduction(string dataElement, decimal amount, string availableBal, string ledgerBal)
    {
        var defaultFormat = "+0000000000000000";
        //decimal resultavail;
        //decimal.TryParse(availableBal, out resultavail);
        //decimal resultLedger;
        //decimal.TryParse(ledgerBal, out resultLedger);

        decimal resultavail;
        decimal.TryParse(availableBal, out resultavail);
        decimal resultLedger;
        decimal.TryParse(ledgerBal, out resultLedger);

        var availableBalance = Convert.ToInt64(resultavail * 100); //to kobo             
        var ledgerBalance = Convert.ToInt64(resultLedger * 100);


        var transactionAmount = Convert.ToInt64(amount * 100);//to kobo             

        availableBalance = availableBalance - transactionAmount;
        ledgerBalance = ledgerBalance - transactionAmount;

        var floatBalance = ledgerBalance - availableBalance;

        var availableBalanceResponse = HelperService.GetIndicator((availableBalance)) + availableBalance.ToString("D16");
        var ledgerBalanceResponse = HelperService.GetIndicator((ledgerBalance)) + ledgerBalance.ToString("D16");
        var floatBalanceResponse = HelperService.GetIndicator((floatBalance)) + floatBalance.ToString("D16");

        Response.Write(ledgerBalanceResponse + availableBalanceResponse + floatBalanceResponse + defaultFormat + defaultFormat + "566" + DateTime.Now.ToString("yyyyMMddHHmmss"));





        //dataElement[48] = ledgerBalanceResponse + availableBalanceResponse + floatBalanceResponse + defaultFormat + defaultFormat + "566" + DateTime.Now.ToString("yyyyMMddHHmmss");
    }


}