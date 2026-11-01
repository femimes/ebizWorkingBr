<%@ Page Title="Log In" Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Account_Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Log In</title>
    <link rel="preconnect" href="https://fonts.googleapis.com" />
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin="anonymous" />
    <link href="https://fonts.googleapis.com/css2?family=Plus+Jakarta+Sans:wght@400;500;600;700&display=swap" rel="stylesheet" />
    <style type="text/css">
        :root {
            --brand: #0F4C81;
            --brand-dark: #0a355a;
            --ink: #14212e;
            --muted: #5d6b7a;
            --line: #d3dce6;
            --surface: #ffffff;
            --page: #f4f7fa;
            --danger: #b3261e;
            --danger-bg: #fdeceb;
            --ring: rgba(15, 76, 129, .22);
        }

        * { box-sizing: border-box; }

        html, body { height: 100%; }

        body {
            margin: 0;
            font-family: "Plus Jakarta Sans", "Segoe UI", Roboto, Arial, sans-serif;
            color: var(--ink);
            background: var(--page);
            -webkit-font-smoothing: antialiased;
        }

        form { min-height: 100%; }

        /* ---------- Layout ---------- */
        .login-container {
            min-height: 100vh;
            display: grid;
            grid-template-columns: minmax(320px, 5fr) 7fr;
        }

        .brand-panel {
            position: relative;
            overflow: hidden;
            display: flex;
            flex-direction: column;
            justify-content: flex-end;
            padding: 56px;
            color: #fff;
            background: linear-gradient(160deg, var(--brand) 0%, #082c4d 100%);
        }

        .brand-panel::before,
        .brand-panel::after {
            content: "";
            position: absolute;
            border-radius: 50%;
            border: 1px solid rgba(255, 255, 255, .14);
            pointer-events: none;
        }
        .brand-panel::before { width: 560px; height: 560px; top: -200px; right: -220px; }
        .brand-panel::after  { width: 380px; height: 380px; top: -110px; right: -130px; }

        .brand-panel h2 {
            position: relative;
            margin: 0 0 14px;
            font-size: 34px;
            line-height: 1.2;
            font-weight: 700;
            letter-spacing: -.01em;
            max-width: 15em;
        }

        .brand-panel p {
            position: relative;
            margin: 0;
            max-width: 26em;
            font-size: 16px;
            line-height: 1.6;
            color: rgba(255, 255, 255, .8);
        }

        .form-panel {
            display: flex;
            align-items: center;
            justify-content: center;
            padding: 40px 24px;
        }

        .login-card {
            width: 100%;
            max-width: 400px;
        }

        /* ---------- Header ---------- */
        .logo-section { margin-bottom: 36px; }
        .logo-section img { display: block; height: 44px; width: auto; max-width: 180px; }

        .page-title {
            margin: 0 0 6px;
            font-size: 30px;
            line-height: 1.2;
            font-weight: 700;
            letter-spacing: -.015em;
        }

        .page-subtitle {
            margin: 0 0 28px;
            color: var(--muted);
            font-size: 15px;
        }

        /* ---------- Fields ---------- */
        .form-group { margin-bottom: 20px; }

        .form-label {
            display: block;
            margin-bottom: 7px;
            font-size: 14px;
            font-weight: 600;
            color: var(--ink);
        }

        .form-control {
            display: block;
            width: 100%;
            height: 48px;
            padding: 0 14px;
            font: inherit;
            font-size: 15px;
            color: var(--ink);
            background-color: var(--surface);
            border: 1.5px solid var(--line);
            border-radius: 10px;
            outline: none;
            transition: border-color .15s ease, box-shadow .15s ease;
        }

        .form-control::placeholder { color: #98a4b1; }
        .form-control:hover { border-color: #aebbc9; }
        .form-control:focus { border-color: var(--brand); box-shadow: 0 0 0 4px var(--ring); }

        select.form-control {
            appearance: none;
            -webkit-appearance: none;
            padding-right: 40px;
            cursor: pointer;
            background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='14' height='9' viewBox='0 0 14 9' fill='none'%3E%3Cpath d='M1 1.5l6 6 6-6' stroke='%235d6b7a' stroke-width='1.8' stroke-linecap='round' stroke-linejoin='round'/%3E%3C/svg%3E");
            background-repeat: no-repeat;
            background-position: right 16px center;
        }

        .password-wrap { position: relative; }
        .password-wrap .form-control { padding-right: 70px; }

        .toggle-password {
            position: absolute;
            top: 50%;
            right: 6px;
            transform: translateY(-50%);
            height: 36px;
            padding: 0 10px;
            font: inherit;
            font-size: 13px;
            font-weight: 600;
            color: var(--brand);
            background: none;
            border: 0;
            border-radius: 8px;
            cursor: pointer;
        }
        .toggle-password:hover { background: #eaf1f8; }
        .toggle-password:focus-visible { outline: 2px solid var(--brand); }

        .hint {
            display: block;
            margin-top: 6px;
            font-size: 13px;
            color: var(--muted);
        }

        #capsWarning { display: none; color: #8a5a00; }

        .field-error {
            display: block;
            margin-top: 6px;
            font-size: 13px;
            font-weight: 500;
            color: var(--danger);
        }

        /* Mark a field red once its validator fires */
        .form-control.has-error { border-color: var(--danger); }
        .form-control.has-error:focus { box-shadow: 0 0 0 4px rgba(179, 38, 30, .18); }

        /* ---------- Alert ---------- */
        .alert {
            margin-bottom: 20px;
            padding: 12px 14px;
            font-size: 14px;
            line-height: 1.45;
            color: var(--danger);
            background: var(--danger-bg);
            border-left: 4px solid var(--danger);
            border-radius: 8px;
        }
        .alert:empty { display: none; }

        /* ---------- Remember me ---------- */
        .remember-section {
            display: flex;
            align-items: center;
            gap: 10px;
            margin: 4px 0 24px;
            font-size: 14px;
            color: var(--ink);
        }
        .remember-section input[type="checkbox"] {
            width: 18px;
            height: 18px;
            margin: 0;
            accent-color: var(--brand);
            cursor: pointer;
        }
        .remember-section label { cursor: pointer; }

        /* ---------- Button ---------- */
        .login-btn {
            display: block;
            width: 100%;
            height: 50px;
            font: inherit;
            font-size: 16px;
            font-weight: 600;
            color: #fff;
            background: var(--brand);
            border: 0;
            border-radius: 10px;
            cursor: pointer;
            transition: background-color .15s ease, box-shadow .15s ease, transform .05s ease;
        }
        .login-btn:hover { background: var(--brand-dark); }
        .login-btn:focus-visible { outline: none; box-shadow: 0 0 0 4px var(--ring); }
        .login-btn:active { transform: translateY(1px); }
        .login-btn.is-busy { opacity: .75; cursor: progress; pointer-events: none; }

        /* ---------- Responsive ---------- */
        @media (max-width: 900px) {
            .login-container { grid-template-columns: 1fr; }
            .brand-panel { display: none; }
            .form-panel { align-items: flex-start; padding: 32px 20px 48px; }
            .logo-section { margin-bottom: 28px; }
        }

        @media (prefers-reduced-motion: reduce) {
            * { transition: none !important; }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-container">

            <!-- Brand side (hidden on phones and tablets) -->
            <aside class="brand-panel" aria-hidden="true">
                <h2>Everything for your branch, in one place.</h2>
                <p>Enter your username and your available branches will appear. Pick one and you are ready to go.</p>
            </aside>

            <!-- Form side -->
            <main class="form-panel">
                <asp:Login ID="LoginUser" runat="server"
                    EnableViewState="true"
                    RenderOuterTable="false">

                    <LayoutTemplate>

                        <div class="login-card">

                            <div class="logo-section">
                                <asp:Image ID="CompanyLogo"
                                    runat="server"
                                    AlternateText="Company logo"
                                    ImageUrl="~/Images/imageswywyw.jpg" />
                            </div>

                            <h1 class="page-title">Welcome back</h1>
                            <p class="page-subtitle">Sign in to continue.</p>

                            <div class="alert" role="alert"><asp:Literal ID="FailureText" runat="server" /></div>

                            <div class="form-group">
                                <asp:Label ID="UserNameLabel" runat="server"
                                    AssociatedControlID="UserName"
                                    CssClass="form-label">Username</asp:Label>

                                <asp:TextBox
                                    ID="UserName"
                                    runat="server"
                                    AutoPostBack="true"
                                    OnTextChanged="UserName_TextChanged"
                                    CssClass="form-control"
                                    autocomplete="username"
                                    autocapitalize="none"
                                    spellcheck="false" />

                                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server"
                                    ControlToValidate="UserName"
                                    CssClass="field-error"
                                    Display="Dynamic"
                                    ErrorMessage="Enter your username."
                                    ValidationGroup="LoginUserValidationGroup">Enter your username.</asp:RequiredFieldValidator>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="PasswordLabel" runat="server"
                                    AssociatedControlID="Password"
                                    CssClass="form-label">Password</asp:Label>

                                <div class="password-wrap">
                                    <asp:TextBox
                                        ID="Password"
                                        runat="server"
                                        TextMode="Password"
                                        CssClass="form-control"
                                        autocomplete="current-password" />

                                    <button type="button" class="toggle-password" aria-pressed="false"
                                        aria-label="Show password" onclick="togglePassword(this)">Show</button>
                                </div>

                                <span id="capsWarning" class="hint" role="status">Caps Lock is on.</span>

                                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server"
                                    ControlToValidate="Password"
                                    CssClass="field-error"
                                    Display="Dynamic"
                                    ErrorMessage="Enter your password."
                                    ValidationGroup="LoginUserValidationGroup">Enter your password.</asp:RequiredFieldValidator>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="BranchLabel" runat="server"
                                    AssociatedControlID="ASPxComboBox1"
                                    CssClass="form-label">Branch</asp:Label>

                                <asp:DropDownList
                                    ID="ASPxComboBox1"
                                    runat="server"
                                    
                                    CssClass="form-control">
                                   <%-- <asp:ListItem Text="Select your branch" Value="" />--%>
                                     
                                </asp:DropDownList>

                                <asp:RequiredFieldValidator ID="BranchRequired" runat="server"
                                    ControlToValidate="ASPxComboBox1"
                                    InitialValue=""
                                    CssClass="field-error"
                                    Display="Dynamic"
                                    ErrorMessage="Select your branch."
                                    ValidationGroup="LoginUserValidationGroup">Select your branch.</asp:RequiredFieldValidator>
                            </div>

                            <div class="remember-section">
                                <asp:CheckBox ID="RememberMe" runat="server" />
                                <asp:Label ID="RememberMeLabel" runat="server"
                                    AssociatedControlID="RememberMe">Keep me signed in</asp:Label>
                            </div>

                            <asp:Button
                                ID="LoginButton"
                                runat="server"
                                Text="Sign in"
                                CssClass="login-btn"
                                ValidationGroup="LoginUserValidationGroup"
                                OnClientClick="return beginSignIn(this);"
                                OnClick="LoginButton_Click" />

                        </div>

                    </LayoutTemplate>

                </asp:Login>
            </main>

        </div>
    </form>

    <script type="text/javascript">
        (function () {
            function byId(suffix) { return document.querySelector('[id$="_' + suffix + '"]'); }

            // Show / hide password
            window.togglePassword = function (btn) {
                var input = byId('Password');
                var show = input.type === 'password';
                input.type = show ? 'text' : 'password';
                btn.textContent = show ? 'Hide' : 'Show';
                btn.setAttribute('aria-pressed', show);
                btn.setAttribute('aria-label', show ? 'Hide password' : 'Show password');
                input.focus();
            };

            // Validate, then show a busy state so the person knows something is happening
            window.beginSignIn = function (btn) {
                if (typeof Page_ClientValidate === 'function' && !Page_ClientValidate('LoginUserValidationGroup')) {
                    return false;
                }
                btn.className += ' is-busy';
                btn.value = 'Signing in\u2026';
                return true;
            };

            document.addEventListener('DOMContentLoaded', function () {
                var user = byId('UserName');
                var pass = byId('Password');
                var branch = byId('ASPxComboBox1');
                var caps = document.getElementById('capsWarning');

                // Caps Lock hint
                if (pass && caps) {
                    var check = function (e) {
                        caps.style.display = (e.getModifierState && e.getModifierState('CapsLock')) ? 'block' : 'none';
                    };
                    pass.addEventListener('keydown', check);
                    pass.addEventListener('keyup', check);
                    pass.addEventListener('blur', function () { caps.style.display = 'none'; });
                }

                // Red border on fields whose validator is showing
                var fields = [['UserName', 'UserNameRequired'], ['Password', 'PasswordRequired'], ['ASPxComboBox1', 'BranchRequired']];
                function paint() {
                    fields.forEach(function (f) {
                        var input = byId(f[0]), msg = byId(f[1]);
                        if (!input || !msg) { return; }
                        var bad = msg.style.display !== 'none' && msg.style.visibility !== 'hidden' && msg.offsetHeight > 0;
                        input.className = input.className.replace(' has-error', '') + (bad ? ' has-error' : '');
                    });
                }
                document.addEventListener('click', function () { setTimeout(paint, 0); });
                document.addEventListener('input', function () { setTimeout(paint, 0); });
                document.addEventListener('change', function () { setTimeout(paint, 0); });

                // After the username postback (which loads branches) or a failed sign-in,
                // put the cursor where the person needs to type next.
                if (user && pass) {
                    if (!user.value) { user.focus(); }
                    else if (!pass.value) { pass.focus(); }
                }
            });
        })();
    </script>
</body>
</html>
