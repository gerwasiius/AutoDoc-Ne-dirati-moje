using AutoDoc.Shared.Model.DTO.Enumerations;
using AutoDoc.Shared.Model.DTO.SectionGroupDTO;
using AutoDocService.Helpers.Utils;

namespace AutoDocService.API.ServiceInterfaces
{
    /// <summary>
    /// Interface of section group service
    /// </summary>
    public interface ISectionGroupService
    {
        /// <summary>
        /// Interface of GET method
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="name"></param>
        /// <param name="offset"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PagedList<SectionGroupGetDTO>> Get(int? id = null, GroupStatusType? status = null, string name = null, int offset = 0, int pageSize = 0);
        /// <summary>
        /// Interface of POST method
        /// </summary>
        /// <param name="sectionGroup"></param>
        /// <returns></returns>
        Task<SectionGroupGetDTO> Post(SectionGroupCreateDTO sectionGroup);
        /// <summary>
        /// Interface of PUT method
        /// </summary>
        /// <param name="sectionGroup"></param>
        /// <returns></returns>
        Task<bool> Put(SectionGroupUpdateDTO sectionGroup);
        /// <summary>
        /// Interface of DELETE method
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Delete(int id);
    }
}
