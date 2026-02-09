using GarageERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GarageERP.Infrastructure.Persistence.Configurations;

public class SupplierConfig : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.ToTable("Suppliers");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
               .IsRequired()
               .HasMaxLength(150);

        builder.Property(s => s.Address)
               .HasMaxLength(500);

        builder.HasMany(s => s.Parts)
               .WithOne(p => p.Supplier)
               .HasForeignKey(p => p.SupplierId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
