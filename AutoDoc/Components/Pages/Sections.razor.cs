using AutoDocFront.Models.DTO;
using AutoDocFront.Models.DTO.GroupSection;
using AutoDocFront.Models.DTO.Sections;
using AutoDocFront.Models.Enumerations;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http.Json;

namespace AutoDocFront.Components.Pages
{
    /// <summary>
    /// Stranica za upravljanje sekcijama i članovima unutar grupe.
    /// Omogućava filtriranje, pretragu, paginaciju i izmjenu statusa sekcija.
    /// </summary>
    public partial class Sections
    {
        // --- PARAMETRI ---

        /// <summary>
        /// ID grupe sekcija (preko URL-a).
        /// </summary>
        [Parameter] public string GroupId { get; set; }

        /// <summary>
        /// Naziv grupe sekcija (preko query stringa, opciono).
        /// </summary>
        [Parameter, SupplyParameterFromQuery] public string? GroupName { get; set; }

        // --- INJECTION ---

        /// <summary>
        /// Fabrika za kreiranje HttpClient instanci.
        /// </summary>
        [Inject] private IHttpClientFactory HttpClientFactory { get; set; }

        /// <summary>
        /// Servis za upravljanje navigacijom.
        /// </summary>
        [Inject] private NavigationManager Navigation { get; set; }

        /// <summary>
        /// Servis za prikaz notifikacija (toast poruka).
        /// </summary>
        [Inject] private IToastService ToastService { get; set; }

        /// <summary>
        /// JavaScript runtime servis.
        /// </summary>
        [Inject] private IJSRuntime JSRuntime { get; set; }

        // --- PRIVATNA POLJA ---

        /// <summary>
        /// HttpClient za komunikaciju sa API-jem.
        /// </summary>
        private HttpClient _client;

        /// <summary>
        /// DTO objekat grupe sekcija.
        /// </summary>
        private SectionGroupGetDTO _group;

        /// <summary>
        /// Lista svih sekcija u grupi.
        /// </summary>
        private List<SectionsGetDTO> _sections = new();

        /// <summary>
        /// Tekući termin za pretragu po nazivu sekcije.
        /// </summary>
        private string _searchTerm = string.Empty;

        /// <summary>
        /// Filter za status sekcije (sve, aktivne, deaktivirane).
        /// </summary>
        private SectionStatusType? _statusFilter = null;

        /// <summary>
        /// Trenutna stranica u paginaciji.
        /// </summary>
        private int _currentPage = 1;

        /// <summary>
        /// Broj sekcija po stranici.
        /// </summary>
        private readonly int _itemsPerPage = 20;

        /// <summary>
        /// Da li je trenutno učitavanje u toku.
        /// </summary>
        private bool _loading = false;

        // --- MODAL STANJE ---

        /// <summary>
        /// Da li je modal za unos/izmjenu sekcije otvoren.
        /// </summary>
        private bool _isSectionModalVisible = false;

        /// <summary>
        /// Trenutni režim modala (pregled, izmjena, unos).
        /// </summary>
        private ModalMode _sectionModalMode = ModalMode.VIEW;

        /// <summary>
        /// Odabrana sekcija za prikaz ili izmjenu.
        /// </summary>
        private SectionsGetDTO _selectedSection;

        /// <summary>
        /// Status values available in the filter bar dropdown.
        /// </summary>
        private static readonly IEnumerable<SectionStatusType> _statusValues =
            Enum.GetValues(typeof(SectionStatusType)).Cast<SectionStatusType>();

        // --- PROPERTY-ji ZA UI ---

        /// <summary>
        /// Ukupan broj stranica za paginaciju.
        /// </summary>
        private int TotalPages => (int)Math.Ceiling((double)_sections.Count / _itemsPerPage);

        /// <summary>
        /// Početni indeks prikazanih sekcija na trenutnoj stranici.
        /// </summary>
        private int StartIndex => (_currentPage - 1) * _itemsPerPage;

        /// <summary>
        /// Krajnji indeks prikazanih sekcija na trenutnoj stranici.
        /// </summary>
        private int EndIndex => Math.Min(StartIndex + _itemsPerPage, _sections.Count);

        /// <summary>
        /// Sekcije koje se prikazuju na trenutnoj stranici.
        /// </summary>
        private IEnumerable<SectionsGetDTO> CurrentSections => _sections.Skip(StartIndex).Take(_itemsPerPage);

        // --- LIFECYCLE ---

        /// <summary>
        /// Inicijalizuje komponentu, učitava podatke o grupi i sekcijama.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            _client = HttpClientFactory.CreateClient("AutoDocService");
            await LoadGroupAsync();
            await LoadSectionsAsync();
        }

        // --- METODE ---

        /// <summary>
        /// Učitava podatke o grupi sekcija sa servera.
        /// </summary>
        private async Task LoadGroupAsync()
        {
            try
            {
                _loading = true;
                var response = await _client.GetAsync($"/api/contract-generation/section-groups?id={GroupId}");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<PagedList<SectionGroupGetDTO>>();
                    _group = result?.Items?.FirstOrDefault() ?? new SectionGroupGetDTO();
                }
                else
                {
                    ToastService.ShowError("Problem prilikom učitavanja grupe članova!");
                }
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Greška: {ex.Message}");
            }
            finally
            {
                _loading = false;
            }
        }

        /// <summary>
        /// Učitava sekcije iz odabrane grupe sa servera, uz primijenjene filtere i pretragu.
        /// </summary>
        private async Task LoadSectionsAsync()
        {
            try
            {
                _loading = true;
                var query = new List<string>
                {
                    $"groupId={GroupId}",
                    "isLatestOnly=true"
                };

                if (!string.IsNullOrWhiteSpace(_searchTerm))
                    query.Add($"name={Uri.EscapeDataString(_searchTerm)}");

                if (_statusFilter == SectionStatusType.ACTIVE)
                    query.Add("isActive=true");
                else if (_statusFilter == SectionStatusType.DEACTIVATED)
                    query.Add("isActive=false");

                var url = "/api/contract-generation/sections";
                if (query.Count > 0)
                    url += "?" + string.Join("&", query);

                var response = await _client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<PagedList<SectionsGetDTO>>();
                    _sections = result?.Items?.ToList() ?? new List<SectionsGetDTO>();
                }
                else
                {
                    ToastService.ShowError("Problem prilikom učitavanja članova/sekcija");
                }
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Greška: {ex.Message}");
            }
            finally
            {
                _loading = false;
            }
        }

        /// <summary>
        /// Mijenja trenutnu stranicu u paginaciji.
        /// </summary>
        /// <param name="page">Broj stranice na koju se prelazi.</param>
        private async Task ChangePage(int page)
        {
            if (page < 1 || page > TotalPages || page == _currentPage) return;
            _currentPage = page;
            await InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Mijenja filter statusa i učitava sekcije.
        /// </summary>
        /// <param name="value">Nova vrijednost filtera.</param>
        private async Task OnStatusFilterChanged(SectionStatusType? value)
        {
            _statusFilter = value;
            _currentPage = 1;
            await LoadSectionsAsync();
        }

        /// <summary>
        /// Pokreće pretragu po nazivu sekcije.
        /// </summary>
        private async Task OnSearchClicked()
        {
            _currentPage = 1;
            await LoadSectionsAsync();
        }

        /// <summary>
        /// Briše sve filtere i učitava sve sekcije.
        /// </summary>
        private async Task OnClearFiltersClicked()
        {
            _searchTerm = string.Empty;
            _statusFilter = null;
            _currentPage = 1;
            await LoadSectionsAsync();
        }

        /// <summary>
        /// Otvara modal za unos nove sekcije.
        /// </summary>
        private void ShowSectionModalForInsert()
        {
            _selectedSection = null;
            _sectionModalMode = ModalMode.INSERT;
            _isSectionModalVisible = true;
        }

        /// <summary>
        /// Otvara modal za izmjenu postojeće sekcije.
        /// </summary>
        /// <param name="section">Sekcija za izmjenu.</param>
        private void OpenEditSectionModal(SectionsGetDTO section)
        {
            _selectedSection = section;
            _sectionModalMode = ModalMode.EDIT;
            _isSectionModalVisible = true;
        }

        /// <summary>
        /// Otvara modal za pregled historijskih podataka sekcije.
        /// </summary>
        /// <param name="section">Sekcija za pregled.</param>
        private void OpenHistoricalSectionModal(SectionsGetDTO section)
        {
            _selectedSection = section;
            _sectionModalMode = ModalMode.VIEW;
            _isSectionModalVisible = true;
        }

        /// <summary>
        /// Zatvara modal za sekciju.
        /// </summary>
        private void CloseSectionModal()
        {
            _isSectionModalVisible = false;
        }

        /// <summary>
        /// Handler koji se poziva nakon uspješnog snimanja sekcije u modalnom dijalogu.
        /// </summary>
        private async Task OnSectionModalSave()
        {
            _isSectionModalVisible = false;
            await LoadSectionsAsync();
        }

        /// <summary>
        /// Aktivira ili deaktivira sekciju.
        /// </summary>
        /// <param name="section">Sekcija kojoj se mijenja status.</param>
        /// <param name="isActive">Novi status (true=aktivna, false=neaktivna).</param>
        private async Task ToggleSectionStatus(SectionsGetDTO section, bool isActive)
        {
            try
            {
                _loading = true;
                var statusUpdateDTO = new { IsActive = isActive };
                var response = await _client.PatchAsJsonAsync(
                    $"/api/contract-generation/sections/update-status?sectionId={section.IdSection}&isActiveStatus={isActive}",
                    statusUpdateDTO);

                if (!response.IsSuccessStatusCode)
                {
                    ToastService.ShowError(isActive ? "Problem prilikom aktivacije člana!" : "Problem prilikom deaktivacije člana!");
                }
                else
                {
                    ToastService.ShowSuccess(isActive ? "Član je uspješno aktiviran!" : "Član je uspješno deaktiviran!");
                    await LoadSectionsAsync();
                }
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Neočekivana greška: {ex.Message}");
            }
            finally
            {
                _loading = false;
            }
        }
    }
}
