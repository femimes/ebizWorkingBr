<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BulkEMailTemp.aspx.cs" Inherits="eOperations_BulkEMailTemp" Async="true"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div><div><strong>Bulk Email Services<br />
            </strong></div>
             <p></p>
            <div>From:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:TextBox ID="txtFrom" runat="server" Width="553px"  ></asp:TextBox> 
                <br />
            </div>
             <p></p>
             <div>Subject:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;  <asp:TextBox ID="txtSubject" runat="server" Width="553px"  ></asp:TextBox> 
                <br />
            </div>
             <p></p>
           <div>Message: <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" Height="137px" Width="563px"></asp:TextBox></div>
            <p></p>
            <div>
                <asp:Button ID="Button1" runat="server" Text="Send Email" OnClick="Button1_Click" /></div>
        </div>
    </form>
</body>
</html>
