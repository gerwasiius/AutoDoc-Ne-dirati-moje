namespace AutoDocFront.Models.DTO.Sections
{
    /// <summary>
    /// Get DTO for Sections
    /// </summary>
    public class SectionsGetDTO : SectionsBaseDTO
    {
        /// <summary>
        /// Unificirana vrijednost sekcije ili clana
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Logički identifikator za sekciju
        /// </summary>
        public int? IdSection { get; set; }
        /// <summary>
        /// Broj verzije sekcije
        /// </summary>
        public int Version { get; set; }
        /// <summary>
        /// Datum kada je sekcija unesena
        /// </summary>
        public DateTime? DateInsert { get; set; }

        /// <summary>
        /// Korisnik koji je unio sekciju
        /// </summary>
        public string UserInsert { get; set; }

        /// <summary>
        /// Datum kada je sekcija posljednji put ažurirana
        /// </summary>
        public DateTime? DateUpdate { get; set; }

        /// <summary>
        /// Korisnik koji je posljednji put ažurirao sekciju
        /// </summary>
        public string UserUpdate { get; set; }
    }
}
