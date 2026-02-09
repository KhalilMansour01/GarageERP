namespace GarageERP.Application.Interfaces;

using GarageERP.Domain.Entities;

public interface IContractRepository
{
    Task<Contract?> GetByIdAsync(int id);
    Task<List<Contract>> GetByVehicleIdAsync(int vehicleId);
    Task<List<Contract>> GetByCustomerIdAsync(int customerId);
    Task<List<Contract>> GetAllAsync();

    Task AddAsync(Contract contract);
    Task UpdateAsync(Contract contract);
    Task DeleteAsync(int id);
}
