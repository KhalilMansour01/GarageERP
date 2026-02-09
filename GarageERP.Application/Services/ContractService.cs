using GarageERP.Domain.Constants;
using GarageERP.Domain.Entities;
using GarageERP.Application.Interfaces;

public class ContractService
{
    private readonly IContractRepository _contractRepo;
    private readonly IVehicleRepository _vehicleRepo;

    public ContractService(
        IContractRepository contractRepo,
        IVehicleRepository vehicleRepo)
    {
        _contractRepo = contractRepo;
        _vehicleRepo = vehicleRepo;
    }

    // READ: single
    public async Task<Contract> GetByIdAsync(int id)
    {
        var contract = await _contractRepo.GetByIdAsync(id);
        if (contract == null)
            throw new Exception("Contract does not exist");

        return contract;
    }

    // READ: all
    public async Task<List<Contract>> GetAllAsync()
    {
        return await _contractRepo.GetAllAsync();
    }

    // READ: by vehicle
    public async Task<List<Contract>> GetByVehicleAsync(int vehicleId)
    {
        return await _contractRepo.GetByVehicleIdAsync(vehicleId);
    }

    // READ: by customer (via vehicle)
    public async Task<List<Contract>> GetByCustomerAsync(int customerId)
    {
        var vehicles = await _vehicleRepo.GetByCustomerIdAsync(customerId);
        var contracts = new List<Contract>();
        foreach (var vehicle in vehicles)
        {
            var vehicleContracts = await _contractRepo.GetByVehicleIdAsync(vehicle.Id);
            contracts.AddRange(vehicleContracts);
        }
        return contracts;
    }

    // CREATE
    public async Task AddAsync(
        int vehicleId,
        DateTime dateIssued,
        DateTime expDate,
        int discount,
        string? description)
    {
        if (dateIssued > expDate)
            throw new Exception("Issue date cannot be after expiration date");

        if (discount < 0 || discount > 100)
            throw new Exception("Discount must be between 0 and 100");

        var vehicle = await _vehicleRepo.GetByIdAsync(vehicleId);
        if (vehicle == null)
            throw new Exception("Vehicle does not exist");

        var contract = new Contract
        {
            VehicleId = vehicleId,
            DateIssued = dateIssued,
            ExpDate = expDate,
            Discount = discount,
            Description = description?.Trim() ?? "",
            Status = ContractStatus.Active
        };

        await _contractRepo.AddAsync(contract);
    }

    // UPDATE (no vehicle change)
    public async Task UpdateAsync(Contract contract)
    {
        var existing = await GetByIdAsync(contract.Id);

        if (existing.DateIssued > existing.ExpDate)
            throw new Exception("Expiration date cannot be before issue date");

        if (existing.Discount < 0 || existing.Discount > 100)
            throw new Exception("Discount must be between 0 and 100");

        existing.ExpDate = contract.ExpDate;
        existing.Discount = contract.Discount;
        existing.Status = contract.Status;
        existing.Description = contract.Description?.Trim() ?? "";

        await _contractRepo.UpdateAsync(existing);
    }

    // DELETE (hard delete)
    public async Task DeleteAsync(int id)
    {
        await GetByIdAsync(id); // ensures existence
        await _contractRepo.DeleteAsync(id);
    }

    // BUSINESS LOGIC

    // Manual close by user action
    public async Task CloseAsync(int contractId)
    {
        var contract = await GetByIdAsync(contractId);

        if (contract.Status == ContractStatus.Closed)
            throw new Exception("Contract is already closed");

        contract.Status = ContractStatus.Closed;
        await _contractRepo.UpdateAsync(contract);
    }

    // This method can be called by a scheduled job to
    // auto-expire contracts past their expiration date
    public async Task AutoExpireAsync(int contractId)
    {
        var contract = await GetByIdAsync(contractId);

        if (DateTime.Today > contract.ExpDate &&
            contract.Status == ContractStatus.Active)
        {
            contract.Status = ContractStatus.Expired;
            await _contractRepo.UpdateAsync(contract);
        }
    }
}
