<%@ Page Language="C#" AutoEventWireup="true" CodeFile="eAccountLimit.aspx.cs" Inherits="Reports_eAccountLimit" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="DevExpress.Web.v19.1, Version=19.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>eChannels AccountLimit</title>
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

 <div class="card-title " style="margin-left:5%"><h4> Account EChannels Limit Report </h4></div>
 </div>
  <div class="row"  style="margin-left:5%">
      
      <div ><dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="ASPxLabel">
        </dx:ASPxLabel></div>
      <dx:ASPxFilterControl ID="ASPxFilterControl1" runat="server">
             <Columns>
              <dx:FilterControlColumn ColumnType="String" DisplayName="Branch Name" PropertyName="BranchName">
              </dx:FilterControlColumn>
              <dx:FilterControlColumn ColumnType="String" DisplayName="Account No" PropertyName="AccountNo">
              </dx:FilterControlColumn>
               <dx:FilterControlColumn ColumnType="String" DisplayName="Policy Type" PropertyName="PolicyDesc">
              </dx:FilterControlColumn>
              <dx:FilterControlColumn ColumnType="DateTime" DisplayName="Request Date" PropertyName="systemDateTime">
              </dx:FilterControlColumn>
              <dx:FilterControlColumn ColumnType="String" DisplayName="Status" PropertyName="Status">
              </dx:FilterControlColumn>
               <dx:FilterControlColumn ColumnType ="Decimal" DisplayName="Amount" PropertyName="PolicyValue">
              </dx:FilterControlColumn>
                     
              <dx:FilterControlColumn ColumnType="String" DisplayName="Request By" PropertyName="userid">
              </dx:FilterControlColumn>
                 <dx:FilterControlColumn ColumnType="String" DisplayName="Approved By" PropertyName="approveUserId">
              </dx:FilterControlColumn>
                    <dx:FilterControlColumn ColumnType="DateTime" DisplayName="Approval Date" PropertyName="approveDateTime">
              </dx:FilterControlColumn>

                <dx:FilterControlColumn ColumnType="String" DisplayName="Account Name" PropertyName="AccountDesc">
              </dx:FilterControlColumn>
              <dx:FilterControlColumn ColumnType="Boolean" DisplayName="Apply Policy" PropertyName="ApplyPolicy">
              </dx:FilterControlColumn>
              
              
          </Columns>
         
          
      </dx:ASPxFilterControl>
  
  </div>

<%--  
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
--%>

      <div class="form-group row" style="margin-left:3%">
        <%--  <div><dx:ASPxButton ID="ASPxButton2" runat="server" Text="go" 
                    onclick="ASPxButton2_Click" Height="25px">
                </dx:ASPxButton></div>--%>
          <div class="card "> 
   <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana"
            Font-Size="8pt" Height="700px" Width="100%" 
          DocumentMapCollapsed="True" SizeToReportContent="True" 
          onreportrefresh="ReportViewer1_ReportRefresh1" 
          InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana" 
          WaitMessageFont-Size="14pt">
         <LocalReport ReportPath="Reports\eAccountLimit.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="SqlDataSource1" Name="DsUserAccountLimits" />
                 </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
        &nbsp; &nbsp;
              </div>
       <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:EazyConnect %>" 
        SelectCommand="Sproc_RepAccountLimits" SelectCommandType="StoredProcedure">
 
    </asp:SqlDataSource>
    </div>
 </div>
            </div>
 
    </form>
</body>
</html>



