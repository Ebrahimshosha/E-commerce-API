using TalabatCore.Entities.Order_Aggregation;

namespace TalabatAPIs.DTO
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }

        public string BuyerEmail { get; set; }

        public DateTimeOffset OrderDate { get; set; } 

        public string Status { get; set; } 

        public Address ShippingAddress { get; set; }

        public string DeliveryMethod { get; set; }

        public decimal DeliveryMethodCost { get; set; }

        public ICollection<OrderItemToRetrunDto> Items { get; set; } 

        public decimal SubTotal { get; set; } // price of product * Quantity

        public string PaymentIntendId { get; set; } = string.Empty;

        public decimal total { get; set;}
    }
}
