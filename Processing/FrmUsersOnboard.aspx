<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FrmUsersOnboard.aspx.vb" Inherits="Processing_FrmUsersOnboard" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>USSD user onboarding</title>
    <link rel="shortcut icon" href="/Resources/images/favicon.ico" />
    <link rel="preconnect" href="https://fonts.googleapis.com" />
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin="anonymous" />
    <link href="https://fonts.googleapis.com/css2?family=Plus+Jakarta+Sans:wght@400;500;600;700&display=swap" rel="stylesheet" />

    <style type="text/css">
        :root {
            --brand: #0F4C81; --brand-dark: #0a355a;
            --ink: #14212e; --muted: #5d6b7a;
            --line: #e1e8ef; --field: #d3dce6;
            --page: #f4f7fa; --surface: #ffffff; --tint: #eaf1f8;
            --ok: #1b6e3c; --ok-bg: #e6f4ea;
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
        .wrap { max-width: 1100px; margin: 0 auto; }

        .page-head {
            margin: 0 0 16px; padding: 20px 22px;
            background: var(--surface); border: 1px solid var(--line);
            border-left: 4px solid var(--brand); border-radius: 12px;
        }
        .page-head h1 { margin: 0 0 2px; font-size: 18px; font-weight: 700; letter-spacing: -.01em; }
        .page-head p { margin: 0; font-size: 12px; color: var(--muted); }

        .notice, .alert { display: block; margin-bottom: 12px; padding: 12px 14px; border-radius: 8px; line-height: 1.5; word-break: break-word; }
        .notice { font-weight: 600; color: var(--ok); background: var(--ok-bg); border-left: 4px solid var(--ok); }
        .alert { font-weight: 500; color: var(--bad); background: var(--bad-bg); border-left: 4px solid var(--bad); }
        .notice:empty, .alert:empty { display: none; }

        .card { margin-bottom: 16px; background: var(--surface); border: 1px solid var(--line); border-radius: 12px; overflow: hidden; }
        .card-head {
            display: flex; align-items: center; justify-content: space-between; gap: 12px; flex-wrap: wrap;
            padding: 14px 20px; border-bottom: 1px solid var(--line);
        }
        .card-head h2 { margin: 0; font-size: 14px; font-weight: 700; }
        .card-head .sub { font-size: 12px; color: var(--muted); font-weight: 500; }
        .card-body { padding: 18px 20px; }

        /* ---------- Forms ---------- */
        .form-grid { display: grid; grid-template-columns: repeat(2, minmax(0, 1fr)); gap: 16px 20px; }
        .form-grid.cols-3 { grid-template-columns: repeat(3, minmax(0, 1fr)); }
        .form-grid .full { grid-column: 1 / -1; }
        .form-label { display: block; margin-bottom: 6px; font-size: 12px; font-weight: 600; }
        .form-hint { display: block; margin-top: 5px; font-size: 12px; color: var(--muted); }
        .form-control, input.form-control, select.form-control {
            display: block; width: 100%; height: 36px; padding: 0 10px;
            font: inherit; font-size: 13px; color: var(--ink); background-color: var(--surface);
            border: 1.5px solid var(--field); border-radius: 9px; outline: none;
            transition: border-color .15s ease, box-shadow .15s ease;
        }
        .form-control:hover { border-color: #aebbc9; }
        .form-control:focus { border-color: var(--brand); box-shadow: 0 0 0 4px var(--ring); }
        textarea.form-control { height: auto; min-height: 72px; padding: 8px 10px; resize: vertical; }
        select.form-control {
            appearance: none; -webkit-appearance: none; padding-right: 34px; cursor: pointer;
            background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='12' height='8' viewBox='0 0 12 8' fill='none'%3E%3Cpath d='M1 1.5l5 5 5-5' stroke='%235d6b7a' stroke-width='1.6' stroke-linecap='round' stroke-linejoin='round'/%3E%3C/svg%3E");
            background-repeat: no-repeat; background-position: right 12px center;
        }
        .inline { display: flex; align-items: center; gap: 8px; flex-wrap: wrap; }
        .inline .form-control { flex: 1 1 180px; width: auto; }
        .field-error { display: block; margin-top: 5px; font-size: 12px; font-weight: 500; color: var(--bad); }
        .lookup { display: block; margin-top: 6px; font-size: 12px; font-weight: 600; color: var(--brand); }
        .lookup:empty { display: none; }

        .check { display: inline-flex; align-items: center; gap: 9px; }
        .check input[type="checkbox"] { width: 17px; height: 17px; margin: 0; accent-color: var(--brand); cursor: pointer; }
        .check label { cursor: pointer; font-weight: 500; }
        .check-list { display: grid; grid-template-columns: repeat(2, minmax(0, 1fr)); gap: 10px; }
        .check-list .check {
            padding: 12px 14px; border: 1.5px solid var(--line); border-radius: 10px;
            transition: border-color .15s ease, background-color .15s ease;
        }
        .check-list .check:hover { border-color: #aebbc9; }
        .check-list .check:has(input:checked) { border-color: var(--brand); background: var(--tint); }
        .check.danger input[type="checkbox"] { accent-color: var(--bad); }
        .check.danger:has(input:checked) label { color: var(--bad); }

        /* ---------- Grids ---------- */
        .table-wrap { overflow-x: auto; }
        table.data-grid { width: 100%; border: 0; border-collapse: collapse; }
        table.data-grid th, table.data-grid td { padding: 10px 14px; text-align: left; vertical-align: middle; border-top: 1px solid var(--line); }
        table.data-grid tr.grid-head th {
            border-top: 0; background: #f8fafc; white-space: nowrap;
            font-size: 11px; font-weight: 600; color: var(--muted);
            text-transform: uppercase; letter-spacing: .04em;
        }
        table.data-grid tr.grid-head a { color: var(--muted); text-decoration: none; }
        table.data-grid tr.grid-head a:hover { color: var(--brand); }
        table.data-grid tr:hover td { background: #f8fafc; }
        table.data-grid tr.is-selected td, table.data-grid tr.grid-edit td { background: var(--tint); }
        table.data-grid .c { text-align: center; }
        table.data-grid .nowrap { white-space: nowrap; }
        table.data-grid .strong { font-weight: 600; }
        table.data-grid .muted { color: var(--muted); }
        table.data-grid .actions { white-space: nowrap; text-align: right; }
        table.data-grid .form-control { min-width: 140px; height: 34px; }
        table.data-grid input[type="checkbox"] { width: 16px; height: 16px; margin: 0; accent-color: var(--brand); cursor: pointer; vertical-align: middle; }

        .pill { display: inline-block; padding: 3px 9px; font-size: 11px; font-weight: 600; border-radius: 999px; white-space: nowrap; }
        .pill.on { color: var(--ok); background: var(--ok-bg); }
        .pill.off { color: var(--muted); background: #eef2f6; }

        .link-btn {
            display: inline-block; margin-left: 4px; padding: 6px 12px; border: 0; cursor: pointer;
            font: inherit; font-size: 12px; font-weight: 600; text-decoration: none; border-radius: 8px;
        }
        .link-btn.edit, .link-btn.cancel { color: var(--brand); background: var(--tint); }
        .link-btn.edit:hover, .link-btn.cancel:hover { background: #dbe7f3; }
        .link-btn.save { color: #fff; background: var(--brand); }
        .link-btn.save:hover { background: var(--brand-dark); }
        .link-btn.delete { color: var(--bad); background: var(--bad-bg); }
        .link-btn.delete:hover { background: #f9d9d6; }

        table.data-grid tr.grid-pager td { padding: 12px; text-align: center; background: var(--surface); }
        table.data-grid tr.grid-pager:hover td { background: var(--surface); }
        table.data-grid tr.grid-pager table { margin: 0 auto; }
        table.data-grid tr.grid-pager table td { padding: 0; border: 0; }
        table.data-grid tr.grid-pager a,
        table.data-grid tr.grid-pager span {
            display: inline-block; min-width: 30px; margin: 0 2px; padding: 5px 8px;
            font-weight: 600; text-decoration: none; border-radius: 7px;
        }
        table.data-grid tr.grid-pager a { color: var(--brand); }
        table.data-grid tr.grid-pager a:hover { background: var(--tint); }
        table.data-grid tr.grid-pager span { color: #fff; background: var(--brand); }
        .empty { padding: 28px; text-align: center; color: var(--muted); }

        /* ---------- Buttons ---------- */
        .btn {
            display: inline-flex; align-items: center; justify-content: center;
            height: 36px; padding: 0 16px; font: inherit; font-size: 13px; font-weight: 600;
            text-decoration: none; white-space: nowrap; border: 1.5px solid transparent;
            border-radius: 9px; cursor: pointer; transition: background-color .15s ease, border-color .15s ease;
        }
        .btn:focus-visible { outline: none; box-shadow: 0 0 0 4px var(--ring); }
        .btn-primary { color: #fff; background: var(--brand); }
        .btn-primary:hover { background: var(--brand-dark); }
        .btn-secondary { color: var(--ink); background: var(--surface); border-color: var(--field); }
        .btn-secondary:hover { background: #f1f5f9; }
        .btn-danger { color: var(--bad); background: var(--surface); border-color: #f0c4c0; }
        .btn-danger:hover { background: var(--bad-bg); }
        .btn-link { color: var(--brand); background: none; padding: 0 6px; }
        .btn-link:hover { text-decoration: underline; }
        .text-link { font-weight: 600; color: var(--brand); text-decoration: none; white-space: nowrap; }
        .text-link:hover { text-decoration: underline; }

        .action-bar {
            position: sticky; bottom: 0; z-index: 5;
            display: flex; align-items: center; gap: 10px; flex-wrap: wrap;
            padding: 12px 20px; background: var(--surface);
            border: 1px solid var(--line); border-radius: 12px;
            box-shadow: 0 -6px 18px rgba(20, 33, 46, .06);
        }
        .action-bar .count {
            display: inline-flex; align-items: center; gap: 6px; margin-right: 6px;
            padding: 6px 12px; font-weight: 600; color: var(--brand);
            background: var(--tint); border-radius: 999px;
        }
        .action-bar .spacer { flex: 1; }
        .form-actions { display: flex; gap: 10px; flex-wrap: wrap; margin-top: 18px; }

        @media (max-width: 760px) {
            body { padding: 14px; }
            .form-grid, .form-grid.cols-3, .check-list { grid-template-columns: 1fr; }
            .card-body { padding: 14px; }
            .action-bar .spacer { display: none; }
            .action-bar .btn { flex: 1 1 auto; }
        }
        @media (prefers-reduced-motion: reduce) { * { transition: none !important; } }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="wrap">

        <header class="page-head">
            <h1>USSD user onboarding</h1>
            <p>Look up a mobile number, link it to an account, and create or delete the customer's USSD profile.</p>
        </header>

        <div class="notice" role="status"><asp:Literal ID="litResult" runat="server"></asp:Literal></div>
        <asp:CustomValidator ID="cus_val" runat="server" Display="Dynamic" CssClass="alert" />
        <div class="alert" role="alert"><asp:Literal ID="litError" runat="server"></asp:Literal></div>
        <asp:HyperLink ID="HyperLink1" Visible="false" runat="server">User Info...</asp:HyperLink>

        <section class="card">
            <div class="card-head">
                <h2>Profile</h2>
                <span class="check danger">
                    <asp:CheckBox ID="disabled" runat="server" Text="Disable user account" />
                </span>
            </div>
            <div class="card-body">
                <div class="form-grid">
                    <div>
                        <asp:Label runat="server" AssociatedControlID="users" CssClass="form-label">Mobile number (MSISDN)</asp:Label>
                        <div class="inline">
                            <asp:TextBox ID="users" runat="server" AutoPostBack="True" CssClass="form-control"
                                inputmode="tel" autocomplete="off" />
                            <asp:Button ID="cmd_go" runat="server" CausesValidation="False" Text="Go" CssClass="btn btn-primary" />
                            <a class="text-link" title="Look up a user" href="#"
                               onclick="window.open('Frm_searchUsers.aspx?textbox=users&p=names','cal','width=600,height=600,Scrollbars=yes'); return false;">Find user&hellip;</a>
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="users"
                            Display="Dynamic" CssClass="field-error" ToolTip="User Id empty"
                            ErrorMessage="User Id empty">Enter a mobile number.</asp:RequiredFieldValidator>
                    </div>
                    <div>
                        <asp:Label runat="server" AssociatedControlID="names" CssClass="form-label">Full names</asp:Label>
                        <asp:TextBox ID="names" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="names"
                            Display="Dynamic" CssClass="field-error" ToolTip="Names empty"
                            ErrorMessage="Names empty">Enter the customer's full names.</asp:RequiredFieldValidator>
                    </div>
                    <div>
                        <asp:Label runat="server" AssociatedControlID="txtAccountNo" CssClass="form-label">Account number</asp:Label>
                        <asp:TextBox ID="txtAccountNo" runat="server" CssClass="form-control" inputmode="numeric" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtAccountNo"
                            Display="Dynamic" CssClass="field-error" ErrorMessage="Account number empty">Enter the account number.</asp:RequiredFieldValidator>
                    </div>
                </div>

                <!-- Kept for the save routine; these are not shown on this page -->
                <asp:CheckBox ID="CheckAllEntries" runat="server" Visible="False" />
                <asp:CheckBox ID="CheckAudit" runat="server" Visible="False" />
                <asp:CheckBox ID="approval" runat="server" Visible="False" />

                <!-- Membership links (only appear if the code-behind binds this grid) -->
                <asp:DataGrid ID="DataGrid1" runat="server" CssClass="data-grid" GridLines="None"
                    AllowPaging="True" PageSize="1" ShowHeader="False" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundColumn DataField="user_names" ReadOnly="True" HeaderText="UserId"></asp:BoundColumn>
                        <asp:TemplateColumn>
                            <ItemTemplate><asp:LinkButton ID="LinkButton1" runat="server" CssClass="link-btn edit" CommandName="Delete">Location membership</asp:LinkButton></ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <ItemTemplate><asp:LinkButton ID="LinkButton2" runat="server" CssClass="link-btn edit" CommandName="Cancel">Role membership</asp:LinkButton></ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Supervisors">
                            <ItemTemplate><asp:LinkButton ID="Linkbutton3" runat="server" CssClass="link-btn edit" CommandName="Update">Supervisors</asp:LinkButton></ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn Visible="False" DataField="user_id" HeaderText="user_id"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="Maintenance Approving Types" Visible="False">
                            <ItemTemplate><asp:LinkButton ID="Linkbutton30" runat="server" CssClass="link-btn edit" CommandName="CmdMaintypeAppr" OnClick="Linkbutton30_Click">Maintenance approving types</asp:LinkButton></ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Maintenance Types" Visible="False">
                            <ItemTemplate><asp:LinkButton ID="Linkbutton13" runat="server" CssClass="link-btn edit" CommandName="cmdmaint" OnClick="Linkbutton13_Click">Maintenance types</asp:LinkButton></ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Company" Visible="False">
                            <ItemTemplate><asp:LinkButton ID="Linkbutton131" runat="server" CssClass="link-btn edit" CommandName="companies" OnClick="Linkbutton131_Click">Companies</asp:LinkButton></ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                    <PagerStyle Visible="False"></PagerStyle>
                </asp:DataGrid>
            </div>
        </section>

        <div class="action-bar">
            <asp:Button ID="Btn_Capture" runat="server" Text="Save" CssClass="btn btn-primary" />
            <asp:Button ID="Button3" runat="server" Text="Reset" CssClass="btn btn-secondary" CausesValidation="False" />
            <a class="btn btn-link" href="#" onclick="window.open('../Reports/DataViewerAllOnly.aspx?p=29'); return false;">Print preview&hellip;</a>
            <span class="spacer"></span>
            <asp:Button ID="btn_delete" runat="server" Text="Delete" CssClass="btn btn-danger" CausesValidation="False"
                OnClientClick="return confirm('Delete this USSD profile?');" />
        </div>

        <asp:ValidationSummary ID="ValidationSummary1" runat="server"
            HeaderText="The following errors were found:" ShowSummary="False" ShowMessageBox="True" />

    </div>
    </form>
</body>
</html>