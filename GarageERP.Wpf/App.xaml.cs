using System.IO;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using GarageERP.Infrastructure.Data;
using GarageERP.Application.Interfaces;
using GarageERP.Application.Services;

namespace GarageERP.Wpf;

public partial class App : System.Windows.Application
{
    public IServiceProvider? ServiceProvider { get; private set; }
    public IConfiguration? Configuration { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Build configuration
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        Configuration = builder.Build();

        // Configure services
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);

        ServiceProvider = serviceCollection.BuildServiceProvider();

        // Show main window
        var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        // Register DbContext
        services.AddDbContext<GarageDbContext>(options =>
            options.UseSqlite(Configuration!.GetConnectionString("DefaultConnection")));

        // Register Services
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IVehicleService, VehicleService>();
        services.AddScoped<IContractService, ContractService>();
        services.AddScoped<ISupplierService, SupplierService>();
        services.AddScoped<IServiceService, ServiceService>();
        services.AddScoped<IPartService, PartService>();
        services.AddScoped<IPartsUsedService, PartsUsedService>();
        services.AddScoped<IJobService, JobService>();
        services.AddScoped<IInvoiceService, InvoiceService>();

        // Register Windows
        services.AddTransient<MainWindow>();
    }
}