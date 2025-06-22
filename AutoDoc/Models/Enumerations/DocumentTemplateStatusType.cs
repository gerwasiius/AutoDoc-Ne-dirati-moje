using System.ComponentModel;

namespace AutoDocFront.Models.Enumerations
{
    /// <summary>
    /// Shows all possible statuses
    /// </summary>
    public enum DocumentTemplateStatusType
    {
        /// <summary>
        /// Enum IN PROGRESS for value: IN_PROGRESS
        /// </summary>
        [Description("IN_PROGRESS")]
        IN_PROGRESS = 1,

        /// <summary>
        /// Enum IN PENDING for value: PENDING
        /// </summary>
        [Description("PENDING")]
        PENDING = 2,

        /// <summary>
        /// Enum ACTIVE for value: ACTIVE
        /// </summary>
        [Description("ACTIVE")]
        ACTIVE = 3,
        /// <summary>
        /// Enum DEACTIVATED for value: DEACTIVATED
        /// </summary>
        [Description("DEACTIVATED")]
        DEACTIVATED = 4
    }
}
