using GarageERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GarageERP.Infrastructure.Persistence.Configurations;

public class InvoiceConfig : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("Invoices");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.DateIssued)
               .IsRequired();

        builder.Property(i => i.NetPrice)
               .HasPrecision(18, 2);

        builder.Property(i => i.FinalPrice)
               .HasPrecision(18, 2);

        builder.Property(i => i.Discount)
               .IsRequired();

        builder.HasMany(i => i.Jobs)
               .WithMany(j => j.Invoices)
               .UsingEntity(j => j.ToTable("InvoiceJobs"));
    }
}
