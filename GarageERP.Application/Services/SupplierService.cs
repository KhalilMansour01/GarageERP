using GarageERP.Application.Interfaces;
using GarageERP.Domain.Entities;
using GarageERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GarageERP.Application.Services;

public class SupplierService : ISupplierService
{
    private readonly GarageDbContext _context;

    public SupplierService(GarageDbContext context)
    {
        _context = context;
    }

    // Basic CRUD
    public async Task<IEnumerable<Supplier>> GetAllSuppliersAsync()
    {
        return await _context.Suppliers
            .OrderBy(s => s.Name)
            .ToListAsync();
    }

    public async Task<Supplier?> GetSupplierByIdAsync(int id)
    {
        return await _context.Suppliers
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Supplier> CreateSupplierAsync(Supplier supplier)
    {
        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync();
        return supplier;
    }

    public async Task<Supplier> UpdateSupplierAsync(Supplier supplier)
    {
        _context.Suppliers.Update(supplier);
        await _context.SaveChangesAsync();
        return supplier;
    }

    public async Task<bool> DeleteSupplierAsync(int id)
    {
        var supplier = await _context.Suppliers.FindAsync(id);
        if (supplier == null) return false;

        // Check if supplier has parts
        var hasParts = await _context.Parts.AnyAsync(p => p.SupplierId == id);
        if (hasParts)
            return false; // Cannot delete supplier with existing parts

        _context.Suppliers.Remove(supplier);
        await _context.SaveChangesAsync();
        return true;
    }

    // Business Logic
    public async Task<Supplier?> GetSupplierWithPartsAsync(int id)
    {
        return await _context.Suppliers
            .Include(s => s.Parts)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Supplier>> SearchSuppliersByNameAsync(string name)
    {
        return await _context.Suppliers
            .Where(s => s.Name.Contains(name))
            .OrderBy(s => s.Name)
            .ToListAsync();
    }

    public async Task<int> GetPartCountBySupplierAsync(int supplierId)
    {
        return await _context.Parts
            .CountAsync(p => p.SupplierId == supplierId);
    }

    public async Task<decimal> GetTotalInventoryValueBySupplierAsync(int supplierId)
    {
        return await _context.Parts
            .Where(p => p.SupplierId == supplierId)
            .SumAsync(p => p.Inventory * p.BuyPrice);
    }
}