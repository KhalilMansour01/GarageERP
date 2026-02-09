using GarageERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GarageERP.Infrastructure.Persistence.Configurations;

public class PartsUsedConfig : IEntityTypeConfiguration<PartsUsed>
{
    public void Configure(EntityTypeBuilder<PartsUsed> builder)
    {
        builder.ToTable("PartsUsed");

        builder.HasKey(pu => pu.Id);

        builder.HasOne(pu => pu.Part)
               .WithMany(p => p.PartsUsed)
               .HasForeignKey(pu => pu.PartId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(pu => pu.Service)
               .WithMany(s => s.PartsUsed)
               .HasForeignKey(pu => pu.ServiceId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
