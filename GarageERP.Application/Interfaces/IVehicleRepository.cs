namespace GarageERP.Application.Interfaces;

using GarageERP.Domain.Entities;

public interface IVehicleRepository
{
    Task<Vehicle?> GetByIdAsync(int id);
    Task<List<Vehicle>> GetByCustomerIdAsync(int customerId);
    Task<List<Vehicle>> GetAllAsync();

    Task AddAsync(Vehicle vehicle);
    Task UpdateAsync(Vehicle vehicle);
    Task DeleteAsync(int id);
}
