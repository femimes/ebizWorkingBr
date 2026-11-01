<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmBulkEmails.aspx.cs" Inherits="eOperations_FrmBulkEmails" Async="true" %>
<%@ Register Assembly="DevExpress.Web.v19.1, Version=19.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../Styles/SiteAll.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
       
    <script type="text/javascript">

        function ctrlGrid_SelectionChanged(s, e) {


            s.GetSelectedFieldValues("AccountNo", GetSelectedFieldValuesCallback);

        }

        function OnAllCheckedChanged(s, e) {


            if (s.GetChecked())
                ASPxGridView1.SelectRows();
            else
                ASPxGridView1.UnselectRows();

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
            document.getElementById("selCount").innerHTML = ASPxGridView1.GetSelectedRowCount();
            //           selList.Visible = false;
        }



    </script>



     
    <div>
     
         <script type="text/javascript">

             function ctrlGrid_SelectionChanged(s, e) {


                 s.GetSelectedFieldValues("AccountNo", GetSelectedFieldValuesCallback);

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
                 document.getElementById("selCount").innerHTML = ASPxGridView1.GetSelectedRowCount();
                 //           selList.Visible = false;
             }



   </script>           
                    
                    
    <table id="TABLE1" border="0" cellpadding="3" cellspacing="2" class="photo-frame"
        style="border-right:   thin outset; border-top: thin outset; font-size: 7pt; border-left: thin outset;
        width: 730px; color:DarkBlue; border-bottom: thin outset; font-family: Verdana,  'Microsoft Sans Serif';
        height: 1px">
        <tr style="color: #9999cc">
            <td align="center" colspan="6" style="height: 29px; text-align: left">
                <strong>ACCOUNT STATEMENT NOTIFICATION (E-Mail) </strong>&nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; 
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp;
                &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;</td>
        </tr>
        <tr style="color: #9999cc">
            <td align="center" colspan="6" style="height: 29px; text-align: left">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
            </td>
        </tr>
        <tr style="color: #9999cc">
            <td align="center" colspan="1" rowspan="9" valign="top" 
                style="white-space:nowrap">
             <div style="overflow:scroll;height:auto">
              <div class="TopPadding">
                                    Selected count: <span id="selCount" style="font-weight: bold">0</span>
                                </div>
                              
                      <dx:ASPxListBox ID="ASPxListBox1" ClientInstanceName="selList" runat="server" Height="250px"
                                     ReadOnly ="True" 
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
                                <asp:Button runat ="server" Text ="clear" ID="cmdClear" 
                    CausesValidation="False" />
                    </div>
            </td>
            <td align="left" colspan="5">
                <asp:CheckBox ID="CheckBox1" runat="server" 
                    Text="Send to Non-Active Transactions Accounts?" Visible="False" />
                <asp:TextBox ID="seqno" runat="server"
                    Visible="False">0</asp:TextBox></td>
        </tr>
        <tr style="color: #9999cc">
            <td align="right" nowrap="nowrap" >
                Trans StartDate</td>
            <td align="center" colspan="1" style="height: 30px; text-align: left; width: 289px;">
                &nbsp;<dx:ASPxDateEdit ID="dateIssue" 
                    runat="server"   DisplayFormatString="D"
                    EditFormat="Custom" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" 
                    CssPostfix="Aqua" ShowShadow="False" 
                    SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" 
                    NullText="Select Start Date" AutoPostBack="True" OnDateChanged="dateIssue_DateChanged">
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
                        <RequiredField ErrorText="Start Date is required" IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxDateEdit>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="dateIssue"
                    Display="Dynamic" ErrorMessage="Start date required" SetFocusOnError="True">*</asp:RequiredFieldValidator>
            </td>
            <td align="right" nowrap >
                Trans EndDate</td>
            <td align="left" colspan="1" nowrap>
                &nbsp;<dx:ASPxDateEdit ID="dateexpiry" runat="server"  DisplayFormatString="D"
                    EditFormat="Custom" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" ShowShadow="False" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" NullText="Select End Date" >
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
                        <RequiredField ErrorText="End Date is required" IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxDateEdit>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="dateexpiry"
                    Display="Dynamic" ErrorMessage="End date required" SetFocusOnError="True">*</asp:RequiredFieldValidator></td>
            <td align="center" colspan="1" rowspan="6" style="text-align: left">
                &nbsp;</td>
        </tr>
        <tr style="color: #9999cc">
            <td align="right" >
                Statement Type</td>
            <td align="center" colspan="1" style="height: 30px; text-align: left; width: 289px;">
                <dx:ASPxComboBox ID="drpStatementtype" runat="server" 
                    CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" 
                       ShowShadow="False" 
                    SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" AutoPostBack="True" OnSelectedIndexChanged="drpStatementtype_SelectedIndexChanged">
                    <Items>
                        <dx:ListEditItem Text="Monthly" Value="M" />
                        <dx:ListEditItem Text="Yearly" Value="Y" />
                        <dx:ListEditItem Text="Quartely" Value="Q" />
                        <dx:ListEditItem Text="Weekly" Value="W" />
                        <dx:ListEditItem Text="Daily" Value="D" />
                        <dx:ListEditItem Text="NonPeriodic" Value="N" />
                    </Items>
                    <LoadingPanelImage Url="~/App_Themes/Aqua/Editors/Loading.gif">
                    </LoadingPanelImage>
                    <DropDownButton>
                        <Image>
                            <SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" 
                                PressedCssClass="dxEditors_edtDropDownPressed_Aqua" />
                        </Image>
                    </DropDownButton>
                    <ValidationSettings>
                        <ErrorFrameStyle ImageSpacing="4px">
                            <ErrorTextPaddings PaddingLeft="4px" />
                        </ErrorFrameStyle>
                    </ValidationSettings>
                </dx:ASPxComboBox>
            </td>
            <td align="right" >
                &nbsp;</td>
            <td align="left" >
                
                &nbsp;</td>
        </tr>
        <tr style="color: #9999cc">
            <td align="right" >
                Produts</td>
            <td align="center" colspan="1" style="height: 30px; text-align: left; width: 289px;">
                <dx:ASPxComboBox ID="drp_products" EnableIncrementalFiltering="True" EnableSynchronization="False" IncrementalFilteringMode="StartsWith"  runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"    ShowShadow="False" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" CssClass="TableRuling" DataSourceID="sqlproducts" TextField="product" ValueField="Product Code" AutoPostBack="True" OnSelectedIndexChanged="drp_products_SelectedIndexChanged">
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
                </dx:ASPxComboBox>
            </td>
            <td align="right" >
                Branch</td>
            <td align="left" >
                
                <table border="0" cellpadding="0" cellspacing="0" style="border-top-style: none;
                    border-right-style: none; border-left-style: none; border-bottom-style: none">
                    <tr>
                        <td align="left">
                <dx:ASPxComboBox ID="drpbranches"  EnableIncrementalFiltering="True" EnableSynchronization="False" IncrementalFilteringMode="StartsWith" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"    ShowShadow="False" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" DataSourceID="sqlBranches" ValueField="Headsub_code" AutoPostBack="True" OnSelectedIndexChanged="drpbranches_SelectedIndexChanged">
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
                </dx:ASPxComboBox>
                        </td>
                        <td align="left">
                            <asp:Button ID="Button4" runat="server" Text="go" OnClick="Button4_Click" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr style="color: #9999cc">
            <td align="right">
                Specific AccountList</td>
            <td align="left" colspan="3">
                <dx:ASPxMemo ID="txtSpecificAcct" runat="server" AutoPostBack="True" Height="71px"
                    Width="376px" OnTextChanged="txtSpecificAcct_TextChanged">
                </dx:ASPxMemo>
            </td>
        </tr>
        <tr style="color: #9999cc">
            <td align="center" colspan="4" style="text-align: center">
                <asp:Button ID="Button6" runat="server" Text="Send Statements" Width="150px" OnClick="Button6_Click" />
&nbsp;<asp:Button ID="Button7" runat="server" Text="Reset" Width="150px" OnClick="Button7_Click"  />
            </td>
        </tr>
        <tr style="color: #9999cc">
            <td align="center" colspan="4" style="height: 117px; text-align: left">
                &nbsp;<dx:ASPxCheckBox   ID="chkAllRecords" runat="server" 
                    Text="Select All Pages?"   ClientSideEvents-CheckedChanged="OnAllCheckedChanged"   >
                </dx:ASPxCheckBox>
                <dx:ASPxGridView ID="ASPxGridView1" ClientInstanceName="ASPxGridView1"
                runat="server" DataSourceID="SqlacctDetails" AutoGenerateColumns="False" 
                    KeyFieldName="AccountNo" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" 
                    CssPostfix="Aqua" Width="100%" OnBeforePerformDataSelect="ASPxGridView1_BeforePerformDataSelect">
                   
                    <Columns>
                       <dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width ="30px" FixedStyle="Left"  >
                                    <HeaderTemplate>
                                     <dx:ASPxCheckBox ID="SelectAllCheckBox" runat="server" ToolTip="Select/Unselect all rows on the page"
                                            ClientSideEvents-CheckedChanged="function(s, e) { ASPxGridView1.SelectAllRowsOnPage(s.GetChecked()); }" />
                                    </HeaderTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </dx:GridViewCommandColumn>
                        <dx:GridViewDataTextColumn Caption="Account No" FieldName="AccountNo" VisibleIndex="0">
                           
                            <HeaderStyle Wrap="False" />
                            <CellStyle Wrap="False">
                            </CellStyle>
                             
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Date of Birth" FieldName="DOB" VisibleIndex="3">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Account Name" FieldName="AccountName" VisibleIndex="1">
                            <HeaderStyle Wrap="False" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Address" FieldName="address" VisibleIndex="4">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Product" FieldName="Product" VisibleIndex="2">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="SeqNo" FieldName="seq" Visible="False" VisibleIndex="18">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Issue Counter" FieldName="CardIssueCounter" VisibleIndex="23">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="City" FieldName="city" VisibleIndex="5">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Book Bal" FieldName="Book Bal" VisibleIndex="19">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Cleared Bal" FieldName="Cleared Bal" VisibleIndex="21">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Transaction Status" FieldName="Trans Status"
                            VisibleIndex="22">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Account Status" FieldName="Account Status" VisibleIndex="23">
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <Settings ShowFilterBar="Visible" ShowGroupedColumns="True" ShowGroupPanel="True"
                        ShowStatusBar="Visible" ShowVerticalScrollBar="True" 
                        VerticalScrollBarStyle="Virtual" />
                    <SettingsPager PageSize="80" Position="TopAndBottom" SEOFriendly="CrawlerOnly">
                    </SettingsPager>
                    <Styles CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua">
                        <LoadingPanel ImageSpacing="8px">
                        </LoadingPanel>
                    </Styles>
                    <SettingsLoadingPanel ImagePosition="Top" />
                    <ImagesFilterControl>
                        <LoadingPanel Url="~/App_Themes/Aqua/Editors/Loading.gif">
                        </LoadingPanel>
                    </ImagesFilterControl>
                    <Images SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                        <LoadingPanelOnStatusBar Url="~/App_Themes/Aqua/GridView/gvLoadingOnStatusBar.gif">
                        </LoadingPanelOnStatusBar>
                        <LoadingPanel Url="~/App_Themes/Aqua/GridView/Loading.gif">
                        </LoadingPanel>
                    </Images>
                    <StylesEditors>
                        <CalendarHeader Spacing="1px">
                        </CalendarHeader>
                        <ProgressBar Height="25px">
                        </ProgressBar>
                    </StylesEditors>
                    <ImagesEditors>
                        <DropDownEditDropDown>
                            <SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" PressedCssClass="dxEditors_edtDropDownPressed_Aqua" />
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
                        
<ClientSideEvents SelectionChanged="ctrlGrid_SelectionChanged"  />
                </dx:ASPxGridView>
            </td>
        </tr>
        <tr style="color: #9999cc">
            <td align="center" colspan="5" style="height: 23px">
                <asp:SqlDataSource ID="sqlproducts" runat="server" SelectCommand = "Sproc_GetAllProductsCards" SelectCommandType="StoredProcedure"  ConnectionString="<%$ ConnectionStrings:EazyConnect %>" ></asp:SqlDataSource>
                <asp:SqlDataSource ID="sqlBranches" runat="server" SelectCommand = "Sproc_GetAllBranchesCards" SelectCommandType="StoredProcedure"  ConnectionString="<%$ ConnectionStrings:EazyConnect %>" ></asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlacctDetails" runat="server" ></asp:SqlDataSource>
                <dx:ASPxCheckBox ID="ASPxCheckBox1" runat="server" Visible="False">
                </dx:ASPxCheckBox>
                <asp:SqlDataSource ID="sqlAppr" runat="server"  ></asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataSource5" runat="server"></asp:SqlDataSource>
                <asp:TextBox ID="txtUserID" runat="server" Wrap="False" Visible="False"></asp:TextBox>
                <asp:TextBox ID="txtref_no" runat="server" Visible="False"></asp:TextBox></td>
        </tr>
        <tr style="color: #9999cc">
            <td align="center" colspan="5" style="height: 23px">
            </td>
        </tr>
        <tr style="color: #9999cc">
            <td  colspan="6" style="height: 23px">
               <%-- <asp:Button ID="Button5" runat="server" Text="Send Statement dfdf" />
                <asp:Button ID="Button1" runat="server" Text="Save Statement" Visible="False" />&nbsp;
                <asp:Button ID="Button3" runat="server" Text="Reset" Width="123px" />--%>
            </td>
        </tr>
    </table>
    </div>
</div>
    </form>
</body>
</html>
