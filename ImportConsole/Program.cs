using Application;
using Infrastructure;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImportConsole;

internal class Program
{
    private static async Task Main()
    {
        Console.WriteLine("Import Departments und Companiesin die Datenbank");
        Console.WriteLine("================================================");
        Console.WriteLine();

        // Konfiguration laden
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .Build();

        //var csvPath = Path.Combine(AppContext.BaseDirectory, "companies.csv");
        var companiesCsvPath = Path.Combine(AppContext.BaseDirectory, "companies.csv");
        var connectionString = configuration.GetConnectionString("Default") 
            ?? throw new InvalidOperationException("Connection string 'Default' nicht in appsettings.json gefunden.");

        // DI Container aufbauen
        var services = new ServiceCollection();
        services.AddInfrastructure(connectionString);
        services.AddApplication(configuration);

        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        try
        {
            Console.WriteLine("Datenbank löschen (EnsureDeleted)...");
            await dbContext.Database.EnsureDeletedAsync();
            
            Console.WriteLine("Datenbank migrieren (Migrate)...");
            await dbContext.Database.MigrateAsync();
            Console.WriteLine();

            await ImportController.ImportCompaniesAndDepartmentsAsync(dbContext, companiesCsvPath);
            Console.WriteLine();

            var departmentCount = await dbContext.Departments.CountAsync();
            var companyCount = await dbContext.Companies.CountAsync();

            Console.WriteLine("=== Importierte Daten ===");
            Console.WriteLine($"Departments: {departmentCount}");
            Console.WriteLine($"Companies: {companyCount}");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine($"FEHLER beim Import: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }

        Console.WriteLine();
        Console.WriteLine("Fertig. Beenden mit Eingabetaste...");
        Console.ReadLine();
    }
}
