namespace GarageERP.Application.Interfaces;

using GarageERP.Domain.Entities;

public interface IJobService
{
    Task<Job?> GetByIdAsync(int id);
    Task<List<Job>> GetAllAsync();
    Task<List<Job>> GetByVehicleIdAsync(int vehicleId);
    Task<List<Job>> GetByServiceIdAsync(int serviceId);
    Task<List<Job>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<List<Job>> GetPendingAsync();
    Task AddAsync(Job job);
    Task UpdateAsync(Job job);
    Task DeleteAsync(int id);
}