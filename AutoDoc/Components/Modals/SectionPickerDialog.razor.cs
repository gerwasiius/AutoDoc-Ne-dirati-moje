using AutoDocFront.Models.DTO;
using AutoDocFront.Models.DTO.GroupSection;
using AutoDocFront.Models.DTO.Sections;
using AutoDocFront.Models.Enumerations;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using static System.Collections.Specialized.BitVector32;

namespace AutoDocFront.Components.Modals
{
    public partial class SectionPickerDialog
    {
        // --- Parametri ---
        [Parameter] public bool IsOpen { get; set; }
        [Parameter] public EventCallback<bool> IsOpenChanged { get; set; }
        [Parameter] public EventCallback<List<SectionsGetDTO>> OnSectionsPicked { get; set; }

        [Inject] private IHttpClientFactory HttpClientFactory { get; set; } = default!;

        // --- Interni state ---
        private PickerStepEnum Step = PickerStepEnum.GROUPS;

        private List<SectionGroupGetDTO> _availableGroups { get; set; } = new();

        private HttpClient _httpClient;
        private string GroupSearchTerm = "";
        private string GroupStatusFilter = "ACTIVE";
        private int CurrentPage = 1;
        private int GroupsPerPage = 5;
        private int TotalGroupCount = 0;
        private bool IsLoadingGroups = false;
        private List<SectionsGetDTO> _availableSections = new();
        private int SectionsCurrentPage = 1;
        private int SectionsPerPage = 5;
        private int TotalSectionCount = 0;
        private bool IsLoadingSections = false;
        private int SelectedGroupId = 0;

        private async Task OnGroupPickedAsync(SectionGroupGetDTO group)
        {
            SelectedGroup = group.Name;
            SelectedGroupId = group.ID;
            Step = PickerStepEnum.SECTIONS;
            SectionsCurrentPage = 1;
            SectionSearchTerm = string.Empty;
            await LoadSectionsForGroupAsync(group.ID);
        }

        private int SectionsTotalPages => (int)Math.Ceiling((double)TotalSectionCount / SectionsPerPage);
        private int SectionsStartIndex => TotalSectionCount == 0 ? 0 : (SectionsCurrentPage - 1) * SectionsPerPage;
        private int SectionsEndIndex => Math.Min(SectionsStartIndex + _availableSections.Count, TotalSectionCount);



        private int TotalPages => (int)Math.Ceiling((double)TotalGroupCount / GroupsPerPage);
        private int StartIndex => TotalGroupCount == 0 ? 0 : (CurrentPage - 1) * GroupsPerPage;
        private int EndIndex => Math.Min(StartIndex + _availableGroups.Count, TotalGroupCount);


        private string SelectedGroup = "";
        private string SectionSearchTerm = "";
        private HashSet<int> SelectedSectionIds = new();

        //private IEnumerable<Section> FilteredSections =>
        //    AvailableSections
        //        .Where(s => (string.IsNullOrEmpty(SelectedGroup) || s.GroupName == SelectedGroup)
        //            && (string.IsNullOrEmpty(SectionSearchTerm) || s.Name.Contains(SectionSearchTerm, StringComparison.OrdinalIgnoreCase)));


        /// <summary>
        /// Inicijalizuje komponentu i učitava grupe sa servera.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            Step = PickerStepEnum.GROUPS; // Ensure the picker starts at the Groups step
            _httpClient = HttpClientFactory.CreateClient("AutoDocService");
            await LoadActiveGroupsAsync();
        }

        private async Task LoadActiveGroupsAsync()
        {
            try
            {
                IsLoadingGroups = true;
                int offset = (CurrentPage - 1) * GroupsPerPage;

                var query = new List<string>
                {
                    $"offset={offset}",
                    $"pageSize={GroupsPerPage}"
                };

                if (!string.IsNullOrWhiteSpace(GroupSearchTerm))
                    query.Add($"name={Uri.EscapeDataString(GroupSearchTerm)}");

                    query.Add($"status=ACTIVE");

                var apiUrl = "/api/contract-generation/section-groups";
                if (query.Count > 0)
                    apiUrl += "?" + string.Join("&", query);

                var response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<PagedList<SectionGroupGetDTO>>() ?? new();
                    _availableGroups = result.Items ?? [];
                    TotalGroupCount = result.TotalItems;
                }
                else
                {
                    _availableGroups = [];
                    TotalGroupCount = 0;
                }
            }
            finally
            {
                IsLoadingGroups = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        private async Task LoadSectionsForGroupAsync(int groupId)
        {
            try
            {
                IsLoadingSections = true;
                int offset = (SectionsCurrentPage - 1) * SectionsPerPage;

                var query = new List<string>
        {
            $"groupId={groupId}",
            $"offset={offset}",
            $"pageSize={SectionsPerPage}",
            $"isActive=true"
        };

                if (!string.IsNullOrWhiteSpace(SectionSearchTerm))
                    query.Add($"name={Uri.EscapeDataString(SectionSearchTerm)}");

                var apiUrl = "/api/contract-generation/sections";
                if (query.Count > 0)
                    apiUrl += "?" + string.Join("&", query);

                var response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<PagedList<SectionsGetDTO>>() ?? new();
                    _availableSections = result.Items ?? [];
                    TotalSectionCount = result.TotalItems;
                }
                else
                {
                    _availableSections = [];
                    TotalSectionCount = 0;
                }
            }
            finally
            {
                IsLoadingSections = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        private async Task SearchSectionsAsync()
        {
            SectionsCurrentPage = 1;
            await LoadSectionsForGroupAsync(SelectedGroupId);
        }

        private async Task ClearSectionFiltersAsync()
        {
            SectionSearchTerm = string.Empty;
            SectionsCurrentPage = 1;
            await LoadSectionsForGroupAsync(SelectedGroupId);
        }

        private async Task ChangeSectionsPageAsync(int page)
        {
            if (page < 1 || page > SectionsTotalPages || page == SectionsCurrentPage) return;
            SectionsCurrentPage = page;
            await LoadSectionsForGroupAsync(SelectedGroupId);
        }

        private void ToggleSectionSelection(int id, object? checkedValue)
        {
            if ((bool?)checkedValue == true)
                SelectedSectionIds.Add(id);
            else
                SelectedSectionIds.Remove(id);
        }

        //private async Task AddSelectedSections()
        //{
        //    var picked = _availableGroups.Where(s => SelectedSectionIds.Contains(s.ID)).ToList();
        //    await OnSectionsPicked.InvokeAsync(picked);
        //    SelectedSectionIds.Clear();
        //    await IsOpenChanged.InvokeAsync(false);
        //}

        private async Task Close()
        {
            SelectedSectionIds.Clear();
            await IsOpenChanged.InvokeAsync(false);
        }

        //private async Task OnGroupChangedHandler()
        //{
        //    if (OnGroupChanged.HasDelegate)
        //        await OnGroupChanged.InvokeAsync(SelectedGroup);
        //}

        private async Task SearchGroupsAsync()
        {
            CurrentPage = 1;
            await LoadActiveGroupsAsync();
        }

        private async Task ClearGroupFiltersAsync()
        {
            GroupSearchTerm = string.Empty;
            CurrentPage = 1;
            await LoadActiveGroupsAsync();
        }

        private async Task ChangePageAsync(int page)
        {
            if (page < 1 || page > TotalPages || page == CurrentPage) return;
            CurrentPage = page;
            await LoadActiveGroupsAsync();
        }

        // Called when a group is picked (advance to SECTIONS step, or your logic)
        private void OnGroupPicked(SectionGroupGetDTO group)
        {
            SelectedGroup = group.Name; // or group.ID, depending on your logic
                                        // Step = PickerStepEnum.SECTIONS; // Uncomment if you want to move to next step
                                        // await LoadSectionsForGroupAsync(group.ID); // Implement as needed
        }

        private async Task AddSelectedSectionsAsync()
        {
            var picked = _availableSections.Where(s => SelectedSectionIds.Contains(s.ID)).ToList();
            await OnSectionsPicked.InvokeAsync(picked);
            SelectedSectionIds.Clear();
            await IsOpenChanged.InvokeAsync(false);
        }
    }
}
