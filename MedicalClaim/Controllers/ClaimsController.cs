using Microsoft.AspNetCore.Mvc;
using MedicalClaim.Data;
using MedicalClaim.Models;
using MedicalClaim.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;

namespace MedicalClaim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ClaimValidationService _validationService;

        public ClaimsController(ApplicationDbContext context, ClaimValidationService validationService)
        {
            _context = context;
            _validationService = validationService;
        }

        [HttpPost]
        public async Task<ActionResult<Claim>> SubmitClaim(Claim claim)
        {
            _context.Claims.Add(claim);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetClaimDetails), new { id = claim.ClaimId }, claim);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Claim>> GetClaimDetails(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null)
            {
                return NotFound();
            }
            return claim;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClaimStatus(int id, [FromBody] string status)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null)
            {
                return NotFound();
            }
            claim.Status = status;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Claim>>> ListAllClaims([FromQuery] string status = null, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            var claims = _context.Claims.AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                claims = claims.Where(c => c.Status == status);
            }

            if (startDate.HasValue)
            {
                claims = claims.Where(c => c.DateOfService >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                claims = claims.Where(c => c.DateOfService <= endDate.Value);
            }

            return await claims.ToListAsync();
        }

        [HttpPost("validate/{id}")]
        public async Task<IActionResult> ValidateClaim(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null)
            {
                return NotFound();
            }

            var isValid = await _validationService.ValidateAsync(claim);
            claim.Status = isValid ? "Approved" : "Rejected";
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}