using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Student_Information_Management_System_SIMS
{
    public partial class ManageAttendance : System.Web.UI.Page
    {
        string connStr =
        ConfigurationManager.ConnectionStrings["CollegeManagementSystem"].ConnectionString;

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
                string query =
                @"SELECT CourseCode, CourseName FROM Courses WHERE CourseID=@CourseID";

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
                    lblCourse.Text =
                        dr["CourseCode"] + " : " +
                        dr["CourseName"];
                }
            }
        }

        private void LoadStudents()
        {
            using (SqlConnection conn =
                new SqlConnection(connStr))
            {
                string query =
                @"SELECT E.EnrollmentID, S.StudentID, U.FullName, U.Email

                FROM Enrollments E

                INNER JOIN Students S
                ON E.StudentID = S.StudentID

                INNER JOIN Users U
                ON S.UserID = U.UserID

                WHERE E.CourseID=@CourseID";

                SqlCommand cmd =
                    new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue(
                    "@CourseID",
                    Convert.ToInt32(Session["SelectedCourseID"]));

                SqlDataAdapter da =
                    new SqlDataAdapter(cmd);

                DataTable dt =
                    new DataTable();

                da.Fill(dt);

                lblStudentCount.Text = "Total Students: " + dt.Rows.Count;

                if (dt.Rows.Count == 0)
                {
                    lblNoStudents.Text =
                        "No students enrolled in this course.";

                    gvStudents.Visible = false;
                    btnSave.Visible = false;
                }
                else
                {
                    lblNoStudents.Text = "";

                    gvStudents.Visible = true;
                    btnSave.Visible = true;

                    gvStudents.DataSource = dt;
                    gvStudents.DataBind();
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (gvStudents.Rows.Count == 0)
            {
                lblMessage.Text =
                    "No students available for attendance.";

                return;
            }

            lblDateError.Text = "";

            if (string.IsNullOrWhiteSpace(txtDate.Text))
            {
                lblDateError.Text =
                    "Please select an attendance date.";

                return;
            }

            DateTime attendanceDate = Convert.ToDateTime(txtDate.Text);
            int currentCourseID = Convert.ToInt32(Session["SelectedCourseID"]);
            string courseName = lblCourse.Text;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                foreach (GridViewRow row in gvStudents.Rows)
                {
                    int enrollmentID = Convert.ToInt32(gvStudents.DataKeys[row.RowIndex].Value);

                    DropDownList ddl =
                    (DropDownList)row.FindControl(
                    "ddlStatus");

                    string status =
                    ddl.SelectedValue;

                    string query = @"
                    IF EXISTS
                    (
                        SELECT 1
                        FROM Attendance
                        WHERE EnrollmentID = @EnrollmentID
                        AND AttendanceDate = @Date
                    )
                    BEGIN
                        UPDATE Attendance
                        SET Status = @Status
                        WHERE EnrollmentID = @EnrollmentID
                        AND AttendanceDate = @Date
                    END
                    ELSE
                    BEGIN
                        INSERT INTO Attendance
                        (
                            EnrollmentID,
                            AttendanceDate,
                            Status
                        )
                        VALUES
                        (
                            @EnrollmentID,
                            @Date,
                            @Status
                        )
                    END";

                    SqlCommand cmd =
                    new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@EnrollmentID", enrollmentID);

                    cmd.Parameters.AddWithValue("@Date", attendanceDate);

                    cmd.Parameters.AddWithValue("@Status", status);

                    cmd.ExecuteNonQuery();
                    if (status == "Absent")
                    {
                        int activeStudentID = 0;
                        string studentLookupQuery = "SELECT StudentID FROM Enrollments WHERE EnrollmentID = @EnrollmentID";
                        using (SqlCommand lookupCmd = new SqlCommand(studentLookupQuery, con))
                        {
                            lookupCmd.Parameters.AddWithValue("@EnrollmentID", enrollmentID);
                            object result = lookupCmd.ExecuteScalar();
                            if (result != null)
                            {
                                activeStudentID = Convert.ToInt32(result);
                            }
                        }

                        if (activeStudentID > 0)
                        {
                            string countQuery = @"
                            SELECT COUNT(*) 
                            FROM Attendance A
                            INNER JOIN Enrollments E ON A.EnrollmentID = E.EnrollmentID
                            WHERE E.StudentID = @StudentID 
                            AND E.CourseID = @CourseID 
                            AND A.Status = 'Absent'";

                            int totalAbsences = 0;
                            using (SqlCommand countCmd = new SqlCommand(countQuery, con))
                            {
                                countCmd.Parameters.AddWithValue("@StudentID", activeStudentID);
                                countCmd.Parameters.AddWithValue("@CourseID", currentCourseID);
                                totalAbsences = Convert.ToInt32(countCmd.ExecuteScalar());
                            }

                            if (totalAbsences >= 4)
                            {
                                string warningMessage = $"Attendance Warning: You have accumulated {totalAbsences} absences for {courseName}. Your attendance is critically low.";

                                string alertCheckQuery = @"
                                SELECT COUNT(*) 
                                FROM StudentAlerts 
                                WHERE StudentID = @StudentID 
                                AND CourseID = @CourseID 
                                AND AlertType = 'Attendance Warning'
                                AND AlertMessage LIKE @MatchMessage";

                                int existingAlerts = 0;
                                using (SqlCommand checkCmd = new SqlCommand(alertCheckQuery, con))
                                {
                                    checkCmd.Parameters.AddWithValue("@StudentID", activeStudentID);
                                    checkCmd.Parameters.AddWithValue("@CourseID", currentCourseID);
                                    checkCmd.Parameters.AddWithValue("@MatchMessage", "%" + totalAbsences + " absences%");
                                    existingAlerts = Convert.ToInt32(checkCmd.ExecuteScalar());
                                }

                                if (existingAlerts == 0)
                                {
                                    string alertQuery = @"
                                    INSERT INTO StudentAlerts (StudentID, CourseID, AlertType, AlertMessage, IsRead, CreatedDate)
                                    VALUES (@StudentID, @CourseID, 'Attendance Warning', @AlertMessage, 0, @CreatedDate)";

                                    using (SqlCommand alertCmd = new SqlCommand(alertQuery, con))
                                    {
                                        alertCmd.Parameters.AddWithValue("@StudentID", activeStudentID);
                                        alertCmd.Parameters.AddWithValue("@CourseID", currentCourseID);
                                        alertCmd.Parameters.AddWithValue("@AlertMessage", warningMessage);
                                        alertCmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                                        alertCmd.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                    }
                }

                lblMessage.Text =
                "Attendance saved successfully.";
            }
        }
    }
}
