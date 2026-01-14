using System.Text.Json.Serialization;

namespace Product.Api.Infrastructure.CRM
{
    public class ODataResponse<T>
    {
        [JsonPropertyName("value")]
        public List<T> Value { get; set; } = [];
    }
}
