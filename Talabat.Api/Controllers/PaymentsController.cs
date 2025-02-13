using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.Api.Dtos;
using Talabat.Api.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Services;

namespace Talabat.Api.Controllers
{
    [Authorize]
    public class PaymentsController : ApiBaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentsController> _logger;
        private  const string _whSecret = "whsec_7ffedb9cfc068eef20da893c068330473917de17f6d64ebb17dfcd85c6fad731";
        public PaymentsController(IPaymentService paymentService,IMapper mapper,
            ILogger<PaymentsController> logger)
        {
            _paymentService = paymentService;
           _mapper = mapper;
            _logger = logger;
        }
        [HttpPost("{basketId}")]
        [ProducesResponseType(typeof(CustomerBasket),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse),StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerBasketDto>> Payment(string basketId)
        {
            var basket=await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            if (basket is null) return BadRequest(new ApiErrorResponse(400, "A problem with your basket"));
            var mappedbasket = _mapper.Map<CustomerBasket, CustomerBasketDto>(basket);
            return Ok(mappedbasket);
        }


        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
           
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], _whSecret);
                var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
             
                switch (stripeEvent.Type)
                {
                    case "payment_intent.succeeded":
                        await _paymentService.UpdatePaymentIntentSucceedOrFail(paymentIntent.Id, true);
                        _logger.LogInformation( "Payment is succeed",paymentIntent.Id);
                        break;
                    case "payment_intent.payment_failed":
                        await _paymentService.UpdatePaymentIntentSucceedOrFail(paymentIntent.Id, false);
                        _logger.LogInformation("Payment is failed", paymentIntent.Id);
                        break;
                }
                
               

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
           
        }
        }
}
