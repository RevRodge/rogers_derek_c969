using SchedulingApp.Data;
using SchedulingApp.Models;
using SchedulingApp.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SchedulingApp.Forms
{
    public partial class LoginForm : Form
    {
        private readonly UserRepo _userRepo = new UserRepo();
        private readonly ApptRepo _apptRepo = new ApptRepo();
        public LoginForm()
        {
            InitializeComponent();
            
            //displays local timezone on login screen
            //cutting if it doesn't work
            try
            {
                if (lblZone != null)
                {
                    lblZone.Text = TimeZoneInfo.Local.DisplayName;
                }
            }
            catch
            {
                // Ignore if anything goes wrong
            }

            // allow Enters to trigger login
            AcceptButton = btnLogin;
        }

        //easy one first, closes form
        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show(GetText("LoginMissingFields"), GetText("LoginTitle"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            User user;
            try
            {
                user = _userRepo.ValidateUser(username, password);
            }
            catch (Exception ex)
            {
                // If DB is down / credentials bad / etc.
                Logger.LogError("Login DB error", ex);
                MessageBox.Show(GetText("LoginDbError"), GetText("LoginTitle"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (user == null)
            {
                MessageBox.Show(GetText("LoginInvalid"), GetText("LoginTitle"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Successful login: log it to file
            Logger.LogLogin(user.UserName);

            // checks for upcoming appointment within 15 minutes (rubric requirement)
            try
            {
                DateTime nowUtc = DateTime.UtcNow;
                DateTime soonUtc = nowUtc.AddMinutes(15);

                var upcoming = _apptRepo.GetForUserBetween(user.UserId, nowUtc, soonUtc);

                if (upcoming.Any())
                {
                    // Pick the earliest one
                    var next = upcoming.OrderBy(a => a.StartUtc).First();

                    DateTime startLocal = TimeZoneHelper.UtcToLocal(next.StartUtc);

                    MessageBox.Show(
                        string.Format(GetText("UpcomingApptYes"), startLocal),
                        GetText("LoginTitle"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                else
                {
                    MessageBox.Show(GetText("UpcomingApptNo"), GetText("LoginTitle"),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                // Don't block login if the alert fails, but do log it
                Logger.LogError("Upcoming appointment check failed", ex);
            }

            // Open main form and hide login
            var main = new MainForm(user);
            Hide();
            main.FormClosed += (s, args) => Close();
            main.Show();
        }
        private string GetText(string key)
        {
            switch (key)
            {
                case "LoginTitle": 
                    return "Login";
                case "LoginMissingFields": 
                    return "Please enter a username and password.";
                case "LoginInvalid": 
                    return "Invalid username or password.";
                case "LoginDbError": 
                    return "Unable to connect to the database.";
                case "UpcomingApptYes": 
                    return "You have an appointment within 15 minutes. Start: {0}";
                case "UpcomingApptNo": 
                    return "You have no appointments within the next 15 minutes.";
                default: return key;
            }
        }

    }
}
