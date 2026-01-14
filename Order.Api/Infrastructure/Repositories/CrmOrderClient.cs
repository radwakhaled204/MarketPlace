using AutoMapper;
using Order.Api.Application.Dtos;
using Order.Api.Domain.Entities;
using Order.Api.Domain.Interfaces;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Order.Api.Infrastructure.Repositories;

public class CrmOrderClient : ICrmOrderClient
{
    private readonly HttpClient _http;
    private readonly IMapper _mapper;

    private const string CustomerEntitySetName = "rk_customers";
    private const string OrderEntitySetName = "rk_orders";
    private const string OrderItemsNav = "rk_rk_order_rk_orderitem_order_fk";
    private const string ColName = "rk_name";
    private const string ColId = "rk_orderid";
    private const string ColDate = "rk_orderdate";
    private const string ColStatus = "rk_orderstatus_optional";
    private const string ColTotalItems = "rk_totalitems";
    private const string ColTotalPrice = "rk_totalorderprice";
    private const string ColCustomerLookup = "_rk_customer_fk_value";

    private static readonly string Select =
        $"{ColId},{ColDate},{ColStatus},{ColTotalItems},{ColTotalPrice},{ColCustomerLookup}";

    public CrmOrderClient(IHttpClientFactory factory, IMapper mapper)
    {
        _http = factory.CreateClient("Crm");
        _mapper = mapper;
    }
    public async Task<Guid> CreateAsync(
        Guid customerId,
        int totalItems,
        decimal totalOrderPrice,
        DateTime? orderDate = null,
        CancellationToken ct = default)
    {

        var url = $"{OrderEntitySetName}";

        var name = $"Order {DateTime.Now:yyyyMMdd-HHmmss}";

        var payload = new Dictionary<string, object?>
        {
            [ColName] = name,
            [ColTotalItems] = totalItems,
            [ColTotalPrice] = totalOrderPrice,
        };

        if (orderDate.HasValue)
            payload[ColDate] = orderDate.Value.ToString("yyyy-MM-dd");

        
        payload["rk_customer_fk@odata.bind"] = $"/{CustomerEntitySetName}({customerId})";

        var json = JsonSerializer.Serialize(payload);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");

        var resp = await _http.PostAsync(url, content, ct);
        var body = await resp.Content.ReadAsStringAsync(ct);

        if (!resp.IsSuccessStatusCode)
        {
            throw new Exception(
                $"CRM Order create failed: {(int)resp.StatusCode}\nURL: {url}\nPAYLOAD:\n{json}\nBODY:\n{body}"
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
    public async Task<List<Order.Api.Domain.Entities.Order>> GetAllAsync(CancellationToken ct = default)
    {
       
        var url = $"{OrderEntitySetName}?$select={Select}";

        var resp = await _http.GetAsync(url, ct);
        var body = await resp.Content.ReadAsStringAsync(ct);

        if (!resp.IsSuccessStatusCode)
        {
            throw new Exception(
                $"CRM Orders failed: {(int)resp.StatusCode}\nURL: {url}\nBODY:\n{body}"
            );
        }

        var data = JsonSerializer.Deserialize<ODataResponse<CrmOrderDto>>(
            body,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );

        return (data?.Value ?? new())
            .Select(o => _mapper.Map<Order.Api.Domain.Entities.Order>(o))
            .ToList();
    }




    public async Task<Order.Api.Domain.Entities.Order?> GetByIdAsync(Guid orderId, CancellationToken ct = default)
    {
        var url = $"{OrderEntitySetName}({orderId})?$select={Select}";

        var resp = await _http.GetAsync(url, ct);
        var body = await resp.Content.ReadAsStringAsync(ct);

        if (!resp.IsSuccessStatusCode)
        {
            throw new Exception(
                $"CRM Order by id failed: {(int)resp.StatusCode}\nURL: {url}\nBODY:\n{body}"
            );
        }

        var crmOrder = JsonSerializer.Deserialize<CrmOrderDto>(
            body,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );

        return crmOrder is null ? null : _mapper.Map<Order.Api.Domain.Entities.Order>(crmOrder);
    }



    public async Task<List<Order.Api.Domain.Entities.Order>> GetByCustomerIdAsync(
    Guid customerId,
    CancellationToken ct = default)
    {
        var url =
            $"{OrderEntitySetName}" +
            $"?$select={Select}" +
            $"&$filter={ColCustomerLookup} eq {customerId}";

        var resp = await _http.GetAsync(url, ct);
        var body = await resp.Content.ReadAsStringAsync(ct);

        if (!resp.IsSuccessStatusCode)
        {
            throw new Exception(
                $"CRM Orders by customer failed: {(int)resp.StatusCode}\nURL: {url}\nBODY:\n{body}"
            );
        }

        var data = JsonSerializer.Deserialize<ODataResponse<CrmOrderDto>>(
            body,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );

        return (data?.Value ?? new())
            .Select(o => _mapper.Map<Order.Api.Domain.Entities.Order>(o))
            .ToList();
    }



    //public async Task<Order.Api.Domain.Entities.Order?> GetByIdAsync(Guid orderId, CancellationToken ct = default)
    //{
    //    var url =
    //        $"{EntitySetName}({orderId})" +
    //        $"?$select={Select}" +
    //        $"&$expand={OrderItemsNav}($select=rk_orderitemid,rk_quantity,rk_unitprice,rk_totalitemprice,_rk_product_fk_value,_rk_order_fk_value)";

    //    var resp = await _http.GetAsync(url, ct);
    //    var body = await resp.Content.ReadAsStringAsync(ct);

    //    if (!resp.IsSuccessStatusCode)
    //        throw new Exception($"CRM Order by id failed: {(int)resp.StatusCode}\nURL: {url}\nBODY:\n{body}");

    //    var crmOrder = JsonSerializer.Deserialize<CrmOrder>(
    //        body,
    //        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
    //    );

    //    return crmOrder is null ? null : _mapper.Map<Order.Api.Domain.Entities.Order>(crmOrder);
    //}

}
