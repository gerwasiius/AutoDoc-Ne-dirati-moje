using AutoDocService.BL.Services;
using AutoDocService.DL.FolderParamZaObrisati;
using AutoDocService.Helpers.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace AutoDocService.API.Controllers
{
    [ApiController]
    [Route("api/placeholders")]
    public class PlaceholdersController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(PlaceholderMetaCache.All); // Keširana lista!
        }
    }
}
