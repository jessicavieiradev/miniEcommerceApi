using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using miniEcommerceApi.Models;

namespace miniEcommerceApi.Data.Configurations
{
    public class CostumersConfig : IEntityTypeConfiguration<Costumers>
    {
        public void Configure(EntityTypeBuilder<Costumers> builder)
        {
            builder.ToTable("Costumers");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.FirstName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.LastName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Phone).IsRequired().HasMaxLength(20);
            builder.Property(x => x.Cpf).IsRequired().HasMaxLength(20);
        }
    }
}
