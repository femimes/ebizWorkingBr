<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FTSingleAcctMofied.aspx.cs" Inherits="eOperations_FTSingleAcctMofied" %>
<%@ Register assembly="DevExpress.Web.v19.1, Version=19.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>









<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../Styles/Siteall.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/Widgets.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            text-align: justify;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <table  >
            
            <tr>
                <td colspan="3">

                    <strong>Funds Transfer Through NAPs</strong><hr style="width: 482px" />
                </td>
            </tr>
            <tr>
                <td rowspan="3" style="border-spacing:5px;padding:5px;" >
                    <asp:Panel ID="Panel1" runat="server" 
                        GroupingText="<font color='red' size='1px'>Please Select/Enter The Input Details...</font>"  BackColor=AliceBlue>
                     <table  style="border-spacing:2px;padding:5px;">
                            <tr >
                                <td style="text-align: right;border-spacing:2px;padding:6px;">
                                    from Account No:</td>
                                <td >
                                    <dx:ASPxTextBox ID="ASPxTextBox1" runat="server" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;border-spacing:2px;padding:6px;">
                                    Beneficiary Bank:</td>
                                <td>
                                    <dx:ASPxComboBox ID="ASPxComboBox1" runat="server">
                                    </dx:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;border-spacing:2px;padding:6px;">
                                    Beneficiary Account No:</td>
                                <td>
                                    <dx:ASPxTextBox ID="ASPxTextBox2" runat="server" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;border-spacing:2px;padding:6px;white-space:nowrap;">
                                    Beneficiary Account Name:</td>
                                <td>
                                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="ASPxLabel">
                                    </dx:ASPxLabel>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;border-spacing:2px;padding:6px;">
                                    Amount:</td>
                                <td>
                                    <dx:ASPxTextBox ID="ASPxTextBox3" runat="server" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;border-spacing:2px;padding:6px;">
                                    &nbsp;Payment Date:</td>
                                <td>
                                    <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="ASPxLabel">
                                    </dx:ASPxLabel>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;border-spacing:2px;padding:6px;">
                                    Payment Narration:</td>
                                <td>
                                    <dx:ASPxMemo ID="ASPxMemo1" runat="server" Height="71px" Width="170px">
                                    </dx:ASPxMemo>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align: right;border-spacing:2px;padding:6px;">
                                    <table align ="right" >
                                        <tr >
                                            <td class="style1">
                                                <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Submit" 
                                                    onclick="ASPxButton1_Click">
                                                </dx:ASPxButton>
                                            </td>
                                            <td class="style1">
                                                <dx:ASPxButton ID="ASPxButton2" runat="server" Text="Reset">
                                                </dx:ASPxButton>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                             
                   
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
          
                </td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>