using Application.Contracts.Repositories;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories;

/// <summary>
/// Spezifisches Repository für Departments mit zusätzlichen Abfragen.
/// </summary>
public class DepartmentRepository(AppDbContext ctx) : GenericRepository<Department>(ctx), IDepartmentRepository
{
}
