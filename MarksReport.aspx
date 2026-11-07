<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MarksReport.aspx.cs" Inherits="SoftwareEngineeringProject.MarksReport" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>Marks Report</title>

        <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />

        <link href="MarksReport.css" rel="stylesheet" />
    </head>

    <body>
        <form id="form1" runat="server">
            <div class="marks-header">
                <asp:Label ID="lblCourse" 
                    runat="server" 
                    CssClass="course-header">

                </asp:Label>

                <div class="report-title-sub">Academic Performance Report (Result Slip)</div>
            </div>

            <div class="marks-menu">
                <asp:Button ID="btnBackDashboard" 
                    runat="server" 
                    Text="← Dashboard" 
                    PostBackUrl="~/LecturerDashboard.aspx" 
                    CssClass="menu-btn" />

                <asp:Button ID="btnAttendance" 
                    runat="server" 
                    Text="Attendance" 
                    PostBackUrl="~/ManageAttendance.aspx" 
                    CssClass="menu-btn" />

                <asp:Button ID="btnAttendanceReport" 
                    runat="server" 
                    Text="Attendance Report" 
                    PostBackUrl="~/AttendanceReport.aspx" 
                    CssClass="menu-btn" />

                <asp:Button ID="btnMarks" 
                    runat="server" 
                    Text="Enter Marks" 
                    PostBackUrl="~/EnterMarks.aspx" 
                    CssClass="menu-btn" />

                <asp:Button ID="btnMarksReport" 
                    runat="server" 
                    Text="Marks Report" 
                    CssClass="menu-btn active-btn" />

            </div>

            <br />

            <div class="marks-container">

                <div class="filter-section d-flex align-items-center gap-2 mb-4">
                    <div style="width: 280px;">
                        <asp:DropDownList ID="ddlStudent" 
                            runat="server" 
                            AutoPostBack="true" 
                            OnSelectedIndexChanged="ddlStudent_SelectedIndexChanged" 
                            CssClass="form-select">
                        </asp:DropDownList>
                    </div>

                    <asp:Button ID="btnPDF" 
                        runat="server" 
                        Text="Export Result Slip (PDF)" 
                        OnClick="btnPDF_Click" 
                        CssClass="btn btn-danger ms-auto" 
                        Visible="false" />

                </div>

                <asp:Panel ID="pnlResultSlip" runat="server" Visible="false">
                
                    <div class="student-meta-card mb-4">
                        <div class="row">
                            <div class="col-md-6 mb-2">
                                <strong>Student Name:</strong> 
                                <asp:Label ID="lblStudentName" runat="server" CssClass="ms-2" />
                            </div>
                            <div class="col-md-6 mb-2 text-md-end">
                                <strong>Programme:</strong> 
                                <asp:Label ID="lblProgramme" runat="server" CssClass="ms-2" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <strong>Student ID:</strong> 
                                <asp:Label ID="lblStudentID" runat="server" CssClass="ms-2" />
                            </div>
                            <div class="col-md-6 text-md-end">
                                <strong>Session/Semester:</strong> 
                                <asp:Label ID="lblSemester" runat="server" CssClass="ms-2" />
                            </div>
                        </div>
                    </div>

                    <asp:Panel ID="pnlProbation" 
                        runat="server" 
                        CssClass="alert alert-danger font-weight-bold text-center mb-4" 
                        Visible="false">
                        ⚠️ ACADEMIC PROBATION ALERT (CGPA IS BELOW 2.00)
                    </asp:Panel>

                    <asp:GridView ID="gvMarks" 
                        runat="server" 
                        AutoGenerateColumns="False" 
                        CssClass="table table-bordered report-grid">

                        <Columns>
                            <asp:BoundField DataField="CODE" HeaderText="CODE" HeaderStyle-CssClass="grid-header" />
                            <asp:BoundField DataField="COURSE DESCRIPTION" HeaderText="COURSE DESCRIPTION" HeaderStyle-CssClass="grid-header" />
                            <asp:BoundField DataField="Cr. HOURS" HeaderText="Cr. HOURS" HeaderStyle-CssClass="grid-header" />
                            <asp:BoundField DataField="GRADE" HeaderText="GRADE" HeaderStyle-CssClass="grid-header" />
                            <asp:BoundField DataField="Cr. PT." HeaderText="Cr. PT." DataFormatString="{0:F2}" HeaderStyle-CssClass="grid-header" />
                        </Columns>
                    </asp:GridView>

                    <div class="row mt-4 mb-5">
                        <div class="col-md-6 ms-auto summary-box">
                            <table class="table table-sm table-borderless m-0">
                                <tr>
                                    <td><strong>CREDIT HOURS EARNED:</strong></td>
                                    <td class="text-end"><asp:Label ID="lblCrHoursEarned" runat="server" /></td>
                                </tr>
                                <tr>
                                    <td><strong>GRADE POINT AVERAGE (GPA):</strong></td>
                                    <td class="text-end"><asp:Label ID="lblGPA" runat="server" font-bold="true" /></td>
                                </tr>
                                <tr>
                                    <td><strong>CUMULATIVE CREDIT HOURS EARNED:</strong></td>
                                    <td class="text-end"><asp:Label ID="lblCumCrHours" runat="server" /></td>
                                </tr>
                                <tr class="border-top">
                                    <td><strong class="text-primary">CUMULATIVE GPA (CGPA):</strong></td>
                                    <td class="text-end"><asp:Label ID="lblCGPA" runat="server" font-bold="true" CssClass="text-primary" /></td>
                                </tr>
                            </table>
                        </div>
                    </div>

                </asp:Panel>

                <div class="text-center">
                    <asp:Label ID="lblNoRecords" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>

            </div>
        </form>
    </body>
</html>
