using GarageERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GarageERP.Infrastructure.Persistence.Configurations;

public class ServiceConfig : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.ToTable("Services");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(s => s.Description)
               .HasMaxLength(1000);

        builder.Property(s => s.Cost)
               .HasPrecision(18, 2)
               .IsRequired();

        builder.HasMany(s => s.Jobs)
               .WithOne(j => j.Service)
               .HasForeignKey(j => j.ServiceId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(s => s.PartsUsed)
               .WithOne(pu => pu.Service)
               .HasForeignKey(pu => pu.ServiceId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
