<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Rs_CardRequestAll.aspx.vb"   Inherits="Reports_Rs_CardRequestAll" %>


<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="DevExpress.Web.v19.1, Version=19.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Card Request Report</title>
        <script type="text/javascript">
    // <![CDATA[
            function UpdateInfoPanelText() {
                var text = filter.IsFilterExpressionValid() ? filter.cpFilterExpression : "Filter expression is not valid";
                if (text == "") text = "Filter expression is empty";
                filterExpressionLabel.SetText(text);
            }
    // ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
  <div>
  <asp:scriptmanager ID="ScriptManager1" runat="server"  ></asp:ScriptManager>
  <table border ="0" >
  <tr><td style="width: 864px"><dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="ASPxLabel">
        </dx:ASPxLabel>
      <dx:ASPxFilterControl ID="ASPxFilterControl1" runat="server">
          <Columns>
              <dx:FilterControlColumn ColumnType="String" DisplayName="Pan No" PropertyName="PanNo">
              </dx:FilterControlColumn>
              <dx:FilterControlColumn ColumnType="String" DisplayName="Card Brand" PropertyName="CardBrand">
              </dx:FilterControlColumn>
              <dx:FilterControlColumn ColumnType="DateTime" DisplayName="Card ExpiryDate" PropertyName="CardExpiryDate">
              </dx:FilterControlColumn>
              <dx:FilterControlColumn ColumnType="Integer" DisplayName="Card Counter" PropertyName="CardCounter">
              </dx:FilterControlColumn>
              <dx:FilterControlColumn ColumnType="String" DisplayName="Batch RefNo" PropertyName="RefNo">
              </dx:FilterControlColumn>
               <dx:FilterControlColumn ColumnType="String" DisplayName="Card Status" PropertyName="Status">
              </dx:FilterControlColumn>
              <dx:FilterControlColumn ColumnType="DateTime" DisplayName="Confirm Date" PropertyName="ConfirmDateTime">
              </dx:FilterControlColumn>
              <dx:FilterControlColumn ColumnType="String" DisplayName="ConfirmBy" PropertyName="ConfirmBy">
              </dx:FilterControlColumn>
              <dx:FilterControlColumn ColumnType="String" DisplayName="Approved By" PropertyName="ApprovedBy">
              </dx:FilterControlColumn>
              <dx:FilterControlColumn ColumnType="String" DisplayName="Added By" PropertyName="AddedBy">
              </dx:FilterControlColumn>
              <dx:FilterControlColumn DisplayName="City" PropertyName="City">
              </dx:FilterControlColumn>
              <dx:FilterControlColumn ColumnType="String" DisplayName="Contact Address" PropertyName="Address">
              </dx:FilterControlColumn>
              <dx:FilterControlColumn ColumnType="String" DisplayName="Card DisplayName" PropertyName="Description">
              </dx:FilterControlColumn>
              <dx:FilterControlColumn ColumnType="String" DisplayName="Account No" PropertyName="AccountNo">
              </dx:FilterControlColumn>
              <dx:FilterControlColumn ColumnType="DateTime" DisplayName="Card IssueDate" PropertyName="CardIssueDate">
              </dx:FilterControlColumn>
              <dx:FilterControlColumn ColumnType="String" DisplayName="Request Title" PropertyName="RequestTitle">
              </dx:FilterControlColumn>
          </Columns>
      </dx:ASPxFilterControl>
  
  </td></tr>
  <tr><td style="width: 864px">
  
       
  </td></tr>
  </table>
   <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana"
            Font-Size="8pt" Height="700px" Width="100%" ShowBackButton="True" DocumentMapCollapsed="True" SizeToReportContent="True">
            <LocalReport ReportPath="Reports\CardRequest.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="SqlDataSource1" Name="DsCardRequest_CardRequest" />
                 </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
      <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetData"
          TypeName="DsCardRequestTableAdapters.CardRequestAdapter"></asp:ObjectDataSource>
        &nbsp; &nbsp;
        <asp:SqlDataSource ID="SqlDataSource1" runat="server"  
           >
        </asp:SqlDataSource>
    
 </div>
    </form>
</body>
</html>
