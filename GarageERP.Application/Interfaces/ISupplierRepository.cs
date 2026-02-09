using GarageERP.Domain.Entities;

namespace GarageERP.Application.Interfaces;

public interface ISupplierRepository
{
    // Basic CRUD
    Task<IEnumerable<Supplier>> GetAllAsync();
    Task<Supplier?> GetByIdAsync(int id);
    Task AddAsync(Supplier supplier);
    Task UpdateAsync(Supplier supplier);
    Task DeleteAsync(int id);

    // Queries
    Task<Supplier?> GetWithPartsAsync(int id);
    Task<IEnumerable<Supplier>> SearchByNameAsync(string name);

    // Aggregates
    Task<int> GetPartCountAsync(int supplierId);
    Task<decimal> GetTotalInventoryValueAsync(int supplierId);
}
