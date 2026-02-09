using GarageERP.Application.Interfaces;
using GarageERP.Domain.Entities;
using GarageERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GarageERP.Infrastructure.Repositories;

public class SupplierRepository : ISupplierRepository
{
    private readonly GarageDbContext _context;

    public SupplierRepository(GarageDbContext context)
    {
        _context = context;
    }

    public async Task<Supplier?> GetByIdAsync(int id)
        => await _context.Suppliers.FindAsync(id);

    public async Task<List<Supplier>> GetAllAsync()
        => await _context.Suppliers.OrderBy(s => s.Name).ToListAsync();

    public async Task AddAsync(Supplier supplier)
    {
        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Supplier supplier)
    {
        _context.Suppliers.Update(supplier);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var supplier = await _context.Suppliers.FindAsync(id);
        if (supplier == null) return false;

        var hasParts = await _context.Parts.AnyAsync(p => p.SupplierId == id);
        if (hasParts) return false;

        _context.Suppliers.Remove(supplier);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Supplier?> GetSupplierWithPartsAsync(int id)
        => await _context.Suppliers
            .Include(s => s.Parts)
            .FirstOrDefaultAsync(s => s.Id == id);

    public async Task<IEnumerable<Supplier>> SearchSuppliersByNameAsync(string name)
        => await _context.Suppliers
            .Where(s => s.Name.Contains(name))
            .OrderBy(s => s.Name)
            .ToListAsync();

    public async Task<int> GetPartCountBySupplierAsync(int supplierId)
        => await _context.Parts.CountAsync(p => p.SupplierId == supplierId);

    public async Task<decimal> GetTotalInventoryValueBySupplierAsync(int supplierId)
        => await _context.Parts
            .Where(p => p.SupplierId == supplierId)
            .SumAsync(p => p.Inventory * p.BuyPrice);
}
