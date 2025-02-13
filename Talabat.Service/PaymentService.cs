using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Configuration;
using Stripe;
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
using Product= Talabat.Core.Entities.Product;
namespace Talabat.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration,IBasketRepository basketRepository,
            IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:Secretkey"];
            var basket=await _basketRepository.GetBasketAsync(basketId);
            if (basket is null) return null;
            var shippingPrice = 0m;
           if(basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod=await _unitOfWork.Repo<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
                shippingPrice = deliveryMethod.Cost;
                basket.ShippingCost= deliveryMethod.Cost;
            }
            if (basket?.Items?.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _unitOfWork.Repo<Product>().GetByIdAsync(item.Id);
                    if (item.Price != product.Price)
                        item.Price = product.Price;
                }
            }
            var service = new PaymentIntentService();
            PaymentIntent paymentIntent;
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var option = new PaymentIntentCreateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Price * item.Quantity * 100)
                    + (long)shippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card" }
                };
                paymentIntent = await service.CreateAsync(option);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                var option = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Price * item.Quantity * 100)
                    + (long)shippingPrice * 100,
                };
                await service.UpdateAsync(basket.PaymentIntentId,option);
            }   
            await _basketRepository.UpdateBasketAsync(basket);
            return basket;
       }

        public async Task<Order> UpdatePaymentIntentSucceedOrFail(string IntentId, bool IsSuccedd)
        {
            var spec = new OrderWithPaymentIntentSpecification(IntentId);
            var order = await _unitOfWork.Repo<Order>().GetByIdWithSpecAsync(spec);
            if (!IsSuccedd) 
                order.Status = OrderStatus.PaymentFailed;
            else
            order.Status = OrderStatus.PaymentRecieved;
             _unitOfWork.Repo<Order>().Update(order);
            await _unitOfWork.Complete();
            return order;
        }
    }
}
