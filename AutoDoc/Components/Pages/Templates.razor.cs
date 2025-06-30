using AutoDocFront.Models.DTO;
using AutoDocFront.Models.DTO.DocumentTemplate;
using AutoDocFront.Models.DTO.DocumentTemplateDTO;
using AutoDocFront.Models.DTO.GroupSection;
using AutoDocFront.Models.DTO.Sections;
using AutoDocFront.Models.Enumerations;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace AutoDocFront.Components.Pages
{
    public partial class Templates
    {
        // --- INJECTION ---

        [Inject] private IHttpClientFactory HttpClientFactory { get; set; }
        [Inject] private NavigationManager Navigation { get; set; }
        [Inject] private IToastService ToastService { get; set; }

        // --- PRIVATNA POLJA ---

        private HttpClient _autoDocServiceClient;
        private List<DocumentTemplateGetDTO> templates = new();
        private string searchTerm = string.Empty;
        private string statusFilter = "all";
        private int currentPage = 1;
        private int itemsPerPage = 30;
        private int totalCount = 0;
        private bool isGroupModalOpen;
        private SectionGroupUpsertDTO selectedGroupForEdit;
        private bool _loading = false;
        private bool isTemplateModalOpen = false;
        private ModalMode templateModalMode = ModalMode.INSERT;
        private DocumentTemplateGetDTO selectedTemplate = null;
        private bool isSectionsModalOpen = false;
        private bool IsSectionPickerOpen = false;
        private List<SectionGroupGetDTO> availableGroups = new();
        private List<SectionsGetDTO> availableSections = new();



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
        private int EndIndex => Math.Min(StartIndex + templates.Count, totalCount);

        /// <summary>
        /// Inicijalizuje komponentu i učitava grupe sa servera.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            _autoDocServiceClient = HttpClientFactory.CreateClient("AutoDocService");
            await LoadDocumentTemplatesAsync();
        }

        /// <summary>
        /// Učitava grupe sa API-ja uz primijenjene filtere i paginaciju.
        /// </summary>
        private async Task LoadDocumentTemplatesAsync()
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

                var apiUrl = "/api/contract-generation/document-templates/";
                if (query.Count > 0)
                    apiUrl += "?" + string.Join("&", query);

                var response = await _autoDocServiceClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var pagedResult = await response.Content.ReadFromJsonAsync<PagedList<DocumentTemplateGetDTO>>() ?? new PagedList<DocumentTemplateGetDTO>();
                    templates = pagedResult.Items ?? new List<DocumentTemplateGetDTO>();
                    totalCount = pagedResult.TotalItems;
                }
                else
                {
                    if (response.StatusCode != System.Net.HttpStatusCode.NotFound)
                        throw new Exception("Problem prilikom učitavanja grupa");

                    templates = [];
                    totalCount = 0;
                }
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Problem prilikom učitavanja grupa: {ex.Message}");
                templates = [];
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
            await LoadDocumentTemplatesAsync();
        }

        /// <summary>
        /// Preusmjerava korisnika na stranicu sekcija za odabranu grupu.
        /// </summary>
        /// <param name="group">Odabrana grupa.</param>
        private void HandleViewMembers(SectionGroupGetDTO group)
        {
            Navigation.NavigateTo($"/sections?groupId={group.ID}&groupName={Uri.EscapeDataString(group.Name)}");
        }

        /// <summary>
        /// Pokreće pretragu po nazivu grupe.
        /// </summary>
        private async Task OnSearchClicked()
        {
            currentPage = 1;
            await LoadDocumentTemplatesAsync();
        }

        /// <summary>
        /// Mijenja filter statusa i učitava grupe.
        /// </summary>
        /// <param name="e">Argument promjene vrijednosti.</param>
        private async Task OnStatusFilterChanged(ChangeEventArgs e)
        {
            statusFilter = e.Value?.ToString() ?? "all";
            currentPage = 1;
            await LoadDocumentTemplatesAsync();
        }

        /// <summary>
        /// Briše sve filtere i učitava sve grupe.
        /// </summary>
        private async Task OnClearFiltersClicked()
        {
            searchTerm = string.Empty;
            statusFilter = "all";
            currentPage = 1;
            await LoadDocumentTemplatesAsync();
        }

        private void ShowEditTemplateModal()
        {
            templateModalMode = ModalMode.EDIT;
            // isTemplateModalOpen ostaje true
            StateHasChanged();
        }

        private async Task OnTemplateModalSave()
        {
            isTemplateModalOpen = false;
            await LoadDocumentTemplatesAsync();
        }

        private void CloseTemplateModal()
        {
            isTemplateModalOpen = false;
        }

        private void ShowViewTemplateModal(DocumentTemplateGetDTO template)
        {
            selectedTemplate = template;
            templateModalMode = ModalMode.VIEW;
            isTemplateModalOpen = true;
        }

        private void ShowCreateTemplateModal()
        {
            selectedTemplate = null;
            templateModalMode = ModalMode.INSERT;
            isTemplateModalOpen = true;
        }

        private string GetStatusBadgeClass(DocumentTemplateStatusType? status)
        {
            return status switch
            {
                DocumentTemplateStatusType.ACTIVE => "bg-success",
                DocumentTemplateStatusType.IN_PROGRESS => "bg-warning text-dark",
                DocumentTemplateStatusType.PENDING => "bg-info text-dark",
                DocumentTemplateStatusType.DEACTIVATED => "bg-secondary",
                _ => "bg-light text-dark"
            };
        }

        private string GetStatusDisplayName(DocumentTemplateStatusType? status)
        {
            return status switch
            {
                DocumentTemplateStatusType.ACTIVE => "ACTIVE",
                DocumentTemplateStatusType.IN_PROGRESS => "IN PROGRESS",
                DocumentTemplateStatusType.PENDING => "PENDING",
                DocumentTemplateStatusType.DEACTIVATED => "DEACTIVATED",
                _ => "UNKNOWN"
            };
        }

        private void ShowSectionsModal(DocumentTemplateGetDTO template)
        {
            selectedTemplate = template;
            isSectionsModalOpen = true;
        }
        private void CloseSectionsModal()
        {
            isSectionsModalOpen = false;
            StateHasChanged();
        }

        private async Task OpenSectionPickerAsync()
        {
            IsSectionPickerOpen = true;
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

                var query = new List<string>
                {
                    $"offset=0",
                    $"pageSize=0"
                };

                query.Add($"status=ACTIVE");

                var apiUrl = "/api/contract-generation/section-groups";
                if (query.Count > 0)
                    apiUrl += "?" + string.Join("&", query);

                var response = await _autoDocServiceClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var pagedResult = await response.Content.ReadFromJsonAsync<PagedList<SectionGroupGetDTO>>() ?? new PagedList<SectionGroupGetDTO>();
                    availableGroups = pagedResult.Items ?? new List<SectionGroupGetDTO>();
                }
                else
                {
                    if (response.StatusCode != System.Net.HttpStatusCode.NotFound)
                        throw new Exception("Problem prilikom učitavanja grupa");

                    availableGroups = [];
                }
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Problem prilikom učitavanja grupa: {ex.Message}");
                availableGroups = [];
            }
            finally
            {
                _loading = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        private async Task HandleGroupChanged(string groupName)
        {
            // Pozovi API za sekcije na osnovu groupName
            await LoadSectionsForGroupAsync(groupName);
            // Osvježi modal (ako treba)
            StateHasChanged();
        }

        private async Task LoadSectionsForGroupAsync(string groupName)
        {
            // Pronađi ID grupe po imenu (ako treba)
            var group = availableGroups.FirstOrDefault(g => g.Name == groupName);
            if (group == null)
            {
                availableSections = [];
                return;
            }

            var response = await _autoDocServiceClient.GetAsync($"/api/contract-generation/sections?groupId={group.ID}&isLatestOnly=true");
            if (response.IsSuccessStatusCode)
            {
                var pagedResult = await response.Content.ReadFromJsonAsync<PagedList<SectionsGetDTO>>() ?? new PagedList<SectionsGetDTO>();
                availableSections = pagedResult.Items ?? new List<SectionsGetDTO>();
            }
            else
            {
                availableSections = [];
            }
        }

        private bool isPreviewModalOpen = false;
        private string previewHtmlContent;
        private bool previewLoading = false;
        private string previewError;
        private string previewTemplateName;


        // Za spremljeni template
        private async Task ShowPreviewModal(DocumentTemplateGetDTO template)
        {
            previewTemplateName = template.Name;
            previewLoading = true;
            previewError = null;
            previewHtmlContent = null;
            isPreviewModalOpen = true;

            try
            {
                var client = HttpClientFactory.CreateClient("AutoDocService");
                var response = await client.GetAsync($"/api/document-render/{template.IdTemplate}/render?version={template.Version}");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<PreviewResponse>();
                    previewHtmlContent = result?.htmlContent;
                }
                else
                {
                    previewError = "Greška prilikom dohvata pregleda dokumenta.";
                }
            }
            catch (Exception ex)
            {
                previewError = $"Greška: {ex.Message}";
            }
            finally
            {
                previewLoading = false;
                StateHasChanged();
            }
        }

        private class PreviewResponse
        {
            public string htmlContent { get; set; }
        }
    }
}
