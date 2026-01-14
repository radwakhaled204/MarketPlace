using AutoMapper;
using OrderItems.Api.Application.Dtos;
using OrderItems.Api.Domain.Entities;
using OrderItems.Api.Domain.Interfaces;
using System.Text;
using System.Text.Json;


namespace OrderItems.Api.Infrastructure.Repositories;

public class CrmOrderItemClient : ICrmOrderItemClient
{
    private readonly HttpClient _http;
    private readonly IMapper _mapper;


   

    private const string OrderItemsEntitySetName = "rk_orderitems"; 
    private const string OrdersEntitySetName = "rk_orders";
    private const string ProductsEntitySetName = "rk_products";
    private const string ColId = "rk_orderitemid";
    private const string ColName = "rk_name";
    private const string ColQty = "rk_quantity";
    private const string ColUnitPrice = "rk_unitprice";
    private const string ColTotalItemPrice = "rk_totalitemprice";

    private const string ColOrderValue = "_rk_order_fk_value";
    private const string ColProductValue = "_rk_product_fk_value";
    private const string ColOrder = "rk_order_fk";
    private const string ColProduct = "rk_product_fk";
    private static readonly string Select =
     $"{ColId},{ColName},{ColQty},{ColUnitPrice},{ColTotalItemPrice},{ColOrderValue},{ColProductValue}";

    // ==============================================

    public CrmOrderItemClient(IHttpClientFactory factory, IMapper mapper)
    {
        _http = factory.CreateClient("Crm");
        _mapper = mapper;
    }
    public async Task<List<OrderItem>> GetAllAsync(CancellationToken ct = default)
    {
        var url = $"{OrderItemsEntitySetName}?$select={Select}";

        var resp = await _http.GetAsync(url, ct);
        var body = await resp.Content.ReadAsStringAsync(ct);

        if (!resp.IsSuccessStatusCode)
        {
            throw new Exception(
                $"CRM OrderItems failed: {(int)resp.StatusCode}\nURL: {url}\nBODY:\n{body}"
            );
        }

        var data = JsonSerializer.Deserialize<ODataResponse<CrmOrderItemDto>>(
            body,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );

        return (data?.Value ?? new())
            .Select(x => _mapper.Map<OrderItem>(x))
            .ToList();
    }
    public async Task<List<OrderItem>> GetByOrderIdAsync(Guid orderId, CancellationToken ct = default)
    {
        var url = $"{OrderItemsEntitySetName}?$select={Select}&$filter=_rk_order_fk_value eq {orderId}";

        var resp = await _http.GetAsync(url, ct);
        var body = await resp.Content.ReadAsStringAsync(ct);

        if (!resp.IsSuccessStatusCode)
        {
            throw new Exception(
                $"CRM OrderItems failed: {(int)resp.StatusCode}\nURL: {url}\nBODY:\n{body}"
            );
        }

        var data = JsonSerializer.Deserialize<ODataResponse<CrmOrderItemDto>>(
            body,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );

        return (data?.Value ?? new())
            .Select(x => _mapper.Map<OrderItem>(x))
            .ToList();
    }

    public async Task<Guid> CreateAsync(
        Guid orderId,
        Guid productId,
        int quantity,
        string name,
        decimal unitPrice,
        decimal totalItemPrice,
        CancellationToken ct = default)
    {
        var url = $"{OrderItemsEntitySetName}";

        //var name = $"Item {DateTime.Now:yyyyMMdd-HHmmss}";

        var payload = new Dictionary<string, object?>
        {
            [ColName] = name,
            [ColQty] = quantity,
            [ColUnitPrice] = unitPrice,
            [ColTotalItemPrice] = totalItemPrice,

           
            [$"{ColOrder}@odata.bind"] = $"/{OrdersEntitySetName}({orderId})",
            [$"{ColProduct}@odata.bind"] = $"/{ProductsEntitySetName}({productId})",
        };

        var json = JsonSerializer.Serialize(payload);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");

        var resp = await _http.PostAsync(url, content, ct);
        var body = await resp.Content.ReadAsStringAsync(ct);

        if (!resp.IsSuccessStatusCode)
        {
            throw new Exception(
                $"CRM OrderItem create failed: {(int)resp.StatusCode}\nURL: {url}\nPAYLOAD:\n{json}\nBODY:\n{body}"
            );
        }

       
        if (resp.Headers.TryGetValues("OData-EntityId", out var values))
        {
            var entityUrl = values.FirstOrDefault();
            var start = entityUrl?.IndexOf('(') ?? -1;
            var end = entityUrl?.IndexOf(')') ?? -1;

            if (start > -1 && end > start)
            {
                var idStr = entityUrl!.Substring(start + 1, end - start - 1);
                return Guid.Parse(idStr);
            }
        }

        return Guid.Empty;
    }
}
