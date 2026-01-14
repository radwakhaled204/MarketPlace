using Order.Api.Application.Dtos;
using Order.Api.Domain.Interfaces;

namespace Order.Api.Application.Services;

public class OrdersService
{
    private readonly ICrmOrderClient _ordersCrm;
    //private readonly OrderItemsService _itemsService;

    public OrdersService(ICrmOrderClient ordersCrm)
    {
        _ordersCrm = ordersCrm;
        
    }
    public async Task<Guid> CreateAsync(CreateOrderDto dto, CancellationToken ct = default)
    {
        return await _ordersCrm.CreateAsync(dto.CustomerId, dto.TotalItems, dto.TotalOrderPrice, DateTime.Now, ct);
    }

    public async Task<List<OrderDto>> GetAllAsync(CancellationToken ct = default)
    {
        var orders = await _ordersCrm.GetAllAsync(ct);

        return orders.Select(o => new OrderDto(
            OrderId: o.OrderId,
            OrderDate: o.OrderDate,
            OrderStatusValue: (int)o.OrderStatus,
            OrderStatusText: o.OrderStatus.ToString(),
            TotalItems: o.TotalItems,
            TotalOrderPrice: o.TotalOrderPrice,
            CustomerId: o.CustomerId
        )).ToList();
    }


    public async Task<OrderDto?> GetByIdAsync(Guid orderId, CancellationToken ct = default)
    {
        var order = await _ordersCrm.GetByIdAsync(orderId, ct);
        if (order is null) return null;

        return new OrderDto(
            OrderId: order.OrderId,
            OrderDate: order.OrderDate,
            OrderStatusValue: (int)order.OrderStatus,
            OrderStatusText: order.OrderStatus.ToString(),
            TotalItems: order.TotalItems,
            TotalOrderPrice: order.TotalOrderPrice,
            CustomerId: order.CustomerId

        );
    }
    public async Task<List<OrderDto>> GetByCustomerIdAsync(
        Guid customerId,
        CancellationToken ct = default)
    {
        var orders = await _ordersCrm.GetByCustomerIdAsync(customerId, ct);

        return orders.Select(o => new OrderDto(
            OrderId: o.OrderId,
            OrderDate: o.OrderDate,
            OrderStatusValue: (int)o.OrderStatus,
            OrderStatusText: o.OrderStatus.ToString(),
            TotalItems: o.TotalItems,
            TotalOrderPrice: o.TotalOrderPrice,
            CustomerId: o.CustomerId
        )).ToList();
    }

    //public async Task<List<OrderDto>> GetAllAsync(CancellationToken ct = default)
    //{
    //    var orders = await _ordersCrm.GetAllAsync(ct);

    //    //loop calls for test
    //    var orderIds = orders.Select(o => o.OrderId).ToList();
    //    var itemsByOrder = await _itemsService.GetByOrderIdsAsync(orderIds, ct);

    //    return orders.Select(o =>
    //    {
    //        var items = itemsByOrder.TryGetValue(o.OrderId, out var list) ? list : new();
    //        return new OrderDto(
    //            OrderId: o.OrderId,
    //            OrderDate: o.OrderDate,
    //             OrderStatusValue: (int)o.OrderStatus,
    //              OrderStatusText: o.OrderStatus.ToString(),
    //            TotalItems: o.TotalItems,
    //            TotalOrderPrice: o.TotalOrderPrice,
    //            CustomerId: o.CustomerId,
    //            Items: items.Select(i => new OrderItemDto(
    //                OrderItemId: i.OrderItemId,
    //                OrderId: i.OrderId,
    //                ProductId: i.ProductId,
    //                UnitPrice: i.UnitPrice,
    //                Quantity: i.Quantity,
    //                TotalItemPrice: i.TotalItemPrice
    //            )).ToList()
    //        );
    //    }).ToList();
    //}

    //public async Task<OrderDto?> GetByIdAsync(Guid orderId, CancellationToken ct = default)
    //{
    //    var order = await _ordersCrm.GetByIdAsync(orderId, ct);
    //    if (order is null) return null;
    //    //expand
    //    var items = order.Items;

    //    return new OrderDto(
    //        OrderId: order.OrderId,
    //        OrderDate: order.OrderDate,
    //        OrderStatusValue: (int)order.OrderStatus,
    //        OrderStatusText: order.OrderStatus.ToString(),
    //        TotalItems: order.TotalItems,
    //        TotalOrderPrice: order.TotalOrderPrice,
    //        CustomerId: order.CustomerId,
    //        Items: items.Select(i => new OrderItemDto(
    //            OrderItemId: i.OrderItemId,
    //            OrderId: i.OrderId,
    //            ProductId: i.ProductId,
    //            UnitPrice: i.UnitPrice,
    //            Quantity: i.Quantity,
    //            TotalItemPrice: i.TotalItemPrice
    //        )).ToList()
    //    );
    //}





}
