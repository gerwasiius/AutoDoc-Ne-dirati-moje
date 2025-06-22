namespace AutoDocService.DL.DTO.SectionGroupDTO
{
    /// <summary>
    /// Update DTO for SectionGroup
    /// </summary>
    public class SectionGroupUpdateDTO : SectionGroupBaseDTO
    {
        /// <summary>
        /// Unique identification number of section's group
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// User who updated group
        /// </summary>
        public string User { get; set; }
    }
}
