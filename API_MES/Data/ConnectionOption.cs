using System;
using System.Data;
using System.Data.SqlClient;

namespace API_MES.Data
{
    public class ConnectionOption
    {
        private static IDbConnection _dbConnection = new SqlConnection();
        public static IDbConnection DbConnection
        {
            get
            {
                if (string.IsNullOrEmpty(_dbConnection.ConnectionString))
                {
                    _dbConnection.ConnectionString = ConnectionString;
                }
                return _dbConnection;
            }
        }


        private static string _connectionString;
        public static string ConnectionString { get => _connectionString; set => _connectionString = value; }

    }
}