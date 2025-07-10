using AutoDoc.Shared.Model.Placeholders;
using AutoDocFront.Components.Shared;
using AutoDocFront.Models.Enumerations;
using AutoDocFront.Services;
using Microsoft.AspNetCore.Components;

namespace AutoDocFront.Components.Modals
{
    public enum StepPlaceholderModal { GROUP, PLACEHOLDER, VALUE }

    public partial class ConditionExpressionPickerModal : ModalBase
    {
        [Inject] private PlaceholdersApiService PlaceholdersApiService { get; set; } = default!;
        [Parameter] public EventCallback<string> OnInsertCondition { get; set; }

        private string _conditionExpression;
        private StepPlaceholderModal _step;
        private bool _isLoading = false;
        private List<PlaceholderGroup> _groups = new();
        private PlaceholderGroup? _selectedGroup;
        private PlaceholderMetadata? _selectedPlaceholder;
        private string _referenceValue = string.Empty;
        private async Task InsertCondition()
        {
            await OnInsertCondition.InvokeAsync(_conditionExpression);
            await CloseAsync();
        }
        protected override async Task OnInitializedAsync()
        {
            _step = StepPlaceholderModal.GROUP;
            await LoadGroupsAsync();
        }

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
        private void SelectGroup(PlaceholderGroup group)
        {
            _selectedGroup = group;
            _step = StepPlaceholderModal.PLACEHOLDER;
        }

        private void BackToGroups()
        {
            _selectedGroup = null;
            _selectedPlaceholder = null;
            _referenceValue = string.Empty;
            _step = StepPlaceholderModal.GROUP;
        }

        private void SelectPlaceholder(PlaceholderMetadata ph)
        {
            _selectedPlaceholder = ph;
            _referenceValue = string.Empty;
            _step = StepPlaceholderModal.VALUE;
        }

        private void BackToPlaceholders()
        {
            _selectedPlaceholder = null;
            _referenceValue = string.Empty;
            _step = StepPlaceholderModal.PLACEHOLDER;
        }
    }
}