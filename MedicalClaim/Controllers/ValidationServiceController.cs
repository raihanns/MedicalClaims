using MedicalClaim.Models;
using Microsoft.AspNetCore.Mvc;

namespace MedicalClaim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidationServiceController : ControllerBase
    {
        [HttpPost]
        public IActionResult ValidateClaim([FromBody] Claim claim)
        {
            // Simulate validation logic
            bool isValid = new Random().Next(2) == 0;
            return Ok(new { IsValid = isValid });
        }
    }
}