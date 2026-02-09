namespace GarageERP.Application.Interfaces;

using GarageERP.Domain.Entities;

public interface ISupplierRepository
{
    Task<Supplier?> GetByIdAsync(int id);
    Task<List<Supplier>> GetAllAsync();
    Task<List<Supplier>> SearchByNameAsync(string name);
    Task AddAsync(Supplier supplier);
    Task UpdateAsync(Supplier supplier);
    Task DeleteAsync(int id);
}