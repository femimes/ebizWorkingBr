<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DemoDevXpress.aspx.cs" Inherits="eOperations_DemoDevXpress" %>

<%@ Register assembly="DevExpress.XtraReports.v19.1.Web.WebForms, Version=19.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraReports.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v19.1, Version=19.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <dx:ASPxButton ID="ASPxButton1" runat="server" onclick="ASPxButton1_Click" 
            Text="ASPxButton">
        </dx:ASPxButton>
    
    </div>
    <dx:ReportViewer ID="ReportViewer1" runat="server">
    </dx:ReportViewer>
    </form>
</body>
</html>
