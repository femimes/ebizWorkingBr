<%@ Page Language="C#" AutoEventWireup="true" CodeFile="eChannelsRequest.aspx.cs" Inherits="Request_eChannelsRequest" %>
<%@ Register assembly="DevExpress.Web.v19.1, Version=19.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

 
<%--<!DOCTYPE html>--%>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../Styles/SiteAll.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        function ctrlGrid_SelectionChanged(s, e) {


            s.GetSelectedFieldValues("remSeqNo", GetSelectedFieldValuesCallback);

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
    <style type="text/css">
        .auto-style1 {
            width: 100px;
        }
        </style>
</head>
<body>
     <form id="form1" runat="server">
    <div>
    
        <table style="width:100%;">
           
           
             <tr> 
                <td colspan="3">
                    <table class="auto-style1">
                        <tr><td colspan="4" >

                      <b>eRequest Booking</b> <font color="Red"><asp:Literal ID="Literal1" runat="server"></asp:Literal></font>
                    <asp:TextBox ID="txtref_no" runat="server" Visible="false"></asp:TextBox>
                            </td></tr>
                        <tr><td style="white-space:nowrap">eRequest Type:</td>
                            <td colspan="3" >
      <dx:ASPxComboBox ID="drpBatchRefNo" runat="server" EnableIncrementalFiltering="True" EnableSynchronization="False" IncrementalFilteringMode="StartsWith" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"    ShowShadow="False" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" DataSourceID="SqlDataRequestType" ValueField="CardTypeCode" AutoPostBack="True">
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
                        <dx:ListBoxColumn Caption="eRequestType Code" FieldName="CardTypeCode" />
                        <dx:ListBoxColumn Caption="eRequestType Description" FieldName="CardTypeDesc" />
                    </Columns>
                                   <ItemStyle Wrap="True" />
                </dx:ASPxComboBox>
                       

                            </td></tr>
                        <tr >
            <td style="text-align: right;white-space:nowrap"  >
                Request Title:</td>
            <td  colspan="3" style="text-align: left; white-space:nowrap"  >
                <dx:aspxtextbox ID="txtTitle" runat="server" Width="100%" 
                    CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" 
                    SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" 
                    NullText="Enter Request Title" RightToLeft="False">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
                        <ErrorFrameStyle ImageSpacing="4px">
                            <ErrorTextPaddings PaddingLeft="4px" />
                        </ErrorFrameStyle>
                        <RequiredField ErrorText="Title is required" IsRequired="True" />
                    </ValidationSettings>
                   
                </dx:aspxtextbox>
            </td>
            <td  colspan="1" rowspan="6" style="text-align: left">
                &nbsp;</td>
        </tr>
                        <tr >
            <td style="text-align: right; white-space:nowrap"  >
                 Issue Date:</td>
            <td align="center" colspan="1" style="height: 30px; text-align: left; width: 289px;">
               <dx:aspxdateedit ID="dateIssue" runat="server"   DisplayFormatString="D"
                    EditFormat="Custom" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" 
                    CssPostfix="Aqua" ShowShadow="False" 
                    SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" 
                    NullText="Select Card Issue Date">
                    <DropDownButton>
                      
                    </DropDownButton>
                    <CalendarProperties>
                        <HeaderStyle Spacing="1px" />
                        <FooterStyle Spacing="17px" />
                    </CalendarProperties>
                    <ValidationSettings  Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
                        <ErrorFrameStyle ImageSpacing="4px">
                            <ErrorTextPaddings PaddingLeft="4px" />
                        </ErrorFrameStyle>
                        <RequiredField ErrorText="Issue Date is required" IsRequired="True" />
                    </ValidationSettings>
                </dx:aspxdateedit>
            </td>
            <td align="right" nowrap >
                Expiry Date:</td>
            <td align="left" colspan="1" nowrap>
                <dx:aspxdateedit ID="dateexpiry" runat="server"  DisplayFormatString="D"
                    EditFormat="Custom" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" 
                    CssPostfix="Aqua" ShowShadow="False" 
                    SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" 
                    NullText="Select Card Expiry Date">
                    <DropDownButton>
                        <Image>
                            <SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" PressedCssClass="dxEditors_edtDropDownPressed_Aqua" />
                        </Image>
                    </DropDownButton>
                    <CalendarProperties>
                        <HeaderStyle Spacing="1px" />
                        <FooterStyle Spacing="17px" />
                    </CalendarProperties>
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
                        <ErrorFrameStyle ImageSpacing="4px">
                            <ErrorTextPaddings PaddingLeft="4px" />
                        </ErrorFrameStyle>
                        <RequiredField ErrorText="Expiry Date is required" IsRequired="True" />
                    </ValidationSettings>
                </dx:aspxdateedit>
            </td>
        </tr>
                        </table>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <dx:ASPxGridView ID="ctrlGrid" runat="server" AutoGenerateColumns="False" 
                        ClientIDMode="AutoID" DataSourceID="SqlDataSource1"
                        KeyFieldName="remSeqNo" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"
                        width="100%" >
                        <SettingsPager    PageSize ="2000" Position="TopAndBottom" 
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
                         
                             
                             <dx:GridViewDataTextColumn FieldName="remSeqNo" VisibleIndex="0" Name="remSeqNo" Caption=" TrackID" Width ="150px" FixedStyle="Left"  >
                            </dx:GridViewDataTextColumn>
                             
                                   <dx:GridViewDataTextColumn Caption="Account No" FieldName="accountNo" VisibleIndex="1" Width="120px">
                                  <%--  <DataItemTemplate><div class="auto-style9">
                                    <asp:CheckBox ID="CheckBox1"    ToolTip='<%# Eval("AccountNo") %>'   runat="server" Text ='<%# Eval("AccountNo") %>' />
                                   </div> </DataItemTemplate>--%>
                               <%--     <HeaderTemplate>
                                    <asp:CheckBox ID="CheckBox2" runat="server"  AutoPostBack="true"   Text="Account No" /> 
                                    </HeaderTemplate>--%>
                                </dx:GridViewDataTextColumn>
                                 <dx:GridViewDataTextColumn Caption="Date of Birth" FieldName="DOB" 
                            VisibleIndex="3" ReadOnly="True" Width="100px">
                            <DataItemTemplate>
                                <dx:ASPxDateEdit ID="ASPxDateEdit1" runat="server" PopupVerticalAlign="Above" Value='<%# Eval("DOB") %>' Width="100%" AutoResizeWithContainer="True">
                                    <Paddings Padding="0px" />
                                </dx:ASPxDateEdit>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Display Name" FieldName="AccountDesc" 
                            VisibleIndex="1" Width="150px" ReadOnly="True">
                            <DataItemTemplate>
                                <%--<dx:ASPxLabel ID="ASPxTextBox2" runat="server" MaxLength="25" Text='<%# Eval("AccountDesc") %>' ToolTip ='<%# Eval("remSeqNo") %>' 
                                    Width="100%" Visible="false">
                                </dx:ASPxLabel>--%>
                                  <dx:ASPxMemo ID="tatAccountDesc" runat="server" MaxLength="25"  Text='<%# Eval("AccountDesc") %>'  ToolTip ='<%# Eval("remSeqNo") %>'    NullText="Account Name/ Description" MaskSettings-ShowHints="True" Width="100%">
                                                </dx:ASPxMemo>
                            </DataItemTemplate>
                            <HeaderStyle Wrap="False" />
                        </dx:GridViewDataTextColumn>
                                
                                <dx:GridViewDataTextColumn FieldName="BranchName"   Caption= "Branch" VisibleIndex="23" Width="150px">
                                </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn VisibleIndex="4" Width ="200px" Caption="Address">

                                <DataItemTemplate>
                                    <table style="width:100%;">
                                        <tr>
                                            <td>
                                                
                                                  <dx:ASPxMemo ID="tatAddress" runat="server" CommandName='<%# Eval("remSeqNo") %>'  NullText="Address of Customer" Text='<%# Eval("Address") %>' MaskSettings-ShowHints="True">
                                                </dx:ASPxMemo>

                                            </td>
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                    </table>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                                  <dx:GridViewDataTextColumn Caption="Product" FieldName="ProdName" VisibleIndex="2" Width="100px">
                        </dx:GridViewDataTextColumn>
                         
                        <dx:GridViewDataTextColumn Caption="City" FieldName="city" VisibleIndex="5" 
                            Width="100px" ReadOnly="True">
                            <DataItemTemplate>
                                <dx:ASPxTextbox ID="TxtCity" runat="server" Text='<%# Eval("city") %>' Width="100%">
                                </dx:ASPxTextbox>
                              </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Book Bal" FieldName="Book Bal" 
                            VisibleIndex="18" Visible="False" Width="60px">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="PhoneNo" VisibleIndex="7" 
                            FieldName="PhoneNo" Width="150px">
                         <DataItemTemplate>
                                <dx:ASPxTextBox ID="ASPxTextBox4" runat="server" Text='<%# Eval("PhoneNo") %>' Width="100%">
                                </dx:ASPxTextBox>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Cleared Bal" FieldName="Cleared Bal" 
                            VisibleIndex="14" Visible="False" Width="100px">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Transaction Status" 
                            FieldName="PostStatusDesc" VisibleIndex="19" Width="120px">
                        </dx:GridViewDataTextColumn>
                        
                        <dx:GridViewDataTextColumn Caption="Account Status" FieldName="AccountStatusDesc" 
                            VisibleIndex="21" Width="100px">
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
                          <Images SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                            <LoadingPanelOnStatusBar Url="~/App_Themes/Aqua/GridView/gvLoadingOnStatusBar.gif">
                            </LoadingPanelOnStatusBar>
                            <LoadingPanel Url="~/App_Themes/Aqua/GridView/Loading.gif">
                            </LoadingPanel>
                        </Images>
                        
                        <SettingsLoadingPanel ImagePosition="Top" />
                        <ClientSideEvents SelectionChanged="ctrlGrid_SelectionChanged"  />
                    </dx:ASPxGridView>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <table  >
                        <tr>
                            <td align="right" class="style2">
                                <dx:ASPxButton ID="BtnApprove" runat="server" Text="Book Selection" 
                                    Width="175px" OnClick="BtnApprove_Click" >
                                </dx:ASPxButton>
                            </td>
                            <td align="left" class="style2">
                                <dx:ASPxButton ID="btnDisApprove" runat="server" Text="Drop Selection" 
                                    Width="175px" OnClick="btnDisApprove_Click" >
                                </dx:ASPxButton>
                            </td>
                            <td align="left" class="style2">
                                <dx:ASPxButton ID="btncancel" runat="server" Text="Cancel" OnClick="btncancel_Click" 
                                   Width="175px"  >
                                </dx:ASPxButton>
                            </td>
                             <td align="left" class="style2">
                               
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
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
    
    </div>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:EazyConnect %>" 
        SelectCommand="SpCardRemoteRequestAllApprType" SelectCommandType="StoredProcedure">
         <SelectParameters>
         <asp:ControlParameter ControlID="txtUserID" Name="userId" Type="String" PropertyName="Text"   />
          <asp:ControlParameter ControlID="txtlocation"  Name="locationID" PropertyName="Text" Type="String" />

         <asp:Parameter DefaultValue="5" Name="status" Type="Int32" />
     <asp:ControlParameter ControlID="drpBatchRefNo"  Name="requestType" PropertyName="Value" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
     <asp:TextBox ID="txtUserID" runat="server" Visible="False"></asp:TextBox>
     <asp:TextBox ID="txtlocation" runat="server" Visible="False"></asp:TextBox>
    &nbsp;<asp:SqlDataSource ID="SqlDataRequestType" runat="server" 
        ConnectionString="<%$ ConnectionStrings:EazyConnect %>" 
        SelectCommand="SpCardRemoteReqSel" SelectCommandType="StoredProcedure">
         <SelectParameters>
             <asp:Parameter DefaultValue="5" Name="status" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    </form>
</body>
</html>
