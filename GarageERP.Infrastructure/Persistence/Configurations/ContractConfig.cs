using GarageERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GarageERP.Infrastructure.Persistence.Configurations;

public class ContractConfig : IEntityTypeConfiguration<Contract>
{
    public void Configure(EntityTypeBuilder<Contract> builder)
    {
        builder.ToTable("Contracts");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.VehicleId)
               .IsRequired();

        builder.Property(c => c.Discount)
               .IsRequired();

        builder.Property(c => c.DateIssued)
               .IsRequired();

        builder.Property(c => c.ExpDate)
               .IsRequired();

        builder.Property(c => c.Status)
               .IsRequired();

        builder.Property(c => c.Description)
               .HasMaxLength(500);

        // FK (no navigation property)
        builder.HasIndex(c => c.VehicleId);
    }
}
