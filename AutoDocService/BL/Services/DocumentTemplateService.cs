using AutoMapper;
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
    /// <summary>
    /// Document template Service
    /// </summary>
    public class DocumentTemplateService : IDocumentTemplateService
    {
        private readonly ContractGenerationContext contractGenerationContext;
        private readonly IMapper _mapper;
        private readonly ILogService _logSvc;

        /// <summary>
        /// Constructor for document template service
        /// </summary>
        /// <param name="logSvc"></param>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        public DocumentTemplateService(ILogService logSvc, ContractGenerationContext context, IMapper mapper)
        {
            _logSvc = logSvc;
            contractGenerationContext = context;
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
        public async Task<PagedList<DocumentTemplateGetDTO>> Get(int? id = null, int? idTemplate = null, string name = null, int? version = null, DocumentTemplateStatusType? status = null, bool? isLastValid = null, int offset = 0, int pageSize = 0)
        {
            try
            {
                DateTime dateTime = DateTime.Now.Date;
                var query = contractGenerationContext.DocumentTemplates.AsNoTracking().AsQueryable();

                if (id != null)
                    query = query.Where(e => e.Id == id);

                if (idTemplate != null)
                    query = query.Where(e => e.IdTemplate == idTemplate);

                if (!string.IsNullOrEmpty(name))
                    query = query.Where(e => e.Name.ToLower().Contains(name.ToLower()));

                if (version != null)
                    query = query.Where(e => e.Version == version);

                if (status != null)
                    query = query.Where(e => e.Status == status.ToString());

                if (isLastValid != null)
                {

                    if (isLastValid == true)
                        query = query.Where(e => e.ValidFrom <= dateTime && (e.ValidTo >= dateTime || e.ValidTo == null));
                    else
                        query = query.Where(e => e.ValidTo < dateTime);
                }

                var totalItems = await query.CountAsync().ConfigureAwait(false);

                if (offset > 0)
                    query = query.Skip(offset);

                if (pageSize > 0)
                    query = query.Take(pageSize);

                var result = await query.ToListAsync().ConfigureAwait(false);

                var documentTemplates = _mapper.Map<List<DocumentTemplateGetDTO>>(result);
                var retVal = new PagedList<DocumentTemplateGetDTO>(documentTemplates, pageSize, offset, totalItems);
                return retVal;
            }
            catch (Exception ex)
            {
                string exceptionAt = Utils.GetMethodAndClassName1(System.Reflection.MethodInfo.GetCurrentMethod()).ToString();
                string allParams = string.Join(",",
                                                id == null ? "idEmpty" : id.ToString(),
                                                idTemplate == null ? "idTemplateEmpty" : idTemplate.ToString(),
                                                string.IsNullOrEmpty(name) ? "nameEmpty" : name,
                                                version == null ? "versionEmpty" : version.ToString(),
                                                status == null ? "statusEmpty" : status);
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
        public async Task<DocumentTemplateGetDTO> Post(DocumentTemplateCreateDTO documentTemplate)
        {
            try
            {
                //Ukoliko nije poslata vrijednost idTemplate-a, dodaje se najveci broj
                if (documentTemplate.IdTemplate == null)
                {
                    documentTemplate.IdTemplate = await contractGenerationContext.DocumentTemplates.MaxAsync(e => (int?)e.IdTemplate).ConfigureAwait(false) + 1 ?? 1;
                }

                //Pronadji sve sekcije sa istim nazivom i pripadajucoj grupi ali razlicitim sectionIDom
                var isDuplicatedName = await contractGenerationContext.DocumentTemplates.AnyAsync(e => e.Name == documentTemplate.Name && e.IdTemplate != documentTemplate.IdTemplate).ConfigureAwait(false);

                //Nije moguce imati isti naziv za clanove koji imaju razlicit sectionId i istu grupu
                if (isDuplicatedName)
                {
                    throw new InvalidOperationException("Predložak sa ovim nazivom u ovoj grupi već postoji!");
                }

                //Pronadji sve sekcije sa istim ID-om.
                var existingItems = await contractGenerationContext.DocumentTemplates.Where(e => e.IdTemplate == documentTemplate.IdTemplate).ToListAsync().ConfigureAwait(false);

                //Ukoliko postoje podaci uneseni za isti sectionID uzima se najveca verzija i dodaje + 1, u suprotnom se kreira novi objekat sa verzijom 1
                if (existingItems != null && existingItems.Count != 0)
                {
                    var maxVersionItem = existingItems.OrderByDescending(e => e.Version).FirstOrDefault();
                    documentTemplate.Version = maxVersionItem.Version + 1;
                }
                else
                {
                    documentTemplate.Version = 1;
                }

                var entity = _mapper.Map<DocumentTemplates>(documentTemplate);

                await contractGenerationContext.DocumentTemplates.AddAsync(entity).ConfigureAwait(false);
                var result = await contractGenerationContext.SaveChangesAsync().ConfigureAwait(false);

                if (result == 0)
                {
                    throw new Exception("Problem prilikom kreiranja novog Document Template-a.");
                }

                return _mapper.Map<DocumentTemplateGetDTO>(entity);
            }
            catch (Exception ex)
            {
                string exceptionAt = Utils.GetMethodAndClassName1(System.Reflection.MethodInfo.GetCurrentMethod()).ToString();
                string allParams = JsonSerializer.Serialize(documentTemplate);
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
                var templateToUpdate = await contractGenerationContext.DocumentTemplates.FindAsync(id).ConfigureAwait(false);

                if (templateToUpdate == null)
                    return false;

                _mapper.Map(documentTemplate, templateToUpdate);

                await contractGenerationContext.SaveChangesAsync().ConfigureAwait(false);

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

        /// <summary>
        /// Method created to get data for document template and all it's related items
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
        public async Task<PagedList<DocumentTemplateAndRelatedItemsDTO>> GetTemplateWithRelationItems(int? id = null, int? idTemplate = null, string name = null, int? version = null, DocumentTemplateStatusType? status = null, bool? isLastValid = null, int offset = 0, int pageSize = 0)
        {
            try
            {
                DateTime dateTime = DateTime.Now.Date;
                var query = contractGenerationContext.DocumentTemplates
                    .AsNoTracking()
                    .Include(dt => dt.TemplateSectionsRelations)
                        .ThenInclude(rel => rel.Section)
                    .AsQueryable();

                if (id != null)
                    query = query.Where(e => e.Id == id);

                if (idTemplate != null)
                    query = query.Where(e => e.IdTemplate == idTemplate);

                if (!string.IsNullOrEmpty(name))
                    query = query.Where(e => e.Name.ToLower().Contains(name.ToLower()));

                if (version != null)
                    query = query.Where(e => e.Version == version);

                if (status != null)
                    query = query.Where(e => e.Status == status.ToString());

                if (isLastValid != null)
                {
                    if (isLastValid == true)
                        query = query.Where(e => e.ValidFrom <= dateTime && (e.ValidTo >= dateTime || e.ValidTo == null));
                    else
                        query = query.Where(e => e.ValidTo < dateTime);
                }

                var totalItems = await query.CountAsync().ConfigureAwait(false);

                if (offset > 0)
                    query = query.Skip(offset);

                if (pageSize > 0)
                    query = query.Take(pageSize);

                //Dobavljanje svih template-a ovisno o queryiju
                var templates = await query.ToListAsync().ConfigureAwait(false);

                // Ako nema rezultata, vrati praznu listu
                if (templates == null || templates.Count == 0)
                {
                    return new PagedList<DocumentTemplateAndRelatedItemsDTO>(
                        new List<DocumentTemplateAndRelatedItemsDTO>(), pageSize, offset, totalItems);
                }

                //Uzimaju se svi template-i te se mapiraju ostali podaci.
                // Mapiranje u DTO
                //var result = templates.Select(template => new DocumentTemplateAndRelatedItemsDTO
                //{
                //    Id = template.Id,
                //    IdTemplate = template.IdTemplate,
                //    Version = template.Version,
                //    Name = template.Name,
                //    Description = template.Description,
                //    Status = Enum.TryParse<DocumentTemplateStatusType>(template.Status, out var status) ? status : (DocumentTemplateStatusType?)null,
                //    UserInsert = template.UserInsert,
                //    DateInsert = template.DateInsert,
                //    UserUpdate = template.UserUpdate,
                //    DateUpdate = template.DateUpdate,
                //    UserVerified = template.UserVerified,
                //    ValidFrom = template.ValidFrom,
                //    ValidTo = template.ValidTo,
                //    // Lista relacija sa sekcijama
                //    Relations = template.TemplateSectionsRelations
                //        .OrderBy(r => r.Order)
                //        .Select(rel => new TemplateSectionRelationWithSectionDTO
                //        {
                //            Id = rel.Id,
                //            IdTemplate = rel.IdTemplate,
                //            TemplateVersion = rel.TemplateVersion,
                //            IdSection = rel.IdSection,
                //            SectionVersion = rel.SectionVersion,
                //            Order = rel.Order,
                //            ConditionExpression = rel.ConditionExpression,
                //            Action = rel.Action,
                //            IsPageBreak = rel.IsPageBreak,
                //            Section = rel.Section != null
                //                ? _mapper.Map<SectionsGetDTO>(rel.Section)
                //                : null
                //        }).ToList()
                //}).ToList();

                var result = templates.Select(template => new DocumentTemplateAndRelatedItemsDTO
                {
                    Id = template.Id,
                    IdTemplate = template.IdTemplate,
                    Version = template.Version,
                    Name = template.Name,
                    Description = template.Description,
                    Status = Enum.TryParse<DocumentTemplateStatusType>(template.Status, out var status) ? status : (DocumentTemplateStatusType?)null,
                    UserInserted = template.UserInserted,
                    DateInserted = template.DateInserted,
                    UserUpdated = template.UserUpdated,
                    DateUpdated = template.DateUpdated,
                    UserVerified = template.UserVerified,
                    ValidFrom = template.ValidFrom,
                    ValidTo = template.ValidTo,
                    // Lista relacija sa sekcijama
                    Relations = template.TemplateSectionsRelations
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
                }).ToList();

                var retVal = new PagedList<DocumentTemplateAndRelatedItemsDTO>(result, pageSize, offset, totalItems);
                return retVal;
            }
            catch (Exception ex)
            {
                string exceptionAt = Utils.GetMethodAndClassName1(System.Reflection.MethodInfo.GetCurrentMethod()).ToString();
                string allParams = string.Join(",",
                    id == null ? "idEmpty" : id.ToString(),
                    idTemplate == null ? "idTemplateEmpty" : idTemplate.ToString(),
                    string.IsNullOrEmpty(name) ? "nameEmpty" : name,
                    version == null ? "versionEmpty" : version.ToString(),
                    status == null ? "statusEmpty" : status);
                var idExcep = await _logSvc.LogException(exceptionAt, ex, allParams).ConfigureAwait(false);
                throw new Exception($"{ex.Message} -ExceptionID:{idExcep}", ex.InnerException);
            }
        }
    }
}