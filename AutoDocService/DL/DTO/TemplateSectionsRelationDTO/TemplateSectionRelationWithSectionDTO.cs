using AutoDocService.DL.DTO.SectionsDTO;

namespace AutoDocService.DL.DTO.TemplateSectionsRelationDTO
{
    /// <summary>
    /// Template Section Relation with Section
    /// </summary>
    public class TemplateSectionRelationWithSectionDTO : TemplateSectionsRelationGetDTO
    {
        /// <summary>
        /// Podaci o sekciji (verzija, naziv, sadržaj, opis, itd.)
        /// </summary>
        public SectionsGetDTO Section { get; set; }
    }
}
