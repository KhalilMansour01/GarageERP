using GarageERP.Application.Interfaces;
using GarageERP.Domain.Entities;

namespace GarageERP.Application.Services;

public class InvoiceService : IInvoiceService
{
    private readonly IInvoiceRepository _invoiceRepository;

    public InvoiceService(IInvoiceRepository invoiceRepository)
    {
        _invoiceRepository = invoiceRepository;
    }

    // Basic CRUD
    public async Task<IEnumerable<Invoice>> GetAllInvoicesAsync()
    {
        return await _invoiceRepository.GetAllAsync();
    }

    public async Task<Invoice?> GetInvoiceByIdAsync(int id)
    {
        return await _invoiceRepository.GetByIdAsync(id);
    }

    public async Task<Invoice> CreateInvoiceAsync(Invoice invoice)
    {
        invoice.DateIssued = DateTime.Now;
        invoice.FinalPrice = await CalculateFinalPriceAsync(invoice.NetPrice, invoice.Discount);

        await _invoiceRepository.AddAsync(invoice);
        return invoice;
    }

    public async Task<Invoice> UpdateInvoiceAsync(Invoice invoice)
    {
        invoice.FinalPrice = await CalculateFinalPriceAsync(invoice.NetPrice, invoice.Discount);

        await _invoiceRepository.UpdateAsync(invoice);
        return invoice;
    }

    public async Task<bool> DeleteInvoiceAsync(int id)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(id);
        if (invoice == null) return false;

        await _invoiceRepository.DeleteAsync(id);
        return true;
    }

    // Business Logic
    public async Task<IEnumerable<Invoice>> GetInvoicesByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _invoiceRepository.GetByDateRangeAsync(startDate, endDate);
    }

    public async Task<IEnumerable<Invoice>> GetInvoicesByMonthAsync(int year, int month)
    {
        return await _invoiceRepository.GetByMonthAsync(year, month);
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
        return await _invoiceRepository.GetTotalRevenueAsync();
    }

    public async Task<decimal> GetRevenueByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _invoiceRepository.GetRevenueByDateRangeAsync(startDate, endDate);
    }

    public async Task<int> GetInvoiceCountByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _invoiceRepository.GetInvoiceCountByDateRangeAsync(startDate, endDate);
    }

    public async Task<Invoice> GenerateInvoiceFromJobsAsync(List<int> jobIds, int discount = 0)
    {
        var jobs = await _invoiceRepository.GetJobsByIdsAsync(jobIds);

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

        await _invoiceRepository.AddAsync(invoice);
        return invoice;
    }
}
