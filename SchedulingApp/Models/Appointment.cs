using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingApp.Models
{
    public class Appointment
    {
        // pk from appt table
        public int AppointmentId { get; set; }

        // fk back to customer table
        public int CustomerId { get; set; }

        // fk back to the user table 
        public int UserId { get; set; }

        // appt type
        public string Type { get; set; } = string.Empty;

        // appt start time UTC
        public DateTime StartUtc { get; set; }

        // end time in UTC
        public DateTime EndUtc { get; set; }

        //Display strings from joined tables
        public string CustomerName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }
}
