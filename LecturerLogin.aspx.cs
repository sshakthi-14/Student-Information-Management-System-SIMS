using System;
using System.Data.SqlClient;

namespace SoftwareEngineeringProject
{
    public partial class Login : System.Web.UI.Page
    {
        // Add declarations for server controls referenced in code-behind.
        // These are normally generated in the designer file; adding them here fixes CS0103 when the designer is missing or not generated.
        protected global::System.Web.UI.WebControls.TextBox txtEmail;
        protected global::System.Web.UI.WebControls.TextBox txtPassword;
        protected global::System.Web.UI.WebControls.Label lblMessage;
        protected global::System.Web.UI.WebControls.Button btnLogin;

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();

            string connectionString =
                @"Server=(localdb)\MSSQLLocalDB;
                Database=CollegeManagementSystem;
                Trusted_Connection=True;";

            using (SqlConnection conn =
                   new SqlConnection(connectionString))
            {
                conn.Open();

                string query =
                @"SELECT UserID,
                         FullName,
                         RoleID
                  FROM Users
                  WHERE Email=@Email
                  AND PasswordHash=@Password";

                SqlCommand cmd =
                    new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue(
                    "@Email", email);

                cmd.Parameters.AddWithValue(
                    "@Password", password);

                SqlDataReader reader =
                    cmd.ExecuteReader();

                if (reader.Read())
                {
                    int UserID =
                        Convert.ToInt32(reader["UserID"]);

                    int RoleID =
                        Convert.ToInt32(reader["RoleID"]);

                    Session["UserID"] = UserID;
                    Session["RoleID"] = RoleID;

                    switch (RoleID)
                    {
                        case 1:
                            Response.Redirect("AdminDashboard.aspx");
                            break;

                        case 2:
                            Response.Redirect("LecturerDashboard.aspx");
                            break;

                        case 3:
                            Response.Redirect("StudentDashboard.aspx");
                            break;

                        default:
                            lblMessage.Text =
                            "Invalid role configuration.";
                            break;
                    }
                }
            }
        }
    }
}
