using Shared.Dtos;
using Shared.Results;

namespace Shared.Contracts;

public interface IApiService
{
    Task<List<GetDepartmentDto>> GetDepartmentsAsync();
    Task<PagedData<GetCompanyDto>?> GetCompaniesByDepartmentPagedAsync(int departmentId, int page, int pageSize);
    Task<GetCompanyDto?> GetCompanyByIdAsync(int id);
    Task<(bool IsSuccess, string? ErrorMessage)> UpdateCompanyAsync(int id, UpdateCompanyRequest request);
    Task<(bool IsSuccess, string? ErrorMessage)> DeleteCompanyAsync(int id);
}

