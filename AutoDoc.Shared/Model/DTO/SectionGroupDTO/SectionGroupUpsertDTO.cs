using AutoDoc.Shared.Model.DTO.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AutoDoc.Shared.Model.DTO.SectionGroupDTO
{
    public class SectionGroupUpsertDTO
    {
        /// <summary>
        /// Unique identification number of section's group (optional for creating)
        /// </summary>
        public int? ID { get; set; }

        /// <summary>
        /// Title of section's group
        /// </summary>
        [Required(ErrorMessage = "Naziv grupe je obavezno polje za popuniti")]
        [StringLength(100, ErrorMessage = "Naziv grupe može imati najviše 100 karaktera.")]
        public string Name { get; set; }

        /// <summary>
        /// Description of section's group
        /// </summary>
        [StringLength(250, ErrorMessage = "Opis grupe može imati najviše 250 karaktera.")]
        public string? Description { get; set; }

        /// <summary>
        /// Status of section's group
        /// </summary>
        [Required(ErrorMessage = "Status grupe je obavezno polje za odabrati")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public GroupStatusType? Status { get; set; }

        /// <summary>
        /// User who inserted or updated the group
        /// </summary>
        public string User { get; set; }
    }
}
