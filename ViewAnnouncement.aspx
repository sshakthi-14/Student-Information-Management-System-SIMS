<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewAnnouncements.aspx.cs" Inherits="SoftwareEngineeringProject.ViewAnnouncements" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Academic Announcements</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="EnterMarks.css" rel="stylesheet" />
    <style>
        /* Forces the left alert border to be highly visible */
        .border-5 { border-width: 5px !important; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        
        <div class="marks-header">
            <span class="course-header">Academic Announcements</span>
        </div>

        <div class="marks-menu">
            <asp:Button ID="btnBackDashboard" runat="server" Text="← Dashboard" OnClick="btnBack_Click" CssClass="menu-btn" />
        </div>

        <div class="container mt-4" style="width: 95%; margin: auto;">
            
            <asp:Label ID="lblMessage" runat="server" CssClass="alert alert-warning d-block fw-bold shadow-sm" Visible="false"></asp:Label>

            <asp:Repeater ID="rptAnnouncements" runat="server">
                <ItemTemplate>
                    
                    <div class="card mb-4 shadow-sm <%# Eval("AuthorRole").ToString() == "Admin" ? "border-start border-danger border-5" : "border-start border-primary border-5" %>">
                        <div class="card-body">
                            
                            <h4 class="card-title fw-bold mb-1">
                                <%# Eval("Title") %> 
                                <span class="badge bg-info text-dark ms-2 align-middle" style="font-size: 0.55em;"><%# Eval("AuthorRole") %></span>
                            </h4>
                            
                            <p class="card-subtitle text-muted mb-3" style="font-size: 0.85em;">
                                Posted by <strong><%# Eval("AuthorName") %></strong> on <%# Convert.ToDateTime(Eval("CreatedDate")).ToString("dd MMM yyyy, hh:mm tt") %>
                            </p>
                            
                            <hr />
                            
                            <p class="card-text"><%# Eval("Content") %></p>
                            
                        </div>
                    </div>

                </ItemTemplate>
            </asp:Repeater>

        </div>
    </form>
</body>
</html>
