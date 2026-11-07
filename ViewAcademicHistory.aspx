<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewAcademicHistory.aspx.cs" Inherits="SoftwareEngineeringProject.ViewAcademicHistory" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Academic History</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        
        <div class="bg-dark text-white p-3 d-flex justify-content-between align-items-center">
            <h4 class="m-0 fw-bold text-info">Student Hub Portal</h4>
            <div>
                <asp:Button ID="btnLogout" runat="server" Text="Logout" CssClass="btn btn-sm btn-outline-light" OnClick="btnLogout_Click" />
            </div>
        </div>

        <div class="d-flex gap-2 p-2 mb-4 bg-light border-bottom">
    <asp:Button ID="btnHome" runat="server" Text="🏠 Dashboard" PostBackUrl="~/Student/StudentDashboard.aspx" CssClass="btn btn-outline-secondary" />
    <asp:Button ID="btnPerformance" runat="server" Text="📈 Performance Report" PostBackUrl="~/Student/PerformanceReport.aspx" CssClass="btn btn-outline-secondary" />
    <asp:Button ID="btnHistory" runat="server" Text="📜 Academic History" PostBackUrl="~/Student/ViewAcademicHistory.aspx" CssClass="btn btn-info text-white fw-bold" />
    <asp:Button ID="btnAlerts" runat="server" Text="🔔 Notifications" PostBackUrl="~/Student/tNotification.aspx" CssClass="btn btn-outline-secondary" />
</div>

        <div class="container mt-4">
            
            <div class="d-flex justify-content-between align-items-center mb-3">
                <h2>My Academic History</h2>
            </div>

            <hr />

            <div class="card mb-4 shadow-sm" style="max-width: 600px;">
                <div class="card-header bg-light fw-bold">Student Identity Details</div>
                <div class="card-body">
                    <p class="mb-2"><strong>Student Name:</strong> <asp:Label ID="lblStudentName" runat="server" CssClass="ms-2" /></p>
                    <p class="mb-0"><strong>Programme:</strong> <asp:Label ID="lblProgrammeName" runat="server" CssClass="ms-2" /></p>
                </div>
            </div>

            <div class="card shadow-sm">
                <div class="card-header bg-light fw-bold">Completed & Active Modules</div>
                <div class="card-body p-0">
                    <asp:GridView ID="gvHistory" runat="server" AutoGenerateColumns="True" CssClass="table table-bordered table-striped mb-0" EmptyDataText="No academic transcript history found.">
                    </asp:GridView>
                </div>
            </div>

        </div>
    </form>
</body>
</html>
