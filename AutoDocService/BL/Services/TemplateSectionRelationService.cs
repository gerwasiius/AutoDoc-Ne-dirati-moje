using AutoMapper;
using AutoDocService.API.Controllers;
using AutoDocService.API.ServiceInterfaces;
using AutoDocService.DL.DBContext;
using AutoDocService.DL.DTO.DocumentTemplateDTO;
using AutoDocService.DL.DTO.TemplateSectionsRelationDTO;
using AutoDocService.DL.Entities;
using AutoDocService.DL.Enums;
using AutoDocService.Helpers.Utils;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace AutoDocService.BL.Services
{
    /// <summary>
    /// Document template Service
    /// </summary>
    public class TemplateSectionsRelationService : ITemplateSectionsRelationService
    {
        private readonly ContractGenerationContext _context;
        private readonly IMapper _mapper;
        private readonly ILogService _logSvc;

        /// <summary>
        /// Constructor for document template service
        /// </summary>
        /// <param name="logSvc"></param>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        public TemplateSectionsRelationService(ILogService logSvc, ContractGenerationContext context, IMapper mapper)
        {
            _logSvc = logSvc;
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Method created to get DocumentTemplate values based on input parameters
        /// If none parameter is provided, it will return all data.
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
        /// <exception cref="Exception"></exception>
        public async Task<PagedList<TemplateSectionsRelationGetDTO>> Get(int? id = null, int? idTemplate = null, int? templateVersion = null, int? idSection = null, int? sectionVersion = null, int offset = 0, int pageSize = 0)
        {
            try
            {

                DateTime dateTime = DateTime.Now.Date;
                var query = _context.TemplateSectionsRelations.AsNoTracking().AsQueryable();

                if (id != null)
                    query = query.Where(e => e.Id == id);

                if (idTemplate != null)
                    query = query.Where(e => e.IdTemplate == idTemplate);

                if (templateVersion != null)
                    query = query.Where(e => e.TemplateVersion == templateVersion);

                if (idSection != null)
                    query = query.Where(e => e.IdSection == idSection);

                if (sectionVersion != null)
                    query = query.Where(e => e.SectionVersion == sectionVersion);

                var totalItems = await query.CountAsync().ConfigureAwait(false);

                if (offset > 0)
                    query = query.Skip(offset);

                if (pageSize > 0)
                    query = query.Take(pageSize);

                var result = await query.ToListAsync().ConfigureAwait(false);

                var templateSectionRelation = _mapper.Map<List<TemplateSectionsRelationGetDTO>>(result);
                var retVal = new PagedList<TemplateSectionsRelationGetDTO>(templateSectionRelation, pageSize, offset, totalItems);
                return retVal;
            }
            catch (Exception ex)
            {
                string exceptionAt = Utils.GetMethodAndClassName1(System.Reflection.MethodInfo.GetCurrentMethod()).ToString();
                string allParams = string.Join(",",
                                                id == null ? "idEmpty" : id.ToString(),
                                                idTemplate == null ? "idTemplateEmpty" : idTemplate.ToString(),
                                                templateVersion == null ? "templateVersion" : templateVersion.ToString(),
                                                idSection == null ? "idSectionEmpty" : idSection.ToString(),
                                                sectionVersion == null ? "sectionVersion" : sectionVersion.ToString());
                var idExcep = await _logSvc.LogException(exceptionAt, ex, allParams).ConfigureAwait(false);
                throw new Exception($"{ex.Message} -ExceptionID:{idExcep}", ex.InnerException);
            }
        }

        /// <summary>
        /// Method created to insert new value document template
        /// </summary>
        /// <param name="documentTemplate"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<TemplateSectionsRelationGetDTO> Post(TemplateSectionsRelationCreateDTO templateSectionRelation)
        {
            try
            {
                var entity = _mapper.Map<TemplateSectionsRelation>(templateSectionRelation);

                await _context.TemplateSectionsRelations.AddAsync(entity).ConfigureAwait(false);
                var result = await _context.SaveChangesAsync().ConfigureAwait(false);

                if (result == 0)
                {
                    throw new Exception("Problem prilikom kreiranja novog Document Template-a.");
                }

                return _mapper.Map<TemplateSectionsRelationGetDTO>(entity);
            }
            catch (Exception ex)
            {
                string exceptionAt = Utils.GetMethodAndClassName1(System.Reflection.MethodInfo.GetCurrentMethod()).ToString();
                string allParams = JsonSerializer.Serialize(templateSectionRelation);
                var idExcep = await _logSvc.LogException(exceptionAt, ex, allParams).ConfigureAwait(false);
                throw new Exception($"{ex.Message} -ExceptionID:{idExcep}", ex.InnerException);
            }
        }

        /// <summary>
        /// Method created to update document template.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="documentTemplate"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> Put(int id, DocumentTemplateUpdateDTO documentTemplate)
        {
            try
            {
                var templateToUpdate = await _context.DocumentTemplates.FindAsync(id).ConfigureAwait(false);

                if (templateToUpdate == null)
                    return false;

                _mapper.Map(documentTemplate, templateToUpdate);

                await _context.SaveChangesAsync().ConfigureAwait(false);

                return true;
            }
            catch (Exception ex)
            {
                string exceptionAt = Utils.GetMethodAndClassName1(System.Reflection.MethodInfo.GetCurrentMethod()).ToString();
                string allParams = JsonSerializer.Serialize(documentTemplate);
                var idExcep = await _logSvc.LogException(exceptionAt, ex, allParams).ConfigureAwait(false);
                throw new Exception($"{ex.Message} -ExceptionID:{idExcep}", ex.InnerException);
            }
        }


        public async Task<DocumentTemplateAndRelatedItemsDTO> ManageRelationsForDocumentTemplate(DocumentTemplateAndRelatedItemsDTO documentTemplate)
        {
            // 1. Update/insert osnovnih podataka o šablonu
            var template = await _context.DocumentTemplates
                .FirstOrDefaultAsync(t => t.Id == documentTemplate.Id);

            // 2. Sinhronizuj relacije
            var existingRelations = await _context.TemplateSectionsRelations
                .Where(r => r.IdTemplate == documentTemplate.IdTemplate && r.TemplateVersion == documentTemplate.Version)
                .ToListAsync();

            var newRelations = documentTemplate.Relations ?? new List<TemplateSectionRelationWithSectionDTO>();


            throw new NotImplementedException();
        }
    }
}