using MedicalClaim.Models;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;

namespace MedicalClaim.Services
{
    public class ClaimValidationService
    {
        private readonly HttpClient _httpClient;

        public ClaimValidationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> ValidateAsync(Claim claim)
        {
            var json = JsonSerializer.Serialize(claim);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("http://localhost:5083/api/ValidationService", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ValidationResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return result.IsValid;
            }

            return false;
        }
    }

    public class ValidationResponse
    {
        public bool IsValid { get; set; }
    }
}