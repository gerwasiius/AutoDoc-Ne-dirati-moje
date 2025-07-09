using AutoDoc.Shared.Model.DTO.SectionGroupDTO;
using AutoDoc.Shared.Model.DTO.SectionsDTO;
using AutoDocFront.Models.Enumerations;
using AutoDocFront.Services;
using AutoDocFront.Utilities;
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

        [Parameter] public int GroupId { get; set; }
        [Parameter, SupplyParameterFromQuery] public string? GroupName { get; set; }

        // --- INJECTION ---
        [Inject] private SectionsApiService SectionsService { get; set; } = default!;

        [Inject] private SectionGroupApiService GroupService { get; set; } = default!;

        [Inject] private IToastService ToastService { get; set; } = default!;
        [Inject] private IDialogService DialogService { get; set; } = default!;

        // --- POLJA ---

        private SectionGroupGetDTO _group;
        private List<SectionsGetDTO> _sections = new();
        private SectionsGetDTO _selectedSection;

        private const int ItemsPerPage = 20;
        
        private string _searchTerm = string.Empty;
        private SectionStatusType? _statusFilter;
        private int _currentPage = 1;
        private int _totalCount = 0;

        private bool _isLoading = false;

        // --- MODAL STATE ---
        private bool _isSectionModalVisible = false;
        private ModalMode _sectionModalMode = ModalMode.VIEW;

        // -------------------

        /// <summary>
        /// Inicijalizuje komponentu, učitava podatke o grupi i sekcijama.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            try
            {
                await LoadGroupAsync();
                await LoadSectionsAsync();
            }
            catch (Exception ex)
            {
                // Kritična greška: korisnik mora pročitati
                await DialogService.ShowErrorAsync("Došlo je do greške prilikom inicijalizacije stranice.\n\nDetalji: " + ex.Message);
            }
        }

        // --- PAGINATION PROPERTIES ---

        /// <summary>
        /// Status vrijednosti dostupne u filter dropdown-u.
        /// </summary>
        private static readonly IEnumerable<SectionStatusType> _statusValues =
            Enum.GetValues(typeof(SectionStatusType)).Cast<SectionStatusType>();


        /// <summary>
        /// Ukupan broj stranica za paginaciju.
        /// </summary>
        private int TotalPages => PaginationHelper.CalculateTotalPages(_totalCount, ItemsPerPage);

        /// <summary>
        /// Početni indeks prikazanih sekcija na trenutnoj stranici.
        /// </summary>
        private int StartIndex => PaginationHelper.CalculateStartIndex(_currentPage, ItemsPerPage, _totalCount);

        /// <summary>
        /// Krajnji indeks prikazanih sekcija na trenutnoj stranici.
        /// </summary>
        private int EndIndex => PaginationHelper.CalculateEndIndex(StartIndex, _sections.Count, _totalCount);

        // --- API POZIVI ---

        /// <summary>
        /// Učitava podatke o grupi sekcija sa servera.
        /// </summary>
        private async Task LoadGroupAsync()
        {
            try
            {
                _isLoading = true;
                var result = await GroupService.GetGroupByIdAsync(GroupId);
                _group = result ?? new SectionGroupGetDTO();
                if (_group.ID == 0)
                {
                    // Kritična greška: grupa nije pronađena
                    await DialogService.ShowErrorAsync("Grupa nije pronađena ili ne postoji. Provjerite URL ili pokušajte ponovo.");
                }
            }
            catch (Exception ex)
            {
                // Kritična greška: korisnik mora pročitati
                await DialogService.ShowErrorAsync("Došlo je do greške prilikom učitavanja grupe.\n\nDetalji: " + ex.Message);
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
                    GroupId,
                    _searchTerm,
                    _statusFilter,
                    (_currentPage - 1) * ItemsPerPage,
                    ItemsPerPage);

                _sections = result.Items ?? [];
                _totalCount = result.TotalItems;
            }
            catch (Exception ex)
            {
                // Kritična greška: korisnik mora pročitati
                await DialogService.ShowErrorAsync("Došlo je do greške prilikom učitavanja sekcija.\n\nDetalji: " + ex.Message);
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
            try
            {
                _currentPage = page;
                await LoadSectionsAsync();
            }
            catch (Exception ex)
            {
                // Ne-kritična greška: može na toast
                ToastService.ShowError("Greška prilikom promjene stranice: " + ex.Message);
            }
        }

        /// <summary>
        /// Pokreće pretragu po nazivu sekcije.
        /// </summary>
        private async Task SearchSectionsAsync()
        {
            try
            {
                _currentPage = 1;
                await LoadSectionsAsync();
            }
            catch (Exception ex)
            {
                ToastService.ShowError("Greška prilikom pretrage sekcija: " + ex.Message);
            }
        }

        /// <summary>
        /// Mijenja filter statusa i učitava sekcije.
        /// </summary>
        /// <param name="value">Nova vrijednost filtera.</param>
        private async Task OnStatusFilterChangedAsync(SectionStatusType? value)
        {
            try
            {
                _statusFilter = value;
                _currentPage = 1;
                await LoadSectionsAsync();
            }
            catch (Exception ex)
            {
                ToastService.ShowError("Greška prilikom filtriranja sekcija: " + ex.Message);
            }
        }

        /// <summary>
        /// Briše sve filtere i učitava sve sekcije.
        /// </summary>
        private async Task ClearSectionFiltersAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_searchTerm) && _statusFilter == null)
                    return;

                _searchTerm = string.Empty;
                _statusFilter = null;
                _currentPage = 1;
                await LoadSectionsAsync();
            }
            catch (Exception ex)
            {
                ToastService.ShowError("Greška prilikom čišćenja filtera: " + ex.Message);
            }
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
