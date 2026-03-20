using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using miniEcommerceApi.Models;

namespace miniEcommerceApi.Data.Configurations
{
    public class CustomersConfig : IEntityTypeConfiguration<Customers>
    {
        public void Configure(EntityTypeBuilder<Customers> builder)
        {
            builder.ToTable("Customers");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.LastName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Phone).IsRequired().HasMaxLength(20);
            builder.Property(x => x.Cpf).IsRequired().HasMaxLength(20);
            builder.HasOne(x => x.User)
                   .WithOne()
                   .HasForeignKey<Customers>(x => x.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
