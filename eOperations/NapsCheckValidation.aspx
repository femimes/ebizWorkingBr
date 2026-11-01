<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NapsCheckValidation.aspx.cs" Inherits="eOperations_NapsCheckValidation" %>

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
    
        <table class="style1">
            <tr>
                <td colspan="2">
                    Account Details</td>
            </tr>
            <tr>
                <td>
                    ScheduleID</td>
                <td>
                    <asp:Label ID="lblScheduleID" runat="server" style="color: #FF0000"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Account No:</td>
                <td>
                    <asp:TextBox ID="txtAcctno" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    AccountName</td>
                <td>
                    <asp:TextBox ID="txtAccName" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Amount</td>
                <td>
                    <asp:TextBox ID="txtAmount" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Destination Bank</td>
                <td>
                    <asp:TextBox ID="txtBank" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Account No2</td>
                <td>
                    <asp:TextBox ID="txtAcctNo2" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Destn Bank2</td>
                <td>
                    <asp:TextBox ID="txtBank2" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnpayment" runat="server" onclick="btnpayment_Click" 
                        Text="Payment Schedule" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnStatus" runat="server" Text="Payment Status" 
                        onclick="btnStatus_Click" Width="166px" />
                    <br />
                    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Payment Multiple" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="Button2" runat="server" onclick="Button2_Click" Text="Button" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
