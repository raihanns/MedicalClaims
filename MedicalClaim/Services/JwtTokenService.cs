using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MedicalClaim.Services
{
    public class JwtTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IDatabase _redisDb;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            var configurationOptions = ConfigurationOptions.Parse(_configuration.GetSection("Redis")["Connection"]);
            configurationOptions.AbortOnConnectFail = false; // Allow retrying on connection failure
            var redis = ConnectionMultiplexer.Connect(configurationOptions);
            _redisDb = redis.GetDatabase();
        }

        public async Task<string> GenerateTokenAsync(string username)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: new[] { new Claim(ClaimTypes.Name, username) },
                expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:ExpireMinutes"])),
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // Store token in Redis with expiration
            await _redisDb.StringSetAsync(tokenString, username, TimeSpan.FromMinutes(double.Parse(_configuration["Jwt:ExpireMinutes"])));

            return tokenString;
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            var username = await _redisDb.StringGetAsync(token);
            return !string.IsNullOrEmpty(username);
        }

        public async Task RevokeTokenAsync(string token)
        {
            await _redisDb.KeyDeleteAsync(token);
        }
    }
}