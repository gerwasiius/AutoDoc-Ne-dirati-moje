using AutoDoc.Shared.Model.DTO.Enumerations;
using AutoDoc.Shared.Model.DTO.SectionGroupDTO;
using AutoDocFront.Models.Enumerations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.FluentUI.AspNetCore.Components;
using System.Net.Http.Json;

namespace AutoDocFront.Components.Pages
{
    /// <summary>
    /// Stranica za odabir grupe sekcija. Prikazuje samo aktivne grupe sa pretragom i paginacijom.
    /// </summary>
    public partial class GroupSelection
    {
        [Inject] private NavigationManager Navigation { get; set; }
        [Inject] private IToastService ToastService { get; set; }
        [Inject] private IDialogService DialogService { get; set; }
        [Inject] private Services.SectionGroupApiService GroupService { get; set; } = default!;

        private const int ItemsPerPage = 30;

        private List<SectionGroupGetDTO> _groups = new();
        private string _searchTerm = string.Empty;
        private int _currentPage = 1;
        private int _totalCount = 0;
        private bool _isLoading = false;

        /// <summary>
        /// Inicijalizuje komponentu i učitava aktivne grupe sa servera.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            try
            {
                _isLoading = true;
                await LoadGroupsAsync();
            }
            catch (Exception ex)
            {
                ToastService.ShowError("Greška prilikom učitavanja grupa: " + ex.Message);
            }
            finally
            {
                _isLoading = false;
            }
        }

        /// <summary>
        /// Ukupan broj stranica za paginaciju.
        /// </summary>
        private int TotalPages => (int)Math.Ceiling((double)_totalCount / ItemsPerPage);

        /// <summary>
        /// Početni indeks prikazanih grupa na trenutnoj stranici.
        /// </summary>
        private int StartIndex => _totalCount == 0 ? 0 : (_currentPage - 1) * ItemsPerPage;

        /// <summary>
        /// Krajnji indeks prikazanih grupa na trenutnoj stranici.
        /// </summary>
        private int EndIndex => Math.Min(StartIndex + _groups.Count, _totalCount);

        /// <summary>
        /// Učitava aktivne grupe sa API-ja uz pretragu i paginaciju.
        /// </summary>
        private async Task LoadGroupsAsync()
        {
            try
            {
                _isLoading = true;
                int offset = (_currentPage - 1) * ItemsPerPage;
                var status = GroupStatusType.ACTIVE.ToString();
                var result = await GroupService.GetGroupsAsync(_searchTerm, status, offset, ItemsPerPage);
                _groups = result.Items ?? [];
                _totalCount = result.TotalItems;
            }
            catch (Exception ex)
            {
                // Kritična greška: korisnik mora videti i potvrditi
                await DialogService.ShowErrorAsync(
                    "Došlo je do greške prilikom učitavanja grupa. Pokušajte ponovo kasnije.\n\nDetalji: " + ex.Message
                );
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
        /// Pokreće pretragu po nazivu grupe.
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
        /// Briše filter pretrage i učitava sve aktivne grupe.
        /// </summary>
        private async Task ClearGroupFiltersAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_searchTerm))
                    return;
                _searchTerm = string.Empty;
                await LoadGroupsAsync();
            }
            catch (Exception ex)
            {
                ToastService.ShowError("Greška prilikom čišćenja filtera: " + ex.Message);
            }
        }


        /// <summary>
        /// Preusmjerava korisnika na sekcije odabrane grupe.
        /// </summary>
        private void HandleGroupSelect(int groupId, string groupName)
        {
            try
            {
                if (groupId <= 0 || string.IsNullOrWhiteSpace(groupName))
                {
                    ToastService.ShowError("Neispravan odabir grupe. Pokušajte ponovo.");
                    return;
                }

                var uri = QueryHelpers.AddQueryString($"/sections/{groupId}", "groupName", groupName);
                Navigation.NavigateTo(uri);
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Greška prilikom preusmjeravanja na sekcije grupe: {ex.Message}");
            }
        }
    }
}
