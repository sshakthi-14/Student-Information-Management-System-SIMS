<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CourseNotes.aspx.cs" Inherits="SoftwareEngineeringProject.CourseNotes" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Course Notes</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="EnterMarks.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">

        <div class="marks-header">
            <span class="course-header">Course Notes : </span>
            <asp:Label ID="lblCourseName" runat="server" CssClass="course-header" />
        </div>

        <div class="marks-menu">
            <asp:Button ID="btnBackDashboard" runat="server" Text="← Dashboard" PostBackUrl="~/LecturerDashboard.aspx" CssClass="menu-btn" />
            <asp:Button ID="btnAttendance" runat="server" Text="Attendance" PostBackUrl="~/ManageAttendance.aspx" CssClass="menu-btn" />
            <asp:Button ID="btnAttendanceReport" runat="server" Text="Attendance Report" PostBackUrl="~/AttendanceReport.aspx" CssClass="menu-btn" />
            <asp:Button ID="btnMarks" runat="server" Text="Enter Marks" PostBackUrl="~/EnterMarks.aspx" CssClass="menu-btn" />
            <asp:Button ID="btnMarksReport" runat="server" Text="Marks Report" PostBackUrl="~/MarksReport.aspx" CssClass="menu-btn" />
            <asp:Button ID="btnNotes" runat="server" Text="Manage Course Notes" CssClass="menu-btn active-btn" />
        </div>

        <div class="container mt-4" style="width: 95%; margin: auto;">
            
            <div class="card mb-4">
                <div class="card-header bg-light fw-bold">Upload New Notes</div>
                <div class="card-body">
                    <div class="row align-items-center g-3">
                        <div class="col-md-4">
                            <asp:Label ID="Label1" runat="server" Text="Select Week" CssClass="form-label fw-semibold" />
                            <asp:DropDownList ID="ddlWeek" runat="server" CssClass="form-select">
                                <asp:ListItem>Week 1</asp:ListItem>
                                <asp:ListItem>Week 2</asp:ListItem>
                                <asp:ListItem>Week 3</asp:ListItem>
                                <asp:ListItem>Week 4</asp:ListItem>
                                <asp:ListItem>Week 5</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-5">
                            <asp:Label ID="Label2" runat="server" Text="Choose File" CssClass="form-label fw-semibold" />
                            <asp:FileUpload ID="FileUpload1" runat="server" CssClass="form-control" />
                        </div>
                        <div class="col-md-3 align-self-end">
                            <asp:Button ID="btnUpload" runat="server" Text="Upload Notes" OnClick="btnUpload_Click" CssClass="btn btn-warning w-100 fw-bold" style="background-color: #EDC36F; border: none;" />
                        </div>
                    </div>
                </div>
            </div>

            <asp:Label ID="lblMessage" runat="server" CssClass="fw-bold d-block mb-3" />

            <div class="marks-table-wrapper">
                <asp:GridView ID="gvNotes" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped">
                    <Columns>
                        <asp:BoundField DataField="WeekNo" HeaderText="Week" />
                        <asp:BoundField DataField="FileName" HeaderText="File Name" />
                        <asp:BoundField DataField="UploadDate" HeaderText="Upload Date" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                        <asp:HyperLinkField HeaderText="Action" Text="Download" DataNavigateUrlFields="FilePath" ControlStyle-CssClass="btn btn-sm btn-outline-primary" />
                    </Columns>
                </asp:GridView>
            </div>

        </div>
    </form>
</body>
</html>
