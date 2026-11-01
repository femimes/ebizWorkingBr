<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RsStatement.aspx.cs" Inherits="Reports_RsStatement" %>


<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register assembly="DevExpress.Web.v19.1, Version=19.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

 

 

 

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
 
<head   runat="server">
    <title>Production Analysis Report</title>
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
            <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
       <dx:ASPxLabel ID="ASPxLabel1" runat="server">
        </dx:ASPxLabel>
        <table style="width:100%;">
        <tr><td colspan ="3">
        
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
                    onclick="ASPxButton2_Click">
                </dx:ASPxButton>
                </td>
             <td>
                 <dx:ASPxTextBox ID="txtAccountNo" runat="server" Width="170px">
                 </dx:ASPxTextBox>
             </td>
             </tr></table>
        </td></tr>
            <tr>
                <td colspan="3">
                    <table >
                        <tr>
                            <td align="right">
        <dx:ASPxFilterControl ID="ASPxFilterControl1" runat="server" 
            ClientIDMode="AutoID" OnCustomJSProperties="ASPxFilterControl1_CustomJSProperties" >
            <Columns>
                  
                        <dx:FilterControlColumn ColumnType="String" DisplayName="Product Names" 
                    PropertyName="trans_desc"> </dx:FilterControlColumn>

                <dx:FilterControlColumn ColumnType="String" DisplayName="Product Class" 
                    PropertyName="prodCategoryDesc">
                 </dx:FilterControlColumn>
                    <dx:FilterControlColumn ColumnType="String" DisplayName="Status" 
                    PropertyName="def_desc">
                 </dx:FilterControlColumn>
                 <dx:FilterControlColumn ColumnType="String" DisplayName="Approved By" 
                    PropertyName="approved_by"></dx:FilterControlColumn>
                       <dx:FilterControlColumn ColumnType="DateTime" DisplayName="Payment Date" 
                    PropertyName="RegistryDate" ></dx:FilterControlColumn>
                <dx:FilterControlColumn ColumnType="String" DisplayName="PolicyNo" 
                    PropertyName="SubscriptionID">
                </dx:FilterControlColumn>
                 <dx:FilterControlColumn ColumnType="Decimal" DisplayName="Amount" 
                     PropertyName="Amount">
                 </dx:FilterControlColumn>
                      <dx:FilterControlColumn ColumnType="String" DisplayName="Narration" 
                    PropertyName="Narrative"></dx:FilterControlColumn>
                     <dx:FilterControlColumn ColumnType="String" DisplayName="Company Code" 
                    PropertyName="Company_code"></dx:FilterControlColumn>
                        <dx:FilterControlColumn ColumnType="String" DisplayName="Payment RefNo" 
                    PropertyName="PaymentRefNo"></dx:FilterControlColumn>
                     <dx:FilterControlColumn ColumnType="Integer" DisplayName="PaymentType Code" 
                    PropertyName="PaymentModeType"></dx:FilterControlColumn>
                         <dx:FilterControlColumn ColumnType="string" DisplayName="Payment Narrative" 
                    PropertyName="Narrative"></dx:FilterControlColumn>
                     <dx:FilterControlColumn ColumnType="string" DisplayName="AgentCode Name" 
                    PropertyName="PurAgentCode"></dx:FilterControlColumn>
            </Columns>
        </dx:ASPxFilterControl>
      </td>
                            <td align="left">
                                <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Apply" Visible="False" 
                                    onclick="ASPxButton1_Click">
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                    
                </td>
            </tr>
            <tr>
                <td colspan="3">
    <div>
    
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" BackColor="" ClientIDMode="AutoID" HighlightBackgroundColor="" InternalBorderColor="204, 204, 204" InternalBorderStyle="Solid" InternalBorderWidth="1px" LinkActiveColor="" LinkActiveHoverColor="" LinkDisabledColor="" PrimaryButtonBackgroundColor="" PrimaryButtonForegroundColor="" PrimaryButtonHoverBackgroundColor="" PrimaryButtonHoverForegroundColor="" SecondaryButtonBackgroundColor="" SecondaryButtonForegroundColor="" SecondaryButtonHoverBackgroundColor="" SecondaryButtonHoverForegroundColor="" SplitterBackColor="" ToolbarDividerColor="" ToolbarForegroundColor="" ToolbarForegroundDisabledColor="" ToolbarHoverBackgroundColor="" ToolbarHoverForegroundColor="" ToolBarItemBorderColor="" ToolBarItemBorderStyle="Solid" ToolBarItemBorderWidth="1px" ToolBarItemHoverBackColor="" ToolBarItemPressedBorderColor="51, 102, 153" ToolBarItemPressedBorderStyle="Solid" ToolBarItemPressedBorderWidth="1px" ToolBarItemPressedHoverBackColor="153, 187, 226" Width="1093px">
            <LocalReport ReportPath="Reports\AccountEstatementNew.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="SqlDataSource1" Name="DataSet2_VwTransHist" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
    
    </div>
    </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td colspan="3">

       <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:EazyConnect %>" 
        
                        SelectCommand="SELECT BranchName as sub_name, AccountNo as account_no, SeqNo as seq_no, AddedBy as posted_by, PostingDateAdded as post_date, ValueDate as trans_date,ParentAccountNo as mini_account_no, currCode as curr_code, isnull((select top 1 BalAcctCurr FROM TRanshist WHERE AccountNo =@param3 AND  PostingDateAdded &lt; @Param2 order by PostingDateAdded desc ),0) as 'base_bf',AccountDesc as description, AccountDesc as accountname, UserNarrative as narrative, debit, credit, BalAcctCurr as balance, TransID as ref_no, BranchAdded as headsub_code, AcctExchRate as exch_rate, transCode as trans_code, UserNarrative as narration, 0 as reversed, cast(@Param1 as datetime)  as startdate, cast(@Param2 as datetime)  as enddate, dbo.FnCustomerAddress(AccountNo, 0) as address, dbo.FnCustomerCity(AccountNo, 0) as city, dbo.FnAccountProductName(AccountNo) as prod_desc, ProdCode as prod_code, TransDesc as trans_desc  FROM VwTransHist WHere accountno =@param3 AND PostingDateAdded >=@Param1 AND PostingDateAdded <= @Param2 order by PostingDaAdded, seq_no">
        <SelectParameters >
       
            <asp:ControlParameter ControlID="txtAccountNo" Name="param3" 
                PropertyName="Text" />
       
          <asp:ControlParameter ControlID="ASPxDateEdit2" Name="Param2" 
                PropertyName="Value" />
 
          <asp:ControlParameter ControlID="ASPxDateEdit1" Name="Param1" 
                PropertyName="Value" />
                                                                          
        </SelectParameters>
    </asp:SqlDataSource>
     </td>
            </tr>
        </table>
    
    </form>
</body>
</html>

