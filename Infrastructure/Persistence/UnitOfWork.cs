using Application.Contracts;
using Application.Contracts.Repositories;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence;

/// <summary>
/// Unit of Work aggregiert Repositories und speichert Änderungen transaktional.
/// </summary>
public class UnitOfWork(AppDbContext dbContext, IDepartmentRepository departments, ICompanyRepository companies,
            ILogger<UnitOfWork> logger) : IUnitOfWork, IDisposable
{
    private readonly AppDbContext _dbContext = dbContext;
    private bool _disposed;

    /// <summary>
    /// Zugriff auf Department-Repository.
    /// </summary>
    public IDepartmentRepository Departments { get; } = departments;

    /// <summary>
    /// Zugriff auf Company-Repository.
    /// </summary>
    public ICompanyRepository Companies { get; } = companies;


    /// <summary>
    /// Persistiert alle Änderungen in die DB. Gibt die Anzahl der betroffenen Zeilen zurück.
    /// </summary>
    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        logger.LogDebug("Saving changes to database");
        var result = await _dbContext.SaveChangesAsync(ct);
        logger.LogInformation("Successfully saved {ChangesCount} changes to database", result);
        return result;
    }

    /// <summary>
    /// Gibt verwaltete Ressourcen frei. Der DbContext gehört zum Scope dieser UoW und wird hier entsorgt.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        if (disposing)
        {
            logger.LogDebug("Disposing UnitOfWork and DbContext");
            _dbContext.Dispose();
        }
        _disposed = true;
    }
}
