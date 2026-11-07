<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StudentDashboard.aspx.cs" Inherits="SoftwareEngineeringProject.StudentDashboard" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Student Dashboard</title>

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>

<body>

<form runat="server">

<div class="container mt-4">

    <h2>Student Dashboard</h2>

    <div class="d-flex justify-content-between align-items-center mb-3">
        <div>
            <a class="btn btn-primary me-2" href="RegisterCourses.aspx">Register Courses</a>
            <a class="btn btn-info text-white me-2" href="Results.aspx">Results</a>
            <a class="btn btn-secondary me-2" href="Notifications.aspx">Notifications</a>
            <a class="btn btn-warning text-dark" href="Attendance.aspx">Attendance</a>

            <a class="btn btn-success me-2" href="PerformanceReport.aspx">📈 Performance Report</a>
            <a class="btn btn-dark" href="ViewAcademicHistory.aspx">📜 Academic History</a>
            <a class="btn btn-outline-secondary" href="tNotification.aspx">🔔 tNotifications</a>
        </div>

        <asp:Button ID="btnLogout" runat="server" Text="Logout"
        CssClass="btn btn-danger float-end"
            OnClick="btnLogout_Click" />
    </div>

    <hr />

    <!-- PROFILE -->
    <h4>Profile</h4>
    <asp:Label ID="lblName" runat="server" />
    <br />
    <asp:Label ID="lblEmail" runat="server" />

    <hr />

    <!-- ENROLLED COURSES -->
    <h4>Enrolled Courses</h4>
    <asp:GridView ID="gvCourses" runat="server" CssClass="table table-bordered" />

</div>

</form>
</body>
</html>
