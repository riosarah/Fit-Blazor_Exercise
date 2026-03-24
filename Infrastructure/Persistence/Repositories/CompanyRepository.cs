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
        var com= await ctx.Companies.FindAsync(companyName, ct);
        if (com != null)
        {
            return com;
        }
        return null;
    }

    /// <summary>
    /// Zählt die Anzahl der Companies für ein bestimmtes Department.
    /// </summary>
    public async Task<int> CountByDepartmentIdAsync(int departmentId, CancellationToken ct = default)
    {
        var com = await ctx.Companies.Where(e => e.DepartmentId == departmentId).CountAsync(ct);
        return com;
    }

    /// <summary>
    /// Holt eine paginierte Liste von Companies für ein bestimmtes Department mit Department-Name.
    /// </summary>
    public async Task<IEnumerable<GetCompanyDto>> GetPagedByDepartmentIdAsync(
        int departmentId, int skip, int pageSize, CancellationToken ct = default)
    {
        var pagedCom = await Set.AsNoTracking().Include(e => e.Department)
            .Where(e => e.DepartmentId == departmentId)
            .OrderByDescending(e => e.DepartmentId)
            .ThenBy(e => e.CompanyName)
            .Skip(skip)
            .Take(pageSize)
            .Select(b => new GetCompanyDto(
                b.Id,
                b.CompanyName,
                b.Zip,
                b.City,
                b.DepartmentId,
                b.Department.DepartmentName)).ToListAsync(ct);

        return pagedCom;

          
    
    }
}
