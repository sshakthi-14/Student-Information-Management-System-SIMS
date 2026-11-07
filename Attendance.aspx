<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Attendance.aspx.cs" Inherits="SoftwareEngineeringProject.Attendance" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Attendance</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>

<body>
<form runat="server">

<div class="container mt-4">

    <h2>My Attendance</h2>

    <asp:Button ID="btnBack" runat="server" Text="← Back"
        CssClass="btn btn-secondary mb-3"
        OnClick="btnBack_Click" />

    <asp:GridView ID="gvAttendance" runat="server"
        CssClass="table table-bordered" />

</div>

</form>
</body>
</html>
