using GarageERP.Application.Interfaces;
using GarageERP.Domain.Entities;
using GarageERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GarageERP.Infrastructure.Repositories;

public class JobRepository : IJobRepository
{
    private readonly GarageDbContext _context;

    public JobRepository(GarageDbContext context)
    {
        _context = context;
    }

    public async Task<Job?> GetByIdAsync(int id)
        => await _context.Jobs
            .Include(j => j.Vehicle)
                .ThenInclude(v => v.Customer)
            .Include(j => j.Service)
                .ThenInclude(s => s.PartsUsed)
                    .ThenInclude(pu => pu.Part)
            .Include(j => j.Invoices)
            .FirstOrDefaultAsync(j => j.Id == id);

    public async Task<List<Job>> GetAllAsync()
        => await _context.Jobs
            .Include(j => j.Vehicle)
            .Include(j => j.Service)
            .OrderByDescending(j => j.Date)
            .ToListAsync();

    public async Task AddAsync(Job job)
    {
        _context.Jobs.Add(job);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Job job)
    {
        _context.Jobs.Update(job);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var job = await _context.Jobs.FindAsync(id);
        if (job == null) return;

        _context.Jobs.Remove(job);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Job>> GetJobsByVehicleIdAsync(int vehicleId)
        => await _context.Jobs
            .Where(j => j.VehicleId == vehicleId)
            .Include(j => j.Service)
            .Include(j => j.Invoices)
            .OrderByDescending(j => j.Date)
            .ToListAsync();

    public async Task<IEnumerable<Job>> GetJobsByServiceIdAsync(int serviceId)
        => await _context.Jobs
            .Where(j => j.ServiceId == serviceId)
            .Include(j => j.Vehicle)
                .ThenInclude(v => v.Customer)
            .OrderByDescending(j => j.Date)
            .ToListAsync();

    public async Task<IEnumerable<Job>> GetJobsByDateRangeAsync(DateTime start, DateTime end)
        => await _context.Jobs
            .Where(j => j.Date >= start && j.Date <= end)
            .Include(j => j.Vehicle)
            .Include(j => j.Service)
            .OrderByDescending(j => j.Date)
            .ToListAsync();

    public async Task<IEnumerable<Job>> GetPendingJobsAsync()
        => await _context.Jobs
            .Where(j => !j.Invoices.Any())
            .Include(j => j.Vehicle)
                .ThenInclude(v => v.Customer)
            .Include(j => j.Service)
            .OrderBy(j => j.Date)
            .ToListAsync();

    public async Task<IEnumerable<Job>> GetCompletedJobsAsync()
        => await _context.Jobs
            .Where(j => j.Invoices.Any())
            .Include(j => j.Vehicle)
            .Include(j => j.Service)
            .Include(j => j.Invoices)
            .OrderByDescending(j => j.Date)
            .ToListAsync();

    public async Task<decimal> GetTotalRevenueByDateRangeAsync(DateTime start, DateTime end)
        => await _context.Jobs
            .Where(j => j.Date >= start && j.Date <= end)
            .SumAsync(j => j.NetCost);
}
