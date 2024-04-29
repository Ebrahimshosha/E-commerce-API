using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalabatCore;
using TalabatCore.Entities;
using TalabatCore.Entities.Order_Aggregation;
using TalabatCore.Repositories;
using TalabatCore.Services;
using TalabatCore.Specifications.OrderSpec;

namespace TalabatServices
{
    public class OrderServices : IorderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitofwork _unitofwork;
        private readonly IPaymentService _paymentService;

        public OrderServices(IBasketRepository basketRepository, IUnitofwork unitofwork,IPaymentService paymentService)
        {
            _basketRepository = basketRepository;
            _unitofwork = unitofwork;
            _paymentService = paymentService;
        }

        public async Task<Order?> CreateOederAsync(string BuyerEmail, string BasketId, int DeliveryMethodId, Address ShippingAddress)
        {
            // 1. Get basket from Basket Repo
            var Basket = await _basketRepository.GetCustomerBasketAsync(BasketId);

            // 2. Get Selected Items at Basket From Product Repo
            var OrderItems = new List<OrderItem>();

            if (Basket?.Items.Count > 0)
            {
                foreach (var item in Basket.Items)
                {
                    var Product = await _unitofwork.Repository<Product>().GetByIdAsync(item.Id);
                    var productItemOrderd = new ProductItemOrdered(item.Id, Product.Name, Product.PictureUrl);
                    var OrderItem = new OrderItem(productItemOrderd, Product.Price, item.Quantity);
                    OrderItems.Add(OrderItem);
                }
            }

            // 3. Calculate SubTotal // price * Quantity
            var SubTotal = OrderItems.Sum(item => item.Price * item.Quantity);

            // 4. Get DeliveryMethod from DeliveryMethod Repo
            var DeliveryMethod = await _unitofwork.Repository<DeliveryMethod>().GetByIdAsync(DeliveryMethodId);

            // 5. create order

            var spec = new OrderWithPaymentIdSpec(Basket.PaymentIntentId);

            var EXOrder = await _unitofwork.Repository<Order>().GetByEntityWithSpecAsync(spec);

            if(EXOrder is not null)
            {
                 _unitofwork.Repository<Order>().Delete(EXOrder);
                await _paymentService.CrreateOrUpdatePaymentIntent(BasketId);
            }

            var order = new Order(BuyerEmail, ShippingAddress, DeliveryMethod, OrderItems, SubTotal, Basket.PaymentIntentId);

            // 6. Add order Locally
            await _unitofwork.Repository<Order>().AddAsync(order);

            // 7. Save Order To Db
            var result = await _unitofwork.Complete();
            if (result <= 0) return null;
            return order;
        }
        public async Task<IReadOnlyList<Order>> GetOrdersForSpecificUserAsync(string BuyerEmail)
        {
            var spec = new OrderSpecifications(BuyerEmail);
            var orders = await _unitofwork.Repository<Order>().GetAllWithSpecAsync(spec);
            return orders;
        }

        public async Task<Order> GetOrdersByIdForSpecificUserAsync(string BuyerEmail, int orderId)
        {
            var spec = new OrderSpecifications(BuyerEmail, orderId);
            var order = await _unitofwork.Repository<Order>().GetByEntityWithSpecAsync(spec);
            return order;

        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethod()
        {
            var deliveryMethods = await _unitofwork.Repository<DeliveryMethod>().GetAllAsync();
            return deliveryMethods;
        }
    }
}
