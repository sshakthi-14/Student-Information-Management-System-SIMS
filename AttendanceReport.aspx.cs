using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace Student_Information_Management_System_SIMS
{
    public partial class AttendanceReport : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["CollegeManagementSystem"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["RoleID"] == null || Convert.ToInt32(Session["RoleID"]) != 2)
            {
                Response.Redirect("LecturerLogin.aspx");
            }

            if (!IsPostBack)
            {
                if (Session["SelectedCourseID"] == null)
                {
                    Response.Redirect("LecturerDashboard.aspx");
                }

                LoadCourseHeader();
                LoadAttendance();
            }
        }

        private void LoadCourseHeader()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT CourseCode, CourseName FROM Courses WHERE CourseID = @CourseID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CourseID", Convert.ToInt32(Session["SelectedCourseID"]));

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    lblCourse.Text = dr["CourseCode"] + " : " + dr["CourseName"];
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadAttendance();
        }

        private void LoadAttendance()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT S.StudentID AS [Student ID], U.FullName AS [Student Name], U.Email AS [Email],
                
                        ISNULL(A.Status, 'N/A') AS [Status on Date],

                        (SELECT COUNT(*) 
                         FROM Attendance A2 
                         INNER JOIN Enrollments E2 ON A2.EnrollmentID = E2.EnrollmentID
                         WHERE E2.StudentID = S.StudentID 
                           AND E2.CourseID = @CourseID 
                           AND A2.Status = 'Absent') AS [Total Absences]

                    FROM Enrollments E
                    INNER JOIN Students S ON E.StudentID = S.StudentID
                    INNER JOIN Users U ON S.UserID = U.UserID
            
                    LEFT JOIN Attendance A ON E.EnrollmentID = A.EnrollmentID ";

                if (!string.IsNullOrEmpty(txtDate.Text))
                {
                    query += " AND A.AttendanceDate = @Date ";
                }
                else
                {
                    query += " AND 1 = 0 ";
                }

                query += @"GROUP BY S.StudentID, U.FullName, U.Email, A.Status ORDER BY [Total Absences] DESC, U.FullName ASC";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@CourseID", Convert.ToInt32(Session["SelectedCourseID"]));

                if (!string.IsNullOrEmpty(txtDate.Text))
                {
                    cmd.Parameters.AddWithValue("@Date", txtDate.Text);
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    lblNoStudents.Text = "No records found.";
                    gvAttendance.Visible = false;
                    btnPDF.Visible = false;
                }
                else
                {
                    lblNoStudents.Text = "";
                    gvAttendance.Visible = true;
                    btnPDF.Visible = true;
                    gvAttendance.DataSource = dt;
                    gvAttendance.DataBind();
                }

                ViewState["AttendanceData"] = dt;
            }
        }

        protected void gvAttendance_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int totalAbsences = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Total Absences"));

                Button btnWarning = (Button)e.Row.FindControl("btnWarning");

                if (totalAbsences >= 4)
                {
                    e.Row.BackColor = System.Drawing.Color.Gray;
                    e.Row.ForeColor = System.Drawing.Color.Black;

                    if (btnWarning != null)
                    {
                        btnWarning.Visible = true; 
                    }
                }
                else
                {
                    if (btnWarning != null)
                    {
                        btnWarning.Visible = false;
                    }
                }
            }
        }

        protected void btnWarning_Click(object sender, EventArgs e)
        {
            // Handle warning logic here (e.g., sending emails or saving system alerts)
        }

        protected void btnPDF_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["AttendanceData"];
            if (dt == null || dt.Rows.Count == 0) return;

            int totalStudents = dt.Rows.Count;
            int criticalStudents = 0;
            int safeStudents = 0;

            foreach (DataRow row in dt.Rows)
            {
                if (Convert.ToInt32(row["Total Absences"]) >= 4)
                    criticalStudents++;
                else
                    safeStudents++;
            }

            float safePercentage = (float)safeStudents / totalStudents * 100f;
            float criticalPercentage = (float)criticalStudents / totalStudents * 100f;

            Document pdfDoc = new Document(PageSize.A4, 25f, 25f, 30f, 30f);
            MemoryStream ms = new MemoryStream();
            PdfWriter.GetInstance(pdfDoc, ms);
            pdfDoc.Open();

            iTextSharp.text.Font titleFont = FontFactory.GetFont("Arial", 20, iTextSharp.text.Font.BOLD, new BaseColor(4, 9, 71));
            iTextSharp.text.Font sectionFont = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font metaFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.ITALIC);
            iTextSharp.text.Font thFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font tdFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL);

            pdfDoc.Add(new Paragraph("Attendance Summary Report", titleFont));
            pdfDoc.Add(new Paragraph("Course: " + lblCourse.Text, tdFont));
            pdfDoc.Add(new Paragraph("Generated On: " + DateTime.Now.ToString("F"), metaFont));
            pdfDoc.Add(new Paragraph(" ")); 

            PdfPTable summaryTable = new PdfPTable(3);
            summaryTable.WidthPercentage = 100;
            summaryTable.SetWidths(new float[] { 1f, 1f, 1f });

            summaryTable.AddCell(CreateSummaryCell("Total Enrolled Students", totalStudents.ToString(), new BaseColor(240, 240, 240)));
            summaryTable.AddCell(CreateSummaryCell("Safe Attendance (< 4)", safeStudents.ToString(), new BaseColor(220, 245, 220)));
            summaryTable.AddCell(CreateSummaryCell("Critical Attendance (>= 4)", criticalStudents.ToString(), new BaseColor(255, 210, 210)));

            pdfDoc.Add(summaryTable);
            pdfDoc.Add(new Paragraph(" "));

            // 4. Visual Progress Bar Chart (Native iTextSharp alternative)
            pdfDoc.Add(new Paragraph("Cohort Health Distribution:", sectionFont));
            pdfDoc.Add(new Paragraph(" "));

            // Fallback protection for single data scenarios
            float displaySafeWidth = safePercentage == 0 ? 0.01f : safePercentage;
            float displayCritWidth = criticalPercentage == 0 ? 0.01f : criticalPercentage;

            if (safeStudents == 0 && criticalStudents > 0) { displayCritWidth = 100f; displaySafeWidth = 0f; }
            if (criticalStudents == 0 && safeStudents > 0) { displaySafeWidth = 100f; displayCritWidth = 0f; }

            // Determine how many columns we actually need to display (1 or 2)
            int columnCount = (safeStudents > 0 ? 1 : 0) + (criticalStudents > 0 ? 1 : 0);

            if (columnCount > 0)
            {
                PdfPTable chartBarTable = new PdfPTable(columnCount);
                chartBarTable.WidthPercentage = 100;

                // Build structural width layout cleanly based on what contains data
                if (columnCount == 2)
                {
                    chartBarTable.SetWidths(new float[] { displaySafeWidth, displayCritWidth });
                }

                if (safeStudents > 0)
                {
                    PdfPCell safeCell = new PdfPCell(new Phrase($"Safe: {safePercentage:0.}%", thFont));
                    safeCell.BackgroundColor = new BaseColor(144, 238, 144); // Light Green
                    safeCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    safeCell.Padding = 12f;
                    chartBarTable.AddCell(safeCell);
                }
                if (criticalStudents > 0)
                {
                    PdfPCell critCell = new PdfPCell(new Phrase($"Warning: {criticalPercentage:0.}%", thFont));
                    critCell.BackgroundColor = new BaseColor(255, 127, 127); // Light Coral Red
                    critCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    critCell.Padding = 12f;
                    chartBarTable.AddCell(critCell);
                }

                pdfDoc.Add(chartBarTable);
            }
            else
            {
                // Edge-case handle if no records match at all
                pdfDoc.Add(new Paragraph("No data available to plot chart distribution.", tdFont));
            }

            pdfDoc.Add(new Paragraph(" "));
            pdfDoc.Add(new Paragraph("Detailed Student Breakdown List:", sectionFont));
            pdfDoc.Add(new Paragraph(" "));

            PdfPTable table = new PdfPTable(dt.Columns.Count);
            table.WidthPercentage = 100;

            foreach (DataColumn col in dt.Columns)
            {
                PdfPCell cell = new PdfPCell(new Phrase(col.ColumnName, thFont));
                cell.BackgroundColor = new BaseColor(183, 214, 131); 
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Padding = 8f;
                table.AddCell(cell);
            }

            foreach (DataRow row in dt.Rows)
            {
                int totalAbsences = Convert.ToInt32(row["Total Absences"]);

                foreach (var item in row.ItemArray)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(item.ToString(), tdFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Padding = 6f;

                    if (totalAbsences >= 4)
                    {
                        cell.BackgroundColor = new BaseColor(255, 182, 193); 
                    }

                    table.AddCell(cell);
                }
            }

            pdfDoc.Add(table);
            pdfDoc.Close();

            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=DetailedAttendanceSummary_" + DateTime.Now.ToString("yyyyMMdd") + ".pdf");
            Response.BinaryWrite(ms.ToArray());
            Response.End();
        }
        private PdfPCell CreateSummaryCell(string header, string value, BaseColor bgColor)
        {
            iTextSharp.text.Font lblFont = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL, BaseColor.DARK_GRAY);
            iTextSharp.text.Font valFont = FontFactory.GetFont("Arial", 16, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

            PdfPCell cell = new PdfPCell();
            cell.BackgroundColor = bgColor;
            cell.Padding = 10f;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.BorderWidth = 1f;
            cell.BorderColor = new BaseColor(210, 210, 210);

            Paragraph p1 = new Paragraph(header, lblFont);
            p1.Alignment = Element.ALIGN_CENTER;
            cell.AddElement(p1);

            Paragraph p2 = new Paragraph(value, valFont);
            p2.Alignment = Element.ALIGN_CENTER;
            cell.AddElement(p2);

            return cell;
        }
    }
}
