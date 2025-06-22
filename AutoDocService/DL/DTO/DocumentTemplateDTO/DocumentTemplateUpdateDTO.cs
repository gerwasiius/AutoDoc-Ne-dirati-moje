namespace AutoDocService.DL.DTO.DocumentTemplateDTO
{
    /// <summary>
    /// Update DTO for DocumentTemplate
    /// </summary>
    public class DocumentTemplateUpdateDTO : DocumentTemplateBaseDTO
    {
        /// <summary>
        /// Korisnik koji je posljednji put ažurirao predložak
        /// </summary>
        public string UserUpdate { get; set; }
    }
}
