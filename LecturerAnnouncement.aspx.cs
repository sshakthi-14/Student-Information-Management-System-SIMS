using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;

namespace SoftwareEngineeringProject
{
    public partial class LecturerAnnouncement : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["CollegeManagementSystem"].ConnectionString;
        int courseID;

        protected void Page_Load(object sender, EventArgs e)
        {
            // 1. Authenticate Lecturer
            if (Session["LecturerID"] == null || Session["UserID"] == null)
            {
                Response.Redirect("LecturerLogin.aspx");
                return;
            }

            // 2. Get CourseID from Session (Matches the Dashboard and Attendance logic!)
            if (Session["SelectedCourseID"] != null)
            {
                courseID = Convert.ToInt32(Session["SelectedCourseID"]);
            }
            else
            {
                // Instant bounce back if no course is selected
                Response.Redirect("LecturerDashboard.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadCourseName();
                LoadAnnouncements();
            }
        }

        void LoadCourseName()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT CourseName FROM Courses WHERE CourseID = @CourseID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@CourseID", courseID);
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        lblCourseName.Text = result.ToString();
                    }
                }
            }
        }

        void LoadAnnouncements()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                // Updated to use your new 'Content' and 'CreatedDate' columns
                string query = @"SELECT Title, Content, CreatedDate 
                                 FROM Announcements 
                                 WHERE CourseID = @CourseID AND AuthorRole = 'Lecturer' 
                                 ORDER BY CreatedDate DESC";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@CourseID", courseID);
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        gvAnnouncements.DataSource = dt;
                        gvAnnouncements.DataBind();
                    }
                }
            }
        }

        protected void btnPost_Click(object sender, EventArgs e)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(txtTitle.Text) || string.IsNullOrWhiteSpace(txtContent.Text))
            {
                lblStatus.Text = "Please enter both a title and a message.";
                lblStatus.ForeColor = Color.Red;
                return;
            }

            // Let's securely grab the Lecturer's real name from the Users table
            string lecturerName = "Lecturer";
            using (SqlConnection nameCon = new SqlConnection(cs))
            {
                string nameQuery = "SELECT FullName FROM Users WHERE UserID = @UserID";
                using (SqlCommand nameCmd = new SqlCommand(nameQuery, nameCon))
                {
                    nameCmd.Parameters.AddWithValue("@UserID", Convert.ToInt32(Session["UserID"]));
                    nameCon.Open();
                    object res = nameCmd.ExecuteScalar();
                    if (res != null) lecturerName = res.ToString();
                }
            }

            // Insert into the database using your new exact schema layout
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"INSERT INTO Announcements (Title, Content, AuthorRole, AuthorName, CourseID, CreatedDate, CreatedBy) 
                                 VALUES (@Title, @Content, @AuthorRole, @AuthorName, @CourseID, @CreatedDate, @CreatedBy)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Title", txtTitle.Text.Trim());
                    cmd.Parameters.AddWithValue("@Content", txtContent.Text.Trim());
                    cmd.Parameters.AddWithValue("@AuthorRole", "Lecturer");
                    cmd.Parameters.AddWithValue("@AuthorName", lecturerName);
                    cmd.Parameters.AddWithValue("@CourseID", courseID);
                    cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToInt32(Session["UserID"]));

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            lblStatus.Text = "Announcement posted successfully!";
            lblStatus.ForeColor = Color.Green;

            // Clear textboxes and refresh the grid
            txtTitle.Text = "";
            txtContent.Text = "";
            LoadAnnouncements();
        }
    }
}
