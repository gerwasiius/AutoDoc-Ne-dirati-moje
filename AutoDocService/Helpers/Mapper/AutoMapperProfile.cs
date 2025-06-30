using AutoMapper;
using AutoDocService.DL.Entities;
using AutoDoc.Shared.Model.DTO.SectionGroupDTO;
using AutoDoc.Shared.Model.DTO.SectionsDTO;
using AutoDoc.Shared.Model.DTO.DocumentTemplateDTO;
using AutoDoc.Shared.Model.DTO.TemplateSectionsRelationDTO;

namespace AutoDocService.Helpers.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region SectionGroup - SectionGroupDTO
            CreateMap<SectionGroup, SectionGroupGetDTO>();
            CreateMap<SectionGroup, SectionGroupCreateDTO>().ForMember(e => e.UserInserted, x => x.MapFrom(e => e.UserInserted)).ReverseMap();
            CreateMap<SectionGroup, SectionGroupUpdateDTO>().ForMember(e => e.UserUpdated, x => x.MapFrom(e => e.UserUpdated)).ReverseMap();
            #endregion

            #region Section - SectionsDTO
            CreateMap<Sections, SectionsGetDTO>();
            CreateMap<Sections, SectionsCreateDTO>().ReverseMap();
            CreateMap<Sections, SectionsUpdateDTO>().ReverseMap();
            #endregion
            
            #region DocumentTeplate - DocumentTemplateDTO
            CreateMap<DocumentTemplates, DocumentTemplateGetDTO>();
            CreateMap<DocumentTemplates, DocumentTemplateCreateDTO>().ReverseMap();
            CreateMap<DocumentTemplates, DocumentTemplateUpdateDTO>().ReverseMap();
            #endregion


            #region TemplateSectionRelation - TemplateSectionRelationDTO
            CreateMap<TemplateSectionsRelation, TemplateSectionsRelationGetDTO>();
            CreateMap<TemplateSectionsRelation, TemplateSectionsRelationCreateDTO>().ReverseMap();
            CreateMap<TemplateSectionsRelation, TemplateSectionsRelationUpdateDTO>().ReverseMap();
            #endregion
        }
    }
}
