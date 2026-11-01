<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Frm_UserReset.aspx.vb" Inherits="Processing_Frm_UserReset" Title="RESET LOCKED USERS" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Reset locked users</title>

    <script type="text/javascript">
        function Button1_onclick() {
            history.back();
        }
    </script>

    <link href="../Styles/SiteAll.css" rel="stylesheet" type="text/css" />
    <link rel="preconnect" href="https://fonts.googleapis.com" />
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin="anonymous" />
    <link href="https://fonts.googleapis.com/css2?family=Plus+Jakarta+Sans:wght@400;500;600;700&display=swap" rel="stylesheet" />

    <!-- Placed after SiteAll.css so these styles win -->
    <style type="text/css">
        :root {
            --brand: #0F4C81; --brand-dark: #0a355a;
            --ink: #14212e; --muted: #5d6b7a;
            --line: #e1e8ef; --field: #d3dce6;
            --page: #f4f7fa; --surface: #ffffff; --tint: #eaf1f8;
            --bad: #b3261e; --bad-bg: #fdeceb;
            --ring: rgba(15, 76, 129, .22);
        }
        * { box-sizing: border-box; }
        body {
            margin: 0; padding: 24px;
            font-family: "Plus Jakarta Sans", "Segoe UI", Roboto, Arial, sans-serif;
            font-size: 13px; color: var(--ink); background: var(--page);
            -webkit-font-smoothing: antialiased;
        }
        .wrap { max-width: 1000px; margin: 0 auto; }

        .page-head {
            margin: 0 0 16px; padding: 20px 22px;
            background: var(--surface); border: 1px solid var(--line);
            border-left: 4px solid var(--brand); border-radius: 12px;
        }
        .page-head h1 { margin: 0 0 2px; font-size: 18px; font-weight: 700; letter-spacing: -.01em; }
        .page-head p { margin: 0; font-size: 12px; color: var(--muted); }

        .card { margin-bottom: 16px; background: var(--surface); border: 1px solid var(--line); border-radius: 12px; }
        .card-head {
            display: flex; align-items: center; justify-content: space-between; gap: 12px;
            padding: 14px 20px; border-bottom: 1px solid var(--line);
        }
        .card-head h2 { margin: 0; font-size: 14px; font-weight: 700; }
        .card-head .sub { font-size: 12px; color: var(--muted); }
        .card-body { padding: 18px 20px; }
        .card-body.flush { padding: 0; }

        /* The old collapsible title has no expand/collapse behaviour on this page, so it is hidden */
        .legacy-hidden { display: none !important; }

        .filter-row { display: grid; grid-template-columns: 1fr 220px auto; gap: 12px; align-items: end; }
        .form-label { display: block; margin-bottom: 6px; font-size: 12px; font-weight: 600; }
        .form-control, input.form-control, select.form-control {
            display: block; width: 100%; height: 36px; padding: 0 10px;
            font: inherit; font-size: 13px; color: var(--ink); background-color: var(--surface);
            border: 1.5px solid var(--field); border-radius: 9px; outline: none;
            transition: border-color .15s ease, box-shadow .15s ease;
        }
        .form-control:hover { border-color: #aebbc9; }
        .form-control:focus { border-color: var(--brand); box-shadow: 0 0 0 4px var(--ring); }
        select.form-control {
            appearance: none; -webkit-appearance: none; padding-right: 34px; cursor: pointer;
            background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='12' height='8' viewBox='0 0 12 8' fill='none'%3E%3Cpath d='M1 1.5l5 5 5-5' stroke='%235d6b7a' stroke-width='1.6' stroke-linecap='round' stroke-linejoin='round'/%3E%3C/svg%3E");
            background-repeat: no-repeat; background-position: right 12px center;
        }

        /* ---------- Results grid ---------- */
        .table-wrap { overflow-x: auto; }
        table.data-grid { width: 100%; border: 0; border-collapse: collapse; }
        table.data-grid td { padding: 10px 14px; border-top: 1px solid var(--line); white-space: nowrap; vertical-align: middle; }
        table.data-grid tr.grid-head td {
            border-top: 0; background: #f8fafc;
            font-size: 11px; font-weight: 600; color: var(--muted);
            text-transform: uppercase; letter-spacing: .04em;
        }
        table.data-grid tr.grid-head a { color: var(--muted); text-decoration: none; }
        table.data-grid tr.grid-head a:hover { color: var(--brand); }
        table.data-grid tr.grid-alt td { background: #fafcfd; }
        table.data-grid tr:hover td { background: var(--tint); }
        table.data-grid tr.grid-head:hover td { background: #f8fafc; }
        table.data-grid input[type="checkbox"] { width: 17px; height: 17px; margin: 0; accent-color: var(--brand); cursor: pointer; }
        table.data-grid tr.grid-pager td { padding: 12px; text-align: center; background: var(--surface); }
        table.data-grid tr.grid-pager:hover td { background: var(--surface); }
        table.data-grid tr.grid-pager a,
        table.data-grid tr.grid-pager span {
            display: inline-block; min-width: 30px; margin: 0 2px; padding: 5px 8px;
            font-weight: 600; text-decoration: none; border-radius: 7px;
        }
        table.data-grid tr.grid-pager a { color: var(--brand); }
        table.data-grid tr.grid-pager a:hover { background: var(--tint); }
        table.data-grid tr.grid-pager span { color: #fff; background: var(--brand); }

        /* ---------- Reset options ---------- */
        .check { display: inline-flex; align-items: center; gap: 9px; }
        .check input[type="checkbox"] { width: 17px; height: 17px; margin: 0; accent-color: var(--brand); cursor: pointer; }
        .check label { cursor: pointer; font-weight: 500; }
        .check-list { display: grid; grid-template-columns: repeat(2, 1fr); gap: 10px; }
        .check-list .check {
            padding: 12px 14px; border: 1.5px solid var(--line); border-radius: 10px;
            transition: border-color .15s ease, background-color .15s ease;
        }
        .check-list .check:hover { border-color: #aebbc9; }
        .check-list .check:has(input:checked) { border-color: var(--brand); background: var(--tint); }

        .btn {
            display: inline-flex; align-items: center; justify-content: center;
            height: 36px; padding: 0 16px; font: inherit; font-size: 13px; font-weight: 600;
            text-decoration: none; white-space: nowrap; border: 1.5px solid transparent;
            border-radius: 9px; cursor: pointer; transition: background-color .15s ease;
        }
        .btn:focus-visible { outline: none; box-shadow: 0 0 0 4px var(--ring); }
        .btn-primary { color: #fff; background: var(--brand); }
        .btn-primary:hover { background: var(--brand-dark); }

        .action-bar {
            position: sticky; bottom: 0;
            display: flex; align-items: center; gap: 10px; flex-wrap: wrap;
            padding: 12px 20px; background: var(--surface);
            border: 1px solid var(--line); border-radius: 12px;
            box-shadow: 0 -6px 18px rgba(20, 33, 46, .06);
        }
        .action-bar .hint { color: var(--muted); font-size: 12px; }

        @media (max-width: 760px) {
            body { padding: 14px; }
            .filter-row, .check-list { grid-template-columns: 1fr; }
            .card-body { padding: 14px; }
            .action-bar .btn { flex: 1 1 auto; }
        }
        @media (prefers-reduced-motion: reduce) { * { transition: none !important; } }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="wrap">

        <header class="page-head">
            <h1>Reset locked and disabled users</h1>
            <p>Find users, tick the ones to reset, choose what to reset, then click Reset.</p>
        </header>

        <!-- ================= Search ================= -->
        <section class="card">
            <div class="card-head">
                <h2>Find users</h2>
                <asp:Panel ID="TitlePanel" runat="server" CssClass="legacy-hidden">
                    <asp:Image ID="Image2" runat="server" ImageUrl="~/images/expand_blue.jpg" />
                    <strong>Criteria</strong>
                    <asp:Label ID="Label1" runat="server">(Show Details...)</asp:Label>
                </asp:Panel>
            </div>
            <div class="card-body">
                <asp:Panel ID="Panel1" runat="server" CssClass="filter-row">
                    <div>
                        <asp:Label runat="server" AssociatedControlID="txtcri" CssClass="form-label">Search for</asp:Label>
                        <asp:TextBox ID="txtcri" runat="server" CssClass="form-control" />
                    </div>
                    <div>
                        <asp:Label runat="server" AssociatedControlID="drp_con" CssClass="form-label">Search by</asp:Label>
                        <asp:DropDownList ID="drp_con" runat="server" CssClass="form-control" ToolTip="Select criteria type">
                            <asp:ListItem Value="" Text="Choose a type"></asp:ListItem>
                            <asp:ListItem Value="2">Full names</asp:ListItem>
                            <asp:ListItem Value="3">Locked users</asp:ListItem>
                            <asp:ListItem Value="1">Logon grace count</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div>
                        <asp:Button ID="Button1" runat="server" Text="Search" CssClass="btn btn-primary" />
                    </div>
                </asp:Panel>
            </div>
        </section>

        <!-- ================= Results ================= -->
        <section class="card">
            <div class="card-head">
                <h2>Users</h2>
                <span class="sub">Tick the users to reset</span>
            </div>
            <div class="card-body flush table-wrap">
                <asp:DataGrid ID="DataGrid1" runat="server" CssClass="data-grid" GridLines="None"
                    AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True">
                    <HeaderStyle CssClass="grid-head" Wrap="False" />
                    <AlternatingItemStyle CssClass="grid-alt" Wrap="False" />
                    <ItemStyle Wrap="False" />
                    <Columns>
                        <asp:TemplateColumn HeaderText="Select">
                            <HeaderStyle Width="5px"></HeaderStyle>
                            <ItemTemplate>
                                <asp:CheckBox ID="chk" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chk2" runat="server"></asp:CheckBox>
                            </EditItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="Full Names" SortExpression="Full Names" HeaderText="Full names"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="User ID" SortExpression="User ID" HeaderText="User ID"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="Logon status">
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:Image ID="Image2" runat="server" ToolTip="User is currently Logged Out" Height="20px"
                                    Width="20px" ImageUrl="~/Images/buddy_offline.gif" AlternateText="User is currently Offline"></asp:Image>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="Textbox2" runat="server"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Locked status">
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="grace_count" SortExpression="grace_count" HeaderText="Logon grace count">
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="Account status">
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn Visible="False" SortExpression="user_id" HeaderText="Logonstatus ">
                            <ItemTemplate>
                                <asp:Label ID="Label2190" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.user_id") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox4190" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.user_id") %>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn Visible="False" SortExpression="locked" HeaderText="statuslocked">
                            <ItemTemplate>
                                <asp:Label ID="Label4" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.locked") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox5" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.locked") %>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn Visible="False" SortExpression="disabled" HeaderText="statusaccount">
                            <ItemTemplate>
                                <asp:Label ID="Label5" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.disabled") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox6" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.disabled") %>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                    <PagerStyle CssClass="grid-pager" HorizontalAlign="Center" Mode="NumericPages"></PagerStyle>
                </asp:DataGrid>
            </div>
        </section>

        <!-- ================= Reset options ================= -->
        <section class="card">
            <div class="card-head">
                <h2>What to reset</h2>
            </div>
            <div class="card-body">
                <div class="check-list">
                    <span class="check"><asp:CheckBox ID="chk_lock" runat="server" Text="Reset locked users only" /></span>
                    <span class="check"><asp:CheckBox ID="chk_log" runat="server" Text="Clear user from logon log" /></span>
                    <span class="check"><asp:CheckBox ID="chk_disabled" runat="server" Text="Reset locked and disabled users" /></span>
                    <span class="check"><asp:CheckBox ID="chk_all" runat="server" Text="Reset all" /></span>
                    <span class="check"><asp:CheckBox ID="chk_pwds" runat="server" Text="Reset user's password only" /></span>
                </div>
            </div>
        </section>

        <div class="action-bar">
            <asp:Button ID="Btn_reset" runat="server" Text="Reset" CssClass="btn btn-primary" />
            <span class="hint">Applies to the users ticked above.</span>
        </div>

        <asp:ValidationSummary ID="ValidationSummary1" runat="server"
            HeaderText="The following errors were found:" ShowMessageBox="True" ShowSummary="False" />

    </div>
    </form>
</body>
</html>