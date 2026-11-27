using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace Student_Information_Management_System__SIMS_.Admin
{
    public partial class Admin_Dashboard : Page
    {
        private string connStr = ConfigurationManager.ConnectionStrings["SIMSConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAdminProfile();
                LoadDashboardStatistics();
                LoadRecentAnnouncements();
            }
        }

        private void LoadAdminProfile()
        {
            if (Session["AdminName"] != null)
            {
                string name = Session["AdminName"].ToString();
                lblAdminName.Text = name;
                if (!string.IsNullOrEmpty(name))
                {
                    string[] parts = name.Split(' ');
                    if (parts.Length >= 2)
                        litAvatarInitials.Text = (parts[0][0].ToString() + parts[1][0].ToString()).ToUpper();
                    else if (name.Length >= 2)
                        litAvatarInitials.Text = name.Substring(0, 2).ToUpper();
                }
            }
        }

        private void LoadDashboardStatistics()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                // Total Students
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Student", con))
                {
                    lblTotalStudents.Text = Convert.ToInt32(cmd.ExecuteScalar()).ToString("N0");
                }

                // Total Lecturers
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Lecturer", con))
                {
                    lblTotalLecturers.Text = Convert.ToInt32(cmd.ExecuteScalar()).ToString("N0");
                }

                // Total Programmes
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Programme", con))
                {
                    lblTotalProgrammes.Text = Convert.ToInt32(cmd.ExecuteScalar()).ToString("N0");
                }

                // Total Active Courses
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Course", con))
                {
                    lblTotalCourses.Text = Convert.ToInt32(cmd.ExecuteScalar()).ToString("N0");
                }
            }
        }

        private void LoadRecentAnnouncements()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                string sql = "SELECT TOP 5 Title, TargetRole, CreatedAt FROM Announcement ORDER BY CreatedAt DESC";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    rptRecentAnnouncements.DataSource = dt;
                    rptRecentAnnouncements.DataBind();

                    pnlNoActivity.Visible = (dt.Rows.Count == 0);
                }
            }
        }
    }
}