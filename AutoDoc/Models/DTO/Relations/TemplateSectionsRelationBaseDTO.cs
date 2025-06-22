namespace AutoDocFront.Models.DTO.Relations
{
    /// <summary>
    /// Base DTO for TemplateSectionsRelation
    /// </summary>
    public class TemplateSectionsRelationBaseDTO
    {
        /// <summary>
        /// Redoslijed sekcije u predlošku
        /// </summary>
        public int SectionOrder { get; set; }
        /// <summary>
        /// Uslovi za prikazivanje sekcije
        /// </summary>
        public string Condition { get; set; }
        /// <summary>
        /// Akcija koja se primjenjuje na sekciju
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// Označava da li sekcija počinje na novoj stranici
        /// </summary>
        public bool PageBreak { get; set; }
    }
}
