namespace Order.Api.Application.Dtos
{
    public class CrmOrderDto
    {
        public Guid rk_orderid { get; set; }
        DateTime? rk_orderdate { get; set; } 
        public decimal? rk_totalorderprice { get; set; }
        public int? rk_totalitems { get; set; }
        public int rk_orderstatus_optional { get; set; } 
        
        public Guid? _rk_customer_fk_value { get; set; }
    }

    //public class CrmOrderItem
    //{
    //    public Guid rk_orderitemid { get; set; }
    //    public int? rk_quantity { get; set; }
    //    public decimal? rk_unitprice { get; set; }
    //    public decimal? rk_totalitemprice { get; set; }
    //    public Guid? _rk_product_fk_value { get; set; }
    //    public Guid? _rk_order_fk_value { get; set; }

    //}

}
