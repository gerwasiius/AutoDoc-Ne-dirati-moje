using AutoDoc.Shared.Model.DTO.Common;
using AutoDoc.Shared.Model.DTO.SectionGroupDTO;
using AutoDoc.Shared.Model.DTO.SectionsDTO;
using AutoDocFront.Components.Shared;
using AutoDocFront.Models.Enumerations;
using AutoDocFront.Services;
using AutoDocFront.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace AutoDocFront.Components.Modals
{
    /// <summary>
    /// Modal za biranje sekcija iz grupa za template.
    /// </summary>
    public partial class TemplateRelationPickerModal : ModalBase
    {
        // --- PARAMETRI ---

        /// <summary>
        /// Event koji se poziva kada su sekcije odabrane.
        /// </summary>
        [Parameter] public EventCallback<List<SectionsGetDTO>> OnSectionsPicked { get; set; }

        // --- INJECTION ---

        [Inject] private SectionGroupApiService GroupService { get; set; } = default!;
        [Inject] private SectionsApiService SectionsService { get; set; } = default!;
        [Inject] private IToastService ToastService { get; set; } = default!;


        // --- STATE ---

        private PickerStepEnum _step = PickerStepEnum.GROUPS;
        private List<SectionGroupGetDTO> _availableGroups = new();
        private List<SectionsGetDTO> _availableSections = new();
        private HashSet<int> _selectedSectionIds = new();

        private string _groupSearchTerm = string.Empty;
        private int _currentGroupPage = 1;
        private const int GroupsPerPage = 5;
        private int _totalGroupCount = 0;
        private bool _isLoadingGroups = false;

        private string _selectedGroupName = string.Empty;
        private int _selectedGroupId = 0;

        private string _sectionSearchTerm = string.Empty;
        private int _currentSectionPage = 1;
        private const int SectionsPerPage = 5;
        private int _totalSectionCount = 0;
        private bool _isLoadingSections = false;

        // --- PAGINATION PROPERTIES ---

        private int TotalGroupPages => PaginationHelper.CalculateTotalPages(_totalGroupCount, GroupsPerPage);
        private int GroupStartIndex => PaginationHelper.CalculateStartIndex(_currentGroupPage, GroupsPerPage, _totalGroupCount);
        private int GroupEndIndex => PaginationHelper.CalculateEndIndex(GroupStartIndex, _availableGroups.Count, _totalGroupCount);

        private int TotalSectionPages => PaginationHelper.CalculateTotalPages(_totalSectionCount, SectionsPerPage);
        private int SectionStartIndex => PaginationHelper.CalculateStartIndex(_currentSectionPage, SectionsPerPage, _totalSectionCount);
        private int SectionEndIndex => PaginationHelper.CalculateEndIndex(SectionStartIndex, _availableSections.Count, _totalSectionCount);

        // --- LIFECYCLE ---

        /// <summary>
        /// Inicijalizuje modal i učitava grupe.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            _step = PickerStepEnum.GROUPS;
            await LoadGroupsAsync();
        }

        // --- GROUPS LOGIKA ---

        private async Task LoadGroupsAsync()
        {
            try
            {
                _isLoadingGroups = true;
                int offset = (_currentGroupPage - 1) * GroupsPerPage;

                var status = "ACTIVE";
                var result = await GroupService.GetGroupsAsync(_groupSearchTerm, status, offset, GroupsPerPage);
                _availableGroups = result.Items ?? [];
                _totalGroupCount = result.TotalItems;
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Greška prilikom učitavanja grupa: {ex.Message}");
                _availableGroups = [];
                _totalGroupCount = 0;
            }
            finally
            {
                _isLoadingGroups = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        private async Task SearchGroupsAsync()
        {
            try
            {
                _currentGroupPage = 1;
                await LoadGroupsAsync();
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Greška prilikom pretrage grupa: {ex.Message}");
            }
        }

        private async Task ClearGroupFiltersAsync()
        {
            try
            {
                _groupSearchTerm = string.Empty;
                _currentGroupPage = 1;
                await LoadGroupsAsync();
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Greška prilikom čišćenja filtera grupa: {ex.Message}");
            }
        }

        private async Task ChangeGroupPageAsync(int page)
        {
            if (page < 1 || page > TotalGroupPages || page == _currentGroupPage) return;
            try
            {
                _currentGroupPage = page;
                await LoadGroupsAsync();
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Greška prilikom promjene stranice grupa: {ex.Message}");
            }
        }

        private async Task OnGroupPickedAsync(SectionGroupGetDTO group)
        {
            try
            {
                _selectedGroupName = group.Name;
                _selectedGroupId = group.ID;
                _step = PickerStepEnum.SECTIONS;
                _currentSectionPage = 1;
                _sectionSearchTerm = string.Empty;
                _selectedSectionIds.Clear();
                await LoadSectionsAsync(group.ID);
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Greška prilikom odabira grupe: {ex.Message}");
            }
        }

        // --- SECTIONS LOGIKA ---

        private async Task LoadSectionsAsync(int groupId)
        {
            try
            {
                _isLoadingSections = true;
                int offset = (_currentSectionPage - 1) * SectionsPerPage;

                var result = await SectionsService.GetSectionsAsync(
                    groupId,
                    _sectionSearchTerm,
                    SectionStatusType.ACTIVE,
                    offset,
                    SectionsPerPage);

                _availableSections = result.Items ?? [];
                _totalSectionCount = result.TotalItems;
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Greška prilikom učitavanja sekcija: {ex.Message}");
                _availableSections = [];
                _totalSectionCount = 0;
            }
            finally
            {
                _isLoadingSections = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        private async Task SearchSectionsAsync()
        {
            try
            {
                _currentSectionPage = 1;
                await LoadSectionsAsync(_selectedGroupId);
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Greška prilikom pretrage sekcija: {ex.Message}");
            }
        }


        private async Task ClearSectionFiltersAsync()
        {
            try
            {
                _sectionSearchTerm = string.Empty;
                _currentSectionPage = 1;
                await LoadSectionsAsync(_selectedGroupId);
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Greška prilikom čišćenja filtera sekcija: {ex.Message}");
            }
        }


        private async Task ChangeSectionPageAsync(int page)
        {
            if (page < 1 || page > TotalSectionPages || page == _currentSectionPage) return;
            try
            {
                _currentSectionPage = page;
                await LoadSectionsAsync(_selectedGroupId);
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Greška prilikom promjene stranice sekcija: {ex.Message}");
            }
        }

        private void ToggleSectionSelection(int id, object? checkedValue)
        {
            try
            {
                if ((bool?)checkedValue == true)
                    _selectedSectionIds.Add(id);
                else
                    _selectedSectionIds.Remove(id);
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Greška prilikom selektovanja sekcije: {ex.Message}");
            }
        }

        private async Task AddSelectedSectionsAsync()
        {
            try
            {
                var picked = _availableSections.Where(s => _selectedSectionIds.Contains(s.ID)).ToList();
                await OnSectionsPicked.InvokeAsync(picked);
                _selectedSectionIds.Clear();
                await CloseAsync();
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Greška prilikom dodavanja sekcija: {ex.Message}");
            }
        }

        private async Task ClosePickerAsync()
        {
            try
            {
                _selectedSectionIds.Clear();
                await CloseAsync();
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Greška prilikom zatvaranja modala: {ex.Message}");
            }
        }
    }
}