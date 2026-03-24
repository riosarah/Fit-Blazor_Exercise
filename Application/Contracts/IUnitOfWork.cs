using Application.Contracts.Repositories;

namespace Application.Contracts;

/// <summary>
/// Aggregiert Repositories und speichert Änderungen. Sicherer Umgang mit Transaktionen.
/// </summary>
public interface IUnitOfWork
{
    IDepartmentRepository Departments { get; }
    ICompanyRepository Companies { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
