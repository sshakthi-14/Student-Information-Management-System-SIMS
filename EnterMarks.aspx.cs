using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Student_Information_Management_System_SIMS
{
    public partial class EnterMarks : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["CollegeManagementSystem"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["SelectedCourseID"] == null)
                {
                    Response.Redirect("LecturerDashboard.aspx");
                }

                LoadCourseInfo();
                LoadStudents();
            }
        }

        private void LoadCourseInfo()
        {
            using (SqlConnection conn =
                new SqlConnection(connStr))
            {
                string query = @"SELECT CourseCode, CourseName FROM Courses WHERE CourseID = @CourseID";

                SqlCommand cmd =
                    new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@CourseID", Convert.ToInt32(Session["SelectedCourseID"]));

                conn.Open();

                SqlDataReader dr =
                    cmd.ExecuteReader();

                if (dr.Read())
                {
                    lblCourse.Text = dr["CourseCode"] + " : " + dr["CourseName"];
                }
            }
        }

        private void LoadStudents()
        {
            using (SqlConnection conn =
                new SqlConnection(connStr))
            {
                string query =
                @"SELECT E.EnrollmentID, S.StudentID, U.FullName, R.AssignmentMarks, R.MidTestMarks, R.FinalExamMarks
                
                FROM Enrollments E
                
                LEFT JOIN Results R
                ON E.EnrollmentID = R.EnrollmentID

                INNER JOIN Students S
                ON E.StudentID = S.StudentID

                INNER JOIN Users U
                ON S.UserID = U.UserID

                WHERE E.CourseID = @CourseID";

                SqlCommand cmd =
                    new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@CourseID", Convert.ToInt32(Session["SelectedCourseID"]));

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();

                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    lblMessage.Text =
                        "No students enrolled in this course.";

                    gvStudents.Visible = false;
                    return;
                }

                gvStudents.Visible = true;

                gvStudents.DataSource = dt;
                gvStudents.DataBind();
            }
        }

        protected void gvStudents_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewAssignments")
            {
                Session["SelectedEnrollmentID"] = Convert.ToInt32(e.CommandArgument);

                Response.Redirect("StudentAssignments.aspx");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                foreach (GridViewRow row in gvStudents.Rows)
                {
                    int enrollmentID = Convert.ToInt32(gvStudents.DataKeys[row.RowIndex].Value);

                    decimal assignment = 0;
                    string assignmentQuery =
                        @"SELECT ISNULL(SUM(Marks),0) 
                FROM AssignmentSubmissions 
                WHERE EnrollmentID = @EnrollmentID";

                    SqlCommand assignmentCmd =
                        new SqlCommand(assignmentQuery, conn);

                    assignmentCmd.Parameters.AddWithValue("@EnrollmentID", enrollmentID);
                    assignment = Convert.ToDecimal(assignmentCmd.ExecuteScalar());

                    decimal midTest = 0;
                    TextBox txtMid = (TextBox)row.FindControl("txtMidTest");

                    if (!string.IsNullOrWhiteSpace(txtMid.Text))
                    {
                        if (!decimal.TryParse(txtMid.Text, out midTest) || midTest < 0 || midTest > 100)
                        {
                            lblMessage.Text = "Mid Test must be a number between 0 and 100 (row " + (row.RowIndex + 1) + ").";
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                    }

                    decimal finalExam = 0;
                    TextBox txtFinal = (TextBox)row.FindControl("txtFinalExam");

                    if (!string.IsNullOrWhiteSpace(txtFinal.Text))
                    {
                        if (!decimal.TryParse(txtFinal.Text, out finalExam) || finalExam < 0 || finalExam > 100)
                        {
                            lblMessage.Text = "Final Exam must be a number between 0 and 100 (row " + (row.RowIndex + 1) + ").";
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                    }

                    decimal weightedAssignment = (assignment / 100m) * 20m;
                    decimal weightedMidTest = (midTest / 100m) * 30m;
                    decimal weightedFinalExam = (finalExam / 100m) * 50m;
                    decimal totalMarks = weightedAssignment + weightedMidTest + weightedFinalExam;

                    string grade;
                    decimal gradePoint;

                    if (totalMarks >= 80)
                    {
                        grade = "A";
                        gradePoint = 4.00m;
                    }
                    else if (totalMarks >= 70)
                    {
                        grade = "B";
                        gradePoint = 3.00m;
                    }
                    else if (totalMarks >= 60)
                    {
                        grade = "C";
                        gradePoint = 2.00m;
                    }
                    else if (totalMarks >= 50)
                    {
                        grade = "D";
                        gradePoint = 1.00m;
                    }
                    else
                    {
                        grade = "F";
                        gradePoint = 0.00m;
                    }

                    // Save or update the results in the database
                    string upsertQuery = @"
                IF EXISTS (SELECT 1 FROM Results WHERE EnrollmentID = @EnrollmentID)
                    UPDATE Results
                    SET AssignmentMarks = @AssignmentMarks,
                        MidTestMarks = @MidTestMarks,
                        FinalExamMarks = @FinalExamMarks,
                        TotalMarks = @TotalMarks,
                        Grade = @Grade,
                        GradePoint = @GradePoint
                    WHERE EnrollmentID = @EnrollmentID
                ELSE
                    INSERT INTO Results (EnrollmentID, AssignmentMarks, MidTestMarks, FinalExamMarks, TotalMarks, Grade, GradePoint)
                    VALUES (@EnrollmentID, @AssignmentMarks, @MidTestMarks, @FinalExamMarks, @TotalMarks, @Grade, @GradePoint)";

                    SqlCommand upsertCmd = new SqlCommand(upsertQuery, conn);
                    upsertCmd.Parameters.AddWithValue("@EnrollmentID", enrollmentID);
                    upsertCmd.Parameters.AddWithValue("@AssignmentMarks", assignment);
                    upsertCmd.Parameters.AddWithValue("@MidTestMarks", midTest);
                    upsertCmd.Parameters.AddWithValue("@FinalExamMarks", finalExam);
                    upsertCmd.Parameters.AddWithValue("@TotalMarks", totalMarks);
                    upsertCmd.Parameters.AddWithValue("@Grade", grade);
                    upsertCmd.Parameters.AddWithValue("@GradePoint", gradePoint);

                    upsertCmd.ExecuteNonQuery();
                }

                lblMessage.Text = "Marks saved successfully.";
                lblMessage.ForeColor = System.Drawing.Color.Green;
            }
        }
    }
}
