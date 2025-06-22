namespace AutoDocFront.Models.DTO.Sections
{
    /// <summary>
    /// Create DTO for Sections
    /// </summary>
    public class SectionsCreateDTO : SectionsBaseDTO
    {
        /// <summary>
        /// Logički identifikator za sekciju
        /// </summary>
        public int? IdSection { get; set; }
        /// <summary>
        /// User koji je unio sekciju
        /// </summary>
        public string UserInsert { get; set; }
    }
}
