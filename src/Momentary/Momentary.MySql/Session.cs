using Momentary.Common;
using MySqlConnector;

namespace Momentary.MySql
{
    internal class Session : ISession
    {
        public void Execute(string connectionString, string command)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (var npgsqlCommand = connection.CreateCommand())
                {
                    npgsqlCommand.CommandText = command;
                    npgsqlCommand.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        public void ExecuteReader(string connectionString, string command, out int rows)
        {
            rows = 0;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (var mySqlCommand = connection.CreateCommand())
                {
                    mySqlCommand.CommandText = command;
                    using (var mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        if (mySqlDataReader.HasRows)
                        {
                            while (mySqlDataReader.Read())
                            {
                                rows++;
                            }
                        }
                    }
                }

                connection.Close();
            }
        }
    }
}