using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingApp.Models
{
    public class User
    {
        //pk for user table
        public int UserId { get; set; }
        //actual username for logging in
        public string UserName { get; set; } = string.Empty;
        //user's password (stored as plain text)
        public string Password { get; set; } = string.Empty;
        // True if user marked active in DB
        public bool Active { get; set; }
    }
}
