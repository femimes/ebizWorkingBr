<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NDDeMandate.aspx.cs" Inherits="NDD_NDDeMandate" %>

<%@ Register assembly="DevExpress.Web.v19.1, Version=19.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
 <head>
        
            <meta charset="utf-8" />
             <title>eBizPlus</title> 
            <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
            <meta content="Premium Multipurpose Admin & Dashboard Template" name="description" />
            <meta content="Themesbrand" name="author" />
    
            <link href="https://fonts.googleapis.com/css?family=Nunito:400,600,700&display=swap" rel="stylesheet"/>
            <link rel="stylesheet" href= "https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css"/>
    
            <!-- App favicon -->
            <link rel="shortcut icon" href="/Resources/images/favicon.ico"/>
    
            <link href="https://cdnjs.cloudflare.com/ajax/libs/animate.css/3.5.2/animate.min.css" rel="stylesheet"/>
    
            <!--Chartist Chart CSS -->
            <link href="../Resources/libs/chartist/chartist.min.css"  rel="stylesheet" type="text/css" /> 
            <!-- plugin css -->
            <link href="../Resources/libs/admin-resources/jquery.vectormap/jquery-jvectormap-1.2.2.css" rel="stylesheet" type="text/css" /> 
            <!-- DataTables -->
            <link href="../Resources/libs/datatables.net-bs4/css/dataTables.bootstrap4.min.css" rel="stylesheet" type="text/css" />
    
            <!-- Bootstrap Css -->
            <link href="../Resources/css/bootstrap.min.css" id="bootstrap-style" rel="stylesheet" type="text/css" />
            <!-- Icons Css -->
            <link href="../Resources/css/icons.min.css" rel="stylesheet" type="text/css" />
            <!-- App Css-->
            <link href="../Resources/css/app.min.css"   id="app-style" rel="stylesheet" type="text/css" />
            <link href="../Resources/css/customstyle.css" rel="stylesheet" type="text/css" />
            <link href="../Resources/css/style.css" rel="stylesheet" type="text/css" />
 
        <script type="text/javascript">

       
            function ctrlGrid_SelectionChanged(s, e) {


                s.GetSelectedFieldValues("SeqNo", GetSelectedFieldValuesCallback);

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

            
             function ASPxGridView1_EndCallback(s, e) {
            if (s.cpShowDeleteConfirmBox)
                pcConfirm.Show();
        }

        function Yes_Click() {
            pcConfirm.Hide();
            ASPxGridView1.DeleteRow(ASPxGridView1.cpRowIndex);  
             
            
        }

        function No_Click() {
            pcConfirm.Hide()
        }

   </script>

    <style type="text/css">
                .auto-style1 {
                    position: relative;
                    width: 100%;
                    -webkit-box-flex: 0;
                    -ms-flex: 0 0 41.66667%;
                    flex: 0 0 41.66667%;
                    max-width: 41.66667%;
                    left: -124px;
                    top: 0px;
                    padding-left: 12px;
                    padding-right: 12px;
                }
            </style>
        </head>
<body>
    <form id="form1" runat="server">
           <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
         
    <div class="container-fluid" style="margin-top:3%">
        <div class="form-group row">
            
                <div  >
                     <asp:CustomValidator ID="cus_val" runat="server" Display="Dynamic"></asp:CustomValidator>
                    &nbsp;</div>
             
           
            <div class="col-sm-10">
              <%--<div class="formwidth">--%>


                    <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" ClientInstanceName="ASPxGridView1"
            DataSourceID="sqlCorpProfile" KeyFieldName="SeqNo" 
            Caption="Setup Customer eMandate Details (NDD)" 
            CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"
                        oncelleditorinitialize="ASPxGridView1_CellEditorInitialize" 
                        oncustomerrortext="ASPxGridView1_CustomErrorText" 
                        onrowinserting="ASPxGridView1_RowInserting" 
                        onrowupdating="ASPxGridView1_RowUpdating"
                        OnRowDeleting ="ASPxGridView1_RowDeleting"
                         
                        OnCustomButtonCallback="ASPxGridView1_CustomButtonCallback"
                        OnInit="ASPxGridView1_Init"
                         SummaryText="T"  width="100%"
                        Styles-Header-BackColor="#002668" Styles-Header-Font-Size="X-Small" 
                         EditFormLayoutProperties-ColumnCount="1"
                          >      
                     <%--   <ClientSideEvents CustomButtonClick="function(s, e) {  
                            if(e.buttonID == 'btdCancel'){  
                                            var rowVisibleIndex = e.visibleIndex;  
                                            var rowKeyValue = s.GetRowKey(rowVisibleIndex);  
                                            alert('Row key value: ' + rowKeyValue);  
                                    }  
                                }" />  --%>
            <Columns>

                <dx:GridViewCommandColumn VisibleIndex="0"  ShowUpdateButton="true" ShowEditButton="true"       ShowNewButtonInHeader="true" ShowInCustomizationForm="true" ShowRecoverButton="true">
               <CustomButtons>  
            <dx:GridViewCommandColumnCustomButton ID="btDelete" Text="Delete"  
                Visibility="AllDataRows"  />  
                      <dx:GridViewCommandColumnCustomButton ID="btRemove" Text="Remove"   
                Visibility="AllDataRows" />        
             </CustomButtons> 
                    
                </dx:GridViewCommandColumn>
               
                <dx:GridViewDataTextColumn FieldName="accountNumber" VisibleIndex="5" Caption="Customer AccountNo">
                     
                      <EditItemTemplate>
                          <dx:ASPxTextBox ID="txtcusAccountNo" runat="server" NullText="Enter Biller Account No" Width="40%" ValidationSettings-Display="Dynamic" 
                              Text='<%# Eval("accountNumber") %>' AutoPostBack="true" ToolTip ='<%# Eval("SeqNo") %>' OnTextChanged="txtAccountNo_TextChanged1"
                              ValidationSettings-RequiredField-IsRequired="true" 
                        ValidationSettings-ErrorDisplayMode="ImageWithTooltip" ValidationSettings-ErrorText="Customer accountNo required"
                              ></dx:ASPxTextBox>
                     
                                
                      </EditItemTemplate>
                   </dx:GridViewDataTextColumn>
                  <dx:GridViewDataTextColumn FieldName="accountName"  VisibleIndex="5" Caption="Customer AccountNames" >
            
                        
                      <EditItemTemplate>
                    <dx:ASPxLabel ID="lblAcctName" runat="server" 
                            Text='<%# Eval("accountName") %>' ToolTip ='<%# Eval("SeqNo") %>'></dx:ASPxLabel>
                         </EditItemTemplate>

                   </dx:GridViewDataTextColumn>
                  <dx:GridViewDataTextColumn FieldName="payerName" VisibleIndex="2" Caption="Payer Name">
            
                       <PropertiesTextEdit NullText="Enter Payer Name or Title"  ValidationSettings-Display="Dynamic"  
                           ValidationSettings-RequiredField-IsRequired="true" 
                        ValidationSettings-ErrorDisplayMode="ImageWithTooltip" ValidationSettings-ErrorText="Payer Name required"  Width="40%"  >
                             </PropertiesTextEdit>

                                               </dx:GridViewDataTextColumn>
          
              <dx:GridViewDataTextColumn FieldName="payerPhoneNumber" VisibleIndex="3" Caption="Phone">
            
                       <PropertiesTextEdit NullText="Enter Phone No." ValidationSettings-Display="Dynamic"   ValidationSettings-RequiredField-IsRequired="true"  
                        ValidationSettings-ErrorDisplayMode="ImageWithTooltip" ValidationSettings-ErrorText="Contact Phone required"  Width="40%"  > </PropertiesTextEdit>

                  </dx:GridViewDataTextColumn>
                  <dx:GridViewDataTextColumn FieldName="payerEmail" VisibleIndex="4" Caption="Email">
            
                       <PropertiesTextEdit NullText="Enter Email"  ValidationSettings-Display="Dynamic" 
                            
                           ValidationSettings-ErrorText="Invalid e-mail." ValidationSettings-ErrorDisplayMode="ImageWithTooltip"   Width="40%" 
                           ValidationSettings-RequiredField-IsRequired="true"  > </PropertiesTextEdit>

                                               </dx:GridViewDataTextColumn>
                  <dx:GridViewDataMemoColumn FieldName="payerAddress" VisibleIndex="5" Caption="Address">
            
                       <PropertiesMemoEdit NullText="Payer Address"  ValidationSettings-Display="Dynamic" ValidationSettings-RequiredField-IsRequired="true" 
                        ValidationSettings-ErrorDisplayMode="ImageWithTooltip" ValidationSettings-ErrorText="Address required"  Width="40%"  > </PropertiesMemoEdit>

                                               </dx:GridViewDataMemoColumn>
                    <dx:GridViewDataMemoColumn FieldName="narration" VisibleIndex="9" Caption="Narration">
            
                       <PropertiesMemoEdit NullText="Narration"  ValidationSettings-Display="Dynamic" ValidationSettings-RequiredField-IsRequired="true" 
                        ValidationSettings-ErrorDisplayMode="ImageWithTooltip" ValidationSettings-ErrorText="Narration required"  Width="40%"  > </PropertiesMemoEdit>

                                               </dx:GridViewDataMemoColumn>
                  <dx:GridViewDataComboBoxColumn FieldName="bank_name" VisibleIndex="6" Caption="Bank"  >
            
                    
                      <EditItemTemplate>

                          <dx:ASPxComboBox  ID="drpBanks" runat="server"  NullText="Select Bank"  ValidationSettings-Display="Dynamic" ValidationSettings-RequiredField-IsRequired="true"  DataSourceID="SqlDataBanks"
                        ValidationSettings-ErrorDisplayMode="ImageWithTooltip"   ValidationSettings-ErrorText="Bank name required"  Width="40%" ValueField="bank_code"  TextField="bank_name" 
                              
                              AutoPostBack="true" ToolTip ='<%# Eval("SeqNo") %>'  OnTextChanged="drpBanks_TextChanged">


                          </dx:ASPxComboBox>

                      </EditItemTemplate>
                   </dx:GridViewDataComboBoxColumn>

                  <dx:GridViewDataTextColumn FieldName="payerAccountNumber" VisibleIndex="7" Caption="AccountNo">
                     
                      <EditItemTemplate>
                          <dx:ASPxTextBox ID="txtAccountNo" runat="server" NullText="Enter Payer Account No"  Width="40%"  ValidationSettings-Display="Dynamic" 
                              Text='<%# Eval("payerAccountNumber") %>' AutoPostBack="true" ToolTip ='<%# Eval("SeqNo") %>' OnTextChanged="txtAccountNo_TextChanged"
                              ValidationSettings-RequiredField-IsRequired="true" 
                        ValidationSettings-ErrorDisplayMode="ImageWithTooltip" ValidationSettings-ErrorText="Payer accountNo required"
                              ></dx:ASPxTextBox>
                     
                                
                      </EditItemTemplate>
                   </dx:GridViewDataTextColumn>
                  <dx:GridViewDataTextColumn FieldName="payerAccountName"  VisibleIndex="8" Caption="Account Name" >
            
                        
                      <EditItemTemplate>
                    <dx:ASPxLabel ID="lblAcctName" runat="server" 
                            Text='<%# Eval("payerAccountName") %>' ToolTip ='<%# Eval("SeqNo") %>' ForeColor="Green"></dx:ASPxLabel>
                         </EditItemTemplate>

                   </dx:GridViewDataTextColumn>
                  <dx:GridViewDataTextColumn FieldName="amount" VisibleIndex="10" Caption="Amount">
                     
                      <EditItemTemplate >
                        <%--  <dx:ASPxTextBox ID="txtamount" runat="server"  Width="100%" ValidationSettings-Display="Dynamic" 
                              Text='<%# Eval("payerAccountNumber") %>'   ToolTip ='<%# Eval("SeqNo") %>'  
                              ValidationSettings-RequiredField-IsRequired="true"  
                        ValidationSettings-ErrorDisplayMode="ImageWithTooltip" ValidationSettings-ErrorText="amount required"
                           DisplayFormatString="{0:n2}" 
                              >
                                <MaskSettings Mask="<-9999999999..9999999999g>.<00..99>" IncludeLiterals="DecimalSymbol" /> 
                                 <MaskSettings Mask="$<0..99999g>.<00..99>" /> 
                          </dx:ASPxTextBox>--%>
                  <dx:ASPxSpinEdit ID="txtamount" runat="server"  ValidationSettings-Display="Dynamic" 
                        DisplayFormatString="=N={0:n2}" AllowMouseWheel="false" SpinButtons-Enabled="false" SpinButtons-ShowIncrementButtons="false"
                      NullText="amount required"  ValidationSettings-ErrorDisplayMode="ImageWithTooltip" ValidationSettings-ErrorText="amount required"
                      Number="0" NumberType="Float"    
                      >
                       
                  </dx:ASPxSpinEdit>
                                
                      </EditItemTemplate>
                   </dx:GridViewDataTextColumn>
                 
                 <dx:GridViewDataTextColumn FieldName="frequency" VisibleIndex="13" Caption="Frequency">
                     
                      <EditItemTemplate >
                        <%--  <dx:ASPxTextBox ID="txtamount" runat="server"  Width="100%" ValidationSettings-Display="Dynamic" 
                              Text='<%# Eval("payerAccountNumber") %>'   ToolTip ='<%# Eval("SeqNo") %>'  
                              ValidationSettings-RequiredField-IsRequired="true"  
                        ValidationSettings-ErrorDisplayMode="ImageWithTooltip" ValidationSettings-ErrorText="amount required"
                           DisplayFormatString="{0:n2}" 
                              >
                                <MaskSettings Mask="<-9999999999..9999999999g>.<00..99>" IncludeLiterals="DecimalSymbol" /> 
                                 <MaskSettings Mask="$<0..99999g>.<00..99>" /> 
                          </dx:ASPxTextBox>--%>
                  <dx:ASPxSpinEdit ID="txtFrequency" runat="server"  ValidationSettings-Display="Dynamic" 
                        DisplayFormatString="{0:n}" AllowMouseWheel="false" SpinButtons-Enabled="false" SpinButtons-ShowIncrementButtons="false"
                      NullText="Frequency required"  ValidationSettings-ErrorDisplayMode="ImageWithTooltip" ValidationSettings-ErrorText="frequency required"
                     Width="40%" Number="1" AutoPostBack="true" OnNumberChanged="txtFrequency_NumberChanged" >
                       
                  </dx:ASPxSpinEdit>
                                
                      </EditItemTemplate>
                   </dx:GridViewDataTextColumn>
             
                 <dx:GridViewDataDateColumn FieldName="startDate" VisibleIndex="11" Caption="Start Date" >
            
                      

                     <EditItemTemplate>
                         <dx:ASPxDateEdit ID="startDate" 
                    runat="server"  AutoPostBack="true" ToolTip ='<%# Eval("SeqNo") %>'  EditFormat="Date"    DisplayFormatInEditMode="true" 
                              ValidationSettings-RequiredField-IsRequired="true" 
                        ValidationSettings-ErrorDisplayMode="ImageWithTooltip" ValidationSettings-ErrorText="start date required"
                             DisplayFormatString="dd-MMM-yyyy"   OnDateChanged="dateIssue_DateChanged"
                             ></dx:ASPxDateEdit>

                     </EditItemTemplate>

                  </dx:GridViewDataDateColumn>
                <dx:GridViewDataDateColumn FieldName="endDate" VisibleIndex="11" Caption="End Date" >
               

                        <EditItemTemplate>
                         <dx:ASPxDateEdit ID="endDate" 
                    runat="server"    ToolTip ='<%# Eval("SeqNo") %>'  EditFormat="Date"    DisplayFormatInEditMode="true" 
                              ValidationSettings-RequiredField-IsRequired="true" 
                        ValidationSettings-ErrorDisplayMode="ImageWithTooltip" ValidationSettings-ErrorText="end date required"
                             DisplayFormatString="dd-MMM-yyyy" ReadOnly="true">
                             </dx:ASPxDateEdit>

                     </EditItemTemplate>
                    

                </dx:GridViewDataDateColumn>

         <dx:GridViewDataCheckColumn FieldName="mandateTypeCon" VisibleIndex="13" Caption="Fixed Mandate?"  >
                    
             <PropertiesCheckEdit    ValueUnchecked="false">

             </PropertiesCheckEdit>
                </dx:GridViewDataCheckColumn>

         <dx:GridViewDataComboBoxColumn FieldName="billerId" VisibleIndex="1" Caption="Customer" >
              
                    <EditItemTemplate>
                    <dx:ASPxComboBox id="drpbillerId" runat="server" NullText ="Select Customer" DataSourceID="SqlDataBillerId"   TextField="name"  
                        ValueField="billerId" TextFormatString="{0};{1}"    Width="40%">

                    <Columns>
                    <dx:ListBoxColumn FieldName="name" Width="130px" Caption="Name" />
                    <dx:ListBoxColumn FieldName="productName" Width="150px" Caption="Title" />
                    <dx:ListBoxColumn FieldName="rcNumber" Width="70px" />
                    <dx:ListBoxColumn FieldName="email" Width="100px" />
                 
                </Columns>
                        </dx:ASPxComboBox>

                    </EditItemTemplate>

                </dx:GridViewDataComboBoxColumn>

                   <dx:GridViewDataTextColumn FieldName="productId" VisibleIndex="15" Caption="product Id" EditFormSettings-Visible="False" Visible="false">
                    

                </dx:GridViewDataTextColumn>
   <dx:GridViewDataTextColumn FieldName="subscriberCode" VisibleIndex="15" Caption="subscriberCode" EditFormSettings-Visible="False" Visible="false">
                    

                </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="statusDesc" VisibleIndex="15" Caption="Status" EditFormSettings-Visible="False">
                    

                </dx:GridViewDataTextColumn>
            </Columns>
           <TotalSummary>
                                                <dx:ASPxSummaryItem ShowInColumn="SeqNo" ShowInGroupFooterColumn="SeqNo" SummaryType="Count"/>
                                               
                                           </TotalSummary>
                                         

                                           <SettingsPager AlwaysShowPager="True"  >
                                           </SettingsPager>
                                         
                             <Settings  VerticalScrollBarMode="Visible" VerticalScrollBarStyle="VirtualSmooth" VerticalScrollableHeight="350"  ShowFilterBar="Visible"/>
                        <SettingsEditing mode="PopupEditForm">
                            
                        </SettingsEditing>
                        <SettingsBehavior  EnableRowHotTrack="true"  /> 
                                           <SettingsLoadingPanel Text="" />
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
            <ClientSideEvents EndCallback="ASPxGridView1_EndCallback" />
        </dx:ASPxGridView>
                 <dx:ASPxPopupControl ID="pcConfirm" runat="server" ClientInstanceName="pcConfirm"
            Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"
            HeaderText="Row Deleting">
            <ContentCollection>
                <dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
                    <table width="100%">
                        <tr>
                            <td colspan="2" align="center">
                                Delete Row?
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <a href="javascript:Yes_Click()">Yes</a>
                            </td>
                            <td align="center">
                                <a href="javascript:No_Click()">No</a>
                            </td>
                        </tr>
                    </table>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
                 <%--</div>--%>
     
                    <asp:SqlDataSource ID="sqlCorpProfile" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:EazyConnect %>" 
                        SelectCommand="SpNDDeMandateGetAll" 
            SelectCommandType="StoredProcedure"  >
                        
                         
                    </asp:SqlDataSource>

                 <asp:SqlDataSource ID="SqlDataBanks" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:EazyConnect %>" 
                        SelectCommand="SpmNNDGetBankAndSchemeCode" 
            SelectCommandType="StoredProcedure"  >
                        
                         
                    </asp:SqlDataSource>

                  <asp:SqlDataSource ID="SqlDataBillerId" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:EazyConnect %>" 
                         SelectCommand="SpNDDBillerGetAppdInfo" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:Parameter DefaultValue="3" Name="status" Type="Int32" />
            
            </SelectParameters>
                        
                         
                    </asp:SqlDataSource>
     </div>
             
           
        </div>


    </div>
    
 
   
    </form>
</body>
</html>
