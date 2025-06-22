using AutoDocFront.Models.DTO.Sections;

namespace AutoDocFront.Models.DTO.Relations
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
