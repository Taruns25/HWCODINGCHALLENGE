using insurance_management.Utilities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//namespace insurance_management.Utilities
//{
/*
{
    public static class DBConnUtil
    {
        private static SqlConnection connection;

        public static SqlConnection GetConnection()
        {
            if (connection == null)
            {
                try
                {
                    var connectionString = DBPropertyUtil.GetConnectionString();

                    connection = new SqlConnection(connectionString);
                    connection.Open();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error connecting to the database: {ex.Message}");
                    throw;
                }
            }
            return connection;
        }
    }

*/
using System;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace insurance_management.Utilities
{
    public static class DBConnUtil
    {
        private static readonly IConfiguration configuration;

        static DBConnUtil()
        {
            // Set up configuration to read from appsettings.json
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            configuration = builder.Build();
        }

        public static SqlConnection GetConnection()
        {
            // Retrieve the connection string from the configuration
            string connectionString = configuration.GetConnectionString("LocalConnectionString");
            return new SqlConnection(connectionString);
        }
    }
}
