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





















//using Microsoft.AspNetCore.Mvc;
//using Order.Api.Api.Dtos;
//using Order.Api.Domain.Interfaces;

//namespace Order.Api.Api.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class OrdersController : ControllerBase
//    {
//        private readonly ICrmOrderClient _crmOrderClient;

//        public OrdersController(ICrmOrderClient crmOrderClient)
//        {
//            _crmOrderClient = crmOrderClient;
//        }

//        // GET: /api/orders
//        [HttpGet]
//        public async Task<ActionResult<List<OrderDto>>> GetAll(CancellationToken ct)
//        {
//            var orders = await _crmOrderClient.GetAllAsync(ct);
//            return Ok(orders);
//        }

//        // GET: /api/orders/{id}
//        [HttpGet("{id:guid}")]
//        public async Task<ActionResult<OrderDto>> GetById(Guid id, CancellationToken ct)
//        {
//            var order = await _crmOrderClient.GetByIdAsync(id, ct);

//            if (order is null)
//                return NotFound(new { message = "Order not found" });

//            return Ok(order);
//        }
//    }
//}
