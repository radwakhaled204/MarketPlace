namespace MarketPlace.Contracts.Orders;

public record OrderItemDto(
    Guid OrderItemId,
    Guid OrderId,
    Guid ProductId,
    decimal UnitPrice,
    int Quantity,
    decimal TotalItemPrice
);
