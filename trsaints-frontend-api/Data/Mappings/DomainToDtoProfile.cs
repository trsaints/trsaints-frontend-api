using System.Globalization;
using AutoMapper;
using trsaints_frontend_api.Data.DTOs;
using trsaints_frontend_api.Data.Entities;

namespace trsaints_frontend_api.Data.Mappings;

public class DomainToDtoProfile : Profile
{
    public DomainToDtoProfile()
    {
        CreateMap<Skill, SkillDTO>().ReverseMap();
        CreateMap<Project, ProjectDTO>().ReverseMap();
        CreateMap<TechStack, TechStackDTO>().ReverseMap();

        // In your AutoMapper configuration
        CreateMap<ProjectDTO, Project>()
            .ForMember(dest => dest.Date,
                       opt => opt.MapFrom(
                           src => DateTime
                                  .ParseExact(
                                      src.Date,
                                      "dd/MM/yyyy",
                                      CultureInfo.InvariantCulture)
                                  .ToUniversalTime()));

        CreateMap<Project, ProjectStackDTO>()
            .ForMember(dto => dto.StackName,
                       opt => opt.MapFrom(src => src.TechStack.Name));
    }
}
