using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using miniEcommerceApi.Models;

namespace miniEcommerceApi.Data.Configurations
{
    public class ProductsConfig : IEntityTypeConfiguration<Products>
    {
        public void Configure(EntityTypeBuilder<Products> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(x=>x.Id);
            builder.Property(x=>x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x=>x.Description).HasMaxLength(500);
            builder.Property(x=>x.Price).HasColumnType("decimal(18,2)");
            builder.Property(x=>x.Stock).IsRequired();
            builder.Property(x=>x.ImageUrl).HasMaxLength(200);
            builder.Property(x=>x.CreatedAt).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();
            builder.HasOne(x=>x.Category)
                .WithMany()
                .HasForeignKey(x=>x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
