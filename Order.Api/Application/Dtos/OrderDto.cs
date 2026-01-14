namespace Order.Api.Application.Dtos;

public record OrderDto(
    Guid OrderId,
    DateTime OrderDate,
    int OrderStatusValue,
    string OrderStatusText,
    int TotalItems,
    decimal TotalOrderPrice,
    Guid CustomerId
 
);
