using GarageERP.Application.Interfaces;
using GarageERP.Domain.Entities;
using GarageERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GarageERP.Infrastructure.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly GarageDbContext _context;

    public VehicleRepository(GarageDbContext context)
    {
        _context = context;
    }

    public async Task<Vehicle?> GetByIdAsync(int id)
        => await _context.Vehicles.FindAsync(id);

    public async Task<List<Vehicle>> GetByCustomerIdAsync(int customerId)
        => await _context.Vehicles
            .Where(v => v.CustomerId == customerId)
            .ToListAsync();

    public async Task<List<Vehicle>> GetAllAsync()
        => await _context.Vehicles.ToListAsync();

    public async Task AddAsync(Vehicle vehicle)
    {
        _context.Vehicles.Add(vehicle);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Vehicle vehicle)
    {
        _context.Vehicles.Update(vehicle);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var vehicle = await _context.Vehicles.FindAsync(id);
        if (vehicle == null) return;

        _context.Vehicles.Remove(vehicle);
        await _context.SaveChangesAsync();
    }
}
