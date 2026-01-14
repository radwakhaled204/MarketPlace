using AutoMapper;
using Product.Api.Application.Dtos;
namespace Product.Api.Infrastructure.Mappings
{
    public class CrmMappingsProfile : Profile
    {
        public CrmMappingsProfile()
        {

            // Products
            CreateMap<CrmProductDto, ProductDto>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.rk_productid))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.rk_name))
                .ForMember(d => d.AvailableQuantity, o => o.MapFrom(s => s.rk_availablequantity))
                .ForMember(d => d.Price, o => o.MapFrom(s => s.rk_price))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.rk_description));


            //CreateMap<CrmOrder, OrderDto>()
            //    .ForMember(d => d.OrderId, o => o.MapFrom(s => s.rk_orderid))
            //    .ForMember(d => d.OrderDate, o => o.MapFrom(s => s.rk_orderdate ?? DateTime.MinValue))
            //    .ForMember(d => d.TotalOrderPrice, o => o.MapFrom(s => s.rk_totalorderprice ?? 0m))
            //    .ForMember(d => d.TotalItems, o => o.MapFrom(s => s.rk_totalitems ?? 0))
            //    .ForMember(d => d.CustomerId, o => o.MapFrom(s => s._rk_customer_fk_value ?? Guid.Empty));

            //CreateMap<CrmOrderItem, OrderItemDto>()
            //    .ForMember(d => d.ProductId, o => o.MapFrom(s => s._rk_product_fk_value ?? Guid.Empty))
            //    .ForMember(d => d.Quantity, o => o.MapFrom(s => s.rk_quantity ?? 0))
            //    .ForMember(d => d.TotalItemPrice, o => o.MapFrom(s => s.rk_totalitemprice ?? 0m))
            //    .ForMember(d => d.UnitPrice, o => o.MapFrom(s => s.rk_unitprice ?? 0m));


        }
    }
}
