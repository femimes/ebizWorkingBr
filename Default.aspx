<%@ Page Title="Home - eBusiness Plus" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <link rel="preconnect" href="https://fonts.googleapis.com" />
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin="anonymous" />
    <link href="https://fonts.googleapis.com/css2?family=Plus+Jakarta+Sans:wght@400;500;600;700&display=swap" rel="stylesheet" />
    <style type="text/css">
        .home {
            --brand: #0f4c81; --brand-dark: #08365a; --ink: #1f2937; --muted: #6b7280;
            --line: #e5e7eb; --surface: #ffffff; --tint: #eef4fb;
            font-family: "Plus Jakarta Sans", "Segoe UI", Arial, sans-serif; color: var(--ink);
            max-width: 1080px; margin: 0 auto; padding: 24px 8px 40px;
        }
        .home * { box-sizing: border-box; }
        .home h1, .home h2, .home h3 { font-variant: normal; text-transform: none; letter-spacing: normal; }
        .home .hero {
            position: relative; overflow: hidden; border-radius: 16px; padding: 40px 36px;
            color: #fff; background: linear-gradient(120deg, var(--brand) 0%, #1a73e8 100%);
            box-shadow: 0 12px 30px rgba(15, 76, 129, .25);
        }
        .home .hero::after {
            content: ""; position: absolute; right: -80px; top: -80px; width: 280px; height: 280px;
            border-radius: 50%; background: rgba(255, 255, 255, .08);
        }
        .home .hero .eyebrow { margin: 0 0 6px; font-size: 13px; font-weight: 600; letter-spacing: .08em; text-transform: uppercase; opacity: .85; }
        .home .hero h1 { margin: 0 0 10px; font-size: 32px; line-height: 1.2; font-weight: 700; color: #fff; }
        .home .hero p { margin: 0; max-width: 620px; font-size: 16px; line-height: 1.6; opacity: .92; }
        .home .hero .actions { margin-top: 24px; display: flex; flex-wrap: wrap; gap: 10px; position: relative; z-index: 1; }
        .home .btn { display: inline-block; padding: 11px 20px; border-radius: 8px; font-size: 14px; font-weight: 600; text-decoration: none; transition: background .15s, color .15s; }
        .home .btn-light { color: var(--brand); background: #fff; }
        .home .btn-light:hover { background: var(--tint); }
        .home .btn-ghost { color: #fff; border: 1px solid rgba(255, 255, 255, .6); }
        .home .btn-ghost:hover { background: rgba(255, 255, 255, .12); }

        .home h2.section { margin: 36px 0 14px; font-size: 18px; font-weight: 700; color: var(--ink); }
        .home .tiles { display: grid; grid-template-columns: repeat(3, minmax(0, 1fr)); gap: 16px; }
        .home .tile {
            display: block; padding: 22px; border: 1px solid var(--line); border-radius: 12px; background: var(--surface);
            color: inherit; text-decoration: none; transition: border-color .15s, box-shadow .15s, transform .15s;
        }
        .home .tile:hover { border-color: var(--brand); box-shadow: 0 8px 20px rgba(15, 76, 129, .12); transform: translateY(-2px); }
        .home .tile .icon { display: inline-flex; align-items: center; justify-content: center; width: 40px; height: 40px; margin-bottom: 12px; border-radius: 10px; font-size: 20px; background: var(--tint); }
        .home .tile h3 { margin: 0 0 6px; font-size: 16px; font-weight: 700; color: var(--brand); }
        .home .tile p { margin: 0; font-size: 14px; line-height: 1.55; color: var(--muted); }
        .home .thanks { margin-top: 32px; padding: 16px 20px; border-radius: 10px; background: var(--tint); font-size: 14px; color: var(--muted); }
        .home .thanks a { color: var(--brand); font-weight: 600; }
        @media (max-width: 820px) {
            .home .tiles { grid-template-columns: 1fr; }
            .home .hero { padding: 30px 22px; }
            .home .hero h1 { font-size: 26px; }
        }
    </style>
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="home">
        <section class="hero">
            <p class="eyebrow">eBusiness Plus</p>
            <h1><asp:Literal ID="litGreeting" runat="server" Text="Welcome to eBusiness Plus" /></h1>
            <p>Back-office banking in one place: card requests and maintenance, direct-debit mandates, approvals and reports.</p>
            <div class="actions">
                <asp:HyperLink ID="lnkSignIn" runat="server" NavigateUrl="~/Account/Login.aspx" CssClass="btn btn-light" Text="Sign in" />
                <a class="btn btn-ghost" href="mailto:femimes@gmail.com?subject=eBusiness%20Plus%20subscription">Subscribe or ask a question</a>
            </div>
        </section>

        <h2 class="section">Get started</h2>
        <div class="tiles">
            <a class="tile" href="Request/CardRequest.aspx">
                <span class="icon" aria-hidden="true">&#128179;</span>
                <h3>Card requests</h3>
                <p>Request new cards for customers, confirm batches and link cards to accounts.</p>
            </a>
            <a class="tile" href="NDD/NDDeMandate.aspx">
                <span class="icon" aria-hidden="true">&#128260;</span>
                <h3>Direct debit (NDD)</h3>
                <p>Set up e-mandates, billers and products, and follow mandate status.</p>
            </a>
            <a class="tile" href="Approvals/ApproveCardMaint.aspx">
                <span class="icon" aria-hidden="true">&#9989;</span>
                <h3>Approvals</h3>
                <p>Review and approve requests waiting for an authoriser.</p>
            </a>
        </div>

        <div class="thanks">
            Thank you for choosing eBusiness Plus. To learn more or subscribe, email <a href="mailto:femimes@gmail.com">femimes@gmail.com</a>.
        </div>
    </div>
</asp:Content>
