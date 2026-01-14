using Order.Api.Domain.Entities;

namespace Order.Api.Domain.Interfaces;

public interface ICrmOrderClient
{
    Task<List<Entities.Order>> GetAllAsync(CancellationToken ct = default);
    Task<Entities.Order?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<List<Entities.Order>> GetByCustomerIdAsync(Guid customerId,CancellationToken ct = default);
    Task<Guid> CreateAsync(
        Guid customerId,
        int totalItems,
        decimal totalOrderPrice,
        DateTime? orderDate = null,
        CancellationToken ct = default
    );

}

