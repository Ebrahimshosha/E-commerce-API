using AutoMapper;
using TalabatAPIs.DTO;
using TalabatCore.Entities;
using TalabatCore.Entities.Identity;
using TalabatCore.Entities.Order_Aggregation;

namespace TalabatAPIs.Helpers
{
	public class MappingProfiles : Profile
	{
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                     .ForMember(d=>d.ProductBrand,o=>o.MapFrom(s=>s.ProductBrand.Name))
                     .ForMember(d=>d.ProductType,o=>o.MapFrom(s=>s.ProductType.Name))
                     .ForMember(d=>d.PictureUrl,o=>o.MapFrom<ProductPictureUrlResolve>()) ;

            CreateMap<TalabatCore.Entities.Identity.Address, AddressDto>().ReverseMap();
            
            CreateMap<CustomerBasketDto, CustomerBasket>();

            CreateMap<BasketItemDto,BasketItem>();

            CreateMap<AddressDto, TalabatCore.Entities.Order_Aggregation.Address>();

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryMethodCost, o => o.MapFrom(s => s.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemToRetrunDto>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.Product.ProductId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.ProductName))
                .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.Product.PictureUrl))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemPictureUrlResolve>());
        }
    }
}
