using GarageERP.Domain.Entities;

namespace GarageERP.Application.Interfaces;

public interface IPartService
{
    // Basic CRUD
    Task<IEnumerable<Part>> GetAllPartsAsync();
    Task<Part?> GetPartByIdAsync(int id);
    Task<Part> CreatePartAsync(Part part);
    Task<Part> UpdatePartAsync(Part part);
    Task<bool> DeletePartAsync(int id);

    // Inventory Management
    Task<IEnumerable<Part>> GetPartsBySupplierId(int supplierId);
    Task<IEnumerable<Part>> GetLowStockPartsAsync(int threshold = 10);
    Task<IEnumerable<Part>> GetOutOfStockPartsAsync();
    Task<bool> UpdateInventoryAsync(int partId, int quantity);
    Task<bool> AddStockAsync(int partId, int quantity);
    Task<bool> RemoveStockAsync(int partId, int quantity);
    Task<decimal> GetPartProfitMarginAsync(int partId);
    Task<IEnumerable<Part>> SearchPartsByNameAsync(string name);
}