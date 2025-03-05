using Microsoft.EntityFrameworkCore;
namespace Context
{
    public class APIContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public APIContext(DbContextOptions<APIContext> options) : base(options)
        {

        }
    }
}
