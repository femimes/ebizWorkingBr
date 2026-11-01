<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Frm_Users.aspx.vb" Inherits="Processing_Frm_Users" Title="Sms Classique" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Users Maintenance</title>

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
            --brand: #0F4C81;
            --brand-dark: #0a355a;
            --ink: #14212e;
            --muted: #5d6b7a;
            --line: #e1e8ef;
            --field: #d3dce6;
            --page: #f4f7fa;
            --surface: #ffffff;
            --tint: #eaf1f8;
            --bad: #b3261e;  --bad-bg: #fdeceb;
            --ring: rgba(15, 76, 129, .22);
        }
        * { box-sizing: border-box; }
        body {
            margin: 0;
            padding: 28px;
            font-family: "Plus Jakarta Sans", "Segoe UI", Roboto, Arial, sans-serif;
            font-size: 13px;
            color: var(--ink);
            background: var(--page);
            -webkit-font-smoothing: antialiased;
        }
        .wrap { max-width: 1000px; margin: 0 auto; }

        /* ---------- Page header ---------- */
        .page-head {
            margin: 0 0 16px;
            padding: 22px 24px;
            background: var(--surface);
            border: 1px solid var(--line);
            border-left: 4px solid var(--brand);
            border-radius: 12px;
        }
        .page-head h1 { margin: 0 0 2px; font-size: 18px; font-weight: 700; letter-spacing: -.01em; }
        .page-head p { margin: 0; font-size: 12px; color: var(--muted); }

        /* ---------- Cards ---------- */
        .card {
            margin-bottom: 16px;
            background: var(--surface);
            border: 1px solid var(--line);
            border-radius: 12px;
        }
        .card-head {
            display: flex; align-items: center; justify-content: space-between; gap: 12px;
            padding: 16px 22px;
            border-bottom: 1px solid var(--line);
        }
        .card-head h2 { margin: 0; font-size: 14px; font-weight: 700; }
        .card-head .sub { font-size: 12px; color: var(--muted); }
        .card-body { padding: 20px 22px; }

        /* ---------- Fields ---------- */
        .field-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 18px 20px; }
        .field-grid .full { grid-column: 1 / -1; }
        .form-label { display: block; margin-bottom: 6px; font-size: 12px; font-weight: 600; }

        .form-control,
        input.form-control {
            display: block;
            width: 100%;
            height: 36px;
            padding: 0 10px;
            font: inherit;
            font-size: 13px;
            color: var(--ink);
            background: var(--surface);
            border: 1.5px solid var(--field);
            border-radius: 9px;
            outline: none;
            transition: border-color .15s ease, box-shadow .15s ease;
        }
        .form-control:hover { border-color: #aebbc9; }
        .form-control:focus { border-color: var(--brand); box-shadow: 0 0 0 4px var(--ring); }
        .form-control.amount { text-align: right; font-variant-numeric: tabular-nums; }

        .inline { display: flex; align-items: center; gap: 8px; flex-wrap: wrap; }
        .inline .form-control { flex: 1 1 180px; width: auto; }

        .field-error { display: block; margin-top: 5px; font-size: 12px; font-weight: 500; color: var(--bad); }

        .alert {
            display: block;
            margin-bottom: 16px;
            padding: 12px 14px;
            font-weight: 500;
            color: var(--bad);
            background: var(--bad-bg);
            border-left: 4px solid var(--bad);
            border-radius: 8px;
        }

        /* ---------- Checkboxes (asp:CheckBox renders input + label) ---------- */
        .check { display: inline-flex; align-items: center; gap: 9px; }
        .check input[type="checkbox"] { width: 18px; height: 18px; margin: 0; accent-color: var(--brand); cursor: pointer; }
        .check label { cursor: pointer; font-weight: 500; }

        .check-list { display: grid; grid-template-columns: repeat(3, 1fr); gap: 12px; }
        .check-list .check {
            padding: 14px 16px;
            border: 1.5px solid var(--line);
            border-radius: 10px;
            transition: border-color .15s ease, background-color .15s ease;
        }
        .check-list .check:hover { border-color: #aebbc9; }
        .check-list .check:has(input:checked) { border-color: var(--brand); background: var(--tint); }

        .check.danger input[type="checkbox"] { accent-color: var(--bad); }
        .check.danger:has(input:checked) label { color: var(--bad); }

        /* ---------- Limits tables ---------- */
        .table-wrap { overflow-x: auto; }
        table.limits { width: 100%; border-collapse: collapse; }
        table.limits th, table.limits td { padding: 8px 10px; text-align: left; }
        table.limits thead th { font-size: 11px; font-weight: 600; color: var(--muted); text-transform: uppercase; letter-spacing: .04em; }
        table.limits tbody th { width: 170px; font-weight: 600; white-space: nowrap; }
        table.limits td { min-width: 150px; }

        /* ---------- Related actions (the DataGrid) ---------- */
        table.actions-grid { width: 100%; border: 0; border-collapse: collapse; }
        table.actions-grid td { padding: 6px 6px 6px 0; border: 0; }
        table.actions-grid td:first-child { font-weight: 700; padding-right: 14px; white-space: nowrap; }
        .action-link {
            display: inline-block;
            padding: 7px 12px;
            font-size: 12px;
            font-weight: 600;
            color: var(--brand);
            text-decoration: none;
            white-space: nowrap;
            background: var(--tint);
            border-radius: 8px;
        }
        .action-link:hover { background: #dbe7f3; }
        .empty-note { margin: 0; color: var(--muted); }

        /* ---------- Buttons ---------- */
        .btn {
            display: inline-flex; align-items: center; justify-content: center;
            height: 36px;
            padding: 0 16px;
            font: inherit;
            font-size: 13px;
            font-weight: 600;
            text-decoration: none;
            white-space: nowrap;
            border: 1.5px solid transparent;
            border-radius: 9px;
            cursor: pointer;
            transition: background-color .15s ease, border-color .15s ease;
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
            position: sticky; bottom: 0;
            display: flex; align-items: center; gap: 10px; flex-wrap: wrap;
            padding: 14px 22px;
            background: var(--surface);
            border: 1px solid var(--line);
            border-radius: 12px;
            box-shadow: 0 -6px 18px rgba(20, 33, 46, .06);
        }
        .action-bar .spacer { flex: 1; }

        /* ---------- Responsive ---------- */
        @media (max-width: 760px) {
            body { padding: 14px; }
            .field-grid { grid-template-columns: 1fr; }
            .check-list { grid-template-columns: 1fr; }
            .card-body { padding: 16px; }
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
            <h1>Users maintenance</h1>
            <p>Set up a user's account, permissions and posting limits.</p>
        </header>

        <asp:CustomValidator ID="cus_val" runat="server" Display="Dynamic" CssClass="alert" />

        <!-- ================= Account ================= -->
        <section class="card">
            <div class="card-head">
                <h2>Account</h2>
                <span class="check danger">
                    <asp:CheckBox ID="disabled" runat="server" Text="Disable user account" />
                </span>
            </div>
            <div class="card-body">
                <div class="field-grid">
                    <div>
                        <asp:Label runat="server" AssociatedControlID="users" CssClass="form-label">User ID</asp:Label>
                        <div class="inline">
                            <asp:TextBox ID="users" runat="server" AutoPostBack="True" CssClass="form-control"
                                autocomplete="off" spellcheck="false" />
                            <asp:Button ID="cmd_go" runat="server" CausesValidation="False" Text="Go" CssClass="btn btn-primary" />
                            <asp:HyperLink ID="HyperLink1" Visible="false" runat="server">User Info...</asp:HyperLink>
                            <a class="text-link" title="Look up a user" href="#"
                               onclick="window.open('Frm_searchUsers.aspx?textbox=users&p=names','cal','width=600,height=600,Scrollbars=yes'); return false;">Find user&hellip;</a>
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="users"
                            Display="Dynamic" CssClass="field-error" ToolTip="User Id empty"
                            ErrorMessage="User Id empty">Enter a user ID.</asp:RequiredFieldValidator>
                    </div>

                    <div>
                        <asp:Label runat="server" AssociatedControlID="names" CssClass="form-label">Full names</asp:Label>
                        <asp:TextBox ID="names" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="names"
                            Display="Dynamic" CssClass="field-error" ToolTip="Names empty"
                            ErrorMessage="Names empty">Enter the user's full names.</asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </section>

        <!-- ================= Permissions ================= -->
        <section class="card">
            <div class="card-head">
                <h2>Permissions</h2>
                <span class="sub">What this user can do with entries</span>
            </div>
            <div class="card-body">
                <div class="check-list">
                    <span class="check"><asp:CheckBox ID="CheckAllEntries" runat="server" Text="Can amend entries" /></span>
                    <span class="check"><asp:CheckBox ID="CheckAudit" runat="server" Text="Can review / confirm entries" /></span>
                    <span class="check"><asp:CheckBox ID="approval" runat="server" Text="Can approve entries" /></span>
                </div>
            </div>
        </section>

        <!-- ================= Data posting limits ================= -->
        <section class="card">
            <div class="card-head">
                <h2>Data posting limits</h2>
                <span class="sub">Amounts this user can post</span>
            </div>
            <div class="card-body table-wrap">
                <table class="limits">
                    <thead>
                        <tr>
                            <th scope="col"></th>
                            <th scope="col">Minimum limit</th>
                            <th scope="col">Maximum limit</th>
                            <th scope="col">Ceiling amount</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <th scope="row">Cash receipts</th>
                            <td><asp:TextBox ID="minrec" runat="server" AutoPostBack="True" CssClass="form-control amount CssStyle3" inputmode="decimal" aria-label="Cash receipts minimum limit" /></td>
                            <td><asp:TextBox ID="maxrec" runat="server" AutoPostBack="True" CssClass="form-control amount CssStyle3" inputmode="decimal" aria-label="Cash receipts maximum limit" /></td>
                            <td><asp:TextBox ID="ceilrec" runat="server" AutoPostBack="True" CssClass="form-control amount CssStyle3" inputmode="decimal" aria-label="Cash receipts ceiling amount" /></td>
                        </tr>
                        <tr>
                            <th scope="row">Cash payments</th>
                            <td><asp:TextBox ID="minpay" runat="server" AutoPostBack="True" CssClass="form-control amount CssStyle3" inputmode="decimal" aria-label="Cash payments minimum limit" /></td>
                            <td><asp:TextBox ID="maxpay" runat="server" AutoPostBack="True" CssClass="form-control amount CssStyle3" inputmode="decimal" aria-label="Cash payments maximum limit" /></td>
                            <td><asp:TextBox ID="ceilpay" runat="server" AutoPostBack="True" CssClass="form-control amount CssStyle3" inputmode="decimal" aria-label="Cash payments ceiling amount" /></td>
                        </tr>
                        <tr>
                            <th scope="row">Non-cash payments</th>
                            <td><asp:TextBox ID="minnoncash" runat="server" AutoPostBack="True" CssClass="form-control amount CssStyle3" inputmode="decimal" aria-label="Non-cash payments minimum limit" /></td>
                            <td><asp:TextBox ID="maxnoncash" runat="server" AutoPostBack="True" CssClass="form-control amount CssStyle3" inputmode="decimal" aria-label="Non-cash payments maximum limit" /></td>
                            <td><asp:TextBox ID="ceilnoncash" runat="server" AutoPostBack="True" CssClass="form-control amount CssStyle3" inputmode="decimal" aria-label="Non-cash payments ceiling amount" /></td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </section>

        <!-- ================= Posting approval limits ================= -->
        <section class="card">
            <div class="card-head">
                <h2>Posting approval limits</h2>
                <span class="sub">Amounts this user can approve</span>
            </div>
            <div class="card-body table-wrap">
                <table class="limits">
                    <thead>
                        <tr>
                            <th scope="col"></th>
                            <th scope="col">Minimum approval limit</th>
                            <th scope="col">Maximum approval limit</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <th scope="row">Cash receipts</th>
                            <td><asp:TextBox ID="minrec1" runat="server" AutoPostBack="True" CssClass="form-control amount CssStyle3" inputmode="decimal" aria-label="Cash receipts minimum approval limit" /></td>
                            <td><asp:TextBox ID="maxrec1" runat="server" AutoPostBack="True" CssClass="form-control amount CssStyle3" inputmode="decimal" aria-label="Cash receipts maximum approval limit" /></td>
                        </tr>
                        <tr>
                            <th scope="row">Cash payments</th>
                            <td><asp:TextBox ID="minpay1" runat="server" AutoPostBack="True" CssClass="form-control amount CssStyle3" inputmode="decimal" aria-label="Cash payments minimum approval limit" /></td>
                            <td><asp:TextBox ID="maxpay1" runat="server" AutoPostBack="True" CssClass="form-control amount CssStyle3" inputmode="decimal" aria-label="Cash payments maximum approval limit" /></td>
                        </tr>
                        <tr>
                            <th scope="row">Non-cash payments</th>
                            <td><asp:TextBox ID="minnoncash1" runat="server" AutoPostBack="True" CssClass="form-control amount CssStyle3" inputmode="decimal" aria-label="Non-cash payments minimum approval limit" /></td>
                            <td><asp:TextBox ID="maxnoncash1" runat="server" AutoPostBack="True" CssClass="form-control amount CssStyle3" inputmode="decimal" aria-label="Non-cash payments maximum approval limit" /></td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </section>

        <!-- ================= Memberships (DataGrid, unchanged IDs and commands) ================= -->
        <section class="card">
            <div class="card-head">
                <h2>Memberships</h2>
                <span class="sub">Shown once a user is loaded</span>
            </div>
            <div class="card-body table-wrap">
                <asp:DataGrid ID="DataGrid1" runat="server" CssClass="actions-grid" GridLines="None"
                    AllowPaging="True" PageSize="1" ShowHeader="False" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundColumn DataField="user_names" ReadOnly="True" HeaderText="UserId"></asp:BoundColumn>
                        <asp:TemplateColumn>
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" CssClass="action-link" CommandName="Delete">Location membership</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton2" runat="server" CssClass="action-link" CommandName="Cancel">Role membership</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Supervisors">
                            <ItemTemplate>
                                <asp:LinkButton ID="Linkbutton3" runat="server" CssClass="action-link" CommandName="Update">Supervisors</asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="Linkbutton4" runat="server" CssClass="action-link" CommandName="Update">Supervisors</asp:LinkButton>
                            </EditItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn Visible="False" DataField="user_id" HeaderText="user_id"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="Maintenance Approving Types" Visible="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="Linkbutton30" runat="server" CssClass="action-link"
                                    CommandName="CmdMaintypeAppr" OnClick="Linkbutton30_Click">Maintenance approving types</asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="Linkbutton40" runat="server" CssClass="action-link" CommandName="CmdMaintypeAppr">Maintenance approving types</asp:LinkButton>
                            </EditItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Maintenance Types" Visible="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="Linkbutton13" runat="server" CssClass="action-link"
                                    CommandName="cmdmaint" OnClick="Linkbutton13_Click">Maintenance types</asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="Linkbutton14" runat="server" CssClass="action-link" CommandName="Update">Maintenance types</asp:LinkButton>
                            </EditItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Company" Visible="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="Linkbutton131" runat="server" CssClass="action-link"
                                    CommandName="companies" OnClick="Linkbutton131_Click">Companies</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                    <PagerStyle Visible="False"></PagerStyle>
                </asp:DataGrid>
            </div>
        </section>

        <!-- ================= Actions ================= -->
        <div class="action-bar">
            <asp:Button ID="Btn_Capture" runat="server" Text="Save" CssClass="btn btn-primary" />
            <asp:Button ID="Button3" runat="server" Text="Reset" CssClass="btn btn-secondary" />
            <a class="btn btn-link" href="#" onclick="window.open('../Reports/DataViewerAllOnly.aspx?p=29'); return false;">Print preview&hellip;</a>
            <span class="spacer"></span>
            <asp:Button ID="btn_delete" runat="server" Text="Delete" CssClass="btn btn-danger" />
        </div>

        <asp:ValidationSummary ID="ValidationSummary1" runat="server"
            HeaderText="The following errors were found:" ShowSummary="False" ShowMessageBox="True" />

    </div>
    </form>
</body>
</html>
