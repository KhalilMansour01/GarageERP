using GarageERP.Domain.Entities;
using GarageERP.Application.Interfaces;

namespace GarageERP.Application.Services;

public class InvoiceService
{
    private readonly IInvoiceService _invoiceRepo;
    private readonly IJobService _jobRepo;

    public InvoiceService(
        IInvoiceService invoiceRepo,
        IJobService jobRepo)
    {
        _invoiceRepo = invoiceRepo;
        _jobRepo = jobRepo;
    }

    public async Task<Invoice> GetByIdAsync(int id)
    {
        var invoice = await _invoiceRepo.GetByIdAsync(id);
        if (invoice == null)
            throw new Exception("Invoice does not exist");
        return invoice;
    }

    public async Task<List<Invoice>> GetAllAsync()
    {
        return await _invoiceRepo.GetAllAsync();
    }

    public async Task<List<Invoice>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _invoiceRepo.GetByDateRangeAsync(startDate, endDate);
    }

    public async Task<List<Invoice>> GetByMonthAsync(int year, int month)
    {
        return await _invoiceRepo.GetByMonthAsync(year, month);
    }

    public async Task AddAsync(decimal netPrice, int discount, List<int> jobIds)
    {
        if (discount < 0 || discount > 100)
            throw new Exception("Discount must be between 0 and 100");

        if (netPrice < 0)
            throw new Exception("Net price cannot be negative");

        var finalPrice = CalculateFinalPrice(netPrice, discount);

        var invoice = new Invoice
        {
            DateIssued = DateTime.Now,
            NetPrice = netPrice,
            Discount = discount,
            FinalPrice = finalPrice
        };

        await _invoiceRepo.AddAsync(invoice);
    }

    public async Task UpdateAsync(Invoice invoice)
    {
        var existing = await GetByIdAsync(invoice.Id);

        if (invoice.Discount < 0 || invoice.Discount > 100)
            throw new Exception("Discount must be between 0 and 100");

        if (invoice.NetPrice < 0)
            throw new Exception("Net price cannot be negative");

        existing.NetPrice = invoice.NetPrice;
        existing.Discount = invoice.Discount;
        existing.FinalPrice = CalculateFinalPrice(invoice.NetPrice, invoice.Discount);

        await _invoiceRepo.UpdateAsync(existing);
    }

    public async Task DeleteAsync(int id)
    {
        await GetByIdAsync(id);
        await _invoiceRepo.DeleteAsync(id);
    }

    public decimal CalculateFinalPrice(decimal netPrice, int discount)
    {
        var discountAmount = netPrice * (discount / 100m);
        return Math.Round(netPrice - discountAmount, 2);
    }

    public async Task<decimal> GetTotalRevenueAsync()
    {
        var invoices = await _invoiceRepo.GetAllAsync();
        return invoices.Sum(i => i.FinalPrice);
    }

    public async Task<decimal> GetRevenueByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var invoices = await _invoiceRepo.GetByDateRangeAsync(startDate, endDate);
        return invoices.Sum(i => i.FinalPrice);
    }
}