using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Talabat.Api.Dtos;
using Talabat.Api.Errors;
using Talabat.Api.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;

namespace Talabat.Api.Controllers
{
  
    public class ProductsController : ApiBaseController
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<ProductBrand> _brandRepo;
        private readonly IGenericRepository<ProductType> _typeRepo;

        public ProductsController(IGenericRepository<Product> ProductRepo,IMapper mapper,
            IGenericRepository<ProductBrand> BrandRepo, IGenericRepository<ProductType> TypeRepo)
        {
            _productRepo = ProductRepo;
            _mapper = mapper;
            
            _brandRepo = BrandRepo;
            _typeRepo = TypeRepo;
        }
        // get all product
        [HttpGet("GetAllProducts")]
        public async Task<ActionResult<Pagination<ProductDto>>> GetAll([FromQuery]ProductSpecParams? productspec)
        {
            var spec = new ProductSpecification(productspec);
            var data = _mapper.Map<IReadOnlyList<ProductDto>>(await _productRepo.GetAllWithSpecAsync(spec));
           var CountSpec=new ProductFiltrationForCountSpecification(productspec);
            var count =await _productRepo.GetCountWithSpecAsync(CountSpec);
            return Ok(new Pagination<ProductDto>(productspec.PageIndex,productspec.PageSize,count ,data));
        }

        // get product by id
        [ProducesResponseType(typeof(ProductDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof( ApiErrorResponse),StatusCodes.Status404NotFound)]
        [HttpGet("GetProductByID/{id}")]
        public async Task<ActionResult<ProductDto>> GetById(int id)
        {
            var spec = new ProductSpecification(id);
            var product = await _productRepo.GetByIdWithSpecAsync(spec);
            if (product == null)
                return NotFound(new ApiErrorResponse(404));
            var mappedproduct = _mapper.Map<ProductDto>(product);
            return Ok(mappedproduct);
        }
        //get all productbrands
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetAllBrands()
        {
           var brands=await _brandRepo.GetAllAsync();
            return Ok(brands);
        }

        //get all producttypes
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetAlltypes()
        {
            var types = await _typeRepo.GetAllAsync();
            return Ok(types);
        }
    }
}
