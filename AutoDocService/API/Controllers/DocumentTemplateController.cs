using AutoDoc.Shared.Model.DTO.DocumentTemplateDTO;
using AutoDoc.Shared.Model.DTO.Enumerations;
using AutoDocService.API.ServiceInterfaces;
using AutoDocService.Helpers.ErrorDTO;
using AutoDocService.Helpers.Utils;
using Microsoft.AspNetCore.Mvc;

namespace AutoDocService.API.Controllers
{
    /// <summary>
    /// DocumentTemplate API
    /// </summary>
    [Produces("application/json")]
    [Route("api/contract-generation/document-templates")]
    public class DocumentTemplateController : ControllerBase
    {
        private readonly ILogService _logSvc;
        private readonly IDocumentTemplateService _documentTemplateService;

        /// <summary>
        /// Constructor for DocumentTemplateController
        /// </summary>
        /// <param name="logSvc"></param>
        /// <param name="documentTemplateService"></param>
        public DocumentTemplateController(ILogService logSvc, IDocumentTemplateService documentTemplateService)
        {
            _logSvc = logSvc;
            _documentTemplateService = documentTemplateService;
        }

        /// <summary>
        /// Method created to get DocumentTemplate values based on input parameters
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idTemplate"></param>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <param name="status"></param>
        /// <param name="isLastValid"></param>
        /// <param name="offset"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedList<DocumentTemplateGetDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Get([FromQuery] int? id = null,
                                            [FromQuery] int? idTemplate = null,
                                            [FromQuery] string name = "",
                                            [FromQuery] int? version = null,
                                            [FromQuery] DocumentTemplateStatusType? status = null,
                                            [FromQuery] bool? isLastValid = null,
                                            [FromQuery] int offset = 0,
                                            [FromQuery] int pageSize = 0)
        {
            try
            {
                var retVal = await _documentTemplateService.Get(id, idTemplate, name, version, status, isLastValid, offset, pageSize);
                return Ok(retVal);
            }
            catch (Exception ex)
            {
                string exceptionAt = Utils.GetMethodAndClassName(System.Reflection.MethodInfo.GetCurrentMethod()).ToString();
                var reasons = new List<ErrorReason>();

                var exceptionMsg = ex.Message.Split(" -ExceptionID:");
                reasons.Add(new ErrorReason("COMMON_INTERNAL_ERROR", SeverityType.ERROR, exceptionMsg[0], exceptionAt));
                return StatusCode(StatusCodes.Status500InternalServerError, new Error(exceptionMsg[1], 500, reasons));
            }
        }

        /// <summary>
        ///  Method created for insert new section
        /// </summary>
        /// <param name="documentTemplate"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(DocumentTemplateGetDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Post([FromBody] DocumentTemplateCreateDTO documentTemplate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var retVal = await _documentTemplateService.Post(documentTemplate);
                return CreatedAtAction(nameof(Get), new { id = retVal.Id }, retVal);
            }
            catch (Exception ex)
            {
                string exceptionAt = Utils.GetMethodAndClassName(System.Reflection.MethodInfo.GetCurrentMethod()).ToString();
                var reasons = new List<ErrorReason>();

                var exceptionMsg = ex.Message.Split(" -ExceptionID:");
                reasons.Add(new ErrorReason("COMMON_INTERNAL_ERROR", SeverityType.ERROR, exceptionMsg[0], exceptionAt));
                return StatusCode(StatusCodes.Status500InternalServerError, new Error(exceptionMsg[1], 500, reasons));
            }
        }

        /// <summary>
        /// Method created to update section
        /// </summary>
        /// <param name="id"></param>
        /// <param name="documentTemplate"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Put(int id, [FromBody] DocumentTemplateUpdateDTO documentTemplate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var retVal = await _documentTemplateService.Put(id, documentTemplate);

                if (retVal)
                    return NoContent();
                else
                    return NotFound($"Entity with {id} ID not found.");
            }
            catch (Exception ex)
            {
                string exceptionAt = Utils.GetMethodAndClassName(System.Reflection.MethodInfo.GetCurrentMethod()).ToString();
                var reasons = new List<ErrorReason>();

                var exceptionMsg = ex.Message.Split(" -ExceptionID:");
                reasons.Add(new ErrorReason("COMMON_INTERNAL_ERROR", SeverityType.ERROR, exceptionMsg[0], exceptionAt));
                return StatusCode(StatusCodes.Status500InternalServerError, new Error(exceptionMsg[1], 500, reasons));
            }
        }

        [HttpGet("template-items")]
        [ProducesResponseType(typeof(PagedList<DocumentTemplateGetDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedList<DocumentTemplateAndRelatedItemsDTO>>> GetTemplateWithRelationItems(
    [FromQuery] int? id = null, [FromQuery] int? idTemplate = null, [FromQuery] string name = null,
    [FromQuery] int? version = null, [FromQuery] DocumentTemplateStatusType? status = null,
    [FromQuery] bool? isLastValid = null, [FromQuery] int offset = 0, [FromQuery] int pageSize = 0)
        {
            try
            {
                var result = await _documentTemplateService.GetTemplateWithRelationItems(id, idTemplate, name, version, status, isLastValid, offset, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                string exceptionAt = Utils.GetMethodAndClassName(System.Reflection.MethodInfo.GetCurrentMethod()).ToString();
                var reasons = new List<ErrorReason>();

                var exceptionMsg = ex.Message.Split(" -ExceptionID:");
                reasons.Add(new ErrorReason("COMMON_INTERNAL_ERROR", SeverityType.ERROR, exceptionMsg[0], exceptionAt));
                return StatusCode(StatusCodes.Status500InternalServerError, new Error(exceptionMsg[1], 500, reasons));
            }
        }
    }
}