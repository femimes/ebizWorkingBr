<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmATMPolicyConsole.aspx.cs" Inherits="Mandate_FrmATMPolicyConsole" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>General policy</title>
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
        .wrap { max-width: 1000px; margin: 0 auto; }

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

        .card { margin-bottom: 16px; background: var(--surface); border: 1px solid var(--line); border-radius: 12px; overflow: hidden; }
        .card-head {
            display: flex; align-items: center; justify-content: space-between; gap: 12px;
            padding: 14px 20px; border-bottom: 1px solid var(--line);
        }
        .card-head h2 { margin: 0; font-size: 14px; font-weight: 700; }
        .card-head .sub { font-size: 12px; color: var(--muted); }

        .table-wrap { overflow-x: auto; }
        table.data-grid { width: 100%; border: 0; border-collapse: collapse; }
        table.data-grid th, table.data-grid td { padding: 10px 14px; text-align: left; vertical-align: middle; border-top: 1px solid var(--line); }
        table.data-grid tr.grid-head th {
            border-top: 0; background: #f8fafc; white-space: nowrap;
            font-size: 11px; font-weight: 600; color: var(--muted);
            text-transform: uppercase; letter-spacing: .04em;
        }
        table.data-grid tr.grid-head a { color: var(--muted); text-decoration: none; }
        table.data-grid tr:hover td { background: #f8fafc; }
        table.data-grid tr.grid-edit td { background: var(--tint); }
        table.data-grid .c { text-align: center; }
        table.data-grid .nowrap { white-space: nowrap; }
        table.data-grid .strong { font-weight: 600; }
        table.data-grid .muted { color: var(--muted); }
        table.data-grid .actions { white-space: nowrap; text-align: right; }

        .form-control {
            width: 100%; min-width: 140px; height: 34px; padding: 0 10px;
            font: inherit; font-size: 13px; color: var(--ink); background: var(--surface);
            border: 1.5px solid var(--field); border-radius: 8px; outline: none;
        }
        .form-control:focus { border-color: var(--brand); box-shadow: 0 0 0 3px var(--ring); }
        table.data-grid input[type="checkbox"] { width: 16px; height: 16px; margin: 0; accent-color: var(--brand); cursor: pointer; vertical-align: middle; }
        .field-error { display: block; margin-top: 4px; font-size: 12px; color: var(--bad); }

        .pill { display: inline-block; padding: 3px 9px; font-size: 11px; font-weight: 600; border-radius: 999px; white-space: nowrap; }
        .pill.on { color: var(--ok); background: var(--ok-bg); }
        .pill.off { color: var(--muted); background: #eef2f6; }

        .link-btn {
            display: inline-block; margin-left: 4px; padding: 6px 12px;
            font-size: 12px; font-weight: 600; text-decoration: none; border-radius: 8px;
        }
        .link-btn.edit, .link-btn.cancel { color: var(--brand); background: var(--tint); }
        .link-btn.edit:hover, .link-btn.cancel:hover { background: #dbe7f3; }
        .link-btn.save { color: #fff; background: var(--brand); }
        .link-btn.save:hover { background: var(--brand-dark); }

        table.data-grid tr.grid-pager td { padding: 12px; text-align: center; background: var(--surface); }
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

        @media (max-width: 760px) { body { padding: 14px; } }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="wrap">

        <header class="page-head">
            <h1>General policy</h1>
            <p>Click Edit on a policy to change its value or turn it on or off.</p>
        </header>

        <div class="notice" role="status"><asp:Literal ID="litResult" runat="server"></asp:Literal></div>
        <div class="alert" role="alert"><asp:Literal ID="litError" runat="server"></asp:Literal></div>

        <section class="card">
            <div class="card-head">
                <h2>Policies</h2>
                <span class="sub">Policy ID and description cannot be changed</span>
            </div>
            <div class="table-wrap">
                <asp:GridView ID="ASPxGridView1" runat="server" DataSourceID="SqlDataSource1"
                    AutoGenerateColumns="False" CssClass="data-grid" GridLines="None"
                    DataKeyNames="PolicyID" AllowPaging="True" PageSize="20" AllowSorting="True"
                    UseAccessibleHeader="True"
                    OnRowUpdating="ASPxGridView1_RowUpdating"
                    OnRowEditing="ASPxGridView1_RowEditing"
                    OnRowCancelingEdit="ASPxGridView1_RowCancelingEdit">
                    <HeaderStyle CssClass="grid-head" />
                    <EditRowStyle CssClass="grid-edit" />
                    <PagerStyle CssClass="grid-pager" />
                    <EmptyDataTemplate><div class="empty">No policies found.</div></EmptyDataTemplate>
                    <Columns>
                        <asp:BoundField DataField="PolicyID" HeaderText="Policy ID" ReadOnly="True" SortExpression="PolicyID" ItemStyle-CssClass="nowrap strong" />
                        <asp:BoundField DataField="PolicyDesc" HeaderText="Description" ReadOnly="True" SortExpression="PolicyDesc" />

                        <asp:TemplateField HeaderText="Value" SortExpression="PolicyValue">
                            <ItemTemplate><%# HttpUtility.HtmlEncode(Convert.ToString(Eval("PolicyValue"))) %></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtPolicyValue" runat="server" CssClass="form-control"
                                    Text='<%# Eval("PolicyValue") %>' aria-label="Policy value" />
                                <asp:RequiredFieldValidator ID="reqPolicyValue" runat="server" ControlToValidate="txtPolicyValue"
                                    Display="Dynamic" CssClass="field-error" ErrorMessage="Enter a value." ValidationGroup="policy" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Apply" SortExpression="ApplyPolicy" ItemStyle-CssClass="c" HeaderStyle-CssClass="c">
                            <ItemTemplate>
                                <span class='<%# AsBool(Eval("ApplyPolicy")) ? "pill on" : "pill off" %>'><%# AsBool(Eval("ApplyPolicy")) ? "On" : "Off" %></span>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkApplyPolicy" runat="server" Checked='<%# AsBool(Eval("ApplyPolicy")) %>' ToolTip="Apply this policy" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField ItemStyle-CssClass="actions">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" CssClass="link-btn edit" CausesValidation="false">Edit</asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="lnkUpdate" runat="server" CommandName="Update" CssClass="link-btn save" ValidationGroup="policy">Save</asp:LinkButton>
                                <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" CssClass="link-btn cancel" CausesValidation="false">Cancel</asp:LinkButton>
                            </EditItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </section>

        <asp:SqlDataSource ID="SqlDataSource1" runat="server"
            ConnectionString="<%$ ConnectionStrings:EazyConnect %>"
            SelectCommand="SpATMGetAllGENPolicy" SelectCommandType="StoredProcedure"
            DataSourceMode="DataSet">
        </asp:SqlDataSource>

    </div>
    </form>
</body>
</html>