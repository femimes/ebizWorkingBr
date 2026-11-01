<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Frm_searchZones.aspx.vb" Inherits="Processing_Frm_searchZones" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Search Locations</title>
    <link href="../Themes/White/Default.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
<P align="center">
				<TABLE id="Table1" cellSpacing="0" cellPadding="0" bgColor="#f5f5f5" border="1">
					<TR>
						<TD noWrap align="center" colSpan="4" height="13"><STRONG> Search&nbsp;Location </STRONG>
						</TD>
					</TR>
					<TR>
						<TD nowrap>Search Item</TD>
						<TD colSpan="3">&nbsp;<asp:textbox id="sString" runat="server" Width="301px"></asp:textbox>
							<asp:button id="Button1" runat="server" Text="Fetch"></asp:button><asp:label id="lbl_norec" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Comic Sans MS"
								Font-Size="Smaller"></asp:label></TD>
					</TR>
					<TR>
						<TD colSpan="4" height="21"><asp:datagrid id="DataGrid1" runat="server" AutoGenerateColumns="False" BorderStyle="None" BackColor="White"
								Height="11px" BorderColor="#CCCCCC" CellPadding="4" BorderWidth="1px" CssClass="background">
								<FooterStyle ForeColor="#003399" BackColor="#99CCCC"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#CCFF99" BackColor="#009999"></SelectedItemStyle>
								<ItemStyle ForeColor="#003399" BackColor="White"></ItemStyle>
								<HeaderStyle Font-Bold="True" Wrap="False" HorizontalAlign="Left"></HeaderStyle>
								<Columns>
									<asp:TemplateColumn SortExpression="LGADesc" HeaderText="LGA">
										<ItemTemplate>
											<asp:LinkButton id="LinkButton1" CommandName=Delete Text='<%# DataBinder.Eval(Container, "DataItem.LGADesc") %>' CssClass='<%# DataBinder.Eval(Container, "DataItem.LGAID") %>' runat="server">
											</asp:LinkButton>
										</ItemTemplate>
										<EditItemTemplate>
											<asp:TextBox id=TextBox1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LGADesc") %>'>
											</asp:TextBox>
										</EditItemTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn SortExpression="StateDesc" HeaderText="State">
										<ItemTemplate>
											<asp:LinkButton id="Linkbutton2" CommandName=Delete Text='<%# DataBinder.Eval(Container, "DataItem.StateDesc") %>' CssClass='<%# DataBinder.Eval(Container, "DataItem.stateID") %>' runat="server">
											</asp:LinkButton>
										</ItemTemplate>
										<EditItemTemplate>
											<asp:TextBox ID="TextBox2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.StateDesc") %>'>
											</asp:TextBox>
										</EditItemTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn SortExpression="zonedesc" HeaderText="Zone">
										<ItemTemplate>
											<asp:LinkButton id="Linkbutton3" CommandName=Delete Text='<%# DataBinder.Eval(Container, "DataItem.ZoneDesc") %>' CssClass='<%# DataBinder.Eval(Container, "DataItem.zoneID") %>' runat="server">
											</asp:LinkButton>
										</ItemTemplate>
										<EditItemTemplate>
											<asp:TextBox ID="TextBox3" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZoneDesc") %>'>
											</asp:TextBox>
										</EditItemTemplate>
									</asp:TemplateColumn>
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
</html>
