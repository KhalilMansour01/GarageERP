using GarageERP.Application.Interfaces;
using GarageERP.Domain.Entities;
using GarageERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GarageERP.Application.Services;

public class JobService : IJobService
{
    private readonly GarageDbContext _context;

    public JobService(GarageDbContext context)
    {
        _context = context;
    }

    // Basic CRUD
    public async Task<IEnumerable<Job>> GetAllJobsAsync()
    {
        return await _context.Jobs
            .Include(j => j.Vehicle)
            .Include(j => j.Service)
            .OrderByDescending(j => j.Date)
            .ToListAsync();
    }

    public async Task<Job?> GetJobByIdAsync(int id)
    {
        return await _context.Jobs
            .Include(j => j.Vehicle)
                .ThenInclude(v => v.Customer)
            .Include(j => j.Service)
                .ThenInclude(s => s.PartsUsed)
                    .ThenInclude(pu => pu.Part)
            .Include(j => j.Invoices)
            .FirstOrDefaultAsync(j => j.Id == id);
    }

    public async Task<Job> CreateJobAsync(Job job)
    {
        job.Date = DateTime.Now;
        _context.Jobs.Add(job);
        await _context.SaveChangesAsync();
        return job;
    }

    public async Task<Job> UpdateJobAsync(Job job)
    {
        _context.Jobs.Update(job);
        await _context.SaveChangesAsync();
        return job;
    }

    public async Task<bool> DeleteJobAsync(int id)
    {
        var job = await _context.Jobs.FindAsync(id);
        if (job == null) return false;

        _context.Jobs.Remove(job);
        await _context.SaveChangesAsync();
        return true;
    }

    // Business Logic
    public async Task<IEnumerable<Job>> GetJobsByVehicleIdAsync(int vehicleId)
    {
        return await _context.Jobs
            .Where(j => j.VehicleId == vehicleId)
            .Include(j => j.Service)
            .Include(j => j.Invoices)
            .OrderByDescending(j => j.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<Job>> GetJobsByServiceIdAsync(int serviceId)
    {
        return await _context.Jobs
            .Where(j => j.ServiceId == serviceId)
            .Include(j => j.Vehicle)
                .ThenInclude(v => v.Customer)
            .OrderByDescending(j => j.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<Job>> GetJobsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Jobs
            .Where(j => j.Date >= startDate && j.Date <= endDate)
            .Include(j => j.Vehicle)
            .Include(j => j.Service)
            .OrderByDescending(j => j.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<Job>> GetPendingJobsAsync()
    {
        return await _context.Jobs
            .Where(j => !j.Invoices.Any())
            .Include(j => j.Vehicle)
                .ThenInclude(v => v.Customer)
            .Include(j => j.Service)
            .OrderBy(j => j.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<Job>> GetCompletedJobsAsync()
    {
        return await _context.Jobs
            .Where(j => j.Invoices.Any())
            .Include(j => j.Vehicle)
            .Include(j => j.Service)
            .Include(j => j.Invoices)
            .OrderByDescending(j => j.Date)
            .ToListAsync();
    }

    public async Task<decimal> CalculateJobTotalCostAsync(int jobId)
    {
        var job = await _context.Jobs
            .Include(j => j.Service)
                .ThenInclude(s => s.PartsUsed)
                    .ThenInclude(pu => pu.Part)
            .FirstOrDefaultAsync(j => j.Id == jobId);

        if (job == null) return 0;

        decimal serviceCost = job.Service?.Cost ?? 0;
        decimal partsCost = job.Service?.PartsUsed.Sum(pu => pu.Part?.PriceSingle ?? 0) ?? 0;

        return serviceCost + partsCost;
    }

    public async Task<decimal> GetTotalRevenueByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Jobs
            .Where(j => j.Date >= startDate && j.Date <= endDate)
            .SumAsync(j => j.NetCost);
    }
}