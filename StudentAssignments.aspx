<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StudentAssignments.aspx.cs" Inherits="SoftwareEngineeringProject.StudentAssignments" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>Student Assignments</title>

        <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />

        <link href="StudentAssignments.css" rel="stylesheet" />
    </head>

    <body>

        <form id="form1" runat="server">

            <div class="assignment-header">

                <asp:Label ID="lblCourse"
                    runat="server"
                    CssClass="course-header">
                </asp:Label>

            </div>

            <div class="assignment-menu">

                <asp:Button ID="btnBackMarks"
                    runat="server"
                    Text="← Enter Marks"
                    PostBackUrl="~/EnterMarks.aspx"
                    CssClass="menu-btn" />

            </div>

            <br />

            <div class="student-box">

                <h4>Student Information</h4>

                <asp:Label ID="lblStudent"
                    runat="server"
                    CssClass="student-label">
                </asp:Label>

            </div>

            <br />

            <!-- Assignment Table -->

            <div class="assignment-table-wrapper">

                <asp:GridView ID="gvAssignments"
                    runat="server"
                    AutoGenerateColumns="False"
                    DataKeyNames="SubmissionID"
                    CssClass="table table-bordered table-striped">

                    <Columns>

                        <asp:BoundField
                            DataField="AssignmentTitle"
                            HeaderText="Assignment Title" />

                        <asp:TemplateField
                            HeaderText="Submitted File">

                            <ItemTemplate>

                                <asp:HyperLink ID="lnkFile"
                                    runat="server"
                                    Text='<%# Eval("FileName") %>'
                                    NavigateUrl='<%# Eval("FilePath") %>'
                                    Target="_blank">
                                </asp:HyperLink>

                            </ItemTemplate>

                        </asp:TemplateField>

                        <asp:TemplateField
                            HeaderText="Marks">

                            <ItemTemplate>

                                <asp:TextBox ID="txtMarks"
                                    runat="server"
                                    CssClass="form-control"
                                    Width="100px"
                                    Text='<%# Eval("Marks") %>'>
                                </asp:TextBox>

                            </ItemTemplate>

                        </asp:TemplateField>

                    </Columns>

                </asp:GridView>

            </div>

            <br />

            <asp:Button ID="btnSave"
                runat="server"
                Text="Save Assignment Marks"
                CssClass="btn btn-success"
                OnClick="btnSave_Click" />

            <br /><br />

            <asp:Label ID="lblMessage"
                runat="server">
            </asp:Label>

        </form>

    </body>
</html>
