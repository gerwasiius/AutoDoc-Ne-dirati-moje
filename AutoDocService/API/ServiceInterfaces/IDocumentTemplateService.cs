using AutoDoc.Shared.Model.DTO.DocumentTemplateDTO;
using AutoDoc.Shared.Model.DTO.Enumerations;
using AutoDocService.Helpers.Utils;

namespace AutoDocService.API.ServiceInterfaces
{
    /// <summary>
    /// Interface za DocumentTemplate
    /// </summary>
    public interface IDocumentTemplateService
    {
        /// <summary>
        /// GET method to get values
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
        Task<PagedList<DocumentTemplateGetDTO>> Get(int? id = null, int? idTemplate = null, string name = null, int? version = null, DocumentTemplateStatusType? status = null, bool? isLastValid = null, int offset = 0, int pageSize = 0);
        /// <summary>
        /// Method to insert values
        /// </summary>
        /// <param name="documentTemplate"></param>
        /// <returns></returns>
        Task<DocumentTemplateGetDTO> Post(DocumentTemplateCreateDTO documentTemplate);
        /// <summary>
        /// Method to update values
        /// </summary>
        /// <param name="id"></param>
        /// <param name="documentTemplate"></param>
        /// <returns></returns>
        Task<bool> Put(int id, DocumentTemplateUpdateDTO documentTemplate);
        /// <summary>
        /// Method created to get data for template including related items
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
        Task<PagedList<DocumentTemplateAndRelatedItemsDTO>> GetTemplateWithRelationItems(int? id = null, int? idTemplate = null, string name = null, int? version = null, DocumentTemplateStatusType? status = null, bool? isLastValid = null, int offset = 0, int pageSize = 0);

    }
}
