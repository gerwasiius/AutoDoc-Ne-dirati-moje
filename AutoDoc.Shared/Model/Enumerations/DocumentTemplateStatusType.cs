using System.Runtime.Serialization;
using System.Text.Json.Serialization;
namespace AutoDoc.Shared.Model.DTO.Enumerations
{
    /// <summary>
    /// Shows all possible statuses
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DocumentTemplateStatusType
    {
        /// <summary>
        /// Enum IN PROGRESS for value: IN_PROGRESS
        /// </summary>
        [EnumMember(Value = "IN_PROGRESS")]
        IN_PROGRESS = 1,

        /// <summary>
        /// Enum IN PENDING for value: PENDING
        /// </summary>
        [EnumMember(Value = "PENDING")]
        PENDING = 2,

        /// <summary>
        /// Enum ACTIVE for value: ACTIVE
        /// </summary>
        [EnumMember(Value = "ACTIVE")]
        ACTIVE = 3,
        /// <summary>
        /// Enum DEACTIVATED for value: DEACTIVATED
        /// </summary>
        [EnumMember(Value = "DEACTIVATED")]
        DEACTIVATED = 4
    }
}
