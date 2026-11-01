<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FrmTelSupervisors.aspx.vb" Inherits="Processing_FrmTelSupervisors" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
       <link href="../App_Themes/White/Default.css" rel="stylesheet" type="text/css" />
 
<script language="javascript" type="text/javascript">
    // <!CDATA[

    function Button1_onclick() {
        history.back();
    }

    // ]]>
</script>
    <link href="../Styles/SiteAll.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">  <div>
<TABLE id="Table1" style="Z-INDEX: 101; LEFT: 8px; WIDTH: 736px; POSITION: absolute; TOP: 8px; HEIGHT: 56px; border-right: thin outset; border-top: thin outset; border-left: thin outset; border-bottom: thin outset;" width="736">
				<TR>
					<TD colSpan="5"><STRONG>Teller SUPERVISORS </STRONG>
					</TD>
				</TR>
				<TR>
					<TD colSpan="5">
						<asp:Label id="user_id" runat="server" Font-Names="Comic Sans MS" Font-Size="X-Small"></asp:Label></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 17px" align="right"><STRONG>Avaliable Items</STRONG>
					</TD>
					<TD style="WIDTH: 101px; HEIGHT: 17px"><FONT color="#ffffff" size="2"><STRONG></STRONG></FONT></TD>
					<TD style="HEIGHT: 17px" colSpan="3"><STRONG>Selected&nbsp;Items</STRONG></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 300px" vAlign="top" align="right" colSpan="1" rowSpan="1"><asp:listbox id="ListBox1" runat="server" Width="303px" Height="80px"></asp:listbox></TD>
					<TD style="WIDTH: 101px" align="center"><asp:imagebutton id="ImageButton1" 
                            runat="server" ImageUrl="~/Images/button-play.gif"
							BorderStyle="Outset"></asp:imagebutton><br>
						<asp:imagebutton id="ImageButton2" runat="server" ImageUrl="~/Images/button-playLeft.gif"
							BorderStyle="Outset"></asp:imagebutton></TD>
					<TD vAlign="top" colSpan="3" rowSpan="1"><asp:listbox id="ListBox2" runat="server" Width="328px" Height="80px"></asp:listbox></TD>
				</TR>
				<TR>
					<TD align="center" colSpan="5"><asp:button id="Button1" runat="server" Text="Back To User Profiles"></asp:button></TD>
				</TR>
			</TABLE>
			<asp:validationsummary id="ValidationSummary1" style="Z-INDEX: 102; LEFT: 808px; POSITION: absolute; TOP: 336px"
				runat="server" Font-Names="Comic Sans MS" HeaderText="The Following errors were found!!!" ShowMessageBox="True"
				ShowSummary="False" ToolTip="The Following errors were  found !!!"></asp:validationsummary>
</div></form></body></html>
