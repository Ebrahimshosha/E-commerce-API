using AutoMapper;
using TalabatAPIs.DTO;
using TalabatCore.Entities;
using TalabatCore.Entities.Order_Aggregation;

namespace TalabatAPIs.Helpers
{
    public class OrderItemPictureUrlResolve : IValueResolver<OrderItem, OrderItemToRetrunDto, string>
    {


        private readonly IConfiguration _configuration;

        public OrderItemPictureUrlResolve(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(OrderItem source, OrderItemToRetrunDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Product.PictureUrl))
            {
                return $"{_configuration["ApiBaseUrl"]}{source.Product.PictureUrl}";
            }
            return string.Empty;
        }
    }
}
