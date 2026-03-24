using Domain.Entities;

namespace Application.Contracts.Repositories;

/// <summary>
/// Department-spezifische Abfragen zusätzlich zu den generischen CRUDs.
/// </summary>
public interface IDepartmentRepository : IGenericRepository<Department>
{
}
