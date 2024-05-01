using AutoMapper;
using trsaints_frontend_api.DTOs;
using trsaints_frontend_api.Entities;

namespace trsaints_frontend_api.Mappings;

public class DomainDTOProfile: Profile
{   
    public DomainDTOProfile()
    {
        CreateMap<Skill, SkillDTO>().ReverseMap();
        CreateMap<Project, ProjectDTO>().ReverseMap();
        CreateMap<TechStack, StackDTO>().ReverseMap();

        CreateMap<Project, ProjectStackDTO>()
            .ForMember(dto => dto.StackName, opt => opt.MapFrom(src => src.TechStack.Name));
    }
}
