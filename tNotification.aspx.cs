using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Student_Information_Management_System_SIMS
{
    public partial class tNotification : System.Web.UI.Page
    {
        private readonly string connStr =
            ConfigurationManager.ConnectionStrings["CollegeManagementSystem"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("StudentLogin.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadAllNotifications();
            }
        }
        private void LoadAllNotifications()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT 
                        A.Title AS Title,
                        A.Content AS AlertMessage,
                        A.AuthorRole AS AlertType,
                        A.CreatedDate AS CreatedDate
                    FROM Announcements A
                    WHERE A.CourseID IS NULL
                       OR A.CourseID IN
                       (
                           SELECT E.CourseID
                           FROM Enrollments E
                           INNER JOIN Students S ON E.StudentID = S.StudentID
                           WHERE S.UserID = @UserID
                       )

                    UNION ALL

                    SELECT
                        SA.AlertType AS Title,
                        SA.AlertMessage AS AlertMessage,
                        SA.AlertType AS AlertType,
                        SA.CreatedDate AS CreatedDate
                    FROM StudentAlerts SA
                    INNER JOIN Students S ON SA.StudentID = S.StudentID
                    WHERE S.UserID = @UserID

                    ORDER BY CreatedDate DESC";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue(
                        "@UserID",
                        Convert.ToInt32(Session["UserID"]));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();

                        try
                        {
                            con.Open();
                            da.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                lblMessage.Visible = false;
                                rptNotifications.DataSource = dt;
                                rptNotifications.DataBind();
                            }
                            else
                            {
                                lblMessage.Text =
                                    "You are all caught up! No notifications right now.";
                                lblMessage.Visible = true;

                                rptNotifications.DataSource = null;
                                rptNotifications.DataBind();
                            }
                        }
                        catch (Exception ex)
                        {
                            lblMessage.Text =
                                "Error loading notifications: " + ex.Message;
                            lblMessage.Visible = true;
                        }
                    }
                }
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("StudentDashboard.aspx");
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("StudentLogin.aspx");
        }
    }
}
