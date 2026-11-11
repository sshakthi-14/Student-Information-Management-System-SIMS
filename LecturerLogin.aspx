<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LecturerLogin.aspx.cs" Inherits="Student_Information_Management_System_SIMS.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Lecturer Login</title>

    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>

<body class="bg-light">

<form id="form1" runat="server">

<div class="container">
    <div class="row justify-content-center align-items-center vh-100">

        <div class="col-md-4">
            <div class="card shadow p-4">

                <h3 class="text-center mb-4">Lecturer Login</h3>

                <div class="mb-3">
                    <label>Email</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>
                </div>

                <div class="mb-3">
                    <label>Password</label>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                </div>

                <asp:Button ID="btnLogin" runat="server"
                    Text="Login"
                    CssClass="btn btn-primary w-100"
                    OnClick="btnLogin_Click" />

                <br />

                <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

            </div>
        </div>

    </div>
</div>

</form>
</body>
</html>
