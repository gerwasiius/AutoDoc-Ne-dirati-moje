using AutoDocFront.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

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
    public string? SearchTerm { get; set; }


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
    [Inject] private IToastService ToastService { get; set; } = default!;

    /// <summary>
    /// Gets the value used for two-way binding of the status dropdown.
    /// </summary>
    private string SelectedStatusValue => StatusFilter?.ToString() ?? "all";

    /// <summary>
    /// Handles On Search term changed
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    private async Task OnSearchTermChanged(ChangeEventArgs e)
    {
        try
        {
            var value = e.Value?.ToString() ?? string.Empty;
            if (SearchTermChanged.HasDelegate)
                await SearchTermChanged.InvokeAsync(value);
        }
        catch (Exception ex)
        {
            ToastService.ShowError($"Greška u pretrazi: {ex.Message}");
        }
    }
    /// <summary>
    /// Handles change events from the status dropdown.
    /// </summary>
    private async Task OnStatusChanged(ChangeEventArgs e)
    {
        try
        {
            TStatusEnum? value = default;
            if (e.Value is not null && Enum.TryParse(typeof(TStatusEnum), e.Value.ToString(), out var parsed))
                value = (TStatusEnum?)parsed;

            if (StatusFilterChanged.HasDelegate)
                await StatusFilterChanged.InvokeAsync(value);
        }
        catch (Exception ex)
        {
            ToastService.ShowError($"Greška pri promjeni statusa: {ex.Message}");
        }
    }
}