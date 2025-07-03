using AutoDoc.Shared.Model.DTO.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace AutoDoc.Shared.Model.DTO.SectionGroupDTO
{
    /// <summary>
    /// Create DTO for SectionGroup
    /// </summary>
    public class SectionGroupCreateDTO
    {
        /// <summary>
        /// Naziv grupe sekcija.
        /// </summary>
        [Required(ErrorMessage = "Naziv grupe je obavezan.")]
        [StringLength(100, ErrorMessage = "Naziv može imati najviše 100 karaktera.")]
        public string Name { get; set; }

        /// <summary>
        /// Status of section's group
        /// </summary>
        public GroupStatusType Status { get; set; }
        /// <summary>
        /// Opis grupe sekcija.
        /// </summary>
        [StringLength(250, ErrorMessage = "Opis može imati najviše 250 karaktera.")]
        public string? Description { get; set; }

        /// <summary>
        /// Korisnik koji je kreirao grupu.
        /// </summary>
        [Required(ErrorMessage = "Korisnik je obavezan.")]
        public string UserInserted { get; set; }

    }
}
