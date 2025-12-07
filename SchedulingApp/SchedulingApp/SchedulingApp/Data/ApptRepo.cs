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
        // Returns all appointments joined with customers and users.
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
                INNER JOIN user ON appointment.userId = user.userId;
            ";

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

        // Finds upcoming appointments for login alert.
        public List<Appointment> GetForUserBetween(int userId, DateTime startUtc, DateTime endUtc)
        {
            var result = new List<Appointment>();

            const string sql = @"
                SELECT *
                FROM appointment
                WHERE userId = @uid
                  AND start BETWEEN @start AND @end;
            ";

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

        // Checks for overlapping appointments for a customer.
        // Requirement A3A2.
        public bool HasOverlappingAppointment(int customerId, int? appointmentId, DateTime startUtc, DateTime endUtc)
        {
            const string sql = @"
                SELECT COUNT(*)
                FROM appointment
                WHERE customerId = @cid
                  AND (@start < `end` AND @end > `start`)
                  AND (@id IS NULL OR appointmentId <> @id);
            ";

            using (var conn = DbConnectionFactory.CreateOpenConnection())
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@cid", customerId);
                cmd.Parameters.AddWithValue("@start", startUtc);
                cmd.Parameters.AddWithValue("@end", endUtc);
                cmd.Parameters.AddWithValue("@id", appointmentId);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

        public void Add(Appointment appt)
        {
            // TODO: INSERT (store StartUtc, EndUtc)
        }

        public void Update(Appointment appt)
        {
            // TODO: UPDATE
        }

        public void Delete(int appointmentId)
        {
            // TODO: DELETE
        }
    }
}
