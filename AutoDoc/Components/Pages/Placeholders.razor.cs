using AutoDoc.Shared.Model.Placeholders;
using AutoDocFront.Services;
using AutoDocFront.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AutoDocFront.Components.Pages;



/// <summary>
/// Komponenta za prikaz i pretragu placeholdera po grupama.
/// Svaka stranica prikazuje jednu grupu, a navigacija je po grupama.
/// </summary>
public partial class Placeholders
{
    /// <summary>
    /// Servis za dohvat placeholdera.
    /// </summary>
    [Inject] private PlaceholdersApiService _placeholdersService { get; set; } = default!;

    /// <summary>
    /// JS runtime za clipboard funkcionalnost.
    /// </summary>
    [Inject] private IJSRuntime _js { get; set; } = default!;

    /// <summary>
    /// Lista svih grupa placeholdera.
    /// </summary>
    private List<PlaceholderGroup> _groups = new();

    /// <summary>
    /// Lista svih imena grupa.
    /// </summary>
    private List<string> _allGroups = new();

    /// <summary>
    /// Trenutna stranica (indeks grupe).
    /// </summary>
    private int _currentPage = 1;

    /// <summary>
    /// Indikator da li su podaci u fazi učitavanja.
    /// </summary>
    private bool _isLoading = false;
    /// <summary>
    /// Ukupan broj stranica (grupa).
    /// </summary>
    private int TotalPages => _allGroups.Count == 0 ? 1 : _allGroups.Count;

    /// <summary>
    /// Naziv trenutno izabrane grupe.
    /// </summary>
    private string? CurrentGroupName
    {
        get => _allGroups.Count >= _currentPage ? _allGroups[_currentPage - 1] : null;
        set
        {
            var idx = _allGroups.IndexOf(value ?? "");
            _currentPage = idx >= 0 ? idx + 1 : 1;
        }
    }

    /// <summary>
    /// Tekst za pretragu (input).
    /// </summary>
    private string _searchInput = string.Empty;

    /// <summary>
    /// Tekst za pretragu (primijenjen).
    /// </summary>
    private string _searchTerm = string.Empty;

    /// <summary>
    /// Filtrirani placeholderi za trenutnu grupu i pretragu.
    /// </summary>
    private List<PlaceholderMetadata> FilteredPlaceholders
    {
        get
        {
            var group = _groups.FirstOrDefault(g => g.Group == CurrentGroupName);
            if (group == null) return new();

            return group.Placeholders
                .Where(p =>
                    string.IsNullOrWhiteSpace(_searchTerm)
                    || (p.Name?.Contains(_searchTerm, StringComparison.OrdinalIgnoreCase) ?? false)
                    || (p.Id?.Contains(_searchTerm, StringComparison.OrdinalIgnoreCase) ?? false)
                    || (p.Description?.Contains(_searchTerm, StringComparison.OrdinalIgnoreCase) ?? false)
                )
                .ToList();
        }
    }

    /// <summary>
    /// Inicijalizuje komponentu i učitava sve grupe placeholdera.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;
        try
        {
            _groups = await _placeholdersService.GetAllPlaceholderGroupsAsync();
            _allGroups = _groups.Select(g => g.Group).ToList();
            _currentPage = 1;
        }
        finally
        {
            _isLoading = false;
        }
    }

    /// <summary>
    /// Mijenja trenutnu grupu na osnovu broja stranice.
    /// </summary>
    /// <param name="page">Broj stranice (grupe).</param>
    private void OnGroupPageChanged(int page)
    {
        if (page < 1 || page > TotalPages || page == _currentPage)
            return;
        _currentPage = page;
    }

    /// <summary>
    /// Primjenjuje pretragu.
    /// </summary>
    private void OnSearch()
    {
        _searchTerm = _searchInput;
    }

    /// <summary>
    /// Briše pretragu.
    /// </summary>
    private void OnClear()
    {
        _searchInput = string.Empty;
        _searchTerm = string.Empty;
    }

    /// <summary>
    /// Prikazuje detalje za izabrani placeholder.
    /// </summary>
    private bool _showDetails;
    private PlaceholderMetadata? _selectedPlaceholder;

    /// <summary>
    /// Otvara modal sa detaljima placeholdera.
    /// </summary>
    /// <param name="ph">Placeholder za prikaz.</param>
    private void OpenDetails(PlaceholderMetadata ph)
    {
        _selectedPlaceholder = ph;
        _showDetails = true;
    }

    /// <summary>
    /// Zatvara modal sa detaljima.
    /// </summary>
    private void CloseDetails()
    {
        _showDetails = false;
        _selectedPlaceholder = null;
    }

    /// <summary>
    /// Vraća CSS klasu za tip placeholdera.
    /// </summary>
    /// <param name="type">Tip placeholdera.</param>
    /// <returns>CSS klasa.</returns>
    private string GetTypeClass(string type) => PlaceholderHelpers.GetTypeBadgeClass(type);
}