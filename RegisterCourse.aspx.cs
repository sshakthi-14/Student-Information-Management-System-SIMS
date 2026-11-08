using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace Student_Information_Management_System_SIMS
{
    public partial class RegisterCourses : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["CollegeManagementSystem"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["RoleID"] == null || Convert.ToInt32(Session["RoleID"]) != 3)
            {
                Response.Redirect("StudentLogin.aspx");
            }

            if (!IsPostBack)
            {
                LoadAvailableCourses();
                LoadEnrolledCourses();
            }
        }

        private void LoadAvailableCourses()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"SELECT CourseID, CourseCode, CourseName, CreditHours 
                                 FROM Courses";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvAvailableCourses.DataSource = dt;
                gvAvailableCourses.DataBind();
            }
        }

        private void LoadEnrolledCourses()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"SELECT E.EnrollmentID, C.CourseCode, C.CourseName, E.Semester
                                 FROM Enrollments E
                                 INNER JOIN Students S ON E.StudentID = S.StudentID
                                 INNER JOIN Courses C ON E.CourseID = C.CourseID
                                 WHERE S.UserID = @UserID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", Session["UserID"]);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvEnrolledCourses.DataSource = dt;
                gvEnrolledCourses.DataBind();
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int courseID = Convert.ToInt32(btn.CommandArgument);
            int studentID = GetStudentID();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // 1. Check any duplicates
                string checkQuery = 
                    "SELECT COUNT(*) FROM Enrollments" +
                    "WHERE StudentID=@StudentID AND CourseID=@CourseID";
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@StudentID", studentID);
                checkCmd.Parameters.AddWithValue("@CourseID", courseID);

                int existing = Convert.ToInt32(checkCmd.ExecuteScalar());
                if (existing > 0)
                {
                    LoadAvailableCourses();
                    LoadEnrolledCourses();
                    return;
                }

                // 2. Insert
                string query = @"
                    INSERT INTO Enrollments (StudentID, CourseID, Semester, AcademicYear, EnrollmentStatus)
                    VALUES (@StudentID, @CourseID, 'Sem 1', 2026, 'Active')";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@StudentID", studentID);
                cmd.Parameters.AddWithValue("@CourseID", courseID);
                cmd.ExecuteNonQuery();
            }

            LoadAvailableCourses();
            LoadEnrolledCourses();
        }

        private int GetStudentID()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT StudentID FROM Students WHERE UserID=@UserID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", Session["UserID"]);

                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("StudentDashboard.aspx");
        }
    }
}
