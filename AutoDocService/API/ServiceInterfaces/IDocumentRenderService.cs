using AutoDoc.Shared.Model.DTO.TemplateSectionsRelationDTO;

namespace AutoDocService.API.ServiceInterfaces
{
    public interface IDocumentRenderService
    {
        Task<string> RenderTemplateAsync(int idTemplate, int version);
        Task<string> RenderPreviewAsync(List<TemplateSectionRelationWithSectionDTO> relations);

    }
}
