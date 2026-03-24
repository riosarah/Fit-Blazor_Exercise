namespace Domain.Entities;

/// <summary>
/// Repräsentiert eine Abteilung.
/// </summary>
public class Department : BaseEntity
{
    /// <summary>
    /// Name der Abteilung.
    /// </summary>
    public string DepartmentName { get; private set; } = string.Empty;

    /// <summary>
    /// Navigation Property: Unternehmen, die zu dieser Abteilung gehören.
    /// </summary>
    public ICollection<Company> Companies { get; private set; } = new List<Company>();

    private Department() { } // Für EF Core notwendig (parameterloser Konstruktor)

    /// <summary>
    /// Erstellt eine neue Department-Instanz mit dem angegebenen Namen.
    /// </summary>
    /// <param name="departmentName">Der Name der Abteilung. Darf nicht null oder leer sein.</param>
    /// <returns>Eine neue Department-Instanz.</returns>
    public static Department Create(string departmentName)
    {
        var trimmedName = (departmentName ?? string.Empty).Trim();
        ValidateDepartmentProperties(trimmedName);
        return new Department { DepartmentName = trimmedName };
    }

    /// <summary>
    /// Aktualisiert die Eigenschaften der Abteilung.
    /// </summary>
    public void Update(string departmentName)
    {
        var trimmedName = (departmentName ?? string.Empty).Trim();
        if (DepartmentName == trimmedName)
            return; // Keine Änderung
        ValidateDepartmentProperties(trimmedName);
        DepartmentName = trimmedName;
    }

    public override string ToString() => DepartmentName;

    private static void ValidateDepartmentProperties(string departmentName)
    {
        if (string.IsNullOrWhiteSpace(departmentName))
        {
            throw new ArgumentException("DepartmentName darf nicht leer sein.", nameof(departmentName));
        }
    }
}
