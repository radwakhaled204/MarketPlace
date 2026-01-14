namespace Product.Api.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public int AvailableQuantity { get; set; }
        public decimal Price { get; set; }
    }


}
