<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AttendanceReport.aspx.cs" Inherits="SoftwareEngineeringProject.AttendanceReport" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>Attendance Report</title>

        <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />

        <link href="AttendanceReport.css" rel="stylesheet" />

    </head>

    <body>
        <form id="form1" runat="server">
            <div class="attendance-header">
                <asp:Label ID="lblCourse" runat="server" CssClass="course-header"></asp:Label>
                <div class="report-title-sub">Attendance Summary Report</div>
            </div>

            <div class="attendance-menu">
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
                    CssClass="menu-btn active-btn" />

                <asp:Button ID="btnMarks" 
                    runat="server" 
                    Text="Enter Marks" 
                    PostBackUrl="~/EnterMarks.aspx" 
                    CssClass="menu-btn" />

                <asp:Button ID="btnMarksReport" 
                    runat="server" 
                    Text="Marks Report" 
                    PostBackUrl="~/MarksReport.aspx" 
                    CssClass="menu-btn" />

            </div>

            <br />

            <div class="attendance-container">

                <div class="filter-section d-flex align-items-center gap-2 mb-4">

                    <div style="width: 250px;">
                        <asp:TextBox ID="txtDate" 
                            runat="server" 
                            TextMode="Date" 
                            CssClass="form-control">

                        </asp:TextBox>

                    </div>
                    <asp:Button ID="btnSearch" 
                        runat="server" 
                        Text="Filter Records" 
                        OnClick="btnSearch_Click" 
                        CssClass="btn btn-secondary btn-filter" />

                    <asp:Button ID="btnPDF" 
                        runat="server" 
                        Text="Export Summary PDF" 
                        OnClick="btnPDF_Click" 
                        CssClass="btn btn-danger ms-auto" 
                        Visible="false" />

                </div>

                <br />

                <asp:GridView ID="gvAttendance" 
                    runat="server" 
                    AutoGenerateColumns="False" 
                    OnRowDataBound="gvAttendance_RowDataBound" 
                    CssClass="table table-bordered report-grid">

                    <Columns>
                        <asp:BoundField DataField="Student ID" 
                            HeaderText="Student ID" 
                            HeaderStyle-CssClass="grid-header" />

                        <asp:BoundField DataField="Student Name" 
                            HeaderText="Student Name" 
                            HeaderStyle-CssClass="grid-header" />

                        <asp:BoundField DataField="Email" 
                            HeaderText="Email" 
                            HeaderStyle-CssClass="grid-header" />
        
                        <asp:BoundField DataField="Status on Date" 
                            HeaderText="Status on Date" 
                            HeaderStyle-CssClass="grid-header" />
        
                        <asp:BoundField DataField="Total Absences" 
                            HeaderText="Total Absences" 
                            HeaderStyle-CssClass="grid-header" />
    
                        <asp:TemplateField HeaderText="Actions" HeaderStyle-CssClass="grid-header">
                            <ItemTemplate>
                                <asp:Button ID="btnWarning" 
                                    runat="server" 
                                    Text="Warning" 
                                    CssClass="btn btn-warning btn-sm table-warning-btn" 
                                    OnClick="btnWarning_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                <asp:Label ID="lblNoStudents" 
                    runat="server" 
                    ForeColor="Red" 
                    Font-Bold="true">
                </asp:Label>

            </div>
        </form>
    </body>
</html>
