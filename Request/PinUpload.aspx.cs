
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using NETClass;
using System.Globalization;
//using Newtonsoft.Json;
//using iBankPoints;

using System.Web.Script.Serialization;

public partial class Request_PinUpload :  System.Web.UI.Page
{

    public string gUser = HttpContext.Current.User.Identity.Name;//HttpContext.Current.User.Identity.Name;
    public string luser = "";
    public string StrLocation = "";
    public string struser = "";
    public decimal l_totalAmt = 0;
    public string transflag = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.IsCallback && this.IsPostBack)
            return;
        DataTable ks = new DataTable();
        //ks = iBankPoints.iPoints.ProcessProfileds(Request.Cookies["eLogonUserID"].Value, Request.Cookies["Token"].Value);
        foreach (DataRow item in ks.Rows)
        {
            lblAccountNo.Text = item["AccountNo"].ToString();
        }
        string strauthUser;
        string strgetauthcookies = Request.Cookies["LogonUserID"].Value;
        if (strgetauthcookies != null)
        {
            strauthUser = strgetauthcookies.Substring(0, strgetauthcookies.LastIndexOf("?"));
        }
        else
        {
            strauthUser = strgetauthcookies.Substring(0, strgetauthcookies.LastIndexOf("?"));
            strauthUser = HttpContext.Current.User.Identity.Name.Substring(0, HttpContext.Current.User.Identity.Name.LastIndexOf("?"));
        }
        lblUser.Text = strauthUser;


        lbldebit.Text = string.Format("NGN {0:0,0.00}", 0);
        lblAvailBal.Text = string.Format("NGN {0:0,0.00}", 0);
        Label1.Text = string.Format("NGN {0:0,0.00}", 0);
        //ASPxComboBox1.Value = lblAccountNo.Text;
        //LoadAcctDetails(lblAccountNo.Text);
        DataGrid1.Attributes["total"] = string.Format("{0}", "0");

    }

    protected void btnImport_Click(object sender, EventArgs e)
    {
        //try
        {
            string str = this.UploadMultipleFiles_Clicked();

            if (string.Compare(str, (string)null, false) == 0)
                return;


            // if (string.IsNullOrEmpty (Request.Cookies["LogonUserID"].Value) == false)
            if (string.IsNullOrEmpty(str) == false)
            {
                //InsertAllRecordsTrans(str);
                InsertAllRecordsPIN(str);

            }
            else
                return;// this.InsertAllRecordHr(str);

        }
        //catch (Exception ex)
        //{

        //    this.Response.Write(String.Format("<script>alert('Invalid file format {0}') </script>", ex.Message));

        //}

    }

    public string UploadMultipleFiles_Clicked()
    {
        string path = "C:\\TempMultipleFiles\\";
        string str1;
        try
        {
            int num1 = 0;
            int num2 = checked(this.Request.Files.Count - 1);
            int index = num1;
            string str2 = "";
            string Left = "";
            while (index <= num2)
            {
                HttpPostedFile httpPostedFile = this.Request.Files[index];

                if (!(httpPostedFile == null | string.Compare(httpPostedFile.FileName, "", false) == 0 | httpPostedFile.ContentLength < 1))
                {
                    str2 = Path.GetFileName(httpPostedFile.FileName);

                    // Get the extension of the uploaded file.
                    string extension = System.IO.Path.GetExtension(str2);

                    if ((extension == ".txt") || (extension == ".csv") || (extension == ".xls"))
                    {

                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);
                        httpPostedFile.SaveAs(path + str2);
                        Left = Left + "Uploaded File: " + httpPostedFile.FileName + " of type " + httpPostedFile.ContentType + " <br> ";
                    }
                    else
                    {
                        Left = "Your file was not uploaded because " +
                                       "it does not have a .txt or .csv extension.";

                        //this.Response.Write("<script>alert('" + Left + "') </script>");
                    }
                }
                checked { ++index; }
            }
            if (string.Compare(Left, "", false) == 0)
            {
                Left = "Select the file to upload or import.";
                str2 = (string)null;
            }
            this.Response.Write("<script>alert('" + Left + "') </script>");
            str1 = str2;
        }
        catch (Exception ex)
        {
            //ProjectData.SetProjectError(ex);
            ex.ToString();
            //ProjectData.ClearProjectError();
            str1 = ex.Message.ToString();

        }
        return str1;
    }


    public void InsertAllRecordsTrans(string m_strFileName)
    {

        //this.luser = Request.Cookies["LogonUserID"].Value;// HttpContext.Current.User.Identity.Name;//HttpContext.Current.User.Identity.Name; //HttpContext.Current.User.Identity.Name;// HttpContext.Current.User.Identity.Name;
        //StrLocation = this.luser.Substring(this.luser.LastIndexOf("?") + 1);
        //this.struser = this.luser.Substring(0, this.luser.LastIndexOf("?"));
        //this.gUser = this.struser;

        StreamReader streamReader = new StreamReader("C:\\TempMultipleFiles\\" + m_strFileName);
        DataSet dataSet = new DataSet();
        DataTable dataTable = new DataTable();


        //dataTable.Columns.Add(new DataColumn("Beneficiary AccountNo ", typeof(string)));
        ////dataTable.Columns.Add(new DataColumn("Beneficiary SortCode", typeof(string)));
        //dataTable.Columns.Add(new DataColumn("Amount", typeof(string)));
        //dataTable.Columns.Add(new DataColumn("Narration", typeof(string)));
        //dataTable.Columns.Add(new DataColumn("Beneficiary AccountName", typeof(string)));
        //dataTable.Columns.Add(new DataColumn("Beneficiary Bank", typeof(string)));
        //dataTable.Columns.Add(new DataColumn("Remarks", typeof(string)));



        dataTable.Columns.Add(new DataColumn("Post Date", typeof(string)));
        dataTable.Columns.Add(new DataColumn("MerchantAcct/ID", typeof(string)));
        dataTable.Columns.Add(new DataColumn("STAN", typeof(string)));
        dataTable.Columns.Add(new DataColumn("TerminalID", typeof(string)));
        dataTable.Columns.Add(new DataColumn("Reference Code", typeof(string)));
        dataTable.Columns.Add(new DataColumn("RETAILER NAME ", typeof(string)));
        dataTable.Columns.Add(new DataColumn("TRANSACTION TYPE", typeof(string)));
        dataTable.Columns.Add(new DataColumn("GROSS AMOUNT", typeof(string)));
        dataTable.Columns.Add(new DataColumn("AMOUNT DUE", typeof(string)));
        dataTable.Columns.Add(new DataColumn("TRANSACTION DATETIME", typeof(string)));
        dataTable.Columns.Add(new DataColumn("MASKEDPAN", typeof(string)));
        dataTable.Columns.Add(new DataColumn("CARDSCHEME", typeof(string)));
        dataTable.Columns.Add(new DataColumn("STATUS", typeof(string)));


        dataTable.Columns.Add(new DataColumn("TRANSACTION ID", typeof(string)));
        dataTable.Columns.Add(new DataColumn("RRN", typeof(string)));
        dataTable.Columns.Add(new DataColumn("ARN", typeof(string)));
        dataTable.Columns.Add(new DataColumn("Remarks", typeof(string)));

        string str1;
        while ((str1 = streamReader.ReadLine()) != null)
        {
            //str1 = streamReader.ReadLine();
            //string.Len(str1);


            //char[] delimiters = new char[] { ',' ,'\t'};
            //char[] delimiters = new char[] { '\t', ',' };
            char[] delimiters = new char[] { '\t' };
            //string[] parts = str1.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            string[] parts = str1.Split(delimiters, StringSplitOptions.None);
            DataRow row = dataTable.NewRow();
            for (int i = 0; i < parts.Length; i++)
            {
                decimal thdecimal;
                if (string.IsNullOrEmpty(parts[i]) == true)
                {
                    row[i] = "";
                }
                else
                {
                    //if ((decimal.TryParse(parts[i], out thdecimal)) == false)
                    //{

                    //    row[i] = decimal.Parse(string.Format(parts[i]));

                    //}
                    //else
                    //{
                    //row[i] = string.Format(parts[i]);
                    //}
                    //if (i == 10)
                    //    {

                    //    row[i] = string.Format(parts[i].Replace(",",""));
                    //     }
                    //else
                    //{

                    //row[i] = string.Format(parts[i]);
                    //}

                    row[i] = string.Format(parts[i]);

                }


            }
            //Response.Write("<br/>");
            dataTable.Rows.Add(row);







        }
        //while (str1 != null);
        streamReader.Close();

        DataView view = new DataView(dataTable);
        DataTable dtQueryTable = view.ToTable(false, new string[] { "TRANSACTION DATETIME", "STAN", "TerminalID", "GROSS AMOUNT", "AMOUNT DUE", "TRANSACTION TYPE", "MASKEDPAN", "RRN", "Remarks" });

        this.DataGrid1.DataSource = dtQueryTable;
        this.DataGrid1.DataBind();
        LoadApprovingOfficers();
        //LoadApprovingOfficers(Convert.ToDecimal(DataGrid1.Attributes["total"]));
         string Left = this.ValidateEntriesPOS();

        if (string.Compare(Left, (string)null, false) == 0)
            return;
        this.Response.Write("<script>alert(' The following records are with errors !!" + Left + "') </script>");
    }
    public void InsertAllRecordsPIN(string m_strFileName)
    {

        //this.luser = Request.Cookies["LogonUserID"].Value;// HttpContext.Current.User.Identity.Name;//HttpContext.Current.User.Identity.Name; //HttpContext.Current.User.Identity.Name;// HttpContext.Current.User.Identity.Name;
        //StrLocation = this.luser.Substring(this.luser.LastIndexOf("?") + 1);
        //this.struser = this.luser.Substring(0, this.luser.LastIndexOf("?"));
        //this.gUser = this.struser;

        StreamReader streamReader = new StreamReader("C:\\TempMultipleFiles\\" + m_strFileName);
        DataSet dataSet = new DataSet();
        DataTable dataTable = new DataTable();


        //dataTable.Columns.Add(new DataColumn("Beneficiary AccountNo ", typeof(string)));
        ////dataTable.Columns.Add(new DataColumn("Beneficiary SortCode", typeof(string)));
        //dataTable.Columns.Add(new DataColumn("Amount", typeof(string)));
        //dataTable.Columns.Add(new DataColumn("Narration", typeof(string)));
        //dataTable.Columns.Add(new DataColumn("Beneficiary AccountName", typeof(string)));
        //dataTable.Columns.Add(new DataColumn("Beneficiary Bank", typeof(string)));
        //dataTable.Columns.Add(new DataColumn("Remarks", typeof(string)));

        

        dataTable.Columns.Add(new DataColumn("CardNo", typeof(string)));
        dataTable.Columns.Add(new DataColumn("PIN", typeof(string)));
        dataTable.Columns.Add(new DataColumn("Bank", typeof(string)));
        dataTable.Columns.Add(new DataColumn("Type", typeof(string)));
        dataTable.Columns.Add(new DataColumn("Address", typeof(string)));
        dataTable.Columns.Add(new DataColumn("City", typeof(string)));
        dataTable.Columns.Add(new DataColumn("AccountNo", typeof(string)));
        dataTable.Columns.Add(new DataColumn("Country", typeof(string)));
        dataTable.Columns.Add(new DataColumn("Remarks", typeof(string)));

        string str1;
        while ((str1 = streamReader.ReadLine()) != null)
        {
            //str1 = streamReader.ReadLine();
            //string.Len(str1);


            //char[] delimiters = new char[] { ',' ,'\t'};
             char[] delimiters = new char[] { '\t', ',' };
            //char[] delimiters = new char[] { '\t' };
            //string[] parts = str1.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            string[] parts = str1.Split(delimiters, StringSplitOptions.None);
            DataRow row = dataTable.NewRow();
            for (int i = 0; i < parts.Length; i++)
            {
                decimal thdecimal;
                if (string.IsNullOrEmpty(parts[i]) == true)
                {
                    row[i] = "";
                }
                else
                {
                    //if ((decimal.TryParse(parts[i], out thdecimal)) == false)
                    //{

                    //    row[i] = decimal.Parse(string.Format(parts[i]));

                    //}
                    //else
                    //{
                    //row[i] = string.Format(parts[i]);
                    //}
                    //if (i == 10)
                    //    {

                    //    row[i] = string.Format(parts[i].Replace(",",""));
                    //     }
                    //else
                    //{

                    //row[i] = string.Format(parts[i]);
                    //}

                    row[i] = string.Format(parts[i]);

                }


            }
            //Response.Write("<br/>");
            dataTable.Rows.Add(row);







        }
        //while (str1 != null);
        streamReader.Close();
   

        DataView view = new DataView(dataTable);
        DataTable dtQueryTable = view.ToTable(false, new string[] { "CardNo", "PIN", "Bank", "Type", "Address", "City", "AccountNo", "Country", "Remarks" });

        this.DataGrid1.DataSource =  dtQueryTable; 
        this.DataGrid1.DataBind();
        //LoadApprovingOfficers();
        //LoadApprovingOfficers(Convert.ToDecimal(DataGrid1.Attributes["total"]));
        string Left = this.ValidateEntriesPIN();

        if (string.Compare(Left, (string)null, false) == 0)
            return;
        this.Response.Write("<script>alert(' The following records are with errors !!" + Left + "') </script>");
    }

    public string ValidateEntriesPOSS()
    {
        DataSet dataSet = new DataSet();
        Decimal num = new Decimal();
        Decimal d1 = new Decimal();


        string str1 = "";
        transflag = "1";
        //luser = Request.Cookies["LogonUserID"].Value;//HttpContext.Current.User.Identity.Name;//HttpContext.Current.User.Identity.Name; //HttpContext.Current.User.Identity.Name;// HttpContext.Current.User.Identity.Name;
        //StrLocation = this.luser.Substring(this.luser.LastIndexOf("?") + 1);
        //struser = this.luser.Substring(0, this.luser.LastIndexOf("?"));
        //gUser = this.struser;
        l_totalAmt = 0;
        try
        {

            foreach (DataGridItem dataGridItem in this.DataGrid1.Items)
            {
                try
                {


                    //dataTable.Columns.Add(new DataColumn("Beneficiary AccountNo ", typeof(string)));
                    //dataTable.Columns.Add(new DataColumn("Beneficiary SortCode", typeof(string)));
                    //dataTable.Columns.Add(new DataColumn("Amount", typeof(string)));
                    //dataTable.Columns.Add(new DataColumn("Narration", typeof(string)));
                    //dataTable.Columns.Add(new DataColumn("Beneficiary AccountName", typeof(string)));
                    //dataTable.Columns.Add(new DataColumn("Beneficiary Bank", typeof(string)));
                    //dataTable.Columns.Add(new DataColumn("Remarks", typeof(string)));


                    string l_BenacctNo = (dataGridItem.Cells[0].Text); // Posting Date
                    string l_Amount = (dataGridItem.Cells[1].Text); // MerchantID
                                                                    // string l_Sortcode = (dataGridItem.Cells[2].Text); // Terminal ID
                    string l_Narration = "";
                    //if (string.IsNullOrEmpty(txtRemarks.Text) == true)
                    //{
                    //      l_Narration =  dataGridItem.Cells[2].Text.Trim().Replace("&nbsp;",""); // STAN
                    //}
                    //else
                    //{
                    //    dataGridItem.Cells[2].Text = txtRemarks.Text;
                    //    l_Narration = txtRemarks.Text;
                    //}
                    decimal amount4sum = 0;


                    #region Begin Validations check amounts
                    decimal thdecimal;
                    if ((decimal.TryParse(l_Amount.Replace(",", "").Replace(@"""", ""), out thdecimal)) == false)
                    {
                        str1 = str1 + l_Amount.Replace(",", "") + "<br>,";
                        dataGridItem.Cells[1].BackColor = Color.Red;
                        dataGridItem.Cells[1].ToolTip = "(Failed)- Invalid amount";
                        dataGridItem.Cells[4].Text = "(Failed)- Invalid amount";
                        dataGridItem.Cells[4].CssClass = Convert.ToString(0);
                        amount4sum = 0;
                        transflag = "0";

                    }
                    else
                    {
                        dataGridItem.Cells[1].Text = string.Format("{0:0,0.00}", Convert.ToDecimal(l_Amount));
                        amount4sum = Convert.ToDecimal(l_Amount);
                    }


                    #endregion


                    #region check Beneficiary narration
                    string l_BenBankName = "";
                    DataSet ds = new DataSet();
                    //string strSortcode = string.Format("SELECT * FROM Banks WHERE Sort_Code = '{0}'", l_Sortcode);
                    ////if (Classes.GetDataset(strSortcode).Tables[0].Rows.Count > 0)
                    if (string.IsNullOrEmpty(l_Narration.Trim()) == false)

                    {
                        //l_BenBankName = ds.Tables[0].Rows[0]["bank_name"].ToString();
                        //dataGridItem.Cells[5].Text = l_BenBankName;
                    }
                    else
                    {
                        str1 = str1 + l_Narration.Replace(",", "") + "<br>,";
                        dataGridItem.Cells[2].BackColor = Color.Red;
                        dataGridItem.Cells[2].ToolTip = "(Failed)- narration empty ";
                        dataGridItem.Cells[4].Text = "(Failed)- narration empty";
                        dataGridItem.Cells[4].CssClass = Convert.ToString(0);

                        transflag = "0";
                    }

                    #endregion


                    #region check Beneficiary AccountName

                    //WebNipServices.GenServices BenNameEnquiry = new WebNipServices.GenServices();
                    dynamic BenNameEnquiry = "";
                    string l_BenName = "";// BenNameEnquiry.NameEnquiry(l_BenacctNo, l_Sortcode);



                    string strRespCode = ""; string strRespDesc = "";
                    string strResults = "";

                    //strResults = iBankPoints.iPoints.ProcessLocalBenName(l_BenacctNo, "00", "001", "00", Request.Cookies["Token"].Value);
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    dynamic json = jss.DeserializeObject(strResults);
                    strRespCode = json["ResponseCode"];
                    strRespDesc = json["ResponseDesc"];
                    if (strRespCode == "00")
                    {
                        l_BenName = strRespDesc;
                        //errMsg.Text = string.Empty;
                    }







                    if (l_BenName == "")
                    {
                        str1 = str1 + l_BenacctNo.Replace(",", "") + "<br>,";
                        dataGridItem.Cells[0].BackColor = Color.Red;
                        dataGridItem.Cells[0].ToolTip = "(Failed)- Invalid Beneficiary AccountNo";
                        dataGridItem.Cells[4].Text = "(Failed)- Invalid Beneficiary AccountNo";
                        dataGridItem.Cells[4].CssClass = Convert.ToString(0);
                        transflag = "0";
                    }
                    else
                    {

                        //str1 = str1 + l_BenacctNo.Replace(",", "") + "<br>,";
                        dataGridItem.Cells[3].Text = l_BenName;

                    }

                    #endregion

                    l_totalAmt = l_totalAmt + amount4sum;


                }
                catch (Exception ex)
                {


                }
            }
            lbldebit.Text = string.Format("NGN {0:0,0.00}", l_totalAmt);
            DataGrid1.Attributes["total"] = string.Format("{0}", l_totalAmt);

        }
        //    catch(Exception ex)
        //{
        //    string error = ex.Message;

        //    }
        finally
        {
            //IEnumerator enumerator;
            //if (enumerator is IDisposable)
            //    (enumerator as IDisposable).Dispose();
        }
        return str1;
    }
    public string ValidateEntriesPIN()
    {
        DataSet dataSet = new DataSet();
        Decimal num = new Decimal();
        Decimal d1 = new Decimal();


        string str1 = "";
        transflag = "1";
        //luser = Request.Cookies["LogonUserID"].Value;//HttpContext.Current.User.Identity.Name;//HttpContext.Current.User.Identity.Name; //HttpContext.Current.User.Identity.Name;// HttpContext.Current.User.Identity.Name;
        //StrLocation = this.luser.Substring(this.luser.LastIndexOf("?") + 1);
        //struser = this.luser.Substring(0, this.luser.LastIndexOf("?"));
        //gUser = this.struser;
        l_totalAmt = 0;
        try
        {

            foreach (DataGridItem dataGridItem in this.DataGrid1.Items)
            {
                try
                {


                    //dataTable.Columns.Add(new DataColumn("Beneficiary AccountNo ", typeof(string)));
                    //dataTable.Columns.Add(new DataColumn("Beneficiary SortCode", typeof(string)));
                    //dataTable.Columns.Add(new DataColumn("Amount", typeof(string)));
                    //dataTable.Columns.Add(new DataColumn("Narration", typeof(string)));
                    //dataTable.Columns.Add(new DataColumn("Beneficiary AccountName", typeof(string)));
                    //dataTable.Columns.Add(new DataColumn("Beneficiary Bank", typeof(string)));
                    //dataTable.Columns.Add(new DataColumn("Remarks", typeof(string)));


                    string l_CardNo = (dataGridItem.Cells[0].Text);
                    string l_Pin = (dataGridItem.Cells[1].Text);
                    // Posting Date
                    string l_Amount = (dataGridItem.Cells[1].Text); // MerchantID
                                                                    // string l_Sortcode = (dataGridItem.Cells[2].Text); // Terminal ID
                    string l_Narration = "";
                    //if (string.IsNullOrEmpty(txtRemarks.Text) == true)
                    //{
                    //      l_Narration =  dataGridItem.Cells[2].Text.Trim().Replace("&nbsp;",""); // STAN
                    //}
                    //else
                    //{
                    //    dataGridItem.Cells[2].Text = txtRemarks.Text;
                    //    l_Narration = txtRemarks.Text;
                    //}
                    decimal amount4sum = 1;


                    #region Begin Validations check amounts



                    #endregion


                    #region check card details narration
                    string l_BenBankName = "";
                    DataSet ds = new DataSet();
                    l_CardNo = l_CardNo.Replace("&nbsp;", string.Empty);
                    //string strSortcode = string.Format("SELECT * FROM Banks WHERE Sort_Code = '{0}'", l_Sortcode);
                    ////if (Classes.GetDataset(strSortcode).Tables[0].Rows.Count > 0)
                    if (string.IsNullOrEmpty(l_CardNo.Trim()) == false)

                    {
                        //l_BenBankName = ds.Tables[0].Rows[0]["bank_name"].ToString();
                        //dataGridItem.Cells[5].Text = l_BenBankName;
                    }
                    else
                    {
                        str1 = string.Format("{0}{1}<br>,", str1, dataGridItem.Cells[0].Text);
                        dataGridItem.Cells[0].BackColor = Color.Red;
                        dataGridItem.Cells[0].ToolTip = "CardNo or Instrument No. empty!!";
                        dataGridItem.Cells[8].Text = "(Failed)- CardNo or Instrument No. empty!!";
                        dataGridItem.Cells[8].CssClass = Convert.ToString(0);
                        amount4sum = 0;
                        transflag = "0";
                    }

                    #endregion
                    #region check Pin Info details 
                    l_Pin = l_Pin.Replace("&nbsp;", string.Empty);
                    if (string.IsNullOrEmpty(l_Pin.Trim()) == false )

                    {
                        //l_BenBankName = ds.Tables[0].Rows[0]["bank_name"].ToString();
                        //dataGridItem.Cells[5].Text = l_BenBankName;
                    }
                    else
                    {
                        str1 = string.Format("{0}{1}<br>,", str1, dataGridItem.Cells[1].Text);
                        dataGridItem.Cells[1].BackColor = Color.Red;
                        dataGridItem.Cells[1].ToolTip = "PIN is empty!!";
                        dataGridItem.Cells[8].Text = "(Failed)- PIN is empty!!";
                        dataGridItem.Cells[8].CssClass = Convert.ToString(0);
                        amount4sum = 0;
                        transflag = "0";
                    }

                    #endregion
 

                    l_totalAmt = l_totalAmt + amount4sum;


                }
                catch (Exception ex)
                {


                }
            }
            lbldebit.Text = string.Format("NGN {0:0,0.00}", l_totalAmt);
            DataGrid1.Attributes["total"] = string.Format("{0}", l_totalAmt);

        }
        //    catch(Exception ex)
        //{
        //    string error = ex.Message;

        //    }
        finally
        {
            //IEnumerator enumerator;
            //if (enumerator is IDisposable)
            //    (enumerator as IDisposable).Dispose();
        }
        return str1;
    }

    public string ValidateEntriesPOS()
    {
        DataSet dataSet = new DataSet();
        Decimal num = new Decimal();
        Decimal d1 = new Decimal();
        decimal amount4sum = 0;
        string str1 = "";
        transflag = "1";
        try
        {
            foreach (DataGridItem dataGridItem in this.DataGrid1.Items)
            {
                try
                {
                    string str2 = (dataGridItem.Cells[0].Text); // Posting Date
                    string str3 = (dataGridItem.Cells[2].Text); // Terminal ID
                    string str4 = (dataGridItem.Cells[2].Text); // MerchantID
                    string str5 = (dataGridItem.Cells[1].Text); // STAN
                    string str14 = (dataGridItem.Cells[7].Text); // Ref No
                    string str15 = (dataGridItem.Cells[6].Text); // Card No
                    string str6 = (dataGridItem.Cells[3].Text.Replace(",", "").Replace(@"""", "")); // Amount
                    string str7 = (dataGridItem.Cells[7].Text.Replace(",", "").Replace(@"""", "")); // Charge
                    string str8 = (dataGridItem.Cells[4].Text.Replace(",", "").Replace(@"""", "")); // Amount Due
                    string str19 = (dataGridItem.Cells[3].Text.Replace(",", "").Replace(@"""", "")); // Lump  Sum 
                    string str9 = (dataGridItem.Cells[0].Text);// Settlement Date


                    string l_Amount = str8;// (dataGridItem.Cells[1].Text); // MerchantID
                    string strgetAcctNo = "";
                    if (Classes.GetDataset(string.Format("Sp_GetTerminalAcctNoInfo '{0}','{1}'", "POS", str3)).Tables[0].Rows.Count > 0)
                    {
                        strgetAcctNo = Classes.GetDataset(string.Format("Sp_GetTerminalAcctNoInfo '{0}','{1}'", "POS", str3)).Tables[0].Rows[0]["TerminalAcctNo"].ToString();
                    }
                    string strContralAcctNo = ""; string ContraAcctNo = "";
                    if (Classes.GetDataset(string.Format("Sp_GetTerminalLedgerAcctNoInfo '{0}','{1}'", "POS", "1")).Tables[0].Rows.Count > 0)
                    {
                        strContralAcctNo = Classes.GetDataset(string.Format("Sp_GetTerminalLedgerAcctNoInfo '{0}','{1}'", "POS", "1")).Tables[0].Rows[0]["TerminalLedgerCode"].ToString();
                    }
                    if (!EazyClass.AXAccountExist(strgetAcctNo))
                        str4 = NETClass.EazyClass.AXFullAccount(this.StrLocation, str3);
                    //if (!EazyClass.AXAccountExist(str8))
                    //ContraAcctNo = string.Format("{0}{1}", StrLocation, strContralAcctNo);
                    ContraAcctNo = string.Format("{0}", strContralAcctNo);// NETClass.EazyClass.AXFullAccount(StrLocation, strContralAcctNo);
                                                                          //ContraAcctNo =  NETClass.EazyClass.AXFullAccount(StrLocation, strContralAcctNo);
                    dataGridItem.Cells[8].CssClass = Convert.ToString(1);

                    bool strdateconvert = true;
                    DateTime thPostDate;
                    Int64 thNumeric;
                    decimal thdecimal;

                    if ((DateTime.TryParse(dataGridItem.Cells[0].Text, out thPostDate)) == false)
                    {
                        str1 = str1 + dataGridItem.Cells[0].Text + "<br>,";
                        dataGridItem.Cells[0].BackColor = Color.Red;
                        dataGridItem.Cells[0].ToolTip = "invalid Posting Date";
                        dataGridItem.Cells[8].Text = "(Failed)- invalid Posting Date";
                        dataGridItem.Cells[8].CssClass = Convert.ToString(0);
                        amount4sum = 0;
                        transflag = "0";
                        //goto dataGridItem;
                    }
                    if (DateTime.Compare(DateTime.Parse(str2), DateTime.Parse(Classes.AXGetDataset("SELECT PostingDate FROM PrmPostingDates WHERE Record = 1").Tables[0].Rows[0]["PostingDate"].ToString())) > 0)
                    {
                        str1 = str1 + dataGridItem.Cells[0].Text + "<br>,";
                        dataGridItem.Cells[0].BackColor = Color.Red;
                        dataGridItem.Cells[0].ToolTip = "Entry Postdate is greater than EazybankAx posting Date";
                        dataGridItem.Cells[8].Text = "( Passed )on Later date";
                        amount4sum = 0;
                        transflag = "0";
                    }
                    if ((DateTime.TryParse(dataGridItem.Cells[0].Text, out thPostDate)) == false)
                    {
                        str1 = str1 + dataGridItem.Cells[0].Text + "<br>,";
                        dataGridItem.Cells[0].BackColor = Color.Red;
                        dataGridItem.Cells[0].ToolTip = "invalid Transaction Date";
                        dataGridItem.Cells[8].Text = "(Failed)- invalid Transaction Date";
                        dataGridItem.Cells[8].CssClass = Convert.ToString(0);
                        amount4sum = 0;
                        transflag = "0";
                    }
                    if (Classes.AxGetBitField("Sproc_GetExtRefNumExist '" + (str14) + "'"))
                    {
                        str1 = str1 + dataGridItem.Cells[4].Text + "<br>,";
                        dataGridItem.Cells[7].BackColor = Color.Red;
                        dataGridItem.Cells[7].ToolTip = "Transaction Sequence Code alreday Exist";
                        dataGridItem.Cells[8].Text = "(Failed)- Transaction Sequence Code alreday Exist";
                        dataGridItem.Cells[8].CssClass = Convert.ToString(0);
                        amount4sum = 0;
                        transflag = "0";
                    }
                    string rString1 = string.Format("SELECT AccountNo FROM VwAccMaster WHERE AccountNo = '{0}'", strgetAcctNo);
                    string rString2 = string.Format("SELECT AccountNo FROM VwAccMaster WHERE AccountNo = '{0}'", ContraAcctNo);
                    string rString3 = string.Format("SELECT AccountNo FROM VwAccMaster WHERE AccountNo = '{0}' AND AccountStatus = '1' ", strgetAcctNo);
                    string rString4 = string.Format("SELECT AccountNo FROM VwAccMaster WHERE AccountNo = '{0}' AND ProdCatCode IN ('1','2','Z')", strgetAcctNo);
                    string rString5 = string.Format("SELECT AccountNo FROM VwAccMaster WHERE AccountNo = '{0}' AND ProdCatCode IN ('1','2','Z')", ContraAcctNo);
                    string rString6 = string.Format("SELECT AccountNo FROM VwAccMaster WHERE AccountNo = '{0}' AND PostingDateAdded <= '{1}' ", strgetAcctNo, Convert.ToDateTime(str2));
                    if (!Classes.AxGetBitField(rString1))
                    {
                        str1 = string.Format("{0}{1}<br>,", str1, dataGridItem.Cells[1].Text);
                        dataGridItem.Cells[1].BackColor = Color.Red;
                        dataGridItem.Cells[1].ToolTip = "Invalid EazybankAx Account for TerminalID ";
                        dataGridItem.Cells[8].Text = "(Failed)- Invalid EazybankAx Account";
                        dataGridItem.Cells[8].CssClass = Convert.ToString(0);
                        amount4sum = 0;
                        transflag = "0";
                    }
                    if (!Classes.AxGetBitField(rString2))
                    {
                        str1 = string.Format("{0}{1}<br>,", str1, dataGridItem.Cells[2].Text);
                        dataGridItem.Cells[2].BackColor = Color.Red;
                        dataGridItem.Cells[2].ToolTip = "Invalid EazybankAx ContraAccount ";
                        dataGridItem.Cells[8].Text = "(Failed)- Invalid EazybankAx ContraAccount";
                        dataGridItem.Cells[8].CssClass = Convert.ToString(0);
                        amount4sum = 0;
                        transflag = "0";
                    }
                    if (!Classes.AxGetBitField(rString3))
                    {
                        str1 = string.Format("{0}{1}<br>,", str1, dataGridItem.Cells[2].Text);
                        dataGridItem.Cells[2].BackColor = Color.Red;
                        dataGridItem.Cells[2].ToolTip = "Account is either dormant, closed or unapproved ";
                        dataGridItem.Cells[8].Text = "(Failed)- Account is either dormant, closed or unapproved";
                        dataGridItem.Cells[8].CssClass = Convert.ToString(0);
                        amount4sum = 0;
                        transflag = "0";
                    }
                    if (!Classes.AxGetBitField(rString4))
                    {
                        str1 = string.Format("{0}{1}<br>,", str1, dataGridItem.Cells[2].Text);
                        dataGridItem.Cells[2].BackColor = Color.Red;
                        dataGridItem.Cells[2].ToolTip = "Only Retail and GL Accounts are allowed in this posting screen ";
                        dataGridItem.Cells[8].Text = "(Failed)- Only Retail and GL Accounts are allowed in this posting screen";
                        dataGridItem.Cells[8].CssClass = Convert.ToString(0);
                        amount4sum = 0;
                        transflag = "0";
                    }
                    if (!Classes.AxGetBitField(rString5))
                    {
                        str1 = string.Format("{0}{1}<br>,", str1, dataGridItem.Cells[8].Text);
                        dataGridItem.Cells[2].BackColor = Color.Red;
                        dataGridItem.Cells[2].ToolTip = "Only Retail and GL Accounts are allowed in this posting screen  ";
                        dataGridItem.Cells[8].Text = "(Failed)- Only Retail and GL Accounts are allowed in this posting screen/Contra";
                        dataGridItem.Cells[8].CssClass = Convert.ToString(0);
                        amount4sum = 0;
                        transflag = "0";
                    }
                    //if (!Classes.AxGetBitField(rString6))
                    //{
                    //    str1 = string.Format("{0}{1}<br>,", str1, dataGridItem.Cells[2].Text);
                    //    dataGridItem.Cells[2].BackColor = Color.Red;
                    //    dataGridItem.Cells[2].ToolTip = "Account Opening Date cannot be later than current Posting Date ";
                    //    dataGridItem.Cells[8].Text = "(Failed)- Account Opening Date cannot be later than current Posting Date";
                    //    dataGridItem.Cells[8].CssClass = Convert.ToString(0);
                    //}
                    //if ( (Classes.AxGetField("Sproc_GetExtTransCodesExist '" + (str5) + "'")).Length <= 0)
                    //{
                    //    str1 = str1 + dataGridItem.Cells[3].Text + "<br>,";
                    //    dataGridItem.Cells[3].BackColor = Color.Red;
                    //    dataGridItem.Cells[3].ToolTip = "Explanation Code description is not yet mapped";
                    //    dataGridItem.Cells[8].Text = "(Failed)- Explanation Code description is not yet mapped";
                    //    dataGridItem.Cells[8].CssClass = Convert.ToString(0);
                    //}


                    //if ((decimal.TryParse(str7.Replace(",", "").Replace(@"""", ""), out thdecimal)) == false)
                    //{
                    //    str1 = str1 + dataGridItem.Cells[7].Text.Replace(",", "") + "<br>,";
                    //    dataGridItem.Cells[7].BackColor = Color.Red;
                    //    dataGridItem.Cells[7].ToolTip = "(Failed)- Invalid charge amount";
                    //    dataGridItem.Cells[8].Text = "(Failed)- Invalid charge amount";
                    //    dataGridItem.Cells[8].CssClass = Convert.ToString(0);
                    //}
                    if ((decimal.TryParse(str6.Replace(",", "").Replace(@"""", ""), out thdecimal)) == false)
                    {
                        str1 = str1 + dataGridItem.Cells[6].Text + "<br>,";
                        dataGridItem.Cells[4].BackColor = Color.Red;
                        dataGridItem.Cells[4].ToolTip = "(Failed)- Invalid  amount";
                        dataGridItem.Cells[8].Text = "(Failed)- Invalid  amount";
                        dataGridItem.Cells[8].CssClass = Convert.ToString(0);
                        amount4sum = 0;
                        transflag = "0";
                    }
                    if (string.Compare((str4), "", false) == 0)
                    {
                        str1 = str1 + dataGridItem.Cells[2].Text + "<br>,";
                        dataGridItem.Cells[2].BackColor = Color.Red;
                        dataGridItem.Cells[2].ToolTip = "Entry already posted before upload";
                        dataGridItem.Cells[8].Text = "(Failed)- Entry already posted before upload";
                        dataGridItem.Cells[8].CssClass = Convert.ToString(0);
                        amount4sum = 0;
                        transflag = "0";
                    }

                    if ((decimal.TryParse(l_Amount.Replace(",", "").Replace(@"""", ""), out thdecimal)) == false)
                    {
                        amount4sum = 0;
                    }
                    else
                    {
                        dataGridItem.Cells[4].Text = string.Format("{0:0,0.00}", Convert.ToDecimal(l_Amount));
                        amount4sum = Convert.ToDecimal(l_Amount);
                    }
                    //if  (transflag == "1")
                    //    {


                    //    }
                    //else { 

                    //     }

                    l_totalAmt = l_totalAmt + amount4sum;
                    Decimal d2 = Convert.ToDecimal(str7);
                    if (Decimal.Compare(d1, d2) < 0)
                        d1 = d2;
                }
                catch (Exception ex)
                {


                }
            }
            lbldebit.Text = string.Format("NGN {0:0,0.00}", l_totalAmt);
            DataGrid1.Attributes["total"] = string.Format("{0}", l_totalAmt);
        }


        //    catch(Exception ex)
        //{
        //    string error = ex.Message;

        //    }
        finally
        {
            //IEnumerator enumerator;
            //if (enumerator is IDisposable)
            //    (enumerator as IDisposable).Dispose();
        }
        return str1;
    }

    protected void Button4_Click(object sender, EventArgs e)
    {
        Response.Redirect("BulkFTLocal.aspx");
    }

    protected void ASPxComboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        //LoadAcctDetails(ASPxComboBox1.Value.ToString());
    }

    string LoadAcctDetails(string strAccountNo)
    {


        DataTable dsAcctList = new DataTable();
        //dsAcctList = iBankPoints.iPoints.ProcessUserAcctList(Request.Cookies["eLogonUserID"].Value, lblAccountNo.Text, Request.Cookies["Token"].Value);


        string expression;  // expression = "Date > #1/1/00#";
        expression = string.Format("AccountNo  = {0}", strAccountNo);

        DataRow[] foundRows;
        foundRows = dsAcctList.Select(expression);
        foreach (DataRow item in foundRows)
        {
            //lblaccountName.Text = "AccountName";
            //lblaccountNameVal.Text = item["AccountDesc"].ToString();

            //lblProd.Text = "Product";
            //lblProdVal.Text = item["ProdName"].ToString();
            //lblStatus.Text = "Account Status";
            //lblStatusVal.Text = item["AccountStatusDesc"].ToString();
            //lblAvailBal.Text = "Available Balance";
            lblAvailBal.Text = string.Format("NGN {0:0,0.00}", item["AvailBal"]);
            //lblbookbal.Text = "BookBal Balance";
            //lblAvailBal.Text = string.Format("NGN {0:0,0.00}", item["Bookbal"]);
            //lblOfficer.Text = "Account Officer";
            //lblOfficerVal.Text = item["RelationshipOfficerName"].ToString();

        }


        return "";
    }


    protected void Button3_Click(object sender, EventArgs e)
    {
        luser = Request.Cookies["LogonUserID"].Value;// HttpContext.Current.User.Identity.Name;//HttpContext.Current.User.Identity.Name; //HttpContext.Current.User.Identity.Name;// HttpContext.Current.User.Identity.Name;
        StrLocation = this.luser.Substring(this.luser.LastIndexOf("?") + 1);
        struser = this.luser.Substring(0, this.luser.LastIndexOf("?"));
        gUser = this.struser;

        transflag = "1";
        string Left = this.ValidateEntriesPIN();
        //if (Left == "")
        //    {
        //    transflag = "1";
        //    }
       
        if (string.Compare((Request.Cookies["LogonUserID"].Value), "", false) != 0)
        {

            if (transflag == "1")
            {
                //Generate New Transaction Reference details 
                //if (double.Parse(DataGrid1.Attributes["total"]) > 0)
                {
                    #region  section input transaction info

                    if (txtref_no.Text.Trim() == "")
                    {
                        //string RequestTimeStamp = DateTime.Now.ToString("yyyyMMdd'T'HH':'mm:ss+0000");
                        string RequestTimeStamp = DateTime.Now.ToString("yyyyMMddHHmm");
                        string TransType = "105";
                        string RefNoSub = Guid.NewGuid().ToString().Substring(0, 8).ToUpper().Replace("-", "");
                        string strBrand = System.Configuration.ConfigurationManager.ConnectionStrings["CompanyBrand"].ConnectionString;
                        string strBatchRefNo = string.Format("{0}{1}", RefNoSub, RequestTimeStamp);

                        txtref_no.Text = strBatchRefNo;


                        {
                            



                            string str1 = this.Request.ServerVariables["REMOTE_ADDR"];
                            string hostName = Dns.GetHostName();
                            string userHostAddress = this.Request.UserHostAddress;
                            string str255 = Dns.GetHostAddresses(hostName).GetValue(1).ToString();
                            Classes.alertwindow(this.Page, "A New Batch Reference Generated : - " + this.txtref_no.Text, "Halt");
 
                            try
                            {
                                foreach (DataGridItem dataGridItem in this.DataGrid1.Items)
                                {

                                    string str2 = (dataGridItem.Cells[0].Text); // cardno
                                    string str3 = (dataGridItem.Cells[1].Text); // Pin
                                    string str4 = (dataGridItem.Cells[6].Text); // AccountNo
 

                                string strLdAppr = string.Format("SpAddPINRequest '{0}','{1}','{2}','{3}','{4}'," +
                                    "0,9115", txtref_no.Text, str2, str3, str4,gUser);
                                    Classes.GetDataset(strLdAppr);
                                }
                                DataGrid1.DataBind();
                                txtref_no.Text = "";

                            }
                           
                            finally
                            {
                                //IEnumerator enumerator;
                                //if (enumerator is IDisposable)
                                //    (enumerator as IDisposable).Dispose();
                            }


                        }





                    }
                    else//use existing transaction details
                    {

                    }
                    #endregion
                }

                //else
                //{
                //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "eBizMessage", "alert('No Transaction Uploaded')", true);
                //    //this.Response.Write("<script>alert('No Transaction Uploaded') </script>");
                //}
            }
            else
            {
                Response.Write("<script>alert(' Transactions can not be committed with errors, try the upload again ') </script>");

            }
        }
        else
        {
            this.Response.Write("<script>alert(' Your user section has expired, please logout and try the upload again ') </script>");
        }

    }

    public static string MessageBox(System.Web.UI.Page page, string strMsg)
    {
        //+ character added after strMsg "')"
        ScriptManager.RegisterClientScriptBlock(page, page.GetType(), "iBankMessage", "alert('" + strMsg + "')", true);
        return "true";
    }
    public static string alertwindow(System.Web.UI.Page aspxpage, string Msgger, string desc)
    {
        //Msgger = """" & Msgger & """"
        //'Msgger = "" & Msgger & ""   javastring.Append("alert(" & Msgger & ");")
        string strInfoAlert = string.Format("Alert('{0}');", Msgger);

        System.Text.StringBuilder javastring = new System.Text.StringBuilder();

        javastring.Append("<script language='Javascript'>");

        //javastring.Append("alert(' & Msgger & ");");
        javastring.Append(strInfoAlert);
        javastring.Append("windows.location='");
        javastring.Append("Urlstring:");
        javastring.Append("';");
        javastring.Append("</script>");


        if (aspxpage.ClientScript.IsClientScriptBlockRegistered(desc) == false)
        {
            aspxpage.ClientScript.RegisterClientScriptBlock(aspxpage.GetType(), desc, javastring.ToString());
        }


        return "";// True
    }

    private void LoadApprovingOfficers()
    {
        luser = HttpContext.Current.User.Identity.Name;// HttpContext.Current.User.Identity.Name; //HttpContext.Current.User.Identity.Name;// HttpContext.Current.User.Identity.Name;
        StrLocation = luser.Substring(this.luser.LastIndexOf("?") + 1);
        struser = luser.Substring(0, this.luser.LastIndexOf("?"));
        gUser = this.struser;
        string strTransTypeCode = "001";
        //string strDefaultTRansID = "BATCH-000000001";
        string strDefaultTRansID = "BATCH-00000627";
        DataSet ds = new DataSet();

        string getAppr = string.Format("SpUserMasterTransApprMems '{0}','{1}','{2}',{3},'{4}'", strTransTypeCode, StrLocation, gUser, 0, strDefaultTRansID);
        ds = Classes.AXGetDataset(string.Format("SpUserMasterTransApprMems '{0}','{1}','{2}',{3},'{4}'", strTransTypeCode, StrLocation, gUser, 0, strDefaultTRansID));
        offr2.DataSource = ds;
        offr2.DataTextField = "UserName";
        offr2.DataValueField = "UserID";
        offr2.DataBind();


    }
}