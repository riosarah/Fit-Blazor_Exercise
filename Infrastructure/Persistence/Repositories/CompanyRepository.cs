using Application.Contracts.Repositories;
using Domain.Entities;
using Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

/// <summary>
/// Spezifisches Repository für Companies mit zusätzlichen Abfragen.
/// </summary>
public class CompanyRepository(AppDbContext ctx) : GenericRepository<Company>(ctx), ICompanyRepository
{
    /// <summary>
    /// Findet eine Company anhand des Company-Namens.
    /// </summary>
    public async Task<Company?> GetByCompanyNameAsync(string companyName, CancellationToken ct = default)
    {
        throw new NotImplementedException("Diese Methode muss noch implementiert werden.");
    }

    /// <summary>
    /// Zählt die Anzahl der Companies für ein bestimmtes Department.
    /// </summary>
    public async Task<int> CountByDepartmentIdAsync(int departmentId, CancellationToken ct = default)
    {
        throw new NotImplementedException("Diese Methode muss noch implementiert werden.");
    }

    /// <summary>
    /// Holt eine paginierte Liste von Companies für ein bestimmtes Department mit Department-Name.
    /// </summary>
    public async Task<IEnumerable<GetCompanyDto>> GetPagedByDepartmentIdAsync(
        int departmentId, int skip, int pageSize, CancellationToken ct = default)
    {
        throw new NotImplementedException("Diese Methode muss noch implementiert werden.");
    }
}
