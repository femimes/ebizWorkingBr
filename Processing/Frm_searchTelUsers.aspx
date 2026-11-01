<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Frm_searchTelUsers.aspx.vb" Inherits="Processing_Frm_searchTelUsers" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Search eTeller Users</title>
    <link href="../Themes/White/Default.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <P align="center">
				<TABLE id="Table1" height="175" cellSpacing="0" cellPadding="0" width="490" bgColor="#f5f5f5"
					border="1">
					<TR>
						<TD noWrap align="center" colSpan="4" height="13"><STRONG>Search&nbsp;User Names </STRONG>
						</TD>
					</TR>
					<TR>
						<TD width="105">Search Names</TD>
						<TD colSpan="3">&nbsp;<asp:textbox id="sString" runat="server" Width="301px"></asp:textbox>
							<asp:button id="Button1" runat="server" Text="Fetch"></asp:button><asp:label id="lbl_norec" runat="server" Font-Size="Smaller" Font-Names="Comic Sans MS" Font-Bold="True"
								ForeColor="Red"></asp:label></TD>
					</TR>
					<TR>
						<TD width="105" colSpan="4" height="21"><asp:datagrid id="DataGrid1" runat="server" Width="541px" ShowHeader="False" BorderWidth="1px"
								CellPadding="4" BorderColor="#CCCCCC" Height="105px" BackColor="White" BorderStyle="None" AutoGenerateColumns="False">
								<FooterStyle ForeColor="#003399" BackColor="#99CCCC"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#CCFF99" BackColor="#009999"></SelectedItemStyle>
								<ItemStyle Font-Size="XX-Small" Font-Names="Comic Sans MS" ForeColor="#003399" BackColor="White"></ItemStyle>
								<HeaderStyle Font-Size="X-Small" Font-Names="Comic Sans MS" Font-Bold="True" HorizontalAlign="Center"
									ForeColor="#CCCCFF" BackColor="DimGray"></HeaderStyle>
								<Columns>
									<asp:TemplateColumn HeaderText="User_Id">
										<ItemTemplate>
											<asp:LinkButton id=Linkbutton3 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.user_id") %>' CommandName="Delete" CausesValidation="False">
											</asp:LinkButton>
										</ItemTemplate>
										<EditItemTemplate>
											<asp:LinkButton id=LinkButton2 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.user_id") %>' CommandName="Delete" CausesValidation="False">
											</asp:LinkButton>
										</EditItemTemplate>
									</asp:TemplateColumn>
									<asp:BoundColumn DataField="user_names" SortExpression="user_names" ReadOnly="True" HeaderText="Description"></asp:BoundColumn>
									<asp:ButtonColumn Text="Select ..." HeaderText="Highlight" CommandName="Select"></asp:ButtonColumn>
								</Columns>
								<PagerStyle HorizontalAlign="Left" ForeColor="#003399" BackColor="#99CCCC" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></TD>
					</TR>
				</TABLE>
				<INPUT onclick="javascrpt:window.close()" type="button" value="  Close   ">&nbsp;&nbsp;&nbsp;
				<INPUT type="button" value="  Print   ">
			</P>
			<P><INPUT id="control" type="hidden" name="control" runat="server">
			</P>
			<P>&nbsp; <INPUT id="control1" type="hidden" name="control1" runat="server"><a href="../Themes/White/Default.skin"><span
                style="color: #0000ff; text-decoration: underline"></span></a></P>
			<P>&nbsp;</P>
			<P></P>
    </form>
</body>
</html>

