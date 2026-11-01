
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
 
public partial class eOperations_FTBulkAcct : System.Web.UI.Page
{


    public string gUser =  HttpContext.Current.User.Identity.Name;//HttpContext.Current.User.Identity.Name;
    public string luser = "";
    public string StrLocation = "";
    public string struser = "";
    public decimal l_totalAmt = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.IsCallback || this.IsPostBack)
            return;
        this.luser =  Request.Cookies["LogonUserID"].Value;//HttpContext.Current.User.Identity.Name;//HttpContext.Current.User.Identity.Name; //HttpContext.Current.User.Identity.Name;// HttpContext.Current.User.Identity.Name;
        StrLocation = luser.Substring(this.luser.LastIndexOf("?") + 1);
         struser = luser.Substring(0, this.luser.LastIndexOf("?"));
        gUser =  struser;
        DataGrid1.Attributes["total"] = string.Format("{0}", "0");
        //this.txtuserID.Text = this.gUser;
        //this.ctrlGrid.PageIndex = 0;
        //this.DataBind(); 
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

    private void LoadApprovingOfficers(decimal  Amount)
    {
        luser = Request.Cookies["LogonUserID"].Value;//HttpContext.Current.User.Identity.Name;// HttpContext.Current.User.Identity.Name; //HttpContext.Current.User.Identity.Name;// HttpContext.Current.User.Identity.Name;
        StrLocation = luser.Substring(this.luser.LastIndexOf("?") + 1);
        struser = luser.Substring(0, this.luser.LastIndexOf("?"));
        gUser = this.struser;
        string strTransTypeCode = "Bulk Pymts";
        //string strDefaultTRansID = "BATCH-000014261";
        DataSet ds = new DataSet();
        string getAppr = string.Format("SpNapsGetUserLimitsByAppr '{0}',{1},'{2}','{3}'", gUser, Amount, StrLocation, strTransTypeCode);
     
        //
        ds = Classes.GetDataset(getAppr);

       // ds = Classes.AXGetDataset(string.Format("SpUserMasterTransApprMems '{0}','{1}','{2}',{3},'{4}'", strTransTypeCode, StrLocation, gUser, 0, strDefaultTRansID));
        offr2.DataSource = ds;
        offr2.DataTextField = "user_names";
        offr2.DataValueField = "User_id";
        offr2.DataBind();


    }

    public void InsertAllRecordsTrans(string m_strFileName)
    {

        this.luser = Request.Cookies["LogonUserID"].Value;// HttpContext.Current.User.Identity.Name;//HttpContext.Current.User.Identity.Name; //HttpContext.Current.User.Identity.Name;// HttpContext.Current.User.Identity.Name;
        StrLocation = this.luser.Substring(this.luser.LastIndexOf("?") + 1);
        this.struser = this.luser.Substring(0, this.luser.LastIndexOf("?"));
        this.gUser = this.struser;

        StreamReader streamReader = new StreamReader("C:\\TempMultipleFiles\\" + m_strFileName);
        DataSet dataSet = new DataSet();
        DataTable dataTable = new DataTable();
   

        dataTable.Columns.Add(new DataColumn("Beneficiary AccountNo ", typeof(string)));
        dataTable.Columns.Add(new DataColumn("Beneficiary SortCode", typeof(string)));
        dataTable.Columns.Add(new DataColumn("Amount", typeof(string)));
        dataTable.Columns.Add(new DataColumn("Narration", typeof(string)));
        dataTable.Columns.Add(new DataColumn("Beneficiary AccountName", typeof(string)));
        dataTable.Columns.Add(new DataColumn("Beneficiary Bank", typeof(string)));
        dataTable.Columns.Add(new DataColumn("Remarks", typeof(string)));
        string str1;
        while ((str1 = streamReader.ReadLine()) != null)
        {
            //str1 = streamReader.ReadLine();
            //string.Len(str1);


            //char[] delimiters = new char[] { ',' ,'\t'};

            char[] delimiters = new char[] { '\t',',' };
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
        LoadApprovingOfficers(Convert.ToDecimal(DataGrid1.Attributes["total"]));
        string Left = this.ValidateEntriesPOS();

        if (string.Compare(Left, (string)null, false) == 0)
            return;
        this.Response.Write("<script>alert(' The following entries are with errors !!" + Left + "') </script>");
    }

    public string ValidateEntriesPOS()
    {
        DataSet dataSet = new DataSet();
        Decimal num = new Decimal();
        Decimal d1 = new Decimal();
        

        string str1 = "";
        string transflag = "1";
        luser = Request.Cookies["LogonUserID"].Value;//HttpContext.Current.User.Identity.Name;//HttpContext.Current.User.Identity.Name; //HttpContext.Current.User.Identity.Name;// HttpContext.Current.User.Identity.Name;
        StrLocation = this.luser.Substring(this.luser.LastIndexOf("?") + 1);
        struser = this.luser.Substring(0, this.luser.LastIndexOf("?"));
        gUser = this.struser;
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
                    string l_Sortcode = (dataGridItem.Cells[1].Text); // Terminal ID
                    string l_Amount = (dataGridItem.Cells[2].Text); // MerchantID
                    string l_Narration = (dataGridItem.Cells[3].Text); // STAN
                    decimal amount4sum = 0;
                     

                    #region Begin Validations check amounts
                    decimal thdecimal;
                    if ((decimal.TryParse(l_Amount.Replace(",", "").Replace(@"""", ""), out thdecimal)) == false)
                    {
                        str1 = str1 + l_Amount.Replace(",", "") + "<br>,";
                        dataGridItem.Cells[2].BackColor = Color.Red;
                        dataGridItem.Cells[2].ToolTip = "(Failed)- Invalid amount";
                        dataGridItem.Cells[6].Text = "(Failed)- Invalid amount";
                        dataGridItem.Cells[6].CssClass = Convert.ToString(0);
                        amount4sum = 0;
                        transflag = "0";

                    }
                    else
                    {
                        amount4sum = Convert.ToDecimal(l_Amount);
                    }


                    #endregion


                    #region check Beneficiary Sort Code
                    string l_BenBankName = "";
                    string strSortcode = string.Format("SELECT * FROM Banks WHERE Sort_Code = '{0}'", l_Sortcode);
                    if (Classes.GetDataset(strSortcode).Tables[0].Rows.Count > 0)
                    {
                        l_BenBankName = Classes.GetDataset(strSortcode).Tables[0].Rows[0]["bank_name"].ToString();
                        dataGridItem.Cells[5].Text = l_BenBankName;
                    }
                    else
                    {
                        str1 = str1 + l_Sortcode.Replace(",", "") + "<br>,";
                        dataGridItem.Cells[1].BackColor = Color.Red;
                        dataGridItem.Cells[1].ToolTip = "(Failed)- Invalid SortCode";
                        dataGridItem.Cells[6].Text = "(Failed)- Invalid SortCode";
                        dataGridItem.Cells[6].CssClass = Convert.ToString(0);
                        
                        transflag = "0";
                    }

                    #endregion


                    #region check Beneficiary AccountName

                    WebNipServices.GenServices BenNameEnquiry = new WebNipServices.GenServices();
                    string l_BenName = BenNameEnquiry.NameEnquiry(l_BenacctNo, l_Sortcode);
                    if (l_BenName == "")
                    {
                        str1 = str1 + l_BenacctNo.Replace(",", "") + "<br>,";
                        dataGridItem.Cells[0].BackColor = Color.Red;
                        dataGridItem.Cells[0].ToolTip = "(Failed)- Invalid Beneficiary AccountNo";
                        dataGridItem.Cells[6].Text = "(Failed)- Invalid Beneficiary AccountNo";
                        dataGridItem.Cells[6].CssClass = Convert.ToString(0);
                        transflag = "0";
                    }
                    else
                    {

                        //str1 = str1 + l_BenacctNo.Replace(",", "") + "<br>,";
                        dataGridItem.Cells[4].Text = l_BenName;
                       
                    }

                    #endregion

                    l_totalAmt = l_totalAmt + amount4sum;
                     
                }
                catch (Exception ex)
                {


                }
            }

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




    protected void Button1_Click(object sender, EventArgs e)
    {

        //WebReference.PitchControlCenterWithXML ks = new WebReference.PitchControlCenterWithXML();
        //string strval = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?><PaymentRequest><Header>  <FileName>0001</FileName>  <ScheduleId>ERCAS_01082  170228133318800005</ScheduleId>  <DebitSortCode>050</DebitSortCode>  <DebitAccountNumber>0123456789</DebitAccountNumber>  <ClientId>ERCAS_01</ClientId></Header><PaymentRecord>  <Beneficiary>OLAKUNRI, ENIOLA</Beneficiary>  <Amount>300.00</Amount>  <AccountNumber>0000080893</AccountNumber>  <SortCode>082  </SortCode>  <Narration>test</Narration></PaymentRecord><HashValue /></PaymentRequest>";
        //Response.Write(strval);
        //string getValidations = ks.uploadPaymentSchedule(strval);
        //Response.Write(getValidations);

        //string strResustatus = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?><StatusRequest><Header><ScheduleId>ERCAS_01082  170228133318800005</ScheduleId><ClientId>ERCAS_01</ClientId></Header><HashValue></HashValue></StatusRequest>";
        //string getStatus = ks.getPaymentScheduleStatus(strResustatus);
        //Response.Write("</br>");
        //Response.Write(getStatus);



        try
        {
            string str = this.UploadMultipleFiles_Clicked();

            if (string.Compare(str, (string)null, false) == 0)
                return;
            //if (Convert.ToDouble(this.drptranstype.SelectedValue) == 2.0)
            //    return; //this.InsertAllRecords(str);
            //else if (Convert.ToDouble(this.drptranstype.SelectedValue) == 3.0)
            //    return;// this.InsertAllRecordsPromo(str);
            //else if (Convert.ToDouble(this.drptranstype.SelectedValue) == 4.0)
            //    return;//  this.InsertAllRecordsPromoAccts(str);
            //else if (Convert.ToDouble(this.drptranstype.SelectedValue) == 7.0)
            //    return;//    this.InsertAllRecords(str);
            //else if (Convert.ToDouble(this.drptranstype.SelectedValue) == 11.0)
            //    return;//   this.InsertAllRecordsTrans(str);

            if (this.drptranstype.SelectedValue != "0")
            {
                this.InsertAllRecordsTrans(str);
            }
             else
                return;// this.InsertAllRecordHr(str);

        }
        catch (Exception ex)
        {

            this.Response.Write(String.Format("<script>alert('Invalid file format {0}') </script>", ex.Message));

        }
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        luser = Request.Cookies["LogonUserID"].Value;// HttpContext.Current.User.Identity.Name;//HttpContext.Current.User.Identity.Name; //HttpContext.Current.User.Identity.Name;// HttpContext.Current.User.Identity.Name;
        StrLocation = this.luser.Substring(this.luser.LastIndexOf("?") + 1);
        struser = this.luser.Substring(0, this.luser.LastIndexOf("?"));
        gUser = this.struser;
        if (string.Compare((gUser), "", false) != 0)
        {

            if (Convert.ToDouble(this.drptranstype.SelectedValue) != 0)

            {
                //Generate New Transaction Reference details 
                #region  section input transaction info

                if (txtref_no.Text.Trim()  == "")
                {
                    txtref_no.Text = Guid.NewGuid().ToString();
                    string str1 = this.Request.ServerVariables["REMOTE_ADDR"];
                    string hostName = Dns.GetHostName();
                    string userHostAddress = this.Request.UserHostAddress;
                    string str21 = Dns.GetHostAddresses(hostName).GetValue(1).ToString();
                    Classes.alertwindow(this.Page, "A New Batch Reference Generated : - " + this.txtref_no.Text, "Halt");
                //    Classes.RecordOp(string.Format("Sproc_AddJournal '{0}','Data Upload Transaction',{1},'N','{2}','{3}',0,'{4}','{5}','{6}'", txtref_no.Text, Decimal.Zero, gUser, StrLocation, offr2.SelectedValue, hostName, str21));




                  
                    #region section insert Recs

                    try
                    {
                        foreach (DataGridItem dataGridItem in this.DataGrid1.Items)
                        {
                            


                            string l_BenacctNo = (dataGridItem.Cells[0].Text); // Posting Date
                            string l_BenacctName = (dataGridItem.Cells[4].Text); // Posting Date
                            string l_Sortcode = (dataGridItem.Cells[1].Text); // Terminal ID
                            string l_Amount = (dataGridItem.Cells[2].Text); // MerchantID
                            string l_Narration = (dataGridItem.Cells[3].Text); // STAN
                            decimal amount4sum = 0;
                            string l_AccountNo = drptranstype.SelectedValue;
                            string l_AccountName = Classes.AXGetDataset(string.Format(" SELECT dbo.FnAccountName('{0}')", l_AccountNo)).Tables[0].Rows[0][0].ToString();

                            //determine the transaction fee amount
                            string feecharge = Classes.GetDataset(string.Format("SpNapsGetFeeCharge {0}", l_Amount)).Tables[0].Rows[0][0].ToString();

                            #region Save Records

                            SqlConnection connection2 = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString);


                            SqlCommand commandIns = new SqlCommand();
                            connection2.Open();
                            commandIns.CommandText = "SpAddNapsExtTransNapsDetWithBatch";
                            commandIns.Connection = connection2;
                            commandIns.CommandType = CommandType.StoredProcedure;
                            commandIns.Parameters.Add(new SqlParameter("@AccountNo", l_AccountNo));
                            commandIns.Parameters.Add(new SqlParameter("@AccountName", l_AccountName));
                            commandIns.Parameters.Add(new SqlParameter("@BenAccountNo", l_BenacctNo));
                            commandIns.Parameters.Add(new SqlParameter("@BenAccountName", l_BenacctName));
                            commandIns.Parameters.Add(new SqlParameter("@BenBankCode", l_Sortcode));
                            commandIns.Parameters.Add(new SqlParameter("@Amount", l_Amount));
                            commandIns.Parameters.Add(new SqlParameter("@FeeAmount", feecharge));
                            commandIns.Parameters.Add(new SqlParameter("@Narration", l_Narration));
                            commandIns.Parameters.Add(new SqlParameter("@Postdate", DateTime.Today));
                            commandIns.Parameters.Add(new SqlParameter("@Postedby", gUser));
                            commandIns.Parameters.Add(new SqlParameter("@BatchRefNo", txtref_no.Text));
                            commandIns.Parameters.Add(new SqlParameter("@SystemIP", userHostAddress));
                            SqlParameter parameter = commandIns.Parameters.Add(new SqlParameter("@SeqID", SqlDbType.UniqueIdentifier));
                            parameter.Direction = ParameterDirection.Output;

                            commandIns.ExecuteNonQuery();
                            connection2.Close();

                            string RefNo = parameter.Value.ToString();

                            #endregion     
                    
                    

                            
                            #region Insert Records into NAPs Entry table

                            #region Save Records

                            SqlConnection connNapsHist = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString);


                            SqlCommand commandNapsHist = new SqlCommand();
                            connNapsHist.Open();
                            commandNapsHist.CommandText = "SpAddNapsExtTransNapsHistBulk";
                            commandNapsHist.Connection = connNapsHist;
                            commandNapsHist.CommandType = CommandType.StoredProcedure;
                            commandNapsHist.Parameters.Add(new SqlParameter("@MobileNo", drptranstype.SelectedValue));
                            commandNapsHist.Parameters.Add(new SqlParameter("@AccountNo", l_AccountNo));
                            commandNapsHist.Parameters.Add(new SqlParameter("@Postdate", DateTime.Today));
                            commandNapsHist.Parameters.Add(new SqlParameter("@BeneficiaryAcctNo", l_BenacctNo));
                            commandNapsHist.Parameters.Add(new SqlParameter("@BeneficiaryName", l_BenacctName));
                            commandNapsHist.Parameters.Add(new SqlParameter("@Narration", l_Narration));
                            commandNapsHist.Parameters.Add(new SqlParameter("@RequestTitle", txtTitle.Text));
                            commandNapsHist.Parameters.Add(new SqlParameter("@CustAcctNo", txtref_no.Text));

                            commandNapsHist.Parameters.Add(new SqlParameter("@Amount", l_Amount));
                            commandNapsHist.Parameters.Add(new SqlParameter("@FeeAmt", feecharge));
                            commandNapsHist.Parameters.Add(new SqlParameter("@TransCode", 863));

                            commandNapsHist.Parameters.Add(new SqlParameter("@AgentID", l_AccountNo));
                            commandNapsHist.Parameters.Add(new SqlParameter("@TransRefNo", ""));
                            commandNapsHist.Parameters.Add(new SqlParameter("@SortCode", l_Sortcode));

                            commandNapsHist.Parameters.Add(new SqlParameter("@Postedby", gUser));

                            commandNapsHist.Parameters.Add(new SqlParameter("@ApprovedBy",offr2.SelectedValue));
                            commandNapsHist.Parameters.Add(new SqlParameter("@ConfirmBy", ""));
                            commandNapsHist.Parameters.Add(new SqlParameter("@TransRemarks", "Bulk entry"));
                            commandNapsHist.Parameters.Add(new SqlParameter("@ProfileID", RefNo));
                            commandNapsHist.Parameters.Add(new SqlParameter("@SystemIP", userHostAddress));
                            commandNapsHist.Parameters.Add(new SqlParameter("@SystemName", hostName));

                            commandNapsHist.ExecuteNonQuery();
                            connNapsHist.Close();




                            #endregion

                            #endregion
      




                        }


                    }
                    catch (Exception)
                    {

                        throw;
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

            }
        }
        else
        {
            this.Response.Write("<script>alert(' Your user section has expired, please logout and try the upload again ') </script>");
        }

    }



}