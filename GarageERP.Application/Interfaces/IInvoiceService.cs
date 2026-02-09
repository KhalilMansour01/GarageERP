namespace GarageERP.Application.Interfaces;

using GarageERP.Domain.Entities;

public interface IInvoiceRepository
{
    Task<Invoice?> GetByIdAsync(int id);
    Task<List<Invoice>> GetAllAsync();
    Task<List<Invoice>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<List<Invoice>> GetByMonthAsync(int year, int month);
    Task AddAsync(Invoice invoice);
    Task UpdateAsync(Invoice invoice);
    Task DeleteAsync(int id);
}