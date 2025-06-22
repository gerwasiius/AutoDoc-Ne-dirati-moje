using AutoDocFront.Models.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace AutoDocFront.Models.DTO.DocumentTemplateDTO
{
    /// <summary>
    /// Base DTO for DocumentTemplate
    /// </summary>
    public class DocumentTemplateBaseDTO : IValidatableObject
    {
        /// <summary>
        /// Ime predloška
        /// </summary>
        [Required(ErrorMessage = "Naziv predloška je obavezan.")]
        [StringLength(100, ErrorMessage = "Naziv može imati najviše 100 karaktera.")]
        public string Name { get; set; }
        /// <summary>
        /// Opis predloška
        /// </summary>
        [Required(ErrorMessage = "Opis predloška je obavezan.")]
        [StringLength(250, ErrorMessage = "Opis grupe može imati najviše 250 karaktera.")]
        public string Description { get; set; }
        /// <summary>
        /// Trenutni status predloška
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Required(ErrorMessage = "Status predloška je obavezan.")]
        public DocumentTemplateStatusType? Status { get; set; }
        /// <summary>
        /// Datum od kada je predložak važeći
        /// </summary>
        [Required(ErrorMessage = "Datum od kada je predložak važeći je obavezan.")]
        public DateTime? ValidFrom { get; set; }
        /// <summary>
        /// Datum do kada je predložak važeći
        /// </summary>
        public DateTime? ValidTo { get; set; }

        /// <summary>
        /// Validacija da ukoliko su proslijedjena oba datuma, polje validTo ne moze biti manje od validFrom
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ValidFrom != null && ValidTo != null && ValidTo < ValidFrom)
            {
                yield return new ValidationResult(
                    "Ovaj datum ne može biti manji od datuma kada počinje važiti.",
                    new[] { nameof(ValidTo) }
                );
            }
        }
    }
}
