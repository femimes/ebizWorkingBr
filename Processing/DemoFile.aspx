<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DemoFile.aspx.cs" Inherits="Processing_DemoFile" %>

<%@ Register Assembly="DevExpress.Web.v19.1, Version=19.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

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
            <style type="text/css">
                .auto-style1 {
                    position: relative;
                    width: 100%;
                    -webkit-box-flex: 0;
                    -ms-flex: 0 0 41.66667%;
                    flex: 0 0 41.66667%;
                    max-width: 41.66667%;
                    left: -124px;
                    top: 0px;
                    padding-left: 12px;
                    padding-right: 12px;
                }
            </style>
        </head>
<body>
    <form id="form1" runat="server">
        <div>
            <dx:ASPxTextBox ID="ASPxTextBox1" runat="server" Width="170px"></dx:ASPxTextBox>
        </div>
    </form>
</body>
</html>
