<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestCardInfo.aspx.cs" Inherits="SCPLite_TestCardInfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div>Sequence No:</div> <div>
                <asp:TextBox ID="txtSeqno" runat="server"></asp:TextBox></div>
             <div>Pan No:</div> <div>
                <asp:TextBox ID="txtPanno" runat="server"></asp:TextBox></div>
             <div>Expiry Date:</div> <div>
                <asp:TextBox ID="txtExpdate" runat="server"></asp:TextBox></div>
             <div>Pin:</div> <div>
                <asp:TextBox ID="txtpin" runat="server"></asp:TextBox></div>
            <div>Cvv:</div> <div>
                <asp:TextBox ID="txtCvv" runat="server"></asp:TextBox>
                <br />
                <br />
                <br />
                <asp:Label ID="lblResp" runat="server"></asp:Label>
                <br />
            </div>
          <div>Token:</div> <div>
                <asp:TextBox ID="txtToken" runat="server"></asp:TextBox></div>

            <div>
                <asp:Button ID="Button1" runat="server" Text="Block" OnClick="Button1_Click" />
            &nbsp;<asp:Button ID="btnUnblock" runat="server" Text="UnBlock" OnClick="btnUnblock_Click" />
            &nbsp;<asp:Button ID="btnCardlink" runat="server" Text="Card Link" OnClick="btnCardlink_Click" />
                 &nbsp;<asp:Button ID="btnCardunlink" runat="server" Text="Card UnLink" OnClick="btnCardunlink_Click" />
            &nbsp;<asp:Button ID="btnPinReissue" runat="server" Text="Pin Reissuance" OnClick="btnPinReissue_Click" />
            &nbsp;<asp:Button ID="btnGetCardInfo" runat="server" Text="CardInfo By AccountNo" OnClick="btnGetCardInfo_Click" />
            </div>
        </div>
    </form>
</body>
</html>
