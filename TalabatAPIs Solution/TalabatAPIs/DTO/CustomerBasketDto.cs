using System.ComponentModel.DataAnnotations;

namespace TalabatAPIs.DTO
{
	public class CustomerBasketDto
	{
		[Required]
		public string Id { get; set; }
		[Required]
		public List<BasketItemDto> Items { get; set; }
        public int? DeliveryMethodId { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
    }
}
