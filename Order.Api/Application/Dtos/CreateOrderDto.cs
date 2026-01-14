namespace Order.Api.Application.Dtos
{
    public record CreateOrderDto(
    Guid CustomerId,
    int TotalItems,
    decimal TotalOrderPrice
);
    //public record CreateOrderDto(
    //    Guid CustomerId,
    //    int TotalItems,
    //    decimal TotalOrderPrice
    //    //DateTime OrderDate
    //);

}
