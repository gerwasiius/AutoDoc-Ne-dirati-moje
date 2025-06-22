namespace AutoDocService.DL.DTO.TemplateSectionsRelationDTO
{
    /// <summary>
    /// Create DTO for TemplateSectionsRelation
    /// </summary>
    public class TemplateSectionsRelationCreateDTO : TemplateSectionsRelationBaseDTO
    {
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
