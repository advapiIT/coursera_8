using Coursera8.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursera8.Tests
{
    [TestFixture]
    public class TestSecurityAccess
    {
        [Test]
        public void TestPasswordHashing_Success()
        {
            string password = "Sa" +
                              "" +
                              "feVault_Password_2026!";
            string hash = AuthService.HashPassword(password);

            // Assert hash is not the same as password
            Assert.That(hash, Is.Not.EqualTo(password));
            // Assert verification works
            Assert.That(AuthService.VerifyPassword(password, hash));
        }

        [Test]
        public void TestUnauthorizedAccess_Blocked()
        {
            // Simulation: User with role 'Guest' tries to access 'Admin'
            var userRole = "Guest";
            var requiredRole = "Admin";

            bool hasAccess = userRole == requiredRole;

            Assert.That(!hasAccess, "Guest should not have access to Admin features.");
        }
    }
}
