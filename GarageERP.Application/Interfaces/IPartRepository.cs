using GarageERP.Domain.Entities;

namespace GarageERP.Application.Interfaces;

public interface IPartRepository
{
    // Basic CRUD
    Task<IEnumerable<Part>> GetAllAsync();
    Task<Part?> GetByIdAsync(int id);
    Task AddAsync(Part part);
    Task UpdateAsync(Part part);
    Task DeleteAsync(int id);

    // Inventory
    Task<IEnumerable<Part>> GetBySupplierIdAsync(int supplierId);
    Task<IEnumerable<Part>> GetLowStockAsync(int threshold = 10);
    Task<IEnumerable<Part>> GetOutOfStockAsync();

    Task UpdateInventoryAsync(int partId, int quantity);
    Task AddStockAsync(int partId, int quantity);
    Task RemoveStockAsync(int partId, int quantity);

    // Queries
    Task<IEnumerable<Part>> SearchByNameAsync(string name);

    // Aggregates
    Task<decimal> GetProfitMarginAsync(int partId);
}
