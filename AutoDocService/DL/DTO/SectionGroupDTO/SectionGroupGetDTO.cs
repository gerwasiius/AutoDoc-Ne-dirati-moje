namespace AutoDocService.DL.DTO.SectionGroupDTO
{
    /// <summary>
    /// Get DTO for SectionGroup
    /// </summary>
    public class SectionGroupGetDTO : SectionGroupBaseDTO
    {
        /// <summary>
        /// Unique identification number of section's group
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Insert Date of section's group
        /// </summary>
        public DateTime? DateInserted { get; set; }
        /// <summary>
        /// User who inserted group
        /// </summary>
        public string UserInserted { get; set; }
        /// <summary>
        /// Date of section's group update
        /// </summary>
        public DateTime? DateUpdated { get; set; }
        /// <summary>
        /// User who updated group
        /// </summary>
        public string UserUpdated { get; set; }
    }
}
