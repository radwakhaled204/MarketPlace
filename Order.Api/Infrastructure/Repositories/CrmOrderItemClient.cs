//using AutoMapper;
//using Order.Api.Domain.Entities;
//using Order.Api.Domain.Interfaces;
//using Order.Api.Infrastructure.CRM.Models;
//using System.Text.Json;

//namespace Order.Api.Infrastructure.CRM;

//public class CrmOrderItemClient : ICrmOrderItemClient
//{
//    private readonly HttpClient _http;
//    private readonly IMapper _mapper;

//    private const string EntitySetName = "rk_orderitems";

//    private const string ColId = "rk_orderitemid";
//    private const string ColOrderLookup = "_rk_order_fk_value";
//    private const string ColProductLookup = "_rk_product_fk_value";
//    private const string ColUnitPrice = "rk_unitprice";
//    private const string ColQty = "rk_quantity";
//    private const string ColTotal = "rk_totalitemprice";

//    private static readonly string Select =
//        $"{ColId},{ColOrderLookup},{ColProductLookup},{ColUnitPrice},{ColQty},{ColTotal}";

//    public CrmOrderItemClient(IHttpClientFactory factory, IMapper mapper)
//    {
//        _http = factory.CreateClient("Crm");
//        _mapper = mapper;
//    }

//    public async Task<List<OrderItem>> GetByOrderIdAsync(Guid orderId, CancellationToken ct = default)
//    {
//        var filter = $"_rk_order_fk_value eq {orderId}";
//        var url = $"{EntitySetName}?$select={Select}&$filter={filter}";

//        var resp = await _http.GetAsync(url, ct);
//        var body = await resp.Content.ReadAsStringAsync(ct);

//        if (!resp.IsSuccessStatusCode)
//        {
//            throw new Exception(
//                $"CRM OrderItems failed: {(int)resp.StatusCode}\nURL: {url}\nBODY:\n{body}"
//            );
//        }

//        var data = JsonSerializer.Deserialize<ODataResponse<CrmOrderItem>>(
//            body,
//            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
//        );

//        return (data?.Value ?? new())
//            .Select(i => _mapper.Map<OrderItem>(i))
//            .ToList();
//    }

//    public async Task<Dictionary<Guid, List<OrderItem>>> GetByOrderIdsAsync(
//        IEnumerable<Guid> orderIds,
//        CancellationToken ct = default)
//    {
//        var ids = orderIds.Distinct().ToList();
//        if (!ids.Any()) return new();

//        var filter = string.Join(" or ", ids.Select(id => $"_rk_order_fk_value eq {id}"));
//        var url = $"{EntitySetName}?$select={Select}&$filter={filter}";

//        var resp = await _http.GetAsync(url, ct);
//        var body = await resp.Content.ReadAsStringAsync(ct);

//        if (!resp.IsSuccessStatusCode)
//        {
//            throw new Exception(
//                $"CRM OrderItems bulk failed: {(int)resp.StatusCode}\nURL: {url}\nBODY:\n{body}"
//            );
//        }

//        var data = JsonSerializer.Deserialize<ODataResponse<CrmOrderItem>>(
//            body,
//            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
//        );

//        return (data?.Value ?? new())
//            .Select(i => _mapper.Map<OrderItem>(i))
//            .GroupBy(i => i.OrderId)
//            .ToDictionary(g => g.Key, g => g.ToList());
//    }
//}
