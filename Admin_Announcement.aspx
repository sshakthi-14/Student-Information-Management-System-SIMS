<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Admin_Announcement.aspx.cs" Inherits="Student_Information_Management_System__SIMS_.Admin.Admin_Announcement" %>
<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Admin Announcement - HELP University SIMS</title>
    <link rel="stylesheet" href="../Styles/SIMS.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.1/css/all.min.css" />
    <style>
        *{box-sizing:border-box}
        body{
            margin:0;
            background:#f5f7fb;
            color:#1f2937;
            font-family:var(--font-primary, Arial, sans-serif);
            font-size:14px;
        }
        .page-wrap{padding:28px}
        .topbar{display:flex;justify-content:space-between;align-items:flex-start;gap:20px;margin-bottom:20px;background:#fff;padding:28px 32px;border-bottom:1px solid #e5e7eb}
        .page-header h1{font-size:24px;line-height:1.2;margin:0 0 10px;font-weight:800;color:#1f2937;letter-spacing:-.2px}
        .page-header h1 i{margin-right:8px;color:#1f2937}
        .page-header p{margin:0;color:#64748b;font-size:14px;line-height:1.5;font-weight:400}
        .card{background:#fff;border-radius:18px;box-shadow:0 6px 18px rgba(15,23,42,.08);margin-bottom:24px;overflow:hidden}
        .card-header{padding:18px 24px;border-bottom:1px solid #e5e7eb;display:flex;justify-content:space-between;align-items:center;font-weight:800;color:#334155}
        .card-body{padding:24px}
        .filter-bar{display:grid;grid-template-columns:1fr 1fr 1fr 1.4fr 230px;gap:14px;align-items:end}
        .filter-action-stack{display:flex;flex-direction:column;gap:12px;align-self:end}
        .filter-action-row{display:flex;gap:12px;align-items:center}
        .filter-action-row .btn{flex:1;min-width:0}
        .filter-action-stack .btn-back{width:100%}
        .filter-item label,.form-label{display:block;font-size:13px;font-weight:700;color:#334155;margin-bottom:8px;text-transform:uppercase;letter-spacing:.3px}
        .search-box{position:relative}
        .search-box i{position:absolute;left:14px;top:50%;transform:translateY(-50%);color:#94a3b8;font-size:15px}
        .search-box .form-control{padding-left:40px}
        .form-control{width:100%;height:50px;padding:13px 15px;border:1px solid #d8dce3;border-radius:12px;box-sizing:border-box;font-size:14px;background:#fff;color:#111827;font-family:inherit;outline:none}
        .form-control:focus{border-color:#f08a00;box-shadow:0 0 0 3px rgba(240,138,0,.12)}
        .grid-2{display:grid;grid-template-columns:1fr 1fr;gap:18px}
        .form-group{margin-bottom:18px}
        textarea.form-control{height:auto;min-height:130px;resize:vertical;line-height:1.5}
        .form-actions{display:flex;justify-content:space-between;align-items:flex-end;gap:16px;flex-wrap:wrap}
        .form-actions-left{display:flex;flex-direction:column;gap:12px;align-items:flex-start}
        .form-actions-left .btn{min-width:160px}
        .form-actions-right{display:flex;justify-content:flex-end;align-items:center;gap:12px}
        .btn{font-family:inherit;border:none;padding:12px 24px;border-radius:30px;font-weight:700;cursor:pointer;text-decoration:none;display:inline-flex;align-items:center;justify-content:center;gap:8px;font-size:14px;line-height:1.2;white-space:nowrap;transition:.2s}
        .btn-sm{padding:11px 20px;min-height:43px}
        .btn-primary{color:#fff;background:linear-gradient(90deg,#ffb32c,#f08a00);border:2px solid transparent}
        .btn-outline{color:#f08a00;background:#fff;border:2px solid #f08a00}
        .btn:hover{transform:translateY(-1px);box-shadow:0 5px 14px rgba(240,138,0,.18)}
        .badge{display:inline-block;padding:7px 12px;border-radius:999px;font-weight:800;font-size:12px}.badge-orange{background:#fff7ed;color:#ea580c}
        .announcement-card{border:1px solid #e5e7eb;border-radius:16px;padding:18px;margin-bottom:16px;background:#fff;transition:.2s}
        .announcement-card:hover{box-shadow:0 8px 22px rgba(15,23,42,.08);transform:translateY(-1px)}
        .announcement-top{display:flex;justify-content:space-between;gap:16px;align-items:flex-start}
        .announcement-title{font-size:18px;font-weight:800;margin-bottom:8px;color:#1f2937}
        .announcement-meta{display:flex;flex-wrap:wrap;gap:13px;color:#64748b;font-size:13px}
        .announcement-content{margin-top:15px;line-height:1.65;color:#334155;white-space:pre-wrap;font-size:14px}
        .announcement-actions{display:flex;gap:10px;white-space:nowrap;align-items:center}
        .icon-btn{width:40px;height:40px;border-radius:50%;display:inline-flex;align-items:center;justify-content:center;text-decoration:none;border:2px solid #f08a00;color:#f08a00;background:#fff;transition:.2s}
        .icon-btn.delete{color:#ef4444;border-color:#ef4444}.icon-btn:hover{transform:translateY(-1px)}
        .empty-state{text-align:center;padding:45px 10px;color:#64748b}.empty-state i{font-size:44px;color:#f08a00;margin-bottom:12px}
        #customModalOverlay{display:none;position:fixed;inset:0;background:rgba(30,30,40,.60);z-index:9999;justify-content:center;align-items:center}#customModalOverlay.active{display:flex}#customModal{background:#fff;border-radius:16px;width:100%;max-width:410px;padding:36px 32px 28px;box-shadow:0 12px 40px rgba(0,0,0,.28);text-align:center}.cm-icon-wrap{width:68px;height:68px;border-radius:50%;display:flex;align-items:center;justify-content:center;margin:0 auto 16px;background:#fff8e1;color:#e8a838;font-size:30px}.cm-icon-wrap svg{width:34px;height:34px}.icon-delete{background:#fff1f2!important}.cm-title{font-size:1.2rem;font-weight:800;margin-bottom:14px}.cm-divider{border:none;border-top:1px solid #ececec;margin:0 -32px 18px}.cm-body{font-size:.97rem;line-height:1.65;color:#555;margin-bottom:28px}.cm-footer{display:flex;justify-content:center;gap:16px}.cm-btn{padding:10px 28px;border-radius:50px;font-weight:700;cursor:pointer;min-width:110px;background:#fff;border:2px solid #e8a838;color:#e8a838}.cm-btn-delete{background:#e74c3c!important;border-color:#e74c3c!important;color:#fff!important}
        @media(max-width:1000px){.filter-bar,.grid-2{grid-template-columns:1fr!important}.form-actions{justify-content:flex-start;align-items:flex-start}.form-actions-right{width:100%;justify-content:flex-start}.topbar{flex-direction:column;align-items:stretch}.filter-action-stack{width:100%}.filter-action-row{width:100%}.filter-action-row .btn{flex:1}.filter-action-stack .btn-back{width:100%}}
    </style>
</head>
<body>
<form id="form1" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server" />
<asp:HiddenField ID="hfAnnouncementId" runat="server" />
<asp:HiddenField ID="hfDeleteTarget" runat="server" />
<asp:Button ID="btnDeleteConfirmed" runat="server" Style="display:none;" OnClick="btnDeleteConfirmed_Click" CausesValidation="false" />
<div class="page-wrap">
    <div class="topbar">
        <div class="page-header">
            <h1><i class="fa-solid fa-bullhorn"></i> HELP University Admin Announcements</h1>
            <p>Select programme, course and session first, then add announcements for target HELP University users.</p>
        </div>
    </div>
    <div class="card">
        <div class="card-body">
            <div class="filter-bar">
                <div class="filter-item">
                    <label>Programme</label>
                    <asp:DropDownList ID="ddlFilterProgramme" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFilterProgramme_SelectedIndexChanged" />
                </div>
                <div class="filter-item">
                    <label>Course</label>
                    <asp:DropDownList ID="ddlFilterCourse" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="Filter_Changed" />
                </div>
                <div class="filter-item">
                    <label>Session</label>
                    <asp:DropDownList ID="ddlFilterSession" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="Filter_Changed" />
                </div>
                <div class="filter-item">
                    <label>Search</label>
                    <div class="search-box">
                        <i class="fa-solid fa-magnifying-glass"></i>
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search announcement title or content..." />
                    </div>
                </div>
                <div class="filter-item filter-action-stack">
                    <div class="filter-action-row">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-outline btn-sm" OnClick="btnSearch_Click" CausesValidation="false" />
                        <asp:LinkButton ID="btnShowAdd" runat="server" CssClass="btn btn-primary btn-sm" OnClick="btnShowAdd_Click" CausesValidation="false"><i class="fa-solid fa-plus"></i> Add</asp:LinkButton>
                    </div>
                    <asp:Button ID="btnBack" runat="server" Text="Back to Dashboard" CssClass="btn btn-outline btn-sm btn-back" OnClick="btnBack_Click" CausesValidation="false" />
                </div>
            </div>
        </div>
    </div>
    <asp:Panel ID="pnlForm" runat="server" CssClass="card" Visible="false">
        <div class="card-header">
            <span><asp:Label ID="lblFormTitle" runat="server" Text="Add HELP University Announcement" /></span>
        </div>
        <div class="card-body">
            <div class="grid-2">
                <div class="form-group">
                    <label class="form-label">Title *</label>
                    <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" MaxLength="200" />
                </div>
                <div class="form-group">
                    <label class="form-label">Target Role</label>
                    <asp:DropDownList ID="ddlTargetRole" runat="server" CssClass="form-control">
                        <asp:ListItem Value="All">All</asp:ListItem>
                        <asp:ListItem Value="Student">Student</asp:ListItem>
                        <asp:ListItem Value="Lecturer">Lecturer</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="grid-2">
                <div class="form-group">
                    <label class="form-label">Programme</label>
                    <asp:DropDownList ID="ddlProgramme" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlProgramme_SelectedIndexChanged" />
                </div>
                <div class="form-group">
                    <label class="form-label">Course</label>
                    <asp:DropDownList ID="ddlCourse" runat="server" CssClass="form-control" />
                </div>
            </div>
            <div class="grid-2">
                <div class="form-group">
                    <label class="form-label">Session</label>
                    <asp:DropDownList ID="ddlSession" runat="server" CssClass="form-control" />
                </div>
            </div>
            <div class="form-group">
                <label class="form-label">Announcement Content *</label>
                <asp:TextBox ID="txtContent" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="6" />
            </div>
            <div class="form-actions">
                <div class="form-actions-left">
                    <asp:Button ID="btnClear" runat="server" Text="Clear Form" CssClass="btn btn-outline btn-sm" OnClick="btnClear_Click" CausesValidation="false" />
                </div>
                <div class="form-actions-right">
                    <asp:Button ID="btnSave" runat="server" Text="Save Announcement" CssClass="btn btn-primary btn-sm" OnClick="btnSave_Click" />
                </div>
            </div>
        </div>
    </asp:Panel>
    <div class="card">
        <div class="card-header">
            <span>Posted Announcements</span>
            <span class="badge badge-orange"><asp:Label ID="lblTotal" runat="server" Text="0" /> Posted</span>
        </div>
        <div class="card-body">
            <asp:Repeater ID="rptAnnouncements" runat="server" OnItemCommand="rptAnnouncements_ItemCommand">
                <ItemTemplate>
                    <div class="announcement-card">
                        <div class="announcement-top">
                            <div>
                                <div class="announcement-title"><%# Eval("Title") %></div>
                                <div class="announcement-meta">
                                    <span><i class="fa-solid fa-calendar-days"></i> <%# Eval("CreatedAt", "{0:dd MMM yyyy, hh:mm tt}") %></span>
                                    <span><i class="fa-solid fa-users"></i> <%# Eval("TargetRole") %></span>
                                    <span><i class="fa-solid fa-layer-group"></i> <%# Eval("ProgrammeDisplay") %></span>
                                    <span><i class="fa-solid fa-book"></i> <%# Eval("CourseDisplay") %></span>
                                    <span><i class="fa-solid fa-clock"></i> <%# Eval("SessionDisplay") %></span>
                                </div>
                            </div>
                            <div class="announcement-actions">
                                <asp:LinkButton ID="btnEdit" runat="server" CssClass="icon-btn" CommandName="EditAnnouncement" CommandArgument='<%# Eval("AnnouncementId") %>' ToolTip="Edit"><i class="fa-solid fa-pen"></i></asp:LinkButton>
                                <asp:LinkButton ID="btnDelete" runat="server" CssClass="icon-btn delete" CommandArgument='<%# Eval("AnnouncementId") %>' ToolTip="Delete" OnClientClick='<%# "showMessageModal(\"Confirm Delete\", \"Are you sure you want to delete this announcement?\", true, \"" + Eval("AnnouncementId") + "\"); return false;" %>'><i class="fa-solid fa-trash"></i></asp:LinkButton>
                            </div>
                        </div>
                        <div class="announcement-content"><%# Eval("Content") %></div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            <asp:Panel ID="pnlEmpty" runat="server" CssClass="empty-state" Visible="false">
                <i class="fa-solid fa-bullhorn"></i>
                <h3>No announcements found</h3>
                <p>Try changing the filters or add a new announcement.</p>
            </asp:Panel>
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
            <button type="button" class="cm-btn" id="cmBtnCancel" style="display:none;" onclick="closeCustomModal()">Cancel</button>
            <button type="button" class="cm-btn cm-btn-delete" id="cmBtnDelete" style="display:none;">Yes, Delete</button>
            <button type="button" class="cm-btn" id="cmBtnOk" style="display:none;" onclick="closeCustomModal()">OK</button>
        </div>
    </div>
</div>
<script>
var SVG_TICK='<svg viewBox="0 0 24 24" fill="none" stroke="#e8a838" stroke-width="2.8" stroke-linecap="round" stroke-linejoin="round"><polyline points="20 6 9 17 4 12"/></svg>';
var SVG_WARN='<svg viewBox="0 0 24 24" fill="none" stroke="#e8a838" stroke-width="2.8" stroke-linecap="round" stroke-linejoin="round"><path d="M10.29 3.86L1.82 18a2 2 0 001.71 3h16.94a2 2 0 001.71-3L13.71 3.86a2 2 0 00-3.42 0z"/><line x1="12" y1="9" x2="12" y2="13"/><line x1="12" y1="17" x2="12.01" y2="17"/></svg>';
var SVG_TRASH='<svg viewBox="0 0 24 24" fill="none" stroke="#e53935" stroke-width="2.8" stroke-linecap="round" stroke-linejoin="round"><polyline points="3 6 5 6 21 6"/><path d="M19 6l-1 14a2 2 0 01-2 2H8a2 2 0 01-2-2L5 6"/><path d="M10 11v6"/><path d="M14 11v6"/><path d="M9 6V4h6v2"/></svg>';
var SVG_CROSS='<svg viewBox="0 0 24 24" fill="none" stroke="#e53935" stroke-width="2.8" stroke-linecap="round" stroke-linejoin="round"><line x1="18" y1="6" x2="6" y2="18"/><line x1="6" y1="6" x2="18" y2="18"/></svg>';
function showMessageModal(title,message,isConfirmDelete,id){
    var o=document.getElementById('customModalOverlay'),iw=document.getElementById('cmIconWrap'),ic=document.getElementById('cmIcon'),t=document.getElementById('cmTitle'),b=document.getElementById('cmBody'),ok=document.getElementById('cmBtnOk'),c=document.getElementById('cmBtnCancel'),d=document.getElementById('cmBtnDelete');
    iw.className='cm-icon-wrap';
    ic.innerHTML=isConfirmDelete?SVG_TRASH:(title==='Edit Mode'?SVG_WARN:(title.indexOf('Error')>=0?SVG_CROSS:SVG_TICK));
    if(isConfirmDelete)iw.classList.add('icon-delete');
    t.innerHTML=title;b.innerHTML=message;
    ok.style.display=isConfirmDelete?'none':'inline-block';
    c.style.display=isConfirmDelete?'inline-block':'none';
    d.style.display=isConfirmDelete?'inline-block':'none';
    if(isConfirmDelete){
        d.onclick=function(){
            document.getElementById('<%= hfDeleteTarget.ClientID %>').value=id;
            document.getElementById('<%= btnDeleteConfirmed.ClientID %>').click();
            };
        }
        o.classList.add('active');
    }
    function showCustomModal(message, type, isConfirmDelete, title) { showMessageModal(title, message, isConfirmDelete, ''); }
    function closeCustomModal() { document.getElementById('customModalOverlay').classList.remove('active'); }
</script>
</form>
</body>
</html>