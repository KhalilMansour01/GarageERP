using GarageERP.Application.Interfaces;
using GarageERP.Domain.Entities;
using GarageERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GarageERP.Application.Services;

public class PartService : IPartService
{
    private readonly GarageDbContext _context;

    public PartService(GarageDbContext context)
    {
        _context = context;
    }

    // Basic CRUD
    public async Task<IEnumerable<Part>> GetAllPartsAsync()
    {
        return await _context.Parts
            .Include(p => p.Supplier)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<Part?> GetPartByIdAsync(int id)
    {
        return await _context.Parts
            .Include(p => p.Supplier)
            .Include(p => p.PartsUsed)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Part> CreatePartAsync(Part part)
    {
        _context.Parts.Add(part);
        await _context.SaveChangesAsync();
        return part;
    }

    public async Task<Part> UpdatePartAsync(Part part)
    {
        _context.Parts.Update(part);
        await _context.SaveChangesAsync();
        return part;
    }

    public async Task<bool> DeletePartAsync(int id)
    {
        var part = await _context.Parts.FindAsync(id);
        if (part == null) return false;

        _context.Parts.Remove(part);
        await _context.SaveChangesAsync();
        return true;
    }

    // Inventory Management
    public async Task<IEnumerable<Part>> GetPartsBySupplierId(int supplierId)
    {
        return await _context.Parts
            .Where(p => p.SupplierId == supplierId)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Part>> GetLowStockPartsAsync(int threshold = 10)
    {
        return await _context.Parts
            .Include(p => p.Supplier)
            .Where(p => p.Inventory > 0 && p.Inventory <= threshold)
            .OrderBy(p => p.Inventory)
            .ToListAsync();
    }

    public async Task<IEnumerable<Part>> GetOutOfStockPartsAsync()
    {
        return await _context.Parts
            .Include(p => p.Supplier)
            .Where(p => p.Inventory == 0)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<bool> UpdateInventoryAsync(int partId, int quantity)
    {
        var part = await _context.Parts.FindAsync(partId);
        if (part == null) return false;

        part.Inventory = quantity;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddStockAsync(int partId, int quantity)
    {
        var part = await _context.Parts.FindAsync(partId);
        if (part == null) return false;

        part.Inventory += quantity;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveStockAsync(int partId, int quantity)
    {
        var part = await _context.Parts.FindAsync(partId);
        if (part == null) return false;

        if (part.Inventory < quantity) return false;

        part.Inventory -= quantity;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<decimal> GetPartProfitMarginAsync(int partId)
    {
        var part = await _context.Parts.FindAsync(partId);
        if (part == null) return 0;

        if (part.BuyPrice == 0) return 0;

        var profitMargin = ((part.PriceSingle - part.BuyPrice) / part.BuyPrice) * 100;
        return profitMargin;
    }

    public async Task<IEnumerable<Part>> SearchPartsByNameAsync(string name)
    {
        return await _context.Parts
            .Include(p => p.Supplier)
            .Where(p => p.Name.Contains(name))
            .OrderBy(p => p.Name)
            .ToListAsync();
    }
}