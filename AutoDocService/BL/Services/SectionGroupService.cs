using AutoMapper;
using AutoDocService.API.ServiceInterfaces;
using AutoDocService.DL.DBContext;
using AutoDocService.DL.Entities;
using AutoDocService.Helpers.TrimHelper;
using AutoDocService.Helpers.Utils;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using AutoDoc.Shared.Model.DTO.SectionGroupDTO;
using AutoDoc.Shared.Model.DTO.Enumerations;

namespace AutoDocService.BL.Services
{
    /// <summary>
    /// SectionGroup service
    /// </summary>
    public class SectionGroupService : ISectionGroupService
    {
        #region Initialize
        private readonly ContractGenerationContext contractGenerationContext ;
        private static ILogService _logSvc;
        private readonly IMapper _mapper;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="logSvc"></param>
        /// <param name="contractGenerationContext"></param>
        /// <param name="mapper"></param>
        public SectionGroupService(ILogService logSvc, ContractGenerationContext contractGenerationContext, IMapper mapper)
        {
            _logSvc = logSvc;
            this.contractGenerationContext = contractGenerationContext;
            _mapper = mapper;
        }
        #endregion


        /// <summary>
        /// Method created to get SectionGroup values based on input parameters
        /// If none parameter is provided, it will return all data.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="name"></param>
        /// <param name="offset"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<PagedList<SectionGroupGetDTO>> Get(int? id = null, GroupStatusType? status = null, string name = null, int offset = 0, int pageSize = 10)
        {
            try
            {
                List<SectionGroupGetDTO> sectionGroup = null;

                //Kreiranje queryable-a.
                var query = contractGenerationContext.SectionGroups.AsQueryable();

                if (id != null)
                    query = query.Where(e => e.ID == id);

                if (status != null)
                    query = query.Where(e => e.Status == status.ToString());

                if (!string.IsNullOrEmpty(name))
                    query = query.Where(e => e.Name.ToLower().Contains(name.ToLower()));

                var totalItems = await query.CountAsync().ConfigureAwait(false);

                if (offset > 0)
                    query = query.Skip(offset);

                if (pageSize > 0)
                    query = query.Take(pageSize);

                var result = await query.AsNoTracking().ToListAsync().ConfigureAwait(false);
                ExtensionMethod.StringTrimer5000(result);


                sectionGroup = _mapper.Map<List<SectionGroupGetDTO>>(result);
                var retVal = new PagedList<SectionGroupGetDTO>(sectionGroup, pageSize, offset, totalItems);
                return retVal;
            }
            catch (Exception ex)
            {
                string exceptionAt = Utils.GetMethodAndClassName1(System.Reflection.MethodInfo.GetCurrentMethod()).ToString();
                string allParams = string.Join(",",
                                                id == null ? "idEmpty" : id.ToString(),
                                                status == null ? "statusEmpty" : status,
                                                string.IsNullOrEmpty(name) ? "nameEmpty" : name);
                var idExcep = await _logSvc.LogException(exceptionAt, ex, allParams).ConfigureAwait(false);
                throw new Exception($"{ex.Message} -ExceptionID:{idExcep}", ex.InnerException);
            }
        }

        /// <summary>
        /// Metohod created to insert new value of SectionGroup.
        /// </summary>
        /// <param name="sectionGroup"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<SectionGroupGetDTO> Post(SectionGroupCreateDTO sectionGroup)
        {
            try
            {
                var entity = _mapper.Map<SectionGroup>(sectionGroup);
                entity.DateInserted = DateTime.Now;

                await contractGenerationContext.SectionGroups.AddAsync(entity);

                await contractGenerationContext.SaveChangesAsync();

                return _mapper.Map<SectionGroupGetDTO>(entity);
            }
            catch (Exception ex)
            {
                string exceptionAt = Utils.GetMethodAndClassName1(System.Reflection.MethodInfo.GetCurrentMethod()).ToString();
                string allParams = JsonSerializer.Serialize(sectionGroup);
                var idExcep = await _logSvc.LogException(exceptionAt, ex, allParams).ConfigureAwait(false);
                throw new Exception($"{ex.Message} -ExceptionID:{idExcep}", ex.InnerException);
            }
        }

        /// <summary>
        /// Method created to update section Group.
        /// </summary>
        /// <param name="sectionGroup"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> Put(SectionGroupUpdateDTO sectionGroup)
        {
            try
            {
                var sectionGroupToUpdate = await contractGenerationContext.SectionGroups.FindAsync(sectionGroup.ID).ConfigureAwait(false);

                if (sectionGroupToUpdate == null)
                    return false;

                sectionGroupToUpdate.DateUpdated = DateTime.Now;

                _mapper.Map(sectionGroup, sectionGroupToUpdate);

                await contractGenerationContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                string exceptionAt = Utils.GetMethodAndClassName1(System.Reflection.MethodInfo.GetCurrentMethod()).ToString();
                string allParams = JsonSerializer.Serialize(sectionGroup);
                var idExcep = await _logSvc.LogException(exceptionAt, ex, allParams).ConfigureAwait(false);
                throw new Exception($"{ex.Message} -ExceptionID:{idExcep}", ex.InnerException);
            }
        }

        /// <summary>
        /// Method created to delete sectionGroup
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> Delete(int id)
        {
            try
            {
                var sectionGroup = await contractGenerationContext.SectionGroups.FindAsync(id);

                if (sectionGroup == null)
                    return false;

                contractGenerationContext.SectionGroups.Remove(sectionGroup);

                await contractGenerationContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                string exceptionAt = Utils.GetMethodAndClassName1(System.Reflection.MethodInfo.GetCurrentMethod()).ToString();
                var idExcep = await _logSvc.LogException(exceptionAt, ex, $"id: {id}").ConfigureAwait(false);
                throw new Exception($"{ex.Message} -ExceptionID:{idExcep}", ex.InnerException);
            }
        }
    }
}
