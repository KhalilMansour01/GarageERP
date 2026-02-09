using GarageERP.Domain.Entities;

namespace GarageERP.Application.Interfaces;

public interface IServiceRepository
{
    // Basic CRUD
    Task<IEnumerable<Service>> GetAllAsync();
    Task<Service?> GetByIdAsync(int id);
    Task AddAsync(Service service);
    Task UpdateAsync(Service service);
    Task DeleteAsync(int id);

    // Optional: include Jobs and PartsUsed if needed
    Task<Service?> GetServiceWithDetailsAsync(int id);
}
