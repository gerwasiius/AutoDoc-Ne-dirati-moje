using System.ComponentModel.DataAnnotations;

namespace AutoDoc.Shared.Model.DTO.TemplateSectionsRelationDTO
{
    /// <summary>
    /// DTO za kreiranje relacije između predloška i sekcije.
    /// </summary>
    public class TemplateSectionsRelationCreateDTO
    {
        /// <summary>
        /// Identifikator predloška kojem sekcija pripada.
        /// </summary>
        [Required]
        public int IdTemplate { get; set; }

        /// <summary>
        /// Verzija predloška kojem sekcija pripada.
        /// </summary>
        [Required]
        public int TemplateVersion { get; set; }

        /// <summary>
        /// Identifikator sekcije u predlošku.
        /// </summary>
        [Required]
        public int IdSection { get; set; }

        /// <summary>
        /// Verzija sekcije u predlošku.
        /// </summary>
        [Required]
        public int SectionVersion { get; set; }

        /// <summary>
        /// Redoslijed prikaza sekcije u predlošku.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Logički izraz za prikaz sekcije (opciono).
        /// </summary>
        [StringLength(300)]
        public string? ConditionExpression { get; set; }

        /// <summary>
        /// Tip akcije za sekciju (npr. INCLUDE, EXCLUDE).
        /// </summary>
        [Required]
        [StringLength(20)]
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
