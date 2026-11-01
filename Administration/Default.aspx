<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Administration_Default" %>

<%@ Register assembly="DevExpress.Web.v19.1, Version=19.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
 <head>
         
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
        <div class="container-fluid">
          <div class="form-group row"style="margin-top:3%;margin-left:2%;width:80%">
            
                <div  >
                     <asp:CustomValidator ID="cus_val" runat="server" Display="Dynamic"></asp:CustomValidator>
                    &nbsp;</div>
             <div class="form-group row">
                 <div >Merchant Name:</div> <div class="col-sm-6"><dx:ASPxComboBox ID="drpMchno" runat="server" 
                     DataSourceID="sqlQRcodeMchLst" TextField ="name" ValueField="mch_no" AutoPostBack="true" OnSelectedIndexChanged="drpMchno_SelectedIndexChanged"></dx:ASPxComboBox></div>
             </div>
             <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" 
            DataSourceID="sqlQRcodeMch" KeyFieldName="SeqNo" 
            Caption="Setup QRCode subMerchant Details" 
            CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"
                           oncelleditorinitialize="ASPxGridView1_CellEditorInitialize" 
                        oncustomerrortext="ASPxGridView1_CustomErrorText" 
                        onrowinserting="ASPxGridView1_RowInserting" 
                        onrowupdating="ASPxGridView1_RowUpdating"
                        OnRowDeleting ="ASPxGridView1_RowDeleting"

                         SummaryText="T"  width="100%"
                        Styles-Header-BackColor="#002668" Styles-Header-Font-Size="X-Small" Styles-Table-CssClass="dxgvControl_Youthful"  
                        >
                <Columns>
                <dx:GridViewCommandColumn VisibleIndex="0" ShowNewButton="true" ShowUpdateButton="true"   ShowDeleteButton="true">
              
                </dx:GridViewCommandColumn>
                 <dx:GridViewDataTextColumn FieldName="name" VisibleIndex="1" Caption="Sub Merchant Name">
            
                       <PropertiesTextEdit NullText="Enter SubMerchant Name or Description"  ValidationSettings-Display="Dynamic"  
                           ValidationSettings-RequiredField-IsRequired="true" 
                        ValidationSettings-ErrorDisplayMode="ImageWithTooltip" ValidationSettings-ErrorText="Name required" >
                             </PropertiesTextEdit>

                                               </dx:GridViewDataTextColumn>
                
            
              <dx:GridViewDataTextColumn FieldName="phone" VisibleIndex="3" Caption="Phone">
            
                       <PropertiesTextEdit NullText="Enter Phone No." ValidationSettings-Display="Dynamic"   ValidationSettings-RequiredField-IsRequired="true"  
                        ValidationSettings-ErrorDisplayMode="ImageWithTooltip" ValidationSettings-ErrorText="Contact Phone required"  > </PropertiesTextEdit>

                                               </dx:GridViewDataTextColumn>
                  <dx:GridViewDataTextColumn FieldName="email" VisibleIndex="4" Caption="Email">
            
                       <PropertiesTextEdit NullText="Enter Email"  ValidationSettings-Display="Dynamic" 
                            
                           ValidationSettings-ErrorText="Invalid e-mail." ValidationSettings-ErrorDisplayMode="ImageWithTooltip" 
                           ValidationSettings-RequiredField-IsRequired="true"  > </PropertiesTextEdit>

                                               </dx:GridViewDataTextColumn>
 

                 

                    
              <dx:GridViewDataSpinEditColumn FieldName="sub_amount" VisibleIndex="7" Caption="Default Amount">
            
                       <PropertiesSpinEdit NullText="Enter sub merchant default Amount"  ValidationSettings-Display="Dynamic" 
                           Increment=".01" AllowUserInput="true"   MinValue="0.0" MaxValue="99999990.90" 
                            DecimalPlaces="2" ValidationSettings-RequiredField-IsRequired="true"  
                        ValidationSettings-ErrorDisplayMode="ImageWithTooltip" ValidationSettings-ErrorText="Contact merchant amount required"  > </PropertiesSpinEdit>

                                               </dx:GridViewDataSpinEditColumn>
                   <dx:GridViewDataTextColumn FieldName="account_number" VisibleIndex="8" Caption="account_number" Visible="false" EditFormSettings-Visible="False">
                    

                </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Mch_no" VisibleIndex="9" Caption="Mechant id" EditFormSettings-Visible="False">
                    

                </dx:GridViewDataTextColumn>
 
                    <dx:GridViewDataTextColumn FieldName="statusDesc" VisibleIndex="10" Caption="Status" EditFormSettings-Visible="False">
                    

                </dx:GridViewDataTextColumn>
                 <dx:GridViewDataTextColumn FieldName="statusDesc" VisibleIndex="10" Caption="Details" EditFormSettings-Visible="False">
                    
                     <DataItemTemplate>
<a href="#"> Print QRCode..</a>
                     </DataItemTemplate>
                </dx:GridViewDataTextColumn>
            </Columns>


           <TotalSummary>
                                                <dx:ASPxSummaryItem ShowInColumn="SeqNo" ShowInGroupFooterColumn="SeqNo" SummaryType="Count"/>
                                               
                                           </TotalSummary>
                                         

                                           <SettingsPager AlwaysShowPager="True"  >
                                           </SettingsPager>
                                         
                             <Settings  VerticalScrollBarMode="Visible" VerticalScrollBarStyle="VirtualSmooth" VerticalScrollableHeight="350" />

<SettingsPopup>
<HeaderFilter MinHeight="140px"></HeaderFilter>
</SettingsPopup>

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
<Table CssClass="dxgvControl_Youthful"></Table>

<Header BackColor="#002668" Font-Size="X-Small"></Header>

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
                 <%--</div>--%> 
                
     
                    <asp:SqlDataSource ID="sqlQRcodeMchLst" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:EazyConnect %>" 
                        SelectCommand="SpQRMerchantGetInfo" 
            SelectCommandType="StoredProcedure"  >
                          <SelectParameters>
            <asp:Parameter DefaultValue="3" Name="status" Type="Int32" />
            
            </SelectParameters>
                    </asp:SqlDataSource>
                    <asp:SqlDataSource ID="sqlQRcodeMch" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:EazyConnect %>" 
                        SelectCommand="SpQRSubMerchantGetInfo" 
            SelectCommandType="StoredProcedure"  >
                        <SelectParameters>
            <asp:Parameter DefaultValue="0" Name="mch_no" Type="String" />
            
            </SelectParameters>
                    </asp:SqlDataSource>
        </div>
            </div>
    </form>
</body>
</html>
