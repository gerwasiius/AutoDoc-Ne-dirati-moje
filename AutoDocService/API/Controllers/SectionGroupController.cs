using AutoDoc.Shared.Model.DTO.Enumerations;
using AutoDoc.Shared.Model.DTO.SectionGroupDTO;
using AutoDocService.API.ServiceInterfaces;
using AutoDocService.Helpers.ErrorDTO;
using AutoDocService.Helpers.Utils;
using Microsoft.AspNetCore.Mvc;

namespace AutoDocService.API.Controllers
{
    /// <summary>
    /// SectionGroup API
    /// </summary>
    [Produces("application/json")]
    [Route("api/contract-generation/section-groups")]
    public class SectionGroupController : ControllerBase
    {
        private readonly ILogService _logSvc;
        private readonly ISectionGroupService sectionGroupService;

        /// <summary>
        /// Constructor for SectionGroupController
        /// </summary>
        public SectionGroupController(ILogService logSvc, ISectionGroupService sectionGroupService)
        {
            _logSvc = logSvc;
            this.sectionGroupService = sectionGroupService;
        }


        /// <summary>
        /// Method created to get SectionGroup values based on input parameters
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="name"></param>
        /// <param name="offset"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(SectionGroupGetDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Get([FromQuery] int? id = null,
                                            [FromQuery] GroupStatusType? status = null,
                                            [FromQuery] string name = "",
                                            [FromQuery] int offset = 0,
                                            [FromQuery] int pageSize = 0)
        {
            try
            {
                var retVal = await sectionGroupService.Get(id, status, name, offset, pageSize);
                if (retVal == null || retVal.Items.Count == 0)
                    return NotFound();

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
        /// Method created for insert new section group 
        /// </summary>
        /// <param name="sectionGroup"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(SectionGroupGetDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Post([FromBody] SectionGroupCreateDTO sectionGroup)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var retVal = await sectionGroupService.Post(sectionGroup);

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
        /// Method created to update section group
        /// </summary>
        /// <param name="sectionGroup"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Put([FromBody] SectionGroupUpdateDTO sectionGroup)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var retVal = await sectionGroupService.Put(sectionGroup);

                if (retVal)
                    return Ok(retVal);
                else
                    return NotFound($"Entity with {sectionGroup.ID} ID not found.");
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
        /// Method create to delete section group.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(int id)
        {
            if (id == 0)
                return BadRequest();

            try
            {
                var retVal = await sectionGroupService.Delete(id);

                if (retVal)
                    return Ok(retVal);
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
    }
}
