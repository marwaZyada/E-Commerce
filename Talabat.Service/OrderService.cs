using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregation;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specifications;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(IBasketRepository basketRepository
           ,IUnitOfWork unitOfWork,IPaymentService paymentService)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }
        public async Task<Order?> CreateOrderAsyn(string BuyerEmail, string BasketId, int DeliveryMethodId, Address ShippingAddress)
        {
            // get basket
            var basket = await _basketRepository.GetBasketAsync(BasketId);

            //get selected item from product repo
            var orderitems = new List<OrderItem>();
            if (basket?.Items?.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _unitOfWork.Repo<Product>().GetByIdAsync(item.Id);
                    var productitemordered = new ProductOrderItem()
                    {
                        ProductId = product.Id,
                        PictureUrl = product.PictureUrl,
                        ProductName = product.Name
                    };
                    var orderitem = new OrderItem()
                    {
                        Product = productitemordered,
                        Price = product.Price,
                        Quantity = item.Quantity

                    };
                    orderitems.Add(orderitem);
                }
            }
                    //calculate sub total
                    var subtotal = orderitems.Sum(o => o.Price * o.Quantity);

                    //get delivery method
                    var deliverymethod =await _unitOfWork.Repo<DeliveryMethod>().GetByIdAsync(DeliveryMethodId);
                     //check if there was anoter order with same intendid
                    var spec = new OrderWithPaymentIntentSpecification(basket.PaymentIntentId);   
                    var orderexist = await _unitOfWork.Repo<Order>().GetByIdWithSpecAsync(spec);
            if(orderexist is not null)
            {
                _unitOfWork.Repo<Order>().Delete(orderexist);
              await  _paymentService.CreateOrUpdatePaymentIntent(basket.Id);
            }
                    //create order
                    var order = new Order()
                    {
                        BuyerEmail = BuyerEmail,
                        ShippingAddress = ShippingAddress,
                        DeliveryMethod = deliverymethod,
                        SubTotal=subtotal,
                        OrderItems=orderitems,
                        PaymentIntetId=basket.PaymentIntentId
                    };

                    //add order local
                    await _unitOfWork.Repo<Order>().Add(order);
                    // save order to database
                  var result=  await _unitOfWork.Complete();
                     if (result <= 0) return null;
                    return order;
                
            

        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeleveryMethods()
        {
           return await _unitOfWork.Repo<DeliveryMethod>().GetAllAsync();
        }

        public async Task<Order> GetOrderByIdForUserAsync(int OrderId, string BuyerEmail)
        {
            var spec = new OrderSpecification(BuyerEmail,OrderId);
            var order = await _unitOfWork.Repo<Order>().GetByIdWithSpecAsync(spec);
            return order;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsyn(string BuyerEmail)
        {
            var spec=new OrderSpecification(BuyerEmail);
            var orders =await _unitOfWork.Repo<Order>().GetAllWithSpecAsync(spec);
            return orders;
        }
    }
}
