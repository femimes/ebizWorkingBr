<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CardActivityAuth.aspx.cs" Inherits="Request_CardActivityAuth" %>

<%@ Register Assembly="DevExpress.Web.v19.1, Version=19.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>






<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Data" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <%--<meta charset="utf-8" />--%>
   <meta charset="utf-8" />
            <title>eBizPlus</title>
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
            <link rel="shortcut icon" href="../Resources/images/favicon.ico"/>
    
            <link href="https://cdnjs.cloudflare.com/ajax/libs/animate.css/3.5.2/animate.min.css" rel="stylesheet"/>
    
            <!--Chartist Chart CSS -->
            <link href="../Resources/libs/chartist/chartist.min.css" rel="stylesheet" type="text/css" /> 
            <!-- plugin css -->
            <link href="../Resources/libs/admin-resources/jquery.vectormap/jquery-jvectormap-1.2.2.css" rel="stylesheet" type="text/css" /> 
            <!-- DataTables -->
            <link href="../Resources/libs/datatables.net-bs4/css/dataTables.bootstrap4.min.css" rel="stylesheet" type="text/css" />
    
            <!-- Bootstrap Css -->
            <link href="../Resources/css/bootstrap.min.css"   rel="stylesheet" type="text/css" />
            <!-- Icons Css -->
            <link href="../Resources/css/icons.min.css" rel="stylesheet" type="text/css" />
            <!-- App Css-->
            <link href="../Resources/css/app.min.css"  rel="stylesheet" type="text/css" />
            <link href="../Resources/css/customstyle.css" rel="stylesheet" type="text/css" />
            <%--<link href="../Resources/css/style.css" rel="stylesheet" type="text/css" />--%>
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

                                <%--    <h4 class="mb-0">Verify Transaction</h4>--%>
                                   
                                    <%--<div class="page-title-right">
                                        <ol class="breadcrumb m-0">
                                            <li class="breadcrumb-item"><a href="javascript: void(0);">Menu</a></li>
                                            <li class="breadcrumb-item active">Password Quiz<asp:Label ID="lblAccountNo" runat="server" Visible="False"></asp:Label>
                                            </li>
                                        </ol>
                                    </div>--%>

                               
              <%--<div   class="card-title" >Authentication Required for Change Password</div>--%>

              <div class="Cusanimation card-body align-content-center"  >
                  <div class="row">
  
                    <div class="col-lg-5 col-12">
                    <div class="progress mt-2 progress-style" style="margin-top: 40px!important; max-width:60%" >
                            <div class="progress-bar progress-animated progress-move-step2  bg-danger" role="progressbar" aria-valuenow="55" aria-valuemin="0" aria-valuemax="70"></div>
                        </div>
                        <div class="recDetails">
                                
                            <%

                                DataTable dt = new DataTable();
                                string strsel = string.Format("SpGetBatRecCardMaintenance '{0}'", Request.QueryString["RefNo"]);
                                dt = NETClass.Classes.GetDataset(strsel).Tables[0];
                                int i = dt.Columns.Count;

                                //foreach (DataRow item in dt.Rows)
                                for (int j = 0; j < dt.Columns.Count; j++)


                                {


                                %>
                            <div>                                         
                            <h6> <%= dt.Columns[j].ColumnName %></h6>

                            </div>
                            <div>
                                       
                                <span style="color:blue"> <%= dt.Rows[0][j] %></span>
                            <hr />
                            </div>
                            <%
                            }
                            %>

                    <div class="custom-control custom-switch mb-2 page-title-right" dir="ltr"   >
                    <input type="checkbox" class="custom-control-input" id="customSwitch1"   onchange="LoadBeneficiaryList()"  name="customSwitch1" runat="server"  />
                   
                    <label class="custom-control-label" for="customSwitch1">Save Beneficiary</label>
 
                    </div>
                     <div class="card-footer transfoot"> <asp:Label  ID="lblwebRefNo" runat="server"></asp:Label></div>
                </div>

                </div>   
                           
                           
             
                     
                <div class="col-lg-2">

                </div>
                     
                  <div class="col-lg-4 col-12"> 
                      <div class="col-12 padgap" style="text-align:center"> 
                          <h4 class="mb-2 cuscardtitle">Verify Maintenance</h4>
                      </div>
                      <div class="padbody">

                         <div class="padamt"> 
                             <h3> <asp:Label ID="lblAmount" runat="server" CssClass="mb-0"></asp:Label> </h3>

                         </div>
                      
                      <div class="align-content-center  payment-card" >
                      <div>
                        <div style="text-align:center;"><span id="errDisp" class="error-img error iPointNumberCenterAlign"> <asp:Literal ID="litEmmorMsg" runat="server" ></asp:Literal></span></div>

                      </div>
                         
                          <div class="pinheight"  >
                        <p class="pinheight" ></p>
                    </div>
                          <div class="row " style="margin: 20px 30px 20px 10px;"> 
                              <div class="row col-10 padinput " style="max-height:40px; margin-left: 13%;">
                               <div class="col-10" style="width:70%">
                                <asp:TextBox runat="server" id="psword" AutoCompleteType="Disabled" ToolTip="Enter Pin" TextMode="Password" MaxLength="8" CssClass="form-control noborder" onkeypress="return true;" autocomplete="off"   placeholder="Enter Token"></asp:TextBox>
                                      <div><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage=" " ControlToValidate="psword"  Display="Dynamic"></asp:RequiredFieldValidator></div>
                                </div>
                              <div class="col-2" style="padding-top: 7px;">
                                  <i class="mdi mdi-eye-off" id="myEyeOff"  style="color: #002668; float:right"  for="plainPsWord" onclick ="changePasswordWithEye()" ></i>
                                  <i class="mdi mdi-eye" id="myEye" style="color: #002668; float:right; display: none;" for="plainPsWord"  onclick ="changePasswordWithEye()"></i>
                              </div>
                              </div>
                           

                              <%-- <td colspan="6"><input type="checkbox" id="plainPsWord" onChange="displayPassword()"><label
                                for="plainPsWord" class="check form-control-sm">Display PIN</label>
                        </td>--%>
                            
                          </div>

                                        
                          <div>

                              <div id="container" class="keyboardstyle padarea">
                
                                <div   class="btnTable border-0"  >
              
                                    <div class="padmedia "  style="margin-left:10%">
                                      <%--  <td></td>--%>
                                        <td class="auto-style1"><input type="button" id="num0" class="btn padbtn" value="1"
                                                onclick="showClickedButton(this.value);" /></td>
                                        <td><input type="button" id="num1" class="btn padbtn" value="2"
                                                onClick="showClickedButton(this.value);" /></td>
                                        <td><input type="button" id="num2" class="btn padbtn" value="3"
                                                onClick="showClickedButton(this.value);" /></td>
                                        <td><input type="button" id="num3" class="btn padbtn" value="4"
                                                onClick="showClickedButton(this.value);" /></td>
                                        <td><input type="button" id="num4" class="btn padbtn" value="5"
                                                onClick="showClickedButton(this.value);" /></td>
                                        <td><input type="button" id="Clr" class="btnDC keyClr padAct" value="CLR" onclick="clearPassword()" /></td>
                                        <td rowspan="2">&nbsp;</td>
                                    </div>
                                    <div class="padmedia" style="margin-left:10%">
                                        <%--<td></td>--%>
                                        <td class="auto-style1"><input type="button" id="num5" class="btn padbtn" value="6"
                                                onClick="showClickedButton(this.value);" /></td>
                                        <td><input type="button" id="num6" class="btn padbtn" value="7"
                                                onClick="showClickedButton(this.value);" /></td>
                                        <td><input type="button" id="num7" class="btn padbtn" value="8"
                                                onClick="showClickedButton(this.value);" /></td>
                                        <td><input type="button" id="num8" class="btn padbtn" value="9"
                                                onClick="showClickedButton(this.value);" /></td>
                                        <td><input type="button" id="num9" class="btn padbtn" value="0"
                                                onClick="showClickedButton(this.value);" /></td>
                                        <td><input type="button" id="Del" class="btnDC keyClr padAct" value="DEL" onClick="deleteChar()" /></td>
                                    </div>


                                  <%--  <tr>
                                        <td></td>
                                        <td colspan="7"><a href="#" target="_blank" class="link">Forgot your password?</a></td>
                                    </tr>--%>
                                        <%--<td></td>--%>

                              </div>
                            </div>
                           <div class="row text-center dispass col-12">
                        <%--<td></td>--%>
                            <div class="col-6">
                                <i class="mdi mdi-refresh hyperaction" id=""  style="color: #002668;"  for="plainPsWord" onclick ="init()" ></i>
                            </div>
                                <div class="col-6">
                                <p><a href="#" onClick="" class="link refbtn hyperaction">Resend OTP</a></p>
                            </div>
                           
                          </div>
                    <div class="row" >
                        <div>
                            <div  >
                            
             
                            
                <%--            <asp:Button ID="btnConfirm" runat="server" CausesValidation="True"  class="btn  btn-success   btn-rounded btn-sm" 
                      style="font-size: 13px;margin-left: 20px;margin-top: -5px;position:absolute"     Text="Confirm" OnClick="btnConfirm_Click"  /></div></td>
                           
                            --%>
                        <dx:ASPxCallback ID="ASPxCallback1" runat="server" ClientInstanceName="Callback">
        <ClientSideEvents CallbackComplete="function(s, e) { LoadingPanel.Hide(); }" />
    </dx:ASPxCallback>
    
     <div id="Panel"   style="max-width:15%; border: none 1px Gray; padding: 5px 0;">
        <div style="display:table" class="horizontal-center-aligned">
            
            <dx:ASPxButton ID="btnConfirm" runat="server"   Text="Confirm" AutoPostBack="False" CausesValidation="True"   OnClick="btnConfirm_Click"  
                class="btn  btn-success btn-rounded btn-sm" CssClass="crmbtn"
                >
                <ClientSideEvents Click="function(s, e) {
                        Callback.PerformCallback();
                        LoadingPanel.Show();
                    }" />
            </dx:ASPxButton>
        </div>
    </div>

            <dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" Text="Please Wait.." 
        Modal="True" LoadingDivStyle-Cursor="crosshair">
    </dx:ASPxLoadingPanel>
                        
                        </div>
                          </div>

                        </div>

                          </div>

                      </div>

                     
                       </div>
                      </div>

                  </div>
                      <script type="text/javascript">
                          var c = document.getElementById("<%=psword.ClientID %>");
                          c.select = function (event, ui)
                              { this.value = ""; return false; }
    </script>

                     <%--<span id="errDisp" class="error-img error"> <asp:Literal ID="litEmmorMsg" runat="server" ></asp:Literal></span>--%>
                      </div></div>
            
                  </div>


          <script src="../Resources/js/NumberPad.js"></script>
            <div> <asp:Label ID="lblUser" runat="server" visible="false"  ></asp:Label></div>
                                    
            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="ProcessGetTempTrans" TypeName="iBankPoints.iPoints">
                <SelectParameters>
                    <asp:QueryStringParameter Name="TransRefSeq" QueryStringField="RefNo" Type="String" />
                      <asp:ControlParameter ControlID="lblUser" DefaultValue="99" Name="UserName" PropertyName="Text" Type="String" />
            
                    <asp:CookieParameter CookieName="Token" Name="Token" Type="String" />
                    <asp:Parameter DefaultValue="1" Name="FormatType" Type="String" />
                </SelectParameters>
              </asp:ObjectDataSource>
        </div>
                                  
                       
    </form>
     <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
            <script src="../preloader.js"></script>
</body>
</html>
