namespace Product.Api.Application.Dtos
{
    public class CrmProductDto
    {

        public Guid rk_productid { get; set; }
        public string rk_name { get; set; } = "";
        public int rk_availablequantity { get; set; }
        public decimal rk_price { get; set; }
        public string? rk_description { get; set; }



    }
}
