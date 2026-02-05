namespace Coursera8.Services
{
    using BCrypt.Net;

    public static class AuthService
    {
        // Generate a secure hash to store in the DB
        public static string HashPassword(string password)
        {
            // Work factor 12 is a good balance between security and speed
            return BCrypt.HashPassword(password, workFactor: 12);
        }

        // Verify the password during login
        public static bool VerifyPassword(string password, string storedHash)
        {
            return BCrypt.Verify(password, storedHash);
        }
    }
}
