using DocumentGenerationLE.Models.Enumerations;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace DocumentGenerationLE.Models.DTO.GroupSection
{
    /// <summary>
    /// Create DTO for SectionGroup
    /// </summary>
    public class SectionGroupCreateDTO
    {
        /// <summary>
        /// Title of section's group
        /// </summary>
        [Required(ErrorMessage = "Naziv grupe je obavezno polje za popuniti")]
        public string Name { get; set; }
        /// <summary>
        /// Description of section's group
        /// </summary>
        [Required(ErrorMessage = "Opis grupe je obavezno polje za popuniti")]

        public string Description { get; set; }
        /// <summary>
        /// Status of section's group
        /// </summary>
        [Required(ErrorMessage = "Status grupe je obavezno polje za odabrati")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public StatusType? Status { get; set; }
        /// <summary>
        /// User who inserted group
        /// </summary>
        public string UserInserted { get; set; } 
    }
}
