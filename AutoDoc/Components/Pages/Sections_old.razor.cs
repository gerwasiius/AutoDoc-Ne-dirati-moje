using AutoDocFront.Models.DTO.GroupSection;
using Microsoft.AspNetCore.Components;

namespace AutoDocFront.Components.Pages
{
    public partial class Sections_old
    {
        private HttpClient _autoDocServiceClient;
        private List<SectionGroupGetDTO> groupSection = new();
        private bool isSearchVisible = false;
        private string searchTerm;
        private bool _loading = false;

        private IEnumerable<SectionGroupGetDTO> filteredGroupSections => groupSection
            .Where(g => string.IsNullOrEmpty(searchTerm) || g.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .ToList();

        protected override async Task OnInitializedAsync()
        {
            // Initialize HTTP client for contract generation
            _autoDocServiceClient = httpClientFactory.CreateClient("AutoDocService");
            await LoadGroupSectionsAsync();
        }

        private async Task LoadGroupSectionsAsync()
        {
            try
            {
                _loading = true;
                var response = await _autoDocServiceClient.GetAsync("/api/contract-generation/section-groups?status=ACTIVE");
                if (response.IsSuccessStatusCode)
                {
                    groupSection = await response.Content.ReadFromJsonAsync<List<SectionGroupGetDTO>>() ?? new List<SectionGroupGetDTO>();
                }
                else
                {
                    // TODO: Handle failure scenario
                }
            }
            catch (HttpRequestException ex)
            {
                toastService.ShowError("Problem prilikom dobavljanja podataka za grupe clanova!");
                // TODO: Handle exception
            }
            finally
            {
                _loading = false;
            }
        }

        private void ClosePrompt()
        {
            NavigationManager.NavigateTo("/");
        }

        private void ToggleSearch()
        {
            isSearchVisible = !isSearchVisible;
            if (isSearchVisible)
            {
                searchTerm = string.Empty;
            }
        }
    }
}