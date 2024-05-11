using Bakers.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace Bakers.ContextClass
{
    public class BakersDbContext: IdentityDbContext<ApplicationUser>
    {
        public BakersDbContext(DbContextOptions<BakersDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public DbSet<Product> products { get; set; }
        public DbSet<Cart> carts { get; set; }
        public DbSet<Customer> customer { get; set; }
    }
}
