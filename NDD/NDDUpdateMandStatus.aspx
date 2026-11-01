<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NDDUpdateMandStatus.aspx.cs" Inherits="NDD_NDDUpdateMandStatus" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
         
             <title>Mandate Status Update</title> 
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
 
  
            </head>
<body>
    <form id="form1" runat="server">
        <div style="margin:5%">
            <asp:DataList ID="DataList1" runat="server" DataSourceID="SqlDataSource1" CssClass="grid-container"  >
                <ItemTemplate>
                   <div class="card" style="width:100%"> 
                    <div class="container-fluid">
                   <p  class="title h3"> NDD Customer Details</p>
                       
                    SeqNo:
                    <asp:Label ID="SeqNoLabel" runat="server" Text='<%# Eval("SeqNo") %>' />
                    <hr />
                   
                    account Number:
                    <asp:Label ID="accountNumberLabel" runat="server" Text='<%# Eval("accountNumber") %>' />
                    <hr />
                    account Name:
                    <asp:Label ID="accountNameLabel" runat="server" Text='<%# Eval("accountName") %>' />
                    <hr />
                    
                    payer Name:
                    <asp:Label ID="payerNameLabel" runat="server" Text='<%# Eval("payerName") %>' />
                    <hr />
                    payer BankCode:
                    <asp:Label ID="payerBankCodeLabel" runat="server" Text='<%# Eval("payerBankCode") %>' />
                    <hr />
                    payer AccountName:
                    <asp:Label ID="payerAccountNameLabel" runat="server" Text='<%# Eval("payerAccountName") %>' />
                    <hr />
                    payer AccountNumber:
                    <asp:Label ID="payerAccountNumberLabel" runat="server" Text='<%# Eval("payerAccountNumber") %>' />
                    <hr />
                    payer PhoneNumber:
                    <asp:Label ID="payerPhoneNumberLabel" runat="server" Text='<%# Eval("payerPhoneNumber") %>' />
                    <hr />
                    payer Email:
                    <asp:Label ID="payerEmailLabel" runat="server" Text='<%# Eval("payerEmail") %>' />
                    <hr />
                    payer Address:
                    <asp:Label ID="payerAddressLabel" runat="server" Text='<%# Eval("payerAddress") %>' />
                    <hr />
                    narration:
                    <asp:Label ID="narrationLabel" runat="server" Text='<%# Eval("narration") %>' />
                   <hr />
                     
                    subscriber Code:
                    <asp:Label ID="subscriberCodeLabel" runat="server" Text='<%# Eval("subscriberCode") %>' />
                    <hr />
                    amount:
                    <asp:Label ID="amountLabel" runat="server" Text='<%# Eval("amount") %>' />
                    <hr />
                    frequency:
                    <asp:Label ID="frequencyLabel" runat="server" Text='<%# Eval("frequency") %>' />
                    <hr />
                    start Date:
                    <asp:Label ID="startDateLabel" runat="server" Text='<%# Eval("startDate") %>' />
                    <hr />
                    end Date:
                    <asp:Label ID="endDateLabel" runat="server" Text='<%# Eval("endDate") %>' />
                    <hr />
                     
                    Creation Datetime:
                    <asp:Label ID="systemdatetimeLabel" runat="server" Text='<%# Eval("systemdatetime") %>' />
                    <hr />
                    added By:
                    <asp:Label ID="addedbyLabel" runat="server" Text='<%# Eval("addedby") %>' />
                        <hr />
                    
                    approved By:
                    <asp:Label ID="approvedbyLabel" runat="server" Text='<%# Eval("approvedby") %>' />
                   <hr />
                     
                    NDDResponsemsg:
                    <asp:Label ID="NDDResponsemsgLabel" runat="server" Text='<%# Eval("NDDResponsemsg") %>' />
                    <hr />
                    mandateCode:
                    <asp:Label ID="mandateCodeLabel" runat="server" Text='<%# Eval("mandateCode") %>' />
                    <hr />
                    LastUpdated:
                    <asp:Label ID="NDDMandateLastUpdateLabel" runat="server" Text='<%# Eval("NDDMandateLastUpdate") %>' />
                    <hr />
                     SessionID:
                    <asp:Label ID="NDDMandateLastSessionIDLabel" runat="server" Text='<%# Eval("NDDMandateLastSessionID") %>' />
                    <hr />
                    
                    NextUpdate:
                    <asp:Label ID="NDDMandateNextUpdateLabel" runat="server"  Text='<%#  Eval("NDDMandateNextUpdate") %>'    />
                    <hr />
                </div>
                     </div>
                </ItemTemplate>
            </asp:DataList>
        <div>
            <asp:Button ID="btnMandstatus" runat="server" Text="" CssClass="btn-default btn-primary" OnClick="btnMandstatus_Click"/>
        </div>
               <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
         ConnectionString="<%$ ConnectionStrings:EazyConnect %>" 
        SelectCommand="SpGetNDDeMandateOnly" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:QueryStringParameter Name="mandateCode" QueryStringField="p" DefaultValue="0" />
             
            
            </SelectParameters>

                        

    </asp:SqlDataSource>
        </div>
    </form>
</body>
</html>
