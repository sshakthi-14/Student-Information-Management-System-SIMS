<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" 
         Inherits="Student_Information_Management_System_SIMS_.Login" %>

<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1.0" />
  <title>SIMS – Login | Help International University</title>

  <!-- Google Fonts -->
  <link rel="preconnect" href="https://fonts.googleapis.com" />
  <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin />
  <link href="https://fonts.googleapis.com/css2?family=Nunito:wght@400;500;600;700;800;900&family=Poppins:wght@400;500;600;700&display=swap" rel="stylesheet" />

  <!-- Font Awesome -->
  <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.1/css/all.min.css" />

  <!-- Project stylesheet -->
  <link rel="stylesheet" href="Styles/SIMS.css" />
</head>
<body>

  <form id="form1" runat="server">

    <!-- ================================================================
         LOGIN PAGE
    ================================================================ -->
    <div class="login-page">

      <div class="login-card">

        <!-- Logo -->
        <div class="login-logo">
          <img src="Images/logo.png" alt="Help International University Malaysia" 
               style="height: 55px; width: auto;" />
        </div>

        <!-- Heading -->
        <h1 class="login-title">Welcome Back</h1>
        <p class="login-subtitle">Sign in to your SIMS account</p>

        <!-- Alert panels -->
        <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-danger">
          <i class="fa-solid fa-circle-exclamation"></i>
          <asp:Label ID="lblAlert" runat="server" Text="" />
        </asp:Panel>

        <asp:Panel ID="pnlSuccess" runat="server" Visible="false" CssClass="alert alert-success">
          <i class="fa-solid fa-circle-check"></i>
          <asp:Label ID="lblSuccess" runat="server" Text="" />
        </asp:Panel>

        <!-- Email field -->
        <div class="form-group">
          <label class="form-label" for="txtEmail">Email Address</label>
          <div class="input-wrapper">
            <i class="fa-solid fa-envelope input-icon"></i>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"
                         placeholder="e.g. S125632@student.help.edu.my"
                         TextMode="Email" MaxLength="100" />
          </div>
          <asp:RequiredFieldValidator ID="rfvEmail" runat="server"
            ControlToValidate="txtEmail" ErrorMessage="Email address is required."
            CssClass="validation-message" Display="Dynamic" ValidationGroup="LoginGroup" />
        </div>

        <!-- Password field -->
        <div class="form-group">
          <label class="form-label" for="txtPassword">Password</label>
          <div class="input-wrapper">
            <i class="fa-solid fa-lock input-icon"></i>
            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control"
                         TextMode="Password" placeholder="Enter your password" MaxLength="255" />
            <button type="button" class="toggle-password" onclick="togglePassword()" title="Show / hide password">
              <i id="eyeIcon" class="fa-solid fa-eye"></i>
            </button>
          </div>
          <asp:RequiredFieldValidator ID="rfvPassword" runat="server"
            ControlToValidate="txtPassword" ErrorMessage="Password is required."
            CssClass="validation-message" Display="Dynamic" ValidationGroup="LoginGroup" />
        </div>

        <!-- Login button -->
        <asp:Button ID="btnLogin" runat="server" Text="Sign In"
                    CssClass="btn btn-primary" OnClick="btnLogin_Click"
                    ValidationGroup="LoginGroup" />

        <!-- Footer links -->
        <div class="login-links">
          <a href="ForgotPassword.aspx">
            <i class="fa-solid fa-key"></i> Forgot Password?
          </a>
          <a href="~/Contact.aspx" runat="server">
            <i class="fa-solid fa-headset"></i> Contact Us
          </a>
        </div>

      </div><!-- /login-card -->
    </div><!-- /login-page -->

  </form>

  <!-- ================================================================
       SCRIPTS
       ================================================================ -->
  <script>
      function togglePassword() {
          var box = document.getElementById('<%= txtPassword.ClientID %>');
      var icon = document.getElementById('eyeIcon');
      if (box.type === 'password') {
        box.type = 'text';
        icon.className = 'fa-solid fa-eye-slash';
      } else {
        box.type = 'password';
        icon.className = 'fa-solid fa-eye';
      }
    }

    document.addEventListener('keypress', function (e) {
      if (e.key === 'Enter') {
        var btn = document.getElementById('<%= btnLogin.ClientID %>');
            if (btn) btn.click();
        }
    });
  </script>

</body>
</html>