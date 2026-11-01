<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AgencyList.aspx.cs" Inherits="AlphaBeta_AgencyList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />
            <br />
            <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Rev Button" />
            <asp:TextBox ID="txtamount" runat="server"></asp:TextBox>
            <br />
            <asp:Button ID="Button3" runat="server" OnClick="Button3_Click" Text="Pay Bills" />
            <asp:Button ID="Button4" runat="server" OnClick="Button4_Click" Text="Print Receipts" />
            <asp:Button ID="Button5" runat="server" OnClick="Button5_Click" Text="Validate WebGuid" />
            <br />
        </div>
    </form>
</body>
</html>
