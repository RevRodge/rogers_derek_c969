using SchedulingApp.Models; //class depends
using MySql.Data.MySqlClient; //installed nuget pkg
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingApp.Data
{
    public class UserRepo
    {
        // validates username/password against user table
        public User ValidateUser(string username, string password)
        {
            const string sql = @"
                SELECT * FROM user 
                WHERE userName = @name AND password = @pw AND active = 1";

            using (var conn = DbConnectionFactory.CreateOpenConnection())
            using (var cmd = new MySqlCommand(sql, conn))
            {
                
                // not necessary for a school project but uses parameters to handle attempted sql injects :P
                cmd.Parameters.AddWithValue("@name", username);
                cmd.Parameters.AddWithValue("@pw", password);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var user = new User
                        {
                            UserId = reader.GetInt32(reader.GetOrdinal("userId")),
                            UserName = reader.GetString(reader.GetOrdinal("userName")),
                            Password = reader.GetString(reader.GetOrdinal("password")),
                            Active = reader.GetInt32(reader.GetOrdinal("active")) == 1
                        };
                        return user;
                    }
                }
            }

            return null;
        }
    }
}
