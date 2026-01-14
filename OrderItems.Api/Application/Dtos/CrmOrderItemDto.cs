namespace OrderItems.Api.Infrastructure.Repositories;

public class CrmOrderItemDto
{

    public Guid rk_orderitemid { get; set; }
    public string? rk_name { get; set; }

    public int? rk_quantity { get; set; }
    public decimal? rk_unitprice { get; set; }
    public decimal? rk_totalitemprice { get; set; }


    public Guid _rk_order_fk_value { get; set; }
    public Guid _rk_product_fk_value { get; set; }
}
