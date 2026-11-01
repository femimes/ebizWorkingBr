<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CardLinkAllo.aspx.vb" Inherits="Request_CardLinkAllo" %>
<%@ Register Assembly="DevExpress.Web.v19.1, Version=19.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../Styles/SiteAll.css" rel="stylesheet" type="text/css" />
     <script type="text/javascript">

        function ctrlGrid_SelectionChanged(s, e) {


            s.GetSelectedFieldValues("accountno", GetSelectedFieldValuesCallback);

        }

        function GetSelectedFieldValuesCallback(values) {
            selList.Visible = true;
            selList.BeginUpdate();

            try {
                selList.ClearItems();

                for (var i = 0; i < values.length; i++) {
                    selList.AddItem(values[i].toString());
                }
            } finally {
                selList.EndUpdate();
            }
            document.getElementById("selCount").innerHTML = ctrlGrid.GetSelectedRowCount();
            //           selList.Visible = false;
        }



   </script>
</head><body>
    <form id="form1" runat="server">
    <div>
    
        <table style="width:100%;" border="0">
            <tr>
                <td colspan="3">
                    <b>Card Linking Maintenance (Allocated)</b></td>
            </tr>
            <tr>
                <td colspan="3">
                    <table border="0">
                        <tr>
                            <td style="width: 100px">
                                Select BatchRefNo:</td>
                            <td style="width: 100px">
                               <dx:ASPxComboBox ID="drpBatchRefNo" runat="server" EnableIncrementalFiltering="True" EnableSynchronization="False" IncrementalFilteringMode="StartsWith" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"    ShowShadow="False" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" DataSourceID="SqlDataSource1" ValueField="RefNo" AutoPostBack="True">
                    <LoadingPanelImage Url="~/App_Themes/Aqua/Editors/Loading.gif">
                    </LoadingPanelImage>
                    <DropDownButton>
                        <Image>
                            <SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" PressedCssClass="dxEditors_edtDropDownPressed_Aqua" />
                        </Image>
                    </DropDownButton>
                    <ValidationSettings>
                        <ErrorFrameStyle ImageSpacing="1px">
                            <ErrorTextPaddings PaddingLeft="1px" />
                        </ErrorFrameStyle>
                    </ValidationSettings>
                    <Columns>
                        <dx:ListBoxColumn Caption="Batch RefNo" FieldName="batchRefNo" />
                        <dx:ListBoxColumn Caption="Request Title" FieldName="RequestTitle" />
                    </Columns>
                                   <ItemStyle Wrap="True" />
                </dx:ASPxComboBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <dx:ASPxGridView ID="ctrlGrid" runat="server" AutoGenerateColumns="False" 
                        ClientIDMode="AutoID" 
                        KeyFieldName="accountno" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"
                        Width="100%" DataSourceID="SqlDataSource2">
                        <SettingsPager PageSize="100">
                        </SettingsPager>
                             <Columns>
                                  <dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width ="30px" FixedStyle="Left"  >
                                    <HeaderTemplate>
                                     <dx:ASPxCheckBox ID="SelectAllCheckBox" runat="server" ToolTip="Select/Unselect all rows on the page"
                                            ClientSideEvents-CheckedChanged="function(s, e) { ctrlGrid.SelectAllRowsOnPage(s.GetChecked()); }" />
                                    </HeaderTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </dx:GridViewCommandColumn>
                         

                                 <dx:GridViewDataDateColumn Caption="Account No" FieldName="accountno" VisibleIndex="0" >


                                 </dx:GridViewDataDateColumn>
                                 <dx:GridViewDataDateColumn Caption="Card ExpiryDate" FieldName="CardExpiryDate" VisibleIndex="17" PropertiesDateEdit-DisplayFormatString="{0:dd-MMM-yyyy}">
                                      
                                 </dx:GridViewDataDateColumn>
                                 <dx:GridViewDataDateColumn Caption="Card IssueDate" FieldName="CardIssueDate" VisibleIndex="16"  PropertiesDateEdit-DisplayFormatString="{0:dd-MMM-yyyy}" >
                                    
                                 </dx:GridViewDataDateColumn>

                                   <dx:GridViewDataDateColumn Caption="Link Account" FieldName="accountno" VisibleIndex="15">
                                     <PropertiesDateEdit DisplayFormatString="D">
                                     </PropertiesDateEdit>
                                      <DataItemTemplate>
                                          <div>
                                         <dx:ASPxTextBox ID="txtAccountNo" runat="server" MaxLength="10" ToolTip='<%# Eval("accountno") %>' AutoPostBack="true" OnTextChanged="txtAccountNoChnaged" >
                                         </dx:ASPxTextBox>
                                     </div>
                                          <div>
                                          <dx:ASPxLabel ID="lblLinkAcctName" runat="server" ToolTip='<%# Eval("accountno") %>'   >
                                         </dx:ASPxLabel>
                                          </div>
                                     </DataItemTemplate>
                                 </dx:GridViewDataDateColumn>

                                 <dx:GridViewDataTextColumn Caption="Card DisplayName" FieldName="AccountName" VisibleIndex="1">
                                     <HeaderStyle Wrap="False" />
                                 </dx:GridViewDataTextColumn>

                                  <dx:GridViewDataTextColumn Caption="Card PIN" FieldName="accountno" VisibleIndex="16">
                                     
                                      <DataItemTemplate>
                                          <div>
                                         <dx:ASPxTextBox ID="txtCardPIN" runat="server"  ToolTip='<%# Eval("accountno") %>' Password="true" MaxLength="4" Text="1234"  NullText="Card PIN"  >
                                         </dx:ASPxTextBox>
                                     </div>
                                          
                                     </DataItemTemplate>
                                 </dx:GridViewDataTextColumn>
                                  <dx:GridViewDataTextColumn Caption="Card CVV" FieldName="accountno" VisibleIndex="18">
                                     
                                      <DataItemTemplate>
                                          <div>
                                         <dx:ASPxTextBox ID="txtCardVV" runat="server" MaxLength="3" ToolTip='<%# Eval("accountno") %>' Password="true" NullText="Card CVV" >
                                             
                                         </dx:ASPxTextBox>
                                     </div>
                                          
                                     </DataItemTemplate>
                                 </dx:GridViewDataTextColumn>
                                 <dx:GridViewDataTextColumn Caption="Address" FieldName="Address" VisibleIndex="4">
                                    
                                 </dx:GridViewDataTextColumn>
                                 <dx:GridViewDataTextColumn Caption="Product" FieldName="Product" VisibleIndex="3">
                                 </dx:GridViewDataTextColumn>
                                 <dx:GridViewDataTextColumn Caption="PanNo" FieldName="PANno"   VisibleIndex="2">
                                 <DataItemTemplate>
                                      <div>
                                          <dx:ASPxLabel ID="LblPanNo" runat="server" ToolTip='<%# Eval("PANno") %>' Text='<%# Left(Eval("PANno"), 6) + "*********" + Right(Eval("PANno"), 4) %>'  >
                                         </dx:ASPxLabel>
                                          </div>
                                 </DataItemTemplate>
                                 </dx:GridViewDataTextColumn>
                                 <dx:GridViewDataTextColumn Caption="Decision" VisibleIndex="23" Visible="false">
                                     <DataItemTemplate>
                                         <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Confirm">
                                         </dx:ASPxButton>
                                     </DataItemTemplate>
                                 </dx:GridViewDataTextColumn>
                                 <dx:GridViewDataTextColumn Caption="Issue Counter" FieldName="CardIssueCounter" Visible="false" VisibleIndex="22">
                                 </dx:GridViewDataTextColumn>
                                  <dx:GridViewDataTextColumn Caption="SeqNum" FieldName="SeqNum" Visible="false" VisibleIndex="23">
                                 </dx:GridViewDataTextColumn>
                    </Columns>
                                        <GroupSummary>
                                                <dx:ASPxSummaryItem DisplayFormat="Total ={0}"  
                                                    SummaryType="Count" />
                                            </GroupSummary>

<SettingsBehavior AllowFocusedRow="True" AutoExpandAllGroups="True" EnableRowHotTrack="True"></SettingsBehavior>

<Settings ShowGroupPanel="True" ShowFilterBar="Visible"></Settings>
                        <SettingsLoadingPanel ImagePosition="Top" />
                        <Images SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                            <LoadingPanelOnStatusBar Url="~/App_Themes/Aqua/GridView/gvLoadingOnStatusBar.gif">
                            </LoadingPanelOnStatusBar>
                            <LoadingPanel Url="~/App_Themes/Aqua/GridView/Loading.gif">
                            </LoadingPanel>
                        </Images>
                        <ImagesEditors>
                            <DropDownEditDropDown>
                                <SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" 
                                    PressedCssClass="dxEditors_edtDropDownPressed_Aqua" />
                            </DropDownEditDropDown>
                            <SpinEditIncrement>
                                <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditIncrementImageHover_Aqua" 
                                    PressedCssClass="dxEditors_edtSpinEditIncrementImagePressed_Aqua" />
                            </SpinEditIncrement>
                            <SpinEditDecrement>
                                <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditDecrementImageHover_Aqua" 
                                    PressedCssClass="dxEditors_edtSpinEditDecrementImagePressed_Aqua" />
                            </SpinEditDecrement>
                            <SpinEditLargeIncrement>
                                <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeIncImageHover_Aqua" 
                                    PressedCssClass="dxEditors_edtSpinEditLargeIncImagePressed_Aqua" />
                            </SpinEditLargeIncrement>
                            <SpinEditLargeDecrement>
                                <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeDecImageHover_Aqua" 
                                    PressedCssClass="dxEditors_edtSpinEditLargeDecImagePressed_Aqua" />
                            </SpinEditLargeDecrement>
                        </ImagesEditors>
                        <ImagesFilterControl>
                            <LoadingPanel Url="~/App_Themes/Aqua/Editors/Loading.gif">
                            </LoadingPanel>
                        </ImagesFilterControl>
                        <Styles CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua">
                            <LoadingPanel ImageSpacing="8px">
                            </LoadingPanel>
                        </Styles>
                        <StylesEditors>
                            <CalendarHeader Spacing="1px">
                            </CalendarHeader>
                            <ProgressBar Height="25px">
                            </ProgressBar>
                        </StylesEditors>
                    </dx:ASPxGridView>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <table border="0"  >
                        <tr>
                            <td align="right" class="style2">
                                <dx:ASPxButton ID="BtnApprove" runat="server" Text="Confirm Selection" 
                                    Width="166px" onclick="BtnApprove_Click" Height="16px">
                                </dx:ASPxButton>
                            </td>
                            <td align="left" class="style2">
                                <dx:ASPxButton ID="BtnDisapprove" runat="server" Visible="false" Text="Remove Selection" 
                                    Width="166px">
                                </dx:ASPxButton>
                            </td>
                            <td align="left" class="style2">
                                <dx:ASPxButton ID="btncancel" runat="server" Text="Cancel" 
                                    onclick="btncancel_Click">
                                </dx:ASPxButton>
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
                    </table>
                </td>
                <td>
                    &nbsp;<asp:TextBox ID="txtuserID" runat="server" Visible="False"></asp:TextBox></td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td class="style1">
                    &nbsp;</td>
                <td><asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:EazyConnect %>" 
        SelectCommand="SpGetCardsAllocatedByUser" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:Parameter DefaultValue="0" Name="status" Type="Int32" />

            <asp:ControlParameter DefaultValue="0" ControlID="txtuserID"  Name="userId" />
            </SelectParameters>
    </asp:SqlDataSource><asp:SqlDataSource ID="SqlDataSource2" runat="server" 
        ConnectionString="<%$ ConnectionStrings:EazyConnect %>" 
        SelectCommand="Sproc_GetAllCardAlloAccountsByRefNo" SelectCommandType="StoredProcedure">
        <SelectParameters>
    
               <asp:ControlParameter DefaultValue="0" ControlID="drpBatchRefNo"  Name="RefNo" />
            <asp:ControlParameter DefaultValue="0" ControlID="txtuserID"  Name="userId"  />
              <asp:Parameter DefaultValue="15" Name="status" Type="Int32" />
        </SelectParameters>

             </asp:SqlDataSource>
                    &nbsp;&nbsp;</td>
            </tr> 
    
        </table>
    
   
</div>
    </form>
</body>
</html>

