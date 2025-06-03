using AutoMapper;
using InventoryAPI.Application.DTO;

namespace InventoryAPI.Application.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateProductDTO, Product>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}