using AutoMapper;
using AutoDocService.API.ServiceInterfaces;
using AutoDocService.DL.DBContext;
using AutoDocService.DL.Entities;
using AutoDocService.Helpers.TrimHelper;
using AutoDocService.Helpers.Utils;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using AutoDoc.Shared.Model.DTO.SectionsDTO;

namespace AutoDocService.BL.Services
{
    /// <summary>
    /// Sections service
    /// </summary>
    public class SectionsService : ISectionsService
    {
        #region Initialize
        private readonly ContractGenerationContext contractGenerationContext;
        private static ILogService _logSvc;
        private readonly IMapper _mapper;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="logSvc"></param>
        /// <param name="contractGenerationContext"></param>
        /// <param name="mapper"></param>
        public SectionsService(ILogService logSvc, ContractGenerationContext contractGenerationContext, IMapper mapper)
        {
            _logSvc = logSvc;
            this.contractGenerationContext = contractGenerationContext;
            _mapper = mapper;
        }
        #endregion


        /// <summary>
        /// Method created to get Sections values based on input parameters
        /// If none parameter is provided, it will return all data.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idTemplate"></param>
        /// <param name="groupId"></param>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <param name="isActive"></param>
        /// <param name="isLatestOnly"></param>
        /// <param name="offset"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<PagedList<SectionsGetDTO>> Get(int? id = null, int? idTemplate = null, int? groupId = null, string name = null, int? version = null, bool? isActive = null, bool? isLatestOnly = null, int offset = 0, int pageSize = 0)
        {
            try
            {
                List<SectionsGetDTO> section = null;

                //Kreiranje queryable-a.
                var query = contractGenerationContext.Sections.AsNoTracking().AsQueryable();

                if (id != null)
                    query = query.Where(e => e.Id == id);

                if (idTemplate != null)
                    query = query.Where(e => e.IdSection == idTemplate);

                if (groupId != null)
                    query = query.Where(e => e.GroupId == groupId);

                if (!string.IsNullOrEmpty(name))
                    query = query.Where(s => s.Name.ToLower().Contains(name.ToLower()));
                if (version != null)
                    query = query.Where(e => e.Version == version);

                if (isActive != null)
                    query = query.Where(e=> e.IsActive == isActive);

                if (isLatestOnly == true)
                {
                    query = query.GroupBy(e => e.IdSection)
                                 .Select(g => g.OrderByDescending(e => e.Version).FirstOrDefault());
                }

                //ukupan broj objekata koji ce biti nakon izvrsavanja query-a.
                var totalItems = await query.CountAsync().ConfigureAwait(false);

                //Dodavanje offseta ukoliko postoji
                if (offset > 0)
                    query = query.Skip(offset);

                //Broj objekata koji ce se prikazati
                if (pageSize > 0)
                    query = query.Take(pageSize);

                //Izvrsavanje query-a
                var result = await query.AsNoTracking().ToListAsync().ConfigureAwait(false);
                ExtensionMethod.StringTrimer5000(result);

                section = _mapper.Map<List<SectionsGetDTO>>(result);
                var retVal = new PagedList<SectionsGetDTO>(section, pageSize, offset, totalItems);
                return retVal;
            }
            catch (Exception ex)
            {
                string exceptionAt = Utils.GetMethodAndClassName1(System.Reflection.MethodInfo.GetCurrentMethod()).ToString();
                string allParams = string.Join(",",
                                                id == null ? "idEmpty" : id.ToString(),
                                                groupId == null ? "statusEmpty" : groupId,
                                                string.IsNullOrEmpty(name) ? "nameEmpty" : name);
                var idExcep = await _logSvc.LogException(exceptionAt, ex, allParams).ConfigureAwait(false);
                throw new Exception($"{ex.Message} -ExceptionID:{idExcep}", ex.InnerException);
            }
        }

        /// <summary>
        /// Metohod created to insert new value of Sections.
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<SectionsGetDTO> Post(SectionsCreateDTO section)
        {
            try
            {
                //Ukoliko nije poslata vrijednost idSectiona, dodaje se najveci broj
                if(section.IdSection == null)
                {
                    section.IdSection = await contractGenerationContext.Sections.MaxAsync(e => (int?)e.IdSection).ConfigureAwait(false) + 1 ?? 1;
                }

                //Pronadji sve sekcije sa istim nazivom i pripadajucoj grupi ali razlicitim sectionIDom
                var isDuplicatedName = await contractGenerationContext.Sections.AnyAsync(e => e.Name == section.Name && e.GroupId == section.GroupId && e.IdSection != section.IdSection).ConfigureAwait(false);

                //Nije moguce imati isti naziv za clanove koji imaju razlicit sectionId i istu grupu
                if (isDuplicatedName)
                {
                    throw new InvalidOperationException("Član sa ovim nazivom u ovoj grupi već postoji!");
                }

                //Pronadji sve sekcije sa istim ID-om.
                var existingItems = await contractGenerationContext.Sections.Where(e => e.IdSection == section.IdSection).ToListAsync().ConfigureAwait(false);

                var entity = _mapper.Map<Sections>(section);

                //Ukoliko postoje podaci uneseni za isti sectionID uzima se najveca verzija i dodaje + 1, u suprotnom se kreira novi objekat sa verzijom 1
                if (existingItems != null && existingItems.Count != 0)
                {
                    var maxVersionItem = existingItems.OrderByDescending(e => e.Version).FirstOrDefault();
                    entity.Version = maxVersionItem.Version + 1;
                }
                else
                {
                    entity.Version = 1;
                }

                await contractGenerationContext.Sections.AddAsync(entity).ConfigureAwait(false);

                await contractGenerationContext.SaveChangesAsync().ConfigureAwait(false);

                return _mapper.Map<SectionsGetDTO>(entity);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException($"{ex.Message}");
            }
            catch (Exception ex)
            {
                string exceptionAt = Utils.GetMethodAndClassName1(System.Reflection.MethodInfo.GetCurrentMethod()).ToString();
                string allParams = JsonSerializer.Serialize(section);
                var idExcep = await _logSvc.LogException("test", ex, "test").ConfigureAwait(false);
                //var idExcep = await _logSvc.LogException(exceptionAt, ex, allParams).ConfigureAwait(false);
                throw new Exception($"{ex.Message} -ExceptionID:{idExcep}", ex.InnerException);
            }
        }

        /// <summary>
        /// Method created to update section.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> Put(int id, SectionsUpdateDTO section)
        {
            try
            {
                var sectionToUpdate = await contractGenerationContext.Sections.FindAsync(id).ConfigureAwait(false);

                if (sectionToUpdate == null)
                    return false;

                _mapper.Map(section, sectionToUpdate);

                sectionToUpdate.DateUpdated = DateTime.Now;

                await contractGenerationContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                string exceptionAt = Utils.GetMethodAndClassName1(System.Reflection.MethodInfo.GetCurrentMethod()).ToString();
                string allParams = JsonSerializer.Serialize(section);
                var idExcep = await _logSvc.LogException(exceptionAt, ex, allParams).ConfigureAwait(false);
                throw new Exception($"{ex.Message} -ExceptionID:{idExcep}", ex.InnerException);
            }
        }

        /// <summary>
        /// Method created to update existing section. 
        /// It can handle creating new version or editing existing values based on input parameters
        /// </summary>
        /// <param name="id"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> ManageSection(int id, SectionsUpdateDTO section)
        {
            try
            {
                var sectionToUpdate = await contractGenerationContext.Sections.FindAsync(id).ConfigureAwait(false);

                if (section.Content == sectionToUpdate.Content)
                {
                    await Put(id, section);

                }
                else
                {
                    var createSection = new SectionsCreateDTO();
                    createSection.IdSection = sectionToUpdate.IdSection;
                    createSection.GroupId = section.GroupId;
                    createSection.Name = section.Name;
                    createSection.Description = section.Description;
                    createSection.Content = section.Content;
                    createSection.IsActive = section.IsActive;
                    createSection.UserInserted = section.UserUpdated;
                    await Post(createSection);
                }
                return true;
            }
            catch (Exception ex)
            {
                string exceptionAt = Utils.GetMethodAndClassName1(System.Reflection.MethodInfo.GetCurrentMethod()).ToString();
                string allParams = JsonSerializer.Serialize(section);
                var idExcep = await _logSvc.LogException(exceptionAt, ex, allParams).ConfigureAwait(false);
                throw new Exception($"{ex.Message} -ExceptionID:{idExcep}", ex.InnerException);
            }
        }

        /// <summary>
        /// Updates the status of a section by Id or all sections by SectionId.
        /// </summary>
        /// <param name="id">The unique identifier of the section to update (optional).</param>
        /// <param name="sectionId">The logical identifier of the sections to update (optional).</param>
        /// <param name="isActiveStatus">The new status to set.</param>
        /// <returns>True if the update was successful, otherwise false.</returns>
        public async Task<bool> UpdateSectionStatus(int? id, int? sectionId, bool isActiveStatus)
        {
            try
            {
                if (!id.HasValue && !sectionId.HasValue)
                    throw new ArgumentException("ID ili ID sekcije mora biti unesen.");

                if (id.HasValue)
                {
                    // Update a single section by Id
                    var section = await contractGenerationContext.Sections.FindAsync(id.Value).ConfigureAwait(false);

                    if (section == null)
                        return false;

                    section.IsActive = isActiveStatus;
                    section.DateUpdated = DateTime.Now;
                }
                else if (sectionId.HasValue)
                {
                    // Update all sections with the given SectionId
                    var sections = await contractGenerationContext.Sections
                        .Where(s => s.IdSection == sectionId.Value)
                        .ToListAsync()
                        .ConfigureAwait(false);

                    if (!sections.Any())
                        return false;

                    foreach (var section in sections)
                    {
                        section.IsActive = isActiveStatus;
                        section.DateUpdated = DateTime.Now;
                    }
                }

                // Save changes to the database
                await contractGenerationContext.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            catch (Exception ex)
            {
                string exceptionAt = Utils.GetMethodAndClassName1(System.Reflection.MethodInfo.GetCurrentMethod()).ToString();
                var idExcep = await _logSvc.LogException(exceptionAt, ex, $"id: {id}, sectionId: {sectionId}, status: {isActiveStatus}").ConfigureAwait(false);
                throw new Exception($"{ex.Message} -ExceptionID:{idExcep}", ex.InnerException);
            }
        }
    }
}
