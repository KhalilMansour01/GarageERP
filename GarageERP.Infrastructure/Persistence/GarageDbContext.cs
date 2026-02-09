using System;
using System.Collections.Generic;
using System.Text;
using GarageERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using GarageERP.Infrastructure.Persistence.Configurations;


namespace GarageERP.Infrastructure.Persistence
{
    public class GarageDbContext : DbContext
    {
        public GarageDbContext(DbContextOptions<GarageDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Vehicle> Vehicles => Set<Vehicle>();
        public DbSet<Contract> Contracts => Set<Contract>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CustomerConfig());
            modelBuilder.ApplyConfiguration(new VehicleConfig());
            modelBuilder.ApplyConfiguration(new ContractConfig());
            modelBuilder.ApplyConfiguration(new InvoiceConfig());
            modelBuilder.ApplyConfiguration(new JobConfig());
            modelBuilder.ApplyConfiguration(new PartConfig());
            modelBuilder.ApplyConfiguration(new SupplierConfig());
            modelBuilder.ApplyConfiguration(new ServiceConfig());
            modelBuilder.ApplyConfiguration(new PartsUsedConfig());
        }
    }
}
