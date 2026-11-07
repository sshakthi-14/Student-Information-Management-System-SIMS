<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LecturerDashboard.aspx.cs" Inherits="SoftwareEngineeringProject.LecturerDashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Lecturer Dashboard</title>

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="LecturerDashboard.css" rel="stylesheet" />

</head>
<body>
    <form id="form1" runat="server">
        <div class="topbar">
            <div class="topbar-title">
                Lecturer Dashboard
            </div>

            <asp:Button ID="btnLogout"
                runat="server"
                Text="Logout"
                CssClass="logout-btn"
                OnClick="btnLogout_Click" />
        </div>

        <div class="dashboard-container">

            <div class="sidebar">

                <div class="sidebar-header">
                    MENU
                </div>

                <div class="selected-course">

                    <div class="course-label">
                        Current Course
                    </div>

                    <asp:Label ID="lblSelectedCourse"
                        runat="server">
                    </asp:Label>

                </div>

                <asp:HyperLink ID="lnkAttendance"
                    runat="server"
                    CssClass="menu-link">
                    Attendance
                </asp:HyperLink>

                <asp:HyperLink ID="lnkAttendanceReport"
                    runat="server"
                    CssClass="menu-link">
                    Attendance Report
                </asp:HyperLink>

                <asp:HyperLink ID="lnkEnterMarks"
                    runat="server"
                    CssClass="menu-link">
                    Enter Marks
                </asp:HyperLink>

                <asp:HyperLink ID="lnkMarksReport"
                    runat="server"
                    CssClass="menu-link">
                    Marks Report
                </asp:HyperLink>
            <asp:HyperLink ID="lnkCourseNotes"
                    runat="server"
                    NavigateUrl="~/CourseNotes.aspx" 
                    CssClass="menu-link">
                    Manage Course Notes
                </asp:HyperLink>
                
                <asp:HyperLink ID="lnkAnnouncements"
                    runat="server"
                    NavigateUrl="~/LecturerAnnouncement.aspx" 
                    CssClass="menu-link">
                    Manage Announcements
                </asp:HyperLink>
                
                <asp:HyperLink ID="lnkViewAnnouncements"
                    runat="server"
                    NavigateUrl="~/ViewAnnouncements.aspx" 
                    CssClass="menu-link">
                    View Announcements
            </asp:HyperLink>

            </div>

            <div class="content-area">

                    <asp:Panel ID="pnlCourses" runat="server">

                        <h1>Welcome to the Lecturer Dashboard</h1>

                        <div class="course-container">

                            <asp:Repeater ID="rptCourses"
                                runat="server"
                                OnItemCommand="rptCourses_ItemCommand">

                                <ItemTemplate>

                                    <asp:LinkButton ID="btnCourse"
                                        runat="server"
                                        CssClass="course-card"
                                        CommandName="SelectCourse"
                                        CommandArgument='<%# Eval("CourseID") %>'>

                                        <h4><%# Eval("CourseCode") %></h4>

                                        <p><%# Eval("CourseName") %></p>

                                    </asp:LinkButton>

                                </ItemTemplate>

                            </asp:Repeater>

                        </div>

                    </asp:Panel>

                    <asp:Panel ID="pnlStudents"
                        runat="server"
                        Visible="false">

                        <asp:Button ID="btnBack"
                            runat="server"
                            Text="← Back"
                            CssClass="btn btn-secondary"
                            OnClick="btnBack_Click" />

                        <br />
                        <br />

                        <div class="course-title-box">
                            <asp:Label ID="lblCourseCode" runat="server" />
                            <br />
                            <asp:Label ID="lblCourseName" runat="server" />
                        </div>

                        <br />
                        <br />

                        <asp:GridView ID="gvStudents"
                            runat="server"
                            CssClass="table table-striped table-bordered">
                        </asp:GridView>

                        <asp:Label ID="lblNoStudents" runat="server" ForeColor="Red"/>

                    </asp:Panel>

            </div>

            <div>

                <div class="profile-box">

                    <h3 class="profile-title">
                        Account Profile
                    </h3>

                    <div class="mb-3">
                        <span class="profile-label">Username</span>
                        <asp:TextBox ID="txtUsername"
                            runat="server"
                            CssClass="form-control"
                            ReadOnly="true">
                        </asp:TextBox>
                    </div>

                    <div class="mb-3">
                        <span class="profile-label">Email</span>
                        <asp:TextBox ID="txtEmail"
                            runat="server"
                            CssClass="form-control"
                            ReadOnly="true">
                        </asp:TextBox>
                    </div>

                    <div class="mb-3">
                        <span class="profile-label">Password</span>
                        <asp:TextBox ID="txtPassword"
                            runat="server"
                            CssClass="form-control"
                            TextMode="Password"
                            ReadOnly="true">
                        </asp:TextBox>
                    </div>

                    <div class="profile-buttons">

                        <asp:Button ID="btnEdit"
                            runat="server"
                            Text="Edit"
                            CssClass="btn btn-primary"
                            OnClick="btnEdit_Click" />

                        <asp:Button ID="btnConfirm"
                            runat="server"
                            Text="Confirm"
                            CssClass="btn btn-success"
                            Visible="false"
                            OnClick="btnConfirm_Click" />

                        <asp:Button ID="btnCancel"
                            runat="server"
                            Text="Cancel"
                            CssClass="btn btn-danger"
                            Visible="false"
                            OnClick="btnCancel_Click" />

                    </div>

                </div>
            
            </div>
        </div>
    </form>
</body>
</html>
