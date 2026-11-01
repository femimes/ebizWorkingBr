<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TellerUnBilled.aspx.cs" Inherits="AlphaBeta_TellerUnBilled" %>

<%@ Register Assembly="DevExpress.Web.v19.1, Version=19.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
 
<!doctype html>
<html lang="en">

        <head>
        
            <meta charset="utf-8" />
            <title>ABBEY - Mortgage Bank</title>
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <meta content="Premium Multipurpose Admin & Dashboard Template" name="description" />
            <meta content="Themesbrand" name="author" />
    
            <link href="https://fonts.googleapis.com/css?family=Nunito:400,600,700&display=swap" rel="stylesheet">
            <link rel="stylesheet" href= "https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
    
            <!-- App favicon -->
            <link rel="shortcut icon" href="../mainpage/assets/images/favicon.ico">
    
            <link href="https://cdnjs.cloudflare.com/ajax/libs/animate.css/3.5.2/animate.min.css" rel="stylesheet"/>
    
            <!--Chartist Chart CSS -->
         <link href="../Resources/css/hartist.min.css" rel="stylesheet" type="text/css" /> 
            <!-- plugin css -->
            <link href="../Resources/css/jquery-jvectormap-1.2.2.css" rel="stylesheet" type="text/css" /> 
            <!-- DataTables -->
            <link href="../Resources/css/dataTables.bootstrap4.min.css" rel="stylesheet" type="text/css" />
    
            <!-- Bootstrap Css -->
            <link href="../Resources/css/bootstrap.min.css" id="bootstrap-style" rel="stylesheet" type="text/css" />
            <!-- Icons Css -->
            <link href="../Resources/css/icons.min.css" rel="stylesheet" type="text/css" />
            <!-- App Css-->
            <link href="../Resources/css/app.min.css" id="app-style" rel="stylesheet" type="text/css" />
            <link href="../Resources/css/customstyle.css" rel="stylesheet" type="text/css" />
            <link href="../Resources/css/style.css" rel="stylesheet" type="text/css" />
     <link href="../Resources/css/SpecialSelect.css" rel="stylesheet" type="text/css" />
                      <link href="../style.css" rel="stylesheet">

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
                <div class="bounce2 spinner-bonce2"></div>
                <div class="bounce3 spinner-bonce3"></div>
                <!-- <div class="title-text">
                        <h5>Welcome to Abbey <br>
                        <span>Mortgage Bank PLC.</span></h5>
                </div> -->
            </div>
        </div>

        <!-- Begin page -->
        <div id="layout-wrapper">
             
       
            <!-- ============================================================== -->
            <!-- Start right Content here -->
            <!-- ============================================================== -->
            <div >

                <div>
                    <div class="container-fluid">

                        <!-- start page title -->
                        <div class="row">
                            <div class="col-12">
                                <div class="page-title-box d-flex align-items-center justify-content-between">
                                    <h4 class="mb-0 font-size-18"> </h4>

                                    <div class="page-title-right">
                                        <ol class="breadcrumb m-0">
                                            <li class="breadcrumb-item">
                                              <asp:HyperLink   ID="hypdashboard" style="color:#4d96ff!important" CssClass="customlink" runat="server"  >Dashboard</asp:HyperLink>
                                            </li>  
                                            <li class="breadcrumb-item"><a>Revenue</a></li>
                                            <li class="breadcrumb-item active">Back Office<asp:Label ID="lblAccountNo" runat="server" Visible="False"></asp:Label></li>
                                        </ol>
                                    </div>

                                </div>
                            </div>
                        </div>
                        <!-- end page title -->
                        <div class="row">
                            <div class="col-lg-8" style="width:70%">
                                <div class="card cuscard">
                                        <div class="card-header">
                                       <h4 class="mb-0 cuscardtitle">Generate Bills & Payment
                                        <%--<p class="cusactive">A
                                            <p class="cusdisable">D</p>
                                        </p>--%>
                                       </h4>
                                     </div>
                                    <div class="card-body" style="padding: 2.25rem;">  
                                      
                              <asp:Label ID="errMsg" runat="server" CssClass="error"></asp:Label> <asp:Label ID="lblPaymentRefNo" runat="server" Visible="false"></asp:Label> 
 
                                            <div class="custom-control custom-switch mb-3" style="text-align: end;visibility:hidden" >
                                                    <input type="checkbox" class="custom-control-input theme-choice"   id="customSwitch1"   onchange="LoadBeneficiaryList()"  name="customSwitch1"  runat="server"  />
                                                    <label class="custom-control-label" for="move" Visible="false">Saved Beneficiary</label>
 
                                            </div>
                                        <div class="row">
                                            <div class="col-lg-12 ">

                                                  <div class="form-group row">
                                                    <label for="example-text-input" class="col-sm-4 col-form-label">Payer Type:</label>
                                                    <div class="col-sm-5">
                             <asp:UpdatePanel ID="UpdatePanel6" runat="server"><ContentTemplate>
                                     <dx:ASPxComboBox ID="drpPayerType"    runat="server"      CssClass="form-control"  Width="100%"  >
                           
                                  <Items>
                                      <dx:ListEditItem Text="Individual" Value="N" Selected="true"/>
                                          <dx:ListEditItem Text="Corporate" Value="C"  />
                                  </Items>
                                   <Paddings Padding="0px" />
                                  
                      </dx:ASPxComboBox>  
                   </ContentTemplate></asp:UpdatePanel>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label for="example-text-input" class="col-sm-4 col-form-label">Customer Reference:</label>
                                               <div class="col-sm-5">
                                                  <div class="row">
                                                            <div class="col-sm-8" >
                                                             <%--  <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                                                   <ContentTemplate>--%>
                                                                     
                                                                       <dx:ASPxSpinEdit ID="txtBenAccountNo" runat="server" AutoPostBack="True" CssClass="form-control" HorizontalAlign="left"  OnNumberChanged="BtnValidate_Click"    Width="250px" NullText=" Enter Payer ID..">
                                                                           <ValidationSettings ErrorImage-ToolTip="Customer reference details" SetFocusOnError="True">
                                                                               <RequiredField IsRequired="True" />
                                                                           </ValidationSettings>
                                                                       </dx:ASPxSpinEdit> 
                                    
<%--                                                                   </ContentTemplate>
                                                                
                                                               </asp:UpdatePanel>  --%> 

                                                            </div>
                                                      <div  class="col-sm-2" >
                                                                    <asp:Button runat="server" ID="BtnValidate" class="btn btn-primary ml-2"  Text="Validate" OnClick="BtnValidate_Click"></asp:Button>  
                                                        </div>
           </div></div>
                                                   

                                                </div>

                                                <div class="form-group row">
                                                    <label class="col-sm-4 col-form-label">Agency/Biller Name:</label>
                                                    <div class="col-sm-5">
                                                            <tr class="row form-control">
                                                                    <td id="ISINcb" class="lblCell_R" align="center">
                                                                    
<asp:UpdatePanel ID="UpdatePanel8" runat="server"><ContentTemplate>
                  <dx:ASPxComboBox ID="drpAccountNo"    runat="server"        ValueField="AgencyRef" CssClass="form-control"  Width="100%" AutoPostBack="True" DataSourceID="ObjectDataSource1" OnSelectedIndexChanged="drpAccountNo_SelectedIndexChanged" IncrementalFilteringMode="Contains"  ItemStyle-Wrap="True" >
                          <Columns>
                                        <%--<dx:ListBoxColumn Caption="Agency Code" FieldName="AgencyRef"  />--%>
                                        <dx:ListBoxColumn Caption="Name" FieldName="AgencyOrcFullName" />
                                        <%--<dx:ListBoxColumn Caption="HeadRef" FieldName="HeadRef" Visible="false" />--%>
                                        
                             
                                    </Columns>
                                  
                                   <Paddings Padding="0px" />
                                   <ValidationSettings Display="Dynamic" SetFocusOnError="True" ErrorImage-ToolTip ="Account No is Required" > 
                                       <ErrorImage ToolTip="Account No is Required">
                                       </ErrorImage>
                                       <RequiredField IsRequired="True"  />
                                       
                                   </ValidationSettings>
                      </dx:ASPxComboBox>  </ContentTemplate></asp:UpdatePanel>
                                                                    </td>
                                                                  </tr>
                                                        
                                                    </div>
                                                </div>

                                                <div class="form-group row">
                                                    <label for="example-text-input" class="col-sm-4 col-form-label">Revenue:</label>
                                                    <div class="col-sm-5">
                             <asp:UpdatePanel ID="UpdatePanel1" runat="server"><ContentTemplate>
                                     <dx:ASPxComboBox ID="drpRevDetails"    runat="server"       ValueField="RevCode"   CssClass="form-control"  Width="100%" DataSourceID="ObjectDataSource3" ItemStyle-Wrap="True"  IncrementalFilteringMode="Contains"  >
                          <Columns>
                                        <%--<dx:ListBoxColumn Caption="Agency Code" FieldName="RevCode"  />--%>
                                        <dx:ListBoxColumn Caption="Name" FieldName="AgencyOrcRevName"  />
                                        <%--<dx:ListBoxColumn Caption="HeadRef" FieldName="AgencyHead" Visible="false" />--%>
                                         
                                        
                             
                                    </Columns>
                                  
                                   <Paddings Padding="0px" />
                                   <ValidationSettings Display="Dynamic" SetFocusOnError="True" ErrorImage-ToolTip ="Account No is Required" > 
                                       <ErrorImage ToolTip="Account No is Required">
                                       </ErrorImage>
                                       <RequiredField IsRequired="True"  />
                                       
                                   </ValidationSettings>
                      </dx:ASPxComboBox>  
                   </ContentTemplate></asp:UpdatePanel>
                                                    </div>
                                                </div>

                                                <div class="form-group row">
                                                    <label for="example-text-input" class="col-sm-4 col-form-label">Bill Types:</label>
                                                    <div class="col-sm-5">
                             <asp:UpdatePanel ID="UpdatePanel7" runat="server"><ContentTemplate>
                                     <dx:ASPxComboBox ID="drpTypes"    runat="server"     TextFormatString="{0}" ValueField="type"    CssClass="form-control"  Width="100%"  DataSourceID="ObjectDataSource2"  Enabled="false" >
                          <Columns>
                                        <dx:ListBoxColumn Caption="Bill Type" FieldName="type"  />
                                         
                                        
                             
                                    </Columns>
                                  
                                   <Paddings Padding="0px" />
                                   <ValidationSettings Display="Dynamic" SetFocusOnError="True" ErrorImage-ToolTip ="Bill Type is Required" > 
                                       <ErrorImage ToolTip="Bill type is Required">
                                       </ErrorImage>
                                       <RequiredField IsRequired="True"  />
                                       
                                   </ValidationSettings>
                      </dx:ASPxComboBox>  
                   </ContentTemplate></asp:UpdatePanel>
                                                    </div>
                                                </div>
                                           
                                                <div class="form-group row">
                                                    <label for="example-text-input" class="col-sm-4 col-form-label">Amount:</label>
                                                    <div class="col-sm-5">
                                                      <asp:UpdatePanel ID="UpdatePanel2" runat="server"><ContentTemplate>
                        <dx:ASPxTextBox ID="txtAmount" runat="server"   NullText="enter amount" Width="250px"
                          Text="0" MaskHintStyle-Font-Underline="false" AutoPostBack="false"   >
                          <MaskSettings Mask="<0..99999999999999g>.<00..99>" IncludeLiterals="DecimalSymbol" />
                         <%-- <MaskSettings Mask="############.##" ShowHints="false" PromptChar=" " IncludeLiterals="DecimalSymbol"  />--%>
                          
                        <MaskHintStyle Font-Underline="False"></MaskHintStyle>
                          
                            <ValidationSettings Display="Dynamic" SetFocusOnError="True"  >
                                <RegularExpression ErrorText="amount cannot be 0" ValidationExpression="^[1-9][0-9]*(\.[0-9]+)?|0+\.[0-9]*[1-9][0-9]*$." />
                                <RequiredField IsRequired="True"  />
                            </ValidationSettings>
                          
                      </dx:ASPxTextBox>
                            </ContentTemplate></asp:UpdatePanel>

                                                    </div>
                                                </div>

                                             <div class="form-group row">
                                                    <label for="example-text-input" class="col-sm-4 col-form-label">Period:</label>
                                                    <div class="col-sm-5">
                                                      <asp:UpdatePanel ID="UpdatePanel4" runat="server"><ContentTemplate>
                        <dx:ASPxTextBox ID="txtPeriod" runat="server"   NullText="enter period details" Width="250px"  ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
                              
                          
                          
                            <ValidationSettings Display="Dynamic" SetFocusOnError="True"   >
                                
                                <RequiredField IsRequired="True"  ErrorText="Perod is required" />
                            </ValidationSettings>
                          
                      </dx:ASPxTextBox>
                            </ContentTemplate></asp:UpdatePanel>

                                                    </div>
                                                </div>
                                               
                                                <div class="form-group row"  style="margin-left:40%"  >
                                                     
                                                  
                                                    <%-- <MaskSettings Mask="############.##" ShowHints="false" PromptChar=" " IncludeLiterals="DecimalSymbol"  />--%>
                                                             <dx:ASPxButton runat="server" Text="Clear"  ID="Button1" CausesValidation="false"  CssClass="btn-sm btn-primary ml-2" OnClick="Button1_Click" />
                                                    <dx:ASPxButton runat="server" Text="Pay Bills"  ID="btnPayBills"   CssClass="btn-sm btn-primary ml-2" OnClick="btnPayBills_Click" ></dx:ASPxButton>
                                           
                                                </div>

                                            </div>   
                                        </div>
                                    </div>
                                </div>
                            </div> <!-- end col -->
                            <div class="col-lg-4" style="width:30%">
                                <div class="card col-12" style="min-height: 150px">
                                        <div style=" 
                                        /* background-position-x: center; */
                                        margin: 0px -12px;
                                        height: 30px;
                                        padding: 5px 14px 4px 14px;
                                        color: #000;" class="card-header">
                                                <h5 class="mt-0">Best Approach</h5>
                                            </div>
                                    <div class="card-body">
                                        <div class="row">
                                      
                                        <p>We advice you to secure your transcation and <br> do not share your code with anyone.</p>
                                        </div>  
                                            <div   class="form-group row"><asp:Label ID ="lblWebGuid" runat="server" ForeColor="Red"></asp:Label> </div>
                                         <%-- <MaskSettings Mask="############.##" ShowHints="false" PromptChar=" " IncludeLiterals="DecimalSymbol"  />--%>
                                      
                                        <div   class="form-group row">Pay ID: <asp:Label ID ="LblPayID" runat="server" ForeColor="Red"></asp:Label></div>
                                         
                                        
                                        <%--<button type="submit" class="btn btn-primary ml-2">Clear</button>--%> 
                                        
                                        <div class="form-group row">Pay Name: <asp:Label ID ="LblPayName" runat="server" ForeColor="Red"></asp:Label></div>
                 
                                        <div class="form-group row"><asp:Label ID ="lblAmount" runat="server" ForeColor="blue" Visible="false"></asp:Label></div>
                 

                                      <div   class="form-group row">
                                           <asp:Button runat="server" Text="Generate Receipts"  ID="btnReceipts"   CssClass="btn btn-primary ml-2" Enabled="false" OnClick="btnReceipts_Click" Visible="false" ></asp:Button>
                                        <div class="col-4 text-right; align-text-bottom"  > <i class="fas fa-print invisible" ><asp:HyperLink runat="server" ID="hypReceipt" Visible="false" >Print</asp:HyperLink></i></div> </div>                   
                                     <div class="form-group row"> 

                                       
                                     </div>
                                    </div>
                                </div>
                                <div class="card col-12" style="min-height: 150px;
                                background-image: url(../mainpage/assets/images/small/img-5.jpg);
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
                        </div> <!-- end row --> 
                      
                    </div> <!-- container-fluid -->
                </div>
                <!-- End Page-content -->

           
            </div>
            <!-- end main content-->

        </div>
        <!-- END layout-wrapper -->
 
        <div>  
     
            <asp:UpdatePanel ID="UpdatePanel3" runat="server"></asp:UpdatePanel>
                      <asp:UpdatePanel ID="UpdatePanel9" runat="server"></asp:UpdatePanel>

                       <asp:UpdatePanel ID="UpdatePanel5" runat="server"></asp:UpdatePanel>

            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="ProcessAphaBetaAgencyds" TypeName="iBankPoints.iPoints">
                <SelectParameters>
 
                     <asp:CookieParameter CookieName="Token" Name="Token" Type="String" DefaultValue="100" />
                </SelectParameters>
              </asp:ObjectDataSource>
             <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" SelectMethod="ProcessAphaBetaBillsType" TypeName="iBankPoints.iPoints">
                <SelectParameters>
<%--                    <asp:CookieParameter CookieName="eLonginUser" Name="strUserName" Type="String" />
                    <asp:ControlParameter ControlID="lblAccountNo" DefaultValue="99" Name="strAccountNo" PropertyName="Text" Type="String" />--%>
                     <asp:CookieParameter CookieName="Token" Name="Token" Type="String" DefaultValue="100" />
                </SelectParameters>
              </asp:ObjectDataSource>
            <asp:ObjectDataSource ID="ObjectDataSource3" runat="server" SelectMethod="ProcessAphaBetaRevList" TypeName="iBankPoints.iPoints">
                <SelectParameters>
 
                     <asp:CookieParameter CookieName="Token" Name="Token" Type="String" DefaultValue="100" />
 
                    <asp:ControlParameter ControlID="drpAccountNo" Name="AgencyRef" PropertyName="Value" Type="String" /> 
                </SelectParameters>
              </asp:ObjectDataSource>

            <%--</div>--%>
            <asp:ValidationSummary ID="ValSumAlert" runat="server" CssClass="error error-img"
                 ValidationGroup="ChangeUserPasswordValidationGroup" ShowMessageBox="True" HeaderText="iPortal Alert" style="margin-bottom: 19px" />
    <asp:TextBox ID="txtUserName" runat="server" Visible="false"></asp:TextBox>
        </div>
        <!-- Right bar overlay-->
        <div class="rightbar-overlay"></div>

   <!-- JAVASCRIPT -->

       

   <script src="../mainpage/assets/libs/jquery/jquery.min.js"></script>
   <script src="../mainpage/assets/libs/bootstrap/js/bootstrap.bundle.min.js"></script>
   <script src="../mainpage/assets/libs/metismenu/metisMenu.min.js"></script>
   <script src="../mainpage/assets/libs/simplebar/simplebar.min.js"></script>
   <script src="../mainpage/assets/libs/node-waves/waves.min.js"></script>
   <script src="../mainpage/assets/js/hidehref.js"></script>
   <script src="../mainpage/assets/js/preloader.js"></script>
     <!-- preload -->
            <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
            <script src="../preloader.js"></script>

   <!-- counter -->
   <script src="../mainpage/assets/js/counters.js"></script>
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
   <script src="../mainpage/assets/libs/chartist/chartist.min.js"></script>
   <script src="../mainpage/assets/libs/chartist-plugin-tooltips/chartist-plugin-tooltip.min.js"></script>
   <script src="../mainpage/assets/libs/morris.js/morris.min.js"></script>
   <script src="../mainpage/assets/libs/raphael/raphael.min.js"></script>
   <!-- Peity chart-->
   <script src="../mainpage/assets/libs/peity/jquery.peity.min.js"></script>

   <!-- Plugins js-->
   <script src="../mainpage/assets/libs/admin-resources/jquery.vectormap/jquery-jvectormap-1.2.2.min.js"></script>
   <script src="../mainpage/assets/libs/admin-resources/jquery.vectormap/maps/jquery-jvectormap-world-mill-en.js"></script>
  
   <!-- Required datatable js -->
   <script src="../mainpage/assets/libs/datatables.net/js/jquery.dataTables.min.js"></script>
   <script src="../mainpage/assets/libs/datatables.net-bs4/js/dataTables.bootstrap4.min.js"></script>

   <script src="../mainpage/assets/js/pages/dashboard2.init.js"></script>

   <script src="../mainpage/assets/js/app.js"></script>
          </form>
    </body>
</html>