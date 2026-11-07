<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EnterMarks.aspx.cs" Inherits="SoftwareEngineeringProject.EnterMarks" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>Enter Marks</title>

        <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
        <link href="EnterMarks.css" rel="stylesheet" />
    </head>

    <body>
        <form id="form1" runat="server">

            <div class="marks-header">
                <asp:Label ID="lblCourse" runat="server" CssClass="course-header" />
            </div>

            <div class="marks-menu">

                <asp:Button ID="btnBackDashboard" runat="server"
                    Text="← Dashboard"
                    PostBackUrl="~/LecturerDashboard.aspx"
                    CssClass="menu-btn" />

                <asp:Button ID="btnAttendance" runat="server"
                    Text="Attendance"
                    PostBackUrl="~/ManageAttendance.aspx"
                    CssClass="menu-btn" />

                <asp:Button ID="btnAttendanceReport" runat="server"
                    Text="Attendance Report"
                    PostBackUrl="~/AttendanceReport.aspx"
                    CssClass="menu-btn" />

                <asp:Button ID="btnMarks" runat="server"
                    Text="Enter Marks"
                    CssClass="menu-btn active-btn" />

                <asp:Button ID="btnMarksReport" runat="server"
                    Text="Marks Report"
                    PostBackUrl="~/MarksReport.aspx"
                    CssClass="menu-btn" />

            </div>

            <br />

            <asp:Label ID="lblMessage" runat="server" ForeColor="Red" />
            <br /><br />

            <div class="marks-table-wrapper">

                <asp:GridView ID="gvStudents"
                    runat="server"
                    AutoGenerateColumns="False"
                    DataKeyNames="EnrollmentID"
                    OnRowCommand="gvStudents_RowCommand"
                    CssClass="table table-bordered table-striped">

                    <Columns>

                        <asp:BoundField DataField="StudentID" HeaderText="Student ID" />
                        <asp:BoundField DataField="FullName" HeaderText="Student Name" />

                        <asp:TemplateField HeaderText="Assignments">
                            <ItemTemplate>

                                <asp:Button ID="btnViewAssignments"
                                    runat="server"
                                    Text="View Assignments"
                                    CssClass="btn btn-primary btn-sm"
                                    CommandName="ViewAssignments"
                                    CommandArgument='<%# Eval("EnrollmentID") %>' />

                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Mid Test">
                            <ItemTemplate>
                                <asp:TextBox ID="txtMidTest" runat="server" CssClass="form-control" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Final Exam">
                            <ItemTemplate>
                                <asp:TextBox ID="txtFinalExam" runat="server" CssClass="form-control" />
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>

                </asp:GridView>

            </div>

            <br />

            <asp:Button ID="btnSave"
                runat="server"
                Text="Save Marks"
                CssClass="btn btn-success"
                OnClick="btnSave_Click" />

        </form>
    </body>
</html>
