using Application.Features.Departments.Queries.GetDepartmentById;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shared.Dtos;
using Shared.Validation;

namespace Blazor.Components.Pages
{
    public partial class AddCompany
    {

        [Inject] private IMediator Mediator { get; set; } = default!;
        [Inject] private NavigationManager Nav { get; set; } = default!;
        [Inject] private ISnackbar Snackbar { get; set; } = default!;

        [Parameter]
        [SupplyParameterFromQuery(Name = "departmentId")]
        public int? DepartmentId { get; set; }

        private readonly AddCompanyModel _model = new();
        private GetDepartmentDto _department = new(0, "");
        private readonly List<string> _errors = [];
        private bool _saving;
        private bool _isFormValid;
        private readonly AddCompanyFluentValidator _companyValidator = new();
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                // Load selected department
                if (DepartmentId.HasValue)
                {
                    var departmentResult = await Mediator.Send(new GetDepartmentByIdQuery(DepartmentId.Value));
                    if (departmentResult.IsSuccess && departmentResult.Value is not null)
                    {
                        _department = departmentResult.Value;
                        _model.DepartmentId = departmentResult.Value.Id;
                    }
                    else
                    {
                        // Department nicht gefunden - Fehler anzeigen
                        AddErrors($"Department mit ID {DepartmentId.Value} wurde nicht gefunden.");
                        StateHasChanged();
                        return;
                    }
                }
                else
                {
                    // Kein DepartmentId Parameter - Fehler
                    AddErrors("Kein Department ausgewählt. Bitte wählen Sie ein Department aus der Liste.");
                    StateHasChanged();
                    return;
                }

                // Set default year to current year
                _model.Year = DateTime.Now.Year;

                StateHasChanged();
            }
        }

        bool CheckFormValidity()
        {
            var validationResult = _companyValidator.Validate(_model);
            return validationResult.IsValid;
        }

        private async Task Submit()
        {
            if (_saving) return;
            _saving = true;
            _errors.Clear();

            // CreateCompanyCommand über MediatR senden
            var createCommand = new Application.Features.Companies.Commands.AddCompany.CreateCompanyCommand(
                _model.CompanyName,
                _model.Zip,
                _model.City,
                _model.DepartmentId
            );

            var createResult = await Mediator.Send(createCommand);
            if (!createResult.IsSuccess)
            {
                AddErrors(createResult.Message ?? "Fehler beim Erstellen der Company");
                _saving = false;
                return;
            }

            Snackbar.Add("Company added successfully", MudBlazor.Severity.Success);
            Nav.NavigateTo($"/companies?departmentId={_model.DepartmentId}", true);
        }

        private void Back()
        {
            if (DepartmentId.HasValue)
            {
                Nav.NavigateTo($"/companies?departmentId={DepartmentId.Value}");
            }
            else
            {
                Nav.NavigateTo("/companies");
            }
        }

        private void AddErrors(string message)
        {
            foreach (var m in message.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                _errors.Add(m);
        }
    }

    public class AddCompanyModel
    {
        public string CompanyName { get; set; } = string.Empty;
        public int Year { get; set; }
        public string City { get; set; } = string.Empty;
        public string Zip { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
    }

    public class AddCompanyFluentValidator : AbstractValidator<AddCompanyModel>
    {
        public AddCompanyFluentValidator()
        {
            RuleFor(x => x.CompanyName).ApplyCompanyNameRules();
            RuleFor(x => x.City).ApplyCityRules(x=>x.Zip);
            RuleFor(x => x.DepartmentId).ApplyDepartmentIdRules();
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            if (model is not AddCompanyModel addCompanyModel) return [];
            var context = ValidationContext<AddCompanyModel>.CreateWithOptions(addCompanyModel,
                        x => x.IncludeProperties(propertyName));
            var result = await ValidateAsync(context);
            return result.IsValid ? [] : result.Errors.Select(e => e.ErrorMessage);
        };
    }
}
