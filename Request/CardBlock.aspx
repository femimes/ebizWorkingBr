<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CardBlock.aspx.cs" Inherits="Request_CardBlock" %>
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

        </head>


    <body data-topbar="dark">
 <form id="form1" runat="server" >

              <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
      
      
        <!--CSS Spinner-->
        <div class="spinner-wrapper">
            <!-- <div class="spinner"></div>
            <div class="title-text">
                    <h5>Welcome to Jubilee-Life <br>
                    <span>Online Banking</span></h5>
            </div> -->
            <div class="spinner">
                <div class="bounce1 spinner-bonce"></div>
                <div class="bounce2 spinner-bonce"></div>
                <div class="bounce3 spinner-bonce"></div>
                <!-- <div class="title-text">
                        <h5>Welcome to Abbey <br>
                        <span>Mortgage Bank PLC.</span></h5>
                </div> -->
            </div>
        </div>

        <!-- Begin page -->
        <div id="layout-wrapper">
             
        <div class="container-fluid">

                        <!-- start page title -->
                        <div class="row">
                            <div class="col-md-12">
                                <div class="page-title-box d-flex align-items-center justify-content-between">
                                    <h4 class="mb-0 font-size-18">Card Operation Maintenance</h4>

                                    <div class="page-title-right">
                                        <ol class="breadcrumb m-0">
                                            <li class="breadcrumb-item"><a href="javascript: void(0);">Dashboard</a></li>
                                            
                                            <li class="breadcrumb-item active">Card Operation Maintenance<asp:Label ID="lblAccountNo" runat="server" Visible="False"></asp:Label></li>
                                        </ol>
                                    </div>

                                </div>
                            </div>
                        </div>
                        <!-- end page title -->
                               <asp:Label ID="errMsg" runat="server" CssClass="error"></asp:Label>
                        <div class="row">
                            <div class="col-md-8">
                                <div class="card ">
                                    <div class="card-body" > 
                                         <div class="row">
                                            <div class="col-md-12 ">
           
                                                            
        <div class="form-group row">
         Request Title:  <asp:Textbox  ID="Textbox1" runat="server" CssClass="form-control" required   /> 

           </div>
        <div class="form-group row">
         Maintenance Type:  <asp:DropDownList  ID="DrpMaithType" runat="server" CssClass="form-control"    /> 

           </div>                           

        <div class="form-group row">
         Product:  <%--<asp:DropDownList  ID="drp_products" runat="server" CssClass="form-control"    /> --%>
             <dx:aspxcombobox ID="drp_products" EnableIncrementalFiltering="True"  
                    EnableSynchronization="False" IncrementalFilteringMode="StartsWith"  
                    runat="server"
      CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"  
                       ShowShadow="False" 
                    SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" 
                    DataSourceID="sqlproducts" TextField="product" ValueField="Product Code" 
                    AutoPostBack="True" Width="100%">
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

           </div>

<div class="form-group row">
         Branch:  
    <%--<asp:DropDownList  ID="drpbranches" runat="server" CssClass="form-control"    />--%> 
    <dx:aspxcombobox ID="drpbranches"  EnableIncrementalFiltering="True" 
                                EnableSynchronization="False" IncrementalFilteringMode="StartsWith" 
                                runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" 
                                   ShowShadow="False" 
                                SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" DataSourceID="sqlBranches" 
                                ValueField="Headsub_code" AutoPostBack="True" Width="100%"
                            >
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
           </div>
<div class="form-group row">
         Specific AccountList:  <asp:TextBox  ID="txtSpecificAcct" runat="server" CssClass="form-control"  TextMode="MultiLine"  /> 

           </div>
      
        <div class="form-group row">
          Reason:  
           <asp:Textbox  ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"  required  /> 

                                                    </div>

 <div class="form-group row">
         <dx:aspxgridview ID="ASPxGridView1" runat="server" 
                    DataSourceID="SqlacctDetails" AutoGenerateColumns="False" 
                    KeyFieldName="SeqCardNo" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" 
                    CssPostfix="Aqua"    >
                   
                    <Settings ShowFilterBar="Visible" ShowGroupedColumns="True" ShowGroupPanel="True"
                        ShowStatusBar="Visible" />
                    <SettingsLoadingPanel ImagePosition="Top" />
                   
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="Account No" FieldName="AccountNo" 
                            VisibleIndex="0" Width="100px">
                            <DataItemTemplate>
                                <dx:ASPxCheckBox ID="ASPxCheckBox1" runat="server" CheckState="Unchecked" Text='<%# Eval("AccountNo") %>'
                                    Wrap="False" ToolTip='<%# Eval("SeqCardNo") %>'>
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
                         
                        <dx:GridViewDataTextColumn Caption="Card DisplayName" FieldName="AccountName" 
                            VisibleIndex="1" Width="200px">
                            <DataItemTemplate>
                                <dx:ASPxLabel ID="ASPxTextBox2" runat="server" MaxLength="25" Text='<%# Eval("AccountName") %>' ToolTip ='<%# Eval("DispLenght") %>' 
                                    Width="100%">
                                </dx:ASPxLabel>
                            </DataItemTemplate>
                            <HeaderStyle Wrap="False" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Address" FieldName="address" 
                            VisibleIndex="4" Width="150px">
                            <DataItemTemplate>
                                &nbsp;<asp:Label ID="txtaddress" runat="server" MaxLength="30" TextMode="MultiLine" Text='<%# Eval("Address") %>' Width="100%" ToolTip ='<%# Eval("AddLenght") %>' ></asp:Label>
                                <dx:ASPxLabel ID="ASPxTextBox1" runat="server" Height="83px" MaxLength="30" Visible="false"
                                    Text='<%# Eval("Address") %>' Width="100%" ToolTip ='<%# Eval("AddLenght") %>' >
                                </dx:ASPxLabel>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Product" FieldName="Product" VisibleIndex="2">
                        </dx:GridViewDataTextColumn>
                              <dx:GridViewDataTextColumn Caption="PanNo" FieldName="depCardpannoMask" VisibleIndex="2">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="SeqNo" FieldName="SeqCardNo" Visible="False" 
                            VisibleIndex="6">
                        </dx:GridViewDataTextColumn>
                    
                        <dx:GridViewDataTextColumn Caption="City" FieldName="city" VisibleIndex="5" 
                            Width="100px">
                            <DataItemTemplate>
                                <dx:ASPxLabel ID="ASPxTextBox3" runat="server" Text='<%# Eval("city") %>' Width="100%">
                                </dx:ASPxLabel>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Book Bal" FieldName="Book Bal" 
                            VisibleIndex="17" Visible="False">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="PhoneNo" VisibleIndex="18" 
                            FieldName="PhoneNo" Width="150px">
                         <DataItemTemplate>
                                <dx:ASPxLabel ID="ASPxTextBox4" runat="server" Text='<%# Eval("PhoneNo") %>' Width="100%">
                                </dx:ASPxLabel>
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
           
        </div>

        <div class="form-group row" >
                                     <p style="align-items:center">
                                                  <asp:Button ID="Button3" runat="server" CssClass="btn btn-primary ml-2" 
            Text="Submit"       /> &nbsp;
            <asp:Button ID="Button4" runat="server" CausesValidation="False"   Text="Reset"  CssClass="btn btn-primary ml-2"   style="width: 59px" /> 
                                          </p>          </div>
                                                  


  

                                                </div>
                                             </div>

                                        </div>

                                </div>

                            </div>

                              <div class="col-md-4">
                                <div class="card col-12" style="min-height: 150px">
                                        <div style="background-color: #c60d00;
                                        /* background-position-x: center; */
                                        margin: 0px -12px;
                                        height: 30px;
                                        padding: 5px 14px 4px 14px;
                                        color: #fff;">
                                                <h5 class="mt-0">Best Approach</h5>
                                            </div>
                                    <div class="card-body">
                                        <div class="row">
                                      
                                        <p>We advice you to secure your transcation and <br> do not share your code with anyone.</p>
                                        </div>   
                                        <div>
                                            <p> <asp:checkbox runat="server" ID="ChkCardType" Text="Print CurrentCard? .." TextAlign="Left"></asp:checkbox></p>
                                        </div>
                                    </div>
                                </div>
                                <div class="card col-12" style="min-height: 150px;
                                background-image: url(/Resources/images/small/img-5.jpg);
                                background-size: cover;
                                height: 200px;">
                                        <div class="card-body">
                                            <!-- <h5 class="mt-0">Best Approach</h5> -->
                                            <div class="row">
                                            <!-- <p>We advice you to secure your transcation and <br> do not share your code with anyone.</p> -->
                                            </div>                                                                  
                                        </div>
                                    </div>
                            </div>
                            </div>
            </div>
        </div>
     
        <asp:SqlDataSource ID="sqlproducts" runat="server" SelectCommand = "Sproc_GetAllProductsCards" SelectCommandType="StoredProcedure"  ConnectionString="<%$ ConnectionStrings:EazyConnect %>" ></asp:SqlDataSource>
                <asp:SqlDataSource ID="sqlBranches" runat="server" SelectCommand = "Sproc_GetAllBranchesCards" SelectCommandType="StoredProcedure"  ConnectionString="<%$ ConnectionStrings:EazyConnect %>" ></asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlacctDetails" runat="server" ></asp:SqlDataSource>
                <asp:SqlDataSource ID="sqlAppr" runat="server"  ></asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataSource5" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:EazyConnect %>" 
                    SelectCommand="SELECT * FROM [CardRequestType]"></asp:SqlDataSource>
                <asp:TextBox ID="txtUserID" runat="server" Wrap="False" Visible="False"></asp:TextBox>
                <asp:TextBox ID="txtref_no" runat="server" Visible="False"></asp:TextBox>
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
