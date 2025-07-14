using AutoDoc.Shared.Model.DTO.TemplateSectionsRelationDTO;
using AutoDocFront.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace AutoDocFront.Components.Modals
{
    public partial class TemplateRelationConditionModal : ModalBase
    {
        [Parameter] public TemplateSectionRelationWithSectionDTO Relation { get; set; }
        [Parameter] public EventCallback OnSaved { get; set; }
        [Inject] private IToastService ToastService { get; set; } = default!;

        private bool _isSaving = false;
        private bool _isExpressionPickerModalOpen = false;
        private List<string> _conditionExpressions = new();

        private void OpenConditionExpressionModal()
        {
            _isExpressionPickerModalOpen = true;
        }

        private void InsertConditionExpression(string expr)
        {
            Relation.ConditionExpression = expr;
            StateHasChanged();
        }

        private async Task HandleValidSubmit()
        {
            if (Relation == null)
            {
                ToastService.ShowError("Greška: Nema podataka o sekciji.");
                return;
            }

            _isSaving = true;
            try
            {
                await OnSaved.InvokeAsync();
                await CloseAsync();
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Greška prilikom snimanja uslova: {ex.Message}");
            }
            finally
            {
                _isSaving = false;
            }
        }

        // Dodaj novi uslov
        private void AddCondition(string expr)
        {
            if (!string.IsNullOrWhiteSpace(expr))
                _conditionExpressions.Add(expr);
        }

        // Ukloni uslov
        private void RemoveCondition(int index)
        {
            if (index >= 0 && index < _conditionExpressions.Count)
                _conditionExpressions.RemoveAt(index);
        }


    }
}
