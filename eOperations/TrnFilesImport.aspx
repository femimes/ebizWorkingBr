<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TrnFilesImport.aspx.cs" Inherits="eOperations_TrnFilesImport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 100px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table border="0" cellpadding="3" cellspacing="2" class="photo-frame" s="" style="border-right: thin outset;
        border-top: thin outset; font-size: 7pt; border-left: thin outset; width: 751px;
        color: #9999cc; border-bottom: thin outset; font-family: Verdana, 'Microsoft Sans Serif';
        height: 27px"><tr><td class="background" colspan="4" style="height: 30px"><strong>EXTERNAL JOURNAL ENTRIES &nbsp;UPLOAD</strong></td></tr><tr style="color: #9999cc; text-decoration: none"><td style="width: 94px">Batch RefNo</td><td><asp:TextBox ID="txtref_no" runat="server"></asp:TextBox></td><td>Select File</td><td>
        <table class="style1">
            <tr>
                <td>
                    <asp:FileUpload ID="Filphoto" runat="server" />
                </td>
                <td>
        <asp:Button ID="Button1" runat="server" Text="Import Entries" Width="102px" 
            onclick="Button1_Click" />
                </td>
            </tr>
        </table>
        </td></tr>
    <tr style="color: #9999cc; text-decoration: none">
        <td style="width: 94px">
            Approving Officer</td>
        <td>
            <asp:DropDownList ID="offr2" runat="server">
            </asp:DropDownList></td>
        <td>
            Transaction Type</td>
        <td>
            <asp:DropDownList ID="drptranstype" runat="server">
                <asp:ListItem Value="0">  ....</asp:ListItem>
                <asp:ListItem Value="1">Hr/Payroll</asp:ListItem>
                <asp:ListItem Value="2">Other Sources</asp:ListItem>
                <asp:ListItem Value="3" Enabled="False">Promo Sources</asp:ListItem>
                <asp:ListItem Value="4" Enabled="False">Promo New Accounts</asp:ListItem>
                <asp:ListItem Value="8" Enabled="False">Reconciliation Transactions(ATM)</asp:ListItem>
                <asp:ListItem Value="10" Enabled="False">Reconciliation Transactions(ATM ReCharge)</asp:ListItem>
                 <asp:ListItem Value="11">Reconciliation Transactions(POS)</asp:ListItem>
                  <asp:ListItem Value="12">Reconciliation Transactions(Merchant POS)</asp:ListItem>
                <asp:ListItem Value="9" Enabled="False">Reconciliation Transactions(ATM2)</asp:ListItem>
                
            </asp:DropDownList></td>
    </tr>
    <tr style="color: #9999cc; text-decoration: none"><td style="width: 94px"></td><td colspan="3"></td></tr><tr><td align="center" colspan="4" style="height: 29px">
        <asp:Button ID="Button3" runat="server" CausesValidation="False" SkinID="add" 
            Text="Commit Entries" Width="105px" onclick="Button3_Click" /> &nbsp; <asp:Button ID="Button4" runat="server" CausesValidation="False" SkinID="add" Text="Reset Entries" Width="94px" />&nbsp; <asp:Button ID="Button2" runat="server" Text="Reject Entries" Width="102px" /></td></tr><tr><td align="center" colspan="4" style="height: 29px"><asp:DataGrid ID="DataGrid1" runat="server" BorderColor="#CCCCCC" BorderStyle="None"
                    BorderWidth="1px" Caption="Import Details" CaptionAlign="Top" CellPadding="3"
                    Font-Bold="False">
                    <FooterStyle BackColor="White" ForeColor="#000066" />
                    <SelectedItemStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" Mode="NumericPages" />
                    <ItemStyle ForeColor="#000066" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Size="XX-Small" Font-Strikeout="False" Font-Underline="False" />
                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" Wrap="False" Font-Size="X-Small" />
                    <AlternatingItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False"
                        Font-Size="XX-Small" Font-Strikeout="False" Font-Underline="False" />
                </asp:DataGrid></td></tr></table>
    </div>
    </form>
</body>
</html>
