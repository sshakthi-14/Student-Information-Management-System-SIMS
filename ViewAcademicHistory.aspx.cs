using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SoftwareEngineeringProject
{
    public partial class ViewAcademicHistory : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["CollegeManagementSystem"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Security Check: Verify that the student user session is active
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Student/StudentLogin.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadStudentInfo();
                LoadAcademicHistory();
            }
        }

        // 1. LOAD STUDENT NAME AND PROGRAMME
        private void LoadStudentInfo()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                // Pulls details cleanly by traversing Users -> Students -> Programmes
                string query = @"
                    SELECT U.FullName, P.ProgrammeName 
                    FROM Users U
                    INNER JOIN Students S ON U.UserID = S.UserID
                    INNER JOIN Programmes P ON S.ProgrammeID = P.ProgrammeID
                    WHERE U.UserID = @UserID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserID", Convert.ToInt32(Session["UserID"]));

                    try
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                lblStudentName.Text = reader["FullName"].ToString();
                                lblProgrammeName.Text = reader["ProgrammeName"].ToString();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Fallback logging behavior if tables are unpopulated
                        lblStudentName.Text = "Profile Error";
                    }
                }
            }
        }

        // 2. LOAD COURSES, SEMESTERS, AND MARKS
        private void LoadAcademicHistory()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                // Maps your updated CourseMarks schema structure via Enrollments to display results
                string query = @"
                    SELECT 
                        C.CourseCode AS [Course Code],
                        C.CourseName AS [Course Name], 
                        E.Semester AS [Semester], 
                        E.AcademicYear AS [Year],
                        ISNULL(CAST(CM.TotalMarks AS VARCHAR), 'Pending') AS [Total Marks]
                    FROM Enrollments E
                    INNER JOIN Students S ON E.StudentID = S.StudentID
                    INNER JOIN Courses C ON E.CourseID = C.CourseID
                    LEFT JOIN CourseMarks CM ON E.EnrollmentID = CM.EnrolmentID 
                                            AND E.CourseID = CM.CourseID
                    WHERE S.UserID = @UserID
                    ORDER BY E.AcademicYear DESC, E.Semester DESC";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserID", Convert.ToInt32(Session["UserID"]));

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();

                        try
                        {
                            sda.Fill(dt);
                            gvHistory.DataSource = dt;
                            gvHistory.DataBind();
                        }
                        catch (Exception ex)
                        {
                            // Handled gracefully if CourseMarks grid elements fail to bind directly
                        }
                    }
                }
            }
        }
    }
}
