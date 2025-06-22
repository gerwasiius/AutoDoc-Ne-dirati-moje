namespace DocumentGenerationLE.Models.DTO.GroupSection
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
        public string UserUpdated { get; set; }
    }
}
