using Domain.Entities;
using Infrastructure.Persistence;

namespace Infrastructure.Services;

/// <summary>
/// Service zum Importieren von Daten aus CSV-Dateien.
/// </summary>
public static class ImportController
{
    /// <summary>
    /// Importiert Departments und Companies aus companies.csv.
    /// Erwartet Format: DepartmentName;CompanyName;Zip;City
    /// </summary>
    public static async Task ImportCompaniesAndDepartmentsAsync(AppDbContext dbContext,
        string companiesCsvPath)
    {
        if (!File.Exists(companiesCsvPath))
        {
            Console.WriteLine($"Warnung: Datei {companiesCsvPath} nicht gefunden. Companies/Departments werden übersprungen.");
            return;
        }

        Console.WriteLine($"Lese Departments und Companies aus {companiesCsvPath}...");
        var lines = await File.ReadAllLinesAsync(companiesCsvPath);
        var departments = new Dictionary<string, Department>();
        int importedDepartments = 0;
        int importedCompanies = 0;

        for (int i = 1; i < lines.Length; i++) // i=1: Header überspringen
        {
            var line = lines[i];
            if (string.IsNullOrWhiteSpace(line)) continue;
            var parts = line.Split(';');
            if (parts.Length < 4) continue;

            var departmentName = parts[0].Trim();
            var companyName = parts[1].Trim();
            var zip = parts[2].Trim();
            var city = parts[3].Trim();

            // Department hinzufügen (falls noch nicht vorhanden) und speichern
            if (!string.IsNullOrWhiteSpace(departmentName) && !departments.ContainsKey(departmentName))
            {
                var department = Department.Create(departmentName);
                departments[departmentName] = department;
                dbContext.Departments.Add(department);
                await dbContext.SaveChangesAsync(); // Department MUSS gespeichert werden, damit Id gesetzt wird
                importedDepartments++;
            }

            // Company mit DepartmentId hinzufügen
            if (!string.IsNullOrWhiteSpace(companyName) && departments.TryGetValue(departmentName, out var dept))
            {
                try
                {
                    var company = Company.Create(companyName, zip, city, dept.Id);
                    dbContext.Companies.Add(company);
                    await dbContext.SaveChangesAsync();
                    importedCompanies++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Fehler beim Importieren von Company '{companyName}': {ex.Message}");
                }
            }
        }

        Console.WriteLine($"Departments importiert: {importedDepartments}");
        Console.WriteLine($"Companies importiert: {importedCompanies}");
    }


}
