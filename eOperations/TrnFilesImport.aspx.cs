
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

public partial class eOperations_TrnFilesImport : System.Web.UI.Page
{

    public string gUser = HttpContext.Current.User.Identity.Name;//HttpContext.Current.User.Identity.Name;
    public string luser = "";
    public string StrLocation = "";
    public string struser = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        if (this.IsCallback || this.IsPostBack)
            return;
        //this.luser =HttpContext.Current.User.Identity.Name;//HttpContext.Current.User.Identity.Name; //HttpContext.Current.User.Identity.Name;// HttpContext.Current.User.Identity.Name;
        //StrLocation = this.luser.Substring(this.luser.LastIndexOf("?") + 1);
        //this.struser = this.luser.Substring(0, this.luser.LastIndexOf("?"));
        //this.gUser = this.struser;
        //this.txtuserID.Text = this.gUser;
        //this.ctrlGrid.PageIndex = 0;
        //this.DataBind();
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        luser =HttpContext.Current.User.Identity.Name;// HttpContext.Current.User.Identity.Name; //HttpContext.Current.User.Identity.Name;// HttpContext.Current.User.Identity.Name;
        StrLocation = this.luser.Substring(this.luser.LastIndexOf("?") + 1);
         struser = this.luser.Substring(0, this.luser.LastIndexOf("?"));
         gUser = this.struser;

       // okpanachi.jerry

   if (string.Compare((gUser), "", false) != 0)
        {
            if (Convert.ToDouble(this.drptranstype.SelectedValue) == 2.0)
            {
                if (string.Compare(this.txtref_no.Text, (string)null, false) == 0)
                {
                    //txtref_no.Text = RetailDepTransMethods.GenerateRetailTransNumber();


                    #region Generate Account Info

                    SqlConnection connection2 = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnectSource"].ConnectionString);


                    SqlCommand commandIns = new SqlCommand();
                    connection2.Open();
                    commandIns.CommandText = "SpGenerateNumberSequence";
                    commandIns.Connection = connection2;
                    commandIns.CommandType = CommandType.StoredProcedure;
                    commandIns.Parameters.Add(new SqlParameter("@NumberID", 2082));
                    SqlParameter parameter = commandIns.Parameters.Add(new SqlParameter("@GeneratedNumber", SqlDbType.VarChar, 100));
                    parameter.Direction = ParameterDirection.Output;

                    commandIns.ExecuteNonQuery();
                    connection2.Close();

                    txtref_no.Text = parameter.Value.ToString();




                    #endregion




                    string str1 = this.Request.ServerVariables["REMOTE_ADDR"];
                    string hostName = Dns.GetHostName();
                    string userHostAddress = this.Request.UserHostAddress;
                    string str21 = Dns.GetHostAddresses(hostName).GetValue(1).ToString();
                    Classes.alertwindow(this.Page, "A New Batch Reference Generated : - " + this.txtref_no.Text, "Halt");
                    Classes.RecordOp(string.Format("Sproc_AddJournal '{0}','Data Upload Transaction',{1},'N','{2}','{3}',0,'{4}','{5}','{6}'",  txtref_no.Text,  Decimal.Zero,  gUser , StrLocation,  offr2.SelectedValue,  hostName,  str21));
                    try
                    {
                        foreach (DataGridItem dataGridItem in this.DataGrid1.Items)
                        {
                            string str2 = (dataGridItem.Cells[0].Text); // Posting Date
                            string str3 = (dataGridItem.Cells[1].Text); // Terminal ID
                            string str4 = (dataGridItem.Cells[2].Text); // MerchantID
                            string str5 = (dataGridItem.Cells[3].Text); // STAN
                            string str14 = (dataGridItem.Cells[4].Text); // Ref No
                            string str15 = (dataGridItem.Cells[5].Text); // Card No
                            string str6 = (dataGridItem.Cells[6].Text.Replace(",", "").Replace(@"""", "")); // Amount
                            string str7 = (dataGridItem.Cells[7].Text.Replace(",", "").Replace(@"""", "")); // Charge
                            string str8 = (dataGridItem.Cells[8].Text.Replace(",", "").Replace(@"""", "")); // Amount Due
                            string str19 = (dataGridItem.Cells[9].Text.Replace(",", "").Replace(@"""", "")); // Lump  Sum 
                            string str9 = (dataGridItem.Cells[10].Text);// Settlement Date
                            string str145 = dataGridItem.Cells[10].CssClass; //Convert.ToString(1);
                         
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
                            ContraAcctNo = string.Format("{0}{1}", StrLocation, strContralAcctNo);// NETClass.EazyClass.AXFullAccount(StrLocation, strContralAcctNo);
                            string narrative = "";
                            narrative = string.Format("Pos Trans {0}{1}", str14, str15);
                            Classes.RecordOp(String.Format("Sproc_AddExtTransHist '{0}','{1}','{2}','{3}','{4}','{5}',{6} ,{7},'{8}','{9}','{10}','{11}','{12}',{13},'{14}','{15}'", DateTime.Parse(str2), DateTime.Parse(str9), str4, "003", narrative, narrative, 0, str8.Replace(",", "").Replace(@"""", ""), str5, strContralAcctNo, str14, this.txtref_no.Text, gUser, offr2.SelectedValue, "1", StrLocation, userHostAddress));
                        }
                    }
                    finally
                    {
                        //IEnumerator enumerator;
                        //if (enumerator is IDisposable)
                        //    (enumerator as IDisposable).Dispose();
                    }
                }
            }

            else if (Convert.ToDouble(this.drptranstype.SelectedValue) == 12.0)
            {
                //txtref_no.Text = RetailDepTransMethods.GenerateRetailTransNumber();


                #region Generate Account Info

                SqlConnection connection2 = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnectSource"].ConnectionString);


                SqlCommand commandIns = new SqlCommand();
                connection2.Open();
                commandIns.CommandText = "SpGenerateNumberSequence";
                commandIns.Connection = connection2;
                commandIns.CommandType = CommandType.StoredProcedure;
                commandIns.Parameters.Add(new SqlParameter("@NumberID", 2082));
                SqlParameter parameter = commandIns.Parameters.Add(new SqlParameter("@GeneratedNumber", SqlDbType.VarChar, 100));
                parameter.Direction = ParameterDirection.Output;

                commandIns.ExecuteNonQuery();
                connection2.Close();

                txtref_no.Text = parameter.Value.ToString();




                #endregion




                string str1 = this.Request.ServerVariables["REMOTE_ADDR"];
                string hostName = Dns.GetHostName();
                string userHostAddress = this.Request.UserHostAddress;
                string str255 = Dns.GetHostAddresses(hostName).GetValue(1).ToString();
                Classes.alertwindow(this.Page, "A New Batch Reference Generated : - " + this.txtref_no.Text, "Halt");

                string strHeader = string.Format("Sproc_AddJournal '{0}','POS Trans Upload',{1},'N','{2}','{3}',0,'{4}','{5}','{6}'", txtref_no.Text, "0", gUser, StrLocation, this.offr2.SelectedValue, hostName, str255);
                Classes.AXRecordOp(strHeader);
                try
                {
                    foreach (DataGridItem dataGridItem in this.DataGrid1.Items)
                    {
                        

                        string str2 = (dataGridItem.Cells[0].Text); // Posting Date
                        string str3 = (dataGridItem.Cells[1].Text); // Terminal ID
                        string str4 = (dataGridItem.Cells[2].Text); // MerchantID
                        string str5 = (dataGridItem.Cells[3].Text); // STAN
                        string str14 = (dataGridItem.Cells[4].Text); // Ref No
                        string str15 = (dataGridItem.Cells[5].Text); // Card No
                        string str6 = (dataGridItem.Cells[6].Text.Replace(",", "").Replace(@"""", "")); // Amount
                        string str7 = (dataGridItem.Cells[7].Text.Replace(",", "").Replace(@"""", "")); // Charge
                        string str8 = (dataGridItem.Cells[8].Text.Replace(",", "").Replace(@"""", "")); // Amount Due
                        string str19 = (dataGridItem.Cells[9].Text.Replace(",", "").Replace(@"""", "")); // Lump  Sum 
                        string str9 = (dataGridItem.Cells[10].Text);// Settlement Date
                        string str145 = dataGridItem.Cells[11].CssClass; //Convert.ToString(1);


                        //if (string.Compare(str11, "", false) == 0)
                        //    str11 = str12;
                        //if (!EazyClass.AccountExist(str5))
                        //    str5 = NETClass.EazyClass.FullAccount(this.StrLocation, str5);
                        //if (!EazyClass.AccountExist(str11))
                        //    str11 = NETClass.EazyClass.FullAccount(this.StrLocation, str11);
                        //Sproc_AddExtTransHist
                        if (str145 == "1")
                        {//
                           
                            //@narrative Varchar(100),@description Varchar(100),
                            //@debit decimal(18,2),@Credit decimal(18,2),@bank_code Varchar(50),
                            //@cheque_no Varchar(50),@extRefNo Varchar(50),
                            //@ExtBatchNo  Varchar(50),@entered_by Varchar(50),
                            //@ApprovingOffr Varchar(50),@ValidateCheck numeric,
                            //@WorkstationAdded Varchar(50),
                            //@WorkstationIPAdded Varchar(50)
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
                            ContraAcctNo = string.Format("{0}{1}", StrLocation, strContralAcctNo);// NETClass.EazyClass.AXFullAccount(StrLocation, strContralAcctNo);
                            string narrative = "";
                            narrative = string.Format("Pos Trans {0}{1}", str14, str15);
                            string strUpdRec = string.Format("Sproc_AddExtTransHist '{0}','{1}','{2}','{3}','{4}','{5}',{6} ,{7},'{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}'", DateTime.Parse(str2), DateTime.Parse(str9), strgetAcctNo, "003", narrative, narrative, 0, str8.Replace(",", "").Replace(@"""", ""), str5, ContraAcctNo, str14, txtref_no.Text, gUser, offr2.SelectedValue, "1", StrLocation, userHostAddress);
                            Classes.AXRecordOp(strUpdRec);
                     //       Classes.RecordOp(String.Format("Sproc_AddExtTransHistJornal '{0}','{1}','{2}','{3}','{4}','{5}',{6} ,{7},'{8}','{9}','{10}','{11}','{12}',{13},'{14}','{15}'", DateTime.Parse(str3), DateTime.Parse(str4), str5, str6, str7, str8, str9, str10, str11, str12, str13, this.txtref_no.Text, this.struser, Convert, this.offr2.SelectedValue, this.StrLocation));
             
                        }


                        // Section load trans 4 approval
                        

                     }
                    string strLdAppr = string.Format("Sproc_ExtBatchUploadTrans '{0}'", txtref_no.Text);
                        Classes.AXRecordOp(strLdAppr);

                }
                finally
                {
                    //IEnumerator enumerator;
                    //if (enumerator is IDisposable)
                    //    (enumerator as IDisposable).Dispose();
                }
            }

            else
            {
                this.txtref_no.Text = EazyClass.GetExtJournalNum(this.StrLocation);
                Classes.alertwindow(this.Page, "A New Batch Reference Generated : - " + this.txtref_no.Text, "Halt");
                Classes.RecordOp(string.Format("Sproc_AddJournal '{0}','Monthly Salaries',{1},'N','{2}','{3}',0",  this.txtref_no.Text,  Decimal.Zero,  this.struser,  this.StrLocation));
                try
                {
                    foreach (DataGridItem dataGridItem in this.DataGrid1.Items)
                    {
                        string str1 = (dataGridItem.Cells[0].Text);
                        string str2 = (dataGridItem.Cells[1].Text);
                        string str3 = (dataGridItem.Cells[2].Text);
                        string str4 = (dataGridItem.Cells[3].Text);
                        string str5 = (dataGridItem.Cells[4].Text);
                        string str6 = (dataGridItem.Cells[5].Text);
                        string str7 = (dataGridItem.Cells[6].Text);
                        string str8 = (dataGridItem.Cells[7].Text);
                        string Left = (dataGridItem.Cells[8].Text);
                        string str9 = (dataGridItem.Cells[10].Text);
                        string str10 = (dataGridItem.Cells[10].Text);
                        string str11 = Convert.ToString(1);
                        if (string.Compare(Left, "", false) == 0)
                            Left = str9;
                        string str12 = String.Format("Sproc_AddExtTransHistJornal '{0}','{1}','{2}','{3}','{4}','{5}',{6} ,{7},'{8}','{9}','{10}','{11}','{12}',{13},'{14}','{15}'", Convert.ToDateTime(str1), Convert.ToDateTime(str2), str3, str4, str5, str6, str7, str8, Left, str9, str10, this.txtref_no.Text, this.struser, Convert.ToString(Convert.ToInt64(str11)), this.offr2.SelectedValue, this.StrLocation);
                        Classes.AXRecordOp(str12);
                        this.Response.Write(str12);
                    }
                }
                finally
                {
                    //IEnumerator enumerator;
                    //if (enumerator is IDisposable)
                    //    (enumerator as IDisposable).Dispose();
                }
            }
            this.Response.Write("<script>alert('Upload Completed') </script>");
            this.txtref_no.Text = (string)null;
            this.DataGrid1.DataSource =  null;
            this.DataGrid1.DataBind();
        }
        else
            this.Response.Write("<script>alert(' Your user section has expired, please logout and try the upload again ') </script>");
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            string str = this.UploadMultipleFiles_Clicked();
           
            if (string.Compare(str, (string)null, false) == 0)
                return;
            if (Convert.ToDouble(this.drptranstype.SelectedValue) == 2.0)
                return; //this.InsertAllRecords(str);
            else if (Convert.ToDouble(this.drptranstype.SelectedValue) == 3.0)
                return;// this.InsertAllRecordsPromo(str);
            else if (Convert.ToDouble(this.drptranstype.SelectedValue) == 4.0)
                return;//  this.InsertAllRecordsPromoAccts(str);
            else if (Convert.ToDouble(this.drptranstype.SelectedValue) == 7.0)
                return;//    this.InsertAllRecords(str);
            else if (Convert.ToDouble(this.drptranstype.SelectedValue) == 11.0)
                return;//   this.InsertAllRecordsTrans(str);
            else if (Convert.ToDouble(this.drptranstype.SelectedValue) == 12.0)
                  this.InsertAllRecordsTrans(str);
                
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



    private void LoadApprovingOfficers()
    {
        luser =HttpContext.Current.User.Identity.Name;// HttpContext.Current.User.Identity.Name; //HttpContext.Current.User.Identity.Name;// HttpContext.Current.User.Identity.Name;
        StrLocation =  luser.Substring(this.luser.LastIndexOf("?") + 1);
        struser =  luser.Substring(0, this.luser.LastIndexOf("?"));
        gUser = this.struser;
        string strTransTypeCode = "001";
        string strDefaultTRansID = "BATCH-000014261";
        DataSet ds = new DataSet();

        string getAppr = string.Format("SpUserMasterTransApprMems '{0}','{1}','{2}',{3},'{4}'", strTransTypeCode, StrLocation, gUser, 0, strDefaultTRansID);
        ds = Classes.AXGetDataset(string.Format("SpUserMasterTransApprMems '{0}','{1}','{2}',{3},'{4}'", strTransTypeCode, StrLocation, gUser, 0, strDefaultTRansID));
        offr2.DataSource = ds;
        offr2.DataTextField = "UserName";
        offr2.DataValueField = "UserID";
        offr2.DataBind();


    }

    private void LoadApprovingOfficers2(Decimal amount)
    {
        // ISSUE: unable to decompile the method.
    }


    public void InsertAllRecordsTrans(string m_strFileName)
    {
        this.luser =HttpContext.Current.User.Identity.Name;//HttpContext.Current.User.Identity.Name; //HttpContext.Current.User.Identity.Name;// HttpContext.Current.User.Identity.Name;
        StrLocation = this.luser.Substring(this.luser.LastIndexOf("?") + 1);
        this.struser = this.luser.Substring(0, this.luser.LastIndexOf("?"));
        this.gUser = this.struser;

        StreamReader streamReader = new StreamReader("C:\\TempMultipleFiles\\" + m_strFileName);
        DataSet dataSet = new DataSet();
        DataTable dataTable = new DataTable();
        //dataTable.Columns.Add(new DataColumn(" Post Date   ", typeof(string)));
        //dataTable.Columns.Add(new DataColumn("TransDate  ", typeof(string)));
        //dataTable.Columns.Add(new DataColumn("Account No", typeof(string)));
        //dataTable.Columns.Add(new DataColumn("Transaction Code", typeof(string)));
        //dataTable.Columns.Add(new DataColumn("Narrative", typeof(string)));
        //dataTable.Columns.Add(new DataColumn("Cheque No ", typeof(string)));
        //dataTable.Columns.Add(new DataColumn("Debit", typeof(string)));
        //dataTable.Columns.Add(new DataColumn("Credit", typeof(string)));
        //dataTable.Columns.Add(new DataColumn("Contra", typeof(string)));
        //dataTable.Columns.Add(new DataColumn("Instr No", typeof(string)));
        //dataTable.Columns.Add(new DataColumn("Reference Code", typeof(string)));
        //dataTable.Columns.Add(new DataColumn("Remarks", typeof(string)));
     
        dataTable.Columns.Add(new DataColumn(" Post Date   ", typeof(string)));
        dataTable.Columns.Add(new DataColumn("TerminalID", typeof(string)));
        dataTable.Columns.Add(new DataColumn("MerchantID", typeof(string)));
        dataTable.Columns.Add(new DataColumn("STAN Code", typeof(string)));
        dataTable.Columns.Add(new DataColumn("Reference Code", typeof(string)));
        dataTable.Columns.Add(new DataColumn("CardNo ", typeof(string)));
        dataTable.Columns.Add(new DataColumn("Amount", typeof(string)));
        dataTable.Columns.Add(new DataColumn("Charge", typeof(string)));
        dataTable.Columns.Add(new DataColumn("Amount Due", typeof(string)));
        dataTable.Columns.Add(new DataColumn("Lump Total", typeof(string)));
        dataTable.Columns.Add(new DataColumn("Settlement Date", typeof(string)));
        dataTable.Columns.Add(new DataColumn("Remarks", typeof(string)));
        string str1;
        while ((str1 = streamReader.ReadLine()) != null)
        {
            //str1 = streamReader.ReadLine();
            //string.Len(str1);


                //char[] delimiters = new char[] { ',' ,'\t'};

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

                        row[i] = string.Format(parts[i]);

                    }
                     

                }
                //Response.Write("<br/>");
                dataTable.Rows.Add(row);

               
         




        //    if (string.Compare(Strings.Trim(str22), (string)null, false) != 0)
        //    {
        //        DataRow row = dataTable.NewRow();
        //        row[0] = Strings.Replace(Strings.Replace(str22, ",", "", 1, -1, CompareMethod.Binary), ",", "", 1, -1, CompareMethod.Binary);
        //        row[1] = Strings.Format(Convert.ToDate(str23.Replace(",", "").Replace(",", "")), "dd-MMM-yyyy");
        //        string str32;
        //        if (NETClass.EazyClass.AccountExist(Strings.Trim(str24.Replace(",", ""))))
        //        {
        //            str32 = Strings.Trim(str24.Replace(",", ""));
        //        }
        //        else
        //        {
        //            string sString = "t" + Strings.Trim(str24.Replace(",", ""));
        //            str32 = !NETClass.EazyClass.AccountExist(NETClass.EazyClass.FullAccount(this.StrLocation, sString)) ? Strings.Trim(str24.Replace(",", "")) : NETClass.EazyClass.FullAccount(this.StrLocation, sString);
        //        }
        //        row[2] = str32;
        //        row[3] = Strings.Trim(str25.Replace(",", ""));
        //        row[4] = Strings.Replace(Strings.Replace(Expression, ",", "", 1, -1, CompareMethod.Binary), ",", "", 1, -1, CompareMethod.Binary);
        //        row[5] = str26.Replace(",", "").Replace(",", "");
        //        row[6] = Strings.Trim(str27.Replace(",", ""));
        //        row[7] = Strings.Trim(str28.Replace(",", ""));
        //        row[8] = Strings.Trim(str29.Replace(",", ""));
        //        row[9] = Strings.Trim(str30.Replace(",", ""));
        //        row[10] = Strings.Trim(str31.Replace(",", ""));
        //        row[11] = "Pass";
        //        dataTable.Rows.Add(row);
        //    }
        }
        //while (str1 != null);
        streamReader.Close();
       
        this.DataGrid1.DataSource = dataTable;
        this.DataGrid1.DataBind();
        LoadApprovingOfficers();
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
        string str1 ="";
        try
        {
            foreach (DataGridItem dataGridItem in this.DataGrid1.Items)
            {
                try
                {
                string str2 = (dataGridItem.Cells[0].Text); // Posting Date
                string str3 = (dataGridItem.Cells[1].Text); // Terminal ID
                string str4 = (dataGridItem.Cells[2].Text); // MerchantID
                string str5 = (dataGridItem.Cells[3].Text); // STAN
                string str14 = (dataGridItem.Cells[4].Text); // Ref No
                string str15 = (dataGridItem.Cells[5].Text); // Card No
                string str6 = (dataGridItem.Cells[6].Text.Replace(",", "").Replace(@"""", "")); // Amount
                string str7 = (dataGridItem.Cells[7].Text.Replace(",", "").Replace(@"""", "")); // Charge
                string str8 = (dataGridItem.Cells[8].Text.Replace(",", "").Replace(@"""", "")); // Amount Due
                string str19 = (dataGridItem.Cells[9].Text.Replace(",", "").Replace(@"""", "")); // Lump  Sum 
                string str9 = (dataGridItem.Cells[10].Text);// Settlement Date



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
                         ContraAcctNo = string.Format("{0}{1}", StrLocation, strContralAcctNo);// NETClass.EazyClass.AXFullAccount(StrLocation, strContralAcctNo);
                        //ContraAcctNo =  NETClass.EazyClass.AXFullAccount(StrLocation, strContralAcctNo);
                dataGridItem.Cells[11].CssClass = Convert.ToString(1);
                
                //if string.IsNullOrEmpty((DateTime.TryParse(str2,null) == false))
                
                bool strdateconvert = true;
                DateTime thPostDate;
                Int64 thNumeric;
                decimal thdecimal;
                //DateTime MyDateTime;
                //var dateKind = DateTime.Parse(dataGridItem.Cells[0].Text).Kind;
                //MyDateTime = new DateTime();
                //MyDateTime = DateTime.ParseExact(dataGridItem.Cells[0].Text, "dddd dd MMMM, yyyy HH:mm tt",
                //                 null);
                //DateTime ldatetoconvert = DateTime.ParseExact(dataGridItem.Cells[0].Text, "dddd-dd-MMMM, YYYY HH:mm tt",
                //                 null); //Convert.ToDateTime(dataGridItem.Cells[0].Text.Replace("char(34)", ""));
                //string lcondate = string.Format("{0}",ldatetoconvert);
               
                    if ((DateTime.TryParse(dataGridItem.Cells[0].Text, out thPostDate)) == false)
                    {
                        str1 = str1 + dataGridItem.Cells[0].Text + "<br>,";
                        dataGridItem.Cells[0].BackColor = Color.Red;
                        dataGridItem.Cells[0].ToolTip = "invalid Posting Date";
                        dataGridItem.Cells[11].Text = "(Failed)- invalid Posting Date";
                        dataGridItem.Cells[11].CssClass = Convert.ToString(0);

                        //goto dataGridItem;
                    }
                    if (DateTime.Compare(DateTime.Parse(str2), DateTime.Parse(Classes.AXGetDataset("SELECT PostingDate FROM PrmPostingDates WHERE Record = 1").Tables[0].Rows[0]["PostingDate"].ToString())) > 0)
                    {
                        str1 = str1 + dataGridItem.Cells[2].Text + "<br>,";
                        dataGridItem.Cells[0].BackColor = Color.Red;
                        dataGridItem.Cells[0].ToolTip = "Entry Postdate is greater than EazybankAx posting Date";
                        dataGridItem.Cells[11].Text = "( Passed )on Later date";
                    }
                    if ((DateTime.TryParse(dataGridItem.Cells[10].Text, out thPostDate)) == false)
                    {
                        str1 = str1 + dataGridItem.Cells[10].Text + "<br>,";
                        dataGridItem.Cells[10].BackColor = Color.Red;
                        dataGridItem.Cells[10].ToolTip = "invalid Transaction Date";
                        dataGridItem.Cells[11].Text = "(Failed)- invalid Transaction Date";
                        dataGridItem.Cells[11].CssClass = Convert.ToString(0);
                    }
                    if (Classes.AxGetBitField("Sproc_GetExtRefNumExist '" + (str14) + "'"))
                    {
                        str1 = str1 + dataGridItem.Cells[4].Text + "<br>,";
                        dataGridItem.Cells[4].BackColor = Color.Red;
                        dataGridItem.Cells[4].ToolTip = "Transaction Sequence Code alreday Exist";
                        dataGridItem.Cells[11].Text = "(Failed)- Transaction Sequence Code alreday Exist";
                        dataGridItem.Cells[11].CssClass = Convert.ToString(0);
                    }
                    string rString1 = string.Format("SELECT AccountNo FROM VwAccMaster WHERE AccountNo = '{0}'", strgetAcctNo);
                    string rString2 = string.Format("SELECT AccountNo FROM VwAccMaster WHERE AccountNo = '{0}'", ContraAcctNo);
                    string rString3 = string.Format("SELECT AccountNo FROM VwAccMaster WHERE AccountNo = '{0}' AND AccountStatus = '1' ", strgetAcctNo);
                    string rString4 = string.Format("SELECT AccountNo FROM VwAccMaster WHERE AccountNo = '{0}' AND ProdCatCode IN ('1','2','Z')", strgetAcctNo);
                    string rString5 = string.Format("SELECT AccountNo FROM VwAccMaster WHERE AccountNo = '{0}' AND ProdCatCode IN ('1','2','Z')", ContraAcctNo);
                    string rString6 = string.Format("SELECT AccountNo FROM VwAccMaster WHERE AccountNo = '{0}' AND PostingDateAdded <=  '{1}' ", strgetAcctNo, Convert.ToDateTime(str9));
                    if (!Classes.AxGetBitField(rString1))
                    {
                        str1 = string.Format("{0}{1}<br>,", str1, dataGridItem.Cells[1].Text);
                        dataGridItem.Cells[1].BackColor = Color.Red;
                        dataGridItem.Cells[1].ToolTip = "Invalid EazybankAx Account for TerminalID ";
                        dataGridItem.Cells[11].Text = "(Failed)- Invalid EazybankAx Account";
                        dataGridItem.Cells[11].CssClass = Convert.ToString(0);
                    }
                    if (!Classes.AxGetBitField(rString2))
                    {
                        str1 = string.Format("{0}{1}<br>,", str1, dataGridItem.Cells[2].Text);
                        dataGridItem.Cells[9].BackColor = Color.Red;
                        dataGridItem.Cells[9].ToolTip = "Invalid EazybankAx ContraAccount ";
                        dataGridItem.Cells[11].Text = "(Failed)- Invalid EazybankAx ContraAccount";
                        dataGridItem.Cells[11].CssClass = Convert.ToString(0);
                    }
                    if (!Classes.AxGetBitField(rString3))
                    {
                        str1 = string.Format("{0}{1}<br>,", str1, dataGridItem.Cells[2].Text);
                        dataGridItem.Cells[2].BackColor = Color.Red;
                        dataGridItem.Cells[2].ToolTip = "Account is either dormant, closed or unapproved ";
                        dataGridItem.Cells[11].Text = "(Failed)- Account is either dormant, closed or unapproved";
                        dataGridItem.Cells[11].CssClass = Convert.ToString(0);
                    }
                    if (!Classes.AxGetBitField(rString4))
                    {
                        str1 = string.Format("{0}{1}<br>,", str1, dataGridItem.Cells[2].Text);
                        dataGridItem.Cells[2].BackColor = Color.Red;
                        dataGridItem.Cells[2].ToolTip = "Only Retail and GL Accounts are allowed in this posting screen ";
                        dataGridItem.Cells[11].Text = "(Failed)- Only Retail and GL Accounts are allowed in this posting screen";
                        dataGridItem.Cells[11].CssClass = Convert.ToString(0);
                    }
                    if (!Classes.AxGetBitField(rString5))
                    {
                        str1 = string.Format("{0}{1}<br>,", str1, dataGridItem.Cells[8].Text);
                        dataGridItem.Cells[2].BackColor = Color.Red;
                        dataGridItem.Cells[2].ToolTip = "Only Retail and GL Accounts are allowed in this posting screen  ";
                        dataGridItem.Cells[11].Text = "(Failed)- Only Retail and GL Accounts are allowed in this posting screen/Contra";
                        dataGridItem.Cells[11].CssClass = Convert.ToString(0);
                    }
                    if (!Classes.AxGetBitField(rString6))
                    {
                        str1 = string.Format("{0}{1}<br>,", str1, dataGridItem.Cells[2].Text);
                        dataGridItem.Cells[2].BackColor = Color.Red;
                        dataGridItem.Cells[2].ToolTip = "Account Opening Date cannot be later than current Posting Date ";
                        dataGridItem.Cells[11].Text = "(Failed)- Account Opening Date cannot be later than current Posting Date";
                        dataGridItem.Cells[11].CssClass = Convert.ToString(0);
                    }
                    //if ( (Classes.AxGetField("Sproc_GetExtTransCodesExist '" + (str5) + "'")).Length <= 0)
                    //{
                    //    str1 = str1 + dataGridItem.Cells[3].Text + "<br>,";
                    //    dataGridItem.Cells[3].BackColor = Color.Red;
                    //    dataGridItem.Cells[3].ToolTip = "Explanation Code description is not yet mapped";
                    //    dataGridItem.Cells[11].Text = "(Failed)- Explanation Code description is not yet mapped";
                    //    dataGridItem.Cells[11].CssClass = Convert.ToString(0);
                    //}


                    if ((decimal.TryParse(str7.Replace(",", "").Replace(@"""", ""), out thdecimal)) == false)
                    {
                        str1 = str1 + dataGridItem.Cells[7].Text.Replace(",", "") + "<br>,";
                        dataGridItem.Cells[7].BackColor = Color.Red;
                        dataGridItem.Cells[7].ToolTip = "(Failed)- Invalid charge amount";
                        dataGridItem.Cells[11].Text = "(Failed)- Invalid charge amount";
                        dataGridItem.Cells[11].CssClass = Convert.ToString(0);
                    }
                    if ((decimal.TryParse(str6.Replace(",", "").Replace(@"""",""), out thdecimal)) == false)
                    {
                        str1 = str1 + dataGridItem.Cells[6].Text + "<br>,";
                        dataGridItem.Cells[6].BackColor = Color.Red;
                        dataGridItem.Cells[6].ToolTip = "(Failed)- Invalid  amount";
                        dataGridItem.Cells[11].Text = "(Failed)- Invalid  amount";
                        dataGridItem.Cells[11].CssClass = Convert.ToString(0);
                    }
                    if (string.Compare((str4), "", false) == 0)
                    {
                        str1 = str1 + dataGridItem.Cells[2].Text + "<br>,";
                        dataGridItem.Cells[2].BackColor = Color.Red;
                        dataGridItem.Cells[2].ToolTip = "Entry already posted before upload";
                        dataGridItem.Cells[11].Text = "(Failed)- Entry already posted before upload";
                        dataGridItem.Cells[11].CssClass = Convert.ToString(0);
                    }
                    Decimal d2 = Convert.ToDecimal(str7);
                    if (Decimal.Compare(d1, d2) < 0)
                        d1 = d2;
                }
                catch (Exception ex)
                {


                }
            }
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


}