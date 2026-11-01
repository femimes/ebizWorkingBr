<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RsMessaging.aspx.cs" Inherits="Reports_RsMessaging" %>


<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="DevExpress.Web.v19.1, Version=19.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Message Request Report</title>
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
    <link href="../Styles/SiteConsole.css" rel="stylesheet" type="text/css" />
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
              <dx:FilterControlColumn ColumnType="String" DisplayName="Serial No" PropertyName="Sequence">
              </dx:FilterControlColumn>
              <dx:FilterControlColumn ColumnType="String" DisplayName="Message" PropertyName="MessageText">
              </dx:FilterControlColumn>
              <dx:FilterControlColumn ColumnType="DateTime" DisplayName="Message DateAdded" PropertyName="DateAdded">
              </dx:FilterControlColumn>
              <dx:FilterControlColumn ColumnType="Integer" DisplayName="Message Status" PropertyName="Sent">
              </dx:FilterControlColumn>
               
              <dx:FilterControlColumn ColumnType="String" DisplayName="Card DisplayName" PropertyName="Description">
              </dx:FilterControlColumn>
              <dx:FilterControlColumn ColumnType="String" DisplayName="Account No" PropertyName="AccountNo">
              </dx:FilterControlColumn>
              <dx:FilterControlColumn ColumnType="DateTime" DisplayName="Message Tag" PropertyName="MessageType">
              </dx:FilterControlColumn>
              <dx:FilterControlColumn ColumnType="String" DisplayName="Transaction Reference" PropertyName="TransID">
              </dx:FilterControlColumn>
          </Columns>
      </dx:ASPxFilterControl>
  
  </td></tr>
  <tr><td style="width: 864px"> 
         <table><tr><td>Start Date:</td>
            <td>
                <dx:ASPxDateEdit ID="ASPxDateEdit1" runat="server" DisplayFormatString="D" 
                       >
                </dx:ASPxDateEdit>
            </td>
            <td>End Date:</td><td>
            <dx:ASPxDateEdit ID="ASPxDateEdit2" runat="server" DisplayFormatString="D" 
                  >
            </dx:ASPxDateEdit>
            </td>
            <td> 
                <dx:ASPxButton ID="ASPxButton2" runat="server" Text="go" 
                    onclick="ASPxButton2_Click" Height="25px">
                </dx:ASPxButton>
                </td></tr></table></td></tr>
  <tr><td style="width: 864px">
  
       
  </td></tr>
  </table>
   <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana"
            Font-Size="8pt" Height="700px" Width="100%" 
          DocumentMapCollapsed="True" SizeToReportContent="True" 
          onreportrefresh="ReportViewer1_ReportRefresh1" 
          InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana" 
          WaitMessageFont-Size="14pt">
            <LocalReport ReportPath="Reports\Report.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="SqlDataSource1" Name="DsReport" />
                 </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
        &nbsp; &nbsp;
       <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:EazyConnect %>" 
        SelectCommand="Sp_RadiusMsgPerdate" SelectCommandType="StoredProcedure">
   <SelectParameters >
    
 
          <asp:ControlParameter ControlID="ASPxDateEdit1" Name="StartDate" PropertyName="Date" 
                                                                          Type ="DateTime" />
                                                                          
          <asp:ControlParameter ControlID="ASPxDateEdit2" Name="Endate" PropertyName="Date" 
                                                                           
              Type ="DateTime"    />
                                                               
        </SelectParameters>
    </asp:SqlDataSource>
    
 </div>
    </form>
</body>
</html>

