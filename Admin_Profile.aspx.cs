using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace Student_Information_Management_System__SIMS_.Admin
{
    public partial class Admin_Profile : Page
    {
        private string connStr = ConfigurationManager.ConnectionStrings["SIMSConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAdminProfile();
            }
        }

        private int GetCurrentAdminId()
        {
            if (Session["AdminId"] != null && int.TryParse(Session["AdminId"].ToString(), out int adminId))
            {
                return adminId;
            }
            return 1; // Default fallback for development preview
        }

        private void LoadAdminProfile()
        {
            int adminId = GetCurrentAdminId();
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                string sql = "SELECT AdminId, Username, FullName, Email, Phone FROM Admin WHERE AdminId = @AdminId";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@AdminId", adminId);
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        txtAdminId.Text = dr["AdminId"].ToString();
                        txtUsername.Text = dr["Username"].ToString();
                        txtFullName.Text = dr["FullName"].ToString();
                        txtEmail.Text = dr["Email"].ToString();
                        txtPhone.Text = dr["Phone"] == DBNull.Value ? "" : dr["Phone"].ToString();

                        string fullName = txtFullName.Text.Trim();
                        lblCardFullName.Text = fullName;
                        SetAvatarInitials(fullName);
                    }
                }
            }
        }

        private void SetAvatarInitials(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                string[] parts = name.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 2)
                    litAvatarInitials.Text = (parts[0][0].ToString() + parts[1][0].ToString()).ToUpper();
                else if (name.Length >= 2)
                    litAvatarInitials.Text = name.Substring(0, 2).ToUpper();
                else
                    litAvatarInitials.Text = "AD";
            }
        }

        protected void btnSaveProfile_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text) || string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                ClientScript.RegisterStartupScript(GetType(), "alert", "showMessageModal('Error', 'Full Name and Email fields are required.');", true);
                return;
            }

            int adminId = GetCurrentAdminId();
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                string sql = @"UPDATE Admin 
                               SET FullName = @FullName, Email = @Email, Phone = @Phone 
                               WHERE AdminId = @AdminId";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@FullName", txtFullName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@Phone", string.IsNullOrWhiteSpace(txtPhone.Text) ? (object)DBNull.Value : txtPhone.Text.Trim());
                    cmd.Parameters.AddWithValue("@AdminId", adminId);

                    cmd.ExecuteNonQuery();
                }
            }

            Session["AdminName"] = txtFullName.Text.Trim();
            lblCardFullName.Text = txtFullName.Text.Trim();
            SetAvatarInitials(txtFullName.Text.Trim());

            ClientScript.RegisterStartupScript(GetType(), "alert", "showMessageModal('Success', 'Profile details updated successfully!');", true);
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            string currentPwd = txtCurrentPassword.Text.Trim();
            string newPwd = txtNewPassword.Text.Trim();
            string confirmPwd = txtConfirmPassword.Text.Trim();

            if (string.IsNullOrEmpty(currentPwd) || string.IsNullOrEmpty(newPwd) || string.IsNullOrEmpty(confirmPwd))
            {
                ClientScript.RegisterStartupScript(GetType(), "alert", "showMessageModal('Error', 'Please fill in all password fields.');", true);
                return;
            }

            if (newPwd != confirmPwd)
            {
                ClientScript.RegisterStartupScript(GetType(), "alert", "showMessageModal('Error', 'New Password and Confirmation do not match.');", true);
                return;
            }

            int adminId = GetCurrentAdminId();
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                // Validate current password
                string verifySql = "SELECT COUNT(*) FROM Admin WHERE AdminId = @AdminId AND Password = @Password";
                using (SqlCommand verifyCmd = new SqlCommand(verifySql, con))
                {
                    verifyCmd.Parameters.AddWithValue("@AdminId", adminId);
                    verifyCmd.Parameters.AddWithValue("@Password", currentPwd);
                    int count = Convert.ToInt32(verifyCmd.ExecuteScalar());
                    if (count == 0)
                    {
                        ClientScript.RegisterStartupScript(GetType(), "alert", "showMessageModal('Error', 'Current password entered is incorrect.');", true);
                        return;
                    }
                }

                // Update password
                string updateSql = "UPDATE Admin SET Password = @Password WHERE AdminId = @AdminId";
                using (SqlCommand updateCmd = new SqlCommand(updateSql, con))
                {
                    updateCmd.Parameters.AddWithValue("@Password", newPwd);
                    updateCmd.Parameters.AddWithValue("@AdminId", adminId);
                    updateCmd.ExecuteNonQuery();
                }
            }

            txtCurrentPassword.Text = string.Empty;
            txtNewPassword.Text = string.Empty;
            txtConfirmPassword.Text = string.Empty;

            ClientScript.RegisterStartupScript(GetType(), "alert", "showMessageModal('Success', 'Security password changed successfully!');", true);
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("Admin_Dashboard.aspx");
        }
    }
}