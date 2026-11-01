<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NapsQueryServices.aspx.cs" Inherits="eOperations_NapsQueryServices" %>


<%@ Register assembly="DevExpress.Web.v19.1, Version=19.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>NAPs ServiceXpress</title>
  
     <link  rel="Shortcut Icon"  href="~/RadiusExpress.jpg"  />
     <script type="text/javascript">
    // <![CDATA[
         var timeout;
         function scheduleGridUpdate(grid) {
             window.clearTimeout(timeout);
             timeout = window.setTimeout(
                function () { grid.Refresh(); },
                8000
            );
         }
         function grid_Init(s, e) {
             scheduleGridUpdate(s);
         }
         function grid_BeginCallback(s, e) {
             window.clearTimeout(timeout);
         }
         function grid_EndCallback(s, e) {
             scheduleGridUpdate(s);
         }
    // ]]>
    </script>
    </head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <table width="70%" >
          
            <tr>
                <td>
                     <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                     <asp:CustomValidator ID="CustomValidator1" runat="server" Display="Dynamic" 
                         ErrorMessage="CustomValidator" ForeColor="Red"></asp:CustomValidator>
                </td>
            </tr>
          
            <tr>
                <td>
                     <table  >
                         <tr>
                             <td>
                                 Message Type:</td>
                             <td>
                                 <dx:ASPxComboBox 
                                     ID="ASPxComboBox1" runat="server"  
                                     >
                                     <Items>
                                         <dx:ListEditItem Text="Inbox" Value="1" 
                                             ImageUrl="~/Images/reviews_joined.gif" />
                                         <dx:ListEditItem Text="PendingBox" Value="0" 
                                             ImageUrl="~/Images/reviewers.gif" />
                                         <dx:ListEditItem Text="Sentbox" Value="2" 
                                             ImageUrl="~/Images/reviews_sent.gif" />
                                         <dx:ListEditItem Text="FailBox" Value="-3" 
                                             ImageUrl="~/Images/review_same_reviewers.gif" />
                                         <dx:ListEditItem Text="Today" Value="6" ImageUrl="~/Images/review_shared.gif" Selected="true" />
                                     </Items>
                                 </dx:ASPxComboBox>
                             </td>
                             <td>
                                 &nbsp;</td>
                             <td>
                                 &nbsp;</td>
                         </tr>
                     </table>
                </td>
            </tr>
          
            <tr>
                <td>
                     <dx:ASPxGridView ID="grid" ClientInstanceName="grid" runat="server" DataSourceID="SqlDataSource1"
        Width="100%" AutoGenerateColumns="False" ClientIDMode="AutoID" 
                         onafterperformcallback="grid_AfterPerformCallback">
        <ClientSideEvents Init="grid_Init" BeginCallback="grid_BeginCallback" EndCallback="grid_EndCallback"  />
        <Columns>
            <dx:GridViewDataTextColumn Caption="Transaction Description" 
                  VisibleIndex="0" Visible="False" />
<dx:GridViewDataTextColumn   ReadOnly="True" Caption="InMsg Responded" VisibleIndex="1" 
                SortIndex="1" SortOrder="Descending" Visible="False">
</dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Name="Percent" Caption="InMsg UnResponded" 
                  ReadOnly="True"
                SortOrder="Descending" VisibleIndex="2" SortIndex="0" Visible="False">
                <DataItemTemplate>
                    <dx:ASPxImage runat="server" ID="icon" ImageUrl="<%# GetIconImageUrl(Container) %>"
                        Visible="<%# GetIconImageVisibility(Container) %>" />
                    <%# GetPercentageText(Container) %>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="OutMsg Failed"   
                ReadOnly="True" VisibleIndex="3" Visible="False" />
            <dx:GridViewDataTextColumn Caption="Msg_id"   
                VisibleIndex="4" Visible="True" FieldName="msg_id" Name="msg_id">
                  <DataItemTemplate>
                    <dx:ASPxLabel ID="lblmsg_id" runat="server" Text='<%# Eval("msg_id") %>'>

                    </dx:ASPxLabel>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Msg_acctno"   
                VisibleIndex="4" Visible="True" FieldName="Msg_acctno" Name="Msg_acctno">
                  <DataItemTemplate>
                    <dx:ASPxLabel ID="lblMsg_acctno" runat="server" Text='<%# Eval("Msg_acctno") %>'>

                    </dx:ASPxLabel>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
             <dx:GridViewDataTextColumn Caption="SortCode"   
                VisibleIndex="4" Visible="True" FieldName="SortCode" Name="SortCode">
                  <DataItemTemplate>
                    <dx:ASPxLabel ID="lblSortCode" runat="server" Text='<%# Eval("SortCode") %>'>

                    </dx:ASPxLabel>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
              <dx:GridViewDataTextColumn Caption="Amount"   
                VisibleIndex="4" Visible="True" FieldName="Amount" Name="Amount">
                  <DataItemTemplate>
                    <dx:ASPxLabel ID="lblAmount" runat="server" Text='<%# Eval("Amount") %>'>

                    </dx:ASPxLabel>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
             <dx:GridViewDataTextColumn Caption="FeeAmt"   
                VisibleIndex="4" Visible="True" FieldName="FeeAmt" Name="FeeAmt">
                  <DataItemTemplate>
                    <dx:ASPxLabel ID="lblFeeAmt" runat="server" Text='<%# Eval("FeeAmt") %>'>

                    </dx:ASPxLabel>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
             <dx:GridViewDataTextColumn Caption="MobileNo"   
                VisibleIndex="4" Visible="True" FieldName="MobileNo" Name="MobileNo">
                  <DataItemTemplate>
                    <dx:ASPxLabel ID="lblMobileNo" runat="server" Text='<%# Eval("MobileNo") %>'>

                    </dx:ASPxLabel>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
             <dx:GridViewDataTextColumn Caption="BeneficiaryAcctNo"   
                VisibleIndex="4" Visible="True" FieldName="BeneficiaryAcctNo" Name="BeneficiaryAcctNo">
                  <DataItemTemplate>
                    <dx:ASPxLabel ID="lblBeneficiaryAcctNo" runat="server" Text='<%# Eval("BeneficiaryAcctNo") %>'>

                    </dx:ASPxLabel>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
             <dx:GridViewDataTextColumn Caption="TransRefNo"   
                VisibleIndex="4" Visible="True" FieldName="TransRefNo" Name="TransRefNo">
                  <DataItemTemplate>
                    <dx:ASPxLabel ID="lblTransRefNo" runat="server" Text='<%# Eval("TransRefNo") %>'>

                    </dx:ASPxLabel>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="OutMsg Waiting"  
                VisibleIndex="4" Visible="False">
            </dx:GridViewDataTextColumn>
             <dx:GridViewDataTextColumn Caption="AccountNo"  FieldName ="AccountNo"
                VisibleIndex="4" Visible="True">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataCheckColumn Caption="Approval Status"  
                VisibleIndex="4" Visible="False">
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataDateColumn Caption="Time" FieldName="Time" VisibleIndex="9">
                <PropertiesDateEdit DisplayFormatString="HH:mm:ss" >
                
                </PropertiesDateEdit>
                <CellStyle HorizontalAlign="Right" />
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn Caption="Message" VisibleIndex="6" 
                FieldName="msg_text" Name="msg_text" Width="200px">
                   <DataItemTemplate>
                    <dx:ASPxLabel ID="lblmsg_text" runat="server" Text='<%# Eval("msg_text") %>'>
                    </dx:ASPxLabel>
                </DataItemTemplate>
                   <CellStyle Wrap="True">
                   </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="BatchRefCode" VisibleIndex="5" 
                FieldName="msg_to" Name="msg_to">
                <DataItemTemplate>
                    <dx:ASPxLabel ID="lblmsg_to" runat="server" Text='<%# Eval("msg_to") %>' ToolTip ='<%# Eval("msg_id") %>'>
                    </dx:ASPxLabel>
                </DataItemTemplate>
                <FooterTemplate>
               <%-- <%#  NIPQueryService.ExecuteavalBal()%>--%>
                </FooterTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Sent" VisibleIndex="7" FieldName="sent_on" 
                Name="sent_on">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Last Update" VisibleIndex="8" 
                FieldName="updated_on" Name="updated_on">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Status" FieldName="msg_status" 
                Name="msg_status" VisibleIndex="10" Visible ="false">
                <DataItemTemplate>
                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text='<%# Eval("msg_status") %>' >
                    </dx:ASPxLabel>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>

             <dx:GridViewDataTextColumn Caption=" " FieldName="MsgStatDesc" 
                Name="MsgStatDesc" VisibleIndex="11">
                <DataItemTemplate>
                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text='<%# Eval("MsgStatDesc") %>' >
                    </dx:ASPxLabel>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
        </Columns>
        <Styles>
            <Header HorizontalAlign="Center" />
        </Styles>
        <Settings GridLines="Vertical" ShowVerticalScrollBar="True" />
        <SettingsBehavior AllowDragDrop="false" />
        <SettingsPager Mode="ShowAllRecords" />
        <SettingsLoadingPanel Mode="ShowOnStatusBar" />
        <Settings ShowFooter="True" />
      <TotalSummary>
            <dx:ASPxSummaryItem FieldName="msg_id" SummaryType="Count"  ShowInColumn="sent_on" />
            
        </TotalSummary>
    </dx:ASPxGridView></td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
            </tr>
     
        </table>
    
    </div>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:EazyConnect %>" 
        SelectCommand="Sproc_GetNapsTsQueryStatus" SelectCommandType="StoredProcedure">
 
    </asp:SqlDataSource>
    </form>
</body>
</html>