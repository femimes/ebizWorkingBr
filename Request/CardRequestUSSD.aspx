<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CardRequestUSSD.aspx.vb" Inherits="Request_CardRequestUSSD" %>
<%@ Register assembly="DevExpress.Web.v19.1, Version=19.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

 
  

 

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
      <style type="text/css">
        .style2
        {
            height: 39px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table id="TABLE1" border="0" cellpadding="3" cellspacing="2" class="photo-frame"
        style="border-right:   thin outset; border-top: thin outset; font-size: 7pt; border-left: thin outset;
        width: 730px; color:DarkBlue; border-bottom: thin outset; font-family: Verdana,  'Microsoft Sans Serif';
        height: 1px">
        <tr style="color: #9999cc">
            <td align="center" colspan="5" style="height: 29px; text-align: left">
                <strong> Card USSD Request Manager</strong><asp:TextBox ID="seqno" runat="server"
                    Visible="False">0</asp:TextBox>&nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; 
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp;
                &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; <a href="EmailListings.aspx" target="_blank">
                    Preview E-Address</a></td>
        </tr>
        <tr style="color: #9999cc">
            <td align="right" >
                Request Title:</td>
            <td align="left" colspan="4"  >
                <dx:aspxtextbox ID="txtTitle" runat="server" Width="405px" 
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
        </tr>
        <tr style="color: #9999cc">
            <td align="right" >
                Card IssueDate:</td>
            <td align="center" colspan="1" style="height: 30px; text-align: left; width: 289px;">
                &nbsp;<dx:aspxdateedit ID="dateIssue" runat="server"   DisplayFormatString="D"
                    EditFormat="Custom" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" 
                    CssPostfix="Aqua" ShowShadow="False" 
                    SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" 
                    NullText="Select Card Issue Date">
                    <DropDownButton>
                        <Image>
                            <SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" PressedCssClass="dxEditors_edtDropDownPressed_Aqua" />
                        </Image>
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
                CardExpiryDate:</td>
            <td align="left" colspan="1" nowrap>
                &nbsp;<dx:aspxdateedit ID="dateexpiry" runat="server"  DisplayFormatString="D"
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
            <td align="center" colspan="1" rowspan="6" style="text-align: left">
                &nbsp;</td>
        </tr>
        <tr style="color: #9999cc">
            <td align="right">
                Card Brand:</td>
            <td align="center" colspan="1" style="height: 30px; text-align: left; width: 289px;">
                
                <dx:ASPxComboBox ID="drpbrand" runat="server">
                    <Items>
                        <dx:ListEditItem Text="Existing Card" Value="8" />
                    </Items>
                </dx:ASPxComboBox>
            </td>
            <td align="center" colspan="2" style="height: 30px; text-align: left">
                <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal" Visible="False">
                    <asp:ListItem Value="1">Active</asp:ListItem>
                    <asp:ListItem Value="2">Stolen</asp:ListItem>
                    <asp:ListItem Value="3">Others</asp:ListItem>
                </asp:RadioButtonList></td>
        </tr>
        <tr style="color: #9999cc">
            <td align="right" >
                Produts:</td>
            <td align="center" colspan="1" style="height: 30px; text-align: left; width: 289px;">
                <dx:aspxcombobox ID="drp_products" EnableIncrementalFiltering="True" 
                    EnableSynchronization="False" IncrementalFilteringMode="StartsWith"  
                    runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" 
                       ShowShadow="False" 
                    SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" CssClass="TableRuling" 
                    DataSourceID="sqlproducts" TextField="product" ValueField="Product Code" 
                    AutoPostBack="True">
                    <LoadingPanelImage Url="~/App_Themes/Aqua/Editors/Loading.gif">
                    </LoadingPanelImage>
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
                    <Columns>
                        <dx:ListBoxColumn Caption="Product" FieldName="Product" Name="Product" />
                        <dx:ListBoxColumn Caption="Type" FieldName="Type" Name="Type" />
                    </Columns>
                    <ItemStyle Wrap="True" />
                </dx:aspxcombobox>
            </td>
            <td align="right" >
                Branch:</td>
            <td align="left" >
                
                <table border="0" cellpadding="0" cellspacing="0" style="border-top-style: none;
                    border-right-style: none; border-left-style: none; border-bottom-style: none">
                    <tr>
                        <td align="left">
                <dx:aspxcombobox ID="drpbranches"  EnableIncrementalFiltering="True" 
                                EnableSynchronization="False" IncrementalFilteringMode="StartsWith" 
                                runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" 
                                   ShowShadow="False" 
                                SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" DataSourceID="sqlBranches" 
                                ValueField="Headsub_code" AutoPostBack="True">
                    <LoadingPanelImage Url="~/App_Themes/Aqua/Editors/Loading.gif">
                    </LoadingPanelImage>
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
                    <Columns>
                        <dx:ListBoxColumn Caption="Branch" FieldName="sub_name" />
                        <dx:ListBoxColumn Caption="Branch Code" FieldName="Headsub_code" Name="Headsub_code" />
                    </Columns>
                </dx:aspxcombobox>
                        </td>
                        <td align="left">
                            <asp:Button ID="Button4" runat="server" Text="go" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr style="color: #9999cc">
            <td align="right">
                Specific AccountList:</td>
            <td align="left" colspan="3">
                <dx:aspxmemo ID="txtSpecificAcct" runat="server" AutoPostBack="True" Height="71px"
                    Width="376px">
                </dx:aspxmemo>
                <asp:TextBox ID="TextBox1" runat="server" Visible="False">4</asp:TextBox>
                <dx:aspxcombobox ID="drp_appr" runat="server" EnableIncrementalFiltering="True" 
                    EnableSynchronization="False" IncrementalFilteringMode="StartsWith" 
                    CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" 
                       ShowShadow="False" 
                    SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" 
                    ValueField="user_id" PopupVerticalAlign="WindowCenter" Visible="False">
                    <LoadingPanelImage Url="~/App_Themes/Aqua/Editors/Loading.gif">
                    </LoadingPanelImage>
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
                    <Columns>
                        <dx:ListBoxColumn Caption="Approving Officer" FieldName="user_id" />
                    </Columns>
                </dx:aspxcombobox>
            </td>
        </tr>
        <tr style="color: #9999cc">
            <td align="center" colspan="4" class="style2">
                <table >
                    <tr>
                        <td>
                            <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Submit">
                            </dx:ASPxButton>
                        </td>
                        <td align="left">
                            <dx:ASPxButton ID="ASPxButton2" runat="server" Text="Reset" 
                                CausesValidation="False">
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr style="color: #9999cc">
            <td align="center" colspan="4" style="height: 117px; text-align: left">
                &nbsp;<asp:CheckBox ID="chkAllRecords" runat="server" Text="Select All Pages?" TextAlign="Left" />
                <dx:aspxgridview ID="ASPxGridView1" runat="server" 
                    DataSourceID="SqlacctDetails" AutoGenerateColumns="False" 
                    KeyFieldName="AccountNo" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" 
                    CssPostfix="Aqua" Width ="100%"  >
                   
                    <Settings ShowFilterBar="Visible" ShowGroupedColumns="True" ShowGroupPanel="True"
                        ShowStatusBar="Visible" />
                    <SettingsLoadingPanel ImagePosition="Top" />
                   
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="PANno" VisibleIndex="17" FieldName="PANno">
                          <DataItemTemplate>
                                <dx:ASPxLabel ID="ASPxTextBox2" runat="server" MaxLength="25" Text='<%# Eval("PANMask") %>'  
                                    Width="100%" CssClass='<%# Eval("PANno") %>'>
                                </dx:ASPxLabel>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Account No" FieldName="AccountNo" 
                            VisibleIndex="0" Width="100px">
                            <DataItemTemplate>
                                <dx:ASPxCheckBox ID="ASPxCheckBox1" runat="server" CheckState="Unchecked" Text='<%# Eval("AccountNo") %>'
                                    Wrap="False">
                                    <CheckBoxFocusedStyle Wrap="False">
                                    </CheckBoxFocusedStyle>
                                    <Border BorderStyle="None" />
                                    <CheckBoxStyle Wrap="False" />
                                </dx:ASPxCheckBox>
                            </DataItemTemplate>
                            <HeaderStyle Wrap="False" />
                            <CellStyle Wrap="False">
                            </CellStyle>
                            <HeaderTemplate>
                                <dx:ASPxCheckBox ID="ASPxCheckBox2" runat="server" AutoPostBack="True" CheckState="Unchecked"
                                    OnCheckedChanged="ASPxCheckBox2_CheckedChanged" Text="Check All">
                                    <Border BorderStyle="None" />
                                </dx:ASPxCheckBox>
                            </HeaderTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Date of Birth" FieldName="DOB" 
                            VisibleIndex="3" ReadOnly="True">
                            <DataItemTemplate>
                                <dx:ASPxDateEdit ID="ASPxDateEdit1" runat="server" PopupVerticalAlign="Above" Value='<%# Eval("DOB") %>' Width="100%" AutoResizeWithContainer="True">
                                    <Paddings Padding="0px" />
                                </dx:ASPxDateEdit>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Card DisplayName" FieldName="AccountName" 
                            VisibleIndex="1" Width="150px" ReadOnly="True">
                            <DataItemTemplate>
                                <dx:ASPxLabel ID="ASPxTextBox2" runat="server" MaxLength="25" Text='<%# Eval("AccountName") %>' ToolTip ='<%# Eval("DispLenght") %>' 
                                    Width="100%">
                                </dx:ASPxLabel>
                            </DataItemTemplate>
                            <HeaderStyle Wrap="False" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Address" FieldName="address" 
                            VisibleIndex="4" Width="150px" ReadOnly="True">
                            <DataItemTemplate>
                                &nbsp;<asp:Label ID="txtaddress" runat="server"    Text='<%# Eval("Address") %>'   ToolTip ='<%# Eval("AddLenght") %>' ></asp:Label>
                                <dx:ASPxLabel ID="ASPxTextBox1" runat="server" Height="83px" MaxLength="30" Visible="false"
                                    Text='<%# Eval("Address") %>' Width="100%" ToolTip ='<%# Eval("AddLenght") %>' >
                                </dx:ASPxLabel>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Product" FieldName="Product" VisibleIndex="2">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="SeqNo" FieldName="seq" Visible="False" 
                            VisibleIndex="6">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Issue Counter" FieldName="CardIssueCounter" 
                            VisibleIndex="22" Visible="False">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="City" FieldName="city" VisibleIndex="5" 
                            Width="100px" ReadOnly="True">
                            <DataItemTemplate>
                                <dx:ASPxLabel ID="ASPxTextBox3" runat="server" Text='<%# Eval("city") %>' Width="100%">
                                </dx:ASPxLabel>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Book Bal" FieldName="Book Bal" 
                            VisibleIndex="7" Visible="False">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="PhoneNo" VisibleIndex="18" 
                            FieldName="PhoneNo" Width="150px">
                         <DataItemTemplate>
                                <dx:ASPxTextBox ID="ASPxTextBox4" runat="server" Text='<%# Eval("PhoneNo") %>' Width="100%">
                                </dx:ASPxTextBox>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Cleared Bal" FieldName="Cleared Bal" 
                            VisibleIndex="19" Visible="False">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Transaction Status" 
                            FieldName="Trans Status" VisibleIndex="21">
                        </dx:GridViewDataTextColumn>
                        
                        <dx:GridViewDataTextColumn Caption="Account Status" FieldName="Account Status" 
                            VisibleIndex="23">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Details" VisibleIndex="23">
                            <DataItemTemplate>
                                <a href="#">Details..</a>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <SettingsPager PageSize="50">
                    </SettingsPager>

<Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterBar="Visible" 
                        ShowGroupedColumns="True"></Settings>

<SettingsLoadingPanel ImagePosition="Top"></SettingsLoadingPanel>

                    <Images SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                        <LoadingPanelOnStatusBar Url="~/App_Themes/Aqua/GridView/gvLoadingOnStatusBar.gif">
                        </LoadingPanelOnStatusBar>
                        <LoadingPanel Url="~/App_Themes/Aqua/GridView/Loading.gif">
                        </LoadingPanel>
                    </Images>
                    <ImagesEditors>
                        <DropDownEditDropDown>
<SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" PressedCssClass="dxEditors_edtDropDownPressed_Aqua"></SpriteProperties>
                        </DropDownEditDropDown>
                        <SpinEditIncrement>
<SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditIncrementImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditIncrementImagePressed_Aqua"></SpriteProperties>
                        </SpinEditIncrement>
                        <SpinEditDecrement>
<SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditDecrementImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditDecrementImagePressed_Aqua"></SpriteProperties>
                        </SpinEditDecrement>
                        <SpinEditLargeIncrement>
<SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeIncImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditLargeIncImagePressed_Aqua"></SpriteProperties>
                        </SpinEditLargeIncrement>
                        <SpinEditLargeDecrement>
<SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeDecImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditLargeDecImagePressed_Aqua"></SpriteProperties>
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
                        

                </dx:aspxgridview>
            </td>
        </tr>
        <tr style="color: #9999cc">
            <td align="center" colspan="5" style="height: 23px">
                <asp:SqlDataSource ID="sqlproducts" runat="server" SelectCommand = "Sproc_GetAllProductsCards" SelectCommandType="StoredProcedure"  ConnectionString="<%$ ConnectionStrings:EazyConnect %>" ></asp:SqlDataSource>
                <asp:SqlDataSource ID="sqlBranches" runat="server" SelectCommand = "Sproc_GetAllBranchesCards" SelectCommandType="StoredProcedure"  ConnectionString="<%$ ConnectionStrings:EazyConnect %>" ></asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlacctDetails" runat="server" ></asp:SqlDataSource>
                <asp:SqlDataSource ID="sqlAppr" runat="server"  ></asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataSource5" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:EazyConnect %>" 
                    SelectCommand="SELECT * FROM [CardRequestType]"></asp:SqlDataSource>
                <asp:TextBox ID="txtUserID" runat="server" Wrap="False" Visible="False"></asp:TextBox>
                <asp:TextBox ID="txtref_no" runat="server" Visible="False"></asp:TextBox></td>
        </tr>
        <tr style="color: #9999cc">
            <td align="center" colspan="5" style="height: 23px">
                <asp:Button ID="Button1" runat="server" Text="Submit Request" />&nbsp;
                <asp:Button ID="Button3" runat="server" Text="Reset" Width="123px" />
                </td>
        </tr>
    </table>
</div>
    </form>
</body>
</html>