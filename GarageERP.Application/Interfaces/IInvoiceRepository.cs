using GarageERP.Domain.Entities;

namespace GarageERP.Application.Interfaces;

public interface IInvoiceRepository
{
    // CRUD
    Task<List<Invoice>> GetAllAsync();
    Task<Invoice?> GetByIdAsync(int id);
    Task AddAsync(Invoice invoice);
    Task UpdateAsync(Invoice invoice);
    Task DeleteAsync(int id);

    // Queries
    Task<List<Invoice>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<List<Invoice>> GetByMonthAsync(int year, int month);

    // Aggregates
    Task<decimal> GetTotalRevenueAsync();
    Task<decimal> GetRevenueByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<int> GetInvoiceCountByDateRangeAsync(DateTime startDate, DateTime endDate);

    // Special cases
    Task<Invoice> GenerateFromJobsAsync(List<int> jobIds, int discount);

    /*
    CalculateFinalPriceAsync is pure business logic → later it belongs in a Service, not the repository.
    For now, we can keep it private inside the repo.
    */
}
