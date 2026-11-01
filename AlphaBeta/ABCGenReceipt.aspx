<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ABCGenReceipt.aspx.cs" Inherits="AlphaBeta_ABCGenReceipt" %>

<%@ Register Assembly="DevExpress.Web.v19.1, Version=19.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
 
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Transaction Confirmation</title>
    <%--<meta charset="utf-8" />--%>

		<!-- input image and display -->
		<link class="jsbin" href="http://ajax.googleapis.com/ajax/libs/jqueryui/1/themes/base/jquery-ui.css" rel="stylesheet" type="text/css" />
		<script class="jsbin" src="http://ajax.googleapis.com/ajax/libs/jquery/1/jquery.min.js"></script>
        <script class="jsbin" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.0/jquery-ui.min.js"></script>
        
    
       <%-- <meta name="viewport" content="width=device-width, initial-scale=1.0">
        <meta content="Premium Multipurpose Admin & Dashboard Template" name="description" />
        <meta content="Themesdesign" name="author" />--%>
        <!-- App favicon -->
       	<!-- input image and display -->
		<link class="jsbin" href="http://ajax.googleapis.com/ajax/libs/jqueryui/1/themes/base/jquery-ui.css" rel="stylesheet" type="text/css" />
		<script class="jsbin" src="http://ajax.googleapis.com/ajax/libs/jquery/1/jquery.min.js"></script>
        <script class="jsbin" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.0/jquery-ui.min.js"></script>
        
        
         
            <link href="https://fonts.googleapis.com/css?family=Nunito:400,600,700&display=swap" rel="stylesheet"/>
            <link rel="stylesheet" href= "https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css"/>
    
            <!-- App favicon -->
            <link rel="shortcut icon" href="../mainpage/assets/images/favicon.ico"/>
    
            <link href="https://cdnjs.cloudflare.com/ajax/libs/animate.css/3.5.2/animate.min.css" rel="stylesheet"/>
    
            <!--Chartist Chart CSS -->
           <link href="../Resources/css/hartist.min.css" rel="stylesheet" type="text/css" /> 
            <!-- plugin css -->
            <link href="../Resources/css/jquery-jvectormap-1.2.2.css" rel="stylesheet" type="text/css" /> 
            <!-- DataTables -->
            <link href="../Resources/css/dataTables.bootstrap4.min.css" rel="stylesheet" type="text/css" />
    
            <!-- Bootstrap Css -->
            <link href="../Resources/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
            <!-- Icons Css -->
            <link href="../Resources/css/icons.min.css" rel="stylesheet" type="text/css" />
            <!-- App Css-->
            <link href="../Resources/css/app.min.css"   rel="stylesheet" type="text/css" />
            <link href="../Resources/css/customstyle.css" rel="stylesheet" type="text/css" />
            <link href="../Resources/css/style.css" rel="stylesheet" type="text/css" />
     <link href="../Resources/css/SpecialSelect.css" rel="stylesheet" type="text/css" />

    <script>
   document.querySelector(".check").addEventListener("click", showPass);
   function showPass2() {
       var inputPassEle = document.getElementById("TxtAnswer");
        if (inputPassEle.TextMode === "password") {
            inputPassEle.TextMode = "text";
      } else {
            inputPassEle.TextMode = "password";
      }
    }
        function showPass(e string) {
            var inputPassEle = document.getElementById("inputPass");
            if (inputPassEle.type === "password") {
                inputPassEle.type = "text";
            } else {
                inputPassEle.type = "password";
            }
        }
        function goBack() {
            window.history.back();
        }  

</script>
    <style type="text/css">
        .auto-style1 {
            width: 28px;
            height: 16px;
        }
      .iPointNumberCenterAlign {
            text-align:center;
        }
      .iPointNumberRightAlign {
            text-align:right;
        }
        .iPointNumberLeftAlign {
            text-align:Left;
        }



    </style>
</head>
<body>
    <form id="form1" runat="server">

        <div class="container-fluid" >
              <div class="row" style="margin-top:15px">
                            <div class="col-12">
                            <%--    <div class="page-title-box d-flex align-items-center justify-content-between">
                                    <h4 class="mb-0"> Transaction Confirmation</h4>

                                 <div class="page-title-right">
                                        <ol class="breadcrumb m-0">
                                            <li class="breadcrumb-item"><a href="javascript: void(0);">Menu</a></li>
                                            <li class="breadcrumb-item active">Password Quiz<asp:Label ID="lblAccountNo" runat="server" Visible="False"></asp:Label>
                                            </li>
                                        </ol>
                                    </div>

                                </div>--%>
                            </div>
                        </div>
            <div class="row">
              <div class="col-lg-7">
                    <div class="progress mt-2 progress-style">
                                            <div class="progress-bar progress-animated progress-move-step3  bg-danger" role="progressbar" aria-valuenow="55" aria-valuemin="0" aria-valuemax="100"></div>
                                        </div> 

                  
              <div class="card Cusanimation card-body "   >
                           <div class="d-flex align-items-center justify-content-between">
                                    <h5 class="mb-0"> Transaction Confirmation</h5>
 

                                </div>
                          <div class="recDetails">
                                <asp:DetailsView ID="DetailsView1" runat="server" Height="50px" Width="100%"   CellPadding="4" DataSourceID="ObjectDataSource1" ForeColor="#333333" GridLines="Horizontal" BorderStyle="None" >
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    <CommandRowStyle BackColor="#E2DED6" Font-Bold="True" />
                                    <EditRowStyle BackColor="#999999" />
                                    <FieldHeaderStyle BackColor="#E9ECF1" Font-Bold="True" />
                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                </asp:DetailsView>
                           </div>
                           </div>
                  </div>  
                <div class="col-lg-4"> 
                    <div   class="successArea" >
                        <div>
                    <img src="../mainpage/assets/images/checkMark.png"  class="successMark"/></div>
                    <h6 > <asp:Label runat="server" ID="lBlPayRefNo" ForeColor="Green" ></asp:Label><br/><br/><i class="fas fa-print"></i> <small><asp:HyperLink runat="server" ID="hypReceipt" >Print Revenue Receipt</asp:HyperLink></small></h6> 
                    <%--<h6 >Transaction Payment Reference: <i class="fas fa-print"></i> <small><asp:HyperLink runat="server" ID="HyperLink1" >Print</asp:HyperLink></small></h6>--%> 

                <div><asp:Literal ID="litEmmorMsg" runat="server" ></asp:Literal></div>
        

               <div>
              
                        <div>
                                    <div  class="transferArea">
                             <div   class="form-group row">
                                           <%--<asp:Button runat="server" Text="Generate Receipts"  ID="btnReceipts"   CssClass="btn btn-primary ml-2" Enabled="false" OnClick="btnReceipts_Click" ></asp:Button>--%>
                            <%--<div class="col-4 text-right; align-text-bottom"  > <i class="fas fa-print"><asp:HyperLink runat="server" ID="hypReceipt" >Print</asp:HyperLink></i></div> </div>--%>                   
                                    

                        <%--       <button type="submit" class="btn btn-primary ml-2 smbtnsize" ><i class="mdi mdi-autorenew"></i> New... </button>
                           <button type="submit" class="btn btn-primary ml-2 smbtnsize" ><i class="mdi mdi-home-thermometer-outline"></i> Home</button>
                             <button type="submit" class="btn btn-primary ml-2 smbtnsize"><i class="mdi mdi-bank-transfer"></i> Transfer</button>
                                                                          
                                 --%>
                             </div>
                        </div>


                </div>

                </div>

                </div>
            </div> 
                
  <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" SelectMethod="ProcessAphaBetaValWebGuidRepDs" TypeName="iBankPoints.iPoints">
                <SelectParameters>
                    <asp:QueryStringParameter Name="WebGuid" QueryStringField="q" Type="String" />
                     <asp:CookieParameter CookieName="Token" Name="Token" Type="String" />
                     <asp:Parameter DefaultValue="WebGuid" Name="type" Type="String" />
                </SelectParameters>
              </asp:ObjectDataSource>
      <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="ProcessGetTempTrans" TypeName="iBankPoints.iPoints">
                <SelectParameters>
                    <asp:QueryStringParameter Name="TransRefSeq" QueryStringField="q" Type="String" />
                    <asp:CookieParameter CookieName="eLogonUserID" Name="UserName" Type="String" />
                    <asp:Parameter DefaultValue="1" Name="FormatType" Type="String" />
                    <asp:CookieParameter CookieName="Token" Name="Token" Type="String" />
                </SelectParameters>
              </asp:ObjectDataSource>
        </div>
    
    
    </form>
</body>
</html>
