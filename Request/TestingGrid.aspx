<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestingGrid.aspx.cs" Inherits="Request_TestingGrid" %>

<%@ Register assembly="DevExpress.Web.v19.1, Version=19.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">

a:link, a:visited
{
    color: #034af3;
}

        </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <dx:ASPxGridView ID="ctrlGrid1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataRequestType" KeyFieldName="CardTypeCode" CssFilePath="~/App_Themes/SoftOrange/{0}/styles.css" CssPostfix="SoftOrange">
                <Columns>
                    <dx:GridViewDataTextColumn FieldName="CardTypeCode" VisibleIndex="0">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="CardTypeDesc" VisibleIndex="1">
                        <DataItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Eval("CardTypeCode") %>'></asp:TextBox>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                </Columns>
                <SettingsLoadingPanel ImagePosition="Top" />
                <Paddings Padding="1px" />
                <Images SpriteCssFilePath="~/App_Themes/SoftOrange/{0}/sprite.css">
                    <LoadingPanelOnStatusBar Url="~/App_Themes/SoftOrange/GridView/gvLoadingOnStatusBar.gif">
                    </LoadingPanelOnStatusBar>
                    <LoadingPanel Url="~/App_Themes/SoftOrange/GridView/Loading.gif">
                    </LoadingPanel>
                </Images>
                <ImagesFilterControl>
                    <LoadingPanel Url="~/App_Themes/SoftOrange/Editors/Loading.gif">
                    </LoadingPanel>
                </ImagesFilterControl>
                <Styles CssFilePath="~/App_Themes/SoftOrange/{0}/styles.css" CssPostfix="SoftOrange" GroupButtonWidth="28">
                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                    </Header>
                    <LoadingPanel ImageSpacing="8px">
                    </LoadingPanel>
                </Styles>
                <StylesEditors>
                    <CalendarHeader Spacing="1px">
                    </CalendarHeader>
                    <ProgressBar Height="29px">
                    </ProgressBar>
                </StylesEditors>
            </dx:ASPxGridView>
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
                                  <dx:ASPxMemo ID="tatAccountDesc" runat="server" MaxLength="25"  Text='<%# Eval("AccountDesc") %>'      NullText="Account Name/ Description" MaskSettings-ShowHints="True" Width="100%">
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
                                                
                                                  <dx:ASPxMemo ID="tatAddress" runat="server" CommandName='<%# Eval("remSeqNo") %>'  NullText="Address of Customer" MaskSettings-ShowHints="True">
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
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:EazyConnect %>" 
        SelectCommand="SpCardRemoteRequestAllAppr" SelectCommandType="StoredProcedure">
         <SelectParameters>
         <asp:ControlParameter ControlID="txtUserID" Name="userId" Type="String" PropertyName="Text"   />
             <asp:ControlParameter ControlID="txtlocation"  Name="locationID" PropertyName="Text" Type="String" />
         <asp:Parameter DefaultValue="5" Name="status" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
            <dx:ASPxComboBox ID="ASPxComboBox1" runat="server">
            </dx:ASPxComboBox>
            <dx:ASPxDateEdit ID="ASPxDateEdit2" runat="server">
            </dx:ASPxDateEdit>
            <dx:ASPxMemo ID="ASPxMemo1" runat="server" Height="71px" Width="170px">
            </dx:ASPxMemo>
            <dx:ASPxGridView ID="ASPxGridView1" runat="server">
            </dx:ASPxGridView>
     <asp:TextBox ID="txtUserID" runat="server" Visible="False"></asp:TextBox>
     <asp:TextBox ID="txtlocation" runat="server" Visible="False"></asp:TextBox>
            <asp:SqlDataSource ID="SqlDataRequestType" runat="server" 
        ConnectionString="<%$ ConnectionStrings:EazyConnect %>" 
        SelectCommand="SpCardRemoteReqSel" SelectCommandType="StoredProcedure">
         <SelectParameters>
             <asp:Parameter DefaultValue="5" Name="status" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
            <br />
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Submit Test" />
        </div>
    </form>
</body>
</html>
