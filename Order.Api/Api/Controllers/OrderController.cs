using Microsoft.AspNetCore.Mvc;
using Order.Api.Application.Dtos;
using Order.Api.Application.Services;

namespace Order.Api.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly OrdersService _service;

    public OrdersController(OrdersService service)
    {
        _service = service;
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderDto dto, CancellationToken ct)
    {
        var id = await _service.CreateAsync(dto, ct);
        return Ok(new { orderId = id });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _service.GetAllAsync(ct));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var order = await _service.GetByIdAsync(id, ct);
        return order is null ? NotFound() : Ok(order);
    }
    [HttpGet("by-customer/{customerId:guid}")]
    public async Task<IActionResult> GetByCustomerId(Guid customerId, CancellationToken ct)
    {
        var orders = await _service.GetByCustomerIdAsync(customerId, ct);
        return Ok(orders);
    }
}




