using GarageERP.Application.Interfaces;
using GarageERP.Domain.Entities;
using GarageERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GarageERP.Infrastructure.Repositories;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly GarageDbContext _context;

    public InvoiceRepository(GarageDbContext context)
    {
        _context = context;
    }

    public async Task<Invoice?> GetByIdAsync(int id)
        => await _context.Invoices
            .Include(i => i.Jobs)
                .ThenInclude(j => j.Service)
            .Include(i => i.Jobs)
                .ThenInclude(j => j.Vehicle)
                    .ThenInclude(v => v.Customer)
            .FirstOrDefaultAsync(i => i.Id == id);

    public async Task<List<Invoice>> GetAllAsync()
        => await _context.Invoices
            .Include(i => i.Jobs)
            .ThenInclude(j => j.Service)
            .ToListAsync();

    public async Task AddAsync(Invoice invoice)
    {
        _context.Invoices.Add(invoice);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Invoice invoice)
    {
        _context.Invoices.Update(invoice);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var invoice = await _context.Invoices.FindAsync(id);
        if (invoice == null) return;

        _context.Invoices.Remove(invoice);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Invoice>> GetInvoicesByDateRangeAsync(DateTime start, DateTime end)
        => await _context.Invoices
            .Where(i => i.DateIssued >= start && i.DateIssued <= end)
            .Include(i => i.Jobs)
            .ThenInclude(j => j.Service)
            .ToListAsync();

    public async Task<IEnumerable<Invoice>> GetInvoicesByMonthAsync(int year, int month)
        => await _context.Invoices
            .Where(i => i.DateIssued.Year == year && i.DateIssued.Month == month)
            .Include(i => i.Jobs)
            .ToListAsync();

    public async Task<decimal> GetTotalRevenueAsync()
        => await _context.Invoices.SumAsync(i => i.FinalPrice);

    public async Task<decimal> GetRevenueByDateRangeAsync(DateTime start, DateTime end)
        => await _context.Invoices
            .Where(i => i.DateIssued >= start && i.DateIssued <= end)
            .SumAsync(i => i.FinalPrice);

    public async Task<int> GetInvoiceCountByDateRangeAsync(DateTime start, DateTime end)
        => await _context.Invoices.CountAsync(i => i.DateIssued >= start && i.DateIssued <= end);
}
