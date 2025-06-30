using AutoDoc.Shared.Model.DTO.Enumerations;
using AutoDoc.Shared.Model.DTO.SectionGroupDTO;
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

        // --- POLJA ---

        private const int GroupsPerPage = 30;

        private List<SectionGroupGetDTO> _groups = new();
        private string _groupSearchTerm = string.Empty;
        private GroupStatusType? _groupStatusFilter;
        private int _currentPage = 1;
        private int _totalGroupCount;
        private bool _isGroupModalVisible;
        private SectionGroupUpsertDTO _selectedGroup;
        private bool _isLoading;

        /// <summary>
        /// Status values available in the filter bar dropdown.
        /// </summary>
        private static readonly IEnumerable<GroupStatusType> _statusValues =
            Enum.GetValues(typeof(GroupStatusType)).Cast<GroupStatusType>();

        /// <summary>
        /// Ukupan broj stranica za paginaciju.
        /// </summary>
        private int TotalPages => (int)Math.Ceiling((double)_totalGroupCount / GroupsPerPage);

        /// <summary>
        /// Početni indeks prikazanih grupa na trenutnoj stranici.
        /// </summary>
        private int StartIndex => _totalGroupCount == 0 ? 0 : (_currentPage - 1) * GroupsPerPage;

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
                int offset = (_currentPage - 1) * GroupsPerPage;
                var status = _groupStatusFilter?.ToString() ?? "all";
                var result = await GroupService.GetGroupsAsync(_groupSearchTerm, status, offset, GroupsPerPage);
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
            var uri = QueryHelpers.AddQueryString($"/sections/{group.ID}", "groupName", group.Name);
            Navigation.NavigateTo(uri);
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
        private async Task OnGroupStatusFilterChangedAsync(GroupStatusType? value)
        {
            _groupStatusFilter = value;
            _currentPage = 1;
            await LoadGroupsAsync();
        }

        /// <summary>
        /// Čisti sve filtere i resetira prikaz grupa.
        /// </summary>
        private async Task ClearGroupFiltersAsync()
        {
            //Ukoliko nema nista za ocistiti, ne cistiti.
            if (_groupSearchTerm == string.Empty && _groupStatusFilter == null)
                return; 

            _groupSearchTerm = string.Empty;
            _groupStatusFilter = null;
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