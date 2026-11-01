<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FrmTelWorkSites.aspx.vb" Inherits="Processing_FrmTelWorkSites" %>
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
<TABLE id="Table1" style="Z-INDEX: 101;Z-INDEX: 101; LEFT: 8px; WIDTH: 736px; POSITION: absolute; TOP: 8px; HEIGHT: 56px; border-right: thin outset; border-top: thin outset; border-left: thin outset; border-bottom: thin outset;" width="736">
				<TR>
					<TD colSpan="3"><STRONG>eTELLER WORKING SITES </STRONG>
					</TD>
				</TR>
				<TR>
					<TD colSpan="3">
						<asp:Label id="user_id" runat="server" Font-Names="Comic Sans MS" Font-Size="X-Small"></asp:Label>
					</TD>
				</TR>
				<TR>
					<TD align="right"><STRONG>Avaliable Items</STRONG>
					</TD>
					<TD style="WIDTH: 101px"><FONT color="#ffffff" size="2"><STRONG></STRONG></FONT></TD>
					<TD><STRONG>Selected&nbsp;Items</STRONG></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 300px" vAlign="top" align="right" colSpan="1" rowSpan="1"><asp:listbox id="ListBox1" runat="server" Height="80px" Width="303px"></asp:listbox></TD>
					<TD style="WIDTH: 101px" align="center"><asp:imagebutton id="ImageButton1" 
                            runat="server" BorderStyle="Outset" ImageUrl="~/Images/button-play.gif"></asp:imagebutton><br>
						<asp:imagebutton id="ImageButton2" runat="server" BorderStyle="Outset" 
                            ImageUrl="~/Images/button-playLeft.gif"></asp:imagebutton></TD>
					<TD vAlign="top" rowSpan="1"><asp:listbox id="ListBox2" runat="server" Height="80px" Width="328px"></asp:listbox></TD>
				</TR>
				<TR>
					<TD align="center" colSpan="3"><asp:button id="Button1" runat="server" Text="Back To User Profiles"></asp:button></TD>
				</TR>
			</TABLE>
			<asp:validationsummary id="ValidationSummary1" style="Z-INDEX: 102; LEFT: 808px; POSITION: absolute; TOP: 336px"
				runat="server" ToolTip="The Following errors were  found !!!" ShowSummary="False" ShowMessageBox="True"
				HeaderText="The Following errors were found!!!" Font-Names="Comic Sans MS"></asp:validationsummary>
</div></form></body></html>
