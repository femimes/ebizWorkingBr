<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OtherBankSht.aspx.cs" Inherits="Transaction_OtherBankSht" %>

<%@ Register Assembly="DevExpress.Web.v19.1, Version=19.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
  <head>
        
            <meta charset="utf-8" />
            <title>eBizPlus</title>
            <meta name="viewport" content="width=device-width, initial-scale=1.0" />
            <meta content="Premium Multipurpose Admin & Dashboard Template" name="description" />
            <meta content="Themesbrand" name="author" />
    
            <link href="https://fonts.googleapis.com/css?family=Nunito:400,600,700&display=swap" rel="stylesheet" />
            <link rel="stylesheet" href= "https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />
    
            <!-- App favicon -->
            <link rel="shortcut icon" href="../Resources/images/favicon.ico" />
    
            <link href="https://cdnjs.cloudflare.com/ajax/libs/animate.css/3.5.2/animate.min.css" rel="stylesheet"/>
    
            <!--Chartist Chart CSS -->
            <link href="../Resources/libs/chartist/chartist.min.css" rel="stylesheet" type="text/css" /> 
            <!-- plugin css -->
            <link href="../Resources/libs/admin-resources/jquery.vectormap/jquery-jvectormap-1.2.2.css" rel="stylesheet" type="text/css" /> 
            <!-- DataTables -->
            <link href="../Resources/libs/datatables.net-bs4/css/dataTables.bootstrap4.min.css" rel="stylesheet" type="text/css" />
    
            <!-- Bootstrap Css -->
            <link href="../Resources/css/bootstrap.min.css" id="bootstrap-style" rel="stylesheet" type="text/css" />
            <!-- Icons Css -->
            <link href="../Resources/css/icons.min.css" rel="stylesheet" type="text/css" />
            <!-- App Css-->
            <link href="../Resources/css/app.min.css" id="app-style" rel="stylesheet" type="text/css" />
            <link href="../Resources/css/customstyle.css" rel="stylesheet" type="text/css" />
            <link href="../Resources/css/style.css" rel="stylesheet" type="text/css" />
     <%--<link href="../Resources/css/SpecialSelect.css" rel="stylesheet" type="text/css" />--%>
                <link href="../style.css" rel="stylesheet"/>
      <script src="../js/PageSession.js"></script>
        </head>
<body>
        <!--CSS Spinner-->
     <%--   <div class="spinner-wrapper">
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
        </div>--%>
    <form id="form1" runat="server" >
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
           <div id="layout-wrapper">
             
       
            <!-- ============================================================== -->
            <!-- Start right Content here -->
            <!-- ============================================================== -->
            <div >
         <div class="container-fluid form-page" style="background-color: #fff;">
              <div class="row">
                            <div class="col-12">
                                <div class="page-title-box d-flex align-items-center justify-content-between">
                                    <h4 class="mb-0 font-size-18">Teller Transfer To Other Banks</h4>

                                    <div class="page-title-right">
                                        <ol class="breadcrumb m-0">
                                        <li class="breadcrumb-item"> 
                                              <a href="../mainpage/UIHome.aspx" onclick="hideMenu()"class="submenutxt menulist hyperaction" target="contentUrlPane">Dashboard</a></li>
                                            <%--< <li class="breadcrumb-item">
                                               a href="../mainpage/UISubitems.aspx?q=3" onclick="hideMenu()"class="submenutxt menulist hyperaction" target="contentUrlPane">Sub Menu</a></li>--%>
                                            <li class="breadcrumb-item active">Teller Transfers (Other Banks)<asp:Label ID="lblAccountNo" runat="server" Visible="False"></asp:Label></li>
                                        </ol>
                                    </div>

                                </div>
                            </div>
                        </div>
    

     <asp:Label ID="errMsg" runat="server" CssClass="error"></asp:Label>
 <div class="row mobilex" >
   
   
        <div class="col-lg-6 col-md-6 col-sm-6 col-xl-6 col-xs-12">
              <div class="card-body formstyle mtt-1" > 
             <div class="row">
                 <div class="col-lg-12 " >
             <div class=" col-sm-10 custom-control custom-switch mb-3" style="text-align: end">
                                          
                     <asp:UpdatePanel ID="UpdatePanel8" runat="server"><ContentTemplate>
                
                           <script>

                                            function myfunction() {

                                                var newloc = document.location.href;
                                                if (document.location.hash) {
                                                    // remove fragment
                                                    newloc = newloc.substr(0, newloc.indexOf(document.location.hash));
                                                }
                                                newloc += document.location.search ? ";" : "?"; // prevent double question mark
                                                newloc += 'code=approved';
                                                if (!window.location.hash) {
                                                    var c = document.getElementById("<%=customSwitch1.ClientID %>")
                                                    if (c.checked === true) {
                                                        window.location.replace(document.location.href = 'OtherBenBankSht.aspx')   //window.location.replace(document.location.href + "?code=approved")
                                                    } else {

                                                        //window.location.replace(document.location.href + "?code=nonOk")
                                                        window.location.replace(document.location.href = 'OtherBankSht.aspx')
                                                    }

                                                }
                                                 }

                                        </script>

                                            <div class="custom-control custom-switch mb-3" style="text-align: end" dir="ltr" >
                                                    <input type="checkbox" class="custom-control-input theme-choice"   id="customSwitch1"  onClick="myfunction()"      onchange="LoadBeneficiaryList()"  name="customSwitch1"  runat="server"  />
                                                     <%--<asp:CheckBox   CssClass="custom-control-input theme-choice"   id="customSwitch1"   name="customSwitch1"  runat="server" Checked="true" />--%>
                                             
                                              <%--<dx:ASPxCheckBox name="customSwitch1"  runat="server" ID="customSwitch1" ClientInstanceName="customSwitch1" CssClass="custom-control-input theme-choice"  ></dx:ASPxCheckBox>--%>
                                                
                                                <label class="custom-control-label" for="customSwitch1">Saved Beneficiary</label>
 
                                            </div>

                                                             </ContentTemplate></asp:UpdatePanel>
       
                 </div>

              <div class="form-group row "><label   class="col-sm-10 col-form2-label" >Teller Ledger:</label>
       
                  <div  class="col-sm-10">
                            <div class="formwidth">
             
                             <asp:UpdatePanel ID="UpdAcct" runat="server"><ContentTemplate>
                             <dx:ASPxComboBox ID="drpAccountNo"    runat="server" DataSourceID="ObjectDataSource1"    ValueField="AccountNo"  CssClass="form-control txtstyle formwidth" OnSelectedIndexChanged="drpAccountNo_SelectedIndexChanged"   AutoPostBack="True" >
                                  <Columns>
                                                <dx:ListBoxColumn Caption="AccountNo" FieldName="AccountNo"  />
                                                <dx:ListBoxColumn Caption="Type" FieldName="ProdName" />
                                                <dx:ListBoxColumn Caption="Avail Bal" FieldName="AvailBal" />
                                                 <dx:ListBoxColumn Caption="AccountDesc" FieldName="AccountDesc" Visible="false"/>
                                   </Columns>
                                  
                                           <Paddings Padding="0px" />
                                           <ValidationSettings Display="Dynamic" SetFocusOnError="True" ErrorImage-ToolTip ="Account No is Required" > 
                                               <ErrorImage ToolTip="Account No is Required">
                                               </ErrorImage>
                                               <RequiredField IsRequired="True"  />
                                       
                                           </ValidationSettings>
                              </dx:ASPxComboBox>
                                 </ContentTemplate></asp:UpdatePanel>
                       
                      <%--<asp:DropDownList ID="DrpBanklist" runat="server" CssClass="form-control" DataSourceID="ObjectDataSource1" DataTextField="AccountDesc" DataValueField="AccountNo"  ></asp:DropDownList>--%>
       
                 </div>
                  </div>
           
              </div>

              <div class="form-group row"><div class="col-sm-10 col-form2-label">
                   <asp:UpdatePanel ID="UpdatePanel4" runat="server"><ContentTemplate>
                  <asp:Label ID="lblBenOrBankList" runat="server" CssClass="text-truncate font-size-14 mb-2" Text="Beneficiary Bank Listings:"></asp:Label> 
                       </ContentTemplate></asp:UpdatePanel>
                  </div>
                     <div  class="col-sm-10">
                            <div class="formwidth">
                           <asp:UpdatePanel ID="UpdatePanel1" runat="server"><ContentTemplate>
                            <dx:ASPxComboBox ID="ASPxComboBox1"    runat="server" DataSourceID="ObjectDataSource2"  AutoPostBack="true"  ValueField="SchemeCode" CssClass="form-control txtstyle formwidth" IncrementalFilteringMode="Contains" TextField="bank_name"  >
                                  <Columns>
                                         
                                                <dx:ListBoxColumn Caption="Bank Name" FieldName="bank_name" />
                                            </Columns>
                                    
                                           <Paddings Padding="0px" />
                                           <ValidationSettings Display="Dynamic" SetFocusOnError="True" ErrorImage-ToolTip ="Bank is Required" > 
                                               <ErrorImage ToolTip="Bank Information is Required">
                                               </ErrorImage>
                                               <RequiredField IsRequired="True"  />
                                       
                                           </ValidationSettings>
                              </dx:ASPxComboBox>
                               </ContentTemplate></asp:UpdatePanel>
                              </div>
                      <div> 
                                      <asp:UpdatePanel ID="UpdatePanel9" runat="server"><ContentTemplate>
                                       <dx:ASPxComboBox ID="drpBenList"  Visible="false" AutoPostBack="true"  runat="server" DataSourceID="ObjBenList"    ValueField="ContAccountNo" CssClass="form-control txtstyle formwidth" IncrementalFilteringMode="Contains" TextField="ContAccountName" OnSelectedIndexChanged ="drpBenList_SelectedIndexChanged" >
                                  <Columns>
                                         
                                                <dx:ListBoxColumn Caption="Beneficiary Name" FieldName="ContAccountName"  />
                                                <dx:ListBoxColumn Caption="AccountNo" FieldName="ContAccountNo" />
                                                 <dx:ListBoxColumn Caption="Bank Code" FieldName="ContBankCode"  Visible="false" />
                                 
                                            </Columns>
                                           <ItemStyle  Wrap="True" />
                                
                                           <Paddings Padding="0px" />
                                           <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                               <RequiredField ErrorText="Beneficiary required!!" IsRequired="True" />
                                           </ValidationSettings>
                              </dx:ASPxComboBox>
                                          </ContentTemplate></asp:UpdatePanel>
                                      </div>

                          <div> 
                               <asp:UpdatePanel ID="UpdatePanel5" runat="server"><ContentTemplate><asp:Label ID="lblBankName" runat="server" class="col-md-2 col-form-label" Visible="False" ></asp:Label> 
                                   </ContentTemplate></asp:UpdatePanel>
                              </div>
                  
                      <%--<asp:DropDownList ID="DrpBanklist" runat="server" CssClass="form-control" DataSourceID="ObjectDataSource1" DataTextField="AccountDesc" DataValueField="AccountNo"  ></asp:DropDownList>--%>
       
           
                  </div>
           
              </div>
              <div class="form-group row" >
                  <div class="col-sm-10 col-form2-label" >Beneficiary Account No:</div>
                  <div class="col-sm-10">
                        <div class="formwidth">
                      <asp:UpdatePanel ID="UpdatePanel6" runat="server"><ContentTemplate>
                      <dx:ASPxTextBox ID="txtBenAccountNo" runat="server" CssClass="form-control txtstyle formwidth"   AutoPostBack="true"  
                          OnTextChanged="txtBenAccountNo_TextChanged"  >

                            <ValidationSettings SetFocusOnError="True" ErrorImage-ToolTip="Beneficiary account no required">
                              <RequiredField IsRequired="True" />
                          </ValidationSettings>
                      </dx:ASPxTextBox>
               </ContentTemplate></asp:UpdatePanel>
                            </div>
                  </div>
              </div>
     
               <div class="form-group row"  >
                   <div class="col-sm-10 col-form2-label" >Beneficiary Name:</div>
                   <div class="col-sm-10">
                        <div class="formwidth">
  
                      <asp:UpdatePanel ID="UpdatePanel7" runat="server"><ContentTemplate>
                      <dx:ASPxLabel   ID="lblBenName" runat="server" CssClass="form-control txtstyle formwidth"    ></dx:ASPxLabel>
                           </ContentTemplate></asp:UpdatePanel></div>
           
                 </div>
              </div>
     
               <div class="form-group row"  >
                   <div class="col-sm-10 col-form2-label" >Amount:</div>
                   <div class="col-sm-10">
                        <div class="formwidth">
  
       
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server"><ContentTemplate>
                                <dx:ASPxTextBox ID="txtAmount" runat="server"  NullText="enter amount"
                                  Text="0" MaskHintStyle-Font-Underline="false"  AutoPostBack="True" OnTextChanged="txtAmount_TextChanged"   CssClass="amttxt formwidth" >
                                  <MaskSettings Mask="<0..99999999999999g>.<00..99>" IncludeLiterals="DecimalSymbol" />
                                 <%-- <MaskSettings Mask="############.##" ShowHints="false" PromptChar=" " IncludeLiterals="DecimalSymbol"  />--%>
                          
        <MaskHintStyle Font-Underline="False"></MaskHintStyle>
                          
                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" >
                                        <RegularExpression ErrorText="Transfer amount cannot be 0" ValidationExpression="^[1-9][0-9]*(\.[0-9]+)?|0+\.[0-9]*[1-9][0-9]*$." />
                                        <RequiredField IsRequired="True"  />
                                    </ValidationSettings>
                          
                              </dx:ASPxTextBox>
                                    </ContentTemplate></asp:UpdatePanel>
                                <%-- <MaskSettings Mask="############.##" ShowHints="false" PromptChar=" " IncludeLiterals="DecimalSymbol"  />--%>
            
                  
                   </div>
                  </div>
           
         
              </div>
     
                <div class="form-group row">
                  <div class="col-sm-10 col-form2-label" >Narration:</div>
                  <div class="col-sm-10">
                        <div class="formwidth">
                        <asp:UpdatePanel ID="UpdatePanel11" runat="server"><ContentTemplate>
                      <dx:ASPxTextBox ID="txtNarration" runat="server" CssClass="text-truncate font-size-11 mb-2 form-control-lg txtstyle formwidth"    Paddings-Padding="0px"  AutoPostBack="true"   MaxLength="80" >
                          <ValidationSettings SetFocusOnError="True">
                              <RequiredField IsRequired="True" />
                          </ValidationSettings>
                      </dx:ASPxTextBox>
                            </ContentTemplate></asp:UpdatePanel>
                       </div>
                   </div>
              </div>
  
                <div class="form-group row">
                  <div  class="col-sm-10">
                        <div class="formwidth">
                    
                             <div class="row">
                     <dx:ASPxButton  runat="server" Text="Cancel" CssClass="dxbButton btn btn-primary ml-2" ID="ASPxButton1" OnClick="btnCancel_Click" ></dx:ASPxButton>
               

                          <dx:ASPxButton  runat="server" Text="Transfer"  ID="btnTransfer" OnClick="btnTransfer_Click" CssClass="dxbButton btn btn-primary ml-2"
                             >

                                 <ClientSideEvents Click="function(s, e) {
                                                                                    showLoadingAnimation();
                                                                                }" />
                          </dx:ASPxButton>
                           <%--<asp:Button runat="server" Text="Transfer" CssClass="btn  btn-success btn-xs"  ID="btnTransfer1" OnClick="btnTransfer_Click"  CausesValidation="true" />--%>
                            </div>
                           </div>
                   
                      </div>

                           </div>
           
                
            <%----%>
            </div> 

             </div>
         </div>

        </div> 

     <!-- end col -->
      <div class="col-lg-2 col-sm-2">

        </div>

     <div class="col-lg-4 col-sm-4 sideads">
            <div class="card col-12" style="min-height: 150px;;
            background-image: url('../Resources/images/Banner 2.fw.png');
            background-size: cover;
            height: 200px;">
                    <div class="card-body">
                        <!-- <h5 class="mt-0">Best Approach</h5> -->
                        <div class="row">
                        <!-- <p>We advice you to secure your transcation and <br> do not share your code with anyone.</p> -->
                        </div>                                                                  
                    </div>
                </div>
                                <div class="card col-12" style="min-height: 150px;">
                                        <div style="background-color: #002668;
                                        /* background-position-x: center; */
                                        margin: 0px -12px;
                                        height: 30px;
                                        padding: 5px 14px 4px 14px;
                                        color: #fff;">
                                                <h5 class="mt-0 "> Details..</h5>
                                            </div>
                                    <div class="card-body">
                                        <div class="row">
                                      
                                        <p>We advice you to secure your transcation and <br> do not share your code with anyone.</p>
                                        </div>  
                                         <div class="form-group row">Teller Name: <asp:Label ID ="LblPayName" runat="server" ForeColor="Red"></asp:Label></div>
                                         <div class="form-group row">Transaction Limit: <asp:Label ID ="lblTransAmountLimit" runat="server" ForeColor="blue"  ></asp:Label></div>
                                   
                                        <div class="form-group row">Available Limit: <asp:Label ID ="lblAmount" runat="server" ForeColor="blue"  ></asp:Label></div>
                                       </div>
                                </div>
                             
                            </div>
</div>

            
         
          <%--<asp:UpdatePanel ID="UpdatePanel5" runat="server"><ContentTemplate>--%> 
         
          <%--</ContentTemplate></asp:UpdatePanel>--%>
           <asp:UpdatePanel ID="UpdatePanel10" runat="server"><ContentTemplate>
              <asp:Label ID="txtFee" runat="server" CssClass="form-control enMoney iPointNumberAlign" TextMode="Number"  Text="0.00" Visible="false" ></asp:Label>
           </ContentTemplate></asp:UpdatePanel> <asp:UpdatePanel ID="UpdatePanel3" runat="server"><ContentTemplate>
                          <asp:Label ID="lblAvail" runat="server"  Visible="false"  CssClass="form-control enMoney iPointNumberAlign" TextMode="Number"  Text="0.00" RightToLeft="True"  ></asp:Label>
                    </ContentTemplate></asp:UpdatePanel> 
        

    

                <asp:Label ID="TxtCurr" runat="server" CssClass="form-control" BorderStyle="None" Visible = "false"  ></asp:Label>
  
             <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="ProcessUserAcctList" TypeName="iBankPoints.iPoints">
                <SelectParameters>
                         <asp:ControlParameter ControlID="txtUserName" DefaultValue="99" Name="strUserName" PropertyName="Text" Type="String" />
               
                    <asp:ControlParameter ControlID="lblAccountNo" DefaultValue="99" Name="strAccountNo" PropertyName="Text" Type="String" />
                     <asp:CookieParameter CookieName="Token" Name="Token" Type="String" />
                </SelectParameters>
              </asp:ObjectDataSource>
            <asp:ObjectDataSource ID="ObjBenList" runat="server" SelectMethod="ProcessGetBenList" TypeName="iBankPoints.iPoints">
                <SelectParameters>
                      <asp:ControlParameter ControlID="txtUserName" DefaultValue="99" Name="strUserName" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="lblAccountNo" DefaultValue="99" Name="AccountNo" PropertyName="Text" Type="String" />
                    <asp:Parameter DefaultValue="1" Name="ContType"  Type="String" />
                </SelectParameters>
              </asp:ObjectDataSource>
             <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" SelectMethod="ProcessBankList" TypeName="iBankPoints.iPoints">
                <SelectParameters>
                    <asp:CookieParameter CookieName="Token" Name="Token" Type="String" />
                </SelectParameters>
              </asp:ObjectDataSource>
            <asp:ValidationSummary ID="ValSumAlert" runat="server" CssClass="error error-img"
                 ValidationGroup="ChangeUserPasswordValidationGroup" ShowMessageBox="True" HeaderText="iPortal Alert" style="margin-bottom: 19px" />
    <asp:TextBox ID="txtUserName" runat="server" Visible="false"></asp:TextBox>
      
            </div>
                 <!-- JAVASCRIPT -->

       

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
                </div>


           </div>
            
  
    </form>
   
</body>
</html>