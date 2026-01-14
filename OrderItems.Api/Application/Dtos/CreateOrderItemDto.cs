namespace OrderItems.Api.Application.Dtos
{
    public record CreateOrderItemDto(
    Guid OrderId,
    Guid ProductId,
    int Quantity,
    string Name,
    decimal UnitPrice
    );

}
