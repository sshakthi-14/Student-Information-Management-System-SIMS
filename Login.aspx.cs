using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using SIMS.Helpers;

/// <summary>
/// Code-behind for Login.aspx
///
/// Flow:
///   1. User submits email + password.
///   2. Look up the email in the Users table.
///   3. Verify the password hash using PasswordHelper.
///   4. Check the Role (1=Admin/HoP, 2=Lecturer, 3=Student).
///   5. Store session via SessionHelper.
///   6. Redirect to the appropriate dashboard.
/// </summary>
/// 

namespace Student_Information_Management_System_SIMS_
{
    public partial class Login : Page
    {
        // ---------------------------------------------------------------
        // Page_Load — redirect already-logged-in users away from Login.
        // ---------------------------------------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // If user is already authenticated, send them to their dashboard
                if (SessionHelper.IsLoggedIn(Session))
                    RedirectByRole(SessionHelper.GetRole(Session));
            }
        }

        // ---------------------------------------------------------------
        // btnLogin_Click — authenticate the user.
        // ---------------------------------------------------------------
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            // ASP.NET validators already checked required fields;
            // only reach here if the form is valid.
            if (!Page.IsValid)
                return;

            string email = txtEmail.Text.Trim().ToLower();
            string password = txtPassword.Text;   // plain text — compare against hash

            try
            {
                // ── 1. Fetch user record by email ──────────────────────
                string sql = @"
                    SELECT u.UserId, u.Email, u.PasswordHash, u.Role, u.IsActive
                    FROM   Users u
                    WHERE  LOWER(u.Email) = @Email";

                SqlParameter[] p = {
                    new SqlParameter("@Email", email)
                };

                DataTable dt = DatabaseHelper.ExecuteQuery(sql, p);

                // ── 2. Email not found ─────────────────────────────────
                if (dt.Rows.Count == 0)
                {
                    ShowError("Invalid email address or password. Please try again.");
                    return;
                }

                DataRow row = dt.Rows[0];
                bool isActive = Convert.ToBoolean(row["IsActive"]);
                string storedHash = row["PasswordHash"].ToString();
                int role = Convert.ToInt32(row["Role"]);
                int userId = Convert.ToInt32(row["UserId"]);

                // ── 3. Account disabled ────────────────────────────────
                if (!isActive)
                {
                    ShowError("Your account has been deactivated. Please contact the helpdesk.");
                    return;
                }

                // ── 4. Verify password hash ────────────────────────────
                if (!PasswordHelper.VerifyPassword(password, storedHash))
                {
                    ShowError("Invalid email address or password. Please try again.");
                    return;
                }

                // ── 5. Fetch profile details (name + profileId) ────────
                string fullName = "";
                string profileId = "";

                switch (role)
                {
                    case SessionHelper.ROLE_ADMIN:
                        var adminRow = FetchProfile(
                            "SELECT HoPId, FirstName + ' ' + LastName AS FullName FROM HoPDetails WHERE UserId = @Uid",
                            userId);
                        if (adminRow != null)
                        {
                            profileId = adminRow["HoPId"].ToString();
                            fullName = adminRow["FullName"].ToString();
                        }
                        break;

                    case SessionHelper.ROLE_LECTURER:
                        var lecRow = FetchProfile(
                            "SELECT LecturerId, FirstName + ' ' + LastName AS FullName FROM LecturerDetails WHERE UserId = @Uid",
                            userId);
                        if (lecRow != null)
                        {
                            profileId = lecRow["LecturerId"].ToString();
                            fullName = lecRow["FullName"].ToString();
                        }
                        break;

                    case SessionHelper.ROLE_STUDENT:
                        var stuRow = FetchProfile(
                            "SELECT StudentId, FirstName + ' ' + LastName AS FullName FROM StudentDetails WHERE UserId = @Uid",
                            userId);
                        if (stuRow != null)
                        {
                            profileId = stuRow["StudentId"].ToString();
                            fullName = stuRow["FullName"].ToString();
                        }
                        break;
                }

                // ── 6. Store session ───────────────────────────────────
                SessionHelper.SetLogin(Session, userId, email, role, profileId, fullName);

                // ── 7. Redirect to the correct dashboard ───────────────
                RedirectByRole(role);
            }
            catch (Exception ex)
            {
                // Log the real exception to the event log / file in production
                // For now, show a generic error to the user
                ShowError("A system error occurred. Please try again later.");
                System.Diagnostics.Debug.WriteLine("[Login Error] " + ex.Message);
            }
        }

        // ---------------------------------------------------------------
        // Helper: fetch a single profile row for the given userId.
        // ---------------------------------------------------------------
        private DataRow FetchProfile(string sql, int userId)
        {
            SqlParameter[] p = { new SqlParameter("@Uid", userId) };
            DataTable dt = DatabaseHelper.ExecuteQuery(sql, p);
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        // ---------------------------------------------------------------
        // Helper: redirect to the correct dashboard by role integer.
        // ---------------------------------------------------------------
        private void RedirectByRole(int role)
        {
            switch (role)
            {
                case SessionHelper.ROLE_ADMIN:
                    Response.Redirect("~/Admin/Dashboard.aspx", false);
                    break;
                case SessionHelper.ROLE_LECTURER:
                    Response.Redirect("~/Lecturer/Lecturer_Dashboard.aspx", false);
                    break;
                case SessionHelper.ROLE_STUDENT:
                    Response.Redirect("~/Student/Student_Dashboard.aspx", false);
                    break;
                default:
                    ShowError("Unrecognised user role. Please contact the administrator.");
                    break;
            }
        }

        // ---------------------------------------------------------------
        // Helper: show the error panel with a message.
        // ---------------------------------------------------------------
        private void ShowError(string message)
        {
            pnlAlert.Visible = true;
            pnlSuccess.Visible = false;
            lblAlert.Text = message;
        }

        // ---------------------------------------------------------------
        // Helper: show the success panel (used after redirect from
        // password-reset page, etc.).
        // ---------------------------------------------------------------
        private void ShowSuccess(string message)
        {
            pnlSuccess.Visible = true;
            pnlAlert.Visible = false;
            lblSuccess.Text = message;
        }
    }
}
