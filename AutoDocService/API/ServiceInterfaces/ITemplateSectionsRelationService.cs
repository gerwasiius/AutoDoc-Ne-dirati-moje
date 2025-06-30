using AutoDoc.Shared.Model.DTO.DocumentTemplateDTO;
using AutoDoc.Shared.Model.DTO.TemplateSectionsRelationDTO;
using AutoDocService.Helpers.Utils;

namespace AutoDocService.API.ServiceInterfaces
{
    /// <summary>
    /// Interface for TemplateSectionRelation
    /// </summary>
    public interface ITemplateSectionsRelationService
    {
        /// <summary>
        /// GET method to get values
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idTemplate"></param>
        /// <param name="templateVersion"></param>
        /// <param name="idSection"></param>
        /// <param name="sectionVersion"></param>
        /// <param name="offset"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PagedList<TemplateSectionsRelationGetDTO>> Get(int? id = null, int? idTemplate = null, int? templateVersion = null, int? idSection = null, int? sectionVersion = null,  int offset = 0, int pageSize = 0);

        /// <summary>
        /// Method to insert values
        /// </summary>
        /// <param name="templateSectionsRelation"></param>
        /// <returns></returns>
        Task<TemplateSectionsRelationGetDTO> Post(TemplateSectionsRelationCreateDTO templateSectionsRelation);

        /// <summary>
        /// Method created to manage relations between template and sections.
        /// </summary>
        /// <param name="documentTemplate"></param>
        /// <returns></returns>
        Task<DocumentTemplateAndRelatedItemsDTO> ManageRelationsForDocumentTemplate(DocumentTemplateAndRelatedItemsDTO documentTemplate);
    }
}