using AutoMapper;
using Order.Api.Application.Dtos;
using Order.Api.Domain.Entities;
namespace Order.Api.Infrastructure.Mappings
{
    public class CrmMappingsProfile : Profile
    {
        public CrmMappingsProfile()
        {




            CreateMap<CrmOrderDto, Order.Api.Domain.Entities.Order>()
                .ForMember(d => d.OrderId, m => m.MapFrom(s => s.rk_orderid))
                //.ForMember(d => d.OrderDate, m => m.MapFrom(s => s.rk_orderdate))
                .ForMember(d => d.TotalItems, m => m.MapFrom(s => s.rk_totalitems))
                .ForMember(d => d.TotalOrderPrice, m => m.MapFrom(s => s.rk_totalorderprice))
                .ForMember(d => d.CustomerId, m => m.MapFrom(s => s._rk_customer_fk_value))

                .ForMember(d => d.OrderStatus, m => m.MapFrom(s => (OrderStatus)s.rk_orderstatus_optional));
                //.ForMember(d => d.Items, m => m.Ignore());

            //CreateMap<CrmOrderItem, OrderItem>()
            //    .ForMember(d => d.OrderItemId, m => m.MapFrom(s => s.rk_orderitemid))
            //    .ForMember(d => d.OrderId, m => m.MapFrom(s => s._rk_order_fk_value))
            //    .ForMember(d => d.ProductId, m => m.MapFrom(s => s._rk_product_fk_value))
            //    .ForMember(d => d.UnitPrice, m => m.MapFrom(s => s.rk_unitprice))
            //    .ForMember(d => d.Quantity, m => m.MapFrom(s => s.rk_quantity))
            //    .ForMember(d => d.TotalItemPrice, m => m.MapFrom(s => s.rk_totalitemprice));


        }
    }
}
