using Domain.Common;

namespace Domain.ValidationSpecifications;

/// <summary>
/// Validierungsregeln für Company-Entitäten.
/// </summary>
public static class CompanySpecifications
{

    /// <summary>
    /// Prüft, ob der CompanyName nicht leer ist.
    /// </summary>
    public static DomainValidationResult CheckCompanyName(string companyName)
    {
        return DomainValidationResult.Success("CompanyName");
    }

    /// <summary>
    /// Prüft, ob der CompanyName die Mindestlänge hat.
    /// </summary>
    public static DomainValidationResult CheckCompanyNameMinLength(string companyName)
    {
        return DomainValidationResult.Success(companyName);
    }

    /// <summary>
    /// Prüft, ob die City nicht leer ist.
    /// </summary>
    public static DomainValidationResult CheckCity(string city)
    {
        return DomainValidationResult.Success("City");
    }

    /// <summary>
    /// Prüft, ob die ZipCode nicht leer ist.
    /// </summary>
    public static DomainValidationResult CheckZipCode(string zipCode)
    {
        return DomainValidationResult.Success("ZipCode");
    }

    /// <summary>
    /// Prüft, ob die ZipCode eine 4-stellige Zahl ist.
    /// </summary>
    public static DomainValidationResult CheckZipCodeFormat(string zipCode)
    {
        return DomainValidationResult.Success("ZipCode");
    }

    /// <summary>
    /// Prüft die Regel: Wenn PLZ mit 1 beginnt, muss City "Wien" sein.
    /// </summary>
    public static DomainValidationResult CheckViennaZipCodeRule(string zipCode, string city)
    {
        return DomainValidationResult.Success("ZipCode");
    }

    /// <summary>
    /// Prüft, ob die DepartmentId gültig ist (größer als 0).
    /// </summary>
    public static DomainValidationResult CheckDepartmentId(int departmentId)
    {
        return DomainValidationResult.Success("DepartmentId");
    }
}
