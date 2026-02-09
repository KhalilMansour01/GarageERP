using GarageERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GarageERP.Infrastructure.Persistence.Configurations;

public class JobConfig : IEntityTypeConfiguration<Job>
{
    public void Configure(EntityTypeBuilder<Job> builder)
    {
        builder.ToTable("Jobs");

        builder.HasKey(j => j.Id);

        builder.Property(j => j.NetCost)
               .HasPrecision(18, 2)
               .IsRequired();

        builder.Property(j => j.Date)
               .IsRequired();

        builder.HasOne(j => j.Vehicle)
               .WithMany(v => v.Jobs)
               .HasForeignKey(j => j.VehicleId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(j => j.Service)
               .WithMany(s => s.Jobs)
               .HasForeignKey(j => j.ServiceId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
