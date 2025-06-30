using AutoDocFront.Models.DTO.GroupSection;
using AutoDocFront.Models.DTO.Sections;
using AutoDocFront.Models.Enumerations;
using AutoDocFront.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace AutoDocFront.Components.Pages
{
    /// <summary>
    /// Blazor stranica za upravljanje sekcijama unutar grupe.
    /// Omogućava filtriranje, pretragu, paginaciju i izmjenu statusa sekcija.
    /// </summary>
    public partial class Sections
    {
        // --- PARAMETRI ---

        [Parameter] public string GroupId { get; set; }
        [Parameter, SupplyParameterFromQuery] public string? GroupName { get; set; }

        // --- INJECTION ---
        [Inject] private SectionsApiService SectionsService { get; set; } = default!;

        [Inject] private SectionGroupApiService GroupService { get; set; } = default!;

        [Inject] private IToastService ToastService { get; set; } = default!;

        // --- POLJA ---

        /// <summary>
        /// DTO objekat grupe sekcija.
        /// </summary>
        private SectionGroupGetDTO _group;

        /// <summary>
        /// Lista sekcija u grupi.
        /// </summary>
        private List<SectionsGetDTO> _sections = new();

        /// <summary>
        /// Tekući termin za pretragu po nazivu sekcije.
        /// </summary>
        private string _searchTerm = string.Empty;

        /// <summary>
        /// Filter za status sekcije (sve, aktivne, deaktivirane).
        /// </summary>
        private SectionStatusType? _statusFilter;

        /// <summary>
        /// Trenutna stranica u paginaciji.
        /// </summary>
        private int _currentPage = 1;

        /// <summary>
        /// Broj sekcija po stranici.
        /// </summary>
        private const int ItemsPerPage = 20;

        /// <summary>
        /// Ukupan broj sekcija (za paginaciju).
        /// </summary>
        private int _totalCount = 0;

        /// <summary>
        /// Da li je trenutno učitavanje u toku.
        /// </summary>
        private bool _isLoading = false;

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
        /// Status vrijednosti dostupne u filter dropdown-u.
        /// </summary>
        private static readonly IEnumerable<SectionStatusType> _statusValues =
            Enum.GetValues(typeof(SectionStatusType)).Cast<SectionStatusType>();

        // --- PAGINATION PROPERTIES ---

        /// <summary>
        /// Ukupan broj stranica za paginaciju.
        /// </summary>
        private int TotalPages => (int)Math.Ceiling((double)_totalCount / ItemsPerPage);

        /// <summary>
        /// Početni indeks prikazanih sekcija na trenutnoj stranici.
        /// </summary>
        private int StartIndex => _totalCount == 0 ? 0 : (_currentPage - 1) * ItemsPerPage;

        /// <summary>
        /// Krajnji indeks prikazanih sekcija na trenutnoj stranici.
        /// </summary>
        private int EndIndex => Math.Min(StartIndex + _sections.Count, _totalCount);

        // --- LIFECYCLE ---

        /// <summary>
        /// Inicijalizuje komponentu, učitava podatke o grupi i sekcijama.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await LoadGroupAsync();
            await LoadSectionsAsync();
        }

        // --- API POZIVI ---

        /// <summary>
        /// Učitava podatke o grupi sekcija sa servera.
        /// </summary>
        private async Task LoadGroupAsync()
        {
            try
            {
                _isLoading = true;
                var result = await GroupService.GetGroupsAsync(null, "all", 0, 1);
                _group = result.Items?.FirstOrDefault(g => g.ID.ToString() == GroupId) ?? new SectionGroupGetDTO();
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Greška prilikom učitavanja grupe: {ex.Message}");
            }
            finally
            {
                _isLoading = false;
            }
        }

        /// <summary>
        /// Učitava sekcije iz odabrane grupe sa servera, uz primijenjene filtere i pretragu.
        /// </summary>
        private async Task LoadSectionsAsync()
        {
            try
            {
                _isLoading = true;
                var result = await SectionsService.GetSectionsAsync(
                    int.Parse(GroupId),
                    _searchTerm,
                    _statusFilter,
                    (_currentPage - 1) * ItemsPerPage,
                    ItemsPerPage);

                _sections = result.Items ?? [];
                _totalCount = result.TotalItems;
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Greška prilikom učitavanja sekcija: {ex.Message}");
                _sections = [];
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
        /// <param name="page">Broj stranice na koju se prelazi.</param>
        private async Task ChangePageAsync(int page)
        {
            if (page < 1 || page > TotalPages || page == _currentPage) return;
            _currentPage = page;
            await LoadSectionsAsync();
        }

        /// <summary>
        /// Pokreće pretragu po nazivu sekcije.
        /// </summary>
        private async Task SearchSectionsAsync()
        {
            _currentPage = 1;
            await LoadSectionsAsync();
        }

        /// <summary>
        /// Mijenja filter statusa i učitava sekcije.
        /// </summary>
        /// <param name="value">Nova vrijednost filtera.</param>
        private async Task OnStatusFilterChangedAsync(SectionStatusType? value)
        {
            _statusFilter = value;
            _currentPage = 1;
            await LoadSectionsAsync();
        }

        /// <summary>
        /// Briše sve filtere i učitava sve sekcije.
        /// </summary>
        private async Task ClearSectionFiltersAsync()
        {
            if (string.IsNullOrWhiteSpace(_searchTerm) && _statusFilter == null)
                return;

            _searchTerm = string.Empty;
            _statusFilter = null;
            _currentPage = 1;
            await LoadSectionsAsync();
        }

        // --- MODAL LOGIKA ---

        /// <summary>
        /// Otvara modal za unos nove sekcije.
        /// </summary>
        private void OpenInsertSectionModal()
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
        /// Otvara modal za pregled sekcije.
        /// </summary>
        /// <param name="section">Sekcija za pregled.</param>
        private void OpenViewSectionModal(SectionsGetDTO section)
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
        private async Task OnSectionModalSavedAsync()
        {
            _isSectionModalVisible = false;
            await LoadSectionsAsync();
        }

        // --- STATUS TOGGLE ---

        /// <summary>
        /// Aktivira ili deaktivira sekciju.
        /// </summary>
        /// <param name="section">Sekcija kojoj se mijenja status.</param>
        /// <param name="isActive">Novi status (true=aktivna, false=neaktivna).</param>
        private async Task ToggleSectionStatusAsync(SectionsGetDTO section, bool isActive)
        {
            try
            {
                _isLoading = true;
                var success = await SectionsService.UpdateSectionStatusAsync(section.ID, section.IdSection, isActive);
                if (!success)
                {
                    ToastService.ShowError(isActive ? "Problem prilikom aktivacije sekcije!" : "Problem prilikom deaktivacije sekcije!");
                }
                else
                {
                    ToastService.ShowSuccess(isActive ? "Sekcija je uspješno aktivirana!" : "Sekcija je uspješno deaktivirana!");
                    await LoadSectionsAsync();
                }
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Neočekivana greška: {ex.Message}");
            }
            finally
            {
                _isLoading = false;
            }
        }
    }
}
