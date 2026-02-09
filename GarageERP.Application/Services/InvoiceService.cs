using GarageERP.Application.Interfaces;
using GarageERP.Domain.Entities;
using GarageERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GarageERP.Application.Services;

public class InvoiceService : IInvoiceService
{
    private readonly GarageDbContext _context;

    public InvoiceService(GarageDbContext context)
    {
        _context = context;
    }

    // Basic CRUD
    public async Task<IEnumerable<Invoice>> GetAllInvoicesAsync()
    {
        return await _context.Invoices
            .Include(i => i.Jobs)
                .ThenInclude(j => j.Service)
            .Include(i => i.Jobs)
                .ThenInclude(j => j.Vehicle)
                    .ThenInclude(v => v.Customer)
            .OrderByDescending(i => i.DateIssued)
            .ToListAsync();
    }

    public async Task<Invoice?> GetInvoiceByIdAsync(int id)
    {
        return await _context.Invoices
            .Include(i => i.Jobs)
                .ThenInclude(j => j.Service)
            .Include(i => i.Jobs)
                .ThenInclude(j => j.Vehicle)
                    .ThenInclude(v => v.Customer)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<Invoice> CreateInvoiceAsync(Invoice invoice)
    {
        invoice.DateIssued = DateTime.Now;
        invoice.FinalPrice = await CalculateFinalPriceAsync(invoice.NetPrice, invoice.Discount);

        _context.Invoices.Add(invoice);
        await _context.SaveChangesAsync();
        return invoice;
    }

    public async Task<Invoice> UpdateInvoiceAsync(Invoice invoice)
    {
        invoice.FinalPrice = await CalculateFinalPriceAsync(invoice.NetPrice, invoice.Discount);

        _context.Invoices.Update(invoice);
        await _context.SaveChangesAsync();
        return invoice;
    }

    public async Task<bool> DeleteInvoiceAsync(int id)
    {
        var invoice = await _context.Invoices.FindAsync(id);
        if (invoice == null) return false;

        _context.Invoices.Remove(invoice);
        await _context.SaveChangesAsync();
        return true;
    }

    // Business Logic
    public async Task<IEnumerable<Invoice>> GetInvoicesByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Invoices
            .Where(i => i.DateIssued >= startDate && i.DateIssued <= endDate)
            .Include(i => i.Jobs)
                .ThenInclude(j => j.Service)
            .OrderByDescending(i => i.DateIssued)
            .ToListAsync();
    }

    public async Task<IEnumerable<Invoice>> GetInvoicesByMonthAsync(int year, int month)
    {
        return await _context.Invoices
            .Where(i => i.DateIssued.Year == year && i.DateIssued.Month == month)
            .Include(i => i.Jobs)
            .OrderByDescending(i => i.DateIssued)
            .ToListAsync();
    }

    public Task<decimal> CalculateFinalPriceAsync(decimal netPrice, int discount)
    {
        if (discount < 0 || discount > 100)
            discount = 0;

        var discountAmount = netPrice * (discount / 100m);
        var finalPrice = netPrice - discountAmount;
        return Task.FromResult(Math.Round(finalPrice, 2));
    }

    public async Task<decimal> GetTotalRevenueAsync()
    {
        return await _context.Invoices
            .SumAsync(i => i.FinalPrice);
    }

    public async Task<decimal> GetRevenueByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Invoices
            .Where(i => i.DateIssued >= startDate && i.DateIssued <= endDate)
            .SumAsync(i => i.FinalPrice);
    }

    public async Task<int> GetInvoiceCountByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Invoices
            .CountAsync(i => i.DateIssued >= startDate && i.DateIssued <= endDate);
    }

    public async Task<Invoice> GenerateInvoiceFromJobsAsync(List<int> jobIds, int discount = 0)
    {
        var jobs = await _context.Jobs
            .Where(j => jobIds.Contains(j.Id))
            .Include(j => j.Service)
            .ToListAsync();

        if (!jobs.Any())
            throw new InvalidOperationException("No jobs found with the provided IDs");

        var netPrice = jobs.Sum(j => j.NetCost);
        var finalPrice = await CalculateFinalPriceAsync(netPrice, discount);

        var invoice = new Invoice
        {
            DateIssued = DateTime.Now,
            NetPrice = netPrice,
            Discount = discount,
            FinalPrice = finalPrice,
            Jobs = jobs
        };

        _context.Invoices.Add(invoice);
        await _context.SaveChangesAsync();

        return invoice;
    }
}