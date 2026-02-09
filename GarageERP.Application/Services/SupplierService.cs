using GarageERP.Domain.Entities;
using GarageERP.Application.Interfaces;

namespace GarageERP.Application.Services;

public class SupplierService
{
    private readonly ISupplierRepository _supplierRepo;

    public SupplierService(ISupplierRepository supplierRepo)
    {
        _supplierRepo = supplierRepo;
    }

    public async Task<Supplier> GetByIdAsync(int id)
    {
        var supplier = await _supplierRepo.GetByIdAsync(id);
        if (supplier == null)
            throw new Exception("Supplier does not exist");
        return supplier;
    }

    public async Task<List<Supplier>> GetAllAsync()
    {
        return await _supplierRepo.GetAllAsync();
    }

    public async Task<List<Supplier>> SearchByNameAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new Exception("Search name cannot be empty");
        return await _supplierRepo.SearchByNameAsync(name);
    }

    public async Task AddAsync(string name, string address)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new Exception("Supplier name is required");

        var supplier = new Supplier
        {
            Name = name.Trim(),
            Address = address?.Trim() ?? ""
        };

        await _supplierRepo.AddAsync(supplier);
    }

    public async Task UpdateAsync(Supplier supplier)
    {
        var existing = await GetByIdAsync(supplier.Id);

        existing.Name = supplier.Name?.Trim() ?? throw new Exception("Name is required");
        existing.Address = supplier.Address?.Trim() ?? "";

        await _supplierRepo.UpdateAsync(existing);
    }

    public async Task DeleteAsync(int id)
    {
        await GetByIdAsync(id);
        await _supplierRepo.DeleteAsync(id);
    }
}