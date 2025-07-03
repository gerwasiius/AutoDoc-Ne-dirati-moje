using System.ComponentModel.DataAnnotations;

namespace AutoDoc.Shared.Model.DTO.SectionsDTO
{
    /// <summary>
    /// DTO za ažuriranje sekcije (kreiranje nove verzije).
    /// </summary>
    public class SectionsUpdateDTO
    {
        /// <summary>
        /// Identifikator grupe kojoj sekcija pripada.
        /// </summary>
        [Required(ErrorMessage = "Grupa je obavezna.")]
        public int GroupId { get; set; }

        /// <summary>
        /// Naziv sekcije.
        /// </summary>
        [Required(ErrorMessage = "Naziv sekcije je obavezan.")]
        [StringLength(100, ErrorMessage = "Naziv može imati najviše 100 karaktera.")]
        public string Name { get; set; }

        /// <summary>
        /// Opis sekcije.
        /// </summary>
        [StringLength(250, ErrorMessage = "Opis može imati najviše 250 karaktera.")]
        public string? Description { get; set; }

        /// <summary>
        /// Sadržaj sekcije.
        /// </summary>
        [Required(ErrorMessage = "Sadržaj sekcije je obavezan.")]
        public string Content { get; set; }

        /// <summary>
        /// Označava da li je sekcija aktivna.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Korisnik koji je posljednji put ažurirao sekciju.
        /// </summary>
        [Required(ErrorMessage = "Korisnik je obavezan.")]
        public string UserUpdated { get; set; }
    }
}
