namespace AutoDocFront.Models.DTO.Relations
{
    /// <summary>
    /// Get DTO for TemplateSectionsRelation
    /// </summary>
    public class TemplateSectionsRelationGetDTO : TemplateSectionsRelationBaseDTO
    {
        /// <summary>
        /// Jedinstveni identifikator za vezu između predloška i sekcije
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Identifikator predloška kojem sekcija pripada
        /// </summary>
        public int IdTemplate { get; set; }
        /// <summary>
        /// Verzija predloška kojem sekcija pripada
        /// </summary>
        public int TemplateVersion { get; set; }
        /// <summary>
        /// Identifikator sekcije u predlošku
        /// </summary>
        public int IdSection { get; set; }
        /// <summary>
        /// Verzija sekcije u predlošku
        /// </summary>
        public int SectionVersion { get; set; }
    }
}
