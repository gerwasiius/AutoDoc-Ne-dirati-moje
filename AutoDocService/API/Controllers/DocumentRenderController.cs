using AutoDoc.Shared.Model.DTO.TemplateSectionsRelationDTO;
using AutoDocService.API.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace AutoDocService.API.Controllers
{
    [ApiController]
    [Route("api/document-render")]
    public class DocumentRenderController : ControllerBase
    {
        private readonly IDocumentRenderService _renderService;

        public DocumentRenderController(IDocumentRenderService renderService)
        {
            _renderService = renderService;
        }

        [HttpGet("{idTemplate}/render")]
        public async Task<IActionResult> RenderTemplate(int idTemplate, [FromQuery] int version)
        {
            var html = await _renderService.RenderTemplateAsync(idTemplate, version);
            return Ok(new { htmlContent = html });
        }

        [HttpPost("preview")]
        public async Task<IActionResult> RenderPreview([FromBody] List<TemplateSectionRelationWithSectionDTO> relations)
        {
            if (relations == null || !relations.Any())
                return BadRequest("No sections provided.");

            var html = await _renderService.RenderPreviewAsync(relations);
            return Ok(new { htmlContent = html });
        }
    }
}
