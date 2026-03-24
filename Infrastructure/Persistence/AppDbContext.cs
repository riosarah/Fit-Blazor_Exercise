using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

/// <summary>
/// EF Core DbContext. Verwaltet die Verbindung zur Datenbank und das Mapping der Entitäten.
/// </summary>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Tabelle/DbSet für Abteilungen.
    /// </summary>
    public DbSet<Department> Departments => Set<Department>();

    /// <summary>
    /// Tabelle/DbSet für Unternehmen.
    /// </summary>
    public DbSet<Company> Companies => Set<Company>();


    /// <summary>
    /// Fluent-API Konfigurationen für EF Core.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Department>(department =>
        {
            department.Property(d => d.DepartmentName).HasMaxLength(200).IsRequired();
            // RowVersion für Optimistic Concurrency
            department.Property(d => d.RowVersion).IsRowVersion();
        });

        modelBuilder.Entity<Company>(company =>
        {
            company.Property(c => c.CompanyName).HasMaxLength(200).IsRequired();
            company.Property(c => c.Zip).HasMaxLength(10).IsRequired();
            company.Property(c => c.City).HasMaxLength(100).IsRequired();
            // RowVersion für Optimistic Concurrency
            company.Property(c => c.RowVersion).IsRowVersion();

            // Beziehung: Jede Company gehört zu genau einem Department (Restrict Delete)
            company.HasOne(c => c.Department)
                .WithMany(d => d.Companies)
                .HasForeignKey(c => c.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            // Index für Foreign Key
            company.HasIndex(c => c.DepartmentId);
        });



    }
}
