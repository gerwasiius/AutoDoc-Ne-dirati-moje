using AutoDoc.Shared.Model.Placeholders.PlaceholderMetadata;
using AutoDocService.DL.FolderParamZaObrisati;
using AutoDocService.Helpers.Utils;
using System.Reflection;

namespace AutoDocService.BL.Services
{
    /// <summary>
    /// Keširani servis za dohvat meta podataka svih placeholdera.
    /// Omogućava brzo i thread-safe dohvaćanje svih placeholder meta podataka putem refleksije.
    /// </summary>
    public static class PlaceholderMetadataCache
    {
        /// <summary>
        /// Interni thread-safe keš svih placeholder meta podataka.
        /// </summary>
        private static readonly Lazy<IReadOnlyList<PlaceholderMeta>> _allPlaceholders =
            new(LoadAllPlaceholderMetadata, isThreadSafe: true);

        /// <summary>
        /// Vraća sve placeholder meta podatke iz keša.
        /// </summary>
        public static IReadOnlyList<PlaceholderMeta> All => _allPlaceholders.Value;

        /// <summary>
        /// Inicijalizuje i generiše listu svih placeholder meta podataka koristeći refleksiju.
        /// </summary>
        /// <returns>Neizmjenjiva lista meta podataka za sve placeholdere.</returns>
        private static IReadOnlyList<PlaceholderMeta> LoadAllPlaceholderMetadata()
        {
            var result = new List<PlaceholderMeta>(64);
            var nullabilityContext = new NullabilityInfoContext();

            // Dohvati sve javne instance property-je iz Placeholders klase (grupe)
            foreach (var groupProperty in typeof(Placeholders).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
            {
                var groupName = groupProperty.Name;
                var groupType = groupProperty.PropertyType;

                // Dohvati sve javne instance property-je unutar grupe (pojedinačni placeholderi)
                foreach (var placeholderProperty in groupType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    var propertyType = placeholderProperty.PropertyType;
                    bool isEnum = propertyType.IsEnum;
                    var attribute = placeholderProperty.GetCustomAttribute<PlaceholderAttribute>();
                    var nullability = nullabilityContext.Create(placeholderProperty);

                    result.Add(new PlaceholderMeta
                    {
                        Id = $"{groupName}.{placeholderProperty.Name}",
                        Group = groupName,
                        Name = attribute?.Label ?? placeholderProperty.Name,
                        Type = attribute?.DataType ?? (isEnum ? "enum" : propertyType.Name),
                        Description = attribute?.Description ?? placeholderProperty.Name,
                        IsNullable = nullability.ReadState == NullabilityState.Nullable,
                        EnumValues = isEnum ? Enum.GetNames(propertyType).ToList() : null
                    });
                }
            }
            return result.AsReadOnly();
        }
    }
}
