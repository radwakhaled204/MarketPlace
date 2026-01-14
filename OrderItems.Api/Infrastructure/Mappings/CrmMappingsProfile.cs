using AutoMapper;
using OrderItems.Api.Application.Dtos;
using OrderItems.Api.Domain.Entities;
using OrderItems.Api.Infrastructure.Repositories;

namespace OrderItems.Api.Infrastructure.Mappings
{
    public class CrmMappingsProfile : Profile
    {
        public CrmMappingsProfile()
        {


            CreateMap<CrmOrderItemDto, OrderItem>()
                .ForMember(d => d.OrderItemId, m => m.MapFrom(s => s.rk_orderitemid))
                 .ForMember(d => d.Name, m => m.MapFrom(s => s.rk_name))
                .ForMember(d => d.OrderId, m => m.MapFrom(s => s._rk_order_fk_value ))
                .ForMember(d => d.ProductId, m => m.MapFrom(s => s._rk_product_fk_value ))
                .ForMember(d => d.UnitPrice, m => m.MapFrom(s => s.rk_unitprice ?? 0))
                .ForMember(d => d.Quantity, m => m.MapFrom(s => s.rk_quantity ?? 0))
                .ForMember(d => d.TotalItemPrice, m => m.MapFrom(s => s.rk_totalitemprice ?? 0));

            // Domain -> API DTO
            CreateMap<OrderItem, OrderItemDto>()
                .ForCtorParam("OrderId", m => m.MapFrom(s => (Guid?)s.OrderId))
                .ForCtorParam("ProductId", m => m.MapFrom(s => (Guid?)s.ProductId));
        }
    }
}
