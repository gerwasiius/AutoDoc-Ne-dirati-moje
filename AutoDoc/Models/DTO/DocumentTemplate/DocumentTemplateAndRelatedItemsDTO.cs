using AutoDocFront.Models.DTO.DocumentTemplateDTO;
using AutoDocFront.Models.DTO.Relations;

namespace AutoDocFront.Models.DTO.DocumentTemplate
{
    /// <summary>
    /// Documents DTO that includes Sections
    /// </summary>
    public class DocumentTemplateAndRelatedItemsDTO : DocumentTemplateGetDTO
    {
        /// <summary>
        /// Lista relacija predloška i sekcija (svaka relacija sadrži i podatke o sekciji)
        /// </summary>
        public List<TemplateSectionRelationWithSectionDTO> Relations { get; set; }
    }
}
