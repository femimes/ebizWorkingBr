<%@ Page  Title="Log In"  Language="C#" AutoEventWireup="true" CodeFile="AuthLogin.aspx.cs" Inherits="Account_AuthLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <style type="text/css">

.login-container {
    min-height: 600px;
    display: flex;
    justify-content: center;
    align-items: center;
    /*background:linear-gradient(135deg,#0F4C81,#1A73E8);*/
    background:linear-gradient(99deg, #54ea921a, #e81ae000);
    background-color:whitesmoke;
    padding: 40px;
}

 

.login-card {
    width: 420px;
    background: #ffffff;
    border-radius: 12px;
    box-shadow: 0 10px 30px rgba(0,0,0,0.15);

    padding: 40px;
}

.logo-section {
    text-align: center;
    margin-bottom: 30px;
}

.logo-section img {
    width: 180px;
}

.page-title {
    text-align: center;
    font-size: 28px;
    font-weight: 600;
    /*color: #333;*/
    margin-bottom: 5px;
}

.page-subtitle {
    text-align: center;
    color: #777;
    margin-bottom: 30px;
}

.form-group {
    margin-bottom: 18px;
}

.form-label {
    font-weight: 600;
    display: block;
    margin-bottom: 6px;
    color: #444;
}

.form-control {
    width: 100%;
    border-radius: 6px;
}

.login-btn {
    width: 100%;
    height: 45px;
    background: #0F4C81;
    color: white;
    border: none;
    border-radius: 6px;
    font-size: 16px;
    font-weight: 600;
}

.login-btn:hover {
    background: #08365a;
}

.failureNotification {
    color: #d9534f;
    display: block;
    text-align: center;
    margin-bottom: 15px;
}

.remember-section {
    margin-top: 10px;
    margin-bottom: 20px;
}

</style>
</head>
<body>
    <form id="form1" runat="server">
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



              <%--      <dx:ASPxComboBox
                        ID="ASPxComboBox1"
                        runat="server"
                        Width="100%">

                        <ValidationSettings>
                            <RequiredField
                                IsRequired="True"
                                ErrorText="Please select a branch" />
                        </ValidationSettings>

                    </dx:ASPxComboBox>--%>
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
    </form>
</body>
</html>
