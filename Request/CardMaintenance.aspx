<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CardMaintenance.aspx.cs" Inherits="Request_CardMaintenance" %>
<%@ Register Assembly="DevExpress.Web.v19.1, Version=19.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

 <!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../Styles/SiteAll.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        function ctrlGrid_SelectionChanged(s, e) {


            s.GetSelectedFieldValues("SeqNum", GetSelectedFieldValuesCallback);

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
</head>
<body>
     <form id="form1" runat="server">
    <div>
    
        <table style="width:100%;">
            <tr>
                <td colspan="3">
                    <b>Maintain eRequest</b> <font color="Red"><asp:Literal ID="Literal1" runat="server"></asp:Literal></font></td>
            </tr>
           <table style="width:100%;" border="0">
            <%--<tr>
                <td colspan="3">
                    <b>eRequest Maintenance</b></td>
            </tr>--%>
            <tr>
                <td colspan="3">
                    <table border="0">
                        <tr>
                            <td style="width: 100px;white-space:nowrap;">
                                &nbsp;Select BatchRefNo: </td>
                            <td style="width: 100px">
                               <dx:ASPxComboBox ID="drpBatchRefNo" runat="server" EnableIncrementalFiltering="True" EnableSynchronization="False" IncrementalFilteringMode="StartsWith" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"    ShowShadow="False" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" DataSourceID="SqlDbReqType" ValueField="RefNo" AutoPostBack="True">
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
                        <dx:ListBoxColumn Caption="Batch RefNo" FieldName="RefNo" />
                        <dx:ListBoxColumn Caption="Request Title" FieldName="RequestTitle" />
                    </Columns>
                                   <ItemStyle Wrap="True" />
                </dx:ASPxComboBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
               <tr><td colspan="3"> <table><tr>
                   <td>Maintain IssueDate:</td><td>   <dx:ASPxDateEdit ID="DatCardIssue"       runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" ShowShadow="False" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"    >
                   <CalendarProperties>
                       <HeaderStyle Spacing="1px" />
                       <FooterStyle Spacing="17px" />
                   </CalendarProperties>
                   <DropDownButton>
                       <Image>
                           <SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" PressedCssClass="dxEditors_edtDropDownPressed_Aqua" />
                       </Image>
                   </DropDownButton>
                   <ValidationSettings>
                       <ErrorFrameStyle ImageSpacing="4px">
                           <ErrorTextPaddings PaddingLeft="4px" />
                       </ErrorFrameStyle>
                   </ValidationSettings>
                   </dx:ASPxDateEdit>
                               </td>
                   <td>Maintain ExpiryDate:</td><td>   <dx:ASPxDateEdit ID="DatCardExpiry"    runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" ShowShadow="False" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"    >
                   <CalendarProperties>
                       <HeaderStyle Spacing="1px" />
                       <FooterStyle Spacing="17px" />
                   </CalendarProperties>
                   <DropDownButton>
                       <Image>
                           <SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" PressedCssClass="dxEditors_edtDropDownPressed_Aqua" />
                       </Image>
                   </DropDownButton>
                   <ValidationSettings>
                       <ErrorFrameStyle ImageSpacing="4px">
                           <ErrorTextPaddings PaddingLeft="4px" />
                       </ErrorFrameStyle>
                   </ValidationSettings>
                   </dx:ASPxDateEdit>
                             </td>
                                           </tr></table></td></tr>
            <tr>
                <td colspan="3">
                    
                   <dx:ASPxGridView ID="ctrlGrid" runat="server" AutoGenerateColumns="False" 
                        ClientIDMode="AutoID" DataSourceID="SqlDbReqDetails"
                        KeyFieldName="SeqNum" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"
                        width="100%" >
                        <SettingsPager    PageSize ="20" Position="TopAndBottom" 
                            SEOFriendly="CrawlerOnly">
                        </SettingsPager>
                        <Settings ShowGroupPanel="True" ShowVerticalScrollBar="True"  ShowHorizontalScrollBar="False"
                            UseFixedTableLayout="True" VerticalScrollBarStyle="Standard"   />
                            <Columns>
                              <dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width ="30px" FixedStyle="Left"  >
                                    <HeaderTemplate>
                                     <dx:ASPxCheckBox ID="SelectAllCheckBox" runat="server" ToolTip="Select/Unselect all rows on the page"
                                            ClientSideEvents-CheckedChanged="function(s, e) { ctrlGrid.SelectAllRowsOnPage(s.GetChecked()); }" />
                                    </HeaderTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </dx:GridViewCommandColumn>
                         
                             
                             <dx:GridViewDataTextColumn FieldName="SeqNum" VisibleIndex="1" Name="SeqNum" Caption=" ReferenceID" Width ="150px" FixedStyle="Left"  >
                            </dx:GridViewDataTextColumn>
                             
                                   <dx:GridViewDataTextColumn Caption="Account No" FieldName="AccountNo" VisibleIndex="0" Width="120px">
                                  <%--  <DataItemTemplate><div class="auto-style9">
                                    <asp:CheckBox ID="CheckBox1"    ToolTip='<%# Eval("AccountNo") %>'   runat="server" Text ='<%# Eval("AccountNo") %>' />
                                   </div> </DataItemTemplate>--%>
                               <%--     <HeaderTemplate>
                                    <asp:CheckBox ID="CheckBox2" runat="server"  AutoPostBack="true"   Text="Account No" /> 
                                    </HeaderTemplate>--%>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="AccountDesc" VisibleIndex="1" Caption ="Account Name" Width="200px">
                                </dx:GridViewDataTextColumn>
                                 <dx:GridViewDataDateColumn   VisibleIndex="2" FieldName="requestType" Caption="Request Type" Width="100px" Visible="false" >
                                    <PropertiesDateEdit DisplayFormatString="D">
                                    </PropertiesDateEdit>
                                </dx:GridViewDataDateColumn>
                                   <dx:GridViewDataDateColumn   VisibleIndex="2" FieldName="CardTypeDesc" Caption="Request Description" Width="120px"  >
                                    <PropertiesDateEdit DisplayFormatString="D">
                                    </PropertiesDateEdit>
                                </dx:GridViewDataDateColumn>
                                  <dx:GridViewDataDateColumn   VisibleIndex="14" FieldName="SystemDateTime" Caption="Request Date" Width="200px"   >
                                    <PropertiesDateEdit DisplayFormatString="D">
                                    </PropertiesDateEdit>
                                </dx:GridViewDataDateColumn>
                                 <dx:GridViewDataDateColumn   VisibleIndex="10" FieldName="CardIssueDate" Caption="Issue Date" Width="200px"   >
                                    <PropertiesDateEdit DisplayFormatString="D">
                                    </PropertiesDateEdit>
                                  

                                </dx:GridViewDataDateColumn>
                                <dx:GridViewDataDateColumn  VisibleIndex="11" FieldName="CardExpiryDate" Caption="Expiry Date" Width="200px"   >
                                    <PropertiesDateEdit DisplayFormatString="D">
                                    </PropertiesDateEdit>
                                   
                                </dx:GridViewDataDateColumn>
                              <%--  <dx:GridViewDataTextColumn FieldName="BranchName"   Caption= "Branch" VisibleIndex="4" Width="150px">
                                </dx:GridViewDataTextColumn>--%>
                                <dx:GridViewDataTextColumn Caption="Reason" FieldName="remarks" Name="remarks" VisibleIndex="20" Width="200px">
                                    <DataItemTemplate>
                                        <table style="width:100%;">
                                            <tr>
                                                <td>
                                                    <dx:ASPxMemo ID="tatReason" runat="server"    CommandName='<%# Eval("SeqNum") %>' MaskSettings-ShowHints="True" NullText="Reason for maintenance">
                                                    </dx:ASPxMemo>
                                           <%--         <asp:TextBox ID="chkTat" runat="server"></asp:TextBox>--%>
                                                </td>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                            </tr>
                                        </table>
                                    </DataItemTemplate>
                                </dx:GridViewDataTextColumn>
                        </Columns>
                        <settingsbehavior allowfocusedrow="True"  
                                                enablerowhottrack="True" AutoExpandAllGroups="True" /> 
                                   <settings showfilterbar="Visible" showgrouppanel="True" />
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
                        <ClientSideEvents SelectionChanged="ctrlGrid_SelectionChanged"  />
                    </dx:ASPxGridView>
                    
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <table  >
                        <tr>
                            <td align="right" class="style2">
                                <dx:ASPxButton ID="BtnApprove" runat="server" Text="Submit Selection" 
                                    Width="166px" OnClick="BtnApprove_Click" >
                                </dx:ASPxButton>
                            </td>
                           <%-- <td align="left" class="style2">
                                <dx:ASPxButton ID="btnDisApprove" runat="server" Text="DisApprove Selection" 
                                    Width="175px" OnClick="btnDisApprove_Click" >
                                </dx:ASPxButton>
                            </td>--%>
                            <td align="left" class="style2">
                                <dx:ASPxButton ID="btncancel" runat="server" Text="Cancel" 
                                     >
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
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                      <div class="TopPadding">
                                    Selected count: <span id="selCount" style="font-weight: bold">0</span>
                                </div>
                       <dx:ASPxListBox ID="ASPxListBox1" ClientInstanceName="selList" runat="server" 
                                              Height="0px"  ReadOnly ="True"  Width="20px"
                                    CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" 
                                       
                                    SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"  >
                                    <LoadingPanelImage Url="~/App_Themes/Aqua/Editors/Loading.gif">
                                    </LoadingPanelImage>
                                    <ValidationSettings>
                                        <ErrorFrameStyle ImageSpacing="4px">
                                            <ErrorTextPaddings PaddingLeft="4px" />
                                        </ErrorFrameStyle>
                                    </ValidationSettings>
                                </dx:ASPxListBox>
                              
                    </td>
                <td class="style1">
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
        </table>
    
    </div>     <asp:TextBox ID="txtUserID" runat="server" Visible="False"></asp:TextBox>
     <asp:TextBox ID="txtlocation" runat="server" Visible="False"></asp:TextBox>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:EazyConnect %>" 
        SelectCommand="SpCardRequestMaint" SelectCommandType="StoredProcedure">
         <SelectParameters>
         <asp:ControlParameter ControlID="txtUserID" Name="userId" Type="String" PropertyName="Text"   />
             <asp:ControlParameter ControlID="txtlocation"   Name="locationID" PropertyName="Text" Type="String" />
           <asp:ControlParameter ControlID="drpBatchRefNo"   Name="RefNo" PropertyName="Value" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>

         <td><asp:SqlDataSource ID="SqlDbReqType" runat="server" 
        ConnectionString="<%$ ConnectionStrings:EazyConnect %>" 
        SelectCommand="Sproc_GetCardForMaint" SelectCommandType="StoredProcedure">
       <%-- <SelectParameters>
            <asp:Parameter DefaultValue="5" Name="status" Type="Int32" />
            </SelectParameters>--%>
    </asp:SqlDataSource>
             <asp:SqlDataSource ID="SqlDbReqDetails" runat="server" 
        ConnectionString="<%$ ConnectionStrings:EazyConnect %>" 
        SelectCommand="SpCardRequestMaint" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtUserID" Name="userId" Type="String" PropertyName="Text" DefaultValue="00"     />
             <asp:ControlParameter ControlID="txtlocation"   Name="locationID" PropertyName="Text" Type="String" DefaultValue="00" />
             
               <asp:ControlParameter ControlID="drpBatchRefNo"  Name="RefNo" PropertyName="Value" Type="String" DefaultValue=""/>
        </SelectParameters>
    </asp:SqlDataSource>
    </form>
</body>
</html>