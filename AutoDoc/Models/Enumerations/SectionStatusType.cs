using System.ComponentModel;

namespace AutoDocFront.Models.Enumerations
{
    /// <summary>
    /// Shows all possible statuses
    /// </summary>
    public enum SectionStatusType
    {
        /// <summary>
        /// Enum ACTIVE for value: ACTIVE
        /// </summary>
        [Description("Aktivni")]
        ACTIVE = 1,
        /// <summary>
        /// Enum DEACTIVATED for value: DEACTIVATED
        /// </summary>
        [Description("Neaktivni")]
        DEACTIVATED = 2
    }
}