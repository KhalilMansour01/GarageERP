using GarageERP.Domain.Entities;

public class VehicleService
{
    private readonly IVehicleRepository _vehicleRepo;
    private readonly ICustomerRepository _customerRepo;

    public VehicleService(
        IVehicleRepository vehicleRepo,
        ICustomerRepository customerRepo)
    {
        _vehicleRepo = vehicleRepo;
        _customerRepo = customerRepo;
    }

    // READ: single
    public async Task<Vehicle> GetByIdAsync(int id)
    {
        var vehicle = await _vehicleRepo.GetByIdAsync(id);
        if (vehicle == null)
            throw new Exception("Vehicle does not exist");

        return vehicle;
    }

    // READ: all
    public async Task<List<Vehicle>> GetAllAsync()
    {
        return await _vehicleRepo.GetAllAsync();
    }

    // READ: by customer
    public async Task<List<Vehicle>> GetByCustomerIdAsync(int customerId)
    {
        if (!await _customerRepo.ExistsAsync(customerId))
            throw new Exception("Customer does not exist");

        return await _vehicleRepo.GetByCustomerIdAsync(customerId);
    }

    // CREATE
    public async Task AddAsync(Vehicle vehicle)
    {
        if (!await _customerRepo.ExistsAsync(vehicle.CustomerId))
            throw new Exception("Customer does not exist");

        ValidateVehicle(vehicle);

        // Prevent duplicate registration number per customer
        var existingVehicles = await _vehicleRepo.GetByCustomerIdAsync(vehicle.CustomerId);
        if (existingVehicles.Any(v => v.Number == vehicle.Number))
            throw new Exception("Customer already has a vehicle with this registration number");

        await _vehicleRepo.AddAsync(vehicle);
    }

    // UPDATE
    public async Task UpdateAsync(Vehicle vehicle)
    {
        var existing = await GetByIdAsync(vehicle.Id);

        ValidateVehicle(vehicle);

        existing.Number = vehicle.Number;
        existing.Residence = vehicle.Residence;
        existing.Brand = vehicle.Brand;
        existing.Model = vehicle.Model;
        existing.Year = vehicle.Year;
        existing.Color = vehicle.Color;
        existing.UseType = vehicle.UseType;
        existing.EngineNumber = vehicle.EngineNumber;
        existing.ChasisNumber = vehicle.ChasisNumber;
        existing.Odometer = vehicle.Odometer;
        existing.EnginePower = vehicle.EnginePower;
        existing.Cylinders = vehicle.Cylinders;
        existing.SeatsNum = vehicle.SeatsNum;
        existing.SeatsNextToDriver = vehicle.SeatsNextToDriver;
        existing.EmptyWeight = vehicle.EmptyWeight;
        existing.CargoWeight = vehicle.CargoWeight;
        existing.TotalWeight = vehicle.TotalWeight;
        existing.DateOfOwnership = vehicle.DateOfOwnership;
        existing.DateOfOperation = vehicle.DateOfOperation;

        await _vehicleRepo.UpdateAsync(existing);
    }

    // DELETE (hard delete – reconsider later)
    public async Task DeleteAsync(int id)
    {
        var vehicle = await GetByIdAsync(id);

        // Future rule:
        // if vehicle has jobs → block deletion

        await _vehicleRepo.DeleteAsync(id);
    }

    // SEARCH
    public async Task<List<Vehicle>> SearchAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await _vehicleRepo.GetAllAsync();

        searchTerm = searchTerm.Trim().ToLower();
        var vehicles = await _vehicleRepo.GetAllAsync();

        return vehicles.Where(v =>
            v.Brand.ToLower().Contains(searchTerm) ||
            v.Model.ToLower().Contains(searchTerm) ||
            v.EngineNumber.ToLower().Contains(searchTerm) ||
            v.ChasisNumber.ToLower().Contains(searchTerm)
        ).ToList();
    }

    // PRIVATE VALIDATION
    private static void ValidateVehicle(Vehicle vehicle)
    {
        if (vehicle.Number <= 0)
            throw new Exception("Vehicle registration number is required");

        if (string.IsNullOrWhiteSpace(vehicle.Brand))
            throw new Exception("Vehicle brand is required");

        if (string.IsNullOrWhiteSpace(vehicle.Model))
            throw new Exception("Vehicle model is required");

        if (string.IsNullOrWhiteSpace(vehicle.EngineNumber))
            throw new Exception("Engine number is required");

        if (string.IsNullOrWhiteSpace(vehicle.ChasisNumber))
            throw new Exception("Chassis number is required");

        if (vehicle.Year < 1950 || vehicle.Year > DateTime.Now.Year + 1)
            throw new Exception("Vehicle year is invalid");

        if (vehicle.DateOfOperation < vehicle.DateOfOwnership)
            throw new Exception("Operation date cannot be before ownership date");
    }
}
