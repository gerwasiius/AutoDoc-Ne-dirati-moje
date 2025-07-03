using AutoDoc.Shared.Model.DTO.TemplateSectionsRelationDTO;
using AutoDocFront.Components.Shared;
using Microsoft.AspNetCore.Components;

namespace AutoDocFront.Components.Modals
{
    public partial class TemplateRelationConditionModal : ModalBase
    {
        [Parameter] public TemplateSectionRelationWithSectionDTO Relation { get; set; }
        [Parameter] public EventCallback OnSaved { get; set; }

        private async Task HandleValidSubmit()
        {
            await OnSaved.InvokeAsync();
            await CloseAsync();
        }

    }
}
