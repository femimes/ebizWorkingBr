using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using Microsoft.ReportingServices;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Reporting.WebForms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;


public partial class Reports_RsStatement : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack && !IsPostBack)
        {
            ASPxDateEdit1.Date = DateTime.Parse("2010-01-01");// DateTime.Today.Date;
            ASPxDateEdit2.Date = DateTime.Today.AddDays(1.0);
            txtAccountNo.Text = "10010650010";
            UpdateDataViewFilterExpression();
            ReportViewer1.LocalReport.DisplayName = "Production Report";
        }
    }


    void UpdateDataViewFilterExpression()
    {
        UpdateDataSourceFilterExpression();
        ReportViewer1.DataBind();
    }
    void UpdateDataSourceFilterExpression()
    {
        try
        {
            DateTime StartDate; DateTime EndDate;

            StartDate = DateTime.Parse(ASPxDateEdit1.Value.ToString());
            EndDate = DateTime.Parse(ASPxDateEdit2.Value.ToString());



            if (ASPxFilterControl1.IsFilterExpressionValid())
            {
                // 'ASPxLabel1.Text = ASPxFilterControl1.GetFilterExpressionForAccess()
                ASPxLabel1.Text = ASPxFilterControl1.FilterExpression;
                //SqlDataSource1.FilterExpression = ASPxLabel1.Text;

                DataSet ds = new DataSet();
                string lstrFilterByDataSet = "";
                if (ASPxFilterControl1.GetFilterExpressionForDataSet() != "")
                {
                    lstrFilterByDataSet = ASPxFilterControl1.GetFilterExpressionForDataSet();
                }
                else
                {
                    lstrFilterByDataSet = ASPxFilterControl1.GetFilterExpressionForDataSet();
                }
                ReportViewer1.LocalReport.DataSources.Clear();
                string Strcon = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ToString();
                System.Data.SqlClient.SqlConnection conn = new SqlConnection(Strcon);

                #region Get Reports profile info

                string strComName = ""; string strComContactTitles = ""; string strComAddress = "";
                string strComCity = ""; string strComComState = ""; string strComPhone = "";
                string strComEmail = ""; string  strComLogojpg = "";
                string strreportcom = "SELECT * FROM AppReportsInfo";

                DataSet comks = new DataSet();
                System.Data.SqlClient.SqlDataAdapter cmddscom = new SqlDataAdapter(strreportcom, conn);
                cmddscom.SelectCommand.CommandType = CommandType.Text;

                cmddscom.Fill(comks);

                foreach (DataRow jkr in comks.Tables[0].Rows)
                {
                    strComName = string.Format("{0}", jkr["ComName"]);
                    strComContactTitles = string.Format("{0}", jkr["ComContactTitles"]);
                    strComAddress = string.Format("{0}", jkr["ComAddress"]);
                    strComCity = string.Format("{0}", jkr["ComCity"]);
                    strComComState = string.Format("{0}", jkr["ComState"]);
                    strComEmail = string.Format("{0}", jkr["ComEmail"]);
                    strComPhone = string.Format("{0}", jkr["ComPhone"]);
                    strComLogojpg = string.Format("{0}", jkr["ComLogojpg"]);



                }

                #endregion
                
                
                #region Make Additional Records for the table

                string Strcons = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnectSource"].ToString();
                System.Data.SqlClient.SqlConnection conns = new SqlConnection(Strcons);
                //RepGetAllPaymentsDetInfoDate
                // string strreportstr2 = string.Format("RepGetAllPayments '{0}','{1}'", Request.QueryString["r"].ToString(), Request.QueryString["p"]).ToString();
                string strreportstr = string.Format("SELECT BranchName as sub_name, AccountNo as account_no, SeqNo as seq_no, AddedBy as posted_by, PostingDateAdded as post_date, ValueDate as trans_date,ParentAccountNo as mini_account_no, currCode as curr_code, isnull((select top 1 BalAcctCurr FROM TRanshist WHERE AccountNo ='{2}' AND  PostingDateAdded < '{3}' order by PostingDateAdded desc ),0) as 'base_bf', " +
                                              "  AccountDesc as description, AccountDesc as accountname, UserNarrative as narrative, debit, credit, BalAcctCurr as balance, TransID as ref_no, BranchAdded as headsub_code, AcctExchRate as exch_rate, transCode as trans_code, UserNarrative as narration, 0 as reversed,  cast('{0}' as datetime)  as startdate, cast('{1}' as datetime)  as enddate, dbo.FnCustomerAddress(AccountNo, 0) as address, " +
                                              " dbo.FnCustomerCity(AccountNo, 0) as city, dbo.FnAccountProductName(AccountNo) as prod_desc, ProdCode as prod_code, TransDesc as trans_desc, '{5}' as ComName, '{6}' as ComContactTitles  FROM VwTransHist " +
                                              " WHere accountno ='{2}' AND PostingDateAdded >='{3}' AND PostingDateAdded <= '{4}' order by PostingDateAdded, seq_no", ((StartDate.ToString("yyyy-MM-dd"))), (EndDate.ToString("yyyy-MM-dd")), txtAccountNo.Text, StartDate.ToString("yyyy-MM-dd"), EndDate.ToString("yyyy-MM-dd"), strComName,strComContactTitles);



                DataSet ks = new DataSet();
                System.Data.SqlClient.SqlDataAdapter cmdds = new SqlDataAdapter(strreportstr, conns);
                cmdds.SelectCommand.CommandType = CommandType.Text;
                //cmdds.SelectCommand.Parameters.Add(new SqlParameter("@FilterStr", ASPxLabel1.Text));
                //cmdds.SelectCommand.Parameters.Add(new SqlParameter("@StartDate", ASPxDateEdit1.Date));
                //cmdds.SelectCommand.Parameters.Add(new SqlParameter("@EndDate", ASPxDateEdit2.Date));
                // cmdds.SelectCommand.Parameters.Add(new SqlParameter("@PaymentRefNo", Request.QueryString["p"].ToString()));
                cmdds.Fill(ks);

                // Create a DataColumn and set various properties. 


                //#region Section for Preminum retrival
                //bool l_UseFormula = false; string l_TransFormula = "0"; string l_InsuredFormula = "0";
                //if (ks.Tables[0].Rows.Count > 0)
                //{
                //    l_UseFormula = bool.Parse(ks.Tables[0].Rows[0]["UseFormula"].ToString());
                //    l_TransFormula = ks.Tables[0].Rows[0]["TransFormula"].ToString();
                //    l_InsuredFormula = ks.Tables[0].Rows[0]["InsuredFormula"].ToString();
                //}
                //decimal PremiumAmt = 0; decimal l_InsurdAmt = 0;
                //// string lkey = l_UseFormula;


                //#endregion
                //DataTable dt = new DataTable();
                //ks.Tables[0].Columns.Add(new DataColumn("VehPremAmt", typeof(decimal)));
                //ks.Tables[0].Columns.Add(new DataColumn("VehInsuredAmt", typeof(decimal)));



                //for (int i = 0; i < ks.Tables[0].Rows.Count; i++)
                //{

                //    string l_insAmt = string.Format("SELECT {0} AS 'VehInsuredAmt'", ks.Tables[0].Rows[i]["VehInsuredAmt2"].ToString());
                //    string l_PreAmt = string.Format("SELECT {0} AS 'VehPremAmt'", ks.Tables[0].Rows[i]["VehPremAmt2"].ToString());


                //    l_InsurdAmt = decimal.Parse(NETClass.Classes.GetDataset(l_insAmt).Tables[0].Rows[0]["VehInsuredAmt"].ToString());
                //    PremiumAmt = decimal.Parse(NETClass.Classes.GetDataset(l_PreAmt).Tables[0].Rows[0]["VehPremAmt"].ToString());



                //    ks.Tables[0].Rows[i]["VehPremAmt"] = PremiumAmt;
                //    ks.Tables[0].Rows[i]["VehInsuredAmt"] = l_InsurdAmt;
                //}

                #endregion

                //ASPxLabel1.Text = lstrFilterByDataSet;
                // Microsoft.Reporting.WebForms.ReportDataSource rprtDTSource = new Microsoft.Reporting.WebForms.ReportDataSource("DsProductAmount", ks.Tables[0].Select(lstrFilterByDataSet));
                Microsoft.Reporting.WebForms.ReportDataSource rprtDTSource = new Microsoft.Reporting.WebForms.ReportDataSource();//"DsProductAmount", ks.Tables[0].Select(lstrFilterByDataSet));
                //Response.Write(lstrFilterByDataSet);
                rprtDTSource.Name = "DataSet2_VwTransHist";
                if (lstrFilterByDataSet == "")
                {
                    rprtDTSource.Value = ks.Tables[0];
                }
                else
                {


                    DataView custView = new DataView(ks.Tables[0], ASPxFilterControl1.GetFilterExpressionForDataSet(),
      "RegistryDate", DataViewRowState.CurrentRows);


                    rprtDTSource.Value = custView;//.Select(ASPxFilterControl1.GetFilterExpressionForDataSet()); ;

                }

                ReportViewer1.LocalReport.DataSources.Add(rprtDTSource);

                //ReportViewer1.LocalReport.Refresh();
                ReportViewer1.DataBind();
                ReportViewer1.Visible = true;
                ASPxLabel1.Visible = false;


            }
            else
            {
                ASPxLabel1.Visible = true;
                ASPxLabel1.Text = "Filter expression is not valid";
            }
            if (String.IsNullOrEmpty(ASPxLabel1.Text))
            {
                ASPxLabel1.Visible = true;
                ASPxLabel1.Text = "Filter expression is empty";
            }

        }
        catch (Exception ex)
        {

            Response.Write(ex.Message);
        }


    }

    protected void ASPxFilterControl1_CustomJSProperties(object sender, DevExpress.Web.CustomJSPropertiesEventArgs e)
    {
        e.Properties["cpFilterExpression"] = ASPxFilterControl1.FilterExpression;
    }
    protected void ReportViewer1_ReportRefresh(object sender, System.ComponentModel.CancelEventArgs e)
    {
        UpdateDataViewFilterExpression();
        ReportViewer1.LocalReport.DisplayName = ASPxLabel1.Text;





    }
    protected void ASPxButton1_Click(object sender, EventArgs e)
    {
        UpdateDataViewFilterExpression();
    }

    protected void ASPxDateEdit1_DateChanged(object sender, EventArgs e)
    {
        UpdateDataViewFilterExpression();
    }
    protected void ASPxDateEdit2_DateChanged(object sender, EventArgs e)
    {
        UpdateDataViewFilterExpression();
    }
    protected void ASPxButton2_Click(object sender, EventArgs e)
    {
        UpdateDataViewFilterExpression();
        //ReportViewer1.LocalReport.DisplayName = ASPxLabel1.Text;
    }

}