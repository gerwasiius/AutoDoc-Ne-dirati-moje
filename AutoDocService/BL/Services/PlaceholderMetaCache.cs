using AutoDocService.DL.FolderParamZaObrisati;
using AutoDocService.Helpers.Utils;
using System.Reflection;

namespace AutoDocService.BL.Services
{
    public class PlaceholderMetaCache
    {
        // Singleton instance - thread-safe init
        private static readonly Lazy<List<PlaceholderMeta>> _cachedPlaceholders =
            new Lazy<List<PlaceholderMeta>>(InitPlaceholderMeta);

        public static List<PlaceholderMeta> All => _cachedPlaceholders.Value;

        private static List<PlaceholderMeta> InitPlaceholderMeta()
        {
            var list = new List<PlaceholderMeta>();
            var placeholdersType = typeof(Placeholders);

            var nullabilityContext = new NullabilityInfoContext();

            foreach (var groupProp in typeof(Placeholders).GetProperties())
            {
                var groupName = groupProp.Name;
                var groupType = groupProp.PropertyType;

                foreach (var prop in groupType.GetProperties())
                {
                    var type = prop.PropertyType;
                    var isEnum = type.IsEnum;
                    var attr = prop.GetCustomAttribute<PlaceholderAttribute>();
                    var nullability = nullabilityContext.Create(prop);

                    var placeholder = new PlaceholderMeta
                    {
                        Id = $"{groupName}.{prop.Name}",
                        Group = groupName,
                        Name = attr?.Label ?? prop.Name,
                        Type = attr?.DataType ?? (isEnum ? "enum" : type.Name),
                        Description = attr?.Description ?? prop.Name,
                        IsNullable = nullability.ReadState == NullabilityState.Nullable,
                        EnumValues = isEnum ? Enum.GetNames(type).ToList() : null
                    };

                    list.Add(placeholder);
                }
            }
            return list;
        }
    }
}
