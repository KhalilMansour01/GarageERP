using GarageERP.Domain.Entities;

namespace GarageERP.Application.Interfaces;

public interface IJobService
{
    // Basic CRUD
    Task<IEnumerable<Job>> GetAllJobsAsync();
    Task<Job?> GetJobByIdAsync(int id);
    Task<Job> CreateJobAsync(Job job);
    Task<Job> UpdateJobAsync(Job job);
    Task<bool> DeleteJobAsync(int id);

    // Business Logic
    Task<IEnumerable<Job>> GetJobsByVehicleIdAsync(int vehicleId);
    Task<IEnumerable<Job>> GetJobsByServiceIdAsync(int serviceId);
    Task<IEnumerable<Job>> GetJobsByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<Job>> GetPendingJobsAsync();
    Task<IEnumerable<Job>> GetCompletedJobsAsync();
    Task<decimal> CalculateJobTotalCostAsync(int jobId);
    Task<decimal> GetTotalRevenueByDateRangeAsync(DateTime startDate, DateTime endDate);
}