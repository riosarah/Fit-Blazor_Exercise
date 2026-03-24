using Application.Common.Utilities;
using Application.Features.Companies.Commands.UpdateCompany;
using Application.Features.Companies.Queries.GetCompanyById;
using Application.Features.Departments.Queries.GetDepartmentById;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shared.Dtos;
using Shared.Validation;

namespace Blazor.Components.Pages
{
    public partial class EditCompany
    {

        [Inject] private IMediator Mediator { get; set; } = default!;
        [Inject] private NavigationManager Nav { get; set; } = default!;
        [Inject] private ISnackbar Snackbar { get; set; } = default!;

        [Parameter] public int Id { get; set; }

        private EditCompanyModel _model = new();
        private EditCompanyModel _originalModel = new();
        private GetDepartmentDto _department = new(0, "");
        private readonly List<string> _errors = [];
        private bool _saving;
        private bool _isFormValid;
        private readonly CompanyFluentValidator _companyValidator = new();
        private bool HasChanges => !ObjectMemberComparer.MembersEqual(_model, _originalModel);


        protected override async Task OnParametersSetAsync()
        {
            // Load book
            var result = await Mediator.Send(new GetCompanyByIdQuery(Id));
            if (!result.IsSuccess || result.Value is null)
            {
                AddErrors(result.Message ?? $"Book {Id} not found");
            }
            else
            {
                _model = new EditCompanyModel
                {
                    CompanyName = result.Value.CompanyName,
                    Zip= result.Value.Zip,
                    City = result.Value.City,
                    DepartmentId = result.Value.DepartmentId,
                    DepartmentName = result.Value.DepartmentName

                };

                _originalModel = new EditCompanyModel
                {
                    CompanyName = _model.CompanyName,
                    Zip = _model.Zip,
                    City = _model.City,
                    DepartmentId = _model.DepartmentId,
                    DepartmentName = _model.DepartmentName
                };

            }

            // Load selected department
            var departmentResult = await Mediator.Send(new GetDepartmentByIdQuery(_model.DepartmentId));
            if (departmentResult.IsSuccess && departmentResult.Value is not null)
            {
                _department = departmentResult.Value;
                _model.DepartmentId = departmentResult.Value.Id;
            }
            else
            {
                // Department nicht gefunden - Fehler anzeigen
                AddErrors($"Department mit ID {_model.DepartmentId} wurde nicht gefunden.");
            }
        }


        /// <summary>
        /// Für die Buttonsteuerung die Formvalidität prüfen. MudBlazor gibt den Button auch frei,
        /// wenn einzelne Felder noch keinen Wert haben.
        /// </summary>
        /// <returns></returns>
        private bool CheckFormValidity()
        {
            var validationResult = _companyValidator.Validate(_model);
            return validationResult.IsValid;
        }


        private async Task Submit()
        {
            if (_saving) return;
            _saving = true;
            _errors.Clear();

            var updateCommand = new UpdateCompanyCommand(
                Id,
                _model.CompanyName,
                _model.Zip,
                _model.City,
                _model.DepartmentId
            );

            var updateResult = await Mediator.Send(updateCommand);
            if (!updateResult.IsSuccess)
            {
                AddErrors(updateResult.Message ?? "Error updating book");
                _saving = false;
                return;
            }

            Snackbar.Add("Saved", MudBlazor.Severity.Success);

            Nav.NavigateTo($"/companies?departmentId={_model.DepartmentId}", true);
        }

        private void Back()
        {
            // Beim Abbrechen den ursprünglichen Department verwenden
            Nav.NavigateTo($"/companies?departmentId={_originalModel.DepartmentId}");
        }

        private void AddErrors(string message)
        {
            foreach (var m in message.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                _errors.Add(m);
        }
    }

    public class EditCompanyModel
    {
        public string CompanyName { get; set; } = string.Empty;
        public string Zip { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        
    }

    public class CompanyFluentValidator : AbstractValidator<EditCompanyModel>
    {
        public CompanyFluentValidator()
        {
            RuleFor(x => x.CompanyName).ApplyCompanyNameRules();
            RuleFor(x => x.City).ApplyCityRules(x => x.Zip);
            RuleFor(x => x.DepartmentId).ApplyDepartmentIdRules();
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            if (model is not EditCompanyModel editBookModel) return [];
            var context = ValidationContext<EditCompanyModel>.CreateWithOptions(editBookModel,
                        x => x.IncludeProperties(propertyName));
            var result = await ValidateAsync(context);
            return result.IsValid ? [] : result.Errors.Select(e => e.ErrorMessage);
        };
    }
}
