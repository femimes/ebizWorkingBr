<%@ Page Language="C#" %>
<%-- If you already have a code-behind for this page, keep your existing <%@ Page ... %> line here instead. --%>

<script runat="server">
    protected string Greeting()
    {
        int h = DateTime.Now.Hour;
        return h < 12 ? "Good morning" : (h < 17 ? "Good afternoon" : "Good evening");
    }
    // The login page stores "username?branchcode" in the LogonUserID cookie and the auth ticket.
    // Read it safely (no crash if the cookie is missing) and split it into its two parts.
    protected string LogonId()
    {
        HttpCookie c = Request.Cookies["LogonUserID"];
        if (c != null && !string.IsNullOrEmpty(c.Value)) { return c.Value; }

        if (Page.User != null && Page.User.Identity.IsAuthenticated) { return Page.User.Identity.Name; }

        return "";
    }
      protected string LogonIdName()
    {
        HttpCookie c = Request.Cookies["LogonUserIDName"];
        if (c != null && !string.IsNullOrEmpty(c.Value)) { return c.Value; }

        if (Page.User != null && Page.User.Identity.IsAuthenticated) { return Page.User.Identity.Name; }

        return "";
    }
    protected string LogonPart(int index)
    {
        string[] parts = LogonId().Split(new[] { '?' }, 2);
        return parts.Length > index ? parts[index].Trim() : "";
    }
     protected string LogonBranchNamePart(int index)
    {
        string[] parts = LogonIdName().Split(new[] { '?' }, 2);
        return parts.Length > index ? parts[index].Trim() : "";
    }
    protected string LogonUser()   { return Server.HtmlEncode(LogonPart(0)); }
    protected string LogonBranch() { return Server.HtmlEncode(LogonPart(1)); }
    protected string LogonBranchName() { return Server.HtmlEncode(LogonBranchNamePart(1)); }
    protected string DisplayName()
    {
        return LogonUser();
    }
    // Current server date and time, shown when the page loads (the script below keeps it ticking)
    protected string NowText()
    {
        return DateTime.Now.ToString("dd MMM yyyy, hh:mm:ss tt");
    }
</script>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Home</title>
    <link rel="preconnect" href="https://fonts.googleapis.com" />
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin="anonymous" />
    <link href="https://fonts.googleapis.com/css2?family=Plus+Jakarta+Sans:wght@400;500;600;700&display=swap" rel="stylesheet" />
    <style type="text/css">
        :root {
            --brand: #0F4C81;
            --brand-dark: #0a355a;
            --ink: #14212e;
            --muted: #5d6b7a;
            --line: #e1e8ef;
            --page: #f4f7fa;
            --surface: #ffffff;
            --ok: #1b6e3c;      --ok-bg: #e6f4ea;
            --warn: #8a5a00;    --warn-bg: #fff4dc;
            --bad: #b3261e;     --bad-bg: #fdeceb;
            --ring: rgba(15, 76, 129, .25);
        }
        * { box-sizing: border-box; }
        body {
            margin: 0;
            padding: 28px;
            font-family: "Plus Jakarta Sans", "Segoe UI", Roboto, Arial, sans-serif;
            color: var(--ink);
            background: var(--page);
            -webkit-font-smoothing: antialiased;
        }
        .wrap { max-width: 1100px; margin: 0 auto; }

        /* Hero, echoes the login brand panel */
        .hero {
            position: relative;
            overflow: hidden;
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 24px;
            padding: 32px 36px;
            color: #fff;
            border-radius: 16px;
            background: linear-gradient(160deg, var(--brand) 0%, #082c4d 100%);
        }
        .hero::before, .hero::after {
            content: ""; position: absolute; border-radius: 50%;
            border: 1px solid rgba(255, 255, 255, .14); pointer-events: none;
        }
        .hero::before { width: 420px; height: 420px; top: -200px; right: -90px; }
        .hero::after  { width: 260px; height: 260px; top: -120px; right: -10px; }
        .hero > * { position: relative; }
        .hero h1 { margin: 0 0 6px; font-size: 28px; line-height: 1.25; font-weight: 700; letter-spacing: -.01em; }
        .hero p { margin: 0; font-size: 15px; color: rgba(255, 255, 255, .8); }

        .btn {
            display: inline-block;
            padding: 13px 22px;
            font: inherit;
            font-size: 15px;
            font-weight: 600;
            text-decoration: none;
            white-space: nowrap;
            color: var(--brand);
            background: #fff;
            border-radius: 10px;
            transition: background-color .15s ease;
        }
        .btn:hover { background: #eaf1f8; }
        .btn:focus-visible { outline: 3px solid rgba(255, 255, 255, .6); outline-offset: 2px; }

        /* Stats */
        .stats {
            display: grid;
            grid-template-columns: repeat(4, 1fr);
            gap: 16px;
            margin: 20px 0;
        }
        .stat {
            padding: 20px;
            background: var(--surface);
            border: 1px solid var(--line);
            border-radius: 12px;
        }
        .stat .label { font-size: 13px; font-weight: 500; color: var(--muted); }
        .stat .value { margin-top: 8px; font-size: 26px; font-weight: 700; letter-spacing: -.01em; }
        .stat .value.clock { font-size: 20px; font-variant-numeric: tabular-nums; }
        .stat .note { margin-top: 4px; font-size: 13px; color: var(--muted); }
        .stat.attention { border-left: 4px solid var(--brand); }

        /* Payments table */
        .panel {
            background: var(--surface);
            border: 1px solid var(--line);
            border-radius: 12px;
        }
        .panel-head {
            display: flex; align-items: baseline; justify-content: space-between;
            padding: 18px 22px;
            border-bottom: 1px solid var(--line);
        }
        .panel-head h2 { margin: 0; font-size: 17px; font-weight: 700; }
        .panel-head a { font-size: 14px; font-weight: 600; color: var(--brand); text-decoration: none; }
        .panel-head a:hover { text-decoration: underline; }

        .table-wrap { overflow-x: auto; }
        table { width: 100%; border-collapse: collapse; font-size: 14px; }
        th, td { padding: 14px 22px; text-align: left; white-space: nowrap; }
        th { font-size: 13px; font-weight: 600; color: var(--muted); background: #f8fafc; }
        td { border-top: 1px solid var(--line); }
        tbody tr:hover { background: #f8fafc; }
        .num { text-align: right; font-variant-numeric: tabular-nums; }

        .pill {
            display: inline-block; padding: 4px 10px;
            font-size: 12px; font-weight: 600; border-radius: 999px;
        }
        .pill.paid { color: var(--ok); background: var(--ok-bg); }
        .pill.pending { color: var(--warn); background: var(--warn-bg); }
        .pill.failed { color: var(--bad); background: var(--bad-bg); }

        @media (max-width: 900px) { .stats { grid-template-columns: repeat(2, 1fr); } }
        @media (max-width: 600px) {
            body { padding: 14px; }
            .hero { flex-direction: column; align-items: flex-start; padding: 24px; }
            .stats { grid-template-columns: 1fr; }
            th, td { padding: 12px 14px; }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="wrap">

        <section class="hero">
            <div>
                <h1><%= Greeting() %><%= DisplayName() != "" ? ", " + DisplayName() : "" %></h1>
                <p>Here is where your account stands today.</p>
            </div>
            <!-- Point this at your subscription page -->
            <a class="btn" href="#">Chat Administrator</a>
        </section>

        <!-- SAMPLE DATA: replace each value with your own (Labels or <%=45 %> from code-behind) -->
        <section class="stats" aria-label="Account summary">
            <div class="stat">
                <div class="label">Signed in as</div>
                <div class="value"><%= LogonUser() != "" ? LogonUser() : "Not signed in" %></div>
                <div class="note"><%= LogonBranchName() != "" ? "Branch: " + LogonBranchName() : "No branch selected" %></div>
            </div>
            <div class="stat">
                <div class="label">Total Request Pending Approval</div>
                <div class="value">0</div>
                <%--<div class="note">12 payments</div>--%>
            </div>
            <div class="stat attention">
                <div class="label">System DateTime</div>
                <div class="value clock" id="systemClock"
                     data-server-ms="<%= (long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds %>"><%= NowText() %></div>
                <%--<div class="note">&#8358;20,000</div>--%>
            </div>
            <div class="stat">
                <div class="label">Outstandings</div>
                <div class="value">0</div>
                <%--<div class="value">&#8358;0</div>--%>
                <div class="note">You are up to date</div>
            </div>
        </section>

        <section class="panel">
            <div class="panel-head">
                <h2>Recent Activity</h2>
                <a href="#">View all Request</a>
            </div>
            <div class="table-wrap">
                <table>
                    <thead>
                        <tr>
                            <th scope="col">Date</th>
                            <th scope="col">Product</th>
                            <th scope="col">Reference</th>
                            <th scope="col" class="num">Amount</th>
                            <th scope="col">Status</th>
                        </tr>
                    </thead>
                    <%--<tbody>
                        <tr><td>15 Oct 2026</td><td>Product A</td><td>REF-100245</td><td class="num">&#8358;20,000</td><td><span class="pill paid">Paid</span></td></tr>
                        <tr><td>15 Sep 2026</td><td>Product A</td><td>REF-100198</td><td class="num">&#8358;20,000</td><td><span class="pill paid">Paid</span></td></tr>
                        <tr><td>02 Sep 2026</td><td>Product B</td><td>REF-100176</td><td class="num">&#8358;45,000</td><td><span class="pill pending">Pending</span></td></tr>
                        <tr><td>15 Aug 2026</td><td>Product A</td><td>REF-100121</td><td class="num">&#8358;20,000</td><td><span class="pill failed">Failed</span></td></tr>
                    </tbody>--%>
                </table>
            </div>
        </section>

    </div>
    </form>

    <script type="text/javascript">
        // Keep the System DateTime card showing the current server time, updated every second.
        (function () {
            var el = document.getElementById('systemClock');
            if (!el) { return; }

            // Offset between the server clock and this browser's clock, so the time stays the server's
            var serverMs = parseInt(el.getAttribute('data-server-ms'), 10);
            var offset = isNaN(serverMs) ? 0 : serverMs - Date.now();

            var months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
            function pad(n) { return (n < 10 ? '0' : '') + n; }

            function tick() {
                // serverMs was taken from the server's local time, so read it back with UTC getters
                var d = new Date(Date.now() + offset);
                var h = d.getUTCHours(), ampm = h < 12 ? 'AM' : 'PM';
                h = h % 12 || 12;
                el.textContent = pad(d.getUTCDate()) + ' ' + months[d.getUTCMonth()] + ' ' + d.getUTCFullYear() +
                    ', ' + pad(h) + ':' + pad(d.getUTCMinutes()) + ':' + pad(d.getUTCSeconds()) + ' ' + ampm;
            }

            tick();
            setInterval(tick, 1000);
        })();
    </script>
</body>
</html>
