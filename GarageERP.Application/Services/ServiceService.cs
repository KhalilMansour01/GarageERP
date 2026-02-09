using GarageERP.Domain.Entities;
using GarageERP.Application.Interfaces;

namespace GarageERP.Application.Services;

public class ServiceService
{
    private readonly IServiceRepository _serviceRepo;

    public ServiceService(IServiceRepository serviceRepo)
    {
        _serviceRepo = serviceRepo;
    }

    public async Task<Service> GetByIdAsync(int id)
    {
        var service = await _serviceRepo.GetByIdAsync(id);
        if (service == null)
            throw new Exception("Service does not exist");
        return service;
    }

    public async Task<List<Service>> GetAllAsync()
    {
        return await _serviceRepo.GetAllAsync();
    }

    public async Task AddAsync(string name, string description, decimal cost)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new Exception("Service name is required");

        if (cost < 0)
            throw new Exception("Cost cannot be negative");

        var service = new Service
        {
            Name = name.Trim(),
            Description = description?.Trim() ?? "",
            Cost = cost
        };

        await _serviceRepo.AddAsync(service);
    }

    public async Task UpdateAsync(Service service)
    {
        var existing = await GetByIdAsync(service.Id);

        if (service.Cost < 0)
            throw new Exception("Cost cannot be negative");

        existing.Name = service.Name?.Trim() ?? throw new Exception("Name is required");
        existing.Description = service.Description?.Trim() ?? "";
        existing.Cost = service.Cost;

        await _serviceRepo.UpdateAsync(existing);
    }

    public async Task DeleteAsync(int id)
    {
        await GetByIdAsync(id);
        await _serviceRepo.DeleteAsync(id);
    }
}