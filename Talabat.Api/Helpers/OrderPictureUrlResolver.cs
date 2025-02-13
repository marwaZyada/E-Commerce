using AutoMapper;
using Talabat.Api.Dtos;
using Talabat.Core.Entities.Order_Aggregation;

namespace Talabat.Api.Helpers
{
    public class OrderPictureUrlResolver : IValueResolver<OrderItem,OrderItemDto, string>
    {
        private readonly IConfiguration _configuration;

        public OrderPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Product.PictureUrl))

                return $"{_configuration["ApiBaseUrl"]}{source.Product.PictureUrl}";
            return string.Empty;

        }
   
    }
}
