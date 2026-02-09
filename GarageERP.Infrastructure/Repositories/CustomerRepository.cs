using GarageERP.Application.Interfaces;
using GarageERP.Domain.Entities;
using GarageERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GarageERP.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly GarageDbContext _context;

    public CustomerRepository(GarageDbContext context)
    {
        _context = context;
    }

    public async Task<Customer?> GetByIdAsync(int id)
        => await _context.Customers.FindAsync(id);

    public async Task<List<Customer>> GetAllAsync()
        => await _context.Customers.ToListAsync();

    public async Task AddAsync(Customer customer)
    {
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Customer customer)
    {
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var customer = await GetByIdAsync(id);
        if (customer == null) return;

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
        => await _context.Customers.AnyAsync(c => c.Id == id);

    public async Task<List<Customer>> SearchAsync(string searchTerm)
    {
        searchTerm = searchTerm.ToLower();

        return await _context.Customers
            .Where(c =>
                c.Name.ToLower().Contains(searchTerm) ||
                c.Phone.ToLower().Contains(searchTerm) ||
                c.Email.ToLower().Contains(searchTerm))
            .ToListAsync();
    }
}
