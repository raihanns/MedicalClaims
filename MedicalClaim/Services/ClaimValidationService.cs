using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MedicalClaim.Models;

namespace MedicalClaim.Services
{
    public class ClaimValidationService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ClaimValidationService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> ValidateAsync(Claim claim)
        {
            var client = _httpClientFactory.CreateClient();
            var json = JsonSerializer.Serialize(claim);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("http://localhost:5083/api/ValidationService", content);

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