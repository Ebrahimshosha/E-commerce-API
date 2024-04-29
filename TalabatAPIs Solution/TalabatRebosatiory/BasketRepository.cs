using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TalabatCore.Entities;
using TalabatCore.Repositories;

namespace TalabatRebosatiory
{
	public class BasketRepository : IBasketRepository
	{
		private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer redias)
        {
			_database = redias.GetDatabase();
		}
        public async Task<CustomerBasket?> GetCustomerBasketAsync(string CustomerBasketId)
		{
			var CustomerBasket = await _database.StringGetAsync(CustomerBasketId); // return Radias value

			//if (CustomerBasket.IsNull) return null;
			//else 
			//	return JsonSerializer.Deserialize<CustomerBasket>(CustomerBasket);

			return CustomerBasket.IsNull ? null : JsonSerializer.Deserialize<CustomerBasket>(CustomerBasket);
		}

		public async Task<CustomerBasket?> UpdateOrCreateCustomerBasketAsync(CustomerBasket customerBasket)
		{
			var JsonCustomerBasket = JsonSerializer.Serialize(customerBasket);
			var CreatedorUpdated =  await _database.StringSetAsync(customerBasket.Id,JsonCustomerBasket,TimeSpan.FromDays(1));
			if (!CreatedorUpdated) return null;
			return await GetCustomerBasketAsync(customerBasket.Id);
		}

		public async Task<bool> DeleteCustomerBasketAsync(string CustomerBasketId)
		{
			return await _database.KeyDeleteAsync(CustomerBasketId);
		}
	}
}
