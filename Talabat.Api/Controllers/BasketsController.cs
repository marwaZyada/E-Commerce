using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Api.Dtos;
using Talabat.Api.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;

namespace Talabat.Api.Controllers
{
 
    public class BasketsController : ApiBaseController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _map;

        public BasketsController(IBasketRepository basketRepository,IMapper map)
        {
            _basketRepository = basketRepository;
           _map = map;
        }
        [HttpGet]
       
        public async Task<ActionResult<CustomerBasket>> get(string id)
        {
          var basket=await  _basketRepository.GetBasketAsync(id);
            
                
            return basket is null?new CustomerBasket(id): basket;
        }
        [HttpPost]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdate(CustomerBasketDto basket)
        {
            var mappedbasket=_map.Map<CustomerBasket>(basket);
            var updatedorcreatedbasket = await _basketRepository.UpdateBasketAsync(mappedbasket);
            if (updatedorcreatedbasket is null)
                return BadRequest(new ApiErrorResponse(400));

            return Ok(updatedorcreatedbasket);
        }
        [HttpDelete]
        public async Task<ActionResult<bool>> Delete(string id)
        {
            

            return await _basketRepository.DeleteBasketAsync(id);
        }
    }
}
