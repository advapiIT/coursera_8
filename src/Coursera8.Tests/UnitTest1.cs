using NUnit.Framework;

[TestFixture]
public class TestInputValidation
{
    [Test]
    public void TestForSQLInjection()
    {
        string maliciousInput = "admin' OR '1'='1";
        string sanitized = SecurityService.SanitizeInput(maliciousInput);

        // Assert that the single quote was encoded, neutralizing the SQL break
        Assert.That(sanitized, Does.Contain("&#39;"));
    }

    [Test]
    public void TestForXSS()
    {
        string xssAttack = "<script>alert('Hacked')</script>";
        string sanitized = SecurityService.SanitizeInput(xssAttack);

        // Assert that tags are converted to plain text entities
        Assert.That(sanitized, Does.StartWith("&lt;script&gt;"));
    }
}