using Microsoft.EntityFrameworkCore;
using miniEcommerceApi.Models;

namespace miniEcommerceApi.Data
{
    public sealed class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Adresses> Adresses { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Costumers> Customers { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
