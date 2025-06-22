namespace AutoDocService.DL.Entities
{
    /// <summary>
    /// Database AutoDocService
    /// Tabela SectionGroup
    /// </summary>
    public class SectionGroup
    {
        /// <summary>
        /// Unique identification number of section's group
        /// </summary>
        public int ID { get; set; }
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
        public string Status { get; set; }
        /// <summary>
        /// Insert Date of section's group
        /// </summary>
        public DateTime DateInserted { get; set; }
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
