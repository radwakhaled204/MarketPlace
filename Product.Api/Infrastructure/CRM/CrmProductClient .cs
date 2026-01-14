using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using AutoMapper;
using Product.Api.Domain.Interfaces;
using Product.Api.Application.Dtos;

namespace Product.Api.Infrastructure.CRM
{
    public class CrmProductClient : ICrmProductClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMapper _mapper;

        private const string EntitySetName = "rk_products";

        private const string ColId = "rk_productid";
        private const string ColName = "rk_name";
        private const string ColQty = "rk_availablequantity";
        private const string ColPrice = "rk_price";
        private const string ColDesc = "rk_description";
        //select sen in odata
        private static readonly string Select =
            $"{ColId},{ColName},{ColQty},{ColPrice},{ColDesc}";
        //no  new HttpClient() everytime
        public CrmProductClient(IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            _httpClientFactory = httpClientFactory;
            _mapper = mapper;

        }

        public async Task<List<ProductDto>> GetAllAsync(CancellationToken ct = default)
        {
            //crm from program.cs
            var client = _httpClientFactory.CreateClient("Crm");
            //url building
            var url = $"{EntitySetName}?$select={Select}";
            var response = await client.GetAsync(url, ct);
            //check response - if not 2xx
            response.EnsureSuccessStatusCode();
            var crmResponse = await response.Content.ReadFromJsonAsync<ODataResponse<CrmProductDto>>(cancellationToken: ct);

            var crmList = crmResponse?.Value ?? [];

            return _mapper.Map<List<ProductDto>>(crmList);


            //var content = await response.Content.ReadAsStringAsync(ct);
            //using var jsonDoc = JsonDocument.Parse(content);

            //var list = new List<ProductDto>();

            //if (jsonDoc.RootElement.TryGetProperty("value", out var valueElement))
            //{
            //    foreach (var item in valueElement.EnumerateArray())
            //    {
            //        list.Add(MapToDto(item));
            //    }
            //}

            //return list;
        }

        public async Task<ProductDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var client = _httpClientFactory.CreateClient("Crm");

            // add id to url
            var url = $"{EntitySetName}({id})?$select={Select}";
            var response = await client.GetAsync(url, ct);
            // if 404, return null
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            var crmProduct = await response.Content.ReadFromJsonAsync<CrmProductDto>(cancellationToken: ct);

            if (crmProduct is null)
                return null;


            return _mapper.Map<ProductDto>(crmProduct);

            //var content = await response.Content.ReadAsStringAsync(ct);
            //using var jsonDoc = JsonDocument.Parse(content);


            //return MapToDto(jsonDoc.RootElement);
        }

    }
}