namespace Shared.Dtos;

public record UpdateCompanyRequest(string CompanyName, string Zip, string City, int DepartmentId);
