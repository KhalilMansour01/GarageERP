using GarageERP.Domain.Entities;
using GarageERP.Application.Interfaces;

namespace GarageERP.Application.Services;

public class PartService
{
    private readonly IPartService _partRepo;
    private readonly ISupplierService _supplierRepo;

    public PartService(
        IPartService partRepo,
        ISupplierService supplierRepo)
    {
        _partRepo = partRepo;
        _supplierRepo = supplierRepo;
    }

    public async Task<Part> GetByIdAsync(int id)
    {
        var part = await _partRepo.GetByIdAsync(id);
        if (part == null)
            throw new Exception("Part does not exist");
        return part;
    }

    public async Task<List<Part>> GetAllAsync()
    {
        return await _partRepo.GetAllAsync();
    }

    public async Task<List<Part>> GetBySupplierIdAsync(int supplierId)
    {
        return await _partRepo.GetBySupplierIdAsync(supplierId);
    }

    public async Task<List<Part>> SearchByNameAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new Exception("Search name cannot be empty");
        return await _partRepo.SearchByNameAsync(name);
    }

    public async Task<List<Part>> GetLowStockAsync(int threshold = 10)
    {
        return await _partRepo.GetLowStockAsync(threshold);
    }

    public async Task<List<Part>> GetOutOfStockAsync()
    {
        return await _partRepo.GetOutOfStockAsync();
    }

    public async Task AddAsync(Part part)
    {
        if (string.IsNullOrWhiteSpace(part.Name))
            throw new Exception("Part name is required");

        if (part.SupplierId <= 0)
            throw new Exception("Supplier ID is required");

        var supplier = await _supplierRepo.GetByIdAsync(part.SupplierId);
        if (supplier == null)
            throw new Exception("Supplier does not exist");

        if (part.PriceSingle < 0 || part.PriceBulk < 0 || part.BuyPrice < 0)
            throw new Exception("Prices cannot be negative");

        await _partRepo.AddAsync(part);
    }

    public async Task UpdateAsync(Part part)
    {
        var existing = await GetByIdAsync(part.Id);

        if (part.PriceSingle < 0 || part.PriceBulk < 0 || part.BuyPrice < 0)
            throw new Exception("Prices cannot be negative");

        existing.Name = part.Name?.Trim() ?? throw new Exception("Name is required");
        existing.PriceSingle = part.PriceSingle;
        existing.PriceBulk = part.PriceBulk;
        existing.BuyPrice = part.BuyPrice;
        existing.Inventory = part.Inventory;
        existing.SupplierId = part.SupplierId;

        await _partRepo.UpdateAsync(existing);
    }

    public async Task DeleteAsync(int id)
    {
        await GetByIdAsync(id);
        await _partRepo.DeleteAsync(id);
    }

    public async Task AddStockAsync(int partId, int quantity)
    {
        if (quantity <= 0)
            throw new Exception("Quantity must be positive");

        var part = await GetByIdAsync(partId);
        part.Inventory += quantity;
        await _partRepo.UpdateAsync(part);
    }

    public async Task RemoveStockAsync(int partId, int quantity)
    {
        if (quantity <= 0)
            throw new Exception("Quantity must be positive");

        var part = await GetByIdAsync(partId);

        if (part.Inventory < quantity)
            throw new Exception("Insufficient stock");

        part.Inventory -= quantity;
        await _partRepo.UpdateAsync(part);
    }

    public decimal GetProfitMargin(Part part)
    {
        if (part.BuyPrice == 0) return 0;
        return ((part.PriceSingle - part.BuyPrice) / part.BuyPrice) * 100;
    }
}