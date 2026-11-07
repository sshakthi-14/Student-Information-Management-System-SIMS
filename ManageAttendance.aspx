<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageAttendance.aspx.cs" Inherits="SoftwareEngineeringProject.ManageAttendance" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Attendance</title>

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="ManageAttendance.css" rel="stylesheet" />

</head>
<body>
    <form id="form1" runat="server">
        <div class="attendance-header">

            <asp:Label ID="lblCourse"
                runat="server"
                CssClass="course-header">
            </asp:Label>

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
                CssClass="menu-btn active-btn" />

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
                PostBackUrl="~/MarksReport.aspx"
                CssClass="menu-btn" />

        </div>

        <br />

        <div class="attendance-container">

            <asp:TextBox ID="txtDate"
                runat="server"
                TextMode="Date"
                CssClass="form-control">
            </asp:TextBox>

            <asp:Label ID="lblDateError"
                runat="server"
                ForeColor="Red">
            </asp:Label>

            <br /> <br />

            <asp:Label ID="lblStudentCount"
                runat="server"
                CssClass="student-count">
            </asp:Label>

            <asp:GridView ID="gvStudents"
                runat="server"
                AutoGenerateColumns="False"
                DataKeyNames="EnrollmentID">

                <Columns>

                    <asp:BoundField
                        DataField="StudentID"
                        HeaderText="Student ID" />

                    <asp:BoundField
                        DataField="FullName"
                        HeaderText="Student Name" />

                    <asp:BoundField
                        DataField="Email"
                        HeaderText="Email" />

                    <asp:TemplateField
                        HeaderText="Attendance">

                        <ItemTemplate>

                            <asp:DropDownList ID="ddlStatus"
                                runat="server"
                                CssClass = "attendance-dropdown">

                                <asp:ListItem Text="Present" Value="Present" />
                                <asp:ListItem Text="Absent" Value="Absent" />
                                <asp:ListItem Text="Late" Value="Late" />

                            </asp:DropDownList>

                        </ItemTemplate>

                    </asp:TemplateField>

                </Columns>

            </asp:GridView>

            <br />

            <asp:Label ID="lblNoStudents"
                runat="server"
                ForeColor="Red"
                Font-Bold="true">
            </asp:Label>

            <br />

            <asp:Button ID="btnSave"
                runat="server"
                Text="Save Attendance"
                CssClass="btn btn-success"
                OnClick="btnSave_Click" />

            <br /><br />

            <asp:Label ID="lblMessage"
                runat="server">
            </asp:Label>

        </div>
    </form>
</body>
</html>
