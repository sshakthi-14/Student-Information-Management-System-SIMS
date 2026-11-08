using System;
using System.Data;
using System.Data.SqlClient;

namespace Student_Information_Management_System_SIMS
{
    public partial class StudentLogin : System.Web.UI.Page
    {
        private readonly string connStr = @"Server=(localdb)\MSSQLLocalDB;Database=CollegeManagementSystem;Integrated Security=True;";

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                lblMessage.Text = "Please enter your email and password.";
                return;
            }

            object userId;
            object roleId;
            string storedHash;
            bool passwordIsValid = false;
            bool upgradePlaintextPassword = false;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                const string query = @"
                    SELECT UserID, RoleID, PasswordHash
                    FROM Users
                    WHERE Email = @Email
                      AND RoleID = 3";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 256)
                                  .Value = email;

                    conn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (!dr.Read())
                        {
                            lblMessage.Text = "Invalid login.";
                            return;
                        }

                        userId = dr["UserID"];
                        roleId = dr["RoleID"];
                        storedHash = Convert.ToString(dr["PasswordHash"]);

                        if (storedHash.StartsWith("$2",
                            StringComparison.Ordinal))
                        {
                            passwordIsValid =
                                BCrypt.Net.BCrypt.Verify(password, storedHash);
                        }
                        else
                        {
                            // Temporary support for the existing plaintext record.
                            passwordIsValid =
                                string.Equals(
                                    password,
                                    storedHash,
                                    StringComparison.Ordinal);

                            upgradePlaintextPassword = passwordIsValid;
                        }
                    }

                    if (!passwordIsValid)
                    {
                        lblMessage.Text = "Invalid login.";
                        return;
                    }

                    // Replace the plaintext password with a BCrypt hash.
                    if (upgradePlaintextPassword)
                    {
                        string newPasswordHash =
                            BCrypt.Net.BCrypt.HashPassword(password);

                        const string updateQuery = @"
                            UPDATE Users
                            SET PasswordHash = @PasswordHash
                            WHERE UserID = @UserID";

                        using (SqlCommand updateCommand =
                               new SqlCommand(updateQuery, conn))
                        {
                            updateCommand.Parameters.Add(
                                "@PasswordHash",
                                SqlDbType.NVarChar,
                                100).Value = newPasswordHash;

                            updateCommand.Parameters.Add(
                                "@UserID",
                                SqlDbType.Int).Value = userId;

                            updateCommand.ExecuteNonQuery();
                        }
                    }
                }
            }

            Session["UserID"] = userId;
            Session["RoleID"] = roleId;

            Response.Redirect("StudentDashboard.aspx");
        }
    }
}
