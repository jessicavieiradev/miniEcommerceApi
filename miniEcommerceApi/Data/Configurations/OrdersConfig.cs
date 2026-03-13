using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using miniEcommerceApi.Models;

namespace miniEcommerceApi.Data.Configurations
{
    public class OrdersConfig : IEntityTypeConfiguration<Orders>
    {
        public void Configure(EntityTypeBuilder<Orders> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(x => x.Id);
            builder.Property(x=>x.OrderDate).IsRequired();
            builder.Property(x=>x.Status).IsRequired();
            builder.Property(x=>x.TotalAmount).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x=>x.ZipCode).IsRequired().HasMaxLength(20);
            builder.Property(x=>x.Street).IsRequired().HasMaxLength(100);
            builder.Property(x=>x.Number).IsRequired().HasMaxLength(20);
            builder.Property(x=>x.Neighborhood).IsRequired().HasMaxLength(50);
            builder.Property(x=>x.City).IsRequired().HasMaxLength(50);
            builder.Property(x=>x.State).IsRequired().HasMaxLength(50);
            builder.Property(x=>x.Notes).HasMaxLength(500);
            builder.Property(x=>x.CreatedAt).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();
            builder.HasOne(x => x.Customer)
                   .WithMany()
                   .HasForeignKey(x => x.CustomerId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
