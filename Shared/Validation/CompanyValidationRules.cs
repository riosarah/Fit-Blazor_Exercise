using FluentValidation;

namespace Shared.Validation;

public static class CompanyValidationRules
{
    public const int CompanyNameMinLength = 2;
    public const int ZipCodeLength = 4;

    public static IRuleBuilderOptions<T, string> ApplyCompanyNameRules<T>(this IRuleBuilder<T, string> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        return builder
            .NotEmpty().WithMessage("CompanyName darf nicht leer sein.")
            .MinimumLength(CompanyNameMinLength).WithMessage($"CompanyName muss mindestens {CompanyNameMinLength} Zeichen haben.");
    }

    public static IRuleBuilderOptions<T, string> ApplyZipCodeRules<T>(
        this IRuleBuilder<T, string> builder,
        Func<T, string> citySelector)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(citySelector);

        return builder
            .NotEmpty().WithMessage("ZipCode darf nicht leer sein.")
            .Length(ZipCodeLength).WithMessage($"ZipCode muss genau {ZipCodeLength} Zeichen haben.")
            .Must(zip => int.TryParse(zip, out _)).WithMessage("ZipCode muss eine Zahl sein.")
            .Must((instance, zip) =>
            {
                if (zip.StartsWith("1"))
                {
                    var city = citySelector(instance);
                    return string.Equals(city, "Wien", StringComparison.OrdinalIgnoreCase);
                }
                return true;
            }).WithMessage("Wenn die PLZ mit 1 beginnt, muss die City 'Wien' sein.");
    }

    public static IRuleBuilderOptions<T, string> ApplyCityRules<T>(
        this IRuleBuilder<T, string> builder,
        Func<T, string> zipSelector)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(zipSelector);

        return builder
            .NotEmpty().WithMessage("City darf nicht leer sein.")
            .Must((instance, city) =>
            {
                var zip = zipSelector(instance);
                if (zip?.StartsWith("1") == true)
                {
                    return string.Equals(city, "Wien", StringComparison.OrdinalIgnoreCase);
                }
                return true;
            }).WithMessage("Wenn die PLZ mit 1 beginnt, muss die City 'Wien' sein.");
    }

    public static IRuleBuilderOptions<T, int> ApplyDepartmentIdRules<T>(this IRuleBuilder<T, int> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        return builder
            .GreaterThan(0).WithMessage("Department muss ausgewählt werden.");
    }
}
