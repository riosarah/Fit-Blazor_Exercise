using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Application.Pipeline;
using Application.Contracts;

namespace Application;

/// <summary>
/// Erweiterungsmethoden für DI-Registrierung der Application-Dienste.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registriert MediatR, FluentValidation und Domain Services.
    /// </summary>
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        // CQRS + MediatR + FluentValidation
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(IUnitOfWork).Assembly); // Application-Assembly
            
            // License Key aus Konfiguration lesen (User Secrets, Umgebungsvariablen, oder appsettings)
            var licenseKey = configuration["MediatR:LicenseKey"];
            if (!string.IsNullOrEmpty(licenseKey))
            {
                cfg.LicenseKey = licenseKey;
            }
        });
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddValidatorsFromAssembly(typeof(IUnitOfWork).Assembly);

        // Domain Services

        return services;
    }
}

