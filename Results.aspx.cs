using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace SoftwareEngineeringProject
{
    public partial class Results : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["CollegeManagementSystem"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["RoleID"] == null || Convert.ToInt32(Session["RoleID"]) != 3)
                Response.Redirect("StudentLogin.aspx");

            if (!IsPostBack)
                LoadResults();
        }

        private void LoadResults()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"SELECT C.CourseCode, C.CourseName, R.Marks, R.Grade, R.GradePoint
                                 FROM Results R
                                 INNER JOIN Enrollments E ON R.EnrollmentID = E.EnrollmentID
                                 INNER JOIN Students S ON E.StudentID = S.StudentID
                                 INNER JOIN Courses C ON E.CourseID = C.CourseID
                                 WHERE S.UserID = @UserID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", Session["UserID"]);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvResults.DataSource = dt;
                gvResults.DataBind();
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("StudentDashboard.aspx");
        }
    }
}
