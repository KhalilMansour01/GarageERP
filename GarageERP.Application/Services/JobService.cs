using GarageERP.Domain.Entities;
using GarageERP.Application.Interfaces;

namespace GarageERP.Application.Services;

public class JobService
{
    private readonly IJobRepository _jobRepo;
    private readonly IVehicleRepository _vehicleRepo;
    private readonly IServiceRepository _serviceRepo;

    public JobService(
        IJobRepository jobRepo,
        IVehicleRepository vehicleRepo,
        IServiceRepository serviceRepo)
    {
        _jobRepo = jobRepo;
        _vehicleRepo = vehicleRepo;
        _serviceRepo = serviceRepo;
    }

    public async Task<Job> GetByIdAsync(int id)
    {
        var job = await _jobRepo.GetByIdAsync(id);
        if (job == null)
            throw new Exception("Job does not exist");
        return job;
    }

    public async Task<List<Job>> GetAllAsync()
    {
        return await _jobRepo.GetAllAsync();
    }

    public async Task<List<Job>> GetByVehicleIdAsync(int vehicleId)
    {
        return await _jobRepo.GetByVehicleIdAsync(vehicleId);
    }

    public async Task<List<Job>> GetByServiceIdAsync(int serviceId)
    {
        return await _jobRepo.GetByServiceIdAsync(serviceId);
    }

    public async Task<List<Job>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _jobRepo.GetByDateRangeAsync(startDate, endDate);
    }

    public async Task<List<Job>> GetPendingAsync()
    {
        return await _jobRepo.GetPendingAsync();
    }

    public async Task AddAsync(int vehicleId, int serviceId, decimal netCost)
    {
        var vehicle = await _vehicleRepo.GetByIdAsync(vehicleId);
        if (vehicle == null)
            throw new Exception("Vehicle does not exist");

        var service = await _serviceRepo.GetByIdAsync(serviceId);
        if (service == null)
            throw new Exception("Service does not exist");

        if (netCost < 0)
            throw new Exception("Net cost cannot be negative");

        var job = new Job
        {
            VehicleId = vehicleId,
            ServiceId = serviceId,
            NetCost = netCost,
            Date = DateTime.Now
        };

        await _jobRepo.AddAsync(job);
    }

    public async Task UpdateAsync(Job job)
    {
        var existing = await GetByIdAsync(job.Id);

        if (job.NetCost < 0)
            throw new Exception("Net cost cannot be negative");

        existing.NetCost = job.NetCost;
        existing.Date = job.Date;

        await _jobRepo.UpdateAsync(existing);
    }

    public async Task DeleteAsync(int id)
    {
        await GetByIdAsync(id);
        await _jobRepo.DeleteAsync(id);
    }

    public async Task<decimal> GetTotalRevenueByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var jobs = await _jobRepo.GetByDateRangeAsync(startDate, endDate);
        return jobs.Sum(j => j.NetCost);
    }
}