namespace GarageERP.Application.Interfaces;

using GarageERP.Domain.Entities;

public interface IServiceRepository
{
    Task<Service?> GetByIdAsync(int id);
    Task<List<Service>> GetAllAsync();
    Task AddAsync(Service service);
    Task UpdateAsync(Service service);
    Task DeleteAsync(int id);
}