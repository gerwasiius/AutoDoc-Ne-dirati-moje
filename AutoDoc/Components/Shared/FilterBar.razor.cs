using Microsoft.AspNetCore.Components;

namespace AutoDocFront.Components.Shared;

/// <summary>
/// Reusable search and filter bar component.
/// </summary>
public partial class FilterBar
{
    private string _searchTerm = string.Empty;

    /// <summary>
    /// Text entered in the search input.
    /// </summary>
    [Parameter]
    public string SearchTerm
    {
        get => _searchTerm;
        set
        {
            if (_searchTerm != value)
            {
                _searchTerm = value;
                SearchTermChanged.InvokeAsync(value);
            }
        }
    }

    /// <summary>
    /// Event triggered when <see cref="SearchTerm"/> changes.
    /// </summary>
    [Parameter]
    public EventCallback<string> SearchTermChanged { get; set; }

    /// <summary>
    /// Placeholder text for the search input.
    /// </summary>
    [Parameter]
    public string Placeholder { get; set; } = string.Empty;

    /// <summary>
    /// Currently selected status filter value.
    /// </summary>
    [Parameter]
    public string StatusFilter { get; set; } = "all";

    /// <summary>
    /// Available status filter options where key is the value and value is the display text.
    /// </summary>
    [Parameter]
    public IEnumerable<KeyValuePair<string, string>> StatusOptions { get; set; } = [];

    /// <summary>
    /// Indicates if the status dropdown should be displayed.
    /// </summary>
    [Parameter]
    public bool ShowStatus { get; set; } = false;

    /// <summary>
    /// Event triggered when the status filter changes.
    /// </summary>
    [Parameter]
    public EventCallback<string> StatusFilterChanged { get; set; }

    /// <summary>
    /// Event triggered when the search button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnSearch { get; set; }

    /// <summary>
    /// Event triggered when the clear button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnClear { get; set; }

    private async Task OnStatusChanged(ChangeEventArgs e)
    {
        StatusFilter = e.Value?.ToString() ?? "all";
        await StatusFilterChanged.InvokeAsync(StatusFilter);
    }
}
