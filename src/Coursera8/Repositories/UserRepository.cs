using Coursera8.Services;
using Microsoft.Data.Sqlite;

public class UserRepository
{
    private string _connectionString = "Data Source=SafeVault.db";

    public UserRepository()
    {
        InitializeDatabase();
        SeedAdminUser(); // Automatically create admin on first run
    }

    private void InitializeDatabase()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        var command = connection.CreateCommand();
        // UPDATED: Added PasswordHash and Role columns
        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS Users (
                UserID INTEGER PRIMARY KEY AUTOINCREMENT,
                Username TEXT NOT NULL UNIQUE,
                Email TEXT NOT NULL,
                PasswordHash TEXT NOT NULL,
                Role TEXT NOT NULL
            );";
        command.ExecuteNonQuery();
    }

    private void SeedAdminUser()
    {
        // Check if admin already exists to avoid duplicates
        if (GetUser("admin") == null)
        {
            // SECURE: Hash the password before saving!
            string hashedPw = AuthService.HashPassword("Admin123!");
            RegisterUser("admin", "admin@safevault.com", hashedPw, "Admin");
        }

        if (GetUser("user1") == null)
        {
            string userHash = AuthService.HashPassword("Password2025!");
            RegisterUser("user1", "user1@safevault.com", userHash, "Guest");
        }
    }

    public void RegisterUser(string username, string email, string hashedPw, string role = "User")
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = "INSERT INTO Users (Username, Email, PasswordHash, Role) VALUES ($name, $email, $hash, $role)";
        command.Parameters.AddWithValue("$name", username);
        command.Parameters.AddWithValue("$email", email);
        command.Parameters.AddWithValue("$hash", hashedPw);
        command.Parameters.AddWithValue("$role", role);
        command.ExecuteNonQuery();
    }

    public UserProfile GetUser(string username)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = "SELECT Username, PasswordHash, Role FROM Users WHERE Username = $user";
        command.Parameters.AddWithValue("$user", username);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new UserProfile
            {
                Username = reader.GetString(0),
                PasswordHash = reader.GetString(1),
                Role = reader.GetString(2)
            };
        }
        return null;
    }

    public void AddUser(string username, string email)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        var command = connection.CreateCommand();

        // 1. SECURE: Generate a hash for a default password
        // For demo purposes, new entries will have "Password123!" as their secret
        string defaultPassword = "Password123!";
        string hashedPassword = AuthService.HashPassword(defaultPassword);

        // 2. SECURE: Parameterized Query updated with security columns
        command.CommandText = @"
        INSERT INTO Users (Username, Email, PasswordHash, Role) 
        VALUES ($name, $email, $hash, $role)";

        command.Parameters.AddWithValue("$name", username);
        command.Parameters.AddWithValue("$email", email);
        command.Parameters.AddWithValue("$hash", hashedPassword);
        command.Parameters.AddWithValue("$role", "User"); // Default role for new entries

        command.ExecuteNonQuery();
    }
}

public class UserProfile
{
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string Role { get; set; }
}