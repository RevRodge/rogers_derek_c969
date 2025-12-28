using SchedulingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SchedulingApp.Utilities
{
    public static class Validator
    {
        // Customer field validations and messages
        public static bool TryValidateCustomer(Customer c, out string error)
        {
            error = string.Empty;

            if (c == null)
            {
                error = "Customer object was null.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(c.CustomerName))
            {
                error = "Customer name is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(c.AddressLine1))
            {
                error = "Address Line 1 is required.";
                return false;
            }

            // AddressLine2 can be empty (OK)

            if (c.CityId <= 0)
            {
                error = "City is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(c.PostalCode))
            {
                error = "Postal code is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(c.Phone))
            {
                error = "Phone number is required.";
                return false;
            }

            // allows digits, spaces, parentheses, hyphens, plus signs
            if (!IsValidPhone(c.Phone))
            {
                error = "Phone number format is invalid.";
                return false;
            }

            return true;
        }

        // Appointment field validations and messages
        public static bool TryValidateAppointmentBasics(Appointment a, out string error)
        {
            error = string.Empty;

            if (a == null)
            {
                error = "Appointment object was null.";
                return false;
            }

            if (a.CustomerId <= 0)
            {
                error = "Customer is required.";
                return false;
            }

            if (a.UserId <= 0)
            {
                error = "User is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(a.Type))
            {
                error = "Appointment type is required.";
                return false;
            }

            if (a.EndUtc <= a.StartUtc)
            {
                error = "End time must be after start time.";
                return false;
            }

            return true;
        }

        public static bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            // Uses a regualar expression to accept any variation of a phone number,
            // rejects letters and weird punctuation
            return Regex.IsMatch(phone.Trim(), @"^[0-9\-\+\(\)\s]+$");
        }
    }
}
