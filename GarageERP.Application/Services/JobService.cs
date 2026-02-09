using GarageERP.Application.Interfaces;
using GarageERP.Domain.Entities;

namespace GarageERP.Application.Services;

public class JobService
{
    private readonly IJobRepository _jobRepo;

    public JobService(IJobRepository jobRepo)
    {
        _jobRepo = jobRepo;
    }

    // Basic CRUD
    public async Task<IEnumerable<Job>> GetAllJobsAsync()
    {
        return await _jobRepo.GetAllAsync();
    }

    public async Task<Job?> GetJobByIdAsync(int id)
    {
        return await _jobRepo.GetByIdAsync(id);
    }

    public async Task<Job> CreateJobAsync(Job job)
    {
        job.Date = DateTime.Now;
        await _jobRepo.AddAsync(job);
        return job;
    }

    public async Task<Job> UpdateJobAsync(Job job)
    {
        await _jobRepo.UpdateAsync(job);
        return job;
    }

    public async Task<bool> DeleteJobAsync(int id)
    {
        return await _jobRepo.DeleteAsync(id);
    }

    // Business Logic
    public async Task<IEnumerable<Job>> GetJobsByVehicleIdAsync(int vehicleId)
    {
        return await _jobRepo.GetByVehicleIdAsync(vehicleId);
    }

    public async Task<IEnumerable<Job>> GetJobsByServiceIdAsync(int serviceId)
    {
        return await _jobRepo.GetByServiceIdAsync(serviceId);
    }

    public async Task<IEnumerable<Job>> GetJobsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _jobRepo.GetByDateRangeAsync(startDate, endDate);
    }

    public async Task<IEnumerable<Job>> GetPendingJobsAsync()
    {
        return await _jobRepo.GetPendingAsync();
    }

    public async Task<IEnumerable<Job>> GetCompletedJobsAsync()
    {
        return await _jobRepo.GetCompletedAsync();
    }

    public async Task<decimal> CalculateJobTotalCostAsync(int jobId)
    {
        return await _jobRepo.CalculateJobTotalCostAsync(jobId);
    }

    public async Task<decimal> GetTotalRevenueByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _jobRepo.GetTotalRevenueByDateRangeAsync(startDate, endDate);
    }
}
