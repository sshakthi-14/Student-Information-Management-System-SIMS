using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoftwareEngineeringProject
{
    public partial class MarksReport : System.Web.UI.Page
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
                LoadStudents();
                if (ddlStudent.Items.Count > 0)
                {
                    GenerateAcademicReport();
                }
            }
        }

        private void LoadStudents()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT S.StudentID, U.FullName FROM Students S INNER JOIN Users U ON S.UserID = U.UserID";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                conn.Open();
                da.Fill(dt);

                ddlStudent.DataSource = dt;
                ddlStudent.DataTextField = "FullName";
                ddlStudent.DataValueField = "StudentID";
                ddlStudent.DataBind();
            }
        }

        protected void ddlStudent_SelectedIndexChanged(object sender, EventArgs e)
        {
            GenerateAcademicReport();
        }

        private void GenerateAcademicReport()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string studentQuery = @"
                    SELECT U.FullName, P.ProgrammeName, E.Semester, S.StudentID 
                    FROM Students S
                    INNER JOIN Users U ON S.UserID = U.UserID
                    LEFT JOIN Programmes P ON S.ProgrammeID = P.ProgrammeID
                    LEFT JOIN Enrollments E ON S.StudentID = E.StudentID
                    WHERE S.StudentID = @StudentID";

                SqlCommand cmdProfile = new SqlCommand(studentQuery, conn);
                cmdProfile.Parameters.AddWithValue("@StudentID", Convert.ToInt32(ddlStudent.SelectedValue));

                conn.Open();
                SqlDataReader dr = cmdProfile.ExecuteReader();
                if (dr.Read())
                {
                    pnlResultSlip.Visible = true;
                    lblNoRecords.Text = "";
                    btnPDF.Visible = true;

                    lblStudentName.Text = dr["FullName"].ToString();
                    lblProgramme.Text = dr["ProgrammeName"].ToString() ?? "General";
                    lblStudentID.Text = dr["StudentID"].ToString();
                    lblSemester.Text = dr["Semester"].ToString() != "" ? dr["Semester"].ToString() : "Current Session";
                }
                else
                {
                    pnlResultSlip.Visible = false;
                    btnPDF.Visible = false;
                    lblNoRecords.Text = "Student Profile missing registration values.";
                    return;
                }
                conn.Close();

                string performanceQuery = @"
                    SELECT C.CourseCode AS [CODE], C.CourseName AS [COURSE DESCRIPTION], C.CreditHours AS [Cr. HOURS], ISNULL(R.Grade, 'F') AS [GRADE], ISNULL(R.GradePoint, 0.00) AS [GRADE_POINT], (C.CreditHours * ISNULL(R.GradePoint, 0.00)) AS [Cr. PT.]
                    
                    FROM Enrollments E

                    INNER JOIN Courses C ON E.CourseID = C.CourseID

                    LEFT JOIN Results R ON E.EnrollmentID = R.EnrollmentID
                    WHERE E.StudentID = @StudentID";

                SqlCommand cmdMarks = new SqlCommand(performanceQuery, conn);
                cmdMarks.Parameters.AddWithValue("@StudentID", Convert.ToInt32(ddlStudent.SelectedValue));

                SqlDataAdapter da = new SqlDataAdapter(cmdMarks);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    gvMarks.Visible = false;
                    lblNoRecords.Text = "No course results found for this student.";
                    btnPDF.Visible = false;
                    return;
                }

                gvMarks.Visible = true;
                gvMarks.DataSource = dt;
                gvMarks.DataBind();

                double hoursEarned = 0;
                double totalPointsEarned = 0;

                foreach (DataRow row in dt.Rows)
                {
                    double currentHours = Convert.ToDouble(row["Cr. HOURS"]);
                    double currentGradePoint = Convert.ToDouble(row["GRADE_POINT"]);

                    totalPointsEarned += (currentHours * currentGradePoint);

                    if (row["GRADE"].ToString() != "F")
                    {
                        hoursEarned += currentHours;
                    }
                }

                double computedGPA = hoursEarned > 0 ? (totalPointsEarned / hoursEarned) : 0.00;

                lblCrHoursEarned.Text = hoursEarned.ToString();
                lblGPA.Text = computedGPA.ToString("F2");
                lblCumCrHours.Text = hoursEarned.ToString();
                lblCGPA.Text = computedGPA.ToString("F2");

                pnlProbation.Visible = (computedGPA < 2.00);

                ViewState["CurrentSlipData"] = dt;
            }
        }

        protected void btnPDF_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["CurrentSlipData"];
            if (dt == null || dt.Rows.Count == 0) return;

            Document pdfDoc = new Document(PageSize.A4, 30f, 30f, 40f, 40f);
            MemoryStream ms = new MemoryStream();
            PdfWriter.GetInstance(pdfDoc, ms);
            pdfDoc.Open();

            iTextSharp.text.Font titleFont = FontFactory.GetFont("Arial", 22, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font alertFont = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD, new BaseColor(204, 0, 0));
            iTextSharp.text.Font regularFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL);
            iTextSharp.text.Font boldFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD);

            pdfDoc.Add(new Paragraph("Academic Performance Report", titleFont));
            pdfDoc.Add(new Paragraph(" "));

            PdfPTable metaTable = new PdfPTable(2);
            metaTable.WidthPercentage = 100;
            metaTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            metaTable.SetWidths(new float[] { 1f, 1f });

            metaTable.AddCell(new Phrase($"Name:  {lblStudentName.Text}", regularFont));
            PdfPCell progCell = new PdfPCell(new Phrase($"Programme:  {lblProgramme.Text}", regularFont)) { HorizontalAlignment = Element.ALIGN_RIGHT, Border = iTextSharp.text.Rectangle.NO_BORDER };
            metaTable.AddCell(progCell);

            metaTable.AddCell(new Phrase($"Student ID:  {lblStudentID.Text}", regularFont));
            PdfPCell semCell = new PdfPCell(new Phrase($"Session/Semester:  {lblSemester.Text}", regularFont)) { HorizontalAlignment = Element.ALIGN_RIGHT, Border = iTextSharp.text.Rectangle.NO_BORDER };
            metaTable.AddCell(semCell);

            pdfDoc.Add(metaTable);

            pdfDoc.Add(new Paragraph("______________________________________________________________________________"));
            pdfDoc.Add(new Paragraph(" "));

            if (pnlProbation.Visible)
            {
                pdfDoc.Add(new Paragraph("ACADEMIC PROBATION", alertFont));
                pdfDoc.Add(new Paragraph(" "));
            }

            PdfPTable reportGrid = new PdfPTable(5);
            reportGrid.WidthPercentage = 100;
            reportGrid.SetWidths(new float[] { 1.2f, 3.5f, 1f, 1f, 1f });

            string[] headers = { "CODE", "COURSE DESCRIPTION", "Cr. HOURS", "GRADE", "Cr. PT." };
            foreach (string headerText in headers)
            {
                PdfPCell cell = new PdfPCell(new Phrase(headerText, boldFont));
                cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                cell.BorderWidthBottom = 1.5f;
                cell.PaddingBottom = 8f;
                reportGrid.AddCell(cell);
            }

            foreach (DataRow row in dt.Rows)
            {
                reportGrid.AddCell(new PdfPCell(new Phrase(row["CODE"].ToString(), regularFont)) { Border = iTextSharp.text.Rectangle.BOTTOM_BORDER, PaddingBottom = 6f });
                reportGrid.AddCell(new PdfPCell(new Phrase(row["COURSE DESCRIPTION"].ToString(), regularFont)) { Border = iTextSharp.text.Rectangle.BOTTOM_BORDER, PaddingBottom = 6f });
                reportGrid.AddCell(new PdfPCell(new Phrase(row["Cr. HOURS"].ToString(), regularFont)) { Border = iTextSharp.text.Rectangle.BOTTOM_BORDER, PaddingBottom = 6f });
                reportGrid.AddCell(new PdfPCell(new Phrase(row["GRADE"].ToString(), regularFont)) { Border = iTextSharp.text.Rectangle.BOTTOM_BORDER, PaddingBottom = 6f });

                double pointVal = Convert.ToDouble(row["Cr. PT."]);
                reportGrid.AddCell(new PdfPCell(new Phrase(pointVal.ToString("F2"), regularFont)) { Border = iTextSharp.text.Rectangle.BOTTOM_BORDER, PaddingBottom = 6f });
            }

            pdfDoc.Add(reportGrid);
            pdfDoc.Add(new Paragraph(" "));

            pdfDoc.Add(new Paragraph($"CREDIT HOURS EARNED = {lblCrHoursEarned.Text}", boldFont));
            pdfDoc.Add(new Paragraph($"GRADE POINT AVERAGE (GPA) = {lblGPA.Text}", boldFont));
            pdfDoc.Add(new Paragraph($"CUMULATIVE CREDIT HOURS EARNED = {lblCumCrHours.Text}", boldFont));
            pdfDoc.Add(new Paragraph($"CUMULATIVE GRADE POINT AVERAGE (CGPA) = {lblCGPA.Text}", boldFont));
            
            pdfDoc.Close();

            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", $"attachment;filename=Official_Result_Slip_{lblStudentID.Text}.pdf");
            Response.BinaryWrite(ms.ToArray());
            Response.End();
        }
    }
}
