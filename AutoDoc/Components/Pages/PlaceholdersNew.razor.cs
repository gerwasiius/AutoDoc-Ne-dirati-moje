using AutoDoc.Shared.Model.Placeholders.PlaceholderMetadata;
using Microsoft.AspNetCore.Components;

namespace AutoDocFront.Components.Pages
{
    public partial class PlaceholdersNew
    {
        private bool _loading = true;
        private string _searchTerm = string.Empty;
        private string SelectedGroup = string.Empty;
        private List<string> _allGroups = new();
        private List<PlaceholderMeta> _filteredPlaceholders = new();
        private Dictionary<string, List<PlaceholderMeta>> _groupedPlaceholders = new();
        private List<string> _groupNames = new();
        private int _currentGroupPage = 1;

        private bool _showDetails = false;
        private PlaceholderMeta _selectedPlaceholder;

        protected override async Task OnInitializedAsync()
        {
            await LoadPlaceholders();
        }

        private async Task LoadPlaceholders()
        {
            _loading = true;
            // Fetch and filter placeholders as needed
            var all = await PlaceholdersService.GetPlaceholdersAsync("");
            _filteredPlaceholders = all
                .Where(p => (string.IsNullOrEmpty(SelectedGroup) || p.Group == SelectedGroup)
                    && (string.IsNullOrEmpty(_searchTerm) || p.Name.Contains(_searchTerm, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            _allGroups = all.Select(p => p.Group).Distinct().OrderBy(g => g).ToList();

            _groupedPlaceholders = _filteredPlaceholders
                .GroupBy(p => p.Group)
                .ToDictionary(g => g.Key, g => g.ToList());

            _groupNames = _groupedPlaceholders.Keys.OrderBy(g => g).ToList();

            // Reset to first group if out of range
            if (_currentGroupPage < 1 || _currentGroupPage > _groupNames.Count)
                _currentGroupPage = 1;

            _loading = false;
        }

        private void PrevGroup()
        {
            if (_currentGroupPage > 1)
                _currentGroupPage--;
        }

        private void NextGroup()
        {
            if (_currentGroupPage < _groupNames.Count)
                _currentGroupPage++;
        }

        private async Task OnSearch()
        {
            _currentGroupPage = 1;
            await LoadPlaceholders();
        }

        private async Task OnClear()
        {
            _searchTerm = string.Empty;
            SelectedGroup = string.Empty;
            _currentGroupPage = 1;
            await LoadPlaceholders();
        }

        private async Task OnGroupFilterChanged(ChangeEventArgs e)
        {
            SelectedGroup = e.Value?.ToString() ?? string.Empty;
            _currentGroupPage = 1;
            await LoadPlaceholders();
        }

        private void ShowDetails(PlaceholderMeta placeholder)
        {
            _selectedPlaceholder = placeholder;
            _showDetails = true;
        }
    }
}