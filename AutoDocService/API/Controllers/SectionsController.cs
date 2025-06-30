using AutoDoc.Shared.Model.DTO.SectionsDTO;
using AutoDocService.API.ServiceInterfaces;
using AutoDocService.BL.Services;
using AutoDocService.DL.FolderParamZaObrisati;
using AutoDocService.Helpers.ErrorDTO;
using AutoDocService.Helpers.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AutoDocService.API.Controllers
{
    /// <summary>
    /// Sections API
    /// </summary>
    [Produces("application/json")]
    [Route("api/contract-generation/sections")]
    public class SectionsController : ControllerBase
    {
        private readonly ILogService _logSvc;
        private readonly ISectionsService sectionsService;

        /// <summary>
        /// Constructor for SectionsController
        /// </summary>
        public SectionsController(ILogService logSvc, ISectionsService sectionsService)
        {
            _logSvc = logSvc;
            this.sectionsService = sectionsService;
        }

        /// <summary>
        /// Method created to get Sections values based on input parameters
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idSection"></param>
        /// <param name="groupId"></param>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <param name="isActive"></param>
        /// <param name="isLatestOnly"></param>
        /// <param name="offset"></param>
        /// <param name="pageSize"></param>
        [HttpGet]
        [ProducesResponseType(typeof(PagedList<SectionsGetDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Get([FromQuery] int? id = null,
                                            [FromQuery] int? idSection = null,
                                            [FromQuery] int? groupId = null,
                                            [FromQuery] string name = "",
                                            [FromQuery] int? version = null,
                                            [FromQuery] bool? isActive = null,
                                            [FromQuery] bool? isLatestOnly = null,
                                            [FromQuery] int offset = 0,
                                            [FromQuery] int pageSize = 0)
        {
            try
            {
                var retVal = await sectionsService.Get(id, idSection, groupId, name, version, isActive, isLatestOnly, offset, pageSize);

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
        /// Method created for insert new section
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(SectionsGetDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Post([FromBody] SectionsCreateDTO section)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                var retVal = await sectionsService.Post(section);

                return CreatedAtAction(nameof(Get), new { id = retVal.ID }, retVal);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
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
        /// <param name="section"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Put(int id, [FromBody] SectionsUpdateDTO section)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var retVal = await sectionsService.Put(id, section);

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

        /// <summary>
        /// Method created to update section, creating new version or editing existing based on parameter
        /// </summary>
        /// <param name="id"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        [HttpPut("{id}/manage-section")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ManageSection(int id, [FromBody] SectionsUpdateDTO section)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var retVal = await sectionsService.ManageSection(id, section);

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

        /// <summary>
        /// Method created to update status of section for complete sectionId
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isActiveStatus"></param>
        /// <returns></returns>
        /// <summary>
        /// Updates the status of a section by Id or all sections by SectionId.
        /// </summary>
        /// <param name="id">The unique identifier of the section to update (optional).</param>
        /// <param name="sectionId">The logical identifier of the sections to update (optional).</param>
        /// <param name="isActiveStatus">The new status to set.</param>
        /// <returns></returns>
        [HttpPatch("update-status")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateSectionStatus([FromQuery] int? id, [FromQuery] int? sectionId, [FromQuery] bool isActiveStatus)
        {
            if (!id.HasValue && !sectionId.HasValue)
                return BadRequest("ID ili ID sekcije mora biti unesen.");

            try
            {
                var retVal = await sectionsService.UpdateSectionStatus(id, sectionId, isActiveStatus);

                if (retVal)
                    return NoContent();
                else
                    return NotFound("Nisu pronadjeni podaci!");
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

        /// Method created to get Sections values based on input parameters
        /// </summary>
        /// <param name="id"></param>
        /// <param name="groupId"></param>
        /// <param name="name"></param>
        /// <param name="status"></param>
        [HttpGet("/parametri")]
        [ProducesResponseType(typeof(SectionsGetDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetParametreObrisati()
        {
            try
            {
                var cc = new Parametri();
                return Ok(cc);

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
