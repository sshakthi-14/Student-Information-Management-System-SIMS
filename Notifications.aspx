<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Notifications.aspx.cs" Inherits="SoftwareEngineeringProject.Notifications" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Notifications</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>

<body>
<form runat="server">

<div class="container mt-4">

    <h2>Notifications</h2>

    <asp:Button ID="btnBack" runat="server" Text="← Back"
        CssClass="btn btn-secondary mb-3"
        OnClick="btnBack_Click" />

    <asp:GridView ID="gvNotifications" runat="server"
        CssClass="table table-bordered" />

</div>

</form>
</body>
</html>
