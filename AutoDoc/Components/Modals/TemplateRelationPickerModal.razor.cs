using AutoDoc.Shared.Model.DTO.Common;
using AutoDoc.Shared.Model.DTO.SectionGroupDTO;
using AutoDoc.Shared.Model.DTO.SectionsDTO;
using AutoDocFront.Models.Enumerations;
using Microsoft.AspNetCore.Components;

namespace AutoDocFront.Components.Modals
{
    /// <summary>
    /// Modal za biranje sekcija iz grupa za template.
    /// </summary>
    public partial class TemplateRelationPickerModal
    {
        // --- PARAMETRI ---

        /// <summary>
        /// Da li je modal otvoren.
        /// </summary>
        [Parameter] public bool IsOpen { get; set; }

        /// <summary>
        /// Event za promjenu stanja otvaranja modala.
        /// </summary>
        [Parameter] public EventCallback<bool> IsOpenChanged { get; set; }

        /// <summary>
        /// Event koji se poziva kada su sekcije odabrane.
        /// </summary>
        [Parameter] public EventCallback<List<SectionsGetDTO>> OnSectionsPicked { get; set; }

        // --- INJECTION ---

        [Inject] private SectionGroupApiService GroupService { get; set; } = default!;
        [Inject] private SectionsApiService SectionsService { get; set; } = default!;

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

        private int TotalGroupPages => (int)Math.Ceiling((double)_totalGroupCount / GroupsPerPage);
        private int GroupStartIndex => _totalGroupCount == 0 ? 0 : (_currentGroupPage - 1) * GroupsPerPage;
        private int GroupEndIndex => Math.Min(GroupStartIndex + _availableGroups.Count, _totalGroupCount);

        private int TotalSectionPages => (int)Math.Ceiling((double)_totalSectionCount / SectionsPerPage);
        private int SectionStartIndex => _totalSectionCount == 0 ? 0 : (_currentSectionPage - 1) * SectionsPerPage;
        private int SectionEndIndex => Math.Min(SectionStartIndex + _availableSections.Count, _totalSectionCount);

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

                var query = new List<string>
                {
                    $"offset={offset}",
                    $"pageSize={GroupsPerPage}",
                    $"status=ACTIVE"
                };

                if (!string.IsNullOrWhiteSpace(_groupSearchTerm))
                    query.Add($"name={Uri.EscapeDataString(_groupSearchTerm)}");

                var response = await GroupService.GetGroupsAsync(_groupSearchTerm, "ACTIVE", offset, GroupsPerPage);
                _availableGroups = response.Items ?? [];
                _totalGroupCount = response.TotalItems;
            }
            finally
            {
                _isLoadingGroups = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        private async Task SearchGroupsAsync()
        {
            _currentGroupPage = 1;
            await LoadGroupsAsync();
        }

        private async Task ClearGroupFiltersAsync()
        {
            _groupSearchTerm = string.Empty;
            _currentGroupPage = 1;
            await LoadGroupsAsync();
        }

        private async Task ChangeGroupPageAsync(int page)
        {
            if (page < 1 || page > TotalGroupPages || page == _currentGroupPage) return;
            _currentGroupPage = page;
            await LoadGroupsAsync();
        }

        private async Task OnGroupPickedAsync(SectionGroupGetDTO group)
        {
            _selectedGroupName = group.Name;
            _selectedGroupId = group.ID;
            _step = PickerStepEnum.SECTIONS;
            _currentSectionPage = 1;
            _sectionSearchTerm = string.Empty;
            _selectedSectionIds.Clear();
            await LoadSectionsAsync(group.ID);
        }

        // --- SECTIONS LOGIKA ---

        private async Task LoadSectionsAsync(int groupId)
        {
            try
            {
                _isLoadingSections = true;
                int offset = (_currentSectionPage - 1) * SectionsPerPage;
                var result = await SectionsService.GetSectionsAsync(groupId, _sectionSearchTerm, SectionStatusType.ACTIVE, offset, SectionsPerPage);
                _availableSections = result.Items ?? [];
                _totalSectionCount = result.TotalItems;
            }
            finally
            {
                _isLoadingSections = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        private async Task SearchSectionsAsync()
        {
            _currentSectionPage = 1;
            await LoadSectionsAsync(_selectedGroupId);
        }

        private async Task ClearSectionFiltersAsync()
        {
            _sectionSearchTerm = string.Empty;
            _currentSectionPage = 1;
            await LoadSectionsAsync(_selectedGroupId);
        }

        private async Task ChangeSectionPageAsync(int page)
        {
            if (page < 1 || page > TotalSectionPages || page == _currentSectionPage) return;
            _currentSectionPage = page;
            await LoadSectionsAsync(_selectedGroupId);
        }

        private void ToggleSectionSelection(int id, object? checkedValue)
        {
            if ((bool?)checkedValue == true)
                _selectedSectionIds.Add(id);
            else
                _selectedSectionIds.Remove(id);
        }

        private async Task AddSelectedSectionsAsync()
        {
            var picked = _availableSections.Where(s => _selectedSectionIds.Contains(s.ID)).ToList();
            await OnSectionsPicked.InvokeAsync(picked);
            _selectedSectionIds.Clear();
            await IsOpenChanged.InvokeAsync(false);
        }

        private async Task CloseAsync()
        {
            _selectedSectionIds.Clear();
            await IsOpenChanged.InvokeAsync(false);
        }
    }
}