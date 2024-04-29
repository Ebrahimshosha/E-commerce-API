using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalabatCore.Entities;

namespace TalabatCore.Repositories
{
	public interface IBasketRepository
	{
		Task<CustomerBasket?> GetCustomerBasketAsync(string CustomerBasketId);
		Task<CustomerBasket?> UpdateOrCreateCustomerBasketAsync(CustomerBasket customerBasket);
		Task<bool> DeleteCustomerBasketAsync(string CustomerBasketId);

	}
}
