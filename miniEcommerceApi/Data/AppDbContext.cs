using Microsoft.EntityFrameworkCore;
using miniEcommerceApi.Models;

namespace miniEcommerceApi.Data
{
    public sealed class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        DbSet<Adresses> Adresses { get; set; }
        DbSet<Categories> Categories { get; set; }
        DbSet<Costumers> Customers { get; set; }
        DbSet<Orders> Orders { get; set; }
        DbSet<Products> Products { get; set; }
        DbSet<OrderItem> OrderItem { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
