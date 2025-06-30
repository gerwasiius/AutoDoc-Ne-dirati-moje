namespace AutoDoc.Shared.Model.DTO.TemplateSectionsRelationDTO
{
    /// <summary>
    /// DTO za prikaz relacije između predloška i sekcije.
    /// </summary>
    public class TemplateSectionsRelationGetDTO
    {
        /// <summary>
        /// Jedinstveni identifikator za vezu između predloška i sekcije.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identifikator predloška kojem sekcija pripada.
        /// </summary>
        public int IdTemplate { get; set; }

        /// <summary>
        /// Verzija predloška kojem sekcija pripada.
        /// </summary>
        public int TemplateVersion { get; set; }

        /// <summary>
        /// Identifikator sekcije u predlošku.
        /// </summary>
        public int IdSection { get; set; }

        /// <summary>
        /// Verzija sekcije u predlošku.
        /// </summary>
        public int SectionVersion { get; set; }

        /// <summary>
        /// Redoslijed prikaza sekcije u predlošku.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Logički izraz za prikaz sekcije (opciono).
        /// </summary>
        public string? ConditionExpression { get; set; }

        /// <summary>
        /// Tip akcije za sekciju (npr. INCLUDE, EXCLUDE).
        /// </summary>
        public string ActionType { get; set; }

        /// <summary>
        /// Označava da li se nakon ove sekcije ubacuje page break.
        /// </summary>
        public bool IsPageBreak { get; set; }

        /// <summary>
        /// Označava da li je sekcija članak (član/članak).
        /// </summary>
        public bool IsArticle { get; set; }
    }

}
