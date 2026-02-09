using GarageERP.Domain.Entities;

namespace GarageERP.Application.Interfaces;

public interface IJobRepository
{
    // Basic CRUD
    Task<IEnumerable<Job>> GetAllAsync();
    Task<Job?> GetByIdAsync(int id);
    Task AddAsync(Job job);
    Task UpdateAsync(Job job);
    Task DeleteAsync(int id);

    // Queries
    Task<IEnumerable<Job>> GetByVehicleIdAsync(int vehicleId);
    Task<IEnumerable<Job>> GetByServiceIdAsync(int serviceId);
    Task<IEnumerable<Job>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<Job>> GetPendingAsync();
    Task<IEnumerable<Job>> GetCompletedAsync();

    // Aggregates
    Task<decimal> CalculateJobTotalCostAsync(int jobId);
    Task<decimal> GetTotalRevenueByDateRangeAsync(DateTime startDate, DateTime endDate);
}
