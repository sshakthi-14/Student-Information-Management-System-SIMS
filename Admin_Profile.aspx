<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Admin_Profile.aspx.cs" Inherits="Student_Information_Management_System__SIMS_.Admin.Admin_Profile" %>
<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Admin Profile - HELP University SIMS</title>
    <link rel="stylesheet" href="../Styles/SIMS.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.1/css/all.min.css" />
    <style>
        *{box-sizing:border-box}
        body{margin:0;background:#f5f7fb;color:#1f2937;font-family:var(--font-primary, Arial, sans-serif);font-size:14px}
        .page-wrap{padding:28px}
        .topbar{display:flex;justify-content:space-between;align-items:center;margin-bottom:24px;background:#fff;padding:28px 32px;border-radius:18px;box-shadow:0 6px 18px rgba(15,23,42,.06)}
        .page-header h1{font-size:24px;line-height:1.2;margin:0 0 6px;font-weight:800;color:#1f2937}
        .page-header p{margin:0;color:#64748b;font-size:14px}
        .profile-container{display:grid;grid-template-columns:320px 1fr;gap:24px}
        .card{background:#fff;border-radius:18px;box-shadow:0 6px 18px rgba(15,23,42,.06);margin-bottom:24px;overflow:hidden}
        .card-header{padding:18px 24px;border-bottom:1px solid #e5e7eb;display:flex;justify-content:space-between;align-items:center;font-weight:800;color:#334155}
        .card-body{padding:24px}
        .profile-card-body{display:flex;flex-direction:column;align-items:center;text-align:center;padding:32px 24px}
        .avatar-large{width:90px;height:90px;border-radius:50%;background:linear-gradient(135deg,#ffb32c,#f08a00);color:#fff;display:flex;align-items:center;justify-content:center;font-weight:800;font-size:32px;margin-bottom:16px;box-shadow:0 8px 20px rgba(240,138,0,.25)}
        .profile-name{font-size:20px;font-weight:800;color:#1f2937;margin-bottom:4px}
        .profile-role{font-size:13px;color:#f08a00;font-weight:700;text-transform:uppercase;letter-spacing:.5px;margin-bottom:16px}
        .badge{display:inline-block;padding:6px 14px;border-radius:999px;font-weight:700;font-size:12px;background:#f0fdf4;color:#16a34a;border:1px solid #bbf7d0}
        .grid-2{display:grid;grid-template-columns:1fr 1fr;gap:18px}
        .form-group{margin-bottom:18px}
        .form-label{display:block;font-size:13px;font-weight:700;color:#334155;margin-bottom:8px;text-transform:uppercase;letter-spacing:.3px}
        .form-control{width:100%;height:50px;padding:13px 15px;border:1px solid #d8dce3;border-radius:12px;box-sizing:border-box;font-size:14px;background:#fff;color:#111827;font-family:inherit;outline:none;transition:.2s}
        .form-control:focus{border-color:#f08a00;box-shadow:0 0 0 3px rgba(240,138,0,.12)}
        .form-control[readonly]{background:#f8fafc;color:#64748b;cursor:not-allowed}
        .form-actions{display:flex;justify-content:flex-end;gap:12px;margin-top:24px}
        .btn{font-family:inherit;border:none;padding:12px 24px;border-radius:30px;font-weight:700;cursor:pointer;text-decoration:none;display:inline-flex;align-items:center;justify-content:center;gap:8px;font-size:14px;line-height:1.2;white-space:nowrap;transition:.2s}
        .btn-primary{color:#fff;background:linear-gradient(90deg,#ffb32c,#f08a00);border:2px solid transparent}
        .btn-outline{color:#f08a00;background:#fff;border:2px solid #f08a00}
        .btn:hover{transform:translateY(-1px);box-shadow:0 5px 14px rgba(240,138,0,.18)}
        #customModalOverlay{display:none;position:fixed;inset:0;background:rgba(30,30,40,.60);z-index:9999;justify-content:center;align-items:center}#customModalOverlay.active{display:flex}#customModal{background:#fff;border-radius:16px;width:100%;max-width:410px;padding:36px 32px 28px;box-shadow:0 12px 40px rgba(0,0,0,.28);text-align:center}.cm-icon-wrap{width:68px;height:68px;border-radius:50%;display:flex;align-items:center;justify-content:center;margin:0 auto 16px;background:#fff8e1;color:#e8a838;font-size:30px}.cm-icon-wrap svg{width:34px;height:34px}.cm-title{font-size:1.2rem;font-weight:800;margin-bottom:14px}.cm-divider{border:none;border-top:1px solid #ececec;margin:0 -32px 18px}.cm-body{font-size:.97rem;line-height:1.65;color:#555;margin-bottom:28px}.cm-footer{display:flex;justify-content:center;gap:16px}.cm-btn{padding:10px 28px;border-radius:50px;font-weight:700;cursor:pointer;min-width:110px;background:#fff;border:2px solid #e8a838;color:#e8a838}
        @media(max-width:900px){.profile-container{grid-template-columns:1fr}.grid-2{grid-template-columns:1fr}}
    </style>
</head>
<body>
<form id="form1" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server" />
<div class="page-wrap">
    <div class="topbar">
        <div class="page-header">
            <h1><i class="fa-solid fa-id-card"></i> HELP University Admin Profile</h1>
            <p>Manage your account credentials and personal contact details.</p>
        </div>
        <asp:Button ID="btnBack" runat="server" Text="Back to Dashboard" CssClass="btn btn-outline" OnClick="btnBack_Click" CausesValidation="false" />
    </div>

    <div class="profile-container">
        <!-- Sidebar Profile Summary Card -->
        <div class="card">
            <div class="profile-card-body">
                <div class="avatar-large"><asp:Literal ID="litAvatarInitials" runat="server" Text="AD" /></div>
                <div class="profile-name"><asp:Label ID="lblCardFullName" runat="server" Text="System Administrator" /></div>
                <div class="profile-role">Administrator</div>
                <span class="badge"><i class="fa-solid fa-circle-check"></i> Active Account</span>
            </div>
        </div>

        <!-- Main Settings Panel -->
        <div>
            <!-- Personal Information Form -->
            <div class="card">
                <div class="card-header"><span>Personal Information</span></div>
                <div class="card-body">
                    <div class="grid-2">
                        <div class="form-group">
                            <label class="form-label">Admin ID</label>
                            <asp:TextBox ID="txtAdminId" runat="server" CssClass="form-control" ReadOnly="true" />
                        </div>
                        <div class="form-group">
                            <label class="form-label">Username</label>
                            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" ReadOnly="true" />
                        </div>
                    </div>
                    <div class="grid-2">
                        <div class="form-group">
                            <label class="form-label">Full Name *</label>
                            <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" MaxLength="100" />
                        </div>
                        <div class="form-group">
                            <label class="form-label">Email Address *</label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" MaxLength="100" />
                        </div>
                    </div>
                    <div class="grid-2">
                        <div class="form-group">
                            <label class="form-label">Phone Number</label>
                            <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" MaxLength="20" />
                        </div>
                    </div>
                    <div class="form-actions">
                        <asp:Button ID="btnSaveProfile" runat="server" Text="Update Profile" CssClass="btn btn-primary" OnClick="btnSaveProfile_Click" />
                    </div>
                </div>
            </div>

            <!-- Password Security Form -->
            <div class="card">
                <div class="card-header"><span>Change Security Password</span></div>
                <div class="card-body">
                    <div class="form-group">
                        <label class="form-label">Current Password *</label>
                        <asp:TextBox ID="txtCurrentPassword" runat="server" CssClass="form-control" TextMode="Password" />
                    </div>
                    <div class="grid-2">
                        <div class="form-group">
                            <label class="form-label">New Password *</label>
                            <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control" TextMode="Password" />
                        </div>
                        <div class="form-group">
                            <label class="form-label">Confirm New Password *</label>
                            <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" />
                        </div>
                    </div>
                    <div class="form-actions">
                        <asp:Button ID="btnChangePassword" runat="server" Text="Change Password" CssClass="btn btn-primary" OnClick="btnChangePassword_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<div id="customModalOverlay">
    <div id="customModal">
        <div class="cm-icon-wrap" id="cmIconWrap"><span id="cmIcon"></span></div>
        <div class="cm-title" id="cmTitle">Message</div>
        <hr class="cm-divider"/>
        <div class="cm-body" id="cmBody"></div>
        <div class="cm-footer">
            <button type="button" class="cm-btn" id="cmBtnOk" onclick="closeCustomModal()">OK</button>
        </div>
        </div>
    </div>
</div>

<script>
var SVG_TICK='<svg viewBox="0 0 24 24" fill="none" stroke="#e8a838" stroke-width="2.8" stroke-linecap="round" stroke-linejoin="round"><polyline points="20 6 9 17 4 12"/></svg>';
var SVG_CROSS='<svg viewBox="0 0 24 24" fill="none" stroke="#e53935" stroke-width="2.8" stroke-linecap="round" stroke-linejoin="round"><line x1="18" y1="6" x2="6" y2="18"/><line x1="6" y1="6" x2="18" y2="18"/></svg>';
function showMessageModal(title, message) {
    var o = document.getElementById('customModalOverlay'), ic = document.getElementById('cmIcon'), t = document.getElementById('cmTitle'), b = document.getElementById('cmBody');
    ic.innerHTML = (title.indexOf('Error') >= 0 || title.indexOf('Failed') >= 0) ? SVG_CROSS : SVG_TICK;
    t.innerHTML = title; b.innerHTML = message;
    o.classList.add('active');
}
function closeCustomModal() { document.getElementById('customModalOverlay').classList.remove('active'); }
</script>
</form>
</body>
</html>