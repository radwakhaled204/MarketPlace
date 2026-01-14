using OrderItems.Api.Application.Dtos;
using OrderItems.Api.Domain.Entities;

namespace OrderItems.Api.Domain.Interfaces;

public interface ICrmOrderItemClient
{
    Task<List<OrderItem>> GetAllAsync(CancellationToken ct = default);

    Task<List<OrderItem>> GetByOrderIdAsync(Guid orderId, CancellationToken ct = default);
    Task<Guid> CreateAsync(
    Guid orderId,
    Guid productId,
    int quantity,
    string name,
    decimal unitPrice,
    decimal totalItemPrice,
    CancellationToken ct = default
);

}
