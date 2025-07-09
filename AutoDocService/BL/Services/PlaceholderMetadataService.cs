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
        /// Vraća meta podatke za placeholder prema identifikatoru.
        /// </summary>
        /// <param name="id">Jedinstveni identifikator placeholdera.</param>
        /// <returns>Meta podaci za traženi placeholder ili null ako ne postoji.</returns>
        public PlaceholderMetadata? GetPlaceholderById(string id)
        {
            return PlaceholderMetadataCache.All.FirstOrDefault(p => p.Id == id);
        }
    }
}
