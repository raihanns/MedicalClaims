using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MedicalClaimFrontEnd.Pages
{
    public class UpdateClaimStatusModel : PageModel
    {
        [BindProperty]
        public int ClaimId { get; set; }
        [BindProperty]
        public string Status { get; set; }

        public void OnGet(int id)
        {
            ClaimId = id;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            using (var httpClient = new HttpClient())
            {
                var content = new StringContent($"\"{Status}\"", Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync($"http://localhost:5083/api/claims/{ClaimId}", content);

                if (!response.IsSuccessStatusCode)
                {
                    return NotFound();
                }

                return RedirectToPage("Success");
            }
        }
    }
}