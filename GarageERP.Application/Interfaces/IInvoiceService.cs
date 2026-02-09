using GarageERP.Domain.Entities;

namespace GarageERP.Application.Interfaces;

public interface IInvoiceService
{
    // Basic CRUD
    Task<IEnumerable<Invoice>> GetAllInvoicesAsync();
    Task<Invoice?> GetInvoiceByIdAsync(int id);
    Task<Invoice> CreateInvoiceAsync(Invoice invoice);
    Task<Invoice> UpdateInvoiceAsync(Invoice invoice);
    Task<bool> DeleteInvoiceAsync(int id);

    // Business Logic
    Task<IEnumerable<Invoice>> GetInvoicesByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<Invoice>> GetInvoicesByMonthAsync(int year, int month);
    Task<decimal> CalculateFinalPriceAsync(decimal netPrice, int discount);
    Task<decimal> GetTotalRevenueAsync();
    Task<decimal> GetRevenueByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<int> GetInvoiceCountByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<Invoice> GenerateInvoiceFromJobsAsync(List<int> jobIds, int discount = 0);
}