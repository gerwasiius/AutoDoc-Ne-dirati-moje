using AutoDoc.Shared.Model.DTO.DocumentTemplateDTO;
using AutoDoc.Shared.Model.DTO.Enumerations;
using AutoDocFront.Models.Enumerations;
using AutoDocFront.Services;
using AutoDocFront.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace AutoDocFront.Components.Pages
{
    /// <summary>
    /// Stranica za upravljanje dokument template-ima.
    /// Omogućava filtriranje, pretragu, paginaciju i rad sa template modalima.
    /// </summary>
    public partial class Templates
    {
        // --- INJECTION ---

        [Inject] private DocumentTemplateApiService TemplateService { get; set; } = default!;
        [Inject] private IToastService ToastService { get; set; } = default!;

        // --- POLJA ---

        private const int ItemsPerPage = 30;

        private List<DocumentTemplateGetDTO> _templates = new();
        private string _searchTerm = string.Empty;
        private int _currentPage = 1;
        private int _totalCount = 0;
        private DocumentTemplateStatusType? _statusFilter;
        private DocumentTemplateGetDTO? _selectedTemplate;
        
        private bool _isLoading = false;

        // --- MODAL STATE ---

        private bool _isTemplateModalOpen = false;
        private ModalMode _templateModalMode = ModalMode.INSERT;

        private bool _isSectionsModalOpen = false;
        private bool _isPreviewModalOpen = false;
        private string? _previewHtmlContent;
        private bool _previewLoading = false;
        private string? _previewError;
        private string? _previewTemplateName;
        // -------------------
        /// <summary>
        /// Svi mogući statusi template-a (za filter bar).
        /// </summary>
        private static readonly IEnumerable<DocumentTemplateStatusType> _statusValues =
            Enum.GetValues(typeof(DocumentTemplateStatusType)).Cast<DocumentTemplateStatusType>();

        // --- PAGINATION PROPERTIES ---

        /// <summary>
        /// Ukupan broj stranica za paginaciju.
        /// </summary>
        private int TotalPages => PaginationHelper.CalculateTotalPages(_totalCount, ItemsPerPage);

        /// <summary>
        /// Indeks prvog prikazanog template-a na trenutnoj stranici.
        /// </summary>
        private int StartIndex => PaginationHelper.CalculateStartIndex(_currentPage, ItemsPerPage, _totalCount);

        /// <summary>
        /// Indeks zadnjeg prikazanog template-a na trenutnoj stranici.
        /// </summary>
        private int EndIndex => PaginationHelper.CalculateEndIndex(StartIndex, _templates.Count, _totalCount);

        // --- LIFECYCLE ---

        /// <summary>
        /// Inicijalizuje komponentu i učitava template-e.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            try
            {
                await LoadTemplatesAsync();
            }
            catch (Exception ex)
            {
                ToastService.ShowError("Došlo je do greške prilikom inicijalizacije stranice.\n\nDetalji: " + ex.Message);
            }
        }

        // --- API POZIVI ---

        /// <summary>
        /// Učitava template-e sa servera uz filtere i paginaciju.
        /// </summary>
        private async Task LoadTemplatesAsync()
        {
            try
            {
                _isLoading = true;
                var result = await TemplateService.GetTemplatesAsync(_searchTerm, _statusFilter, (_currentPage - 1) * ItemsPerPage, ItemsPerPage);
                _templates = result.Items ?? [];
                _totalCount = result.TotalItems;
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Greška prilikom učitavanja template-a: {ex.Message}");
                _templates = [];
                _totalCount = 0;
            }
            finally
            {
                _isLoading = false;
            }
        }

        // --- PAGINATION & FILTERS ---

        /// <summary>
        /// Mijenja trenutnu stranicu u paginaciji.
        /// </summary>
        /// <param name="page">Nova stranica.</param>
        private async Task ChangePageAsync(int page)
        {
            if (page < 1 || page > TotalPages || page == _currentPage) return;
            try
            {
                _currentPage = page;
                await LoadTemplatesAsync();
            }
            catch (Exception ex)
            {
                ToastService.ShowError("Greška prilikom promjene stranice: " + ex.Message);
            }
        }

        /// <summary>
        /// Pokreće pretragu po nazivu template-a.
        /// </summary>
        private async Task SearchTemplatesAsync()
        {
            try
            {
                _currentPage = 1;
                await LoadTemplatesAsync();
            }
            catch (Exception ex)
            {
                ToastService.ShowError("Greška prilikom pretrage template-a: " + ex.Message);
            }
        }

        /// <summary>
        /// Mijenja filter statusa i učitava template-e.
        /// </summary>
        /// <param name="value">Novi status filtera.</param>
        private async Task OnStatusFilterChangedAsync(DocumentTemplateStatusType? value)
        {
            try
            {
                _statusFilter = value;
                _currentPage = 1;
                await LoadTemplatesAsync();
            }
            catch (Exception ex)
            {
                ToastService.ShowError("Greška prilikom filtriranja template-a: " + ex.Message);
            }
        }

        /// <summary>
        /// Briše sve filtere i učitava sve template-e.
        /// </summary>
        private async Task ClearTemplateFiltersAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_searchTerm) && _statusFilter == null)
                    return;

                _searchTerm = string.Empty;
                _statusFilter = null;
                _currentPage = 1;
                await LoadTemplatesAsync();
            }
            catch (Exception ex)
            {
                ToastService.ShowError("Greška prilikom čišćenja filtera: " + ex.Message);
            }
        }

        // --- MODAL LOGIKA ---

        /// <summary>
        /// Otvara modal za unos novog template-a.
        /// </summary>
        private void OpenInsertTemplateModal()
        {
            _selectedTemplate = null;
            _templateModalMode = ModalMode.INSERT;
            _isTemplateModalOpen = true;
        }

        /// <summary>
        /// Otvara modal za izmjenu postojećeg template-a.
        /// </summary>
        /// <param name="template">Template za izmjenu.</param>
        private void OpenEditTemplateModal(DocumentTemplateGetDTO template)
        {
            _selectedTemplate = template;
            _templateModalMode = ModalMode.EDIT;
            _isTemplateModalOpen = true;
        }

        /// <summary>
        /// Otvara modal za pregled template-a.
        /// </summary>
        /// <param name="template">Template za pregled.</param>
        private void OpenViewTemplateModal(DocumentTemplateGetDTO template)
        {
            _selectedTemplate = template;
            _templateModalMode = ModalMode.VIEW;
            _isTemplateModalOpen = true;
        }

        /// <summary>
        /// Handler nakon uspješnog snimanja template-a iz modala.
        /// </summary>
        private async Task OnTemplateModalSavedAsync()
        {
            try
            {
                _isTemplateModalOpen = false;
                await LoadTemplatesAsync();
            }
            catch (Exception ex)
            {
                ToastService.ShowError("Greška prilikom osvježavanja template-a nakon izmjene: " + ex.Message);
            }
        }

        /// <summary>
        /// Otvara modal za uređivanje sekcija template-a.
        /// </summary>
        /// <param name="template">Template za uređivanje sekcija.</param>
        private void OpenSectionsModal(DocumentTemplateGetDTO template)
        {
            try
            {
                _selectedTemplate = template;
                _isSectionsModalOpen = true;
            }
            catch (Exception ex)
            {
                ToastService.ShowError("Greška prilikom otvaranja modala za sekcije: " + ex.Message);
            }
        }

        /// <summary>
        /// Zatvara modal za sekcije.
        /// </summary>
        private void CloseSectionsModal()
        {
            try
            {
                _isSectionsModalOpen = false;
            }
            catch (Exception ex)
            {
                ToastService.ShowError("Greška prilikom zatvaranja modala za sekcije: " + ex.Message);
            }
        }

        /// <summary>
        /// Otvara modal za preview template-a.
        /// </summary>
        /// <param name="template">Template za preview.</param>
        private async Task ShowPreviewModalAsync(DocumentTemplateGetDTO template)
        {
            try
            {
                ResetPreviewModal();
                _previewTemplateName = template.Name;
                _isPreviewModalOpen = true;
                _previewLoading = true;

                try
                {
                    var html = await TemplateService.GetTemplatePreviewAsync(template.IdTemplate, template.Version);
                    _previewHtmlContent = html;
                }
                catch (Exception ex)
                {
                    _previewError = $"Greška: {ex.Message}";
                }
                finally
                {
                    _previewLoading = false;
                }
            }
            catch (Exception ex)
            {
                ToastService.ShowError("Greška prilikom otvaranja preview modala: " + ex.Message);
                _previewLoading = false;
            }
        }

        /// <summary>
        /// Resetuje stanje preview modala.
        /// </summary>
        private void ResetPreviewModal()
        {
            try
            {
                _previewHtmlContent = null;
                _previewError = null;
                _previewTemplateName = null;
                _previewLoading = false;
            }
            catch (Exception ex)
            {
                ToastService.ShowError("Greška prilikom resetovanja preview modala: " + ex.Message);
            }
        }

        // --- HELPERI ---

        /// <summary>
        /// Vraća CSS klasu za badge statusa template-a.
        /// </summary>
        /// <param name="status">Status template-a.</param>
        /// <returns>CSS klasa za badge.</returns>
        private string GetStatusBadgeClass(DocumentTemplateStatusType? status) => status switch
        {
            DocumentTemplateStatusType.ACTIVE => "bg-success",
            DocumentTemplateStatusType.IN_PROGRESS => "bg-warning text-dark",
            DocumentTemplateStatusType.PENDING => "bg-info text-dark",
            DocumentTemplateStatusType.DEACTIVATED => "bg-secondary",
            _ => "bg-light text-dark"
        };

        /// <summary>
        /// Vraća prikazni naziv statusa template-a.
        /// </summary>
        /// <param name="status">Status template-a.</param>
        /// <returns>Prikazni naziv statusa.</returns>
        private string GetStatusDisplayName(DocumentTemplateStatusType? status) => status switch
        {
            DocumentTemplateStatusType.ACTIVE => "ACTIVE",
            DocumentTemplateStatusType.IN_PROGRESS => "IN PROGRESS",
            DocumentTemplateStatusType.PENDING => "PENDING",
            DocumentTemplateStatusType.DEACTIVATED => "DEACTIVATED",
            _ => "UNKNOWN"
        };
    }
}
