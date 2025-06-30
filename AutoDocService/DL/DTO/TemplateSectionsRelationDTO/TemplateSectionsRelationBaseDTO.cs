namespace AutoDocService.DL.DTO.TemplateSectionsRelationDTO
{
    /// <summary>
    /// Base DTO for TemplateSectionsRelation
    /// </summary>
    public class TemplateSectionsRelationBaseDTO
    {
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
    }
}
