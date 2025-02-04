using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.Api.Dtos;
using Talabat.Api.Errors;
using Talabat.Core.Entities.Order_Aggregation;
using Talabat.Core.Services;

namespace Talabat.Api.Controllers
{
    [Authorize]
    public class OrderController : ApiBaseController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderService,IMapper mapper)
        {
          _orderService = orderService;
            _mapper = mapper;
        }
        [HttpPost]
        [ProducesResponseType(typeof(Order),StatusCodes.Status200OK )]
        [ProducesResponseType(typeof(ApiErrorResponse),StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Order>> CreateOrder(Orderdto orderdto)
        {
            var buyeremail = User.FindFirstValue(ClaimTypes.Email);
            var address=_mapper.Map<Address>(orderdto.ShippingAddress);
          var order=  await _orderService.CreateOrderAsyn(buyeremail, orderdto.BasketId, orderdto.DeliveryMethodId, address);
            if (order is null) return BadRequest(new ApiErrorResponse(400));
            return Ok(order);
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Order>>> GetOrdersForUser()
        {
            var buyeremail = User.FindFirstValue(ClaimTypes.Email);
            var orders =await _orderService.GetOrdersForUserAsyn(buyeremail);
            return Ok(orders);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Order>> GetOrdersForUser(int id)
        {
            var buyeremail = User.FindFirstValue(ClaimTypes.Email);
            var order = await _orderService.GetOrderByIdForUserAsync(id,buyeremail);
            if (order is null) return NotFound(new ApiErrorResponse(404));
            return Ok(order);
        }
        [HttpGet("AllDeliveryMethod")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethod()
        {
           
            var DeliveryMethods = await _orderService.GetDeleveryMethods();
            return Ok(DeliveryMethods);
        }
    }
}
