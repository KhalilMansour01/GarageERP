using GarageERP.Application.Interfaces;
using GarageERP.Domain.Entities;
using GarageERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GarageERP.Infrastructure.Repositories;

public class ContractRepository : IContractRepository
{
    private readonly GarageDbContext _context;

    public ContractRepository(GarageDbContext context)
    {
        _context = context;
    }

    public async Task<Contract?> GetByIdAsync(int id)
        => await _context.Contracts.FindAsync(id);

    public async Task<List<Contract>> GetByVehicleIdAsync(int vehicleId)
        => await _context.Contracts
            .Where(c => c.VehicleId == vehicleId)
            .ToListAsync();

    public async Task<List<Contract>> GetByCustomerIdAsync(int customerId)
        => await _context.Contracts
            .Where(c => c.Vehicle.CustomerId == customerId)
            .ToListAsync();

    public async Task<List<Contract>> GetAllAsync()
        => await _context.Contracts.ToListAsync();

    public async Task AddAsync(Contract contract)
    {
        _context.Contracts.Add(contract);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Contract contract)
    {
        _context.Contracts.Update(contract);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var contract = await _context.Contracts.FindAsync(id);
        if (contract == null) return;

        _context.Contracts.Remove(contract);
        await _context.SaveChangesAsync();
    }
}
