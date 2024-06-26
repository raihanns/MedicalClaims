using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MedicalClaim.Services;
using System.Threading.Tasks;

namespace MedicalClaim.Pages
{
    public class LoginModel : PageModel
    {
        private readonly JwtTokenService _jwtTokenService;

        public LoginModel(JwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
        }

        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Password { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            // This is a simple example. In a real application, you would validate the username and password.
            if (Username == "test" && Password == "password")
            {
                var token = await _jwtTokenService.GenerateTokenAsync(Username);
                // Store the token in a cookie or local storage
                // For example:
                Response.Cookies.Append("jwt", token);
                return RedirectToPage("/Index");
            }

            return Page();
        }
    }
}