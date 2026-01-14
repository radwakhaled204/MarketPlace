using System.Text.Json.Serialization;

namespace OrderItems.Api.Infrastructure.Repositories
{
    public class ODataResponse<T>
    {
        [JsonPropertyName("value")]
        public List<T> Value { get; set; } = [];
    }
}
