using GarageERP.Domain.Entities;
using GarageERP.Application.Interfaces;


public class CustomerService
{
    private readonly ICustomerRepository _customerRepo;
    private readonly IVehicleRepository _vehicleRepo;

    public CustomerService(
        ICustomerRepository customerRepo,
        IVehicleRepository vehicleRepo)
    {
        _customerRepo = customerRepo;
        _vehicleRepo = vehicleRepo;
    }

    // READ: single
    public async Task<Customer> GetByIdAsync(int id)
    {
        var customer = await _customerRepo.GetByIdAsync(id);
        if (customer == null)
            throw new Exception("Customer does not exist");

        return customer;
    }

    // READ: all
    public async Task<List<Customer>> GetAllAsync()
    {
        return await _customerRepo.GetAllAsync();
    }

    // CREATE
    public async Task AddAsync(string name, string phone, string? email, int type = 0)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new Exception("Customer name is required");

        if (string.IsNullOrWhiteSpace(phone))
            throw new Exception("Phone number is required");

        var customer = new Customer
        {
            Name = name.Trim(),
            Phone = phone.Trim(),
            Email = email?.Trim() ?? "",
            Type = type,
            Balance = 0m
        };

        await _customerRepo.AddAsync(customer);
    }

    // UPDATE
    public async Task UpdateAsync(Customer customer)
    {
        var existing = await GetByIdAsync(customer.Id);

        if (string.IsNullOrWhiteSpace(customer.Name))
            throw new Exception("Customer name is required");

        if (string.IsNullOrWhiteSpace(customer.Phone))
            throw new Exception("Phone number is required");

        existing.Name = customer.Name.Trim();
        existing.Phone = customer.Phone.Trim();
        existing.Email = customer.Email?.Trim() ?? "";
        existing.Type = customer.Type;

        await _customerRepo.UpdateAsync(existing);
    }

    // DELETE
    /*
         better to not have it to keep records
       or 
         soft delete by adding IsActive field with deactivate/activate methods
    */
    public async Task DeleteAsync(int id)
    {
        var vehicles = await _vehicleRepo.GetByCustomerIdAsync(id);
        if (vehicles.Any())
            throw new Exception("Cannot delete customer with registered vehicles");

        await _customerRepo.DeleteAsync(id);
    }

    // SEARCH CUSTOMERS BY NAME, PHONE, AND EMAIL
    public async Task<List<Customer>> SearchAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await _customerRepo.GetAllAsync();

        searchTerm = searchTerm.Trim().ToLower();

        // or use await _customerRepo.SearchAsync(searchTerm) for better performance
        var customers = await _customerRepo.GetAllAsync();

        return customers.Where(c =>
            c.Name.ToLower().Contains(searchTerm) ||
            c.Phone.ToLower().Contains(searchTerm) ||
            c.Email.ToLower().Contains(searchTerm)
        ).ToList();
    }

    // GET CUSTOMERS BY TYPE
    public async Task<List<Customer>> GetByTypeAsync(int type)
    {
        var customers = await _customerRepo.GetAllAsync();
        return customers.Where(c => c.Type == type).ToList();
    }

    // CREDIT CUSTOMER (payment made → balance decreases)
    public async Task CreditBalanceAsync(int customerId, decimal amount)
    {
        if (amount <= 0)
            throw new Exception("Credit amount must be positive");

        var customer = await GetByIdAsync(customerId);
        customer.Balance -= amount;

        await _customerRepo.UpdateAsync(customer);
    }

    // DEBIT CUSTOMER (charge added → balance increases)
    public async Task DebitBalanceAsync(int customerId, decimal amount)
    {
        if (amount <= 0)
            throw new Exception("Debit amount must be positive");

        var customer = await GetByIdAsync(customerId);
        customer.Balance += amount;

        await _customerRepo.UpdateAsync(customer);
    }

    /*

        Additional methods can be added here, such as:
        - DONE Searching customers by name or phone or email 
        - DONE Retrieving customers by type (e.g., VIP)
        - DONE Incrementing/decrementing customer balances
        - Generating reports on customer balances

    */
}
