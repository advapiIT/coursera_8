using System.Text.RegularExpressions;
using System.Web;

public class SecurityService
{
    public static string SanitizeInput(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;

        // Trim whitespace and encode HTML to prevent XSS
        // This converts <script> into &lt;script&gt;
        return HttpUtility.HtmlEncode(input.Trim());
    }

    public static bool IsValidEmail(string email)
    {
        // Basic Regex for email validation
        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern);
    }
}