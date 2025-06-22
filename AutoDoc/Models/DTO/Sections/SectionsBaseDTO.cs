using System.ComponentModel.DataAnnotations;

namespace AutoDocFront.Models.DTO.Sections
{
    /// <summary>
    /// Base DTO for Sections
    /// </summary>
    public class SectionsBaseDTO
    {
        /// <summary>
        /// Oznaka kojoj grupi ovaj clan pripada
        /// </summary>
        public int GroupId { get; set; }
        /// <summary>
        /// Ime sekcije
        /// </summary>
        [Required(ErrorMessage = "Naziv člana je obavezno polje za popuniti")]
        [StringLength(100, ErrorMessage = "Korisničko ime ne smije biti duže od 100 karaktera.")]
        public string Name { get; set; }
        /// <summary>
        /// Opis sekcije
        /// </summary>
        [Required(ErrorMessage = "Opis člana je obavezno polje za popuniti")]
        [StringLength(250, ErrorMessage = "Opis ne smije biti duže od 250 karaktera.")]
        public string Description { get; set; }
        /// <summary>
        /// Sadržaj sekcije
        /// </summary>
        [Required(ErrorMessage = "Sadržaj je obavezno polje za popuniti")]
        public string Content { get; set; }
        /// <summary>
        /// Označava da li je sekcija aktivna
        /// </summary>
        public bool IsActive { get; set; }
    }
}