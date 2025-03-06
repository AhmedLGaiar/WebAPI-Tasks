using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebAPI_Labs.Model;
namespace Context
{
    public class APIContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public APIContext(DbContextOptions<APIContext> options) : base(options)
        {

        }
    }
}
