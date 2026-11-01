<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Rs_CardRequest.aspx.vb" Inherits="Reports_Rs_CardRequest" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

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
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana"
            Font-Size="8pt" Height="700px" Width="100%">
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
