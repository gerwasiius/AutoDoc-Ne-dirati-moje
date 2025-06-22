using AutoDocFront.Models.DTO;
using AutoDocFront.Models.DTO.GroupSection;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using System.Net.Http.Json;

namespace AutoDocFront.Components.Pages
{
    /// <summary>
    /// Stranica za odabir grupe sekcija. Prikazuje samo aktivne grupe sa pretragom i paginacijom.
    /// </summary>
    public partial class GroupSelection
    {
        [Inject] private IHttpClientFactory HttpClientFactory { get; set; }
        [Inject] private NavigationManager Navigation { get; set; }
        [Inject] private IToastService ToastService { get; set; }

        private HttpClient _autoDocServiceClient;
        private List<SectionGroupGetDTO> groups = new();
        private string searchTerm = string.Empty;
        private int currentPage = 1;
        private int itemsPerPage = 30;
        private int totalCount = 0;
        private bool _loading = false;

        private int TotalPages => (int)Math.Ceiling((double)totalCount / itemsPerPage);
        private int StartIndex => totalCount == 0 ? 0 : (currentPage - 1) * itemsPerPage;
        private int EndIndex => Math.Min(StartIndex + groups.Count, totalCount);

        /// <summary>
        /// Inicijalizuje komponentu i učitava aktivne grupe sa servera.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            _autoDocServiceClient = HttpClientFactory.CreateClient("AutoDocService");
            await LoadGroupsAsync();
        }

        /// <summary>
        /// Učitava aktivne grupe sa API-ja uz pretragu i paginaciju.
        /// </summary>
        private async Task LoadGroupsAsync()
        {
            try
            {
                _loading = true;
                int offset = (currentPage - 1) * itemsPerPage;
                var query = new List<string>
                {
                    $"status=ACTIVE",
                    $"offset={offset}",
                    $"pageSize={itemsPerPage}"
                };

                if (!string.IsNullOrWhiteSpace(searchTerm))
                    query.Add($"name={Uri.EscapeDataString(searchTerm)}");

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
        private async Task ChangePage(int page)
        {
            if (page < 1 || page > TotalPages || page == currentPage) return;
            currentPage = page;
            await LoadGroupsAsync();
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
        /// Briše filter pretrage i učitava sve aktivne grupe.
        /// </summary>
        private async Task OnClearFiltersClicked()
        {
            searchTerm = string.Empty;
            currentPage = 1;
            await LoadGroupsAsync();
        }

        /// <summary>
        /// Preusmjerava korisnika na sekcije odabrane grupe.
        /// </summary>
        private void HandleGroupSelect(int groupId, string groupName)
        {
            Navigation.NavigateTo($"/sections/{groupId}?groupName={Uri.EscapeDataString(groupName)}");
        }
    }
}
