using AutoDoc.Shared.Model.Placeholders.PlaceholderMetadata;
using AutoDocFront.Services;
using AutoDocFront.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AutoDocFront.Components.Pages;

public partial class Placeholders
{
    [Inject] private PlaceholdersApiService PlaceholdersService { get; set; } = default!;
    [Inject] private IJSRuntime JS { get; set; } = default!;

    private List<PlaceholderMeta> _allPlaceholders = new();
    private List<string> _allGroups = new();

    private int CurrentPage { get; set; } = 1;
    private int TotalPages => _allGroups.Count == 0 ? 1 : _allGroups.Count;

    private string? CurrentGroupName
    {
        get => _allGroups.Count >= CurrentPage ? _allGroups[CurrentPage - 1] : null;
        set
        {
            var idx = _allGroups.IndexOf(value ?? "");
            if (idx >= 0)
                CurrentPage = idx + 1;
            else
                CurrentPage = 1;
            // Optionally reset search when group changes:
            // SearchInput = SearchTerm = string.Empty;
        }
    }

    private string SearchInput { get; set; } = string.Empty;
    private string SearchTerm { get; set; } = string.Empty;

    private List<PlaceholderMeta> FilteredPlaceholders =>
        _allPlaceholders
            .Where(p => p.Group == CurrentGroupName &&
                (string.IsNullOrWhiteSpace(SearchTerm)
                 || (p.Name?.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ?? false)
                 || (p.Id?.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ?? false)
                 || (p.Description?.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ?? false)
                )
            )
            .ToList();

    protected override async Task OnInitializedAsync()
    {
        _allPlaceholders = (await PlaceholdersService.GetPlaceholdersAsync(""))?.ToList() ?? [];
        _allGroups = _allPlaceholders.Select(p => p.Group).Distinct().ToList();
        CurrentPage = 1;
    }

    private void OnGroupPageChanged(int page)
    {
        if (page < 1 || page > TotalPages || page == CurrentPage)
            return;
        CurrentPage = page;
    }

    private void OnSearch()
    {
        SearchTerm = SearchInput;
    }

    private void OnClear()
    {
        SearchInput = string.Empty;
        SearchTerm = string.Empty;
    }

    private bool ShowDetails { get; set; }
    private PlaceholderMeta? SelectedPlaceholder { get; set; }

    private void OpenDetails(PlaceholderMeta ph)
    {
        SelectedPlaceholder = ph;
        ShowDetails = true;
    }

    private void CloseDetails()
    {
        ShowDetails = false;
        SelectedPlaceholder = null;
    }

    private async Task CopyPlaceholderValue()
    {
        if (SelectedPlaceholder?.Id is not null)
        {
            await JS.InvokeVoidAsync("navigator.clipboard.writeText", SelectedPlaceholder.Id);
        }
    }

    private string GetTypeClass(string type) => PlaceholderHelpers.GetTypeBadgeClass(type);
}
