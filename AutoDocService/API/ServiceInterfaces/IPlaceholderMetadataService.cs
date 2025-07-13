using AutoDoc.Shared.Model.Enumerations;
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
        /// Vraća meta podatke za placeholder prema input parametrima
        /// </summary>
        /// <param name="group"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="description"></param>
        /// <returns>Meta podaci za traženi placeholder ili null ako ne postoji.</returns>
        IReadOnlyList<PlaceholderMetadata> GetFilteredPlaceholders(string? group, string? name, PlaceholderType? type, string? description);
    }
}
