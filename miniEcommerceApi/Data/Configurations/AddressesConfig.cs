using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using miniEcommerceApi.Models;

namespace miniEcommerceApi.Datx.Configurations
{
    public class AddressesConfig : IEntityTypeConfiguration<Addresses>
    {
        public void Configure(EntityTypeBuilder<Addresses> builder)
        {
            builder.ToTable("Addresses");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ZipCode)
               .IsRequired()
               .HasMaxLength(20);
            builder.Property(x => x.Street)
                   .IsRequired()
                   .HasMaxLength(200);
            builder.Property(x => x.Number)
                   .HasMaxLength(20);
            builder.Property(x => x.Neighborhood)
                   .HasMaxLength(100);
            builder.Property(x => x.City)
                   .IsRequired()
                   .HasMaxLength(100);
            builder.Property(x => x.State)
                   .IsRequired()
                   .HasMaxLength(50);
            builder.HasOne(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
