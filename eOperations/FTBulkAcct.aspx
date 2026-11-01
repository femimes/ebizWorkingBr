<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FTBulkAcct.aspx.cs" Inherits="eOperations_FTBulkAcct" %>

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
        height: 27px"><tr><td class="background" colspan="4" style="height: 30px"><strong>Bulk Funds Transfer UPLOAD</strong></td></tr><tr style="color: #9999cc; text-decoration: none">
            <td style="width: 94px; text-align: right;">Batch RefNo:</td><td>
            <asp:TextBox ID="txtref_no" runat="server" Width="200px"></asp:TextBox></td>
            <td style="text-align: right">Select File:</td><td>
        <table class="style1">
            <tr>
                <td>
                    <asp:FileUpload ID="Filphoto" runat="server" />
                </td>
                <td>
        <asp:Button ID="Button1" runat="server" Text="Import Entries" Width="102px" 
            onclick="Button1_Click" CausesValidation="False" />
                </td>
            </tr>
        </table>
        </td></tr>
    <tr style="color: #9999cc; text-decoration: none">
        <td style="width: 94px; text-align: right;">
            Approving Officer:</td>
        <td>
            <asp:DropDownList ID="offr2" runat="server">
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ControlToValidate="offr2" Display="Dynamic" 
                ErrorMessage="Approving Officer required" ForeColor="Red" 
                SetFocusOnError="True">*</asp:RequiredFieldValidator>
        </td>
        <td style="text-align: right">
            Transaction Type:</td>
        <td>
            <asp:DropDownList ID="drptranstype" runat="server" 
                DataSourceID="SqlDataSource1" DataTextField="TransRefDesc" 
                DataValueField="AccountNo">
                <asp:ListItem Value="0">  ....</asp:ListItem>
                <asp:ListItem Value="1">Bulk Payments</asp:ListItem>
                <asp:ListItem Value="2">Other Sources</asp:ListItem>
                
            </asp:DropDownList></td>
    </tr>
    <tr style="color: #9999cc; text-decoration: none">
        <td style="width: 94px; text-align: right;" nowrap="nowrap">
            Batch Title/Remarks:</td>
        <td colspan="3">
            <asp:TextBox ID="txtTitle" runat="server" TextMode="MultiLine" Width="200px"></asp:TextBox>
        </td>
    </tr>
    <tr style="color: #9999cc; text-decoration: none">
        <td style="width: 94px; text-align: right;">&nbsp;</td><td colspan="3">
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                ConnectionString="<%$ ConnectionStrings:EazyConnect %>" 
                SelectCommand="SpNapsGetAllTransaction" SelectCommandType="StoredProcedure">
            </asp:SqlDataSource>
        </td></tr><tr><td align="center" colspan="4" style="height: 29px">
        <asp:Button ID="Button3" runat="server" SkinID="add" 
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
