namespace AutoDocService.DL.DTO.SectionsDTO
{
    /// <summary>
    /// Update DTO for Sections
    /// </summary>
    public class SectionsUpdateDTO : SectionsBaseDTO
    {
        /// <summary>
        /// User who updated group
        /// </summary>
        public string UserUpdate { get; set; }
    }
}
