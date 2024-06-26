using Microsoft.EntityFrameworkCore;

namespace MedicalClaim.Data
{
    public class ApplicationDbContextTest : ApplicationDbContext
    {
        public ApplicationDbContextTest(DbContextOptions<ApplicationDbContextTest> options)
            : base(new DbContextOptions<ApplicationDbContext>())
        {
        }
    }
}