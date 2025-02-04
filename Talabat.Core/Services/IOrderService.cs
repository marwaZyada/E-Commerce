using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregation;

namespace Talabat.Core.Services
{
    public interface IOrderService
    {
        Task<Order?> CreateOrderAsyn(string BuyerEmail,string BasketId,int DeliveryMethodId,Address ShippingAddress);
        Task<IReadOnlyList<Order>> GetOrdersForUserAsyn(string BuyerEmail);
        Task<Order> GetOrderByIdForUserAsync(int OrderId,string BuyerEmail);
        Task<IReadOnlyList<DeliveryMethod>> GetDeleveryMethods();
    }
}
