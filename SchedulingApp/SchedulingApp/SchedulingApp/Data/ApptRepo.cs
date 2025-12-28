using MySql.Data.MySqlClient;
using SchedulingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingApp.Data
{
    public class ApptRepo
    {
        // Returns all appointments joined with customers and users
        public List<Appointment> GetAll()
        {
            var result = new List<Appointment>();

            const string sql = @"
                SELECT 
                    appointment.*,
                    customer.customerName,
                    user.userName
                FROM appointment
                INNER JOIN customer ON appointment.customerId = customer.customerId
                INNER JOIN user ON appointment.userId = user.userId;";

            using (var conn = DbConnectionFactory.CreateOpenConnection())
            using (var cmd = new MySqlCommand(sql, conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new Appointment
                    {
                        AppointmentId = reader.GetInt32("appointmentId"),
                        CustomerId = reader.GetInt32("customerId"),
                        UserId = reader.GetInt32("userId"),
                        Type = reader.GetString("type"),
                        StartUtc = reader.GetDateTime("start"),
                        EndUtc = reader.GetDateTime("end"),
                        CustomerName = reader.GetString("customerName"),
                        UserName = reader.GetString("userName")
                    });
                }
            }

            return result;
        }

        // Finds upcoming appointments for login alerts
        public List<Appointment> GetForUserBetween(int userId, DateTime startUtc, DateTime endUtc)
        {
            var result = new List<Appointment>();

            const string sql = @"
                SELECT *
                FROM appointment
                WHERE userId = @uid
                  AND start BETWEEN @start AND @end;";

            using (var conn = DbConnectionFactory.CreateOpenConnection())
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@uid", userId);
                cmd.Parameters.AddWithValue("@start", startUtc);
                cmd.Parameters.AddWithValue("@end", endUtc);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new Appointment
                        {
                            AppointmentId = reader.GetInt32("appointmentId"),
                            CustomerId = reader.GetInt32("customerId"),
                            UserId = reader.GetInt32("userId"),
                            Type = reader.GetString("type"),
                            StartUtc = reader.GetDateTime("start"),
                            EndUtc = reader.GetDateTime("end")
                        });
                    }
                }
            }

            return result;
        }

        // Checks for overlapping appointments for a customer
        public bool HasOverlappingAppointment(int customerId, int? appointmentId, DateTime startUtc, DateTime endUtc)
        {
            const string sql = @"
                SELECT COUNT(*)
                FROM appointment
                WHERE customerId = @cid
                  AND (@start < `end` AND @end > `start`)
                  AND (@id IS NULL OR appointmentId <> @id);";

            using (var conn = DbConnectionFactory.CreateOpenConnection())
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@cid", customerId);
                cmd.Parameters.AddWithValue("@start", startUtc);
                cmd.Parameters.AddWithValue("@end", endUtc);
                cmd.Parameters.AddWithValue("@id", (object)appointmentId ?? DBNull.Value);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

        public void Add(Appointment appt)
        {
            if (appt == null)
                throw new ArgumentNullException(nameof(appt));

            const string sql = @"
                INSERT INTO appointment
                    (customerId, userId, title, description, location, contact, type, url, start, end,
                     createDate, createdBy, lastUpdate, lastUpdateBy)
                VALUES
                    (@customerId, @userId, '', '', '', '', @type, '', @start, @end,
                     NOW(), 'system', NOW(), 'system');";

            using (var conn = DbConnectionFactory.CreateOpenConnection())
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@customerId", appt.CustomerId);
                cmd.Parameters.AddWithValue("@userId", appt.UserId);
                cmd.Parameters.AddWithValue("@type", appt.Type);

                // Stores UTC timestamps in the database
                cmd.Parameters.AddWithValue("@start", appt.StartUtc);
                cmd.Parameters.AddWithValue("@end", appt.EndUtc);

                cmd.ExecuteNonQuery();

            }
        }

        public void Update(Appointment appt)
        {
            if (appt == null)
                throw new ArgumentNullException(nameof(appt));

            const string sql = @"
                UPDATE appointment
                SET
                    customerId   = @customerId,
                    userId       = @userId,
                    type         = @type,
                    start        = @start,
                    end          = @end,
                    lastUpdate   = NOW(),
                    lastUpdateBy = 'system'
                WHERE appointmentId = @appointmentId;";

            using (var conn = DbConnectionFactory.CreateOpenConnection())
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@customerId", appt.CustomerId);
                cmd.Parameters.AddWithValue("@userId", appt.UserId);
                cmd.Parameters.AddWithValue("@type", appt.Type);
                cmd.Parameters.AddWithValue("@start", appt.StartUtc);
                cmd.Parameters.AddWithValue("@end", appt.EndUtc);
                cmd.Parameters.AddWithValue("@appointmentId", appt.AppointmentId);

                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int appointmentId)
        {
            const string sql = @"
                DELETE FROM appointment
                WHERE appointmentId = @appointmentId;";

            using (var conn = DbConnectionFactory.CreateOpenConnection())
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@appointmentId", appointmentId);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
