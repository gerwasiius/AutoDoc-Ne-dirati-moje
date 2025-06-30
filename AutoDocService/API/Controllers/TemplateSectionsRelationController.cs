using AutoDoc.Shared.Model.DTO.DocumentTemplateDTO;
using AutoDoc.Shared.Model.DTO.TemplateSectionsRelationDTO;
using AutoDocService.API.ServiceInterfaces;
using AutoDocService.BL.Services;
using AutoDocService.DL.Entities;
using AutoDocService.Helpers.ErrorDTO;
using AutoDocService.Helpers.Utils;
using Microsoft.AspNetCore.Mvc;

namespace AutoDocService.API.Controllers
{
    /// <summary>
    /// TemplateSectionsRelation API
    /// </summary>
    [Route("api/contract-generation/template-sections-relations")]
    [ApiController]
    public class TemplateSectionsRelationController : ControllerBase
    {
        private readonly ILogService _logSvc;
        private readonly ITemplateSectionsRelationService _templateSectionsRelationService;

        /// <summary>
        /// Constructor for Template Sections relation
        /// </summary>
        /// <param name="logSvc"></param>
        /// <param name="templateSectionsRelationService"></param>
        public TemplateSectionsRelationController(ILogService logSvc, ITemplateSectionsRelationService templateSectionsRelationService)
        {
            _logSvc = logSvc;
            _templateSectionsRelationService = templateSectionsRelationService;
        }


        /// <summary>
        /// Method created to get DocumentTemplate values based on input parameters
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idTemplate"></param>
        /// <param name="templateVersion"></param>
        /// <param name="idSection"></param>
        /// <param name="sectionVersion"></param>
        /// <param name="offset"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedList<TemplateSectionsRelationGetDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Get([FromQuery] int? id = null,
                                            [FromQuery] int? idTemplate = null,
                                            [FromQuery] int? templateVersion = null,
                                            [FromQuery] int? idSection = null,
                                            [FromQuery] int? sectionVersion = null,
                                            [FromQuery] int offset = 0,
                                            [FromQuery] int pageSize = 0)
        {
            try
            {
                var retVal = await _templateSectionsRelationService.Get(id, idTemplate, templateVersion, idSection, sectionVersion,  offset, pageSize);
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
        [ProducesResponseType(typeof(TemplateSectionsRelationCreateDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Post([FromBody] TemplateSectionsRelationCreateDTO templateSectionsRelationCreate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var retVal = await _templateSectionsRelationService.Post(templateSectionsRelationCreate);
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
        ///  Method created to manage relations between template and sections
        /// </summary>
        /// <param name="documentTemplate"></param>
        /// <returns></returns>
        [HttpPost("manage-relations")]
        [ProducesResponseType(typeof(TemplateSectionsRelationCreateDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ManageRelations([FromBody] DocumentTemplateAndRelatedItemsDTO documentTemplate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (documentTemplate == null)
                return BadRequest("Nema podataka za template.");

            try
            {
                var retVal = await _templateSectionsRelationService.ManageRelationsForDocumentTemplate(documentTemplate);
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
    }

}
