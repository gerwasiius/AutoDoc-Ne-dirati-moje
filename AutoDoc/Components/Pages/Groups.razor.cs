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
        // --- INJECTION ---

        [Inject] private IHttpClientFactory HttpClientFactory { get; set; }
        [Inject] private NavigationManager Navigation { get; set; }
        [Inject] private IToastService ToastService { get; set; }

        // --- PRIVATNA POLJA ---

        private HttpClient _autoDocServiceClient;
        private List<SectionGroupGetDTO> groups = new();
        private string searchTerm = string.Empty;
        private string statusFilter = "all";
        private int currentPage = 1;
        private int itemsPerPage = 30;
        private int totalCount = 0;
        private bool isGroupModalOpen;
        private SectionGroupUpsertDTO selectedGroupForEdit;
        private bool _loading = false;

        /// <summary>
        /// Ukupan broj stranica za paginaciju.
        /// </summary>
        private int TotalPages => (int)Math.Ceiling((double)totalCount / itemsPerPage);

        /// <summary>
        /// Početni indeks prikazanih grupa na trenutnoj stranici.
        /// </summary>
        private int StartIndex => totalCount == 0 ? 0 : (currentPage - 1) * itemsPerPage;

        /// <summary>
        /// Krajnji indeks prikazanih grupa na trenutnoj stranici.
        /// </summary>
        private int EndIndex => Math.Min(StartIndex + groups.Count, totalCount);

        /// <summary>
        /// Inicijalizuje komponentu i učitava grupe sa servera.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            _autoDocServiceClient = HttpClientFactory.CreateClient("AutoDocService");
            await LoadGroupsAsync();
        }

        /// <summary>
        /// Učitava grupe sa API-ja uz primijenjene filtere i paginaciju.
        /// </summary>
        private async Task LoadGroupsAsync()
        {
            try
            {
                _loading = true;

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

                var response = await _autoDocServiceClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var pagedResult = await response.Content.ReadFromJsonAsync<PagedList<SectionGroupGetDTO>>() ?? new PagedList<SectionGroupGetDTO>();
                    groups = pagedResult.Items ?? new List<SectionGroupGetDTO>();
                    totalCount = pagedResult.TotalItems;
                }
                else
                {
                    if (response.StatusCode != System.Net.HttpStatusCode.NotFound)
                        throw new Exception("Problem prilikom učitavanja grupa");

                    groups = [];
                    totalCount = 0;
                }
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Problem prilikom učitavanja grupa: {ex.Message}");
                groups = [];
                totalCount = 0;
            }
            finally
            {
                _loading = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        /// <summary>
        /// Mijenja trenutnu stranicu u paginaciji.
        /// </summary>
        /// <param name="page">Broj stranice na koju se prelazi.</param>
        private async Task ChangePage(int page)
        {
            if (page < 1 || page > TotalPages || page == currentPage) return;
            currentPage = page;
            await LoadGroupsAsync();
        }

        /// <summary>
        /// Preusmjerava korisnika na stranicu sekcija za odabranu grupu.
        /// </summary>
        /// <param name="group">Odabrana grupa.</param>
        private void HandleViewMembers(SectionGroupGetDTO group)
        {
            Navigation.NavigateTo($"/sections/{group.ID}&groupName={Uri.EscapeDataString(group.Name)}");
        }

        /// <summary>
        /// Pokreće pretragu po nazivu grupe.
        /// </summary>
        private async Task OnSearchClicked()
        {
            currentPage = 1;
            await LoadGroupsAsync();
        }

        /// <summary>
        /// Mijenja filter statusa i učitava grupe.
        /// </summary>
        /// <param name="e">Argument promjene vrijednosti.</param>
        private async Task OnStatusFilterChanged(ChangeEventArgs e)
        {
            statusFilter = e.Value?.ToString() ?? "all";
            currentPage = 1;
            await LoadGroupsAsync();
        }

        /// <summary>
        /// Briše sve filtere i učitava sve grupe.
        /// </summary>
        private async Task OnClearFiltersClicked()
        {
            searchTerm = string.Empty;
            statusFilter = "all";
            currentPage = 1;
            await LoadGroupsAsync();
        }

        /// <summary>
        /// Otvara modal za unos nove grupe.
        /// </summary>
        private void OpenGroupModal()
        {
            selectedGroupForEdit = null;
            isGroupModalOpen = true;
        }

        /// <summary>
        /// Otvara modal za izmjenu postojeće grupe.
        /// </summary>
        /// <param name="group">Grupa za izmjenu.</param>
        private void OpenEditGroupModal(SectionGroupGetDTO group)
        {
            if (group != null)
            {
                selectedGroupForEdit = new SectionGroupUpsertDTO
                {
                    ID = group.ID,
                    Name = group.Name,
                    Description = group.Description,
                    Status = group.Status
                };
            }
            else
            {
                selectedGroupForEdit = null;
            }
            isGroupModalOpen = true;
        }

        /// <summary>
        /// Zatvara modal i ponovo učitava grupe.
        /// </summary>
        private async Task CloseGroupModal()
        {
            isGroupModalOpen = false;
            await LoadGroupsAsync();
        }
    }
}
