using AutoDocService.DL.Enums;

namespace AutoDocService.DL.DTO.SectionGroupDTO
{
    /// <summary>
    /// Base DTO for SectionGroup
    /// </summary>
    public class SectionGroupBaseDTO
    {
        /// <summary>
        /// Title of section's group
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Description of section's group
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Status of section's group
        /// </summary>
        public GroupStatusType Status { get; set; }
    }
}
