using AutoDoc.Shared.Model.Enumerations;
using AutoDoc.Shared.Model.Placeholders;
using AutoDocFront.Components.Shared;
using AutoDocFront.Models.Enumerations;
using AutoDocFront.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace AutoDocFront.Components.Modals
{
    /// <summary>
    /// Modal za biranje i generisanje izraza uslova na osnovu placeholdera.
    /// Omogućava pretragu grupa, placeholdera i unos vrijednosti za uslov.
    /// </summary>
    public partial class ConditionExpressionPickerModal : ModalBase
    {
        /// <summary>
        /// Servis za dohvat placeholdera.
        /// </summary>
        [Inject] private PlaceholdersApiService PlaceholdersApiService { get; set; } = default!;
        [Inject] private IToastService ToastService { get; set; } // Dodaj ako već nije

        /// <summary>
        /// Event koji se poziva kada se ubaci uslov.
        /// </summary>
        [Parameter] public EventCallback<string> OnInsertCondition { get; set; }

        private PlaceholderPickerStepEnum _step;
        private bool _isLoading;
        private List<PlaceholderGroup> _groups = new();
        private List<PlaceholderGroup> _searchedGroups = new();
        private List<PlaceholderMetadata> _searchedPlaceholders = new();
        private List<PlaceholderMetadata> _placeholdersForSelectedGroup = new();

        private PlaceholderGroup? _selectedGroup;
        private PlaceholderMetadata? _selectedPlaceholder;
        private string _selectedOperator = "==";
        private string? _inputValue;
        private DateTime? _inputDateValue;

        private string _searchInput = string.Empty;
        private string _searchTerm = string.Empty;
        private string _placeholderSearchInput = string.Empty;
        private string _placeholderSearchTerm = string.Empty;

        /// <summary>
        /// Inicijalizacija modala i učitavanje grupa placeholdera.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            _step = PlaceholderPickerStepEnum.GROUP;
            await LoadGroupsAsync();
        }

        /// <summary>
        /// Učitava sve grupe placeholdera.
        /// </summary>
        private async Task LoadGroupsAsync()
        {
            _isLoading = true;
            try
            {
                _groups = await PlaceholdersApiService.GetAllPlaceholderGroupsAsync();
            }
            finally
            {
                _isLoading = false;
            }
        }

        /// <summary>
        /// Selektuje grupu i učitava njene placeholder-e.
        /// </summary>
        /// <param name="group">Odabrana grupa.</param>
        private async Task SelectGroup(PlaceholderGroup group)
        {
            _selectedGroup = group;
            _step = PlaceholderPickerStepEnum.PLACEHOLDER;
            _isLoading = true;
            try
            {
                _placeholdersForSelectedGroup = await PlaceholdersApiService.GetPlaceholdersAsync(group: group.Group);
                _searchedPlaceholders.Clear();
                _placeholderSearchInput = string.Empty;
                _placeholderSearchTerm = string.Empty;
            }
            finally
            {
                _isLoading = false;
            }
        }

        /// <summary>
        /// Vraća na prikaz grupa.
        /// </summary>
        private void BackToGroups()
        {
            _selectedGroup = null;
            _selectedPlaceholder = null;
            _step = PlaceholderPickerStepEnum.GROUP;
        }

        /// <summary>
        /// Selektuje placeholder i priprema unos vrijednosti.
        /// </summary>
        /// <param name="ph">Odabrani placeholder.</param>
        private void SelectPlaceholder(PlaceholderMetadata ph)
        {
            _selectedPlaceholder = ph;
            _selectedOperator = GetOperatorsForType(ph.Type).FirstOrDefault() ?? "==";
            _inputValue = null;
            _inputDateValue = null;

            _step = PlaceholderPickerStepEnum.VALUE;
        }

        /// <summary>
        /// Vraća na prikaz placeholdera u grupi.
        /// </summary>
        private void BackToPlaceholders()
        {
            _selectedPlaceholder = null;
            _step = PlaceholderPickerStepEnum.PLACEHOLDER;
        }

        /// <summary>
        /// Vraća listu operatora na osnovu tipa placeholdera.
        /// </summary>
        /// <param name="type">Tip placeholdera.</param>
        private static List<string> GetOperatorsForType(PlaceholderType type) => type switch
        {
            PlaceholderType.STRING or PlaceholderType.BOOL => new() { "==", "!=" },
            PlaceholderType.INT or PlaceholderType.DECIMAL => new() { "==", "!=", ">", "<", ">=", "<=" },
            PlaceholderType.DATETIME => new() { "==", "!=", ">", "<", ">=", "<=" },
            PlaceholderType.ENUM => new() { "==", "!=" },
            _ => new() { "==", "!=" }
        };

        /// <summary>
        /// Dodaje uslov i resetuje modal.
        /// </summary>
        private void AddCondition()
        {
            if (_selectedPlaceholder == null)
                return;

            // Provjera za datum
            if (_selectedPlaceholder.Type == PlaceholderType.DATETIME)
            {
                if (_inputDateValue == null)
                {
                    ToastService.ShowError("Datum ne smije biti prazan.");
                    return; // Ne dozvoljava dodavanje ako nije odabran datum
                }

                // Parsiranje datuma u string (npr. "dd-MM-yyyy")
                _inputValue = _inputDateValue.Value.ToString("dd.MM.yyyy");
            }
            else
            {
                // Provjera za ostale tipove
                if (string.IsNullOrWhiteSpace(_inputValue))
                {
                    ToastService.ShowError("Vrijednost ne smije biti prazna.");
                    return;
                }
            }

            string value = _inputValue!;
            if (_selectedPlaceholder.Type == PlaceholderType.STRING || _selectedPlaceholder.IsEnum)
                value = $"\"{value}\"";
            else if (_selectedPlaceholder.Type == PlaceholderType.BOOL)
                value = value.ToLower();

            var condition = $"{_selectedPlaceholder.Id} {_selectedOperator} {value}";
            OnInsertCondition.InvokeAsync(condition);
            IsOpenChanged.InvokeAsync(false);

            // Reset stanja
            _step = PlaceholderPickerStepEnum.GROUP;
            _selectedGroup = null;
            _selectedPlaceholder = null;
            _inputValue = null;
            _inputDateValue = null;
        }

        /// <summary>
        /// Filtrira grupe na osnovu pretrage.
        /// </summary>
        private List<PlaceholderGroup> FilteredGroups =>
            string.IsNullOrWhiteSpace(_searchTerm) ? _groups : _searchedGroups;

        /// <summary>
        /// Filtrira placeholder-e u grupi na osnovu pretrage.
        /// </summary>
        private List<PlaceholderMetadata> FilteredPlaceholders(PlaceholderGroup group, string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return group.Placeholders ?? new List<PlaceholderMetadata>();

            return (group.Placeholders ?? new List<PlaceholderMetadata>())
                .Where(p =>
                    (p.Name?.Contains(term, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (p.Id?.Contains(term, StringComparison.OrdinalIgnoreCase) ?? false)
                )
                .ToList();
        }

        /// <summary>
        /// Filtrirani placeholderi za selektovanu grupu.
        /// </summary>
        private List<PlaceholderMetadata> FilteredPlaceholdersForSelectedGroup =>
            _selectedGroup == null
                ? new List<PlaceholderMetadata>()
                : string.IsNullOrWhiteSpace(_placeholderSearchTerm)
                    ? _placeholdersForSelectedGroup
                    : _searchedPlaceholders.Where(p => p.Group == _selectedGroup.Group).ToList();

        /// <summary>
        /// Pretraga grupa placeholdera.
        /// </summary>
        private async Task OnSearchGroups()
        {
            _searchTerm = _searchInput;
            _isLoading = true;
            try
            {
                _searchedPlaceholders = await PlaceholdersApiService.GetPlaceholdersAsync(
                    group: null,
                    name: _searchTerm,
                    id: _searchTerm
                );

                _searchedGroups = _searchedPlaceholders
                    .GroupBy(p => p.Group)
                    .Select(g => new PlaceholderGroup
                    {
                        Group = g.Key,
                        Placeholders = g.ToList()
                    })
                    .ToList();
            }
            finally
            {
                _isLoading = false;
            }
        }

        /// <summary>
        /// Briše pretragu grupa.
        /// </summary>
        private void ClearSearchGroups()
        {
            _searchInput = string.Empty;
            _searchTerm = string.Empty;
        }

        /// <summary>
        /// Pretraga placeholdera u selektovanoj grupi.
        /// </summary>
        private async Task OnSearchPlaceholders()
        {
            _placeholderSearchTerm = _placeholderSearchInput;
            _isLoading = true;
            try
            {
                if (string.IsNullOrWhiteSpace(_placeholderSearchTerm))
                {
                    _searchedPlaceholders.Clear();
                }
                else
                {
                    _searchedPlaceholders = await PlaceholdersApiService.GetPlaceholdersAsync(
                        group: _selectedGroup?.Group,
                        name: _placeholderSearchTerm,
                        id: _placeholderSearchTerm
                    );
                }
            }
            finally
            {
                _isLoading = false;
            }
        }

        /// <summary>
        /// Briše pretragu placeholdera.
        /// </summary>
        private void ClearPlaceholderSearch()
        {
            _placeholderSearchInput = string.Empty;
            _placeholderSearchTerm = string.Empty;
        }
    }
}