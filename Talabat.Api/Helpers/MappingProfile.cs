using AutoMapper;
using Talabat.Api.Dtos;
using Talabat.Core.Entities;



namespace Talabat.Api.Helpers
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(s=>s.ProductBrand,o=>o.MapFrom(d=>d.ProductBrand.Name))
                .ForMember(s => s.ProductType, o => o.MapFrom(d => d.ProductType.Name))
                .ForMember(p=>p.PictureUrl,o=>o.MapFrom<ProductPictureUrlResolver>());
            CreateMap<Talabat.Core.Entities.Identity.Address, AddressDto>().ReverseMap();
            CreateMap<AddressDto, Talabat.Core.Entities.Order_Aggregation.Address>();
            CreateMap<CustomerBasket,CustomerBasketDto>().ReverseMap();
            CreateMap<BasketItem,BasketItemDto>().ReverseMap();
            

        }
    }
}
