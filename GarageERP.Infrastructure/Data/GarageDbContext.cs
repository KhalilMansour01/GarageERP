using GarageERP.Application.Services;
using GarageERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace GarageERP.Infrastructure.Data;

public class GarageDbContext : DbContext
{
    public GarageDbContext(DbContextOptions<GarageDbContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Contract> Contracts { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Part> Parts { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<PartsUsed> PartsUsed { get; set; }
    public DbSet<Job> Jobs { get; set; }
    public DbSet<Invoice> Invoices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Customer configuration
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.Balance).HasColumnType("decimal(18,2)");
        });

        // Vehicle configuration
        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Customer)
                .WithMany(c => c.Vehicles)
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.Property(e => e.Brand).HasMaxLength(100);
            entity.Property(e => e.Model).HasMaxLength(100);
            entity.Property(e => e.Residence).HasMaxLength(200);
            entity.Property(e => e.UseType).HasMaxLength(50);
            entity.Property(e => e.EngineNumber).HasMaxLength(100);
            entity.Property(e => e.ChasisNumber).HasMaxLength(100);
            entity.Property(e => e.Color).HasMaxLength(50);
            entity.Property(e => e.Shape).HasMaxLength(50);
            entity.Property(e => e.EnginePower).HasMaxLength(50);
        });

        // Contract configuration
        modelBuilder.Entity<Contract>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Vehicle)
                .WithMany(v => v.Contracts)
                .HasForeignKey(e => e.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Supplier configuration
        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Address).HasMaxLength(500);
        });

        // Part configuration
        modelBuilder.Entity<Part>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.PriceSingle).HasColumnType("decimal(18,2)");
            entity.Property(e => e.PriceBulk).HasColumnType("decimal(18,2)");
            entity.Property(e => e.BuyPrice).HasColumnType("decimal(18,2)");
            entity.HasOne(e => e.Supplier)
                .WithMany(s => s.Parts)
                .HasForeignKey(e => e.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Service configuration
        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Cost).HasColumnType("decimal(18,2)");
        });

        // PartsUsed configuration (Many-to-Many between Part and Service)
        modelBuilder.Entity<PartsUsed>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Part)
                .WithMany(p => p.PartsUsed)
                .HasForeignKey(e => e.PartId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Service)
                .WithMany(s => s.PartsUsed)
                .HasForeignKey(e => e.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Job configuration
        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NetCost).HasColumnType("decimal(18,2)");
            entity.HasOne(e => e.Vehicle)
                .WithMany(v => v.Jobs)
                .HasForeignKey(e => e.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Service)
                .WithMany(s => s.Jobs)
                .HasForeignKey(e => e.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Invoice configuration
        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NetPrice).HasColumnType("decimal(18,2)");
            entity.Property(e => e.FinalPrice).HasColumnType("decimal(18,2)");
            entity.HasMany(e => e.Jobs)
                .WithMany(j => j.Invoices)
                .UsingEntity(j => j.ToTable("InvoiceJobs"));
        });
    }
}