<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Test2FA.aspx.cs" Inherits="Request_Test2FA" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            UserID<br />
            <asp:TextBox ID="txtuserid" runat="server" OnTextChanged="TextBox1_TextChanged"></asp:TextBox>
            <br />
            ClienId<br />
            <asp:TextBox ID="txtclientid" runat="server"></asp:TextBox>
            <br />
            Serial<br />
            <asp:TextBox ID="txtserial" runat="server"></asp:TextBox>
            <br />
            Token<br />
            <asp:TextBox ID="txtToken" runat="server"></asp:TextBox>
            <br />
            <br />
            <asp:Label ID="lblResp" runat="server"></asp:Label>
            <br />
            &nbsp;&nbsp;
            <br />
                Expairy Month:  <asp:TextBox ID ="txtMonth" runat ="server" TextMode="Month"  CssClass="form-control"  required ></asp:TextBox>
     
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Verify" />
         <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Check value" />
            <br />
            <asp:Button ID="Button3" runat="server" OnClick="Button3_Click" Text="Check Result" />
            <br />
            <br />
        </div>
    </form>
</body>
</html>
