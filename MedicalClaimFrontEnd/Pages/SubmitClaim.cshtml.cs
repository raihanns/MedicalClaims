using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MedicalClaim.Models;  // Ensure this namespace is correct
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;

namespace MedicalClaimFrontEnd.Pages
{
    public class SubmitClaimModel : PageModel
    {
        [BindProperty]
        public Claim Claim { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            using (var httpClient = new HttpClient())
            {
                var json = JsonSerializer.Serialize(Claim);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("http://localhost:5083/api/claims", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("Success");
                }
            }

            return Page();
        }
    }
}