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
        /// Vraća meta podatke za jedan placeholder prema ID-u.
        /// </summary>
        /// <param name="id">Jedinstveni identifikator placeholdera (npr. "Grupa1.Placeholder1").</param>
        /// <returns>Meta podaci za traženi placeholder ili 404 ako ne postoji.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PlaceholderMetadata), 200)]
        [ProducesResponseType(404)]
        public ActionResult<PlaceholderMetadata> GetById(string id)
        {
            var placeholder = _placeholderService.GetPlaceholderById(id);
            if (placeholder == null)
                return NotFound($"Placeholder sa ID '{id}' nije pronađen.");
            return Ok(placeholder);
        }
    }
}
