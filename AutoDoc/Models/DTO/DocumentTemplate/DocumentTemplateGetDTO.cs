namespace AutoDocFront.Models.DTO.DocumentTemplateDTO
{
    /// <summary>
    /// Get DTO for DocumentTemplate
    /// </summary>
    public class DocumentTemplateGetDTO : DocumentTemplateBaseDTO
    {
        /// <summary>
        /// Jedinstveni identifikator za predložak dokumenta
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Logički identifikator za predložak
        /// </summary>
        public int IdTemplate { get; set; }
        /// <summary>
        /// Broj verzije predloška
        /// </summary>
        public int Version { get; set; }
        /// <summary>
        /// Datum kada je predložak unesen
        /// </summary>
        public DateTime DateInsert { get; set; }
        /// <summary>
        /// Korisnik koji je unio predložak
        /// </summary>
        public string UserInsert { get; set; }
        /// <summary>
        /// Datum kada je predložak posljednji put ažuriran
        /// </summary>
        public DateTime? DateUpdate { get; set; }
        /// <summary>
        /// Korisnik koji je posljednji put ažurirao predložak
        /// </summary>
        public string UserUpdate { get; set; }
        /// <summary>
        /// Datum kada je predložak verificiran
        /// </summary>
        public DateTime? DateVerified { get; set; }
        /// <summary>
        /// Korisnik koji je verificirao predložak
        /// </summary>
        public string UserVerified { get; set; }
    }
}
