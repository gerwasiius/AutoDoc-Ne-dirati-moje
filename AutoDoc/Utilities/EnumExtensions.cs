using System;
using System.ComponentModel;
using System.Reflection;

namespace AutoDocFront.Utilities
{
    /// <summary>
    /// Helper extension methods for working with enumerations.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Returns the description of the enum value if <see cref="DescriptionAttribute"/> is present; otherwise the enum name.
        /// </summary>
        /// <param name="value">Enumeration value.</param>
        public static string GetDescription(this Enum value)
        {
            ArgumentNullException.ThrowIfNull(value);
            var fi = value.GetType().GetField(value.ToString());
            var attr = fi?.GetCustomAttribute<DescriptionAttribute>();
            return attr?.Description ?? value.ToString();
        }
    }
}