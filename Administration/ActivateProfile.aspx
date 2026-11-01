<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ActivateProfile.aspx.cs" Inherits="Administration_ActivateProfile" Async="true" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Customer profile activation</title>

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
        .wrap { max-width: 1400px; margin: 0 auto; }

        .page-head {
            margin: 0 0 16px; padding: 20px 22px;
            background: var(--surface); border: 1px solid var(--line);
            border-left: 4px solid var(--brand); border-radius: 12px;
        }
        .page-head h1 { margin: 0 0 2px; font-size: 18px; font-weight: 700; letter-spacing: -.01em; }
        .page-head p { margin: 0; font-size: 12px; color: var(--muted); }

        .notice, .alert { margin-bottom: 12px; padding: 12px 14px; border-radius: 8px; line-height: 1.5; word-break: break-word; }
        .notice { font-weight: 600; color: var(--ok); background: var(--ok-bg); border-left: 4px solid var(--ok); }
        .alert { font-weight: 500; color: var(--bad); background: var(--bad-bg); border-left: 4px solid var(--bad); }
        .notice:empty, .alert:empty { display: none; }

        .card { margin-bottom: 16px; background: var(--surface); border: 1px solid var(--line); border-radius: 12px; }
        .card-head {
            display: flex; align-items: center; justify-content: space-between; gap: 12px; flex-wrap: wrap;
            padding: 14px 20px; border-bottom: 1px solid var(--line);
        }
        .card-head h2 { margin: 0; font-size: 14px; font-weight: 700; }
        .card-head .sub { font-size: 12px; color: var(--muted); font-weight: 500; }

        /* ---------- Search ---------- */
        .search { display: flex; gap: 8px; align-items: center; flex: 0 1 420px; }
        .form-control {
            display: block; width: 100%; height: 36px; padding: 0 10px;
            font: inherit; font-size: 13px; color: var(--ink); background: var(--surface);
            border: 1.5px solid var(--field); border-radius: 9px; outline: none;
            transition: border-color .15s ease, box-shadow .15s ease;
        }
        .form-control:hover { border-color: #aebbc9; }
        .form-control:focus { border-color: var(--brand); box-shadow: 0 0 0 4px var(--ring); }

        /* ---------- Grid ---------- */
        .table-wrap { overflow-x: auto; }
        table.data-grid { width: 100%; border: 0; border-collapse: collapse; }
        table.data-grid th, table.data-grid td { padding: 10px 12px; text-align: left; vertical-align: middle; border-top: 1px solid var(--line); }
        table.data-grid tr.grid-head th {
            border-top: 0; background: #f8fafc; white-space: nowrap;
            font-size: 11px; font-weight: 600; color: var(--muted);
            text-transform: uppercase; letter-spacing: .04em;
        }
        table.data-grid tr.grid-head a { color: var(--muted); text-decoration: none; }
        table.data-grid tr.grid-head a:hover { color: var(--brand); }
        table.data-grid tr:hover td { background: #f8fafc; }
        table.data-grid tr.is-selected td { background: var(--tint); }
        table.data-grid .c { text-align: center; }
        table.data-grid .nowrap { white-space: nowrap; }
        table.data-grid .strong { font-weight: 600; }
        table.data-grid .muted { color: var(--muted); }
        table.data-grid input[type="checkbox"] { width: 16px; height: 16px; margin: 0; accent-color: var(--brand); cursor: pointer; vertical-align: middle; }
        table.data-grid textarea.reason {
            width: 100%; min-width: 180px; padding: 6px 8px; font: inherit; font-size: 12px; resize: vertical;
            border: 1.5px solid var(--field); border-radius: 8px; outline: none;
        }
        table.data-grid textarea.reason:focus { border-color: var(--brand); box-shadow: 0 0 0 3px var(--ring); }

        .pill { display: inline-block; padding: 3px 9px; font-size: 11px; font-weight: 600; border-radius: 999px; white-space: nowrap; }
        .pill.on { color: var(--ok); background: var(--ok-bg); }
        .pill.off { color: var(--muted); background: #eef2f6; }

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
            white-space: nowrap; border: 1.5px solid transparent; border-radius: 9px; cursor: pointer;
            transition: background-color .15s ease, border-color .15s ease;
        }
        .btn:focus-visible { outline: none; box-shadow: 0 0 0 4px var(--ring); }
        .btn-primary { color: #fff; background: var(--brand); }
        .btn-primary:hover { background: var(--brand-dark); }
        .btn-secondary { color: var(--ink); background: var(--surface); border-color: var(--field); }
        .btn-secondary:hover { background: #f1f5f9; }
        .btn-danger { color: var(--bad); background: var(--surface); border-color: #f0c4c0; }
        .btn-danger:hover { background: var(--bad-bg); }
        .btn-link { color: var(--muted); background: none; }
        .btn-link:hover { color: var(--ink); text-decoration: underline; }

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

        @media (max-width: 760px) {
            body { padding: 14px; }
            .search { flex: 1 1 100%; }
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
            <h1>Customer profile activation</h1>
            <p>Tick customers in the list, set their channels, then activate or deactivate them. A reason is required to deactivate.</p>
        </header>

        <div class="notice" role="status"><asp:Literal ID="litResult" runat="server"></asp:Literal></div>
        <div class="alert" role="alert"><asp:Literal ID="Literal1" runat="server"></asp:Literal></div>

        <section class="card">
            <div class="card-head">
                <h2>Customers <span class="sub"><asp:Literal ID="litTotal" runat="server"></asp:Literal></span></h2>
                <asp:Panel ID="pnlSearch" runat="server" CssClass="search" DefaultButton="btnSearch">
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control"
                        placeholder="Search user name, account name or number" aria-label="Search customers" />
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-secondary" OnClick="btnSearch_Click" CausesValidation="false" />
                </asp:Panel>
            </div>

            <div class="table-wrap">
                <asp:GridView ID="ctrlGrid" runat="server" DataSourceID="SqlDataSource1"
                    AutoGenerateColumns="False" CssClass="data-grid" GridLines="None"
                    DataKeyNames="user_id,AccountNumber,AccountNos,LastName"
                    AllowPaging="True" PageSize="50" AllowSorting="True"
                    UseAccessibleHeader="True">
                    <HeaderStyle CssClass="grid-head" />
                    <PagerStyle CssClass="grid-pager" />
                    <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" PageButtonCount="10" />
                    <EmptyDataTemplate><div class="empty">No customers found.</div></EmptyDataTemplate>
                    <Columns>
                        <asp:TemplateField ItemStyle-CssClass="c" HeaderStyle-CssClass="c">
                            <HeaderTemplate>
                                <input type="checkbox" id="chkAll" title="Select or unselect all rows on this page" onclick="selectAllRows(this.checked);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelect" runat="server" CssClass="row-select" ToolTip="Select this customer" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="AccountNos" HeaderText="Account no" SortExpression="AccountNos" ItemStyle-CssClass="nowrap" />
                        <asp:BoundField DataField="user_id" HeaderText="User name" SortExpression="user_id" ItemStyle-CssClass="nowrap strong" />
                        <asp:BoundField DataField="AccountDesc" HeaderText="Account name" SortExpression="AccountDesc" />

                        <asp:TemplateField HeaderText="Activation" SortExpression="activated" ItemStyle-CssClass="c" HeaderStyle-CssClass="c">
                            <ItemTemplate>
                                <span class='<%# AsBool(Eval("activated")) ? "pill on" : "pill off" %>'><%# AsBool(Eval("activated")) ? "Active" : "Inactive" %></span>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="eToken" ItemStyle-CssClass="c" HeaderStyle-CssClass="c">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkeToken" runat="server" Checked='<%# AsBool(Eval("booleToken")) %>' ToolTip="eToken" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="Phone" HeaderText="Registration phone" SortExpression="Phone" ItemStyle-CssClass="nowrap muted" />
                        <asp:BoundField DataField="Address" HeaderText="Registration address" SortExpression="Address" ItemStyle-CssClass="muted" />

                        <asp:TemplateField HeaderText="Mobile" ItemStyle-CssClass="c" HeaderStyle-CssClass="c">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkMobile" runat="server" Checked='<%# AsBool(Eval("boolMobile")) %>' ToolTip="Use mobile" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="iPortal" ItemStyle-CssClass="c" HeaderStyle-CssClass="c">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkPortal" runat="server" Checked='<%# AsBool(Eval("booliPortal")) %>' ToolTip="Use iPortal" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="USSD" ItemStyle-CssClass="c" HeaderStyle-CssClass="c">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkUssD" runat="server" Checked='<%# AsBool(Eval("USSDenabled")) %>' ToolTip="Use USSD" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Reason">
                            <ItemTemplate>
                                <asp:TextBox ID="tatReason" runat="server" TextMode="MultiLine" Rows="2" CssClass="reason"
                                    placeholder="Reason (to deactivate or clear questions)" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </section>

        <!-- ================= Actions ================= -->
        <div class="action-bar">
            <span class="count"><span id="selCount">0</span> selected</span>
            <asp:Button ID="btnDevice" runat="server" Text="Activate selection" CssClass="btn btn-primary"
                OnClick="BtnApprove_Click" OnClientClick="return confirmSelection('activate');" />
            <asp:Button ID="ASPxButton1" runat="server" Text="Deactivate selection" CssClass="btn btn-danger"
                OnClick="btnDisApprove_Click" OnClientClick="return confirmSelection('deactivate');" />
            <span class="spacer"></span>

            <asp:Button ID="btncancel" runat="server" Text="Cancel" CssClass="btn btn-link" OnClick="btncancel_Click" CausesValidation="false" />
        </div>

    </div>

    <asp:SqlDataSource ID="SqlDataSource1" runat="server"
        ConnectionString="<%$ ConnectionStrings:EazyConnect %>"
        SelectCommand="Sproc_Aspx_Userstatus_sel" SelectCommandType="StoredProcedure"
        DataSourceMode="DataSet" OnSelected="SqlDataSource1_Selected">
    </asp:SqlDataSource>
    <asp:TextBox ID="txtUserID" runat="server" Visible="False"></asp:TextBox>
    <asp:TextBox ID="txtlocation" runat="server" Visible="False"></asp:TextBox>

    <script type="text/javascript">
        (function () {
            function rowBoxes() {
                return document.querySelectorAll('#<%= ctrlGrid.ClientID %> .row-select input[type="checkbox"]');
            }

            function refresh() {
                var boxes = rowBoxes(), n = 0;
                for (var i = 0; i < boxes.length; i++) {
                    var tr = boxes[i].closest('tr');
                    if (boxes[i].checked) { n++; if (tr) tr.classList.add('is-selected'); }
                    else if (tr) { tr.classList.remove('is-selected'); }
                }
                document.getElementById('selCount').textContent = n;
                var all = document.getElementById('chkAll');
                if (all) {
                    all.checked = boxes.length > 0 && n === boxes.length;
                    all.indeterminate = n > 0 && n < boxes.length;
                }
                return n;
            }

            window.selectAllRows = function (checked) {
                var boxes = rowBoxes();
                for (var i = 0; i < boxes.length; i++) { boxes[i].checked = checked; }
                refresh();
            };

            // Stop an action when nothing is ticked; ask before bulk changes
            window.confirmSelection = function (verb) {
                var n = refresh();
                if (n === 0) { alert('Select at least one customer first.'); return false; }
                if (verb) { return confirm('Are you sure you want to ' + verb + ' ' + n + ' customer' + (n === 1 ? '' : 's') + '?'); }
                return true;
            };

            document.addEventListener('change', function (e) {
                if (e.target && e.target.closest && e.target.closest('.row-select')) { refresh(); }
            });

            refresh();
        })();
    </script>
    </form>
</body>
</html>