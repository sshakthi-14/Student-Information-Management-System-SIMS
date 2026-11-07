<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="tNotification.aspx.cs" Inherits="SoftwareEngineeringProject.Notifications" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Notifications</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form runat="server">
        
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
                <h2>Notifications</h2>
                <asp:Button ID="btnBack" runat="server" Text="← Back" CssClass="btn btn-secondary" OnClick="btnBack_Click" />
            </div>

            <hr />

            <asp:Label ID="lblMessage" runat="server" CssClass="alert alert-warning d-block fw-bold mb-3" Visible="false"></asp:Label>

            <asp:Repeater ID="rptNotifications" runat="server">
                <ItemTemplate>
                    <div class='card mb-3 shadow-sm border-start border-5 <%# Eval("AlertType").ToString() == "Attendance Warning" ? "border-danger" : "border-primary" %>'>
                        <div class="card-body">
                            <div class="d-flex justify-content-between align-items-center mb-2">
                                <h5 class="card-title fw-bold mb-0 text-dark"><%# Eval("Title") %></h5>
                                <span class="text-muted small"><%# Convert.ToDateTime(Eval("CreatedDate")).ToString("yyyy-MM-dd HH:mm") %></span>
                            </div>
                            <p class="card-text text-secondary mb-0"><%# Eval("AlertMessage") %></p>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

        </div>
    </form>
</body>
</html>
