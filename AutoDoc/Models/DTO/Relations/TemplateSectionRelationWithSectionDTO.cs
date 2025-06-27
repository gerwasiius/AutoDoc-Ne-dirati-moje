using AutoDocFront.Models.DTO.Sections;

namespace AutoDocFront.Models.DTO.Relations
{
    /// <summary>
    /// Template Section Relation with Section
    /// </summary>
    public class TemplateSectionRelationWithSectionDTO //: TemplateSectionsRelationGetDTO
    {
        /// <summary>
        /// Podaci o sekciji (verzija, naziv, sadržaj, opis, itd.)
        /// </summary>
        //public SectionsGetDTO Section { get; set; }

        public int RelationId { get; set; }
        public int SectionId { get; set; }
        public int SectionVersion { get; set; }
        public int SectionOrder { get; set; }
        public string Condition { get; set; }
        public string Action { get; set; }
        public bool IsPageBreak { get; set; }
        // Section info
        public int SectionUniqueId { get; set; }
        public string SectionName { get; set; }
        public string SectionDescription { get; set; }
    }
}
