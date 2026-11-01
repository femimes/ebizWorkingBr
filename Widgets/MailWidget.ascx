<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MailWidget.ascx.cs" Inherits="Widgets_MailWidget" %>
 <%@ Register assembly="DevExpress.Web.v19.1, Version=19.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>










<style type="text/css">
    .style1
    {
        width: 100%;
    }
    .style2
    {
        font-family: Arial;
        font-size: x-small;
    }
</style>

<ul class="mailFolders">
    <li>
    
        <dx:ASPxHyperLink ID="ASPxHyperLink5" runat="server"   Text="Cards Pending Approvals" NavigateUrl="~/Approvals/ApproveCards.aspx" />
    </li>
    <li>
        <dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" NavigateUrl="javascript:void(0)" Text="Reconciliation Pending Approvals">
        </dx:ASPxHyperLink>
    </li>
     
    <li>
        <dx:ASPxHyperLink ID="ASPxHyperLink4" runat="server" 
            NavigateUrl="javascript:void(0)" Text="Drafts" 
            CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua">
        </dx:ASPxHyperLink>
    </li>
</ul>
<dx:ASPxPanel ID="ASPxPanel1" runat="server" Width="200px">
    <PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
    <table class="style1">
        <tr>
            <td class="style2" colspan="4">
                <dx:ASPxHyperLink ID="ASPxHyperLink6" runat="server" 
                    Text="Comment / Feedback ..">
                </dx:ASPxHyperLink>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <dx:ASPxMemo ID="ASPxMemo1" runat="server" Height="141px" Width="100%" 
                    CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" 
                    SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                    <ValidationSettings>
                        <ErrorFrameStyle ImageSpacing="4px">
                            <ErrorTextPaddings PaddingLeft="4px" />
                        </ErrorFrameStyle>
                    </ValidationSettings>
                </dx:ASPxMemo>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxButton ID="ASPxButton1" runat="server" 
                    CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" 
                    SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" Text="Submit" 
                    OnClick="ASPxButton1_Click">
                </dx:ASPxButton>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
        </dx:PanelContent>
</PanelCollection>
</dx:ASPxPanel>
