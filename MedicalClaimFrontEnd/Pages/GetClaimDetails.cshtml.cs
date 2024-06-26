using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MedicalClaim.Models;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MedicalClaimFrontEnd.Pages
{
    public class GetClaimDetailsModel : PageModel
    {
        public Claim Claim { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"http://localhost:5083/api/claims/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    return NotFound();
                }

                var json = await response.Content.ReadAsStringAsync();
                Claim = JsonSerializer.Deserialize<Claim>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return Page();
            }
        }
    }
}