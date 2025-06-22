namespace AutoDocFront.Models.DTO.DocumentTemplateDTO
{
    /// <summary>
    /// Create DTO for DocumentTemplate
    /// </summary>
    public class DocumentTemplateCreateDTO : DocumentTemplateBaseDTO
    {
        /// <summary>
        /// Logički identifikator za predložak
        /// </summary>
        public int? IdTemplate { get; set; }
        /// <summary>
        /// Broj verzije predloška
        /// </summary>
        public int Version { get; set; }
        /// <summary>
        /// Korisnik koji je unio predložak
        /// </summary>
        public string UserInsert { get; set; }
    }
}
