using AutoMapper;
using OrderItems.Api.Application.Dtos;
using OrderItems.Api.Domain.Entities;
using OrderItems.Api.Domain.Interfaces;

namespace OrderItems.Api.Application.Services;

public class OrderItemService
{
    private readonly ICrmOrderItemClient _crm;
    private readonly IMapper _mapper;

    public OrderItemService(ICrmOrderItemClient crm, IMapper mapper)
    {
        _crm = crm;
        _mapper = mapper;
    }


    public async Task<List<OrderItemDto>> GetAllAsync(CancellationToken ct = default)
    {
        var items = await _crm.GetAllAsync(ct); // Domain
        return items.Select(x => _mapper.Map<OrderItemDto>(x)).ToList();
    }

    public async Task<List<OrderItemDto>> GetByOrderIdAsync(Guid orderId, CancellationToken ct = default)
    {
        var items = await _crm.GetByOrderIdAsync(orderId, ct);
        return items.Select(x => _mapper.Map<OrderItemDto>(x)).ToList();
    }
    public async Task<Guid> CreateAsync(CreateOrderItemDto dto, CancellationToken ct = default)
        {
          
            var totalItemPrice = dto.UnitPrice * dto.Quantity;

            
            return await _crm.CreateAsync(
                orderId: dto.OrderId,
                name: dto.Name,
                productId: dto.ProductId,
                quantity: dto.Quantity,
                unitPrice: dto.UnitPrice,
                totalItemPrice: totalItemPrice,
                ct: ct
            );
        }
    
}
