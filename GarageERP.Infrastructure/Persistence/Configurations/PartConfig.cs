using GarageERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GarageERP.Infrastructure.Persistence.Configurations;

public class PartConfig : IEntityTypeConfiguration<Part>
{
    public void Configure(EntityTypeBuilder<Part> builder)
    {
        builder.ToTable("Parts");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
               .IsRequired()
               .HasMaxLength(150);

        builder.Property(p => p.PriceSingle)
               .HasPrecision(18, 2)
               .IsRequired();

        builder.Property(p => p.PriceBulk)
               .HasPrecision(18, 2);

        builder.Property(p => p.BuyPrice)
               .HasPrecision(18, 2);

        builder.Property(p => p.Inventory)
               .IsRequired();

        builder.HasOne(p => p.Supplier)
               .WithMany(s => s.Parts)
               .HasForeignKey(p => p.SupplierId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.PartsUsed)
               .WithOne(pu => pu.Part)
               .HasForeignKey(pu => pu.PartId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
