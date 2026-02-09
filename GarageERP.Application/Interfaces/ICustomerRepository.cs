namespace GarageERP.Application.Interfaces;

using GarageERP.Domain.Entities;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(int id);
    Task<List<Customer>> GetAllAsync();
    Task<List<Customer>> SearchByNameAsync(string name);

    Task AddAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(int id);
}
