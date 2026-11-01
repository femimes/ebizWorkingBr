<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmAppKYCTiers.aspx.cs" Inherits="Processing_FrmAppKYCTiers" %>

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

        .form-label { display: block; margin-bottom: 6px; font-size: 12px; font-weight: 600; }
        .type-field { max-width: 280px; margin-bottom: 18px; }
        select.form-control {
            display: block; width: 100%; height: 36px; padding: 0 34px 0 10px;
            font: inherit; font-size: 13px; color: var(--ink); background-color: var(--surface);
            border: 1.5px solid var(--field); border-radius: 9px; outline: none; cursor: pointer;
            appearance: none; -webkit-appearance: none;
            background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='12' height='8' viewBox='0 0 12 8' fill='none'%3E%3Cpath d='M1 1.5l5 5 5-5' stroke='%235d6b7a' stroke-width='1.6' stroke-linecap='round' stroke-linejoin='round'/%3E%3C/svg%3E");
            background-repeat: no-repeat; background-position: right 12px center;
            transition: border-color .15s ease, box-shadow .15s ease;
        }
        select.form-control:hover { border-color: #aebbc9; }
        select.form-control:focus { border-color: var(--brand); box-shadow: 0 0 0 4px var(--ring); }

        .picker { display: grid; grid-template-columns: 1fr auto 1fr; gap: 16px; align-items: center; }
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

        @media (max-width: 760px) {
            body { padding: 14px; }
            .card-body { padding: 14px; }
            .type-field { max-width: none; }
            .picker { grid-template-columns: 1fr; }
            .move-buttons { flex-direction: row; justify-content: center; padding-top: 0; }
            .move-buttons input.move-btn { transform: rotate(90deg); }
            .list-box { height: 200px; }
        }
        @media (prefers-reduced-motion: reduce) { * { transition: none !important; } }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="wrap">

        <header class="page-head">
            <h1>KYC tiering requirements</h1>
            <p>Choose a tier, then move requirements into Selected requirements to apply them to that tier.</p>
        </header>

        <section class="card">
            <div class="card-head">
                <h2>Requirements</h2>
                <span class="sub">Pick a requirement, then use the arrows</span>
            </div>
            <div class="card-body">
                <div class="type-field">
                    <asp:Label runat="server" AssociatedControlID="drpTiers" CssClass="form-label">KYC tier</asp:Label>
                    <asp:DropDownList ID="drpTiers" runat="server" CssClass="form-control" AutoPostBack="true"
                        OnSelectedIndexChanged="drpTiers_SelectedIndexChanged">
                        <asp:ListItem Value="0" Selected="True">Tier 0</asp:ListItem>
                        <asp:ListItem Value="1">Tier 1</asp:ListItem>
                        <asp:ListItem Value="2">Tier 2</asp:ListItem>
                        <asp:ListItem Value="3">Tier 3</asp:ListItem>
                        <asp:ListItem Value="4">Tier 4</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="picker">
                    <div>
                        <asp:Label runat="server" AssociatedControlID="ListBox1" CssClass="form-label">Available requirements</asp:Label>
                        <asp:ListBox ID="ListBox1" runat="server" CssClass="list-box"></asp:ListBox>
                    </div>

                    <div class="move-buttons">
                        <asp:ImageButton ID="ImageButton1" runat="server" CssClass="move-btn" OnClick="ImageButton1_Click"
                            AlternateText="Add selected requirement" ToolTip="Add selected requirement"
                            ImageUrl="data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='20' height='20' viewBox='0 0 24 24' fill='none' stroke='%230F4C81' stroke-width='2.4' stroke-linecap='round' stroke-linejoin='round'%3E%3Cpath d='M5 12h14M13 6l6 6-6 6'/%3E%3C/svg%3E" />
                        <asp:ImageButton ID="ImageButton2" runat="server" CssClass="move-btn" OnClick="ImageButton2_Click"
                            AlternateText="Remove selected requirement" ToolTip="Remove selected requirement"
                            ImageUrl="data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='20' height='20' viewBox='0 0 24 24' fill='none' stroke='%230F4C81' stroke-width='2.4' stroke-linecap='round' stroke-linejoin='round'%3E%3Cpath d='M19 12H5M11 6l-6 6 6 6'/%3E%3C/svg%3E" />
                    </div>

                    <div>
                        <asp:Label runat="server" AssociatedControlID="ListBox2" CssClass="form-label">Selected requirements</asp:Label>
                        <asp:ListBox ID="ListBox2" runat="server" CssClass="list-box"></asp:ListBox>
                    </div>
                </div>
            </div>
        </section>

    </div>
    </form>
</body>
</html>