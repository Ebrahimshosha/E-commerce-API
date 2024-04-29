using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalabatCore.Entities.Order_Aggregation;

namespace TalabatCore.Services
{
	public interface IorderService
	{
		Task<Order?> CreateOederAsync(string BuyerEmail, string BasketId, int DeliveryMethodId, Address ShippingAddress);
		Task<IReadOnlyList<Order>> GetOrdersForSpecificUserAsync(string BuyerEmail);
		Task<Order> GetOrdersByIdForSpecificUserAsync(string BuyerEmail, int orderId);
		Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethod();


    }
}
