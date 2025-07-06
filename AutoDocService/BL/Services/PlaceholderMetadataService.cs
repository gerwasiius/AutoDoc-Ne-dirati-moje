using AutoDoc.Shared.Model.Placeholders.PlaceholderMetadata;
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
        public IReadOnlyList<PlaceholderMeta> GetAllPlaceholders()
        {
            return PlaceholderMetadataCache.All;
        }

        /// <summary>
        /// Vraća meta podatke za placeholder prema identifikatoru.
        /// </summary>
        /// <param name="id">Jedinstveni identifikator placeholdera.</param>
        /// <returns>Meta podaci za traženi placeholder ili null ako ne postoji.</returns>
        public PlaceholderMeta? GetPlaceholderById(string id)
        {
            return PlaceholderMetadataCache.All.FirstOrDefault(p => p.Id == id);
        }
    }
}
