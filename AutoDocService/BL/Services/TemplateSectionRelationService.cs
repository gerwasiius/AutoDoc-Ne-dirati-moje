using AutoMapper;
using AutoDocService.API.Controllers;
using AutoDocService.API.ServiceInterfaces;
using AutoDocService.DL.DBContext;
using AutoDocService.DL.Entities;
using AutoDocService.Helpers.Utils;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using AutoDoc.Shared.Model.DTO.TemplateSectionsRelationDTO;
using AutoDoc.Shared.Model.DTO.DocumentTemplateDTO;
using AutoDoc.Shared.Model.DTO.Enumerations;

namespace AutoDocService.BL.Services
{

    public class TemplateSectionsRelationDiffResult
    {
        public List<TemplateSectionsRelationCreateDTO> ToAdd { get; set; } = new();
        public List<TemplateSectionsRelationUpdateDTO> ToUpdate { get; set; } = new();
        public List<int> ToDelete { get; set; } = new(); // List of Ids to delete
    }
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

            // 1. Find differences
            var differences = await CompareTemplateSectionsRelations(documentTemplate);

            // 2. Insert new relations
            foreach (var toAdd in differences.ToAdd)
            {
                var entity = _mapper.Map<TemplateSectionsRelation>(toAdd);
                entity.ActionType = "NONE";
                await _context.TemplateSectionsRelations.AddAsync(entity);
            }

            // 3. Update existing relations
            foreach (var toUpdate in differences.ToUpdate)
            {
                var entity = await _context.TemplateSectionsRelations.FindAsync(toUpdate.Id);
                if (entity != null)
                {
                    entity.Order = toUpdate.Order;
                    entity.ConditionExpression = toUpdate.ConditionExpression;
                    entity.ActionType = toUpdate.ActionType;
                    entity.ActionType = "NONE";
                    entity.IsPageBreak = toUpdate.IsPageBreak;
                    entity.IsArticle = toUpdate.IsArticle;
                    // Optionally update other fields if needed
                }
            }

            // 4. Delete removed relations
            foreach (var id in differences.ToDelete)
            {
                var entity = await _context.TemplateSectionsRelations.FindAsync(id);
                if (entity != null)
                {
                    _context.TemplateSectionsRelations.Remove(entity);
                }
            }

            // 5. Save all changes
            await _context.SaveChangesAsync();

            // 6. Return the updated template with relations
            // (Assuming you want to return the latest state)
            var updated = await _context.DocumentTemplates
                .AsNoTracking()
                .Include(dt => dt.TemplateSectionsRelations)
                    .ThenInclude(rel => rel.Section)
                .FirstOrDefaultAsync(dt => dt.Id == documentTemplate.Id);

            if (updated == null)
                throw new Exception("Template not found after update.");

            // Map to DTO
            var result = new DocumentTemplateAndRelatedItemsDTO
            {
                Id = updated.Id,
                IdTemplate = updated.IdTemplate,
                Version = updated.Version,
                Name = updated.Name,
                Description = updated.Description,
                Status = Enum.TryParse<DocumentTemplateStatusType>(updated.Status, out var status) ? status : (DocumentTemplateStatusType?)null,
                UserInserted = updated.UserInserted,
                DateInserted = updated.DateInserted,
                UserUpdated = updated.UserUpdated,
                DateUpdated = updated.DateUpdated,
                UserVerified = updated.UserVerified,
                ValidFrom = updated.ValidFrom,
                ValidTo = updated.ValidTo,
                Relations = updated.TemplateSectionsRelations
                    .OrderBy(r => r.Order)
                    .Select(rel => new TemplateSectionRelationWithSectionDTO
                    {
                        RelationId = rel.Id,
                        SectionId = rel.IdSection,
                        SectionVersion = rel.SectionVersion,
                        Order = rel.Order,
                        ConditionExpression = rel.ConditionExpression,
                        ActionType = rel.ActionType,
                        IsPageBreak = rel.IsPageBreak,
                        IsArticle = rel.IsArticle,
                        SectionUniqueId = rel.Section != null ? rel.Section.Id : 0,
                        SectionName = rel.Section != null ? rel.Section.Name : null,
                        SectionDescription = rel.Section != null ? rel.Section.Description : null
                    }).ToList()
            };

            return result;
        }

        public async Task<TemplateSectionsRelationDiffResult> CompareTemplateSectionsRelations(DocumentTemplateAndRelatedItemsDTO dto)
        {
            try
            {

            // 1. Fetch current relations from DB for the given template and version
            var dbRelations = await _context.TemplateSectionsRelations
                .Where(r => r.IdTemplate == dto.IdTemplate && r.TemplateVersion == dto.Version)
                .ToListAsync();

            // 2. Prepare incoming relations for comparison
            var incoming = dto.Relations ?? new List<TemplateSectionRelationWithSectionDTO>();

            // 3. Find relations to add, update, and delete
            var toAdd = new List<TemplateSectionsRelationCreateDTO>();
            var toUpdate = new List<TemplateSectionsRelationUpdateDTO>();
            var toDelete = new List<int>();

            // Map DB relations by (SectionId, SectionVersion)
            var dbMap = dbRelations.ToDictionary(
                r => (r.IdSection, r.SectionVersion),
                r => r
            );

            // Map incoming relations by (SectionId, SectionVersion)
            var incomingMap = incoming.ToDictionary(
                r => (r.SectionId, r.SectionVersion),
                r => r
            );

            // 3a. Find to add or update
            foreach (var rel in incoming)
            {
                var key = (rel.SectionId, rel.SectionVersion);
                if (!dbMap.TryGetValue(key, out var dbRel))
                {
                    // Not in DB, needs to be added
                    toAdd.Add(new TemplateSectionsRelationCreateDTO
                    {
                        IdTemplate = dto.IdTemplate,
                        TemplateVersion = dto.Version,
                        IdSection = rel.SectionId,
                        SectionVersion = rel.SectionVersion,
                        Order = rel.Order,
                        ConditionExpression = rel.ConditionExpression,
                        ActionType = rel.ActionType,
                        IsPageBreak = rel.IsPageBreak,
                        IsArticle = rel.IsArticle
                    });
                }
                else
                {
                    // Exists in DB, check if any field differs
                    if (dbRel.Order != rel.Order ||
                        dbRel.ConditionExpression != rel.ConditionExpression ||
                        dbRel.ActionType != rel.ActionType ||
                        dbRel.IsPageBreak != rel.IsPageBreak || dbRel.IsArticle != rel.IsArticle)
                    {
                        toUpdate.Add(new TemplateSectionsRelationUpdateDTO
                        {
                            Id = dbRel.Id,
                            Order = rel.Order,
                            ConditionExpression = rel.ConditionExpression,
                            ActionType = rel.ActionType,
                            IsPageBreak = rel.IsPageBreak,
                            IsArticle = rel.IsArticle
                            
                        });
                    }
                }
            }

            // 3b. Find to delete (in DB but not in incoming)
            foreach (var dbRel in dbRelations)
            {
                var key = (dbRel.IdSection, dbRel.SectionVersion);
                if (!incomingMap.ContainsKey(key))
                {
                    toDelete.Add(dbRel.Id);
                }
            }

            return new TemplateSectionsRelationDiffResult
            {
                ToAdd = toAdd,
                ToUpdate = toUpdate,
                ToDelete = toDelete
            };
            }catch(Exception ex)
            {
                string exceptionAt = Utils.GetMethodAndClassName1(System.Reflection.MethodInfo.GetCurrentMethod()).ToString();
                string allParams = JsonSerializer.Serialize(dto);
                var idExcep = await _logSvc.LogException(exceptionAt, ex, allParams).ConfigureAwait(false);
                throw new Exception($"{ex.Message} -ExceptionID:{idExcep}", ex.InnerException);
            }

        }
    }
}
