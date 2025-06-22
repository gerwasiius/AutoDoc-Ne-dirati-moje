using AutoDocFront.Models.Enumerations;
using System.Text.Json.Serialization;
    

namespace AutoDocFront.Models.DTO.GroupSection
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
        [JsonConverter(typeof(JsonStringEnumConverter))]

        public GroupStatusType Status { get; set; }
    }
}
