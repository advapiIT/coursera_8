using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Coursera8.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        [Required]
        public string Username { get; set; }

        [BindProperty]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Message { get; set; }

        public void OnGet()
        {
            // This runs when the page loads for the first time
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Message = "Please correct the errors in the form.";
                return Page();
            }

            // --- STEP 2: SANITIZATION ---
            string cleanUser = SecurityService.SanitizeInput(Username);
            string cleanEmail = SecurityService.SanitizeInput(Email);

            // --- STEP 3: PARAMETERIZED QUERY ---
            try
            {
                var repo = new UserRepository();
                repo.AddUser(cleanUser, cleanEmail);
                Message = "Data securely stored!";
            }
            catch (Exception ex)
            {
                Message = "An error occurred while saving.";
            }

            return Page();
        }
    }
}
