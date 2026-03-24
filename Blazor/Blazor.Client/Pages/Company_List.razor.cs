using Blazor.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using Shared.Dtos;

namespace Blazor.Client.Pages
{
    public partial class Company_List
    {
        [Inject] private ApiService ApiService { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
        [Inject] private ISnackbar Snackbar { get; set; } = default!;
        [Inject] private IDialogService DialogService { get; set; } = default!;

        [Parameter]
        [SupplyParameterFromQuery(Name = "departmentId")]
        public int? ReturnDepartmentId { get; set; }

        private MudDataGrid<GetCompanyDto>? _grid;
        private List<GetDepartmentDto> _departments = [];
        private int? _selectedDepartmentId;

        protected override async Task OnInitializedAsync()
        {
            await LoadDepartmentsAsync();
        }

        protected override async Task OnParametersSetAsync()
        {
            if (ReturnDepartmentId.HasValue && _departments.Any(a => a.Id == ReturnDepartmentId.Value))
            {
                _selectedDepartmentId = ReturnDepartmentId.Value;
                if (_grid != null)
                {
                    await _grid.ReloadServerData();
                }
            }
            else if (_selectedDepartmentId == null && _departments.Count != 0)
            {
                _selectedDepartmentId = _departments.First().Id;
                StateHasChanged();
                if (_grid != null)
                {
                    await _grid.ReloadServerData();
                }
            }
        }

        private async Task LoadDepartmentsAsync()
        {
            try
            {
                var dep = await ApiService.GetAllDepartmentsAsync();
                _departments = dep.ToList();

                if (_selectedDepartmentId == null && _departments.Count != 0 && ReturnDepartmentId == null)
                {
                    _selectedDepartmentId = _departments.First().Id;
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Fehler beim Laden der Departments: {ex.Message}", Severity.Error);
            }
        }

        private async Task OnDepartmentChanged(int? departmentId)
        {
            _selectedDepartmentId = departmentId;
            if (_grid != null && departmentId.HasValue)
            {
                await _grid.ReloadServerData();
            }
        }

        private async Task<GridData<GetCompanyDto>> LoadServerData(GridState<GetCompanyDto> state)
        {
            if (!_selectedDepartmentId.HasValue)
            {
                return new GridData<GetCompanyDto> { Items = [], TotalItems = 0 };
            }

            try
            {
                var result = await ApiService.GetCompanyByDepartmentId(_selectedDepartmentId.Value, state.Page, state.PageSize);

                if (result == null || result.Value == null)
                {
                    Snackbar.Add("Fehler beim Laden der Companies", Severity.Warning);
                    return new GridData<GetCompanyDto> { Items = [], TotalItems = 0 };
                }

                return new GridData<GetCompanyDto>
                {
                    Items = result.Value.Items,
                    TotalItems = result.Value.TotalCount
                };
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Fehler beim Laden: {ex.Message}", Severity.Error);
                return new GridData<GetCompanyDto> { Items = [], TotalItems = 0 };
            }
        }
        private void AddCompany()
        {
            if (_selectedDepartmentId.HasValue)
            {
                NavigationManager.NavigateTo($"/company/add?departmentId={_selectedDepartmentId}");
            }
        }

        private void EditCompany(int id)
        {
            // AuthorId als Query-Parameter für Rücksprung mitgeben
            NavigationManager.NavigateTo($"/company/edit/{id}?returnDepartmentId={_selectedDepartmentId}");
        }

        private async Task DeleteCompany(int id, string title)
        {
            bool? result = await DialogService.ShowMessageBox(
                "Löschen bestätigen",
                $"Möchten Sie die Company '{title}' wirklich löschen?",
                yesText: "Löschen",
                cancelText: "Abbrechen");

            if (result == true)
            {
                var deleteResult = await ApiService.DeleteCompanyAsync(id);
                if (deleteResult.IsSuccess)
                {
                    Snackbar.Add($"Company '{title}' wurde gelöscht", Severity.Success);
                    if (_grid != null)
                    {
                        await _grid.ReloadServerData();
                    }
                }
                else
                {
                    Snackbar.Add(deleteResult.Message ?? "Fehler beim Löschen der Company", Severity.Error);
                }
            }
        }


    }
}
