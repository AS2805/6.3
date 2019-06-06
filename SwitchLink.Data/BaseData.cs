using System;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace SwitchLink.Data
{
    public abstract class BaseData:IDisposable
    {
        private const string ENVIRONMENT_KEY = "Environment";

        private string ConnectionString
        {
            get
            {
          

                return @"server=localhost;user id=root;password=1275;database=switch;persistsecurityinfo=True";
            }
        } 

        protected MySqlConnection Connection => GetOpenConnection();

        private MySqlConnection _connection;

        private MySqlConnection GetOpenConnection()
        {
            if (_connection == null)
            {
                _connection = new MySqlConnection(ConnectionString);
                _connection.Open();
            }
            return _connection;
        }

        public void Dispose()
        {
            _connection?.Close();
        }
    }
}
