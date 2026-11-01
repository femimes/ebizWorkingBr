<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FTSingleConfirm.aspx.cs" Inherits="eOperations_FTSingleConfirm" %>

<%@ Register assembly="DevExpress.Web.v19.1, Version=19.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 259px;
        }
        .style2
        {
            width: 100px;
        }
    </style>
    <link href="../Styles/SiteAll.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div style ="position:relative">
    
        <table class="style1" cellpadding="5" cellspacing="3">
            <tr>
                <td colspan="2" style="text-align: center">
                    <strong>Transaction Details ...</strong></td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center; color:Red; ">
                    <asp:Literal ID="Literal1"  runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td nowrap="nowrap" style="text-align: right">
                    Transaction RefCode:</td>
                <td>
                    <dx:ASPxLabel ID="lblRefNo" runat="server">
                    </dx:ASPxLabel>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    Account No:</td>
                <td>
                    <dx:ASPxLabel ID="lblAccountNo" runat="server">
                    </dx:ASPxLabel>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    Account Name:</td>
                <td>
                    <dx:ASPxLabel ID="lblAcctName" runat="server">
                    </dx:ASPxLabel>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    Beneficiary Account No:</td>
                <td>
                    <dx:ASPxLabel ID="lblBenAcctNo" runat="server">
                    </dx:ASPxLabel>
                </td>
            </tr>
            <tr>
                <td nowrap="nowrap" style="text-align: right">
                    Beneficiary Account Name:</td>
                <td>
                    <dx:ASPxLabel ID="lblBenAcctName" runat="server">
                    </dx:ASPxLabel>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    Beneficiary Bank:</td>
                <td>
                    <dx:ASPxLabel ID="lblBenBank" runat="server">
                    </dx:ASPxLabel>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    Amount:</td>
                <td>
                    <dx:ASPxLabel ID="lblAmount" runat="server">
                    </dx:ASPxLabel>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    Payment Details:</td>
                <td>
                    <dx:ASPxLabel ID="lblPayDetails" runat="server">
                    </dx:ASPxLabel>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    Fee Amount</td>
                <td>
                    <dx:ASPxLabel ID="lblFeeAmount" runat="server">
                    </dx:ASPxLabel>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    OTP Code:</td>
                <td>
                    <dx:ASPxTextBox ID="txtOTP" runat="server" Width="170px">
                    </dx:ASPxTextBox>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <table class="style2">
                        <tr>
                            <td>
                    <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Submit" Height="25px" 
                                    onclick="ASPxButton1_Click">
                    </dx:ASPxButton>
                            </td>
                            <td>
                    <dx:ASPxButton ID="ASPxButton2" runat="server" Text="Cancel">
                    </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
