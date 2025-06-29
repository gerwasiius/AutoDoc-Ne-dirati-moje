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

        [Inject] private Services.SectionGroupApiService GroupService { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private IToastService ToastService { get; set; } = default!;

        // --- POLJA ---

        private List<SectionGroupGetDTO> _groups = new();
        private string _groupSearchTerm = string.Empty;
        private string _groupStatusFilter = "all";
        private int _currentPage = 1;
        private int _groupsPerPage = 30;
        private int _totalGroupCount = 0;
        private bool _isGroupModalVisible;
        private SectionGroupUpsertDTO _selectedGroup;
        private bool _isLoading;

        /// <summary>
        /// Ukupan broj stranica za paginaciju.
        /// </summary>
        private int TotalPages => (int)Math.Ceiling((double)_totalGroupCount / _groupsPerPage);

        /// <summary>
        /// Početni indeks prikazanih grupa na trenutnoj stranici.
        /// </summary>
        private int StartIndex => _totalGroupCount == 0 ? 0 : (_currentPage - 1) * _groupsPerPage;

        /// <summary>
        /// Krajnji indeks prikazanih grupa na trenutnoj stranici.
        /// </summary>
        private int EndIndex => Math.Min(StartIndex + _groups.Count, _totalGroupCount);

        /// <summary>
        /// Inicijalizuje komponentu i učitava grupe sa servera.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await LoadGroupsAsync();
        }

        /// <summary>
        /// Učitava grupe sa API-ja uz primijenjene filtere i paginaciju.
        /// </summary>
        private async Task LoadGroupsAsync()
        {
            try
            {
                _isLoading = true;
                int offset = (_currentPage - 1) * _groupsPerPage;
                var result = await GroupService.GetGroupsAsync(_groupSearchTerm, _groupStatusFilter, offset, _groupsPerPage);
                _groups = result.Items ?? [];
                _totalGroupCount = result.TotalItems;
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Greška prilikom učitavanja grupa: {ex.Message}");
                _groups = [];
                _totalGroupCount = 0;
            }
            finally
            {
                _isLoading = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        /// <summary>
        /// Mijenja trenutnu stranicu u paginaciji.
        /// </summary>
        private async Task ChangePageAsync(int page)
        {
            if (page < 1 || page > TotalPages || page == _currentPage) return;
            _currentPage = page;
            await LoadGroupsAsync();
        }

        /// <summary>
        /// Preusmjerava korisnika na stranicu članova grupe.
        /// </summary>
        private void NavigateToGroupSections(SectionGroupGetDTO group)
        {
            Navigation.NavigateTo($"/sections/{group.ID}&groupName={Uri.EscapeDataString(group.Name)}");
        }

        /// <summary>
        /// Aktivira pretragu grupa na osnovu naziva.
        /// </summary>
        private async Task SearchGroupsAsync()
        {
            _currentPage = 1;
            await LoadGroupsAsync();
        }

        /// <summary>
        /// Mijenja filter statusa i ponovo učitava grupe.
        /// </summary>
        private async Task OnGroupStatusFilterChangedAsync(ChangeEventArgs e)
        {
            _groupStatusFilter = e.Value?.ToString() ?? "all";
            _currentPage = 1;
            await LoadGroupsAsync();
        }

        /// <summary>
        /// Čisti sve filtere i resetira prikaz grupa.
        /// </summary>
        private async Task ClearGroupFiltersAsync()
        {
            _groupSearchTerm = string.Empty;
            _groupStatusFilter = "all";
            _currentPage = 1;
            await LoadGroupsAsync();
        }

        /// <summary>
        /// Otvara modal za unos nove grupe.
        /// </summary>
        private void OpenNewGroupModal()
        {
            _selectedGroup = null;
            _isGroupModalVisible = true;
        }

        /// <summary>
        /// Otvara modal za uređivanje postojeće grupe.
        /// </summary>
        private void OpenEditGroupModal(SectionGroupGetDTO group)
        {
            _selectedGroup = new SectionGroupUpsertDTO
            {
                ID = group.ID,
                Name = group.Name,
                Description = group.Description,
                Status = group.Status
            };
            _isGroupModalVisible = true;
        }

        /// <summary>
        /// Zatvara modal i osvježava prikaz grupa nakon izmjene.
        /// </summary>
        private async Task OnGroupModalSavedAsync()
        {
            _isGroupModalVisible = false;
            await LoadGroupsAsync();
        }
    }
}