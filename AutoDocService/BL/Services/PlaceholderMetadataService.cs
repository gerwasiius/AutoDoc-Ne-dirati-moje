using AutoDoc.Shared.Model.Enumerations;
using AutoDoc.Shared.Model.Placeholders;
using AutoDocService.API.ServiceInterfaces;
using AutoDocService.DL.FolderParamZaObrisati;

namespace AutoDocService.BL.Services
{
    /// <summary>
    /// Servis za rad sa meta podacima placeholdera.
    /// Omogućava dohvat svih placeholder meta podataka iz keša.
    /// </summary>
    public class PlaceholderMetadataService : IPlaceholderMetadataService
    {
        /// <summary>
        /// Vraća sve meta podatke za placeholdere.
        /// </summary>
        /// <returns>Neizmjenjiva lista meta podataka za sve placeholdere.</returns>
        public IReadOnlyList<PlaceholderGroup> GetAllPlaceholders()
        {
            var all = PlaceholderMetadataCache.All;

            // Grupisanje po Group
            var groupedPlaceholders = all
                .GroupBy(p => p.Group)
                .Select(g => new PlaceholderGroup
                {
                    Group = g.Key,
                    Placeholders = g.ToList()
                })
                .ToList();

            return groupedPlaceholders;
        }

        /// <summary>
        /// Metoda kreirana da vraca podatke o placeholderima ovisno o input parametrima.
        /// </summary>
        /// <param name="group"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public IReadOnlyList<PlaceholderMetadata> GetFilteredPlaceholders(string? group, string? name, PlaceholderType? type, string? description)
        {
            var allPlaceholders = GetAllPlaceholders().SelectMany(g => g.Placeholders);

            if (!string.IsNullOrWhiteSpace(group))
                allPlaceholders = allPlaceholders.Where(p => p.Group.Equals(group, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(name))
                allPlaceholders = allPlaceholders.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            if (type != null)
                    allPlaceholders = allPlaceholders.Where(p => p.Type == type);

            if (!string.IsNullOrWhiteSpace(description))
                allPlaceholders = allPlaceholders.Where(p => p.Description != null && p.Description.Contains(description, StringComparison.OrdinalIgnoreCase));

            return allPlaceholders.ToList();
        }
    }
}
