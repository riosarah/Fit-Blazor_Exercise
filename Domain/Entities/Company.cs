using Domain.Common;
using Domain.Exceptions;
using Domain.ValidationSpecifications;

namespace Domain.Entities;

/// <summary>
/// Repräsentiert ein Unternehmen.
/// </summary>
public class Company : BaseEntity
{
    /// <summary>
    /// Name des Unternehmens.
    /// </summary>
    public string CompanyName { get; private set; } = string.Empty;

    /// <summary>
    /// Postleitzahl des Unternehmens.
    /// </summary>
    public string Zip { get; private set; } = string.Empty;

    /// <summary>
    /// Stadt des Unternehmens.
    /// </summary>
    public string City { get; private set; } = string.Empty;

    /// <summary>
    /// Fremdschlüssel auf die Abteilung.
    /// </summary>
    public int DepartmentId { get; private set; }

    /// <summary>
    /// Navigation Property: Abteilung, zu der das Unternehmen gehört.
    /// </summary>
    public Department Department { get; private set; } = null!;

    private Company() { } // Für EF Core notwendig (parameterloser Konstruktor)

    /// <summary>
    /// Erstellt asynchron eine neue Company-Instanz mit den angegebenen Daten.
    /// </summary>
    /// <param name="companyName">Der Name des Unternehmens. Darf nicht null oder leer sein.</param>
    /// <param name="zip">Die Postleitzahl. Darf nicht null oder leer sein.</param>
    /// <param name="city">Die Stadt. Darf nicht null oder leer sein.</param>
    /// <param name="departmentId">Die ID der zugehörigen Abteilung.</param>
    /// <param name="uniquenessChecker">Service zur Prüfung der Eindeutigkeit des Company-Namens.</param>
    /// <param name="ct">Cancellation Token.</param>
    /// <returns>Eine neue Company-Instanz.</returns>
    public static Company Create(string companyName, string zip, string city,
        int departmentId)
    {
        var trimmedCompanyName = (companyName ?? string.Empty).Trim();
        var trimmedZip = (zip ?? string.Empty).Trim();
        var trimmedCity = (city ?? string.Empty).Trim();

        ValidateCompanyProperties(trimmedCompanyName, trimmedZip, trimmedCity, departmentId);

        return new Company
        {
            CompanyName = trimmedCompanyName,
            Zip = trimmedZip,
            City = trimmedCity,
            DepartmentId = departmentId
        };
    }

    /// <summary>
    /// Aktualisiert asynchron die Eigenschaften des Unternehmens.
    /// </summary>
    public void Update(string companyName, string zip, string city, int departmentId)
    {
        var trimmedCompanyName = (companyName ?? string.Empty).Trim();
        var trimmedZip = (zip ?? string.Empty).Trim();
        var trimmedCity = (city ?? string.Empty).Trim();

        if (CompanyName == trimmedCompanyName && Zip == trimmedZip && City == trimmedCity && DepartmentId == departmentId)
            return; // Keine Änderung

        ValidateCompanyProperties(trimmedCompanyName, trimmedZip, trimmedCity, departmentId);

        CompanyName = trimmedCompanyName;
        Zip = trimmedZip;
        City = trimmedCity;
        DepartmentId = departmentId;
    }

    public override string ToString() => $"{CompanyName}, {Zip} {City}";

    /// <summary>
    /// Validiert die Company-Eigenschaften gegen alle Specifications.
    /// </summary>
    public static void ValidateCompanyProperties(string companyName, string zip, string city, int departmentId)
    {
        var validationResults = new List<DomainValidationResult>
        {
            CompanySpecifications.CheckCompanyName(companyName),
            CompanySpecifications.CheckCompanyNameMinLength(companyName),
            CompanySpecifications.CheckCity(city),
            CompanySpecifications.CheckZipCode(zip),
            CompanySpecifications.CheckZipCodeFormat(zip),
            CompanySpecifications.CheckViennaZipCodeRule(zip, city),
            CompanySpecifications.CheckDepartmentId(departmentId)
        };

        foreach (var result in validationResults)
        {
            if (!result.IsValid)
            {
                throw new DomainValidationException(result.Property, result.ErrorMessage!);
            }
        }
    }

}
