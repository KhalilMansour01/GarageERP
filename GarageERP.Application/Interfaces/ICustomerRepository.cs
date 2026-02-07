using GarageERP.Domain.Entities;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(int id);
    Task<List<Customer>> GetAllAsync();
    Task AddAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<List<Customer>> SearchAsync(string searchTerm);
}