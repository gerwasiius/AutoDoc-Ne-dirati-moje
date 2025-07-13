using AutoDoc.Shared.Model.Enumerations;
using AutoDoc.Shared.Model.Placeholders;
using AutoDocService.API.ServiceInterfaces;
using AutoDocService.BL.Services;
using AutoDocService.DL.FolderParamZaObrisati;
using Microsoft.AspNetCore.Mvc;

namespace AutoDocService.API.Controllers
{
    /// <summary>
    /// API kontroler za rad sa meta podacima placeholdera.
    /// Omogućava dohvat svih placeholder meta podataka ili pojedinačnog placeholdera po ID-u.
    /// </summary>
    [ApiController]
    [Route("api/contract-generation/placeholders")]
    [Produces("application/json")]
    public class PlaceholdersController : ControllerBase
    {
        private readonly IPlaceholderMetadataService _placeholderService;

        /// <summary>
        /// Konstruktor sa injekcijom servisa za placeholder meta podatke.
        /// </summary>
        /// <param name="placeholderService">Servis za meta podatke placeholdera.</param>
        public PlaceholdersController(IPlaceholderMetadataService placeholderService)
        {
            _placeholderService = placeholderService;
        }

        /// <summary>
        /// Vraća sve meta podatke za placeholdere.
        /// </summary>
        /// <returns>Lista svih placeholder meta podataka.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<PlaceholderGroup>), 200)]
        public ActionResult<IReadOnlyList<PlaceholderGroup>> GetAll()
        {
            var result = _placeholderService.GetAllPlaceholders();
            return Ok(result);
        }

        /// <summary>
        /// Vraća filtrirane meta podatke za placeholdere po zadatim parametrima.
        /// </summary>
        /// <param name="group">Naziv grupe.</param>
        /// <param name="name">Naziv placeholdera.</param>
        /// <param name="type">Tip placeholdera.</param>
        /// <param name="description">Opis placeholdera.</param>
        /// <returns>Lista filtriranih placeholder meta podataka.</returns>
        [HttpGet("filter")]
        [ProducesResponseType(typeof(IReadOnlyList<PlaceholderMetadata>), 200)]
        public ActionResult<IReadOnlyList<PlaceholderMetadata>> GetFiltered(
            [FromQuery] string? group = null,
            [FromQuery] string? name = null,
            [FromQuery] PlaceholderType? type = null,
            [FromQuery] string? description = null)
        {
            var result = _placeholderService.GetFilteredPlaceholders(group, name, type, description);
            return Ok(result);
        }
    }
}
