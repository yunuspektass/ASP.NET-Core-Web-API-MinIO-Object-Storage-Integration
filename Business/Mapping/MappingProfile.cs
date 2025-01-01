using AutoMapper;
using Business.DTOs;
using FileInfo = Domain.Entities.FileInfo;

namespace Business.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<FileInfo, FileCreateDto>().ReverseMap();
        CreateMap<FileInfo, FileGetDto>().ReverseMap();
        CreateMap<FileInfo, FileUpdateDto>().ReverseMap()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

    }

}
