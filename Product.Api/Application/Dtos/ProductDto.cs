using System.Text.Json.Serialization;

namespace Product.Api.Application.Dtos
{
    // may i will change to record later
    public class ProductDto
    {
        //[JsonPropertyName("id")]
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public int AvailableQuantity { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
    }
  

}
