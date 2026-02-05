    using Microsoft.Data.Sqlite;

    public class UserRepository
    {
        // OPTION A: File-based (Data persists after restart)
        private string _connectionString = "Data Source=SafeVault.db";

        // OPTION B: In-Memory (Data is wiped when the app stops)
        // private string _connectionString = "Data Source=:memory:;Mode=Memory;Cache=Shared";

        public UserRepository()
        {
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                // Create the table if it doesn't exist
                command.CommandText =
                    @"
                CREATE TABLE IF NOT EXISTS Users (
                    UserID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL,
                    Email TEXT NOT NULL
                );
            ";
                command.ExecuteNonQuery();
            }
        }

        public void AddUser(string username, string email)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();

            // SECURE: Parameterized Query
            command.CommandText = "INSERT INTO Users (Username, Email) VALUES ($name, $email)";
            command.Parameters.AddWithValue("$name", username);
            command.Parameters.AddWithValue("$email", email);

            command.ExecuteNonQuery();
        }
    }
