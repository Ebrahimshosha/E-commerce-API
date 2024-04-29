using System.ComponentModel.DataAnnotations;

namespace TalabatAPIs.DTO
{
	public class BasketItemDto
	{
		[Required]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public string PictureUrl { get; set; }

		[Required]
		public string Type { get; set; }

		[Required]
		public string Brand { get; set; }

		[Required]
		[Range(0.1, double.MaxValue,ErrorMessage ="Price Can not Be Zero")]
		public decimal Price { get; set; }

		[Required]
		[Range(1, int.MaxValue, ErrorMessage = "Quantity Can not Be Zero")]
		public int Quantity { get; set; }
	}
}