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
using NETClass;

public partial class Reports_eRemoteRequestRpt :  System.Web.UI.Page
{
    public string gUser = HttpContext.Current.User.Identity.Name;//HttpContext.Current.User.Identity.Name;
    public string luser = HttpContext.Current.User.Identity.Name;
    public string StrLocation = "";
    public string struser = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack && !IsCallback)
        {
            try
            {
                luser =  HttpContext.Current.User.Identity.Name;
                StrLocation = luser.Substring(luser.LastIndexOf("?") + 1);
                struser = luser.Substring(0, luser.LastIndexOf("?"));
                gUser = struser;
            }
            catch (Exception ex) { }
            TxtUserId.Text = struser;
            ASPxDateEdit1.Date = DateTime.Parse(DateTime.Today.Date.ToString());
            ASPxDateEdit2.Date = DateTime.Parse(DateTime.Today.AddDays(1.0).Date.ToString());
           
            UpdateDataViewFilterExpression();
            ReportViewer1.LocalReport.DisplayName = "eChannels Request Portfolio";
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
            luser =  HttpContext.Current.User.Identity.Name;
            StrLocation = luser.Substring(luser.LastIndexOf("?") + 1);
            struser = luser.Substring(0, luser.LastIndexOf("?"));
            gUser = struser;
        }
        catch (Exception ex) { }
        DateTime StartDate; DateTime EndDate;

        StartDate = DateTime.Parse(ASPxDateEdit1.Value.ToString());
        EndDate = DateTime.Parse(ASPxDateEdit2.Value.ToString());
        string check = string.Format("Sproc_GetAllRemoteRepInfoDate '{0}','{1}','{2}'", StartDate.ToString("yyyy-MM-dd"), EndDate.ToString("yyyy-MM-dd"), TxtUserId.Text);
        //Response.Write(check);
        if (ASPxFilterControl1.IsFilterExpressionValid())
        {
            // 'ASPxLabel1.Text = ASPxFilterControl1.GetFilterExpressionForAccess()
            ASPxLabel1.Text = ASPxFilterControl1.FilterExpression;
            string lstrFilterByDataSet = "";
            if (ASPxFilterControl1.GetFilterExpressionForDataSet() != "")
            {
                lstrFilterByDataSet = ASPxFilterControl1.GetFilterExpressionForDataSet();
            }
            else
            {
                lstrFilterByDataSet = ASPxFilterControl1.GetFilterExpressionForDataSet();
            }
            if (lstrFilterByDataSet != "")
            {
                //ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Clear();
                
                DataSet ks = new DataSet();
                Microsoft.Reporting.WebForms.ReportDataSource rprtDTSource = new Microsoft.Reporting.WebForms.ReportDataSource();//"DsProductAmount", ks.Tables[0].Select(lstrFilterByDataSet));

                rprtDTSource.Name = "DsDetails";
                // NETClass.Classes.GetDataset(string.Format("Sproc_GetAllRepInfoDate '{0}','{1}'", ASPxDateEdit1.Value, ASPxDateEdit2.Value));
                if (lstrFilterByDataSet == "")
                {
                  rprtDTSource.Value = NETClass.Classes.GetDataset(string.Format("Sproc_GetAllRemoteRepInfoDate '{0}','{1}','{2}'", StartDate.ToString("yyyy-MM-dd"), EndDate.ToString("yyyy-MM-dd"), TxtUserId.Text)).Tables[0];
                }
                else
                {
                    ks = NETClass.Classes.GetDataset(string.Format("Sproc_GetAllRemoteRepInfoDate '{0}','{1}','{2}'", StartDate.ToString("yyyy-MM-dd"), EndDate.ToString("yyyy-MM-dd"), TxtUserId.Text));
                    //DataView custView = new DataView(NETClass.Classes.GetDataset(string.Format("Sproc_GetAllRepInfoDate '{0}','{1}'", ASPxDateEdit1.Value, ASPxDateEdit2.Value)).Tables[0], ASPxFilterControl1.GetFilterExpressionForDataSet(),
                    //    "panno", DataViewRowState.CurrentRows);

                    DataView custView = new DataView(ks.Tables[0], ASPxFilterControl1.GetFilterExpressionForDataSet(),
                      "AccountNo", DataViewRowState.CurrentRows);
                    rprtDTSource.Value = custView;//.Select(ASPxFilterControl1.GetFilterExpressionForDataSet()); ;

                }
                ReportViewer1.LocalReport.DataSources.Add(rprtDTSource);

                //ReportViewer1.LocalReport.Refresh();
                ReportViewer1.DataBind();
                ReportViewer1.Visible = true;
                ASPxLabel1.Visible = false;

            }
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
            ReportViewer1.LocalReport.DataSources.Clear();
            Microsoft.Reporting.WebForms.ReportDataSource rprtDTSource = new Microsoft.Reporting.WebForms.ReportDataSource();//"DsProductAmount", ks.Tables[0].Select(lstrFilterByDataSet));

            rprtDTSource.Name = "DsDetails";
          rprtDTSource.Value = NETClass.Classes.GetDataset(string.Format("Sproc_GetAllRemoteRepInfoDate '{0}','{1}','{2}'", StartDate.ToString("yyyy-MM-dd"), EndDate.ToString("yyyy-MM-dd"), TxtUserId.Text)).Tables[0];
            ReportViewer1.LocalReport.DataSources.Add(rprtDTSource);
            ReportViewer1.DataBind();
            ReportViewer1.Visible = true;
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

    protected void ASPxButton2_Click(object sender, EventArgs e)
    {
        UpdateDataViewFilterExpression();
    }


    protected void ReportViewer1_ReportRefresh1(object sender, System.ComponentModel.CancelEventArgs e)
    {
        UpdateDataViewFilterExpression();
    }

}