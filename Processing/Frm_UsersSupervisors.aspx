<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Frm_UsersSupervisors.aspx.vb" Inherits="Processing_Frm_UsersSupervisors" Title="Sms Classique" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>User supervisors</title>

    <script type="text/javascript">
        function Button1_onclick() {
            history.back();
        }
    </script>

    <link href="../App_Themes/White/Default.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/SiteAll.css" rel="stylesheet" type="text/css" />
    <link rel="preconnect" href="https://fonts.googleapis.com" />
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin="anonymous" />
    <link href="https://fonts.googleapis.com/css2?family=Plus+Jakarta+Sans:wght@400;500;600;700&display=swap" rel="stylesheet" />

    <!-- Placed after the site stylesheets so these styles win -->
    <style type="text/css">
        :root {
            --brand: #0F4C81; --brand-dark: #0a355a;
            --ink: #14212e; --muted: #5d6b7a;
            --line: #e1e8ef; --field: #d3dce6;
            --page: #f4f7fa; --surface: #ffffff; --tint: #eaf1f8;
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
        .page-head .user-line { display: block; margin-top: 8px; font-size: 13px; font-weight: 600; color: var(--brand); }
        .page-head .user-line:empty { display: none; }

        .card { margin-bottom: 16px; background: var(--surface); border: 1px solid var(--line); border-radius: 12px; }
        .card-head {
            display: flex; align-items: center; justify-content: space-between; gap: 12px;
            padding: 14px 20px; border-bottom: 1px solid var(--line);
        }
        .card-head h2 { margin: 0; font-size: 14px; font-weight: 700; }
        .card-head .sub { font-size: 12px; color: var(--muted); }
        .card-body { padding: 18px 20px; }

        .picker { display: grid; grid-template-columns: 1fr auto 1fr; gap: 16px; align-items: center; }
        .form-label { display: block; margin-bottom: 6px; font-size: 12px; font-weight: 600; }
        .list-box {
            display: block; width: 100%; height: 260px; padding: 6px;
            font: inherit; font-size: 13px; color: var(--ink); background: var(--surface);
            border: 1.5px solid var(--field); border-radius: 10px; outline: none;
            transition: border-color .15s ease, box-shadow .15s ease;
        }
        .list-box:focus { border-color: var(--brand); box-shadow: 0 0 0 4px var(--ring); }
        .list-box option { padding: 7px 10px; border-radius: 6px; }
        .list-box option:checked { background: var(--tint); color: var(--brand); font-weight: 600; }

        .move-buttons { display: flex; flex-direction: column; gap: 10px; padding-top: 22px; }
        input.move-btn {
            width: 40px; height: 40px; padding: 10px;
            background: var(--tint); border: 1.5px solid transparent; border-radius: 10px;
            cursor: pointer; transition: background-color .15s ease;
        }
        input.move-btn:hover { background: #dbe7f3; }
        input.move-btn:focus-visible { outline: none; box-shadow: 0 0 0 4px var(--ring); }

        .btn {
            display: inline-flex; align-items: center; justify-content: center;
            height: 36px; padding: 0 16px; font: inherit; font-size: 13px; font-weight: 600;
            white-space: nowrap; border: 1.5px solid transparent; border-radius: 9px; cursor: pointer;
            transition: background-color .15s ease;
        }
        .btn:focus-visible { outline: none; box-shadow: 0 0 0 4px var(--ring); }
        .btn-secondary { color: var(--ink); background: var(--surface); border-color: var(--field); }
        .btn-secondary:hover { background: #f1f5f9; }

        .action-bar {
            display: flex; align-items: center; gap: 10px; flex-wrap: wrap;
            padding: 12px 20px; background: var(--surface);
            border: 1px solid var(--line); border-radius: 12px;
        }

        @media (max-width: 760px) {
            body { padding: 14px; }
            .card-body { padding: 14px; }
            .picker { grid-template-columns: 1fr; }
            .move-buttons { flex-direction: row; justify-content: center; padding-top: 0; }
            .move-buttons input.move-btn { transform: rotate(90deg); }
            .list-box { height: 200px; }
            .action-bar .btn { flex: 1 1 auto; }
        }
        @media (prefers-reduced-motion: reduce) { * { transition: none !important; } }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="wrap">

        <header class="page-head">
            <h1>User supervisors</h1>
            <p>Move supervisors into Selected items to assign them to this user.</p>
            <asp:Label ID="user_id" runat="server" CssClass="user-line"></asp:Label>
        </header>

        <section class="card">
            <div class="card-head">
                <h2>Supervisors</h2>
                <span class="sub">Pick a supervisor, then use the arrows</span>
            </div>
            <div class="card-body">
                <div class="picker">
                    <div>
                        <asp:Label runat="server" AssociatedControlID="ListBox1" CssClass="form-label">Available items</asp:Label>
                        <asp:ListBox ID="ListBox1" runat="server" CssClass="list-box"></asp:ListBox>
                    </div>

                    <div class="move-buttons">
                        <asp:ImageButton ID="ImageButton1" runat="server" CssClass="move-btn"
                            AlternateText="Add selected supervisor" ToolTip="Add selected supervisor"
                            ImageUrl="data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='20' height='20' viewBox='0 0 24 24' fill='none' stroke='%230F4C81' stroke-width='2.4' stroke-linecap='round' stroke-linejoin='round'%3E%3Cpath d='M5 12h14M13 6l6 6-6 6'/%3E%3C/svg%3E" />
                        <asp:ImageButton ID="ImageButton2" runat="server" CssClass="move-btn"
                            AlternateText="Remove selected supervisor" ToolTip="Remove selected supervisor"
                            ImageUrl="data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='20' height='20' viewBox='0 0 24 24' fill='none' stroke='%230F4C81' stroke-width='2.4' stroke-linecap='round' stroke-linejoin='round'%3E%3Cpath d='M19 12H5M11 6l-6 6 6 6'/%3E%3C/svg%3E" />
                    </div>

                    <div>
                        <asp:Label runat="server" AssociatedControlID="ListBox2" CssClass="form-label">Selected items</asp:Label>
                        <asp:ListBox ID="ListBox2" runat="server" CssClass="list-box"></asp:ListBox>
                    </div>
                </div>
            </div>
        </section>

        <div class="action-bar">
            <asp:Button ID="Button1" runat="server" Text="Back to user profiles" CssClass="btn btn-secondary" />
        </div>

        <asp:ValidationSummary ID="ValidationSummary1" runat="server"
            HeaderText="The following errors were found:" ShowSummary="False" ShowMessageBox="True" />

    </div>
    </form>
</body>
</html>