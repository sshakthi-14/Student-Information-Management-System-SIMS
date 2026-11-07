<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StudentLogin.aspx.cs" Inherits="SoftwareEngineeringProject.StudentLogin" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Student Login</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>

<body class="bg-light">

<form id="form1" runat="server">

<div class="container">
    <div class="row justify-content-center vh-100 align-items-center">

        <div class="col-md-4">
            <div class="card p-4 shadow">

                <h3 class="text-center">Student Login</h3>

                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control mt-3" placeholder="Email" />
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control mt-3" placeholder="Password" />

                <asp:Button ID="btnLogin" runat="server" Text="Login"
                    CssClass="btn btn-primary w-100 mt-3"
                    OnClick="btnLogin_Click" />

                <asp:Label ID="lblMessage" runat="server" ForeColor="Red" />

            </div>
        </div>

    </div>
</div>

</form>
</body>
</html>
