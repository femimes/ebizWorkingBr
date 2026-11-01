<%@ Page Language="C#" AutoEventWireup="true" CodeFile="demo.aspx.cs" Inherits="Request_demo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <table style="width: 100%;">
                <tr>
                    <td colspan="3">
                        <b>Maintain eRequest</b> <font color="Red"><asp:Literal ID="Literal1" runat="server"></asp:Literal></font></td>
                </tr>
                <table style="width: 100%;" border="0">
                    <tr>
                        <td colspan="3">
                            <b>eRequest Maintenance</b></td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <table border="0">
                                <tr>
                                    <td style="width: 100px">Select BatchRefNo:</td>
                                    <td style="width: 100px">
                                        <%--  <dx:ASPxComboBox ID="drpBatchRefNo" runat="server" EnableIncrementalFiltering="True" EnableSynchronization="False" IncrementalFilteringMode="StartsWith" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"    ShowShadow="False" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" DataSourceID="SqlDbReqType" ValueField="RefNo" AutoPostBack="True">
                    <LoadingPanelImage Url="~/App_Themes/Aqua/Editors/Loading.gif">
                    </LoadingPanelImage>
                    <DropDownButton>
                        <Image>
                            <SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" PressedCssClass="dxEditors_edtDropDownPressed_Aqua" />
                        </Image>
                    </DropDownButton>
                    <ValidationSettings>
                        <ErrorFrameStyle ImageSpacing="1px">
                            <ErrorTextPaddings PaddingLeft="1px" />
                        </ErrorFrameStyle>
                    </ValidationSettings>
                    <Columns>
                        <dx:ListBoxColumn Caption="Batch RefNo" FieldName="RefNo" />
                        <dx:ListBoxColumn Caption="Request Title" FieldName="RequestTitle" />
                    </Columns>
                                   <ItemStyle Wrap="True" />
                </dx:ASPxComboBox>--%>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">

                            <asp:GridView ID="ctrlGrid" runat="server" DataSourceID="SqlDbReqDetails" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:BoundField DataField="AccountNo" HeaderText="AccountNo" SortExpression="AccountNo" />
                                    <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                                    <asp:BoundField DataField="PanNo" HeaderText="PanNo" SortExpression="PanNo" />
                                    <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" />
                                    <asp:BoundField DataField="City" HeaderText="City" SortExpression="City" />
                                    <asp:BoundField DataField="CountryCode" HeaderText="CountryCode" SortExpression="CountryCode" />
                                    <asp:BoundField DataField="CardBrand" HeaderText="CardBrand" SortExpression="CardBrand" />
                                    <asp:BoundField DataField="CardIssueDate" HeaderText="CardIssueDate" SortExpression="CardIssueDate" />
                                    <asp:BoundField DataField="CardExpiryDate" HeaderText="CardExpiryDate" SortExpression="CardExpiryDate" />
                                    <asp:BoundField DataField="CardCounter" HeaderText="CardCounter" SortExpression="CardCounter" />
                                    <asp:BoundField DataField="SeqNo" HeaderText="SeqNo" InsertVisible="False" ReadOnly="True" SortExpression="SeqNo" />
                                    <asp:BoundField DataField="SeqNum" HeaderText="SeqNum" SortExpression="SeqNum" />
                                    <asp:BoundField DataField="Seq" HeaderText="Seq" SortExpression="Seq" />
                                    <asp:BoundField DataField="RequestTitle" HeaderText="RequestTitle" SortExpression="RequestTitle" />
                                    <asp:BoundField DataField="DOB" HeaderText="DOB" SortExpression="DOB" />
                                    <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
                                    <asp:BoundField DataField="RefNo" HeaderText="RefNo" SortExpression="RefNo" />
                                    <asp:BoundField DataField="AddedBy" HeaderText="AddedBy" SortExpression="AddedBy" />
                                    <asp:BoundField DataField="ApprovedBy" HeaderText="ApprovedBy" SortExpression="ApprovedBy" />
                                    <asp:BoundField DataField="ApproveDateTime" HeaderText="ApproveDateTime" SortExpression="ApproveDateTime" />
                                    <asp:BoundField DataField="SystemDateTime" HeaderText="SystemDateTime" SortExpression="SystemDateTime" />
                                    <asp:BoundField DataField="ConfirmBy" HeaderText="ConfirmBy" SortExpression="ConfirmBy" />
                                    <asp:BoundField DataField="ConfirmDateTime" HeaderText="ConfirmDateTime" SortExpression="ConfirmDateTime" />
                                    <asp:BoundField DataField="CardPhoneNo" HeaderText="CardPhoneNo" SortExpression="CardPhoneNo" />
                                    <asp:BoundField DataField="TrackID" HeaderText="TrackID" SortExpression="TrackID" />
                                    <asp:BoundField DataField="DOB1" HeaderText="DOB1" SortExpression="DOB1" />
                                    <asp:BoundField DataField="AccountDesc" HeaderText="AccountDesc" SortExpression="AccountDesc" />
                                    <asp:BoundField DataField="AccountStatusDesc" HeaderText="AccountStatusDesc" SortExpression="AccountStatusDesc" />
                                    <asp:BoundField DataField="ProdCode" HeaderText="ProdCode" SortExpression="ProdCode" />
                                    <asp:BoundField DataField="ProdName" HeaderText="ProdName" SortExpression="ProdName" />
                                    <asp:BoundField DataField="ProdCatDesc" HeaderText="ProdCatDesc" SortExpression="ProdCatDesc" />
                                    <asp:BoundField DataField="City1" HeaderText="City1" ReadOnly="True" SortExpression="City1" />
                                    <asp:BoundField DataField="Address1" HeaderText="Address1" ReadOnly="True" SortExpression="Address1" />
                                    <asp:BoundField DataField="PhoneNo" HeaderText="PhoneNo" ReadOnly="True" SortExpression="PhoneNo" />
                                    <asp:BoundField DataField="EmailAddr" HeaderText="EmailAddr" ReadOnly="True" SortExpression="EmailAddr" />
                                    <asp:BoundField DataField="PostStatusDesc" HeaderText="PostStatusDesc" SortExpression="PostStatusDesc" />
                                    <asp:BoundField DataField="PostStatus" HeaderText="PostStatus" SortExpression="PostStatus" />
                                    <asp:BoundField DataField="AccountStatus" HeaderText="AccountStatus" SortExpression="AccountStatus" />
                                    <asp:BoundField DataField="CardTypeDesc" HeaderText="CardTypeDesc" SortExpression="CardTypeDesc" />
                                </Columns>
                            </asp:GridView>

                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table>
                                <tr>
                                    <td align="right" class="style2">
                                        <%--<dx:aspxbutton id="BtnApprove" runat="server" text="Approve Selection"
                                            width="166px" onclick="BtnApprove_Click">
                                </dx:aspxbutton>--%>
                                    </td>
                                    <td align="left" class="style2">
                                        <%--          <dx:ASPxButton ID="btnDisApprove" runat="server" Text="DisApprove Selection" 
                                    Width="175px" OnClick="btnDisApprove_Click" >
                                </dx:ASPxButton>--%>
                                    </td>
                                    <td align="left" class="style2">
                                        <%--<dx:ASPxButton ID="btncancel" runat="server" Text="Cancel" 
                                     >
                                </dx:ASPxButton>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                      <div class="TopPadding">
                          Selected count: <span id="selCount" style="font-weight: bold">0</span>
                      </div>
                         <%--   <dx:aspxlistbox id="ASPxListBox1" clientinstancename="selList" runat="server"
                                height="0px" readonly="True" width="20px"
                                cssfilepath="~/App_Themes/Aqua/{0}/styles.css" csspostfix="Aqua"
                                  
                                spritecssfilepath="~/App_Themes/Aqua/{0}/sprite.css">
                                    <LoadingPanelImage Url="~/App_Themes/Aqua/Editors/Loading.gif">
                                    </LoadingPanelImage>
                                    <ValidationSettings>
                                        <ErrorFrameStyle ImageSpacing="4px">
                                            <ErrorTextPaddings PaddingLeft="4px" />
                                        </ErrorFrameStyle>
                                    </ValidationSettings>
                                </dx:aspxlistbox>--%>

                        </td>
                        <td class="style1">&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                </table>
        </div>
        <asp:TextBox ID="txtUserID" runat="server" Visible="False"></asp:TextBox>
        <asp:TextBox ID="txtlocation" runat="server" Visible="False"></asp:TextBox>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server"
            ConnectionString="<%$ ConnectionStrings:EazyConnect %>"
            SelectCommand="SpCardRequestMaint" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:ControlParameter ControlID="txtUserID" Name="userId" Type="String" PropertyName="Text" />
                <asp:ControlParameter ControlID="txtlocation" Name="locationID" PropertyName="Text" Type="String" />
                <%--<asp:ControlParameter ControlID="drpBatchRefNo" Name="RefNo" PropertyName="Value" Type="String" />--%>
            </SelectParameters>
        </asp:SqlDataSource>

        <td>
            <asp:SqlDataSource ID="SqlDbReqType" runat="server"
                ConnectionString="<%$ ConnectionStrings:EazyConnect %>"
                SelectCommand="Sproc_GetCardForMaint" SelectCommandType="StoredProcedure">
                <%-- <SelectParameters>
            <asp:Parameter DefaultValue="5" Name="status" Type="Int32" />
            </SelectParameters>--%>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDbReqDetails" runat="server"
                ConnectionString="<%$ ConnectionStrings:EazyConnect %>"
                SelectCommand="SpCardRequestMaint" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:ControlParameter ControlID="txtUserID" Name="userId" Type="String" PropertyName="Text"  DefaultValue="FFEA8311-D14" />
                    <asp:ControlParameter ControlID="txtlocation" Name="locationID" PropertyName="Text" Type="String"  DefaultValue="FFEA8311-D14" />
                    <asp:Parameter DefaultValue="FFEA8311-D14" Name="RefNo" Type="string" />
                    <%--           <asp:ControlParameter ControlID="drpBatchRefNo"  Name="RefNo" PropertyName="Value" Type="String" DefaultValue="FFEA8311-D14"/>
                    --%>
                </SelectParameters>
            </asp:SqlDataSource>
    </form>
</body>
</html>
