<%@ Page Title="Log In" Language="C#" AutoEventWireup="true" CodeFile="Auth.aspx.cs" Inherits="Account_Auth" MasterPageFile="~/Site.master"  %>
<%@ Register assembly="DevExpress.Web.v19.1, Version=19.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">



</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

<div class="login-container">

    <asp:Login ID="LoginUser" runat="server"
        EnableViewState="false"
        RenderOuterTable="false">

        <LayoutTemplate>

            <div class="login-card">

                <div class="logo-section">

                    <asp:Image ID="CompanyLogo"
                        runat="server"
                        ImageUrl="~/Images/imageswywyw.jpg" />

                </div>

                <div class="page-title">
                    Welcome Back
                </div>

                <div class="page-subtitle">
                    Sign in to continue
                </div>

                <span class="failureNotification">
                    <asp:Literal ID="FailureText" runat="server" />
                </span>

                <asp:ValidationSummary
                    ID="LoginUserValidationSummary"
                    runat="server"
                    CssClass="failureNotification"
                    ValidationGroup="LoginUserValidationGroup" />

                <div class="form-group">
                    <label class="form-label">
                        Username feqaass
                    </label>
                    <dx:ASPxTextBox ID="Nmae" AutoPostBack="true" runat="server"></dx:ASPxTextBox>
                    <asp:TextBox
                        ID="UserName"
                        runat="server" AutoPostBack="true"     ontextchanged="UserName_TextChanged"
                         />
                </div>

                <div class="form-group">
                    <label class="form-label">
                        Password
                    </label>

                    <asp:TextBox
                        ID="Password"
                        runat="server"
                        TextMode="Password"
                        CssClass="form-control" />
                </div>

                <div class="form-group">
                    <label class="form-label">
                        Branch / Location
                    </label>



                    <dx:ASPxComboBox
                        ID="ASPxComboBox1"
                        runat="server"
                        Width="100%">

                        <ValidationSettings>
                            <RequiredField
                                IsRequired="True"
                                ErrorText="Please select a branch" />
                        </ValidationSettings>

                    </dx:ASPxComboBox>
                </div>

                <div class="remember-section">

                    <asp:CheckBox
                        ID="RememberMe"
                        runat="server" />

                    <asp:Label
                        ID="RememberMeLabel"
                        runat="server"
                        AssociatedControlID="RememberMe">
                        Keep me signed in
                    </asp:Label>

                </div>

                <asp:Button
                    ID="LoginButton"
                    runat="server"
                    Text="Sign In"
                    CommandName="Login"
                    CssClass="login-btn"
                    ValidationGroup="LoginUserValidationGroup"
                    OnClick="LoginButton_Click" />



            </div>

        </LayoutTemplate>

    </asp:Login>

</div>

</asp:Content>