using AutoDocService.DL.DTO.TemplateSectionsRelationDTO;

namespace AutoDocService.API.ServiceInterfaces
{
    public interface IDocumentRenderService
    {
        Task<string> RenderTemplateAsync(int idTemplate, int version);
        Task<string> RenderPreviewAsync(List<TemplateSectionRelationWithSectionDTO> relations);
    }
}
