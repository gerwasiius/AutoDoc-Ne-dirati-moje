using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
namespace AutoDocService.DL.Enums
{
    /// <summary>
    /// Shows all possible statuses
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
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
