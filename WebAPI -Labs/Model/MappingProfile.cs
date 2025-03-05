using AutoMapper;
using Context;
using WebAPI_Labs.DTOs;
using WebAPI_Labs.Model;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProdWithCatDto>()
            .ForMember(dest => dest.CatName, opt => opt.MapFrom(src => src.Category.CategoryName));

        CreateMap<AddProductDto, Product>();

    }
}
