using GarageERP.Domain.Entities;

namespace GarageERP.Application.Interfaces;

public interface IPartsUsedRepository
{
    // Basic CRUD
    Task<IEnumerable<PartsUsed>> GetAllAsync();
    Task<PartsUsed?> GetByIdAsync(int id);
    Task AddAsync(PartsUsed partsUsed);
    Task UpdateAsync(PartsUsed partsUsed);
    Task DeleteAsync(int id);

    // Optional: get by PartId or ServiceId
    Task<IEnumerable<PartsUsed>> GetByPartIdAsync(int partId);
    Task<IEnumerable<PartsUsed>> GetByServiceIdAsync(int serviceId);
}
