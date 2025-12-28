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
            "Server=localhost;Port=3306;database=client_schedule;Uid=sqlUser;Pwd=Passw0rd!;";

        //connects to db for joins and pulls
        public static MySqlConnection CreateOpenConnection() 
        {
            var conn = new MySqlConnection(ConnectionString);
            conn.Open();
            return conn;
        }
    }
}
