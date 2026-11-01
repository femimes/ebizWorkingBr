<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DeStmt.aspx.cs" Inherits="Reports_DeStmt" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

 
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <script type="text/javascript">
    // <![CDATA[
         function UpdateInfoPanelText() {
             var text = filter.IsFilterExpressionValid() ? filter.cpFilterExpression : "Filter expression is not valid";
             if (text == "") text = "Filter expression is empty";
             filterExpressionLabel.SetText(text);
         }
    // ]]>
         
    </script>
    <link href="../Styles/SiteAll.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
           <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>


            <br />
               
                   <rsweb:ReportViewer ID="ReportViewer1" runat="server" BackColor="" ClientIDMode="AutoID" HighlightBackgroundColor="" InternalBorderColor="204, 204, 204" InternalBorderStyle="Solid" InternalBorderWidth="1px" LinkActiveColor="" LinkActiveHoverColor="" LinkDisabledColor="" PrimaryButtonBackgroundColor="" PrimaryButtonForegroundColor="" PrimaryButtonHoverBackgroundColor="" PrimaryButtonHoverForegroundColor="" SecondaryButtonBackgroundColor="" SecondaryButtonForegroundColor="" SecondaryButtonHoverBackgroundColor="" SecondaryButtonHoverForegroundColor="" SplitterBackColor="" ToolbarDividerColor="" ToolbarForegroundColor="" ToolbarForegroundDisabledColor="" ToolbarHoverBackgroundColor="" ToolbarHoverForegroundColor="" ToolBarItemBorderColor="" ToolBarItemBorderStyle="Solid" ToolBarItemBorderWidth="1px" ToolBarItemHoverBackColor="" ToolBarItemPressedBorderColor="51, 102, 153" ToolBarItemPressedBorderStyle="Solid" ToolBarItemPressedBorderWidth="1px" ToolBarItemPressedHoverBackColor="153, 187, 226" Width="1093px">
            <LocalReport ReportPath="Reports\AccountEstatementNew.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="SqlDataSource1" Name="DataSet2_VwTransHist" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
    
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:EazyConnectSource %>" 
        
                        SelectCommand="SELECT BranchName AS sub_name, AccountNo AS account_no, SeqNo AS seq_no, AddedBy AS posted_by, PostingDateAdded AS post_date, ValueDate AS trans_date, ParentAccountNo AS mini_account_no, currCode AS curr_code, ISNULL((SELECT TOP (1) BalAcctCurr FROM TRanshist WHERE (AccountNo = '0000094018') ORDER BY PostingDateAdded DESC), 0) AS 'base_bf', AccountDesc AS description, AccountDesc AS accountname, UserNarrative AS narrative, debit, credit, BalAcctCurr AS balance, TransID AS ref_no, BranchAdded AS headsub_code, AcctExchRate AS exch_rate, transCode AS trans_code, UserNarrative AS narration, 0 AS reversed, CAST('2020-01-21' AS datetime) AS startdate, CAST('2023-01-21' AS datetime) AS enddate, dbo.FnCustomerAddress(AccountNo, 0) AS address, dbo.FnCustomerCity(AccountNo, 0) AS city, dbo.FnAccountProductName(AccountNo) AS prod_desc, ProdCode AS prod_code, TransDesc AS trans_desc FROM VwTransHist WHERE (AccountNo = '0000094018')">
       
    </asp:SqlDataSource>
        </div>
    </form>
</body>
</html>
