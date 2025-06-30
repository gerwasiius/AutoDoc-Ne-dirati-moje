using AutoDoc.Shared.Model.DTO.TemplateSectionsRelationDTO;

namespace AutoDoc.Shared.Model.DTO.DocumentTemplateDTO
{
    /// <summary>
    /// Documents DTO that includes Sections
    /// </summary>
    public class DocumentTemplateAndRelatedItemsDTO : DocumentTemplateGetDTO
    {
        public List<TemplateSectionRelationWithSectionDTO> Relations { get; set; }

    }
}
