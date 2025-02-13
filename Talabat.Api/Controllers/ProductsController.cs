using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Talabat.Api.Dtos;
using Talabat.Api.Errors;
using Talabat.Api.Helpers;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;

namespace Talabat.Api.Controllers
{
  
    public class ProductsController : ApiBaseController
    {
        private readonly IUnitOfWork _repo;
        private readonly IMapper _mapper;
      

        public ProductsController(IUnitOfWork Repo,IMapper mapper )
        {
           
            
            _repo = Repo;
            _mapper = mapper;
            
         
        }
        // get all product
        [HttpGet("GetAllProducts")]
        public async Task<ActionResult<Pagination<ProductDto>>> GetAll([FromQuery]ProductSpecParams? productspec)
        {
            var spec = new ProductSpecification(productspec);
            var data = _mapper.Map<IReadOnlyList<ProductDto>>(await _repo.Repo<Product>().GetAllWithSpecAsync(spec));
           var CountSpec=new ProductFiltrationForCountSpecification(productspec);
            var count =await _repo.Repo<Product>().GetCountWithSpecAsync(CountSpec);
            return Ok(new Pagination<ProductDto>(productspec.PageIndex,productspec.PageSize,count ,data));
        }

        // get product by id
        [ProducesResponseType(typeof(ProductDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof( ApiErrorResponse),StatusCodes.Status404NotFound)]
        [HttpGet("GetProductByID/{id}")]
        public async Task<ActionResult<ProductDto>> GetById(int id)
        {
            var spec = new ProductSpecification(id);
            var product = await _repo.Repo<Product>().GetByIdWithSpecAsync(spec);
            if (product == null)
                return NotFound(new ApiErrorResponse(404));
            var mappedproduct = _mapper.Map<ProductDto>(product);
            return Ok(mappedproduct);
        }
        //get all productbrands
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetAllBrands()
        {
           var brands=await _repo.Repo<ProductBrand>().GetAllAsync();
            return Ok(brands);
        }

        //get all producttypes
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetAlltypes()
        {
            var types = await _repo.Repo<ProductType>().GetAllAsync();
            return Ok(types);
        }
    }
}
