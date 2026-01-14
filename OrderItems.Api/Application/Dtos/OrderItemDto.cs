namespace OrderItems.Api.Application.Dtos
{
    public record OrderItemDto(
        Guid OrderItemId,
        string? Name,
        int Quantity,
        decimal UnitPrice,
        decimal TotalItemPrice,
        Guid? OrderId,
        Guid? ProductId
    );
}
