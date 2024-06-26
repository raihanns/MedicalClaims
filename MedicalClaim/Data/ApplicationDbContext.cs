using Microsoft.EntityFrameworkCore;
using MedicalClaim.Models;

namespace MedicalClaim.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Claim> Claims { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("Server=localhost;Database=MedicalClaimDb;User=root;Password=;", 
                new MySqlServerVersion(new Version(8, 0, 21)));
        }
    }
}