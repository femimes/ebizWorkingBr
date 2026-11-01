
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
using Newtonsoft.Json;
using iBankPoints;
 
using System.Web.Script.Serialization;

public partial class Bulk_BulkFTLocal : System.Web.UI.Page
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
        hypdashboard.NavigateUrl = "../mainpage/homeboard.aspx";
        ks = iBankPoints.iPoints.ProcessProfileds(Request.Cookies["eLogonUserID"].Value, Request.Cookies["Token"].Value);
        foreach (DataRow item in ks.Rows)
        {
            lblAccountNo.Text = item["AccountNo"].ToString();
        }
        lblUser.Text = Request.Cookies["eLogonUserID"].Value;

        lbldebit.Text = string.Format("NGN {0:0,0.00}", 0);
        lblAvailBal.Text = string.Format("NGN {0:0,0.00}", 0);

        ASPxComboBox1.Value = lblAccountNo.Text;
        LoadAcctDetails(lblAccountNo.Text);
        DataGrid1.Attributes["total"] = string.Format("{0}", "0");

    }

    protected void btnImport_Click(object sender, EventArgs e)
    {
        try
        {
            string str = this.UploadMultipleFiles_Clicked();

            if (string.Compare(str, (string)null, false) == 0)
                return;


           // if (string.IsNullOrEmpty (Request.Cookies["LogonUserID"].Value) == false)
                if (string.IsNullOrEmpty(str) == false)
                {
                InsertAllRecordsTrans(str);
                }
            else
                return;// this.InsertAllRecordHr(str);

        }
        catch (Exception ex)
        {

            this.Response.Write(String.Format("<script>alert('Invalid file format {0}') </script>", ex.Message));

        }

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


        dataTable.Columns.Add(new DataColumn("Beneficiary AccountNo ", typeof(string)));
        //dataTable.Columns.Add(new DataColumn("Beneficiary SortCode", typeof(string)));
        dataTable.Columns.Add(new DataColumn("Amount", typeof(string)));
        dataTable.Columns.Add(new DataColumn("Narration", typeof(string)));
        dataTable.Columns.Add(new DataColumn("Beneficiary AccountName", typeof(string)));
        //dataTable.Columns.Add(new DataColumn("Beneficiary Bank", typeof(string)));
        dataTable.Columns.Add(new DataColumn("Remarks", typeof(string)));
        string str1;
        while ((str1 = streamReader.ReadLine()) != null)
        {
            //str1 = streamReader.ReadLine();
            //string.Len(str1);


            //char[] delimiters = new char[] { ',' ,'\t'};

            char[] delimiters = new char[] { '\t', ',' };
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

                    row[i] = string.Format(parts[i]);

                }


            }
            //Response.Write("<br/>");
            dataTable.Rows.Add(row);







        }
        //while (str1 != null);
        streamReader.Close();

        this.DataGrid1.DataSource = dataTable;
        this.DataGrid1.DataBind();
        //LoadApprovingOfficers(Convert.ToDecimal(DataGrid1.Attributes["total"]));
        string Left = this.ValidateEntriesPOS();

        if (string.Compare(Left, (string)null, false) == 0)
            return;
        this.Response.Write("<script>alert(' The following records are with errors !!" + Left + "') </script>");
    }

    public string ValidateEntriesPOS()
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
                    if (string.IsNullOrEmpty(txtRemarks.Text) == true)
                    {
                          l_Narration =  dataGridItem.Cells[2].Text.Trim().Replace("&nbsp;",""); // STAN
                    }
                    else
                    {
                        dataGridItem.Cells[2].Text = txtRemarks.Text;
                        l_Narration = txtRemarks.Text;
                    }
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
                    dynamic BenNameEnquiry ="";
                    string l_BenName = "";// BenNameEnquiry.NameEnquiry(l_BenacctNo, l_Sortcode);



                    string strRespCode = ""; string strRespDesc = "";
                    string strResults = "";
                    
                    strResults = iBankPoints.iPoints.ProcessLocalBenName(l_BenacctNo, "00", "001", "00", Request.Cookies["Token"].Value);
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


    protected void Button4_Click(object sender, EventArgs e)
    {
        Response.Redirect("BulkFTLocal.aspx");
    }

    protected void ASPxComboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadAcctDetails(ASPxComboBox1.Value.ToString());
    }

    string LoadAcctDetails(string strAccountNo)
    {


        DataTable dsAcctList = new DataTable();
        dsAcctList = iBankPoints.iPoints.ProcessUserAcctList(Request.Cookies["eLogonUserID"].Value, lblAccountNo.Text, Request.Cookies["Token"].Value);


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
        //luser = Request.Cookies["LogonUserID"].Value;// HttpContext.Current.User.Identity.Name;//HttpContext.Current.User.Identity.Name; //HttpContext.Current.User.Identity.Name;// HttpContext.Current.User.Identity.Name;
        //StrLocation = this.luser.Substring(this.luser.LastIndexOf("?") + 1);
        //struser = this.luser.Substring(0, this.luser.LastIndexOf("?"));
        //gUser = this.struser;
        string Left = this.ValidateEntriesPOS();
        if (string.Compare((Request.Cookies["eLogonUserID"].Value), "", false) != 0)
        {

            if (transflag == "1")
            { 
            //Generate New Transaction Reference details 
                if  ( double.Parse(DataGrid1.Attributes["total"]) > 0)
            { 
                #region  section input transaction info

                if (txtref_no.Text.Trim() == "")
                {
                        //string RequestTimeStamp = DateTime.Now.ToString("yyyyMMdd'T'HH':'mm:ss+0000");
                        string RequestTimeStamp = DateTime.Now.ToString("yyyyMMddHHmm");
                        string TransType = "105";
                        string RefNoSub = Guid.NewGuid().ToString().Substring(0, 8).ToUpper().Replace("-", "");
                        string strBrand = System.Configuration.ConfigurationManager.ConnectionStrings["CompanyBrand"].ConnectionString;
                        string strBatchRefNo = string.Format("{0}{1}",  RefNoSub, RequestTimeStamp);


                        txtref_no.Text = strBatchRefNo;// Guid.NewGuid().ToString();
                    string str1 = this.Request.ServerVariables["REMOTE_ADDR"];
                    string hostName = Dns.GetHostName();
                    string userHostAddress = this.Request.UserHostAddress;
                    string str21 = Dns.GetHostAddresses(hostName).GetValue(1).ToString();
                    MessageBox(this.Page, "A New Batch Reference Generated : - " + this.txtref_no.Text);
                    //    Classes.RecordOp(string.Format("Sproc_AddJournal '{0}','Data Upload Transaction',{1},'N','{2}','{3}',0,'{4}','{5}','{6}'", txtref_no.Text, Decimal.Zero, gUser, StrLocation, offr2.SelectedValue, hostName, str21));





                    #region section insert Recs

                    try
                    {
                        foreach (DataGridItem dataGridItem in this.DataGrid1.Items)
                        {



                            string l_BenacctNo = (dataGridItem.Cells[0].Text); // Posting Date
                            string l_BenacctName = (dataGridItem.Cells[3].Text); // Posting Date
                            //string l_Sortcode = (dataGridItem.Cells[1].Text); // Terminal ID
                            string l_Amount = (dataGridItem.Cells[1].Text); // MerchantID
                            string l_Narration = (dataGridItem.Cells[2].Text); // STAN
                            decimal amount4sum = 0;
                                //string l_AccountNo = drptranstype.SelectedValue;
                                //string l_AccountName = Classes.AXGetDataset(string.Format(" SELECT dbo.FnAccountName('{0}')", l_AccountNo)).Tables[0].Rows[0][0].ToString();

                                ////determine the transaction fee amount
                                //string feecharge = Classes.GetDataset(string.Format("SpNapsGetFeeCharge {0}", l_Amount)).Tables[0].Rows[0][0].ToString();

                                #region Save Records

                                string strSortCode = txtref_no.Text; string TransRefNoVal = "";
                                string RefNo = "";
                                string strAccountName = "";
                                //drpAccountNo.Columns[2].Visible = true;
                                strAccountName = ASPxComboBox1.SelectedItem.GetValue("AccountDesc").ToString();
                                RefNo = iPoints.ProcessTempTrans(l_BenacctNo, strSortCode, iPoints.GetIPAddress(),
                            "", ASPxComboBox1.Value.ToString(), strAccountName, decimal.Parse(l_Amount), 0, l_BenacctName, l_Narration, strSortCode, Request.Cookies["eLogonUserID"].Value, strBatchRefNo, TransType, Request.Cookies["Token"].Value);
                                JavaScriptSerializer jssSeqRef = new JavaScriptSerializer();
                                dynamic jsonSeqRef = jssSeqRef.DeserializeObject(RefNo);
                                TransRefNoVal = jsonSeqRef["ResponseCode"];

                               
 

                                #endregion




                                #region Insert Records into NAPs Entry table

                                #region Save Records

                                //SqlConnection connNapsHist = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString);


                                //SqlCommand commandNapsHist = new SqlCommand();
                                //connNapsHist.Open();
                                //commandNapsHist.CommandText = "SpAddNapsExtTransNapsHistBulk";
                                //commandNapsHist.Connection = connNapsHist;
                                //commandNapsHist.CommandType = CommandType.StoredProcedure;
                                //commandNapsHist.Parameters.Add(new SqlParameter("@MobileNo", drptranstype.SelectedValue));
                                //commandNapsHist.Parameters.Add(new SqlParameter("@AccountNo", l_AccountNo));
                                //commandNapsHist.Parameters.Add(new SqlParameter("@Postdate", DateTime.Today));
                                //commandNapsHist.Parameters.Add(new SqlParameter("@BeneficiaryAcctNo", l_BenacctNo));
                                //commandNapsHist.Parameters.Add(new SqlParameter("@BeneficiaryName", l_BenacctName));
                                //commandNapsHist.Parameters.Add(new SqlParameter("@Narration", l_Narration));
                                //commandNapsHist.Parameters.Add(new SqlParameter("@RequestTitle", txtTitle.Text));
                                //commandNapsHist.Parameters.Add(new SqlParameter("@CustAcctNo", txtref_no.Text));

                                //commandNapsHist.Parameters.Add(new SqlParameter("@Amount", l_Amount));
                                //commandNapsHist.Parameters.Add(new SqlParameter("@FeeAmt", feecharge));
                                //commandNapsHist.Parameters.Add(new SqlParameter("@TransCode", 863));

                                //commandNapsHist.Parameters.Add(new SqlParameter("@AgentID", l_AccountNo));
                                //commandNapsHist.Parameters.Add(new SqlParameter("@TransRefNo", ""));
                                //commandNapsHist.Parameters.Add(new SqlParameter("@SortCode", l_Sortcode));

                                //commandNapsHist.Parameters.Add(new SqlParameter("@Postedby", gUser));

                                //commandNapsHist.Parameters.Add(new SqlParameter("@ApprovedBy", offr2.SelectedValue));
                                //commandNapsHist.Parameters.Add(new SqlParameter("@ConfirmBy", ""));
                                //commandNapsHist.Parameters.Add(new SqlParameter("@TransRemarks", "Bulk entry"));
                                //commandNapsHist.Parameters.Add(new SqlParameter("@ProfileID", RefNo));
                                //commandNapsHist.Parameters.Add(new SqlParameter("@SystemIP", userHostAddress));
                                //commandNapsHist.Parameters.Add(new SqlParameter("@SystemName", hostName));

                                //commandNapsHist.ExecuteNonQuery();
                                //connNapsHist.Close();




                                #endregion

                                #endregion





                            }

                            errMsg.Text = "Successful";
                        }
                    catch (Exception ex)
                    {
                            errMsg.Text = ex.Message;
                            //throw;
                    }



                    #endregion

                }
                else//use existing transaction details
                {

                }
                #endregion
            }

                else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "iBankMessage", "alert('No Transaction Uploaded')", true);
                //this.Response.Write("<script>alert('No Transaction Uploaded') </script>");
            }
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
    public static string  alertwindow(System.Web.UI.Page aspxpage, string Msgger, string desc )
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



}