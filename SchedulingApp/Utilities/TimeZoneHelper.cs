using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingApp.Utilities
{
    public static class TimeZoneHelper
    {
        // Converts a local DateTime into UTC for db storage
        public static DateTime LocalToUtc(DateTime localTime)
        {
            // DateTimePicker gives Unspecified or Local depending on usage
            // Treat it as local and convert to UTC.
            return DateTime.SpecifyKind(localTime, DateTimeKind.Local).ToUniversalTime();
        }

        // Converts UTC from db into local time for display
        public static DateTime UtcToLocal(DateTime utcTime)
        {
            return DateTime.SpecifyKind(utcTime, DateTimeKind.Utc).ToLocalTime();
        }

        // Time/day checks
        // business hours based on Eastern Time: Mon-Fri, 9:00 to 17:00
        public static bool IsWithinBusinessHoursEastern(DateTime startLocal, DateTime endLocal)
        {
            // check for silly error
            if (endLocal <= startLocal)
                return false;

            TimeZoneInfo eastern = GetEasternTimeZone();

            DateTime startEastern = TimeZoneInfo.ConvertTime(startLocal, TimeZoneInfo.Local, eastern);
            DateTime endEastern = TimeZoneInfo.ConvertTime(endLocal, TimeZoneInfo.Local, eastern);

            // same day check (in eastern time)
            if (startEastern.Date != endEastern.Date)
                return false;

            // Mon–Fri check
            if (startEastern.DayOfWeek == DayOfWeek.Saturday || startEastern.DayOfWeek == DayOfWeek.Sunday)
                return false;

            // 9:00 AM to 5:00 PM (17:00) Eastern
            TimeSpan open = new TimeSpan(9, 0, 0);
            TimeSpan close = new TimeSpan(17, 0, 0);

            return startEastern.TimeOfDay >= open && endEastern.TimeOfDay <= close;
        }

        public static TimeZoneInfo GetEasternTimeZone()
        {
            // Windows time zone id
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            }
            catch
            {
                // Fallback: if not found for some reason, treat local as "business timezone"
                return TimeZoneInfo.Local;
            }
        }
    }
}
