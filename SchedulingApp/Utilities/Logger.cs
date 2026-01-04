using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingApp.Utilities
{
    public static class Logger
    {
        private static readonly object _lock = new object();

        // Writes all logins or attempts to login_history.txt
        public static void LogLogin(string userName, bool success)
        {
            if (string.IsNullOrWhiteSpace(userName))
                userName = "UNKNOWN";

            string status = success ? "SUCCESS" : "FAILURE";
            string line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - LOGIN_ATTEMPT - {userName} - {status}";

            WriteLine("login_history.txt", line);
        }

        // helper to log errors while testing
        public static void LogError(string message, Exception ex)
        {
            if (string.IsNullOrWhiteSpace(message))
                message = "Unknown error";

            string line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - ERROR - {message} - {ex}";

            WriteLine("error_log.txt", line);
        }

        private static void WriteLine(string fileName, string line)
        {
            // Write into the folder
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

            lock (_lock)
            {
                File.AppendAllText(path, line + Environment.NewLine);
            }
        }
    }
}
