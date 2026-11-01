<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BulkFTLocal.aspx.cs" Inherits="Bulk_BulkFTLocal" %>
<%@ Register Assembly="DevExpress.Web.v19.1, Version=19.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>


 
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

    <body data-topbar="dark">
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
 <form id="form1" runat="server" >

              <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
      
      
       
       

        <!-- Begin page -->
        <div id="layout-wrapper">
             
        <div class="container-fluid">

                        <!-- start page title -->
                        <div class="row">
                            <div class="col-md-12">
                                <div class="page-title-box d-flex align-items-center justify-content-between">
                                    <h4 class="mb-0 font-size-18">Bulk Funds Transfers </h4>

                                    <div class="page-title-right">
                                        <ol class="breadcrumb m-0">
                                            <li class="breadcrumb-item">
                                              <asp:HyperLink   ID="hypdashboard" style="color:#4d96ff!important" CssClass="customlink" runat="server">Dashboard</asp:HyperLink>
                                            </li>  
                                            <li class="breadcrumb-item"><a >Bulk Transfers</a></li>
                                            <li class="breadcrumb-item active">IntraBank Account Transfers<asp:Label ID="lblAccountNo" runat="server" Visible="False"></asp:Label></li>
                                        </ol>
                                    </div>

                                </div>
                            </div>
                        </div>
                        <!-- end page title -->
                               <asp:Label ID="errMsg" runat="server" CssClass="error"></asp:Label>
                        <div class="row">
                            <div class="col-md-8">
                                <div class="card cuscard">
                                          <div class="cuscardheader">
                                       <h4 class="mb-0 cuscardtitle">Bulk Transfers (IntraBank Accounts Upload)
                                        <p class="cusactive">A
                                            <p class="cusdisable">D</p>
                                        </p>
                                       </h4>
                                     </div>
                                    <div class="card-body" > 
                                         <div class="row">
                                            <div class="col-md-12 ">
                                                  <div class="form-group row">
                                                     <div class="col-md-8">
                                                   <asp:FileUpload ID="Filphoto" runat="server" CssClass="form-control" Width="100%"  />

                                                                 </div> 
                                                      <div class="text-right"> 

                                                       <asp:Button ID="btnImport" runat="server" CssClass="btn btn-primary ml-2" Text="Import" OnClick="btnImport_Click" />

                                                      </div>
                                                      </div>
                                                   
                                                    <div class="form-group row">
                                                 Transaction Remarks: 

                                                    </div>
                                                         <div class="form-group row "  > 
                                                             <div class="col-md-8">
                                                  <asp:Textbox  ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"  Width="100%"  />  
                                                            </div>
                                                    </div>

                                 <div class="form-group row"  style="margin-left:15%">
                                                  <asp:Button ID="Button3" runat="server" CssClass="btn btn-primary ml-2" 
            Text="Commit Records" OnClick="Button3_Click"     /> &nbsp;
            <asp:Button ID="Button4" runat="server" CausesValidation="False"   Text="New Records"  CssClass="btn btn-primary ml-2" OnClick="Button4_Click" />&nbsp;
            
                                                    </div>
                                                    <div class="form-group row">
<asp:DataGrid ID="DataGrid1" runat="server" BorderColor="#CCCCCC" BorderStyle="None"
                    BorderWidth="1px" Caption="Import Details" CaptionAlign="Top" CellPadding="3"
                    Font-Bold="False" Width="100%">
                    <FooterStyle BackColor="White" ForeColor="#000066" />
                    <SelectedItemStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" Mode="NumericPages" />
                    <ItemStyle ForeColor="#000066" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Size="XX-Small" Font-Strikeout="False" Font-Underline="False" />
                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" Wrap="False" Font-Size="X-Small" />
                    <AlternatingItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False"
                        Font-Size="XX-Small" Font-Strikeout="False" Font-Underline="False" />
                </asp:DataGrid>
                                                        </div>
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
                                        <div class="form-group row">
                                          
                                     <dx:ASPxComboBox ID="ASPxComboBox1"    runat="server" DataSourceID="ObjectDataSource1" Width="100%"   TextFormatString="{0}; {1}"    ValueField="AccountNo" CssClass="form-control"
                                         OnSelectedIndexChanged="ASPxComboBox1_SelectedIndexChanged" AutoPostBack="True" Border-BorderColor="AliceBlue" >
                                     <Columns >
                                   
                                        <dx:ListBoxColumn Caption="AccountNo" FieldName="AccountNo"  />
                                        <dx:ListBoxColumn Caption="Type" FieldName="ProdName" />
                                         <dx:ListBoxColumn Caption="AccountName" FieldName="AccountDesc"  Visible="false"/>
                                    </Columns>
                                                          
                                   <ItemStyle  />
                                   
                                   <Paddings Padding="0px" />
                                   <ValidationSettings Display="Dynamic">
                                       <RequiredField IsRequired="true" ErrorText="select account type" />
                                   </ValidationSettings>
                      </dx:ASPxComboBox>
                                           
                                </div>

                                      <div   class="form-group row"><asp:Label ID ="txtref_no" runat="server" ForeColor="blue"></asp:Label> </div>
                                         <%-- <MaskSettings Mask="############.##" ShowHints="false" PromptChar=" " IncludeLiterals="DecimalSymbol"  />--%>
                                      
                                        <div   class="form-group row">Total Debit : <asp:Label ID ="lbldebit" runat="server" ForeColor="Red"></asp:Label></div>
                                         
                                        
                                        <%--<button type="submit" class="btn btn-primary ml-2">Clear</button>--%> 
                                        
                                        <div class="form-group row">Available Balance : <asp:Label ID ="lblAvailBal" runat="server" ForeColor="blue"></asp:Label></div>
                 
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
                                </div>

                                
                            </div>
                            </div>   
     <asp:Label ID="lblUser" runat="server" visible="false"  ></asp:Label>
                                    
            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="ProcessUserAcctList" TypeName="iBankPoints.iPoints">
                <SelectParameters>
                    <%--<asp:CookieParameter CookieName="eLonginUser" Name="strUserName" Type="String" />--%>
                    
                    <asp:ControlParameter ControlID="lblUser" DefaultValue="99" Name="strUserName" PropertyName="Text" Type="String" />
              
                    <asp:ControlParameter ControlID="lblAccountNo" DefaultValue="99" Name="strAccountNo" PropertyName="Text" Type="String" />
                     <asp:CookieParameter CookieName="Token" Name="Token" Type="String" />
                </SelectParameters>
              </asp:ObjectDataSource>
            </div>
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
