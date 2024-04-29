using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalabatCore;
using TalabatCore.Entities;
using TalabatCore.Entities.Order_Aggregation;
using TalabatCore.Repositories;
using TalabatCore.Specifications.OrderSpec;
using Product = TalabatCore.Entities.Product;

namespace TalabatServices
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitofwork _unitofwork;

        public PaymentService(IConfiguration configuration, IBasketRepository basketRepository, IUnitofwork unitofwork)
        {
            _configuration = configuration;
            _basketRepository = basketRepository;
            _unitofwork = unitofwork;
        }
        public async Task<CustomerBasket?> CrreateOrUpdatePaymentIntent(string BaasketId)
        {
            // Get Secret Key
            StripeConfiguration.ApiKey = _configuration["Stripekeys:Secretkey"];

            // Get Basket
            var Basket = await _basketRepository.GetCustomerBasketAsync(BaasketId);
            if (Basket is null) return null;

            // Get DeliveryMethod Cost
            var ShippingPrice = 0M; // Decimal
            if (Basket.DeliveryMethodId.HasValue)
            {
                var DeliveryMethod = await _unitofwork.Repository<DeliveryMethod>().GetByIdAsync(Basket.DeliveryMethodId.Value);
                ShippingPrice = DeliveryMethod.Cost;
            }

            // Get SubTotal 
            if (Basket.Items.Count > 0)
            {
                foreach (var item in Basket.Items)
                {
                    var product = await _unitofwork.Repository<Product>().GetByIdAsync(item.Id);
                    if (item.Price != product.Price)
                        item.Price = product.Price;
                }
            }
            var SubTotal = Basket.Items.Sum(item => item.Price * item.Quantity);

            // Create or Update PaymentIntent
            var Sevice = new PaymentIntentService();
            PaymentIntent paymentIntent;
            if (string.IsNullOrEmpty(Basket.PaymentIntentId))  // Create 
            {
                var Option = new PaymentIntentCreateOptions()
                {
                    Amount = (long)(SubTotal * 100) + (long)(ShippingPrice * 100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }

                };
                paymentIntent = await Sevice.CreateAsync(Option);
                Basket.PaymentIntentId = paymentIntent.Id;
                Basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else // update
            {
                var Option = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)(SubTotal * 100) + (long)(ShippingPrice * 100),
                };
                paymentIntent = await Sevice.UpdateAsync(Basket.PaymentIntentId, Option);
                Basket.PaymentIntentId = paymentIntent.Id;
                Basket.ClientSecret = paymentIntent.ClientSecret;
            }

            // Update Basket
            await _basketRepository.UpdateOrCreateCustomerBasketAsync(Basket);

            return Basket;
        }

        public async Task<Order> UpdaePaymentIntentWithSucceedorFailed(string paymentIntentId, bool IsSucceed)
        {
            var spec = new OrderWithPaymentIdSpec(paymentIntentId);
            var order = await _unitofwork.Repository<Order>().GetByEntityWithSpecAsync(spec);

            if (IsSucceed)
            {
                order.Status = OrderStatus.PaymentRecieved;
            }
            else
            {
                order.Status = OrderStatus.PaymentFailed;
            }

            _unitofwork.Repository<Order>().Update(order);
            await _unitofwork.Complete();

            return order;
        }
    }
}
