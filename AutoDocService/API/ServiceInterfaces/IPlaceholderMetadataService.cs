using AutoDoc.Shared.Model.Placeholders.PlaceholderMetadata;
using AutoDocService.DL.FolderParamZaObrisati;

namespace AutoDocService.API.ServiceInterfaces
{
    /// <summary>
    /// Interfejs za servis meta podataka placeholdera.
    /// </summary>
    public interface IPlaceholderMetadataService
    {
        /// <summary>
        /// Vraća sve meta podatke za placeholdere.
        /// </summary>
        IReadOnlyList<PlaceholderMeta> GetAllPlaceholders();

        /// <summary>
        /// Vraća meta podatke za placeholder prema identifikatoru.
        /// </summary>
        /// <param name="id">Jedinstveni identifikator placeholdera.</param>
        /// <returns>Meta podaci za traženi placeholder ili null ako ne postoji.</returns>
        PlaceholderMeta? GetPlaceholderById(string id);
    }
}
