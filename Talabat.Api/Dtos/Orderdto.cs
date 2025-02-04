using Talabat.Core.Entities.Order_Aggregation;

namespace Talabat.Api.Dtos
{
    public class Orderdto
    {
        public string BasketId { get; set; }
        public int DeliveryMethodId { get; set; }
        public AddressDto ShippingAddress { get; set; }
    }
}
