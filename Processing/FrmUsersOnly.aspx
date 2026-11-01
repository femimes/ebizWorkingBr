<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FrmUsersOnly.aspx.vb" Inherits="Processing_FrmUsersOnly"  %>

 
 
	</script>
    <link href="../Styles/SiteAll.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">  <div>
		<asp:ScriptManager 
                            ID="ScriptManager1" runat="server">
                        </asp:ScriptManager>
			<TABLE id="Table1"  border="1" class="TableRuling" >
				<TR>
					<TD colSpan="4" class="background">
						<STRONG>USERS MAINTENANCE&nbsp;</STRONG>
					</TD>
				</TR>
				<TR>
					<TD>
						User ID
					</TD>
					<TD><asp:textbox id="users" runat="server" Width="122px" AutoPostBack="True"></asp:textbox>&nbsp;
						<asp:Button id="cmd_go" runat="server" Text="go"></asp:Button>
                        <STRONG><A title="Click to view Users Info" onclick="window.open('Frm_searchUsers.aspx?textbox=users&amp;p=names','cal','width=600,height=450,scrollbars=yes')"
								href="#">
                            <asp:HyperLink ID="hypusers" runat="server" Font-Size="XX-Small" ToolTip="Search User Info ...">Search User Info ...</asp:HyperLink></A></STRONG></TD>
					<TD colSpan="2"><asp:checkbox id="disabled" runat="server" Height="12px" Text="Disable User Account" Font-Bold="True"
							Font-Size="XX-Small"></asp:checkbox><FONT face="Comic Sans MS" color="#ffffff" size="2"></FONT></TD>
				</TR>
				<TR>
					<TD>
						Full Names</TD>
					<TD colSpan="3"><asp:textbox id="names" runat="server" Width="376px"></asp:textbox><asp:requiredfieldvalidator id="RequiredFieldValidator2" runat="server" ControlToValidate="names" Display="Dynamic"
							ToolTip="Names empty" ErrorMessage="Names empty">*</asp:requiredfieldvalidator><FONT face="Comic Sans MS" color="#ffffff" size="2"></FONT></TD>
				</TR>
				<TR>
					<TD colSpan="4"><STRONG>Data Entry Section</STRONG>
					</TD>
				</TR>
				<TR>
					<TD>
					</TD>
					<TD>
                        <strong>
						Minimum </strong>
					</TD>
					<TD style="WIDTH: 192px">
                        <strong>
						User Posting Limit </strong>
					</TD>
					<TD>
                        <strong>
						Ceiling Amount</strong></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 25px">
						Cash Receipts
					</TD>
					<TD style="HEIGHT: 25px">
					<asp:UpdatePanel ID="UpdatePanel3" runat="server">
                      <ContentTemplate>
					<asp:textbox id="minrec" runat="server" AutoPostBack="True" CssClass="CssStyle3"></asp:textbox>
					</ContentTemplate></asp:UpdatePanel>
					</TD>
					<TD style="WIDTH: 192px; HEIGHT: 25px">
					<asp:UpdatePanel ID="UpdatePanel50" runat="server">
                      <ContentTemplate>
					<asp:textbox id="maxrec" runat="server" AutoPostBack="True" 
					CssClass="CssStyle3"></asp:textbox></ContentTemplate></asp:UpdatePanel>
					</TD>
					<TD style="HEIGHT: 25px">
					<asp:UpdatePanel ID="UpdatePanel0672z" runat="server">
                      <ContentTemplate>
					<asp:textbox id="ceilrec" runat="server" CssClass="CssStyle3"
					 AutoPostBack="True" ></asp:textbox></ContentTemplate></asp:UpdatePanel>
					 
                    </TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 28px">
						Cash Payments
					</TD>
					<TD style="HEIGHT: 28px">
					<asp:UpdatePanel ID="UpdatePanel2" runat="server">
                      <ContentTemplate>
					<asp:textbox id="minpay" runat="server" CssClass="CssStyle3" AutoPostBack="True" ></asp:textbox>
					</ContentTemplate></asp:UpdatePanel>
					<FONT face="Comic Sans MS" color="#ffffff" size="2"></FONT></TD>
					<TD style="WIDTH: 192px; HEIGHT: 28px">
					<asp:UpdatePanel ID="UpdatePanel115" runat="server">
                      <ContentTemplate>
					<asp:textbox id="maxpay" runat="server" CssClass="CssStyle3" AutoPostBack="True" ></asp:textbox>
					</ContentTemplate></asp:UpdatePanel>
					<FONT face="Comic Sans MS" color="#ffffff" size="2"></FONT></TD>
					<TD style="HEIGHT: 28px">
					<asp:UpdatePanel ID="UpdatePanel65" runat="server">
                      <ContentTemplate>
					<asp:textbox id="ceilpay" runat="server" AutoPostBack="True" CssClass="CssStyle3"></asp:textbox>
					</ContentTemplate></asp:UpdatePanel>
					</TD>
				</TR>
				<TR>
					<TD style="height: 30px">
						Non Cash Posting
					</TD>
					<TD style="height: 30px">
					<asp:UpdatePanel ID="UpdatePanel5670" runat="server">
                      <ContentTemplate>
					<asp:textbox id="minnoncash" runat="server" AutoPostBack="True"  CssClass="CssStyle3"></asp:textbox>
					</ContentTemplate></asp:UpdatePanel>
					<FONT face="Comic Sans MS" color="#ffffff" size="2"></FONT></TD>
					<TD style="WIDTH: 192px; height: 30px;">
					<asp:UpdatePanel ID="UpdatePanel25" runat="server">
                      <ContentTemplate>
					<asp:textbox id="maxnoncash" runat="server" AutoPostBack="True" CssClass="CssStyle3"></asp:textbox>
					</ContentTemplate></asp:UpdatePanel>
					</TD>
					<TD style="height: 30px">
					<asp:UpdatePanel ID="UpdatePanel35" runat="server">
                      <ContentTemplate>
					<asp:textbox id="ceilnoncash" runat="server" AutoPostBack="True" CssClass="CssStyle3"></asp:textbox>
					</ContentTemplate></asp:UpdatePanel>
					</TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 37px">
						Teller Cash Account
					</TD>
					<TD style="HEIGHT: 37px" colSpan="3"><asp:textbox id="tellerAcct" runat="server" Width="392px" AutoPostBack="True"></asp:textbox>&nbsp;
                        <asp:HyperLink ID="hypTeller" runat="server" Font-Size="XX-Small" ToolTip=" Search Teller Account ..."> Search Teller Account ...</asp:HyperLink>
                        <FONT face="Comic Sans MS" size="1"><STRONG>&nbsp;
								<asp:TextBox id="accountname" runat="server" Width="16px" BorderWidth=0 BorderStyle="None"></asp:TextBox></STRONG></FONT></TD>
				</TR>
				<TR>
					<TD colSpan="4">
						<STRONG>Entry Approval Section
							<asp:customvalidator id="cus_val" runat="server" ControlToValidate="ceilnoncash" Display="Dynamic"></asp:customvalidator></STRONG>
					</TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 28px">
						Min. Cash Receipts
					</TD>
					<TD style="HEIGHT: 28px">
					<asp:UpdatePanel ID="UpdatePanel5" runat="server">
                      <ContentTemplate>
					<asp:textbox id="minrec1" runat="server" CssClass="CssStyle3" AutoPostBack="True" ></asp:textbox>
					</ContentTemplate></asp:UpdatePanel>
					</TD>
					<TD style="WIDTH: 192px; HEIGHT: 28px">
						Max. Cash Receipts
					</TD>
					<TD style="HEIGHT: 28px">
					<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                      <ContentTemplate>
					<asp:textbox id="maxrec1" runat="server" CssClass="CssStyle3" AutoPostBack="True" ></asp:textbox>
					</ContentTemplate></asp:UpdatePanel>
					</TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 23px">
						Min. Cash Payments
					</TD>
					<TD style="HEIGHT: 23px">
					<asp:UpdatePanel ID="UpdatePanel067" runat="server">
                      <ContentTemplate>
					<asp:textbox id="minpay1" runat="server" CssClass="CssStyle3" AutoPostBack="True"  ></asp:textbox>
					</ContentTemplate></asp:UpdatePanel>
					</TD>
					<TD style="WIDTH: 192px; HEIGHT: 23px">
						Max. Cash payments
					</TD>
					<TD style="HEIGHT: 23px">
					<asp:UpdatePanel ID="UpdatePanel068" runat="server">
                      <ContentTemplate>
					<asp:textbox id="maxpay1" runat="server" AutoPostBack="True" CssClass="CssStyle3"></asp:textbox>
					</ContentTemplate></asp:UpdatePanel>
					</TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 22px">
						Min. Non Cash
					</TD>
					<TD style="HEIGHT: 22px">
					<asp:UpdatePanel ID="UpdatePanel867" runat="server">
                      <ContentTemplate>
					<asp:textbox id="minnoncash1" runat="server" AutoPostBack="True"  CssClass="CssStyle3"></asp:textbox>
					</ContentTemplate></asp:UpdatePanel>
					</TD>
					<TD style="WIDTH: 192px; HEIGHT: 22px">
						Max.Non Cash
					</TD>
					<TD style="HEIGHT: 22px">
					<asp:UpdatePanel ID="UpdatePanel567" runat="server">
                      <ContentTemplate>
					<asp:textbox id="maxnoncash1" runat="server" AutoPostBack="True" CssClass="CssStyle3"></asp:textbox>
					</ContentTemplate></asp:UpdatePanel>
					<FONT face="Comic Sans MS" color="#ffffff" size="2"></FONT></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 8px">
						Min. O/D Approval
					</TD>
					<TD style="HEIGHT: 8px">
					<asp:UpdatePanel ID="UpdatePanel767" runat="server">
                      <ContentTemplate>
					<asp:textbox id="minod" runat="server" AutoPostBack="True"  CssClass="CssStyle3"></asp:textbox>
					</ContentTemplate></asp:UpdatePanel>
					<FONT face="Comic Sans MS" color="#ffffff" size="2"></FONT></TD>
					<TD style="WIDTH: 192px; HEIGHT: 8px">
						Max. O/D Approval
					</TD>
					<TD style="HEIGHT: 8px">
					<asp:UpdatePanel ID="UpdatePanel662" runat="server">
                      <ContentTemplate>
					<asp:textbox id="maxod" runat="server" AutoPostBack="True" CssClass="CssStyle3"></asp:textbox>
					</ContentTemplate></asp:UpdatePanel>
					</TD>
				</TR>
				<TR>
					<TD noWrap align="left" colSpan="4">
						<asp:CheckBox id="post_ib_gl" runat="server" Text="User can post across Branch GL Accounts?"></asp:CheckBox><br>
						<asp:CheckBox id="viewprotected" runat="server" Text="User can View and Print Protected Accounts Balances?"></asp:CheckBox><br>
						<asp:CheckBox id="printstmt" runat="server" Text="User can print Account Statements?"></asp:CheckBox></TD>
				</TR>
				<TR>
					<TD align="center" colSpan="4"><FONT color="#ffffff"></FONT>
						<asp:datagrid id="DataGrid1" runat="server" Width="552px" AllowPaging="True" PageSize="1" ShowHeader="False"
							AutoGenerateColumns="False">
							<ItemStyle Font-Size="XX-Small"  ></ItemStyle>
							<HeaderStyle Font-Size="XX-Small"  Font-Bold="True"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="user_names" ReadOnly="True" HeaderText="UserId">
                                    <ItemStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
								<asp:TemplateColumn>
									<ItemTemplate>
										<asp:LinkButton id="LinkButton1" runat="server" Font-Size="X-Small" Font-Bold="True" CommandName="Delete">Site Membership</asp:LinkButton>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn>
									<ItemTemplate>
										<asp:LinkButton id="LinkButton2" runat="server" Font-Size="X-Small" Font-Bold="True" CommandName="Cancel">Role Membership</asp:LinkButton>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="user_id" HeaderText="user_id"></asp:BoundColumn>
                                <asp:TemplateColumn>
                                    <ItemTemplate>
                                    
                                    	<A href='Frm_UserMaint.aspx?p=<%# DataBinder.Eval(Container, "DataItem.user_id") %>'>
											Account Maintenance Entry</A>
                                         
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn>
                                    <ItemTemplate>
                                    <A href='Frm_UserMaintAppr.aspx?p=<%# DataBinder.Eval(Container, "DataItem.user_id") %>'>
											Account Maintenance Approval</A>
                                        
                                    </ItemTemplate>
                                </asp:TemplateColumn>
							</Columns>
							<PagerStyle Visible="False"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD align="center" colSpan="4"><asp:button id="Btn_Capture" runat="server" Text="Save" Width="67px"></asp:button>&nbsp;&nbsp;&nbsp;
						<asp:button id="btn_delete" runat="server" Text="Delete"></asp:button>&nbsp;
						<asp:button id="Button3" runat="server" Text="Reset" CausesValidation="False"></asp:button>&nbsp;&nbsp;&nbsp;</TD>
				</TR>
			</TABLE>
			<asp:validationsummary id="ValidationSummary1" style="Z-INDEX: 102; LEFT: 760px; POSITION: absolute; TOP: 350px"
				runat="server" ToolTip="The Following errors were  found !!!" 
                Font-Names="Comic Sans MS" HeaderText="The Following errors were found!!!"
				ShowMessageBox="True" ShowSummary="False"></asp:validationsummary> 
	</div></form></body></html>
