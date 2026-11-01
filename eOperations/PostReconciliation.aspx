<%@ Page Language="VB"   AutoEventWireup="false" CodeFile="PostReconciliation.aspx.vb" Inherits="Mandate_PostReconciliation" title="Untitled Page" %>

<%@ Register Assembly="DevExpress.Web.v19.1, Version=19.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../Styles/SiteAll.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table summary="Journal Posting Screen">
        <caption>
            <strong>Post Journals</strong></caption>
        <tr>
            <td style=" white-space:nowrap;" align="right">
                <strong>No of A/cs:</strong></td>
            <td colspan="3" align="left">
                <dx:ASPxTextBox ID="txtAcNos" runat="server" Width="170px" AutoPostBack="True" Text="0">
                </dx:ASPxTextBox>
            </td>
        </tr>
        <tr>
            <td style=" white-space:nowrap;" align="right">
                <strong>Total Trans. Amt:</strong></td>
            <td colspan="3" style="height: 20px">
                <dx:ASPxLabel ID="lblTotalAmt" runat="server" Text="0" RightToLeft="True" Width="159px">
                </dx:ASPxLabel>
            </td>
        </tr>
        <tr>
            <td style=" white-space:nowrap;" align="right">
                <strong>Total SubCharge Amt:</strong></td>
            <td colspan="3">
                <dx:ASPxLabel ID="lblFeeAmt" runat="server" Text="0" RightToLeft="True" Width="159px">
                </dx:ASPxLabel>
            </td>
        </tr>
        <tr>
            <td align="right" style="white-space: nowrap">
                <strong>Balance Amt:</strong></td>
            <td colspan="3">
                <dx:ASPxLabel ID="lblBal" runat="server" Text="0" RightToLeft="True" Width="159px">
                </dx:ASPxLabel>
            </td>
        </tr>
        <tr>
            <td align="right" style="white-space: nowrap">
                <strong>Approving Officer:</strong></td>
            <td colspan="3">
                &nbsp;<table border="0">
                    <tr>
                        <td style="width: 100px">
                <dx:ASPxComboBox ID="offr2" runat="server">
                </dx:ASPxComboBox>
                        </td>
                        <td style="width: 100px">
                            <strong>Value Date</strong></td>
                        <td style="width: 100px">
                            <dx:ASPxDateEdit ID="drpValueDate" runat="server" DisplayFormatString="D">
                            </dx:ASPxDateEdit>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <dx:ASPxGridView ID="grids" runat="server" AutoGenerateColumns="False" KeyFieldName="SerialNo">
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="AccountNo" VisibleIndex="0">
                            <DataItemTemplate>
                                <dx:ASPxTextBox ID="txtAccountNo" runat="server" Width="129px" CssClass='<%# Eval("SerialNo") %>' AutoPostBack = "true" OnTextChanged="txtAccountNo_TextChanged1">
                                </dx:ASPxTextBox>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="AccountName" VisibleIndex="1">
                            <DataItemTemplate>
                                <dx:ASPxLabel ID="lblAccountName" runat="server" Wrap="True">
                                </dx:ASPxLabel>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Amount Type" Name="AmountType" VisibleIndex="3">
                            <DataItemTemplate>
                                <dx:ASPxComboBox ID="drpAmtType" runat="server" SelectedIndex="0" CssClass='<%# Eval("SerialNo") %>' AutoPostBack="True" OnSelectedIndexChanged="drpAmtType_SelectedIndexChanged" >
                                    <Items>
                                        <dx:ListEditItem Selected="True" Text="Total Amount" Value="1" />
                                        <dx:ListEditItem Text="Total Subcharge Fee" Value="2" />
                                        <dx:ListEditItem Text="Total NetBalance" Value="3" />
                                        <dx:ListEditItem Text="Total Amount + (Subcharge Prop)" Value="4" />
                                        <dx:ListEditItem Text="Total Amount (Prop)+ Total Subcharge" Value="5" />
                                    </Items>
                                </dx:ASPxComboBox>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="TransactionType" VisibleIndex="2">
                            <DataItemTemplate>
                                <dx:ASPxComboBox ID="ASPxComboBox1" runat="server">
                                    <Items>
                                        <dx:ListEditItem Text="Debit" Value="Dr" />
                                        <dx:ListEditItem Text="Credit" Value="Cr" />
                                    </Items>
                                </dx:ASPxComboBox>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Proportion Amount(%)" Name="Proportion" VisibleIndex="4">
                            <DataItemTemplate>
                                <dx:ASPxTextBox ID="txtProp" runat="server" Width="170px" CssClass='<%# Eval("SerialNo") %>' AutoPostBack = "true" OnTextChanged="txtProp_TextChanged" Text="0">
                                </dx:ASPxTextBox>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Clear" VisibleIndex="7">
                            <DataItemTemplate>
                                <dx:ASPxButton ID="btnClear" runat="server" Text="Clear">
                                </dx:ASPxButton>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Status" VisibleIndex="6">
                            <DataItemTemplate>
                                <dx:ASPxLabel ID="lblStatus" runat="server" Text="No Account Entry" Font-Size="XX-Small" ForeColor="Crimson">
                                </dx:ASPxLabel>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Amount" VisibleIndex="5">
                            <DataItemTemplate>
                                <dx:ASPxLabel ID="lblAmount" runat="server" RightToLeft="True" Text="0" Width="100px">
                                </dx:ASPxLabel>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    

         
                </dx:ASPxGridView>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <table border="0">
                    <tr>
                        <td  >
                            <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Submit">
                            </dx:ASPxButton>
                        </td>
                        <td   >
                            <dx:ASPxButton ID="ASPxButton2" runat="server" Text="Reset">
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                &nbsp;</td>
        </tr>
        <tr>
            <td style="width: 93px">
            </td>
            <td style="width: 100px">
            </td>
            <td style="width: 100px">
            </td>
            <td style="width: 100px">
            </td>
        </tr>
    </table>
</div>
    </form>
</body>
</html>
