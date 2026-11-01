<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CardAfroPayConfirm.aspx.cs" Inherits="Request_CardAfroPayConfirm" %>
<%@ Register Assembly="DevExpress.Web.v19.1, Version=19.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
        
            <link href="https://fonts.googleapis.com/css?family=Nunito:400,600,700&display=swap" rel="stylesheet"/>
            <link rel="stylesheet" href= "https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css"/>
    
           
    
            <link href="https://cdnjs.cloudflare.com/ajax/libs/animate.css/3.5.2/animate.min.css" rel="stylesheet"/>
    
</head>
<body>
    <form id="form1" runat="server">
        <div id="layout-wrapper" style="margin-top:2%;margin-left:2%;">
        <div class="container-fluid" >
          <div class="row">
        
              <div class="form-group row">
            Activate AfriGOPAY Cards</div>
              <div class="form-group row">&nbsp;</div>
                   
          <div><asp:Literal ID="Literal1" runat="server"></asp:Literal></div>
        <div class="form-group row">
               <dx:ASPxComboBox ID="drpBatchRefNo" runat="server" EnableIncrementalFiltering="True" EnableSynchronization="False" IncrementalFilteringMode="StartsWith" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"    ShowShadow="False" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" DataSourceID="SqlDataSource2" ValueField="RefNo" AutoPostBack="True">
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

        </div>
        <div class="form-group row">
         <dx:ASPxGridView ID="ctrlGrid" runat="server" AutoGenerateColumns="False" 
                        ClientIDMode="AutoID" DataSourceID="SqlDataSource1"
                        KeyFieldName="SeqNum" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" Width="100%"
                        >
                        <SettingsPager PageSize="100">
                        </SettingsPager>
                            <Columns>
                                   <dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width ="30px" FixedStyle="Left"  ></dx:GridViewCommandColumn>
                                
                                
                                 <dx:GridViewDataTextColumn Caption="Batch RefNo" FieldName="RefNo" VisibleIndex="1">
                                    
                                 </dx:GridViewDataTextColumn>
                                     <dx:GridViewDataTextColumn Caption="SeqNum" FieldName="SeqNum" VisibleIndex="17" Visible="false">
                                    
                                 </dx:GridViewDataTextColumn>
                                 <dx:GridViewDataTextColumn Caption="Card DisplayName" FieldName="AccountName" VisibleIndex="1">
                                     <HeaderStyle Wrap="False" />
                                 </dx:GridViewDataTextColumn>
                                 <dx:GridViewDataTextColumn Caption="Account No" FieldName="AccountNo" VisibleIndex="1">
                                     <HeaderStyle Wrap="False" />
                                 </dx:GridViewDataTextColumn>
                                 <dx:GridViewDataTextColumn Caption="Address" FieldName="Address" VisibleIndex="4">
                                    
                                 </dx:GridViewDataTextColumn>
                                 <dx:GridViewDataTextColumn Caption="Product" FieldName="Product" VisibleIndex="3">
                                 </dx:GridViewDataTextColumn>
                                 <dx:GridViewDataTextColumn Caption="PanNo" FieldName="PANno"   VisibleIndex="2">
                                 </dx:GridViewDataTextColumn>
                                 <dx:GridViewDataDateColumn Caption="Card ExpiryDate" FieldName="CardExpiryDate" VisibleIndex="17">
                                     <PropertiesDateEdit DisplayFormatString="D">
                                     </PropertiesDateEdit>
                                     <DataItemTemplate>
                                         <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text='<%# Eval("CardExpiryDate") %>'>
                                         </dx:ASPxLabel>
                                     
                                     </DataItemTemplate>
                                 </dx:GridViewDataDateColumn>
                                 <dx:GridViewDataDateColumn Caption="Card IssueDate" FieldName="CardIssueDate" VisibleIndex="16">
                                     <PropertiesDateEdit DisplayFormatString="D">
                                     </PropertiesDateEdit>
                                      <DataItemTemplate>
                                         <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text='<%# Eval("CardIssueDate") %>'>
                                         </dx:ASPxLabel>
                                     
                                     </DataItemTemplate>
                                 </dx:GridViewDataDateColumn>
                                 <dx:GridViewDataTextColumn Caption="Issue Counter" FieldName="CardIssueCounter" VisibleIndex="22">
                                 </dx:GridViewDataTextColumn>

                                 <dx:GridViewDataTextColumn Caption="TrackID" FieldName="TrackID" VisibleIndex="29" Visible="false">
                                 </dx:GridViewDataTextColumn>
                                 <dx:GridViewDataDateColumn   VisibleIndex="2" FieldName="CardIssueDate" Caption="Card IssueDate"  >
                                    <PropertiesDateEdit DisplayFormatString="D">
                                    </PropertiesDateEdit>
                                </dx:GridViewDataDateColumn>
                                  <dx:GridViewDataDateColumn   VisibleIndex="3" FieldName="CardExpiryDate" Caption="Card Expiry"   >
                                    <PropertiesDateEdit DisplayFormatString="D">
                                    </PropertiesDateEdit>
                                </dx:GridViewDataDateColumn>
                                
                                <dx:GridViewDataTextColumn FieldName="AddedBy"   Caption= "Generated By" VisibleIndex="4">
                                </dx:GridViewDataTextColumn>
                                
                               
  
                                <dx:GridViewDataTextColumn VisibleIndex="14">
                                    <DataItemTemplate>
                                    <table style="width:100%;">
                                        <tr>
                                            <td>
                                                &nbsp;</td>
                                            <td> 
                                            </td>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                    </table>
                                    </DataItemTemplate>
                                </dx:GridViewDataTextColumn>
                        </Columns>
                                        <GroupSummary>
                                                <dx:ASPxSummaryItem DisplayFormat="Total ={0}"  
                                                    SummaryType="Count" />
                                            </GroupSummary>

<SettingsBehavior AllowFocusedRow="True" AutoExpandAllGroups="True" EnableRowHotTrack="True"></SettingsBehavior>

<Settings ShowGroupPanel="True" ShowFilterBar="Visible" ShowFooter="True"></Settings>
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

        </div>
     </div>

            <div class="form-group row" >
                                     <p style="align-items:center">
                                     
            
                                         <asp:Button ID="Button5" runat="server"  
                                    Text="Activate Selection" Height="30px" OnClick="Button5_Click" Width="227px"       /> &nbsp;
             <asp:Button ID="Button3" runat="server"  
                                    Text="Remove Selection" Height="30px" OnClick="Button3_Click" Width="166px"       /> &nbsp;
                                         <asp:Button ID="Button4" runat="server" CausesValidation="False"   Text="Reset"  CssClass="btn btn-primary ml-2" Height="30px" OnClick="Button4_Click" Width="166px" /> 
                                          </p>          

            </div>
                                                  
            </div>
            <asp:SqlDataSource ID="SqlDataSource4" runat="server" 
        ConnectionString="<%$ ConnectionStrings:EazyConnect %>" 
        SelectCommand="SprAfroGGetCardForExtactInfo" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:Parameter DefaultValue="3" Name="status" Type="Int32" />
        
        </SelectParameters>
    </asp:SqlDataSource>

            <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
        ConnectionString="<%$ ConnectionStrings:EazyConnect %>" 
        SelectCommand="SprAfroGGetCardForExtactInfo" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:Parameter DefaultValue="5" Name="status" Type="Int32" />
            </SelectParameters>
    </asp:SqlDataSource><asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:EazyConnect %>" 
        SelectCommand="Sproc_GetAllCardAccountsByRefNo" SelectCommandType="StoredProcedure">
        <SelectParameters>
 
               <asp:ControlParameter DefaultValue="0" ControlID="drpBatchRefNo"  Name="RefNo" />
        </SelectParameters>
    </asp:SqlDataSource>



            </div>
    </form>
</body>
</html>