using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Student_Information_Management_System__SIMS_.Admin
{
    public partial class Admin_Announcement : Page
    {
        private string connStr = ConfigurationManager.ConnectionStrings["SIMSConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindFilterDropdowns();
                BindFormDropdowns();
                LoadAnnouncements();
            }
        }

        private void BindFilterDropdowns()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                // Programme
                using (SqlCommand cmd = new SqlCommand("SELECT ProgrammeId, ProgrammeName FROM Programme ORDER BY ProgrammeName", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    ddlFilterProgramme.DataSource = dt;
                    ddlFilterProgramme.DataTextField = "ProgrammeName";
                    ddlFilterProgramme.DataValueField = "ProgrammeId";
                    ddlFilterProgramme.DataBind();
                    ddlFilterProgramme.Items.Insert(0, new ListItem("-- All Programmes --", "0"));
                }
                // Session
                using (SqlCommand cmd = new SqlCommand("SELECT SessionId, SessionName FROM Session ORDER BY SessionName DESC", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    ddlFilterSession.DataSource = dt;
                    ddlFilterSession.DataTextField = "SessionName";
                    ddlFilterSession.DataValueField = "SessionId";
                    ddlFilterSession.DataBind();
                    ddlFilterSession.Items.Insert(0, new ListItem("-- All Sessions --", "0"));
                }
            }
            BindFilterCourses();
        }

        private void BindFilterCourses()
        {
            int programmeId = Convert.ToInt32(ddlFilterProgramme.SelectedValue);
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                string sql = "SELECT CourseId, CourseName FROM Course";
                if (programmeId > 0)
                    sql += " WHERE ProgrammeId = @ProgrammeId";
                sql += " ORDER BY CourseName";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    if (programmeId > 0)
                        cmd.Parameters.AddWithValue("@ProgrammeId", programmeId);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    ddlFilterCourse.DataSource = dt;
                    ddlFilterCourse.DataTextField = "CourseName";
                    ddlFilterCourse.DataValueField = "CourseId";
                    ddlFilterCourse.DataBind();
                    ddlFilterCourse.Items.Insert(0, new ListItem("-- All Courses --", "0"));
                }
            }
        }

        private void BindFormDropdowns()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT ProgrammeId, ProgrammeName FROM Programme ORDER BY ProgrammeName", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    ddlProgramme.DataSource = dt;
                    ddlProgramme.DataTextField = "ProgrammeName";
                    ddlProgramme.DataValueField = "ProgrammeId";
                    ddlProgramme.DataBind();
                    ddlProgramme.Items.Insert(0, new ListItem("-- All Programmes --", "0"));
                }
                using (SqlCommand cmd = new SqlCommand("SELECT SessionId, SessionName FROM Session ORDER BY SessionName DESC", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    ddlSession.DataSource = dt;
                    ddlSession.DataTextField = "SessionName";
                    ddlSession.DataValueField = "SessionId";
                    ddlSession.DataBind();
                    ddlSession.Items.Insert(0, new ListItem("-- All Sessions --", "0"));
                }
            }
            BindFormCourses();
        }

        private void BindFormCourses()
        {
            int programmeId = Convert.ToInt32(ddlProgramme.SelectedValue);
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                string sql = "SELECT CourseId, CourseName FROM Course";
                if (programmeId > 0)
                    sql += " WHERE ProgrammeId = @ProgrammeId";
                sql += " ORDER BY CourseName";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    if (programmeId > 0)
                        cmd.Parameters.AddWithValue("@ProgrammeId", programmeId);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    ddlCourse.DataSource = dt;
                    ddlCourse.DataTextField = "CourseName";
                    ddlCourse.DataValueField = "CourseId";
                    ddlCourse.DataBind();
                    ddlCourse.Items.Insert(0, new ListItem("-- All Courses --", "0"));
                }
            }
        }

        private void LoadAnnouncements()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                string sql = @"SELECT a.AnnouncementId, a.Title, a.Content, a.TargetRole, a.CreatedAt,
                               ISNULL(p.ProgrammeName, 'All Programmes') AS ProgrammeDisplay,
                               ISNULL(c.CourseName, 'All Courses') AS CourseDisplay,
                               ISNULL(s.SessionName, 'All Sessions') AS SessionDisplay
                               FROM Announcement a
                               LEFT JOIN Programme p ON a.ProgrammeId = p.ProgrammeId
                               LEFT JOIN Course c ON a.CourseId = c.CourseId
                               LEFT JOIN Session s ON a.SessionId = s.SessionId
                               WHERE 1=1";

                int progId = Convert.ToInt32(ddlFilterProgramme.SelectedValue);
                int courseId = Convert.ToInt32(ddlFilterCourse.SelectedValue);
                int sessionId = Convert.ToInt32(ddlFilterSession.SelectedValue);
                string search = txtSearch.Text.Trim();

                if (progId > 0) sql += " AND (a.ProgrammeId = @ProgId OR a.ProgrammeId IS NULL)";
                if (courseId > 0) sql += " AND (a.CourseId = @CourseId OR a.CourseId IS NULL)";
                if (sessionId > 0) sql += " AND (a.SessionId = @SessionId OR a.SessionId IS NULL)";
                if (!string.IsNullOrEmpty(search)) sql += " AND (a.Title LIKE '%' + @Search + '%' OR a.Content LIKE '%' + @Search + '%')";

                sql += " ORDER BY a.CreatedAt DESC";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    if (progId > 0) cmd.Parameters.AddWithValue("@ProgId", progId);
                    if (courseId > 0) cmd.Parameters.AddWithValue("@CourseId", courseId);
                    if (sessionId > 0) cmd.Parameters.AddWithValue("@SessionId", sessionId);
                    if (!string.IsNullOrEmpty(search)) cmd.Parameters.AddWithValue("@Search", search);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    lblTotal.Text = dt.Rows.Count.ToString();
                    rptAnnouncements.DataSource = dt;
                    rptAnnouncements.DataBind();

                    pnlEmpty.Visible = (dt.Rows.Count == 0);
                }
            }
        }

        protected void ddlFilterProgramme_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindFilterCourses();
            LoadAnnouncements();
        }

        protected void Filter_Changed(object sender, EventArgs e)
        {
            LoadAnnouncements();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadAnnouncements();
        }

        protected void btnShowAdd_Click(object sender, EventArgs e)
        {
            ClearForm();
            lblFormTitle.Text = "Add HELP University Announcement";
            pnlForm.Visible = true;
        }

        protected void ddlProgramme_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindFormCourses();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            hfAnnouncementId.Value = string.Empty;
            txtTitle.Text = string.Empty;
            txtContent.Text = string.Empty;
            ddlTargetRole.SelectedValue = "All";
            ddlProgramme.SelectedValue = "0";
            BindFormCourses();
            ddlCourse.SelectedValue = "0";
            ddlSession.SelectedValue = "0";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text) || string.IsNullOrWhiteSpace(txtContent.Text))
            {
                ClientScript.RegisterStartupScript(GetType(), "alert", "showMessageModal('Error', 'Please fill in Title and Content.', false, '');", true);
                return;
            }

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                string sql;
                if (string.IsNullOrEmpty(hfAnnouncementId.Value))
                {
                    sql = @"INSERT INTO Announcement (Title, Content, TargetRole, ProgrammeId, CourseId, SessionId, CreatedAt)
                            VALUES (@Title, @Content, @TargetRole, @ProgrammeId, @CourseId, @SessionId, GETDATE())";
                }
                else
                {
                    sql = @"UPDATE Announcement 
                            SET Title = @Title, Content = @Content, TargetRole = @TargetRole,
                                ProgrammeId = @ProgrammeId, CourseId = @CourseId, SessionId = @SessionId
                            WHERE AnnouncementId = @AnnouncementId";
                }

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Title", txtTitle.Text.Trim());
                    cmd.Parameters.AddWithValue("@Content", txtContent.Text.Trim());
                    cmd.Parameters.AddWithValue("@TargetRole", ddlTargetRole.SelectedValue);
                    cmd.Parameters.AddWithValue("@ProgrammeId", ddlProgramme.SelectedValue == "0" ? (object)DBNull.Value : Convert.ToInt32(ddlProgramme.SelectedValue));
                    cmd.Parameters.AddWithValue("@CourseId", ddlCourse.SelectedValue == "0" ? (object)DBNull.Value : Convert.ToInt32(ddlCourse.SelectedValue));
                    cmd.Parameters.AddWithValue("@SessionId", ddlSession.SelectedValue == "0" ? (object)DBNull.Value : Convert.ToInt32(ddlSession.SelectedValue));

                    if (!string.IsNullOrEmpty(hfAnnouncementId.Value))
                        cmd.Parameters.AddWithValue("@AnnouncementId", Convert.ToInt32(hfAnnouncementId.Value));

                    cmd.ExecuteNonQuery();
                }
            }

            pnlForm.Visible = false;
            ClearForm();
            LoadAnnouncements();
            ClientScript.RegisterStartupScript(GetType(), "alert", "showMessageModal('Success', 'Announcement saved successfully!', false, '');", true);
        }

        protected void rptAnnouncements_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "EditAnnouncement")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                LoadAnnouncementForEdit(id);
            }
        }

        private void LoadAnnouncementForEdit(int id)
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Announcement WHERE AnnouncementId = @Id", con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        hfAnnouncementId.Value = dr["AnnouncementId"].ToString();
                        txtTitle.Text = dr["Title"].ToString();
                        txtContent.Text = dr["Content"].ToString();
                        ddlTargetRole.SelectedValue = dr["TargetRole"].ToString();

                        string progId = dr["ProgrammeId"] == DBNull.Value ? "0" : dr["ProgrammeId"].ToString();
                        ddlProgramme.SelectedValue = progId;
                        BindFormCourses();

                        string courseId = dr["CourseId"] == DBNull.Value ? "0" : dr["CourseId"].ToString();
                        ddlCourse.SelectedValue = courseId;

                        string sessionId = dr["SessionId"] == DBNull.Value ? "0" : dr["SessionId"].ToString();
                        ddlSession.SelectedValue = sessionId;

                        lblFormTitle.Text = "Edit HELP University Announcement";
                        pnlForm.Visible = true;
                    }
                }
            }
        }

        protected void btnDeleteConfirmed_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hfDeleteTarget.Value))
            {
                int id = Convert.ToInt32(hfDeleteTarget.Value);
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM Announcement WHERE AnnouncementId = @Id", con))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
                hfDeleteTarget.Value = string.Empty;
                LoadAnnouncements();
                ClientScript.RegisterStartupScript(GetType(), "alert", "showMessageModal('Deleted', 'Announcement deleted successfully.', false, '');", true);
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("Admin_Dashboard.aspx");
        }
    }
}