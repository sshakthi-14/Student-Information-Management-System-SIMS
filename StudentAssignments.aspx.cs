using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SoftwareEngineeringProject
{
    public partial class StudentAssignments : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["CollegeManagementSystem"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["SelectedEnrollmentID"] == null)
                {
                    Response.Redirect(
                        "EnterMarks.aspx");
                }

                LoadStudentInfo();
                LoadAssignments();
            }
        }

        private void LoadStudentInfo()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query =
                @"SELECT S.StudentID, U.FullName, C.CourseCode, C.CourseName

                FROM Enrollments E

                INNER JOIN Students S
                ON E.StudentID = S.StudentID

                INNER JOIN Users U
                ON S.UserID = U.UserID

                INNER JOIN Courses C
                ON E.CourseID = C.CourseID

                WHERE E.EnrollmentID = @EnrollmentID";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@EnrollmentID", Convert.ToInt32(Session["SelectedEnrollmentID"]));

                conn.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    lblStudent.Text =
                        dr["StudentID"] + " - " +
                        dr["FullName"];

                    lblCourse.Text =
                        dr["CourseCode"] + " : " +
                        dr["CourseName"];
                }
            }
        }

        private void LoadAssignments()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query =
                @"SELECT S.SubmissionID, A.Title, S.FileName, S.FilePath, S.Marks

                FROM AssignmentSubmissions S

                INNER JOIN Assignments A
                ON S.AssignmentID = A.AssignmentID

                WHERE S.EnrollmentID = @EnrollmentID";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@EnrollmentID", Convert.ToInt32(Session["SelectedEnrollmentID"]));

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();

                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    lblMessage.Text =
                        "No assignment submissions found.";

                    gvAssignments.Visible = false;
                    btnSave.Visible = false;

                    return;
                }

                gvAssignments.Visible = true;
                btnSave.Visible = true;

                gvAssignments.DataSource = dt;
                gvAssignments.DataBind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                foreach (GridViewRow row in gvAssignments.Rows)
                {
                    int submissionID = Convert.ToInt32(gvAssignments.DataKeys[row.RowIndex].Value);

                    TextBox txtMarks = (TextBox)row.FindControl("txtMarks");

                    decimal marks = string.IsNullOrWhiteSpace(txtMarks.Text)
                    ? 0
                    : Convert.ToDecimal(txtMarks.Text);

                    string query =
                    @"UPDATE AssignmentSubmissions

                    SET Marks = @Marks

                    WHERE SubmissionID =
                    @SubmissionID";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@Marks", marks);

                    cmd.Parameters.AddWithValue("@SubmissionID", submissionID);

                    cmd.ExecuteNonQuery();
                }

                lblMessage.Text =
                "Assignment marks saved successfully.";
            }

            LoadAssignments();
        }
    }
}
