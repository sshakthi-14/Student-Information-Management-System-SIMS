using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Student_Information_Management_System_SIMS
{
    public partial class Attendance : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["CollegeManagementSystem"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["RoleID"] == null || Convert.ToInt32(Session["RoleID"]) != 3)
            {
                Response.Redirect("StudentLogin.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadAttendance();
            }
        }

        private void LoadAttendance()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"SELECT C.CourseCode, C.CourseName, A.AttendanceDate, A.Status
                                 FROM Attendance A
                                 INNER JOIN Enrollments E ON A.EnrollmentID = E.EnrollmentID
                                 INNER JOIN Students S ON E.StudentID = S.StudentID
                                 INNER JOIN Courses C ON E.CourseID = C.CourseID
                                 WHERE S.UserID = @UserID
                                 ORDER BY A.AttendanceDate DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", Convert.ToInt32(Session["UserID"]));

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvAttendance.DataSource = dt;
                gvAttendance.DataBind();
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("StudentDashboard.aspx");
        }
    }
}
