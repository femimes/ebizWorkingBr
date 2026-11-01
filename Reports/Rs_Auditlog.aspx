<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Rs_Auditlog.aspx.cs" Inherits="Reports_Rs_Auditlog" %>



<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="DevExpress.Web.v19.1, Version=19.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>AuditLog Report</title>
    <link href="../Styles/SiteAll.css" rel="stylesheet" type="text/css" />
        <script type="text/javascript">
    // <![CDATA[
            function UpdateInfoPanelText() {
                var text = filter.IsFilterExpressionValid() ? filter.cpFilterExpression : "Filter expression is not valid";
                if (text == "") text = "Filter expression is empty";
                filterExpressionLabel.SetText(text);
            }
    // ]]>
    </script>
     <link href="../Resources/css/app.min.css" rel="stylesheet" />
    <link href="../Resources/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Resources/css/bootstrap.css" rel="stylesheet" />
    <link href="../Resources/css/loginstyle.css" rel="stylesheet" />
    <link href="../Styles/SiteConsole.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
       
<div id="layout-wrapper">
    
  <div class="container-fluid">

  <asp:scriptmanager ID="ScriptManager1" runat="server" />
     <div class="form-group row">

 <div class="card-title " style="margin-left:5%"><h4> Application AuditLog Report</h4></div>
 </div>
  <div class="row"  style="margin-left:5%;">
      
      <div ><dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="ASPxLabel">
        </dx:ASPxLabel></div>
      <dx:ASPxFilterControl ID="ASPxFilterControl1" runat="server">
          <Columns>
              <dx:FilterControlColumn ColumnType="String" DisplayName="Audit No" PropertyName="SequenceNo">
              </dx:FilterControlColumn>
              <dx:FilterControlColumn ColumnType="String" DisplayName="User Name" PropertyName="UserID">
              </dx:FilterControlColumn>
              <dx:FilterControlColumn ColumnType="DateTime" DisplayName="Period Or Posting Date" PropertyName="PeriodDate">
              </dx:FilterControlColumn>
             
               
              <dx:FilterControlColumn ColumnType="String" DisplayName="Operation Or Audit Type" PropertyName="AuditType">
              </dx:FilterControlColumn>
              <dx:FilterControlColumn ColumnType="String" DisplayName="Remarks" PropertyName="remarks">
              </dx:FilterControlColumn>
              <dx:FilterControlColumn ColumnType="DateTime" DisplayName="Operation Time" PropertyName="SystemDate">
              </dx:FilterControlColumn>
         
          </Columns>
      </dx:ASPxFilterControl>
  
  </div>

  
  <div class="form-group row "  style="margin-left:5%;width:100%" >
      
      <div style="width:10%" >Start Date: </div> 

      <div class="col-lg-3"  style="width:40%"> 
         
                <dx:ASPxDateEdit ID="ASPxDateEdit1" runat="server" DisplayFormatString="D"  >
                </dx:ASPxDateEdit>
            </div>
            <div class="col-lg-1"  style="width:10%">End Date: </div>
      <div class="col-lg-3"  style="width:40%">
      <div class="row"><div> <dx:ASPxDateEdit ID="ASPxDateEdit2" runat="server" DisplayFormatString="D" 
                  >
            </dx:ASPxDateEdit></div><div><dx:ASPxButton ID="ASPxButton2" runat="server" Text="go" 
                    onclick="ASPxButton2_Click" Height="25px">
                </dx:ASPxButton></div></div> 
           
          <div  >
            </div></div>
        
 
  </div>


      <div class="form-group row" style="margin-left:3%">
          <div class="card "> 
   <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana"
            Font-Size="8pt" Height="700px" Width="100%" 
          DocumentMapCollapsed="True" SizeToReportContent="True" 
          onreportrefresh="ReportViewer1_ReportRefresh1" 
          InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana" 
          WaitMessageFont-Size="14pt">
            <LocalReport ReportPath="Reports\CustAuditLogGrp.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="SqlDataSource1" Name="DataSet1" />
                 </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
        &nbsp; &nbsp;
              </div>
       <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:EazyConnect %>" 
        SelectCommand="Sproc_RepGetAuditLogDate" SelectCommandType="StoredProcedure">
   <SelectParameters >
    
 
          <asp:ControlParameter ControlID="ASPxDateEdit1" Name="StartDate" PropertyName="Date" 
                                                                          Type ="DateTime" />
                                                                          
          <asp:ControlParameter ControlID="ASPxDateEdit2" Name="Endate" PropertyName="Date" 
                                                                           
              Type ="DateTime"    />
                                                               
        </SelectParameters>
    </asp:SqlDataSource>
    </div>
 </div>
            </div>
 
    </form>
</body>
</html>

