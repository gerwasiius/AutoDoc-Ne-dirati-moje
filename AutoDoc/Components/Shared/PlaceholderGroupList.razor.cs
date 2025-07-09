using AutoDoc.Shared.Model.Placeholders;
using AutoDocFront.Services;
using Microsoft.AspNetCore.Components;

namespace AutoDocFront.Components.Shared
{
    public partial class PlaceholderGroupList
    {
        [Inject] private PlaceholdersApiService PlaceholdersApiService { get; set; }
        private List<PlaceholderGroup> _placeholderGroups = new();
        private HashSet<string> _expandedGroups = new();
        private bool _isLoading = true;

        protected override async Task OnInitializedAsync()
        {
            _isLoading = true;
            try
            {
                _placeholderGroups = await PlaceholdersApiService.GetAllPlaceholderGroupsAsync();
            }
            finally
            {
                _isLoading = false;
            }
        }

        private void ToggleGroup(string groupName)
        {
            if (_expandedGroups.Contains(groupName))
                _expandedGroups.Remove(groupName);
            else
                _expandedGroups.Add(groupName);
        }

        private bool IsGroupExpanded(string groupName) => _expandedGroups.Contains(groupName);

        private void ShowPlaceholderDetails(PlaceholderMetadata ph)
        {
            // Ovdje možeš prikazati detalje placeholdera (modal, toast, itd.)
            // Ostavljen prazan za proširenje po potrebi.
        }
    }
}
