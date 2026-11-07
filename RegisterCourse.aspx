<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegisterCourses.aspx.cs" Inherits="SoftwareEngineeringProject.RegisterCourses" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Register / Drop Courses</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>

<body>
<form id="form1" runat="server">

<div class="container mt-4">

    <h2>Course Registration</h2>

    <asp:Button ID="btnBack" runat="server" Text="← Back"
        CssClass="btn btn-secondary mb-3"
        OnClick="btnBack_Click" />

    <hr />

    <!-- AVAILABLE COURSES -->
    <h4>Available Courses</h4>

    <asp:GridView ID="gvAvailableCourses"
        runat="server"
        AutoGenerateColumns="False"
        CssClass="table table-bordered">

        <Columns>
            <asp:BoundField DataField="CourseCode" HeaderText="Code" />
            <asp:BoundField DataField="CourseName" HeaderText="Course Name" />
            <asp:BoundField DataField="CreditHours" HeaderText="Credits" />

            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Button ID="btnRegister"
                        runat="server"
                        Text="Register"
                        CssClass="btn btn-success"
                        CommandArgument='<%# Eval("CourseID") %>'
                        OnClick="btnRegister_Click" />
                </ItemTemplate>
            </asp:TemplateField>

        </Columns>
    </asp:GridView>

    <hr />

    <!-- ENROLLED COURSES -->
    <h4>My Enrolled Courses</h4>

    <asp:GridView ID="gvEnrolledCourses"
        runat="server"
        AutoGenerateColumns="False"
        CssClass="table table-striped">

        <Columns>
            <asp:BoundField DataField="CourseCode" HeaderText="Code" />
            <asp:BoundField DataField="CourseName" HeaderText="Course Name" />
            <asp:BoundField DataField="Semester" HeaderText="Semester" />

            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Button ID="btnDrop"
                        runat="server"
                        Text="Drop"
                        CssClass="btn btn-danger"
                        CommandArgument='<%# Eval("EnrollmentID") %>'
                        OnClick="btnDrop_Click" />
                </ItemTemplate>
            </asp:TemplateField>

        </Columns>
    </asp:GridView>

</div>

</form>
</body>
</html>
