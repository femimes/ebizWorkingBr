<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Frm_searchRoles.aspx.vb" Inherits="Processing_Frm_searchRoles" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<HTML>
	<HEAD>
		<title>User Roles Enquiry</title>
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
		<LINK href="../Themes/White/Default.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body>
		<form id="Form1" runat="server">
			<P align="center">
				<TABLE id="Table1" height="175" cellSpacing="0" cellPadding="0" width="490" bgColor="#f5f5f5"
					border="1">
					<TR>
						<TD noWrap align="center" colSpan="4" height="13"><STRONG>Search&nbsp;User Roles </STRONG>
						</TD>
					</TR>
					<TR>
						<TD width="105">Search Item</TD>
						<TD colSpan="3">&nbsp;<asp:textbox id="sString" runat="server" Width="301px"></asp:textbox>
							<asp:button id="Button1" runat="server" Text="Fetch"></asp:button><asp:label id="lbl_norec" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Comic Sans MS"
								Font-Size="Smaller"></asp:label></TD>
					</TR>
					<TR>
						<TD width="105" colSpan="4" height="21"><asp:datagrid id="DataGrid1" runat="server" Width="541px" AutoGenerateColumns="False" BorderStyle="None"
								BackColor="White" Height="105px" BorderColor="#CCCCCC" CellPadding="4" BorderWidth="1px" ShowHeader="False">
								<FooterStyle ForeColor="#003399" BackColor="#99CCCC"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#CCFF99" BackColor="#009999"></SelectedItemStyle>
								<ItemStyle Font-Size="XX-Small" Font-Names="Comic Sans MS" ForeColor="#003399" BackColor="White"></ItemStyle>
								<HeaderStyle Font-Size="X-Small" Font-Names="Comic Sans MS" Font-Bold="True" HorizontalAlign="Center"
									ForeColor="#CCCCFF" BackColor="DimGray"></HeaderStyle>
								<Columns>
									<asp:TemplateColumn HeaderText="Role ID">
										<ItemTemplate>
											<asp:LinkButton id=Linkbutton3 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.role_id") %>' CausesValidation="False" CommandName="Delete">
											</asp:LinkButton>
										</ItemTemplate>
										<EditItemTemplate>
											<asp:LinkButton id=LinkButton2 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.role_id") %>' CausesValidation="False" CommandName="Delete">
											</asp:LinkButton>
										</EditItemTemplate>
									</asp:TemplateColumn>
									<asp:BoundColumn DataField="role_desc" ReadOnly="True" HeaderText="Description"></asp:BoundColumn>
									<asp:ButtonColumn Visible="False" Text="Select ..." HeaderText="Highlight" CommandName="Select"></asp:ButtonColumn>
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
			<P>&nbsp; <INPUT id="control1" type="hidden" name="control1" runat="server"></P>
			<P>&nbsp;</P>
			<P></P>
		</form>
	</body>
</HTML>

