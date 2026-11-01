<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ABConfirmation.aspx.cs" Inherits="AlphaBeta_ABConfirmation" %>

<%@ Register Assembly="DevExpress.Web.v19.1, Version=19.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
 
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <%--<meta charset="utf-8" />--%>
   <meta charset="utf-8" />
            <title>ABBEY - Mortgage Bank</title>
            <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
            <meta content="Premium Multipurpose Admin & Dashboard Template" name="description" />
            <meta content="Themesbrand" name="author" />
    
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
            <link href="../Resources/css/bootstrap.min.css"   rel="stylesheet" type="text/css" />
            <!-- Icons Css -->
            <link href="../Resources/css/icons.min.css" rel="stylesheet" type="text/css" />
            <!-- App Css-->
            <link href="../Resources/css/app.min.css" rel="stylesheet" type="text/css" />
            <link href="../Resources/css/customstyle.css" rel="stylesheet" type="text/css" />
            <link href="../Resources/css/style.css" rel="stylesheet" type="text/css" />
     <link href="../Resources/css/SpecialSelect.css" rel="stylesheet" type="text/css" />
                      <link href="../style.css" rel="stylesheet"/>
    

         
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
    <form id="form1" runat="server"  >

        <div class="container-fluid " >
              <div class="row">
                            <div class="col-12">
                             
                                    <div class="progress mt-2 progress-style" style="margin-top: 40px!important" >
                                            <div class="progress-bar progress-animated progress-move-step2  bg-danger" role="progressbar" aria-valuenow="55" aria-valuemin="0" aria-valuemax="100"></div>
                                        </div>
                                <%--    <h4 class="mb-0">Verify Transaction</h4>--%>
                                   
                                    <%--<div class="page-title-right">
                                        <ol class="breadcrumb m-0">
                                            <li class="breadcrumb-item"><a href="javascript: void(0);">Menu</a></li>
                                            <li class="breadcrumb-item active">Password Quiz<asp:Label ID="lblAccountNo" runat="server" Visible="False"></asp:Label>
                                            </li>
                                        </ol>
                                    </div>--%>

                               
              <%--<div   class="card-title" >Authentication Required for Change Password</div>--%>
              <div class="card Cusanimation card-body align-content-center"  >
                  <div><div class="row hv"> <h4 class="mb-0" style="font-size:16px">Verify Transaction</h4></div></div>
               
                  <div class="row">
            
                            
                              <div class="col-lg-6">
                                  <div class="recDetails">
                                <asp:DetailsView ID="DetailsView1" runat="server" Height="50px" Width="100%" CssClass="table"   CellPadding="4" DataSourceID="ObjectDataSource1" ForeColor="#333333" GridLines="Horizontal" BorderStyle="None" >
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
                           
                           
                            
                     

                     
                           <div class="col-lg-6"> 
                      <div style="margin-left:26%"> <h3> <asp:Label ID="lblAmount" runat="server" CssClass="mb-0"></asp:Label> </h3></div>
                      <div class="align-content-center  payment-card" >
                      <div>
                        <div style="text-align:center;"><span id="errDisp" class="error-img error iPointNumberCenterAlign"> <asp:Literal ID="litEmmorMsg" runat="server" ></asp:Literal></span></div>

                      </div>
                         
                          <div class="pinheight"  >
                        <p class="pinheight" ></p>
                    </div>
                          <div class="row " style="margin-left:20px">
                             <p class="text-xl-right align-content-between text-truncate font-size-16 mb-2 pintext" style="margin-right:5px">PIN:</p> 
                        
                      <div>
                        <asp:TextBox runat="server" id="psword" AutoCompleteType="Disabled" ToolTip="Enter Pin" TextMode="Password" MaxLength="5" CssClass="form-control"></asp:TextBox>
                              <div><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="PIN Required !!" ControlToValidate="psword"></asp:RequiredFieldValidator></div>
                        </div>
                      </div>

                                        
                          <div>

                              <div id="container" style="margin-left:30px">


            
                
                <div   class="btnTable border-0">
              

                    
                    <div style="width:230px">
                      <%--  <td></td>--%>
                        <td class="auto-style1"><input type="button" id="num0" class="btn" value="1"
                                onclick="showClickedButton(this.value);" /></td>
                        <td><input type="button" id="num1" class="btn" value="2"
                                onClick="showClickedButton(this.value);" /></td>
                        <td><input type="button" id="num2" class="btn" value="3"
                                onClick="showClickedButton(this.value);" /></td>
                        <td><input type="button" id="num3" class="btn" value="4"
                                onClick="showClickedButton(this.value);" /></td>
                        <td><input type="button" id="num4" class="btn" value="5"
                                onClick="showClickedButton(this.value);" /></td>
                        <td><input type="button" id="Clr" class="btnDC keyClr" value="CLR" onclick="clearPassword()" /></td>
                        <td rowspan="2">&nbsp;</td>
                    </div>
                    <div style="width:230px">
                        <%--<td></td>--%>
                        <td class="auto-style1"><input type="button" id="num5" class="btn" value="6"
                                onClick="showClickedButton(this.value);" /></td>
                        <td><input type="button" id="num6" class="btn" value="7"
                                onClick="showClickedButton(this.value);" /></td>
                        <td><input type="button" id="num7" class="btn" value="8"
                                onClick="showClickedButton(this.value);" /></td>
                        <td><input type="button" id="num8" class="btn" value="9"
                                onClick="showClickedButton(this.value);" /></td>
                        <td><input type="button" id="num9" class="btn" value="0"
                                onClick="showClickedButton(this.value);" /></td>
                        <td><input type="button" id="Del" class="btnDC keyClr" value="DEL" onClick="deleteChar()" /></td>
                    </div>
                    <div class="dispass">
                        <%--<td></td>--%>
                        <td colspan="7"><input type="checkbox" id="plainPsWord" onChange="displayPassword()"><label
                                for="plainPsWord" class="check form-control-sm">Display password</label>

                        </td>
                    </div>
                  <%--  <tr>
                        <td></td>
                        <td colspan="7"><a href="#" target="_blank" class="link">Forgot your password?</a></td>
                    </tr>--%>
                    <p>
                        <%--<td></td>--%>
                        <div class="row" style="margin-left: 23px;margin-top: -10px;">
                        <td colspan="6"><a href="#btnTable" onClick="init()" class="link">Refresh keypad</a></td>
                        <td> <div><asp:Button ID="btnConfirm" runat="server" CausesValidation="True"  class="btn  btn-success   btn-rounded btn-sm" 
                      style="font-size: 13px;margin-left: 20px;margin-top: -5px;"   Text="Confirm" OnClick="btnConfirm_Click"  /></div></td>
                            </div>
                    </p>
                </div>

          




        </div>
                          </div>

                      </div>

                     
                       
                      </div>

                  </div>
               <div class="card-footer"> <asp:Label  ID="lblwebRefNo" runat="server"></asp:Label></div>
                      <script type="text/javascript">
                          var c = document.getElementById("<%=psword.ClientID %>");
                          c.select =
                              function (event, ui)
                              { this.value = ""; return false; }
    </script>

                     <%--<span id="errDisp" class="error-img error"> <asp:Literal ID="litEmmorMsg" runat="server" ></asp:Literal></span>--%>
                      </div></div>
            
                  </div>


          <script src="../mainpage/assets/js/NumberPad.js"></script>
            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="ProcessAphaBetaValWebGuidDs" TypeName="iBankPoints.iPoints">
                <SelectParameters>
                    <asp:QueryStringParameter Name="WebGuid" QueryStringField="RefNo" Type="String" />
                     <asp:CookieParameter CookieName="Token" Name="Token" Type="String" />
                      <asp:QueryStringParameter Name="type" QueryStringField="r" Type="String" />
                   
                </SelectParameters>
              </asp:ObjectDataSource>
        </div>
                                  
                       
    </form>
</body>
</html>
