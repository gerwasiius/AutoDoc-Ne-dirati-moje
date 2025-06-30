namespace AutoDocService.DL.Entities
{
    /// <summary>
    /// Tabela za relacije izmedju Template i Sections
    /// </summary>
    public partial class TemplateSectionsRelation
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

        /// <summary>
        /// Redoslijed sekcije u predlošku
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Uslovi za prikazivanje sekcije
        /// </summary>
        public string ConditionExpression { get; set; }

        /// <summary>
        /// Akcija koja se primjenjuje na sekciju
        /// </summary>
        public string ActionType { get; set; }

        /// <summary>
        /// Označava da li sekcija počinje na novoj stranici
        /// </summary>
        public bool IsPageBreak { get; set; }

        /// <summary>
        /// Oznacava da li je sekcija pravni clan u templateu
        /// </summary>
        public bool IsArticle { get; set; }

        /// <summary>
        /// Relacija sa DocumentTemplate-om
        /// </summary>
        public virtual DocumentTemplates DocumentTemplate { get; set; }
        /// <summary>
        /// Relacija sa Sections tabelom
        /// </summary>
        public virtual Sections Section { get; set; }
    }
}