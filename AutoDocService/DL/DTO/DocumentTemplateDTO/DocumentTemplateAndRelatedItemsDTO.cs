using AutoDocService.DL.DTO.TemplateSectionsRelationDTO;

namespace AutoDocService.DL.DTO.DocumentTemplateDTO
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

        /// <summary>
        /// Lista relacija predloška i sekcija (svaka relacija sadrži i podatke o sekciji)
        /// </summary>
        //public List<TemplateSectionRelationFlatDTO> Relations { get; set; } = new();
    }
}
