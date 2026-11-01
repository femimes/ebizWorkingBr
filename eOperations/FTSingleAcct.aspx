<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FTSingleAcct.aspx.cs" Inherits="eOperations_FTSingleAcct" %>

<%@ Register assembly="DevExpress.Web.v19.1, Version=19.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>








<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Styles/Siteall.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/Widgets.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            text-align: justify;
        }
        .style3
        {
            width: 250px;
        }
        .style4
        {
            width: 100px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    
        <table  >
            
            <tr>
                <td colspan="3"  >

                    <strong>Funds Transfer Through NAPs<br />
                    </strong>
                    <font color="red">
                    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                    </font>
                    <hr style="width: 482px; text-align: left;" />
                </td>
            </tr>
            <tr>
                <td rowspan="4" style="border-spacing:5px;padding:5px;" >
                              <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" 
                        CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="480px" 
                        HeaderText="Please Select/Enter The Input Details..." GroupBoxCaptionOffsetY="-24px">
                        <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px"  />
                        <HeaderStyle Font-Size="X-Small" Height="23px" >
                        
                        <Paddings PaddingBottom="0px" PaddingTop="0px" PaddingLeft="2px" />
                        </HeaderStyle>
                        <PanelCollection>
<dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True"> <table  style="border-spacing:2px;padding:5px;">
                            <tr >
                                <td style="text-align: right;border-spacing:2px;padding:6px;">
                                    from Account No:</td>
                                <td class="style3" >
                                    <table class="style4">
                                        <tr>
                                            <td>
                                            <asp:UpdatePanel ID="UpdatePanelAccountNo" runat ="server" >
                                                    <ContentTemplate>
                                                <dx:ASPxTextBox ID="txtAccountNo" runat="server" AutoPostBack="True" 
                                                    Width="170px" ontextchanged="txtAccountNo_TextChanged">
                                                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" 
                                                        ErrorText="Account No missing" SetFocusOnError="True">
                                                        <RequiredField ErrorText="Account No missing" IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                                </ContentTemplate></asp:UpdatePanel>
                                            </td>
                                            <td><asp:UpdatePanel ID="UpdatePanellblAccountName" runat ="server" >
                                                    <ContentTemplate>
                                                <dx:ASPxLabel ID="lblAccountName" runat="server" 
                                                    style="color: #FF0000; font-weight: 700; font-size: x-small" Wrap="False">
                                                </dx:ASPxLabel>
                                                </ContentTemplate></asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;border-spacing:2px;padding:6px;">
                                    Beneficiary Bank:</td>
                                <td class="style3">
                                <asp:UpdatePanel ID="UpdatePanelBanks" runat ="server" >
                                                    <ContentTemplate>
                                    <dx:ASPxComboBox ID="drpBanks" runat="server" AutoPostBack="True" 
                                        DataSourceID="sqlBenBanks" TextFormatString="{0}; {1}" ValueField="sort_code">
                                        <Columns>
                                            <dx:ListBoxColumn Caption="Bank Code/Sort Code" FieldName="sort_code" />
                                            <dx:ListBoxColumn Caption="Bank Name" FieldName="bank_name" />
                                        </Columns>
                                    </dx:ASPxComboBox>
                                    </ContentTemplate></asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;border-spacing:2px;padding:6px;">
                                    Beneficiary Account No:</td>
                                <td class="style3">
                                <asp:UpdatePanel ID="UpdatePanelBenAcctNo" runat ="server" >
                                                    <ContentTemplate>
                                    <dx:ASPxTextBox ID="txtBenAccountNo" runat="server" Width="170px" 
                                        AutoPostBack="True" ontextchanged="txtBenAccountNo_TextChanged">
                                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" 
                                            ErrorText="Beneficiary AccountNo missing" SetFocusOnError="True">
                                            <RequiredField IsRequired="True" ErrorText="Beneficiary AccountNo missing" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                    </ContentTemplate></asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;border-spacing:2px;padding:6px;white-space:nowrap;">
                                    Beneficiary Account Name:</td>
                                <td class="style3">
                                <asp:UpdatePanel ID="UpdatePanellblBenAcctName" runat ="server" >
                                                    <ContentTemplate>
                                    <dx:ASPxLabel ID="lblBenAccountNo" runat="server" style="color: #0000FF">
                                    </dx:ASPxLabel></ContentTemplate></asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;border-spacing:2px;padding:6px;">
                                    Amount:</td>
                                <td class="style3" nowrap="nowrap">
                                <table><tr><td>
                                <asp:UpdatePanel ID="UpdatePanelAmt" runat ="server" >
                                                    <ContentTemplate>
                                    <dx:ASPxSpinEdit ID="txtAmounts" runat="server" Height="21px" 
                                        MaxValue="99999999999.99" MinValue="0.01" NullText="0.00" Number="0">
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" 
                                            ErrorText="Invalid amount" SetFocusOnError="True">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxSpinEdit>
                                    </ContentTemplate></asp:UpdatePanel>
                                    </td>
                                    <td style="font-weight: 700; font-size: x-small; color: #FF0000">(NGN)</td></tr></table>
                                    
                                     </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;border-spacing:2px;padding:6px;">
                                    &nbsp;Payment Date:</td>
                                <td class="style3">
                                    <dx:ASPxLabel ID="lblPaymentDate" runat="server">
                                    </dx:ASPxLabel>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;border-spacing:2px;padding:6px;">
                                    Payment Narration:</td>
                                <td class="style3">
                                <asp:UpdatePanel ID="UpdatePanelmemo" runat ="server" >
                                                    <ContentTemplate>
                                    <dx:ASPxMemo ID="txtNarration" runat="server" Height="71px" Width="170px">
                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField ErrorText="Payment description missing" IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxMemo></ContentTemplate></asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align: right;border-spacing:2px;padding:6px;">
                                    <table align ="right" >
                                        <tr >
                                            <td class="style1">
                                          <%--  <asp:UpdatePanel ID="UpdatePanel1" runat ="server" >
                                                    <ContentTemplate>--%>
                                                <dx:ASPxButton ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" 
                                                     >
                                                </dx:ASPxButton>
                                                <%--</ContentTemplate></asp:UpdatePanel>--%>
                                            </td>
                                            <td class="style1">
                                                <dx:ASPxButton ID="btnReset" runat="server" Text="Reset">
                                                </dx:ASPxButton>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table></dx:PanelContent>
</PanelCollection>
                    </dx:ASPxRoundPanel>
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
            <tr>
                <td>
                    <asp:SqlDataSource ID="sqlBenBanks" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:EazyConnect %>" 
                        SelectCommand="SpNapsGetBanks" SelectCommandType="StoredProcedure">
                    </asp:SqlDataSource>
                </td>
                <td>
          
                    &nbsp;</td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
