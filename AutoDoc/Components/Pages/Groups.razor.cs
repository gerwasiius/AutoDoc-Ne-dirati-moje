using AutoDoc.Shared.Model.DTO.Enumerations;
using AutoDoc.Shared.Model.DTO.SectionGroupDTO;
using AutoDocFront.Models.Enumerations;
using AutoDocFront.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.FluentUI.AspNetCore.Components;

namespace AutoDocFront.Components.Pages
{
    /// <summary>
    /// Blazor stranica za upravljanje grupama sekcija (Grupe članova).
    /// Omogućava filtriranje, pretragu, paginaciju i CRUD operacije nad grupama.
    /// </summary>
    public partial class Groups
    {
        // --- INJECTION ---

        [Inject] private Services.SectionGroupApiService GroupService { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private IToastService ToastService { get; set; } = default!;
        [Inject] private IDialogService DialogService { get; set; } = default!;

        // --- POLJA ---

        private const int ItemsPerPage = 30;

        private List<SectionGroupGetDTO> _groups = new();
        private string _searchTerm = string.Empty;
        private int _currentPage = 1;
        private int _totalCount = 0;
        private SectionGroupUpsertDTO _selectedGroup;
        private GroupStatusType? _statusFilter;

        private bool _isLoading;

        // --- MODAL STATE ---
        private bool _isGroupModalVisible;
        private ModalMode _groupModalMode = ModalMode.INSERT;
        // -------------------
        /// <summary>
        /// Inicijalizuje komponentu i učitava grupe sa servera.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            try
            {
                await LoadGroupsAsync();
            }
            catch (Exception ex)
            {
                await DialogService.ShowErrorAsync("Došlo je do greške prilikom inicijalizacije stranice.\n\nDetalji: " + ex.Message);
            }
        }

        /// <summary>
        /// Status values available in the filter bar dropdown.
        /// </summary>
        private static readonly IEnumerable<GroupStatusType> _statusValues =
            Enum.GetValues(typeof(GroupStatusType)).Cast<GroupStatusType>();

        /// <summary>
        /// Ukupan broj stranica za paginaciju.
        /// </summary>
        private int TotalPages
        {
            get
            {
                try
                {
                    return PaginationHelper.CalculateTotalPages(_totalCount, ItemsPerPage);
                }
                catch (Exception ex)
                {
                    ToastService.ShowError("Greška u izračunu broja stranica: " + ex.Message);
                    return 1;
                }
            }
        }

        /// <summary>
        /// Početni indeks prikazanih grupa na trenutnoj stranici.
        /// </summary>
        private int StartIndex
        {
            get
            {
                try
                {
                    return PaginationHelper.CalculateStartIndex(_currentPage, ItemsPerPage, _totalCount);
                }
                catch (Exception ex)
                {
                    ToastService.ShowError("Greška u izračunu početnog indeksa: " + ex.Message);
                    return 0;
                }
            }
        }

        /// <summary>
        /// Krajnji indeks prikazanih grupa na trenutnoj stranici.
        /// </summary>
        private int EndIndex
        {
            get
            {
                try
                {
                    return PaginationHelper.CalculateEndIndex(StartIndex, _groups.Count, _totalCount);
                }
                catch (Exception ex)
                {
                    ToastService.ShowError("Greška u izračunu krajnjeg indeksa paginacije: " + ex.Message);
                    return 0;
                }
            }
        }


        /// <summary>
        /// Učitava grupe sa API-ja uz primijenjene filtere i paginaciju.
        /// </summary>
        private async Task LoadGroupsAsync()
        {
            try
            {
                _isLoading = true;
                int offset = (_currentPage - 1) * ItemsPerPage;
                var status = _statusFilter?.ToString() ?? "all";
                var result = await GroupService.GetGroupsAsync(_searchTerm, status, offset, ItemsPerPage);
                _groups = result.Items ?? [];
                _totalCount = result.TotalItems;
            }
            catch (Exception ex)
            {
                // Kritična greška: server error ili nepoznata greška
                await DialogService.ShowErrorAsync("Došlo je do greške prilikom učitavanja grupa. Pokušajte ponovo kasnije.\n\nDetalji: " + ex.Message);
                _groups = [];
                _totalCount = 0;
            }
            finally
            {
                _isLoading = false;
            }
        }

        /// <summary>
        /// Mijenja trenutnu stranicu u paginaciji.
        /// </summary>
        private async Task ChangePageAsync(int page)
        {
            try
            {
                if (page < 1 || page > TotalPages || page == _currentPage) return;
                _currentPage = page;
                await LoadGroupsAsync();
            }
            catch (Exception ex)
            {
                ToastService.ShowError("Greška prilikom promjene stranice: " + ex.Message);
            }
        }

        /// <summary>
        /// Preusmjerava korisnika na stranicu članova grupe.
        /// </summary>
        private void NavigateToGroupSections(SectionGroupGetDTO group)
        {
            try
            {
                if (group == null)
                {
                    ToastService.ShowError("Nije moguće otvoriti grupu jer nije odabrana.");
                    return;
                }
                var uri = QueryHelpers.AddQueryString($"/sections/{group.ID}", "groupName", group.Name ?? string.Empty);
                Navigation.NavigateTo(uri);
            }
            catch (Exception ex)
            {
                ToastService.ShowError("Greška prilikom otvaranja grupe: " + ex.Message);
            }
        }

        /// <summary>
        /// Aktivira pretragu grupa na osnovu naziva.
        /// </summary>
        private async Task SearchGroupsAsync()
        {
            try
            {
                _currentPage = 1;
                await LoadGroupsAsync();
            }
            catch (Exception ex)
            {
                ToastService.ShowError("Greška prilikom pretrage grupa: " + ex.Message);
            }
        }

        /// <summary>
        /// Mijenja filter statusa i ponovo učitava grupe.
        /// </summary>
        private async Task OnGroupStatusFilterChangedAsync(GroupStatusType? value)
        {
            try
            {
                _statusFilter = value;
                _currentPage = 1;
                await LoadGroupsAsync();
            }
            catch (Exception ex)
            {
                ToastService.ShowError("Greška prilikom filtriranja grupa: " + ex.Message);
            }
        }

        /// <summary>
        /// Čisti sve filtere i resetira prikaz grupa.
        /// </summary>
        private async Task ClearGroupFiltersAsync()
        {
            try
            {
                //Ukoliko nema nista za ocistiti, ne cistiti.
                if (_searchTerm == string.Empty && _statusFilter == null)
                    return;

                _searchTerm = string.Empty;
                _statusFilter = null;
                _currentPage = 1;
                await LoadGroupsAsync();
            }
            catch (Exception ex)
            {
                ToastService.ShowError("Greška prilikom čišćenja filtera: " + ex.Message);
            }
        }

        /// <summary>
        /// Otvara modal za pregled
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="group"></param>
        private void OpenGroupModal(ModalMode mode, SectionGroupGetDTO group = null)
        {
            try
            {
                if (mode == ModalMode.INSERT)
                {
                    _selectedGroup = null;
                }
                else if (group != null)
                {
                    _selectedGroup = new SectionGroupUpsertDTO
                    {
                        ID = group.ID,
                        Name = group.Name ?? string.Empty,
                        Description = group.Description ?? string.Empty,
                        Status = group.Status
                    };
                }
                _groupModalMode = mode;
                _isGroupModalVisible = true;
            }
            catch (Exception ex)
            {
                ToastService.ShowError("Greška prilikom otvaranja modala: " + ex.Message);
            }
        }

        /// <summary>
        /// Zatvara modal i osvježava prikaz grupa nakon izmjene.
        /// </summary>
        private async Task OnGroupModalSavedAsync()
        {
            try
            {
                _isGroupModalVisible = false;
                await LoadGroupsAsync();
            }
            catch (Exception ex)
            {
                ToastService.ShowError("Greška prilikom osvježavanja grupa nakon izmjene: " + ex.Message);
            }
        }
    }
}