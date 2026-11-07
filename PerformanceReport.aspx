<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PerformanceReport.aspx.cs" Inherits="SoftwareEngineeringProject.PerformanceReport" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Performance Report</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        .report-header { margin-bottom: 30px; border-bottom: 2px solid #333; padding-bottom: 10px; }
        .summary-section { margin-top: 30px; font-weight: bold; }
    </style>
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
            <asp:Button ID="btnPerformance" runat="server" Text="📈 Performance Report" PostBackUrl="~/Student/PerformanceReport.aspx" CssClass="btn btn-info text-white fw-bold" />
            <asp:Button ID="btnHistory" runat="server" Text="📜 Academic History" PostBackUrl="~/Student/ViewAcademicHistory.aspx" CssClass="btn btn-outline-secondary" />
            <asp:Button ID="btnAlerts" runat="server" Text="🔔 Notifications" PostBackUrl="~/Student/tNotification.aspx" CssClass="btn btn-outline-secondary" />
        </div>

        <div class="container mt-4">
            
            <div class="report-header">
                <h3>Academic Performance Report</h3>
                <div class="row mt-4">
                    <div class="col-md-6">
                        <p><strong>Name:</strong> <asp:Label ID="lblName" runat="server" /></p>
                    </div>
                    <div class="col-md-6 text-md-end">
                        <p><strong>Programme:</strong> <asp:Label ID="lblProgramme" runat="server" /></p>
                        <p><strong>Session/Semester:</strong> <asp:Label ID="lblSession" runat="server" /></p>
                    </div>
                </div>
            </div>

            <div class="card shadow-sm mb-4">
                <div class="card-body p-0">
                    <table class="table table-striped table-bordered mb-0">
                        <thead class="table-dark">
                            <tr>
                                <th>CODE</th>
                                <th>COURSE DESCRIPTION</th>
                                <th class="text-center">Cr. HOURS</th>
                                <th class="text-center">GRADE</th>
                                <th class="text-end">Cr. PT.</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptCourses" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td><strong><%# Eval("CourseCode") %></strong></td>
                                        <td><%# Eval("CourseName") %></td>
                                        <td class="text-center"><%# Eval("CreditHours") %></td>
                                        <td class="text-center"><span class='badge <%# Eval("CalculatedGrade").ToString() == "F" ? "bg-danger" : "bg-success" %>'><%# Eval("CalculatedGrade") %></span></td>
                                        <td class="text-end"><%# Convert.ToDecimal(Eval("CalculatedCrPt")).ToString("F2") %></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
            </div>

            <hr style="border-top: 2px solid #333;" />

            <div class="card bg-light shadow-sm p-4 summary-section" style="max-width: 600px;">
                <h5 class="mb-3 text-secondary">Academic Status Result</h5>
                <p class="mb-3 fs-5"><asp:Label ID="lblAcademicStanding" runat="server" /></p>
                
                <p class="mb-1 text-dark">CREDIT HOURS EARNED = <asp:Label ID="lblTermCredits" runat="server" Text="0" /></p>
                <p class="mb-0 text-dark">GRADE POINT AVERAGE (GPA) = <asp:Label ID="lblTermGPA" runat="server" Text="0.00" /></p>
            </div>

        </div>
    </form>
</body>
</html>
