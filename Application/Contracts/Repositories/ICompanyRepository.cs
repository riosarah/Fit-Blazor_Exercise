using Domain.Entities;
using Shared.Dtos;

namespace Application.Contracts.Repositories;

/// <summary>
/// Company-spezifische Abfragen zusätzlich zu den generischen CRUDs.
/// </summary>
public interface ICompanyRepository : IGenericRepository<Company>
{
    Task<Company?> GetByCompanyNameAsync(string companyName, CancellationToken ct = default);
    Task<int> CountByDepartmentIdAsync(int departmentId, CancellationToken ct = default);
    Task<IEnumerable<GetCompanyDto>> GetPagedByDepartmentIdAsync(int departmentId, int skip, int pageSize, CancellationToken ct = default);
}
