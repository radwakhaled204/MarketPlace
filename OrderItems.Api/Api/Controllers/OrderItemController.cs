using Microsoft.AspNetCore.Mvc;
using OrderItems.Api.Application.Dtos;
using OrderItems.Api.Application.Services;

namespace OrderItems.Api.Controllers;

[ApiController]
[Route("api/order-items")]
public class OrderItemController : ControllerBase
{
    private readonly OrderItemService _service;

    public OrderItemController(OrderItemService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var items = await _service.GetAllAsync(ct);
        return Ok(items);
    }

  
    [HttpGet("by-order/{orderId:guid}")]
    public async Task<IActionResult> GetByOrder(Guid orderId, CancellationToken ct)
    {
        var items = await _service.GetByOrderIdAsync(orderId, ct);
        return Ok(items);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderItemDto dto, CancellationToken ct)
    {
        var id = await _service.CreateAsync(dto, ct);
        return Ok(new { orderItemId = id });
    }
}




