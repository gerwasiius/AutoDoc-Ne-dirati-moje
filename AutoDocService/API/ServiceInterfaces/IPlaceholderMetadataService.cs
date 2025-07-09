using AutoDoc.Shared.Model.Placeholders;
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
        IReadOnlyList<PlaceholderGroup> GetAllPlaceholders();

        /// <summary>
        /// Vraća meta podatke za placeholder prema identifikatoru.
        /// </summary>
        /// <param name="id">Jedinstveni identifikator placeholdera.</param>
        /// <returns>Meta podaci za traženi placeholder ili null ako ne postoji.</returns>
        PlaceholderMetadata? GetPlaceholderById(string id);
    }
}
