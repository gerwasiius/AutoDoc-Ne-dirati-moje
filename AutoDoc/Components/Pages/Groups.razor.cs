using AutoDocFront.Components.Shared;
using AutoDocFront.Models.DTO;
using AutoDocFront.Models.DTO.GroupSection;
using AutoDocFront.Models.Enumerations;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using System.Net.Http.Json;

namespace AutoDocFront.Components.Pages
{
    /// <summary>
    /// Blazor stranica za upravljanje grupama sekcija (Grupe članova).
    /// Omogućava filtriranje, pretragu, paginaciju i CRUD operacije nad grupama.
    /// </summary>
    public partial class Groups
    {
        // --- DEPENDENCY INJECTION ---

        [Inject] private IHttpClientFactory HttpClientFactory { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private IToastService ToastService { get; set; } = default!;

        // --- POLJA ---

        private HttpClient _httpClient;
        private List<SectionGroupGetDTO> groupList = new();
        private string searchTerm = string.Empty;
        private string statusFilter = "all";
        private int currentPage = 1;
        private int itemsPerPage = 30;
        private int totalGroupCount = 0;
        private bool isGroupModalVisible;
        private SectionGroupUpsertDTO selectedGroupDto;
        private bool isLoading;

        /// <summary>
        /// Ukupan broj stranica za paginaciju.
        /// </summary>
        private int TotalPages => (int)Math.Ceiling((double)totalGroupCount / itemsPerPage);

        /// <summary>
        /// Početni indeks prikazanih grupa na trenutnoj stranici.
        /// </summary>
        private int StartIndex => totalGroupCount == 0 ? 0 : (currentPage - 1) * itemsPerPage;

        /// <summary>
        /// Krajnji indeks prikazanih grupa na trenutnoj stranici.
        /// </summary>
        private int EndIndex => Math.Min(StartIndex + groupList.Count, totalGroupCount);

        /// <summary>
        /// Inicijalizuje komponentu i učitava grupe sa servera.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            _httpClient = HttpClientFactory.CreateClient("AutoDocService");
            await LoadGroupsAsync();
        }

        /// <summary>
        /// Učitava grupe sa API-ja uz primijenjene filtere i paginaciju.
        /// </summary>
        private async Task LoadGroupsAsync()
        {
            try
            {
                isLoading = true;
                int offset = (currentPage - 1) * itemsPerPage;

                var query = new List<string>
                {
                    $"offset={offset}",
                    $"pageSize={itemsPerPage}"
                };

                if (!string.IsNullOrWhiteSpace(searchTerm))
                    query.Add($"name={Uri.EscapeDataString(searchTerm)}");

                if (statusFilter != "all")
                    query.Add($"status={statusFilter}");

                var apiUrl = "/api/contract-generation/section-groups";
                if (query.Count > 0)
                    apiUrl += "?" + string.Join("&", query);

                var response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<PagedList<SectionGroupGetDTO>>() ?? new();
                    groupList = result.Items ?? [];
                    totalGroupCount = result.TotalItems;
                }
                else
                {
                    if (response.StatusCode != System.Net.HttpStatusCode.NotFound)
                        throw new Exception("Greška prilikom učitavanja grupa.");

                    groupList = [];
                    totalGroupCount = 0;
                }
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Greška prilikom učitavanja grupa: {ex.Message}");
                groupList = [];
                totalGroupCount = 0;
            }
            finally
            {
                isLoading = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        /// <summary>
        /// Mijenja trenutnu stranicu u paginaciji.
        /// </summary>
        private async Task ChangePageAsync(int page)
        {
            if (page < 1 || page > TotalPages || page == currentPage) return;
            currentPage = page;
            await LoadGroupsAsync();
        }

        /// <summary>
        /// Preusmjerava korisnika na stranicu članova grupe.
        /// </summary>
        private void NavigateToGroupSections(SectionGroupGetDTO group)
        {
            Navigation.NavigateTo($"/sections/{group.ID}&groupName={Uri.EscapeDataString(group.Name)}");
        }

        /// <summary>
        /// Aktivira pretragu grupa na osnovu naziva.
        /// </summary>
        private async Task SearchGroupsAsync()
        {
            currentPage = 1;
            await LoadGroupsAsync();
        }

        /// <summary>
        /// Mijenja filter statusa i ponovo učitava grupe.
        /// </summary>
        private async Task HandleStatusFilterChanged(ChangeEventArgs e)
        {
            statusFilter = e.Value?.ToString() ?? "all";
            currentPage = 1;
            await LoadGroupsAsync();
        }

        /// <summary>
        /// Čisti sve filtere i resetira prikaz grupa.
        /// </summary>
        private async Task ClearFiltersAsync()
        {
            searchTerm = string.Empty;
            statusFilter = "all";
            currentPage = 1;
            await LoadGroupsAsync();
        }

        /// <summary>
        /// Otvara modal za unos nove grupe.
        /// </summary>
        private void ShowCreateGroupModal()
        {
            selectedGroupDto = null;
            isGroupModalVisible = true;
        }

        /// <summary>
        /// Otvara modal za uređivanje postojeće grupe.
        /// </summary>
        private void ShowEditGroupModal(SectionGroupGetDTO group)
        {
            selectedGroupDto = new SectionGroupUpsertDTO
            {
                ID = group.ID,
                Name = group.Name,
                Description = group.Description,
                Status = group.Status
            };
            isGroupModalVisible = true;
        }

        /// <summary>
        /// Zatvara modal i osvježava prikaz grupa nakon izmjene.
        /// </summary>
        private async Task OnGroupModalSavedAsync()
        {
            isGroupModalVisible = false;
            await LoadGroupsAsync();
        }
    }
}