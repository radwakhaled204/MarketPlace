//using Order.Api.Domain.Entities;
//using Order.Api.Domain.Interfaces;

//namespace Order.Api.Application.Services;

//public class OrderItemsService
//{
//    private readonly ICrmOrderItemClient _crm;

//    public OrderItemsService(ICrmOrderItemClient crm)
//    {
//        _crm = crm;
//    }

//    public Task<List<OrderItem>> GetByOrderIdAsync(Guid orderId, CancellationToken ct = default)
//        => _crm.GetByOrderIdAsync(orderId, ct);

//    public Task<Dictionary<Guid, List<OrderItem>>> GetByOrderIdsAsync(IEnumerable<Guid> orderIds, CancellationToken ct = default)
//        => _crm.GetByOrderIdsAsync(orderIds, ct);
//}
