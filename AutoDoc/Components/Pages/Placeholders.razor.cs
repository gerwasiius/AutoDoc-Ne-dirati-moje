using AutoDoc.Shared.Model.Placeholders.PlaceholderMetadata;
using Microsoft.AspNetCore.Components;

namespace AutoDocFront.Components.Pages
{
    public partial class Placeholders
    {
        private List<PlaceholderMeta> _placeholders = new();
        private List<PlaceholderMeta> _filteredPlaceholders = new();
        private List<PlaceholderMeta> _pagedPlaceholders = new();
        private string _searchTerm = "";
        private bool _loading = false;

        private List<string> _allGroups = new();
        private string _selectedGroup = "";
        private string SelectedGroup
        {
            get => _selectedGroup;
            set
            {
                if (_selectedGroup != value)
                {
                    _selectedGroup = value;
                    _currentPage = 1;
                    ApplyFilterAndPaging();
                }
            }
        }

        private bool _showDetails = false;
        private PlaceholderMeta? _selectedPlaceholder;

        private int _currentPage = 1;
        private const int ItemsPerPage = 20;

        private int TotalPages => (_filteredPlaceholders.Count + ItemsPerPage - 1) / ItemsPerPage;

        protected override async Task OnInitializedAsync()
        {
            await LoadPlaceholders();
            _allGroups = _placeholders
                .Select(p => p.Group)
                .Distinct()
                .OrderBy(g => g)
                .ToList();
            // Do NOT call ApplyFilterAndPaging() here, wait for user to click search
            _filteredPlaceholders = _placeholders.ToList();
            _pagedPlaceholders = _filteredPlaceholders.Take(ItemsPerPage).ToList();
        }

        private async Task LoadPlaceholders()
        {
            _loading = true;
            StateHasChanged();

            _placeholders = await PlaceholdersService.GetPlaceholdersAsync("");
            _filteredPlaceholders = _placeholders.ToList();
            _pagedPlaceholders = _filteredPlaceholders.Take(ItemsPerPage).ToList();

            _loading = false;
        }

        // Only called on search button click
        private void OnSearch()
        {
            _currentPage = 1;
            ApplyFilterAndPaging();
        }

        // Only called on clear button click
        private void OnClear()
        {
            _searchTerm = "";
            _selectedGroup = "";
            _currentPage = 1;
            ApplyFilterAndPaging();
        }

        private void ApplyFilterAndPaging()
        {
            IEnumerable<PlaceholderMeta> filtered = _placeholders;

            if (!string.IsNullOrWhiteSpace(_selectedGroup))
                filtered = filtered.Where(p => p.Group == _selectedGroup);

            if (!string.IsNullOrWhiteSpace(_searchTerm))
            {
                var term = _searchTerm.Trim().ToLowerInvariant();
                filtered = filtered.Where(p =>
                    (!string.IsNullOrEmpty(p.Name) && p.Name.ToLowerInvariant().Contains(term)) ||
                    (!string.IsNullOrEmpty(p.Description) && p.Description.ToLowerInvariant().Contains(term)));
            }

            _filteredPlaceholders = filtered.ToList();

            _currentPage = Math.Min(_currentPage, TotalPages == 0 ? 1 : TotalPages);
            var skip = (_currentPage - 1) * ItemsPerPage;
            _pagedPlaceholders = _filteredPlaceholders.Skip(skip).Take(ItemsPerPage).ToList();
        }

        private void OnPageChanged(int page)
        {
            _currentPage = page;
            ApplyFilterAndPaging();
        }

        private void ShowDetails(PlaceholderMeta p)
        {
            _selectedPlaceholder = p;
            _showDetails = true;
        }
        private void HideDetails()
        {
            _showDetails = false;
            _selectedPlaceholder = null;
        }

        private async Task OnGroupChanged(ChangeEventArgs e)
        {
            _selectedGroup = e.Value?.ToString() ?? "";
            _currentPage = 1;
            ApplyFilterAndPaging();
            await InvokeAsync(StateHasChanged);
        }
    }
}
