using Microsoft.EntityFrameworkCore;

namespace MedicalClaim.Data
{
    public class ApplicationDbContextTest : ApplicationDbContext
    {
        public ApplicationDbContextTest(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}