<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NDDeMandate.aspx.cs" Inherits="NDD_NDDeMandate" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>NDD e-mandates</title>
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
        .pill.bad { color: var(--bad); background: var(--bad-bg); }
        .page-head .back { display: inline-block; margin-bottom: 8px; font-size: 12px; font-weight: 600; color: var(--brand); text-decoration: none; }
        .page-head .back:hover { text-decoration: underline; }
        .req { color: var(--bad); }
        .readonly { background-color: #f8fafc !important; color: var(--muted); }
        .details { display: grid; grid-template-columns: minmax(150px, 220px) 1fr; margin: 0; }
        .details dt, .details dd { margin: 0; padding: 9px 0; border-top: 1px solid var(--line); }
        .details dt { font-size: 12px; font-weight: 600; color: var(--muted); }
        .details dd { font-weight: 500; word-break: break-word; }
        .details dt:first-of-type, .details dt:first-of-type + dd { border-top: 0; }
        @media (max-width: 760px) { .details { grid-template-columns: 1fr; } .details dd { border-top: 0; padding-top: 0; } }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="wrap">

        <header class="page-head">
            <h1>NDD e-mandates</h1>
            <p>Capture a customer's direct-debit mandate: who pays, from which bank account, how much and for how long. New and edited mandates go to <strong>Approval &gt; NDD e-mandate approval</strong>; approved mandates cannot be changed or deleted.</p>
        </header>

        <div class="notice" role="status"><asp:Literal ID="litResult" runat="server" /></div>
        <div class="alert" role="alert"><asp:Literal ID="litError" runat="server" /></div>
        <asp:ValidationSummary ID="valSummary" runat="server" CssClass="alert" ValidationGroup="mandate" DisplayMode="BulletList" HeaderText="Please correct the following:" />

        <section class="card">
            <div class="card-head">
                <h2><asp:Literal ID="litFormTitle" runat="server" Text="New e-mandate" /></h2>
                <span class="sub">Fields marked <span class="req">*</span> are required</span>
            </div>
            <div class="card-body">
                <asp:Panel ID="pnlForm" runat="server" DefaultButton="btnSave">
                    <asp:HiddenField ID="hidSeqNo" runat="server" />
                    <asp:HiddenField ID="hidSubscriberCode" runat="server" />

                    <div class="form-grid">
                        <div class="full">
                            <asp:Label runat="server" AssociatedControlID="ddlBiller" CssClass="form-label">Customer (biller / product) <span class="req">*</span></asp:Label>
                            <asp:DropDownList ID="ddlBiller" runat="server" CssClass="form-control" AppendDataBoundItems="true" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlBiller" InitialValue="" ValidationGroup="mandate" Display="Dynamic" CssClass="field-error" ErrorMessage="Choose the customer (biller)." />
                        </div>
                        <div>
                            <asp:Label runat="server" AssociatedControlID="txtCustomerAccount" CssClass="form-label">Customer account no <span class="req">*</span></asp:Label>
                            <asp:TextBox ID="txtCustomerAccount" runat="server" CssClass="form-control" MaxLength="20" AutoPostBack="true" CausesValidation="false"
                                OnTextChanged="txtCustomerAccount_TextChanged" placeholder="Account the collections are paid into" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCustomerAccount" ValidationGroup="mandate" Display="Dynamic" CssClass="field-error" ErrorMessage="Customer account number is required." />
                            <span class="lookup"><asp:Literal ID="litCustomerAccountName" runat="server" /></span>
                        </div>
                        <div>
                            <asp:Label runat="server" AssociatedControlID="txtPayerName" CssClass="form-label">Payer name <span class="req">*</span></asp:Label>
                            <asp:TextBox ID="txtPayerName" runat="server" CssClass="form-control" MaxLength="150" placeholder="Name or title of the payer" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPayerName" ValidationGroup="mandate" Display="Dynamic" CssClass="field-error" ErrorMessage="Payer name is required." />
                        </div>
                        <div>
                            <asp:Label runat="server" AssociatedControlID="txtPhone" CssClass="form-label">Payer phone <span class="req">*</span></asp:Label>
                            <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" TextMode="Phone" MaxLength="20" placeholder="e.g. 08012345678" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPhone" ValidationGroup="mandate" Display="Dynamic" CssClass="field-error" ErrorMessage="Payer phone is required." />
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="txtPhone" ValidationGroup="mandate" Display="Dynamic" CssClass="field-error"
                                ValidationExpression="^\+?[0-9 ]{7,20}$" ErrorMessage="Enter a valid phone number." />
                        </div>
                        <div>
                            <asp:Label runat="server" AssociatedControlID="txtEmail" CssClass="form-label">Payer email <span class="req">*</span></asp:Label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" MaxLength="150" placeholder="name@example.com" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail" ValidationGroup="mandate" Display="Dynamic" CssClass="field-error" ErrorMessage="Payer email is required." />
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="txtEmail" ValidationGroup="mandate" Display="Dynamic" CssClass="field-error"
                                ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" ErrorMessage="Enter a valid email address." />
                        </div>
                        <div class="full">
                            <asp:Label runat="server" AssociatedControlID="txtAddress" CssClass="form-label">Payer address <span class="req">*</span></asp:Label>
                            <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" MaxLength="250" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtAddress" ValidationGroup="mandate" Display="Dynamic" CssClass="field-error" ErrorMessage="Payer address is required." />
                        </div>
                    </div>
                </asp:Panel>
            </div>

            <div class="card-head" style="border-top: 1px solid var(--line);">
                <h2>Payer's bank account</h2>
                <span class="sub">The name is checked with core banking, or NIBSS for other banks</span>
            </div>
            <div class="card-body">
                <div class="form-grid">
                    <div>
                        <asp:Label runat="server" AssociatedControlID="ddlBank" CssClass="form-label">Bank <span class="req">*</span></asp:Label>
                        <asp:DropDownList ID="ddlBank" runat="server" CssClass="form-control" AppendDataBoundItems="true" AutoPostBack="true" CausesValidation="false"
                            DataSourceID="sqlBanks" DataTextField="bank_name" DataValueField="bank_code" OnSelectedIndexChanged="PayerAccount_Changed">
                            <asp:ListItem Value="" Text="Choose a bank" />
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlBank" InitialValue="" ValidationGroup="mandate" Display="Dynamic" CssClass="field-error" ErrorMessage="Choose the payer's bank." />
                    </div>
                    <div>
                        <asp:Label runat="server" AssociatedControlID="txtPayerAccount" CssClass="form-label">Payer account no <span class="req">*</span></asp:Label>
                        <asp:TextBox ID="txtPayerAccount" runat="server" CssClass="form-control" MaxLength="20" AutoPostBack="true" CausesValidation="false"
                            OnTextChanged="PayerAccount_Changed" placeholder="Account to be debited" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPayerAccount" ValidationGroup="mandate" Display="Dynamic" CssClass="field-error" ErrorMessage="Payer account number is required." />
                        <span class="lookup"><asp:Literal ID="litPayerAccountName" runat="server" /></span>
                    </div>
                </div>
            </div>

            <div class="card-head" style="border-top: 1px solid var(--line);">
                <h2>Amount and period</h2>
                <span class="sub">End date = start date + frequency (months) &minus; 1 day</span>
            </div>
            <div class="card-body">
                <div class="form-grid cols-3">
                    <div>
                        <asp:Label runat="server" AssociatedControlID="txtAmount" CssClass="form-label">Amount (NGN) <span class="req">*</span></asp:Label>
                        <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" TextMode="Number" step="0.01" min="0.01" placeholder="0.00" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtAmount" ValidationGroup="mandate" Display="Dynamic" CssClass="field-error" ErrorMessage="Amount is required." />
                        <asp:CompareValidator runat="server" ControlToValidate="txtAmount" ValidationGroup="mandate" Display="Dynamic" CssClass="field-error"
                            Operator="GreaterThan" ValueToCompare="0" Type="Double" ErrorMessage="Amount must be more than zero." />
                    </div>
                    <div>
                        <asp:Label runat="server" AssociatedControlID="txtFrequency" CssClass="form-label">Frequency (months) <span class="req">*</span></asp:Label>
                        <asp:TextBox ID="txtFrequency" runat="server" CssClass="form-control" TextMode="Number" min="1" max="120" Text="1" AutoPostBack="true" CausesValidation="false"
                            OnTextChanged="Period_Changed" />
                        <asp:RangeValidator runat="server" ControlToValidate="txtFrequency" ValidationGroup="mandate" Display="Dynamic" CssClass="field-error"
                            MinimumValue="1" MaximumValue="120" Type="Integer" ErrorMessage="Frequency must be a whole number of months from 1 to 120." />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFrequency" ValidationGroup="mandate" Display="Dynamic" CssClass="field-error" ErrorMessage="Frequency is required." />
                    </div>
                    <div>
                        <span class="form-label">Mandate type</span>
                        <span class="check" style="height: 36px;">
                            <asp:CheckBox ID="chkFixed" runat="server" Text="Fixed amount mandate" />
                        </span>
                    </div>
                    <div>
                        <asp:Label runat="server" AssociatedControlID="txtStartDate" CssClass="form-label">Start date <span class="req">*</span></asp:Label>
                        <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" TextMode="Date" AutoPostBack="true" CausesValidation="false" OnTextChanged="Period_Changed" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtStartDate" ValidationGroup="mandate" Display="Dynamic" CssClass="field-error" ErrorMessage="Start date is required." />
                    </div>
                    <div>
                        <asp:Label runat="server" AssociatedControlID="txtEndDate" CssClass="form-label">End date</asp:Label>
                        <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control readonly" ReadOnly="true" TabIndex="-1" />
                    </div>
                    <div class="full">
                        <asp:Label runat="server" AssociatedControlID="txtNarration" CssClass="form-label">Narration <span class="req">*</span></asp:Label>
                        <asp:TextBox ID="txtNarration" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" MaxLength="250" placeholder="What the debit is for" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNarration" ValidationGroup="mandate" Display="Dynamic" CssClass="field-error" ErrorMessage="Narration is required." />
                    </div>
                </div>
                <div class="form-actions">
                    <asp:Button ID="btnSave" runat="server" Text="Save mandate" CssClass="btn btn-primary" ValidationGroup="mandate" OnClick="btnSave_Click"
                        OnClientClick="if (typeof(Page_ClientValidate) === 'function' && !Page_ClientValidate('mandate')) return false; document.getElementById('busy').classList.add('show'); return true;" />
                    <asp:Button ID="btnCancel" runat="server" Text="Clear" CssClass="btn btn-secondary" CausesValidation="false" OnClick="btnCancel_Click" />
                </div>
            </div>
        </section>

        <section class="card">
            <div class="card-head">
                <h2>E-mandates <span class="sub"><asp:Literal ID="litTotal" runat="server" /></span></h2>
            </div>
            <div class="table-wrap">
                <asp:GridView ID="grid" runat="server" DataSourceID="sqlMandates"
                    AutoGenerateColumns="False" CssClass="data-grid" GridLines="None" UseAccessibleHeader="True"
                    DataKeyNames="SeqNo" AllowPaging="True" PageSize="25" AllowSorting="True"
                    OnRowCommand="grid_RowCommand" OnRowDataBound="grid_RowDataBound">
                    <HeaderStyle CssClass="grid-head" />
                    <PagerStyle CssClass="grid-pager" />
                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" />
                    <EmptyDataTemplate><div class="empty">No e-mandates yet. Capture one above.</div></EmptyDataTemplate>
                    <Columns>
                        <asp:BoundField DataField="payerName" HeaderText="Payer" SortExpression="payerName" ItemStyle-CssClass="strong" />
                        <asp:BoundField DataField="payerAccountNumber" HeaderText="Payer account" SortExpression="payerAccountNumber" ItemStyle-CssClass="nowrap" />
                        <asp:BoundField DataField="payerAccountName" HeaderText="Payer account name" />
                        <asp:BoundField DataField="bank_name" HeaderText="Bank" SortExpression="bank_name" />
                        <asp:BoundField DataField="accountNumber" HeaderText="Customer account" SortExpression="accountNumber" ItemStyle-CssClass="nowrap" />
                        <asp:BoundField DataField="amount" HeaderText="Amount" SortExpression="amount" DataFormatString="{0:N2}" ItemStyle-CssClass="num" HeaderStyle-CssClass="num" />
                        <asp:BoundField DataField="frequency" HeaderText="Months" SortExpression="frequency" DataFormatString="{0:0}" ItemStyle-CssClass="num" HeaderStyle-CssClass="num" />
                        <asp:BoundField DataField="startDate" HeaderText="Start" SortExpression="startDate" DataFormatString="{0:dd MMM yyyy}" ItemStyle-CssClass="nowrap" />
                        <asp:BoundField DataField="endDate" HeaderText="End" SortExpression="endDate" DataFormatString="{0:dd MMM yyyy}" ItemStyle-CssClass="nowrap" />
                        <asp:TemplateField HeaderText="Fixed" ItemStyle-CssClass="c" HeaderStyle-CssClass="c">
                            <ItemTemplate><%# IsFixed(Eval("mandateTypeCon")) ? "Yes" : "No" %></ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="narration" HeaderText="Narration" ItemStyle-CssClass="muted" />
                        <asp:TemplateField HeaderText="Status" SortExpression="statusDesc">
                            <ItemTemplate><span class='<%# StatusCss(Eval("statusDesc")) %>'><%# HttpUtility.HtmlEncode(Convert.ToString(Eval("statusDesc"))) %></span></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-CssClass="actions">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEdit" runat="server" CssClass="link-btn edit" CommandName="EditRow" CommandArgument='<%# Eval("SeqNo") %>' CausesValidation="false">Edit</asp:LinkButton>
                                <asp:LinkButton ID="lnkDelete" runat="server" CssClass="link-btn delete" CommandName="DeleteRow" CommandArgument='<%# Eval("SeqNo") %>'
                                    CausesValidation="false" OnClientClick="return confirm('Delete this e-mandate? This cannot be undone.');">Delete</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </section>

        <div id="busy" class="busy-overlay"><span class="spinner"></span>Checking accounts and saving&hellip;</div>
    </div>

    <asp:SqlDataSource ID="sqlMandates" runat="server" ConnectionString="<%$ ConnectionStrings:EazyConnect %>"
        SelectCommand="SpNDDeMandateGetAll" SelectCommandType="StoredProcedure" DataSourceMode="DataSet" OnSelected="sqlMandates_Selected" />
    <asp:SqlDataSource ID="sqlBanks" runat="server" ConnectionString="<%$ ConnectionStrings:EazyConnect %>"
        SelectCommand="SpmNNDGetBankAndSchemeCode" SelectCommandType="StoredProcedure" />
    <asp:SqlDataSource ID="sqlBillers" runat="server" ConnectionString="<%$ ConnectionStrings:EazyConnect %>"
        SelectCommand="SpNDDBillerGetAppdInfo" SelectCommandType="StoredProcedure" DataSourceMode="DataSet">
        <SelectParameters>
            <asp:Parameter DefaultValue="3" Name="status" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    </form>
</body>
</html>
