using System.Data.SQLite;
using Momentary.Common;

namespace Momentary.Sqlite
{
    internal class Session : ISession
    {
        public void Execute(string connectionString, string command)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand sqLiteCommand = connection.CreateCommand())
                {
                    sqLiteCommand.CommandText = command;
                    sqLiteCommand.ExecuteNonQuery();
                }
                
                connection.Close();
            }
        }

        public void ExecuteReader(string connectionString, string command, out int rows)
        {
            rows = 0;
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand sqLiteCommand = connection.CreateCommand())
                {
                    sqLiteCommand.CommandText = command;
                    using (SQLiteDataReader sqliteReader = sqLiteCommand.ExecuteReader())
                    {
                        if (sqliteReader.HasRows)
                        {
                            while (sqliteReader.Read()) rows++;
                        }
                    }
                }
                connection.Close();
            }
        }
    }
}