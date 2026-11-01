<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FrmCorpAppUser.aspx.vb" Inherits="Processing_FrmCorpAppUser" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>eBizPlus</title>
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

        .field-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 16px 20px; }
        .field-grid .full { grid-column: 1 / -1; }
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
        .inline { display: flex; align-items: center; gap: 8px; flex-wrap: wrap; }
        .inline .form-control { flex: 1 1 180px; width: auto; }
        .field-error { display: block; margin-top: 5px; font-size: 12px; font-weight: 500; color: var(--bad); }
        .alert {
            display: block; margin-bottom: 16px; padding: 12px 14px; font-weight: 500;
            color: var(--bad); background: var(--bad-bg); border-left: 4px solid var(--bad); border-radius: 8px;
        }

        .check { display: inline-flex; align-items: center; gap: 9px; }
        .check input[type="checkbox"] { width: 17px; height: 17px; margin: 0; accent-color: var(--brand); cursor: pointer; }
        .check label { cursor: pointer; font-weight: 500; }
        .check-list { display: grid; grid-template-columns: repeat(3, 1fr); gap: 10px; }
        .check-list .check {
            padding: 12px 14px; border: 1.5px solid var(--line); border-radius: 10px;
            transition: border-color .15s ease, background-color .15s ease;
        }
        .check-list .check:hover { border-color: #aebbc9; }
        .check-list .check:has(input:checked) { border-color: var(--brand); background: var(--tint); }
        .check.danger input[type="checkbox"] { accent-color: var(--bad); }
        .check.danger:has(input:checked) label { color: var(--bad); }

        .perm-groups { display: grid; grid-template-columns: 1fr 1fr; gap: 18px 20px; }
        .perm-groups .full { grid-column: 1 / -1; }
        .group-label {
            margin: 0 0 8px; font-size: 11px; font-weight: 600; color: var(--muted);
            text-transform: uppercase; letter-spacing: .04em;
        }
        .perm-groups .check-list { grid-template-columns: 1fr 1fr; }
        .perm-groups .full .check-list { grid-template-columns: repeat(3, 1fr); }

        .table-wrap { overflow-x: auto; }
        table.actions-grid { width: 100%; border: 0; border-collapse: collapse; }
        table.actions-grid td { padding: 5px 6px 5px 0; border: 0; }
        table.actions-grid td:first-child { font-weight: 700; padding-right: 14px; white-space: nowrap; }
        .action-link {
            display: inline-block; padding: 7px 12px; font-size: 12px; font-weight: 600;
            color: var(--brand); text-decoration: none; white-space: nowrap;
            background: var(--tint); border-radius: 8px;
        }
        .action-link:hover { background: #dbe7f3; }

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
            position: sticky; bottom: 0;
            display: flex; align-items: center; gap: 10px; flex-wrap: wrap;
            padding: 12px 20px; background: var(--surface);
            border: 1px solid var(--line); border-radius: 12px;
            box-shadow: 0 -6px 18px rgba(20, 33, 46, .06);
        }
        .action-bar .spacer { flex: 1; }

        @media (max-width: 760px) {
            body { padding: 14px; }
            .field-grid, .perm-groups, .check-list,
            .perm-groups .check-list, .perm-groups .full .check-list { grid-template-columns: 1fr; }
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
            <h1>Corporate users maintenance</h1>
            <p>Set up a corporate user's account, permissions, contact details and organisation.</p>
        </header>

        <asp:CustomValidator ID="cus_val" runat="server" Display="Dynamic" CssClass="alert" />
        <asp:HyperLink ID="HyperLink1" Visible="false" runat="server">User Info...</asp:HyperLink>

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
                                required="required" autocomplete="off" spellcheck="false" />
                            <asp:Button ID="cmd_go" runat="server" CausesValidation="False" Text="Go" CssClass="btn btn-primary" />
                            <a class="text-link" title="Look up a user" href="#"
                               onclick="window.open('Frm_searchUsers.aspx?textbox=users&p=names','cal','width=600,height=600,Scrollbars=yes'); return false;">Find user&hellip;</a>
                        </div>
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
                <span class="sub">What this user can do</span>
            </div>
            <div class="card-body">
                <div class="perm-groups">
                    <div>
                        <p class="group-label">Single transactions</p>
                        <div class="check-list">
                            <span class="check"><asp:CheckBox ID="ChkIsCorpSingle" runat="server" Text="Can post" /></span>
                            <span class="check"><asp:CheckBox ID="chkIsCorpSingleAppr" runat="server" Text="Can approve" /></span>
                        </div>
                    </div>
                    <div>
                        <p class="group-label">Bulk transactions</p>
                        <div class="check-list">
                            <span class="check"><asp:CheckBox ID="ChkIsCorpBulk" runat="server" Text="Can post" /></span>
                            <span class="check"><asp:CheckBox ID="chkIsCorpBulkAppr" runat="server" Text="Can approve" /></span>
                        </div>
                    </div>
                    <div class="full">
                        <p class="group-label">Other</p>
                        <div class="check-list">
                            <span class="check"><asp:CheckBox ID="chkCorpBal" runat="server" Text="Can view balance" /></span>
                            <span class="check"><asp:CheckBox ID="chkCorpAppr" runat="server" Text="Is approving officer" /></span>
                            <span class="check"><asp:CheckBox ID="ChkIsCorpStatus" runat="server" Text="Can view status" /></span>
                        </div>
                    </div>
                </div>
            </div>
        </section>

        <!-- ================= Contact & organisation ================= -->
        <section class="card">
            <div class="card-head">
                <h2>Contact and organisation</h2>
            </div>
            <div class="card-body">
                <div class="field-grid">
                    <div>
                        <asp:Label runat="server" AssociatedControlID="txtPhoneNo" CssClass="form-label">Contact phone number</asp:Label>
                        <asp:TextBox ID="txtPhoneNo" runat="server" TextMode="Phone" CssClass="form-control" autocomplete="tel" />
                    </div>
                    <div>
                        <asp:Label runat="server" AssociatedControlID="txtEmail" CssClass="form-label">Contact email</asp:Label>
                        <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" CssClass="form-control" autocomplete="email" />
                    </div>
                    <div class="full">
                        <asp:Label runat="server" AssociatedControlID="txtAcctNo" CssClass="form-label">Organisation name</asp:Label>
                        <asp:DropDownList ID="txtAcctNo" runat="server" CssClass="form-control"
                            DataSourceID="sqlCorpProfile" DataTextField="CorpDesc" DataValueField="CorpCode"></asp:DropDownList>
                    </div>
                </div>
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

        <asp:SqlDataSource ID="sqlCorpProfile" runat="server"
            ConnectionString="<%$ ConnectionStrings:EazyConnect %>"
            SelectCommand="SpGetAllCorporateProfile"
            SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:Parameter DefaultValue="0" Name="CorpStatus" Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>

    </div>

    <!-- Site loading animation (kept from the original page) -->
    <script src="../Resources/libs/jquery/jquery.min.js"></script>
    <script src="../Resources/js/hidehref.js"></script>
    <script src="../Resources/js/preloader.js"></script>
    <script src="../showOrHideLoadingAnimation.js"></script>
    </form>
</body>
</html>