using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Student_Information_Management_System_SIMS
{
    public partial class ViewAnnouncements : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["CollegeManagementSystem"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // 1. Security Check: Kick them back to login if session is dead
            if (Session["LecturerID"] == null)
            {
                Response.Redirect("LecturerLogin.aspx");
                return;
            }

            // 2. Load data only on the first page load
            if (!IsPostBack)
            {
                LoadAnnouncements();
            }
        }

        private void LoadAnnouncements()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                // Updated to use your new column names: Content and CreatedDate
                string query = @"
                    SELECT Title, Content, CreatedDate, AuthorName, AuthorRole 
                    FROM Announcements 
                    ORDER BY CreatedDate DESC";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();

                        try
                        {
                            con.Open();
                            da.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                lblMessage.Visible = false;
                                rptAnnouncements.DataSource = dt;
                                rptAnnouncements.DataBind();
                            }
                            else
                            {
                                lblMessage.Text = "No announcements to display right now.";
                                lblMessage.Visible = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            lblMessage.Text = "Error loading announcements: " + ex.Message;
                            lblMessage.Visible = true;
                        }
                    }
                }
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            // Return securely back to the dashboard layout context
            Response.Redirect("LecturerDashboard.aspx");
        }
    }
}
