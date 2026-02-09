using GarageERP.Application.Interfaces;
using GarageERP.Domain.Entities;

namespace GarageERP.Application.Services;

public class PartService
{
    private readonly IPartRepository _partRepo;

    public PartService(IPartRepository partRepo)
    {
        _partRepo = partRepo;
    }

    // Basic CRUD
    public async Task<IEnumerable<Part>> GetAllPartsAsync()
    {
        return await _partRepo.GetAllAsync();
    }

    public async Task<Part?> GetPartByIdAsync(int id)
    {
        return await _partRepo.GetByIdAsync(id);
    }

    public async Task<Part> CreatePartAsync(Part part)
    {
        await _partRepo.AddAsync(part);
        return part;
    }

    public async Task<Part> UpdatePartAsync(Part part)
    {
        await _partRepo.UpdateAsync(part);
        return part;
    }

    public async Task<bool> DeletePartAsync(int id)
    {
        return await _partRepo.DeleteAsync(id);
    }

    // Inventory Management
    public async Task<IEnumerable<Part>> GetPartsBySupplierId(int supplierId)
    {
        return await _partRepo.GetBySupplierIdAsync(supplierId);
    }

    public async Task<IEnumerable<Part>> GetLowStockPartsAsync(int threshold = 10)
    {
        return await _partRepo.GetLowStockPartsAsync(threshold);
    }

    public async Task<IEnumerable<Part>> GetOutOfStockPartsAsync()
    {
        return await _partRepo.GetOutOfStockPartsAsync();
    }

    public async Task<bool> UpdateInventoryAsync(int partId, int quantity)
    {
        return await _partRepo.UpdateInventoryAsync(partId, quantity);
    }

    public async Task<bool> AddStockAsync(int partId, int quantity)
    {
        return await _partRepo.AddStockAsync(partId, quantity);
    }

    public async Task<bool> RemoveStockAsync(int partId, int quantity)
    {
        return await _partRepo.RemoveStockAsync(partId, quantity);
    }

    public async Task<decimal> GetPartProfitMarginAsync(int partId)
    {
        return await _partRepo.CalculateProfitMarginAsync(partId);
    }

    public async Task<IEnumerable<Part>> SearchPartsByNameAsync(string name)
    {
        return await _partRepo.SearchByNameAsync(name);
    }
}
