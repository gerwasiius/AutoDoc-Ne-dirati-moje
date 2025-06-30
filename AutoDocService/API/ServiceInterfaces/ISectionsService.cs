using AutoDoc.Shared.Model.DTO.SectionsDTO;
using AutoDocService.Helpers.Utils;

namespace AutoDocService.API.ServiceInterfaces
{
    /// <summary>
    /// Interface for ISections
    /// </summary>
    public interface ISectionsService
    {
        /// <summary>
        /// Interface of GET method
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
        /// <returns></returns>
        Task<PagedList<SectionsGetDTO>> Get(int? id = null, int? idSection = null, int? groupId = null, string name = null, int? version = null, bool? isActive = null, bool? isLatestOnly = null, int offset = 0, int pageSize = 0);
        /// <summary>
        /// Interface of POST method
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        Task<SectionsGetDTO> Post(SectionsCreateDTO section);
        /// <summary>
        /// Interface of PUT method
        /// </summary>
        /// <param name="id"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        Task<bool> Put(int id,SectionsUpdateDTO section);
        /// <summary>
        /// Interface of ManageSection method
        /// </summary>
        /// <param name="id"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        Task<bool> ManageSection(int id, SectionsUpdateDTO section);

        /// <summary>
        /// Updates the status of a section by Id or all sections by SectionId.
        /// </summary>
        /// <param name="id">The unique identifier of the section to update (optional).</param>
        /// <param name="sectionId">The logical identifier of the sections to update (optional).</param>
        /// <param name="isActiveStatus">The new status to set.</param>
        /// <returns>True if the update was successful, otherwise false.</returns>
        Task<bool> UpdateSectionStatus(int? id, int? sectionId, bool isActiveStatus);
    }
}
