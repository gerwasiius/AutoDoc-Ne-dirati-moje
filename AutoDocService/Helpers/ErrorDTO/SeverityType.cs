using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace AutoDocService.Helpers.ErrorDTO
{
    /// <summary>
    /// Shows if the reason for an unexpected situation is critical or just information.
    /// </summary>
    /// <value>Shows if the reason for an unexpected situation is critical or just information.</value>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SeverityType
    {
        /// <summary>
        /// Enum ERROR for value: ERROR
        /// </summary>
        [EnumMember(Value = "ERROR")]
        ERROR = 1,
        /// <summary>
        /// Enum WARNING for value: WARNING
        /// </summary>
        [EnumMember(Value = "WARNING")]
        WARNING = 2,
        /// <summary>
        /// Enum INFO for value: INFO
        /// </summary>
        [EnumMember(Value = "INFO")]
        INFO = 3
    }
}