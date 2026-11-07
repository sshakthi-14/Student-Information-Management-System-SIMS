<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LecturerAnnouncement.aspx.cs" Inherits="SoftwareEngineeringProject.LecturerAnnouncement" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Course Announcements</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="EnterMarks.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">

        <div class="marks-header">
            <span class="course-header">Announcements for: </span>
            <asp:Label ID="lblCourseName" runat="server" CssClass="course-header" Font-Bold="true"></asp:Label>
        </div>

        <div class="marks-menu">
            <asp:Button ID="btnBackDashboard" runat="server" Text="← Dashboard" PostBackUrl="~/LecturerDashboard.aspx" CssClass="menu-btn" />
            <asp:Button ID="btnAttendance" runat="server" Text="Attendance" PostBackUrl="~/ManageAttendance.aspx" CssClass="menu-btn" />
            <asp:Button ID="btnAttendanceReport" runat="server" Text="Attendance Report" PostBackUrl="~/AttendanceReport.aspx" CssClass="menu-btn" />
            <asp:Button ID="btnMarks" runat="server" Text="Enter Marks" PostBackUrl="~/EnterMarks.aspx" CssClass="menu-btn" />
            <asp:Button ID="btnMarksReport" runat="server" Text="Marks Report" PostBackUrl="~/MarksReport.aspx" CssClass="menu-btn" />
            <asp:Button ID="btnNotes" runat="server" Text="Manage Course Notes" PostBackUrl="~/CourseNotes.aspx" CssClass="menu-btn" />
            <asp:Button ID="btnAnnouncements" runat="server" Text="Manage Announcements" CssClass="menu-btn active-btn" />
        </div>

        <div class="container mt-4" style="width: 95%; margin: auto;">

            <div class="card mb-4 shadow-sm">
                <div class="card-header bg-light fw-bold fs-5">Post New Announcement</div>
                <div class="card-body">
                    
                    <div class="mb-3">
                        <asp:Label ID="lblTitle" runat="server" AssociatedControlID="txtTitle" Text="Title:" CssClass="form-label fw-semibold" />
                        <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" />
                    </div>

                    <div class="mb-3">
                        <asp:Label ID="lblContent" runat="server" AssociatedControlID="txtContent" Text="Message:" CssClass="form-label fw-semibold" />
                        <asp:TextBox ID="txtContent" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control" />
                    </div>

                    <asp:Button ID="btnPost" runat="server" Text="Post Announcement" OnClick="btnPost_Click" CssClass="btn btn-warning fw-bold px-4" style="background-color: #EDC36F; border: none; color: white;" />
                    
                    <div class="mt-3">
                        <asp:Label ID="lblStatus" runat="server" CssClass="fw-bold"></asp:Label>
                    </div>

                </div>
            </div>

            <div class="card shadow-sm">
                <div class="card-header bg-light fw-bold fs-5">Past Announcements</div>
                <div class="card-body p-0">
                    <div class="marks-table-wrapper m-0 w-100">
                        <asp:GridView ID="gvAnnouncements" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped mb-0">
                            <Columns>
                                <asp:BoundField DataField="Title" HeaderText="Title" ItemStyle-CssClass="fw-semibold" />
                                <asp:BoundField DataField="Content" HeaderText="Message" />
                                <asp:BoundField DataField="CreatedDate" HeaderText="Date Posted" DataFormatString="{0:yyyy-MM-dd HH:mm}" ItemStyle-Width="150px" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>

        </div>

    </form>
</body>
</html>
