using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalabatAPIs.DTO;
using TalabatAPIs.Error;
using TalabatAPIs.Helpers;
using TalabatCore;
using TalabatCore.Entities;
using TalabatCore.Repositories;
using TalabatCore.Specifications;

namespace TalabatAPIs.Controllers
{
    public class ProductsController : APIBaseController
    {
        private readonly IUnitofwork _unitofwork;
        private readonly IMapper _mapper;


        public ProductsController(IUnitofwork unitofwork, IMapper mapper)
        {         
            _unitofwork = unitofwork;
            _mapper = mapper;
        }


        // Get All Products
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [CachedAttribute(600)]
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetAllProducts([FromQuery] ProductSpecParams Params)
        {
            var spec = new ProductWithBrandandTypeWithSpecification(Params);
            var Products = await _unitofwork.Repository<Product>().GetAllWithSpecAsync(spec);
            var mappedProduct = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(Products);

            var CountSpec = new ProductWithFilterationForCountSpec(Params);
            var Count = await _unitofwork.Repository<Product>().GetCountWithSpecAsync(CountSpec);

            return Ok(new Pagination<ProductToReturnDto>(Params.Pagesize, Params.PageIndex, mappedProduct, Count));

        }


        // Get Product By Id
        [CachedAttribute(600)]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiRespponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProductById(int id)
        {
            var spec = new ProductWithBrandandTypeWithSpecification(id);
            var Product = await _unitofwork.Repository<Product>().GetByEntityWithSpecAsync(spec);
            if (Product is null) return NotFound(new ApiRespponse(404));
            //if (Product is null) return Ok(Product.ToString());
            var mappedProduct = _mapper.Map<Product, ProductToReturnDto>(Product);
            return Ok(mappedProduct);
        }


        // Baseurl/api/products/Types
        [HttpGet("Types")]
        [CachedAttribute(600)]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            var Types = await _unitofwork.Repository<ProductType>().GetAllAsync();
            return Ok(Types);
        }


        // Baseurl/api/products/Brands
        [CachedAttribute(600)]
        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var Brands = await _unitofwork.Repository<ProductBrand>().GetAllAsync();
            return Ok(Brands);
        }
    }
}
