using TalabatCore.Entities.Order_Aggregation;

namespace TalabatAPIs.DTO
{
    public class OrderDto
    {
        public string BasketId { get; set; }
        public int DeliveryMethodId { get; set; }
        public AddressDto ShippingAddress { get; set; }
    }
}
