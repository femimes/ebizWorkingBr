<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmBulkImageUploadAx.aspx.cs" Inherits="eOperations_FrmBulkImageUploadAx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Mandate bulk upload</title>
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
        .schedule-row { display: flex; flex-wrap: wrap; align-items: center; gap: 12px 18px; }
        .schedule-every { display: inline-flex; align-items: center; gap: 8px; }
        .schedule-every .form-control { width: auto; min-width: 130px; }
        .schedule-info { margin: 10px 0 0; }
    </style>
    <style type="text/css">
        .picker { display: grid; grid-template-columns: minmax(0, 1fr) minmax(0, 1fr); gap: 16px; align-items: start; }
        .picker .card { margin-bottom: 0; }
        .picker .scroll-y { max-height: 460px; }
        .card-head .tools { display: flex; align-items: center; gap: 10px; flex-wrap: wrap; }
        .card-head .search { width: 200px; height: 32px; }
        .filter-row { display: flex; align-items: flex-end; gap: 12px; flex-wrap: wrap; }
        .filter-row > div { flex: 0 1 380px; }
        .file-name { font-size: 12px; color: var(--muted); word-break: break-all; }
        tr.is-hidden { display: none; }
        .count-badge { font-size: 12px; font-weight: 600; color: var(--muted); }
        @media (max-width: 900px) { .picker { grid-template-columns: 1fr; } }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="wrap">

        <header class="page-head">
            <h1>Mandate / signature bulk upload</h1>
            <p>Mandate images waiting on the server are listed on the left. Tick the accounts and click <strong>Upload selection</strong> to save their images to the mandate store; uploaded accounts appear on the right, where <strong>Delete selection</strong> removes them again.</p>
        </header>

        <div class="notice" role="status"><asp:Literal ID="litResult" runat="server" /></div>
        <div class="alert" role="alert"><asp:Literal ID="litError" runat="server" /></div>

        <section class="card">
            <div class="card-body">
                <div class="filter-row">
                    <div>
                        <asp:Label runat="server" AssociatedControlID="drp_products" CssClass="form-label">Product</asp:Label>
                        <asp:DropDownList ID="drp_products" runat="server" CssClass="form-control" AutoPostBack="true"
                            OnSelectedIndexChanged="drp_products_SelectedIndexChanged" />
                    </div>
                    <span class="form-hint">Choose a product to list only its accounts, or All products.</span>
                </div>
            </div>
        </section>

        <div class="picker">
            <section class="card">
                <div class="card-head">
                    <h2>Waiting for upload <span class="count-badge"><asp:Literal ID="litAvailCount" runat="server" /></span></h2>
                    <div class="tools">
                        <input type="search" class="form-control search" placeholder="Find account" data-filter="avail" aria-label="Find account in waiting list" />
                    </div>
                </div>
                <div class="table-wrap scroll-y">
                    <asp:GridView ID="gvAvailable" runat="server" CssClass="data-grid" GridLines="None" AutoGenerateColumns="false"
                        DataKeyNames="AccountNo,SerialNo,FilePath" ShowHeaderWhenEmpty="false" data-picker="avail">
                        <HeaderStyle CssClass="grid-head" />
                        <Columns>
                            <asp:TemplateField ItemStyle-CssClass="c row-select" HeaderStyle-CssClass="c" ItemStyle-Width="40px">
                                <HeaderTemplate><input type="checkbox" class="all-select" data-for="avail" aria-label="Tick every waiting account" /></HeaderTemplate>
                                <ItemTemplate><asp:CheckBox ID="chkSelect" runat="server" /></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="AccountNo" HeaderText="Account no" ItemStyle-CssClass="nowrap strong" />
                            <asp:BoundField DataField="SerialNo" HeaderText="Serial" ItemStyle-CssClass="nowrap" />
                            <asp:TemplateField HeaderText="Image file">
                                <ItemTemplate><span class="file-name"><%# HttpUtility.HtmlEncode(Convert.ToString(Eval("FileName"))) %></span></ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate><div class="empty">No mandate images are waiting for this product.</div></EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </section>

            <section class="card">
                <div class="card-head">
                    <h2>Uploaded <span class="count-badge"><asp:Literal ID="litSelCount" runat="server" /></span></h2>
                    <div class="tools">
                        <input type="search" class="form-control search" placeholder="Find account" data-filter="done" aria-label="Find account in uploaded list" />
                    </div>
                </div>
                <div class="table-wrap scroll-y">
                    <asp:GridView ID="gvUploaded" runat="server" CssClass="data-grid" GridLines="None" AutoGenerateColumns="false"
                        DataKeyNames="account_no" ShowHeaderWhenEmpty="false" data-picker="done">
                        <HeaderStyle CssClass="grid-head" />
                        <Columns>
                            <asp:TemplateField ItemStyle-CssClass="c row-select" HeaderStyle-CssClass="c" ItemStyle-Width="40px">
                                <HeaderTemplate><input type="checkbox" class="all-select" data-for="done" aria-label="Tick every uploaded account" /></HeaderTemplate>
                                <ItemTemplate><asp:CheckBox ID="chkSelect" runat="server" /></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="account_no" HeaderText="Account no" ItemStyle-CssClass="nowrap strong" />
                        </Columns>
                        <EmptyDataTemplate><div class="empty">No mandates uploaded yet.</div></EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </section>
        </div>

        <div class="action-bar" style="margin-top:16px;">
            <span class="count"><span id="cntAvail">0</span>&nbsp;to upload</span>
            <asp:Button ID="btnUpload" runat="server" CssClass="btn btn-primary" Text="Upload selection" OnClick="btnUpload_Click"
                OnClientClick="return confirmPick('avail', 'Upload the mandate images for {n} account(s)?');" />
            <span class="spacer"></span>
            <span class="count"><span id="cntDone">0</span>&nbsp;to delete</span>
            <asp:Button ID="btnDelete" runat="server" CssClass="btn btn-danger" Text="Delete selection" OnClick="btnDelete_Click"
                OnClientClick="return confirmPick('done', 'Delete the uploaded mandates for {n} account(s)? This cannot be undone.');" />
            <asp:Button ID="btnReset" runat="server" CssClass="btn btn-secondary" Text="Reset" CausesValidation="false" OnClick="btnReset_Click" />
        </div>

        <div id="busy" class="busy-overlay"><span class="spinner"></span>Working&hellip;</div>
    </div>

    <script type="text/javascript">
        (function () {
            function grid(key) { return document.querySelector('table[data-picker="' + key + '"]'); }
            function boxes(key) {
                var g = grid(key);
                return g ? g.querySelectorAll('.row-select input[type="checkbox"]') : [];
            }
            function refresh(key) {
                var list = boxes(key), n = 0, visible = 0, ticked = 0;
                for (var i = 0; i < list.length; i++) {
                    var tr = list[i].closest('tr'), hidden = tr && tr.classList.contains('is-hidden');
                    if (list[i].checked) { n++; if (tr) tr.classList.add('is-selected'); } else if (tr) { tr.classList.remove('is-selected'); }
                    if (!hidden) { visible++; if (list[i].checked) ticked++; }
                }
                var c = document.getElementById(key === 'avail' ? 'cntAvail' : 'cntDone');
                if (c) c.textContent = n;
                var all = document.querySelector('.all-select[data-for="' + key + '"]');
                if (all) { all.checked = visible > 0 && ticked === visible; all.indeterminate = ticked > 0 && ticked < visible; }
                return n;
            }
            window.confirmPick = function (key, question) {
                var n = refresh(key);
                if (n === 0) { alert('Tick at least one account first.'); return false; }
                if (!confirm(question.replace('{n}', n))) return false;
                document.getElementById('busy').classList.add('show');
                return true;
            };
            document.addEventListener('change', function (e) {
                var t = e.target;
                if (!t || !t.closest) return;
                if (t.classList.contains('all-select')) {
                    var key = t.getAttribute('data-for'), list = boxes(key);
                    for (var i = 0; i < list.length; i++) {
                        var tr = list[i].closest('tr');
                        if (!tr || !tr.classList.contains('is-hidden')) list[i].checked = t.checked;
                    }
                    refresh(key);
                } else if (t.closest('.row-select')) {
                    refresh(t.closest('table').getAttribute('data-picker'));
                }
            });
            document.addEventListener('input', function (e) {
                var t = e.target;
                if (!t || !t.hasAttribute || !t.hasAttribute('data-filter')) return;
                var key = t.getAttribute('data-filter'), q = t.value.trim().toLowerCase(), g = grid(key);
                if (!g) return;
                var rows = g.querySelectorAll('tr');
                for (var i = 0; i < rows.length; i++) {
                    if (!rows[i].querySelector('.row-select')) continue;
                    var match = q === '' || rows[i].textContent.toLowerCase().indexOf(q) >= 0;
                    rows[i].classList.toggle('is-hidden', !match);
                }
                refresh(key);
            });
            refresh('avail'); refresh('done');
        })();
    </script>
    </form>
</body>
</html>
