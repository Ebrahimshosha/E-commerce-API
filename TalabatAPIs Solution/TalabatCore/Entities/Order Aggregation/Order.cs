using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalabatCore.Entities.Order_Aggregation
{
    public class Order : BaseEntity
    {
        public Order() // When you do Migration , EF needs an Paramterless constructor for any class will be table  
        {

        }
        public Order(string buyerEmail, Address shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subTotal, string paymentIntendId)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            SubTotal = subTotal;
            PaymentIntendId = paymentIntendId;
        }

        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public Address ShippingAddress { get; set; }

        public DeliveryMethod DeliveryMethod { get; set; } // Navigational Property [ONE] [Ef will Generate FK DeliveryMethodId]

        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>(); // Navigational Property [Many] [Ef will Generate FK OrderId in OrderItem Table ]

        public decimal SubTotal { get; set; } // price of product * Quantity

        public decimal GetTotal() => SubTotal + DeliveryMethod.Cost;

        public string PaymentIntendId { get; set; } 

    }
}
