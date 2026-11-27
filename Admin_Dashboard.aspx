<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Admin_Dashboard.aspx.cs" Inherits="Student_Information_Management_System__SIMS_.Admin.Admin_Dashboard" %>
<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Admin Dashboard - HELP University SIMS</title>
    <link rel="stylesheet" href="../Styles/SIMS.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.1/css/all.min.css" />
    <style>
        *{box-sizing:border-box}
        body{margin:0;background:#f5f7fb;color:#1f2937;font-family:var(--font-primary, Arial, sans-serif);font-size:14px}
        .page-wrap{padding:28px}
        .topbar{display:flex;justify-content:space-between;align-items:center;margin-bottom:24px;background:#fff;padding:24px 32px;border-radius:18px;box-shadow:0 6px 18px rgba(15,23,42,.06)}
        .page-header h1{font-size:24px;line-height:1.2;margin:0 0 6px;font-weight:800;color:#1f2937}
        .page-header p{margin:0;color:#64748b;font-size:14px}
        .user-info{display:flex;align-items:center;gap:14px}
        .avatar{width:46px;height:46px;border-radius:50%;background:linear-gradient(135deg,#ffb32c,#f08a00);color:#fff;display:flex;align-items:center;justify-content:center;font-weight:800;font-size:18px}
        .stats-grid{display:grid;grid-template-columns:repeat(4,1fr);gap:20px;margin-bottom:28px}
        .stat-card{background:#fff;padding:22px 24px;border-radius:18px;box-shadow:0 6px 18px rgba(15,23,42,.06);display:flex;align-items:center;justify-content:space-between}
        .stat-info .stat-number{font-size:28px;font-weight:800;color:#1f2937;margin-bottom:4px}
        .stat-info .stat-label{font-size:13px;color:#64748b;font-weight:600;text-transform:uppercase}
        .stat-icon{width:54px;height:54px;border-radius:14px;display:flex;align-items:center;justify-content:center;font-size:22px}
        .icon-blue{background:#eff6ff;color:#2563eb}
        .icon-orange{background:#fff7ed;color:#ea580c}
        .icon-green{background:#f0fdf4;color:#16a34a}
        .icon-purple{background:#faf5ff;color:#9333ea}
        .quick-links-grid{display:grid;grid-template-columns:repeat(3,1fr);gap:18px;margin-bottom:28px}
        .quick-link-item{background:#fff;padding:20px;border-radius:16px;box-shadow:0 4px 14px rgba(15,23,42,.05);text-decoration:none;color:#1f2937;display:flex;align-items:center;gap:16px;transition:.2s}
        .quick-link-item:hover{transform:translateY(-2px);box-shadow:0 8px 22px rgba(15,23,42,.1)}
        .quick-icon{width:44px;height:44px;border-radius:12px;background:#fff7ed;color:#f08a00;display:flex;align-items:center;justify-content:center;font-size:18px}
        .quick-text .title{font-weight:700;font-size:15px;margin-bottom:2px}
        .quick-text .desc{font-size:12px;color:#64748b}
        .card{background:#fff;border-radius:18px;box-shadow:0 6px 18px rgba(15,23,42,.06);margin-bottom:24px;overflow:hidden}
        .card-header{padding:18px 24px;border-bottom:1px solid #e5e7eb;display:flex;justify-content:space-between;align-items:center;font-weight:800;color:#334155}
        .card-body{padding:24px}
        .activity-list{list-style:none;padding:0;margin:0}
        .activity-item{display:flex;align-items:flex-start;gap:14px;padding:12px 0;border-bottom:1px solid #f1f5f9}
        .activity-item:last-child{border-bottom:none}
        .activity-dot{width:10px;height:10px;border-radius:50%;background:#f08a00;margin-top:5px;flex-shrink:0}
        .activity-details{flex:1}
        .activity-title{font-weight:600;color:#1f2937}
        .activity-time{font-size:12px;color:#94a3b8;margin-top:2px}
        @media(max-width:1100px){.stats-grid{grid-template-columns:repeat(2,1fr)}.quick-links-grid{grid-template-columns:repeat(2,1fr)}}
        @media(max-width:650px){.stats-grid,.quick-links-grid{grid-template-columns:1fr}.topbar{flex-direction:column;align-items:flex-start;gap:16px}}
    </style>
</head>
<body>
<form id="form1" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server" />
<div class="page-wrap">
    <div class="topbar">
        <div class="page-header">
            <h1><i class="fa-solid fa-gauge-high"></i> HELP University Admin Dashboard</h1>
            <p>Welcome back! Here is an overview of system metrics and administration quick links.</p>
        </div>
        <div class="user-info">
            <div class="avatar"><asp:Literal ID="litAvatarInitials" runat="server" Text="AD" /></div>
            <div>
                <div style="font-weight:800;color:#1f2937"><asp:Label ID="lblAdminName" runat="server" Text="System Administrator" /></div>
                <div style="font-size:12px;color:#64748b">HELP University Admin</div>
            </div>
        </div>
    </div>

    <!-- Key Statistics -->
    <div class="stats-grid">
        <div class="stat-card">
            <div class="stat-info">
                <div class="stat-number"><asp:Label ID="lblTotalStudents" runat="server" Text="0" /></div>
                <div class="stat-label">Total Students</div>
            </div>
            <div class="stat-icon icon-blue"><i class="fa-solid fa-user-graduate"></i></div>
        </div>
        <div class="stat-card">
            <div class="stat-info">
                <div class="stat-number"><asp:Label ID="lblTotalLecturers" runat="server" Text="0" /></div>
                <div class="stat-label">Total Lecturers</div>
            </div>
            <div class="stat-icon icon-purple"><i class="fa-solid fa-chalkboard-user"></i></div>
        </div>
        <div class="stat-card">
            <div class="stat-info">
                <div class="stat-number"><asp:Label ID="lblTotalProgrammes" runat="server" Text="0" /></div>
                <div class="stat-label">Programmes</div>
            </div>
            <div class="stat-icon icon-green"><i class="fa-solid fa-layer-group"></i></div>
        </div>
        <div class="stat-card">
            <div class="stat-info">
                <div class="stat-number"><asp:Label ID="lblTotalCourses" runat="server" Text="0" /></div>
                <div class="stat-label">Active Courses</div>
            </div>
            <div class="stat-icon icon-orange"><i class="fa-solid fa-book"></i></div>
        </div>
    </div>

    <!-- Quick Navigation Links -->
    <div class="card-header" style="margin-bottom:14px;background:none;padding:0;border:none">
        <span>Admin System Navigation</span>
    </div>
    <div class="quick-links-grid">
        <a href="Admin_Announcement.aspx" class="quick-link-item">
            <div class="quick-icon"><i class="fa-solid fa-bullhorn"></i></div>
            <div class="quick-text">
                <div class="title">Announcements</div>
                <div class="desc">Post & manage target notices</div>
            </div>
        </a>
        <a href="Student_Admin_Enrollment.aspx" class="quick-link-item">
            <div class="quick-icon"><i class="fa-solid fa-user-plus"></i></div>
            <div class="quick-text">
                <div class="title">Student Enrollment</div>
                <div class="desc">Register & enroll students</div>
            </div>
        </a>
        <a href="Admin_Profile.aspx" class="quick-link-item">
            <div class="quick-icon"><i class="fa-solid fa-id-card"></i></div>
            <div class="quick-text">
                <div class="title">Admin Profile</div>
                <div class="desc">Update personal account details</div>
            </div>
        </a>
        <a href="Assign_Lecturer_Course.aspx" class="quick-link-item">
            <div class="quick-icon"><i class="fa-solid fa-link"></i></div>
            <div class="quick-text">
                <div class="title">Assign Lecturer</div>
                <div class="desc">Map teaching assignments</div>
            </div>
        </a>
        <a href="Course_Offering.aspx" class="quick-link-item">
            <div class="quick-icon"><i class="fa-solid fa-calendar-days"></i></div>
            <div class="quick-text">
                <div class="title">Course Offerings</div>
                <div class="desc">Configure academic sessions</div>
            </div>
        </a>
        <a href="Manage_Course.aspx" class="quick-link-item">
            <div class="quick-icon"><i class="fa-solid fa-book-open"></i></div>
            <div class="quick-text">
                <div class="title">Manage Courses</div>
                <div class="desc">Add or edit course units</div>
            </div>
        </a>
        <a href="Manage_Fees.aspx" class="quick-link-item">
            <div class="quick-icon"><i class="fa-solid fa-receipt"></i></div>
            <div class="quick-text">
                <div class="title">Manage Fees</div>
                <div class="desc">Structure tuition & payments</div>
            </div>
        </a>
        <a href="Manage_Lecturers.aspx" class="quick-link-item">
            <div class="quick-icon"><i class="fa-solid fa-user-tie"></i></div>
            <div class="quick-text">
                <div class="title">Manage Lecturers</div>
                <div class="desc">Faculty profiles & credentials</div>
            </div>
        </a>
        <a href="Manage_Programs.aspx" class="quick-link-item">
            <div class="quick-icon"><i class="fa-solid fa-graduation-cap"></i></div>
            <div class="quick-text">
                <div class="title">Manage Programmes</div>
                <div class="desc">Degrees & academic paths</div>
            </div>
        </a>
        <a href="Manage_Students_Report.aspx" class="quick-link-item">
            <div class="quick-icon"><i class="fa-solid fa-chart-pie"></i></div>
            <div class="quick-text">
                <div class="title">Student Reports</div>
                <div class="desc">Analytics & student reports</div>
            </div>
        </a>
    </div>

    <!-- Recent Announcements Activity -->
    <div class="card">
        <div class="card-header">
            <span>Recent Announcements Overview</span>
            <a href="Admin_Announcement.aspx" style="color:#f08a00;text-decoration:none;font-size:13px">View All <i class="fa-solid fa-arrow-right"></i></a>
        </div>
        <div class="card-body">
            <asp:Repeater ID="rptRecentAnnouncements" runat="server">
                <HeaderTemplate><ul class="activity-list"></HeaderTemplate>
                <ItemTemplate>
                    <li class="activity-item">
                        <div class="activity-dot"></div>
                        <div class="activity-details">
                            <div class="activity-title"><%# Eval("Title") %></div>
                            <div class="activity-time">Posted on <%# Eval("CreatedAt", "{0:dd MMM yyyy, hh:mm tt}") %> • Target: <%# Eval("TargetRole") %></div>
                        </div>
                    </li>
                </ItemTemplate>
                <FooterTemplate></ul></FooterTemplate>
            </asp:Repeater>
            <asp:Panel ID="pnlNoActivity" runat="server" Visible="false" Style="text-align:center;color:#64748b;padding:20px 0;">
                No recent announcements posted.
            </asp:Panel>
        </div>
    </div>
</div>
</form>
</body>
</html>