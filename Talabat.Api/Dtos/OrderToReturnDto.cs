using System.ComponentModel.DataAnnotations.Schema;
using Talabat.Core.Entities.Order_Aggregation;

namespace Talabat.Api.Dtos
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } 
        public string Status { get; set; } 
        public Address ShippingAddress { get; set; }
        public decimal DeliveryMethodCost { get; set; }
        public string DeliveryMethod { get; set; }
        public ICollection<OrderItemDto> OrderItems { get; set; } = new HashSet<OrderItemDto>();
        public decimal SubTotal { get; set; }
        public string PaymentIntetId { get; set; } 
      
        public decimal Total { get; set; }

    }
}
