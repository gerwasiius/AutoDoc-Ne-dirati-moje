using System.ComponentModel.DataAnnotations;

namespace AutoDoc.Shared.Model.DTO.TemplateSectionsRelationDTO
{
    /// <summary>
    /// DTO za ažuriranje relacije između predloška i sekcije.
    /// </summary>
    public class TemplateSectionsRelationUpdateDTO
    {
        /// <summary>
        /// Jedinstveni identifikator za vezu između predloška i sekcije.
        /// </summary>
        [Required]
        public int Id { get; set; }

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
