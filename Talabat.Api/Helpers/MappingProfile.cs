using AutoMapper;
using Talabat.Api.Dtos;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregation;



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
            CreateMap<OrderItem,OrderItemDto>()
                .ForMember(p => p.ProductId, o => o.MapFrom(d => d.Product.ProductId))
                .ForMember(p => p.ProductName, o => o.MapFrom(d => d.Product.ProductName))
                .ForMember(p => p.PictureUrl, o => o.MapFrom(d => d.Product.PictureUrl))
                 .ForMember(p => p.PictureUrl, o => o.MapFrom<OrderPictureUrlResolver>());
            CreateMap<Order, OrderToReturnDto>()
               .ForMember(m => m.DeliveryMethod, o => o.MapFrom(d => d.DeliveryMethod.ShortName))
               .ForMember(m => m.DeliveryMethodCost, o => o.MapFrom(d => d.DeliveryMethod.Cost));
             


        }
    }
}
