using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicalClaim.Controllers;
using MedicalClaim.Data; // Ensure this namespace is correct
using MedicalClaim.Models;
using MedicalClaim.Services;
using Moq;
using Xunit;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MedicalClaim.Tests
{
    public class ClaimsControllerTests
    {
        private readonly ClaimsController _controller;
        private readonly ApplicationDbContextTest _context;

        public ClaimsControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContextTest(options);
            _context.Claims.Add(new Claim
            {
                PatientName = "Test Patient",
                DateOfService = DateTime.Now,
                MedicalProvider = "Test Provider",
                Diagnosis = "Test Diagnosis",
                ClaimAmount = 100,
                Status = "Pending"
            });
            _context.SaveChanges();

            var httpClientFactory = new Mock<IHttpClientFactory>();
            var client = new HttpClient();
            httpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            var validationService = new ClaimValidationService((HttpClient)httpClientFactory.Object);
            _controller = new ClaimsController(_context, validationService);
        }

        [Fact]
        public async Task SubmitClaim_ReturnsCreatedResult()
        {
            var claim = new Claim
            {
                PatientName = "John Doe",
                DateOfService = DateTime.Now,
                MedicalProvider = "Provider A",
                Diagnosis = "Diagnosis A",
                ClaimAmount = 250.00M,
                Status = "Pending"
            };

            var result = await _controller.SubmitClaim(claim);

            Assert.IsType<CreatedAtActionResult>(result.Result);
        }
    }
}