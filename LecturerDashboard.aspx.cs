using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace Student_Information_Management_System_SIMS
{
    public partial class LecturerDashboard : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["CollegeManagementSystem"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Protect page (must login first)
            if (Session["RoleID"] == null || Convert.ToInt32(Session["RoleID"]) != 2)
            {
                Response.Redirect("LecturerLogin.aspx");
            }

            if (!IsPostBack)
            {
                if (Session["LecturerID"] == null)
                {
                    LoadLecturerID();
                }

                LoadProfile();
                LoadCourses();

                if (Session["SelectedCourseID"] != null)
                {
                    pnlCourses.Visible = false;
                    pnlStudents.Visible = true;

                    LoadSelectedCourseInfo();
                    LoadStudents();
                }

                UpdateSidebar();
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();              // clear session
            Response.Redirect("LecturerLogin.aspx"); // go back to login
        }

        private void LoadLecturerID()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"SELECT LecturerID FROM Lecturers WHERE UserID = @UserID";

                SqlCommand cmd =
                    new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue(
                    "@UserID",
                    Convert.ToInt32(Session["UserID"]));

                conn.Open();

                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    Session["LecturerID"] =
                        Convert.ToInt32(result);
                }
            }
        }

        private void LoadCourses()
        {
            using (SqlConnection conn =
                new SqlConnection(connStr))
            {
                string query = @"SELECT CourseID, CourseCode, CourseName FROM Courses WHERE LecturerID = @LecturerID";

                SqlCommand cmd =
                    new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@LecturerID", Convert.ToInt32(Session["LecturerID"]));

                SqlDataAdapter da =
                    new SqlDataAdapter(cmd);

                DataTable dt =
                    new DataTable();

                da.Fill(dt);

                rptCourses.DataSource = dt;
                rptCourses.DataBind();
            }
        }

        private void LoadStudents()
        {
            using (SqlConnection conn =
                new SqlConnection(connStr))
            {
                string query =
                    @"SELECT S.StudentID, U.FullName, U.Email
                      FROM Enrollments E

                      INNER JOIN Students S
                      ON E.StudentID = S.StudentID

                      INNER JOIN Users U
                      ON S.UserID = U.UserID

                      INNER JOIN Courses C
                      ON E.CourseID = C.CourseID

                      WHERE E.CourseID = @CourseID
                      AND C.LecturerID = @LecturerID";

                SqlCommand cmd =
                    new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue(
                    "@CourseID",
                    Convert.ToInt32(Session["SelectedCourseID"]));

                cmd.Parameters.AddWithValue("@LecturerID", Convert.ToInt32(Session["LecturerID"]));

                SqlDataAdapter da =
                    new SqlDataAdapter(cmd);

                DataTable dt =
                    new DataTable();

                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    lblNoStudents.Text =
                        "No students enrolled in this course.";

                    gvStudents.Visible = false;
                }
                else
                {
                    lblNoStudents.Text = "";

                    gvStudents.Visible = true;

                    gvStudents.DataSource = dt;
                    gvStudents.DataBind();
                }
            }
        }

        private void UpdateSidebar()
        {
            if (Session["SelectedCourseID"] == null)
            {
                lblSelectedCourse.Text =
                    "No Course Selected";

                lnkAttendance.CssClass = "menu-disabled";
                lnkAttendanceReport.CssClass = "menu-disabled";
                lnkEnterMarks.CssClass = "menu-disabled";
                lnkMarksReport.CssClass = "menu-disabled";
                lnkCourseNotes.CssClass = "menu-disabled";
                lnkAnnouncements.CssClass = "menu-disabled";
                lnkViewAnnouncements.CssClass = "menu-disabled";
            }
            else
            {
                LoadSelectedCourseInfo();

                lnkAttendance.CssClass = "menu-link";
                lnkAttendanceReport.CssClass = "menu-link";
                lnkEnterMarks.CssClass = "menu-link";
                lnkMarksReport.CssClass = "menu-link";

                lnkAttendance.NavigateUrl = "ManageAttendance.aspx";

                lnkAttendanceReport.NavigateUrl = "AttendanceReport.aspx";

                lnkEnterMarks.NavigateUrl = "EnterMarks.aspx";

                lnkMarksReport.NavigateUrl = "MarksReport.aspx";

                lnkCourseNotes.CssClass = "menu-link";
                lnkAnnouncements.CssClass = "menu-link";
                lnkViewAnnouncements.CssClass = "menu-link";
        
                lnkCourseNotes.NavigateUrl = "CourseNotes.aspx";
                lnkAnnouncements.NavigateUrl = "LecturerAnnouncement.aspx";
                lnkViewAnnouncements.NavigateUrl = "ViewAnnouncements.aspx";
            }
        }

        protected void rptCourses_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "SelectCourse")
            {
                Session["SelectedCourseID"] =
                    Convert.ToInt32(e.CommandArgument);

                LoadSelectedCourseInfo();
                LoadStudents();

                pnlCourses.Visible = false;
                pnlStudents.Visible = true;

                UpdateSidebar();
            }
        }

        protected void btnBack_Click(object sender,EventArgs e)
        {
            Session.Remove("SelectedCourseID");

            pnlStudents.Visible = false;
            pnlCourses.Visible = true;

            UpdateSidebar();
        }

        private void LoadSelectedCourseInfo()
        {
            using (SqlConnection conn =
                new SqlConnection(connStr))
            {
                string query = @"SELECT CourseCode, CourseName FROM Courses WHERE CourseID = @CourseID";

                SqlCommand cmd =
                    new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue(
                    "@CourseID",
                    Convert.ToInt32(Session["SelectedCourseID"]));

                conn.Open();

                SqlDataReader dr =
                    cmd.ExecuteReader();

                if (dr.Read())
                {
                    string courseCode =
                        dr["CourseCode"].ToString();

                    string courseName =
                        dr["CourseName"].ToString();

                    lblSelectedCourse.Text =
                        courseCode + ": " + courseName;

                    lblCourseCode.Text =
                        courseCode;

                    lblCourseName.Text =
                        courseName;
                }
            }
        }

        private void LoadProfile()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query =
                    "SELECT FullName, Email, PasswordHash FROM Users WHERE UserID = @UserID AND RoleID = 2";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue(
                    "@UserID",
                    Convert.ToInt32(Session["UserID"])
                );

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtUsername.Text = reader["FullName"].ToString();
                    txtEmail.Text = reader["Email"].ToString();
                }
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            txtUsername.ReadOnly = false;
            txtEmail.ReadOnly = false;
            txtPassword.ReadOnly = false;

            btnEdit.Visible = false;
            btnConfirm.Visible = true;
            btnCancel.Visible = true;
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query;
                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    query = @" UPDATE Users SET FullName = @Username, Email = @Email WHERE UserID = @UserID";
                }
                else
                {
                    query = @" UPDATE Users SET FullName = @Username, Email = @Email, PasswordHash = @Password WHERE UserID = @UserID";
                }

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@UserID", Convert.ToInt32(Session["UserID"]));

                if (!string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    cmd.Parameters.AddWithValue("@Password", txtPassword.Text);
                }

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            txtUsername.ReadOnly = true;
            txtEmail.ReadOnly = true;
            txtPassword.ReadOnly = true;

            btnEdit.Visible = true;
            btnConfirm.Visible = false;
            btnCancel.Visible = false;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            LoadProfile();

            txtUsername.ReadOnly = true;
            txtEmail.ReadOnly = true;
            txtPassword.ReadOnly = true;

            btnEdit.Visible = true;
            btnConfirm.Visible = false;
            btnCancel.Visible = false;
        }
    }
}
