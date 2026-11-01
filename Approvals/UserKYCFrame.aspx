<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserKYCFrame.aspx.cs" Inherits="Approvals_UserKYCFrame" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>KYC document</title>
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

        /* ---------- Extras ---------- */
        .stat-grid { display: grid; grid-template-columns: repeat(4, minmax(0, 1fr)); gap: 12px; }
        .stat { padding: 12px 14px; background: #f8fafc; border: 1px solid var(--line); border-radius: 10px; }
        .stat .label { font-size: 11px; font-weight: 600; color: var(--muted); text-transform: uppercase; letter-spacing: .04em; }
        .stat input.stat-value {
            display: block; width: 100%; margin-top: 4px; padding: 0; border: 0; background: transparent;
            font: inherit; font-size: 18px; font-weight: 700; color: var(--ink); font-variant-numeric: tabular-nums; outline: none;
        }
        .stat.key { background: var(--tint); border-color: #c9dbee; }
        .stat.good input.stat-value { color: var(--ok); }
        .stat.bad input.stat-value { color: var(--bad); }
        .scroll-y { max-height: 520px; overflow: auto; }
        table.data-grid .num { text-align: right; font-variant-numeric: tabular-nums; white-space: nowrap; }
        table.data-grid thead th, table.data-grid tr.grid-head th { position: sticky; top: 0; z-index: 1; }
        .busy-overlay {
            position: fixed; inset: 0; z-index: 50; display: none; align-items: center; justify-content: center;
            background: rgba(244, 247, 250, .8); font-weight: 600; color: var(--brand);
        }
        .busy-overlay.show { display: flex; }
        .busy-overlay .spinner {
            width: 28px; height: 28px; margin-right: 12px; border-radius: 50%;
            border: 3px solid #c9dbee; border-top-color: var(--brand); animation: spin .8s linear infinite;
        }
        @keyframes spin { to { transform: rotate(360deg); } }
        @media (max-width: 760px) { .stat-grid { grid-template-columns: repeat(2, minmax(0, 1fr)); } }
        .kv { display: grid; grid-template-columns: max-content 1fr; gap: 6px 18px; margin: 0 0 16px; }
        .kv dt { color: var(--muted); font-weight: 600; }
        .kv dd { margin: 0; word-break: break-word; }
        .kyc-img { display: block; max-width: 100%; height: auto; margin-top: 16px; border: 1px solid var(--line); border-radius: 8px; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="wrap">

        <header class="page-head">
            <h1>KYC document</h1>
            <p>Details of the document a customer uploaded.</p>
        </header>

        <div class="alert" role="alert"><asp:Literal ID="litError" runat="server"></asp:Literal></div>

        <asp:Panel ID="pnlDoc" runat="server" CssClass="card">
            <div class="card-body">
                <asp:Repeater ID="rptFields" runat="server">
                    <HeaderTemplate><dl class="kv"></HeaderTemplate>
                    <ItemTemplate><dt><%# HttpUtility.HtmlEncode(Eval("Key")) %></dt><dd><%# HttpUtility.HtmlEncode(Eval("Value")) %></dd></ItemTemplate>
                    <FooterTemplate></dl></FooterTemplate>
                </asp:Repeater>
                <asp:HyperLink ID="lnkDoc" runat="server" Target="_blank" CssClass="btn btn-secondary" Visible="false">Open document</asp:HyperLink>
                <asp:Image ID="Image1" runat="server" CssClass="kyc-img" Visible="false" AlternateText="Uploaded KYC document" />
            </div>
        </asp:Panel>
    </div>
    </form>
</body>
</html>