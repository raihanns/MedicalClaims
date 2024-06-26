using Microsoft.AspNetCore.Mvc.RazorPages;
using MedicalClaim.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System;

namespace MedicalClaimFrontEnd.Pages
{
    public class ListAllClaimsModel : PageModel
    {
        public List<Claim> Claims { get; set; }

        public async Task OnGetAsync(string status = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            using (var httpClient = new HttpClient())
            {
                var queryParams = new List<string>();
                if (!string.IsNullOrEmpty(status))
                {
                    queryParams.Add($"status={status}");
                }
                if (startDate.HasValue)
                {
                    queryParams.Add($"startDate={startDate.Value.ToString("yyyy-MM-dd")}");
                }
                if (endDate.HasValue)
                {
                    queryParams.Add($"endDate={endDate.Value.ToString("yyyy-MM-dd")}");
                }

                var queryString = string.Join("&", queryParams);
                var url = $"http://localhost:5083/api/claims";
                if (queryParams.Count > 0)
                {
                    url += "?" + queryString;
                }

                var response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    Claims = JsonSerializer.Deserialize<List<Claim>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    Claims = new List<Claim>();
                }
            }
        }
    }
}