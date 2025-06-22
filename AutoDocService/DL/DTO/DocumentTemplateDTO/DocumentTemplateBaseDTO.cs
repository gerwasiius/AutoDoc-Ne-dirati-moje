using AutoDocService.DL.Enums;

namespace AutoDocService.DL.DTO.DocumentTemplateDTO
{
    /// <summary>
    /// Base DTO for DocumentTemplate
    /// </summary>
    public class DocumentTemplateBaseDTO
    {
        /// <summary>
        /// Ime predloška
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Opis predloška
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Trenutni status predloška
        /// </summary>
        public DocumentTemplateStatusType? Status { get; set; }
        /// <summary>
        /// Datum od kada je predložak važeći
        /// </summary>
        public DateTime? ValidFrom { get; set; }
        /// <summary>
        /// Datum do kada je predložak važeći
        /// </summary>
        public DateTime? ValidTo { get; set; }
    }
}
