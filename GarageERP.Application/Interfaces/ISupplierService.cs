using GarageERP.Domain.Entities;

namespace GarageERP.Application.Interfaces;

public interface ISupplierService
{
    // Basic CRUD
    Task<IEnumerable<Supplier>> GetAllSuppliersAsync();
    Task<Supplier?> GetSupplierByIdAsync(int id);
    Task<Supplier> CreateSupplierAsync(Supplier supplier);
    Task<Supplier> UpdateSupplierAsync(Supplier supplier);
    Task<bool> DeleteSupplierAsync(int id);

    // Business Logic
    Task<Supplier?> GetSupplierWithPartsAsync(int id);
    Task<IEnumerable<Supplier>> SearchSuppliersByNameAsync(string name);
    Task<int> GetPartCountBySupplierAsync(int supplierId);
    Task<decimal> GetTotalInventoryValueBySupplierAsync(int supplierId);
}