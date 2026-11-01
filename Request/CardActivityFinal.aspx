<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CardActivityFinal.aspx.cs" Inherits="Request_CardActivityFinal" %>
<%@ Register assembly="DevExpress.Web.v19.1, Version=19.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<!DOCTYPE html>

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
     <link href="../Resources/css/SpecialSelect.css" rel="stylesheet" type="text/css" />
        <script type="text/javascript">

            function ctrlGrid_SelectionChanged(s, e) {


                s.GetSelectedFieldValues("SeqCardNo", GetSelectedFieldValuesCallback);

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
        </head>
<body>
    <form id="form1" runat="server">
                <div id="layout-wrapper">
             
        <div class="container-fluid">

             <div class="form-group row">
         <dx:aspxgridview ID="ASPxGridView1" runat="server" 
                    DataSourceID="SqlacctDetails" AutoGenerateColumns="False" 
                    KeyFieldName="SeqCardNo" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" 
                    CssPostfix="Aqua" Width="100%"    >
                   
                    <Settings ShowFilterBar="Visible" ShowGroupedColumns="True" ShowGroupPanel="True"
                        ShowStatusBar="Visible" />
                    <SettingsLoadingPanel ImagePosition="Top" />
                   
                    <Columns>
                            <dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" FixedStyle="Left" Width="3%"  >
                                    <HeaderTemplate>
                                     <dx:ASPxCheckBox ID="SelectAllCheckBox" runat="server" ToolTip="Select/Unselect all rows on the page"
                                            ClientSideEvents-CheckedChanged="function(s, e) { ASPxGridView1.SelectAllRowsOnPage(s.GetChecked()); }" />
                                    </HeaderTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </dx:GridViewCommandColumn>
                         <dx:GridViewDataTextColumn FieldName="SeqCardNo" VisibleIndex="1" Name="SeqCardNo" Caption=" ReferenceID"   FixedStyle="Left" Visible="false"  >
                            </dx:GridViewDataTextColumn>
                             <dx:GridViewDataTextColumn FieldName="depCardpanno" VisibleIndex="6" Name="depCardpanno" Caption="depCardpanno"   FixedStyle="Left" Visible="false"  >
                            </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Account No" FieldName="AccountNo" 
                            VisibleIndex="3" >
                         
                            
                        </dx:GridViewDataTextColumn>
                         
                        <dx:GridViewDataTextColumn Caption="Card DisplayName" FieldName="AccountName" 
                            VisibleIndex="3" Width="20%">
                            <DataItemTemplate>
                                <dx:ASPxLabel ID="ASPxTextBox2" runat="server" MaxLength="25" Text='<%# Eval("AccountName") %>' ToolTip ='<%# Eval("DispLenght") %>' 
                                    Width="100%">
                                </dx:ASPxLabel>
                            </DataItemTemplate>
                            <HeaderStyle Wrap="False" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Address" FieldName="address" 
                            VisibleIndex="9" >
                            <DataItemTemplate>
                                &nbsp;<asp:Label ID="txtaddress" runat="server" MaxLength="30" TextMode="MultiLine" Text='<%# Eval("Address") %>' Width="100%" ToolTip ='<%# Eval("AddLenght") %>' ></asp:Label>
                                <dx:ASPxLabel ID="ASPxTextBox1" runat="server" Height="83px" MaxLength="30" Visible="false"
                                    Text='<%# Eval("Address") %>' Width="100%" ToolTip ='<%# Eval("AddLenght") %>' >
                                </dx:ASPxLabel>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Product" FieldName="Product" VisibleIndex="8">
                        </dx:GridViewDataTextColumn>
                              <dx:GridViewDataTextColumn Caption="PanNo" FieldName="depCardpannoMask" VisibleIndex="2">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="SeqNo" FieldName="SeqCardNo" Visible="False" 
                            VisibleIndex="6">
                        </dx:GridViewDataTextColumn>
                    
                        <dx:GridViewDataTextColumn Caption="City" FieldName="city" VisibleIndex="5" 
                             >
                            <DataItemTemplate>
                                <dx:ASPxLabel ID="ASPxTextBox3" runat="server" Text='<%# Eval("city") %>' Width="100%">
                                </dx:ASPxLabel>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        
                        
                        <dx:GridViewDataTextColumn Caption="Transaction Status" 
                            FieldName="Trans Status" VisibleIndex="21">
                        </dx:GridViewDataTextColumn>
                        
                        <dx:GridViewDataTextColumn Caption="Account Status" FieldName="Account Status" 
                            VisibleIndex="23">
                        </dx:GridViewDataTextColumn>
                             <dx:GridViewDataTextColumn VisibleIndex="24"   Caption="Reason">

                                <DataItemTemplate>
                            
                                                
                                                  <dx:ASPxMemo ID="tatReason" runat="server" CommandName='<%# Eval("SeqCardNo") %>' ToolTip='<%# Eval("AccountNo") %>' NullText="Reason for rejection" MaskSettings-ShowHints="True">
                                                </dx:ASPxMemo>
 
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                    </Columns>
                    <SettingsPager PageSize="50">
                    </SettingsPager>
             <Settings  ShowVerticalScrollBar="true"  VerticalScrollableHeight="150" />

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
                        
                   <ClientSideEvents SelectionChanged="ctrlGrid_SelectionChanged"  />
                </dx:aspxgridview>
           
        </div>
        </div> </div>
          <asp:SqlDataSource ID="SqlacctDetails" runat="server"  >

                </asp:SqlDataSource>
        <script src="../Resources/libs/jquery/jquery.min.js"></script>
   <script src="../Resources/libs/bootstrap/js/bootstrap.bundle.min.js"></script>
   <script src="../Resources/libs/metismenu/metisMenu.min.js"></script>
   <script src="../Resources/libs/simplebar/simplebar.min.js"></script>
   <script src="../Resources/libs/node-waves/waves.min.js"></script>
   <script src="../Resources/js/hidehref.js"></script>
   <script src="../Resources/js/preloader.js"></script>

     <!-- preload -->
            <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
            <script src="../preloader.js"></script>
   <!-- counter -->
   <script src="../Resources/js/counters.js"></script>
   <script>
       var slideIndex = 0;
       carousel();

       function carousel() {
           var i;
           var x = document.getElementsByClassName("mySlides");
           for (i = 0; i < x.length; i++) {
               x[i].style.display = "none";
           }
           slideIndex++;
           if (slideIndex > x.length) { slideIndex = 1 }
           x[slideIndex - 1].style.display = "block";
           setTimeout(carousel, 9000); // Change image every 2 seconds
       }
   </script>

   <!-- Plugin Js-->
   <script src="../Resources/libs/chartist/chartist.min.js"></script>
   <script src="../Resources/libs/chartist-plugin-tooltips/chartist-plugin-tooltip.min.js"></script>
   <script src="../Resources/libs/morris.js/morris.min.js"></script>
   <script src="../Resources/libs/raphael/raphael.min.js"></script>
   <!-- Peity chart-->
   <script src="../Resources/libs/peity/jquery.peity.min.js"></script>

   <!-- Plugins js-->
   <script src="../Resources/libs/admin-resources/jquery.vectormap/jquery-jvectormap-1.2.2.min.js"></script>
   <script src="../Resources/libs/admin-resources/jquery.vectormap/maps/jquery-jvectormap-world-mill-en.js"></script>
  
   <!-- Required datatable js -->
   <script src="../Resources/libs/datatables.net/js/jquery.dataTables.min.js"></script>
   <script src="../Resources/libs/datatables.net-bs4/js/dataTables.bootstrap4.min.js"></script>

   <script src="../Resources/js/pages/dashboard2.init.js"></script>

   <script src="../Resources/js/app.js"></script>
         <script src="../showOrHideLoadingAnimation.js"></script>

    </form>
</body>
</html>
