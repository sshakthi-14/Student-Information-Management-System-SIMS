using System;
using System.Data.SqlClient;
using System.Configuration;

namespace Student_Information_Management_System_SIMS
{
    public partial class StudentLogin : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["CollegeManagementSystem"].ConnectionString;

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT UserID, RoleID, PasswordHash FROM Users WHERE Email = @Email AND RoleID = 3";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    string storedHash = dr["PasswordHash"].ToString();
                    if (BCrypt.Net.BCrypt.Verify(txtPassword.Text, storedHash))
                    {
                        Session["UserID"] = dr["UserID"];
                        Session["RoleID"] = dr["RoleID"];
                        Response.Redirect("StudentDashboard.aspx");
                    }
                    else
                    {
                        lblMessage.Text = "Invalid login.";
                    }
                }
                else
                {
                    lblMessage.Text = "Invalid login.";
                }
            }
        }
    }
}
