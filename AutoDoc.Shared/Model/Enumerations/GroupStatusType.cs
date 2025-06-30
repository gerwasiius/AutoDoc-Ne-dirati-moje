using System.Runtime.Serialization;
using System.Text.Json.Serialization;
namespace AutoDoc.Shared.Model.DTO.Enumerations
{
    /// <summary>
    /// Shows all possible statuses
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum GroupStatusType
    {
        /// <summary>
        /// Enum ACTIVE for value: ACTIVE
        /// </summary>
        [EnumMember(Value = "ACTIVE")]
        ACTIVE = 1,
        /// <summary>
        /// Enum DEACTIVATED for value: DEACTIVATED
        /// </summary>
        [EnumMember(Value = "DEACTIVATED")]
        DEACTIVATED = 2,
    }
}
