namespace Shared.Dtos;

/// <summary>
/// Datenübertragungsobjekt (DTO) für Companies.
/// </summary>
public sealed record GetCompanyDto(
    int Id,
    string CompanyName,
    string Zip,
    string City,
    int DepartmentId,
    string DepartmentName);
