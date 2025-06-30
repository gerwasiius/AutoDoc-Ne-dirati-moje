using Microsoft.AspNetCore.Components;
using AutoDocFront.Utilities;

namespace AutoDocFront.Components.Shared;

/// <summary>
/// Reusable search and filter bar component.
/// </summary>
public partial class FilterBar<TStatusEnum> where TStatusEnum : struct, Enum
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
    public TStatusEnum? StatusFilter { get; set; }

    /// <summary>
    /// Available status filter options where key is the value and value is the display text.
    /// </summary>
    [Parameter]
    public IEnumerable<TStatusEnum> StatusValues { get; set; } = [];

    /// <summary>
    /// Label shown for the option that selects all statuses.
    /// </summary>
    [Parameter]
    public string AllLabel { get; set; } = "Svi";

    /// <summary>
    /// Indicates if the status dropdown should be displayed.
    /// </summary>
    [Parameter]
    public bool ShowStatus { get; set; } = false;

    /// <summary>
    /// Event triggered when the status filter changes.
    /// </summary>
    [Parameter]
    public EventCallback<TStatusEnum?> StatusFilterChanged { get; set; }

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

    /// <summary>
    /// Gets the value used for two-way binding of the status dropdown.
    /// </summary>
    private string SelectedStatusValue => StatusFilter?.ToString() ?? "all";

    /// <summary>
    /// Handles change events from the status dropdown.
    /// </summary>
    private async Task OnStatusChanged(ChangeEventArgs e)
    {
        var value = e.Value?.ToString();
        if (string.IsNullOrEmpty(value) || value == "all")
        {
            StatusFilter = null;
        }
        else if (Enum.TryParse<TStatusEnum>(value, out var parsed))
        {
            StatusFilter = parsed;
        }
        else
        {
            StatusFilter = null;
        }

        await StatusFilterChanged.InvokeAsync(StatusFilter);
    }
}