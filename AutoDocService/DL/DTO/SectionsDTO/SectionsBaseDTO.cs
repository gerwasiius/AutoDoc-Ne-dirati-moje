using AutoDocService.DL.Enums;

namespace AutoDocService.DL.DTO.SectionsDTO
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
        public string Name { get; set; }
        /// <summary>
        /// Opis sekcije
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Sadržaj sekcije
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// Označava da li je sekcija aktivna
        /// </summary>
        public bool IsActive { get; set; }
    }
}
