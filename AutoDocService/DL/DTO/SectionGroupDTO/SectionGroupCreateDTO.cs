namespace AutoDocService.DL.DTO.SectionGroupDTO
{
    /// <summary>
    /// Create DTO for SectionGroup
    /// </summary>
    public class SectionGroupCreateDTO : SectionGroupBaseDTO
    {
        /// <summary>
        /// User who inserted group
        /// </summary>
        public string User { get; set; }
    }
}
