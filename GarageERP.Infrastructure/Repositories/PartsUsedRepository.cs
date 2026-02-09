using GarageERP.Application.Interfaces;
using GarageERP.Domain.Entities;
using GarageERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GarageERP.Infrastructure.Repositories;

public class PartsUsedRepository : IPartsUsedRepository
{
    private readonly GarageDbContext _context;

    public PartsUsedRepository(GarageDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PartsUsed>> GetAllAsync()
        => await _context.PartsUsed.ToListAsync();

    public async Task<PartsUsed?> GetByIdAsync(int id)
        => await _context.PartsUsed.FindAsync(id);

    public async Task AddAsync(PartsUsed partsUsed)
    {
        _context.PartsUsed.Add(partsUsed);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(PartsUsed partsUsed)
    {
        _context.PartsUsed.Update(partsUsed);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var partsUsed = await GetByIdAsync(id);
        if (partsUsed == null) return;

        _context.PartsUsed.Remove(partsUsed);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<PartsUsed>> GetByPartIdAsync(int partId)
        => await _context.PartsUsed
            .Where(pu => pu.PartId == partId)
            .Include(pu => pu.Part)
            .Include(pu => pu.Service)
            .ToListAsync();

    public async Task<IEnumerable<PartsUsed>> GetByServiceIdAsync(int serviceId)
        => await _context.PartsUsed
            .Where(pu => pu.ServiceId == serviceId)
            .Include(pu => pu.Part)
            .Include(pu => pu.Service)
            .ToListAsync();
}
