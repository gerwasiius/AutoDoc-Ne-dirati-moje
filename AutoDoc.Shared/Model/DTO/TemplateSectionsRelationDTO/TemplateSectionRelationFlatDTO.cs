namespace AutoDoc.Shared.Model.DTO.TemplateSectionsRelationDTO
{
    /// <summary>
    /// Flat DTO koji predstavlja relaciju između predloška i sekcije, uključujući osnovne podatke o sekciji.
    /// </summary>
    public class TemplateSectionRelationFlatDTO
    {
        /// <summary>
        /// Jedinstveni identifikator relacije (veze) između predloška i sekcije.
        /// </summary>
        public int RelationId { get; set; }

        /// <summary>
        /// Logički identifikator sekcije (IdSection).
        /// </summary>
        public int SectionId { get; set; }

        /// <summary>
        /// Verzija sekcije.
        /// </summary>
        public int SectionVersion { get; set; }

        /// <summary>
        /// Redoslijed prikaza sekcije u predlošku.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Logički izraz za prikaz sekcije (opciono).
        /// </summary>
        public string ConditionExpression { get; set; }

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

        // --- Podaci o sekciji ---

        /// <summary>
        /// Unikatni identifikator sekcije u bazi (ID verzije sekcije).
        /// </summary>
        public int SectionDbId { get; set; }

        /// <summary>
        /// Naziv sekcije.
        /// </summary>
        public string SectionName { get; set; }

        /// <summary>
        /// Opis sekcije.
        /// </summary>
        public string SectionDescription { get; set; }
    }
}
