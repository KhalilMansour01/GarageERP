namespace GarageERP.Application.Interfaces;

using GarageERP.Domain.Entities;

public interface IPartRepository
{
    Task<Part?> GetByIdAsync(int id);
    Task<List<Part>> GetAllAsync();
    Task<List<Part>> GetBySupplierIdAsync(int supplierId);
    Task<List<Part>> SearchByNameAsync(string name);
    Task<List<Part>> GetLowStockAsync(int threshold);
    Task<List<Part>> GetOutOfStockAsync();
    Task AddAsync(Part part);
    Task UpdateAsync(Part part);
    Task DeleteAsync(int id);
}