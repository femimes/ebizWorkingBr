<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CardRequestInstant.aspx.cs" Inherits="Request_CardRequestInstant" %>
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
                                    <h4 class="mb-0 font-size-18">Instant Request</h4>

                                    <div class="page-title-right">
                                        <ol class="breadcrumb m-0">
                                            <li class="breadcrumb-item"><a href="javascript: void(0);">Menu</a></li>
                                            <li class="breadcrumb-item"><a href="javascript: void(0);">Request</a></li>
                                            <li class="breadcrumb-item active">Instant Request<asp:Label ID="lblAccountNo" runat="server" Visible="False"></asp:Label></li>
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
         Request Title:  <asp:Textbox  ID="txtTitle" runat="server" CssClass="form-control" required   /> 

           </div>
          <div class="form-group row">
         Card Type:  <asp:DropDownList  ID="drpCardType" runat="server" CssClass="form-control" DataSourceID="SqlDataSource1"   DataValueField="CardTypeCode"  DataTextField="CardTypeDesc"  required   /> 

           </div>
        <div class="form-group row">
        CardDummy Count:  <asp:Textbox  ID="TxtdummyCount" runat="server" CssClass="form-control" Type="Number"   Text ="0"  required /> 

           </div>
                                                    <div class="form-group row">
                                                 Transaction Remarks:  <asp:Textbox  ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"  required  /> 

                                                    </div>

                                 <div class="form-group row" >
                                     <p style="align-items:center">
                                                  <asp:Button ID="Button3" runat="server" CssClass="btn btn-primary ml-2" 
            Text="Submit" OnClick="Button3_Click"     /> &nbsp;
            <asp:Button ID="Button4" runat="server" CausesValidation="False"   Text="Reset"  CssClass="btn btn-primary ml-2" OnClick="Button4_Click" style="width: 59px" /> 
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
                                          <%--  <p> <asp:checkbox runat="server" ID="ChkCardType" Text="Print CurrentCard? .." TextAlign="Left"></asp:checkbox></p>--%>
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
     <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:EazyConnect %>" 
        SelectCommand="SpGetInstCardRequestType" SelectCommandType="StoredProcedure">
       
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
