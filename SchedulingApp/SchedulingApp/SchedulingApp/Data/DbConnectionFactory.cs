using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient; // installed nuget pkg

namespace SchedulingApp.Data
{
    public static class DbConnectionFactory
    {
        // TODO: replace with actual db settings from lab vm
        private const string ConnectionString =
            "server=localhost;user id=YOUR_USER;password=YOUR_PASSWORD;database=YOUR_DB;";

        public static MySqlConnection CreateOpenConnection() //connects to db for joins and pulls
        {
            var conn = new MySqlConnection(ConnectionString);
            conn.Open();
            return conn;
        }
    }
}
