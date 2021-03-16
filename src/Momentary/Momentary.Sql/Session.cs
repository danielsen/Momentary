using System.Data.SqlClient;
using Momentary.Common;

namespace Momentary.Sql
{
    internal class Session : ISession
    {
        public void Execute(string connectionString, string command)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var sqlCommand = connection.CreateCommand())
                {
                    sqlCommand.CommandText = command;
                    sqlCommand.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        public void ExecuteReader(string connectionString, string command, out int rows)
        {
            rows = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var sqlCommand = connection.CreateCommand())
                {
                    sqlCommand.CommandText = command;
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
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