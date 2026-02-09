using GarageERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GarageERP.Infrastructure.Persistence.Configurations;

public class VehicleConfig : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable("Vehicles");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Number)
               .IsRequired();

        builder.Property(v => v.CustomerId)
               .IsRequired();

        builder.Property(v => v.Brand)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(v => v.Model)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(v => v.Residence)
               .HasMaxLength(100);

        builder.Property(v => v.UseType)
               .HasMaxLength(50);

        builder.Property(v => v.EngineNumber)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(v => v.ChasisNumber)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(v => v.Color)
               .HasMaxLength(50);

        builder.Property(v => v.Shape)
               .HasMaxLength(50);

        builder.Property(v => v.EnginePower)
               .HasMaxLength(50);

        builder.Property(v => v.Year)
               .IsRequired();

        builder.Property(v => v.DateOfOwnership)
               .IsRequired();

        builder.Property(v => v.DateOfOperation)
               .IsRequired();

        // FK (no navigation property required)
        builder.HasIndex(v => new { v.CustomerId, v.Number })
               .IsUnique();
                // one customer cannot register the same vehicle number twice
    }
}
