<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Frm_Roles.aspx.vb" Inherits="Processing_Frm_Roles" Title="Roles & Permission" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>User roles maintenance</title>

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
        .card-body { padding: 18px 20px; }

        .field-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 16px 20px; }
        .form-label { display: block; margin-bottom: 6px; font-size: 12px; font-weight: 600; }
        .form-control, input.form-control {
            display: block; width: 100%; height: 36px; padding: 0 10px;
            font: inherit; font-size: 13px; color: var(--ink); background: var(--surface);
            border: 1.5px solid var(--field); border-radius: 9px; outline: none;
            transition: border-color .15s ease, box-shadow .15s ease;
        }
        .form-control:hover { border-color: #aebbc9; }
        .form-control:focus { border-color: var(--brand); box-shadow: 0 0 0 4px var(--ring); }
        .inline { display: flex; align-items: center; gap: 8px; flex-wrap: wrap; }
        .inline .form-control { flex: 1 1 160px; width: auto; }
        .field-error { display: block; margin-top: 5px; font-size: 12px; font-weight: 500; color: var(--bad); }
        .alert {
            display: block; margin-bottom: 16px; padding: 12px 14px; font-weight: 500;
            color: var(--bad); background: var(--bad-bg); border-left: 4px solid var(--bad); border-radius: 8px;
        }

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
        .text-link { font-weight: 600; color: var(--brand); text-decoration: none; white-space: nowrap; }
        .text-link:hover { text-decoration: underline; }

        .action-bar {
            display: flex; align-items: center; gap: 10px; flex-wrap: wrap;
            padding: 12px 20px; background: var(--surface);
            border: 1px solid var(--line); border-radius: 12px;
        }
        .action-bar .spacer { flex: 1; }

        @media (max-width: 760px) {
            body { padding: 14px; }
            .field-grid { grid-template-columns: 1fr; }
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
            <h1>User roles maintenance</h1>
            <p>Create or update a role, then set what it can do under Permissions.</p>
        </header>

        <asp:CustomValidator ID="cus_val" runat="server" Display="Dynamic" ControlToValidate="role_id" CssClass="alert" />
        <asp:HyperLink ID="HyperLink1" runat="server" Visible="false">Search Roles Info ..</asp:HyperLink>

        <section class="card">
            <div class="card-head">
                <h2>Role</h2>
            </div>
            <div class="card-body">
                <div class="field-grid">
                    <div>
                        <asp:Label runat="server" AssociatedControlID="role_id" CssClass="form-label">Role ID</asp:Label>
                        <div class="inline">
                            <asp:TextBox ID="role_id" runat="server" AutoPostBack="True" CssClass="form-control"
                                autocomplete="off" spellcheck="false" />
                            <asp:Button ID="cmd_go" runat="server" Text="Go" CausesValidation="False" CssClass="btn btn-primary" />
                            <a class="text-link" title="Look up a role" href="#"
                               onclick="window.open('Frm_searchRoles.aspx?textbox=role_id&p=role_desc','cal','width=600,height=600'); return false;">Find role&hellip;</a>
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="role_id"
                            Display="Dynamic" CssClass="field-error" ToolTip="User Id empty"
                            ErrorMessage="User Id empty">Enter a role ID.</asp:RequiredFieldValidator>
                    </div>

                    <div>
                        <asp:Label runat="server" AssociatedControlID="role_desc" CssClass="form-label">Description</asp:Label>
                        <asp:TextBox ID="role_desc" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="role_desc"
                            Display="Dynamic" CssClass="field-error" ToolTip="Names empty"
                            ErrorMessage="Names empty">Enter a description.</asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </section>

        <div class="action-bar">
            <asp:Button ID="Btn_Capture" runat="server" Text="Save" CssClass="btn btn-primary" />
            <asp:Button ID="btn_perm" runat="server" Text="Permissions" CssClass="btn btn-secondary" />
            <asp:Button ID="Button3" runat="server" Text="Reset" CssClass="btn btn-secondary" />
            <span class="spacer"></span>
            <asp:Button ID="btn_delete" runat="server" Text="Delete" CssClass="btn btn-danger" />
        </div>

        <asp:ValidationSummary ID="ValidationSummary1" runat="server"
            HeaderText="The following errors were found:" ShowMessageBox="True" ShowSummary="False" />

    </div>
    </form>
</body>
</html>