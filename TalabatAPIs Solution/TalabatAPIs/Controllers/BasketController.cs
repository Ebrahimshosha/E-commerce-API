using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using TalabatAPIs.DTO;
using TalabatAPIs.Error;
using TalabatCore.Entities;
using TalabatCore.Repositories;

namespace TalabatAPIs.Controllers
{

    public class BasketController : APIBaseController
	{
		private readonly IBasketRepository _basketRepository;
		private readonly IMapper _mapper;

		public BasketController(IBasketRepository basketRepository,IMapper mapper)
		{
			_basketRepository = basketRepository;
			_mapper = mapper;
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<CustomerBasket>> GetCustomerBasket(string id)
		{
			var Basket = await _basketRepository.GetCustomerBasketAsync(id);
			return Basket is null ? new CustomerBasket(id) : Basket;
		}
        [ProducesResponseType(typeof(CustomerBasket), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiRespponse), StatusCodes.Status404NotFound)]
        [HttpPost]
		public async Task<ActionResult<CustomerBasket>> UpdateCustomerBasket(CustomerBasketDto Basket)
		{
			var MappedBasket = _mapper.Map<CustomerBasketDto,CustomerBasket>(Basket);
			var Updatedorcretedbasket = await _basketRepository.UpdateOrCreateCustomerBasketAsync(MappedBasket);
			if (Updatedorcretedbasket is null) return BadRequest(new ApiRespponse(404));
			return Ok(Updatedorcretedbasket);
		}

		[HttpDelete]
		public async Task<ActionResult<bool>> DeleteCustomerBasket(string id)
		{
			return await _basketRepository.DeleteCustomerBasketAsync(id);
		}
	}
}
