using Application.Contracts;
using Application.Contracts.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure;

/// <summary>
/// Erweiterungsmethoden für DI-Registrierung der Infrastrukturdienste.
/// </summary>
public static class DependencyInjection
{
    ///// <summary>
    ///// Registriert DbContext, Repositories, UnitOfWork, CSV-Reader und Seeder.
    ///// </summary>
    //public static IServiceCollection AddInfrastructure(this IServiceCollection services, string csvPath, 
    //            string connectionString)
    //{
    //    services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(connectionString));

    //    // Repositories und UoW (Scoped: pro HTTP-Request eine Instanz)
    //    services.AddScoped<IAuthorRepository, AuthorRepository>();
    //    services.AddScoped<IBookRepository, BookRepository>();
    //    services.AddScoped<IUnitOfWork, UnitOfWork>();

    //    return services;
    //}

    /// <summary>
    /// Registriert DbContext und Repositories ohne HostedService (für Console-Apps).
    /// </summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(connectionString));

        // Repositories und UoW (Scoped: pro HTTP-Request eine Instanz)
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
