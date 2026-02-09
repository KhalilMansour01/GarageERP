using GarageERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GarageERP.Infrastructure.Persistence.Configurations;

public class CustomerConfig : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
               .IsRequired()
               .HasMaxLength(150);

        builder.Property(c => c.Phone)
               .IsRequired()
               .HasMaxLength(30);

        builder.Property(c => c.Email)
               .HasMaxLength(150);

        builder.Property(c => c.Type)
               .IsRequired();

        builder.Property(c => c.Balance)
               .HasPrecision(18, 2);

        builder.Property(c => c.isActive)
               .HasDefaultValue(true);
    }
}
