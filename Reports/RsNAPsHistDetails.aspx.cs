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

public partial class Reports_RsNAPsHistDetails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack && !IsCallback)
        {
            ASPxDateEdit1.Date = DateTime.Today.Date;
            ASPxDateEdit2.Date = DateTime.Today.AddDays(1.0);
            UpdateDataViewFilterExpression();
            ReportViewer1.LocalReport.DisplayName = "Messaging Details";
        }
    }
    void UpdateDataViewFilterExpression()
    {
        UpdateDataSourceFilterExpression();
        ReportViewer1.DataBind();

    }
    void UpdateDataSourceFilterExpression()
    {
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
                SqlDataSource1.FilterExpression = lstrFilterByDataSet;
                SqlDataSource1.DataBind();


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