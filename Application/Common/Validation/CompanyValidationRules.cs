using FluentValidation;

namespace Application.Common.Validation;

public static class CompanyValidationRules
{
    public const int CompanyNameMinLength = 2;
    public const int ZipCodeLength = 4;

    public static IRuleBuilderOptions<T, string> ApplyCompanyNameRules<T>(this IRuleBuilder<T, string> builder)
    {
        throw new NotImplementedException();
    }

    public static IRuleBuilderOptions<T, string> ApplyZipCodeRules<T>(
        this IRuleBuilder<T, string> builder,
        Func<T, string> citySelector)
    {
        throw new NotImplementedException();
    }

    public static IRuleBuilderOptions<T, string> ApplyCityRules<T>(
        this IRuleBuilder<T, string> builder,
        Func<T, string> zipSelector)
    {
        throw new NotImplementedException();
    }

    public static IRuleBuilderOptions<T, int> ApplyDepartmentIdRules<T>(this IRuleBuilder<T, int> builder)
    {
        throw new NotImplementedException();
    }
}
