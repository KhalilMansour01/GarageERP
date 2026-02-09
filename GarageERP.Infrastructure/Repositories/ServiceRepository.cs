using GarageERP.Application.Interfaces;
using GarageERP.Domain.Entities;
using GarageERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GarageERP.Infrastructure.Repositories;

public class ServiceRepository : IServiceRepository
{
    private readonly GarageDbContext _context;

    public ServiceRepository(GarageDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Service>> GetAllAsync()
        => await _context.Services.ToListAsync();

    public async Task<Service?> GetByIdAsync(int id)
        => await _context.Services.FindAsync(id);

    public async Task AddAsync(Service service)
    {
        _context.Services.Add(service);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Service service)
    {
        _context.Services.Update(service);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var service = await GetByIdAsync(id);
        if (service == null) return;

        _context.Services.Remove(service);
        await _context.SaveChangesAsync();
    }

    public async Task<Service?> GetServiceWithDetailsAsync(int id)
    {
        return await _context.Services
            .Include(s => s.Jobs)
            .Include(s => s.PartsUsed)
                .ThenInclude(pu => pu.Part)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}
