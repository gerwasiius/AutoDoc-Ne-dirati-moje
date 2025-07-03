using AutoDoc.Shared.Model.DTO.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AutoDoc.Shared.Model.DTO.SectionGroupDTO
{
    /// <summary>
    /// DTO za ažuriranje grupe sekcija.
    /// </summary>
    public class SectionGroupUpdateDTO
    {
        /// <summary>
        /// Jedinstveni identifikator grupe.
        /// </summary>
        [Required(ErrorMessage = "ID grupe je obavezan.")]
        public int ID { get; set; }

        /// <summary>
        /// Naziv grupe sekcija.
        /// </summary>
        [Required(ErrorMessage = "Naziv grupe je obavezan.")]
        [StringLength(100, ErrorMessage = "Naziv može imati najviše 100 karaktera.")]
        public string Name { get; set; }

        /// <summary>
        /// Opis grupe sekcija.
        /// </summary>
        [StringLength(250, ErrorMessage = "Opis može imati najviše 250 karaktera.")]
        public string? Description { get; set; }

        /// <summary>
        /// Status of section's group
        /// </summary>
        [Required(ErrorMessage = "Status grupe je obavezno polje za odabrati")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public GroupStatusType? Status { get; set; }
        /// <summary>
        /// Korisnik koji je posljednji put ažurirao grupu.
        /// </summary>
        [Required(ErrorMessage = "Korisnik je obavezan.")]
        public string UserUpdated { get; set; }
    }
}
