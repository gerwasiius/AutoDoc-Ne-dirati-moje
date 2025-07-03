using AutoDoc.Shared.Model.DTO.TemplateSectionsRelationDTO;
using Microsoft.AspNetCore.Components;

namespace AutoDocFront.Components.Modals
{
    public partial class TemplateRelationConditionModal
    {
        [Parameter] public bool IsOpen { get; set; }
        [Parameter] public EventCallback<bool> IsOpenChanged { get; set; }
        [Parameter] public TemplateSectionRelationWithSectionDTO Relation { get; set; }
        [Parameter] public EventCallback OnSaved { get; set; }

        private async Task HandleValidSubmit()
        {
            await OnSaved.InvokeAsync();
            await IsOpenChanged.InvokeAsync(false);
        }

        private async Task Close()
        {
            await IsOpenChanged.InvokeAsync(false);
        }
    }
}
