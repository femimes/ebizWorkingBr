<%@ Page Title="Log In" Language="C#" AutoEventWireup="true" CodeFile="Auth.aspx.cs" Inherits="Account_Auth" MasterPageFile="~/Site.master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .login-card .form-control, .login-card select.form-control { margin: 0; }
        .login-card .field-error { display: block; margin-top: 5px; font-size: 12px; color: var(--bad); }
        .login-card .remember-section { display: flex; align-items: center; gap: 8px; }
        .login-card .remember-section label { font-weight: 500; font-size: 13px; color: var(--muted); }
        .login-card .failureNotification:empty { display: none; }
        .login-card .failureNotification { margin-bottom: 14px; padding: 10px 12px; border-radius: 8px; background: var(--bad-bg); text-align: left; }
        .login-card .hint { display: block; margin-top: 5px; font-size: 12px; color: var(--muted); }
    </style>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<div class="login-container">
    <asp:Login ID="LoginUser" runat="server" RenderOuterTable="false">
        <LayoutTemplate>
            <div class="login-card">
                <div class="logo-section">
                    <asp:Image ID="CompanyLogo" runat="server" ImageUrl="~/Images/imageswywyw.jpg" AlternateText="Company logo" />
                </div>

                <h1 class="page-title">Welcome back</h1>
                <p class="page-subtitle">Sign in to continue</p>

                <span class="failureNotification" role="alert"><asp:Literal ID="FailureText" runat="server" /></span>
                <asp:ValidationSummary ID="LoginUserValidationSummary" runat="server" CssClass="failureNotification" ValidationGroup="LoginUserValidationGroup" DisplayMode="BulletList" />

                <div class="form-group">
                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" CssClass="form-label">Username</asp:Label>
                    <asp:TextBox ID="UserName" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="UserName_TextChanged"
                        autocomplete="username" placeholder="Your username" />
                    <span class="hint">Your branches load after you enter your username.</span>
                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" CssClass="field-error" Display="None"
                        ErrorMessage="Enter your username." ValidationGroup="LoginUserValidationGroup" />
                </div>

                <div class="form-group">
                    <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" CssClass="form-label">Password</asp:Label>
                    <asp:TextBox ID="Password" runat="server" TextMode="Password" CssClass="form-control" autocomplete="current-password" placeholder="Your password" />
                </div>

                <div class="form-group">
                    <asp:Label ID="LocationLabel" runat="server" AssociatedControlID="drpLocation" CssClass="form-label">Branch / location</asp:Label>
                    <asp:DropDownList ID="drpLocation" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="LocationRequired" runat="server" ControlToValidate="drpLocation" CssClass="field-error" Display="None"
                        ErrorMessage="Please select a branch." ValidationGroup="LoginUserValidationGroup" />
                </div>

                <div class="remember-section">
                    <asp:CheckBox ID="RememberMe" runat="server" />
                    <asp:Label ID="RememberMeLabel" runat="server" AssociatedControlID="RememberMe">Keep me signed in</asp:Label>
                </div>

                <asp:Button ID="LoginButton" runat="server" Text="Sign in" CommandName="Login" CssClass="login-btn"
                    ValidationGroup="LoginUserValidationGroup" OnClick="LoginButton_Click" />
            </div>
        </LayoutTemplate>
    </asp:Login>
</div>
</asp:Content>
