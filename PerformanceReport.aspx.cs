using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Student_Information_Management_System_SIMS
{
    public partial class PerformanceReport : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["CollegeManagementSystem"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // 1. Security Check: Verify user session context as an active student
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Student/StudentLogin.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadHeaderInfo();
                GenerateReport();
            }
        }

        private void LoadHeaderInfo()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                // Updated to fetch identity data cleanly through Users -> Students -> Programmes -> Enrollments
                string query = @"
                    SELECT TOP 1 
                        U.FullName, 
                        P.ProgrammeName, 
                        E.Semester, 
                        E.AcademicYear
                    FROM Users U
                    INNER JOIN Students S ON U.UserID = S.UserID
                    INNER JOIN Programmes P ON S.ProgrammeID = P.ProgrammeID
                    LEFT JOIN Enrollments E ON S.StudentID = E.StudentID
                    WHERE U.UserID = @UserID
                    ORDER BY E.AcademicYear DESC, E.Semester DESC";

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
                                lblName.Text = reader["FullName"].ToString();
                                lblProgramme.Text = reader["ProgrammeName"].ToString();
                                lblSession.Text = reader["AcademicYear"].ToString() + " / " + reader["Semester"].ToString();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lblName.Text = "Error loading profile data.";
                    }
                }
            }
        }

        private void GenerateReport()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                // Pulls enrollment records and safely maps marks via LEFT JOIN
                string query = @"
                    SELECT 
                        C.CourseCode, 
                        C.CourseName, 
                        C.CreditHours, 
                        CM.TotalMarks
                    FROM Enrollments E
                    INNER JOIN Students S ON E.StudentID = S.StudentID
                    INNER JOIN Courses C ON E.CourseID = C.CourseID
                    LEFT JOIN CourseMarks CM ON E.EnrollmentID = CM.EnrolmentID 
                                            AND E.CourseID = CM.CourseID
                    WHERE S.UserID = @UserID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserID", Convert.ToInt32(Session["UserID"]));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        dt.Columns.Add("CalculatedGrade", typeof(string));
                        dt.Columns.Add("CalculatedCrPt", typeof(decimal));

                        int totalCreditHours = 0;
                        decimal totalCreditPoints = 0;

                        foreach (DataRow row in dt.Rows)
                        {
                            int creditHour = Convert.ToInt32(row["CreditHours"]);

                            // Improved Safe-check: Use Convert.ToDecimal first to safely read decimal(5,2) from SQL Server
                            if (row["TotalMarks"] != DBNull.Value)
                            {
                                // Round or cast the decimal value to an integer for your grade conversion function
                                int mark = Convert.ToInt32(Convert.ToDecimal(row["TotalMarks"]));

                                var gradeInfo = ConvertMarkToGrade(mark);
                                decimal crPt = creditHour * gradeInfo.GradePoint;

                                row["CalculatedGrade"] = gradeInfo.Grade;
                                row["CalculatedCrPt"] = crPt;

                                totalCreditHours += creditHour;
                                totalCreditPoints += crPt;
                            }
                            else
                            {
                                // Fallback for active courses that haven't been graded yet
                                row["CalculatedGrade"] = "Pending";
                                row["CalculatedCrPt"] = 0.00m;
                            }
                        }

                        rptCourses.DataSource = dt;
                        rptCourses.DataBind();

                        decimal termGPA = 0;
                        if (totalCreditHours > 0)
                        {
                            termGPA = totalCreditPoints / totalCreditHours;
                        }

                        lblTermCredits.Text = totalCreditHours.ToString();
                        lblTermGPA.Text = termGPA.ToString("F2");

                        if (termGPA >= 2.00m)
                        {
                            lblAcademicStanding.Text = "GOOD STANDING";
                            lblAcademicStanding.ForeColor = System.Drawing.Color.Green;
                        }
                        else
                        {
                            lblAcademicStanding.Text = "ACADEMIC PROBATION";
                            lblAcademicStanding.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                }
            }
        }

        private (string Grade, decimal GradePoint) ConvertMarkToGrade(int totalMark)
        {
            if (totalMark >= 90) return ("A+", 4.00m);
            if (totalMark >= 80) return ("A", 4.00m);
            if (totalMark >= 75) return ("A-", 3.67m);
            if (totalMark >= 70) return ("B+", 3.33m);
            if (totalMark >= 65) return ("B", 3.00m);
            if (totalMark >= 60) return ("B-", 2.67m);
            if (totalMark >= 55) return ("C+", 2.33m);
            if (totalMark >= 50) return ("C", 2.00m);
            if (totalMark >= 45) return ("C-", 1.50m);
            if (totalMark >= 40) return ("D", 1.00m);
            return ("F", 0.00m);
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("~/Student/StudentLogin.aspx");
        }
    }
}
