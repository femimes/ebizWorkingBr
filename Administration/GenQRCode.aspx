<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GenQRCode.aspx.cs" Inherits="Administration_GenQRCode" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head  >
      <title>QRCode</title> 
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
        <p>
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Generate QRCode" />
        </p>
        <p>
            Url:&nbsp;&nbsp;
            <asp:TextBox ID="txtUrl" runat="server" Width="466px"></asp:TextBox>
        </p>
        <p>
            Title:
            <asp:TextBox ID="txtTitle" runat="server" Width="466px"></asp:TextBox>
        </p>
        <div>
            <div style="margin:5%">
                <asp:Image ID="imgqrcode" runat="server" CssClass="mw-100 mdi-margin" Height="573px" Width="689px" />
            </div>
        </div>
    </form>
</body>
</html>
